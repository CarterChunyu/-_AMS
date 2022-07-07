using Business;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AMS.Controllers
{
    [CustomAuthorize(AccessLevel = "System,Accounting,Sales,SalesManager,")]
    public class ICASHOPReportController : BaseController
    {
        public ICASHOPReportManager ICASHOPREPManager { get; set; }

        public ICASHOPReportController()
        {
            ICASHOPREPManager = new ICASHOPReportManager();
        }
        

        public ActionResult Index(string[] checkboxs)
        {
            ViewBag.RepName = "自串點數明細與發票檔原資料";

            string yearMonth = "";
            string Group_TaiYa = "";//台亞
            string Group_TaiSu = "";//台塑
            string Group_XiOu = "";//西歐
            string Group_FuMou = "";//福懋
            string Group_TongYiDuJiaCun = "";//--統一渡假村

            
            yearMonth = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString().PadLeft(2, '0');
            if (Request.Form["ExportExcel"] != null)
            {
                yearMonth = Request.Form["yearMonth"];
            }

            if (checkboxs != null)
            {
                for (int i = 0; i <= checkboxs.Length - 1; i++)
                {
                    int checkboxsValue = Convert.ToInt16(checkboxs[i]);
                    switch (checkboxsValue)
                    {
                        case 0:
                            Group_TaiYa = "Y";
                            break;
                        case 1:
                            Group_TaiSu = "Y";
                            break;
                        case 2:
                            Group_XiOu = "Y";
                            break;
                        case 3:
                            Group_FuMou = "Y";
                            break;
                        case 4:
                            Group_TongYiDuJiaCun = "Y";
                            break;
                        default:
                            break;
                    }
                }
            }
            
            ViewBag.YearMonth = yearMonth;
            
            
            if (Request.Form["ExportExcel"] != null)
            {
                DataTable dt_01 = ICASHOPREPManager.Report_01(yearMonth, Group_TaiYa, Group_TaiSu, Group_XiOu, Group_FuMou, Group_TongYiDuJiaCun);
                DataTable dt_02 = ICASHOPREPManager.Report_02(yearMonth, Group_TaiYa, Group_TaiSu, Group_XiOu, Group_FuMou, Group_TongYiDuJiaCun);
                DataTable dt_03 = ICASHOPREPManager.Report_03(yearMonth, Group_TaiYa, Group_TaiSu, Group_XiOu, Group_FuMou, Group_TongYiDuJiaCun);
                DataTable dt_04 = ICASHOPREPManager.Report_04(yearMonth, Group_TaiYa, Group_TaiSu, Group_XiOu, Group_FuMou, Group_TongYiDuJiaCun);

                using (ExcelPackage p = new ExcelPackage())
                {
                    ExcelWorksheet ws = p.Workbook.Worksheets.Add("明細_銷帳編號日期");

                    int startRowNumber = 1;
                    var columns = new[] { "機構日期","CptDate", "UnifiedBusinessName", "StoreNo", "StoreName", "PosNo", "TransNo", "TransDate", "TransType", "Amount", "CardType", "CardNo", "Point", "WriteOffNo",
                                                  "CreateDate", "FileName", "UnifiedBusinessNo" };

                    for (int j = 1; j <= columns.Length; j++)
                    {
                        //設值為欄位名稱
                        ws.Cells[startRowNumber, j].Value = columns[j - 1];
                        //設定樣式
                        ws.Cells[startRowNumber, j].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center; //欄位置中     
                        ws.Cells[startRowNumber, j].Style.Font.Size = 12;
                    }

                    for (int i = 1; i <= dt_01.Rows.Count; i++)
                    {

                        for (int j = 1; j <= columns.Length; j++)
                        {
                            switch (j)
                            {
                                case 10:
                                case 13:
                                    ws.Cells[i + 1, j].Value = Convert.ToDecimal(dt_01.Rows[i - 1][j - 1]);

                                    ws.Cells[i + 1, j].Style.Font.Size = 12;
                                    ws.Cells[i + 1, j].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center; //欄位置中

                                    break;

                                default:
                                    ws.Cells[i + 1, j].Value = dt_01.Rows[i - 1][j - 1].ToString();

                                    ws.Cells[i + 1, j].Style.Font.Size = 12;
                                    ws.Cells[i + 1, j].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center; //欄位置中

                                    break;
                            }
                        }
                    }

                    ws.Cells.AutoFitColumns();

                    ExcelWorksheet ws2 = p.Workbook.Worksheets.Add("明細_OP日期");
                    var columns2 = new[] { "OP日期", "CptDate", "UnifiedBusinessName", "StoreNo", "StoreName", "PosNo", "TransNo", "TransDate", "TransType", "Amount", "CardType", "CardNo", "Point", "WriteOffNo", "CreateDate", "FileName", "UnifiedBusinessNo" };

                    for (int j = 1; j <= columns2.Length; j++)
                    {
                        //設值為欄位名稱
                        ws2.Cells[startRowNumber, j].Value = columns2[j - 1];
                        //設定樣式
                        ws2.Cells[startRowNumber, j].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center; //欄位置中
                        ws2.Cells[startRowNumber, j].Style.Font.Size = 12;
                    }

                    for (int i = 1; i <= dt_02.Rows.Count; i++)
                    {

                        for (int j = 1; j <= columns2.Length; j++)
                        {
                            switch (j)
                            {
                                case 10:
                                case 13:
                                    ws2.Cells[i + 1, j].Value = Convert.ToDecimal(dt_02.Rows[i - 1][j - 1]);

                                    ws2.Cells[i + 1, j].Style.Font.Size = 12;
                                    ws2.Cells[i + 1, j].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center; //欄位置中

                                    break;

                                default:
                                    ws2.Cells[i + 1, j].Value = dt_02.Rows[i - 1][j - 1].ToString();

                                    ws2.Cells[i + 1, j].Style.Font.Size = 12;
                                    ws2.Cells[i + 1, j].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center; //欄位置中

                                    break;
                            }
                        }
                    }

                    ws2.Cells.AutoFitColumns();

                    ExcelWorksheet ws3 = p.Workbook.Worksheets.Add("兩明細差異");
                    var columns3 = new[] { "OP日期", "機構日期", "CptDate", "機構認列月份","UnifiedBusinessName", "StoreNo", "StoreName", "PosNo", "TransNo", "TransDate", "TransType", "Amount", "CardType", "CardNo", "Point", "WriteOffNo", "CreateDate", "FileName", "UnifiedBusinessNo" };

                    for (int j = 1; j <= columns3.Length; j++)
                    {
                        //設值為欄位名稱
                        ws3.Cells[startRowNumber, j].Value = columns3[j - 1];
                        //設定樣式
                        ws3.Cells[startRowNumber, j].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center; //欄位置中
                        ws3.Cells[startRowNumber, j].Style.Font.Size = 12;
                    }

                    for (int i = 1; i <= dt_03.Rows.Count; i++)
                    {

                        for (int j = 1; j <= columns3.Length; j++)
                        {
                            switch (j)
                            {
                                case 12:
                                case 15:
                                    ws3.Cells[i + 1, j].Value = Convert.ToDecimal(dt_03.Rows[i - 1][j - 1]);

                                    ws3.Cells[i + 1, j].Style.Font.Size = 12;
                                    ws3.Cells[i + 1, j].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center; //欄位置中

                                    break;

                                default:
                                    ws3.Cells[i + 1, j].Value = dt_03.Rows[i - 1][j - 1].ToString();

                                    ws3.Cells[i + 1, j].Style.Font.Size = 12;
                                    ws3.Cells[i + 1, j].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center; //欄位置中

                                    break;
                            }
                        }
                    }

                    ws3.Cells.AutoFitColumns();

                    ExcelWorksheet ws4 = p.Workbook.Worksheets.Add("發票檔原資料");
                    var columns4 = new[] { "UnifiedBusinessNo", "MerchantName", "TotalGetPoint(OP帳)", "TotalRedeemPoint(OP帳)", "TotalGetPoint(機構帳)", "TotalRedeemPoint(機構帳)" };

                    for (int j = 1; j <= columns4.Length; j++)
                    {
                        //設值為欄位名稱
                        ws4.Cells[startRowNumber, j].Value = columns4[j - 1];
                        //設定樣式
                        ws4.Cells[startRowNumber, j].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center; //欄位置中
                        ws4.Cells[startRowNumber, j].Style.Font.Size = 12;
                    }

                    for (int i = 1; i <= dt_04.Rows.Count; i++)
                    {

                        for (int j = 1; j <= columns4.Length; j++)
                        {
                            switch (j)
                            {
                                case 3:
                                case 4:
                                case 5:
                                case 6:
                                    if (!String.IsNullOrWhiteSpace(dt_04.Rows[i - 1][j - 1].ToString()))
                                    {
                                        ws4.Cells[i + 1, j].Value = Convert.ToDecimal(dt_04.Rows[i - 1][j - 1]);
                                    }
                                    else
                                    {
                                        ws4.Cells[i + 1, j].Value = "";
                                    }


                                    ws4.Cells[i + 1, j].Style.Font.Size = 12;
                                    ws4.Cells[i + 1, j].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center; //欄位置中

                                    break;

                                default:
                                    ws4.Cells[i + 1, j].Value = dt_04.Rows[i - 1][j - 1].ToString();

                                    ws4.Cells[i + 1, j].Style.Font.Size = 12;
                                    ws4.Cells[i + 1, j].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center; //欄位置中

                                    break;
                            }
                        }
                    }

                    ws4.Cells.AutoFitColumns();

                    var stream = new MemoryStream();
                    p.SaveAs(stream);
                    stream.Close();

                    return File(stream.ToArray(), "application/vnd.ms-excel", ViewBag.RepName + yearMonth + ".xlsx");
                }

            }
            return View();
        }
    }
}