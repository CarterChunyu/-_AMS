using AMS.Models;
using Business;
using DataAccess;
using Domain;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common.Logging;
using Microsoft.AspNet.Identity;


namespace AMS.Controllers
{
    [CustomAuthorize(AccessLevel = "System,Accounting,Sales")]
    public class GmTypeController : BaseController
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(GmTypeController));

        public GmTypeManager gmTypeManager { get; set; }
        public GmMerchantManager gmMerchantManager { get; set; }

        public GmTypeController()
        {
            gmTypeManager = new GmTypeManager();
            gmMerchantManager = new GmMerchantManager();
        }
        public List<SelectListItem> SetGroup(string group)
        {
            List<SelectListItem> groups = new List<SelectListItem>();
            groups.AddRange(new SelectList(gmMerchantManager.FindAllGroup(), "MerchantNo", "MerchantName", group));
            return groups;

        }
        public List<SelectListItem> SetGroup2(string group)
        {
            List<SelectListItem> groups = new List<SelectListItem>();
            groups.AddRange(new SelectList(gmMerchantManager.FindGroupNoMstore(), "MerchantNo", "MerchantName", group));
            return groups;

        }

        public List<SelectListItem> SetRetail(string retail)
        {
            List<SelectListItem> retails = new List<SelectListItem>();
            retails.Add(new SelectListItem() { Text = "ALL", Value = "ALL", Selected = false });
            retails.AddRange(new SelectList(gmMerchantManager.FindAllRetail(), "MerchantNo", "MerchantName", retail));
            return retails;

        }

        public List<SelectListItem> SetBus(string bus)
        {
            List<SelectListItem> buses = new List<SelectListItem>();
            buses.Add(new SelectListItem() { Text = "ALL", Value = "ALL", Selected = false });
            buses.AddRange(new SelectList(gmMerchantManager.FindAllBus(), "MerchantNo", "MerchantName", bus));
            return buses;

        }

        public List<SelectListItem> SetBike(string bike)
        {
            List<SelectListItem> bikes = new List<SelectListItem>();
            bikes.Add(new SelectListItem() { Text = "ALL", Value = "ALL", Selected = false });
            bikes.AddRange(new SelectList(gmMerchantManager.FindAllBike(), "MerchantNo", "MerchantName", bike));
            return bikes;

        }

        public List<SelectListItem> SetTrack(string track)
        {
            List<SelectListItem> tracks = new List<SelectListItem>();
            tracks.Add(new SelectListItem() { Text = "ALL", Value = "ALL", Selected = false });
            tracks.AddRange(new SelectList(gmMerchantManager.FindAllTrack(), "MerchantNo", "MerchantName", track));
            return tracks;

        }

        public List<SelectListItem> SetParking(string parking)
        {
            List<SelectListItem> parkings = new List<SelectListItem>();
            parkings.Add(new SelectListItem() { Text = "ALL", Value = "ALL", Selected = false });
            parkings.AddRange(new SelectList(gmMerchantManager.FindAllParking(), "MerchantNo", "MerchantName", parking));
            return parkings;

        }

        public List<SelectListItem> SetOutsourcing(string outsourcing)
        {
            List<SelectListItem> outsourcings = new List<SelectListItem>();
            outsourcings.Add(new SelectListItem() { Text = "ALL", Value = "ALL", Selected = false });
            outsourcings.AddRange(new SelectList(gmMerchantManager.FindAllOutsourcing(), "MerchantNo", "MerchantName", outsourcing));
            return outsourcings;

        }
        public List<SelectListItem> SetRetailNotXDD(string retail)
        {
            List<SelectListItem> retails = new List<SelectListItem>();
            retails.Add(new SelectListItem() { Text = "ALL", Value = "ALL", Selected = false });
            retails.AddRange(new SelectList(gmMerchantManager.FindRetailNotXDD(), "MerchantNo", "MerchantName", retail));
            return retails;
        }// 初始化DropDownList      
        List<SelectListItem> GetSelectItem(bool dvalue = true)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            return items;
        }

        // 第一層下拉選項       
        //private List<SelectListItem> SetMerchantDropDown(string no = "")
        //{
        //    List<SelectListItem> items = GetSelectItem();
        //    items.AddRange(new SelectList(this.gmMerchantManager.FindAll(), "MerchantNo", "MerchantName", no));
        //    return items;
        //}

        private List<SelectListItem> SetMerchantBankDropDown(string no = "")
        {
            List<SelectListItem> items = GetSelectItem();
            items.AddRange(new SelectList(this.gmMerchantManager.FindAllBnak(), "MerchantNo", "MerchantName", no));
            return items;
        }
        private List<SelectListItem> SetMerchantBankWithAll(string no = "")
        {
            List<SelectListItem> items = GetSelectItem();
            items.Add(new SelectListItem() { Text = "ALL", Value = "ALL", Selected = false });
            items.AddRange(new SelectList(this.gmMerchantManager.FindAllBnak(), "MerchantNo", "MerchantName", no));
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
            parkings.Add(new SelectListItem() { Text = "ALL", Value = "ALL", Selected = false });
            parkings.AddRange(new SelectList(gmMerchantManager.FindAllbyGroup(group), "MerchantNo", "MerchantName", group));
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
                default:
                    return SetAllbyGroup(groupName);
            }
        }
        public ActionResult Index()
        {
            string merchant = "";
            string group = "";
            string item = "";
            DataTable dt = new DataTable();
            GmTypeDAO ga = new GmTypeDAO();

            if (Request.Form["searchConfirm"] != null&& Request.Form["group"]!=null)
            {
                group = Request.Form["group"];
                item = Request.Form["items"];
                merchant = Request.Form["items"];
            }

            ViewBag.RepName = "特店群組維護";
            ViewBag.MERCHANT = SetGroup(group);
            ViewBag.MERCHANT_NAME = group;
            ViewBag.ITEM = item;

            if (Request.Form["searchConfirm"] != null)
            {
                if (group == "ALL")
                {
                    dt = ga.GM_TYPE_INDEX(group,"ALL");
                }
                else if (merchant!="")
                {
                    dt = ga.GM_TYPE_INDEX(group, merchant);
                }
                else { dt = new DataTable(); }

                //排序
                if (dt.Rows.Count > 0)
                {
                    dt.DefaultView.Sort = "GROUP_ID,SHOW_ORDER";
                    dt = dt.DefaultView.ToTable();
                }
            }
            if (Request.Form["searchConfirm"] != null)
            {
                return View(dt);
            }
            else return View(new DataTable());
        }
        public ActionResult Edit(string group,string merchant)
        {
            ViewBag.RepName = "特店群組維護-編輯";
            DataTable dt = new DataTable();
            GmTypeDAO ga = new GmTypeDAO();
            dt = ga.GM_TYPE_EDIT(group, merchant);
            ViewBag.MERCHANT_NO = dt.Rows[0]["MERCHANT_NO"].ToString();
            ViewBag.MERCHANT_NAME = dt.Rows[0]["MERCHANT_NAME"].ToString();
            ViewBag.GROUP_NAME = dt.Rows[0]["GROUP_NAME"].ToString();
            ViewBag.SHOW_ORDER = dt.Rows[0]["SHOW_ORDER"].ToString();

            if (dt.Rows[0]["GROUP_ID"].ToString().Equals("") || dt.Rows[0]["GROUP_ID"].ToString() == null)
            {
                ViewBag.GROUP_ID_VALUES = "01";
            }
            else
            {
                ViewBag.GROUP_ID_VALUES = dt.Rows[0]["GROUP_ID"].ToString();
            }
            ViewBag.GROUP_ID = SetGroup("");

            return View(dt);
        }
        public ActionResult Update()
        {
            GmType entity = new GmType();
            entity.MERCHANT_NO = Request.Form["MERCHANT_NO"];
            try
            {
                entity.GROUP_ID = Request.Form["GROUP_ID"];
                entity.SHOW_ORDER = Request.Form["SHOW_ORDER"].Trim();
            }
            catch (Exception ex)
            {
                log.Debug(ex.Message);
            }
            entity.SHOW_FLG = Request.Form["classtype"];

            if (Request.Form["searchConfirm"] != null)
            {
                GmTypeDAO g = new GmTypeDAO();

                g.GM_TYPE_UPDATE(entity, User.Identity.Name.ToString());
                DataTable dt = g.GM_TYPE_INDEX(entity.GROUP_ID,entity.MERCHANT_NO);
                return View(ReIndex(entity.GROUP_ID, entity.MERCHANT_NO));
            }
            else { return View(); }
        }

        public DataTable ReIndex(string group,string merchant)
        {

            GmTypeDAO g = new GmTypeDAO();

            ViewBag.RepName = "特店群組維護";
            ViewBag.MERCHANT = SetGroup(group);
            ViewBag.MERCHANT_NAME = group;
            ViewBag.ITEM = merchant;

            DataTable dt = g.GM_TYPE_INDEX(group, merchant);
            return dt;

        }

    }
}