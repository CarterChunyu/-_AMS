using Business;
using DataAccess;
using Domain.ICASHOPOverdraft;
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
    public class ICASHOPOverdraftController : BaseController
    {
        public ActionResult Index()
        {
            ViewBag.REPORT_YM = DateTime.Now.ToString("yyyyMM");
            return View();
        }

        [HttpPost]
        public ActionResult Index(FormCollection value)
        {
            ICASHOPOverdraftManager iCASHOPOverdraftManager = new ICASHOPOverdraftManager();
            List<OverdraftData> OverdraftList = new List<OverdraftData>();
            string REPORT_YM = value["REPORT_YM"];
            ViewBag.REPORT_YM = REPORT_YM;
            if (!string.IsNullOrWhiteSpace(REPORT_YM))
            {
                OverdraftList = iCASHOPOverdraftManager.GetData(REPORT_YM);
                ViewBag.OverdraftList = OverdraftList;
                decimal OverdraftPointTotal =0- OverdraftList.Sum(x => x.OverdraftPoint);
                decimal CorrectionPointTotal = OverdraftList.Sum(x => x.CorrectionPoint);
                decimal OverdraftAndCorrectionTotal = OverdraftPointTotal + CorrectionPointTotal;
                ViewBag.OverdraftPointTotal = OverdraftPointTotal;
                ViewBag.CorrectionPointTotal = CorrectionPointTotal;
                ViewBag.OverdraftAndCorrectionTotal = OverdraftAndCorrectionTotal;

                if (Request.Form["ExportExcel"] != null)
                {
                    ExcelPackage excel = new ExcelPackage();

                    #region sheet1
                    ExcelWorksheet sheet1 = excel.Workbook.Worksheets.Add("總計");
                    sheet1.Cells[1, 1].Value = "OP代墊總點數";
                    sheet1.Cells[1, 2].Value = "用戶補正總點數";
                    sheet1.Cells[1, 3].Value = "代墊+補正總點數";
                    sheet1.Cells[2, 1].Value = OverdraftPointTotal;
                    sheet1.Cells[2, 2].Value = CorrectionPointTotal;
                    sheet1.Cells[2, 3].Value = OverdraftAndCorrectionTotal;
                    sheet1.Cells[sheet1.Dimension.Address].AutoFitColumns();
                    #endregion

                    #region sheet2
                    ExcelWorksheet sheet2 = excel.Workbook.Worksheets.Add("明細");

                    #region 欄位名稱
                    sheet2.Cells[1, 1].Value = "點數日結日期";
                    sheet2.Cells[1, 2].Value = "通路代號";
                    sheet2.Cells[1, 3].Value = "店號";
                    sheet2.Cells[1, 4].Value = "機號";
                    sheet2.Cells[1, 5].Value = "交易序號";
                    sheet2.Cells[1, 6].Value = "交易時間";
                    sheet2.Cells[1, 7].Value = "卡別";
                    sheet2.Cells[1, 8].Value = "卡號";
                    sheet2.Cells[1, 9].Value = "點別";
                    sheet2.Cells[1, 10].Value = "OP代墊負點數";
                    sheet2.Cells[1, 11].Value = "用戶補正負點數";
                    sheet2.Cells[1, 12].Value = "銷帳編號";
                    sheet2.Cells[1, 13].Value = "建立日期";
                    sheet2.Cells[1, 14].Value = "檔案名稱";
                    sheet2.Cells[1, 1, 1, 14].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    #endregion

                    if (OverdraftList.Count > 0)
                    {
                        #region 資料列
                        int irow = 1;
                        foreach (var item in OverdraftList)
                        {
                            sheet2.Cells[(irow + 1), 1].Value = item.CptDate;
                            sheet2.Cells[(irow + 1), 2].Value = item.UnifiedBusinessNo;
                            sheet2.Cells[(irow + 1), 3].Value = item.StoreNo;
                            sheet2.Cells[(irow + 1), 4].Value = item.PosNo;
                            sheet2.Cells[(irow + 1), 5].Value = item.TransNo;
                            sheet2.Cells[(irow + 1), 6].Value = item.TransDate;
                            sheet2.Cells[(irow + 1), 7].Value = item.CardType;
                            sheet2.Cells[(irow + 1), 8].Value = item.CardNo;
                            sheet2.Cells[(irow + 1), 9].Value = item.PointType;
                            sheet2.Cells[(irow + 1), 10].Value = item.OverdraftPoint;
                            sheet2.Cells[(irow + 1), 11].Value = item.CorrectionPoint;
                            sheet2.Cells[(irow + 1), 12].Value = item.WriteOffNo;
                            sheet2.Cells[(irow + 1), 13].Value = item.CreateDate;
                            sheet2.Cells[(irow + 1), 14].Value = item.FileName;
                            irow++;
                        }
                        sheet2.Cells[2, 1, irow, 14].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        sheet2.Cells[2, 10, irow, 11].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                        sheet2.Cells[sheet2.Dimension.Address].AutoFitColumns();
                        #endregion   
                    }
                    #endregion

                    var stream = new MemoryStream();
                    excel.SaveAs(stream);
                    stream.Close();

                    return File(stream.ToArray(), "application/vnd.ms-excel", string.Format("每日代墊款明細_{0}", REPORT_YM) + ".xlsx");
                }
            }
            return View();
        }

    }
}