using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;

using System.IO;
using Business;
using Domain;

using OfficeOpenXml;
using System.Xml;
using OfficeOpenXml.Style;

namespace AMS.Controllers
{
    [CustomAuthorize(AccessLevel = "System,Accounting,Sales,PM,SystemSE,SalesManager")]
    public class ReportBankController : BaseController
    {
        //
        public ReportBankManager RBankManager { get; set; }
        //
        public ReportBankController()
        {
            RBankManager = new ReportBankManager(); 
        }

        public ActionResult RPT_161001()
        {
            int year = DateTime.Now.Year;
            int month = DateTime.Now.Month;
            DateTime startDate = new DateTime(year, month, 1);
            string start = startDate.ToString("yyyyMMdd");
            string end = DateTime.Now.ToString("yyyyMMdd");

            string bank = "";
            string cardId = "";

            ViewBag.RepName = "餘返異常明細表";

            if (Request.Form["searchConfirm"] != null || Request.Form["ExportExcel"] != null)
            {
                start = Request.Form["sDate"];
                end = Request.Form["eDate"];
                bank = Request.Form["bank"];
                cardId = Request.Form["cardId"];
            }

            ViewBag.sDate = start;
            ViewBag.eDate = end;
            ViewBag.bank = bank;
            ViewBag.cardId = cardId;


            List<SelectListItem> banksList = new List<SelectListItem>();

            List<GmMerchant> merchants = new GmMerchantManager().FindAllBnak();
            IEnumerable<GmMerchant> distinctMerchants = from k in merchants
                                                        group k by new { k.MerchantNo, k.MerchantName } into g
                                                        select g.FirstOrDefault();
            SelectList list = new SelectList(distinctMerchants, "MerchantNo", "MerchantName");
            banksList.AddRange(list);

            ViewBag.BanksList = banksList;

            DataTable dt = RBankManager.ReportBank161001(start, end, bank, cardId);

            if (Request.Form["searchConfirm"] != null)
            { return View(dt); }
            else if (Request.Form["ExportExcel"] != null)
            {
                using (ExcelPackage p = new ExcelPackage())
                {
                    ExcelWorksheet ws = p.Workbook.Worksheets.Add(ViewBag.RepName.ToString());


                    ExcelRange r = ws.Cells["A1:I1"];
                    r.Merge = true;
                    r.Value = ViewBag.RepName.ToString();


                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    r.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                    //r.Style.Font.Bold = true;
                    r.Style.Font.Size = 14;

                    int startRowNumber = 2;
                    var columns = new[] { "清分日", "異常代號", "卡號", "銀行別","交易別", "交易日", "本次交易金額", "儲值序號", "消費序號" };

                    for (int j = 1; j <= columns.Length; j++)
                    {
                        //設值為欄位名稱
                        ws.Cells[startRowNumber, j].Value = columns[j - 1];
                        //設定樣式
                        ws.Cells[startRowNumber, j].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center; //欄位置中
                        ws.Cells[startRowNumber, j].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                        ws.Cells[startRowNumber, j].Style.Font.Bold = true;
                        ws.Cells[startRowNumber, j].Style.Font.Size = 12;
                    }

                    for (int i = 1; i <= dt.Rows.Count; i++)
                    {

                        for (int j = 1; j <= columns.Length; j++)
                        {
                            switch (j)
                            {
                                case 7:
                                    ws.Cells[i + 2, j].Value = Convert.ToDouble(dt.Rows[i - 1][j - 1]);


                                    ws.Cells[i + 2, j].Style.Numberformat.Format = "#,##0";
                                    ws.Cells[i + 2, j].Style.Font.Size = 12;
                                    ws.Cells[i + 2, j].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;//欄位置右
                                    ws.Cells[i + 2, j].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                                    break;
                                default:
                                    ws.Cells[i + 2, j].Value = dt.Rows[i - 1][j - 1].ToString();

                                    ws.Cells[i + 2, j].Style.Font.Size = 12;
                                    ws.Cells[i + 2, j].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center; //欄位置中
                                    ws.Cells[i + 2, j].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                                    break;
                            }

                            //設定樣式
                            //ws.Cells[i + 2, j].AutoFitColumns(); //自動欄寬

                        }
                    }

                    ws.Cells[ws.Dimension.Address].AutoFitColumns();

                    var stream = new MemoryStream();
                    p.SaveAs(stream);
                    stream.Close();

                    return File(stream.ToArray(), "application/vnd.ms-excel", ViewBag.RepName + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx");
                }

            }
            else return View(new DataTable());
        }

        public ActionResult RPT_161002()
        {
            string year = DateTime.Now.Year.ToString();
            string month = DateTime.Now.Month.ToString().PadLeft(2,'0');

            string yearMonth = year + month;

            string bankMerchant = "";

            if (Request.Form["searchConfirm"] != null || Request.Form["ExportExcel"] != null)
            {
                yearMonth = Request.Form["yearMonth"];
                bankMerchant = Request.Form["bankMerchant"];               
            }

            ViewBag.YearMonth = yearMonth;
            ViewBag.BankMerchant = bankMerchant;

            ViewBag.RepName = "發卡暨補/換/續卡月明細表";

            List<SelectListItem> banksList = new List<SelectListItem>();

            List<GmMerchant> merchants = new GmMerchantManager().FindAllBnak();
            IEnumerable<GmMerchant> distinctMerchants = from k in merchants
                                                        group k by new { k.MerchantNo, k.MerchantName } into g
                                                        select g.FirstOrDefault();
            SelectList list = new SelectList(distinctMerchants, "MerchantNo", "MerchantName");
            banksList.AddRange(list);

            ViewBag.BanksList = banksList;

            string lastYearMonth = DateTime.ParseExact(yearMonth + "01", "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture).AddMonths(-1).ToString("yyyyMM");

            DataTable dt = RBankManager.ReportBank161002(lastYearMonth+"25", yearMonth+"24", bankMerchant);

            if (Request.Form["searchConfirm"] != null)
            { return View(dt); }
            else if (Request.Form["ExportExcel"] != null)
            {
                using (ExcelPackage p = new ExcelPackage())
                {
                    ExcelWorksheet ws = p.Workbook.Worksheets.Add(ViewBag.RepName.ToString());


                    ExcelRange r = ws.Cells["A1:E1"];
                    r.Merge = true;
                    r.Value = ViewBag.RepName.ToString();


                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    r.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                    //r.Style.Font.Bold = true;
                    r.Style.Font.Size = 14;

                    int startRowNumber = 2;
                    var columns = new[] { "愛金卡認列日", "卡號", "申辦原因", "第三方聯名", "卡款名稱" };

                    for (int j = 1; j <= columns.Length; j++)
                    {
                        //設值為欄位名稱
                        ws.Cells[startRowNumber, j].Value = columns[j - 1];
                        //設定樣式
                        ws.Cells[startRowNumber, j].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center; //欄位置中
                        ws.Cells[startRowNumber, j].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                        ws.Cells[startRowNumber, j].Style.Font.Bold = true;
                        ws.Cells[startRowNumber, j].Style.Font.Size = 12;
                    }

                    for (int i = 1; i <= dt.Rows.Count; i++)
                    {

                        for (int j = 1; j <= columns.Length; j++)
                        {
                            switch (j)
                            {
                                /*
                                case 6:
                                    ws.Cells[i + 2, j].Value = Convert.ToDouble(dt.Rows[i - 1][j - 1]);


                                    ws.Cells[i + 2, j].Style.Numberformat.Format = "#,##0";
                                    ws.Cells[i + 2, j].Style.Font.Size = 12;
                                    ws.Cells[i + 2, j].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;//欄位置右
                                    ws.Cells[i + 2, j].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                                    break;
                                */
                                default:
                                    ws.Cells[i + 2, j].Value = dt.Rows[i - 1][j - 1].ToString();

                                    ws.Cells[i + 2, j].Style.Font.Size = 12;
                                    ws.Cells[i + 2, j].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center; //欄位置中
                                    ws.Cells[i + 2, j].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                                    break;
                            }

                        }
                    }

                    ws.Cells[ws.Dimension.Address].AutoFitColumns();

                    var stream = new MemoryStream();
                    p.SaveAs(stream);
                    stream.Close();

                    return File(stream.ToArray(), "application/vnd.ms-excel", ViewBag.RepName + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx");
                }

            }
            else return View(new DataTable());

        }
	}
}