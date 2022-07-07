using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;

using System.IO;
using Business;
using DataAccess;
using Domain;

using OfficeOpenXml;
using System.Xml;
using OfficeOpenXml.Style;

namespace AMS.Controllers
{
     [CustomAuthorize(AccessLevel = "System,Sales,SystemSE,SalesManager,PM")]
    public class ReportMarketingController : BaseController
    {
        public ReportMarketingManager RMManager { get; set; }
        public GmMerchantManager GMManager { get; set; }
        //
        // GET: /ReportMarketing/
        public ReportMarketingController()
        {
            RMManager = new ReportMarketingManager();
            GMManager = new GmMerchantManager();
        }

        public List<SelectListItem> SetGroup(string group)
        {
            List<SelectListItem> groups = new List<SelectListItem>();
            groups.AddRange(new SelectList(GMManager.FindAllGroup(), "MerchantNo", "MerchantName", group));
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


        public ActionResult RPT_160101()
        {
            string start = DateTime.Now.AddDays(-1).ToString("yyyyMMdd");
            string end = DateTime.Now.ToString("yyyyMMdd");
            string merchant = "";
            string card_kind1=""; string kind1="";
            string card_kind2=""; string kind2="";
            string card_kind3=""; string kind3="";

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

                //merchant = Request.Form["COMPANY"];
                //ViewData["COMPANY"] = merchant;

                card_kind1=Request.Form["kind1"];
                card_kind2=Request.Form["kind2"];
                card_kind3=Request.Form["kind3"];
                
                if(card_kind1=="I")
                {ViewBag.ch1="checked"; kind1="一代卡";}
                if (card_kind2=="II")  
                {ViewBag.ch2="checked"; kind2="二代卡";}
                if (card_kind3=="COMBO")
                { ViewBag.ch3 = "checked"; kind3="聯名卡"; }

                group = Request.Form["group"];
                item = Request.Form["items"];
                merchant = Request.Form["items"];

                //group = Request.Form["group"];
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

            ViewBag.RepName = "月別特約機構每日消費明細表";
            ViewBag.Start = start;
            ViewBag.End = end;
            /*
            List<SelectListItem> companiesList = new List<SelectListItem>();
            IEnumerable<GmMerchant> allMerchants = GMManager.FindAll();
            IEnumerable<GmMerchant> distinctMerchants = from k in allMerchants
                                                        group k by new { k.MerchantNo, k.MerchantName } into g
                                                        select g.FirstOrDefault();
            companiesList.AddRange(new SelectList(distinctMerchants, "MerchantNo", "MerchantName"));
            companiesList.Add(new SelectListItem() { Value = "ALL",Text = "ALL" , Selected = false });
            ViewBag.CompaniesList = companiesList;
            */
            DataTable dt = new DataTable();
            List<string> mNoList= new List<string>(); 
            if (group == "ALL" )
            {
                mNoList = new GmMerchantTypeDAO().FindAll();

                foreach (string mNo in mNoList)
                {dt.Merge(RMManager.ReportMarketing160101(start, end, mNo,kind1,kind2,kind3));}
            }
            else if (item == "ALL")
            //else if (retail == "ALL" || bus == "ALL" || bike == "ALL" || track == "ALL" || parking == "ALL" || outsourcing == "ALL")
            {
                mNoList = new GmMerchantTypeDAO().FindGroupAll(group);
                foreach (string mNo in mNoList)
                { dt.Merge(RMManager.ReportMarketing160101(start, end, mNo, kind1, kind2, kind3)); }
            }
            else if (merchant !="")
            {
                dt = RMManager.ReportMarketing160101(start, end, merchant, kind1, kind2, kind3);
            }
            else { dt = new DataTable(); }

            if (Request.Form["searchConfirm"] != null)
            { return View(dt); }
            else if (Request.Form["ExportExcel"] != null)
            {
                using (ExcelPackage p = new ExcelPackage())
                {
                    ExcelWorksheet ws = p.Workbook.Worksheets.Add(ViewBag.RepName.ToString());


                    ExcelRange r = ws.Cells["A1:J1"];
                    r.Merge = true;
                    r.Value = ViewBag.RepName.ToString();


                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    r.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                    //r.Style.Font.Bold = true;
                    r.Style.Font.Size = 14;

                    int startRowNumber = 2;
                    var columns = new[] { "特約機構", "交易日", "特約機構名稱", "卡別", "購貨總額", "退貨總額", "購貨總數", "退貨總數","購貨卡數","退貨卡數" };

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
                                case 8-10:
                                    if (!DBNull.Value.Equals(dt.Rows[i - 1][j - 1]))
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

        public ActionResult RPT_160102()
        {
            string start = DateTime.Now.AddDays(-1).ToString("yyyyMMdd");
            string end = DateTime.Now.ToString("yyyyMMdd");
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
                group = Request.Form["group"];
                item = Request.Form["items"];
                merchant = Request.Form["items"];
                //group = Request.Form["group"];
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

            ViewBag.RepName = "聯名卡特約機構每日消費明細表";
            ViewBag.Start = start;
            ViewBag.End = end;

            DataTable dt = new DataTable();
            List<string> mNoList = new List<string>();
            if (group == "ALL")
            {
                mNoList = new GmMerchantTypeDAO().FindAll();

                foreach (string mNo in mNoList)
                { dt.Merge(RMManager.ReportMarketing160102(start, end, mNo)); }
                var view = dt.DefaultView;
                dt = view.ToTable();
            }
            else if (item == "ALL")
            //else if (retail == "ALL" || bus == "ALL" || bike == "ALL" || track == "ALL" || parking == "ALL" || outsourcing == "ALL")
            {
                mNoList = new GmMerchantTypeDAO().FindGroupAll(group);
                foreach (string mNo in mNoList)
                { dt.Merge(RMManager.ReportMarketing160102(start, end, mNo)); }
                var view = dt.DefaultView;
                dt = view.ToTable();
            }
            else if (merchant != "")
            {
                dt = RMManager.ReportMarketing160102(start, end, merchant);
                var view = dt.DefaultView;
                dt = view.ToTable();
            }
            else { dt = new DataTable(); }

            if (Request.Form["searchConfirm"] != null)
                return View(dt);
            else if(Request.Form["ExportExcel"] != null)
                return File(ExportRPT160102FileStream(dt).ToArray(), "application/vnd.ms-excel", ViewBag.RepName + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx");
            else 
                return View(new DataTable());
        }

        private MemoryStream ExportRPT160102FileStream(DataTable dt)
        {
            var stream = new MemoryStream();
            using (ExcelPackage p = new ExcelPackage())
            {
                DataTable merchantDistinct = dt.DefaultView.ToTable(true, "STNAME");

                int startRowNumber = 2;
                var banks = new GmMerchantManager().FindAllBnak();
                var distinctBank = from k in banks
                                   group k by new { k.MerchantNo, k.MerchantName } into g
                                   select g.FirstOrDefault();
                var headers = new List<string>();
                headers.Add("對帳平台日期");
                foreach (var item in distinctBank)
                {
                    headers.Add(item.MerchantName);
                }

                var dataAll = dt.AsEnumerable().Select(row => new
                {
                    Date = row[0].ToString(),
                    Bank = row[1].ToString(),
                    Merchant = row[2].ToString(),
                    Amt = row[3].ToString(),
                    Cnt = row[4].ToString()
                });

                ExcelWorksheet ws = null;
                foreach (DataRow sheet in merchantDistinct.Rows)
                {
                    ws = p.Workbook.Worksheets.Add(sheet.ItemArray[0].ToString());
                    ExcelRange r = ws.Cells["A1:K1"];
                    r.Merge = true;
                    r.Value = ViewBag.RepName.ToString();

                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    r.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    r.Style.Font.Size = 14;
                    for (int j = 1; j <= headers.Count; j++)
                    {
                        //設值為欄位名稱
                        ws.Cells[startRowNumber, j].Value = headers[j - 1];
                        //設定樣式
                        ws.Cells[startRowNumber, j].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center; //欄位置中
                        ws.Cells[startRowNumber, j].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                        ws.Cells[startRowNumber, j].Style.Font.Bold = true;
                        ws.Cells[startRowNumber, j].Style.Font.Size = 12;
                    }
                    var dataMerchant = dataAll.Where(x => x.Merchant == sheet.ItemArray[0].ToString()).OrderBy(x => x.Date).ToList();
                    var dates = dataMerchant.Select(x => x.Date).Distinct().ToList();
                    for(int i=0;i< dates.Count();i++)
                    {
                        ws.Cells[i + 3, 1].Value = dates.ElementAt(i);
                        ws.Cells[i + 3, 1].Style.Font.Size = 12;
                        ws.Cells[i + 3, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center; //欄位置中
                        ws.Cells[i + 3, 1].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        var dataDate = dataMerchant.Where(x => x.Date == dates.ElementAt(i));
                        for (int j = 0; j < distinctBank.Count(); j++)
                        {
                            var bankAmt = dataDate.Where(x => x.Bank == distinctBank.ElementAt(j).MerchantName).Select(x => x.Amt);
                            ws.Cells[i + 3, j + 2].Value = bankAmt.Count() > 0 ? bankAmt.First() : "0";
                            ws.Cells[i + 3, j + 2].Style.Font.Size = 12;
                            ws.Cells[i + 3, j + 2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center; //欄位置中
                            ws.Cells[i + 3, j + 2].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        }
                    }
                    ws.Cells[dates.Count() + 3, 1].Value = "小計";
                    ws.Cells[dates.Count() + 3, 1].Style.Font.Size = 12;
                    ws.Cells[dates.Count() + 3, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center; //欄位置中
                    ws.Cells[dates.Count() + 3, 1].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    for (int x = 0; x < distinctBank.Count(); x++)
                    {
                        int sum = 0;
                        for (int y = 0; y < dates.Count(); y++)
                        {
                            sum += int.Parse(ws.Cells[y + 3, x + 2].Value.ToString());
                        }
                        ws.Cells[dates.Count() + 3, x + 2].Value = sum;
                        ws.Cells[dates.Count() + 3, x + 2].Style.Font.Size = 12;
                        ws.Cells[dates.Count() + 3, x + 2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center; //欄位置中
                        ws.Cells[dates.Count() + 3, x + 2].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    }
                    ws.Cells[ws.Dimension.Address].AutoFitColumns();
                }               
                p.SaveAs(stream);
                stream.Close();
            }
            return stream;
        }
	}
}