using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;

using System.IO;
using Business;
using Domain;
using DataAccess;

using OfficeOpenXml;
using System.Xml;
using OfficeOpenXml.Style;


namespace AMS.Controllers
{
    [CustomAuthorize(AccessLevel = "System,Accounting,Sales,PM,SystemSE,SalesManager")]
    public class ReportACTController : BaseController
    {
        public ReportACTManager RACTManager { get; set; }
        public GmMerchantManager GMManager { get; set; }
        //
        public ReportACTController()
        {
            RACTManager = new ReportACTManager();
            GMManager = new GmMerchantManager();
        }
        // GET: /ReportACT/

        public List<SelectListItem> SetGroup(string group)
        {
            List<SelectListItem> groups = new List<SelectListItem>();
            groups.AddRange(new SelectList(GMManager.FindAllGroup(), "MerchantNo", "MerchantName", group));
            groups.Add(new SelectListItem() { Text = "ALL", Value = "ALL", Selected = false });
            return groups;

        }
        //找小商家以外的所有群組
        public List<SelectListItem> SetGroup2(string group)
        {
            List<SelectListItem> groups = new List<SelectListItem>();
            groups.AddRange(new SelectList(GMManager.FindGroupNoMstore(), "MerchantNo", "MerchantName", group));
            groups.Add(new SelectListItem() { Text = "ALL", Value = "ALL", Selected = false });
            return groups;

        }

        public List<SelectListItem> SetRetail(string retail)
        {
            List<SelectListItem> retails = new List<SelectListItem>();
            retails.AddRange(new SelectList(GMManager.FindAllRetail(), "MerchantNo", "MerchantName", retail));
            retails.Add(new SelectListItem() { Text = "ALL", Value = "ALL", Selected = false });
            return retails;

        }

        public List<SelectListItem> SetBus(string bus)
        {
            List<SelectListItem> buses = new List<SelectListItem>();
            buses.AddRange(new SelectList(GMManager.FindAllBus(), "MerchantNo", "MerchantName", bus));
            buses.Add(new SelectListItem() { Text = "ALL", Value = "ALL", Selected = false });
            return buses;

        }

        public List<SelectListItem> SetBike(string bike)
        {
            List<SelectListItem> bikes = new List<SelectListItem>();
            bikes.AddRange(new SelectList(GMManager.FindAllBike(), "MerchantNo", "MerchantName", bike));
            bikes.Add(new SelectListItem() { Text = "ALL", Value = "ALL", Selected = false });
            return bikes;

        }

        public List<SelectListItem> SetTrack(string track)
        {
            List<SelectListItem> tracks = new List<SelectListItem>();
            tracks.AddRange(new SelectList(GMManager.FindAllTrack(), "MerchantNo", "MerchantName", track));
            tracks.Add(new SelectListItem() { Text = "ALL", Value = "ALL", Selected = false });
            return tracks;

        }

        public List<SelectListItem> SetParking(string parking)
        {
            List<SelectListItem> parkings = new List<SelectListItem>();
            parkings.AddRange(new SelectList(GMManager.FindAllParking(), "MerchantNo", "MerchantName", parking));
            parkings.Add(new SelectListItem() { Text = "ALL", Value = "ALL", Selected = false });
            return parkings;

        }

        public List<SelectListItem> SetOutsourcing(string outsourcing)
        {
            List<SelectListItem> outsourcings = new List<SelectListItem>();
            outsourcings.AddRange(new SelectList(GMManager.FindAllOutsourcing(), "MerchantNo", "MerchantName", outsourcing));
            outsourcings.Add(new SelectListItem() { Text = "ALL", Value = "ALL", Selected = false });
            return outsourcings;

        }
        public List<SelectListItem> SetRetailXDD(string retail)
        {
            List<SelectListItem> retails = new List<SelectListItem>();
            retails.AddRange(new SelectList(GMManager.FindOnlyXDD(), "MerchantNo", "MerchantName", retail));
            retails.Add(new SelectListItem() { Text = "ALL", Value = "ALL", Selected = false });
            return retails;
        }
        public List<SelectListItem> SetNCCC(string nccc)
        {
            List<SelectListItem> outsourcings = new List<SelectListItem>();
            outsourcings.AddRange(new SelectList(GMManager.FindNCCC(), "MerchantNo", "MerchantName", nccc));
            outsourcings.Add(new SelectListItem() { Text = "ALL", Value = "ALL", Selected = false });
            return outsourcings;

        }
        public List<SelectListItem> SetKSH(string ksh)
        {
            List<SelectListItem> outsourcings = new List<SelectListItem>();
            outsourcings.AddRange(new SelectList(GMManager.FindKSH(), "MerchantNo", "MerchantName", ksh));
            outsourcings.Add(new SelectListItem() { Text = "ALL", Value = "ALL", Selected = false });
            return outsourcings;

        }
        public List<SelectListItem> SetParkings(string pk)
        {
            List<SelectListItem> parking = new List<SelectListItem>();
            parking.AddRange(new SelectList(GMManager.FindParking(), "MerchantNo", "MerchantName", pk));
            //outsourcings.Add(new SelectListItem() { Text = "ALL", Value = "ALL", Selected = false });
            return parking;

        }
        /// <summary>
        /// 限零售和停車場群組
        /// </summary>
        /// <param name="pk"></param>
        /// <returns></returns>
        public List<SelectListItem> SetRetailParking(string group)
        {
            List<SelectListItem> parking = new List<SelectListItem>();
            parking.AddRange(new SelectList(GMManager.FindGroupRetailParking(), "MerchantNo", "MerchantName", group));
            //outsourcings.Add(new SelectListItem() { Text = "ALL", Value = "ALL", Selected = false });
            return parking;

        }
        /// <summary>
        /// 找群組內所有的特約機構
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        public List<SelectListItem> SetAllbyGroup(string group)
        {
            List<SelectListItem> parkings = new List<SelectListItem>();
            parkings.AddRange(new SelectList(GMManager.FindAllbyGroup(group), "MerchantNo", "MerchantName", group));
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
        /// <summary>
        /// 代收類型群組,收單行
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        public List<SelectListItem> SetMutiGroup(string group)
        {
            List<SelectListItem> groups = new List<SelectListItem>();
            groups.AddRange(new SelectList(GMManager.FindMutiGroup(), "MerchantNo", "MerchantName", group));
            //groups.Add(new SelectListItem() { Text = "ALL", Value = "ALL", Selected = false });
            return groups;

        }
        /// <summary>
        /// 找代收類型特約機構
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        public List<SelectListItem> SetMutiMerchant(string group)
        {
            List<SelectListItem> parkings = new List<SelectListItem>();
            parkings.AddRange(new SelectList(GMManager.FindMutiMerchant(group), "MerchantNo", "MerchantName", group));
            parkings.Add(new SelectListItem() { Text = "ALL", Value = "ALL", Selected = false });
            return parkings;

        }
        /// <summary>
        /// mutic merc用
        /// </summary>
        /// <param name="groupName">群組名稱</param>
        /// <returns></returns>
        private List<SelectListItem> GetMutiMerchantData(string groupName)
        {
            switch (groupName)
            {
                case "":
                    return null;
                default:
                    return SetMutiMerchant(groupName);
            }
        }
        /// <summary>
        /// 頁面json用 mutimerc
        /// </summary>
        /// <param name="groupName"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetMutiMerchant(string groupName)
        {
            List<KeyValuePair<string, string>> items = new List<KeyValuePair<string, string>>();

            if (!string.IsNullOrWhiteSpace(groupName))
            {
                var merchants = this.GetMutiMerchantData(groupName);
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
        public ActionResult RPT_160901()
        {
            string yearMonth = "";

            decimal newRate = 0;

            ViewBag.RepName = "各關係特約消費手續費率調整總表";

            if (Request.Form["searchConfirm"] != null || Request.Form["ExportExcel"] != null)
            {
                yearMonth = Request.Form["YM"];
                newRate = Convert.ToDecimal(Request.Form["NRate"]);
            }

            DataTable dt = RACTManager.ReportACT160901(yearMonth, newRate);

            if (Request.Form["searchConfirm"] != null)
                return View(dt);

            else if (Request.Form["ExportExcel"] != null)
            {
                using (ExcelPackage p = new ExcelPackage())
                {
                    ExcelWorksheet ws = p.Workbook.Worksheets.Add(yearMonth + ViewBag.RepName.ToString());

                    ExcelRange r = ws.Cells["A1:G1"];
                    r.Merge = true;
                    r.Value = yearMonth + ViewBag.RepName.ToString();

                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    r.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                    //r.Style.Font.Bold = true;
                    r.Style.Font.Size = 14;

                    int startRowNumber = 2;
                    var columns = new[] { "序號", "特約機構", "購貨淨額", "購貨手續費率", "購貨手續費(含)", "", "" };

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
                    int k = 0;
                    long untaxedTotal = 0;
                    long taxTotal = 0;

                    for (int i = 1; i <= dt.Rows.Count * 3; i++)
                    {
                        if (i % 3 == 1)
                        {
                            k++;
                            for (int j = 1; j <= columns.Length; j++)
                            {
                                switch (j)
                                {
                                    case 1:
                                        ws.Cells[i + 2, j, i + 3, j].Merge = true;
                                        ws.Cells[i + 2, j, i + 3, j].Value = k.ToString();

                                        ws.Cells[i + 2, j, i + 3, j].Style.Font.Size = 12;
                                        ws.Cells[i + 2, j, i + 3, j].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;//欄位置中
                                        ws.Cells[i + 2, j, i + 3, j].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                                        ws.Cells[i + 2, j, i + 3, j].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                                        break;

                                    case 2:
                                        ws.Cells[i + 2, j, i + 3, j].Merge = true;
                                        ws.Cells[i + 2, j, i + 3, j].Value = dt.Rows[k - 1][0].ToString();

                                        ws.Cells[i + 2, j, i + 3, j].Style.Font.Size = 12;
                                        ws.Cells[i + 2, j, i + 3, j].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;//欄位置中
                                        ws.Cells[i + 2, j, i + 3, j].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                                        ws.Cells[i + 2, j, i + 3, j].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                                        break;
                                    case 3:
                                        ws.Cells[i + 2, j].Value = Convert.ToInt64(dt.Rows[k - 1][1]);

                                        ws.Cells[i + 2, j].Style.Numberformat.Format = "#,##0";
                                        ws.Cells[i + 2, j].Style.Font.Size = 12;
                                        ws.Cells[i + 2, j].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;//欄位置右
                                        ws.Cells[i + 2, j].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                                        break;

                                    case 4:
                                        ws.Cells[i + 2, j].Value = Convert.ToDecimal(dt.Rows[k - 1][2]);

                                        ws.Cells[i + 2, j].Style.Numberformat.Format = "0.00%";
                                        ws.Cells[i + 2, j].Style.Font.Size = 12;
                                        ws.Cells[i + 2, j].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;//欄位置右
                                        ws.Cells[i + 2, j].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                                        break;

                                    case 5:
                                        ws.Cells[i + 2, j].Value = Convert.ToInt64(dt.Rows[k - 1][4]);

                                        ws.Cells[i + 2, j].Style.Numberformat.Format = "#,##0";
                                        ws.Cells[i + 2, j].Style.Font.Size = 12;
                                        ws.Cells[i + 2, j].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;//欄位置右
                                        ws.Cells[i + 2, j].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                                        break;

                                    case 6:
                                        ws.Cells[i + 2, j, i + 3, j].Merge = true;
                                        ws.Cells[i + 2, j, i + 3, j].Value = "未稅";

                                        ws.Cells[i + 2, j, i + 3, j].Style.Font.Size = 12;
                                        ws.Cells[i + 2, j, i + 3, j].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;//欄位置中
                                        ws.Cells[i + 2, j, i + 3, j].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                                        ws.Cells[i + 2, j, i + 3, j].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                                        break;
                                    case 7:
                                        ws.Cells[i + 2, j, i + 3, j].Merge = true;
                                        ws.Cells[i + 2, j, i + 3, j].Value = "稅額";

                                        ws.Cells[i + 2, j, i + 3, j].Style.Font.Size = 12;
                                        ws.Cells[i + 2, j, i + 3, j].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;//欄位置中
                                        ws.Cells[i + 2, j, i + 3, j].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                                        ws.Cells[i + 2, j, i + 3, j].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                                        break;


                                }
                            }
                        }
                        else if (i % 3 == 2)
                        {
                            for (int j = 1; j <= columns.Length; j++)
                            {
                                switch (j)
                                {

                                    case 3:
                                        ws.Cells[i + 2, j].Value = Convert.ToInt64(dt.Rows[k - 1][1]);

                                        ws.Cells[i + 2, j].Style.Numberformat.Format = "#,##0";
                                        ws.Cells[i + 2, j].Style.Font.Size = 12;
                                        ws.Cells[i + 2, j].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;//欄位置右
                                        ws.Cells[i + 2, j].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                                        break;

                                    case 4:
                                        ws.Cells[i + 2, j].Value = Convert.ToDecimal(dt.Rows[k - 1][3]);

                                        ws.Cells[i + 2, j].Style.Numberformat.Format = "0.00%";
                                        ws.Cells[i + 2, j].Style.Font.Size = 12;
                                        ws.Cells[i + 2, j].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;//欄位置右
                                        ws.Cells[i + 2, j].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                                        break;

                                    case 5:
                                        ws.Cells[i + 2, j].Value = Convert.ToInt64(dt.Rows[k - 1][5]);

                                        ws.Cells[i + 2, j].Style.Numberformat.Format = "#,##0";
                                        ws.Cells[i + 2, j].Style.Font.Size = 12;
                                        ws.Cells[i + 2, j].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;//欄位置右
                                        ws.Cells[i + 2, j].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                                        break;
                                }
                            }
                        }
                        else if (i % 3 == 0)
                        {
                            for (int j = 1; j <= columns.Length; j++)
                            {
                                switch (j)
                                {
                                    case 1:
                                        ws.Cells[i + 2, j, i + 2, j + 3].Merge = true;
                                        ws.Cells[i + 2, j, i + 2, j + 3].Value = "費率調整";

                                        ws.Cells[i + 2, j, i + 2, j + 3].Style.Font.Size = 12;
                                        ws.Cells[i + 2, j, i + 2, j + 3].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;//欄位置中
                                        ws.Cells[i + 2, j, i + 2, j + 3].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                                        break;
                                    case 5:
                                        ws.Cells[i + 2, j].Value = dt.Rows[k - 1][6];

                                        ws.Cells[i + 2, j].Style.Numberformat.Format = "#,##0";
                                        ws.Cells[i + 2, j].Style.Font.Size = 12;
                                        ws.Cells[i + 2, j].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;//欄位置右
                                        ws.Cells[i + 2, j].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                                        break;
                                    case 6:
                                        ws.Cells[i + 2, j].Value = dt.Rows[k - 1][7]; untaxedTotal += Convert.ToInt64(dt.Rows[k - 1][7]);

                                        ws.Cells[i + 2, j].Style.Numberformat.Format = "#,##0";
                                        ws.Cells[i + 2, j].Style.Font.Size = 12;
                                        ws.Cells[i + 2, j].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;//欄位置右
                                        ws.Cells[i + 2, j].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                                        break;
                                    case 7:
                                        ws.Cells[i + 2, j].Value = dt.Rows[k - 1][8]; taxTotal += Convert.ToInt64(dt.Rows[k - 1][8]);

                                        ws.Cells[i + 2, j].Style.Numberformat.Format = "#,##0";
                                        ws.Cells[i + 2, j].Style.Font.Size = 12;
                                        ws.Cells[i + 2, j].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;//欄位置右
                                        ws.Cells[i + 2, j].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                                        break;

                                }

                            }
                        }
                    }
                    ws.Cells[dt.Rows.Count * 3 + 3, 1, dt.Rows.Count * 3 + 3, 5].Merge = true;
                    ws.Cells[dt.Rows.Count * 3 + 3, 1, dt.Rows.Count * 3 + 3, 5].Value = "總計";
                    ws.Cells[dt.Rows.Count * 3 + 3, 1, dt.Rows.Count * 3 + 3, 5].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    ws.Cells[dt.Rows.Count * 3 + 3, 1, dt.Rows.Count * 3 + 3, 5].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                    ws.Cells[dt.Rows.Count * 3 + 3, 6].Value = untaxedTotal;
                    ws.Cells[dt.Rows.Count * 3 + 3, 6].Style.Numberformat.Format = "#,##0";
                    ws.Cells[dt.Rows.Count * 3 + 3, 6].Style.Font.Size = 12;
                    ws.Cells[dt.Rows.Count * 3 + 3, 6].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                    ws.Cells[dt.Rows.Count * 3 + 3, 6].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                    ws.Cells[dt.Rows.Count * 3 + 3, 7].Value = taxTotal;
                    ws.Cells[dt.Rows.Count * 3 + 3, 7].Style.Numberformat.Format = "#,##0";
                    ws.Cells[dt.Rows.Count * 3 + 3, 7].Style.Font.Size = 12;
                    ws.Cells[dt.Rows.Count * 3 + 3, 7].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                    ws.Cells[dt.Rows.Count * 3 + 3, 7].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                    ws.Cells[ws.Dimension.Address].AutoFitColumns();

                    MemoryStream stream = new MemoryStream();
                    p.SaveAs(stream);
                    stream.Close();
                    return File(stream.ToArray(), "application/vnd.ms-excel", ViewBag.RepName.ToString() + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx");
                }


            }

            else return View(new DataTable());

        }

        public ActionResult RPT_161101()
        {
            string yearMonth = "";

            string merchant = "";

            ViewBag.RepName = "各特約機構重複性資料統計表";

            yearMonth = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString().PadLeft(2, '0');

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
                yearMonth = Request.Form["yearMonth"];
                //merchant = Request.Form["merchant"]; 

                group = Request.Form["group"];
                item = Request.Form["items"];
                merchant = Request.Form["items"];
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
            }

            ViewBag.YearMonth = yearMonth;
            //ViewData["merchant"] = merchant;
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

            /*
            List<SelectListItem> merchantList = new List<SelectListItem>();

            List<GmMerchant> merchants = new GmMerchantManager().FindAll();
            IEnumerable<GmMerchant> distinctMerchants = from k in merchants
                                                        group k by new { k.MerchantNo, k.MerchantName } into g
                                                        select g.FirstOrDefault();
            SelectList list = new SelectList(distinctMerchants, "MerchantNo", "MerchantName");
            merchantList.AddRange(list);

            ViewBag.MerchantList = merchantList;
            */


            string firstDate = yearMonth + "01";

            string lastDate = DateTime.ParseExact(yearMonth + "01", "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture).AddMonths(1).AddDays(-1).ToString("yyyyMMdd");

            //DataTable dt = RACTManager.ReportACT161101(firstDate,lastDate,merchant);
            DataTable dt = new DataTable();
            List<string> mNoList = new List<string>();
            if (group == "ALL")
            {
                mNoList = new GmMerchantTypeDAO().FindAll();

                foreach (string mNo in mNoList)
                { dt.Merge(RACTManager.ReportACT161101(firstDate, lastDate, mNo)); }
            }
            else if (retail == "ALL" || bus == "ALL" || bike == "ALL" || track == "ALL" || parking == "ALL" || outsourcing == "ALL")
            {
                mNoList = new GmMerchantTypeDAO().FindGroupAll(group);
                foreach (string mNo in mNoList)
                { dt.Merge(RACTManager.ReportACT161101(firstDate, lastDate, mNo)); }
            }
            else if (merchant != "")
            {
                dt = RACTManager.ReportACT161101(firstDate, lastDate, merchant);
            }
            else { dt = new DataTable(); }

            if (Request.Form["searchConfirm"] != null)
                return View(dt);
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
                    var columns = new[] { "特約機構", "交易代號", "交易金額", "帳務認列", "卡務認列" };

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

                                case 3:
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

        public ActionResult RPT_161201()
        {
            string settleDate = "";

            string merchant = "";

            string group = "";
            string item = "";
            string retail = "";
            string bus = "";
            string bike = "";
            string track = "";
            string parking = "";
            string outsourcing = "";

            ViewBag.RepName = "特約機構每日清分彙總表";

            settleDate = DateTime.Now.ToString("yyyyMMdd");

            if (Request.Form["searchConfirm"] != null || Request.Form["ExportExcel"] != null)
            {
                settleDate = Request.Form["settleDate"];
                // merchant = Request.Form["merchant"];
                group = Request.Form["group"];
                item = Request.Form["items"];
                merchant = Request.Form["items"];
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
            }

            ViewBag.SettleDate = settleDate;
            ViewBag.MERCHANT = SetGroup(group);
            ViewBag.MERCHANT_NAME = group;
            ViewBag.ITEM = item;
            //ViewData["merchant"] = merchant;
            //ViewBag.GROUP = SetGroup(group); ViewBag.groups = group;
            //ViewBag.RETAIL = SetRetail(retail); ViewBag.retails = retail;
            //ViewBag.BUS = SetBus(bus); ViewBag.buses = bus;
            //ViewBag.BIKE = SetBike(bike); ViewBag.bikes = bike;
            //ViewBag.TRACK = SetTrack(track); ViewBag.tracks = track;
            //ViewBag.PARKING = SetParking(parking); ViewBag.parkings = parking;
            //ViewBag.OUTSOURCING = SetOutsourcing(outsourcing); ViewBag.outsourcings = outsourcing;

            /*
            List<SelectListItem> merchantList = new List<SelectListItem>();

            List<GmMerchant> merchants = new GmMerchantManager().FindAll();
            IEnumerable<GmMerchant> distinctMerchants = from k in merchants
                                                        group k by new { k.MerchantNo, k.MerchantName } into g
                                                        select g.FirstOrDefault();
            SelectList list = new SelectList(distinctMerchants, "MerchantNo", "MerchantName");
            merchantList.AddRange(list);

            ViewBag.Merchants = merchantList;
            */


            //DataTable dt = RACTManager.ReportACT161201(settleDate, merchant);
            DataTable dt = new DataTable();
            List<string> mNoList = new List<string>();
            if (group == "ALL")
            {
                mNoList = new GmMerchantTypeDAO().FindAll();

                foreach (string mNo in mNoList)
                { dt.Merge(RACTManager.ReportACT161201(settleDate, mNo)); }
            }
            else if (item == "ALL")
            //else if (retail == "ALL" || bus == "ALL" || bike == "ALL" || track == "ALL" || parking == "ALL" || outsourcing == "ALL")
            {
                mNoList = new GmMerchantTypeDAO().FindGroupAll(group);
                foreach (string mNo in mNoList)
                { dt.Merge(RACTManager.ReportACT161201(settleDate, mNo)); }
            }
            else if (merchant != "")
            {
                dt = RACTManager.ReportACT161201(settleDate, merchant);
            }
            else { dt = new DataTable(); }

            if (Request.Form["searchConfirm"] != null)
                return View(dt);
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
                    var columns = new[] { "特約機構", "交易類別", "交易筆數", "交易金額", "交易類型" };

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
                                case 3:
                                case 4:
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

        public ActionResult RPT_161202()
        {
            string settleDate = "";
            string sDate = "";
            string eDate = "";
            string merchant = "";

            string group = "";
            string item = "";
            string retail = "";
            string bus = "";
            string bike = "";
            string track = "";
            string parking = "";
            string outsourcing = "";

            ViewBag.RepName = "特約機構每日清分明細表";

            settleDate = DateTime.Now.ToString("yyyyMMdd");
            sDate = DateTime.Now.ToString("yyyyMMdd");
            eDate = DateTime.Now.ToString("yyyyMMdd");

            if (Request.Form["searchConfirm"] != null || Request.Form["ExportExcel"] != null)
            {
                sDate = Request.Form["sDate"];
                eDate = Request.Form["eDate"];
                //merchant = Request.Form["merchant"];
                group = Request.Form["group"];
                item = Request.Form["items"];
                merchant = Request.Form["items"];
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
            }

            ViewBag.StartDate = sDate;
            ViewBag.EndDate = eDate;
            //ViewData["merchant"] = merchant;
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

            /*
            List<SelectListItem> merchantList = new List<SelectListItem>();

            List<GmMerchant> merchants = new GmMerchantManager().FindAll();
            IEnumerable<GmMerchant> distinctMerchants = from k in merchants
                                                        group k by new { k.MerchantNo, k.MerchantName } into g
                                                        select g.FirstOrDefault();
            SelectList list = new SelectList(distinctMerchants, "MerchantNo", "MerchantName");
            merchantList.AddRange(list);

            ViewBag.Merchants = merchantList;
            */


            //DataTable dt = RACTManager.ReportACT161202(settleDate, merchant);
            DataTable dt = new DataTable();
            List<string> mNoList = new List<string>();
            if (group == "ALL")
            {
                mNoList = new GmMerchantTypeDAO().FindAll();

                foreach (string mNo in mNoList)
                { dt.Merge(RACTManager.ReportACT161202(sDate, eDate, mNo)); }
            }
            else if (merchant != "" && merchant != "ALL")
            {
                dt = RACTManager.ReportACT161202(sDate, eDate, merchant);
            }
            else if (item == "ALL")
            //else if (retail == "ALL" || bus == "ALL" || bike == "ALL" || track == "ALL" || parking == "ALL" || outsourcing == "ALL")
            {
                mNoList = new GmMerchantTypeDAO().FindGroupAll(group);
                foreach (string mNo in mNoList)
                { dt.Merge(RACTManager.ReportACT161202(sDate, eDate, mNo)); }
            }
            else { dt = new DataTable(); }

            if (Request.Form["searchConfirm"] != null)
                return View(dt);
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
                    var columns = new[] { "特約機構", "清算日期", "門市代號", "門市名稱", "收銀機代號", "卡機交易時間", "交易類型", "交易金額", "預計撥匯款日期" };

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
                                case 8:

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

        public ActionResult RPT_170101()
        {
            string yearMonth = "";

            //string merchant = "";

            ViewBag.RepName = "月結-超商自動加值與企業加值";

            yearMonth = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString().PadLeft(2, '0');

            if (Request.Form["searchConfirm"] != null || Request.Form["ExportExcel"] != null)
            {
                yearMonth = Request.Form["yearMonth"];
                //merchant = Request.Form["merchant"];
            }

            ViewBag.YearMonth = yearMonth;
            //ViewData["merchant"] = merchant;

            /*
            List<SelectListItem> merchantList = new List<SelectListItem>();

            List<GmMerchant> merchants = new GmMerchantManager().FindAll();
            IEnumerable<GmMerchant> distinctMerchants = from k in merchants
                                                        group k by new { k.MerchantNo, k.MerchantName } into g
                                                        select g.FirstOrDefault();
            SelectList list = new SelectList(distinctMerchants, "MerchantNo", "MerchantName");
            merchantList.AddRange(list);

            ViewBag.MerchantList = merchantList;
            */

            string firstDate = yearMonth + "01";

            string lastDate = DateTime.ParseExact(yearMonth + "01", "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture).AddMonths(1).AddDays(-1).ToString("yyyyMMdd");

            DataTable dt = RACTManager.ReportACT170101(firstDate, lastDate);

            if (Request.Form["searchConfirm"] != null)
                return View(dt);
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
                    var columns = new[] { "清算日期", "自動加值總額", "POS自動加值額", "ibon自動加值額", "ibon企業禮金加值額", "自動加值筆數", "POS自動加值筆數", "ibon自動加值筆數", "ibon企業禮金加值筆數" };

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

                                case 2:
                                case 3:
                                case 4:
                                case 5:
                                case 6:
                                case 7:
                                case 8:
                                case 9:
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

        public ActionResult RPT_170102()
        {
            string yearMonth = "";

            //string merchant = "";

            ViewBag.RepName = "月結-超商POS代收售統計";

            yearMonth = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString().PadLeft(2, '0');

            if (Request.Form["searchConfirm"] != null || Request.Form["ExportExcel"] != null)
            {
                yearMonth = Request.Form["yearMonth"];
                //merchant = Request.Form["merchant"];
            }

            ViewBag.YearMonth = yearMonth;
            //ViewData["merchant"] = merchant;

            /*
            List<SelectListItem> merchantList = new List<SelectListItem>();

            List<GmMerchant> merchants = new GmMerchantManager().FindAll();
            IEnumerable<GmMerchant> distinctMerchants = from k in merchants
                                                        group k by new { k.MerchantNo, k.MerchantName } into g
                                                        select g.FirstOrDefault();
            SelectList list = new SelectList(distinctMerchants, "MerchantNo", "MerchantName");
            merchantList.AddRange(list);

            ViewBag.MerchantList = merchantList;
            */

            string firstDate = yearMonth + "02";

            string lastDate = DateTime.ParseExact(yearMonth + "02", "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture).AddMonths(1).AddDays(-1).ToString("yyyyMMdd");

            DataTable dt = RACTManager.ReportACT170102(firstDate, lastDate);

            if (Request.Form["searchConfirm"] != null)
                return View(dt);
            else if (Request.Form["ExportExcel"] != null)
            {
                using (ExcelPackage p = new ExcelPackage())
                {
                    ExcelWorksheet ws = p.Workbook.Worksheets.Add(ViewBag.RepName.ToString());


                    ExcelRange r = ws.Cells["A1:K1"];

                    r.Merge = true;
                    r.Value = ViewBag.RepName.ToString();


                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    r.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                    //r.Style.Font.Bold = true;
                    r.Style.Font.Size = 14;

                    int startRowNumber = 2;
                    var columns = new[] { "傳輸日", "交易類別", "ㄧ般金額", "健康捐", "代收金額", "代售金額", "交易類別", "ㄧ般金額", "健康捐", "代收金額", "代售金額" };

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

                                case 3:
                                case 4:
                                case 5:
                                case 6:
                                case 8:
                                case 9:
                                case 10:
                                case 11:
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

        public ActionResult RPT_170103()
        {
            string yearMonth = "";

            //string merchant = "";

            ViewBag.RepName = "月結-台鐵交易統計表";

            yearMonth = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString().PadLeft(2, '0');

            if (Request.Form["searchConfirm"] != null || Request.Form["ExportExcel"] != null)
            {
                yearMonth = Request.Form["yearMonth"];
                //merchant = Request.Form["merchant"];
            }

            ViewBag.YearMonth = yearMonth;
            //ViewData["merchant"] = merchant;

            /*
            List<SelectListItem> merchantList = new List<SelectListItem>();

            List<GmMerchant> merchants = new GmMerchantManager().FindAll();
            IEnumerable<GmMerchant> distinctMerchants = from k in merchants
                                                        group k by new { k.MerchantNo, k.MerchantName } into g
                                                        select g.FirstOrDefault();
            SelectList list = new SelectList(distinctMerchants, "MerchantNo", "MerchantName");
            merchantList.AddRange(list);

            ViewBag.MerchantList = merchantList;
            */

            string firstDate = yearMonth + "01";

            string lastDate = DateTime.ParseExact(yearMonth + "01", "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture).AddMonths(1).AddDays(-1).ToString("yyyyMMdd");

            DataTable dt = RACTManager.ReportACT170103(firstDate, lastDate);

            if (Request.Form["searchConfirm"] != null)
                return View(dt);
            else if (Request.Form["ExportExcel"] != null)
            {
                using (ExcelPackage p = new ExcelPackage())
                {
                    ExcelWorksheet ws = p.Workbook.Worksheets.Add(ViewBag.RepName.ToString());


                    ExcelRange r = ws.Cells["A1:AA1"];
                    r.Merge = true;
                    r.Value = ViewBag.RepName.ToString();


                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    r.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                    //r.Style.Font.Bold = true;
                    r.Style.Font.Size = 14;

                    int startRowNumber = 2;
                    //var columns = new[] { "特約機構代號", "清分日", "交易日期", "購貨金額", "購貨有值筆數", "購貨取消金額", "購貨取消有值筆數", "購貨手續費率", "加值金額", "加值筆數", "加值取消金額", "加值取消筆數", "加值手續費率" };
                    //20211018加值類分為3小類
                    var columns = new[] { "特約機構代號", "清分日", "交易日期", "購貨金額", "購貨有值筆數", "購貨取消金額", "購貨取消有值筆數", "購貨手續費率", "其他加值金額", "其他加值筆數", "其他加值取消金額", "其他加值取消筆數", "其他加值手續費率", "ATIM加值金額", "ATIM加值筆數", "ATIM加值取消金額", "ATIM加值取消筆數", "ATIM加值手續費率", "晚點補值加值金額", "晚點補值加值筆數", "晚點補值加值取消金額", "晚點補值加值取消筆數", "晚點補值加值手續費率", "小計金額", "小計加值筆數", "小計加值取消金額", "小計加值取消筆數" };
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

                                case 4:
                                case 5:
                                case 6:
                                case 7:
                                case 9:
                                case 10:
                                case 11:
                                case 12:
                                case 14:
                                case 15:
                                case 16:
                                case 17:
                                case 19:
                                case 20:
                                case 21:
                                case 22:
                                case 24:
                                case 25:
                                case 26:
                                case 27:
                                    ws.Cells[i + 2, j].Value = Convert.ToDouble(dt.Rows[i - 1][j - 1]);


                                    ws.Cells[i + 2, j].Style.Numberformat.Format = "#,##0";
                                    ws.Cells[i + 2, j].Style.Font.Size = 12;
                                    ws.Cells[i + 2, j].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;//欄位置右
                                    ws.Cells[i + 2, j].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                                    break;
                                case 8:
                                case 13:
                                case 18:
                                case 23:
                                    ws.Cells[i + 2, j].Value = Convert.ToDouble(dt.Rows[i - 1][j - 1]);


                                    ws.Cells[i + 2, j].Style.Numberformat.Format = "#,##0.000";
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

        public ActionResult RPT_170104()
        {
            string yearMonth = "";

            //string merchant = "";

            ViewBag.RepName = "月結-高捷交易統計表";

            yearMonth = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString().PadLeft(2, '0');

            if (Request.Form["searchConfirm"] != null || Request.Form["ExportExcel"] != null)
            {
                yearMonth = Request.Form["yearMonth"];
                //merchant = Request.Form["merchant"];
            }

            ViewBag.YearMonth = yearMonth;
            //ViewData["merchant"] = merchant;

            /*
            List<SelectListItem> merchantList = new List<SelectListItem>();

            List<GmMerchant> merchants = new GmMerchantManager().FindAll();
            IEnumerable<GmMerchant> distinctMerchants = from k in merchants
                                                        group k by new { k.MerchantNo, k.MerchantName } into g
                                                        select g.FirstOrDefault();
            SelectList list = new SelectList(distinctMerchants, "MerchantNo", "MerchantName");
            merchantList.AddRange(list);

            ViewBag.MerchantList = merchantList;
            */

            string firstDate = yearMonth + "01";

            string lastDate = DateTime.ParseExact(yearMonth + "01", "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture).AddMonths(1).AddDays(-1).ToString("yyyyMMdd");

            DataTable dt = RACTManager.ReportACT170104(firstDate, lastDate);

            if (Request.Form["searchConfirm"] != null)
                return View(dt);
            else if (Request.Form["ExportExcel"] != null)
            {
                using (ExcelPackage p = new ExcelPackage())
                {
                    ExcelWorksheet ws = p.Workbook.Worksheets.Add(ViewBag.RepName.ToString());


                    ExcelRange r = ws.Cells["A1:M1"];
                    r.Merge = true;
                    r.Value = ViewBag.RepName.ToString();


                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    r.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                    //r.Style.Font.Bold = true;
                    r.Style.Font.Size = 14;

                    int startRowNumber = 2;
                    var columns = new[] { "特約機構代號", "清分日", "交易日期", "購貨金額", "購貨有值筆數", "購貨取消金額", "購貨取消有值筆數", "購貨手續費率", "加值金額", "加值筆數", "加值取消金額", "加值取消筆數", "加值手續費率" };

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

                                case 4:
                                case 5:
                                case 6:
                                case 7:
                                case 9:
                                case 10:
                                case 11:
                                case 12:
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

        public ActionResult RPT_170105()
        {
            string yearMonth = "";

            //string merchant = "";

            ViewBag.RepName = "月結-高捷交易統計表(自動加值)";

            yearMonth = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString().PadLeft(2, '0');

            if (Request.Form["searchConfirm"] != null || Request.Form["ExportExcel"] != null)
            {
                yearMonth = Request.Form["yearMonth"];
                //merchant = Request.Form["merchant"];
            }

            ViewBag.YearMonth = yearMonth;
            //ViewData["merchant"] = merchant;

            /*
            List<SelectListItem> merchantList = new List<SelectListItem>();

            List<GmMerchant> merchants = new GmMerchantManager().FindAll();
            IEnumerable<GmMerchant> distinctMerchants = from k in merchants
                                                        group k by new { k.MerchantNo, k.MerchantName } into g
                                                        select g.FirstOrDefault();
            SelectList list = new SelectList(distinctMerchants, "MerchantNo", "MerchantName");
            merchantList.AddRange(list);

            ViewBag.MerchantList = merchantList;
            */

            string firstDate = yearMonth + "01";

            string lastDate = DateTime.ParseExact(yearMonth + "01", "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture).AddMonths(1).AddDays(-1).ToString("yyyyMMdd");

            DataTable dt = RACTManager.ReportACT170105(firstDate, lastDate);

            if (Request.Form["searchConfirm"] != null)
                return View(dt);
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
                    var columns = new[] { "特約機構代號", "清分日", "交易日期", "自動加值金額", "自動加值筆數" };

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

                                case 4:
                                case 5:

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
        public ActionResult RPT_170201()
        {
            string settleDate = "";

            string merchant = "";

            ViewBag.RepName = "北捷剔退明細表";

            settleDate = DateTime.Now.ToString("yyyyMMdd");

            if (Request.Form["searchConfirm"] != null || Request.Form["ExportExcel"] != null)
            {
                settleDate = Request.Form["settleDate"];
                merchant = Request.Form["merchant"];
            }

            ViewBag.SettleDate = settleDate;
            ViewData["merchant"] = merchant;


            List<SelectListItem> merchantList = new List<SelectListItem>();

            List<GmMerchant> merchants = new GmMerchantManager().FindAll();
            IEnumerable<GmMerchant> distinctMerchants = from k in merchants
                                                        group k by new { k.MerchantNo, k.MerchantName } into g
                                                        select g.FirstOrDefault();
            SelectList list = new SelectList(distinctMerchants, "MerchantNo", "MerchantName");
            merchantList.AddRange(list);

            ViewBag.Merchants = merchantList;



            DataTable dt = RACTManager.ReportACT170201(settleDate);

            if (Request.Form["searchConfirm"] != null)
                return View(dt);
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
                    var columns = new[] { "清算日期", "車站代號", "車站名稱", "卡機代號", "卡機交易時間", "卡號", "交易類型", "交易金額", "剔退原因" };

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
                                case 8:

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
        public ActionResult RPT_170202()
        {
            string settleDate = "";

            string merchant = "";

            ViewBag.RepName = "北捷每日清分剔退彙總表";

            settleDate = DateTime.Now.ToString("yyyyMMdd");

            if (Request.Form["searchConfirm"] != null || Request.Form["ExportExcel"] != null)
            {
                settleDate = Request.Form["settleDate"];

            }

            ViewBag.SettleDate = settleDate;



            DataTable dt = RACTManager.ReportACT170202(settleDate);

            if (Request.Form["searchConfirm"] != null)
                return View(dt);
            else if (Request.Form["ExportExcel"] != null)
            {
                using (ExcelPackage p = new ExcelPackage())
                {
                    ExcelWorksheet ws = p.Workbook.Worksheets.Add(ViewBag.RepName.ToString());


                    ExcelRange r = ws.Cells["A1:D1"];
                    r.Merge = true;
                    r.Value = ViewBag.RepName.ToString();


                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    r.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                    //r.Style.Font.Bold = true;
                    r.Style.Font.Size = 14;

                    int startRowNumber = 2;
                    var columns = new[] { "交易類別", "交易筆數", "交易金額", "剔退原因" };

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
                                case 2:
                                case 3:
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

        public ActionResult RPT_170401()
        {
            string yearMonth = "";

            ViewBag.RepName = "每月支付代收售報表";

            yearMonth = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString().PadLeft(2, '0');

            if (Request.Form["searchConfirm"] != null || Request.Form["ExportExcel"] != null)
            {
                yearMonth = Request.Form["yearMonth"];
                //merchant = Request.Form["merchant"];
            }

            ViewBag.YearMonth = yearMonth;

            string firstDate = yearMonth + "02";

            string lastDate = DateTime.ParseExact(yearMonth + "01", "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture).AddMonths(1).ToString("yyyyMMdd");

            DataTable dt = RACTManager.ReportACT170401(yearMonth, firstDate, lastDate);

            if (Request.Form["searchConfirm"] != null)
                return View(dt);
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
                    var columns = new[] { "月份", "icash支付代收售金額(A) A=B+C", "聯名卡支付代收售金額(B)", "非聯名卡支付代收售金額(B)", "非聯名卡支付代收售現金儲值成本(D) D=C*0.2%" };

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
                            if (!DBNull.Value.Equals(dt.Rows[i - 1][j - 1]))
                            {
                                switch (j)
                                {
                                    case 2:
                                    case 3:
                                    case 4:

                                        ws.Cells[i + 2, j].Value = Convert.ToDouble(dt.Rows[i - 1][j - 1]);

                                        ws.Cells[i + 2, j].Style.Numberformat.Format = "#,##0";
                                        ws.Cells[i + 2, j].Style.Font.Size = 12;
                                        ws.Cells[i + 2, j].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;//欄位置右
                                        ws.Cells[i + 2, j].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                                        break;

                                    case 5:

                                        ws.Cells[i + 2, j].Value = Convert.ToDouble(dt.Rows[i - 1][j - 1]);

                                        ws.Cells[i + 2, j].Style.Numberformat.Format = "#,##0.00";
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

        public ActionResult RPT_170501()
        {
            string yearMonth = "";

            ViewBag.RepName = "手續費彙總表";

            yearMonth = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString().PadLeft(2, '0');

            if (Request.Form["searchConfirm"] != null || Request.Form["ExportExcel"] != null)
            {
                yearMonth = Request.Form["yearMonth"];
                //merchant = Request.Form["merchant"];
            }

            ViewBag.YearMonth = yearMonth;
            DataTable dt = new DataTable();
            if (Request.Form["searchConfirm"] != null || Request.Form["ExportExcel"] != null)
            {
                dt = RACTManager.ReportACT170501(yearMonth);
            }
            if (Request.Form["searchConfirm"] != null)
                return View(dt);
            else if (Request.Form["ExportExcel"] != null)
            {
                using (ExcelPackage p = new ExcelPackage())
                {
                    ExcelWorksheet ws = p.Workbook.Worksheets.Add(ViewBag.RepName.ToString());


                    ExcelRange r = ws.Cells["A1:W1"];
                    r.Merge = true;
                    r.Value = ViewBag.RepName.ToString();


                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    r.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                    //r.Style.Font.Bold = true;
                    r.Style.Font.Size = 14;

                    int startRowNumber = 2;
                    var columns = new[] { "序號","統編", "特店代碼", "門市代號對應值(手續費)", "特店性質", "單別代號", "特約機構","簡稱", "購貨淨額", "重複", "調帳", "購貨手續費率", "消費手續費", "未稅", "稅金", "加值淨額", "重複", "調帳", "加值手續費率", "加值手續費", "未稅", "稅金", "自動加值額", "重複", "調帳", "自動加值手續費率", "自動加值手續費", "未稅", "稅金" };

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
                            if (!DBNull.Value.Equals(dt.Rows[i - 1][j - 1]))
                            {
                                switch (j)
                                {
                                    //case 3:
                                    //case 7:
                                    //case 8:
                                    //case 9:
                                    //case 10:
                                    //case 14:
                                    //case 15:
                                    //case 16:
                                    //case 17:
                                    //case 21:
                                    //case 22:
                                    //case 23:
                                    //20200917 新增欄位
                                    //case 5:
                                    //case 9:
                                    //case 10:
                                    //case 11:
                                    //case 12:
                                    //case 16:
                                    //case 17:
                                    //case 18:
                                    //case 19:
                                    //case 23:
                                    //case 24:
                                    //case 25:
                                    case 9:
                                    case 13:
                                    case 14:
                                    case 15:
                                    case 16:
                                    case 20:
                                    case 21:
                                    case 22:
                                    case 23:
                                    case 27:
                                    case 28:
                                    case 29:

                                        ws.Cells[i + 2, j].Value = Convert.ToDouble(dt.Rows[i - 1][j - 1]);

                                        ws.Cells[i + 2, j].Style.Numberformat.Format = "#,##0";
                                        ws.Cells[i + 2, j].Style.Font.Size = 12;
                                        ws.Cells[i + 2, j].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;//欄位置右
                                        ws.Cells[i + 2, j].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                                        break;

                                    //case 6:
                                    //case 13:
                                    //case 20:
                                    //20200917
                                    //case 8:
                                    //case 15:
                                    //case 22:
                                    case 12:
                                    case 19:
                                    case 26:

                                        ws.Cells[i + 2, j].Value = Convert.ToDouble(dt.Rows[i - 1][j - 1]);

                                        ws.Cells[i + 2, j].Style.Numberformat.Format = "#,##0.00";
                                        ws.Cells[i + 2, j].Style.Font.Size = 12;
                                        ws.Cells[i + 2, j].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;//欄位置右
                                        ws.Cells[i + 2, j].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                                        break;

                                    default:

                                        ws.Cells[i + 2, j].Value = dt.Rows[i - 1][j - 1].ToString()+"";

                                        ws.Cells[i + 2, j].Style.Font.Size = 12;
                                        ws.Cells[i + 2, j].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center; //欄位置中
                                        ws.Cells[i + 2, j].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                                        break;
                                }
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
        public ActionResult RPT_170801()
        {
            string settleDate = "";

            string merchant = "";

            string group = "";
            string item = "";
            string retail = "";
            string bus = "";
            string bike = "";
            string track = "";
            string parking = "";
            string outsourcing = "";

            ViewBag.RepName = "特約機構每日清分明細彙總表";

            settleDate = DateTime.Now.ToString("yyyyMMdd");

            if (Request.Form["searchConfirm"] != null || Request.Form["ExportExcel"] != null)
            {
                settleDate = Request.Form["settleDate"];
                //merchant = Request.Form["merchant"];
                group = Request.Form["group"];
                item = Request.Form["items"];
                merchant = Request.Form["items"];

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
            }

            ViewBag.SettleDate = settleDate;
            ViewBag.MERCHANT = SetGroup(group);
            ViewBag.MERCHANT_NAME = group;
            ViewBag.ITEM = item;
            //ViewData["merchant"] = merchant;
            //ViewBag.GROUP = SetGroup(group); ViewBag.groups = group;
            //ViewBag.RETAIL = SetRetail(retail); ViewBag.retails = retail;
            //ViewBag.BUS = SetBus(bus); ViewBag.buses = bus;
            //ViewBag.BIKE = SetBike(bike); ViewBag.bikes = bike;
            //ViewBag.TRACK = SetTrack(track); ViewBag.tracks = track;
            //ViewBag.PARKING = SetParking(parking); ViewBag.parkings = parking;
            //ViewBag.OUTSOURCING = SetOutsourcing(outsourcing); ViewBag.outsourcings = outsourcing;

            /*
            List<SelectListItem> merchantList = new List<SelectListItem>();

            List<GmMerchant> merchants = new GmMerchantManager().FindAll();
            IEnumerable<GmMerchant> distinctMerchants = from k in merchants
                                                        group k by new { k.MerchantNo, k.MerchantName } into g
                                                        select g.FirstOrDefault();
            SelectList list = new SelectList(distinctMerchants, "MerchantNo", "MerchantName");
            merchantList.AddRange(list);

            ViewBag.Merchants = merchantList;
            */


            //DataTable dt = RACTManager.ReportACT170801(settleDate, merchant);
            DataTable dt = new DataTable();
            List<string> mNoList = new List<string>();
            if (group == "ALL")
            {
                mNoList = new GmMerchantTypeDAO().FindAll();

                foreach (string mNo in mNoList)
                { dt.Merge(RACTManager.ReportACT170801(settleDate, mNo)); }
            }
            else if (item == "ALL")
            //else if (retail == "ALL" || bus == "ALL" || bike == "ALL" || track == "ALL" || parking == "ALL" || outsourcing == "ALL")
            {
                mNoList = new GmMerchantTypeDAO().FindGroupAll(group);
                foreach (string mNo in mNoList)
                { dt.Merge(RACTManager.ReportACT170801(settleDate, mNo)); }
            }
            else if (merchant != "")
            {
                dt = RACTManager.ReportACT170801(settleDate, merchant);
            }
            else { dt = new DataTable(); }

            if (Request.Form["searchConfirm"] != null)
                return View(dt);
            else if (Request.Form["ExportExcel"] != null)
            {
                using (ExcelPackage p = new ExcelPackage())
                {
                    ExcelWorksheet ws = p.Workbook.Worksheets.Add(ViewBag.RepName.ToString());


                    //ExcelRange r = ws.Cells["A1:E1"];
                    //r.Merge = true;
                    //r.Value = ViewBag.RepName.ToString();


                    //r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    //r.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                    ////r.Style.Font.Bold = true;
                    //r.Style.Font.Size = 14;

                    int startRowNumber = 1;
                    var columns = new[] { "特約機構", "交易日期", "交易類別", "交易筆數", "交易總金額" };

                    for (int j = 1; j <= columns.Length; j++)
                    {
                        //設值為欄位名稱
                        ws.Cells[startRowNumber, j].Value = columns[j - 1];
                        //設定樣式
                        ws.Cells[startRowNumber, j].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center; //欄位置中
                        ws.Cells[startRowNumber, j].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                        ws.Cells[startRowNumber, j].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        ws.Cells[startRowNumber, j].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(212, 212, 212));
                        ws.Cells[startRowNumber, j].Style.Font.Bold = true;
                        ws.Cells[startRowNumber, j].Style.Font.Size = 12;
                    }

                    for (int i = 1; i <= dt.Rows.Count; i++)
                    {

                        for (int j = 1; j <= columns.Length; j++)
                        {
                            switch (j)
                            {
                                case 5:

                                    ws.Cells[i + 1, j].Value = Convert.ToDouble(dt.Rows[i - 1][j - 1]);

                                    ws.Cells[i + 1, j].Style.Numberformat.Format = "#,##0";
                                    ws.Cells[i + 1, j].Style.Font.Size = 12;
                                    ws.Cells[i + 1, j].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;//欄位置右
                                    ws.Cells[i + 1, j].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                                    break;

                                default:
                                    ws.Cells[i + 1, j].Value = dt.Rows[i - 1][j - 1].ToString();

                                    ws.Cells[i + 1, j].Style.Font.Size = 12;
                                    ws.Cells[i + 1, j].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center; //欄位置中
                                    ws.Cells[i + 1, j].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
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
        public ActionResult RPT_170802()
        {
            string startDate = "";
            string endDate = "";
            string merchant = "";

            string group = "";
            string retail = "";
            string bus = "";
            string bike = "";
            string track = "";
            string parking = "";
            string outsourcing = "";

            ViewBag.RepName = "報表確認系統";

            startDate = DateTime.Now.ToString("yyyyMMdd");
            endDate = DateTime.Now.ToString("yyyyMMdd");

            if (Request.Form["searchConfirm"] != null)
            {
                startDate = Request.Form["StartDate"];
                endDate = Request.Form["EndDate"];
                //merchant = Request.Form["merchant"];
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

            ViewBag.startDate = startDate;
            ViewBag.endDate = endDate;
            //ViewData["merchant"] = merchant;
            ViewBag.GROUP = SetGroup(group); ViewBag.groups = group;
            ViewBag.RETAIL = SetRetail(retail); ViewBag.retails = retail;
            ViewBag.BUS = SetBus(bus); ViewBag.buses = bus;
            ViewBag.BIKE = SetBike(bike); ViewBag.bikes = bike;
            ViewBag.TRACK = SetTrack(track); ViewBag.tracks = track;
            ViewBag.PARKING = SetParking(parking); ViewBag.parkings = parking;
            ViewBag.OUTSOURCING = SetOutsourcing(outsourcing); ViewBag.outsourcings = outsourcing;

            /*
            List<SelectListItem> merchantList = new List<SelectListItem>();

            List<GmMerchant> merchants = new GmMerchantManager().FindAll();
            IEnumerable<GmMerchant> distinctMerchants = from k in merchants
                                                        group k by new { k.MerchantNo, k.MerchantName } into g
                                                        select g.FirstOrDefault();
            SelectList list = new SelectList(distinctMerchants, "MerchantNo", "MerchantName");
            merchantList.AddRange(list);

            ViewBag.Merchants = merchantList;
            */


            //DataTable dt = RACTManager.ReportACT170801(settleDate, merchant);
            DataTable dt = new DataTable();
            List<string> mNoList = new List<string>();
            if (group == "ALL")
            {
                mNoList = new GmMerchantTypeDAO().FindAll();

                foreach (string mNo in mNoList)
                { dt.Merge(RACTManager.ReportACT170802(startDate, endDate, mNo)); }
            }
            else if (retail == "ALL" || bus == "ALL" || bike == "ALL" || track == "ALL" || parking == "ALL" || outsourcing == "ALL")
            {
                mNoList = new GmMerchantTypeDAO().FindGroupAll(group);
                foreach (string mNo in mNoList)
                { dt.Merge(RACTManager.ReportACT170802(startDate, endDate, mNo)); }
            }
            else if (merchant != "")
            {
                dt = RACTManager.ReportACT170802(startDate, endDate, merchant);
            }
            else { dt = new DataTable(); }

            if (Request.Form["searchConfirm"] != null)
                return View(dt);
            else return View(new DataTable());
        }
        public ActionResult RPT_170802_DOWNLOAD(string cptdate, string merchant, string filename)
        {

            return File("~/ftpdata/OUT/" + cptdate + "/" + merchant + "/" + filename, "application/unknown", Server.HtmlEncode(filename));
        }

        public ActionResult RPT_180301()
        {
            string settleDate = "";
            string sDate = "";
            string iccNo = "";
            string merchant = "";

            string group = "";
            string item = "";
            string retail = "";
            string bus = "";
            string bike = "";
            string track = "";
            string parking = "";
            string outsourcing = "";

            ViewBag.RepName = "帳差查詢表";



            if (Request.Form["searchConfirm"] != null || Request.Form["ExportExcel"] != null)
            {
                sDate = Request.Form["sDate"];
                iccNo = Request.Form["iccNo"];
                //merchant = Request.Form["merchant"];
                group = Request.Form["group"];
                item = Request.Form["items"];
                merchant = Request.Form["items"];
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
            }

            ViewBag.StartDate = sDate;
            ViewBag.IccNo = iccNo;
            ViewBag.MERCHANT = SetGroup(group);
            ViewBag.MERCHANT_NAME = group;
            ViewBag.ITEM = item;

            //ViewData["merchant"] = merchant;
            //ViewBag.GROUP = SetGroup(group); ViewBag.groups = group;
            //ViewBag.RETAIL = SetRetail(retail); ViewBag.retails = retail;
            //ViewBag.BUS = SetBus(bus); ViewBag.buses = bus;
            //ViewBag.BIKE = SetBike(bike); ViewBag.bikes = bike;
            //ViewBag.TRACK = SetTrack(track); ViewBag.tracks = track;
            //ViewBag.PARKING = SetParking(parking); ViewBag.parkings = parking;
            //ViewBag.OUTSOURCING = SetOutsourcing(outsourcing); ViewBag.outsourcings = outsourcing;

            /*
            List<SelectListItem> merchantList = new List<SelectListItem>();

            List<GmMerchant> merchants = new GmMerchantManager().FindAll();
            IEnumerable<GmMerchant> distinctMerchants = from k in merchants
                                                        group k by new { k.MerchantNo, k.MerchantName } into g
                                                        select g.FirstOrDefault();
            SelectList list = new SelectList(distinctMerchants, "MerchantNo", "MerchantName");
            merchantList.AddRange(list);

            ViewBag.Merchants = merchantList;
            */


            //DataTable dt = RACTManager.ReportACT161202(settleDate, merchant);
            DataTable dt = new DataTable();
            List<string> mNoList = new List<string>();
            if (group == "ALL")
            {
                mNoList = new GmMerchantTypeDAO().FindAll();

                foreach (string mNo in mNoList)
                { dt.Merge(RACTManager.ReportACT180301(iccNo, sDate, mNo)); }
            }
            else if (merchant != "" && merchant != "ALL")
            {
                dt = RACTManager.ReportACT180301(iccNo, sDate, merchant);
            }
            else if (item == "ALL")
            //else if (retail == "ALL" || bus == "ALL" || bike == "ALL" || track == "ALL" || parking == "ALL" || outsourcing == "ALL")
            {
                mNoList = new GmMerchantTypeDAO().FindGroupAll(group);
                foreach (string mNo in mNoList)
                { dt.Merge(RACTManager.ReportACT180301(iccNo, sDate, mNo)); }
            }
            else { dt = new DataTable(); }

            if (Request.Form["searchConfirm"] != null)
                return View(dt);
            else if (Request.Form["ExportExcel"] != null)
            {
                using (ExcelPackage p = new ExcelPackage())
                {
                    ExcelWorksheet ws = p.Workbook.Worksheets.Add(ViewBag.RepName.ToString());


                    ExcelRange r = ws.Cells["A1:L1"];
                    r.Merge = true;
                    r.Value = ViewBag.RepName.ToString();


                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    r.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                    //r.Style.Font.Bold = true;
                    r.Style.Font.Size = 14;

                    int startRowNumber = 2;
                    var columns = new[] { "特約機構", "清算日期", "門市代號", "門市名稱", "收銀機代號", "卡號", "卡機交易時間", "交易類型", "交易金額", "認列代碼", "認列原因", "對帳表格" };

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
                                case 9:

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
        public ActionResult RPT_180401()
        {
            string yearMonth = DateTime.Now.ToString("yyyyMM");

            string merchant = "";

            string group = "";
            string retail = "";
            string bus = "";
            string bike = "";
            string track = "";
            string parking = "";
            string outsourcing = "";

            ViewBag.RepName = "月別特約機構交易類別-明細表";



            if (Request.Form["searchConfirm"] != null || Request.Form["ExportExcel"] != null)
            {
                yearMonth = Request.Form["yearMonth"];
                // merchant = Request.Form["merchant"];
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

            ViewBag.yearMonth = yearMonth;
            //ViewData["merchant"] = merchant;
            ViewBag.GROUP = SetGroup(group); ViewBag.groups = group;
            ViewBag.RETAIL = SetRetail(retail); ViewBag.retails = retail;
            ViewBag.BUS = SetBus(bus); ViewBag.buses = bus;
            ViewBag.BIKE = SetBike(bike); ViewBag.bikes = bike;
            ViewBag.TRACK = SetTrack(track); ViewBag.tracks = track;
            ViewBag.PARKING = SetParking(parking); ViewBag.parkings = parking;
            ViewBag.OUTSOURCING = SetOutsourcing(outsourcing); ViewBag.outsourcings = outsourcing;

            /*
            List<SelectListItem> merchantList = new List<SelectListItem>();

            List<GmMerchant> merchants = new GmMerchantManager().FindAll();
            IEnumerable<GmMerchant> distinctMerchants = from k in merchants
                                                        group k by new { k.MerchantNo, k.MerchantName } into g
                                                        select g.FirstOrDefault();
            SelectList list = new SelectList(distinctMerchants, "MerchantNo", "MerchantName");
            merchantList.AddRange(list);

            ViewBag.Merchants = merchantList;
            */


            //DataTable dt = RACTManager.ReportACT180401(settleDate, merchant);
            DataTable dt = new DataTable();
            List<string> mNoList = new List<string>();
            if (group == "ALL")
            {
                mNoList = new GmMerchantTypeDAO().FindAll();

                foreach (string mNo in mNoList)
                { dt.Merge(RACTManager.ReportACT180401(yearMonth, mNo)); }
            }
            else if (retail == "ALL" || bus == "ALL" || bike == "ALL" || track == "ALL" || parking == "ALL" || outsourcing == "ALL")
            {
                mNoList = new GmMerchantTypeDAO().FindGroupAll(group);
                foreach (string mNo in mNoList)
                { dt.Merge(RACTManager.ReportACT180401(yearMonth, mNo)); }
            }
            else if (merchant != "")
            {
                dt = RACTManager.ReportACT180401(yearMonth, merchant);
            }
            else { dt = new DataTable(); }
            string sumAmt = string.Empty;
            if (dt.Rows.Count != 0)
            {
                sumAmt = dt.Compute("SUM(SUM_AMT)", "").ToString();
            }

            ViewBag.Total = sumAmt;
            if (Request.Form["searchConfirm"] != null)
                return View(dt);
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
                    var columns = new[] { "清分日", "特約機構名稱", "交易系統別", "交易類別", "金額" };

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
                                case 5:
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

                        }
                        if (i == dt.Rows.Count)
                        {

                            ws.Cells[i + 3, 1].Value = "合計";
                            ws.Cells[i + 3, 1].Style.Font.Size = 12;
                            ws.Cells[i + 3, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                            ws.Cells[i + 3, 1].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                            string i2 = (i + 3).ToString();
                            r = ws.Cells["A" + i2 + ":D" + i2];
                            r.Value = ws.Cells[i + 3, 1].Value;
                            r.Merge = true;
                            r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                            r.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                            ws.Cells[i + 3, 5].Value = sumAmt.ToString();
                            ws.Cells[i + 3, 5].Style.Numberformat.Format = "#,##0";
                            ws.Cells[i + 3, 5].Style.Font.Size = 12;
                            ws.Cells[i + 3, 5].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;//欄位置右
                            ws.Cells[i + 3, 5].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
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
        public ActionResult RPT_180402()
        {
            string yearMonth = DateTime.Now.ToString("yyyyMM");

            string merchant = "";

            string group = "";
            string retail = "";
            string bus = "";
            string bike = "";
            string track = "";
            string parking = "";
            string outsourcing = "";

            ViewBag.RepName = "月別特約機構交易類別-彙總表";



            if (Request.Form["searchConfirm"] != null || Request.Form["ExportExcel"] != null)
            {
                yearMonth = Request.Form["yearMonth"];
                // merchant = Request.Form["merchant"];
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

            ViewBag.yearMonth = yearMonth;
            //ViewData["merchant"] = merchant;
            ViewBag.GROUP = SetGroup(group); ViewBag.groups = group;
            ViewBag.RETAIL = SetRetail(retail); ViewBag.retails = retail;
            ViewBag.BUS = SetBus(bus); ViewBag.buses = bus;
            ViewBag.BIKE = SetBike(bike); ViewBag.bikes = bike;
            ViewBag.TRACK = SetTrack(track); ViewBag.tracks = track;
            ViewBag.PARKING = SetParking(parking); ViewBag.parkings = parking;
            ViewBag.OUTSOURCING = SetOutsourcing(outsourcing); ViewBag.outsourcings = outsourcing;

            /*
            List<SelectListItem> merchantList = new List<SelectListItem>();

            List<GmMerchant> merchants = new GmMerchantManager().FindAll();
            IEnumerable<GmMerchant> distinctMerchants = from k in merchants
                                                        group k by new { k.MerchantNo, k.MerchantName } into g
                                                        select g.FirstOrDefault();
            SelectList list = new SelectList(distinctMerchants, "MerchantNo", "MerchantName");
            merchantList.AddRange(list);

            ViewBag.Merchants = merchantList;
            */


            //DataTable dt = RACTManager.ReportACT180402(settleDate, merchant);
            DataTable dt = new DataTable();
            List<string> mNoList = new List<string>();
            if (group == "ALL")
            {
                mNoList = new GmMerchantTypeDAO().FindAll();

                foreach (string mNo in mNoList)
                { dt.Merge(RACTManager.ReportACT180402(yearMonth, mNo)); }
            }
            else if (retail == "ALL" || bus == "ALL" || bike == "ALL" || track == "ALL" || parking == "ALL" || outsourcing == "ALL")
            {
                mNoList = new GmMerchantTypeDAO().FindGroupAll(group);
                foreach (string mNo in mNoList)
                { dt.Merge(RACTManager.ReportACT180402(yearMonth, mNo)); }
            }
            else if (merchant != "")
            {
                dt = RACTManager.ReportACT180402(yearMonth, merchant);
            }
            else { dt = new DataTable(); }
            string sumAmt = string.Empty;
            if (dt.Rows.Count != 0)
            {
                sumAmt = dt.Compute("SUM(SUM_AMT)", "").ToString();
            }

            ViewBag.Total = sumAmt;
            if (Request.Form["searchConfirm"] != null)
                return View(dt);
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
                    var columns = new[] { "月別", "特約機構名稱", "交易系統別", "交易類別", "金額" };

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
                                case 5:
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

                        }
                        if (i == dt.Rows.Count)
                        {

                            ws.Cells[i + 3, 1].Value = "合計";
                            ws.Cells[i + 3, 1].Style.Font.Size = 12;
                            ws.Cells[i + 3, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                            ws.Cells[i + 3, 1].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                            string i2 = (i + 3).ToString();
                            r = ws.Cells["A" + i2 + ":D" + i2];
                            r.Value = ws.Cells[i + 3, 1].Value;
                            r.Merge = true;
                            r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                            r.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                            ws.Cells[i + 3, 5].Value = sumAmt.ToString();
                            ws.Cells[i + 3, 5].Style.Numberformat.Format = "#,##0";
                            ws.Cells[i + 3, 5].Style.Font.Size = 12;
                            ws.Cells[i + 3, 5].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;//欄位置右
                            ws.Cells[i + 3, 5].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
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
        public ActionResult RPT_180403()
        {
            string start = DateTime.Now.ToString("yyyyMMdd");
            string end = DateTime.Now.ToString("yyyyMMdd");


            DataTable dt = null;



            if (Request.Form["searchConfirm"] != null || Request.Form["ExportExcel"] != null)
            {
                start = Request.Form["startDate"];
                end = Request.Form["endDate"];


            }

            ViewBag.RepName = "無人店扣薪檔表";
            ViewBag.Start = start;
            ViewBag.End = end;




            dt = RACTManager.ReportACT180403(start, end);



            if (Request.Form["searchConfirm"] != null)
            { return View(dt); }
            else if (Request.Form["ExportExcel"] != null)
            {
                //MemoryStream ms = new MemoryStream();
                //引用StreamWriter類別
                MemoryStream ms = new MemoryStream();

                StreamWriter outStream = new StreamWriter(ms, System.Text.Encoding.GetEncoding(950));
                //寫資料至記憶體
                string temp = "";
                for (int int_Index = 0; int_Index < dt.Rows.Count; int_Index++)
                {
                    temp = String.Join(",", dt.Rows[int_Index].ItemArray);
                    outStream.WriteLine(temp);

                }
                outStream.Flush();
                outStream.Close();
                outStream.Dispose();
                ms.Close();
                ms.Dispose();


                return File(ms.ToArray(), "application/txt", DateTime.Now.ToString("yyyyMMdd") + @"無人店扣薪檔" + @".csv");

            }
            else return View(new DataTable());

        }
        /// <summary>
        /// 日別小商家
        /// </summary>
        /// <returns></returns>
        public ActionResult RPT_180701()
        {
            string start = DateTime.Now.ToString("yyyyMMdd");
            string end = DateTime.Now.ToString("yyyyMMdd");

            string merchant = "";

            string group = "";
            string retail = "";
            string bus = "";
            string bike = "";
            string track = "";
            string parking = "";
            string outsourcing = "";

            ViewBag.RepName = "日別-每日小商家交易表";



            if (Request.Form["searchConfirm"] != null || Request.Form["ExportExcel"] != null || Request.Form["ExportAll"] != null)
            {
                start = Request.Form["StartDate"];
                end = Request.Form["EndDate"];
                merchant = Request.Form["retail"];

                retail = Request.Form["retail"];



            }

            ViewBag.StartDate = start;
            ViewBag.EndDate = end;
            //ViewData["merchant"] = merchant;

            ViewBag.RETAIL = SetRetailXDD(retail); ViewBag.retails = retail;


            /*
            List<SelectListItem> merchantList = new List<SelectListItem>();

            List<GmMerchant> merchants = new GmMerchantManager().FindAll();
            IEnumerable<GmMerchant> distinctMerchants = from k in merchants
                                                        group k by new { k.MerchantNo, k.MerchantName } into g
                                                        select g.FirstOrDefault();
            SelectList list = new SelectList(distinctMerchants, "MerchantNo", "MerchantName");
            merchantList.AddRange(list);

            ViewBag.Merchants = merchantList;
            */


            //DataTable dt = RACTManager.ReportACT180402(settleDate, merchant);
            DataTable dt = new DataTable();
            List<string> mNoList = new List<string>();
            if (group == "ALL")
            {
                mNoList = new GmMerchantTypeDAO().FindAll();

                foreach (string mNo in mNoList)
                { dt.Merge(RACTManager.ReportACT180701(start, end, mNo)); }
            }
            else if (retail == "ALL" || bus == "ALL" || bike == "ALL" || track == "ALL" || parking == "ALL" || outsourcing == "ALL")
            {

                mNoList = new GmMerchantTypeDAO().FindMstore(group);
                foreach (string mNo in mNoList)
                { dt.Merge(RACTManager.ReportACT180701(start, end, mNo)); }
            }
            else if (merchant != "")
            {
                dt = RACTManager.ReportACT180701(start, end, merchant);
            }
            else { dt = new DataTable(); }
            //string sumAmt = string.Empty;
            //if (dt.Rows.Count != 0)
            //{
            //    sumAmt = dt.Compute("SUM(SUM_AMT)", "").ToString();
            //}

            //ViewBag.Total = sumAmt;
            if (Request.Form["searchConfirm"] != null)
                return View(dt);
            else if (Request.Form["ExportExcel"] != null || Request.Form["ExportAll"] != null)
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
                    r = ws.Cells["A2:G2"];
                    r.Merge = true;
                    r.Value = "清分日期：" + start + "~" + end;


                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                    r.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                    //r.Style.Font.Bold = true;
                    r.Style.Font.Size = 12;

                    int startRowNumber = 3;
                    //var columns = new[] { "月別", "特約機構名稱", "交易系統別", "交易類別", "金額" };




                    for (int j = 1; j <= dt.Columns.Count; j++)
                    {
                        if (j != 4 && j < 5)
                        {
                            //設值為欄位名稱
                            ws.Cells[startRowNumber, j].Value = dt.Columns[j - 1].ColumnName;
                            //設定樣式
                            ws.Cells[startRowNumber, j].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center; //欄位置中
                            ws.Cells[startRowNumber, j].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                            ws.Cells[startRowNumber, j].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                            ws.Cells[startRowNumber, j].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                            ws.Cells[startRowNumber, j].Style.Font.Bold = true;
                            ws.Cells[startRowNumber, j].Style.Font.Size = 12;

                        }
                        else if (j >= 5)
                        {
                            ws.Cells[startRowNumber, j - 1].Value = dt.Columns[j - 1].ColumnName;
                            //設定樣式
                            ws.Cells[startRowNumber, j - 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center; //欄位置中
                            ws.Cells[startRowNumber, j - 1].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                            ws.Cells[startRowNumber, j - 1].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                            ws.Cells[startRowNumber, j - 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                            ws.Cells[startRowNumber, j - 1].Style.Font.Bold = true;
                            ws.Cells[startRowNumber, j - 1].Style.Font.Size = 12;
                        }
                    }

                    #region 計算日期區間小計和總計
                    SortedSet<string> dateSet = new SortedSet<string>();// to store unique dates

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {

                        dateSet.Add(dt.Rows[i][0].ToString());
                    }

                    Dictionary<string, decimal[]> pTotal = new Dictionary<string, decimal[]>();//partial sum for individual dates 

                    decimal[] totalSum = new decimal[] { 0, 0, 0, 0 };

                    foreach (string date in dateSet)
                    {
                        pTotal[date] = new decimal[] { 0, 0, 0, 0 };
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            if (date.Equals(dt.Rows[i][0].ToString()))
                            {
                                for (int j = 4; j < dt.Columns.Count; j++)
                                {
                                    pTotal[date][j - 4] += Convert.ToDecimal(dt.Rows[i][j]);
                                }
                            }
                        }
                    }

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        for (int j = 4; j < dt.Columns.Count; j++)
                        {
                            totalSum[j - 4] += Convert.ToDecimal(dt.Rows[i][j]);
                        }
                    }
                    #endregion

                    if (Request.Form["ExportExcel"] != null)
                    {
                        int k = 3;
                        int l = 0;
                        foreach (string date in dateSet)
                        {

                            ws.Cells[k + 1, 1].Value = date;
                            ws.Cells[k + 1, 1].Style.Font.Size = 12;
                            ws.Cells[k + 1, 1].Style.Font.Bold = true;
                            ws.Cells[k + 1, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center; //欄位置中
                            ws.Cells[k + 1, 1].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                            ws.Cells[k + 1, 2].Value = "小計";
                            ws.Cells[k + 1, 2].Style.Font.Bold = true;
                            ws.Cells[k + 1, 2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center; //欄位置中
                            ws.Cells[k + 1, 2].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                            ws.Cells[k + 1, 3].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                            ws.Cells[k + 1, 4].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                            for (int j = 4; j < dt.Columns.Count; j++)
                            {
                                ws.Cells[k + 1, j].Value = Convert.ToDecimal(pTotal[date][j - 4]);
                                if (j == 5) { ws.Cells[k + 1, j].Value = ""; }
                                ws.Cells[k + 1, j].Style.Numberformat.Format = "#,##0";
                                ws.Cells[k + 1, j].Style.Font.Size = 12;
                                ws.Cells[k + 1, j].Style.Font.Bold = true;
                                ws.Cells[k + 1, j].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;//欄位置右
                                ws.Cells[k + 1, j].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                            }
                            k++;
                        }
                        ws.Cells[k + 1, 1].Value = "總計";
                        ws.Cells[k + 1, 1].Style.Font.Size = 12;
                        ws.Cells[k + 1, 1].Style.Font.Bold = true;
                        ws.Cells[k + 1, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center; //欄位置中
                        ws.Cells[k + 1, 1].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        ws.Cells[k + 1, 2].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        ws.Cells[k + 1, 3].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        ws.Cells[k + 1, 4].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        for (int j = 4; j < dt.Columns.Count; j++)
                        {

                            ws.Cells[k + 1, j].Value = Convert.ToDecimal(totalSum[j - 4]);
                            if (j == 5) { ws.Cells[k + 1, j].Value = ""; }
                            ws.Cells[k + 1, j].Style.Numberformat.Format = "#,##0";
                            ws.Cells[k + 1, j].Style.Font.Size = 12;
                            ws.Cells[k + 1, j].Style.Font.Bold = true;
                            ws.Cells[k + 1, j].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;//欄位置右
                            ws.Cells[k + 1, j].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        }
                    }
                    else if (Request.Form["ExportALL"] != null)
                    {
                        int k = 3;
                        int l = 0;
                        foreach (string date in dateSet)
                        {
                            for (int i = 1; i <= dt.Rows.Count; i++)
                            {
                                if (date.Equals(dt.Rows[i - 1][0].ToString()))
                                {
                                    for (int j = 1; j <= dt.Columns.Count; j++)
                                    {

                                        switch (j)
                                        {
                                            case 4:

                                                break;
                                            case 6:
                                                ws.Cells[k + 1, j - 1].Value = Convert.ToDouble(dt.Rows[i - 1][j - 1]) / 100;
                                                ws.Cells[k + 1, j - 1].Style.Numberformat.Format = "#,##0.00%"; ;
                                                ws.Cells[k + 1, j - 1].Style.Font.Size = 12;
                                                ws.Cells[k + 1, j - 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;//欄位置右
                                                ws.Cells[k + 1, j - 1].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                                                break;
                                            case 5:
                                            case 7:
                                            case 8:
                                                ws.Cells[k + 1, j - 1].Value = Convert.ToDouble(dt.Rows[i - 1][j - 1]);

                                                ws.Cells[k + 1, j - 1].Style.Numberformat.Format = "#,##0";
                                                ws.Cells[k + 1, j - 1].Style.Font.Size = 12;
                                                ws.Cells[k + 1, j - 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;//欄位置右
                                                ws.Cells[k + 1, j - 1].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                                                break;

                                            default:
                                                ws.Cells[k + 1, j].Value = dt.Rows[i - 1][j - 1].ToString();

                                                ws.Cells[k + 1, j].Style.Font.Size = 12;
                                                ws.Cells[k + 1, j].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center; //欄位置中
                                                ws.Cells[k + 1, j].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                                                break;
                                        }
                                    }

                                    k++;


                                }
                            }
                            //
                            ws.Cells[k + 1, 1].Value = date;
                            ws.Cells[k + 1, 1].Style.Font.Size = 12;
                            ws.Cells[k + 1, 1].Style.Font.Bold = true;
                            ws.Cells[k + 1, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center; //欄位置中
                            ws.Cells[k + 1, 1].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                            ws.Cells[k + 1, 2].Value = "小計";
                            ws.Cells[k + 1, 2].Style.Font.Bold = true;
                            ws.Cells[k + 1, 2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center; //欄位置中
                            ws.Cells[k + 1, 2].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                            ws.Cells[k + 1, 3].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                            ws.Cells[k + 1, 4].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                            for (int j = 4; j < dt.Columns.Count; j++)
                            {
                                ws.Cells[k + 1, j].Value = Convert.ToDecimal(pTotal[date][j - 4]);
                                if (j == 5) { ws.Cells[k + 1, j].Value = ""; }
                                ws.Cells[k + 1, j].Style.Numberformat.Format = "#,##0";
                                ws.Cells[k + 1, j].Style.Font.Size = 12;
                                ws.Cells[k + 1, j].Style.Font.Bold = true;
                                ws.Cells[k + 1, j].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;//欄位置右
                                ws.Cells[k + 1, j].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                            }
                            k++;

                            /*                    for (int i = 1; i <= dt.Rows.Count; i++)
                                                {

                                                    for (int j = 1; j <= dt.Columns.Count; j++)
                                                    {
                                                        switch (j)
                                                        {
                                                            case 4:
                                    
                                                                break;
                                                            case 6:
                                                                ws.Cells[i + 3, j - 1].Value = Convert.ToDouble(dt.Rows[i - 1][j - 1])/100;
                                                                ws.Cells[i + 3, j - 1].Style.Numberformat.Format = "#,##0.00%";;
                                                                ws.Cells[i + 3, j - 1].Style.Font.Size = 12;
                                                                ws.Cells[i + 3, j - 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;//欄位置右
                                                                ws.Cells[i + 3, j - 1].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                                                                break;
                                                            case 5:
                                                            case 7:
                                                            case 8:
                                                                ws.Cells[i + 3, j - 1].Value = Convert.ToDouble(dt.Rows[i - 1][j - 1]);

                                                                ws.Cells[i + 3, j - 1].Style.Numberformat.Format = "#,##0";
                                                                ws.Cells[i + 3, j - 1].Style.Font.Size = 12;
                                                                ws.Cells[i + 3, j - 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;//欄位置右
                                                                ws.Cells[i + 3, j - 1].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                                                                break;

                                                            default:
                                                                ws.Cells[i + 3, j].Value = dt.Rows[i - 1][j - 1].ToString();

                                                                ws.Cells[i + 3, j].Style.Font.Size = 12;
                                                                ws.Cells[i + 3, j].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center; //欄位置中
                                                                ws.Cells[i + 3, j].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                                                                break;
                                                        }

                                                    }
                                                        if (i == dt.Rows.Count)
                                                        {

                                                            ws.Cells[i + 4, 1].Value = "總計";
                                                            ws.Cells[i + 4, 1].Style.Font.Size = 12;
                                                            ws.Cells[i + 4, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                                                            ws.Cells[i + 4, 1].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                                                            ws.Cells[i + 4, 2].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                                                            ws.Cells[i + 4, 3].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                                                            ws.Cells[i + 4, 5].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                            

                                                            ws.Cells[i + 4, 4].Formula = string.Format("SUM(D{0}:D{1})",4,i+3);
                                                            ws.Cells[i + 4, 4].Style.Numberformat.Format = "#,##0";
                                                            ws.Cells[i + 4, 4].Style.Font.Size = 12;
                                                            ws.Cells[i + 4, 4].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;//欄位置右
                                                            ws.Cells[i + 4, 4].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                                                            ws.Cells[i + 4, 6].Formula = string.Format("SUM(F{0}:F{1})", 4, i + 3);
                                                            ws.Cells[i + 4, 6].Style.Numberformat.Format = "#,##0";
                                                            ws.Cells[i + 4, 6].Style.Font.Size = 12;
                                                            ws.Cells[i + 4, 6].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;//欄位置右
                                                            ws.Cells[i + 4, 6].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                                                            ws.Cells[i + 4, 7].Formula = string.Format("SUM(G{0}:G{1})", 4, i + 3);
                                                            ws.Cells[i + 4, 7].Style.Numberformat.Format = "#,##0";
                                                            ws.Cells[i + 4, 7].Style.Font.Size = 12;
                                                            ws.Cells[i + 4, 7].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;//欄位置右
                                                            ws.Cells[i + 4, 7].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                                                        } */
                        }
                        ws.Cells[k + 1, 1].Value = "總計";
                        ws.Cells[k + 1, 1].Style.Font.Size = 12;
                        ws.Cells[k + 1, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center; //欄位置中
                        ws.Cells[k + 1, 1].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        ws.Cells[k + 1, 2].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        ws.Cells[k + 1, 3].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        ws.Cells[k + 1, 4].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        for (int j = 4; j < dt.Columns.Count; j++)
                        {

                            ws.Cells[k + 1, j].Value = Convert.ToDecimal(totalSum[j - 4]);
                            if (j == 5) { ws.Cells[k + 1, j].Value = ""; }
                            ws.Cells[k + 1, j].Style.Numberformat.Format = "#,##0";
                            ws.Cells[k + 1, j].Style.Font.Size = 12;
                            ws.Cells[k + 1, j].Style.Font.Bold = true;
                            ws.Cells[k + 1, j].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;//欄位置右
                            ws.Cells[k + 1, j].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
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
        public ActionResult RPT_180702()
        {
            string start = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).ToString("yyyyMMdd");
            string end = new DateTime(DateTime.Now.AddMonths(1).Year, DateTime.Now.AddMonths(1).Month, 1).AddDays(-1).ToString("yyyyMMdd");
            string merchant = "";

            string group = "";
            string retail = "";
            string bus = "";
            string bike = "";
            string track = "";
            string parking = "";
            string outsourcing = "";

            ViewBag.RepName = "月別-小商家手續費彙總表";



            if (Request.Form["searchConfirm"] != null || Request.Form["ExportExcel"] != null)
            {
                start = Request.Form["StartDate"];
                end = Request.Form["EndDate"];
                merchant = Request.Form["retail"];

                retail = Request.Form["retail"];



            }

            ViewBag.StartDate = start;
            ViewBag.EndDate = end;
            //ViewData["merchant"] = merchant;

            DataTable dt = new DataTable();
            List<string> mNoList = new List<string>();

            if (Request.Form["searchConfirm"] != null || Request.Form["ExportExcel"] != null)
            {
                mNoList = new GmMerchantTypeDAO().FindMstore("RETAIL");

                dt.Merge(RACTManager.ReportACT180702(start, end));
            }
            else { dt = new DataTable(); }

            if (Request.Form["searchConfirm"] != null)
                return View(dt);
            else if (Request.Form["ExportExcel"] != null)
            {
                using (ExcelPackage p = new ExcelPackage())
                {
                    ExcelWorksheet ws = p.Workbook.Worksheets.Add(ViewBag.RepName.ToString());

                    //--序號	客戶代號(會計的)	特約機構	購貨金額	購貨手續費率	購貨手續費	未稅	稅金
                    ExcelRange r = ws.Cells["A1:H1"];
                    r.Merge = true;
                    r.Value = ViewBag.RepName.ToString();


                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    r.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                    //r.Style.Font.Bold = true;
                    r.Style.Font.Size = 14;
                    r = ws.Cells["A2:H2"];
                    r.Merge = true;
                    r.Value = "清分日期：" + start + "~" + end;


                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                    r.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                    //r.Style.Font.Bold = true;
                    r.Style.Font.Size = 12;

                    int startRowNumber = 3;
                    //var columns = new[] { "月別", "特約機構名稱", "交易系統別", "交易類別", "金額" };

                    for (int j = 1; j <= dt.Columns.Count; j++)
                    {

                        //設值為欄位名稱
                        ws.Cells[startRowNumber, j].Value = dt.Columns[j - 1].ColumnName;
                        //設定樣式
                        ws.Cells[startRowNumber, j].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center; //欄位置中
                        ws.Cells[startRowNumber, j].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                        ws.Cells[startRowNumber, j].Style.Font.Bold = true;
                        ws.Cells[startRowNumber, j].Style.Font.Size = 12;

                    }

                    for (int i = 1; i <= dt.Rows.Count; i++)
                    {

                        for (int j = 1; j <= dt.Columns.Count; j++)
                        {
                            switch (j)
                            {

                                case 5:
                                    ws.Cells[i + 3, j].Value = Convert.ToDouble(dt.Rows[i - 1][j - 1]) / 100;
                                    ws.Cells[i + 3, j].Style.Numberformat.Format = "#,##0.00%"; ;
                                    ws.Cells[i + 3, j].Style.Font.Size = 12;
                                    ws.Cells[i + 3, j].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;//欄位置右
                                    ws.Cells[i + 3, j].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                                    break;
                                case 4:
                                case 6:
                                case 7:
                                case 8:
                                    ws.Cells[i + 3, j].Value = Convert.ToDouble(dt.Rows[i - 1][j - 1]);
                                    ws.Cells[i + 3, j].Style.Numberformat.Format = "#,##0";
                                    ws.Cells[i + 3, j].Style.Font.Size = 12;
                                    ws.Cells[i + 3, j].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;//欄位置右
                                    ws.Cells[i + 3, j].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                                    break;

                                default:
                                    ws.Cells[i + 3, j].Value = dt.Rows[i - 1][j - 1].ToString();

                                    ws.Cells[i + 3, j].Style.Font.Size = 12;
                                    ws.Cells[i + 3, j].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center; //欄位置中
                                    ws.Cells[i + 3, j].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                                    break;
                            }

                        }
                        /*                         if (i == dt.Rows.Count)
                                                {

                                                    ws.Cells[i + 4, 1].Value = "總計";
                                                    ws.Cells[i + 4, 1].Style.Font.Size = 12;
                                                    ws.Cells[i + 4, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                                                    ws.Cells[i + 4, 1].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                                                    ws.Cells[i + 4, 2].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                                                    ws.Cells[i + 4, 3].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                                                    ws.Cells[i + 4, 5].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                            

                                                    ws.Cells[i + 4, 4].Formula = string.Format("SUM(D{0}:D{1})",4,i+3);
                                                    ws.Cells[i + 4, 4].Style.Numberformat.Format = "#,##0";
                                                    ws.Cells[i + 4, 4].Style.Font.Size = 12;
                                                    ws.Cells[i + 4, 4].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;//欄位置右
                                                    ws.Cells[i + 4, 4].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                                                    ws.Cells[i + 4, 6].Formula = string.Format("SUM(F{0}:F{1})", 4, i + 3);
                                                    ws.Cells[i + 4, 6].Style.Numberformat.Format = "#,##0";
                                                    ws.Cells[i + 4, 6].Style.Font.Size = 12;
                                                    ws.Cells[i + 4, 6].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;//欄位置右
                                                    ws.Cells[i + 4, 6].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                                                    ws.Cells[i + 4, 7].Formula = string.Format("SUM(G{0}:G{1})", 4, i + 3);
                                                    ws.Cells[i + 4, 7].Style.Numberformat.Format = "#,##0";
                                                    ws.Cells[i + 4, 7].Style.Font.Size = 12;
                                                    ws.Cells[i + 4, 7].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;//欄位置右
                                                    ws.Cells[i + 4, 7].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                                                } */
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
        public ActionResult RPT_181101()
        {
            string start = DateTime.Now.ToString("yyyyMMdd");
            string end = DateTime.Now.ToString("yyyyMMdd");

            string merchant = "";

            string group = "";
            string retail = "";
            string bus = "";
            string bike = "";
            string track = "";
            string parking = "";
            string outsourcing = "";
            string nccc = "";

            ViewBag.RepName = "日扣手續費撥付表";



            if (Request.Form["searchConfirm"] != null || Request.Form["ExportExcel"] != null)
            {
                start = Request.Form["StartDate"];
                end = Request.Form["EndDate"];
                group = Request.Form["group"];
                merchant = Request.Form["items"];
                nccc = Request.Form["items"];



            }

            ViewBag.StartDate = start;
            ViewBag.EndDate = end;
            //ViewData["merchant"] = merchant;
            ViewBag.GROUP = SetMutiGroup(group);
            ViewBag.groups = group;
            ViewBag.ITEM = nccc;


            /*
            List<SelectListItem> merchantList = new List<SelectListItem>();

            List<GmMerchant> merchants = new GmMerchantManager().FindAll();
            IEnumerable<GmMerchant> distinctMerchants = from k in merchants
                                                        group k by new { k.MerchantNo, k.MerchantName } into g
                                                        select g.FirstOrDefault();
            SelectList list = new SelectList(distinctMerchants, "MerchantNo", "MerchantName");
            merchantList.AddRange(list);

            ViewBag.Merchants = merchantList;
            */

            DataTable dt = new DataTable();
            DataTable dt2 = new DataTable();
            decimal[] dt2_total = new decimal[2];
            //DataTable dt = RACTManager.ReportACT180402(settleDate, merchant);
            if (Request.Form["searchConfirm"] != null || Request.Form["ExportExcel"] != null)
            {
                
                dt = RACTManager.ReportACT181101(start, end, merchant, group);
                //List<string> mNoList = new List<string>();
                //if (nccc == "ALL")
                //{

                //    mNoList = new GmMerchantTypeDAO().FindMutiMercMerchant(group);
                //    foreach (string mNo in mNoList)
                //    { dt.Merge(RACTManager.ReportACT181101(start, end, mNo, group)); }
                //}
                //else if (merchant != "")
                //{
                //    dt = RACTManager.ReportACT181101(start, end, merchant, group);
                //}
                //else { dt = new DataTable(); }
                if (dt.Columns.Count > 0)
                {
                    DataView dv = dt.DefaultView;
                    dv.Sort = "清分日 ,特店編號 ";
                    dt = dv.ToTable();
                    decimal[] total = new decimal[3];
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        for (int j = 0; j < dt.Columns.Count; j++)
                        {
                            switch (j)
                            {
                                case 3:
                                    total[0] = total[0] + Convert.ToDecimal(dt.Rows[i][j]);
                                    break;
                                case 5:
                                    total[1] = total[1] + Convert.ToDecimal(dt.Rows[i][j]);
                                    break;
                                case 6:
                                    total[2] = total[2] + Convert.ToDecimal(dt.Rows[i][j]);
                                    break;
                            }

                        }
                    }
                    ViewBag.AMT = total[0];
                    ViewBag.FEE_AMT = total[1];
                    ViewBag.NET_AMT = total[2];


                }
                dt2 = RACTManager.ReportACT181101_2(start, end, merchant, group);
                if (dt2.Columns.Count > 0)
                {
                    DataView dv = dt.DefaultView;
                    
                    
                    for (int i = 0; i < dt2.Rows.Count; i++)
                    {
                        for (int j = 0; j < dt2.Columns.Count; j++)
                        {
                            switch (j)
                            {
                                case 2:
                                    dt2_total[0] = dt2_total[0] + Convert.ToDecimal(dt2.Rows[i][j]);
                                    break;
                                case 3:
                                    dt2_total[1] = dt2_total[1] + Convert.ToDecimal(dt2.Rows[i][j]);
                                    break;
                            }

                        }
                    }


                }




            }
            if (Request.Form["searchConfirm"] != null)
                return View(dt);
            else if (Request.Form["ExportExcel"] != null)
            {
                using (ExcelPackage p = new ExcelPackage())
                {
                    ExcelWorksheet ws = p.Workbook.Worksheets.Add(ViewBag.RepName.ToString());


                    ExcelRange r = ws.Cells["A1:H1"];
                    r.Merge = true;
                    r.Value = ViewBag.RepName.ToString();


                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    r.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                    //r.Style.Font.Bold = true;
                    r.Style.Font.Size = 14;
                    r = ws.Cells["A2:H2"];
                    r.Merge = true;
                    r.Value = "清分日期：" + start + "~" + end;


                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                    r.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                    //r.Style.Font.Bold = true;
                    r.Style.Font.Size = 12;

                    int startRowNumber = 3;

                    int nextRow = 0;
                    var columns = new[] { "清分日", "特店名稱", "特店編號", "購貨淨額", "扣款手續費率", "扣款手續費", "撥付淨額", "撥付日期" };




                    for (int j = 1; j <= columns.Length; j++)
                    {

                        ws.Cells[startRowNumber, j].Value = columns[j - 1].ToString();
                        //設定樣式
                        ws.Cells[startRowNumber, j].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right; //欄位置中
                        ws.Cells[startRowNumber, j].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        ws.Cells[startRowNumber, j].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        ws.Cells[startRowNumber, j].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                        ws.Cells[startRowNumber, j].Style.Font.Bold = true;
                        ws.Cells[startRowNumber, j].Style.Font.Size = 12;

                    }
                    for (int i = 1; i <= dt.Rows.Count; i++)
                    {

                        for (int j = 1; j <= dt.Columns.Count; j++)
                        {
                            switch (j)
                            {

                                case 5:
                                    ws.Cells[i + 3, j].Value = Convert.ToDouble(dt.Rows[i - 1][j - 1]) / 100;
                                    ws.Cells[i + 3, j].Style.Numberformat.Format = "#,##0.00%"; ;
                                    ws.Cells[i + 3, j].Style.Font.Size = 12;
                                    ws.Cells[i + 3, j].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;//欄位置右
                                    ws.Cells[i + 3, j].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                                    break;
                                case 4:
                                case 6:
                                case 7:
                                    ws.Cells[i + 3, j].Value = Convert.ToDouble(dt.Rows[i - 1][j - 1]);
                                    ws.Cells[i + 3, j].Style.Numberformat.Format = "#,##0";
                                    ws.Cells[i + 3, j].Style.Font.Size = 12;
                                    ws.Cells[i + 3, j].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;//欄位置右
                                    ws.Cells[i + 3, j].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                                    break;

                                default:
                                    ws.Cells[i + 3, j].Value = dt.Rows[i - 1][j - 1].ToString();

                                    ws.Cells[i + 3, j].Style.Font.Size = 12;
                                    ws.Cells[i + 3, j].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right; //欄位置中
                                    ws.Cells[i + 3, j].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                                    break;
                            }
                            if (i == dt.Rows.Count)
                            {
                                ws.Cells[i + 4, 2].Value = "總計";
                                ws.Cells[i + 4, 2].Style.Font.Size = 12;
                                ws.Cells[i + 4, 2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right; //欄位置中
                                //設定框線
                                ws.Cells[i + 4, 1].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                                ws.Cells[i + 4, 2].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                                ws.Cells[i + 4, 3].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                                ws.Cells[i + 4, 4].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                                ws.Cells[i + 4, 5].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                                ws.Cells[i + 4, 6].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                                ws.Cells[i + 4, 7].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                                ws.Cells[i + 4, 8].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                                //總計值
                                ws.Cells[i + 4, 4].Formula = string.Format("SUM(D{0}:D{1})", 4, i + 3);
                                ws.Cells[i + 4, 6].Formula = string.Format("SUM(F{0}:F{1})", 4, i + 3);
                                ws.Cells[i + 4, 7].Formula = string.Format("SUM(G{0}:G{1})", 4, i + 3);
                                nextRow = i;
                            }
                        }

                    }

                    //另外依主檔編號加總
                    var columns2 = new[] { "統編", "入帳特店名稱", "加總 - 扣款手續費", "加總 - 撥付淨額" };

                    for (int j = 1; j <= columns2.Length; j++)
                    {

                        ws.Cells[startRowNumber + nextRow + 3, j].Value = columns2[j - 1].ToString();
                        //設定樣式
                        ws.Cells[startRowNumber + nextRow + 3, j].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right; //欄位置中
                        ws.Cells[startRowNumber + nextRow + 3, j].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        ws.Cells[startRowNumber + nextRow + 3, j].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        ws.Cells[startRowNumber + nextRow + 3, j].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                        ws.Cells[startRowNumber + nextRow + 3, j].Style.Font.Bold = true;
                        ws.Cells[startRowNumber + nextRow + 3, j].Style.Font.Size = 12;

                    }
                    for (int i = 1; i <= dt2.Rows.Count; i++)
                    {

                        for (int j = 1; j <= dt2.Columns.Count; j++)
                        {
                            switch (j)
                            {

                                case 3:
                                case 4:
                                    ws.Cells[i + startRowNumber + nextRow + 3, j].Value = Convert.ToDouble(dt2.Rows[i - 1][j - 1]);
                                    ws.Cells[i + startRowNumber + nextRow + 3, j].Style.Numberformat.Format = "#,##0";
                                    ws.Cells[i + startRowNumber + nextRow + 3, j].Style.Font.Size = 12;
                                    ws.Cells[i + startRowNumber + nextRow + 3, j].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;//欄位置右
                                    ws.Cells[i + startRowNumber + nextRow + 3, j].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                                    break;

                                default:
                                    ws.Cells[i + startRowNumber + nextRow + 3, j].Value = dt2.Rows[i - 1][j - 1].ToString();
                                    ws.Cells[i + startRowNumber + nextRow + 3, j].Style.Font.Size = 12;
                                    ws.Cells[i + startRowNumber + nextRow + 3, j].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right; //欄位置中
                                    ws.Cells[i + startRowNumber + nextRow + 3, j].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                                    break;
                            }
                            if (i == dt2.Rows.Count)
                            {
                                ws.Cells[i + startRowNumber + nextRow + 3 + 1, 1].Value = "總計";
                                ws.Cells[i + startRowNumber + nextRow + 3 + 1, 1].Style.Font.Size = 12;
                                ws.Cells[i + startRowNumber + nextRow + 3 + 1, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right; //欄位置中
                                //設定框線
                                ws.Cells[i + startRowNumber + nextRow + 3 + 1, 1].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                                ws.Cells[i + startRowNumber + nextRow + 3 + 1, 2].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                                ws.Cells[i + startRowNumber + nextRow + 3 + 1, 3].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                                ws.Cells[i + startRowNumber + nextRow + 3 + 1, 4].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                                
                                //總計值
                                ws.Cells[i + startRowNumber + nextRow + 3+1, 3].Value = dt2_total[0];
                                ws.Cells[i + startRowNumber + nextRow + 3+1, 4].Value = dt2_total[1];
                                //格式
                                ws.Cells[i + startRowNumber + nextRow + 3+1, 3].Style.Numberformat.Format = "#,##0";
                                ws.Cells[i + startRowNumber + nextRow + 3+1, 4].Style.Numberformat.Format = "#,##0";


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
        public ActionResult RPT_181201()
        {
            int year = DateTime.Today.Year;
            int month = DateTime.Today.Month;
            DateTime startDate = new DateTime(year, month, 1);
            DateTime endDate = new DateTime(year, month,
                                    DateTime.DaysInMonth(year, month));
            string start = startDate.ToString("yyyyMMdd");
            string end = endDate.ToString("yyyyMMdd");

            string merchant = "";

            string group = "";
            string retail = "";
            string bus = "";
            string bike = "";
            string track = "";
            string parking = "";
            string outsourcing = "";
            string nccc = "";

            ViewBag.RepName = "月結分潤報表";



            if (Request.Form["searchConfirm"] != null || Request.Form["ExportExcel"] != null)
            {
                start = Request.Form["StartDate"];
                end = Request.Form["EndDate"];

                group = Request.Form["group"];



            }

            ViewBag.StartDate = start;
            ViewBag.EndDate = end;
            ViewBag.GROUP = SetMutiGroup(group);
            ViewBag.groups = group;
            merchant = group;
            //ViewData["merchant"] = merchant;

            //ViewBag.NCCC = SetNCCC(retail); ViewBag.ncccs = nccc;


            /*
            List<SelectListItem> merchantList = new List<SelectListItem>();

            List<GmMerchant> merchants = new GmMerchantManager().FindAll();
            IEnumerable<GmMerchant> distinctMerchants = from k in merchants
                                                        group k by new { k.MerchantNo, k.MerchantName } into g
                                                        select g.FirstOrDefault();
            SelectList list = new SelectList(distinctMerchants, "MerchantNo", "MerchantName");
            merchantList.AddRange(list);

            ViewBag.Merchants = merchantList;
            */


            //DataTable dt = RACTManager.ReportACT180402(settleDate, merchant);
            DataTable dt = new DataTable();
            List<string> mNoList = new List<string>();
            dt = RACTManager.ReportACT181201(start, end,merchant);
            decimal[] total = new decimal[10];
            if (dt.Columns.Count > 0) 
            {
                
                
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    for (int j = 3; j < dt.Columns.Count; j++)
                    {
						
						
							total[j-3] = total[j-3] + Convert.ToDecimal(dt.Rows[i][j]);
						
                      /*   switch (j)
                        {
                            case 2:
                                total[0] = total[0] + Convert.ToDecimal(dt.Rows[i][j]);
                                break;
                            case 5:
                                total[1] = total[1] + Convert.ToDecimal(dt.Rows[i][j]);
                                break;
                            case 6:
                                total[2] = total[2] + Convert.ToDecimal(dt.Rows[i][j]);
                                break;
                        } */

                    }
                }
                ViewBag.TOTAL = total;


            }
            var columns = new[] { "清分日", "特店編號","特店名稱","筆數", "購貨淨額", "購貨分潤費率", "月結分潤總額", "加值淨額", "加值分潤費率", "月結分潤總額", "自動加值金額", "自動加值分潤費率", "月結分潤總額" };
            ViewBag.COL = columns;
            if (Request.Form["searchConfirm"] != null)
                return View(dt);
            else if (Request.Form["ExportExcel"] != null )
            {
                using (ExcelPackage p = new ExcelPackage())
                {
                    ExcelWorksheet ws = p.Workbook.Worksheets.Add(ViewBag.RepName.ToString());


                    ExcelRange r = ws.Cells["A1:M1"];
                    r.Merge = true;
                    r.Value = ViewBag.RepName.ToString();


                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    r.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                    //r.Style.Font.Bold = true;
                    r.Style.Font.Size = 14;
                    r = ws.Cells["A2:M2"];
                    r.Merge = true;
                    r.Value = "清分日期：" + start + "~" + end;


                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                    r.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                    //r.Style.Font.Bold = true;
                    r.Style.Font.Size = 12;

                    int startRowNumber = 3;
                    //var columns = new[] {"清分日","特店編號","購貨淨額","購貨分潤費率","月結分潤總額","加值淨額","加值分潤費率","月結分潤總額","自動加值金額","自動加值分潤費率","月結分潤總額"};
					//11欄
                    


                    for (int j = 1; j <= columns.Length; j++)
                    {

                        ws.Cells[startRowNumber, j].Value = columns[j - 1].ToString();
                        //設定樣式
                        ws.Cells[startRowNumber, j].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right; //欄位置中
                        ws.Cells[startRowNumber, j].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        ws.Cells[startRowNumber, j].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        ws.Cells[startRowNumber, j].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                        ws.Cells[startRowNumber, j].Style.Font.Bold = true;
                        ws.Cells[startRowNumber, j].Style.Font.Size = 12;

                    }
                    for (int i = 1; i <= dt.Rows.Count; i++)
                    {

                        for (int j = 1; j <= dt.Columns.Count; j++)
                        {
                            switch (j)
                            {

                                //case 4:
                                //case 7:
                                //case 10:
                                case 6:
                                case 9:
                                case 12:
                                    ws.Cells[i + 3, j].Value = Convert.ToDouble(dt.Rows[i - 1][j - 1]) / 100;
                                    ws.Cells[i + 3, j].Style.Numberformat.Format = "#,##0.00%"; ;
                                    ws.Cells[i + 3, j].Style.Font.Size = 12;
                                    ws.Cells[i + 3, j].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;//欄位置右
                                    ws.Cells[i + 3, j].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                                    break;
                                //case 3:
                                //case 5:
                                //case 6:
                                //case 8:
                                //case 9:
                                //case 11:
                                case 4:
                                case 5:
                                case 7:
                                case 8:
                                case 10:
                                case 11:
                                case 13:
                                    ws.Cells[i + 3, j].Value = Convert.ToDouble(dt.Rows[i - 1][j - 1]);
                                    ws.Cells[i + 3, j].Style.Numberformat.Format = "#,##0";
                                    ws.Cells[i + 3, j].Style.Font.Size = 12;
                                    ws.Cells[i + 3, j].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;//欄位置右
                                    ws.Cells[i + 3, j].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                                    break;
                                default:
                                    ws.Cells[i + 3, j].Value = dt.Rows[i - 1][j - 1].ToString();

                                    ws.Cells[i + 3, j].Style.Font.Size = 12;
                                    ws.Cells[i + 3, j].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right; //欄位置中
                                    ws.Cells[i + 3, j].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                                    break;
                            }
                            if (i == dt.Rows.Count)
                            {
                                ws.Cells[i + 4, 2].Value = "總計";
                                ws.Cells[i + 4, 2].Style.Font.Size = 12;
                                ws.Cells[i + 4, 2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right; //欄位置中
                                //設定框線
                                ws.Cells[i + 4, 1].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                                ws.Cells[i + 4, 2].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                                ws.Cells[i + 4, 3].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                                ws.Cells[i + 4, 4].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                                ws.Cells[i + 4, 5].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                                ws.Cells[i + 4, 6].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                                ws.Cells[i + 4, 7].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                                ws.Cells[i + 4, 8].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                                ws.Cells[i + 4, 9].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                                ws.Cells[i + 4, 10].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                                ws.Cells[i + 4, 11].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                                ws.Cells[i + 4, 12].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                                ws.Cells[i + 4, 13].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                                //總計值
                                ws.Cells[i + 4, 4].Formula = string.Format("SUM(D{0}:D{1})", 4, i + 3);
                                ws.Cells[i + 4, 5].Formula = string.Format("SUM(E{0}:E{1})", 4, i + 3);
                                ws.Cells[i + 4, 7].Formula = string.Format("SUM(G{0}:G{1})", 4, i + 3);
                                ws.Cells[i + 4, 8].Formula = string.Format("SUM(H{0}:H{1})", 4, i + 3);
								ws.Cells[i + 4, 10].Formula = string.Format("SUM(J{0}:J{1})", 4, i + 3);
								ws.Cells[i + 4, 11].Formula = string.Format("SUM(K{0}:K{1})", 4, i + 3);
								ws.Cells[i + 4, 13].Formula = string.Format("SUM(M{0}:M{1})", 4, i + 3);

                                ws.Cells[i + 4, 4].Style.Numberformat.Format = "#,##0"; 
                                ws.Cells[i + 4, 5].Style.Numberformat.Format = "#,##0"; 
                                ws.Cells[i + 4, 7].Style.Numberformat.Format = "#,##0";
                                ws.Cells[i + 4, 8].Style.Numberformat.Format = "#,##0";
                                ws.Cells[i + 4, 10].Style.Numberformat.Format = "#,##0";
                                ws.Cells[i + 4, 11].Style.Numberformat.Format = "#,##0";
                                ws.Cells[i + 4, 13].Style.Numberformat.Format = "#,##0";

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
        public ActionResult RPT_181202()
        {
            string yearMonth = "";
            string ksh = "";
            //string merchant = "";

            ViewBag.RepName = "月結-高雄地區公車零值明細表";

            yearMonth = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString().PadLeft(2, '0');

            if (Request.Form["searchConfirm"] != null || Request.Form["ExportExcel"] != null || Request.Form["ExportExcelDetail"] != null)
            {
                yearMonth = Request.Form["yearMonth"];
                ksh = Request.Form["ksh"];
                //merchant = Request.Form["merchant"];
            }

            ViewBag.YearMonth = yearMonth;
            ViewBag.KSH = SetKSH(ksh); ViewBag.kshs = ksh;
            //ViewData["merchant"] = merchant;

            /*
            List<SelectListItem> merchantList = new List<SelectListItem>();

            List<GmMerchant> merchants = new GmMerchantManager().FindAll();
            IEnumerable<GmMerchant> distinctMerchants = from k in merchants
                                                        group k by new { k.MerchantNo, k.MerchantName } into g
                                                        select g.FirstOrDefault();
            SelectList list = new SelectList(distinctMerchants, "MerchantNo", "MerchantName");
            merchantList.AddRange(list);

            ViewBag.MerchantList = merchantList;
            */

            string firstDate = yearMonth + "01";

            string lastDate = DateTime.ParseExact(yearMonth + "01", "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture).AddMonths(1).AddDays(-1).ToString("yyyyMMdd");

            DataTable dt = new DataTable();

            //按下查詢才跑SQL
            if (Request.Form["searchConfirm"] != null || Request.Form["ExportExcel"] != null)
            {
                if (ksh == "ALL")
                {
                    firstDate = DateTime.ParseExact(yearMonth + "01", "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture).AddMonths(1).ToString("yyyyMMdd");
                    List<string> mNoList = new GmMerchantTypeDAO().FindKSH();
                    foreach (string mNo in mNoList)
                    { dt.Merge(RACTManager.ReportACT181202(firstDate, mNo)); }
                }
                else if (ksh != "")
                {
                    firstDate = DateTime.ParseExact(yearMonth + "01", "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture).AddMonths(1).ToString("yyyyMMdd");
                    dt.Merge(RACTManager.ReportACT181202(firstDate, ksh));
                }
            }
            if (Request.Form["ExportExcelDetail"] != null)
            {
                if (ksh == "ALL")
                {
                    firstDate = DateTime.ParseExact(yearMonth + "01", "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture).AddMonths(1).ToString("yyyyMMdd");
                    List<string> mNoList = new GmMerchantTypeDAO().FindKSH();
                    foreach (string mNo in mNoList)
                    { dt.Merge(RACTManager.ReportACT181202D(firstDate, mNo)); }
                }
                else if (ksh != "")
                {
                    firstDate = DateTime.ParseExact(yearMonth + "01", "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture).AddMonths(1).ToString("yyyyMMdd");
                    dt.Merge(RACTManager.ReportACT181202D(firstDate, ksh));
                }

            }
            if (Request.Form["searchConfirm"] != null)
                return View(dt);
            else if (Request.Form["ExportExcel"] != null)
            {
                using (ExcelPackage p = new ExcelPackage())
                {
                    ExcelWorksheet ws = p.Workbook.Worksheets.Add(ViewBag.RepName.ToString());


                    ExcelRange r = ws.Cells["A1:H1"];
                    r.Merge = true;
                    r.Value = ViewBag.RepName.ToString();


                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    r.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                    //r.Style.Font.Bold = true;
                    r.Style.Font.Size = 14;

                    r = ws.Cells["A2:H2"];
                    r.Merge = true;
                    r.Value = "清分年月：" + yearMonth;
                    r.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                    int startRowNumber = 3;
                    var columns = new[] { "特店編號", "特店名稱", "統編", "清分日", "交易日", "計算類別", "零值交易筆數小計", "手續費率" };

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

                                    ws.Cells[i + startRowNumber, j].Value = Convert.ToDouble(dt.Rows[i - 1][j - 1]);


                                    ws.Cells[i + startRowNumber, j].Style.Numberformat.Format = "#,##0";
                                    ws.Cells[i + startRowNumber, j].Style.Font.Size = 12;
                                    ws.Cells[i + startRowNumber, j].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;//欄位置右
                                    ws.Cells[i + startRowNumber, j].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                                    break;
                                case 8:
                                
                                    ws.Cells[i + startRowNumber, j].Value = Convert.ToDouble(dt.Rows[i - 1][j - 1]);


                                    ws.Cells[i + startRowNumber, j].Style.Numberformat.Format = "0.#";
                                    ws.Cells[i + startRowNumber, j].Style.Font.Size = 12;
                                    ws.Cells[i + startRowNumber, j].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;//欄位置右
                                    ws.Cells[i + startRowNumber, j].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                                    break;

                                default:
                                    ws.Cells[i + startRowNumber, j].Value = dt.Rows[i - 1][j - 1].ToString();

                                    ws.Cells[i + startRowNumber, j].Style.Font.Size = 12;
                                    ws.Cells[i + startRowNumber, j].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center; //欄位置中
                                    ws.Cells[i + startRowNumber, j].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
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
            else if (Request.Form["ExportExcelDetail"] != null)
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

                    r = ws.Cells["A2:G2"];
                    r.Merge = true;
                    r.Value = "清分年月：" + yearMonth;
                    r.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                    int startRowNumber = 3;
                    var columns = new[] { "特店名稱", "特店編號", "0值交易筆數", "手續費/筆/元", "消費手續費", "未稅", "稅金" };

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
                                case 3:
                                case 5:
                                case 6:
                                case 7:

                                    ws.Cells[i + startRowNumber, j].Value = Convert.ToDouble(dt.Rows[i - 1][j - 1]);
                                    ws.Cells[i + startRowNumber, j].Style.Numberformat.Format = "#,##0";
                                    ws.Cells[i + startRowNumber, j].Style.Font.Size = 12;
                                    ws.Cells[i + startRowNumber, j].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;//欄位置右
                                    ws.Cells[i + startRowNumber, j].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                                    break;
                                case 4:
                                    ws.Cells[i + startRowNumber, j].Value = Convert.ToDouble(dt.Rows[i - 1][j - 1]);
                                    ws.Cells[i + startRowNumber, j].Style.Numberformat.Format = "0.#";
                                    ws.Cells[i + startRowNumber, j].Style.Font.Size = 12;
                                    ws.Cells[i + startRowNumber, j].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;//欄位置右
                                    ws.Cells[i + startRowNumber, j].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                                    break;
                                default:
                                    ws.Cells[i + startRowNumber, j].Value = dt.Rows[i - 1][j - 1].ToString();

                                    ws.Cells[i + startRowNumber, j].Style.Font.Size = 12;
                                    ws.Cells[i + startRowNumber, j].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center; //欄位置中
                                    ws.Cells[i + startRowNumber, j].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                                    break;
                            }

                        }
                        if (i == dt.Rows.Count)
                        {
                            ws.Cells[i + 4, 1].Value = "合計";
                            ws.Cells[i + 4, 1].Style.Font.Size = 12;
                            ws.Cells[i + 4, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right; //欄位置中
                            //設定框線
                            ws.Cells[i + 4, 1].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                            ws.Cells[i + 4, 2].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                            ws.Cells[i + 4, 3].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                            ws.Cells[i + 4, 4].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                            ws.Cells[i + 4, 5].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                            ws.Cells[i + 4, 6].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                            ws.Cells[i + 4, 7].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                            
                            //總計值
                            ws.Cells[i + 4, 3].Formula = string.Format("SUM(C{0}:C{1})", 4, i + 3);
                            ws.Cells[i + 4, 5].Formula = string.Format("SUM(E{0}:E{1})", 4, i + 3);
                            ws.Cells[i + 4, 6].Formula = string.Format("SUM(F{0}:F{1})", 4, i + 3);
                            ws.Cells[i + 4, 7].Formula = string.Format("SUM(G{0}:G{1})", 4, i + 3);

                            ws.Cells[i + 4, 3].Style.Numberformat.Format = "#,##0";
                            ws.Cells[i + 4, 5].Style.Numberformat.Format = "#,##0";
                            ws.Cells[i + 4, 6].Style.Numberformat.Format = "#,##0";
                            ws.Cells[i + 4, 7].Style.Numberformat.Format = "#,##0";

                            ws.Cells[i + 4, 3].Style.Font.Size = 12;
                            ws.Cells[i + 4, 5].Style.Font.Size = 12;
                            ws.Cells[i + 4, 6].Style.Font.Size = 12;
                            ws.Cells[i + 4, 7].Style.Font.Size = 12;

                        }
                    }

                    ws.Cells[ws.Dimension.Address].AutoFitColumns();

                    var stream = new MemoryStream();
                    p.SaveAs(stream);
                    stream.Close();

                    return File(stream.ToArray(), "application/vnd.ms-excel", ViewBag.RepName+"(統計)" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx");
                }

            }
            else return View(new DataTable());
        }
        public ActionResult RPT_190101()
        {
            string yearMonth = "";
            string ksh = "";
            //string merchant = "";

            ViewBag.RepName = "高雄交通零值手續費匯總表";

            yearMonth = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString().PadLeft(2, '0');

            if (Request.Form["searchConfirm"] != null || Request.Form["ExportExcel"] != null)
            {
                yearMonth = Request.Form["yearMonth"];
                //ksh = Request.Form["ksh"];
                //merchant = Request.Form["merchant"];
            }

            ViewBag.YearMonth = yearMonth;
            //ViewBag.KSH = SetKSH(ksh); ViewBag.kshs = ksh;
            //ViewData["merchant"] = merchant;

            /*
            List<SelectListItem> merchantList = new List<SelectListItem>();

            List<GmMerchant> merchants = new GmMerchantManager().FindAll();
            IEnumerable<GmMerchant> distinctMerchants = from k in merchants
                                                        group k by new { k.MerchantNo, k.MerchantName } into g
                                                        select g.FirstOrDefault();
            SelectList list = new SelectList(distinctMerchants, "MerchantNo", "MerchantName");
            merchantList.AddRange(list);

            ViewBag.MerchantList = merchantList;
            */

            string firstDate = yearMonth + "01";

            string lastDate = DateTime.ParseExact(yearMonth + "01", "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture).AddMonths(1).AddDays(-1).ToString("yyyyMMdd");

            DataTable dt = new DataTable();
            var COL = new[] { "序號", "特約機構", "統編", "購貨淨額", "重複","0值交易筆數", "購貨手續費率", "消費手續費", "未稅","稅金","加值手續費率","加值手續費",
						"未稅","稅金","自動加值額","自動加值手續費率","自動加值手續費","未稅","稅金"};
            ViewBag.COL = COL;

            //按下查詢才跑SQL
            if (Request.Form["searchConfirm"] != null || Request.Form["ExportExcel"] != null)
            {

                firstDate = DateTime.ParseExact(yearMonth + "01", "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture).AddMonths(1).ToString("yyyyMMdd");
                List<string> mNoList = new GmMerchantTypeDAO().FindKSH();
                foreach (string mNo in mNoList)
                { dt.Merge(RACTManager.ReportACT190101(firstDate, mNo)); }

                //加序號
                for (int i = 1; i <= dt.Rows.Count; i++)
                {
                    dt.Rows[i-1][0] = i;
                }
                /*
                int los = 1;
               
                foreach (string mNo in mNoList)
                {
                    for (int i = 1; i <= dt.Rows.Count; i++)
                    {
                        if (dt.Rows[i-1][1].Equals(mNo))
                        {
                            dt.Rows[i - 1][0] = los;
                        }
                    }
                    los++;
                }
                */
                
                
                /*int j = 1;
                foreach (string mNo in mNoList)
                {
                    
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (dt.Rows[i][1].Equals(mNo))
                        {
                            dt.Rows[i][0] = j;
                        }
                    }
                    j++;
                }*/
                

            }

            if (Request.Form["searchConfirm"] != null)
                return View(dt);
            else if (Request.Form["ExportExcel"] != null)
            {
                using (ExcelPackage p = new ExcelPackage())
                {
                    ExcelWorksheet ws = p.Workbook.Worksheets.Add(ViewBag.RepName.ToString());

                    
                    ExcelRange r = ws.Cells["A1:S1"];
                    r.Merge = true;
                    r.Value = ViewBag.RepName.ToString();


                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    r.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                    //r.Style.Font.Bold = true;
                    r.Style.Font.Size = 14;

                    r = ws.Cells["A2:S2"];
                    r.Merge = true;
                    r.Value = "清分年月：" + yearMonth;
                    r.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                    int startRowNumber = 3;
                    var columns = new[] { "序號", "特約機構", "統編", "購貨淨額", "重複","0值交易筆數", "購貨手續費率", "消費手續費", "未稅","稅金","加值手續費率","加值手續費",
						"未稅","稅金","自動加值額","自動加值手續費率","自動加值手續費","未稅","稅金"};

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

                    dt.Columns.Remove("MERCHANT_NO");
                    
                    for (int i = 1; i <= dt.Rows.Count; i++)
                    {
                        for (int j = 1; j <= columns.Length; j++)
                        {
                           
                            if(dt.Rows[i - 1][1].Equals("小計"))
                            {
                                switch (j)
                                {
                                    
                                    case 4:
                                    case 6:
                                    case 8:
                                    case 9:
                                    case 10:
                                        ws.Cells[i + startRowNumber, j].Value = Convert.ToDouble(dt.Rows[i - 1][j - 1]);
                                        ws.Cells[i + startRowNumber, j].Style.Numberformat.Format = "#,##0";
                                        ws.Cells[i + startRowNumber, j].Style.Font.Size = 12;
                                        ws.Cells[i + startRowNumber, j].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;//欄位置右
                                        ws.Cells[i + startRowNumber, j].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                                        ws.Cells[i + startRowNumber, j].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                                        break;
                                    case 7:
                                        ws.Cells[i + startRowNumber, j].Value = "";                                   
                                        ws.Cells[i + startRowNumber, j].Style.Font.Size = 12;
                                        ws.Cells[i + startRowNumber, j].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;//欄位置右
                                        ws.Cells[i + startRowNumber, j].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                                        ws.Cells[i + startRowNumber, j].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                                        break;

                                    default:
                                        ws.Cells[i + startRowNumber, j].Value = dt.Rows[i - 1][j - 1].ToString();
                                        ws.Cells[i + startRowNumber, j].Style.Font.Size = 12;
                                        ws.Cells[i + startRowNumber, j].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center; //欄位置中
                                        ws.Cells[i + startRowNumber, j].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                                        ws.Cells[i + startRowNumber, j].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                                        break;
                                }
                            }
                            else {
                                switch (j)
                                {
                                   
                                    case 4:
                                    case 6:
                                    case 8:
                                    case 9:
                                    case 10:
                                        ws.Cells[i + startRowNumber, j].Value = Convert.ToDouble(dt.Rows[i - 1][j - 1]);
                                        ws.Cells[i + startRowNumber, j].Style.Numberformat.Format = "#,##0";
                                        ws.Cells[i + startRowNumber, j].Style.Font.Size = 12;
                                        ws.Cells[i + startRowNumber, j].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;//欄位置右
                                        ws.Cells[i + startRowNumber, j].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                                        break;
                                    case 7:
                                            ws.Cells[i + startRowNumber, j].Value = Convert.ToDouble(dt.Rows[i - 1][j - 1]) ;
                                            ws.Cells[i + startRowNumber, j].Style.Numberformat.Format = "#,##0.00";
                                            ws.Cells[i + startRowNumber, j].Style.Font.Size = 12;
                                            ws.Cells[i + startRowNumber, j].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;//欄位置右
                                            ws.Cells[i + startRowNumber, j].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                                        break;

                                    default:
                                        ws.Cells[i + startRowNumber, j].Value = dt.Rows[i - 1][j - 1].ToString();
                                        ws.Cells[i + startRowNumber, j].Style.Font.Size = 12;
                                        ws.Cells[i + startRowNumber, j].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center; //欄位置中
                                        ws.Cells[i + startRowNumber, j].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                                        break;
                                }
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
        public ActionResult RPT_190301()
        {
            string yearMonth = "";
            string group = "";
            string item = "";
            string merchant = "";

            ViewBag.RepName = " 愛金卡月別特約機構(門市)手續費";

            yearMonth = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString().PadLeft(2, '0');

            if (Request.Form["searchConfirm"] != null || Request.Form["ExportExcel"] != null)
            {
                yearMonth = Request.Form["yearMonth"];
                group = Request.Form["group"];
                item = Request.Form["items"];
                merchant = Request.Form["items"];
            }

            ViewBag.YearMonth = yearMonth;
            ViewBag.MERCHANT = SetRetailParking(group);
            ViewBag.MERCHANT_NAME = group;
            ViewBag.ITEM = item;

            /*
            List<SelectListItem> merchantList = new List<SelectListItem>();

            List<GmMerchant> merchants = new GmMerchantManager().FindAll();
            IEnumerable<GmMerchant> distinctMerchants = from k in merchants
                                                        group k by new { k.MerchantNo, k.MerchantName } into g
                                                        select g.FirstOrDefault();
            SelectList list = new SelectList(distinctMerchants, "MerchantNo", "MerchantName");
            merchantList.AddRange(list);

            ViewBag.MerchantList = merchantList;
            */

            string firstDate = yearMonth + "01";

            string lastDate = DateTime.ParseExact(yearMonth + "01", "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture).AddMonths(1).AddDays(-1).ToString("yyyyMMdd");
            decimal[] total = new decimal[23];
            DataTable dt = new DataTable();
			//報表頁面和產出檔欄位不同，頁面沒有重複 調帳，產出檔案沒有序號
            var COL = new[] { "序號", "特約機構", "門市名稱", "購貨淨額","購貨手續費率", "消費手續費", "未稅","稅金",
						"加值淨額","加值手續費率","加值手續費","未稅","稅金",
						"自動加值額","自動加值手續費率","自動加值手續費","未稅","稅金"};
            ViewBag.COL = COL;

            //按下查詢才跑SQL
            if (Request.Form["searchConfirm"] != null || Request.Form["ExportExcel"] != null)
            {

                firstDate = DateTime.ParseExact(yearMonth + "01", "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture).AddMonths(1).ToString("yyyyMMdd");
                //List<string> mNoList = new GmMerchantTypeDAO().FindKSH();
                //foreach (string mNo in mNoList)
                //{ dt.Merge(RACTManager.ReportACT190101(firstDate, mNo)); }
				//dt.Merge(RACTManager.ReportACT190301(firstDate, park));
                //DataTable dt = new DataTable();
                List<string> mNoList = new List<string>();
                if (group == "ALL")
                {
                    mNoList = new GmMerchantTypeDAO().FindAll();

                    foreach (string mNo in mNoList)
                    { dt.Merge(RACTManager.ReportACT190301(firstDate, mNo)); }
                }
                else if (merchant != "" && merchant != "ALL")
                {
                    dt = RACTManager.ReportACT190301(firstDate, merchant);
                }
                else if (item == "ALL")
                //else if (retail == "ALL" || bus == "ALL" || bike == "ALL" || track == "ALL" || parking == "ALL" || outsourcing == "ALL")
                {
                    mNoList = new GmMerchantTypeDAO().FindGroupAll(group);
                    foreach (string mNo in mNoList)
                    { dt.Merge(RACTManager.ReportACT190301(firstDate, mNo)); }
                }
                else { dt = new DataTable(); }
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    for (int j = 0; j < dt.Columns.Count;j++ )
                        switch (j)
                        {
                            case 2:
                            case 6:
                            case 7:
                            case 8:
                            case 9:
                            case 13:
                            case 14:
                            case 15:
                            case 16:
                            case 20:
                            case 21:
                            case 22:

                                total[j] = total[j] + Convert.ToDecimal(dt.Rows[i][j]);
                                break;
                        }
                }
                ViewBag.TOTAL = total;
                            

            }

            if (Request.Form["searchConfirm"] != null)
            {
                //加序號
                dt.Columns.Add("ID", typeof(int));
                dt.Columns["ID"].SetOrdinal(0);
                for (int i = 1; i <= dt.Rows.Count; i++)
                {
                    dt.Rows[i - 1][0] = i;
                }
                return View(dt);
            }
            else if (Request.Form["ExportExcel"] != null)
            {
                using (ExcelPackage p = new ExcelPackage())
                {
                    ExcelWorksheet ws = p.Workbook.Worksheets.Add(ViewBag.RepName.ToString());


                    ExcelRange r = ws.Cells["A1:W1"];
                    r.Merge = true;
                    r.Value = ViewBag.RepName.ToString();


                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    r.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                    //r.Style.Font.Bold = true;
                    r.Style.Font.Size = 14;

                    r = ws.Cells["A2:W2"];
                    r.Merge = true;
                    r.Value = "清分年月：" + yearMonth;
                    r.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                    int startRowNumber = 3;
                    var columns = new[] { "特約機構", "門市名稱", "購貨淨額", "重複","調帳", "購貨手續費率", "消費手續費", "未稅","稅金",
						"加值淨額","重複","調帳","加值手續費率","加值手續費","未稅","稅金",
						"自動加值額","重複","調帳","自動加值手續費率","自動加值手續費","未稅","稅金"};

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

                                    case 3:
                                    case 7:
                                    case 8:
                                    case 9:
                                    case 10:
                                    case 14:
                                    case 15:
                                    case 16:
                                    case 17:
                                    case 21:
                                    case 22:
                                    case 23:
                                        ws.Cells[i + startRowNumber, j].Value = Convert.ToDouble(dt.Rows[i - 1][j - 1]);
                                        ws.Cells[i + startRowNumber, j].Style.Numberformat.Format = "#,##0";
                                        ws.Cells[i + startRowNumber, j].Style.Font.Size = 12;
                                        ws.Cells[i + startRowNumber, j].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;//欄位置右
                                        ws.Cells[i + startRowNumber, j].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                                        break;
                                    case 6:
                                    case 13:
                                    case 20:
                                        ws.Cells[i + startRowNumber, j].Value = Convert.ToDouble(dt.Rows[i - 1][j - 1]);
                                        ws.Cells[i + startRowNumber, j].Style.Numberformat.Format = "#,##0.00";
                                        ws.Cells[i + startRowNumber, j].Style.Font.Size = 12;
                                        ws.Cells[i + startRowNumber, j].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;//欄位置右
                                        ws.Cells[i + startRowNumber, j].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                                        break;

                                    default:
                                        ws.Cells[i + startRowNumber, j].Value = dt.Rows[i - 1][j - 1].ToString();
                                        ws.Cells[i + startRowNumber, j].Style.Font.Size = 12;
                                        ws.Cells[i + startRowNumber, j].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center; //欄位置中
                                        ws.Cells[i + startRowNumber, j].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                                        break;
                                
                            }

                        }
                        if (i == dt.Rows.Count)
                        {
                            for (int k = 1; k <= total.Length; k++)
                            {
                                switch (k)
                                {
                                    case 2:
                                        ws.Cells[i + startRowNumber + 1, k].Value = "總計";
                                        ws.Cells[i + startRowNumber + 1 ,k].Style.Font.Size = 12;
                                        ws.Cells[i + startRowNumber + 1 ,k].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center; //欄位置中
                                        ws.Cells[i + startRowNumber + 1 ,k].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                                        break;
                                    case 3:
                                    case 7:
                                    case 8:
                                    case 9:
                                    case 10:
                                    case 14:
                                    case 15:
                                    case 16:
                                    case 17:
                                    case 21:
                                    case 22:
                                    case 23:
                                        ws.Cells[i + startRowNumber + 1, k].Value = total[k - 1];
                                        ws.Cells[i + startRowNumber + 1, k].Style.Numberformat.Format = "#,##0";
                                        ws.Cells[i + startRowNumber + 1, k].Style.Font.Size = 12;
                                        ws.Cells[i + startRowNumber + 1, k].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;//欄位置右
                                        ws.Cells[i + startRowNumber + 1, k].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                                        break;
                                    default:
                                        ws.Cells[i + startRowNumber + 1, k].Value = "";
                                        ws.Cells[i + startRowNumber + 1 ,k].Style.Font.Size = 12;
                                        ws.Cells[i + startRowNumber + 1 ,k].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center; //欄位置中
                                        ws.Cells[i + startRowNumber + 1 ,k].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                                        break;
                                }
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
        public ActionResult RPT_190701()
        {
            string settleDate = "";
            string sDate = "";
            string eDate = "";
            string merchant = "";

            string group = "";
            string item = "";
            string retail = "";
            string bus = "";
            string bike = "";
            string track = "";
            string parking = "";
            string outsourcing = "";
            double pch = 0;
            double pchr = 0;
            double pch_net = 0;
            ViewBag.RepName = "布萊恩紅茶門市淨額小計";

            settleDate = DateTime.Now.ToString("yyyyMMdd");
            sDate = DateTime.Now.ToString("yyyyMMdd");
            eDate = DateTime.Now.ToString("yyyyMMdd");

            if (Request.Form["searchConfirm"] != null || Request.Form["ExportExcel"] != null || Request.Form["ExportExcelCPT"] != null)
            {
                sDate = Request.Form["sDate"];
                eDate = Request.Form["eDate"];

            }

            ViewBag.StartDate = sDate;
            ViewBag.EndDate = eDate;

            DataTable dt = new DataTable();
            if (Request.Form["searchConfirm"] != null || Request.Form["ExportExcel"] != null)
            {
                dt = RACTManager.ReportACT190701(sDate, eDate);
                if (dt.Rows.Count > 0)
                {
                    pch = Convert.ToDouble(dt.Compute("SUM(PCH_AMT)", "").ToString());
                    pchr = Convert.ToDouble(dt.Compute("SUM(PCHR_AMT)", "").ToString());
                    pch_net = Convert.ToDouble(dt.Compute("SUM(NET_AMT)", "").ToString());
                    ViewBag.PCH = pch;
                    ViewBag.PCHR = pchr;
                    ViewBag.PCH_NET = pch_net;
                }
            }
            else if (Request.Form["ExportExcelCPT"] != null)
            {
                dt = RACTManager.ReportACT190701_02(sDate, eDate);
                if (dt.Rows.Count > 0)
                {
                    pch = Convert.ToDouble(dt.Compute("SUM(PCH_AMT)", "").ToString());
                    pchr = Convert.ToDouble(dt.Compute("SUM(PCHR_AMT)", "").ToString());
                    pch_net = Convert.ToDouble(dt.Compute("SUM(NET_AMT)", "").ToString());

                }
            }
            else { dt = new DataTable(); }

            if (Request.Form["searchConfirm"] != null)
                return View(dt);
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
                    //統編	門市代號	門市名稱	購貨金額	購貨取消金額	購貨淨額
                    var columns = new[] { "統編", "門市代號", "門市名稱", "購貨金額", "購貨取消金額", "購貨淨額" };

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
                                case 4:
                                case 5:
                                case 6:
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

                        }
                        if (i == dt.Rows.Count) 
                        { 
                            for (int j = 1; j <= columns.Length; j++)
                            {
                                switch (j)
                                {
								    case 1:
								        ws.Cells[i + 3, j].Value = "小計";
                                        ws.Cells[i + 3, j].Style.Font.Size = 12;
                                        ws.Cells[i + 3, j].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center; //欄位置中
                                        ws.Cells[i + 3, j].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                                        break;
								
								
                                    case 4:
                                        ws.Cells[i + 3, j].Value = Convert.ToDouble(dt.Compute("SUM(PCH_AMT)", "").ToString());

                                        ws.Cells[i + 3, j].Style.Numberformat.Format = "#,##0";
                                        ws.Cells[i + 3, j].Style.Font.Size = 12;
                                        ws.Cells[i + 3, j].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;//欄位置右
                                        ws.Cells[i + 3, j].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                                        break;
                                    case 5:
                                        ws.Cells[i + 3, j].Value = Convert.ToDouble(dt.Compute("SUM(PCHR_AMT)", "").ToString());

                                        ws.Cells[i + 3, j].Style.Numberformat.Format = "#,##0";
                                        ws.Cells[i + 3, j].Style.Font.Size = 12;
                                        ws.Cells[i + 3, j].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;//欄位置右
                                        ws.Cells[i + 3, j].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                                        break;
                                    case 6:
                                        ws.Cells[i + 3, j].Value = Convert.ToDouble(dt.Compute("SUM(NET_AMT)", "").ToString());

                                        ws.Cells[i + 3, j].Style.Numberformat.Format = "#,##0";
                                        ws.Cells[i + 3, j].Style.Font.Size = 12;
                                        ws.Cells[i + 3, j].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;//欄位置右
                                        ws.Cells[i + 3, j].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                                        break;

                                    default:
                                        ws.Cells[i + 3, j].Style.Font.Size = 12;
                                        ws.Cells[i + 3, j].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center; //欄位置中
                                        ws.Cells[i + 3, j].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                                        break;
                                }

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
            else if (Request.Form["ExportExcelCPT"] != null)
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
                    //統編	門市代號	門市名稱	購貨金額	購貨取消金額	購貨淨額
                    var columns = new[] { "清分日", "統編", "門市代號", "門市名稱", "購貨金額", "購貨取消金額", "購貨淨額" };

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
                                case 5:
                                case 6:
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

                        }
                        if (i == dt.Rows.Count)
                        {
                            for (int j = 1; j <= columns.Length; j++)
                            {
                                switch (j)
                                {
                                    case 1:
                                        ws.Cells[i + 3, j].Value = "小計";
                                        ws.Cells[i + 3, j].Style.Font.Size = 12;
                                        ws.Cells[i + 3, j].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center; //欄位置中
                                        ws.Cells[i + 3, j].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                                        break;


                                    case 5:
                                        ws.Cells[i + 3, j].Value = Convert.ToDouble(dt.Compute("SUM(PCH_AMT)", "").ToString());

                                        ws.Cells[i + 3, j].Style.Numberformat.Format = "#,##0";
                                        ws.Cells[i + 3, j].Style.Font.Size = 12;
                                        ws.Cells[i + 3, j].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;//欄位置右
                                        ws.Cells[i + 3, j].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                                        break;
                                    case 6:
                                        ws.Cells[i + 3, j].Value = Convert.ToDouble(dt.Compute("SUM(PCHR_AMT)", "").ToString());

                                        ws.Cells[i + 3, j].Style.Numberformat.Format = "#,##0";
                                        ws.Cells[i + 3, j].Style.Font.Size = 12;
                                        ws.Cells[i + 3, j].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;//欄位置右
                                        ws.Cells[i + 3, j].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                                        break;
                                    case 7:
                                        ws.Cells[i + 3, j].Value = Convert.ToDouble(dt.Compute("SUM(NET_AMT)", "").ToString());

                                        ws.Cells[i + 3, j].Style.Numberformat.Format = "#,##0";
                                        ws.Cells[i + 3, j].Style.Font.Size = 12;
                                        ws.Cells[i + 3, j].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;//欄位置右
                                        ws.Cells[i + 3, j].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                                        break;

                                    default:
                                        ws.Cells[i + 3, j].Style.Font.Size = 12;
                                        ws.Cells[i + 3, j].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center; //欄位置中
                                        ws.Cells[i + 3, j].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                                        break;
                                }

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

        public ActionResult RPT_190901()
        {
            string start = DateTime.Now.ToString("yyyyMMdd");
            string end = DateTime.Now.ToString("yyyyMMdd");
			

            ViewBag.RepName = "屏東客運手續費";



            if (Request.Form["searchConfirm"] != null || Request.Form["ExportExcel"] != null)
            {
                start = Request.Form["StartDate"];
                end = Request.Form["EndDate"];
              
            }

            ViewBag.StartDate = start;
            ViewBag.EndDate = end;

			
			DataSet ds = new DataSet();
            DataTable dt1 = new DataTable();
			DataTable dt2 = new DataTable();
            DataTable dt3 = new DataTable();
            if (Request.Form["searchConfirm"] != null || Request.Form["ExportExcel"] != null)
			{

				ds = RACTManager.ReportACT190901(start,end);
                if (ds.Tables.Count > 1)
                {
                    dt1 = ds.Tables[0];
                    dt2 = ds.Tables[1];
                    dt3 = ds.Tables[2];
                    if (dt2.Rows.Count > 0)
                    {   
                        decimal sum_amt = Convert.ToDecimal(dt2.Compute("SUM(SUM_AMT)", ""));
                        decimal fee_amt = Convert.ToDecimal(dt2.Compute("SUM(FEE_AMT)", ""));
                    }
                    if (dt3.Rows.Count > 0)
                    {
                        ViewBag.DT3 = dt3;
                    }
                }
			}	 
			
           
            if (Request.Form["searchConfirm"] != null)
                return View(dt2);
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
                    r = ws.Cells["A2:E2"];
                    r.Merge = true;
                    r.Value = "清分日期：" + start + "~" + end;


                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                    r.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                    //r.Style.Font.Bold = true;
                    r.Style.Font.Size = 12;

                    int startRowNumber = 3;
                    var columns = new[] { "交易年月", "加總-交易金額", "加總-交易筆數", "手續費率", "手續費" };




                    for (int j = 1; j <= columns.Length; j++)
                    {

                        ws.Cells[startRowNumber, j].Value = columns[j - 1].ToString();
                        //設定樣式
                        ws.Cells[startRowNumber, j].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right; //欄位置中
                        ws.Cells[startRowNumber, j].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        ws.Cells[startRowNumber, j].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        ws.Cells[startRowNumber, j].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                        ws.Cells[startRowNumber, j].Style.Font.Bold = true;
                        ws.Cells[startRowNumber, j].Style.Font.Size = 12;

                    }
                    for (int i = 1; i <= dt2.Rows.Count; i++)
                    {

                        for (int j = 1; j <= dt2.Columns.Count; j++)
                        {
                            switch (j)
                            {

                                case 4:
                                    ws.Cells[i + 3, j].Value = Convert.ToDouble(dt2.Rows[i - 1][j - 1]);
                                    ws.Cells[i + 3, j].Style.Numberformat.Format = "#,##0.00"; ;
                                    ws.Cells[i + 3, j].Style.Font.Size = 12;
                                    ws.Cells[i + 3, j].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;//欄位置右
                                    ws.Cells[i + 3, j].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                                    break;
                                case 2:
                                case 3:
                                case 5:
                                    ws.Cells[i + 3, j].Value = Convert.ToDouble(dt2.Rows[i - 1][j - 1]);
                                    ws.Cells[i + 3, j].Style.Numberformat.Format = "#,##0";
                                    ws.Cells[i + 3, j].Style.Font.Size = 12;
                                    ws.Cells[i + 3, j].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;//欄位置右
                                    ws.Cells[i + 3, j].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                                    break;

                                default:
                                    ws.Cells[i + 3, j].Value = dt2.Rows[i - 1][j - 1].ToString();

                                    ws.Cells[i + 3, j].Style.Font.Size = 12;
                                    ws.Cells[i + 3, j].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right; //欄位置中
                                    ws.Cells[i + 3, j].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                                    break;
                            }
                            if (i == dt2.Rows.Count)
                            {
                                ws.Cells[i + 4, 1].Value = "總計";
                                ws.Cells[i + 4, 1].Style.Font.Size = 12;
                                ws.Cells[i + 4, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right; //欄位置中
                                //設定框線
                                ws.Cells[i + 4, 1].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                                ws.Cells[i + 4, 2].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                                ws.Cells[i + 4, 3].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                                ws.Cells[i + 4, 4].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                                ws.Cells[i + 4, 5].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                                
                                //總計值
                                ws.Cells[i + 4, 2].Value = Convert.ToDouble(dt3.Rows[0][1]);
                                ws.Cells[i + 4, 3].Value = Convert.ToDouble(dt3.Rows[0][2]);
                                ws.Cells[i + 4, 4].Value = Convert.ToDouble(dt3.Rows[0][3]);
                                ws.Cells[i + 4, 5].Value = Convert.ToDouble(dt3.Rows[0][4]);

                                //
                                ws.Cells[i + 4, 2].Style.Numberformat.Format = "#,##0";
                                ws.Cells[i + 4, 3].Style.Numberformat.Format = "#,##0";
                                ws.Cells[i + 4, 4].Style.Numberformat.Format = "0.00";
                                ws.Cells[i + 4, 5].Style.Numberformat.Format = "#,##0";


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

        public ActionResult RPT_191001()
        {
            string yearMonth = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString().PadLeft(2, '0');
            string start = "";
            string end = "";


            ViewBag.RepName = "國光客運手續費";

            ViewBag.YearMonth = yearMonth;

            if (Request.Form["searchConfirm"] != null || Request.Form["ExportExcel"] != null)
            {
                yearMonth = Request.Form["YearMonth"];


            }

            ViewBag.YearMonth = yearMonth;

            DataSet ds = new DataSet();
            DataTable dt1 = new DataTable();
            DataTable dt2 = new DataTable();
            DataTable dt3 = new DataTable();

            if (Request.Form["searchConfirm"] != null || Request.Form["ExportExcel"] != null)
            {
                string ym = DateTime.ParseExact(yearMonth + "01", "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture).AddMonths(1).ToString("yyyyMMdd"); ;
                ds = RACTManager.ReportACT191001(ym);
                if (ds.Tables.Count > 0)
                {
                    dt1 = ds.Tables[0];
                    dt2 = ds.Tables[1];
                    dt3 = ds.Tables[2];
                    if (dt1.Rows.Count > 0) 
                    {
                        start = dt1.Rows[0][0].ToString();
                        end = dt1.Rows[0][1].ToString();
                    }
                  

                }
            }


            if (Request.Form["searchConfirm"] != null)
                return View(dt3);
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
                    r = ws.Cells["A2:E2"];
                    r.Merge = true;
                    r.Value = "清分日期：" + start + "~" + end;


                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                    r.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                    //r.Style.Font.Bold = true;
                    r.Style.Font.Size = 12;

                    int startRowNumber = 3;
                    var columns = new[] { "交易日期", "交易類別", "筆數", "手續費率", "手續費" };




                    for (int j = 1; j <= columns.Length; j++)
                    {

                        ws.Cells[startRowNumber, j].Value = columns[j - 1].ToString();
                        //設定樣式
                        ws.Cells[startRowNumber, j].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right; //欄位置中
                        ws.Cells[startRowNumber, j].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        ws.Cells[startRowNumber, j].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        ws.Cells[startRowNumber, j].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                        ws.Cells[startRowNumber, j].Style.Font.Bold = true;
                        ws.Cells[startRowNumber, j].Style.Font.Size = 12;

                    }
                    for (int i = 1; i <= dt3.Rows.Count; i++)
                    {

                        for (int j = 1; j <= dt3.Columns.Count; j++)
                        {
                            switch (j)
                            {

                                case 4:
                                    ws.Cells[i + 3, j].Value = Convert.ToDouble(dt3.Rows[i - 1][j - 1]);
                                    ws.Cells[i + 3, j].Style.Numberformat.Format = "#,##0.00#"; ;
                                    ws.Cells[i + 3, j].Style.Font.Size = 12;
                                    ws.Cells[i + 3, j].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;//欄位置右
                                    ws.Cells[i + 3, j].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                                    break;

                                case 3:
                                case 5:
                                    ws.Cells[i + 3, j].Value = Convert.ToDouble(dt3.Rows[i - 1][j - 1]);
                                    ws.Cells[i + 3, j].Style.Numberformat.Format = "#,##0";
                                    ws.Cells[i + 3, j].Style.Font.Size = 12;
                                    ws.Cells[i + 3, j].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;//欄位置右
                                    ws.Cells[i + 3, j].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                                    break;

                                default:
                                    ws.Cells[i + 3, j].Value = dt3.Rows[i - 1][j - 1].ToString();

                                    ws.Cells[i + 3, j].Style.Font.Size = 12;
                                    ws.Cells[i + 3, j].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right; //欄位置中
                                    ws.Cells[i + 3, j].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
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