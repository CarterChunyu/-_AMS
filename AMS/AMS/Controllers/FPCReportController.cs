using Business;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using System.Net;
using System.IO.Compression;

namespace AMS.Controllers
{
    [CustomAuthorize(AccessLevel = "System,Accounting,Sales,SalesManager,")]
    public class FPCReportController : BaseController
    {
        public ICASHOPReportManager ICASHOPREPManager { get; set; }

        public FPCReportController()
        {
            ICASHOPREPManager = new ICASHOPReportManager();
        }
        public void Index()
        {
            try
            {
                string yearMonth = "";
                //只取台塑資料
                string Group_TaiYa = "";//台亞
                string Group_TaiSu = "Y";//台塑
                string Group_XiOu = "";//西歐
                string Group_FuMou = "";//福懋
                string Group_TongYiDuJiaCun = "";//--統一渡假村
                string ftpUserName = "user";
                string ftpPassword = "test123";
                string ftpUrl = "ftp://localhost:2222/folder/path/filename.zip";


                yearMonth = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString().PadLeft(2, '0');


                DataTable dt_01 = ICASHOPREPManager.Report_01(yearMonth, Group_TaiYa, Group_TaiSu, Group_XiOu, Group_FuMou, Group_TongYiDuJiaCun);
                using (var memoryStream = new MemoryStream())
                {
                    var csv = new StringBuilder();
                    using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                    {
                        for (int i = 0; i < dt_01.Columns.Count; i++)
                        {
                            csv.Append(dt_01.Columns[i]);
                            if (i < dt_01.Columns.Count - 1)
                            {
                                csv.Append(",");
                            }
                        }
                        csv.AppendLine();

                        foreach (DataRow row in dt_01.Rows)
                        {
                            for (int i = 0; i < dt_01.Columns.Count; i++)
                            {
                                if (!Convert.IsDBNull(row[i]))
                                {
                                    string value = row[i].ToString();
                                    if (value.Contains(","))
                                    {
                                        value = String.Format("\"{0}\"", value);
                                        csv.Append(value);
                                    }
                                    else
                                    {
                                        csv.Append(value);
                                    }
                                }
                                if (i < dt_01.Columns.Count - 1)
                                {
                                    csv.Append(",");
                                }
                            }
                            csv.AppendLine();
                        }
                        var file = archive.CreateEntry("tempFilename.csv");
                        using (var streamWriter = new StreamWriter(file.Open()))
                        {
                            streamWriter.Write(csv);
                        }
                    }
                    FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftpUrl);
                    request.Method = WebRequestMethods.Ftp.UploadFile;

                    request.Credentials = new NetworkCredential(ftpUserName, ftpPassword);
                    //建立file stream
                    //byte[] fileContent = Encoding.UTF8.GetBytes(csv.ToString());

                    using (Stream requestStream = request.GetRequestStream())
                    {
                        //requestStream.Write(fileContent, 0, fileContent.Length);0
                        memoryStream.CopyTo(requestStream);
                    }

                    using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
                    {
                        //Ftp server 回應訊息

                    }

                }



                //後續不回應，改放FTP
                //return File(stream.ToArray(), "application/vnd.ms-excel", ViewBag.RepName + yearMonth + ".xlsx");
                //File(stream.ToArray(), "application/vnd.ms-excel", ViewBag.RepName + yearMonth + ".xlsx");

                //設定FTP 連線URL & 設定本次連線是要上傳檔案

            }
            catch (Exception ex)
            {
                //執行出現異常
            }
        }

    }
}