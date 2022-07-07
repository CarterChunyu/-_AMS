using Business;
using Domain;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AMS.Controllers
{
    [CustomAuthorize(AccessLevel = "System,Accounting,Sales,PM,SystemSE,SalesManager")]
    public partial class ACInvoiceController
    {
        public ACInvoiceRewardReport2021Manager aCInvoiceRewardReport2021 { get; set; }

        [HttpGet]
        public ActionResult ACInvoiceRewardReport2021()
        {
            List<SelectListItem> MemberItem = this.GetMemberList();
            List<SelectListItem> CouponItem = GetCouponList();
            ViewBag.PAY_TYPE = MemberItem[0].Value;
            ViewBag.PAY_TYPE_DDL = MemberItem;
            ViewBag.COUPON_TYPE = CouponItem[0].Value;
            ViewBag.COUPON_TYPE_DDL = CouponItem;
            ViewBag.DATE = DateTime.Now.ToString("yyyyMMdd");
            return View();
        }

        [HttpPost]
        public ActionResult ACInvoiceRewardReport2021(FormCollection value)
        {
            aCInvoiceRewardReport2021 = new ACInvoiceRewardReport2021Manager();
            ACInvoiceRewardReport2021.ReportData reportData = new ACInvoiceRewardReport2021.ReportData();
            List<SelectListItem> MemberItem = this.GetMemberList();
            List<SelectListItem> CouponItem = GetCouponList();
            ViewBag.PAY_TYPE = value["PAY_TYPE"];
            ViewBag.PAY_TYPE_DDL = MemberItem;
            ViewBag.COUPON_TYPE = value["COUPON_TYPE"];
            ViewBag.COUPON_TYPE_DDL = CouponItem;
            ViewBag.DATE = value["DATE"];

            if (!string.IsNullOrWhiteSpace(value["PAY_TYPE"].ToString()) && !string.IsNullOrWhiteSpace(value["DATE"].ToString())
                 && !string.IsNullOrWhiteSpace(value["COUPON_TYPE"]))
            {
                reportData = aCInvoiceRewardReport2021.GetReportData(value["PAY_TYPE"].ToString(), value["DATE"].ToString(), value["COUPON_TYPE"].ToString());

                if (reportData.Date != null)
                {
                    ViewBag.DATEdt = reportData.Date;
                }
                if (reportData.Total != null)
                {
                    ViewBag.TOTALdt = reportData.Total;
                }
                if (reportData.Reports != null)
                {
                    ViewBag.REPORTdt = reportData.Reports;
                }
                if (reportData.Details != null)
                {
                    ViewBag.DETAILdt = reportData.Details;
                }
            }

            if (Request.Form["excelConfirm"] != null)
            {

                #region Sheet Name (期數)
                string no = string.Empty;
                if (reportData != null & reportData.Date != null)
                {
                    no = reportData.Date.SEDATE.Split('-')[1].Substring(1, reportData.Date.SEDATE.Split('-')[1].Length - 6);
                }
                #endregion
                #region 檔名&標題
                string coupon_type_name = string.Empty;
                string pay_type_name = string.Empty;
                string pay_type_cname = string.Empty;
                switch (value["COUPON_TYPE"].ToString())
                {
                    case "0":
                        coupon_type_name = "五倍券+好食券";
                        break;
                    case "1":
                        coupon_type_name = "五倍券";
                        break;
                    case "2":
                        coupon_type_name = "好食券";
                        break;
                }
                switch (value["PAY_TYPE"].ToString())
                {
                    case "T00004":
                        pay_type_name = "Icash 2.0";
                        pay_type_cname = "電子票證";
                        break;
                    case "E00005":
                        pay_type_name = "ICP";
                        pay_type_cname = "行動支付";
                        break;
                }
                string TitleName = string.Format("《{0}》{1} 消費者實際靠卡回饋統計報表", coupon_type_name, pay_type_name);
                #endregion
                #region 產出Excel
                if (reportData != null && (reportData.Reports != null || reportData.Details != null))
                {
                    ExcelPackage excel = new ExcelPackage();

                    #region 總表

                    ExcelWorksheet sheet1 = excel.Workbook.Worksheets.Add(string.Format("總表-{0}(第{1}期)", pay_type_cname,no));

                    sheet1.Cells[1, 1].Value = TitleName;
                    sheet1.Cells[1, 1, 1, 8].Merge = true;
                    sheet1.Cells[2, 1, 2, 8].Merge = true;
                    sheet1.Cells[3, 1].Value =string.Format("業者名稱：愛金卡股份有限公司-{0}",pay_type_cname);
                    sheet1.Cells[3, 1, 3, 8].Merge = true;
                    sheet1.Cells[4, 1].Value = reportData.Date != null ? reportData.Date.SEDATE.ToString() : "";
                    sheet1.Cells[4, 1, 4, 8].Merge = true;
                    sheet1.Cells[5, 1].Value = reportData.Total != null ? reportData.Total.TOTAL.ToString() : "";
                    sheet1.Cells[5, 1, 5, 8].Merge = true;
                    sheet1.Cells[6, 1].Value = "序";
                    sheet1.Cells[6, 2].Value = "消費者 UUID";
                    sheet1.Cells[6, 3].Value = "綁定日";
                    sheet1.Cells[6, 4].Value = "綁定序號";
                    sheet1.Cells[6, 5].Value = "交易筆數";
                    sheet1.Cells[6, 6].Value = "交易總額";
                    sheet1.Cells[6, 7].Value = "實際靠卡回饋總額";
                    sheet1.Cells[6, 8].Value = "註記";

                    int row = 6;
                    if (reportData.Reports != null && reportData.Reports.Count > 0)
                    {
                        foreach (var item in reportData.Reports)
                        {
                            sheet1.Cells[row + 1, 1].Value = item.TW_ID;
                            sheet1.Cells[row + 1, 2].Value = item.CUS_UUID;
                            sheet1.Cells[row + 1, 3].Value = item.AUTH_TIME;
                            sheet1.Cells[row + 1, 4].Value = item.TXN_NO;
                            sheet1.Cells[row + 1, 5].Value = item.TRANS_CNT;
                            sheet1.Cells[row + 1, 6].Value = item.TRANS_AMT;
                            sheet1.Cells[row + 1, 7].Value = item.REWARD_AMT;
                            sheet1.Cells[row + 1, 8].Value = item.REMARK;
                            row++;
                        }
                    }

                    #region style
                    sheet1.Cells[1, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    sheet1.Cells[6, 1, 6, 8].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    sheet1.Cells[7,1,row,1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    sheet1.Cells[7, 2, row, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    sheet1.Cells[7, 3, row, 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    sheet1.Cells[7, 4, row, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    sheet1.Cells[7, 5, row, 5].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    sheet1.Cells[7, 6, row, 6].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    sheet1.Cells[7, 7, row, 7].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    sheet1.Cells[7, 8, row, 8].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    sheet1.Cells[6, 1, row, 8].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    sheet1.Cells[6, 1, row, 8].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    sheet1.Cells[6, 1, row, 8].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    sheet1.Cells[6, 1, row, 8].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    #endregion

                    sheet1.Cells[sheet1.Dimension.Address].AutoFitColumns();

                    #endregion
                    #region 明細表
                    ExcelWorksheet sheet2 = excel.Workbook.Worksheets.Add(string.Format("明細表-{0}(第{1}期)",pay_type_cname,no));

                    sheet2.Cells[1, 1].Value = "序";
                    sheet2.Cells[1, 2].Value = "UUID";
                    sheet2.Cells[1, 3].Value = "交易店鋪名稱";
                    sheet2.Cells[1, 4].Value = "交易日期";
                    sheet2.Cells[1, 5].Value = "交易序號或流水編號";
                    sheet2.Cells[1, 6].Value = "交易金額";
                    sheet2.Cells[1, 7].Value = "統編或特店編號";
                    sheet2.Cells[1, 8].Value = "交易店家行業別";
                    sheet2.Cells[1, 9].Value = "實際靠卡回饋總額";
                    sheet2.Cells[1, 10].Value = "提報檔名";

                    row = 2;
                    if (reportData.Details != null && reportData.Details.Count > 0)
                    {
                        foreach (var item in reportData.Details)
                        {
                            sheet2.Cells[row, 1].Value = item.TW_ID;
                            sheet2.Cells[row, 2].Value = item.CUS_UUID;
                            sheet2.Cells[row, 3].Value = item.STORE_NAME;
                            sheet2.Cells[row, 4].Value = item.TRANS_DATE;
                            sheet2.Cells[row, 5].Value = item.TXLOG_ID;
                            sheet2.Cells[row, 6].Value = item.TRANS_AMT;
                            sheet2.Cells[row, 7].Value = item.TAX_ID;
                            sheet2.Cells[row, 8].Value = item.TRANS_STORE_TYPE;
                            sheet2.Cells[row, 9].Value = item.REWARD_AMT;
                            sheet2.Cells[row, 10].Value = item.FILE_NAME;
                            row++;
                        }
                    }

                    #region style
                    sheet2.Cells[sheet1.Dimension.Address].AutoFitColumns();
                    sheet2.Cells[1, 1, 1, 10].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    sheet2.Cells[2, 1, row, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    sheet2.Cells[2, 2, row, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    sheet2.Cells[2, 3, row, 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    sheet2.Cells[2, 4, row, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    sheet2.Cells[2, 5, row, 5].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    sheet2.Cells[2, 6, row, 6].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    sheet2.Cells[2, 7, row, 7].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    sheet2.Cells[2, 8, row, 8].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    sheet2.Cells[2, 9, row, 9].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    sheet2.Cells[2, 10, row, 10].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    #endregion
                    #endregion

                    var stream = new MemoryStream();
                    excel.SaveAs(stream);
                    stream.Close();

                    return File(stream.ToArray(), "application/vnd.ms-excel", string.Format("{0}-{1}.xlsx",pay_type_cname,TitleName));
                }
                #endregion
            }
            return View();
        }

        /// <summary>
        /// 取得券種清單
        /// </summary>
        public List<SelectListItem> GetCouponList()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = "全部", Value = "0" });
            items.Add(new SelectListItem { Text = "五倍券", Value = "1" });
            items.Add(new SelectListItem { Text = "好食券", Value = "2" });
            return items;
        }
    }
}