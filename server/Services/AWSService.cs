using Amazon.S3;
using Amazon.S3.Model;
using ManagerImage.Data;
using ManagerImage.Models.ManagerImage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ManagerImage.Services
{
    public class AWSService
    {
        int SIZE_PALLET = 11;
        private readonly Silfab_caContext context;
        private readonly ManagerImageContext managerImageContext;

        public AWSService(Silfab_caContext context, ManagerImageContext managerImageContext)
        {
            this.context = context;
            this.managerImageContext = managerImageContext;
        }


        public AWSConection getAWSConnection()
        {
            var task = managerImageContext.BatchTasks.Where(t => t.Id == 5).FirstOrDefault();
            ParameterAWSDto parameterModel = Util.Deserialize<ParameterAWSDto>(task.Parameters);

            Amazon.S3.AmazonS3Config config = new AmazonS3Config();
            config.ServiceURL = parameterModel.ServiceURL;

            AWSConection aWSConection = new AWSConection();
            aWSConection.amazonS3Client = new AmazonS3Client(parameterModel.AccessKey, parameterModel.SecretKey, config);
            aWSConection.ParameterAWSDto = parameterModel;
            return aWSConection;
        }

        public List<ServiceAWSDto> UrlPublic(List<ServiceAWSDto> serviceAWSDtoList)
        {
            foreach (var item in serviceAWSDtoList)
            {
                if (item.Active)
                {
                    item.Url = GeneratePreSignedURL(item.Path);
                    item.Active = false;
                }
            }
            return serviceAWSDtoList;

        }

        /// <summary>
        /// Generate a public url with token
        /// </summary>
        /// <param name="objectKey"></param>
        /// <returns></returns>
        public string GeneratePreSignedURL(string objectKey)
        {

            AWSConection AWSConection = getAWSConnection();
            string urlString = "";
            try
            {
                GetPreSignedUrlRequest request = new GetPreSignedUrlRequest
                {
                    BucketName = AWSConection.ParameterAWSDto.BucketName,
                    Key = objectKey,
                    Expires = DateTime.Now.AddMinutes(Convert.ToInt32(AWSConection.ParameterAWSDto.ExpireImage))
                };
                urlString = AWSConection.amazonS3Client.GetPreSignedURL(request);
            }
            catch (AmazonS3Exception e)
            {
                throw new Exception("S3 error occurred. Exception: " + e.ToString());
            }
           
            return urlString;
        }

        public List<string> getSerialList(string names)
        {

            string[] namesList = null;
            if (names.Contains("|"))
                namesList = names.Split('|');
            else if (names.Contains(";"))
                namesList = names.Split(';');
            else if (names.Contains(","))
                namesList = names.Split(',');
            else if (names.Contains("\n"))
                namesList = names.Split('\n');
            else
                namesList = names.Split('|');

            var lis = namesList.Where(t => !t.Equals("")).ToList();

            return lis;
        }


        /// <summary>
        /// Find all serial missing and add to list
        /// </summary>
        /// <param name="names"></param>
        /// <param name="serviceAWSDtoList"></param>
        public void FindSerialMissing(string names, List<ServiceAWSDto> serviceAWSDtoList)
        {
            var serialList = getSerialList(names);
            foreach (var item in serialList)
            {
                var exist = false;
                foreach (var item1 in serviceAWSDtoList)
                {
                    var serial = item1.Name.Split('.')[0];
                    if (serial.Equals(item))
                    {
                        exist = true;
                        break;
                    }
                }
                
                if (!exist && item.Length >= SIZE_PALLET)
                {
                    ServiceAWSDto serviceAWSDto = new ServiceAWSDto();
                    serviceAWSDto.Name = item;
                    serviceAWSDto.Path = "Serial Missing";
                    serviceAWSDtoList.Add(serviceAWSDto);
                }

                exist = false;
            }

        }

        public List<ServiceAWSDto> ListingBySerial(string names, List<ServiceAWSDto> serviceAWSDtoList)
        {

            var namesList = getSerialList(names);
           
            foreach (var serial in namesList)
            {
                var serials = managerImageContext.Awskeys.Where(t => t.SerialImage == serial);
                foreach (var item in serials)
                {
                    ListingObjectsBySerial(item.S3url, serviceAWSDtoList);
                }
                
            }
            FindSerialMissing(names, serviceAWSDtoList);
            return serviceAWSDtoList;
        }
        public List<ServiceAWSDto> ListingByPallet(string names, List<ServiceAWSDto> serviceAWSDtoList, bool isFile)
        {
            var namesList = getSerialList(names); 

            foreach (var name in namesList)
            {
                if (name.Length < SIZE_PALLET)//validation for pallet or serial
                {

                    AWSConection AWSConection = getAWSConnection();

                    try
                    {
                        string prefixNew = "";
                        ListObjectsV2Response response;

                        ListObjectsV2Request request = new ListObjectsV2Request
                        {
                            BucketName = AWSConection.ParameterAWSDto.BucketName,
                            Delimiter = "/"//this character indicate that search is in forlders 
                        };

                        response = AWSConection.amazonS3Client.ListObjectsV2Async(request).Result;

                        var subdirectory = response.CommonPrefixes;//contains folders

                        foreach (var item in subdirectory)
                        {
                            request = new ListObjectsV2Request
                            {
                                BucketName = AWSConection.ParameterAWSDto.BucketName,
                                Delimiter = isFile ? null : "/",//validation to identify if the search is a folder or file
                                Prefix = item + name 
                            };

                            response = AWSConection.amazonS3Client.ListObjectsV2Async(request).Result;

                            if (response.CommonPrefixes.Count() > 0)//process a folder
                            {
                                prefixNew = item + name;
                                ListingObjectsBySerial(prefixNew, serviceAWSDtoList);
                            }else if (isFile && response.S3Objects.Count() > 0)//process a file
                            {
                                addSerial(response, serviceAWSDtoList, name);
                            }

                        }

                    }
                    catch (AmazonS3Exception amazonS3Exception)
                    {
                        throw new Exception("S3 error occurred. Exception: " + amazonS3Exception.ToString());  
                    }
                }

            }
            return serviceAWSDtoList;
        }


        /// <summary>
        /// add a new serial to list
        /// </summary>
        /// <param name="response"></param>
        /// <param name="serviceAWSDtoList"></param>
        /// <param name="file"></param>
        public void addSerial(ListObjectsV2Response response, List<ServiceAWSDto> serviceAWSDtoList, string file)
        {
            var dic = serviceAWSDtoList.ToDictionary(t => t.Path);

            foreach (S3Object entry in response.S3Objects)
            {
                if (entry.Key.Contains(file))
                {
                    var names = entry.Key.Split('/');

                    ServiceAWSDto serviceAWSDto = new ServiceAWSDto();
                    if (entry.Size > 0)
                    {
                        var image = names[names.Count() - 1];//image
                        if (!dic.ContainsKey(entry.Key))
                        {
                            serviceAWSDto.Name = image;
                            serviceAWSDto.Path = entry.Key;
                            serviceAWSDto.LastModified = entry.LastModified.ToString("yyyy/MM/dd hh:mm:ss");
                            serviceAWSDto.Size = entry.Size;
                            serviceAWSDtoList.Add(serviceAWSDto);
                        }
                    }
                }
            }
        }

        public List<ServiceAWSDto> ListingObjectsBySerial(string name, List<ServiceAWSDto> serviceAWSDtoList)
        {
            AWSConection AWSConection = getAWSConnection();
            try
            {
                ListObjectsV2Request request = new ListObjectsV2Request
                {
                    BucketName = AWSConection.ParameterAWSDto.BucketName,
                    Prefix = name,
                };
                ListObjectsV2Response response = null;
                do
                {
                    response = AWSConection.amazonS3Client.ListObjectsV2Async(request).Result;
                    addSerial(response, serviceAWSDtoList, name);
                    request.ContinuationToken = response.NextContinuationToken;
                } while (response.IsTruncated);
            }
            catch (AmazonS3Exception amazonS3Exception)
            {
                throw new Exception("S3 error occurred. Exception: " + amazonS3Exception.ToString());
            }
            
            return serviceAWSDtoList;
        }


        public List<ServiceAWSDto> ListingBySerialAll(string name, List<ServiceAWSDto> serviceAWSDtoList)
        {
            var log = managerImageContext.Log.FirstOrDefault(t => t.Sno == name && t.S3url != null);
            if (log != null)
            {
                FindAWSSerialAll(name, serviceAWSDtoList, log.S3url);
            }
            else
            {
                FindAWSSerialAll(name, serviceAWSDtoList, null);
            }


            return serviceAWSDtoList;
        }

        public List<ServiceAWSDto> FindAWSSerialAll(string name, List<ServiceAWSDto> serviceAWSDtoList, string s3url)
        {

            AWSConection AWSConection = getAWSConnection();
            try
            {
                ListObjectsV2Request request = new ListObjectsV2Request
                {
                    BucketName = AWSConection.ParameterAWSDto.BucketName,
                    Prefix = s3url ?? null
                };
                bool exist = false;
                ListObjectsV2Response response = null;
                do
                {
                    response = AWSConection.amazonS3Client.ListObjectsV2Async(request).Result;

                    foreach (S3Object entry in response.S3Objects)
                    {
                        if (entry.Key.Contains(name))
                        {
                            var names = entry.Key.Split('/');

                            ServiceAWSDto serviceAWSDto = new ServiceAWSDto();
                            if (entry.Size > 0)
                            {
                                var image = names[names.Count() - 1];//image
                                {
                                    serviceAWSDto.Name = image;
                                    serviceAWSDto.Path = entry.Key;
                                    serviceAWSDto.LastModified = entry.LastModified.ToString("yyyy/MM/dd hh:mm:ss");
                                    serviceAWSDto.Size = entry.Size;
                                    serviceAWSDtoList.Add(serviceAWSDto);
                                    exist = true;
                                    break;
                                }
                            }
                        }
                    }

                    if (exist)break;

                    request.ContinuationToken = response.NextContinuationToken;
                } while (response.IsTruncated);
            }
            catch (AmazonS3Exception amazonS3Exception)
            {
                throw new Exception("S3 error occurred. Exception: " + amazonS3Exception.ToString());
            }

            return serviceAWSDtoList;
        }
        public async Task GetObjectRequest(string keyName)
        {

            AWSConection AWSConection = getAWSConnection();
            GetObjectRequest request = new GetObjectRequest()
            {
                BucketName = AWSConection.ParameterAWSDto.BucketName,
                Key = keyName
            };

            var names = keyName.Split('/');
            var image = names[names.Count() - 1];//image
            using (var response = AWSConection.amazonS3Client.GetObjectAsync(request))
            {
                await response.Result.WriteResponseStreamToFileAsync(AWSConection.ParameterAWSDto.ServerDownload + image, true, default);
            }
        }

        public string ExportImages(List<ServiceAWSDto> serviceAWSDtoList)
        {

            foreach (var item in serviceAWSDtoList)
            {
                GetObjectRequest(item.Path);
            }
            return "Files exported";
        }

    }
}
