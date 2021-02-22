using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ManagerImage.Data;
using ManagerImage.Models.ManagerImage;
using ManagerImage.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using OfficeOpenXml;
using OfficeOpenXml.Drawing.Chart;
using OfficeOpenXml.Style;

namespace ManagerImage
{
    public partial class ELImageService
    {

        private readonly Silfab_caContext context;
        private readonly ManagerImageContext managerImageContext;
        private readonly eMaintContext eMaintContext;
        public IConfiguration Configuration { get; }

        public ELImageService(ManagerImageContext managerImageContext, 
            Silfab_caContext context, 
            IConfiguration configuration,
            eMaintContext eMaintContext
            )
        {
            this.managerImageContext = managerImageContext;
            this.context = context;
            this.Configuration = configuration;
            this.eMaintContext = eMaintContext;
        }
        String USER = "";
        public string SortImage(string pallet)
        {
            
            string result = "success";
            List<SilfabOntarioSerialNoInformation> myReader1 = getSerialsPallet(pallet);
            List<SilfabOntarioSerialNoInformation> myReader2 = getSerialsPallet(pallet);
            List<SilfabOntarioSerialNoInformation> myReader3 = getSerialsPallet(pallet);
 

            bool resort_flag = false;

            if (myReader1.Count == 0 && myReader2.Count == 0)
            {
                result = "Entered Wrong pallet";
            }
            else
            {

                foreach (var itemSerial in myReader1)
                {
                    string serial = itemSerial.SerialNo.ToString();
                    string unsortPath = @"\\vcb\images\Unsorted\" + serial + ".jpg";
                    string sortPath = @"\\vcb\images\Sorted\" + pallet + @"\" + serial + ".jpg";
                    string resortImagePath = @"\\vcb\images\Resorted\" + pallet + @"\" + serial + ".jpg";
                    DirectoryInfo folder = Directory.CreateDirectory(@"\\vcb\images\Sorted\" + pallet);
                    if (File.Exists(unsortPath))
                    {
                        if (File.Exists(sortPath))
                        {

                            if (File.Exists(resortImagePath))
                            {
                                System.IO.File.Replace(unsortPath, sortPath, resortImagePath);
                            }
                            else
                            {
                                Directory.CreateDirectory(@"\\vcb\images\Resorted\" + pallet);
                                System.IO.File.Move(sortPath, resortImagePath);
                                System.IO.File.Move(unsortPath, sortPath);
                                UpdateLog(pallet, serial);
                            }
                        }
                        else
                        {
                            System.IO.File.Move(unsortPath, sortPath);
                            resort_flag = false;
                            UpdateLog(pallet, serial);
                        }
                    }
                }



                foreach (var itemSerial in myReader2)
                {
                    string serial = itemSerial.SerialNo.ToString();

                    string unsortPath = @"\\vcb\images\Unsorted\" + serial + ".jpg";
                    string sortPath = @"\\vcb\images\Sorted\" + pallet + @"\" + serial + ".jpg";
                    string Path = @"\\vcb\images\Sorted\" + pallet;
                    string resortFolderPath = @"\\vcb\images\Resorted\" + pallet;
                    string resortImagePath = @"\\vcb\images\Resorted\" + pallet + @"\" + serial + ".jpg";
                    if (resort_flag)
                    {
                        if (!File.Exists(sortPath))
                        {
                            if (File.Exists(resortImagePath))
                            {
                                System.IO.File.Move(resortImagePath, sortPath);
                            }
                        }
                    }
                    else
                    {
                        if (Directory.Exists(Path) && !resort_flag)
                        {
                            if (Directory.Exists(resortFolderPath))
                            {
                                string[] fileEntries = Directory.GetFiles(Path);
                                foreach (string fileName in fileEntries)
                                {
                                    if (!File.Exists(resortFolderPath + @"\" + System.IO.Path.GetFileName(fileName)))
                                    {
                                        System.IO.File.Move(fileName, resortFolderPath + @"\" + System.IO.Path.GetFileName(fileName));
                                    }

                                }
                                if (!File.Exists(sortPath) && File.Exists(resortImagePath))
                                {
                                    System.IO.File.Move(resortFolderPath + @"\" + serial + ".jpg", sortPath);
                                }
                                resort_flag = true;
                            }
                            else
                            {
                                if (!Directory.Exists(@"\\vcb\images\Sorted\" + pallet))
                                {
                                    DirectoryInfo folder = Directory.CreateDirectory(@"\\vcb\images\Sorted\" + pallet);
                                }
                                if (File.Exists(resortFolderPath + @"\" + serial)) 
                                {
                                    System.IO.File.Move(resortFolderPath + @"\" + serial + ".jpg", sortPath);
                                }
                                resort_flag = true;
                            }
                        }
                        else
                        {
                            DirectoryInfo folder = Directory.CreateDirectory(@"\\vcb\images\Sorted\" + pallet);
                            if (File.Exists(unsortPath))
                            {
                                System.IO.File.Move(unsortPath, sortPath);
                                UpdateLog(pallet, serial);
                            }
                            else
                                resort_flag = true;

                        }
                    }

                }

                foreach (var itemSerial in myReader3) // Fix the bug because of one image sits in resorted folder
                {
                    string serial = itemSerial.SerialNo.ToString();

                    string unsortPath = @"\\vcb\images\Unsorted\" + serial + ".jpg";
                    string sortPath = @"\\vcb\images\Sorted\" + pallet + @"\" + serial + ".jpg";
                    string Path = @"\\vcb\images\Sorted\" + pallet;
                    string resortFolderPath = @"\\vcb\images\Resorted\" + pallet;
                    string resortImagePath = @"\\vcb\images\Resorted\" + pallet + @"\" + serial + ".jpg";

                    if (Directory.Exists(resortFolderPath))
                    {
                        if (File.Exists(resortImagePath))
                        {
                            if (!File.Exists(sortPath))
                            {
                                System.IO.File.Move(resortImagePath, sortPath);
                            }
                        }

                    }
                }
                //Added code to move images from resorted to unsorted folder

                string ResortPallet = @"\\vcb\images\Resorted\" + pallet;
                string unSort = @"\\vcb\images\Unsorted\";

                if (Directory.Exists(ResortPallet))
                {
                    string[] fileEntries = Directory.GetFiles(ResortPallet);

                    foreach (string fileName in fileEntries)
                    {
                        if (File.Exists(fileName) && Path.GetExtension(fileName) == ".jpg")
                        {
                            if (!File.Exists(unSort + @"\" + System.IO.Path.GetFileName(fileName)))
                            {
                                System.IO.File.Move(fileName, unSort + @"\" + System.IO.Path.GetFileName(fileName));
                            }
                        }
                    }
                }



                result = "Sorting has been done for pallet " + pallet ;

                
            }

            return result;
        }

        protected List<SilfabOntarioSerialNoInformation> getSerialsPallet(string pallet)
        {
            return context.SilfabOntarioSerialNoInformation.Where(t => t.PalletNo.Equals(pallet)).ToList();
        }

        protected void UpdateLog(string pallet, string serial)
        {
            USER = "AutoPage";
            Log log = new Log();
            log.PalletNo = pallet;
            log.Sno = serial;
            log.UserName = USER;
            managerImageContext.Add(log);
            managerImageContext.SaveChanges();
        }

        protected List<Log> getLog(string pallet)
        {
            var list = managerImageContext.Log.Where(t => t.PalletNo.Equals(pallet)).ToHashSet();

            Dictionary<String, Log> dic = new System.Collections.Generic.Dictionary<string, Log>();
            foreach (var item in list)
            {
                if (!dic.ContainsKey(item.Sno))
                {
                    dic.Add(item.Sno, item);
                }
            }
            return dic.Values.ToList();
        }

        public string UnsortImage(string pallet)
        {
            string result = "success";
            List<SilfabOntarioSerialNoInformation> resortReader = getSerialsPallet(pallet);

            if (resortReader.Count == 0)
            {
                result = "Entered Wrong pallet";
            }
            else
            {
                foreach (var itemSerial in resortReader)
                {
                    string s = itemSerial.LastPalletNo;
                    if (s == "")
                    {
                        pallet = itemSerial.PalletNo;
                    }
                    else
                    {
                        pallet = itemSerial.LastPalletNo;
                    }

                    string lastPallet = @"\\vcb\images\Sorted\" + pallet;
                    string unSort = @"\\vcb\images\Unsorted\";

                    string ResortlastPallet = @"\\vcb\images\Resorted\" + pallet;

                    if (Directory.Exists(lastPallet))
                    {
                        string[] fileEntries = Directory.GetFiles(lastPallet);

                        foreach (string fileName in fileEntries)
                        {
                            if (File.Exists(fileName) && Path.GetExtension(fileName) == ".jpg" && !File.Exists(unSort + @"\" + System.IO.Path.GetFileName(fileName)))
                            {
                                System.IO.File.Move(fileName, unSort + @"\" + System.IO.Path.GetFileName(fileName));
                            }
                        }
                    }
                    //Added code to move images from resort folder
                    if (Directory.Exists(ResortlastPallet))
                    {
                        string[] fileEntries = Directory.GetFiles(ResortlastPallet);

                        foreach (string fileName in fileEntries)
                        {
                            if (File.Exists(fileName) && Path.GetExtension(fileName) == ".jpg")
                            {
                                if (!File.Exists(unSort + @"\" + System.IO.Path.GetFileName(fileName)))
                                {
                                    System.IO.File.Move(fileName, unSort + @"\" + System.IO.Path.GetFileName(fileName));
                                }
                            }
                        }
                    }

                }
                result = "Unsorting has been done for pallet " + pallet;

                
            }
            return result;

        }

        public List<LogDto> GetMissingPallets(string pallets, List<LogDto> list)
        {
            var palletArray = pallets.Split("|");
            foreach (var item in palletArray)
            {
                list = GetMissingPallet(item, list);
            }
            return list;
        }

        public List<LogDto> GetMissingPallet(string pallet, List<LogDto> list )
        {
            List<Log> listLog = getLog(pallet); //select all images from pallet
            List<SilfabOntarioSerialNoInformation> serialList = getSerialsPallet(pallet); //select all serials pallet
            var dic = listLog.ToDictionary(t => t.Sno);
            if (list.Count() >= 250)
            {
                list = new List<LogDto>();
            }
            int count = list.Count + 1;

            foreach (var itemSerial in serialList)
            {
                
                LogDto log = new LogDto();
                log.Id = count;
                log.PalletNo = pallet;
                log.Sno = itemSerial.SerialNo;
                log.DateModified = itemSerial.ScannedSerialOnPallet;
                bool exist = false;
                if (!dic.ContainsKey(log.Sno))//find the difference
                {
                    foreach (var item in list)
                    {
                       if(item.Sno == log.Sno)//validation for duplicate serial
                       {
                            exist = true;
                            break;
                       }
                    }

                    if (!exist)//if does not exist create a new one
                    {
                        log.Type = getFlashing(log.Sno);
                        list.Add(log);
                        count++;
                    }
                    exist = false;
                    
                }
                
            }
            return list;
        }


        public string getFlashing(string serial)
        {
            string type = "No type";
            if(getQuickSun(serial, new List<ModuleData>()).Count > 0)
            {
                type = "QuickSun";
            }
            else 
            {
                if (getPasanData(serial, new List<ModuleDataAts>()).Count > 0)
                {
                    type = "PasanData";
                }
            }
            return type;
        }

        public List<ModuleDataAts> getPasanData(string serials, List<ModuleDataAts> currentlist)
        {
            var serial = serials.Split("|");
            var serialistDB = context.ModuleDataAts.Where(t => serial.Contains(t.SerialNumberNest1) ).ToList();
            bool exist = false;

            foreach (var itemDataBase in serialistDB)
            {
                foreach (var itemCurrent in currentlist)
                {
                    if (itemCurrent.SerialNumberNest1 == itemDataBase.SerialNumberNest1)//validation for duplicate serial
                    {
                        exist = true;
                        break;
                    }
                }

                if (!exist)//if does not exist add a new one
                {
                    currentlist.Add(itemDataBase);
                }
                exist = false;

            }

            return currentlist;
        }

        public List<ModuleData> getQuickSun(string serials, List<ModuleData> currentlist)
        {
            var serial = serials.Split("|");
            var serialistDB = context.ModuleData.Where(t => serial.Contains(t.Sn)).ToList();
            bool exist = false;

            foreach (var itemDataBase in serialistDB)
            {
                foreach (var itemCurrent in currentlist)
                {
                    if (itemCurrent.Sn == itemDataBase.Sn)//validation for duplicate serial
                    {
                        exist = true;
                        break;
                    }
                }

                if (!exist)//if does not exist add a new one
                {
                    currentlist.Add(itemDataBase);
                }
                exist = false;

            }

            return currentlist;
        }

        public static void CreateLog(string text)
        {
            string path = @"C:\IIS\";
            string filename = "LogManagerImage_" + DateTime.Now.ToString("yyyy_MM") + ".txt";

            if (!Directory.Exists(path))
            {
                DirectoryInfo di = Directory.CreateDirectory(path);
            }
            using (StreamWriter writer = new StreamWriter(path + filename, true))
            {
                writer.WriteLine(text);
                writer.Close();
            }
        }

        public string ProcessUnsortedImages()
        {
            string path = @"\\VCB\Images\Unsorted";
            string[] fileEntries = Directory.GetFiles(path);
            int cont = 0;
            List<string> palletList = new List<string>();
            foreach (string oldName in fileEntries)
            {
                if (oldName.Contains(" "))
                {
                    try { 
                    
                        var array = oldName.Split(@"\");
                        var image = array[array.Count() - 1];
                        var serial = image.Split('.')[0].Trim();

                        var serialInfo = context.SilfabOntarioSerialNoInformation.FirstOrDefault(t => t.SerialNo.Equals(serial));
                        if (serialInfo != null)
                        {
                            if(!palletList.Contains(serialInfo.PalletNo))
                                palletList.Add(serialInfo.PalletNo);

                            string newName = path + @"\" + serial + ".jpg";
                            File.Move(oldName, newName);
                            CreateLog(oldName + " Renamed to " + newName + " pallet "+ serialInfo.PalletNo);
                            cont++;
                        } 
                    }
                    catch(Exception e)
                    {
                        CreateLog(e.Message + " " + oldName);
                    }   
                }
                
            }

            foreach (var pallet in palletList)
            {
                if(pallet != "")
                    SortImage(pallet);
            }
            return cont + "";
        }

        public ExcelPackage ReportWeeklyPMCompletion()
        {

            string name = "Maintenance PM Completion " + DateTime.Now.Year;

            var pmtCompletionList = eMaintContext.PmtCompletion.ToList();

            ExcelPackage package = new ExcelPackage();

            package.Workbook.Worksheets.Add(name);
            // access the first sheet named Sheet1
            ExcelWorksheet worksheet = package.Workbook.Worksheets[name];

            int row = 2;
            int col = 1;
            
            worksheet.Cells[row, 7].Value = "Weekly Maintenance PM Rate";
            worksheet.Cells[row, 7].Style.Font.Bold = true;
            worksheet.Cells[row, 7].Style.Font.Size = 16;

            row = row + 2;

            worksheet.Cells[row, col].Value = "WW";
            worksheet.Cells[row, col].Style.Font.Bold = true;
            worksheet.Cells[row, col].Style.Border.BorderAround(ExcelBorderStyle.Thin, System.Drawing.Color.Black);
            
            worksheet.Cells[row + 1, col].Value = "PM %";
            worksheet.Cells[row + 1, col].Style.Font.Bold = true;
            worksheet.Cells[row + 1, col].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            worksheet.Cells[row + 1, col].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Orange);
            worksheet.Cells[row + 1, col].Style.Border.BorderAround(ExcelBorderStyle.Thin, System.Drawing.Color.Black);

            worksheet.Cells[row + 2, col].Value = "Goal";
            worksheet.Cells[row + 2, col].Style.Font.Bold = true;
            worksheet.Cells[row + 2, col].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            worksheet.Cells[row + 2, col].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Orange);
            worksheet.Cells[row + 2, col].Style.Border.BorderAround(ExcelBorderStyle.Thin, System.Drawing.Color.Black);

            foreach (var item in pmtCompletionList)
            {
                col++;
                worksheet.Cells[row, col].Value = item.WeeklyPm;
                worksheet.Cells[row, col].Style.Border.BorderAround(ExcelBorderStyle.Thin, System.Drawing.Color.Black);

                worksheet.Cells[row + 1, col].Value = item.Percentage ;
                worksheet.Cells[row + 1, col].Style.Numberformat.Format = "#0%";
                worksheet.Cells[row + 1, col].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                worksheet.Cells[row + 1, col].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Orange);
                worksheet.Cells[row + 1, col].Style.Border.BorderAround(ExcelBorderStyle.Thin, System.Drawing.Color.Black);

                worksheet.Cells[row + 2, col].Value = 0.9;
                worksheet.Cells[row + 2, col].Style.Numberformat.Format = "#0%";
                worksheet.Cells[row + 2, col].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                worksheet.Cells[row + 2, col].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Orange);
                worksheet.Cells[row + 2, col].Style.Border.BorderAround(ExcelBorderStyle.Thin, System.Drawing.Color.Black);
            }
            // add chart 
            var chart = worksheet.Drawings.AddChart("chart", eChartType.ColumnClustered);
            chart.Series.Add(ExcelRange.GetAddress(5, 2, 5, pmtCompletionList.Count + 1), ExcelRange.GetAddress(4, 2, 4, pmtCompletionList.Count + 1));

            var chartGoal = chart.PlotArea.ChartTypes.Add(eChartType.Line);
            chartGoal.Series.Add(ExcelRange.GetAddress(6, 2, 6, pmtCompletionList.Count + 1), ExcelRange.GetAddress(4, 2, 4, pmtCompletionList.Count + 1));

            chart.Series[0].Fill.Color = System.Drawing.Color.Orange;
            chart.Title.Text = "Weekly Maintenance PM Rate";
            chart.SetSize(600, 300);
            chart.XAxis.Title.Text = "WW"; //give label to x-axis of chart  
            chart.XAxis.Title.Font.Size = 10;
            chart.YAxis.Title.Text = "PM %"; //give label to Y-axis of chart  
            chart.YAxis.Title.Font.Size = 10;
            chart.SetPosition(8, 0, 3, 0);
            chart.Series[0].Header = "Weekly PM";
            chartGoal.Series[0].Header = "Goal";
            chartGoal.Series[0].Border.Fill.Color = System.Drawing.Color.Blue;


            return package;
        }


        public List<ModuleDataAts> getPasanDataDate(FlashDataDto flashDataDto, List<ModuleDataAts> currentlist)
        {
            var serialistDB = context.ModuleDataAts.Where(t => t.TestTime >= flashDataDto.DateFrom && t.TestTime <= flashDataDto.DateTo).ToList();
            bool exist = false;

            foreach (var itemDataBase in serialistDB)
            {
                foreach (var itemCurrent in currentlist)
                {
                    if (itemCurrent.SerialNumberNest1 == itemDataBase.SerialNumberNest1)//validation for duplicate serial
                    {
                        exist = true;
                        break;
                    }
                }

                if (!exist)//if does not exist add a new one
                {
                    currentlist.Add(itemDataBase);
                }
                exist = false;

            }

            return currentlist;
        }


        public List<ModuleData> getQuickSunDate(FlashDataDto flashDataDto, List<ModuleData> currentlist)
        {
            var serialistDB = context.ModuleData.Where(t => t.Time >= flashDataDto.DateFrom && t.Time <= flashDataDto.DateTo).ToList();

            bool exist = false;

            foreach (var itemDataBase in serialistDB)
            {
                foreach (var itemCurrent in currentlist)
                {
                    if (itemCurrent.Sn == itemDataBase.Sn)//validation for duplicate serial
                    {
                        exist = true;
                        break;
                    }
                }

                if (!exist)//if does not exist add a new one
                {
                    currentlist.Add(itemDataBase);
                }
                exist = false;

            }
            return currentlist;
        }


    }
}
