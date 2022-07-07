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
    public class ReportSystemController : BaseController
    {
        //
        public ReportSystemManager RSManager { get; set; }
        public GmSchemaObjManager GSOManager { get; set; }

        public GmMerchantManager GMManager { get; set; }
        public ReportSystemController()
        {
            RSManager = new ReportSystemManager();
            GSOManager = new GmSchemaObjManager();
            GMManager = new GmMerchantManager();
        }

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




        public ActionResult RPT_160501()
        {
            string start = DateTime.Now.ToString("yyyyMMdd");
            string end = DateTime.Now.ToString("yyyyMMdd");
            string group = "";
            string item = "";
            string merchant = "";
            string retail = "";
            string bus = "";
            string bike = "";
            string track = "";
            string parking = "";
            string outsourcing = "";

            DataTable dt = null;

               

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

                ViewBag.RepName = "每日特約機構清分結果確認表";
                ViewBag.Start = start;
                ViewBag.End = end;

                

                //if (group == "ALL" || retail == "ALL" || bus == "ALL" || bike == "ALL" || track == "ALL" || parking == "ALL" || outsourcing == "ALL")
                if (group == "ALL" || item == "ALL")
                { dt = RSManager.ReportSystem160501(start, end, group, "ALL",""); }
                else
                {
                    if (group != null && group != "")
                    {
                        //if(group == "PARKING_LOT") { group="PARKING"; }
                        //else if (group == "BANK_OUTSOURCING") { group = "OUTSOURCING"; } 

                        //var selected = ((List<SelectListItem>)ViewData[group]).Where(x => x.Selected == true);
                        //if ( selected.Count() !=0 )
                        //{
                        //    string valueSelected = selected.FirstOrDefault().Value.ToString();
                            dt = RSManager.ReportSystem160501(start, end, group, GSOManager.FindSchemaObj(item).MerchantSchemaId, item);
                        //}
                        //else { dt = new DataTable(); }
                    }
                    else 
                    {
                        dt = new DataTable();
                    }
                }

            
            
            if (Request.Form["searchConfirm"] != null)
            { return View(dt); }
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
                    var columns = new[] { "清分日期", "特約機構編號", "特約機構", "清分結果" };

                    for (int j = 1; j <= columns.Length; j++)
                    {
                        //設值為欄位名稱
                        ws.Cells[startRowNumber, j].Value = columns[j - 1];
                        //設定樣式
                        ws.Cells[startRowNumber, j].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center; //欄位置中
                        ws.Cells[startRowNumber, j].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                        ws.Cells[startRowNumber, j].Style.Font.Bold = true;
                        ws.Cells[startRowNumber, j].Style.Font.Size = 12;   //較小字形
                    }

                    for (int i = 1; i <= dt.Rows.Count; i++)
                    {

                        for (int j = 1; j <= columns.Length; j++)
                        {
                            switch (j)
                            {
                                /*
                                case :
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
        
	}
}