using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.S3;

namespace ManagerImage.Models.ManagerImage
{
    public class ServiceAWSDto
    {
        public bool Active { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public string LastModified { get; set; }
        public long Size { get; set; }
        public string Url { get; set; }
    }


    public class ParameterAWSDto
    {
        public string ServiceURL { get; set; }
        public string AccessKey { get; set; }
        public string SecretKey { get; set; }
        public string BucketName { get; set; }
        public string ExpireImage { get; set; }
        public string ServerDownload { get; set; }
    }

    public class AWSConection
    {
        public ParameterAWSDto ParameterAWSDto { get; set; }
        public AmazonS3Client amazonS3Client { get; set; }
    }

}
