using Business;
using DataAccess;
using Domain;
using Domain.Entities;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Data;

using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.Drawing;
using System.Web.UI;
using System.Web.UI.DataVisualization.Charting;
using System.Web.UI.WebControls;
using NPOI.SS.Util;
using Microsoft.AspNet.Identity;

using OfficeOpenXml;
using System.Xml;
using OfficeOpenXml.Style;


namespace AMS.Controllers
{
    [CustomAuthorize(AccessLevel = "System,Accounting,Sales,PM,SystemSE,SalesManager")]
    public class ReportController : BaseController
    {
        public ReportManager reportManager { get; set; }
        public GmMerchantManager gmMerchantManager { get; set; }

        public ReportController()
        {
            reportManager = new ReportManager();
            gmMerchantManager = new GmMerchantManager();
        }

        public List<SelectListItem> SetGroup(string group)
        {
            List<SelectListItem> groups = new List<SelectListItem>();
            groups.AddRange(new SelectList(gmMerchantManager.FindAllGroup(), "MerchantNo", "MerchantName", group));
            groups.Add(new SelectListItem() { Text = "ALL", Value = "ALL", Selected = false });
            return groups;

        }
        public List<SelectListItem> SetGroup2(string group)
        {
            List<SelectListItem> groups = new List<SelectListItem>();
            groups.AddRange(new SelectList(gmMerchantManager.FindGroupNoMstore(), "MerchantNo", "MerchantName", group));
            groups.Add(new SelectListItem() { Text = "ALL", Value = "ALL", Selected = false });
            return groups;

        }

        public List<SelectListItem> SetRetail(string retail)
        {
            List<SelectListItem> retails = new List<SelectListItem>();
            retails.AddRange(new SelectList(gmMerchantManager.FindAllRetail(), "MerchantNo", "MerchantName", retail));
            retails.Add(new SelectListItem() { Text = "ALL", Value = "ALL", Selected = false });
            return retails;

        }

        public List<SelectListItem> SetBus(string bus)
        {
            List<SelectListItem> buses = new List<SelectListItem>();
            buses.AddRange(new SelectList(gmMerchantManager.FindAllBus(), "MerchantNo", "MerchantName", bus));
            buses.Add(new SelectListItem() { Text = "ALL", Value = "ALL", Selected = false });
            return buses;

        }

        public List<SelectListItem> SetBike(string bike)
        {
            List<SelectListItem> bikes = new List<SelectListItem>();
            bikes.AddRange(new SelectList(gmMerchantManager.FindAllBike(), "MerchantNo", "MerchantName", bike));
            bikes.Add(new SelectListItem() { Text = "ALL", Value = "ALL", Selected = false });
            return bikes;

        }

        public List<SelectListItem> SetTrack(string track)
        {
            List<SelectListItem> tracks = new List<SelectListItem>();
            tracks.AddRange(new SelectList(gmMerchantManager.FindAllTrack(), "MerchantNo", "MerchantName", track));
            tracks.Add(new SelectListItem() { Text = "ALL", Value = "ALL", Selected = false });
            return tracks;

        }

        public List<SelectListItem> SetParking(string parking)
        {
            List<SelectListItem> parkings = new List<SelectListItem>();
            parkings.AddRange(new SelectList(gmMerchantManager.FindAllParking(), "MerchantNo", "MerchantName", parking));
            parkings.Add(new SelectListItem() { Text = "ALL", Value = "ALL", Selected = false });
            return parkings;

        }

        public List<SelectListItem> SetOutsourcing(string outsourcing)
        {
            List<SelectListItem> outsourcings = new List<SelectListItem>();
            outsourcings.AddRange(new SelectList(gmMerchantManager.FindAllOutsourcing(), "MerchantNo", "MerchantName", outsourcing));
            outsourcings.Add(new SelectListItem() { Text = "ALL", Value = "ALL", Selected = false });
            return outsourcings;

        }
        public List<SelectListItem> SetRetailNotXDD(string retail)
        {
            List<SelectListItem> retails = new List<SelectListItem>();
            retails.AddRange(new SelectList(gmMerchantManager.FindRetailNotXDD(), "MerchantNo", "MerchantName", retail));
            retails.Add(new SelectListItem() { Text = "ALL", Value = "ALL", Selected = false });
            return retails;
        }

        // GET: Report
        public ActionResult Report0111Index()
        {
            string startYM = DateTime.Today.ToString("yyyyMM");
            ViewBag.StartYM = startYM;
            return View();
        }

        // 初始化DropDownList      
        List<SelectListItem> GetSelectItem(bool dvalue = true)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            if (dvalue) { items.Insert(0, new SelectListItem { Text = "--請選擇--", Value = "" }); }
            return items;
        }

        // 第一層下拉選項       
        private List<SelectListItem> SetMerchantDropDown(string no = "")
        {
            List<SelectListItem> items = GetSelectItem();
            items.AddRange(new SelectList(this.gmMerchantManager.FindAll(), "MerchantNo", "MerchantName", no));
            return items;
        }

        private List<SelectListItem> SetMerchantBankDropDown(string no = "")
        {
            List<SelectListItem> items = GetSelectItem();
            items.AddRange(new SelectList(this.gmMerchantManager.FindAllBnak(), "MerchantNo", "MerchantName", no));
            return items;
        }
        private List<SelectListItem> SetMerchantBankWithAll(string no = "")
        {
            List<SelectListItem> items = GetSelectItem();
            items.AddRange(new SelectList(this.gmMerchantManager.FindAllBnak(), "MerchantNo", "MerchantName", no));
            items.Add(new SelectListItem() { Text = "ALL", Value = "ALL", Selected = false });
            return items;
        }

        private List<SelectListItem> SetMerchantRAMTDropDown(string no = "")
        {
            List<SelectListItem> items = GetSelectItem();
            items.AddRange(new SelectList(this.gmMerchantManager.FindAllRAMT(), "MerchantNo", "MerchantName", no));
            return items;
        }

        private List<SelectListItem> SetStoreDropDown(string no = "")
        {
            List<SelectListItem> items = GetSelectItem();
            items.AddRange(new SelectList(this.gmMerchantManager.FindAllStore(), "MerchantNo", "MerchantName", no));
            return items;
        }

        /// <summary>
        /// 找群組內所有的特約機構
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        public List<SelectListItem> SetAllbyGroup(string group)
        {
            List<SelectListItem> parkings = new List<SelectListItem>();
            parkings.AddRange(new SelectList(gmMerchantManager.FindAllbyGroup(group), "MerchantNo", "MerchantName", group));
            parkings.Add(new SelectListItem() { Text = "ALL", Value = "ALL", Selected = false });
            return parkings;

        }
        public class Merchant
        {
            public string MerchantNo { get; set; }

            public string MerchantName { get; set; }
        }

        [HttpPost]
        public JsonResult GetMerchant(string groupName)
        {
            List<KeyValuePair<string, string>> items = new List<KeyValuePair<string, string>>();

            if (!string.IsNullOrWhiteSpace(groupName))
            {
                var merchants = this.GetMerchantData(groupName);
                if (merchants != null && merchants.Count() > 0)
                {
                    foreach (var merchant in merchants)
                    {
                        items.Add(new KeyValuePair<string, string>(
                            merchant.Value.ToString(),
                            merchant.Text));
                    }
                }
            }
            return this.Json(items);
        }

        private List<SelectListItem> GetMerchantData(string groupName)
        {
            switch (groupName)
            {
                case "RETAIL":
                    return SetAllbyGroup(groupName);
                case "BUS":
                    return SetAllbyGroup(groupName);
                case "BIKE":
                    return SetAllbyGroup(groupName);
                case "PARKING_LOT":
                    return SetAllbyGroup(groupName);
                case "TRACK":
                    return SetAllbyGroup(groupName);
                case "BANK_OUTSOURCING":
                    return SetAllbyGroup(groupName);
                case "":
                case "ALL":
                    return null;
                default:
                    return SetAllbyGroup(groupName);
            }
        }

        public ActionResult Report0124Result()
        {
            DateTime startDate = DateTime.Now;
            DateTime endDate = DateTime.Now;
            string start = startDate.ToString("yyyyMMdd");
            string end = endDate.ToString("yyyyMMdd");
            string StartA = "";
            string EndA = "";
            string StartB = "";
            string EndB = "";
            ViewBag.RepName = "電支儲電票交易報表";
            if (Request.Form["StartA"] == null)
            {
                ViewBag.StartA = start;
            }
            else
            {
                ViewBag.StartA = Request.Form["StartA"].ToString();
            }
            StartA = ViewBag.StartA;

            if (Request.Form["EndA"] == null)
            {
                ViewBag.EndA = end;
            }
            else
            {
                ViewBag.EndA = Request.Form["EndA"].ToString();
            }
            EndA = ViewBag.EndA;

            if (String.IsNullOrEmpty(Request.Form["StartB"]))
            {
                ViewBag.StartB = "";
                StartB = "-";
            }
            else
            {
                ViewBag.StartB = Request.Form["StartB"].ToString();
                StartB = ViewBag.StartB;
            }

            if (String.IsNullOrEmpty(Request.Form["EndB"]))
            {
                ViewBag.EndB = "";
                EndB = "-";
            }
            else
            {
                ViewBag.EndB = Request.Form["EndB"].ToString();
                EndB = ViewBag.EndB;
            }

            ViewBag.condition2_val_1 = "";
            ViewBag.condition2_val_2 = "";
            ViewBag.spancondi2_visibility = "";
            if (ViewBag.EndB == "")
            {
                ViewBag.condition2_val_1 = "checked";
                ViewBag.spancondi2_visibility = "hidden";
            }
            else
            {
                ViewBag.condition2_val_2 = "checked";
            }

            DataTable dt = new DataTable();
            if (Request.Form["searchConfirm"] != null || Request.Form["ExportExcel"] != null)
            {
                dt = reportManager.Report0124T(StartA, EndA, StartB, EndB);
                if (dt == null)
                {
                    ViewBag.totalCnt = 0;
                }
                else
                {
                    ViewBag.totalCnt = dt.Rows.Count;
                }

                if (Request.Form["searchConfirm"] != null)
                    return View(dt);
                else if (Request.Form["ExportExcel"] != null)
                {
                    //Mark Execl
                    using (ExcelPackage p = new ExcelPackage())
                    {
                        ExcelWorksheet ws = p.Workbook.Worksheets.Add(ViewBag.RepName.ToString());

                        ExcelRange r = ws.Cells["A1:Q1"];
                        r.Merge = true;
                        r.Value = ViewBag.RepName.ToString();
                        
                        r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        r.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                        //r.Style.Font.Bold = true;
                        r.Style.Font.Size = 14;
                        StartA = StartA.Insert(6, "/").Insert(4, "/");
                        EndA = EndA.Insert(6, "/").Insert(4, "/");
                        var strCondition = "交易區間:"+StartA+"-"+EndA;
                        if(StartB != "" && StartB != "-")
                        {
                            StartB = StartB.Insert(6, "/").Insert(4, "/");
                            EndB = EndB.Insert(6, "/").Insert(4, "/");
                            strCondition += "、清分區間:" + StartB + "-" + EndB;
                        }
                        //ws.Cells[2, 1].Value = "條件："+strCondition;
                        r = ws.Cells["A2:Q2"];
                        r.Merge = true;
                        r.Value = "條件：" + strCondition; ;
                        r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                        r.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        r.Style.Font.Size = 14;

                        r = ws.Cells["A3:J3"];
                        r.Merge = true;
                        r.Value = "電支";
                        r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        r.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        r.Style.Font.Size = 14;

                        r = ws.Cells["K3:Q3"];
                        r.Merge = true;
                        r.Value = "電票";
                        r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        r.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        r.Style.Font.Size = 14;

                        int startRowNumber = 4;
                        var columns = new[] { "交易類型", "訂單日期", "收款方電支帳號", "付款方電支帳號", "特店訂單編號", "付款方式", "款項來源(銀行)","信託金額", "付款狀態", "人工沖正狀態", "特約機構", "清算日期", "門市名稱", "卡機交易時間", "卡號", "RRN", "交易金額" };

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

                        //資料欄
                        for (int i = 1; i <= dt.Rows.Count; i++)
                        {                            
                            for (int j = 1; j <= columns.Length; j++)
                            {
                                if(j == 8 || j == 17)
                                {
                                    //8:信託金額、17:交易金額
                                    ws.Cells[i + 2, j].Style.Numberformat.Format = "#,##0";
                                    var xValue = dt.Rows[i - 1][j - 1].ToString();
                                    if (string.IsNullOrEmpty(xValue))
                                    {
                                        ws.Cells[i + startRowNumber, j].Value = "";
                                    }
                                    else
                                    {
                                        ws.Cells[i + startRowNumber, j].Value = Convert.ToDouble(xValue);
                                    }
                                    
                                }
                                else
                                {
                                    ws.Cells[i + startRowNumber, j].Value = dt.Rows[i - 1][j - 1].ToString();
                                }
                                
                                ws.Cells[i + startRowNumber, j].Style.Font.Size = 12;
                                ws.Cells[i + startRowNumber, j].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center; //欄位置中
                                ws.Cells[i + startRowNumber, j].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                            }
                        }
                        ws.Cells[ws.Dimension.Address].AutoFitColumns();

                        var stream = new MemoryStream();
                        p.SaveAs(stream);
                        stream.Close();

                        return File(stream.ToArray(), "application/vnd.ms-excel", ViewBag.RepName + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx");
                    }
                }
            }
            else
            {
                return View(dt);
            }
            return View();
        }
            public ActionResult Report0112Result()
        {
            //int year = DateTime.Today.Year;
            //int month = DateTime.Today.Month;
            DateTime startDate = DateTime.Now;
            DateTime endDate = DateTime.Now;
            string start = startDate.ToString("yyyyMMdd");
            string end = endDate.ToString("yyyyMMdd");
            //string merchantNo = "";
            string merchant = "";

            string group = "";
            string item = "";
            //string retail = "";
            //string bus = "";
            //string bike = "";
            //string track = "";
            //string parking = "";
            //string outsourcing = "";

            if (Request.Form["searchConfirm"] != null || Request.Form["ExportExcel"] != null || Request.Form["ExportAll"] != null)
            {
                start = Request.Form["startDate"];
                end = Request.Form["endDate"];
                //merchantNo = Request.Form["Merchant"];
                group = Request.Form["group"];
                item = Request.Form["items"];
                //retail = Request.Form["retail"];
                //bus = Request.Form["bus"];
                //bike = Request.Form["bike"];
                //track = Request.Form["track"];
                //parking = Request.Form["parking"];
                //outsourcing = Request.Form["outsourcing"];

                
                merchant = Request.Form["items"];
            }
            ViewBag.Start = start;
            ViewBag.End = end;
            //ViewBag.Merchant = SetMerchantDropDown(merchantNo);
            ViewBag.RepName = "愛金卡每日特約機構交易表";

            ViewBag.MERCHANT = SetGroup2(group);
            ViewBag.MERCHANT_NAME = group;
            ViewBag.ITEM = item;
            //ViewBag.GROUP = SetGroup(group); ViewBag.groups = group;
            //ViewBag.RETAIL = SetRetailNotXDD(retail); ViewBag.retails = retail;
            //ViewBag.BUS = SetBus(bus); ViewBag.buses = bus;
            //ViewBag.BIKE = SetBike(bike); ViewBag.bikes = bike;
            //ViewBag.TRACK = SetTrack(track); ViewBag.tracks = track;
            //ViewBag.PARKING = SetParking(parking); ViewBag.parkings = parking;
            //ViewBag.OUTSOURCING = SetOutsourcing(outsourcing); ViewBag.outsourcings = outsourcing;

            //DataTable dt = reportManager.Report0112T(start, end, merchantNo);

            DataTable dt = new DataTable();
            if (group.ToUpper() == "ALL") { merchant = "ALL"; }
            dt = reportManager.Report0112T(start, end, group, merchant);

            ViewBag.Count = dt.Rows.Count;

            string merchantNo = merchant;
            //return View(dt);
            if (Request.Form["ExportExcel"] != null || Request.Form["ExportAll"] != null) //產生標籤Excel
            {
                //建立Excel 2003檔案
                IWorkbook wb = new HSSFWorkbook();
                ISheet ws;
                //ISheet ws2;

                ////建立Excel 2007檔案
                //IWorkbook wb = new XSSFWorkbook();
                //ISheet ws;
                ws = wb.CreateSheet("愛金卡每日特約機構交易表");

                //ws.CreateRow(0);//第一行為欄位名稱
                var headerRow = ws.CreateRow(2);

                HSSFCellStyle cs = (HSSFCellStyle)wb.CreateCellStyle();
                //啟動多行文字
                //cs.WrapText = true;
                //文字置中
                cs.VerticalAlignment = VerticalAlignment.Center;
                cs.Alignment = HorizontalAlignment.Center;
                //框線樣式及顏色
                cs.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
                cs.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                cs.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
                cs.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
                cs.BottomBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                cs.LeftBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                cs.RightBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                cs.TopBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                //背景顏色
                cs.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.Grey25Percent.Index;
                cs.FillPattern = NPOI.SS.UserModel.FillPattern.SolidForeground;
                HSSFFont font1 = (HSSFFont)wb.CreateFont();
                HSSFFont font2 = (HSSFFont)wb.CreateFont();
                //字體顏色
                font1.Color = NPOI.HSSF.Util.HSSFColor.Black.Index;
                font2.Color = NPOI.HSSF.Util.HSSFColor.Black.Index;
                //字體粗體
                font2.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.Bold;
                //字體尺寸
                font1.FontHeightInPoints = 12;
                font2.FontHeightInPoints = 12;
                cs.SetFont(font1);

                headerRow.HeightInPoints = 24;

                for (int i = 0; i < dt.Columns.Count-4; i++)
                {
                    var cell = headerRow.CreateCell(i);
                    cell.SetCellValue(dt.Columns[i].ColumnName);
                    cell.CellStyle = cs;
                    //ws.AutoSizeColumn(i);
                    if (i == 0)
                        ws.SetColumnWidth(0, 12 * 256);
                    else if (i == 1)
                        ws.SetColumnWidth(i, 30 * 256);
                    else if (i == 2 & Request.Form["ExportExcel"] != null)
                        ws.SetColumnWidth(i, 0 * 256);
                    else if (i == 2 & Request.Form["ExportAll"] != null)
                        ws.SetColumnWidth(i, 15 * 256);
                    else if (i == 3 & Request.Form["ExportAll"] != null)
                        ws.SetColumnWidth(i, 15 * 256);
                    else if (i == 4 & Request.Form["ExportAll"] != null)
                        ws.SetColumnWidth(i, 15 * 256);
                    else if (i == 3)
                        ws.SetColumnWidth(i, 0 * 256);
                    else if (i == 4)
                        ws.SetColumnWidth(i, 0 * 256);
                    else
                        ws.SetColumnWidth(i, 20 * 256);
                }

                //fill content 
                ICellStyle style3 = wb.CreateCellStyle();
                //框線樣式及顏色
                style3.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
                style3.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                style3.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
                style3.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
                style3.BottomBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                style3.LeftBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                style3.RightBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                style3.TopBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                style3.Alignment = HorizontalAlignment.Center;
                style3.VerticalAlignment = VerticalAlignment.Center;
                //字體尺寸
                font1.FontHeightInPoints = 11;
                style3.SetFont(font1);
                //style3.FillForegroundColor = IndexedColors.LemonChiffon.Index;
                //style3.FillPattern = FillPattern.SolidForeground;
                //style3.FillBackgroundColor = IndexedColors.LightGreen.Index;

                ICellStyle style2 = wb.CreateCellStyle();
                //框線樣式及顏色
                style2.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
                style2.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                style2.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
                style2.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
                style2.BottomBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                style2.LeftBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                style2.RightBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                style2.TopBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                style2.Alignment = HorizontalAlignment.Left;
                style2.VerticalAlignment = VerticalAlignment.Center;
                //字體尺寸
                font1.FontHeightInPoints = 11;
                style2.SetFont(font1);



                // Create the style object
                var currencyCellStyle = wb.CreateCellStyle();
                // Right-align currency values
                currencyCellStyle.Alignment = HorizontalAlignment.Right;
                currencyCellStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
                currencyCellStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                currencyCellStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
                currencyCellStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
                currencyCellStyle.BottomBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                currencyCellStyle.LeftBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                currencyCellStyle.RightBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                currencyCellStyle.TopBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                font1.FontHeightInPoints = 11;
                currencyCellStyle.SetFont(font1);

                // Get / create the data format string
                //format.GetFormat("#,##0.00_);[紅色](#,##0.00)");
                var formatId = HSSFDataFormat.GetBuiltinFormat("#,##0;[Red](#,##0)");
                if (formatId == -1)
                {
                    var newDataFormat = wb.CreateDataFormat();
                    currencyCellStyle.DataFormat = newDataFormat.GetFormat("#,##0;[Red](#,##0)");
                }
                else
                    currencyCellStyle.DataFormat = formatId;

                var currencyCellStyle4 = wb.CreateCellStyle();
                currencyCellStyle4.Alignment = HorizontalAlignment.Right;
                currencyCellStyle4.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
                currencyCellStyle4.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                currencyCellStyle4.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
                currencyCellStyle4.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
                currencyCellStyle4.BottomBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                currencyCellStyle4.LeftBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                currencyCellStyle4.RightBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                currencyCellStyle4.TopBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                //font2.Boldweight = (short)FontBoldWeight.Bold;
                currencyCellStyle4.SetFont(font2);
                if (formatId == -1)
                {
                    var newDataFormat = wb.CreateDataFormat();
                    currencyCellStyle4.DataFormat = newDataFormat.GetFormat("#,##0;[Red](#,##0)");
                }
                else
                    currencyCellStyle4.DataFormat = formatId;


                ICellStyle style4 = wb.CreateCellStyle();
                style4.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
                style4.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                style4.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
                style4.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
                style4.BottomBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                style4.LeftBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                style4.RightBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                style4.TopBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                style4.Alignment = HorizontalAlignment.Center;
                style4.VerticalAlignment = VerticalAlignment.Center;
                style4.SetFont(font2);

                ICellStyle style02 = wb.CreateCellStyle();
                style02.Alignment = HorizontalAlignment.Center;
                style02.VerticalAlignment = VerticalAlignment.Center;
                //字體尺寸
                font1.FontHeightInPoints = 20;
                style02.SetFont(font1);

                ICellStyle style1 = wb.CreateCellStyle();
                //框線樣式及顏色
                style1.Alignment = HorizontalAlignment.Left;
                style1.VerticalAlignment = VerticalAlignment.Center;
                //字體尺寸
                font1.FontHeightInPoints = 11;
                style1.SetFont(font1);

                ws.CreateRow(0).HeightInPoints = 20; ;
                var rang = ws.CreateRow(1);
                var cell3 = rang.CreateCell(0);
                cell3.SetCellValue("清算期間： " + ViewBag.Start + "~" + ViewBag.End);
                cell3.CellStyle = style1;
                cell3 = ws.GetRow(0).CreateCell(0);
                cell3.SetCellValue(ViewBag.RepName);
                ws.AddMergedRegion(new CellRangeAddress(0, 0, 0, 14));
                cell3.CellStyle = style02;

                //決定頁面顯示前幾欄位 0112
                //Double[] total = new Double[16];
                //Double[] totalp = new Double[16];   //小計
                //Double[] totals = new Double[16];   //總計
                //int ii = 2;
                //string temp = "";
                //string temp2 = "";
                if(Request.Form["ExportExcel"] != null)
                {
                    SortedSet<string> dateSet = new SortedSet<string>();// to store unique dates
    
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                               
                        dateSet.Add(dt.Rows[i][0].ToString());
                    }

                    Dictionary<string,long[]> pTotal = new Dictionary<string,long[]>();//partial sum for individual dates 

                    long[] totalSum = new long[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 ,0,0};

                    foreach (string date in dateSet)
                    {
                        pTotal[date] = new long[] {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0};
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            if (date.Equals(dt.Rows[i][0].ToString()))
                            {
                                for (int j = 5; j < dt.Columns.Count - 4; j++)
                                {
                                    pTotal[date][j] += Convert.ToInt64(dt.Rows[i][j]);
                                }
                            }     
                        }
                    }

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        for (int j = 5; j < dt.Columns.Count - 4; j++)
                        {
                            totalSum[j] += Convert.ToInt64(dt.Rows[i][j]);
                        }
                    }


                    int ii=2;
                    foreach (string date in dateSet)
                    {
                        ii++;
                        HSSFRow row = (NPOI.HSSF.UserModel.HSSFRow)ws.CreateRow(ii);
                        HSSFCell cell0 = (NPOI.HSSF.UserModel.HSSFCell)row.CreateCell(0); cell0.SetCellValue(date); cell0.CellStyle=currencyCellStyle;
                        HSSFCell cell1 = (NPOI.HSSF.UserModel.HSSFCell)row.CreateCell(1); cell1.SetCellValue("小計"); cell1.CellStyle = currencyCellStyle;
                        HSSFCell cell2 = (NPOI.HSSF.UserModel.HSSFCell)row.CreateCell(2); cell2.SetCellValue(""); cell2.CellStyle = currencyCellStyle;

                        HSSFCell cell = null;
                        for (int j = 5; j < dt.Columns.Count - 4; j++)
                        {
                            cell = (NPOI.HSSF.UserModel.HSSFCell)row.CreateCell(j); cell.SetCellValue(pTotal[date][j]); cell.CellStyle = currencyCellStyle;
                        }
                    }

                    HSSFRow rowEnd = (NPOI.HSSF.UserModel.HSSFRow)ws.CreateRow(ii+1);
                    HSSFCell cellEnd0 = (NPOI.HSSF.UserModel.HSSFCell)rowEnd.CreateCell(0); cellEnd0.SetCellValue("總計"); cellEnd0.CellStyle = currencyCellStyle;
                    HSSFCell cellEnd1 = (NPOI.HSSF.UserModel.HSSFCell)rowEnd.CreateCell(1); cellEnd1.SetCellValue(""); cellEnd1.CellStyle = currencyCellStyle;
                    HSSFCell cellEnd2 = (NPOI.HSSF.UserModel.HSSFCell)rowEnd.CreateCell(2); cellEnd2.SetCellValue(""); cellEnd2.CellStyle = currencyCellStyle;
                    HSSFCell cellEnd = null;
                    for (int j = 5; j < dt.Columns.Count - 4; j++)
                    {
                        cellEnd = (NPOI.HSSF.UserModel.HSSFCell)rowEnd.CreateCell(j); cellEnd.SetCellValue(totalSum[j]); cellEnd.CellStyle = currencyCellStyle;
                    }

                }
                else if (Request.Form["ExportAll"] != null)
                {
                    SortedSet<string> dateSet = new SortedSet<string>();// to store unique dates

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {

                        dateSet.Add(dt.Rows[i][0].ToString());
                    }

                    Dictionary<string, long[]> pTotal = new Dictionary<string, long[]>();//partial sum for individual dates 

                    long[] totalSum = new long[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,0 };

                    foreach (string date in dateSet)
                    {
                        pTotal[date] = new long[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 ,0 ,0};
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            if (date.Equals(dt.Rows[i][0].ToString()))
                            {
                                for (int j = 5; j < dt.Columns.Count - 4; j++)
                                {
                                    pTotal[date][j] += Convert.ToInt64(dt.Rows[i][j]);
                                }
                            }
                        }
                    }

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        for (int j = 5; j < dt.Columns.Count - 4; j++)
                        {
                            totalSum[j] += Convert.ToInt64(dt.Rows[i][j]);
                        }
                    }

                    int ii = 2;
                    foreach (string date in dateSet)
                    {
                        DataView view = new DataView(dt);
                        view.RowFilter = string.Format("清算日 = '{0}'", date);
                        DataTable dtPart = view.ToTable();

                        
                        for(int i = 0 ; i<dtPart.Rows.Count; i++)
                        {
                            ii++;
                            HSSFRow rowPart = (NPOI.HSSF.UserModel.HSSFRow)ws.CreateRow(ii);
                            HSSFCell cellPart0 = (NPOI.HSSF.UserModel.HSSFCell)rowPart.CreateCell(0); cellPart0.SetCellValue(date); cellPart0.CellStyle = currencyCellStyle;
                            HSSFCell cellPart1 = (NPOI.HSSF.UserModel.HSSFCell)rowPart.CreateCell(1); cellPart1.SetCellValue(dtPart.Rows[i][1].ToString()); cellPart1.CellStyle = currencyCellStyle;
                            HSSFCell cellPart2 = (NPOI.HSSF.UserModel.HSSFCell)rowPart.CreateCell(2); cellPart2.SetCellValue(dtPart.Rows[i][2].ToString()); cellPart2.CellStyle = currencyCellStyle;
                            HSSFCell cellPart3 = (NPOI.HSSF.UserModel.HSSFCell)rowPart.CreateCell(3); cellPart3.SetCellValue(dtPart.Rows[i][3].ToString()); cellPart3.CellStyle = currencyCellStyle;
                            //HSSFCell cellPart2 = (NPOI.HSSF.UserModel.HSSFCell)rowPart.CreateCell(2); cellPart2.SetCellValue(""); cellPart2.CellStyle = currencyCellStyle;

                            HSSFCell cellPart = null;
                            for (int j = 5; j < dt.Columns.Count - 4; j++)
                            {
                                cellPart = (NPOI.HSSF.UserModel.HSSFCell)rowPart.CreateCell(j); cellPart.SetCellValue(Convert.ToInt64(dtPart.Rows[i][j])); cellPart.CellStyle = currencyCellStyle;
                            }
                        }

                        ii++;
                        HSSFRow row = (NPOI.HSSF.UserModel.HSSFRow)ws.CreateRow(ii);
                        HSSFCell cell0 = (NPOI.HSSF.UserModel.HSSFCell)row.CreateCell(0); cell0.SetCellValue(date); cell0.CellStyle = currencyCellStyle4;
                        HSSFCell cell1 = (NPOI.HSSF.UserModel.HSSFCell)row.CreateCell(1); cell1.SetCellValue("小計"); cell1.CellStyle = currencyCellStyle4;
                        HSSFCell cell2 = (NPOI.HSSF.UserModel.HSSFCell)row.CreateCell(2); cell2.SetCellValue(""); cell2.CellStyle = currencyCellStyle4;
                        cell2 = (NPOI.HSSF.UserModel.HSSFCell)row.CreateCell(3); cell2.SetCellValue(""); cell2.CellStyle = currencyCellStyle4;
                        HSSFCell cell = null;
                        for (int j = 5; j < dt.Columns.Count - 4; j++)
                        {
                            cell = (NPOI.HSSF.UserModel.HSSFCell)row.CreateCell(j); cell.SetCellValue(pTotal[date][j]); cell.CellStyle = currencyCellStyle4;
                        }
                    }

                    HSSFRow rowEnd = (NPOI.HSSF.UserModel.HSSFRow)ws.CreateRow(ii + 1);
                    HSSFCell cellEnd0 = (NPOI.HSSF.UserModel.HSSFCell)rowEnd.CreateCell(0); cellEnd0.SetCellValue("總計"); cellEnd0.CellStyle = currencyCellStyle4;
                    HSSFCell cellEnd1 = (NPOI.HSSF.UserModel.HSSFCell)rowEnd.CreateCell(1); cellEnd1.SetCellValue(""); cellEnd1.CellStyle = currencyCellStyle4;
                    HSSFCell cellEnd2 = (NPOI.HSSF.UserModel.HSSFCell)rowEnd.CreateCell(2); cellEnd2.SetCellValue(""); cellEnd2.CellStyle = currencyCellStyle4;
                    cellEnd2 = (NPOI.HSSF.UserModel.HSSFCell)rowEnd.CreateCell(3); cellEnd2.SetCellValue(""); cellEnd2.CellStyle = currencyCellStyle4;
                    HSSFCell cellEnd = null;
                    for (int j = 5; j < dt.Columns.Count - 4; j++)
                    {
                        cellEnd = (NPOI.HSSF.UserModel.HSSFCell)rowEnd.CreateCell(j); cellEnd.SetCellValue(totalSum[j]); cellEnd.CellStyle = currencyCellStyle4;
                    }


                }

                var stream = new MemoryStream();
                wb.Write(stream);
                stream.Close();

                return File(stream.ToArray(), "application/vnd.ms-excel", ViewBag.RepName + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls");


            }
            else
            {
                return View(dt);
            }

        }

        public ActionResult Report0113Result()
        {
            int year = DateTime.Today.Year;
            int month = DateTime.Today.Month;
            DateTime startDate = new DateTime(year, month, 1);
            DateTime endDate = new DateTime(year, month,
                                    DateTime.DaysInMonth(year, month));
            string start = startDate.ToString("yyyyMMdd");
            string end = endDate.ToString("yyyyMMdd");
            string merchantNo = "";
            DataTable dtd = reportManager.ReportMerchant();
            if (dtd.Rows.Count > 0)
            {
                for (int j = 0; j < dtd.Rows.Count; j++)
                {
                    if (dtd.Rows[j][0].ToString()!="22555003")
                    {
                        if (merchantNo=="")
                            merchantNo = "'" + dtd.Rows[j][0].ToString() +"'";
                        else
                            merchantNo = merchantNo + ",'" + dtd.Rows[j][0].ToString() + "'";
                    }
                }
            }

            if (Request.Form["searchConfirm"] != null || Request.Form["ExportExcel"] != null || Request.Form["ExportAll"] != null)
            {
                start = Request.Form["startDate"];
                end = Request.Form["endDate"];
                merchantNo = Request.Form["Merch"];
            }
            ViewBag.Start = start;
            ViewBag.End = end;
            ViewBag.RepName = "特約機構交易表現圖(金額)";
            ViewBag.RepID = "0113";
            //ViewBag.Merchant = SetMerchantDropDown(merchantNo);
            //ViewBag.Merchant = merchantNo;
            ViewBag.Merch = merchantNo;
            DataTable dt = reportManager.Report0113T(start, end, merchantNo);
            ViewBag.Count = dt.Rows.Count;

            return Report113Result("(金額)", dt);
        }

        public ActionResult Report0114Result()
        {
            int year = DateTime.Today.Year;
            int month = DateTime.Today.Month;
            DateTime startDate = new DateTime(year, month, 1);
            DateTime endDate = new DateTime(year, month,
                                    DateTime.DaysInMonth(year, month));
            string start = startDate.ToString("yyyyMMdd");
            string end = endDate.ToString("yyyyMMdd");
            string merchantNo = "";
            DataTable dtd = reportManager.ReportMerchant();
            if (dtd.Rows.Count > 0)
            {
                for (int j = 0; j < dtd.Rows.Count; j++)
                {
                    if (dtd.Rows[j][0].ToString() != "22555003")
                    {
                        if (merchantNo == "")
                            merchantNo = "'" + dtd.Rows[j][0].ToString() + "'";
                        else
                            merchantNo = merchantNo + ",'" + dtd.Rows[j][0].ToString() + "'";
                    }
                }
            }

            if (Request.Form["searchConfirm"] != null || Request.Form["ExportExcel"] != null || Request.Form["ExportAll"] != null)
            {
                start = Request.Form["startDate"];
                end = Request.Form["endDate"];
                merchantNo = Request.Form["Merch"];
            }
            ViewBag.Start = start;
            ViewBag.End = end;
            ViewBag.RepName = "特約機構交易表現圖(數量)";
            ViewBag.RepID = "0114";
            //ViewBag.Merchant = SetMerchantDropDown(merchantNo);
            //ViewBag.Merchant = merchantNo;
            ViewBag.Merch = merchantNo;
            DataTable dt = reportManager.Report0114T(start, end, merchantNo);
            ViewBag.Count = dt.Rows.Count;

            return Report113Result("(數量)", dt);
        }

        public ActionResult Report113Result(string kind, DataTable dt)
        {
            DataTable dtd = reportManager.ReportMerchant();
            ViewBag.Countd = dtd.Rows.Count;

            ViewBag.SQLs = "";
            ViewBag.ViewBv = new String[dtd.Rows.Count];
            ViewBag.ViewBt = new String[dtd.Rows.Count];
            //String[] ViewBt = new String[16];
            string colValue;
            string colValue1;
            if (ViewBag.Countd > 0)
            {
                for (int j = 0; j < dtd.Rows.Count; j++)
                {
                    colValue = dtd.Rows[j][0].ToString();
                    colValue1 = dtd.Rows[j][1].ToString();
                    ViewBag.ViewBv[j] = colValue;
                    ViewBag.ViewBt[j] = colValue1;
                }
            }

            string ReportImg = System.Web.Configuration.WebConfigurationManager.AppSettings["ReportImg"];
            if (System.IO.File.Exists(ReportImg))
            System.IO.File.Delete(ReportImg);

            string[] xValues = new string[dt.Rows.Count];

            Double[] yValues = new Double[dt.Rows.Count];

            if (dt.Rows.Count == 0) return View(dt);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                xValues[i] = dt.Rows[i][0].ToString();
                yValues[i] = Convert.ToDouble(dt.Rows[i][3].ToString());
            }
            //ChartAreas,Series,Legends 基本設定------------------------------------------------- 
            Chart Chart1 = new Chart();
            Chart1.ChartAreas.Add("ChartArea1"); //圖表區域集合 
            Chart1.Legends.Add("Legends1"); //圖例集合說明 
            Chart1.Series.Add("Series1"); //數據序列集合 
            //設定 Chart------------------------------------------------------------------------- 
            Chart1.Width = 770;
            Chart1.Height = 550;
            Title title = new Title();
            title.Text = "特約機構交易表現圖"+ kind;
            title.Alignment = ContentAlignment.MiddleCenter;
            title.Font = new System.Drawing.Font("Trebuchet MS", 14F, FontStyle.Bold);
            Chart1.Titles.Add(title);
            //設定 ChartArea1-------------------------------------------------------------------- 
            Chart1.ChartAreas["ChartArea1"].Area3DStyle.Enable3D = true; //is3D;
            Chart1.ChartAreas[0].AxisX.Interval = 1;
            //設定 Legends-------------------------------------------------------------------------                 
            //Chart1.Legends["Legends1"].DockedToChartArea = "ChartArea1"; //顯示在圖表內 
            Chart1.Legends["Legends1"].Docking = Docking.Bottom; //自訂顯示位置 
            //背景色 
            Chart1.Legends["Legends1"].BackColor = Color.FromArgb(235, 235, 235);
            //斜線背景 
            Chart1.Legends["Legends1"].BackHatchStyle = ChartHatchStyle.DarkDownwardDiagonal;
            Chart1.Legends["Legends1"].BorderWidth = 1;
            //Chart1.Legends["Legends1"].BorderColor = Color.FromArgb(0, 250, 250);  //圖說的框
            //設定 Series1----------------------------------------------------------------------- 
            Chart1.Series["Series1"].ChartType = SeriesChartType.Pie;
            //Chart1.Series["Series1"].ChartType = SeriesChartType.Doughnut; 
            Chart1.Series["Series1"].Points.DataBindXY(xValues, yValues);
            Chart1.Series["Series1"].LegendText = "#VALX:    [ #PERCENT{P1} ]"; //X軸 + 百分比 
            Chart1.Series["Series1"].Label = "#VALX\n#PERCENT{P1}"; //X軸 + 百分比 
            Chart1.Series["Series1"].LabelForeColor = Color.FromArgb(0, 90, 255); //字體顏色 
            //字體設定 
            Chart1.Series["Series1"].Font = new System.Drawing.Font("Trebuchet MS", 10, System.Drawing.FontStyle.Bold);
            Chart1.Series["Series1"].Points.FindMaxByValue().LabelForeColor = Color.Red;
            //Chart1.Series["Series1"].Points.FindMaxByValue().Color = Color.Red; 
            //Chart1.Series["Series1"].Points.FindMaxByValue()["Exploded"] = "true"; 
            Chart1.Series["Series1"].BorderColor = Color.FromArgb(255, 101, 101, 101);
            //Chart1.Series["Series1"]["DoughnutRadius"] = "80"; // ChartType為Doughnut時，Set Doughnut hole size 
            //Chart1.Series["Series1"]["PieLabelStyle"] = "Inside"; //數值顯示在圓餅內 
            Chart1.Series["Series1"]["PieLabelStyle"] = "Outside"; //數值顯示在圓餅外 
            //Chart1.Series["Series1"]["PieLabelStyle"] = "Disabled"; //不顯示數值 
            //設定圓餅效果，除 Default 外其他效果3D不適用 
            Chart1.Series["Series1"]["PieDrawingStyle"] = "Default";
            //Chart1.Series["Series1"]["PieDrawingStyle"] = "SoftEdge"; 
            //Chart1.Series["Series1"]["PieDrawingStyle"] = "Concave"; 
            //Random rnd = new Random();  //亂數產生區塊顏色 
            //foreach (DataPoint point in Chart1.Series["Series1"].Points) 
            //{ 
            //    //pie 顏色 
            //    point.Color = Color.FromArgb(150, rnd.Next(0, 255), rnd.Next(0, 255), rnd.Next(0, 255));  
            //} 
            
            Chart1.SaveImage(ReportImg);
            //Page.Controls.Add(Chart1);
            //ViewBag.Chart1 = "2";
            return View(dt);
        }

        public ActionResult Report0115Result()
        {
            int year = DateTime.Today.Year;
            int month = DateTime.Today.Month;
            DateTime startDate = new DateTime(year, month, 1);
            DateTime endDate = new DateTime(year, month,
                                    DateTime.DaysInMonth(year, month));
            string start = startDate.ToString("yyyyMMdd");
            string end = endDate.ToString("yyyyMMdd");
            string merchantNo = "";
            if (Request.Form["searchConfirm"] != null || Request.Form["ExportExcel"] != null || Request.Form["ExportAll"] != null)
            {
                start = Request.Form["startDate"];
                end = Request.Form["endDate"];
                merchantNo = Request.Form["Merchant"];
            }
            ViewBag.Start = start;
            ViewBag.End = end;
            ViewBag.Merchant = SetMerchantDropDown(merchantNo);
            DataTable dt = reportManager.Report0115T(start, end, merchantNo);
            ViewBag.Count = dt.Rows.Count;

            return Report115Result("(金額)", dt);
        }

        public ActionResult Report0116Result()
        {
            int year = DateTime.Today.Year;
            int month = DateTime.Today.Month;
            DateTime startDate = new DateTime(year, month, 1);
            DateTime endDate = new DateTime(year, month,
                                    DateTime.DaysInMonth(year, month));
            string start = startDate.ToString("yyyyMMdd");
            string end = endDate.ToString("yyyyMMdd");
            string merchantNo = "";
            if (Request.Form["searchConfirm"] != null || Request.Form["ExportExcel"] != null || Request.Form["ExportAll"] != null)
            {
                start = Request.Form["startDate"];
                end = Request.Form["endDate"];
                merchantNo = Request.Form["Merchant"];
            }
            ViewBag.Start = start;
            ViewBag.End = end;
            ViewBag.Merchant = SetMerchantDropDown(merchantNo);
            DataTable dt = reportManager.Report0116T(start, end, merchantNo);
            ViewBag.Count = dt.Rows.Count;

            return Report115Result("(數量)", dt);
        }

        public ActionResult Report115Result(string kind, DataTable dt)
        {

            string ReportImg = System.Web.Configuration.WebConfigurationManager.AppSettings["ReportImg"];
            System.IO.File.Delete(ReportImg);

            if (dt.Rows.Count == 0) return View(dt);

            string[] xValues = new string[dt.Rows.Count];//{ "1日", "2日", "3日" };
            string[] titleArr = { "購貨", "加值" };
            Double[] yValues = new Double[dt.Rows.Count];
            Double[] yValues2 = new Double[dt.Rows.Count];

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                xValues[i] = dt.Rows[i][0].ToString().Substring(4,4);
                yValues[i] = Convert.ToDouble(dt.Rows[i][1].ToString());
                yValues2[i] = Convert.ToDouble(dt.Rows[i][2].ToString());
            }

            //ChartAreas,Series,Legends 基本設定-------------------------------------------------- 
            Chart Chart1 = new Chart();
            Chart1.ChartAreas.Add("ChartArea1"); //圖表區域集合 
            Chart1.Series.Add("Series1"); //數據序列集合 
            Chart1.Series.Add("Series2");
            Chart1.Legends.Add("Legends1"); //圖例集合 

            //設定 Chart 
            Chart1.Width = 900;
            Chart1.Height = 400;
            Title title = new Title();
            title.Text = "每日交易趨勢圖"+ kind;
            title.Alignment = ContentAlignment.MiddleCenter;
            title.Font = new System.Drawing.Font("Trebuchet MS", 14F, FontStyle.Bold);
            Chart1.Titles.Add(title);

            //設定 ChartArea---------------------------------------------------------------------- 
            Chart1.ChartAreas["ChartArea1"].Area3DStyle.Enable3D = false; //3D效果 
            Chart1.ChartAreas["ChartArea1"].Area3DStyle.IsClustered = false; //並排顯示 
            Chart1.ChartAreas["ChartArea1"].Area3DStyle.Rotation = 40; //垂直角度 
            Chart1.ChartAreas["ChartArea1"].Area3DStyle.Inclination = 50; //水平角度 
            Chart1.ChartAreas["ChartArea1"].Area3DStyle.PointDepth = 30; //數據條深度 
            Chart1.ChartAreas["ChartArea1"].Area3DStyle.WallWidth = 0; //外牆寬度 
            Chart1.ChartAreas["ChartArea1"].Area3DStyle.LightStyle = LightStyle.Realistic; //光源 
            Chart1.ChartAreas["ChartArea1"].BackColor = Color.FromArgb(240, 240, 240); //背景色 
            Chart1.ChartAreas["ChartArea1"].AxisX2.Enabled = AxisEnabled.False; //隱藏 X2 標示 
            Chart1.ChartAreas["ChartArea1"].AxisY2.Enabled = AxisEnabled.False; //隱藏 Y2 標示 
            Chart1.ChartAreas["ChartArea1"].AxisY2.MajorGrid.Enabled = false;   //隱藏 Y2 軸線 

            //Y 軸線顏色 
            Chart1.ChartAreas["ChartArea1"].AxisY.MajorGrid.LineColor = Color.FromArgb(150, 150, 150);

            //X 軸線顏色 
            Chart1.ChartAreas["ChartArea1"].AxisX.MajorGrid.LineColor = Color.FromArgb(150, 150, 150);
            Chart1.ChartAreas["ChartArea1"].AxisY.LabelStyle.Format = "#,###";
            //Chart1.ChartAreas["ChartArea1"].AxisY2.Maximum = 160; 
            //Chart1.ChartAreas["ChartArea1"].AxisY2.Interval = 20; 

            //設定 Legends------------------------------------------------------------------------                 
            //Chart1.Legends["Legends1"].DockedToChartArea = "ChartArea1"; //顯示在圖表內 活動1
            Chart1.Legends["Legends1"].Docking = Docking.Bottom; //自訂顯示位置 
            //Chart1.Legends["Legends1"].BackColor = Color.FromArgb(235, 235, 235); //背景色 

            //斜線背景 


            //Chart1.Legends["Legends1"].BackHatchStyle = ChartHatchStyle.DarkDownwardDiagonal;
            //Chart1.Legends["Legends1"].BorderWidth = 1;
            //Chart1.Legends["Legends1"].BorderColor = Color.FromArgb(200, 200, 200);

            //設定 Series----------------------------------------------------------------------- 
            Chart1.Series["Series1"].ChartType = SeriesChartType.Column; //直條圖 
            //Chart1.Series["Series1"].ChartType = SeriesChartType.Bar; //橫條圖 
            Chart1.Series["Series1"].Points.DataBindXY(xValues, yValues);
            Chart1.Series["Series1"].Legend = "Legends1";
            Chart1.Series["Series1"].LegendText = titleArr[0];
            Chart1.Series["Series1"].LabelFormat = "#,###"; //金錢格式 
            Chart1.Series["Series1"].MarkerSize = 8; //Label 範圍大小 
            Chart1.Series["Series1"].LabelForeColor = Color.FromArgb(0, 90, 255); //字體顏色 

            //字體設定 
            Chart1.Series["Series1"].Font = new System.Drawing.Font("Trebuchet MS", 10, System.Drawing.FontStyle.Bold);

            //Label 背景色 
            Chart1.Series["Series1"].LabelBackColor = Color.FromArgb(150, 255, 255, 255);
            Chart1.Series["Series1"].Color = Color.FromArgb(240, 65, 140, 240); //背景色 
            Chart1.Series["Series1"].IsValueShownAsLabel = false; // Show data points labels 
            Chart1.Series["Series2"].Points.DataBindXY(xValues, yValues2);
            Chart1.Series["Series2"].Legend = "Legends1";
            Chart1.Series["Series2"].LegendText = titleArr[1];
            Chart1.Series["Series2"].LabelFormat = "#,###"; //金錢格式 
            Chart1.Series["Series2"].MarkerSize = 8; //Label 範圍大小 
            Chart1.Series["Series2"].LabelForeColor = Color.FromArgb(255, 103, 0);
            Chart1.Series["Series2"].Font = new System.Drawing.Font("Trebuchet MS", 10, FontStyle.Bold);
            Chart1.Series["Series2"].LabelBackColor = Color.FromArgb(150, 255, 255, 255);
            Chart1.Series["Series2"].Color = Color.FromArgb(240, 252, 180, 65); //背景色 
            Chart1.Series["Series2"].IsValueShownAsLabel = false; //顯示數據 

            Chart1.SaveImage(ReportImg);
            return View(dt);
        }

        public ActionResult Report0117Result()
        {
            //int year = DateTime.Today.Year;
            //int month = DateTime.Today.Month;
            //DateTime startDate = new DateTime(year, month, 1);
            //DateTime endDate = new DateTime(year, month,
            //                        DateTime.DaysInMonth(year, month));
            //string start = startDate.ToString("yyyyMM");
            //string end = endDate.ToString("yyyyMM");
            string yearMonth = DateTime.Now.ToString("yyyyMM"); 
            //string merchantNo = "";
            string merchant = "";

            string group = "";
            string item = "";
            //string retail = "";
            //string bus = "";
            //string bike = "";
            //string track = "";
            //string parking = "";
            //string outsourcing = "";

            if (Request.Form["searchConfirm"] != null || Request.Form["ExportExcel"] != null)
            {
                yearMonth = Request.Form["yearMonth"];
                //end = Request.Form["endDate"] ;
                //end = start;
                // merchantNo = Request.Form["Merchant"];
                group = Request.Form["group"];
                item = Request.Form["items"];
                //retail = Request.Form["retail"];
                //bus = Request.Form["bus"];
                //bike = Request.Form["bike"];
                //track = Request.Form["track"];
                //parking = Request.Form["parking"];
                //outsourcing = Request.Form["outsourcing"];

                merchant = Request.Form["items"];
            }
            //ViewBag.Start = start;
            //ViewBag.End = end;
            ViewBag.YearMonth = yearMonth;
            //ViewBag.Merchant = SetMerchantDropDown(merchantNo);
            ViewBag.RepID = "0117";
            ViewBag.RepName = "愛金卡月別特約機構每日交易一覽表";

            ViewBag.MERCHANT = SetGroup(group);
            ViewBag.MERCHANT_NAME = group;
            ViewBag.ITEM = item;
            //ViewBag.RETAIL = SetRetail(retail); ViewBag.retails = retail;
            //ViewBag.BUS = SetBus(bus); ViewBag.buses = bus;
            //ViewBag.BIKE = SetBike(bike); ViewBag.bikes = bike;
            //ViewBag.TRACK = SetTrack(track); ViewBag.tracks = track;
            //ViewBag.PARKING = SetParking(parking); ViewBag.parkings = parking;
            //ViewBag.OUTSOURCING = SetOutsourcing(outsourcing); ViewBag.outsourcings = outsourcing;


            DataTable dt = new DataTable();
            List<string> mNoList = new List<string>();
            if (group == "ALL")
            {
                mNoList = new GmMerchantTypeDAO().FindAll();

                foreach (string mNo in mNoList)
                { dt.Merge(reportManager.Report0117T(yearMonth, mNo)); }
            }
            else if (item == "ALL")
            //else if (retail == "ALL" || bus == "ALL" || bike == "ALL" || track == "ALL" || parking == "ALL" || outsourcing == "ALL")
            {
                mNoList = new GmMerchantTypeDAO().FindGroupAll(group);
                foreach (string mNo in mNoList)
                { dt.Merge(reportManager.Report0117T(yearMonth, mNo)); }
            }
            else if (merchant !="")
            {
                dt = reportManager.Report0117T(yearMonth, merchant);
            }
            else { dt = new DataTable(); }
            /*
            DataTable dt = null;
            if ((Request.Form["searchConfirm"] != null || Request.Form["ExportExcel"] != null) )
            {
                dt = reportManager.Report0117T(yearMonth, merchantNo);
            }
            else
            {
                dt = new DataTable();
            }
            */
            return Report117Result(dt);
            
        }

        public ActionResult Report0122Result()
        {
            DateTime tempDate;

            int year = DateTime.Today.Year;
            int month = DateTime.Today.Month;
            DateTime startDate = new DateTime(year, month, 1);
            DateTime endDate = new DateTime(year, month,
                                    DateTime.DaysInMonth(year, month));
            string start = startDate.ToString("yyyyMMdd");
            string end = endDate.ToString("yyyyMMdd");
            string merchantNo = "22555003";

            string SRC_FLG = "POS";

            ViewBag.Start = startDate.ToString("yyyyMM");

            if (Request.Form["searchConfirm"] != null || Request.Form["ExportExcel"] != null)
            {
                start = Request.Form["startDate"]+"01";
                tempDate = DateTime.Parse(start.Substring(0, 4) + "-" + start.Substring(4, 2) + "-" + start.Substring(6, 2)).AddDays(-1);
                start = tempDate.ToString("yyyyMMdd");

                int days = DateTime.DaysInMonth(int.Parse(Request.Form["startDate"].Substring(0, 4)), int.Parse(Request.Form["startDate"].Substring(4, 2))) -1;
                end = Request.Form["startDate"] + days.ToString();
                
                //merchantNo = Request.Form["Merchant"];
                SRC_FLG = Request.Form["SRC_FLG"];
                ViewBag.Start = Request.Form["startDate"];
                ViewBag.End = Request.Form["startDate"];
            }
            if (SRC_FLG == "POS")
            {
                ViewBag.ch1 = "checked";
                ViewBag.ch2 = "";
                ViewBag.RepName = "愛金卡月別超商每日交易一覽表(POS)";
            }
            else
            {
                ViewBag.ch1 = "";
                ViewBag.ch2 = "checked";
                ViewBag.RepName = "愛金卡月別超商每日交易一覽表(TXLOG)";
            }
            //ViewBag.Merchant = SetMerchantDropDown(merchantNo);
            ViewBag.RepID = "0122";
            DataTable dt = reportManager.Report0122T(start, end, merchantNo, SRC_FLG);
            return Report117Result(dt);
        }


        public ActionResult Report0118Result()
        {
            int year = DateTime.Today.Year;
            int month = DateTime.Today.Month;
            //DateTime startDate = new DateTime(year, month, 1);
            //DateTime endDate = new DateTime(year, month,
            //                        DateTime.DaysInMonth(year, month));
            //string start = startDate.ToString("yyyyMM");
            //string end = endDate.ToString("yyyyMM");
            //string merchantNo = "";
            string yearMonth = year.ToString()+month.ToString().PadLeft(2,'0');
            string merchant = "";
            string item = "";

            string group = "";
            string retail = "";
            string bus = "";
            string bike = "";
            string track = "";
            string parking = "";
            string outsourcing = "";

            if (Request.Form["searchConfirm"] != null || Request.Form["ExportExcel"] != null)
            {
                //start = Request.Form["startDate"];
                //end = Request.Form["endDate"] ;
                //end = start;
                //merchantNo = Request.Form["Merchant"];
                yearMonth = Request.Form["yearMonth"];

                group = Request.Form["group"];
                item = Request.Form["items"];
                //retail = Request.Form["retail"];
                //bus = Request.Form["bus"];
                //bike = Request.Form["bike"];
                //track = Request.Form["track"];
                //parking = Request.Form["parking"];
                //outsourcing = Request.Form["outsourcing"];

                //if (group == "PARKING_LOT")
                //{ merchant = Request.Form["parking"]; }
                //else if (group == "BANK_OUTSOURCING")
                //{ merchant = Request.Form["outsourcing"]; }
                //else
                //{ merchant = Request.Form[group.ToLower()]; }
                merchant = Request.Form["items"];
            }
            //ViewBag.Start = start;
            //ViewBag.End = end;
            ViewBag.YearMonth = yearMonth;
            //ViewBag.Merchant = SetMerchantDropDown(merchantNo);
            ViewBag.RepID = "0118";
            ViewBag.RepName = "愛金卡月別特約機構每日交易數量表";

            ViewBag.MERCHANT = SetGroup(group);
            ViewBag.MERCHANT_NAME = group;
            ViewBag.ITEM = item;
            //ViewBag.GROUP = SetGroup(group); ViewBag.groups = group;
            //ViewBag.RETAIL = SetRetail(retail); ViewBag.retails = retail;
            //ViewBag.BUS = SetBus(bus); ViewBag.buses = bus;
            //ViewBag.BIKE = SetBike(bike); ViewBag.bikes = bike;
            //ViewBag.TRACK = SetTrack(track); ViewBag.tracks = track;
            //ViewBag.PARKING = SetParking(parking); ViewBag.parkings = parking;
            //ViewBag.OUTSOURCING = SetOutsourcing(outsourcing); ViewBag.outsourcings = outsourcing;
            //DataTable dt = reportManager.Report0118T(start, end, merchantNo);

            DataTable dt = new DataTable();
            List<string> mNoList = new List<string>();
           
            if (group == "ALL")
            {
                mNoList = new GmMerchantTypeDAO().FindAll();

                foreach (string mNo in mNoList)
                { dt.Merge(reportManager.Report0118T(yearMonth, mNo)); }
            }
            else if (merchant != "" && merchant != "ALL")
            {
                dt = reportManager.Report0118T(yearMonth, merchant);
            }
            else if (item == "ALL") 
            //else if (retail == "ALL" || bus == "ALL" || bike == "ALL" || track == "ALL" || parking == "ALL" || outsourcing == "ALL")
            {
                mNoList = new GmMerchantTypeDAO().FindGroupAll(group);
                foreach (string mNo in mNoList)
                { dt.Merge(reportManager.Report0118T(yearMonth, mNo)); }
            }
            else { dt = new DataTable(); }

            return Report117Result(dt);
        }

        public ActionResult Report0119Result()
        {
            int year = DateTime.Today.Year;
            int month = DateTime.Today.Month;
            DateTime startDate = new DateTime(year, month, 1);
            DateTime endDate = new DateTime(year, month,
                                    DateTime.DaysInMonth(year, month));
            string start = startDate.ToString("yyyyMMdd");
            string end = endDate.ToString("yyyyMMdd");
            string StoreNo = "";
            if (Request.Form["searchConfirm"] != null || Request.Form["ExportExcel"] != null)
            {
                start = Request.Form["startDate"];
                end = Request.Form["endDate"] ;
                StoreNo = Request.Form["StoreNo"];
            }
            ViewBag.Start = start;
            ViewBag.End = end;
            ViewBag.StoreNo = SetStoreDropDown(StoreNo);
            ViewBag.RepID = "0119";
            ViewBag.RepName = "微程式交易明細表";
            DataTable dt = reportManager.Report0119T(start, end, StoreNo);
            return Report117Result(dt);
        }

        public ActionResult Report0123Result()
        {
            int year = DateTime.Today.Year;
            int month = DateTime.Today.Month;
            DateTime startDate = new DateTime(year, month, 1);
            DateTime endDate = new DateTime(year, month,
                                    DateTime.DaysInMonth(year, month));
            string start = startDate.ToString("yyyyMMdd");
            string end = endDate.ToString("yyyyMMdd");
            //string merchantNo = "";
            ViewBag.ViewBv = new String[6];
            ViewBag.Kinds = "1";
            ViewBag.Kind = " ";

            string merchant = "";

            string group = "";
            string retail = "";
            string bus = "";
            string bike = "";
            string track = "";
            string parking = "";
            string outsourcing = "";

            if (Request.Form["searchConfirm"] != null || Request.Form["ExportExcel"] != null)
            {
                start = Request.Form["startDate"];
                end = Request.Form["endDate"] ;
                //end = start;
                //merchantNo = Request.Form["Merchant"];
                ViewBag.Kind = Request.Form["Kind"];
                ViewBag.Kinds = Request.Form["Kinds"];

                if (ViewBag.Kinds == "1")
                {
                    //Response.Write("<script>alert('');</script>"); return View();
                    DisplayMessage("請選擇卡別");
                    //DisplayMessage(Request.Form["Kinds"]);

                }
                if (Request.Form["Kind"] == "")
                {
                    ViewBag.ViewBv[0] = "selected='selected'";
                    ViewBag.ViewBv[1] = "";
                    ViewBag.ViewBv[2] = "";
                    ViewBag.ViewBv[3] = "";
                }
                if (Request.Form["Kind"] == "一代卡")
                {
                    ViewBag.ViewBv[0] = "";
                    ViewBag.ViewBv[1] = "selected='selected'";
                    ViewBag.ViewBv[2] = "";
                    ViewBag.ViewBv[3] = "";
                }
                if (Request.Form["Kind"] == "二代卡")
                {
                    ViewBag.ViewBv[0] = "";
                    ViewBag.ViewBv[1] = "";
                    ViewBag.ViewBv[2] = "selected='selected'";
                    ViewBag.ViewBv[3] = "";
                }
                if (Request.Form["Kind"] == "聯名卡")
                {
                    ViewBag.ViewBv[0] = "";
                    ViewBag.ViewBv[1] = "";
                    ViewBag.ViewBv[2] = "";
                    ViewBag.ViewBv[3] = "selected='selected'";
                }
                if (Request.Form["Kind"] == "一代卡,二代卡")
                {
                    ViewBag.ViewBv[0] = "";
                    ViewBag.ViewBv[1] = "selected='selected'";
                    ViewBag.ViewBv[2] = "selected='selected'";
                    ViewBag.ViewBv[3] = "";
                }
                if (Request.Form["Kind"] == "一代卡,聯名卡")
                {
                    ViewBag.ViewBv[0] = "";
                    ViewBag.ViewBv[1] = "selected='selected'";
                    ViewBag.ViewBv[2] = "";
                    ViewBag.ViewBv[3] = "selected='selected'";
                }
                if (Request.Form["Kind"] == "二代卡,聯名卡")
                {
                    ViewBag.ViewBv[0] = "";
                    ViewBag.ViewBv[1] = "";
                    ViewBag.ViewBv[2] = "selected='selected'";
                    ViewBag.ViewBv[3] = "selected='selected'";
                }
                if (Request.Form["Kind"] == "一代卡,二代卡,聯名卡")
                {
                    ViewBag.ViewBv[0] = "";
                    ViewBag.ViewBv[1] = "selected='selected'";
                    ViewBag.ViewBv[2] = "selected='selected'";
                    ViewBag.ViewBv[3] = "selected='selected'";
                }
                if (ViewBag.Kind != "" && ViewBag.Kind != null)
                {
                    if (ViewBag.Kind.Substring(0, 1) == ",")
                    {
                        ViewBag.ViewBv[0] = "selected='selected'";
                        ViewBag.ViewBv[1] = "";
                        ViewBag.ViewBv[2] = "";
                        ViewBag.ViewBv[3] = "";
                        ViewBag.Kinds = "''";
                    }
                }
                if (ViewBag.Kinds == "'")
                {
                    DisplayMessage("請選擇卡別");

                }

                group = Request.Form["group"];
                retail = Request.Form["retail"];
                bus = Request.Form["bus"];
                bike = Request.Form["bike"];
                track = Request.Form["track"];
                parking = Request.Form["parking"];
                outsourcing = Request.Form["outsourcing"];

                if (group == "PARKING_LOT")
                { merchant = Request.Form["parking"]; }
                else if (group == "BANK_OUTSOURCING")
                { merchant = Request.Form["outsourcing"]; }
                else
                { merchant = Request.Form[group.ToLower()]; }
            }

            ViewBag.Start = start;
            ViewBag.End = end;
            //ViewBag.Merchant = SetMerchantDropDown(merchantNo);
            ViewBag.RepID = "0123";
            ViewBag.RepName = "愛金卡月別特約機構每日消費明細表";

            ViewBag.GROUP = SetGroup(group); ViewBag.groups = group;
            ViewBag.RETAIL = SetRetail(retail); ViewBag.retails = retail;
            ViewBag.BUS = SetBus(bus); ViewBag.buses = bus;
            ViewBag.BIKE = SetBike(bike); ViewBag.bikes = bike;
            ViewBag.TRACK = SetTrack(track); ViewBag.tracks = track;
            ViewBag.PARKING = SetParking(parking); ViewBag.parkings = parking;
            ViewBag.OUTSOURCING = SetOutsourcing(outsourcing); ViewBag.outsourcings = outsourcing;


            string end1 = start;
            DateTime tempDate = DateTime.Parse(start.Substring(0, 4) + "-" + start.Substring(4, 2) + "-" + start.Substring(6, 2)).AddDays(92);
            end1 = tempDate.ToString("yyyyMMdd");

            //DataTable dt=reportManager.Report0123T(start, end, merchantNo, "1");
            DataTable dt = new DataTable();
            if (int.Parse(end1) < int.Parse(end))
            {
                //Response.Write("<script>alert('');</script>"); return View();
                DisplayMessage("交易期間不可大於3個月");
                //DisplayMessage(Request.Form["Kind"]);
                
            }
            else
            {
                //dt = reportManager.Report0123T(start, end, merchantNo, ViewBag.Kinds);
                
                List<string> mNoList = new List<string>();
                if (group == "ALL")
                {
                    mNoList = new GmMerchantTypeDAO().FindAll();

                    foreach (string mNo in mNoList)
                    { dt.Merge(reportManager.Report0123T(start, end, mNo, ViewBag.Kinds)); }
                }
                else if (retail == "ALL" || bus == "ALL" || bike == "ALL" || track == "ALL" || parking == "ALL" || outsourcing == "ALL")
                {
                    mNoList = new GmMerchantTypeDAO().FindGroupAll(group);
                    foreach (string mNo in mNoList)
                    { dt.Merge(reportManager.Report0123T(start, end, mNo, ViewBag.Kinds)); }
                }
                else if (merchant != "")
                {
                    dt = reportManager.Report0123T(start, end, merchant, ViewBag.Kinds);
                }
                else { dt = new DataTable(); }
            }
            return Report117Result(dt);
        }

        public ActionResult Report117Result(DataTable dt)
        {
            ViewBag.Count = dt.Rows.Count;
            //return View(dt);

            if (Request.Form["ExportExcel"] != null) //產生標籤Excel
            {
                //建立Excel 2003檔案
                IWorkbook wb = new HSSFWorkbook();
                ISheet ws;
                //ISheet ws2;

                ////建立Excel 2007檔案
                //IWorkbook wb = new XSSFWorkbook();
                //ISheet ws;
                ws = wb.CreateSheet(ViewBag.RepName);

                //ws.CreateRow(0);//第一行為欄位名稱
                var headerRow = ws.CreateRow(2);

                HSSFCellStyle cs = (HSSFCellStyle)wb.CreateCellStyle();
                //啟動多行文字
                //cs.WrapText = true;
                //文字置中
                cs.VerticalAlignment = VerticalAlignment.Center;
                cs.Alignment = HorizontalAlignment.Center;
                //框線樣式及顏色
                cs.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
                cs.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                cs.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
                cs.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
                cs.BottomBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                cs.LeftBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                cs.RightBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                cs.TopBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                //背景顏色
                cs.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.Grey25Percent.Index;
                cs.FillPattern = NPOI.SS.UserModel.FillPattern.SolidForeground;
                HSSFFont font1 = (HSSFFont)wb.CreateFont();
                HSSFFont font2 = (HSSFFont)wb.CreateFont();
                //字體顏色
                font1.Color = NPOI.HSSF.Util.HSSFColor.Black.Index;
                font2.Color = NPOI.HSSF.Util.HSSFColor.Black.Index;
                //字體粗體
                font2.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.Bold;
                //字體尺寸
                font1.FontHeightInPoints = 12;
                cs.SetFont(font1);

                headerRow.HeightInPoints = 24;

                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    var cell = headerRow.CreateCell(i);
                    cell.SetCellValue(dt.Columns[i].ColumnName);
                    cell.CellStyle = cs;
                    //ws.AutoSizeColumn(i);
                    if (i == 0 && (ViewBag.RepID != "0119"))
                        ws.SetColumnWidth(0, 0 * 256);
                    else if (i == 1)
                        ws.SetColumnWidth(1, 12 * 256);
                    else if (i == 2)
                        ws.SetColumnWidth(i, 30 * 256);
                    else
                        ws.SetColumnWidth(i, 20 * 256);
                }

                //fill content 
                ICellStyle style3 = wb.CreateCellStyle();
                //框線樣式及顏色
                style3.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
                style3.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                style3.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
                style3.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
                style3.BottomBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                style3.LeftBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                style3.RightBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                style3.TopBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                style3.Alignment = HorizontalAlignment.Center;
                style3.VerticalAlignment = VerticalAlignment.Center;
                //字體尺寸
                font1.FontHeightInPoints = 11;
                style3.SetFont(font1);
                //style3.FillForegroundColor = IndexedColors.LemonChiffon.Index;
                //style3.FillPattern = FillPattern.SolidForeground;
                //style3.FillBackgroundColor = IndexedColors.LightGreen.Index;

                ICellStyle style2 = wb.CreateCellStyle();
                style2.Alignment = HorizontalAlignment.Center;
                style2.VerticalAlignment = VerticalAlignment.Center;
                //字體尺寸
                font1.FontHeightInPoints = 20;
                style2.SetFont(font1);

                ICellStyle style1 = wb.CreateCellStyle();
                //框線樣式及顏色
                style1.Alignment = HorizontalAlignment.Left;
                style1.VerticalAlignment = VerticalAlignment.Center;
                //字體尺寸
                font1.FontHeightInPoints = 11;
                style1.SetFont(font1);

                // Create the style object
                var currencyCellStyle = wb.CreateCellStyle();
                // Right-align currency values
                currencyCellStyle.Alignment = HorizontalAlignment.Right;
                currencyCellStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
                currencyCellStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                currencyCellStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
                currencyCellStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
                currencyCellStyle.BottomBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                currencyCellStyle.LeftBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                currencyCellStyle.RightBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                currencyCellStyle.TopBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                font1.FontHeightInPoints = 11;
                currencyCellStyle.SetFont(font1);
                // Get / create the data format string
                var formatId = HSSFDataFormat.GetBuiltinFormat("#,##0;[Red](#,##0)");
                if (formatId == -1)
                {
                    var newDataFormat = wb.CreateDataFormat();
                    currencyCellStyle.DataFormat = newDataFormat.GetFormat("#,##0;[Red](#,##0)");
                }
                else
                    currencyCellStyle.DataFormat = formatId;

                var currencyCellStyle4 = wb.CreateCellStyle();
                currencyCellStyle4.Alignment = HorizontalAlignment.Right;
                currencyCellStyle4.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
                currencyCellStyle4.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                currencyCellStyle4.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
                currencyCellStyle4.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
                currencyCellStyle4.BottomBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                currencyCellStyle4.LeftBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                currencyCellStyle4.RightBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                currencyCellStyle4.TopBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                //font2.Boldweight = (short)FontBoldWeight.Bold;
                font2.FontHeightInPoints = 11;
                currencyCellStyle4.SetFont(font2);
                if (formatId == -1)
                {
                    var newDataFormat = wb.CreateDataFormat();
                    currencyCellStyle4.DataFormat = newDataFormat.GetFormat("#,##0;[Red](#,##0)");
                }
                else
                    currencyCellStyle4.DataFormat = formatId;


                ICellStyle style4 = wb.CreateCellStyle();
                style4.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
                style4.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                style4.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
                style4.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
                style4.BottomBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                style4.LeftBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                style4.RightBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                style4.TopBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                style4.Alignment = HorizontalAlignment.Center;
                style4.VerticalAlignment = VerticalAlignment.Center;
                style4.SetFont(font2);

                ws.CreateRow(0).HeightInPoints = 20; ;
                var rang = ws.CreateRow(1);
                var cell3 = rang.CreateCell(0);
                if (ViewBag.RepID != "0119")
                {
                    if (ViewBag.RepID == "0122")
                    {
                        cell3 = rang.CreateCell(1);
                        cell3.SetCellValue("傳輸月份： " + ViewBag.Start);
                    }
                    else
                    {
                        cell3 = rang.CreateCell(1);
                        cell3.SetCellValue("清算月份： " + ViewBag.Start);
                    }
                }
                else
                {
                    cell3.SetCellValue("清算期間： " + ViewBag.Start + "~" + ViewBag.End);
                }
                if (ViewBag.RepID == "0123")
                {
                    cell3.SetCellValue("交易期間： " + ViewBag.Start + "~" + ViewBag.End);
                }

                cell3.CellStyle = style1;
                cell3 = ws.GetRow(0).CreateCell(0);
                cell3.SetCellValue(ViewBag.RepName);
                if (ViewBag.RepID == "0117")
                    ws.AddMergedRegion(new CellRangeAddress(0, 0, 0, 23));
                else
                    ws.AddMergedRegion(new CellRangeAddress(0, 0, 0, 8));
                cell3.CellStyle = style2;

                Double[] total = new Double[27];
                Double[] totals = new Double[27];
                int ii = 2;
                string temp = "";
                Double totFeeUT = 0;
                Double totFeeUT3 = 0;
                Double totFee6 = 0;
                Double totFee = 0;
                Double totFee12 = 0;
                Double totFee79 = 0;

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (i == 0) temp = dt.Rows[i][0].ToString();
                    if (temp != dt.Rows[i][0].ToString() )
                    {
                        temp = dt.Rows[i][0].ToString();
                        //小計
                        ws.CreateRow(ii + 1);
                        cell3 = ws.GetRow(ii + 1).CreateCell(0);
                        cell3.CellStyle = currencyCellStyle4;
                        cell3 = ws.GetRow(ii + 1).CreateCell(1);
                        cell3.CellStyle = currencyCellStyle4;
                        cell3 = ws.GetRow(ii + 1).CreateCell(2);
                        cell3.CellStyle = currencyCellStyle4;
                        cell3.SetCellValue("小計");
                        cell3.CellStyle = style4;

                        if (dt.Rows.Count > 0)
                        {
                            for (int j = 3; j < dt.Columns.Count; j++)
                            {
                                if (ViewBag.RepID == "0123" && j==3) j = j+1;

                                cell3 = ws.GetRow(ii + 1).CreateCell(j);
                                //決定頁面顯示前幾欄位 0117
                                if ((j == 6 || j == 11 || j == 16 || j == 19 || j == 22) && (ViewBag.RepID == "0117")) //&& dt.Rows[1][j].ToString() != ""
                                {
                                    if (dt.Rows[i - 1][j].ToString() == "")
                                        cell3.SetCellValue(0);
                                    else
                                    {
                                        ///cell3.SetCellValue(total[j - 1] * Convert.ToDouble(dt.Rows[i - 1][j].ToString()) / 100.0);
                                        //cell3.SetCellValue(100);
                                        //cell3.SetCellValue(total[j - 1] * Convert.ToDouble(dt.Rows[i - 1][j].ToString()) / 100.0);
                                        if (j == 6) totFeeUT = totFeeUT + total[j - 1] * Convert.ToDouble(dt.Rows[i - 1][j].ToString()) / 100.0;
                                        if (j == 11) totFeeUT3 = totFeeUT3 + total[j - 1] * Convert.ToDouble(dt.Rows[i - 1][j].ToString()) / 100.0;
                                        if (j == 16) totFee6 = totFee6 + total[j - 1] * Convert.ToDouble(dt.Rows[i - 1][j].ToString()) / 100.0;
                                        if (j == 19) totFee = totFee + total[j - 1] * Convert.ToDouble(dt.Rows[i - 1][j].ToString()) / 100.0;
                                        if (j == 22) totFee79 = totFee79 + total[j - 1] * Convert.ToDouble(dt.Rows[i - 1][j].ToString()) / 100.0;
                                        //if (j == 24) totFee79 = totFee79 + total[j - 1] * Convert.ToDouble(dt.Rows[i - 1][j].ToString()) / 100.0;
                                    }
                                    //cell3.SetCellValue(dt.Rows[i-1][j].ToString());
                                }
                                //決定頁面顯示前幾欄位 0117
                                else if ((j == 7 || j == 12 || j == 17 || j == 20 || j == 23) && (ViewBag.RepID == "0117")) //&& dt.Rows[1][j].ToString() != ""
                                {
                                    if (dt.Rows[i - 1][j - 1].ToString() == "")
                                        cell3.SetCellValue(0);
                                    else
                                    {
                                        ///cell3.SetCellValue(total[j - 1] * Convert.ToDouble(dt.Rows[i - 1][j].ToString()) / 100.0);
                                        //cell3.SetCellValue(100);
                                        cell3.SetCellValue(total[j - 2] * Convert.ToDouble(dt.Rows[i - 1][j -1].ToString()) / 100.0);
                                    }
                                    //cell3.SetCellValue(dt.Rows[i-1][j].ToString());
                                }
                                else
                                    cell3.SetCellValue(Convert.ToDouble(total[j].ToString()));
                                cell3.CellStyle = currencyCellStyle4;
                            }

                        }
                        ii = ii + 1;

                        for (int j = 0; j < 27; j++)
                        {
                            total[j] = 0;
                        }
                    }

                    ws.CreateRow(ii + 1);
                    //明細
                    int jn = 2;
                    if (ViewBag.RepID == "0123") jn = 3;
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        var cell2 = ws.GetRow(ii + 1).CreateCell(j);
                        if (ViewBag.RepID == "0122") jn = 4;

                        if (j > jn) //前3欄為文字
                        {
                            //cell2.SetCellValue(Convert.ToDouble(dt.Rows[i][j].ToString()));
                            if (ViewBag.RepID == "0117")
                            {
                                //決定頁面顯示前幾欄位 0117
                                if (j == 6 || j == 11 || j == 16 || j == 19 || j == 22)
                                {
                                    //費率
                                }
                                else
                                {
                                    total[j] = total[j] + Convert.ToDouble(dt.Rows[i][j].ToString());
                                    totals[j] = totals[j] + Convert.ToDouble(dt.Rows[i][j].ToString());
                                }
                            }
                            else if (ViewBag.RepID == "0122")
                                if (j == 8 || j == 13 || j == 16 || j == 19 || j == 22)
                                {
                                    //費率
                                }
                                else
                                {
                                    total[j] = total[j] + Convert.ToDouble(dt.Rows[i][j].ToString());
                                    totals[j] = totals[j] + Convert.ToDouble(dt.Rows[i][j].ToString());
                                }

                            else
                            {
                                total[j] = total[j] + Convert.ToDouble(dt.Rows[i][j].ToString());
                                totals[j] = totals[j] + Convert.ToDouble(dt.Rows[i][j].ToString());
                            }
                            if (ViewBag.RepID == "0117")
                            {
                                //決定頁面顯示前幾欄位 0117
                                if (j == 6 || j == 11 || j == 16 || j == 19 || j == 22)
                                    cell2.SetCellValue(dt.Rows[i][j].ToString() + "%");
                                else
                                    cell2.SetCellValue(Convert.ToDouble(dt.Rows[i][j].ToString()));
                            }
                            else if (ViewBag.RepID == "0122")
                            {
                                if (j == 8 || j == 13 || j == 16 || j == 19 || j == 22)
                                    cell2.SetCellValue(dt.Rows[i][j].ToString() + "%");
                                else
                                    cell2.SetCellValue(Convert.ToDouble(dt.Rows[i][j].ToString()));
                            }
                            else
                                cell2.SetCellValue(Convert.ToDouble(dt.Rows[i][j].ToString()));

                            cell2.CellStyle = currencyCellStyle;
                        }
                        else
                        {
                            cell2.SetCellValue(dt.Rows[i][j].ToString());
                            cell2.CellStyle = style3;
                        }

                    }
                    ii = ii + 1;

                }

                //最後小計
                ws.CreateRow(ii + 1);
                var cell3n = ws.GetRow(ii + 1).CreateCell(0);
                cell3n.CellStyle = currencyCellStyle4;
                cell3n = ws.GetRow(ii + 1).CreateCell(1);
                cell3n.CellStyle = currencyCellStyle;
                cell3n = ws.GetRow(ii + 1).CreateCell(2);
                cell3n.CellStyle = currencyCellStyle4;
                int jnn = 3;
                if (ViewBag.RepID == "0122")
                {
                    jnn = 5;
                    cell3n = ws.GetRow(ii + 1).CreateCell(3);
                    cell3n.CellStyle = currencyCellStyle4;
                    cell3n = ws.GetRow(ii + 1).CreateCell(4);
                    cell3n.CellStyle = currencyCellStyle4;
                }
                cell3n.SetCellValue("小計");
                if (ViewBag.RepID == "0122")
                    cell3n.SetCellValue("總計");
                cell3n.CellStyle = style4;
                if (ViewBag.RepID == "0123")
                {
                    jnn = 4;
                    cell3n = ws.GetRow(ii + 1).CreateCell(3);
                    cell3n.CellStyle = currencyCellStyle4;
                }


                if (dt.Rows.Count > 0)
                {
                    for (int j = jnn; j < dt.Columns.Count ; j++)
                    {
                        cell3n = ws.GetRow(ii + 1).CreateCell(j);
                        if (ViewBag.RepID == "0117")
                        {
                            //決定頁面顯示前幾欄位 0117
                            if (j == 6 || j == 11 || j == 16 || j == 19 || j == 22) //&& dt.Rows[1][j].ToString() != ""
                            {
                                if (dt.Rows[dt.Rows.Count - 1][j].ToString() == "")
                                    cell3n.SetCellValue(0);
                                else
                                {
                                    ///cell3n.SetCellValue(total[j - 1] * Convert.ToDouble(dt.Rows[dt.Rows.Count - 1][j].ToString()) / 100.0);
                                    if (j == 6) totFeeUT = totFeeUT + total[j - 1] * Convert.ToDouble(dt.Rows[dt.Rows.Count - 1][j].ToString()) / 100.0;
                                    if (j == 11) totFeeUT3 = totFeeUT3 + total[j - 1] * Convert.ToDouble(dt.Rows[dt.Rows.Count - 1][j].ToString()) / 100.0;
                                    if (j == 16) totFee6 = totFee6 + total[j - 1] * Convert.ToDouble(dt.Rows[dt.Rows.Count - 1][j].ToString()) / 100.0;
                                    if (j == 19) totFee = totFee + total[j - 1] * Convert.ToDouble(dt.Rows[dt.Rows.Count - 1][j].ToString()) / 100.0;
                                    if (j == 22) totFee79 = totFee79 + total[j - 1] * Convert.ToDouble(dt.Rows[dt.Rows.Count - 1][j].ToString()) / 100.0;
                                }
                                //cell3n.SetCellValue(total[j - 1] * Convert.ToDouble(dt.Rows[dt.Rows.Count -1][j].ToString()) / 100.0);
                                //cell3n.SetCellValue(dt.Rows[dt.Rows.Count - 1][j].ToString());
                            }
                            //決定頁面顯示前幾欄位 0117
                            else if (j == 7 || j == 12 || j == 17 || j == 20 || j == 23) //&& dt.Rows[1][j].ToString() != ""
                            {
                                if (dt.Rows[dt.Rows.Count - 1][j - 1].ToString() == "")
                                    cell3n.SetCellValue(0);
                                else
                                {
                                    cell3n.SetCellValue(total[j - 2] * Convert.ToDouble(dt.Rows[dt.Rows.Count - 1][j - 1].ToString()) / 100.0);
                                }
                                //cell3n.SetCellValue(total[j - 1] * Convert.ToDouble(dt.Rows[dt.Rows.Count -1][j].ToString()) / 100.0);
                                //cell3n.SetCellValue(dt.Rows[dt.Rows.Count - 1][j].ToString());
                            }
                            else
                                cell3n.SetCellValue(Convert.ToDouble(total[j].ToString()));

                        }
                        else if (ViewBag.RepID == "0122")
                        {
                            if (j == 8 || j == 13 || j == 16 || j == 19 || j == 22 ) //&& dt.Rows[1][j].ToString() != ""
                            {
                                //if (dt.Rows[dt.Rows.Count - 1][j].ToString() == "")
                                //    cell3n.SetCellValue(0);
                                //else
                                //{
                                    ///cell3n.SetCellValue(total[j - 1] * Convert.ToDouble(dt.Rows[dt.Rows.Count - 1][j].ToString()) / 100.0);
                                    if (j == 8) totFeeUT = totFeeUT + total[j - 1] * Convert.ToDouble(dt.Rows[dt.Rows.Count - 1][j].ToString()) / 100.0;
                                    if (j == 13) totFeeUT3 = totFeeUT3 + total[j - 1] * Convert.ToDouble(dt.Rows[dt.Rows.Count - 1][j].ToString()) / 100.0;
                                    if (j == 16) totFee6 = totFee6 + total[j - 1] * Convert.ToDouble(dt.Rows[dt.Rows.Count - 1][j].ToString()) / 100.0;
                                    if (j == 19) totFee = totFee + total[j - 1] * Convert.ToDouble(dt.Rows[dt.Rows.Count - 1][j].ToString()) / 100.0;
                                    if (j == 22) totFee12 = totFee12 + total[j - 1] * Convert.ToDouble(dt.Rows[dt.Rows.Count - 1][j].ToString()) / 100.0;
                                //}
                                //cell3.SetCellValue(total[j - 1] * Convert.ToDouble(dt.Rows[dt.Rows.Count -1][j].ToString()) / 100.0);
                                //cell3.SetCellValue(dt.Rows[dt.Rows.Count - 1][j].ToString());
                            }
                            else
                                cell3n.SetCellValue(Convert.ToDouble(total[j].ToString()));

                        }
                        else
                            cell3n.SetCellValue(Convert.ToDouble(total[j].ToString()));
                        cell3n.CellStyle = currencyCellStyle4;
                    }

                }
                ii = ii + 1;

                if (ViewBag.RepID != "0122")
                {
                //總計
                ws.CreateRow(ii + 1);
                var cell3s = ws.GetRow(ii + 1).CreateCell(0);
                cell3s.CellStyle = currencyCellStyle;
                cell3s = ws.GetRow(ii + 1).CreateCell(1);
                cell3s.CellStyle = currencyCellStyle;
                cell3s = ws.GetRow(ii + 1).CreateCell(2);
                cell3s.CellStyle = currencyCellStyle;
                if (ViewBag.RepID == "0122")
                {
                    cell3s = ws.GetRow(ii + 1).CreateCell(3);
                    cell3s.CellStyle = currencyCellStyle;
                    cell3s = ws.GetRow(ii + 1).CreateCell(4);
                    cell3s.CellStyle = currencyCellStyle;
                }
                cell3s.SetCellValue("總計");
                cell3s.CellStyle = currencyCellStyle;
                if (ViewBag.RepID == "0123")
                {
                    jnn = 4;
                    cell3n = ws.GetRow(ii + 1).CreateCell(3);
                    cell3n.CellStyle = currencyCellStyle4;
                }

                if (dt.Rows.Count > 0)
                {
                    for (int j = jnn; j < dt.Columns.Count; j++)
                    {
                        cell3s = ws.GetRow(ii + 1).CreateCell(j);
                        if ((j == 6 || j == 11 || j == 16 || j == 19 || j == 22) && (ViewBag.RepID == "0117") ) //&& dt.Rows[1][j].ToString() != ""
                        {
                            //cell3s.SetCellValue(totals[j - 1] * Convert.ToDouble(dt.Rows[1][j].ToString()) / 100.0);
                            //if (j == 6) cell3s.SetCellValue(totFeeUT);
                            //if (j == 11) cell3s.SetCellValue(totFeeUT3);
                            //if (j == 16) cell3s.SetCellValue(totFee6);
                            //if (j == 19) cell3s.SetCellValue(totFee);
                            //if (j == 22) cell3s.SetCellValue(totFee12);
                        }
                        //決定頁面顯示前幾欄位 0117
                        else if ((j == 7 || j == 12 || j == 17 || j == 20 || j == 23) && (ViewBag.RepID == "0117")) //&& dt.Rows[1][j].ToString() != ""
                        {
                            if (j == 7) cell3s.SetCellValue(totFeeUT);
                            if (j == 12) cell3s.SetCellValue(totFeeUT3);
                            if (j == 17) cell3s.SetCellValue(totFee6);
                            if (j == 20) cell3s.SetCellValue(totFee);
                            if (j == 23) cell3s.SetCellValue(totFee79);
                        }
                        else
                            cell3s.SetCellValue(Convert.ToDouble(totals[j].ToString()));

                        cell3s.CellStyle = currencyCellStyle;
                    }

                }
                }

                var stream = new MemoryStream();
                wb.Write(stream);
                stream.Close();

                return File(stream.ToArray(), "application/vnd.ms-excel",  ViewBag.RepName + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls");


            }
            else
            {
                return View(dt);
            }

        }


        [HttpPost]
        public ActionResult Report0111Result()
        {
            string startYM = Request.Form["startDate"];
            ViewBag.StartYM = startYM;

            DataTable dt = reportManager.Report0111(startYM);
            DataTable dt2 = reportManager.Report0111D(startYM);

            if (Request.Form["ExportExcel"] != null) //產生標籤Excel
            {
                //建立Excel 2003檔案
                IWorkbook wb = new HSSFWorkbook();
                ISheet ws;
                ISheet ws2;

                ////建立Excel 2007檔案
                //IWorkbook wb = new XSSFWorkbook();
                //ISheet ws;
                ws = wb.CreateSheet("愛金卡儲值餘額總表");
                
                ws.CreateRow(2);//第一行為欄位名稱
                var headerRow = ws.CreateRow(2);

                HSSFCellStyle cs = (HSSFCellStyle)wb.CreateCellStyle();
                //啟動多行文字
                //cs.WrapText = true;
                //文字置中
                cs.VerticalAlignment = VerticalAlignment.Center;
                cs.Alignment = HorizontalAlignment.Center;
                //框線樣式及顏色
                cs.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
                cs.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                cs.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
                cs.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
                cs.BottomBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                cs.LeftBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                cs.RightBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                cs.TopBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                //背景顏色
                cs.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.Grey25Percent.Index;
                cs.FillPattern = NPOI.SS.UserModel.FillPattern.SolidForeground;
                HSSFFont font1 = (HSSFFont)wb.CreateFont();
                //字體顏色
                font1.Color = NPOI.HSSF.Util.HSSFColor.Black.Index;
                //字體粗體
                //font1.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.Bold;
                //字體尺寸
                font1.FontHeightInPoints = 12;
                cs.SetFont(font1);

                headerRow.HeightInPoints = 24;

                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    var cell = headerRow.CreateCell(i);
                    cell.SetCellValue(dt.Columns[i].ColumnName);
                    cell.CellStyle = cs;
                    //ws.AutoSizeColumn(i);
                    if(i==0)
                        ws.SetColumnWidth(0, 8 * 256);
                    else if (i == 1)
                        ws.SetColumnWidth(1, 12 * 256);
                    else
                        ws.SetColumnWidth(i, 20 * 256);
                }

                //fill content 
                ICellStyle style3 = wb.CreateCellStyle();
                //框線樣式及顏色
                style3.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
                style3.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                style3.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
                style3.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
                style3.BottomBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                style3.LeftBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                style3.RightBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                style3.TopBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                style3.Alignment = HorizontalAlignment.Center;
                style3.VerticalAlignment = VerticalAlignment.Center;
                //字體尺寸
                font1.FontHeightInPoints = 11;
                style3.SetFont(font1);
                //style3.FillForegroundColor = IndexedColors.LemonChiffon.Index;
                //style3.FillPattern = FillPattern.SolidForeground;
                //style3.FillBackgroundColor = IndexedColors.LightGreen.Index;

                ICellStyle style2 = wb.CreateCellStyle();
                //框線樣式及顏色
                style2.Alignment = HorizontalAlignment.Center;
                style2.VerticalAlignment = VerticalAlignment.Center;
                //字體尺寸
                font1.FontHeightInPoints = 20;
                style2.SetFont(font1);

                // Create the style object
                var currencyCellStyle = wb.CreateCellStyle();
                // Right-align currency values
                currencyCellStyle.Alignment = HorizontalAlignment.Right;
                currencyCellStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
                currencyCellStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                currencyCellStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
                currencyCellStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
                currencyCellStyle.BottomBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                currencyCellStyle.LeftBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                currencyCellStyle.RightBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                currencyCellStyle.TopBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                font1.FontHeightInPoints = 11;
                currencyCellStyle.SetFont(font1);
                // Get / create the data format string
                var formatId = HSSFDataFormat.GetBuiltinFormat("#,##0");
                if (formatId == -1)
                {
                    var newDataFormat = wb.CreateDataFormat();
                    currencyCellStyle.DataFormat = newDataFormat.GetFormat("#,##0");
                }
                else
                    currencyCellStyle.DataFormat = formatId;

                ws.CreateRow(0);
                var cell3 = ws.GetRow(0).CreateCell(0);
                cell3.SetCellValue("愛金卡儲值餘額總表");
                ws.AddMergedRegion(new CellRangeAddress(0, 0, 0, 12));
                cell3.CellStyle = style2;

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ws.CreateRow(i + 3);
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        var cell2 = ws.GetRow(i + 3).CreateCell(j);
                        if(j>1){
                            cell2.SetCellValue(Convert.ToDouble(dt.Rows[i][j].ToString()));
                            cell2.CellStyle = currencyCellStyle;
                        }
                        else
                        {
                            cell2.SetCellValue(dt.Rows[i][j].ToString());
                            cell2.CellStyle = style3;
                        }                          
                        
                      
                    }
                }

                ws2 = wb.CreateSheet("愛金卡儲值餘額明細表");

                ws2.CreateRow(0);
                cell3 = ws2.GetRow(0).CreateCell(0);
                cell3.SetCellValue("愛金卡儲值餘額明細表");
                ws2.AddMergedRegion(new CellRangeAddress(0, 0, 0, 12));
                cell3.CellStyle = style2;

                ws2.CreateRow(2);//第一行為欄位名稱
                var headerRow2 = ws2.CreateRow(2);

                headerRow2.HeightInPoints = 24;

                for (int i = 0; i < dt2.Columns.Count; i++)
                {
                    var cell = headerRow2.CreateCell(i);
                    cell.SetCellValue(dt2.Columns[i].ColumnName);
                    cell.CellStyle = cs;
                    //ws.AutoSizeColumn(i);
                    if (i == 0)
                        ws2.SetColumnWidth(0, 8 * 256);
                    else if (i == 1)
                        ws2.SetColumnWidth(1, 12 * 256);
                    else
                        ws2.SetColumnWidth(i, 20 * 256);
                }

                for (int i = 0; i < dt2.Rows.Count; i++)
                {
                    ws2.CreateRow(i + 3);
                    for (int j = 0; j < dt2.Columns.Count; j++)
                    {
                        var cell2 = ws2.GetRow(i + 3).CreateCell(j);
                        if (j > 4)
                        {
                            cell2.SetCellValue(Convert.ToDouble(dt2.Rows[i][j].ToString()));
                            cell2.CellStyle = currencyCellStyle;
                        }
                        else
                        {
                            cell2.SetCellValue(dt2.Rows[i][j].ToString());
                            cell2.CellStyle = style3;
                        }
                        

                    }
                }

                var stream = new MemoryStream();
                wb.Write(stream);
                stream.Close();

                return File(stream.ToArray(), "application/vnd.ms-excel", "愛金卡儲值餘額表_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls");


            }
            else
            {
                return View(dt);
            }
        }

        [HttpPost]
        public ActionResult Report0111Detail()
        {
            string startYM = Request.Form["startDate"];
            ViewBag.StartYMD = startYM;
            ViewBag.StartYM = startYM.Substring(0, 6);

            DataTable dt = reportManager.Report0111D(startYM);

            if (Request.Form["ExportExcel"] != null) //產生標籤Excel
            {
                //建立Excel 2003檔案
                IWorkbook wb = new HSSFWorkbook();
                ISheet ws;

                ////建立Excel 2007檔案
                //IWorkbook wb = new XSSFWorkbook();
                //ISheet ws;

                if (dt.TableName != string.Empty)
                {
                    ws = wb.CreateSheet(dt.TableName);
                }
                else
                {
                    ws = wb.CreateSheet("Sheet1");
                }

                ws.CreateRow(0);//第一行為欄位名稱
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    ws.GetRow(0).CreateCell(i).SetCellValue(dt.Columns[i].ColumnName);
                }

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ws.CreateRow(i + 1);
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        ws.GetRow(i + 1).CreateCell(j).SetCellValue(dt.Rows[i][j].ToString());
                    }
                }

                var stream = new MemoryStream();
                wb.Write(stream);
                stream.Close();

                return File(stream.ToArray(), "application/vnd.ms-excel", "愛金卡儲值餘額表_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls");


            }
            else
            {
                return View(dt);
            }
        }

        public ActionResult Report0211Result()
        {
            string startICCNO = Request.Form["iccno"];
            ViewBag.StartICCNO = startICCNO;
            string startSTORE_NO = Request.Form["store_no"];
            ViewBag.StartSTORE_NO = startSTORE_NO;

            DataTable dt = reportManager.Report0211(startICCNO, startSTORE_NO);

            if (Request.Form["ExportExcel"] != null) //產生標籤Excel
            {
                //建立Excel 2003檔案
                IWorkbook wb = new HSSFWorkbook();
                ISheet ws;
                //ISheet ws2;

                ////建立Excel 2007檔案
                //IWorkbook wb = new XSSFWorkbook();
                //ISheet ws;
                ws = wb.CreateSheet("愛金卡IM_ISET_TXLOG_T表");

                ws.CreateRow(0);//第一行為欄位名稱
                var headerRow = ws.CreateRow(0);

                HSSFCellStyle cs = (HSSFCellStyle)wb.CreateCellStyle();
                //啟動多行文字
                //cs.WrapText = true;
                //文字置中
                cs.VerticalAlignment = VerticalAlignment.Center;
                cs.Alignment = HorizontalAlignment.Center;
                //框線樣式及顏色
                cs.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
                cs.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                cs.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
                cs.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
                cs.BottomBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                cs.LeftBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                cs.RightBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                cs.TopBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                //背景顏色
                cs.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.Grey25Percent.Index;
                cs.FillPattern = NPOI.SS.UserModel.FillPattern.SolidForeground;
                HSSFFont font1 = (HSSFFont)wb.CreateFont();
                //字體顏色
                font1.Color = NPOI.HSSF.Util.HSSFColor.Black.Index;
                //字體粗體
                //font1.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.Bold;
                //字體尺寸
                font1.FontHeightInPoints = 12;
                cs.SetFont(font1);

                headerRow.HeightInPoints = 24;

                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    var cell = headerRow.CreateCell(i);
                    cell.SetCellValue(dt.Columns[i].ColumnName);
                    cell.CellStyle = cs;
                    //ws.AutoSizeColumn(i);
                    if (i == 0)
                        ws.SetColumnWidth(0, 8 * 256);
                    else if (i == 1)
                        ws.SetColumnWidth(1, 12 * 256);
                    else
                        ws.SetColumnWidth(i, 20 * 256);
                }

                //fill content 
                ICellStyle style3 = wb.CreateCellStyle();
                //框線樣式及顏色
                style3.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
                style3.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                style3.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
                style3.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
                style3.BottomBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                style3.LeftBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                style3.RightBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                style3.TopBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                style3.Alignment = HorizontalAlignment.Center;
                style3.VerticalAlignment = VerticalAlignment.Center;
                //字體尺寸
                font1.FontHeightInPoints = 11;
                style3.SetFont(font1);
                //style3.FillForegroundColor = IndexedColors.LemonChiffon.Index;
                //style3.FillPattern = FillPattern.SolidForeground;
                //style3.FillBackgroundColor = IndexedColors.LightGreen.Index;


                // Create the style object
                var currencyCellStyle = wb.CreateCellStyle();
                // Right-align currency values
                currencyCellStyle.Alignment = HorizontalAlignment.Right;
                currencyCellStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
                currencyCellStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                currencyCellStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
                currencyCellStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
                currencyCellStyle.BottomBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                currencyCellStyle.LeftBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                currencyCellStyle.RightBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                currencyCellStyle.TopBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                font1.FontHeightInPoints = 11;
                currencyCellStyle.SetFont(font1);
                // Get / create the data format string
                var formatId = HSSFDataFormat.GetBuiltinFormat("#,##0");
                if (formatId == -1)
                {
                    var newDataFormat = wb.CreateDataFormat();
                    currencyCellStyle.DataFormat = newDataFormat.GetFormat("#,##0");
                }
                else
                    currencyCellStyle.DataFormat = formatId;



                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ws.CreateRow(i + 1);
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        var cell2 = ws.GetRow(i + 1).CreateCell(j);
                        if (j > 1)
                        {
                            //cell2.SetCellValue(Convert.ToDouble(dt.Rows[i][j].ToString()));
                            cell2.SetCellValue(dt.Rows[i][j].ToString());
                            cell2.CellStyle = currencyCellStyle;
                        }
                        else
                        {
                            cell2.SetCellValue(dt.Rows[i][j].ToString());
                            cell2.CellStyle = style3;
                        }


                    }
                }


                var stream = new MemoryStream();
                wb.Write(stream);
                stream.Close();

                return File(stream.ToArray(), "application/vnd.ms-excel", "愛金卡TXLOG_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls");


            }
            else
            {
                return View(dt);
            }
        }

        public ActionResult Report0212Result()
        {
            int year = DateTime.Today.Year;
            int month = DateTime.Today.Month;
            int day = DateTime.Today.Day;
            DateTime startDate = new DateTime(year, month, day);
            string start = startDate.ToString("yyyyMMdd");
            if (Request.Form["searchConfirm"] != null || Request.Form["ExportExcel"] != null)
            {
                start = Request.Form["startDate"];
            }
            ViewBag.Start = start;
            ViewBag.RepName = "儲值餘額表重複資料";
            ViewBag.RepID = "0212";
            //AMS.Models.AmUsersModel model = new AMS.Models.AmUsersModel();
            //ViewBag.Username = DateTime.Now.ToString("mmssfff");
            ViewBag.Username = User.Identity.GetUserName();
            DataTable dt = reportManager.Report0212T(start);
            return Report312Result(dt, "儲值餘額表重複資料", "", "0212");

        }

        public ActionResult Report0213Result()
        {
            //DateTime tempDate;

            int year = DateTime.Today.Year;
            int month = DateTime.Today.Month;
            DateTime startDate = new DateTime(year, month, 1);
            DateTime endDate = new DateTime(year, month,
                                    DateTime.DaysInMonth(year, month));
            string start = startDate.ToString("yyyyMMdd");
            string end = endDate.ToString("yyyyMMdd");
            start = "";
            end = "";
            //string merchantNo = "";
            string merchant = "";
            string item = "";

            string group = "";
            string retail = "";
            string bus = "";
            string bike = "";
            string track = "";
            string parking = "";
            string outsourcing = "";

            string SRC_FLG = "TXLOG";

            ViewBag.Start = startDate.ToString("yyyyMMdd");
            ViewBag.End = endDate.ToString("yyyyMMdd");

            if (Request.Form["searchConfirm"] != null || Request.Form["ExportExcel"] != null)
            {
                //start = Request.Form["startDate"] + "01";
                //tempDate = DateTime.Parse(start.Substring(0, 4) + "-" + start.Substring(4, 2) + "-" + start.Substring(6, 2)).AddDays(-1);
                //start = tempDate.ToString("yyyyMMdd");
                start = Request.Form["startDate"];
                //int days = DateTime.DaysInMonth(int.Parse(Request.Form["startDate"].Substring(0, 4)), int.Parse(Request.Form["startDate"].Substring(4, 2))) - 1;
                //end = Request.Form["startDate"] + days.ToString();
                end = Request.Form["endDate"];
                //merchantNo = Request.Form["Merchant"];
                SRC_FLG = Request.Form["SRC_FLG"];
                ViewBag.Start = Request.Form["startDate"];
                ViewBag.End = Request.Form["endDate"];
                /*
                if (merchantNo == "")
                {
                    DisplayMessage("請選擇特約機構");
                }
                */
                group = Request.Form["group"];
                item = Request.Form["items"];
                //group = Request.Form["group"];
                //retail = Request.Form["retail"];
                //bus = Request.Form["bus"];
                //bike = Request.Form["bike"];
                //track = Request.Form["track"];
                //parking = Request.Form["parking"];
                //outsourcing = Request.Form["outsourcing"];

                //if (group == "PARKING_LOT")
                //    {merchant = Request.Form["parking"];}
                //else if (group == "BANK_OUTSOURCING")
                //{ merchant = Request.Form["outsourcing"]; }
                //else
                //{ merchant = Request.Form[group.ToLower()]; }
                merchant = Request.Form["items"];

                if (start == "" || end == "")
                {
                    DisplayMessage("請選擇日期");
                }
            }
            if (SRC_FLG == "TXLOG")
            {
                ViewBag.ch1 = "checked";
                ViewBag.ch2 = "";
                ViewBag.RepName = "每日交易統計表(卡機帳)";
            }
            else
            {
                ViewBag.ch1 = "";
                ViewBag.ch2 = "checked";
                ViewBag.RepName = "每日交易統計表(POS帳)";
            }

            ViewBag.MERCHANT = SetGroup(group);
            ViewBag.MERCHANT_NAME = group;
            ViewBag.ITEM = item;
            //ViewBag.GROUP = SetGroup(group); ViewBag.groups = group;
            //ViewBag.RETAIL = SetRetail(retail); ViewBag.retails = retail;
            //ViewBag.BUS = SetBus(bus); ViewBag.buses = bus;
            //ViewBag.BIKE = SetBike(bike); ViewBag.bikes = bike;
            //ViewBag.TRACK = SetTrack(track); ViewBag.tracks = track;
            //ViewBag.PARKING = SetParking(parking); ViewBag.parkings = parking;
            //ViewBag.OUTSOURCING = SetOutsourcing(outsourcing); ViewBag.outsourcings = outsourcing;

            //ViewBag.Merchant = SetMerchantDropDown(merchantNo);
            ViewBag.RepID = "0213";
            /*
            DataTable dtn = reportManager.MerchantName(merchantNo);
            if (dtn.Rows.Count > 0)
            {
                ViewBag.MerchantName = dtn.Rows[0][1].ToString()+" ";
            }
            else
            {
                ViewBag.MerchantName = "";
            }
            if (ViewBag.MerchantName == "")
            {
                merchantNo = "1";
            }
            */
            DataTable dt = new DataTable();

            List<string> mNoList = new List<string>();
            if (group == "ALL")
            {
                mNoList = new GmMerchantTypeDAO().FindAll();

                foreach (string mNo in mNoList)
                { dt.Merge(reportManager.Report0213T(start, end, mNo, SRC_FLG)); }
            }
            else if (item == "ALL") 
            //else if (retail == "ALL" || bus == "ALL" || bike == "ALL" || track == "ALL" || parking == "ALL" || outsourcing == "ALL")
            {
                mNoList = new GmMerchantTypeDAO().FindGroupAll(group);
                foreach (string mNo in mNoList)
                { dt.Merge(reportManager.Report0213T(start, end, mNo, SRC_FLG)); }
            }
            else if (merchant != "")
            {
                dt = reportManager.Report0213T(start, end, merchant, SRC_FLG);
            }
            else { dt = new DataTable(); }


            long[] total = new long[12] {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            for (int i = 0; i < dt.Rows.Count;i++ )
            {
                for (int j = 4; j < dt.Columns.Count; j++)
                {
                    total[j - 4] += Convert.ToInt64(dt.Rows[i][j]);
                }
            }

            ViewBag.Total = total;

                //DataTable dt = reportManager.Report0213T(start, end, merchantNo, SRC_FLG);
                ViewBag.Count = dt.Rows.Count;
            //return Report314Result(dt, ViewBag.MerchantName+ViewBag.RepName, "0213", "");
            if (Request.Form["searchConfirm"] != null)
            { return View(dt); }
            else if (Request.Form["ExportExcel"] != null) //產生標籤Excel
            {

                //建立Excel 2003檔案
                IWorkbook wb = new HSSFWorkbook();
                ISheet ws;
                //ISheet ws2;

                ////建立Excel 2007檔案
                //IWorkbook wb = new XSSFWorkbook();
                //ISheet ws;
                ws = wb.CreateSheet(ViewBag.RepName);

                var headerRow = ws.CreateRow(3);//第4行為欄位名稱


                HSSFCellStyle cs = (HSSFCellStyle)wb.CreateCellStyle();
                //啟動多行文字
                //cs.WrapText = true;
                //文字置中
                cs.VerticalAlignment = VerticalAlignment.Center;
                cs.Alignment = HorizontalAlignment.Center;
                //框線樣式及顏色
                cs.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
                cs.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                cs.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
                cs.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
                cs.BottomBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                cs.LeftBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                cs.RightBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                cs.TopBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                //背景顏色
                cs.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.PaleBlue.Index;
                cs.FillPattern = NPOI.SS.UserModel.FillPattern.SolidForeground;
                HSSFFont font1 = (HSSFFont)wb.CreateFont();
                //字體顏色
                font1.Color = NPOI.HSSF.Util.HSSFColor.Black.Index;
                //字體粗體
                //font1.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.Bold;
                //字體尺寸
                font1.FontHeightInPoints = 12;
                cs.SetFont(font1);
                cs.WrapText = true;

                HSSFFont font2 = (HSSFFont)wb.CreateFont();
                //字體顏色
                font2.Color = NPOI.HSSF.Util.HSSFColor.Black.Index;
                //字體粗體
                font2.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.Bold;
                //字體尺寸
                font2.FontHeightInPoints = 20;

                //headerRow.HeightInPoints = 20;
                var cell = headerRow.CreateCell(0);

                //if (RepID == "0213")
                {
                    ws.PrintSetup.Landscape = true; //橫向列印
                    headerRow = ws.CreateRow(3);

                    cell = headerRow.CreateCell(0);
                    cell.SetCellValue("特約機構");
                    cell.CellStyle = cs;

                    cell = headerRow.CreateCell(1);
                    cell.CellStyle = cs;
                    cell.SetCellValue("傳輸日");

                    cell = headerRow.CreateCell(2);
                    cell.CellStyle = cs;
                    cell.SetCellValue("清算日");

                    cell = headerRow.CreateCell(3);
                    cell.CellStyle = cs;
                    cell.SetCellValue("會計日");


                    cell = headerRow.CreateCell(4);
                    cell.SetCellValue("購貨");
                    cell.CellStyle = cs;
                    cell = headerRow.CreateCell(5);
                    cell.SetCellValue("");
                    cell.CellStyle = cs;
                    cell = headerRow.CreateCell(6);
                    cell.SetCellValue("");
                    cell.CellStyle = cs;
                    cell = headerRow.CreateCell(7);
                    cell.SetCellValue("");
                    cell.CellStyle = cs;
                    cell = headerRow.CreateCell(8);
                    cell.SetCellValue("");
                    cell.CellStyle = cs;
                    cell = headerRow.CreateCell(9);
                    cell.SetCellValue("");
                    cell.CellStyle = cs;

                    cell = headerRow.CreateCell(10);
                    cell.SetCellValue("加值");
                    cell.CellStyle = cs;
                    cell = headerRow.CreateCell(11);
                    cell.SetCellValue("");
                    cell.CellStyle = cs;
                    cell = headerRow.CreateCell(12);
                    cell.SetCellValue("");
                    cell.CellStyle = cs;
                    cell = headerRow.CreateCell(13);
                    cell.SetCellValue("");
                    cell.CellStyle = cs;
                    cell = headerRow.CreateCell(14);
                    cell.SetCellValue("");
                    cell.CellStyle = cs;
                    cell = headerRow.CreateCell(15);
                    cell.SetCellValue("");
                    cell.CellStyle = cs;
                    

                    headerRow = ws.CreateRow(4);

                    cell = headerRow.CreateCell(0);
                    cell.SetCellValue("特約機構");
                    cell.CellStyle = cs;

                    cell = headerRow.CreateCell(1);
                    cell.CellStyle = cs;
                    cell.SetCellValue("傳輸日");

                    cell = headerRow.CreateCell(2);
                    cell.CellStyle = cs;
                    cell.SetCellValue("清算日");

                    cell = headerRow.CreateCell(3);
                    cell.CellStyle = cs;
                    cell.SetCellValue("會計日");

                    cell = headerRow.CreateCell(4);
                    cell.CellStyle = cs;
                    cell.SetCellValue("購貨");
                    cell = headerRow.CreateCell(6);
                    cell.CellStyle = cs;
                    cell.SetCellValue("購貨取消");
                    cell = headerRow.CreateCell(8);
                    cell.CellStyle = cs;
                    cell.SetCellValue("小計");

                    cell = headerRow.CreateCell(10);
                    cell.CellStyle = cs;
                    cell.SetCellValue("加值");
                    cell = headerRow.CreateCell(12);
                    cell.CellStyle = cs;
                    cell.SetCellValue("加值取消");
                    cell = headerRow.CreateCell(14);
                    cell.CellStyle = cs;
                    cell.SetCellValue("小計");
                    cell = headerRow.CreateCell(15);
                    cell.CellStyle = cs;
                    cell.SetCellValue("");

                    
                    ws.AddMergedRegion(new CellRangeAddress(3, 3, 4, 9));
                    ws.AddMergedRegion(new CellRangeAddress(3, 3, 10, 15));

                    ws.AddMergedRegion(new CellRangeAddress(3, 5, 0, 0));
                    ws.AddMergedRegion(new CellRangeAddress(3, 5, 1, 1));
                    ws.AddMergedRegion(new CellRangeAddress(3, 5, 2, 2));
                    ws.AddMergedRegion(new CellRangeAddress(3, 5, 3, 3));

                    ws.AddMergedRegion(new CellRangeAddress(4, 4, 4, 5));
                    ws.AddMergedRegion(new CellRangeAddress(4, 4, 6, 7));
                    ws.AddMergedRegion(new CellRangeAddress(4, 4, 8, 9));
                    ws.AddMergedRegion(new CellRangeAddress(4, 4, 10, 11));
                    ws.AddMergedRegion(new CellRangeAddress(4, 4, 12, 13));
                    ws.AddMergedRegion(new CellRangeAddress(4, 4, 14, 15));

                }

                headerRow = ws.CreateRow(5);
                headerRow.HeightInPoints = 20;

                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    cell = headerRow.CreateCell(i);
                    cell.CellStyle = cs;

                    string[] cols = { "筆數", "金額" };
                    if (i <= 3)
                    {
                        cell.SetCellValue(dt.Columns[i].ColumnName);
                    }
                    else
                    {
                        cell.SetCellValue(cols[(i) % 2]);
                    }

                    
                    
                    if (i == 0)
                        ws.SetColumnWidth(0, 18 * 256);
                    else if (i == 1)
                        ws.SetColumnWidth(1, 20 * 256);
                    else if (i == 2)
                        ws.SetColumnWidth(i, 20 * 256);
                    else
                        ws.SetColumnWidth(i, 20 * 256);
                    
                }

                ICellStyle style1 = wb.CreateCellStyle();
                //框線樣式及顏色
                style1.Alignment = HorizontalAlignment.Left;
                style1.VerticalAlignment = VerticalAlignment.Center;
                //字體尺寸
                font1.FontHeightInPoints = 11;
                style1.SetFont(font1);

                ICellStyle style2 = wb.CreateCellStyle();
                //框線樣式及顏色
                style2.Alignment = HorizontalAlignment.Center;
                style2.VerticalAlignment = VerticalAlignment.Center;
                //字體尺寸
                font1.FontHeightInPoints = 11;
                //字體粗體
                style2.SetFont(font2);
                style2.WrapText = true; //自動換列

                //fill content 
                ICellStyle style3 = wb.CreateCellStyle();
                //框線樣式及顏色
                style3.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
                style3.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                style3.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
                style3.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
                style3.BottomBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                style3.LeftBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                style3.RightBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                style3.TopBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                style3.Alignment = HorizontalAlignment.Center;
                style3.VerticalAlignment = VerticalAlignment.Center;
                //字體尺寸
                font1.FontHeightInPoints = 11;
                style3.SetFont(font1);
                style3.WrapText = true;
                //style3.FillForegroundColor = IndexedColors.LemonChiffon.Index;
                //style3.FillPattern = FillPattern.SolidForeground;
                //style3.FillBackgroundColor = IndexedColors.LightGreen.Index;

                // Create the style object
                var currencyCellStyle = wb.CreateCellStyle();
                // Right-align currency values
                currencyCellStyle.Alignment = HorizontalAlignment.Right;
                currencyCellStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
                currencyCellStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                currencyCellStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
                currencyCellStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
                currencyCellStyle.BottomBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                currencyCellStyle.LeftBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                currencyCellStyle.RightBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                currencyCellStyle.TopBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                font1.FontHeightInPoints = 11;
                currencyCellStyle.SetFont(font1);
                // Get / create the data format string
                //var formatId = HSSFDataFormat.GetBuiltinFormat("#,##0");
                //if (formatId == -1)
                //{
                //    var newDataFormat = wb.CreateDataFormat();
                //    currencyCellStyle.DataFormat = newDataFormat.GetFormat("#,##0");
                //}
                //else
                //currencyCellStyle.DataFormat = formatId;
                //HSSFDataFormat myformat = wb.CreateDataFormat();
                //ICellStyle newDataFormat = wb.CreateCellStyle();
                currencyCellStyle.DataFormat = wb.CreateDataFormat().GetFormat("#,##0");

                {
                    var rang = ws.CreateRow(2);
                    ICell cell3 = rang.CreateCell(0);
                    cell3.CellStyle = style1;
                    cell3.SetCellValue("查詢日：" + ViewBag.Start + "~" + ViewBag.End);

                    cell3 = ws.CreateRow(0).CreateCell(0);
                    cell3.SetCellValue(ViewBag.MerchantName+" "+ViewBag.RepName);
                    ws.AddMergedRegion(new CellRangeAddress(0, 0, 0, 14));
                    cell3.CellStyle = style2; //標題

                    rang = ws.CreateRow(1);
                    cell3 = rang.CreateCell(0);
                    cell3.CellStyle = style1;
                    cell3.SetCellValue("列印日：" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                    //ws.AddMergedRegion(new CellRangeAddress(2, 2, 0, 2));
                }

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ws.CreateRow(i + 6).HeightInPoints=13;
         
                    for (int j = 0; j<dt.Columns.Count; j++)
                    {
                        ICell innerCell = ws.GetRow(i + 6).CreateCell(j);
                        switch (j)
                        {
                            case 0:
                            case 1:
                            case 2:
                            case 3:
                                innerCell.SetCellValue(dt.Rows[i][j].ToString());
                                innerCell.CellStyle = style3;
                                break;
                            default:
                                innerCell.SetCellValue(Convert.ToDouble(dt.Rows[i][j]));
                                innerCell.CellStyle=currencyCellStyle;
                                break;
                        }
                        //ws.AutoSizeColumn(i);
                    }
                }

                ws.CreateRow(dt.Rows.Count + 6).HeightInPoints = 13;

                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    ICell innerCell = ws.GetRow(dt.Rows.Count + 6).CreateCell(j);
                    switch (j)
                    {
                        case 0:
                            innerCell.SetCellValue("合計");
                            innerCell.CellStyle = style3;
                            break;
                        case 1:
                        case 2:
                        case 3:
                            innerCell.SetCellValue("");
                            innerCell.CellStyle = style3;
                            break;
                        default:
                            innerCell.SetCellValue(ViewBag.Total[j-4]);
                            innerCell.CellStyle = currencyCellStyle;
                            break;
                    }
                    //ws.AutoSizeColumn(i);
                }

                var stream = new MemoryStream();
                wb.Write(stream);
                stream.Close();

                return File(stream.ToArray(), "application/vnd.ms-excel", ViewBag.RepName + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls");

            }
            else { return View(new DataTable()); }
        }

        public ActionResult Report0214Result()
        {
            //int year = DateTime.Today.Year;
            //int month = DateTime.Today.Month;
            DateTime startDate = DateTime.Now;
            DateTime endDate = DateTime.Now;

            string start = startDate.ToString("yyyyMMdd");
            string end = endDate.ToString("yyyyMMdd");
            //start = "";
            //end = "";
            //string merchantNo = "22555003";

            string merchant = "";

            string group = "";
            string retail = "";
            string bus = "";
            string bike = "";
            string track = "";
            string parking = "";
            string outsourcing = "";

            string Kind = "00,02";

            if (Request.Form["searchConfirm"] != null || Request.Form["ExportExcel"] != null)
            {
                start = Request.Form["startDate"];
                end = Request.Form["endDate"];
                //merchantNo = Request.Form["Merchant"];
                Kind = Request.Form["Kind"];
                //if (merchantNo == "")
                //{
                //    DisplayMessage("請選擇特約機構");
                //}
                if (start == "" || end == "")
                {
                    DisplayMessage("請選擇日期");
                }

                group = Request.Form["group"];
                retail = Request.Form["retail"];
                bus = Request.Form["bus"];
                bike = Request.Form["bike"];
                track = Request.Form["track"];
                parking = Request.Form["parking"];
                outsourcing = Request.Form["outsourcing"];

                if (group == "PARKING_LOT")
                { merchant = Request.Form["parking"]; }
                else if (group == "BANK_OUTSOURCING")
                { merchant = Request.Form["outsourcing"]; }
                else
                { merchant = Request.Form[group.ToLower()]; }
            }
            ViewBag.Start = start;
            ViewBag.End = end;
            //ViewBag.Merchant = SetMerchantDropDown(merchantNo);
            ViewBag.Kind = Kind;

            ViewBag.GROUP = SetGroup(group); ViewBag.groups = group;
            ViewBag.RETAIL = SetRetail(retail); ViewBag.retails = retail;
            ViewBag.BUS = SetBus(bus); ViewBag.buses = bus;
            ViewBag.BIKE = SetBike(bike); ViewBag.bikes = bike;
            ViewBag.TRACK = SetTrack(track); ViewBag.tracks = track;
            ViewBag.PARKING = SetParking(parking); ViewBag.parkings = parking;
            ViewBag.OUTSOURCING = SetOutsourcing(outsourcing); ViewBag.outsourcings = outsourcing;
            /*
            DataTable dtn = reportManager.MerchantName(merchantNo);
            if (dtn.Rows.Count > 0)
            {
                ViewBag.MerchantName = dtn.Rows[0][1].ToString()+" ";
            }
            else
            {
                ViewBag.MerchantName = "";
            }
            if (ViewBag.MerchantName == "")
            {
                merchantNo = "1";
            }
            */

            //DisplayMessage(Request.Form["Kind"]);
            if (ViewBag.Kind == null)
            {
                //Response.Write("<script>alert('');</script>"); return View();
                DisplayMessage("請選擇差異類型");
                //DisplayMessage(Request.Form["Kinds"]);
            }
            if (Kind == "00")
            {
                ViewBag.ch1 = "checked";
                ViewBag.ch2 = "";
                ViewBag.ch3 = "";
                ViewBag.ch4 = "";
                ViewBag.Kinds = "一般";
            }
            if (Kind == "01")
            {
                ViewBag.ch1 = "";
                ViewBag.ch2 = "checked";
                ViewBag.ch3 = "";
                ViewBag.ch4 = "";
                ViewBag.Kinds = "重複";
            }
            if (Kind == "02")
            {
                ViewBag.ch1 = "";
                ViewBag.ch2 = "";
                ViewBag.ch3 = "checked";
                ViewBag.ch4 = "";
                ViewBag.Kinds = "跨月";
            }
            if (Kind == "03")
            {
                ViewBag.ch1 = "";
                ViewBag.ch2 = "";
                ViewBag.ch3 = "";
                ViewBag.ch4 = "checked";
                ViewBag.Kinds = "門市補傳";
            }
            if (Kind == "00,01")
            {
                ViewBag.ch1 = "checked";
                ViewBag.ch2 = "checked";
                ViewBag.ch3 = "";
                ViewBag.ch4 = "";
                ViewBag.Kinds = "一般/重複";
            }
            if (Kind == "00,02")
            {
                ViewBag.ch1 = "checked";
                ViewBag.ch2 = "";
                ViewBag.ch3 = "checked";
                ViewBag.ch4 = "";
                ViewBag.Kinds = "一般/跨月";
            }
            if (Kind == "00,03")
            {
                ViewBag.ch1 = "checked";
                ViewBag.ch2 = "";
                ViewBag.ch3 = "";
                ViewBag.ch4 = "checked";
                ViewBag.Kinds = "一般/門市補傳";
            }
            if (Kind == "01,02")
            {
                ViewBag.ch1 = "";
                ViewBag.ch2 = "checked";
                ViewBag.ch3 = "checked";
                ViewBag.ch4 = "";
                ViewBag.Kinds = "重複/跨月";
            }
            if (Kind == "01,03")
            {
                ViewBag.ch1 = "";
                ViewBag.ch2 = "checked";
                ViewBag.ch3 = "";
                ViewBag.ch4 = "checked";
                ViewBag.Kinds = "重複/門市補傳";
            }
            if (Kind == "02,03")
            {
                ViewBag.ch1 = "";
                ViewBag.ch2 = "";
                ViewBag.ch3 = "checked";
                ViewBag.ch4 = "checked";
                ViewBag.Kinds = "跨月/門市補傳";
            }
            if (Kind == "00,01,02")
            {
                ViewBag.ch1 = "checked";
                ViewBag.ch2 = "checked";
                ViewBag.ch3 = "checked";
                ViewBag.ch4 = "";
                ViewBag.Kinds = "一般/重複/跨月";
            }
            if (Kind == "00,01,03")
            {
                ViewBag.ch1 = "checked";
                ViewBag.ch2 = "checked";
                ViewBag.ch3 = "";
                ViewBag.ch4 = "checked";
                ViewBag.Kinds = "一般/重複/門市補傳";
            }
            if (Kind == "00,02,03")
            {
                ViewBag.ch1 = "checked";
                ViewBag.ch2 = "";
                ViewBag.ch3 = "checked";
                ViewBag.ch4 = "checked";
                ViewBag.Kinds = "一般/跨月/門市補傳";
            }
            if (Kind == "01,02,03")
            {
                ViewBag.ch1 = "";
                ViewBag.ch2 = "checked";
                ViewBag.ch3 = "checked";
                ViewBag.ch4 = "checked";
                ViewBag.Kinds = "重複/跨月/門市補傳";
            }
            if (Kind == "00,01,02,03")
            {
                ViewBag.ch1 = "checked";
                ViewBag.ch2 = "checked";
                ViewBag.ch3 = "checked";
                ViewBag.ch4 = "checked";
                ViewBag.Kinds = "一般/重複/跨月/門市補傳";
            }


            //DataTable dt = reportManager.Report0214T(start, end, merchantNo, Kind);
            ////return Report314Result(dt, ViewBag.MerchantName+"帳務差異明細表", "0214", start + "~" + end);

            DataTable dt = new DataTable();

            List<string> mNoList = new List<string>();
            if (group == "ALL")
            {
                mNoList = new GmMerchantTypeDAO().FindAll();

                foreach (string mNo in mNoList)
                { dt.Merge(reportManager.Report0214T(start, end, mNo, Kind)); }
            }
            else if (retail == "ALL" || bus == "ALL" || bike == "ALL" || track == "ALL" || parking == "ALL" || outsourcing == "ALL")
            {
                mNoList = new GmMerchantTypeDAO().FindGroupAll(group);
                foreach (string mNo in mNoList)
                { dt.Merge(reportManager.Report0214T(start, end, mNo, Kind)); }
            }
            else if (merchant != "")
            {
                dt = reportManager.Report0214T(start, end, merchant, Kind);
            }
            else { dt = new DataTable(); }

            string RepName = "帳務差異明細表";
            if (Request.Form["searchConfirm"] != null)
            { return View(dt); }
            else if (Request.Form["ExportExcel"] != null) //產生標籤Excel
            {

                //建立Excel 2003檔案
                IWorkbook wb = new HSSFWorkbook();
                ISheet ws;
                //ISheet ws2;

                ////建立Excel 2007檔案
                //IWorkbook wb = new XSSFWorkbook();
                //ISheet ws;
                ws = wb.CreateSheet(RepName);

                //var headerRow = ws.CreateRow(5);//第4行為欄位名稱


                HSSFCellStyle cs = (HSSFCellStyle)wb.CreateCellStyle();
                //啟動多行文字
                //cs.WrapText = true;
                //文字置中
                cs.VerticalAlignment = VerticalAlignment.Center;
                cs.Alignment = HorizontalAlignment.Center;
                //框線樣式及顏色
                cs.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
                cs.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                cs.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
                cs.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
                cs.BottomBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                cs.LeftBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                cs.RightBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                cs.TopBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                //背景顏色
                cs.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.PaleBlue.Index;
                cs.FillPattern = NPOI.SS.UserModel.FillPattern.SolidForeground;
                HSSFFont font1 = (HSSFFont)wb.CreateFont();
                //字體顏色
                font1.Color = NPOI.HSSF.Util.HSSFColor.Black.Index;
                //字體粗體
                //font1.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.Bold;
                //字體尺寸
                font1.FontHeightInPoints = 12;
                cs.SetFont(font1);
                cs.WrapText = true;

                HSSFFont font2 = (HSSFFont)wb.CreateFont();
                //字體顏色
                font2.Color = NPOI.HSSF.Util.HSSFColor.Black.Index;
                //字體粗體
                font2.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.Bold;
                //字體尺寸
                font2.FontHeightInPoints = 20;

                //headerRow.HeightInPoints = 20;
                //var cell = headerRow.CreateCell(0);

               
                var headerRow = ws.CreateRow(5);
                headerRow.HeightInPoints = 20;

                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    ICell cell = headerRow.CreateCell(i);
                    cell.CellStyle = cs;

                    //string[] cols = { "筆數", "金額" };
                    
                    cell.SetCellValue(dt.Columns[i].ColumnName);


                    ws.SetColumnWidth(0, 10 * 256);
                    ws.SetColumnWidth(1, 12 * 256);
                    ws.SetColumnWidth(2, 20 * 256);
                    ws.SetColumnWidth(3, 10 * 256);
                    ws.SetColumnWidth(4, 10 * 256);
                    ws.SetColumnWidth(5, 10 * 256);
                    ws.SetColumnWidth(6, 20 * 256);
                    ws.SetColumnWidth(7, 10 * 256);
                    ws.SetColumnWidth(8, 10 * 256);
                    ws.SetColumnWidth(9, 10 * 256);
                    ws.SetColumnWidth(10, 10 * 256);
                    ws.SetColumnWidth(11, 10 * 256);
                    ws.SetColumnWidth(12, 12 * 256);
                    ws.SetColumnWidth(13, 12 * 256);
                }

                ICellStyle style1 = wb.CreateCellStyle();
                //框線樣式及顏色
                style1.Alignment = HorizontalAlignment.Left;
                style1.VerticalAlignment = VerticalAlignment.Center;
                //字體尺寸
                font1.FontHeightInPoints = 11;
                style1.SetFont(font1);

                ICellStyle style2 = wb.CreateCellStyle();
                //框線樣式及顏色
                style2.Alignment = HorizontalAlignment.Center;
                style2.VerticalAlignment = VerticalAlignment.Center;
                //字體尺寸
                font1.FontHeightInPoints = 11;
                //字體粗體
                style2.SetFont(font2);
                style2.WrapText = true; //自動換列

                //fill content 
                ICellStyle style3 = wb.CreateCellStyle();
                //框線樣式及顏色
                style3.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
                style3.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                style3.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
                style3.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
                style3.BottomBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                style3.LeftBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                style3.RightBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                style3.TopBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                style3.Alignment = HorizontalAlignment.Center;
                style3.VerticalAlignment = VerticalAlignment.Center;
                //字體尺寸
                font1.FontHeightInPoints = 11;
                style3.SetFont(font1);
                style3.WrapText = true;
                //style3.FillForegroundColor = IndexedColors.LemonChiffon.Index;
                //style3.FillPattern = FillPattern.SolidForeground;
                //style3.FillBackgroundColor = IndexedColors.LightGreen.Index;

                // Create the style object
                var currencyCellStyle = wb.CreateCellStyle();
                // Right-align currency values
                currencyCellStyle.Alignment = HorizontalAlignment.Right;
                currencyCellStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
                currencyCellStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                currencyCellStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
                currencyCellStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
                currencyCellStyle.BottomBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                currencyCellStyle.LeftBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                currencyCellStyle.RightBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                currencyCellStyle.TopBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                font1.FontHeightInPoints = 11;
                currencyCellStyle.SetFont(font1);
                // Get / create the data format string
                //var formatId = HSSFDataFormat.GetBuiltinFormat("#,##0");
                //if (formatId == -1)
                //{
                //    var newDataFormat = wb.CreateDataFormat();
                //    currencyCellStyle.DataFormat = newDataFormat.GetFormat("#,##0");
                //}
                //else
                //currencyCellStyle.DataFormat = formatId;
                //HSSFDataFormat myformat = wb.CreateDataFormat();
                //ICellStyle newDataFormat = wb.CreateCellStyle();
                currencyCellStyle.DataFormat = wb.CreateDataFormat().GetFormat("#,##0");

                {
                    var rang = ws.CreateRow(3);
                    ICell cell3 = rang.CreateCell(0);
                    cell3.CellStyle = style1;
                    cell3.SetCellValue("會計日：" + ViewBag.Start + "~" + ViewBag.End);

                    rang = ws.CreateRow(4);
                    cell3 = rang.CreateCell(0);
                    cell3.SetCellValue("差異類型：" + ViewBag.Kinds);
                    cell3.CellStyle = style1;

                    cell3 = ws.CreateRow(0).CreateCell(0);
                    cell3.SetCellValue(ViewBag.MerchantName+" "+RepName);
                    ws.AddMergedRegion(new CellRangeAddress(0, 0, 0, 12));
                    cell3.CellStyle = style2;

                    rang = ws.CreateRow(2);
                    cell3 = rang.CreateCell(0);
                    cell3.CellStyle = style1;
                    cell3.SetCellValue("");

                    rang = ws.CreateRow(2);
                    cell3 = rang.CreateCell(0);
                    cell3.CellStyle = style1;
                    cell3.SetCellValue("列印日：" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                    //ws.AddMergedRegion(new CellRangeAddress(2, 2, 0, 2));
                }

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ws.CreateRow(i + 6).HeightInPoints = 13;

                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        ICell innerCell = ws.GetRow(i + 6).CreateCell(j);
                        switch (j)
                        {
                            case 9:
                            case 10:
                                innerCell.SetCellValue(Convert.ToDouble(dt.Rows[i][j]));
                                innerCell.CellStyle = currencyCellStyle;
                                break;
                               
                            default:
                                innerCell.SetCellValue(dt.Rows[i][j].ToString());
                                innerCell.CellStyle = style3;
                                break;
                        }
                        //ws.AutoSizeColumn(i);
                    }
                }

                var stream = new MemoryStream();
                wb.Write(stream);
                stream.Close();

                return File(stream.ToArray(), "application/vnd.ms-excel", RepName + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls");

            }
            else { return View(new DataTable()); }

        }


        public ActionResult Report0311Result()
        {
            //int year = DateTime.Today.Year;
            //int month = DateTime.Today.Month;
            DateTime startDate = DateTime.Now;
            DateTime endDate = DateTime.Now;

            string start = startDate.ToString("yyyyMMdd");
            string end = endDate.ToString("yyyyMMdd");
            string merchantNo = "";
            if (Request.Form["searchConfirm"] != null || Request.Form["ExportExcel"] != null)
            {
                start = Request.Form["startDate"];
                end = Request.Form["endDate"] ;
                merchantNo = Request.Form["Merchant"];
            }
            ViewBag.Start = start;
            ViewBag.End = end;
            var melist = SetMerchantBankDropDown(merchantNo);
            melist.Add(new SelectListItem() { Text = "愛金卡", Value = "ICP", Selected = false });
            //ViewBag.Merchant = SetMerchantBankDropDown(merchantNo);
            ViewBag.Merchant = melist;
            DataTable dt = reportManager.Report0311T(start, end, merchantNo);
            ViewBag.Count = dt.Rows.Count;
            //return View(dt);

            if (Request.Form["ExportExcel"] != null) //產生標籤Excel
            {
                //建立Excel 2003檔案
                IWorkbook wb = new HSSFWorkbook();
                ISheet ws;
                //ISheet ws2;

                ////建立Excel 2007檔案
                //IWorkbook wb = new XSSFWorkbook();
                //ISheet ws;
                ws = wb.CreateSheet("各銀行聯名卡交易統計表");

                //ws.CreateRow(0);//第一行為欄位名稱
                var headerRow = ws.CreateRow(2);

                HSSFCellStyle cs = (HSSFCellStyle)wb.CreateCellStyle();
                //啟動多行文字
                //cs.WrapText = true;
                //文字置中
                cs.VerticalAlignment = VerticalAlignment.Center;
                cs.Alignment = HorizontalAlignment.Center;
                //框線樣式及顏色
                cs.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
                cs.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                cs.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
                cs.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
                cs.BottomBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                cs.LeftBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                cs.RightBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                cs.TopBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                //背景顏色
                cs.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.Grey25Percent.Index;
                cs.FillPattern = NPOI.SS.UserModel.FillPattern.SolidForeground;
                HSSFFont font1 = (HSSFFont)wb.CreateFont();
                HSSFFont font2 = (HSSFFont)wb.CreateFont();
                //字體顏色
                font1.Color = NPOI.HSSF.Util.HSSFColor.Black.Index;
                font2.Color = NPOI.HSSF.Util.HSSFColor.Black.Index;
                //字體粗體
                //font1.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.Bold;
                font2.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.Bold;

                //字體尺寸
                font1.FontHeightInPoints = 12;
                font2.FontHeightInPoints = 12;
                cs.SetFont(font1);

                headerRow.HeightInPoints = 24;

                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    var cell = headerRow.CreateCell(i);
                    if (i==1)
                        cell.SetCellValue("清分日期");
                    else
                        cell.SetCellValue(dt.Columns[i].ColumnName);
                    cell.CellStyle = cs;
                    //ws.AutoSizeColumn(i);
                    if (i == 0)
                        ws.SetColumnWidth(0, 20 * 256);
                    else if (i == 1)
                        ws.SetColumnWidth(1, 12 * 256);
                    else if (i == 2)
                        ws.SetColumnWidth(i, 0 * 256);
                    else
                        ws.SetColumnWidth(i, 20 * 256);
                }

                ICellStyle style1 = wb.CreateCellStyle();
                //框線樣式及顏色
                style1.Alignment = HorizontalAlignment.Left;
                style1.VerticalAlignment = VerticalAlignment.Center;
                //字體尺寸
                font1.FontHeightInPoints = 11;
                style1.SetFont(font1);

                //fill content 
                ICellStyle style3 = wb.CreateCellStyle();
                //框線樣式及顏色
                style3.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
                style3.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                style3.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
                style3.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
                style3.BottomBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                style3.LeftBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                style3.RightBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                style3.TopBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                style3.Alignment = HorizontalAlignment.Center;
                style3.VerticalAlignment = VerticalAlignment.Center;
                //字體尺寸
                font1.FontHeightInPoints = 11;
                style3.SetFont(font1);
                //style3.FillForegroundColor = IndexedColors.LemonChiffon.Index;
                //style3.FillPattern = FillPattern.SolidForeground;
                //style3.FillBackgroundColor = IndexedColors.LightGreen.Index;

                ICellStyle style2 = wb.CreateCellStyle();
                //框線樣式及顏色
                style2.Alignment = HorizontalAlignment.Center;
                style2.VerticalAlignment = VerticalAlignment.Center;
                //字體尺寸
                font1.FontHeightInPoints = 20;
                style2.SetFont(font1);

                // Create the style object
                var currencyCellStyle = wb.CreateCellStyle();
                // Right-align currency values
                currencyCellStyle.Alignment = HorizontalAlignment.Right;
                currencyCellStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
                currencyCellStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                currencyCellStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
                currencyCellStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
                currencyCellStyle.BottomBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                currencyCellStyle.LeftBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                currencyCellStyle.RightBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                currencyCellStyle.TopBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                font1.FontHeightInPoints = 11;
                currencyCellStyle.SetFont(font1);
                // Get / create the data format string
                var formatId = HSSFDataFormat.GetBuiltinFormat("#,##0");
                if (formatId == -1)
                {
                    var newDataFormat = wb.CreateDataFormat();
                    currencyCellStyle.DataFormat = newDataFormat.GetFormat("#,##0");
                }
                else
                    currencyCellStyle.DataFormat = formatId;

                var currencyCellStyle4 = wb.CreateCellStyle();
                currencyCellStyle4.Alignment = HorizontalAlignment.Right;
                currencyCellStyle4.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
                currencyCellStyle4.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                currencyCellStyle4.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
                currencyCellStyle4.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
                currencyCellStyle4.BottomBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                currencyCellStyle4.LeftBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                currencyCellStyle4.RightBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                currencyCellStyle4.TopBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                //font2.Boldweight = (short)FontBoldWeight.Bold;
                currencyCellStyle4.SetFont(font2);
                if (formatId == -1)
                {
                    var newDataFormat = wb.CreateDataFormat();
                    currencyCellStyle4.DataFormat = newDataFormat.GetFormat("#,##0;[Red](#,##0)");
                }
                else
                    currencyCellStyle4.DataFormat = formatId;


                ICellStyle style4 = wb.CreateCellStyle();
                style4.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
                style4.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                style4.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
                style4.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
                style4.BottomBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                style4.LeftBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                style4.RightBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                style4.TopBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                style4.Alignment = HorizontalAlignment.Center;
                style4.VerticalAlignment = VerticalAlignment.Center;
                style4.SetFont(font2);

                ws.CreateRow(0).HeightInPoints = 20; ;
                var rang = ws.CreateRow(1);
                var cell3 = rang.CreateCell(0);
                cell3.SetCellValue("清算期間： " + ViewBag.Start + "~" + ViewBag.End);
                cell3.CellStyle = style1;
                //rang = ws.CreateRow(2);
                //cell3 = rang.CreateCell(0);
                //cell3.CellStyle = style1;
                //cell3.SetCellValue("機構名稱：" + ViewBag.MerchantBankName);
                    cell3 = ws.GetRow(0).CreateCell(0);
                    cell3.SetCellValue("各銀行聯名卡交易統計表");
                    ws.AddMergedRegion(new CellRangeAddress(0, 0, 0, 14));
                    cell3.CellStyle = style2;

                int ii = 2;
                Double[] total = new Double[14];
                Double[] totalp = new Double[14];//小計
                Double[] totals = new Double[14];//總計
                string temp = ""; //
                string temp2 = ""; //明細用


                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (temp != dt.Rows[i][0].ToString() && temp != "")
                    {
                        for (int j = 3; j < dt.Columns.Count-1 ; j++)
                        {
                            totalp[j] = total[j]; //前一家小計
                            total[j] = 0;
                        }
                    }

                    for (int j = 0; j < dt.Columns.Count-1 ; j++)
                    {
                        if (j >= 3 && j <= 13) total[j] = total[j] + Convert.ToDouble(dt.Rows[i][j].ToString());
                    }

                    if (temp != dt.Rows[i][0].ToString()) //第二家之後
                    {

                        if (temp == "")
                        {
                            temp = dt.Rows[i][0].ToString();
                        }
                        else
                        {
                            if (1==1)
                            {
                                //先算小計, 再填明細, 再填小計
                                //填明細
                                ws.SetColumnWidth(1, 30 * 256); //name
                                for (int i2 = 0; i2 < dt.Rows.Count; i2++)
                                {
                                    if (temp2 == dt.Rows[i2][0].ToString() || temp2 == "")
                                    {
                                        ii = ii + 1;
                                        ws.CreateRow(ii);
                                        var cell3d = ws.GetRow(ii).CreateCell(0);
                                        temp2 = dt.Rows[i2][0].ToString();
                                        for (int j = 0; j < dt.Columns.Count ; j++)
                                        {
                                            cell3d = ws.GetRow(ii).CreateCell(j);
                                            if (j >= 3 && j<=12)
                                                cell3d.SetCellValue(Convert.ToDouble(dt.Rows[i2][j].ToString()));
                                            else
                                                cell3d.SetCellValue(dt.Rows[i2][j].ToString());
                                            cell3d.CellStyle = currencyCellStyle;
                                        }
                                    }
                                    else
                                    {
                                        //if (dt.Rows[i2][0].ToString() == temp)
                                        if (i2>=i)
                                        {
                                            temp2 = dt.Rows[i2][0].ToString();
                                            i2 = dt.Rows.Count;
                                            break;
                                        }
                                    }
                                }
                            }


                            ii = ii + 1;
                            ws.CreateRow(ii);
                            var cell2 = ws.GetRow(ii).CreateCell(1);
                            cell2.SetCellValue("小計");
                            cell2.CellStyle = style4;
                            //小計
                            cell3 = ws.GetRow(ii).CreateCell(0);
                            cell3.SetCellValue(temp); //銀行
                            cell3.CellStyle = style4;
                            cell3 = ws.GetRow(ii).CreateCell(2);
                            if (merchantNo != "") cell3.SetCellValue(dt.Rows[0][1].ToString());
                            cell3.CellStyle = style1;
                            temp = dt.Rows[i][0].ToString();
                            if (dt.Rows.Count > 0)
                            {
                                for (int j = 3; j < dt.Columns.Count -1 ; j++)
                                {
                                    cell3 = ws.GetRow(ii).CreateCell(j);
                                    cell3.SetCellValue(totalp[j]);
                                    cell3.CellStyle = currencyCellStyle4;
                                    totals[j] = totals[j] + totalp[j];
                                }
                            }

                        }

                    }

                }

                    //最後一筆明細
                    ws.SetColumnWidth(1, 30 * 256); //name
                    ws.SetColumnWidth(2, 0 * 256); //name

                    for (int i2 = 0; i2 < dt.Rows.Count; i2++)
                    {
                        if (temp2 == dt.Rows[i2 ][0].ToString() || temp2=="")
                        {
                            ii = ii + 1;
                            ws.CreateRow(ii);
                            var cell3d = ws.GetRow(ii).CreateCell(0);
                            temp2 = dt.Rows[i2][0].ToString();
                            for (int j = 0; j < dt.Columns.Count ; j++)
                            {
                                cell3d = ws.GetRow(ii).CreateCell(j);
                                if (j>=3 && j<=12)
                                    cell3d.SetCellValue(Convert.ToDouble(dt.Rows[i2][j].ToString()));
                                else
                                    cell3d.SetCellValue(dt.Rows[i2][j].ToString());
                                cell3d.CellStyle = currencyCellStyle;
                            }
                        }
                        else
                        {
                            //if (Convert.ToDouble(dt.Rows[i2][0].ToString()) > Convert.ToDouble(temp))
                            //if (i2 > ii)
                            //{
                            //    temp2 = dt.Rows[i2][0].ToString();
                            //    i2 = dt.Rows.Count;
                            //}
                        }
                    }

                ii = ii + 1;
                ws.CreateRow(ii);
                var cell2n = ws.GetRow(ii).CreateCell(1);
                cell2n.SetCellValue("小計");
                cell2n.CellStyle = style4;
                //小計
                var cell3n = ws.GetRow(ii).CreateCell(0);
                cell3n.SetCellValue(temp); //date
                cell3n.CellStyle = style4;
                cell3n = ws.GetRow(ii).CreateCell(2);
                if (merchantNo != "") cell3n.SetCellValue(dt.Rows[0][1].ToString());
                cell3n.CellStyle = style1;
                
                if (dt.Rows.Count > 0)
                {
                    for (int j = 3; j < dt.Columns.Count -1; j++)
                    {
                        cell3n = ws.GetRow(ii).CreateCell(j);
                        cell3n.SetCellValue(total[j]);
                        cell3n.CellStyle = currencyCellStyle4;
                        totals[j] = totals[j] + total[j];
                    }
                }

                ii = ii + 1;
                ws.CreateRow(ii);
                cell2n = ws.GetRow(ii).CreateCell(1);
                cell2n.CellStyle = style3;
                //總計
                cell3n = ws.GetRow(ii).CreateCell(0);
                cell3n.SetCellValue("總計");
                cell3n.CellStyle = style3;
                cell3n = ws.GetRow(ii).CreateCell(2);
                if (merchantNo != "") cell3n.SetCellValue(dt.Rows[0][1].ToString());
                cell3n.CellStyle = style1;

                if (dt.Rows.Count > 0)
                {
                    for (int j = 3; j < dt.Columns.Count -1; j++)
                    {
                        cell3n = ws.GetRow(ii).CreateCell(j);
                        cell3n.SetCellValue(totals[j]);
                        cell3n.CellStyle = currencyCellStyle;
                    }
                }


                var stream = new MemoryStream();
                wb.Write(stream);
                stream.Close();

                return File(stream.ToArray(), "application/vnd.ms-excel", "各銀行聯名卡交易統計表_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls");


            }
            else
            {
                return View(dt);
            }

        }

        public ActionResult Report0312Result()
        {
            //int year = DateTime.Today.Year;
            //int month = DateTime.Today.Month;
            DateTime startDate = DateTime.Now;
            DateTime endDate = DateTime.Now;

            string start = startDate.ToString("yyyyMMdd");
            string end = endDate.ToString("yyyyMMdd");
            //string merchantNo = "";
            string merchantBankNo = "";

            string merchant = "";

            string group = "";
            string item = "";
            string retail = "";
            string bus = "";
            string bike = "";
            string track = "";
            string parking = "";
            string outsourcing = "";

            if (Request.Form["searchConfirm"] != null || Request.Form["ExportExcel"] != null)
            {
                start = Request.Form["startDate"];
                end = Request.Form["endDate"];
                //merchantNo = Request.Form["Merchant"];
                merchantBankNo = Request.Form["MerchantBank"];

                group = Request.Form["group"];
                item = Request.Form["items"];
                //retail = Request.Form["retail"];
                //bus = Request.Form["bus"];
                //bike = Request.Form["bike"];
                //track = Request.Form["track"];
                //parking = Request.Form["parking"];
                //outsourcing = Request.Form["outsourcing"];

                //if (group == "PARKING_LOT")
                //{ merchant = Request.Form["parking"]; }
                //else if (group == "BANK_OUTSOURCING")
                //{ merchant = Request.Form["outsourcing"]; }
                //else
                //{ merchant = Request.Form[group.ToLower()]; }
                merchant = Request.Form["items"];

            }
            ViewBag.Start = start;
            ViewBag.End = end;
            ViewBag.RepName = "特約機構自動加值統計表";
            ViewBag.RepID = "0312";
            //ViewBag.Merchant = SetMerchantDropDown(merchantNo);
            ViewBag.MerchantBank = SetMerchantBankDropDown(merchantBankNo);

            ViewBag.MERCHANT = SetGroup(group);
            ViewBag.MERCHANT_NAME = group;
            ViewBag.ITEM = item;
            //ViewBag.RETAIL = SetRetail(retail); ViewBag.retails = retail;
            //ViewBag.BUS = SetBus(bus); ViewBag.buses = bus;
            //ViewBag.BIKE = SetBike(bike); ViewBag.bikes = bike;
            //ViewBag.TRACK = SetTrack(track); ViewBag.tracks = track;
            //ViewBag.PARKING = SetParking(parking); ViewBag.parkings = parking;
            //ViewBag.OUTSOURCING = SetOutsourcing(outsourcing); ViewBag.outsourcings = outsourcing;



            //DataTable dt = reportManager.Report0312T(start, end, merchantNo, merchantBankNo);
            DataTable dt = new DataTable();

            List<string> mNoList = new List<string>();
            if (group == "ALL")
            {
                mNoList = new GmMerchantTypeDAO().FindAll();

                foreach (string mNo in mNoList)
                { dt.Merge(reportManager.Report0312T(start, end, mNo, merchantBankNo)); }
            }
            else if (item == "ALL")
            //else if (retail == "ALL" || bus == "ALL" || bike == "ALL" || track == "ALL" || parking == "ALL" || outsourcing == "ALL")
            {
                mNoList = new GmMerchantTypeDAO().FindGroupAll(group);
                foreach (string mNo in mNoList)
                { dt.Merge(reportManager.Report0312T(start, end, mNo, merchantBankNo)); }
            }
            else if (merchant != "")
            {
                dt = reportManager.Report0312T(start, end, merchant, merchantBankNo);
            }
            else { dt = new DataTable(); }
            return Report312Result(dt, "特約機構自動加值統計表", "", "0312");

        }

        public ActionResult Report0313Result()
        {
            //int year = DateTime.Today.Year;
            //int month = DateTime.Today.Month;
            DateTime startDate = DateTime.Now;
            DateTime endDate = DateTime.Now;

            string start = startDate.ToString("yyyyMMdd");
            string end = endDate.ToString("yyyyMMdd");
            if (Request.Form["searchConfirm"] != null || Request.Form["ExportExcel"] != null)
            {
                start = Request.Form["startDate"];
                end = Request.Form["endDate"];
            }
            ViewBag.Start = start;
            ViewBag.End = end;
            ViewBag.RepName="掛失補發明細表";
            ViewBag.RepID = "0313";
            //AMS.Models.AmUsersModel model = new AMS.Models.AmUsersModel();
            //ViewBag.Username = DateTime.Now.ToString("mmssfff");
            ViewBag.Username = User.Identity.GetUserName();
            DataTable dt = reportManager.Report0313T(start, end);
            if (dt.Rows.Count==0)
            {
                //dt = reportManager.Report0313SPT(start, end);
            }
            return Report312Result(dt, "掛失補發明細表", "", "0313");

        }

        public ActionResult Report312Result(DataTable dt, string RepName, string merchantNo, string RepID)
        {
            ViewBag.Count = dt.Rows.Count;
            //return View(dt);
            if (Request.Form["ExportExcel"] != null ) //產生標籤Excel
            {
                //建立Excel 2003檔案
                IWorkbook wb = new HSSFWorkbook();
                ISheet ws;
                //ISheet ws2;

                ////建立Excel 2007檔案
                //IWorkbook wb = new XSSFWorkbook();
                //ISheet ws;
                ws = wb.CreateSheet(RepName);

                //ws.CreateRow(3);//第一行為欄位名稱
                var headerRow = ws.CreateRow(2);
                if (RepName == "掛失補發明細表")
                {
                    headerRow = ws.CreateRow(3);
                }

                HSSFCellStyle cs = (HSSFCellStyle)wb.CreateCellStyle();
                //啟動多行文字
                //cs.WrapText = true;
                //文字置中
                cs.VerticalAlignment = VerticalAlignment.Center;
                cs.Alignment = HorizontalAlignment.Center;
                //框線樣式及顏色
                cs.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
                cs.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                cs.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
                cs.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
                cs.BottomBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                cs.LeftBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                cs.RightBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                cs.TopBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                //背景顏色
                cs.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.Grey25Percent.Index;
                cs.FillPattern = NPOI.SS.UserModel.FillPattern.SolidForeground;
                HSSFFont font1 = (HSSFFont)wb.CreateFont();
                //字體顏色
                font1.Color = NPOI.HSSF.Util.HSSFColor.Black.Index;
                //字體粗體
                //font1.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.Bold;
                //字體尺寸
                font1.FontHeightInPoints = 12;
                cs.SetFont(font1);

                headerRow.HeightInPoints = 24;

                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    var cell = headerRow.CreateCell(i);
                    cell.SetCellValue(dt.Columns[i].ColumnName);
                    cell.CellStyle = cs;
                    //ws.AutoSizeColumn(i);
                    if (i == 0)
                        ws.SetColumnWidth(0, 12 * 256);
                    else if (i == 1 )
                        ws.SetColumnWidth(i, 12 * 256);
                    else if (i == 2 || i == 3)
                        ws.SetColumnWidth(i, 28 * 256);
                    else
                        ws.SetColumnWidth(i, 20 * 256);
                }
                if (RepName == "掛失補發明細表")
                {
                    ws.SetColumnWidth(1, 12 * 256);
                    ws.SetColumnWidth(2, 20 * 256);
                    ws.SetColumnWidth(3, 12 * 256);
                }
                if (RepName == "儲值餘額表重複資料")
                {
                    ws.SetColumnWidth(0, 18 * 256);
                    ws.SetColumnWidth(2, 12 * 256);
                    ws.SetColumnWidth(3, 12 * 256);
                    ws.SetColumnWidth(4, 12 * 256);
                    ws.SetColumnWidth(6, 12 * 256);
                    ws.SetColumnWidth(7, 42 * 256);
                    ws.SetColumnWidth(8, 12 * 256);
                    ws.SetColumnWidth(9, 12 * 256);
                    ws.SetColumnWidth(10, 12 * 256);
                }

                ICellStyle style1 = wb.CreateCellStyle();
                //框線樣式及顏色
                style1.Alignment = HorizontalAlignment.Left;
                style1.VerticalAlignment = VerticalAlignment.Center;
                //字體尺寸
                font1.FontHeightInPoints = 11;
                style1.SetFont(font1);

                //fill content 
                ICellStyle style3 = wb.CreateCellStyle();
                //框線樣式及顏色
                style3.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
                style3.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                style3.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
                style3.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
                style3.BottomBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                style3.LeftBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                style3.RightBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                style3.TopBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                style3.Alignment = HorizontalAlignment.Center;
                style3.VerticalAlignment = VerticalAlignment.Center;
                //字體尺寸
                font1.FontHeightInPoints = 11;
                style3.SetFont(font1);
                //style3.FillForegroundColor = IndexedColors.LemonChiffon.Index;
                //style3.FillPattern = FillPattern.SolidForeground;
                //style3.FillBackgroundColor = IndexedColors.LightGreen.Index;

                ICellStyle style2 = wb.CreateCellStyle();
                //框線樣式及顏色
                style2.Alignment = HorizontalAlignment.Center;
                style2.VerticalAlignment = VerticalAlignment.Center;

                //字體尺寸
                font1.FontHeightInPoints = 20;
                style2.SetFont(font1);

                // Create the style object
                var currencyCellStyle = wb.CreateCellStyle();
                // Right-align currency values
                currencyCellStyle.Alignment = HorizontalAlignment.Right;
                currencyCellStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
                currencyCellStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                currencyCellStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
                currencyCellStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
                currencyCellStyle.BottomBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                currencyCellStyle.LeftBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                currencyCellStyle.RightBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                currencyCellStyle.TopBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                font1.FontHeightInPoints = 11;
                currencyCellStyle.SetFont(font1);
                // Get / create the data format string
                var formatId = HSSFDataFormat.GetBuiltinFormat("#,##0");
                if (formatId == -1)
                {
                    var newDataFormat = wb.CreateDataFormat();
                    currencyCellStyle.DataFormat = newDataFormat.GetFormat("#,##0");
                }
                else
                    currencyCellStyle.DataFormat = formatId;

                ws.CreateRow(0).HeightInPoints = 20; ;
                var rang = ws.CreateRow(1);
                if (RepName == "掛失補發明細表")
                {
                    rang = ws.CreateRow(2);
                }
                var cell3 = rang.CreateCell(0);
                if (RepID == "0212")
                {
                    cell3.SetCellValue("愛金卡儲值餘額表_" + ViewBag.Start);
                    ws.AddMergedRegion(new CellRangeAddress(1, 1, 0, 1));
                }
                else
                    cell3.SetCellValue("清算期間： " + ViewBag.Start +"~" + ViewBag.End);
                cell3.CellStyle = style1;
                //rang = ws.CreateRow(2);
                //cell3 = rang.CreateCell(0);
                //cell3.CellStyle = style1;
                //cell3.SetCellValue("機構名稱：" + ViewBag.MerchantName);
                if (RepID == "0312")
                {
                    cell3 = ws.GetRow(0).CreateCell(0);
                    cell3.SetCellValue("特約機構自動加值統計表");
                    ws.AddMergedRegion(new CellRangeAddress(0, 0, 0, 10));
                }
                if (RepID == "0313")
                {
                    cell3 = ws.GetRow(0).CreateCell(0);
                    cell3.SetCellValue("愛金卡股份有限公司");
                    cell3.CellStyle = style2;
                    ws.AddMergedRegion(new CellRangeAddress(0, 0, 0, 8));
                    
                    cell3 = ws.GetRow(1).CreateCell(0);
                    cell3.SetCellValue("掛失補發明細表");
                    ws.AddMergedRegion(new CellRangeAddress(1, 1, 0, 8));
                }
                cell3.CellStyle = style2;

                Double[] total = new Double[11];
                int ii = 2;
                string temp2 = "";
                if (RepName == "掛失補發明細表")
                {
                    ii=3;
                }
                for (int i = 0; i < dt.Rows.Count; i++)
                {

                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        if (RepName == "掛失補發明細表")
                        {
                                if (j >= 5 && j<=8) total[j] = total[j] + Convert.ToDouble(dt.Rows[i][j].ToString());
                        }
                        else
                        {
                            if (RepName != "儲值餘額表重複資料" && j > 2)
                            {
                                if (j >= 5) total[j] = total[j] + Convert.ToDouble(dt.Rows[i][j].ToString());
                            }

                        }
                    }

                    //明細
                        ii = ii + 1;
                        ws.CreateRow(ii);
                        var cell3d = ws.GetRow(ii).CreateCell(0);
                        temp2 = dt.Rows[i][0].ToString();
                        for (int j = 0; j < dt.Columns.Count; j++)
                        {
                            cell3d = ws.GetRow(ii).CreateCell(j);
                            if (j >= 5 && RepName != "掛失補發明細表" && RepName != "儲值餘額表重複資料")
                                cell3d.SetCellValue(Convert.ToDouble(dt.Rows[i][j].ToString()));
                            else
                                cell3d.SetCellValue(dt.Rows[i][j].ToString());

                            if (j == 8 && RepName == "儲值餘額表重複資料" && dt.Rows[i][j].ToString()!="")
                                cell3d.SetCellValue(Convert.ToDouble(dt.Rows[i][j].ToString()));
                            cell3d.CellStyle = currencyCellStyle;
                        }


                }
                if (RepName != "儲值餘額表重複資料")
                {

                ii = ii + 1;
                ws.CreateRow(ii);
                var cell2n = ws.GetRow(ii).CreateCell(1);
                cell2n.CellStyle = style3;
                //總計
                var cell3n = ws.GetRow(ii).CreateCell(0);
                cell3n.SetCellValue("總計");
                cell3n.CellStyle = style3;
                cell3n = ws.GetRow(ii).CreateCell(2);
                if (merchantNo != "") cell3n.SetCellValue(dt.Rows[0][1].ToString());
                cell3n.CellStyle = style3;

                if (dt.Rows.Count > 0)
                {
                    if (RepName == "掛失補發明細表")
                    {
                        cell3n = ws.GetRow(ii).CreateCell(3);
                        cell3n.CellStyle = currencyCellStyle;
                        cell3n = ws.GetRow(ii).CreateCell(4);
                        cell3n.CellStyle = currencyCellStyle;
                        for (int j = 5; j < dt.Columns.Count; j++)
                        {
                            cell3n = ws.GetRow(ii).CreateCell(j);
                            cell3n.SetCellValue(total[j]);
                            cell3n.CellStyle = currencyCellStyle;
                        }
                    }
                    else
                    {
                        cell3n = ws.GetRow(ii).CreateCell(3);
                        cell3n.CellStyle = currencyCellStyle;
                        cell3n = ws.GetRow(ii).CreateCell(4);
                        cell3n.CellStyle = currencyCellStyle;
                        for (int j = 5; j < dt.Columns.Count; j++)
                        {
                            cell3n = ws.GetRow(ii).CreateCell(j);
                            cell3n.SetCellValue(total[j]);
                            cell3n.CellStyle = currencyCellStyle;
                        }

                    }
                    
                }
                }

                //if (RepName == "儲值餘額表重複資料")
                //{
                //    //第2頁籤
                //    ws = wb.CreateSheet("工作表");
                //    DataTable dt2 = reportManager.Report0212_2T(ViewBag.Start);
                //    headerRow = ws.CreateRow(0);

                //    for (int i = 0; i < dt2.Columns.Count; i++)
                //    {
                //        var cell = headerRow.CreateCell(i);
                //        cell.SetCellValue(dt2.Columns[i].ColumnName);
                //        cell.CellStyle = cs;
                //    }

                //    for (int i = 0; i < dt2.Rows.Count; i++)
                //    {
                //        ws.CreateRow(i + 1);
                //        for (int j = 0; j < dt2.Columns.Count; j++)
                //        {
                //            var cell2 = ws.GetRow(i + 1).CreateCell(j);
                //            if (j > 1)
                //            {
                //                cell2.SetCellValue(dt2.Rows[i][j].ToString());
                //                cell2.CellStyle = currencyCellStyle;
                //            }
                //            else
                //            {
                //                cell2.SetCellValue(dt2.Rows[i][j].ToString());
                //                cell2.CellStyle = style3;
                //            }


                //        }
                //    }
                //}
                var stream = new MemoryStream();
                wb.Write(stream);
                stream.Close();

                return File(stream.ToArray(), "application/vnd.ms-excel", ViewBag.RepName +"_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls");


            }
            else
            {
                return View(dt);
            }

        }

        public ActionResult Report0314Result()
        {
            int year = DateTime.Today.Year;
            int month = DateTime.Today.Month;
            int day = DateTime.Today.Day;
            DateTime startDate = new DateTime(year, month, day);
            string start = startDate.ToString("yyyyMMdd");
            string merchantNo = "";
            string MERC_GROUP = "BNK000";
            if (Request.Form["searchConfirm"] != null || Request.Form["ExportExcel"] != null)
            {
                start = Request.Form["startDate"];
                merchantNo = Request.Form["Merchant"];
            }
            ViewBag.Start = start;
            ViewBag.Merchant = SetMerchantBankDropDown(merchantNo);
            ViewBag.MerchantBankName = "";

            DataTable dtn = reportManager.MerchantName(merchantNo);
            if (dtn.Rows.Count > 0)
            {
                ViewBag.MerchantBankName = dtn.Rows[0][1].ToString();
                MERC_GROUP = "BNK" + dtn.Rows[0][2].ToString();
            }
            else
            {
                if (Request.Form["startDate"] != null)
                {
                    //Response.Write("<script>alert('');</script>"); return View();
                    //DisplayMessage();
                }
            }

            DataTable dt = reportManager.Report0314T(start, merchantNo, MERC_GROUP);
            return Report314Result(dt, "每日自動加值請款報表", "ALDS", "");

        }

        public ActionResult Report0320Result()
        {
            int year = DateTime.Today.Year;
            int month = DateTime.Today.Month;
            int day = DateTime.Today.Day;
            DateTime startDate = new DateTime(year, month, 1);
            DateTime endDate = new DateTime(year, month,
                                    DateTime.DaysInMonth(year, month));
            string start = startDate.ToString("yyyyMM");
            string end = endDate.ToString("yyyyMM");

            string merchantNo = "";
            if (Request.Form["searchConfirm"] != null || Request.Form["ExportExcel"] != null)
            {
                start = Request.Form["startDate"];
                end = start;
                merchantNo = Request.Form["Merchant"];
            }
            ViewBag.Start = start;
            //ViewBag.End = end;
            string MERC_GROUP = "BNK000";
            ViewBag.Merchant = SetMerchantBankDropDown(merchantNo);
            ViewBag.MerchantBankName = "";

            DataTable dtn = reportManager.MerchantName(merchantNo);
            if (dtn.Rows.Count > 0)
            {
                ViewBag.MerchantBankName = dtn.Rows[0][1].ToString();
                MERC_GROUP = "BNK" + dtn.Rows[0][2].ToString();
            }
            else
            {
                if (Request.Form["startDate"] != null)
                {
                    //Response.Write("<script>alert('');</script>"); return View();
                    //DisplayMessage();
                }
            }

            DataTable dt = reportManager.Report0320T(start, end, merchantNo, MERC_GROUP);

            start = start + "01";
            DateTime startD = DateTime.Parse(start.Substring(0, 4) + "-" + start.Substring(4, 2) + "-" + start.Substring(6, 2)).AddMonths(-2);
            year = startD.Year;
            month = startD.Month;
            startD = new DateTime(year, month, 25);
            DateTime EndD = DateTime.Parse(start.Substring(0, 4) + "-" + start.Substring(4, 2) + "-" + start.Substring(6, 2)).AddMonths(-1);
            year = EndD.Year;
            month = EndD.Month;
            EndD = new DateTime(year, month, 24);
            ViewBag.Rang = startD.ToString("yyyyMMdd") + "~" + EndD.ToString("yyyyMMdd");

            return Report314Result(dt, "自動加值首次啟用月報表", "FALS", ViewBag.Rang);

        }

        public ActionResult Report0321Result()
        {
            int year = DateTime.Today.Year;
            int month = DateTime.Today.Month;
            int day = DateTime.Today.Day;
            DateTime startDate = new DateTime(year, month, day);
            string start = startDate.ToString("yyyyMMdd");
            string merchantNo = "";
            string MERC_GROUP = "BNK000";
            if (Request.Form["searchConfirm"] != null || Request.Form["ExportExcel"] != null)
            {
                start = Request.Form["startDate"];
                merchantNo = Request.Form["Merchant"];
            }
            ViewBag.Start = start;
            ViewBag.Merchant = SetMerchantBankDropDown(merchantNo);
            ViewBag.MerchantBankName = "";

            DataTable dtn = reportManager.MerchantName(merchantNo);
            if (dtn.Rows.Count > 0)
            {
                ViewBag.MerchantBankName = dtn.Rows[0][1].ToString();
                MERC_GROUP = "BNK" + dtn.Rows[0][2].ToString();
            }
            else
            {
                if (Request.Form["startDate"] != null)
                {
                    //Response.Write("<script>alert('');</script>"); return View();
                    //DisplayMessage();
                }
            }

            DataTable dt = reportManager.Report0321T(start, merchantNo, MERC_GROUP);
            return Report314Result(dt, "每日餘額返還報表", "RTDS", "");

        }

        public ActionResult Report0325Result()
        {
            int year = DateTime.Today.Year;
            int month = DateTime.Today.Month;
            DateTime startDate = new DateTime(year, month, 1);
            DateTime endDate = new DateTime(year, month,
                                    DateTime.DaysInMonth(year, month));
            string start = startDate.ToString("yyyyMMdd");
            string end = endDate.ToString("yyyyMMdd");
            //string merchantNo = "";
            string merchantBankNo = "";
            string MERC_GROUP = "BNK000";
            string PRT_TYPE = "21";

            string merchant = "";

            string group = "";
            string item = "";
            string retail = "";
            string bus = "";
            string bike = "";
            string track = "";
            string parking = "";
            string outsourcing = "";

            if (Request.Form["searchConfirm"] != null || Request.Form["ExportExcel"] != null)
            {
                start = Request.Form["startDate"];
                end = Request.Form["endDate"];
                //merchantNo = Request.Form["Merchant"];
                merchantBankNo = Request.Form["MerchantBank"];
                PRT_TYPE = Request.Form["PRT_TYPE"];

                group = Request.Form["group"];
                item = Request.Form["items"];
                //retail = Request.Form["retail"];
                //bus = Request.Form["bus"];
                //bike = Request.Form["bike"];
                //track = Request.Form["track"];
                //parking = Request.Form["parking"];
                //outsourcing = Request.Form["outsourcing"];

                //if (group == "PARKING_LOT")
                //{ merchant = Request.Form["parking"]; }
                //else if (group == "BANK_OUTSOURCING")
                //{ merchant = Request.Form["outsourcing"]; }
                //else
                //{ merchant = Request.Form[group.ToLower()]; }
                merchant = Request.Form["items"];
            }
            ViewBag.Start = start;
            ViewBag.End = end;
            //ViewBag.Merchant = SetMerchantDropDown(merchantNo);
            ViewBag.MerchantBank = SetMerchantBankDropDown(merchantBankNo);
            if (PRT_TYPE == "21")
            {
                ViewBag.ch1 = "checked";
                ViewBag.ch2 = "";
            }
            else
            {
                ViewBag.ch1 = "";
                ViewBag.ch2 = "checked";
            }

            DataTable dtn = reportManager.MerchantName(merchantBankNo);
            if (dtn.Rows.Count > 0)
            {
                ViewBag.MerchantBankName = dtn.Rows[0][1].ToString();
                MERC_GROUP = "BNK" + dtn.Rows[0][2].ToString();
            }
            else
            {
                if (Request.Form["startDate"] != null)
                {
                    //Response.Write("<script>alert('');</script>"); return View();
                    //DisplayMessage();
                }
            }
            /*
            dtn = reportManager.MerchantName(merchantNo);
            if (dtn.Rows.Count > 0)
            {
                ViewBag.MerchantName = dtn.Rows[0][1].ToString();
            }
            else
                ViewBag.MerchantName = "全部";            
            */
            ViewBag.MERCHANT = SetGroup(group);
            ViewBag.MERCHANT_NAME = group;
            ViewBag.ITEM = item;

            //ViewBag.RETAIL = SetRetail(retail); ViewBag.retails = retail;
            //ViewBag.BUS = SetBus(bus); ViewBag.buses = bus;
            //ViewBag.BIKE = SetBike(bike); ViewBag.bikes = bike;
            //ViewBag.TRACK = SetTrack(track); ViewBag.tracks = track;
            //ViewBag.PARKING = SetParking(parking); ViewBag.parkings = parking;
            //ViewBag.OUTSOURCING = SetOutsourcing(outsourcing); ViewBag.outsourcings = outsourcing;

            //DataTable dt = reportManager.Report0325T(start, end, merchantNo, merchantBankNo, MERC_GROUP, PRT_TYPE);
            DataTable dt = new DataTable();

            List<string> mNoList = new List<string>();
            if (group == "ALL")
            {
                mNoList = new GmMerchantTypeDAO().FindAll();

                foreach (string mNo in mNoList)
                { dt.Merge(reportManager.Report0325T(start, end, mNo, merchantBankNo, MERC_GROUP, PRT_TYPE)); }
            }
            else if (item == "ALL") 
            //else if (retail == "ALL" || bus == "ALL" || bike == "ALL" || track == "ALL" || parking == "ALL" || outsourcing == "ALL")
            {
                mNoList = new GmMerchantTypeDAO().FindGroupAll(group);
                foreach (string mNo in mNoList)
                { dt.Merge(reportManager.Report0325T(start, end, mNo, merchantBankNo, MERC_GROUP, PRT_TYPE)); }
            }
            else if (merchant != "")
            {
                dt = reportManager.Report0325T(start, end, merchant, merchantBankNo, MERC_GROUP, PRT_TYPE);
            }
            else { dt = new DataTable(); }

            return Report314Result(dt, "銀行回覆自動加值異常表", "DIFF", PRT_TYPE);

        }

        public ActionResult Report0326Result()
        {
            int year = DateTime.Today.Year;
            int month = DateTime.Today.Month;
            DateTime startDate = new DateTime(year, month, 1);
            DateTime endDate = new DateTime(year, month,
                                    DateTime.DaysInMonth(year, month));
            string start = startDate.ToString("yyyyMMdd");
            string end = endDate.ToString("yyyyMMdd");
            string merchantBankNo = "";
            string RAMT_TYPE = "";
            string ICC_NO = "";
            if (Request.Form["searchConfirm"] != null || Request.Form["ExportExcel"] != null)
            {
                start = Request.Form["startDate"];
                end = Request.Form["endDate"];
                merchantBankNo = Request.Form["MerchantBank"];
                RAMT_TYPE = Request.Form["RAMT_TYPE"];
                ICC_NO = Request.Form["ICC_NO"];
            }
            ViewBag.Start = start;
            ViewBag.End = end;
            ViewBag.MerchantBank = SetMerchantBankDropDown(merchantBankNo);
            ViewBag.RAMT_TYPE = SetMerchantRAMTDropDown(RAMT_TYPE);
            ViewBag.ICC_NO = ICC_NO;

            DataTable dtn = reportManager.MerchantName(merchantBankNo);
            if (dtn.Rows.Count > 0)
            {
                ViewBag.MerchantBankName = dtn.Rows[0][1].ToString();
                //MERC_GROUP = "BNK" + dtn.Rows[0][2].ToString();
            }
            else
            {
                ViewBag.MerchantBankName = "全部";
            //    if (Request.Form["startDate"] != null)
            //    {
            //        //Response.Write("<script>alert('');</script>"); return View();
            //        DisplayMessage();
            //    }
            }
            dtn = reportManager.TypeName(RAMT_TYPE, "RAMT_TYPE", "RAMT_NAME", "CR_RAMT_TYPE_MST");
            if (dtn.Rows.Count > 0)
            {
                ViewBag.RAMT = dtn.Rows[0][1].ToString();
            }
            else
            {
                ViewBag.RAMT = "全部";
            }

            DataTable dt = reportManager.Report0326T(start, end, merchantBankNo, ICC_NO, RAMT_TYPE);
            return Report314Result(dt, "聯名卡餘返明細報表", "RAMT", start+"~"+end);

        }

        public ActionResult Report0328Result()
        {
            //int year = DateTime.Today.Year;
            //int month = DateTime.Today.Month;
            DateTime startDate = DateTime.Now;
            DateTime endDate = DateTime.Now;

            string start = startDate.ToString("yyyyMMdd");
            string end = endDate.ToString("yyyyMMdd");
            //string merchantNo = "";
            string merchantBankNo = "";

            string merchant = "";

            string group = "";
            string item = "";
            string retail = "";
            string bus = "";
            string bike = "";
            string track = "";
            string parking = "";
            string outsourcing = "";


            if (Request.Form["searchConfirm"] != null || Request.Form["ExportExcel"] != null)
            {
                start = Request.Form["startDate"];
                end = Request.Form["endDate"];
                //merchantNo = Request.Form["Merchant"];
                merchantBankNo = Request.Form["MerchantBank"];

                group = Request.Form["group"];
                item = Request.Form["items"];
                //retail = Request.Form["retail"];
                //bus = Request.Form["bus"];
                //bike = Request.Form["bike"];
                //track = Request.Form["track"];
                //parking = Request.Form["parking"];
                //outsourcing = Request.Form["outsourcing"];

                //if (group == "PARKING_LOT")
                //{ merchant = Request.Form["parking"]; }
                //else if (group == "BANK_OUTSOURCING")
                //{ merchant = Request.Form["outsourcing"]; }
                //else
                //{ merchant = Request.Form[group.ToLower()]; }
                merchant = Request.Form["items"];

            }
            ViewBag.Start = start;
            ViewBag.End = end;
            //ViewBag.Merchant = SetMerchantDropDown(merchantNo);
            ViewBag.MerchantBank = SetMerchantBankDropDown(merchantBankNo);
            DataTable dtn = reportManager.MerchantName(merchantBankNo);
            if (dtn.Rows.Count > 0)
            {
                ViewBag.MerchantBankName = dtn.Rows[0][1].ToString();
                //MERC_GROUP = "BNK" + dtn.Rows[0][2].ToString();
            }
            /*
            dtn = reportManager.MerchantName(merchantNo);
            if (dtn.Rows.Count > 0)
            {
                ViewBag.MerchantName = dtn.Rows[0][1].ToString();
            }
            */
            
            //ViewBag.GROUP = SetGroup(group); ViewBag.groups = group;
            //ViewBag.RETAIL = SetRetail(retail); ViewBag.retails = retail;
            //ViewBag.BUS = SetBus(bus); ViewBag.buses = bus;
            //ViewBag.BIKE = SetBike(bike); ViewBag.bikes = bike;
            //ViewBag.TRACK = SetTrack(track); ViewBag.tracks = track;
            //ViewBag.PARKING = SetParking(parking); ViewBag.parkings = parking;
            //ViewBag.OUTSOURCING = SetOutsourcing(outsourcing); ViewBag.outsourcings = outsourcing;
            //DataTable dt = reportManager.Report0328T(start, end, merchantNo, merchantBankNo);
            ViewBag.MERCHANT = SetGroup(group);
            ViewBag.MERCHANT_NAME = group;
            ViewBag.ITEM = item;

            DataTable dt = new DataTable();

            List<string> mNoList = new List<string>();
            if (group == "ALL")
            {
                mNoList = new GmMerchantTypeDAO().FindAll();

                foreach (string mNo in mNoList)
                { dt.Merge(reportManager.Report0328T(start, end, mNo, merchantBankNo)); }
            }
            if(item == "ALL")
            //else if (retail == "ALL" || bus == "ALL" || bike == "ALL" || track == "ALL" || parking == "ALL" || outsourcing == "ALL")
            {
                mNoList = new GmMerchantTypeDAO().FindGroupAll(group);
                foreach (string mNo in mNoList)
                { dt.Merge(reportManager.Report0328T(start, end, mNo, merchantBankNo)); }
            }
            else if (merchant != "")
            {
                dt = reportManager.Report0328T(start, end, merchant, merchantBankNo);
            }
            else { dt = new DataTable(); }

            long sum = 0;

            for(int i=0;i<dt.Rows.Count;i++)
            {
                sum+= Convert.ToInt64(dt.Rows[i][10]);
            }

            if (dt.Columns.Count > 0)
            {
                DataRow lastRow = dt.NewRow();
                lastRow["清分日"] = "總計";
                lastRow["交易金額"] = sum;
                dt.Rows.Add(lastRow);
            }

            //return Report314Result(dt, "中心端自動加值比對認列明細表", "0328", "");
            ViewBag.Count = dt.Rows.Count;
            string RepName = "中心端自動加值比對認列明細表";
            string RepID = "0328";

            //return View(dt);
            if (Request.Form["ExportExcel"] != null) //產生標籤Excel
            {
                //建立Excel 2003檔案
                IWorkbook wb = new HSSFWorkbook();
                ISheet ws;
                //ISheet ws2;

                ////建立Excel 2007檔案
                //IWorkbook wb = new XSSFWorkbook();
                //ISheet ws;
                ws = wb.CreateSheet(RepName);

                var headerRow = ws.CreateRow(3);//第4行為欄位名稱


                HSSFCellStyle cs = (HSSFCellStyle)wb.CreateCellStyle();
                //啟動多行文字
                //cs.WrapText = true;
                //文字置中
                cs.VerticalAlignment = VerticalAlignment.Center;
                cs.Alignment = HorizontalAlignment.Center;
                //框線樣式及顏色
                cs.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
                cs.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                cs.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
                cs.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
                cs.BottomBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                cs.LeftBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                cs.RightBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                cs.TopBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                //背景顏色
                cs.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.Grey25Percent.Index;
                cs.FillPattern = NPOI.SS.UserModel.FillPattern.SolidForeground;
                HSSFFont font1 = (HSSFFont)wb.CreateFont();
                //字體顏色
                font1.Color = NPOI.HSSF.Util.HSSFColor.Black.Index;
                //字體粗體
                //font1.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.Bold;
                //字體尺寸
                font1.FontHeightInPoints = 12;
                cs.SetFont(font1);
                cs.WrapText = true;

                HSSFFont font2 = (HSSFFont)wb.CreateFont();
                //字體顏色
                font2.Color = NPOI.HSSF.Util.HSSFColor.Black.Index;
                //字體粗體
                font2.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.Bold;
                //字體尺寸
                font2.FontHeightInPoints = 20;

                //headerRow.HeightInPoints = 20;
                var cell = headerRow.CreateCell(0);

                headerRow = ws.CreateRow(4);
                cell = headerRow.CreateCell(0);

                headerRow = ws.CreateRow(5);
                headerRow.HeightInPoints = 20;

                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    cell = headerRow.CreateCell(i);
                    cell.SetCellValue(dt.Columns[i].ColumnName);
                    cell.CellStyle = cs;

                    //ws.AutoSizeColumn(i);
                    if (i == 0)
                        ws.SetColumnWidth(0, 18 * 256);
                    else if (i == 1)
                        ws.SetColumnWidth(1, 20 * 256);
                    else if (i == 2)
                        ws.SetColumnWidth(i, 20 * 256);
                    else
                        ws.SetColumnWidth(i, 20 * 256);
                }



                ICellStyle style1 = wb.CreateCellStyle();
                //框線樣式及顏色
                style1.Alignment = HorizontalAlignment.Left;
                style1.VerticalAlignment = VerticalAlignment.Center;
                //字體尺寸
                font1.FontHeightInPoints = 11;
                style1.SetFont(font1);

                ICellStyle style2 = wb.CreateCellStyle();
                //框線樣式及顏色
                style2.Alignment = HorizontalAlignment.Center;
                style2.VerticalAlignment = VerticalAlignment.Center;
                //字體尺寸
                font1.FontHeightInPoints = 11;
                //字體粗體
                style2.SetFont(font2);
                style2.WrapText = true; //自動換列

                //fill content 
                ICellStyle style3 = wb.CreateCellStyle();
                //框線樣式及顏色
                style3.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
                style3.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                style3.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
                style3.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
                style3.BottomBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                style3.LeftBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                style3.RightBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                style3.TopBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                style3.Alignment = HorizontalAlignment.Left;
                style3.VerticalAlignment = VerticalAlignment.Center;
                //字體尺寸
                font1.FontHeightInPoints = 11;
                style3.SetFont(font1);
                style3.WrapText = true;
                //style3.FillForegroundColor = IndexedColors.LemonChiffon.Index;
                //style3.FillPattern = FillPattern.SolidForeground;
                //style3.FillBackgroundColor = IndexedColors.LightGreen.Index;

                // Create the style object
                var currencyCellStyle = wb.CreateCellStyle();
                // Right-align currency values
                currencyCellStyle.Alignment = HorizontalAlignment.Right;
                currencyCellStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
                currencyCellStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                currencyCellStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
                currencyCellStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
                currencyCellStyle.BottomBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                currencyCellStyle.LeftBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                currencyCellStyle.RightBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                currencyCellStyle.TopBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                font1.FontHeightInPoints = 11;
                currencyCellStyle.SetFont(font1);

                currencyCellStyle.DataFormat = wb.CreateDataFormat().GetFormat("#,##0");

                ws.CreateRow(0).HeightInPoints = 22; //列高
                var rang = ws.CreateRow(1);
                var cell3 = rang.CreateCell(0);
                cell3.SetCellValue("報表編號： " + RepID);
                cell3.CellStyle = style1;
                rang = ws.CreateRow(2);
                cell3 = rang.CreateCell(0);
                cell3.CellStyle = style1;
                cell3.SetCellValue("銀行名稱：" + ViewBag.MerchantBankName);

                rang = ws.CreateRow(3);
                cell3 = rang.CreateCell(0);
                cell3.CellStyle = style1;
                cell3.SetCellValue("特約機構：" + ViewBag.MerchantName);
                rang = ws.CreateRow(4);
                cell3 = rang.CreateCell(0);
                cell3.SetCellValue("清算日期：" + ViewBag.Start + "~" + ViewBag.End);
                cell3.CellStyle = style1;

                cell3 = ws.GetRow(0).CreateCell(0);
                cell3.SetCellValue("中心端自動加值比對認列明細表");
                ws.AddMergedRegion(new CellRangeAddress(0, 0, 0, 13));

                cell3.CellStyle = style2;

                int ii = 5;
                //int stcol = 0;

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ii = ii + 1;
                    ws.CreateRow(ii);
                    var cell2 = ws.GetRow(ii).CreateCell(0);
                    cell2.CellStyle = currencyCellStyle;
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        cell2 = ws.GetRow(ii).CreateCell(j);

                        if (j == 10)
                        {
                            cell2.SetCellValue(Convert.ToDouble(dt.Rows[i][j].ToString()));
                            cell2.CellStyle = currencyCellStyle;
                        }
                        else
                        {
                            cell2.SetCellValue(dt.Rows[i][j].ToString());
                            cell2.CellStyle = style3;
                        }
                    }
                }

                var stream = new MemoryStream();
                wb.Write(stream);
                stream.Close();

                return File(stream.ToArray(), "application/vnd.ms-excel", RepName + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls");

            }
            else { return View(dt); }

        }

        public ActionResult Report0329Result()
        {
            int year = DateTime.Today.Year;
            int month = DateTime.Today.Month;
            int day = DateTime.Today.Day;
            DateTime startDate = new DateTime(year, month, day);
            DateTime endDate = startDate;
            string start = startDate.ToString("yyyyMMdd");
            string end = endDate.ToString("yyyyMMdd");
            string merchantBankNo = "";
            if (Request.Form["searchConfirm"] != null || Request.Form["ExportExcel"] != null)
            {
                start = Request.Form["startDate"];
                end = start; //Request.Form["endDate"];
                merchantBankNo = Request.Form["MerchantBank"];
            }
            ViewBag.Start = start;
            //ViewBag.End = start;
            ViewBag.MerchantBank = SetMerchantBankWithAll(merchantBankNo);
            DataTable dtn = reportManager.MerchantName(merchantBankNo);
            if (dtn.Rows.Count > 0)
            {
                ViewBag.MerchantBankName = dtn.Rows[0][1].ToString();
                //MERC_GROUP = "BNK" + dtn.Rows[0][2].ToString();
            }

            DataTable dt = reportManager.Report0329T(start, end, merchantBankNo);
            return Report314Result(dt, "剔退及再提示明細表", "0329", "");

        }


        public ActionResult Report314Result(DataTable dt, string RepName, string RepID, string Rang)
        {
            ViewBag.Count = dt.Rows.Count;

            //return View(dt);
            if (Request.Form["ExportExcel"] != null) //產生標籤Excel
            {
                //建立Excel 2003檔案
                IWorkbook wb = new HSSFWorkbook();
                ISheet ws;
                //ISheet ws2;

                ////建立Excel 2007檔案
                //IWorkbook wb = new XSSFWorkbook();
                //ISheet ws;
                ws = wb.CreateSheet(RepName);

                var headerRow = ws.CreateRow(3);//第4行為欄位名稱


                HSSFCellStyle cs = (HSSFCellStyle)wb.CreateCellStyle();
                //啟動多行文字
                //cs.WrapText = true;
                //文字置中
                cs.VerticalAlignment = VerticalAlignment.Center;
                cs.Alignment = HorizontalAlignment.Center;
                //框線樣式及顏色
                cs.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
                cs.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                cs.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
                cs.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
                cs.BottomBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                cs.LeftBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                cs.RightBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                cs.TopBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                //背景顏色
                cs.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.Grey25Percent.Index;
                cs.FillPattern = NPOI.SS.UserModel.FillPattern.SolidForeground;
                HSSFFont font1 = (HSSFFont)wb.CreateFont();
                //字體顏色
                font1.Color = NPOI.HSSF.Util.HSSFColor.Black.Index;
                //字體粗體
                //font1.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.Bold;
                //字體尺寸
                font1.FontHeightInPoints = 12;
                cs.SetFont(font1);
                cs.WrapText = true;

                HSSFFont font2 = (HSSFFont)wb.CreateFont();
                //字體顏色
                font2.Color = NPOI.HSSF.Util.HSSFColor.Black.Index;
                //字體粗體
                font2.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.Bold;
                //字體尺寸
                font2.FontHeightInPoints = 20;

                //headerRow.HeightInPoints = 20;
                var cell = headerRow.CreateCell(0);

                if (RepID == "0213")
                {
                    ws.PrintSetup.Landscape = true; //橫向列印
                    headerRow = ws.CreateRow(3);
                    cell = headerRow.CreateCell(0);
                    cell.CellStyle = cs;
                    cell.SetCellValue("傳輸日");

                    cell = headerRow.CreateCell(1);
                    cell.CellStyle = cs;
                    cell.SetCellValue("清算日");

                    cell = headerRow.CreateCell(2);
                    cell.CellStyle = cs;
                    cell.SetCellValue("會計日");


                    cell = headerRow.CreateCell(3);
                    cell.SetCellValue("購貨");
                    cell.CellStyle = cs;
                    cell = headerRow.CreateCell(4);
                    cell.SetCellValue("");
                    cell.CellStyle = cs;
                    cell = headerRow.CreateCell(5);
                    cell.SetCellValue("");
                    cell.CellStyle = cs;
                    cell = headerRow.CreateCell(6);
                    cell.SetCellValue("");
                    cell.CellStyle = cs;
                    cell = headerRow.CreateCell(7);
                    cell.SetCellValue("");
                    cell.CellStyle = cs;
                    cell = headerRow.CreateCell(8);
                    cell.SetCellValue("");
                    cell.CellStyle = cs;

                    cell = headerRow.CreateCell(9);
                    cell.SetCellValue("加值");
                    cell.CellStyle = cs;
                    cell = headerRow.CreateCell(10);
                    cell.SetCellValue("");
                    cell.CellStyle = cs;
                    cell = headerRow.CreateCell(11);
                    cell.SetCellValue("");
                    cell.CellStyle = cs;
                    cell = headerRow.CreateCell(12);
                    cell.SetCellValue("");
                    cell.CellStyle = cs;
                    cell = headerRow.CreateCell(13);
                    cell.SetCellValue("");
                    cell.CellStyle = cs;
                    cell = headerRow.CreateCell(14);
                    cell.SetCellValue("");
                    cell.CellStyle = cs;


                    headerRow = ws.CreateRow(4);
                    cell = headerRow.CreateCell(0);
                    cell.CellStyle = cs;
                    cell.SetCellValue("傳輸日");

                    cell = headerRow.CreateCell(1);
                    cell.CellStyle = cs;
                    cell.SetCellValue("清算日");

                    cell = headerRow.CreateCell(2);
                    cell.CellStyle = cs;
                    cell.SetCellValue("會計日");

                    cell = headerRow.CreateCell(3);
                    cell.CellStyle = cs;
                    cell.SetCellValue("購貨");
                    cell = headerRow.CreateCell(5);
                    cell.CellStyle = cs;
                    cell.SetCellValue("購貨取消");
                    cell = headerRow.CreateCell(7);
                    cell.CellStyle = cs;
                    cell.SetCellValue("小計");

                    cell = headerRow.CreateCell(9);
                    cell.CellStyle = cs;
                    cell.SetCellValue("加值");
                    cell = headerRow.CreateCell(11);
                    cell.CellStyle = cs;
                    cell.SetCellValue("加值取消");
                    cell = headerRow.CreateCell(13);
                    cell.CellStyle = cs;
                    cell.SetCellValue("小計");

                    ws.AddMergedRegion(new CellRangeAddress(3, 3, 3, 8));
                    ws.AddMergedRegion(new CellRangeAddress(3, 3, 9, 14));

                    ws.AddMergedRegion(new CellRangeAddress(3, 5, 0, 0));
                    ws.AddMergedRegion(new CellRangeAddress(3, 5, 1, 1));
                    ws.AddMergedRegion(new CellRangeAddress(3, 5, 2, 2));

                    ws.AddMergedRegion(new CellRangeAddress(4, 4, 3, 4));
                    ws.AddMergedRegion(new CellRangeAddress(4, 4, 5, 6));
                    ws.AddMergedRegion(new CellRangeAddress(4, 4, 7, 8));
                    ws.AddMergedRegion(new CellRangeAddress(4, 4, 9, 10));
                    ws.AddMergedRegion(new CellRangeAddress(4, 4, 11, 12));
                    ws.AddMergedRegion(new CellRangeAddress(4, 4, 13, 14));

                }
                else
                {
                    headerRow = ws.CreateRow(4);
                    cell = headerRow.CreateCell(0);

                }


                if (RepID == "FALS")
                {
                    cell.CellStyle = cs;
                    cell.SetCellValue("銀行");
                    ws.AddMergedRegion(new CellRangeAddress(4, 5, 0, 0));
                    cell = headerRow.CreateCell(1);
                    cell.SetCellValue("首次啟用日期");
                    cell.CellStyle = cs;
                    ws.AddMergedRegion(new CellRangeAddress(4, 5, 1, 1));
                    cell = headerRow.CreateCell(2);
                    cell.SetCellValue("新發卡期間首次啟用");
                    cell.CellStyle = cs;
                    cell = headerRow.CreateCell(3);
                    cell.CellStyle = cs;
                    ws.AddMergedRegion(new CellRangeAddress(4, 4, 2, 3));
                    cell = headerRow.CreateCell(4);
                    cell.SetCellValue("非新發卡期間首次啟用");
                    cell.CellStyle = cs;
                    cell = headerRow.CreateCell(5);
                    cell.CellStyle = cs;
                    ws.AddMergedRegion(new CellRangeAddress(4, 4, 4, 5));
                    cell = headerRow.CreateCell(6);
                    cell.CellStyle = cs;
                    cell.SetCellValue("愛金卡應付金額");
                    ws.AddMergedRegion(new CellRangeAddress(4, 5, 6, 6));
                }
                if (RepID == "RTDS")
                {
                    ws.PrintSetup.Landscape = true; //橫向列印
                    cell.CellStyle = cs;
                    cell.SetCellValue("銀行");
                    ws.AddMergedRegion(new CellRangeAddress(4, 5, 0, 0));
                    cell = headerRow.CreateCell(1);
                    cell.SetCellValue("通知日期");
                    cell.CellStyle = cs;
                    ws.AddMergedRegion(new CellRangeAddress(4, 5, 1, 1));
                    cell = headerRow.CreateCell(2);
                    cell.SetCellValue("處理日期");
                    cell.CellStyle = cs;
                    ws.AddMergedRegion(new CellRangeAddress(4, 5, 2, 2));

                    cell = headerRow.CreateCell(3);
                    cell.SetCellValue("一般餘額返還");
                    cell.CellStyle = cs;
                    cell = headerRow.CreateCell(4);
                    cell.CellStyle = cs;
                    ws.AddMergedRegion(new CellRangeAddress(4, 4, 3, 4));
                    cell = headerRow.CreateCell(5);
                    cell.SetCellValue("掛失餘額返還");
                    cell.CellStyle = cs;
                    cell = headerRow.CreateCell(6);
                    cell.CellStyle = cs;
                    ws.AddMergedRegion(new CellRangeAddress(4, 4, 5, 6));
                    cell = headerRow.CreateCell(7);
                    cell.CellStyle = cs;
                    cell.SetCellValue("愛金卡應返還金額");
                    ws.AddMergedRegion(new CellRangeAddress(4, 5, 7, 7));
                }
                if (RepID == "ALDS")
                {
                    ws.PrintSetup.Landscape = true; //橫向列印
                    cell.CellStyle = cs;
                    cell.SetCellValue("銀行");
                    ws.AddMergedRegion(new CellRangeAddress(4, 5, 0, 0));
                    cell = headerRow.CreateCell(1);
                    cell.SetCellValue("請款日期");
                    ws.AddMergedRegion(new CellRangeAddress(4, 5, 1, 1));
                    cell.CellStyle = cs;
                    cell = headerRow.CreateCell(2);
                    cell.SetCellValue("自動加值交易");
                    cell.CellStyle = cs;
                    cell = headerRow.CreateCell(3);
                    cell.CellStyle = cs;
                    ws.AddMergedRegion(new CellRangeAddress(4, 4, 2, 3));
                    cell = headerRow.CreateCell(4);
                    cell.SetCellValue("剔退交易");
                    cell.CellStyle = cs;
                    cell = headerRow.CreateCell(5);
                    cell.CellStyle = cs;
                    ws.AddMergedRegion(new CellRangeAddress(4, 4, 4, 5));
                    cell = headerRow.CreateCell(6);
                    cell.SetCellValue("再提示交易");
                    cell.CellStyle = cs;
                    cell = headerRow.CreateCell(7);
                    cell.CellStyle = cs;
                    ws.AddMergedRegion(new CellRangeAddress(4, 4, 6, 7));
                    cell = headerRow.CreateCell(8);
                    cell.CellStyle = cs;
                    cell.SetCellValue("發卡行應付金額");
                    ws.AddMergedRegion(new CellRangeAddress(4, 5, 8, 8));

                }

                headerRow = ws.CreateRow(5);
                headerRow.HeightInPoints = 20;

                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    cell = headerRow.CreateCell(i);
                    cell.SetCellValue(dt.Columns[i].ColumnName);
                    cell.CellStyle = cs;

                    //ws.AutoSizeColumn(i);
                    if (i == 0)
                        ws.SetColumnWidth(0, 18 * 256);
                    else if (i == 1)
                        ws.SetColumnWidth(1, 20 * 256);
                    else if (i == 2)
                        ws.SetColumnWidth(i, 20 * 256);
                    else
                        ws.SetColumnWidth(i, 20 * 256);
                }
                if (RepID == "DIFF" || RepID == "RAMT" || RepID == "0328")
                {
                    ws.PrintSetup.Landscape = true; //橫向列印
                }
                if (RepID == "0329")
                {
                    ws.PrintSetup.Landscape = true; //橫向列印
                    headerRow.HeightInPoints = 30;
                    for (int i = 0; i < 7; i++)
                    {
                        ws.SetColumnWidth(i, 10 * 256);
                    }
                    ws.SetColumnWidth(1, 5 * 256);
                    ws.SetColumnWidth(2, 12 * 256);
                    ws.SetColumnWidth(3, 18 * 256);
                    ws.SetColumnWidth(4, 18 * 256);
                    ws.SetColumnWidth(5, 10 * 256);
                    ws.SetColumnWidth(6, 18 * 256);
                    ws.SetColumnWidth(8, 10 * 256);
                    ws.SetColumnWidth(9, 8 * 256);
                    ws.SetColumnWidth(10, 10 * 256);
                    ws.SetColumnWidth(11, 15 * 256);
                    ws.SetColumnWidth(12, 10 * 256);
                    ws.SetColumnWidth(13, 5 * 256);
                    ws.SetColumnWidth(14, 10 * 256);
                    ws.SetColumnWidth(15, 10 * 256);
                    //for (int i = 0; i < dt.Columns.Count; i++)
                    //{
                        //ws.AutoSizeColumn(i);
                    //}
                }
                if (RepID == "0214")
                {
                    ws.PrintSetup.Landscape = true; //橫向列印
                    ws.SetColumnWidth(0, 10 * 256);
                    ws.SetColumnWidth(2, 8 * 256);
                    ws.SetColumnWidth(3, 10 * 256);
                    ws.SetColumnWidth(4, 10 * 256);
                    ws.SetColumnWidth(6, 5 * 256);
                    ws.SetColumnWidth(7, 10 * 256);
                    ws.SetColumnWidth(11, 10 * 256);
                    ws.SetColumnWidth(12, 10 * 256);

                }
                if (RepID == "0213")
                {
                    ws.SetColumnWidth(0, 10 * 256);
                    ws.SetColumnWidth(1, 10 * 256);
                    ws.SetColumnWidth(2, 10 * 256);
                }

                if (RepID == "RTDS")
                {
                    //ws.SetColumnWidth(1, 12 * 256);
                    ws.AutoSizeColumn(0);
                    ws.AutoSizeColumn(1);
                    ws.AutoSizeColumn(2);
                }
                ICellStyle style1 = wb.CreateCellStyle();
                //框線樣式及顏色
                style1.Alignment = HorizontalAlignment.Left;
                style1.VerticalAlignment = VerticalAlignment.Center;
                //字體尺寸
                font1.FontHeightInPoints = 11;
                style1.SetFont(font1);

                ICellStyle style2 = wb.CreateCellStyle();
                //框線樣式及顏色
                style2.Alignment = HorizontalAlignment.Center; 
                style2.VerticalAlignment = VerticalAlignment.Center;
                //字體尺寸
                font1.FontHeightInPoints = 11;
                //字體粗體
                style2.SetFont(font2);
                style2.WrapText = true; //自動換列

                //fill content 
                ICellStyle style3 = wb.CreateCellStyle();
                //框線樣式及顏色
                style3.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
                style3.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                style3.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
                style3.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
                style3.BottomBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                style3.LeftBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                style3.RightBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                style3.TopBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                style3.Alignment = HorizontalAlignment.Left;
                style3.VerticalAlignment = VerticalAlignment.Center;
                //字體尺寸
                font1.FontHeightInPoints = 11;
                style3.SetFont(font1);
                style3.WrapText = true;
                //style3.FillForegroundColor = IndexedColors.LemonChiffon.Index;
                //style3.FillPattern = FillPattern.SolidForeground;
                //style3.FillBackgroundColor = IndexedColors.LightGreen.Index;

                // Create the style object
                var currencyCellStyle = wb.CreateCellStyle();
                // Right-align currency values
                currencyCellStyle.Alignment = HorizontalAlignment.Right;
                currencyCellStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
                currencyCellStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                currencyCellStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
                currencyCellStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
                currencyCellStyle.BottomBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                currencyCellStyle.LeftBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                currencyCellStyle.RightBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                currencyCellStyle.TopBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                font1.FontHeightInPoints = 11;
                currencyCellStyle.SetFont(font1);
                // Get / create the data format string
                //var formatId = HSSFDataFormat.GetBuiltinFormat("#,##0");
                //if (formatId == -1)
                //{
                //    var newDataFormat = wb.CreateDataFormat();
                //    currencyCellStyle.DataFormat = newDataFormat.GetFormat("#,##0");
                //}
                //else
                //currencyCellStyle.DataFormat = formatId;
                //HSSFDataFormat myformat = wb.CreateDataFormat();
                //ICellStyle newDataFormat = wb.CreateCellStyle();
                currencyCellStyle.DataFormat = wb.CreateDataFormat().GetFormat("#,##0");

                ws.CreateRow(0).HeightInPoints = 22; //列高
                var rang = ws.CreateRow(1);
                var cell3 = rang.CreateCell(0);
                cell3.SetCellValue("報表編號： " + RepID);
                cell3.CellStyle = style1;
                rang = ws.CreateRow(2);
                cell3 = rang.CreateCell(0);
                cell3.CellStyle = style1;
                cell3.SetCellValue("銀行名稱：" + ViewBag.MerchantBankName);
                if (RepID == "DIFF")
                {
                    cell3.SetCellValue("銀行名稱：" + ViewBag.MerchantBankName + "    機構名稱：" + ViewBag.MerchantName);
                }
                if (RepID == "RAMT")
                {
                    rang = ws.CreateRow(3);
                    cell3 = rang.CreateCell(0);
                    cell3.CellStyle = style1;
                    cell3.SetCellValue("清算起迄：" + Rang);
                    rang = ws.CreateRow(4);
                    cell3 = rang.CreateCell(0);
                    cell3.SetCellValue("餘返原因：" + ViewBag.RAMT);
                    cell3.CellStyle = style1;
                    cell3 = ws.GetRow(0).CreateCell(0);
                    cell3.SetCellValue("聯名卡餘返明細報表");
                    ws.AddMergedRegion(new CellRangeAddress(0, 0, 0, 10));
                }

                if (RepID == "FALS")
                {
                    rang = ws.CreateRow(3);
                    cell3 = rang.CreateCell(0);
                    cell3.CellStyle = style1;
                    //cell3.SetCellValue("清算週期：" + Rang);
                    cell3.SetCellValue("年月：" + ViewBag.Start);
                    cell3 = ws.GetRow(0).CreateCell(0);
                    cell3.SetCellValue("自動加值首次啟用月報表");
                    ws.AddMergedRegion(new CellRangeAddress(0, 0, 0, 6));
                }
                else
                {
                    if (RepID == "ALDS")
                    {
                        rang = ws.CreateRow(3);
                        cell3 = rang.CreateCell(0);
                        cell3.CellStyle = style1;
                        cell3.SetCellValue("清算日期：" + ViewBag.Start);
                        cell3 = ws.GetRow(0).CreateCell(0);
                        cell3.SetCellValue("每日自動加值請款報表");
                        ws.AddMergedRegion(new CellRangeAddress(0, 0, 0, 8));
                    }
                }

                if (RepID == "DIFF")
                {
                    rang = ws.CreateRow(3);
                    cell3 = rang.CreateCell(0);
                    cell3.CellStyle = style1;
                    cell3.SetCellValue("清算日期：" + ViewBag.Start + "~" + ViewBag.End);
                    rang = ws.CreateRow(4);
                    cell3 = rang.CreateCell(0);
                    if (Rang == "21")
                        cell3.SetCellValue("剔退異常表");
                    else
                        cell3.SetCellValue("愛金卡認損");
                    cell3.CellStyle = style1;

                    cell3 = ws.GetRow(0).CreateCell(0);
                    cell3.SetCellValue("銀行回覆自動加值異常表");
                    ws.AddMergedRegion(new CellRangeAddress(0, 0, 0, 10));
                }
                if (RepID == "0328")
                {
                    rang = ws.CreateRow(3);
                    cell3 = rang.CreateCell(0);
                    cell3.CellStyle = style1;
                    cell3.SetCellValue("特約機構：" + ViewBag.MerchantName);
                    rang = ws.CreateRow(4);
                    cell3 = rang.CreateCell(0);
                    cell3.SetCellValue("清算日期：" + ViewBag.Start + "~" + ViewBag.End);
                    cell3.CellStyle = style1;

                    cell3 = ws.GetRow(0).CreateCell(0);
                    cell3.SetCellValue("中心端自動加值比對認列明細表");
                    ws.AddMergedRegion(new CellRangeAddress(0, 0, 0, 13));
                }
                if (RepID == "0329")
                {
                    rang = ws.CreateRow(4);
                    cell3 = rang.CreateCell(0);
                    cell3.SetCellValue("剔退日期：" + ViewBag.Start );
                    cell3.CellStyle = style1;

                    ws.AddMergedRegion(new CellRangeAddress(0, 0, 0, 15));
                    cell3 = ws.GetRow(0).CreateCell(0);
                    cell3.SetCellValue("剔退及再提示明細表");
                    cell3.CellStyle = style2;
                }

                if (RepID == "RTDS")
                {
                    rang = ws.CreateRow(3);
                    cell3 = rang.CreateCell(0);
                    cell3.CellStyle = style1;
                    cell3.SetCellValue("日期：" + ViewBag.Start);

                    cell3 = ws.GetRow(0).CreateCell(0);
                    cell3.SetCellValue("每日餘額返還報表");
                    ws.AddMergedRegion(new CellRangeAddress(0, 0, 0, 7));
                }
                cell3.CellStyle = style2;

                if (RepID == "0213")
                {
                    rang = ws.CreateRow(2);
                    cell3 = rang.CreateCell(0);
                    cell3.CellStyle = style1;
                    cell3.SetCellValue("查詢日：" + ViewBag.Start + "~" + ViewBag.End);

                    cell3 = ws.GetRow(0).CreateCell(0);
                    cell3.SetCellValue(RepName);
                    ws.AddMergedRegion(new CellRangeAddress(0, 0, 0, 14));
                    cell3.CellStyle = style2; //標題

                    rang = ws.CreateRow(1);
                    cell3 = rang.CreateCell(0);
                    cell3.CellStyle = style1;
                    cell3.SetCellValue("列印日：" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                    //ws.AddMergedRegion(new CellRangeAddress(2, 2, 0, 2));
                }
                if (RepID == "0214")
                {
                    rang = ws.CreateRow(3);
                    cell3 = rang.CreateCell(0);
                    cell3.CellStyle = style1;
                    cell3.SetCellValue("會計日：" + ViewBag.Start + "~" + ViewBag.End);

                    rang = ws.CreateRow(4);
                    cell3 = rang.CreateCell(0);
                    cell3.SetCellValue("差異類型：" + ViewBag.Kinds);
                    cell3.CellStyle = style1;

                    cell3 = ws.GetRow(0).CreateCell(0);
                    cell3.SetCellValue(RepName);
                    ws.AddMergedRegion(new CellRangeAddress(0, 0, 0, 12));
                    cell3.CellStyle = style2;

                    rang = ws.CreateRow(2);
                    cell3 = rang.CreateCell(0);
                    cell3.CellStyle = style1;
                    cell3.SetCellValue("");

                    rang = ws.CreateRow(2);
                    cell3 = rang.CreateCell(0);
                    cell3.CellStyle = style1;
                    cell3.SetCellValue("列印日：" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                    //ws.AddMergedRegion(new CellRangeAddress(2, 2, 0, 2));
                }
                


                int ii = 5;
                int stcol = 0;

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ii = ii + 1;
                    ws.CreateRow(ii);
                    var cell2 = ws.GetRow(ii).CreateCell(0);
                    cell2.CellStyle = currencyCellStyle;
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        cell2 = ws.GetRow(ii).CreateCell(j );

                        if (RepID == "ALDS" || RepID == "FALS")
                            stcol = 2;
                        else
                            stcol = 1;

                        if (RepID == "DIFF" || RepID == "RAMT" || RepID == "0328")
                        {
                            stcol = 22;
                        }
                        if (RepID == "RTDS" )
                            stcol = 3;
                if (RepID == "0213")
                    stcol = 3;
                        if (j >= stcol && RepID != "0329" && RepID != "0214")
                        {
                            //cell2.SetCellValue(Convert.ToDouble(dt.Rows[i][j].ToString()));
                            //total = Convert.ToDouble(dt.Rows[i][j].ToString());
                            cell2.SetCellValue(Convert.ToDouble(dt.Rows[i][j].ToString()));
                            cell2.CellStyle = currencyCellStyle;
                        }
                        else
                        {
                            cell2.SetCellValue(dt.Rows[i][j].ToString());
                            cell2.CellStyle = style3;
                            if ((RepID == "DIFF" || RepID == "0328") && j == 10)
                            {
                                cell2.SetCellValue(Convert.ToDouble(dt.Rows[i][j].ToString()));
                                cell2.CellStyle = currencyCellStyle;
                            }
                            if (RepID == "RAMT" && j >= 5 && j <= 7)
                            {
                                cell2.SetCellValue(Convert.ToDouble(dt.Rows[i][j].ToString()));
                                cell2.CellStyle = currencyCellStyle;
                            }
                            if (RepID == "0329" && (j == 8))
                            {
                                cell2.SetCellValue(Convert.ToDouble(dt.Rows[i][j].ToString()));
                                cell2.CellStyle = currencyCellStyle;
                            }
                        }
                        if (RepID == "0214")
                        {
                            //stcol = 33;
                            if (j == 8 || j == 9)
                            {
                                cell2.SetCellValue(Convert.ToDouble(dt.Rows[i][j].ToString()));
                                cell2.CellStyle = currencyCellStyle;
                            }
                        }

                    }
                }

                var stream = new MemoryStream();
                wb.Write(stream);
                stream.Close();

                return File(stream.ToArray(), "application/vnd.ms-excel", RepName +"_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls");


            }
            else
            {
                return View(dt);
            }

        }

        public ActionResult DisplayMessage() 
        {
            TempData["message"] = "請選擇銀行"; 
            return RedirectToAction("SimpleAlert", "Home"); 
        }
        public ActionResult DisplayMessage(string msg)
        {
            TempData["message"] = msg;
            return RedirectToAction("SimpleAlert", "Home");
        }

        public ActionResult Report0315Result()
        {
            int year = DateTime.Today.Year;
            int month = DateTime.Today.Month;
            int day = DateTime.Today.Day;
            DateTime startDate = new DateTime(year, month, 1);
            DateTime endDate = new DateTime(year, month,
                                    DateTime.DaysInMonth(year, month));
            string start = startDate.ToString("yyyyMM");
            string end = endDate.ToString("yyyyMM");

            string merchantNo = "";
            if (Request.Form["searchConfirm"] != null || Request.Form["ExportExcel"] != null)
            {
                start = Request.Form["startDate"];
                end = start;
                merchantNo = Request.Form["Merchant"];
            }
            ViewBag.Start = start;
            //ViewBag.End = end;
            string MERC_GROUP = "BNK000";

            ViewBag.Merchant = SetMerchantBankDropDown(merchantNo);
            ViewBag.MerchantBankName = "";
           
            DataTable dtn = reportManager.MerchantName(merchantNo);
            if (dtn.Rows.Count > 0)
            {
                ViewBag.MerchantBankName = dtn.Rows[0][1].ToString();
                MERC_GROUP = "BNK" + dtn.Rows[0][2].ToString();
            }
            else
            {
                if (Request.Form["startDate"] != null)
                {
                    //Response.Write("<script>alert('');</script>"); return View();
                    //DisplayMessage();
                }
            }

            DataTable dt = reportManager.Report0315T(start, end, merchantNo, MERC_GROUP);
            ViewBag.Count = dt.Rows.Count;

            //return View(dt);
            if (Request.Form["ExportExcel"] != null) //產生標籤Excel
            {
                //建立Excel 2003檔案
                IWorkbook wb = new HSSFWorkbook();
                ISheet ws;
                //ISheet ws2;

                ////建立Excel 2007檔案
                //IWorkbook wb = new XSSFWorkbook();
                //ISheet ws;
                ws = wb.CreateSheet("簽帳回饋金月報表");

                var headerRow = ws.CreateRow(4);//第一行為欄位名稱


                HSSFCellStyle cs = (HSSFCellStyle)wb.CreateCellStyle();
                //啟動多行文字
                //cs.WrapText = true;
                //文字置中
                cs.VerticalAlignment = VerticalAlignment.Center;
                cs.Alignment = HorizontalAlignment.Center;
                //框線樣式及顏色
                cs.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
                cs.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                cs.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
                cs.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
                cs.BottomBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                cs.LeftBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                cs.RightBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                cs.TopBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                //背景顏色
                cs.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.Grey25Percent.Index;
                cs.FillPattern = NPOI.SS.UserModel.FillPattern.SolidForeground;
                HSSFFont font1 = (HSSFFont)wb.CreateFont();
                //字體顏色
                font1.Color = NPOI.HSSF.Util.HSSFColor.Black.Index;
                //字體粗體
                //font1.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.Bold;
                //字體尺寸
                font1.FontHeightInPoints = 12;
                cs.SetFont(font1);

                headerRow.HeightInPoints = 20;

                var cell = headerRow.CreateCell(0);
                cell.CellStyle = cs;
                cell.SetCellValue("銀行");
                ws.AddMergedRegion(new CellRangeAddress(4, 5, 0, 0));
                cell = headerRow.CreateCell(1);
                cell.CellStyle = cs;
                cell.SetCellValue("簽帳金額計算期間");
                ws.AddMergedRegion(new CellRangeAddress(4, 5, 1, 1));
                cell = headerRow.CreateCell(2);
                cell.SetCellValue("　一般卡　");
                cell.CellStyle = cs;
                cell = headerRow.CreateCell(3);
                cell.CellStyle = cs;
                cell = headerRow.CreateCell(4);
                cell.CellStyle = cs;
                ws.AddMergedRegion(new CellRangeAddress(4, 4, 2, 4));
                cell = headerRow.CreateCell(5);
                cell.SetCellValue("三方聯名卡");
                cell.CellStyle = cs;
                cell = headerRow.CreateCell(6);
                cell.CellStyle = cs;
                cell = headerRow.CreateCell(7);
                cell.CellStyle = cs;
                ws.AddMergedRegion(new CellRangeAddress(4, 4, 5, 7));
                cell = headerRow.CreateCell(8);
                cell.CellStyle = cs;
                cell.SetCellValue("發卡行應付金額");
                ws.AddMergedRegion(new CellRangeAddress(4, 5, 8, 8));

                headerRow = ws.CreateRow(5);
                headerRow.HeightInPoints = 20;

                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    cell = headerRow.CreateCell(i);
                    cell.SetCellValue(dt.Columns[i].ColumnName);
                    cell.CellStyle = cs;

                    //ws.AutoSizeColumn(i);
                    if (i == 0)
                        ws.SetColumnWidth(0, 20 * 256);
                    else if (i == 1)
                        ws.SetColumnWidth(1, 26 * 256);
                    else if (i == 2)
                        ws.SetColumnWidth(i, 20 * 256);
                    else
                        ws.SetColumnWidth(i, 20 * 256);
                }

                ICellStyle style1 = wb.CreateCellStyle();
                //框線樣式及顏色
                style1.Alignment = HorizontalAlignment.Left;
                style1.VerticalAlignment = VerticalAlignment.Center;
                //字體尺寸
                font1.FontHeightInPoints = 11;
                style1.SetFont(font1);

                ICellStyle style2 = wb.CreateCellStyle();
                //框線樣式及顏色
                style2.Alignment = HorizontalAlignment.Center;
                style2.VerticalAlignment = VerticalAlignment.Center;
                //字體尺寸
                font1.FontHeightInPoints = 11;
                style2.SetFont(font1);

                //fill content 
                ICellStyle style3 = wb.CreateCellStyle();
                //框線樣式及顏色
                style3.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
                style3.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                style3.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
                style3.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
                style3.BottomBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                style3.LeftBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                style3.RightBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                style3.TopBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                style3.Alignment = HorizontalAlignment.Left;
                style3.VerticalAlignment = VerticalAlignment.Center;
                //字體尺寸
                font1.FontHeightInPoints = 11;
                style3.SetFont(font1);

                // Create the style object
                var currencyCellStyle = wb.CreateCellStyle();
                // Right-align currency values
                currencyCellStyle.Alignment = HorizontalAlignment.Right;
                currencyCellStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
                currencyCellStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                currencyCellStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
                currencyCellStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
                currencyCellStyle.BottomBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                currencyCellStyle.LeftBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                currencyCellStyle.RightBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                currencyCellStyle.TopBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                font1.FontHeightInPoints = 11;
                currencyCellStyle.SetFont(font1);
                currencyCellStyle.DataFormat = wb.CreateDataFormat().GetFormat("#,##0");

                ws.CreateRow(0).HeightInPoints = 20; ;
                var cell3 = ws.GetRow(0).CreateCell(0);
                cell3.SetCellValue("簽帳回饋金月報表");
                cell3.CellStyle = style2;
                ws.AddMergedRegion(new CellRangeAddress(0, 0, 0, 8));

                var rang = ws.CreateRow(1);
                cell3 = rang.CreateCell(0);
                cell3.SetCellValue("報表編號： SASD");
                cell3.CellStyle = style1;

                rang = ws.CreateRow(2);
                cell3 = rang.CreateCell(0);
                cell3.SetCellValue("銀行名稱：" + ViewBag.MerchantBankName);
                cell3.CellStyle = style1;

                rang = ws.CreateRow(3);
                cell3 = rang.CreateCell(0);
                cell3.SetCellValue("清算年月：" + ViewBag.Start);
                cell3.CellStyle = style1;

                int ii = 5;

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ii = ii + 1;
                    ws.CreateRow(ii);
                    var cell2 = ws.GetRow(ii).CreateCell(0);
                    cell2.CellStyle = currencyCellStyle;
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        cell2 = ws.GetRow(ii).CreateCell(j);
                        if (j >= 2)
                        {
                            //cell2.SetCellValue(Convert.ToDouble(dt.Rows[i][j].ToString()));
                            //total = Convert.ToDouble(dt.Rows[i][j].ToString());
                            cell2.SetCellValue(Convert.ToDouble(dt.Rows[i][j].ToString()));
                            cell2.CellStyle = currencyCellStyle;
                        }
                        else
                        {
                            cell2.SetCellValue(dt.Rows[i][j].ToString());
                            cell2.CellStyle = style3;
                        }

                    }
                }

                var stream = new MemoryStream();
                wb.Write(stream);
                stream.Close();

                return File(stream.ToArray(), "application/vnd.ms-excel", "簽帳回饋金月報表_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls");


            }
            else
            {
                return View(dt);
            }

        }

        public ActionResult Report0316Result()
        {
            int year = DateTime.Today.Year;
            int month = DateTime.Today.Month;
            int day = DateTime.Today.Day;
            DateTime startDate = new DateTime(year, month, day);
            string start = startDate.ToString("yyyyMMdd");
            string merchantNo = "";
            string MERC_GROUP = "BNK000";
            if (Request.Form["searchConfirm"] != null || Request.Form["ExportExcel"] != null)
            {
                start = Request.Form["startDate"];
                merchantNo = Request.Form["Merchant"];
            }
            ViewBag.Start = start;
            ViewBag.Merchant = SetMerchantBankDropDown(merchantNo);
            ViewBag.MerchantBankName = "";

            DataTable dtn = reportManager.MerchantName(merchantNo);
            if (dtn.Rows.Count > 0)
            {
                ViewBag.MerchantBankName = dtn.Rows[0][1].ToString();
                MERC_GROUP = "BNK" + dtn.Rows[0][2].ToString();
            }
            else
            {
                if (Request.Form["startDate"] != null)
                {
                    //Response.Write("<script>alert('');</script>"); return View();
                    //DisplayMessage();
                }
            }

            DataTable dt = reportManager.Report0316T(start, merchantNo, MERC_GROUP);
            return Report316Result(dt, "帳務調整回饋明細表", "AJDM","");

        }

        public ActionResult Report0120Result()
        {
            int year = DateTime.Today.Year;
            int month = DateTime.Today.Month;
            int day = DateTime.Today.Day;
            DateTime startDate = new DateTime(year, month, 1);
            DateTime endDate = new DateTime(year, month,
                                    DateTime.DaysInMonth(year, month));
            string start = startDate.ToString("yyyyMMdd");
            string end = endDate.ToString("yyyyMMdd");
            if (Request.Form["searchConfirm"] != null || Request.Form["ExportExcel"] != null)
            {
                start = Request.Form["startDate"];
                end = Request.Form["endDate"];
            }
            ViewBag.Start = start;
            ViewBag.End = end;

            DataTable dt = reportManager.Report0120T(start, end);
            return Report316Result(dt, "點數活動給點統計表", "0120", "");

        }

        public ActionResult Report0121Result()
        {
            int year = DateTime.Today.Year;
            int month = DateTime.Today.Month;
            int day = DateTime.Today.Day;
            DateTime startDate = new DateTime(year, month, 1);
            DateTime endDate = new DateTime(year, month,
                                    DateTime.DaysInMonth(year, month));
            string start = startDate.ToString("yyyyMM");
            if (Request.Form["searchConfirm"] != null || Request.Form["ExportExcel"] != null)
            {
                start = Request.Form["startDate"];
            }
            ViewBag.Start = start;

            DataTable dtn = reportManager.Report0121_1T();
            if (dtn.Rows.Count > 0)
            {
                ViewBag.Idollar = dtn.Rows[0][1].ToString();
            }

            DataTable dt = reportManager.Report0121T(start);
            return Report316Result(dt, "IDollar餘額轉置月結明細表", "0121", "");

        }

        public ActionResult Report0317Result()
        {
            int year = DateTime.Today.Year;
            int month = DateTime.Today.Month;
            int day = DateTime.Today.Day;
            DateTime startDate = new DateTime(year, month, day);
            string start = startDate.ToString("yyyyMMdd");
            string merchantNo = "";
            string MERC_GROUP = "BNK000";
            if (Request.Form["searchConfirm"] != null || Request.Form["ExportExcel"] != null)
            {
                start = Request.Form["startDate"];
                merchantNo = Request.Form["Merchant"];
            }
            ViewBag.Start = start;
            ViewBag.Merchant = SetMerchantBankDropDown(merchantNo);
            ViewBag.MerchantBankName = "";

            DataTable dtn = reportManager.MerchantName(merchantNo);
            if (dtn.Rows.Count > 0)
            {
                ViewBag.MerchantBankName = dtn.Rows[0][1].ToString();
                MERC_GROUP = "BNK" + dtn.Rows[0][2].ToString();
            }
            else
            {
                if (Request.Form["startDate"] != null)
                {
                    //Response.Write("<script>alert('');</script>"); return View();
                    //DisplayMessage();
                }
            }

            DataTable dt = reportManager.Report0317T(start, merchantNo, MERC_GROUP);
            return Report316Result(dt, "每日帳務調整回饋統計表", "AJSM","");

        }

        public ActionResult Report0318Result()
        {
            int year = DateTime.Today.Year;
            int month = DateTime.Today.Month;
            int day = DateTime.Today.Day;
            DateTime startDate = new DateTime(year, month, 1);
            DateTime endDate = new DateTime(year, month,
                                    DateTime.DaysInMonth(year, month));
            string start = startDate.ToString("yyyyMM");
            string end = endDate.ToString("yyyyMM");

            string merchantNo = "";
            if (Request.Form["searchConfirm"] != null || Request.Form["ExportExcel"] != null)
            {
                start = Request.Form["startDate"];
                end = start;
                merchantNo = Request.Form["Merchant"];
            }
            ViewBag.Start = start;
            //ViewBag.End = end;
            string MERC_GROUP = "BNK000";

            ViewBag.Merchant = SetMerchantBankDropDown(merchantNo);
            ViewBag.MerchantBankName = "";

            DataTable dtn = reportManager.MerchantName(merchantNo);
            if (dtn.Rows.Count > 0)
            {
                ViewBag.MerchantBankName = dtn.Rows[0][1].ToString();
                MERC_GROUP = "BNK" + dtn.Rows[0][2].ToString();
            }
            else
            {
                if (Request.Form["startDate"] != null)
                {
                    //Response.Write("<script>alert('');</script>"); return View();
                    //DisplayMessage();
                }
            }

            start = start + "01";
            DateTime startD = DateTime.Parse(start.Substring(0, 4) + "-" + start.Substring(4, 2) + "-" + start.Substring(6, 2)).AddMonths(-2);
            year = startD.Year;
            month = startD.Month;
            startD = new DateTime(year, month, 25);
            DateTime EndD = DateTime.Parse(start.Substring(0, 4) + "-" + start.Substring(4, 2) + "-" + start.Substring(6, 2)).AddMonths(-1);
            year = EndD.Year;
            month = EndD.Month;
            EndD = new DateTime(year, month, 24);
            ViewBag.Rang = startD.ToString("yyyyMMdd") + "~" + EndD.ToString("yyyyMMdd");

            DataTable dt = reportManager.Report0318T(start, end, merchantNo, MERC_GROUP);
            return Report316Result(dt, "自動加值手續費月報表", "ALDM","");

        }

        public ActionResult Report0319Result(string SDate)
        {
            int year = DateTime.Today.Year;
            int month = DateTime.Today.Month;
            int day = DateTime.Today.Day;
            DateTime startDate = new DateTime(year, month, 1);
            DateTime endDate = new DateTime(year, month,
                                    DateTime.DaysInMonth(year, month));
            string start = startDate.ToString("yyyyMM");
            string end = endDate.ToString("yyyyMM");

            string merchantNo = "";
            if (Request.Form["searchConfirm"] != null || Request.Form["ExportExcel"] != null)
            {
                start = Request.Form["startDate"];
                end = start;
                merchantNo = Request.Form["Merchant"];
            }
            //ViewBag.End = end;
            if (SDate != null)
            {
                start = SDate;
                //merchantNo = SDate.Substring(8, 8);
            }
            ViewBag.Start = start;
            string MERC_GROUP = "BNK000";
            ViewBag.Merchant = SetMerchantBankDropDown(merchantNo);
            ViewBag.MerchantBankName = "";

            DataTable dtn = reportManager.MerchantName(merchantNo);
            if (dtn.Rows.Count > 0)
            {
                ViewBag.MerchantBankName = dtn.Rows[0][1].ToString();
                MERC_GROUP = "BNK" + dtn.Rows[0][2].ToString();
            }
            else
            {
                if (Request.Form["startDate"] != null)
                {
                    //Response.Write("<script>alert('');</script>"); return View();
                    //DisplayMessage();
                }
            }

            start = start + "01";
            DateTime startD = DateTime.Parse(start.Substring(0, 4) + "-" + start.Substring(4, 2) + "-" + start.Substring(6, 2)).AddMonths(-2);
            year = startD.Year;
            month = startD.Month;
            startD = new DateTime(year, month, 25);
            DateTime EndD = DateTime.Parse(start.Substring(0, 4) + "-" + start.Substring(4, 2) + "-" + start.Substring(6, 2)).AddMonths(-1);
            year = EndD.Year;
            month = EndD.Month;
            EndD = new DateTime(year, month, 24);
            ViewBag.Rang = startD.ToString("yyyyMMdd") + "~" + EndD.ToString("yyyyMMdd");

            DataTable dt = reportManager.Report0319T(start, end, merchantNo, MERC_GROUP);
            return Report316Result(dt, "自動加值首次啟用月明細表", "FALD", ViewBag.Rang);

        }

        public ActionResult Report0322Result()
        {
            int year = DateTime.Today.Year;
            int month = DateTime.Today.Month;
            int day = DateTime.Today.Day;
            DateTime startDate = new DateTime(year, month, day);
            string start = startDate.ToString("yyyyMMdd");
            string merchantNo = "";
            string MERC_GROUP = "BNK000";
            if (Request.Form["searchConfirm"] != null || Request.Form["ExportExcel"] != null)
            {
                start = Request.Form["startDate"];
                merchantNo = Request.Form["Merchant"];
            }
            ViewBag.Start = start;
            ViewBag.Merchant = SetMerchantBankDropDown(merchantNo);
            ViewBag.MerchantBankName = "";

            DataTable dtn = reportManager.MerchantName(merchantNo);
            if (dtn.Rows.Count > 0)
            {
                ViewBag.MerchantBankName = dtn.Rows[0][1].ToString();
                MERC_GROUP = "BNK" + dtn.Rows[0][2].ToString();
            }
            else
            {
                if (Request.Form["startDate"] != null)
                {
                    //Response.Write("<script>alert('');</script>"); return View();
                    DisplayMessage();
                }
            }

            DataTable dt = reportManager.Report0322T(start, merchantNo, MERC_GROUP);
            return Report316Result(dt, "發卡機構日結款項彙總表", "TMCD", "");

        }

        public ActionResult Report0323Result(string SDate)
        {
            int year = DateTime.Today.Year;
            int month = DateTime.Today.Month;
            int day = DateTime.Today.Day;
            DateTime startDate = new DateTime(year, month, 1);
            DateTime endDate = new DateTime(year, month,
                                    DateTime.DaysInMonth(year, month));
            string start = startDate.ToString("yyyyMM");
            string end = endDate.ToString("yyyyMM");

            string merchantNo = "";
            if (Request.Form["searchConfirm"] != null || Request.Form["ExportExcel"] != null)
            {
                start = Request.Form["startDate"];
                end = start;
                merchantNo = Request.Form["Merchant"];
            }
            ViewBag.Start = start;
            //ViewBag.End = end;
            string MERC_GROUP = "BNK000";
            ViewBag.Merchant = SetMerchantBankDropDown(merchantNo);
            ViewBag.MerchantBankName = "";

            DataTable dtn = reportManager.MerchantName(merchantNo);
            if (dtn.Rows.Count > 0)
            {
                ViewBag.MerchantBankName = dtn.Rows[0][1].ToString();
                MERC_GROUP = "BNK" + dtn.Rows[0][2].ToString();
            }
            else
            {
                if (Request.Form["startDate"] != null)
                {
                    //Response.Write("<script>alert('');</script>"); return View();
                    DisplayMessage();
                }
            }

            start = start + "01";
            DateTime startD = DateTime.Parse(start.Substring(0, 4) + "-" + start.Substring(4, 2) + "-" + start.Substring(6, 2)).AddMonths(-2);
            year = startD.Year;
            month = startD.Month;
            startD = new DateTime(year, month, 25);
            DateTime EndD = DateTime.Parse(start.Substring(0, 4) + "-" + start.Substring(4, 2) + "-" + start.Substring(6, 2)).AddMonths(-1);
            year = EndD.Year;
            month = EndD.Month;
            EndD = new DateTime(year, month, 24);
            ViewBag.Rang = startD.ToString("yyyyMMdd") + "~" + EndD.ToString("yyyyMMdd");

            DataTable dt = reportManager.Report0323T(start, end, merchantNo, MERC_GROUP);
            return Report316Result(dt, "發卡機構月結款項彙總表", "TMCM", ViewBag.Rang);

        }

        public ActionResult Report0324Result()
        {
            int year = DateTime.Today.Year;
            int month = DateTime.Today.Month;
            int day = DateTime.Today.Day;
            DateTime startDate = new DateTime(year, month, 1);
            DateTime endDate = new DateTime(year, month,
                                    DateTime.DaysInMonth(year, month));
            string start = startDate.ToString("yyyyMM");
            string end = endDate.ToString("yyyyMM");

            string merchantNo = "";
            if (Request.Form["searchConfirm"] != null || Request.Form["ExportExcel"] != null)
            {
                start = Request.Form["startDate"];
                end = start;
                merchantNo = Request.Form["Merchant"];
            }
            ViewBag.Start = start;
            //ViewBag.End = end;
            string MERC_GROUP = "BNK000"; 
            ViewBag.Merchant = SetMerchantBankDropDown(merchantNo);
            ViewBag.MerchantBankName = "";

            DataTable dtn = reportManager.MerchantName(merchantNo);
            if (dtn.Rows.Count > 0)
            {
                ViewBag.MerchantBankName = dtn.Rows[0][1].ToString();
                MERC_GROUP = "BNK" + dtn.Rows[0][2].ToString();
            }
            else
            {
                if (Request.Form["startDate"] != null)
                {
                    //Response.Write("<script>alert('');</script>"); return View();
                    //DisplayMessage();
                }
            }

            string start2 = start + "01";
            DateTime startD = DateTime.Parse(start2.Substring(0, 4) + "-" + start2.Substring(4, 2) + "-" + start2.Substring(6, 2)).AddMonths(-2);
            year = startD.Year;
            month = startD.Month;
            startD = new DateTime(year, month, 25);
            DateTime EndD = DateTime.Parse(start2.Substring(0, 4) + "-" + start2.Substring(4, 2) + "-" + start2.Substring(6, 2)).AddMonths(-1);
            year = EndD.Year;
            month = EndD.Month;
            EndD = new DateTime(year, month, 24);
            ViewBag.Rang = startD.ToString("yyyyMMdd") + "~" + EndD.ToString("yyyyMMdd");

            DataTable dt = reportManager.Report0324T(start, end, merchantNo, MERC_GROUP);
            return Report316Result(dt, "發卡暨補換續卡統計月報表", "CNSM", "");

        }

        public ActionResult Report0327Result()
        {
            int year = DateTime.Today.Year;
            int month = DateTime.Today.Month;
            int day = DateTime.Today.Day;
            DateTime startDate = new DateTime(year, month, day);
            string start = startDate.ToString("yyyyMMdd");
            if (Request.Form["searchConfirm"] != null || Request.Form["ExportExcel"] != null)
            {
                start = Request.Form["startDate"];
            }
            ViewBag.Start = start;

            DataTable dt = reportManager.Report0327T(start);
            return Report316Result(dt, "已餘返異常交易明細表", "0327", "");

        }


        public ActionResult Report316Result(DataTable dt, string RepName, string RepID, string Rang)
        {

            ViewBag.Count = dt.Rows.Count;

            //return View(dt);
            if (Request.Form["ExportExcel"] != null) //產生標籤Excel
            {
                //建立Excel 2003檔案
                IWorkbook wb = new HSSFWorkbook();
                ISheet ws;
                //ISheet ws2;

                ////建立Excel 2007檔案
                //IWorkbook wb = new XSSFWorkbook();
                //ISheet ws;
                ws = wb.CreateSheet(RepName);

                var headerRow = ws.CreateRow(3);//第一行為欄位名稱

                HSSFCellStyle cs = (HSSFCellStyle)wb.CreateCellStyle();
                //啟動多行文字
                //cs.WrapText = true;
                //文字置中
                cs.VerticalAlignment = VerticalAlignment.Center;
                cs.Alignment = HorizontalAlignment.Center;
                //框線樣式及顏色
                cs.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
                cs.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                cs.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
                cs.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
                cs.BottomBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                cs.LeftBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                cs.RightBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                cs.TopBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                //背景顏色
                cs.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.Grey25Percent.Index;
                cs.FillPattern = NPOI.SS.UserModel.FillPattern.SolidForeground;
                HSSFFont font1 = (HSSFFont)wb.CreateFont();
                //字體顏色
                font1.Color = NPOI.HSSF.Util.HSSFColor.Black.Index;
                //字體粗體
                //font1.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.Bold;
                //字體尺寸
                font1.FontHeightInPoints = 12;
                cs.SetFont(font1);

                headerRow.HeightInPoints = 20;

                headerRow = ws.CreateRow(4);
                if (RepID == "0327") headerRow = ws.CreateRow(3);

                headerRow.HeightInPoints = 20;

                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    var cell = headerRow.CreateCell(i);
                    cell.SetCellValue(dt.Columns[i].ColumnName);
                    cell.CellStyle = cs;

                    //ws.AutoSizeColumn(i);
                    if (i == 0)
                        ws.SetColumnWidth(0, 8 * 256);
                    else if (i == 2)
                        ws.SetColumnWidth(i, 28 * 256);
                    else if (i == 5)
                        ws.SetColumnWidth(i, 30 * 256);
                    else
                        ws.SetColumnWidth(i, 20 * 256);
                    if (RepID == "AJDM")
                    {
                        ws.SetColumnWidth(1, 12 * 256);
                        ws.SetColumnWidth(2, 20 * 256);
                        ws.SetColumnWidth(5, 20 * 256);
                        ws.SetColumnWidth(6, 30 * 256);
                    }
                    if (RepID == "AJSM" || RepID=="CNSM")
                    {
                        ws.SetColumnWidth(0, 18 * 256);
                        ws.SetColumnWidth(2, 20 * 256);
                        ws.SetColumnWidth(3, 30 * 256);
                        ws.SetColumnWidth(4, 30 * 256);
                    }
                    if (RepID == "ALDM" || RepID == "TMCD" || RepID == "TMCM")
                    {
                        ws.SetColumnWidth(0, 18 * 256);
                        ws.SetColumnWidth(1, 20 * 256);
                        ws.SetColumnWidth(2, 20 * 256);
                    }
                    if (RepID == "0327")
                    {
                        ws.SetColumnWidth(0, 26 * 256);
                        ws.SetColumnWidth(1, 20 * 256);
                        ws.SetColumnWidth(2, 20 * 256);
                        ws.SetColumnWidth(3, 20 * 256);
                        ws.SetColumnWidth(4, 20 * 256);
                        ws.SetColumnWidth(5, 20 * 256);
                    }
                    if (RepID == "0120")
                    {
                        ws.SetColumnWidth(0, 18 * 256);
                        ws.SetColumnWidth(1, 18 * 256);
                        ws.SetColumnWidth(2, 18 * 256);
                        ws.SetColumnWidth(5, 20 * 256);
                    }
                    if (RepID == "0121")
                    {
                        ws.SetColumnWidth(0, 20 * 256);
                        ws.SetColumnWidth(1, 24 * 256);
                    }
                }
                //ws.AddMergedRegion(new CellRangeAddress(3, 4, 0, 0));
                //ws.AddMergedRegion(new CellRangeAddress(3, 4, 7, 7));

                ICellStyle style1 = wb.CreateCellStyle();
                //框線樣式及顏色
                style1.Alignment = HorizontalAlignment.Left;
                style1.VerticalAlignment = VerticalAlignment.Center;
                //字體尺寸
                font1.FontHeightInPoints = 11;
                style1.SetFont(font1);

                ICellStyle style2 = wb.CreateCellStyle();
                //框線樣式及顏色
                style2.Alignment = HorizontalAlignment.Center;
                style2.VerticalAlignment = VerticalAlignment.Center;
                //字體尺寸
                font1.FontHeightInPoints = 11;
                style2.SetFont(font1);

                //fill content 
                ICellStyle style3 = wb.CreateCellStyle();
                //框線樣式及顏色
                style3.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
                style3.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                style3.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
                style3.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
                style3.BottomBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                style3.LeftBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                style3.RightBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                style3.TopBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                style3.Alignment = HorizontalAlignment.Left;
                style3.VerticalAlignment = VerticalAlignment.Center;
                //字體尺寸
                font1.FontHeightInPoints = 11;
                style3.SetFont(font1);

                // Create the style object
                var currencyCellStyle = wb.CreateCellStyle();
                // Right-align currency values
                currencyCellStyle.Alignment = HorizontalAlignment.Right;
                currencyCellStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
                currencyCellStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                currencyCellStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
                currencyCellStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
                currencyCellStyle.BottomBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                currencyCellStyle.LeftBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                currencyCellStyle.RightBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                currencyCellStyle.TopBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                font1.FontHeightInPoints = 11;
                currencyCellStyle.SetFont(font1);
                currencyCellStyle.DataFormat = wb.CreateDataFormat().GetFormat("#,##0");

                ws.CreateRow(0).HeightInPoints = 20; ;
                var cell3 = ws.GetRow(0).CreateCell(0);
                cell3.SetCellValue(RepName);
                cell3.CellStyle = style2;
                if (RepID == "0121")
                    ws.AddMergedRegion(new CellRangeAddress(0, 0, 0, 2));
                if (RepID == "0120")
                    ws.AddMergedRegion(new CellRangeAddress(0, 0, 0, 7));
                if (RepID == "0327")
                    ws.AddMergedRegion(new CellRangeAddress(0, 0, 0, 5));
                if (RepID == "AJDM")
                    ws.AddMergedRegion(new CellRangeAddress(0, 0, 0, 6));
                if (RepID == "AJSM")
                    ws.AddMergedRegion(new CellRangeAddress(0, 0, 0, 4));
                if (RepID == "TMCD" || RepID == "TMCM")
                    ws.AddMergedRegion(new CellRangeAddress(0, 0, 0, 2));
                if (RepID == "ALDM" )
                    ws.AddMergedRegion(new CellRangeAddress(0, 0, 0, 3));
                if (RepID == "FALD")
                    ws.AddMergedRegion(new CellRangeAddress(0, 0, 0, 5));
                if (RepID == "CNSM")
                    ws.AddMergedRegion(new CellRangeAddress(0, 0, 0, 10));
                var rang = ws.CreateRow(1);
                cell3 = rang.CreateCell(0);
                cell3.CellStyle = style1;
                cell3.SetCellValue("報表編號："+ RepID);

                rang = ws.CreateRow(2);
                cell3 = rang.CreateCell(0);
                cell3.CellStyle = style1;
                if (RepID != "0327" && RepID != "0120" && RepID != "0121")
                {
                    cell3.SetCellValue("銀行名稱：" + ViewBag.MerchantBankName);
                    ws.AddMergedRegion(new CellRangeAddress(2, 2, 0, 1));
                    rang = ws.CreateRow(3);
                    cell3 = rang.CreateCell(0);
                    cell3.CellStyle = style1;
                    cell3.SetCellValue("清算日期：" + ViewBag.Start);
                }
                else
                {
                    cell3.SetCellValue("清算日期：" + ViewBag.Start);
                }
                if (RepID == "ALDM" || RepID == "FALD")
                {
                    cell3.SetCellValue("清算年月：" + ViewBag.Start );
                }
                if (RepID == "CNSM")
                {
                    cell3.SetCellValue("統計年月：" + ViewBag.Start);
                }
                if (RepID == "0121")
                {
                    rang = ws.CreateRow(1);
                    cell3 = rang.CreateCell(0);
                    cell3.SetCellValue("年月：" + ViewBag.Start );
                    rang = ws.CreateRow(2);
                    cell3 = rang.CreateCell(1);
                    cell3.CellStyle = style1;
                    cell3.SetCellValue("目前 idollar未轉置餘額：");
                    cell3 = rang.CreateCell(2);
                    cell3.SetCellValue(Convert.ToDouble(ViewBag.Idollar.ToString()));
                    cell3.CellStyle = currencyCellStyle;
                }

                if (RepID == "0120")
                {
                    cell3.SetCellValue("清算日期：" + ViewBag.Start + " ~ " + ViewBag.End);
                }
                if (RepID == "TMCM")
                {
                    cell3.SetCellValue("結算年月：" + ViewBag.Start + "    清算週期：" + Rang);
                }
                if (RepID == "FALD")
                {
                    cell3 = rang.CreateCell(2);
                    cell3.CellStyle = style1;
                    cell3.SetCellValue("清算週期：" + Rang);
                }
                //ws.AddMergedRegion(new CellRangeAddress(3, 3, 0, 1));

                int ii = 4;
                if (RepID == "0327") ii = 3;

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ii = ii + 1;
                    ws.CreateRow(ii);
                    var cell2 = ws.GetRow(ii).CreateCell(0);
                    cell2.CellStyle = currencyCellStyle;
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        cell2 = ws.GetRow(ii).CreateCell(j);
                        cell2.SetCellValue(dt.Rows[i][j].ToString());
                        cell2.CellStyle = style3;
                        if (j == 2 && RepID == "0121")
                        {
                            cell2.SetCellValue(Convert.ToDouble(dt.Rows[i][j].ToString()));
                            cell2.CellStyle = currencyCellStyle;
                        }
                        if (j == 7 && RepID == "0120")
                        {
                            cell2.SetCellValue(Convert.ToDouble(dt.Rows[i][j].ToString()));
                            cell2.CellStyle = currencyCellStyle;
                        }
                        if (j == 5 && RepID == "AJDM")
                        {
                            cell2.SetCellValue(Convert.ToDouble(dt.Rows[i][j].ToString()));
                            cell2.CellStyle = currencyCellStyle;
                        }
                        if ((j == 3 || j == 4) && RepID == "AJSM")
                        {
                            cell2.SetCellValue(Convert.ToDouble(dt.Rows[i][j].ToString()));
                            cell2.CellStyle = currencyCellStyle;
                        }
                        if ((j == 2 || j == 3) && RepID == "ALDM")
                        {
                            cell2.SetCellValue(Convert.ToDouble(dt.Rows[i][j].ToString()));
                            cell2.CellStyle = currencyCellStyle;
                        }
                        if ((j == 1) && (RepID == "TMCD" || RepID == "TMCM"))
                        {
                            cell2.SetCellValue(Convert.ToDouble(dt.Rows[i][j].ToString()));
                            cell2.CellStyle = currencyCellStyle;
                        }
                        if (j >= 2 && (RepID == "CNSM" || RepID == "0327"))
                        {
                            cell2.SetCellValue(Convert.ToDouble(dt.Rows[i][j].ToString()));
                            cell2.CellStyle = currencyCellStyle;
                        }
                    }
                }

                var stream = new MemoryStream();
                wb.Write(stream);
                stream.Close();

                return File(stream.ToArray(), "application/vnd.ms-excel", RepName +"_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls");


            }
            else
            {
                return View(dt);
            }

        }


        public ActionResult Report0411Result()
        {
            int year = DateTime.Today.Year;
            int month = DateTime.Today.Month;
            DateTime startDate = new DateTime(year, month, 1);
            DateTime endDate = new DateTime(year, month,
                                    DateTime.DaysInMonth(year, month));
            string start = startDate.ToString("yyyyMM");
            string end = endDate.ToString("yyyyMM");

            string merchantNo = "";
            string CARD_TYPE = "21";
            if (Request.Form["searchConfirm"] != null || Request.Form["ExportExcel"] != null )
            {
                start = Request.Form["startDate"];
                end = start;
                merchantNo = Request.Form["Merchant"];
                CARD_TYPE = Request.Form["CARD_TYPE"];
            }
            ViewBag.Start = start;
            //ViewBag.End = end;
            ViewBag.Merchant = SetMerchantDropDown(merchantNo);
            ViewBag.MerchantName = "";
            if (CARD_TYPE == "21")
            {
                ViewBag.ch1 = "checked";
                ViewBag.ch2 = "";
            }
            else
            {
                ViewBag.ch1 = "";
                ViewBag.ch2 = "checked";
            }
            
            DataTable dtn = reportManager.MerchantName(merchantNo);
            if (dtn.Rows.Count > 0)
            {
                ViewBag.MerchantName = dtn.Rows[0][1].ToString();
            }
            else
                ViewBag.MerchantName = "全部";

            DataTable dt = reportManager.Report0411T(start, end, merchantNo, CARD_TYPE);
            ViewBag.Count = dt.Rows.Count;

            //return View(dt);
            if (Request.Form["ExportExcel"] != null) //產生標籤Excel
            {
                //建立Excel 2003檔案
                IWorkbook wb = new HSSFWorkbook();
                ISheet ws;
                //ISheet ws2;

                ////建立Excel 2007檔案
                //IWorkbook wb = new XSSFWorkbook();
                //ISheet ws;
                ws = wb.CreateSheet("電子票證發行資料統計表");

                //ws.CreateRow(4);//第一行為欄位名稱
                var headerRow = ws.CreateRow(4);

                HSSFCellStyle cs = (HSSFCellStyle)wb.CreateCellStyle();
                //啟動多行文字
                //cs.WrapText = true;
                //文字置中
                cs.VerticalAlignment = VerticalAlignment.Center;
                cs.Alignment = HorizontalAlignment.Center;
                //框線樣式及顏色
                cs.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
                cs.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                cs.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
                cs.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
                cs.BottomBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                cs.LeftBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                cs.RightBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                cs.TopBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                //背景顏色
                cs.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.Grey25Percent.Index;
                cs.FillPattern = NPOI.SS.UserModel.FillPattern.SolidForeground;
                HSSFFont font1 = (HSSFFont)wb.CreateFont();
                //字體顏色
                font1.Color = NPOI.HSSF.Util.HSSFColor.Black.Index;
                //字體粗體
                //font1.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.Bold;
                //字體尺寸
                font1.FontHeightInPoints = 12;
                cs.SetFont(font1);

                headerRow.HeightInPoints = 24;


                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    var cell = headerRow.CreateCell(i);
                    cell.SetCellValue(dt.Columns[i].ColumnName);
                    cell.CellStyle = cs;
                    //ws.AutoSizeColumn(i);
                    if (i == 0)
                        ws.SetColumnWidth(0, 28 * 256);
                    else if (i == 1)
                        ws.SetColumnWidth(1, 24 * 256);
                    else
                        ws.SetColumnWidth(i, 6 * 256);
                }

                ICellStyle style2 = wb.CreateCellStyle();
                //框線樣式及顏色
                style2.Alignment = HorizontalAlignment.Center;
                style2.VerticalAlignment = VerticalAlignment.Center;
                //字體尺寸
                font1.FontHeightInPoints = 20;
                style2.SetFont(font1);

                //fill content 
                ICellStyle style3 = wb.CreateCellStyle();
                //框線樣式及顏色
                style3.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
                style3.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                style3.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
                style3.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
                style3.BottomBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                style3.LeftBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                style3.RightBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                style3.TopBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                style3.Alignment = HorizontalAlignment.Left;
                style3.VerticalAlignment = VerticalAlignment.Center;
                //字體尺寸
                font1.FontHeightInPoints = 11;
                style3.SetFont(font1);
                //style3.FillForegroundColor = IndexedColors.LemonChiffon.Index;
                //style3.FillPattern = FillPattern.SolidForeground;
                //style3.FillBackgroundColor = IndexedColors.LightGreen.Index;


                // Create the style object
                var currencyCellStyle = wb.CreateCellStyle();
                // Right-align currency values
                currencyCellStyle.Alignment = HorizontalAlignment.Right;
                currencyCellStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
                currencyCellStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                currencyCellStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
                currencyCellStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
                currencyCellStyle.BottomBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                currencyCellStyle.LeftBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                currencyCellStyle.RightBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                currencyCellStyle.TopBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                font1.FontHeightInPoints = 11;
                currencyCellStyle.SetFont(font1);
                // Get / create the data format string
                var formatId = HSSFDataFormat.GetBuiltinFormat("#,##0");
                //if (formatId == -1)
                //{
                //    var newDataFormat = wb.CreateDataFormat();
                //    currencyCellStyle.DataFormat = newDataFormat.GetFormat("#,##0");
                //}
                //else
                    currencyCellStyle.DataFormat = formatId;

                    ws.CreateRow(0).HeightInPoints = 20; ;
                    var cell3 = ws.GetRow(0).CreateCell(0);
                    cell3.SetCellValue("電子票證發行資料統計表");
                    cell3.CellStyle = style2;
                    ws.AddMergedRegion(new CellRangeAddress(0, 0, 0, 2));

                    ws.CreateRow(1);
                    cell3 = ws.GetRow(1).CreateCell(0);
                    cell3.SetCellValue("機構名稱");
                    cell3.CellStyle = style3;
                    cell3 = ws.GetRow(1).CreateCell(1);
                    cell3.SetCellValue(ViewBag.MerchantName);
                    cell3.CellStyle = style3;
                    cell3 = ws.GetRow(1).CreateCell(2);
                    cell3.CellStyle = style3;
                    ws.AddMergedRegion(new CellRangeAddress(1, 1, 1, 2));

                    ws.CreateRow(2);
                    cell3 = ws.GetRow(2).CreateCell(0);
                    cell3.SetCellValue("資料月");
                    cell3.CellStyle = style3;
                    cell3 = ws.GetRow(2).CreateCell(1);
                    cell3.SetCellValue(ViewBag.Start );
                    cell3.CellStyle = style3;
                    cell3 = ws.GetRow(2).CreateCell(2);
                    cell3.CellStyle = style3;
                    ws.AddMergedRegion(new CellRangeAddress(2, 2, 1, 2));

                    ws.CreateRow(3);
                    cell3 = ws.GetRow(3).CreateCell(0);
                    cell3.SetCellValue("卡片名稱");
                    cell3.CellStyle = style3;
                    if (CARD_TYPE == "21")
                    {
                        cell3 = ws.GetRow(3).CreateCell(1);
                        cell3.SetCellValue("icash 2.0");
                        cell3.CellStyle = style3;
                    }
                    else
                    {
                        cell3 = ws.GetRow(3).CreateCell(1);
                        cell3.CellStyle = style3;
                        cell3.SetCellValue("icash");
                    }
                    cell3 = ws.GetRow(3).CreateCell(2);
                    cell3.CellStyle = style3;
                    ws.AddMergedRegion(new CellRangeAddress(3, 3, 1, 2));


                int ii = 4;

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ii = ii + 1;
                    ws.CreateRow(ii );
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        var cell2 = ws.GetRow(ii).CreateCell(j);
                        if (j == 1)
                        {
                            //cell2.SetCellValue(Convert.ToDouble(dt.Rows[i][j].ToString()));
                            //total = Convert.ToDouble(dt.Rows[i][j].ToString());
                            cell2.SetCellValue(Convert.ToDouble(dt.Rows[i][j].ToString()));
                            cell2.CellStyle = currencyCellStyle;
                        }
                        else
                        {
                            cell2.SetCellValue(dt.Rows[i][j].ToString());
                            cell2.CellStyle = style3;
                        }

                    }
                }

                var stream = new MemoryStream();
                wb.Write(stream);
                stream.Close();

                return File(stream.ToArray(), "application/vnd.ms-excel", "電子票證發行資料統計表_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls");


            }
            else
            {
                return View(dt);
            }

        }

        public ActionResult Report0412Result()
        {
            int year = DateTime.Today.Year;
            int month = DateTime.Today.Month;
            DateTime startDate = new DateTime(year, month, 1);
            DateTime endDate = new DateTime(year, month,
                                    DateTime.DaysInMonth(year, month));
            string start = startDate.ToString("yyyyMM");
            string end = endDate.ToString("yyyyMM");
            string merchantNo = "";
            if (Request.Form["searchConfirm"] != null || Request.Form["ExportExcel"] != null)
            {
                start = Request.Form["startDate"];
                end = Request.Form["endDate"];
                merchantNo = Request.Form["Merchant"];
            }
            ViewBag.Start = start;
            ViewBag.End = end;
            ViewBag.Merchant = SetMerchantDropDown(merchantNo);
            ViewBag.MerchantName = "";

            DataTable dtn = reportManager.MerchantName(merchantNo);
            if (dtn.Rows.Count > 0)
            {
                ViewBag.MerchantName = dtn.Rows[0][1].ToString();
            }
            else
                ViewBag.MerchantName = "全部";

            DataTable dt = reportManager.Report0412T(start, end, merchantNo);
            ViewBag.Count = dt.Rows.Count;

            //return View(dt);
            if (Request.Form["ExportExcel"] != null) //產生標籤Excel
            {
                //建立Excel 2003檔案
                IWorkbook wb = new HSSFWorkbook();
                ISheet ws;
                //ISheet ws2;

                ////建立Excel 2007檔案
                //IWorkbook wb = new XSSFWorkbook();
                //ISheet ws;
                ws = wb.CreateSheet("電子票證應用安全強度準則應申報資料統計表");

                //ws.CreateRow(3);//第一行為欄位名稱
                var headerRow = ws.CreateRow(3);

                HSSFCellStyle cs = (HSSFCellStyle)wb.CreateCellStyle();
                //啟動多行文字
                //cs.WrapText = true;
                //文字置中
                cs.VerticalAlignment = VerticalAlignment.Center;
                cs.Alignment = HorizontalAlignment.Center;
                //框線樣式及顏色
                cs.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
                cs.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                cs.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
                cs.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
                cs.BottomBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                cs.LeftBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                cs.RightBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                cs.TopBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                //背景顏色
                cs.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.Grey25Percent.Index;
                cs.FillPattern = NPOI.SS.UserModel.FillPattern.SolidForeground;
                HSSFFont font1 = (HSSFFont)wb.CreateFont();
                //字體顏色
                font1.Color = NPOI.HSSF.Util.HSSFColor.Black.Index;
                //字體粗體
                //font1.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.Bold;
                //字體尺寸
                font1.FontHeightInPoints = 12;
                cs.SetFont(font1);

                headerRow.HeightInPoints = 24;

                var cell = headerRow.CreateCell(0);
                cell.SetCellValue(dt.Columns[0].ColumnName);
                cell.CellStyle = cs;
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    cell = headerRow.CreateCell(i+1);
                    cell.SetCellValue(dt.Columns[i].ColumnName);
                    cell.CellStyle = cs;

                    //ws.AutoSizeColumn(i);
                    if (i == 0)
                        ws.SetColumnWidth(0, 18 * 256);
                    else if (i == 1)
                        ws.SetColumnWidth(1, 38 * 256);
                    else if (i == 2)
                        ws.SetColumnWidth(i, 20 * 256);
                    else
                        ws.SetColumnWidth(i, 6 * 256);
                }
                ws.AddMergedRegion(new CellRangeAddress(3, 3, 0, 1));
                ws.AddMergedRegion(new CellRangeAddress(3, 3, 2, 3));

                ICellStyle style2 = wb.CreateCellStyle();
                //框線樣式及顏色
                style2.Alignment = HorizontalAlignment.Center;
                style2.VerticalAlignment = VerticalAlignment.Center;
                //字體尺寸
                font1.FontHeightInPoints = 20;
                style2.SetFont(font1);

                //fill content 
                ICellStyle style3 = wb.CreateCellStyle();
                //框線樣式及顏色
                style3.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
                style3.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                style3.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
                style3.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
                style3.BottomBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                style3.LeftBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                style3.RightBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                style3.TopBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                style3.Alignment = HorizontalAlignment.Left;
                style3.VerticalAlignment = VerticalAlignment.Center;
                //字體尺寸
                font1.FontHeightInPoints = 11;
                style3.SetFont(font1);
                //style3.FillForegroundColor = IndexedColors.LemonChiffon.Index;
                //style3.FillPattern = FillPattern.SolidForeground;
                //style3.FillBackgroundColor = IndexedColors.LightGreen.Index;


                // Create the style object
                var currencyCellStyle = wb.CreateCellStyle();
                // Right-align currency values
                currencyCellStyle.Alignment = HorizontalAlignment.Right;
                currencyCellStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
                currencyCellStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                currencyCellStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
                currencyCellStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
                currencyCellStyle.BottomBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                currencyCellStyle.LeftBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                currencyCellStyle.RightBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                currencyCellStyle.TopBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                font1.FontHeightInPoints = 11;
                currencyCellStyle.SetFont(font1);
                // Get / create the data format string
                //var formatId = HSSFDataFormat.GetBuiltinFormat("#,##0");
                //if (formatId == -1)
                //{
                //    var newDataFormat = wb.CreateDataFormat();
                //    currencyCellStyle.DataFormat = newDataFormat.GetFormat("#,##0");
                //}
                //else
                //currencyCellStyle.DataFormat = formatId;
                //HSSFDataFormat myformat = wb.CreateDataFormat();
                //ICellStyle newDataFormat = wb.CreateCellStyle();
                currencyCellStyle.DataFormat = wb.CreateDataFormat().GetFormat("#,##0");

                ws.CreateRow(0).HeightInPoints = 20; ;
                var cell3 = ws.GetRow(0).CreateCell(0);
                cell3.SetCellValue("電子票證應用安全強度準則應申報資料統計表");
                cell3.CellStyle = style2;
                ws.AddMergedRegion(new CellRangeAddress(0, 0, 0, 3));

                ws.CreateRow(1);
                cell3 = ws.GetRow(1).CreateCell(0);
                cell3.SetCellValue("機構名稱");
                cell3.CellStyle = style3;
                cell3 = ws.GetRow(1).CreateCell(1);
                cell3.CellStyle = style3;
                cell3 = ws.GetRow(1).CreateCell(2);
                cell3.SetCellValue(ViewBag.MerchantName);
                cell3.CellStyle = style3;
                cell3 = ws.GetRow(1).CreateCell(3);
                cell3.CellStyle = style3;
                ws.AddMergedRegion(new CellRangeAddress(1, 1, 0, 1));
                ws.AddMergedRegion(new CellRangeAddress(1, 1, 2, 3));

                ws.CreateRow(2);
                cell3 = ws.GetRow(2).CreateCell(0);
                cell3.SetCellValue("資料區間");
                cell3.CellStyle = style3;
                cell3 = ws.GetRow(2).CreateCell(1);
                cell3.CellStyle = style3;
                cell3 = ws.GetRow(2).CreateCell(2);
                cell3.SetCellValue(ViewBag.Start + "~" + ViewBag.End);
                cell3.CellStyle = style3;
                cell3 = ws.GetRow(2).CreateCell(3);
                cell3.CellStyle = style3;
                ws.AddMergedRegion(new CellRangeAddress(2, 2, 0, 1));
                ws.AddMergedRegion(new CellRangeAddress(2, 2, 2, 3));


                int ii = 3;

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ii = ii + 1;
                    ws.CreateRow(ii);
                    var cell2 = ws.GetRow(ii).CreateCell(0);
                    if (ii == 4) cell2.SetCellValue("第一類");
                    if (ii == 6) cell2.SetCellValue("第二類");
                    if (ii == 8) cell2.SetCellValue("異常交易金額");
                    cell2.CellStyle = currencyCellStyle;
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        cell2 = ws.GetRow(ii).CreateCell(j+1);
                        if (j == 1)
                        {
                            //cell2.SetCellValue(Convert.ToDouble(dt.Rows[i][j].ToString()));
                            //total = Convert.ToDouble(dt.Rows[i][j].ToString());
                            cell2.SetCellValue(Convert.ToDouble(dt.Rows[i][j].ToString()));
                            cell2.CellStyle = currencyCellStyle;
                        }
                        else
                        {
                            cell2.SetCellValue(dt.Rows[i][j].ToString());
                            cell2.CellStyle = style3;
                        }

                    }
                }
                ws.AddMergedRegion(new CellRangeAddress(4, 5, 0, 0));
                ws.AddMergedRegion(new CellRangeAddress(6, 7, 0, 0));
                ws.AddMergedRegion(new CellRangeAddress(8, 11, 0, 0));

                var stream = new MemoryStream();
                wb.Write(stream);
                stream.Close();

                return File(stream.ToArray(), "application/vnd.ms-excel", "電子票證應用安全強度準則應申報資料統計表_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls");


            }
            else
            {
                return View(dt);
            }

        }


        public ActionResult Report0811Index()
        {
            string startYMD = DateTime.Today.ToString("yyyyMMdd");
            ViewBag.StartYMD = startYMD;
            return View();
        }

        [HttpPost]
        public ActionResult Report0811Export()
        {
            string startYMD = Request.Form["StartDate"];
            ViewBag.StartYMD = startYMD;

            DataTable dt = reportManager.Report0811(startYMD);

            var string_with_your_data1 = "";
            char[] charsToTrim = { ',' };
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (i > 0)
                    string_with_your_data1 += "\r\n";

                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    string_with_your_data1 += "\"" + dt.Rows[i][j].ToString() + "\",";
                }
                string_with_your_data1 = string_with_your_data1.TrimEnd(charsToTrim);
            }
            byte[] data = System.Text.Encoding.Default.GetBytes(string_with_your_data1);
            var stream = new MemoryStream(data);

            return File(stream, "text/plain", "X" + startYMD.Substring(2,6) + "001_TM-DSC_DAILYPAY-" + startYMD + ".dat");

            //return File(stream.ToArray(), "application/vnd.ms-excel", "愛金卡儲值餘額表_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls");
        }

        public ActionResult ReportA03Index()
        {
            string startYM = DateTime.Today.ToString("yyyyMM");
            ViewBag.StartYM = startYM;
            return View();
        }

        [HttpPost]
        public ActionResult ReportA03Export()
        {
            string startYM = Request.Form["StartDate"];
            ViewBag.StartYM = startYM;
            string sYear = startYM.Substring(0, 4);
            string sMonth = startYM.Substring(4, 2);
            try
            {
                HSSFWorkbook workbook = new HSSFWorkbook();
                ISheet sheet = (ISheet)workbook.CreateSheet("信託餘額表");
                int colCnt = 10;

                //合併儲存格
                sheet.AddMergedRegion(new CellRangeAddress(0, 0, 0, 9));
                sheet.AddMergedRegion(new CellRangeAddress(1, 1, 0, 9));
                sheet.AddMergedRegion(new CellRangeAddress(2, 2, 0, 9));


                sheet.AddMergedRegion(new CellRangeAddress(3, 3, 8, 9));
                sheet.AddMergedRegion(new CellRangeAddress(4, 5, 0, 0));
                sheet.AddMergedRegion(new CellRangeAddress(4, 5, 1, 1));
                sheet.AddMergedRegion(new CellRangeAddress(4, 5, 3, 3));
                sheet.AddMergedRegion(new CellRangeAddress(4, 4, 5, 7));
                sheet.AddMergedRegion(new CellRangeAddress(4, 5, 9, 9));

                //設定欄寬
                for (int i = 0; i < colCnt; i++)
                {
                    sheet.SetColumnWidth(i, 21 * 256);
                }
                sheet.SetColumnWidth(0, 18 * 256);
                sheet.SetColumnWidth(1, 8 * 256);
                sheet.SetColumnWidth(2, 1 * 256);
                sheet.SetColumnWidth(4, 1 * 256);
                sheet.SetColumnWidth(8, 1 * 256);
                sheet.SetColumnWidth(9, 26 * 256);

                //
                //報表主題樣式======================================
                HSSFCellStyle titleStyle = (HSSFCellStyle)workbook.CreateCellStyle();
                titleStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
                titleStyle.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center;
                ///報表主題字體
                HSSFFont titleFont = (HSSFFont)workbook.CreateFont();
                titleFont.FontHeightInPoints = 22;
                titleFont.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.Bold;
                titleFont.Color = NPOI.HSSF.Util.HSSFColor.Black.Index;
                titleFont.FontName = "Consolas";
                titleStyle.SetFont(titleFont);
                //框線樣式
                titleStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.None;
                titleStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.None;
                titleStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.None;
                titleStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.None;

                //
                //特殊日期樣式======================================
                HSSFCellStyle speDateStyle = (HSSFCellStyle)workbook.CreateCellStyle();
                //speDateStyle.DataFormat = (workbook.CreateDataFormat()).GetFormat("yyyy/m/d");

                //speDateStyle.DataFormat = speDateFormat.GetFormat("[$-404]e\"年\"m\"月\";@");
                speDateStyle.DataFormat = workbook.CreateDataFormat().GetFormat("[$-404]e\"年\"m\"月\";@");
                //speDateStyle.DataFormat = HSSFDataFormat.GetBuiltinFormat("m/d/yy h:mm");


                speDateStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
                speDateStyle.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center;
                //特殊日期字體
                HSSFFont speDateFont = (HSSFFont)workbook.CreateFont();
                speDateFont.FontHeightInPoints = 12;
                speDateFont.Color = NPOI.HSSF.Util.HSSFColor.Black.Index;
                titleFont.FontName = "Consolas";
                speDateStyle.SetFont(speDateFont);
                //框線樣式
                speDateStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.None;
                speDateStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.None;
                speDateStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.None;
                speDateStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.None;

                //
                // 條件樣式======================================
                HSSFCellStyle condStyle = (HSSFCellStyle)workbook.CreateCellStyle();
                condStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
                condStyle.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center;
                //標題列字體
                HSSFFont condFont = (HSSFFont)workbook.CreateFont();
                condFont.FontHeightInPoints = 12;
                condFont.Color = NPOI.HSSF.Util.HSSFColor.Black.Index;
                titleFont.FontName = "Consolas";
                condStyle.SetFont(condFont);
                //框線樣式
                condStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.None;
                condStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.None;
                condStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.None;
                condStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.None;

                //
                //標題列樣式======================================
                HSSFCellStyle headStyle = (HSSFCellStyle)workbook.CreateCellStyle();
                headStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
                headStyle.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center;
                //標題列背景顏色
                headStyle.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.Grey25Percent.Index;
                headStyle.FillPattern = NPOI.SS.UserModel.FillPattern.SolidForeground;
                //標題列字體
                HSSFFont headFont = (HSSFFont)workbook.CreateFont();
                headFont.FontHeightInPoints = 12;
                headFont.Color = NPOI.HSSF.Util.HSSFColor.Black.Index;
                titleFont.FontName = "Consolas";
                headStyle.SetFont(headFont);
                //框線樣式
                headStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
                headStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                headStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
                headStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;

                //
                //資料列(日期)樣式====================================
                HSSFCellStyle rowDateStyle = (HSSFCellStyle)workbook.CreateCellStyle();
                rowDateStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
                rowDateStyle.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center;
                //設定日期格式
                rowDateStyle.DataFormat = (workbook.CreateDataFormat()).GetFormat("yyyy/m/d");

                //字體
                HSSFFont rowDateFont = (HSSFFont)workbook.CreateFont();
                rowDateFont.FontHeightInPoints = 12;
                titleFont.FontName = "Consolas";
                rowDateStyle.SetFont(rowDateFont);
                //框線樣式
                rowDateStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
                rowDateStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                rowDateStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
                rowDateStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;

                //
                //資料列(文字)樣式====================================
                HSSFCellStyle rowTextStyle = (HSSFCellStyle)workbook.CreateCellStyle();
                rowTextStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
                rowTextStyle.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center;

                //資料列字體
                HSSFFont rowTextFont = (HSSFFont)workbook.CreateFont();
                rowTextFont.FontHeightInPoints = 12;
                titleFont.FontName = "Consolas";
                rowTextStyle.SetFont(rowTextFont);
                //框線樣式
                rowTextStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
                rowTextStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                rowTextStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
                rowTextStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;

                //資料列(數字)樣式====================================
                HSSFCellStyle rowNumberStyle = (HSSFCellStyle)workbook.CreateCellStyle();
                rowNumberStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Right;
                rowNumberStyle.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center;
                //格式
                HSSFDataFormat format = (HSSFDataFormat)workbook.CreateDataFormat();
                rowNumberStyle.DataFormat = format.GetFormat("#,##0;[Red](#,##0)");
                //資料列字體
                HSSFFont rowNumberFont = (HSSFFont)workbook.CreateFont();
                rowNumberFont.FontHeightInPoints = 12;
                titleFont.FontName = "Consolas";
                rowNumberStyle.SetFont(rowNumberFont);
                //框線樣式
                rowNumberStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
                rowNumberStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                rowNumberStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
                rowNumberStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;

                //====================================
                //將目前欄位的CellStyle設定為自動換行
                HSSFCellStyle newline = (HSSFCellStyle)workbook.CreateCellStyle();
                newline.WrapText = true;
                //====================================
                //預設欄位寬度高度
                sheet.DefaultColumnWidth = 22;

                //====================================
                //標題列
                IRow rowTitle = sheet.CreateRow(0);
                //rowTitle.HeightInPoints = 30;
                rowTitle.CreateCell(0).SetCellValue(String.Format(" 愛金卡股份有限公司"));
                rowTitle.GetCell(0).CellStyle = condStyle;

                DateTime firstDate = DateTime.ParseExact(
                    String.Concat(sYear, sMonth, "01"),
                    "yyyyMMdd", null, System.Globalization.DateTimeStyles.AllowWhiteSpaces);

                IRow rowPrintTime = sheet.CreateRow(1);
                rowPrintTime.CreateCell(0).SetCellValue("儲值金信託餘額檢核表");
                rowPrintTime.GetCell(0).CellStyle = condStyle;
                rowPrintTime.CreateCell(1).SetCellValue(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                rowPrintTime.GetCell(1).CellStyle = condStyle;
                sheet.CreateRow(2).CreateCell(0).CellStyle = speDateStyle;

                sheet.GetRow(2).GetCell(0).SetCellValue(firstDate);



                for (int i = 4; i <= 6; i++)
                {
                    IRow rw = sheet.CreateRow(i);
                    rw.HeightInPoints = 20;
                    for (int j = 0; j < 10; j++)
                    {
                        rw.CreateCell(j);
                        rw.GetCell(j).CellStyle = headStyle;
                    }
                }
                sheet.CreateRow(7).CreateCell(0).SetCellValue("");
                sheet.GetRow(7).HeightInPoints = 10;
                sheet.GetRow(6).HeightInPoints = 15;
                sheet.CreateRow(3).CreateCell(8).SetCellValue("單位：新台幣元");
                sheet.GetRow(3).GetCell(8).SetCellValue("單位：新台幣元");
                sheet.GetRow(4).GetCell(0).SetCellValue("日期");
                sheet.GetRow(4).GetCell(0).CellStyle.DataFormat = (workbook.CreateDataFormat()).GetFormat("yyyy/m/d"); //Stupid fuck's format
                sheet.GetRow(4).GetCell(0).SetCellValue("日期");
                sheet.GetRow(4).GetCell(1).SetCellValue("星期");
                sheet.GetRow(4).GetCell(3).SetCellValue("期末餘額");
                sheet.GetRow(4).GetCell(5).SetCellValue("信託資產");
                sheet.GetRow(5).GetCell(5).SetCellValue("信託帳戶實際餘額");
                sheet.GetRow(5).GetCell(6).SetCellValue("準備金帳戶");
                sheet.GetRow(5).GetCell(7).SetCellValue("小計");
                sheet.GetRow(4).GetCell(9).SetCellValue("期未餘額與信託資產差額");
                sheet.GetRow(6).GetCell(3).SetCellValue("A");
                sheet.GetRow(6).GetCell(5).SetCellValue("B");
                sheet.GetRow(6).GetCell(6).SetCellValue("C");
                sheet.GetRow(6).GetCell(7).SetCellValue("D=(B+C)");
                sheet.GetRow(6).GetCell(9).SetCellValue("E=D-A");

                ////====================================
                ////資料列                   
                int rowIndex = 11;


                //上個月底資料日期

                DateTime lastMonthDate = firstDate.AddDays(-1);
                String strLastMonthDate = firstDate.AddDays(-1).ToString("yyyyMMdd");
                DataTable dt = reportManager.ReportA03(strLastMonthDate);
                DataTable dt2 = reportManager.ReportA03(startYM);
                dt.Merge(dt2);

                sheet.CreateRow(8);
                sheet.CreateRow(9);
                sheet.CreateRow(10);

                for (int i = 0; i < colCnt; i++)
                {
                    if (i == 0)
                    {
                        sheet.GetRow(8).CreateCell(i).CellStyle = rowDateStyle;
                        sheet.GetRow(9).CreateCell(i).CellStyle = rowDateStyle;
                        sheet.GetRow(10).CreateCell(i).CellStyle = rowDateStyle;
                    }
                    else if (i < 3)
                    {
                        sheet.GetRow(8).CreateCell(i).CellStyle = rowTextStyle;
                        sheet.GetRow(9).CreateCell(i).CellStyle = rowTextStyle;
                        sheet.GetRow(10).CreateCell(i).CellStyle = rowTextStyle;
                    }
                    else
                    {
                        sheet.GetRow(8).CreateCell(i).CellStyle = rowNumberStyle;
                        sheet.GetRow(9).CreateCell(i).CellStyle = rowNumberStyle;
                        sheet.GetRow(10).CreateCell(i).CellStyle = rowNumberStyle;
                    }
                }

                //預設值
                //sheet.GetRow(8).GetCell(0).SetCellValue(lastMonthDate.ToString("yyyy/M/d", CultureInfo.InvariantCulture));
                //String tmp = lastMonthDate.ToString("yyyy/M/d", CultureInfo.InvariantCulture);
                sheet.GetRow(8).GetCell(0).SetCellValue(lastMonthDate);
                sheet.GetRow(9).GetCell(0).SetCellValue("次月上傳金額");
                sheet.GetRow(10).GetCell(0).SetCellValue(String.Format("{0}期末帳上金額", lastMonthDate.ToString("yyyy/M/d", CultureInfo.InvariantCulture)));

                sheet.GetRow(8).GetCell(1).SetCellValue(transweek(strLastMonthDate));
                for (int i = 8; i <= 10; i++)
                {
                    sheet.GetRow(i).GetCell(2).SetCellValue("");
                    sheet.GetRow(i).GetCell(3).SetCellValue(0);
                    sheet.GetRow(i).GetCell(4).SetCellValue("");
                    sheet.GetRow(i).GetCell(5).SetCellValue(0);
                    sheet.GetRow(i).GetCell(6).SetCellValue(0);
                    sheet.GetRow(i).GetCell(7).SetCellValue(0);
                    sheet.GetRow(i).GetCell(8).SetCellValue("");
                    sheet.GetRow(i).GetCell(9).SetCellValue(0);
                }


                for (int t = 0; t < dt.Rows.Count; t++)
                {
                    IRow dR = sheet.CreateRow(rowIndex);
                    for (int i = 0; i < colCnt; i++)
                    {
                        if (i == 0)
                        {
                            dR.CreateCell(i).CellStyle = rowDateStyle;
                        }
                        else if (i < 3)
                        {
                            dR.CreateCell(i).CellStyle = rowTextStyle;
                        }
                        else
                        {
                            dR.CreateCell(i).CellStyle = rowNumberStyle;
                        }
                    }

                    //上個月底資料寫入(如果有的話)
                    if (dt.Rows[t]["CAL_DATE"].ToString().Equals(strLastMonthDate))
                    {
                        //daySum(小計) = 信託帳戶實際餘額	+準備金帳戶
                        //double daySum = Convert.ToDouble(dt.Rows[t]["BAL_ACT"].ToString()) + Convert.ToDouble(dt.Rows[t]["BAL_PP"].ToString());
                        //期未餘額與信託資產差額
                        //double diffBal = daySum - Convert.ToDouble(dt.Rows[t]["BAL_LST"].ToString());

                        sheet.GetRow(8).GetCell(1).SetCellValue(transweek(dt.Rows[t]["CAL_DATE"].ToString()));
                        sheet.GetRow(8).GetCell(2).SetCellValue("");
                        sheet.GetRow(8).GetCell(3).SetCellValue(Convert.ToDouble(dt.Rows[t]["BAL_LST"].ToString()));
                        sheet.GetRow(8).GetCell(4).SetCellValue("");
                        sheet.GetRow(8).GetCell(5).SetCellValue(0);
                        sheet.GetRow(8).GetCell(6).SetCellValue(0);
                        sheet.GetRow(8).GetCell(7).SetCellValue(0);
                        sheet.GetRow(8).GetCell(8).SetCellValue("");
                        sheet.GetRow(8).GetCell(9).SetCellValue(0);

                        sheet.GetRow(10).GetCell(1).SetCellValue(transweek(dt.Rows[t]["CAL_DATE"].ToString()));
                        sheet.GetRow(10).GetCell(2).SetCellValue("");
                        sheet.GetRow(10).GetCell(3).SetCellValue(Convert.ToDouble(dt.Rows[t]["BAL_LST"].ToString()));
                        sheet.GetRow(10).GetCell(4).SetCellValue("");
                        sheet.GetRow(10).GetCell(5).SetCellValue(0);
                        sheet.GetRow(10).GetCell(6).SetCellValue(0);
                        sheet.GetRow(10).GetCell(7).SetCellValue(0);
                        sheet.GetRow(10).GetCell(8).SetCellValue("");
                        sheet.GetRow(10).GetCell(9).SetCellValue(0);
                    }
                    else
                    {
                        //daySum(小計) = 信託帳戶實際餘額	+準備金帳戶
                        //double daySum = Convert.ToDouble(dt.Rows[t]["BAL_ACT"].ToString()) + Convert.ToDouble(dt.Rows[t]["BAL_PP"].ToString());
                        //期未餘額與信託資產差額
                        //double diffBal = daySum - Convert.ToDouble(dt.Rows[t]["BAL_LST"].ToString());

                        DateTime dtData = DateTime.ParseExact(dt.Rows[t]["CAL_DATE"].ToString(), "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None);

                        dR.GetCell(0).CellStyle = rowDateStyle;
                        dR.GetCell(0).SetCellValue(dtData);
                        dR.GetCell(1).SetCellValue(transweek(dt.Rows[t]["CAL_DATE"].ToString()));
                        dR.GetCell(2).SetCellValue("");
                        dR.GetCell(3).SetCellValue(Convert.ToDouble(dt.Rows[t]["BAL_LST"].ToString()));
                        dR.GetCell(4).SetCellValue("");
                        dR.GetCell(5).SetCellValue(0);
                        dR.GetCell(6).SetCellValue(0);
                        dR.GetCell(7).SetCellValue(0);
                        dR.GetCell(8).SetCellValue("");
                        dR.GetCell(9).SetCellValue(0);
rowIndex++;

                    }
                    
                }

                //user端下載
                MemoryStream stream = new MemoryStream();
                workbook.Write(stream);
                stream.Dispose();
                return File(stream.ToArray(), "application/vnd.ms-excel", "儲值金信託餘額檢核表_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls");

            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private string transweek(string date)
        {
            string weekday = "";
            int year, month, day;
            year = Convert.ToInt32(date.Substring(0, 4));
            month = Convert.ToInt32(date.Substring(4, 2));
            day = Convert.ToInt32(date.Substring(6, 2));
            DateTime dateValue = new DateTime(year, month, day);

            weekday = dateValue.ToString("dddd",
                  new CultureInfo("zh-cn"));
            weekday = weekday.Replace("星期", "週");
            return weekday;
        }

        /*
        [HttpPost]
        public ActionResult ReportA03Export()
        {
            string startYM = Request.Form["StartDate"];
            ViewBag.StartYM = startYM;
            DateTime firstOfMonth = new DateTime(Int32.Parse(startYM.Substring(0, 4)), Int32.Parse(startYM.Substring(4, 2)), 1);
            //last month last day
            DateTime lastOfLastMonth = firstOfMonth.AddDays(-1);

            DataTable dt = reportManager.ReportA03(lastOfLastMonth.ToString("yyyyMMdd"));
            DataTable dt2 = reportManager.ReportA03(startYM);

            dt.Merge(dt2, true);

            IWorkbook wb = new HSSFWorkbook();
            ISheet ws;
            ws = wb.CreateSheet("信託餘額表");
            //ws.CreateRow(4);//第一行為欄位名稱
            var headerRow = ws.CreateRow(4);
            var headerRow2 = ws.CreateRow(5);
            var headerRow3 = ws.CreateRow(6);

            HSSFCellStyle cs = (HSSFCellStyle)wb.CreateCellStyle();
            //啟動多行文字
            //cs.WrapText = true;
            //文字置中
            cs.VerticalAlignment = VerticalAlignment.Center;
            cs.Alignment = HorizontalAlignment.Center;
            //框線樣式及顏色
            cs.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
            cs.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
            cs.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
            cs.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
            cs.BottomBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
            cs.LeftBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
            cs.RightBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
            cs.TopBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
            //背景顏色
            cs.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.Grey25Percent.Index;
            cs.FillPattern = NPOI.SS.UserModel.FillPattern.SolidForeground;
            HSSFFont font1 = (HSSFFont)wb.CreateFont();
            //字體顏色
            font1.Color = NPOI.HSSF.Util.HSSFColor.Black.Index;
            //字體粗體
            //font1.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.Bold;
            //字體尺寸
            font1.FontHeightInPoints = 12;
            cs.SetFont(font1);
            cs.WrapText = true; //自動換列

            headerRow.HeightInPoints = 24;

            var columns = new[] { "日期", "星期","", "期末餘額","", "信託資產", "", "","", "期未餘額與信託資產差額" };
            var columns2 = new[] { "", "", "", "", "", "信託帳戶實際餘額", "準備金帳戶", "小計", "", "" };
            var columns3 = new[] { "", "", "", "A", "", "B", "C", "D=B+C", "", "E=D-A" };
            //create header
            for (int i = 0; i < columns.Length; i++)
            {
                var cell = headerRow.CreateCell(i);
                cell.SetCellValue(columns[i]);
                cell.CellStyle = cs;

                if (i == 0)
                    ws.SetColumnWidth(0, 18 * 256);
                else if (i == 1)
                    ws.SetColumnWidth(1, 10 * 256);
                else if (i == 2)
                    ws.SetColumnWidth(2, 1 * 256);
                else if (i == 4)
                    ws.SetColumnWidth(4, 1 * 256);
                else if (i == 8)
                    ws.SetColumnWidth(8, 1 * 256);
                else
                    ws.SetColumnWidth(i, 26 * 256);

                //if (i == 6) cell.SetCellValue(string.Format("期未餘額與信託資產差額{0}(E=D-A)", System.Environment.NewLine));
            }
            for (int i = 0; i < columns2.Length; i++)
            {
                var cell = headerRow2.CreateCell(i);
                cell.SetCellValue(columns2[i]);
                cell.CellStyle = cs;
            }
            for (int i = 0; i < columns3.Length; i++)
            {
                var cell = headerRow3.CreateCell(i);
                cell.SetCellValue(columns3[i]);
                cell.CellStyle = cs;
            }
            ws.AddMergedRegion(new CellRangeAddress(4, 5, 0, 0));
            ws.AddMergedRegion(new CellRangeAddress(4, 5, 1, 1));
            ws.AddMergedRegion(new CellRangeAddress(4, 5, 3, 3));
            ws.AddMergedRegion(new CellRangeAddress(4, 4, 5, 7));

            ws.AddMergedRegion(new CellRangeAddress(4, 5, 9, 9));
            //fill content 
            ICellStyle style2 = wb.CreateCellStyle();
            //框線樣式及顏色
            style2.Alignment = HorizontalAlignment.Center;
            style2.VerticalAlignment = VerticalAlignment.Center;
            //字體尺寸
            style2.SetFont(font1);
           
            ICellStyle style3 = wb.CreateCellStyle();
            //框線樣式及顏色
            style3.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
            style3.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
            style3.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
            style3.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
            style3.BottomBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
            style3.LeftBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
            style3.RightBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
            style3.TopBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
            style3.Alignment = HorizontalAlignment.Center;
            style3.VerticalAlignment = VerticalAlignment.Center;
            //字體尺寸
            style3.SetFont(font1);
            //style3.FillForegroundColor = IndexedColors.LemonChiffon.Index;
            //style3.FillPattern = FillPattern.SolidForeground;
            //style3.FillBackgroundColor = IndexedColors.LightGreen.Index;

            // Create the style object
            var currencyCellStyle = wb.CreateCellStyle();
            // Right-align currency values
            currencyCellStyle.Alignment = HorizontalAlignment.Right;
            currencyCellStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
            currencyCellStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
            currencyCellStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
            currencyCellStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
            currencyCellStyle.BottomBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
            currencyCellStyle.LeftBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
            currencyCellStyle.RightBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
            currencyCellStyle.TopBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
            currencyCellStyle.SetFont(font1);
            // Get / create the data format string
            var formatId = HSSFDataFormat.GetBuiltinFormat("#,##0");
            if (formatId == -1)
            {
                var newDataFormat = wb.CreateDataFormat();
                currencyCellStyle.DataFormat = newDataFormat.GetFormat("#,##0");
            }
            else
                currencyCellStyle.DataFormat = formatId;


            ws.CreateRow(0);
            var cell3 = ws.GetRow(0).CreateCell(0);
            cell3.SetCellValue("愛金卡股份有限公司");
            cell3.CellStyle = style2;
            ws.AddMergedRegion(new CellRangeAddress(0, 0, 0, 9));

            ws.CreateRow(1);
            cell3 = ws.GetRow(1).CreateCell(0);
            cell3.SetCellValue("儲值金信託餘額檢核表");
            cell3.CellStyle = style2;
            ws.AddMergedRegion(new CellRangeAddress(1, 1, 0, 9));

            ICellStyle styleMingDate = wb.CreateCellStyle();
            //框線樣式及顏色
            styleMingDate.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
            styleMingDate.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
            styleMingDate.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
            styleMingDate.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
            styleMingDate.BottomBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
            styleMingDate.LeftBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
            styleMingDate.RightBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
            styleMingDate.TopBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
            styleMingDate.Alignment = HorizontalAlignment.Center;
            styleMingDate.VerticalAlignment = VerticalAlignment.Center;
            //字體尺寸
            styleMingDate.SetFont(font1);
            var mingDataFormat = wb.CreateDataFormat();
            styleMingDate.DataFormat = mingDataFormat.GetFormat("[$-404]e\"年\"m\"月\";@");
            DateTime firstDate = DateTime.ParseExact(
                    String.Concat(startYM.Substring(0, 4), startYM.Substring(4, 2), "01"),
                    "yyyyMMdd", null, System.Globalization.DateTimeStyles.AllowWhiteSpaces);
            ws.CreateRow(2);
            cell3 = ws.GetRow(2).CreateCell(0);
            cell3.SetCellValue(firstDate);
            //cell3.SetCellValue(firstOfMonth.ToString("yyy") + "年" + firstOfMonth.ToString("MM") + "月");
            cell3.CellStyle = styleMingDate;
            ws.AddMergedRegion(new CellRangeAddress(2, 2, 0, 9));


            ws.CreateRow(3);
            cell3 = ws.GetRow(3).CreateCell(9);
            cell3.SetCellValue("單位：新台幣元");
            //cell3.CellStyle = style3;
            int ii = 2;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ws.CreateRow(ii + 1+5);
                string dd = dt.Rows[i][0].ToString();
                DateTime dateValue = new DateTime(Int32.Parse(dd.Substring(0, 4)), Int32.Parse(dd.Substring(4, 2)), Int32.Parse(dd.Substring(6, 2)));

                for (int j = 0; j < columns.Length; j++)
                {
                    var cell2 = ws.GetRow(ii + 1+5).CreateCell(j);
                    if(j==0){
                        cell2.SetCellValue(dd);
                        cell2.CellStyle = style3;
                    }else if(j==1){
                        cell2.SetCellValue(dateValue.ToString("ddd", new CultureInfo("zh-TW")));
                        cell2.CellStyle = style3;
                    }
                    else if (j == 3)
                    {
                        cell2.SetCellValue(Convert.ToDouble(dt.Rows[i][1].ToString()));
                        cell2.CellStyle = currencyCellStyle;
                    }
                    else if (j == 2 || j==4 || j==8)
                    {
                        cell2.SetCellValue("");
                        cell2.CellStyle = style3;
                    }
                    else 
                    {
                        cell2.SetCellValue(0);
                        cell2.CellStyle = currencyCellStyle;
                    }
                      
                }
                ii = ii + 1;
                if (i == 0)
                {
                    ws.CreateRow(ii + 1 + 5);
                    for (int j = 0; j < columns.Length; j++)
                    {
                        //次月上傳金額
                        var cell2 = ws.GetRow(ii + 1 + 5).CreateCell(j);
                        if (j == 0)
                        {
                            cell2.SetCellValue("次月上傳金額");
                            cell2.CellStyle = style3;
                        }
                        else if (j == 1)
                        {
                            cell2.SetCellValue("");
                            cell2.CellStyle = style3;
                        }
                        else if (j == 2 || j == 4 || j == 8)
                        {
                            cell2.SetCellValue("");
                            cell2.CellStyle = style3;
                        }
                        else
                        {
                            cell2.SetCellValue(0);
                            cell2.CellStyle = currencyCellStyle;
                        }
                    }
                    ii = ii + 1;
                    //期末帳上金額
                    ws.CreateRow(ii + 1 + 5);
                    for (int j = 0; j < columns.Length; j++)
                    {
                        var cell2 = ws.GetRow(ii + 1 + 5).CreateCell(j);
                        if (j == 0)
                        {
                            cell2.SetCellValue(dt.Rows[i][0].ToString()+"期末帳上金額");
                            cell2.CellStyle = style3;
                        }
                        else if (j == 1)
                        {
                            cell2.SetCellValue(dateValue.ToString("ddd", new CultureInfo("zh-TW")));
                            cell2.CellStyle = style3;
                        }
                        else if (j == 3)
                        {
                            cell2.SetCellValue(Convert.ToDouble(dt.Rows[i][1].ToString()));
                            cell2.CellStyle = currencyCellStyle;
                        }
                        else if (j == 2 || j == 4 || j == 8)
                        {
                            cell2.SetCellValue("");
                            cell2.CellStyle = style3;
                        }
                        else
                        {
                            cell2.SetCellValue(0);
                            cell2.CellStyle = currencyCellStyle;
                        }
                    }
                    ii = ii + 1;

                }

            }
            var stream = new MemoryStream();
            wb.Write(stream);
            stream.Close();

            return File(stream.ToArray(), "application/vnd.ms-excel", "儲值金信託餘額檢核表_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls");
        }
        */

        public ActionResult Report151201Result()
        {
            string start = DateTime.Now.AddMonths(-12).ToString("yyyyMMdd");
            string end = DateTime.Now.ToString("yyyyMMdd");
            string src = "AMUSEMENT_PARK";
            
            if (Request.Form["searchConfirm"] != null || Request.Form["ExportExcel"] != null )
            {
                start = Request.Form["startDate"];
                end = Request.Form["endDate"];
                src = Request.Form["src"];
            }
            if (src == "AMUSEMENT_PARK")
            {
                ViewBag.ch1 = "checked"; ViewBag.ch2 = "";
            }
            else
            {
                ViewBag.ch1 = ""; ViewBag.ch2 = "checked";
            }
            ViewBag.Start = start;
            ViewBag.End = end;
            ViewBag.RepName = "特約機構營收資料(微程式)";
            DataTable dt = reportManager.Report151201T(start,end,src);
            if (Request.Form["searchConfirm"] != null)
            { return View(dt); }
            else if (Request.Form["ExportExcel"] != null)
            {
                using (ExcelPackage p = new ExcelPackage())
                {
                    ExcelWorksheet ws = p.Workbook.Worksheets.Add(ViewBag.RepName.ToString());


                    ExcelRange r = ws.Cells["A1:G1"];
                    r.Merge = true;
                    r.Value = ViewBag.RepName.ToString();


                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    r.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                    //r.Style.Font.Bold = true;
                    r.Style.Font.Size = 14;

                    int startRowNumber = 2;
                    var columns = new[] { "門市名稱", "交易日", "購貨總金額", "購貨取消總金額", "購貨淨額", "自動加值總額", "清分日" };

                    for (int j = 1; j <= columns.Length; j++)
                    {
                        //設值為欄位名稱
                        ws.Cells[startRowNumber, j].Value = columns[j-1];
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
                                case 3:
                                case 4:
                                case 5:
                                case 6:
                                    ws.Cells[i + 2, j].Value = Convert.ToDouble(dt.Rows[i-1][j-1]);


                                    ws.Cells[i + 2, j].Style.Numberformat.Format = "#,##0";
                                    ws.Cells[i + 2, j].Style.Font.Size = 12;
                                    ws.Cells[i + 2, j].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;//欄位置右
                                    ws.Cells[i + 2, j].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                                    break;
                                default:
                                    ws.Cells[i + 2, j].Value = dt.Rows[i-1][j-1].ToString();

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

        public ActionResult Report151202Result()
        {
            string start = DateTime.Now.AddMonths(-12).ToString("yyyyMMdd");
            string end = DateTime.Now.ToString("yyyyMMdd");
            string src = "STATION";

            if (Request.Form["searchConfirm"] != null || Request.Form["ExportExcel"] != null)
            {
                start = Request.Form["startDate"];
                end = Request.Form["endDate"];
                src = Request.Form["src"];
            }

            if (src == "STATION")
            {
                ViewBag.ch1 = "checked"; ViewBag.ch2 = "";
            }
            else
            {
                ViewBag.ch1 = ""; ViewBag.ch2 = "checked";
            }
            ViewBag.Start = start;
            ViewBag.End = end;
            ViewBag.RepName = "特約機構營收資料(葛瑪蘭)";
            DataTable dt = reportManager.Report151202T(start, end, src);
            if (Request.Form["searchConfirm"] != null)
            { return View(dt); }
            else if (Request.Form["ExportExcel"] != null)
            {
                IWorkbook wb = new HSSFWorkbook();
                ISheet ws = wb.CreateSheet(ViewBag.Repname.ToString());

                HSSFCellStyle cs = (HSSFCellStyle)wb.CreateCellStyle();
                //啟動多行文字
                //cs.WrapText = true;
                //文字置中
                cs.VerticalAlignment = VerticalAlignment.Center;
                cs.Alignment = HorizontalAlignment.Center;
                //框線樣式及顏色
                cs.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
                cs.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                cs.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
                cs.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
                cs.BottomBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                cs.LeftBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                cs.RightBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                cs.TopBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                //背景顏色
                cs.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.Grey25Percent.Index;
                cs.FillPattern = NPOI.SS.UserModel.FillPattern.SolidForeground;
                HSSFFont font1 = (HSSFFont)wb.CreateFont();
                //字體顏色
                font1.Color = NPOI.HSSF.Util.HSSFColor.Black.Index;
                //字體粗體
                font1.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.Bold;
                //字體尺寸
                font1.FontHeightInPoints = 14;
                cs.SetFont(font1);
                cs.WrapText = true; //自動換列
                cs.ShrinkToFit = true;


                HSSFCellStyle cs2 = (HSSFCellStyle)wb.CreateCellStyle();
                //啟動多行文字
                //cs.WrapText = true;
                //文字置中
                cs2.VerticalAlignment = VerticalAlignment.Center;
                cs2.Alignment = HorizontalAlignment.Center;
                //框線樣式及顏色
                cs2.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
                cs2.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                cs2.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
                cs2.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
                cs2.BottomBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                cs2.LeftBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                cs2.RightBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                cs2.TopBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                //背景顏色
                //cs2.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.Grey25Percent.Index;
                //cs2.FillPattern = NPOI.SS.UserModel.FillPattern.SolidForeground;

                HSSFFont font2 = (HSSFFont)wb.CreateFont();
                //字體顏色
                font2.Color = NPOI.HSSF.Util.HSSFColor.Black.Index;
                //字體粗體
                //font1.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.Bold;
                //字體尺寸
                font2.FontHeightInPoints = 12;
                cs2.SetFont(font2);
                //cs2.WrapText = true; //自動換列



                var headerRow = ws.CreateRow(0);
                headerRow.HeightInPoints = 18;

                var columns = new[] { "線路", "交易日期", "交易總金額", "清分日期" };

                for (int j = 0; j < columns.Length; j++)
                {
                    ICell cell = headerRow.CreateCell(j);
                    cell.CellStyle = cs;
                    cell.SetCellValue(columns[j]);
                    //ws.AutoSizeColumn(j);
                }

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ws.CreateRow(i + 1).HeightInPoints = 14;
                    for (int j = 0; j < columns.Length; j++)
                    {
                        ICell newCell = ws.GetRow(i + 1).CreateCell(j);
                        newCell.CellStyle = cs2;
                        switch (j)
                        {
                            case 2:
                                newCell.SetCellType(CellType.Numeric);
                                newCell.SetCellValue(Convert.ToDouble(dt.Rows[i][j]));
                                break;
                            default:
                                newCell.SetCellValue(dt.Rows[i][j].ToString());
                                break;
                        }
                        ws.AutoSizeColumn(j);

                       
                    }
                }


                var stream = new MemoryStream();
                wb.Write(stream);
                stream.Close();

                return File(stream.ToArray(), "application/vnd.ms-excel", ViewBag.RepName + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls");
            }
            else return View(new DataTable());
        }

        public ActionResult Report151203Result()
        { 
            string start = DateTime.Now.ToString("yyyyMMdd");
            string end = DateTime.Now.ToString("yyyyMMdd");
            string type = "";
            string code = "";
            string merchantNoCom="";
            

            if (Request.Form["searchConfirm"] != null || Request.Form["ExportExcel"] != null)
            {
                start = Request.Form["startDate"];
                end = Request.Form["endDate"];
                type = Request.Form["type"];
                code = Request.Form["ACTIVITY_CODE"];
                ViewData["ACTIVITY_CODE"] = code;
                merchantNoCom = Request.Form["COMPANY"];
                ViewData["COMPANY"] = merchantNoCom;
            }

            ViewBag.RepName = "企業加值明細";
            ViewBag.Start = start;
            ViewBag.End = end;

            if(type!="BY_ACTIVITY")
            {
                ViewBag.ch1 = "checked"; ViewBag.ch2 = "";
            }
            else
            {
                ViewBag.ch1 = ""; ViewBag.ch2 = "checked";

            }

            //Activity Code
            List<SelectListItem> codesList = new List<SelectListItem>();
            IEnumerable<IbonActivity> allActivities = new IbonActivityManager().FindAll();
            IEnumerable<IbonActivity> distinctActivities = from k in allActivities
                    group k by new { k.Code, k.Name } into g
                    select g.FirstOrDefault();
            codesList.AddRange(new SelectList(distinctActivities, "Code", "Name"));
            codesList.Add(new SelectListItem() { Text = "ALL", Value = "ALL", Selected = false });
            ViewBag.CodesList = codesList;

            //Merchant Number and Names
            List<SelectListItem> companiesList = new List<SelectListItem>(); 
            IEnumerable<GmMerchant> allMerchants = new GmMerchantManager().FindAllMerchantNoCom();
            IEnumerable<GmMerchant> distinctMerchants = from k in allMerchants
                    group k by new { k.MerchantNo, k.MerchantName } into g
                    select g.FirstOrDefault();
            companiesList.AddRange(new SelectList(distinctMerchants, "MerchantNo", "MerchantName"));
            companiesList.Add(new SelectListItem() { Text = "ALL", Value = "ALL", Selected = false });
            ViewBag.CompaniesList = companiesList;

            DataTable dt = reportManager.Report151203T(start, end, type, code, merchantNoCom);
            if (Request.Form["searchConfirm"] != null)
            { return View(dt); }
            else if (Request.Form["ExportExcel"] != null)
            {
                using (ExcelPackage p = new ExcelPackage())
                {
                    ExcelWorksheet ws = p.Workbook.Worksheets.Add(ViewBag.RepName.ToString());


                    ExcelRange r = ws.Cells["A1:G1"];
                    r.Merge = true;
                    r.Value = ViewBag.RepName.ToString();


                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    r.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                    //r.Style.Font.Bold = true;
                    r.Style.Font.Size = 14;

                    int startRowNumber = 2;
                    var columns = new[] { "公司統一編號", "公司名稱", "活動代號", "活動名稱", "清分日期", "加值金額", "加值狀態" };

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
                                case 6:
                                    if (dt.Rows[i - 1][1].Equals("總計") && DBNull.Value.Equals(dt.Rows[i - 1][j]))
                                    {
                                        ws.Cells[i + 2, j].Value = 0;
                                    }
                                    else
                                    {
                                        ws.Cells[i + 2, j].Value = Convert.ToDouble(dt.Rows[i - 1][j]);
                                    }

                                    ws.Cells[i + 2, j].Style.Numberformat.Format = "#,##0";
                                    ws.Cells[i + 2, j].Style.Font.Size = 12;
                                    ws.Cells[i + 2, j].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;//欄位置右
                                    ws.Cells[i + 2, j].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                                    break;
                                default:
                                    ws.Cells[i + 2, j].Value = dt.Rows[i - 1][j].ToString();

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

        public ActionResult Report151204Result()
        {
            string start = DateTime.Now.AddDays(-1).ToString("yyyyMMdd");
            string end = DateTime.Now.ToString("yyyyMMdd");
           
            string code = "";

            if (Request.Form["searchConfirm"] != null || Request.Form["ExportExcel"] != null)
            {
                start = Request.Form["startDate"];
                end = Request.Form["endDate"];
                
                code = Request.Form["ACTIVITY_CODE"];
                ViewData["ACTIVITY_CODE"] = code;
              
            }

            ViewBag.RepName = "企業加值活動表";
            ViewBag.Start = start;
            ViewBag.End = end;

            //Distict DropDownList members for IbonActivities
            List<SelectListItem> codesList = new List<SelectListItem>();
            IEnumerable<IbonActivity> allActivities = new IbonActivityManager().FindAll();
            IEnumerable<IbonActivity> distinctActivities = from k in allActivities
                    group k by new { k.Code, k.Name } into g
                    select g.FirstOrDefault();
            codesList.AddRange(new SelectList(distinctActivities, "Code", "Name"));
            codesList.Add(new SelectListItem() { Text = "ALL", Value = "ALL", Selected = false });
            ViewBag.CodesList = codesList;


            DataTable dt = reportManager.Report151204T(start, end, code);
            if (Request.Form["searchConfirm"] != null)
            { return View(dt); }
            else if (Request.Form["ExportExcel"] != null)
            {
                IWorkbook wb = new HSSFWorkbook();
                ISheet ws = wb.CreateSheet(ViewBag.Repname.ToString());

                HSSFCellStyle cs = (HSSFCellStyle)wb.CreateCellStyle();
                //啟動多行文字
                //cs.WrapText = true;
                //文字置中
                cs.VerticalAlignment = VerticalAlignment.Center;
                cs.Alignment = HorizontalAlignment.Center;
                //框線樣式及顏色
                cs.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
                cs.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                cs.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
                cs.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
                cs.BottomBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                cs.LeftBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                cs.RightBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                cs.TopBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                //背景顏色
                cs.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.Grey25Percent.Index;
                cs.FillPattern = NPOI.SS.UserModel.FillPattern.SolidForeground;
                HSSFFont font1 = (HSSFFont)wb.CreateFont();
                //字體顏色
                font1.Color = NPOI.HSSF.Util.HSSFColor.Black.Index;
                //字體粗體
                font1.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.Bold;
                //字體尺寸
                font1.FontHeightInPoints = 14;
                cs.SetFont(font1);
                cs.WrapText = true; //自動換列
                cs.ShrinkToFit = true;


                HSSFCellStyle cs2 = (HSSFCellStyle)wb.CreateCellStyle();
                //啟動多行文字
                //cs.WrapText = true;
                //文字置中
                cs2.VerticalAlignment = VerticalAlignment.Center;
                cs2.Alignment = HorizontalAlignment.Center;
                //框線樣式及顏色
                cs2.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
                cs2.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                cs2.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
                cs2.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
                cs2.BottomBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                cs2.LeftBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                cs2.RightBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                cs2.TopBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                //背景顏色
                //cs2.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.Grey25Percent.Index;
                //cs2.FillPattern = NPOI.SS.UserModel.FillPattern.SolidForeground;

                HSSFFont font2 = (HSSFFont)wb.CreateFont();
                //字體顏色
                font2.Color = NPOI.HSSF.Util.HSSFColor.Black.Index;
                //字體粗體
                //font1.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.Bold;
                //字體尺寸
                font2.FontHeightInPoints = 12;
                cs2.SetFont(font2);
                //cs2.WrapText = true; //自動換列



                var headerRow = ws.CreateRow(0);
                headerRow.HeightInPoints = 18;

                var columns = new[] { "公司統一編號", "公司名稱", "活動代號", "活動名稱", "加值有效起始日", "加值有效終止日", "企業加值活動總金額", "已清分總額", "未使用餘額" };

                for (int j = 0; j < columns.Length; j++)
                {
                    ICell cell = headerRow.CreateCell(j);
                    cell.CellStyle = cs;
                    cell.SetCellValue(columns[j]);
                    //ws.AutoSizeColumn(j);
                }

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ws.CreateRow(i + 1).HeightInPoints = 14;
                    for (int j = 0; j < columns.Length; j++)
                    {
                        ICell newCell = ws.GetRow(i + 1).CreateCell(j);
                        newCell.CellStyle = cs2;
                        switch (j)
                        {
                            case 6:
                            case 7:
                            case 8:
                                newCell.SetCellType(CellType.Numeric);
                                newCell.SetCellValue(Convert.ToDouble(dt.Rows[i][j]));
                                newCell.CellStyle.DataFormat = wb.CreateDataFormat().GetFormat("#,##0");
                                break;
                            default:
                                newCell.SetCellValue(dt.Rows[i][j].ToString());
                                break;
                        }
                        ws.AutoSizeColumn(j);


                    }
                }

                var stream = new MemoryStream();
                wb.Write(stream);
                stream.Close();
                return File(stream.ToArray(), "application/vnd.ms-excel", ViewBag.RepName + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls");
            }
            else return View(new DataTable());
        }

        public ActionResult Report151205Result()
        {
            string start = DateTime.Now.ToString("yyyyMMdd");
            string end = DateTime.Now.ToString("yyyyMMdd");
            string type = "";
            string code = "";
            string merchantNoCom = "";


            if (Request.Form["searchConfirm"] != null || Request.Form["ExportExcel"] != null)
            {
                start = Request.Form["startDate"];
                end = Request.Form["endDate"];
                type = Request.Form["type"];
                code = Request.Form["ACTIVITY_CODE"];
                ViewData["ACTIVITY_CODE"] = code;
                merchantNoCom = Request.Form["COMPANY"];
                ViewData["COMPANY"] = merchantNoCom;
            }

            ViewBag.RepName = "企業加值明細(已加值)";
            ViewBag.Start = start;
            ViewBag.End = end;

            if (type != "BY_ACTIVITY")
            {
                ViewBag.ch1 = "checked"; ViewBag.ch2 = "";
            }
            else
            {
                ViewBag.ch1 = ""; ViewBag.ch2 = "checked";

            }

            //Activity Code
            List<SelectListItem> codesList = new List<SelectListItem>();
            IEnumerable<IbonActivity> allActivities = new IbonActivityManager().FindAll();
            IEnumerable<IbonActivity> distinctActivities = from k in allActivities
                                                           group k by new { k.Code, k.Name } into g
                                                           select g.FirstOrDefault();
            codesList.AddRange(new SelectList(distinctActivities, "Code", "Name"));
            codesList.Add(new SelectListItem() { Text = "ALL", Value = "ALL", Selected = false });
            ViewBag.CodesList = codesList;

            //Merchant Number and Names
            List<SelectListItem> companiesList = new List<SelectListItem>();
            IEnumerable<GmMerchant> allMerchants = new GmMerchantManager().FindAllMerchantNoCom();
            IEnumerable<GmMerchant> distinctMerchants = from k in allMerchants
                                                        group k by new { k.MerchantNo, k.MerchantName } into g
                                                        select g.FirstOrDefault();
            companiesList.AddRange(new SelectList(distinctMerchants, "MerchantNo", "MerchantName"));
            companiesList.Add(new SelectListItem() { Text = "ALL", Value = "ALL", Selected = false });
            ViewBag.CompaniesList = companiesList;

            DataTable dt = reportManager.Report151205T(start, end, type, code, merchantNoCom);
            if (Request.Form["searchConfirm"] != null)
            { return View(dt); }
            else if (Request.Form["ExportExcel"] != null)
            {
                using (ExcelPackage p = new ExcelPackage())
                {
                    ExcelWorksheet ws = p.Workbook.Worksheets.Add(ViewBag.RepName.ToString());


                    ExcelRange r = ws.Cells["A1:F1"];
                    r.Merge = true;
                    r.Value = ViewBag.RepName.ToString();


                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    r.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                    //r.Style.Font.Bold = true;
                    r.Style.Font.Size = 14;

                    int startRowNumber = 2;
                    var columns = new[] { "公司統一編號", "公司名稱", "活動代號", "活動名稱", "清分日期", "加值金額" };

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
                                case 6:
                                    if (dt.Rows[i - 1][1].Equals("總計") && DBNull.Value.Equals(dt.Rows[i - 1][j]))
                                    {
                                        ws.Cells[i + 2, j].Value = "";
                                    }
                                    else
                                    {
                                        ws.Cells[i + 2, j].Value = Convert.ToDouble(dt.Rows[i - 1][j]);
                                    }


                                    ws.Cells[i + 2, j].Style.Numberformat.Format = "#,##0";
                                    ws.Cells[i + 2, j].Style.Font.Size = 12;
                                    ws.Cells[i + 2, j].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;//欄位置右
                                    ws.Cells[i + 2, j].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                                    break;
                                default:
                                    ws.Cells[i + 2, j].Value = dt.Rows[i - 1][j].ToString();

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

        public ActionResult Report151206Result()
        {
            string start = DateTime.Now.AddDays(-1).ToString("yyyyMMdd");
            string end = DateTime.Now.ToString("yyyyMMdd");

            string code = "";

            if (Request.Form["searchConfirm"] != null || Request.Form["ExportExcel"] != null)
            {
                start = Request.Form["startDate"];
                end = Request.Form["endDate"];

                code = Request.Form["ACTIVITY_CODE"];
                ViewData["ACTIVITY_CODE"] = code;

            }

            ViewBag.RepName = "企業加值活動表(已加值)";
            ViewBag.Start = start;
            ViewBag.End = end;

            //Distict DropDownList members for IbonActivities
            List<SelectListItem> codesList = new List<SelectListItem>();
            IEnumerable<IbonActivity> allActivities = new IbonActivityManager().FindAll();
            IEnumerable<IbonActivity> distinctActivities = from k in allActivities
                                                           group k by new { k.Code, k.Name } into g
                                                           select g.FirstOrDefault();
            codesList.AddRange(new SelectList(distinctActivities, "Code", "Name"));
            codesList.Add(new SelectListItem() { Text = "ALL", Value = "ALL", Selected = false });
            ViewBag.CodesList = codesList;


            DataTable dt = reportManager.Report151206T(start, end, code);
            if (Request.Form["searchConfirm"] != null)
            { return View(dt); }
            else if (Request.Form["ExportExcel"] != null)
            {
                IWorkbook wb = new HSSFWorkbook();
                ISheet ws = wb.CreateSheet(ViewBag.Repname.ToString());

                HSSFCellStyle cs = (HSSFCellStyle)wb.CreateCellStyle();
                //啟動多行文字
                //cs.WrapText = true;
                //文字置中
                cs.VerticalAlignment = VerticalAlignment.Center;
                cs.Alignment = HorizontalAlignment.Center;
                //框線樣式及顏色
                cs.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
                cs.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                cs.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
                cs.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
                cs.BottomBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                cs.LeftBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                cs.RightBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                cs.TopBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                //背景顏色
                cs.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.Grey25Percent.Index;
                cs.FillPattern = NPOI.SS.UserModel.FillPattern.SolidForeground;
                HSSFFont font1 = (HSSFFont)wb.CreateFont();
                //字體顏色
                font1.Color = NPOI.HSSF.Util.HSSFColor.Black.Index;
                //字體粗體
                font1.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.Bold;
                //字體尺寸
                font1.FontHeightInPoints = 14;
                cs.SetFont(font1);
                cs.WrapText = true; //自動換列
                cs.ShrinkToFit = true;


                HSSFCellStyle cs2 = (HSSFCellStyle)wb.CreateCellStyle();
                //啟動多行文字
                //cs.WrapText = true;
                //文字置中
                cs2.VerticalAlignment = VerticalAlignment.Center;
                cs2.Alignment = HorizontalAlignment.Center;
                //框線樣式及顏色
                cs2.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
                cs2.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                cs2.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
                cs2.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
                cs2.BottomBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                cs2.LeftBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                cs2.RightBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                cs2.TopBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                //背景顏色
                //cs2.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.Grey25Percent.Index;
                //cs2.FillPattern = NPOI.SS.UserModel.FillPattern.SolidForeground;

                HSSFFont font2 = (HSSFFont)wb.CreateFont();
                //字體顏色
                font2.Color = NPOI.HSSF.Util.HSSFColor.Black.Index;
                //字體粗體
                //font1.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.Bold;
                //字體尺寸
                font2.FontHeightInPoints = 12;
                cs2.SetFont(font2);
                //cs2.WrapText = true; //自動換列



                var headerRow = ws.CreateRow(0);
                headerRow.HeightInPoints = 18;

                var columns = new[] { "公司統一編號", "公司名稱", "活動代號", "活動名稱", "加值有效起始日", "加值有效終止日", "企業加值活動總金額", "已清分總額", "未使用餘額" };

                for (int j = 0; j < columns.Length; j++)
                {
                    ICell cell = headerRow.CreateCell(j);
                    cell.CellStyle = cs;
                    cell.SetCellValue(columns[j]);
                    //ws.AutoSizeColumn(j);
                }

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ws.CreateRow(i + 1).HeightInPoints = 14;
                    for (int j = 0; j < columns.Length; j++)
                    {
                        ICell newCell = ws.GetRow(i + 1).CreateCell(j);
                        newCell.CellStyle = cs2;
                        switch (j)
                        {
                            case 6:
                            case 7:
                            case 8:
                                newCell.SetCellType(CellType.Numeric);
                                newCell.SetCellValue(Convert.ToDouble(dt.Rows[i][j]));
                                newCell.CellStyle.DataFormat = wb.CreateDataFormat().GetFormat("#,##0");
                                break;
                            default:
                                newCell.SetCellValue(dt.Rows[i][j].ToString());
                                break;
                        }
                        ws.AutoSizeColumn(j);


                    }
                }

                var stream = new MemoryStream();
                wb.Write(stream);
                stream.Close();
                return File(stream.ToArray(), "application/vnd.ms-excel", ViewBag.RepName + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls");
            }
            else return View(new DataTable());
        }
        public ActionResult Report170901Result()
        {
            //DateTime tempDate;

            int year = DateTime.Today.Year;
            int month = DateTime.Today.Month;
            DateTime startDate = new DateTime(year, month, 1);
            DateTime endDate = new DateTime(year, month,
                                    DateTime.DaysInMonth(year, month));
            string start = startDate.ToString("yyyyMMdd");
            string end = endDate.ToString("yyyyMMdd");
            start = "";
            end = "";
            //string merchantNo = "";
            string merchant = "";
            string item = "";

            string group = "";
            string retail = "";
            string bus = "";
            string bike = "";
            string track = "";
            string parking = "";
            string outsourcing = "";

            string SRC_FLG = "TXLOG";

            ViewBag.Start = startDate.ToString("yyyyMMdd");
            ViewBag.End = endDate.ToString("yyyyMMdd");

            if (Request.Form["searchConfirm"] != null || Request.Form["ExportExcel"] != null)
            {
                //start = Request.Form["startDate"] + "01";
                //tempDate = DateTime.Parse(start.Substring(0, 4) + "-" + start.Substring(4, 2) + "-" + start.Substring(6, 2)).AddDays(-1);
                //start = tempDate.ToString("yyyyMMdd");
                start = Request.Form["startDate"];
                //int days = DateTime.DaysInMonth(int.Parse(Request.Form["startDate"].Substring(0, 4)), int.Parse(Request.Form["startDate"].Substring(4, 2))) - 1;
                //end = Request.Form["startDate"] + days.ToString();
                end = Request.Form["endDate"];
                //merchantNo = Request.Form["Merchant"];
                SRC_FLG = Request.Form["SRC_FLG"];
                ViewBag.Start = Request.Form["startDate"];
                ViewBag.End = Request.Form["endDate"];
                /*
                if (merchantNo == "")
                {
                    DisplayMessage("請選擇特約機構");
                }
                */
                group = Request.Form["group"];
                item = Request.Form["items"];
                //retail = Request.Form["retail"];
                //bus = Request.Form["bus"];
                //bike = Request.Form["bike"];
                //track = Request.Form["track"];
                //parking = Request.Form["parking"];
                //outsourcing = Request.Form["outsourcing"];

                //if (group == "PARKING_LOT")
                //{ merchant = Request.Form["parking"]; }
                //else if (group == "BANK_OUTSOURCING")
                //{ merchant = Request.Form["outsourcing"]; }
                //else
                //{ merchant = Request.Form[group.ToLower()]; }
                merchant = Request.Form["items"];
                if (start == "" || end == "")
                {
                    DisplayMessage("請選擇日期");
                }
            }
            if (SRC_FLG == "TXLOG")
            {
                ViewBag.ch1 = "checked";
                ViewBag.ch2 = "";
                ViewBag.RepName = "每日交易統計表(卡機帳)";
            }
            else
            {
                ViewBag.ch1 = "";
                ViewBag.ch2 = "checked";
                ViewBag.RepName = "每日交易統計表(POS帳)";
            }

            ViewBag.GROUP = SetGroup(group); ViewBag.groups = group;
            ViewBag.MERCHANT_NAME = group;
            ViewBag.ITEM = item;
            //ViewBag.RETAIL = SetRetail(retail); ViewBag.retails = retail;
            //ViewBag.BUS = SetBus(bus); ViewBag.buses = bus;
            //ViewBag.BIKE = SetBike(bike); ViewBag.bikes = bike;
            //ViewBag.TRACK = SetTrack(track); ViewBag.tracks = track;
            //ViewBag.PARKING = SetParking(parking); ViewBag.parkings = parking;
            //ViewBag.OUTSOURCING = SetOutsourcing(outsourcing); ViewBag.outsourcings = outsourcing;

            //ViewBag.Merchant = SetMerchantDropDown(merchantNo);
            ViewBag.RepID = "170901";
            /*
            DataTable dtn = reportManager.MerchantName(merchantNo);
            if (dtn.Rows.Count > 0)
            {
                ViewBag.MerchantName = dtn.Rows[0][1].ToString()+" ";
            }
            else
            {
                ViewBag.MerchantName = "";
            }
            if (ViewBag.MerchantName == "")
            {
                merchantNo = "1";
            }
            */
            DataTable dt = new DataTable();

            List<string> mNoList = new List<string>();
            if (group == "ALL")
            {
                mNoList = new GmMerchantTypeDAO().FindAll();

                foreach (string mNo in mNoList)
                { dt.Merge(reportManager.Report170901T(start, end, mNo)); }
            }
            else if (item == "ALL")
            //else if (retail == "ALL" || bus == "ALL" || bike == "ALL" || track == "ALL" || parking == "ALL" || outsourcing == "ALL")
            {
                mNoList = new GmMerchantTypeDAO().FindGroupAll(group);
                foreach (string mNo in mNoList)
                { dt.Merge(reportManager.Report170901T(start, end, mNo)); }
            }
            else if (merchant != "")
            {
                dt = reportManager.Report170901T(start, end, merchant);
            }
            else { dt = new DataTable(); }


            long[] total = new long[8] { 0, 0, 0, 0, 0, 0, 0, 0 };
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                for (int j = 3; j < dt.Columns.Count; j++)
                {
                    total[j - 3] += Convert.ToInt64(dt.Rows[i][j]);
                }
            }

            ViewBag.Total = total;

            //DataTable dt = reportManager.Report0213T(start, end, merchantNo, SRC_FLG);
            ViewBag.Count = dt.Rows.Count;
            //return Report314Result(dt, ViewBag.MerchantName+ViewBag.RepName, "0213", "");
            if (Request.Form["searchConfirm"] != null)
            { return View(dt); }
            else if (Request.Form["ExportExcel"] != null) //產生標籤Excel
            {

                //建立Excel 2003檔案
                IWorkbook wb = new HSSFWorkbook();
                ISheet ws;
                //ISheet ws2;

                ////建立Excel 2007檔案
                //IWorkbook wb = new XSSFWorkbook();
                //ISheet ws;
                ws = wb.CreateSheet(ViewBag.RepName);

                var headerRow = ws.CreateRow(3);//第4行為欄位名稱


                HSSFCellStyle cs = (HSSFCellStyle)wb.CreateCellStyle();
                //啟動多行文字
                //cs.WrapText = true;
                //文字置中
                cs.VerticalAlignment = VerticalAlignment.Center;
                cs.Alignment = HorizontalAlignment.Center;
                //框線樣式及顏色
                cs.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
                cs.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                cs.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
                cs.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
                cs.BottomBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                cs.LeftBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                cs.RightBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                cs.TopBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                //背景顏色
                cs.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.PaleBlue.Index;
                cs.FillPattern = NPOI.SS.UserModel.FillPattern.SolidForeground;
                HSSFFont font1 = (HSSFFont)wb.CreateFont();
                //字體顏色
                font1.Color = NPOI.HSSF.Util.HSSFColor.Black.Index;
                //字體粗體
                //font1.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.Bold;
                //字體尺寸
                font1.FontHeightInPoints = 12;
                cs.SetFont(font1);
                cs.WrapText = true;

                HSSFFont font2 = (HSSFFont)wb.CreateFont();
                //字體顏色
                font2.Color = NPOI.HSSF.Util.HSSFColor.Black.Index;
                //字體粗體
                font2.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.Bold;
                //字體尺寸
                font2.FontHeightInPoints = 20;

                //headerRow.HeightInPoints = 20;
                var cell = headerRow.CreateCell(0);

                //if (RepID == "0213")
                {
                    ws.PrintSetup.Landscape = true; //橫向列印
                    headerRow = ws.CreateRow(3);

                    cell = headerRow.CreateCell(0);
                    cell.SetCellValue("特約機構");
                    cell.CellStyle = cs;

                    cell = headerRow.CreateCell(1);
                    cell.CellStyle = cs;
                    cell.SetCellValue("交易日");

                    cell = headerRow.CreateCell(2);
                    cell.CellStyle = cs;
                    cell.SetCellValue("店名");

                    cell = headerRow.CreateCell(3);
                    cell.SetCellValue("購貨");
                    cell.CellStyle = cs;
                    cell = headerRow.CreateCell(4);
                    cell.SetCellValue("");
                    cell.CellStyle = cs;
                    cell = headerRow.CreateCell(5);
                    cell.SetCellValue("");
                    cell.CellStyle = cs;
                    cell = headerRow.CreateCell(6);
                    cell.SetCellValue("");
                    cell.CellStyle = cs;

                    cell = headerRow.CreateCell(7);
                    cell.SetCellValue("加值");
                    cell.CellStyle = cs;
                    cell = headerRow.CreateCell(8);
                    cell.SetCellValue("");
                    cell.CellStyle = cs;
                    cell = headerRow.CreateCell(9);
                    cell.SetCellValue("");
                    cell.CellStyle = cs;
                    cell = headerRow.CreateCell(10);
                    cell.SetCellValue("");
                    cell.CellStyle = cs;


                    headerRow = ws.CreateRow(4);

                    cell = headerRow.CreateCell(0);
                    cell.SetCellValue("特約機構");
                    cell.CellStyle = cs;

                    cell = headerRow.CreateCell(1);
                    cell.CellStyle = cs;
                    cell.SetCellValue("交易日");

                    cell = headerRow.CreateCell(2);
                    cell.CellStyle = cs;
                    cell.SetCellValue("店名");

                    cell = headerRow.CreateCell(3);
                    cell.CellStyle = cs;
                    cell.SetCellValue("購貨");
                    cell = headerRow.CreateCell(5);
                    cell.CellStyle = cs;
                    cell.SetCellValue("購貨取消");
                    //cell = headerRow.CreateCell(8);
                    //cell.CellStyle = cs;
                    //cell.SetCellValue("小計");

                    cell = headerRow.CreateCell(7);
                    cell.CellStyle = cs;
                    cell.SetCellValue("加值");
                    cell = headerRow.CreateCell(9);
                    cell.CellStyle = cs;
                    cell.SetCellValue("加值取消");
                    cell = headerRow.CreateCell(10);
                    cell.CellStyle = cs;
                    //cell.SetCellValue("小計");
                    //cell = headerRow.CreateCell(15);
                    //cell.CellStyle = cs;
                    //cell.SetCellValue("");


                    ws.AddMergedRegion(new CellRangeAddress(3, 3, 3, 6));
                    ws.AddMergedRegion(new CellRangeAddress(3, 3, 7, 10));

                    ws.AddMergedRegion(new CellRangeAddress(3, 5, 0, 0));
                    ws.AddMergedRegion(new CellRangeAddress(3, 5, 1, 1));
                    ws.AddMergedRegion(new CellRangeAddress(3, 5, 2, 2));
                    //ws.AddMergedRegion(new CellRangeAddress(3, 5, 3, 3));

                    ws.AddMergedRegion(new CellRangeAddress(4, 4, 3, 4));
                    ws.AddMergedRegion(new CellRangeAddress(4, 4, 5, 6));
                    ws.AddMergedRegion(new CellRangeAddress(4, 4, 7, 8));
                    ws.AddMergedRegion(new CellRangeAddress(4, 4, 9, 10));
                    //ws.AddMergedRegion(new CellRangeAddress(4, 4, 12, 13));
                    //ws.AddMergedRegion(new CellRangeAddress(4, 4, 14, 15));

                }

                headerRow = ws.CreateRow(5);
                headerRow.HeightInPoints = 20;

                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    cell = headerRow.CreateCell(i);
                    cell.CellStyle = cs;

                    string[] cols = { "筆數", "金額" };
                    if (i <= 2)
                    {
                        cell.SetCellValue(dt.Columns[i].ColumnName);
                    }
                    else
                    {
                        cell.SetCellValue(cols[(i - 1) % 2]);
                    }



                    if (i == 0)
                        ws.SetColumnWidth(0, 18 * 256);
                    else if (i == 1)
                        ws.SetColumnWidth(1, 20 * 256);
                    else if (i == 2)
                        ws.SetColumnWidth(i, 20 * 256);
                    else
                        ws.SetColumnWidth(i, 20 * 256);

                }

                ICellStyle style1 = wb.CreateCellStyle();
                //框線樣式及顏色
                style1.Alignment = HorizontalAlignment.Left;
                style1.VerticalAlignment = VerticalAlignment.Center;
                //字體尺寸
                font1.FontHeightInPoints = 11;
                style1.SetFont(font1);

                ICellStyle style2 = wb.CreateCellStyle();
                //框線樣式及顏色
                style2.Alignment = HorizontalAlignment.Center;
                style2.VerticalAlignment = VerticalAlignment.Center;
                //字體尺寸
                font1.FontHeightInPoints = 11;
                //字體粗體
                style2.SetFont(font2);
                style2.WrapText = true; //自動換列

                //fill content 
                ICellStyle style3 = wb.CreateCellStyle();
                //框線樣式及顏色
                style3.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
                style3.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                style3.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
                style3.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
                style3.BottomBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                style3.LeftBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                style3.RightBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                style3.TopBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                style3.Alignment = HorizontalAlignment.Center;
                style3.VerticalAlignment = VerticalAlignment.Center;
                //字體尺寸
                font1.FontHeightInPoints = 11;
                style3.SetFont(font1);
                style3.WrapText = true;
                //style3.FillForegroundColor = IndexedColors.LemonChiffon.Index;
                //style3.FillPattern = FillPattern.SolidForeground;
                //style3.FillBackgroundColor = IndexedColors.LightGreen.Index;

                // Create the style object
                var currencyCellStyle = wb.CreateCellStyle();
                // Right-align currency values
                currencyCellStyle.Alignment = HorizontalAlignment.Right;
                currencyCellStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
                currencyCellStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                currencyCellStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
                currencyCellStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
                currencyCellStyle.BottomBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                currencyCellStyle.LeftBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                currencyCellStyle.RightBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                currencyCellStyle.TopBorderColor = NPOI.HSSF.Util.HSSFColor.Black.Index;
                font1.FontHeightInPoints = 11;
                currencyCellStyle.SetFont(font1);
                // Get / create the data format string
                //var formatId = HSSFDataFormat.GetBuiltinFormat("#,##0");
                //if (formatId == -1)
                //{
                //    var newDataFormat = wb.CreateDataFormat();
                //    currencyCellStyle.DataFormat = newDataFormat.GetFormat("#,##0");
                //}
                //else
                //currencyCellStyle.DataFormat = formatId;
                //HSSFDataFormat myformat = wb.CreateDataFormat();
                //ICellStyle newDataFormat = wb.CreateCellStyle();
                currencyCellStyle.DataFormat = wb.CreateDataFormat().GetFormat("#,##0");

                {
                    var rang = ws.CreateRow(2);
                    ICell cell3 = rang.CreateCell(0);
                    cell3.CellStyle = style1;
                    cell3.SetCellValue("查詢日：" + ViewBag.Start + "~" + ViewBag.End);

                    cell3 = ws.CreateRow(0).CreateCell(0);
                    cell3.SetCellValue(ViewBag.MerchantName + " " + ViewBag.RepName);
                    ws.AddMergedRegion(new CellRangeAddress(0, 0, 0, 11));
                    cell3.CellStyle = style2; //標題

                    rang = ws.CreateRow(1);
                    cell3 = rang.CreateCell(0);
                    cell3.CellStyle = style1;
                    cell3.SetCellValue("列印日：" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                    //ws.AddMergedRegion(new CellRangeAddress(2, 2, 0, 2));
                }

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ws.CreateRow(i + 6).HeightInPoints = 13;

                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        ICell innerCell = ws.GetRow(i + 6).CreateCell(j);
                        switch (j)
                        {
                            case 0:
                            case 1:
                            case 2:
                                innerCell.SetCellValue(dt.Rows[i][j].ToString());
                                innerCell.CellStyle = style3;
                                break;
                            //case 3:
                            //    innerCell.SetCellValue(dt.Rows[i][j].ToString());
                            //    innerCell.CellStyle = style3;
                            //    break;
                            default:
                                innerCell.SetCellValue(Convert.ToDouble(dt.Rows[i][j]));
                                innerCell.CellStyle = currencyCellStyle;
                                break;
                        }
                        //ws.AutoSizeColumn(i);
                    }
                }

                ws.CreateRow(dt.Rows.Count + 6).HeightInPoints = 13;

                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    ICell innerCell = ws.GetRow(dt.Rows.Count + 6).CreateCell(j);
                    switch (j)
                    {
                        case 0:
                            innerCell.SetCellValue("合計");
                            innerCell.CellStyle = style3;
                            break;
                        case 1:
                        case 2:
                            innerCell.SetCellValue("");
                            innerCell.CellStyle = style3;
                            break;
                        //case 3:
                        //    innerCell.SetCellValue("");
                        //    innerCell.CellStyle = style3;
                        //    break;
                        default:
                            innerCell.SetCellValue(ViewBag.Total[j - 3]);
                            innerCell.CellStyle = currencyCellStyle;
                            break;
                    }
                    //ws.AutoSizeColumn(i);
                }

                var stream = new MemoryStream();
                wb.Write(stream);
                stream.Close();

                return File(stream.ToArray(), "application/vnd.ms-excel", ViewBag.RepName + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls");

            }
            else { return View(new DataTable()); }
        }
        public dynamic colvalue { get; set; }

        public dynamic colvalue1 { get; set; }

        //20200501-20200502 汪翊新增
        //20210127 汪翊修改

        public ActionResult Report200501Result()
        {
            string start = Request.Form["startDate"];
            string end = Request.Form["EndDate"];


            ViewBag.Start = start;
            ViewBag.End = end;

            DataTable dt = reportManager.Report200501T(start, end);
            DataTable dt2 = reportManager.Report200501T_2(start, end);

            dt.NewRow();


            int[] columnWidth = { 18, 15, 18, 25, 25 };//北捷用
            int[] columnStyle = { 0, 0, 0, 0, 1, }; //columnStyle=> 0 文字置中, 1金額置右, 2文字置左
            return Report200501Export(dt, string.Format("日期:{0}-{1}", start, end), "特店加值回饋明細表", columnWidth, columnStyle, dt2: dt2);//北捷
        }


        public ActionResult Report200501Export(DataTable dt, string input, string RepName, int[] ColumnWidth, int[] columnStyle, DataTable dt2 = null)
        {
            ViewBag.Count = dt.Rows.Count;
            if (Request.Form["ExportExcel"] != null) //產生標籤Excel
            {
                //建立Excel 2003檔案
                IWorkbook wb = new HSSFWorkbook();
                ISheet ws = wb.CreateSheet(RepName);

                #region 樣式設定
                //FONT STYLE
                HSSFFont font = (HSSFFont)wb.CreateFont();
                font.Color = NPOI.HSSF.Util.HSSFColor.Black.Index;//字體顏色
                font.FontHeightInPoints = 12;//字體尺寸

                //Style 灰底 有框線
                HSSFCellStyle columnHeaderStyle = (HSSFCellStyle)wb.CreateCellStyle();
                columnHeaderStyle.VerticalAlignment = VerticalAlignment.Center;//文字置中
                columnHeaderStyle.Alignment = HorizontalAlignment.Center;
                columnHeaderStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;//框線樣式及顏色
                columnHeaderStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                columnHeaderStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
                columnHeaderStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
                columnHeaderStyle.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.Grey25Percent.Index;//背景顏色
                columnHeaderStyle.FillPattern = NPOI.SS.UserModel.FillPattern.SolidForeground;
                columnHeaderStyle.SetFont(font);

                //Style 置中 有框線
                ICellStyle contentStyle = wb.CreateCellStyle();
                contentStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
                contentStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                contentStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
                contentStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
                contentStyle.Alignment = HorizontalAlignment.Center;
                contentStyle.VerticalAlignment = VerticalAlignment.Center;
                contentStyle.SetFont(font);

                //Style 置左 有框線
                ICellStyle contentStyle2 = wb.CreateCellStyle();
                contentStyle2.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
                contentStyle2.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                contentStyle2.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
                contentStyle2.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
                contentStyle2.Alignment = HorizontalAlignment.Left;
                contentStyle2.VerticalAlignment = VerticalAlignment.Center;
                contentStyle2.SetFont(font);



                //Style 置右 金額欄位(,)
                ICellStyle moneyStyle = wb.CreateCellStyle();
                moneyStyle.Alignment = HorizontalAlignment.Right;
                moneyStyle.VerticalAlignment = VerticalAlignment.Center;
                moneyStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
                moneyStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                moneyStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
                moneyStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
                moneyStyle.DataFormat = wb.CreateDataFormat().GetFormat("#,##0");
                moneyStyle.SetFont(font);
                #endregion

                //Title
                ws.CreateRow(0).HeightInPoints = 20;
                var cell3 = ws.GetRow(0).CreateCell(0);
                cell3.SetCellValue(RepName + "  " + input);
                cell3.CellStyle = contentStyle;
                ws.AddMergedRegion(new CellRangeAddress(0, 0, 0, dt.Columns.Count - 1));

                //ColumnHeader
                var headerRow = ws.CreateRow(1);
                headerRow.HeightInPoints = 20;

                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    var cell = headerRow.CreateCell(i);
                    cell.SetCellValue(dt.Columns[i].ColumnName);
                    cell.CellStyle = columnHeaderStyle;
                    ws.SetColumnWidth(i, ColumnWidth[i] * 256);
                }

                //填入內容
                int startRow = 2;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    var row2 = ws.CreateRow(startRow + i);
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        var cell2 = row2.CreateCell(j);
                        cell2.SetCellValue(dt.Rows[i][j].ToString());
                    }
                }


                //調整格式
                //置中 置左 置右 金額
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    var row2 = ws.GetRow(startRow + i);
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        var cell2 = row2.GetCell(j);
                        switch (columnStyle[j])
                        {
                            case 0: cell2.CellStyle = contentStyle; break;
                            case 1:
                                cell2.SetCellValue(int.Parse(cell2.StringCellValue));
                                cell2.CellStyle = moneyStyle; break;
                            case 2: cell2.CellStyle = contentStyle2; break;
                        }
                    }
                }

                //填入彙總Table
                startRow += dt.Rows.Count + 1;
                if (dt2 != null)
                {
                    var headerRow2 = ws.CreateRow(startRow);
                    //填入標題
                    for (int i = 0; i < dt2.Columns.Count; i++)
                    {
                        var cell = headerRow2.CreateCell(i);
                        cell.SetCellValue(dt2.Columns[i].ColumnName);
                        cell.CellStyle = columnHeaderStyle;
                    }


                    startRow++;
                    //填入資料
                    for (int i = 0; i < dt2.Rows.Count; i++)
                    {
                        var row = ws.CreateRow(startRow + i);

                        for (int j = 0; j < dt2.Columns.Count; j++)
                        {
                            var cell = row.CreateCell(j);
                            cell.CellStyle = contentStyle;

                            if (j == 2)//金額欄位加上逗號並靠右
                            {
                                string dollar = int.Parse(dt2.Rows[i][j].ToString()).ToString("N0");
                                cell.CellStyle = moneyStyle;
                                cell.SetCellValue(dollar);
                            }
                            else
                            {
                                cell.SetCellValue(dt2.Rows[i][j].ToString());
                            }
                        }
                    }

                }

                var stream = new MemoryStream();
                wb.Write(stream);
                stream.Close();

                return File(stream.ToArray(), "application/vnd.ms-excel", RepName + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls");
            }
            else
            {
                return View(dt);
            }
        }

        public ActionResult Report200502Result()
        {
            string start = Request.Form["startDate"];
            ViewBag.Start = start;
            DataTable dt = reportManager.Report200502T(start);
            int[] columnWidth = { 15, 40, 33, 15, 12, 13, 13, 13 };
            int[] columnStyle = { 0, 2, 2, 1, 0, 1, 1, 1 };//columnStyle=> 0 文字置中, 1金額置右,2文字置左
            return Report200501Export(dt, string.Format("日期:{0}", start), "麥當勞回饋金明細表", columnWidth, columnStyle);
        }


    

    }
}