using AMS.Models;
using Business;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Domain;

namespace AMS.Controllers
{
    [CustomAuthorize(AccessLevel = "System,Accounting,Sales,PM,SystemSE,SalesManager")]
    public class MerchantController : BaseController
    {
        public GmMerchantManager gmMerchantManager { get; set; }
        public GmMerchantStoreManager gmMerchantStoreManager { get; set; }
        public GmMerchTMManager gmMerchTMManager { get; set; }
        public GmCodeMappginManager gmCodeMappginManager { get; set; }

        public MerchantController()
        {
            gmMerchantManager = new GmMerchantManager();
            gmMerchantStoreManager = new GmMerchantStoreManager();
            gmMerchTMManager = new GmMerchTMManager();
            gmCodeMappginManager = new GmCodeMappginManager();
        }

        #region UI元件

        /// <summary>
        /// 取得特約機構群組
        /// </summary>
        /// <param name="group_id">預設群組</param>
        /// <returns></returns>
        public List<SelectListItem> GetMerchantGroup(string group_id)
        {
            List<SelectListItem> groupList = new List<SelectListItem>();
            groupList.AddRange(new SelectList(gmMerchantManager.FindAllGroup(), "MerchantNo", "MerchantName", group_id));
            //groupList.Add(new SelectListItem() { Text = "ALL", Value = "ALL", Selected = false });

            return groupList;
        }

        /// <summary>
        /// 依群組取得相關特約機構
        /// </summary>
        /// <param name="group_id">群組</param>
        /// <param name="merchant_no">預設特約機構</param>
        /// <returns></returns>
        private List<SelectListItem> GetMerchantByGroup(string group_id, string merchant_no)
        {
            List<SelectListItem> merchantList = new List<SelectListItem>();
            merchantList.AddRange(new SelectList(gmMerchantManager.FindMerchant(group_id), "MerchantNo", "MerchantName", merchant_no));
            //merchantList.Add(new SelectListItem() { Text = "ALL", Value = "ALL", Selected = false });

            return merchantList;
        }

        /// <summary>
        /// 依群組取得可編輯門市主檔的相關特約機構
        /// </summary>
        /// <param name="group_id">群組</param>
        /// <param name="merchant_no">預設特約機構</param>
        /// <returns></returns>
        private List<SelectListItem> GetMerchantStoreByGroup(string group_id, string merchant_no)
        {
            List<SelectListItem> merchantList = new List<SelectListItem>();
            merchantList.AddRange(new SelectList(gmMerchantStoreManager.FindList(new Domain.GmMerchantStore() { GROUP_ID = group_id, IS_DEL = false }), "MERCHANT_NO", "MERCHANT_NAME", merchant_no));
            
            return merchantList;
        }

        /// <summary>
        /// 取得可編輯門市主檔的相關特約機構
        /// </summary>
        /// <param name="group_id">群組</param>
        /// <param name="merchant_no">特約機構</param>
        /// <returns></returns>
        private List<Domain.GmMerchantStore> GetMerchant_MaintainStoreData(string group_id, string merchant_no)
        {
            return gmMerchantStoreManager.FindList(new Domain.GmMerchantStore() { GROUP_ID = (group_id.ToUpper() == "ALL") ? "" : group_id, MERCHANT_NO = (group_id.ToUpper() == "ALL" || merchant_no.ToUpper() == "ALL") ? "" : merchant_no, IS_DEL = false });
        }

        /// <summary>
        /// 取得指定特約機構的相關門市
        /// </summary>
        /// <param name="merchant_no">特約機構代號</param>
        /// <param name="merchant_name">特約機構名稱</param>
        /// <returns></returns>
        private List<BmStoreM> GetStoreData(string merchant_no, string store_type, out string merchant_name)
        {
            List<BmStoreM> storeList = new List<BmStoreM>();
            DataRow merchant = gmMerchantManager.FindMerchantData(merchant_no);
            merchant_name = "";
            if (merchant == null)
            { return null; }
            else
            {
                string schema = "" + merchant["MERCH_SCHEMAID"];
                merchant_name = "" + merchant["MERCHANT_STNAME"];
                DataTable result = gmMerchantStoreManager.FindStoreData(schema, merchant_no, "", store_type);
                foreach (DataRow row in result.Rows)
                {
                    if (store_type == @"TRAFFIC_1")
                    {
                        storeList.Add(new BmStoreM()
                        {
                            MERCHANT_NO = "" + row["MERCHANT_NO"],
                            STORE_NO = "" + row["STORE_NO"],
                            STO_NAME_LONG = "" + row["STO_NAME_LONG"],
                            STO_NAME_SHORT = "" + row["STO_NAME_SHORT"],
                            LINE_TYPE = "" + row["LINE_TYPE_DESC"],
                            LINE_NO_04 = "" + row["LINE_NO_04_DESC"],
                            EFF_DATE_FROM = "" + row["EFF_DATE_FROM"],
                            EFF_DATE_TO = "" + row["EFF_DATE_TO"],
                            OPEN_DATE = "" + row["OPEN_DATE"],
                            UPD_DATE = "" + row["UPD_DATE"]
                        });
                    }
                    else
                    {
                        storeList.Add(new BmStoreM()
                        {
                            MERCHANT_NO = "" + row["MERCHANT_NO"],
                            STORE_NO = "" + row["STORE_NO"],
                            STO_NAME_LONG = "" + row["STO_NAME_LONG"],
                            STO_NAME_SHORT = "" + row["STO_NAME_SHORT"],
                            EFF_DATE_FROM = "" + row["EFF_DATE_FROM"],
                            EFF_DATE_TO = "" + row["EFF_DATE_TO"],
                            OPEN_DATE = "" + row["OPEN_DATE"],
                            UPD_DATE = "" + row["UPD_DATE"]
                        });
                    }
                }
            }

            return storeList;
        }

        /// <summary>
        /// 依群組取得相關特約機構
        /// </summary>
        /// <param name="group_id">群組</param>
        /// <param name="merchant_name">預設特約機構</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetMerchant(string group_id, string merchant_name)
        {
            List<KeyValuePair<string, string>> items = new List<KeyValuePair<string, string>>();
            List<KeyValuePair<string, string>> itemStores = new List<KeyValuePair<string, string>>();

            if (!string.IsNullOrWhiteSpace(group_id))
            {
                var merchants = this.GetMerchantByGroup(group_id, merchant_name);
                if (merchants != null && merchants.Count() > 0)
                {
                    foreach (var merchant in merchants)
                    {
                        items.Add(new KeyValuePair<string, string>(
                            merchant.Value.ToString(),
                            merchant.Text));
                    }
                }

                var merchantStore = this.GetMerchantStoreByGroup(group_id, merchant_name);
                if (merchantStore != null && merchantStore.Count() > 0)
                {
                    foreach (var store in merchantStore)
                    {
                        itemStores.Add(new KeyValuePair<string, string>(
                            store.Value.ToString(),
                            store.Text));
                    }
                }
            }
            return this.Json(items.Except(itemStores).ToList());
        }

        /// <summary>
        /// 依群組取得可編輯門市主檔的相關特約機構
        /// </summary>
        /// <param name="group_id">群組</param>
        /// <returns></returns>
        public JsonResult GetMerchantStore(string group_id)
        {
            List<KeyValuePair<string, string>> items = new List<KeyValuePair<string, string>>();

            group_id = (group_id.ToUpper() == "ALL") ? "" : group_id;
            if (!string.IsNullOrWhiteSpace(group_id))
            {
                var merchants = this.GetMerchantStoreByGroup(group_id, "");
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

            return this.Json(items, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 取得 ibon顯示類型
        /// </summary>
        /// <param name="ibon_show_type"></param>
        /// <returns></returns>
        public List<SelectListItem> GetIbonShowType(string ibon_show_type)
        {
            List<SelectListItem> typeList = new List<SelectListItem>();
            for (int i = 0; i < ibonShowTypeConfig.Count; i++)
            { typeList.Add(new SelectListItem() { Text = ibonShowTypeConfig[i].VALUE, Value = ibonShowTypeConfig[i].ID, Selected = (ibonShowTypeConfig[i].ID == ibon_show_type) }); }

            return typeList;
        }

        private static Dictionary<int, SCONFIG> ibonShowTypeConfig = new Dictionary<int, SCONFIG>() 
        {
            {0, new SCONFIG(){ ID = @"MERC", VALUE = @"特店"}}, 
            {1, new SCONFIG(){ ID = @"MERC_STATION", VALUE = @"特店+站名"}}
        };

        public struct SCONFIG
        {
            public string ID;
            public string VALUE;
        }

        public List<SelectListItem> GetIsActive(string is_active)
        {
            List<SelectListItem> typeList = new List<SelectListItem>();
            for (int i = 0; i < isActiveConfig.Count; i++)
            { typeList.Add(new SelectListItem() { Text = isActiveConfig[i].VALUE, Value = isActiveConfig[i].ID, Selected = (isActiveConfig[i].ID == is_active) }); }

            return typeList;
        }

        private static Dictionary<int, SCONFIG> isActiveConfig = new Dictionary<int, SCONFIG>() 
        {
            {0, new SCONFIG(){ ID = @"Y", VALUE = @"生效"}}, 
            {1, new SCONFIG(){ ID = @"N", VALUE = @"失效"}}
        };

        /// <summary>
        /// 取得設定檔
        /// </summary>
        /// <param name="mapping_group">指定GROUP</param>
        /// <param name="code_type">指定TYPE</param>
        /// <param name="value">預設value</param>
        /// <returns></returns>
        public List<SelectListItem> GetGmCodeMapping(string mapping_group, string code_type, string value)
        {
            List<SelectListItem> itemList = new List<SelectListItem>();
            itemList.AddRange(new SelectList(gmCodeMappginManager.FindData(mapping_group, code_type, ""), "INPUT_VALUE", "OUTPUT_DESC", value));
            
            return itemList;
        }

        #endregion

        #region 特約機構維護

        #region 清單

        public ActionResult MerchantList(string group_id, string errMsg)
        {
            if (string.IsNullOrEmpty(errMsg))
            { ViewBag.hasError = false; }
            else
            {
                ViewBag.hasError = true;
                ViewBag.errMsg = errMsg.Split(';');
            }
            ViewBag.GROUP = this.GetMerchantGroup(group_id);
            ViewBag.MERCHANT = this.GetMerchantStoreByGroup((string.IsNullOrWhiteSpace(group_id)) ? "NULL" : group_id, "");
            ViewBag.MERCHANT_LIST = this.GetMerchant_MaintainStoreData((group_id) ?? "", "");
            
            return View();
        }

        [HttpPost]
        public ActionResult MerchantList(FormCollection value)
        {
            ViewBag.hasError = false;
            ViewBag.GROUP = this.GetMerchantGroup(value["MercStore_Group"]);
            ViewBag.MERCHANT = this.GetMerchantStoreByGroup(value["MercStore_Group"], value["MercStore_Merchant"]);
            ViewBag.MERCHANT_LIST = this.GetMerchant_MaintainStoreData(value["MercStore_Group"], value["MercStore_Merchant"]);
            
            return View();
        }

        public ActionResult MerchantDelete(int id)
        {
            List<string> listError = new List<string>();
            string group_id = "";
            string merchant_no = "";

            try
            {
                Domain.GmMerchantStore merchant = new Domain.GmMerchantStore()
                {
                    ID = id,
                    UPDATE_USER = User.Identity.Name,
                    UPDATE_TIME = DateTime.Now.ToString("yyyyMMddHHmmss")
                };

                DataTable result = gmMerchantStoreManager.FindData(merchant);
                if (result.Rows.Count > 0)
                { 
                    group_id = "" + result.Rows[0]["GROUP_ID"]; 
                    merchant_no = "" + result.Rows[0]["MERCHANT_NO"]; 

                    gmMerchantStoreManager.Delete(merchant);

                    return RedirectToAction("MerchantList", new { group_id = group_id });
                }
                else
                { listError.Add(@"找不到特約機構"); }
                
            }
            catch (Exception ex)
            { listError.Add(ex.Message); }

            return RedirectToAction("MerchantList", new { group_id = group_id, errMsg = string.Join(";", listError.ToArray()) });
        }

        #endregion

        #region 新增

        public ActionResult MerchantCreate()
        {
            ViewBag.hasError = false;
            ViewBag.GROUP = this.GetMerchantGroup("");
            ViewBag.MERCHANT = this.GetMerchantByGroup("NULL", "NULL");
            
            return View();
        }

        [HttpPost]
        public ActionResult MerchantCreate(FormCollection value)
        {
            List<string> listError = new List<string>();

            try
            {
                if (ModelState.IsValid)
                {
                    Domain.GmMerchantStore merchant = new Domain.GmMerchantStore() 
                    {
                        MERCHANT_NO = value["Merc_Merchant"], 
                        CREATE_USER = User.Identity.Name, 
                        CREATE_TIME = DateTime.Now.ToString("yyyyMMddHHmmss"), 
                        IS_DEL = false
                    };

                    if (value["Merc_Merchant"].Replace(@"--請選擇--", "") == "")
                    { listError.Add(@"請選擇特約機構"); }
                    else
                    {
                        DataTable result = gmMerchantStoreManager.FindData(merchant);
                        if (result.Rows.Count == 0)
                        {
                            gmMerchantStoreManager.Insert(merchant);

                            return RedirectToAction("MerchantList");
                        }
                        else
                        { listError.Add(@"此特約機構已設定"); }
                    }
                }
            }
            catch (Exception ex)
            { listError.Add(ex.Message); }

            string group_id = (string.IsNullOrWhiteSpace(value["Merc_Group"])) ? "NULL" : value["Merc_Group"];
            var merchants = this.GetMerchantByGroup(group_id, value["Merc_Merchant"]);
            var merchantStore = this.GetMerchantStoreByGroup(group_id, value["Merc_Merchant"]);
            merchantStore.ForEach(x => 
            {
                merchants.RemoveAll(m => m.Text == x.Text);
            });
            ViewBag.hasError = true;
            ViewBag.errMsg = listError.ToArray();
            ViewBag.GROUP = this.GetMerchantGroup(value["Merc_Group"]);
            ViewBag.MERCHANT = merchants;

            return View();
        }

        #endregion

        #endregion

        #region 門市主檔維護

        #region 清單

        public ActionResult StoreList(string group_id, string merchant_no, string errMsg)
        {
            string merchant_name = "";
            DataTable result = gmMerchantStoreManager.FindData(new GmMerchantStore() { MERCHANT_NO = merchant_no });
            if (string.IsNullOrEmpty(errMsg))
            { ViewBag.hasError = false; }
            else
            { 
                ViewBag.hasError = true;
                ViewBag.errMsg = errMsg.Split(';');
            }
            if (result.Rows.Count > 0)
            { ViewBag.STORE_TYPE = "" + result.Rows[0]["STORE_TYPE"]; }
            else
            { ViewBag.STORE_TYPE = ""; }
            ViewBag.GROUP = this.GetMerchantGroup(group_id);
            ViewBag.GROUP_ID = group_id;
            ViewBag.STORE_LIST = this.GetStoreData(merchant_no, ViewBag.STORE_TYPE, out merchant_name);
            ViewBag.MERCHANT = this.GetMerchantStoreByGroup((string.IsNullOrWhiteSpace(group_id)) ? "NULL" : group_id, merchant_no);
            ViewBag.MERCHANT_NO = merchant_no;
            ViewBag.MERCHANT_NAME = merchant_name;

            return View();
        }

        [HttpPost]
        public ActionResult StoreList(FormCollection value)
        {
            string group_id = value["MercStore_Group"];
            string merchant_no = value["MercStore_Merchant"];
            string merchant_name = "";
            DataTable result = gmMerchantStoreManager.FindData(new GmMerchantStore() { MERCHANT_NO = merchant_no });

            if (result.Rows.Count > 0)
            { ViewBag.STORE_TYPE = "" + result.Rows[0]["STORE_TYPE"]; }
            else
            { ViewBag.STORE_TYPE = ""; }
            ViewBag.hasError = false;
            ViewBag.GROUP = this.GetMerchantGroup(group_id);
            ViewBag.GROUP_ID = group_id;
            ViewBag.STORE_LIST = this.GetStoreData(merchant_no, ViewBag.STORE_TYPE, out merchant_name);
            ViewBag.MERCHANT = this.GetMerchantStoreByGroup(group_id, merchant_no);
            ViewBag.MERCHANT_NO = merchant_no;
            ViewBag.MERCHANT_NAME = merchant_name;

            return View();
        }

        public ActionResult StoreDelete(string merchant_no, string store_no)
        {
            List<string> listError = new List<string>();
            string group_id = "";
            string merchant_name = "";
            string store_type = "";

            try
            {
                DataRow merchant = gmMerchantManager.FindMerchantData(merchant_no);
                if (merchant == null)
                { listError.Add(@"此特約機構不存在"); }
                else
                {
                    DataTable result = gmMerchantStoreManager.FindData(new GmMerchantStore() { MERCHANT_NO = merchant_no });
                    if (result.Rows.Count > 0)
                    { 
                        group_id = "" + result.Rows[0]["GROUP_ID"];
                        store_type = "" + result.Rows[0]["STORE_TYPE"]; 
                    }
                    if (store_type == "")
                    { listError.Add(@"此特約機構未提供維護門市主檔"); }
                    else
                    {
                        string schema = "" + merchant["MERCH_SCHEMAID"];
                        gmMerchantStoreManager.DeleteStore(schema, merchant_no, store_no);

                        return RedirectToAction("StoreList", new { group_id = group_id, merchant_no = merchant_no });
                    }
                }
            }
            catch (Exception ex)
            { listError.Add(ex.Message); }

            return RedirectToAction("StoreList", new { group_id = group_id, merchant_no = merchant_no, errMsg = string.Join(";", listError.ToArray()) });
        }

        #endregion

        #region 新增

        public ActionResult StoreCreate(string merchant_no)
        {
            if ("" + merchant_no == "")
            { return RedirectToAction("StoreList", new { group_id = "", merchant_no = "" }); }

            DataTable result = gmMerchantStoreManager.FindData(new GmMerchantStore() { MERCHANT_NO = merchant_no, IS_DEL = false });
            if (result.Rows.Count == 0)
            { return RedirectToAction("StoreList"); }
            else
            {
                string groupName = "";
                Domain.BmStoreM store = new BmStoreM()
                {
                    LINE_QTY = "",
                    OPERATE_TYPE = "",
                    LINE_TYPE = "",
                    CITY_TYPE = "",
                    LINE_NO_03 = "",
                    LINE_NO_04 = "",
                    TRANSFER_TYPE = "",
                    SUBSIDY_TYPE = "",
                    LINE_SI_NO = "",
                    STORE_TYPE = "" + result.Rows[0]["STORE_TYPE"]
                };

                SetPage_StoreCreate("" + result.Rows[0]["GROUP_ID"], merchant_no, "" + result.Rows[0]["MERCHANT_NAME"], @"新增", store, null);
            }

            return View();
        }

        [HttpPost]
        public ActionResult StoreCreate(Domain.BmStoreM store)
        {
            List<string> listError = new List<string>();
            string group_id = Request.Form["Hidden_Group_ID"];
            string merchant_no = store.MERCHANT_NO;
            string merchant_name = Request.Form["Hidden_Merchant_Name"];
            string store_type = "";
            string schema = "";
            
            try
            {
                if (ModelState.IsValid)
                {
                    DataTable result = gmMerchantStoreManager.FindData(new GmMerchantStore() { MERCHANT_NO = store.MERCHANT_NO, IS_DEL = false });
                    if (result.Rows.Count == 0)
                    { listError.Add(@"此特約機構不存在"); }
                    else
                    {
                        group_id = "" + result.Rows[0]["GROUP_ID"];
                        schema = "" + result.Rows[0]["MERCH_SCHEMAID"];
                        store_type = "" + result.Rows[0]["STORE_TYPE"];
                        merchant_name = "" + result.Rows[0]["MERCHANT_NAME"];

                        store.STORE_TYPE = store_type;
                        switch (store_type)
                        {
                            case @"RETAIL_1":
                            case @"TRACK_1":
                                SetStoreData_Retail_1(ref store);
                                break;
                            case @"TRAFFIC_1":
                                SetStoreData_Traffic_1(ref store);
                                break;
                            default:
                                throw new Exception(@"此特約機構未提供維護門市主檔");
                                break;
                        }

                        if (this.CheckInput_Store(schema, false, store, out listError))
                        {
                            if (!gmMerchantStoreManager.InsertStore(schema, store_type, store))
                            { throw new Exception(@"新增門市主檔發生錯誤"); }

                            return RedirectToAction("StoreList", new { group_id = group_id, merchant_no = store.MERCHANT_NO });
                        }
                    }
                }
            }
            catch (Exception ex)
            { listError.Add(ex.Message); }
            
            SetPage_StoreCreate(group_id, merchant_no, merchant_name, @"新增", store, listError.ToArray());

            return View(store);
        }

        #endregion

        #region 編輯

        public ActionResult StoreEdit(string merchant_no, string store_no)
        {
            if ("" + merchant_no == "" && "" + store_no == "")
            { return RedirectToAction("StoreList", new { group_id = "", merchant_no = "" }); }

            string group_id = "";
            string merchant_name = "";
            string store_type = "";
            BmStoreM store = new BmStoreM();
            DataTable result = gmMerchantStoreManager.FindData(new GmMerchantStore() { MERCHANT_NO = merchant_no, IS_DEL = false });
            if (result.Rows.Count > 0)
            {
                string schema = "" + result.Rows[0]["MERCH_SCHEMAID"];
                group_id = "" + result.Rows[0]["GROUP_ID"];
                merchant_name = "" + result.Rows[0]["MERCHANT_NAME"];
                store_type = "" + result.Rows[0]["STORE_TYPE"];
                DataTable resultStore = gmMerchantStoreManager.FindStoreData(schema, merchant_no, store_no);

                if (resultStore.Rows.Count > 0)
                {
                    store.MERCHANT_NO = "" + resultStore.Rows[0]["MERCHANT_NO"];
                    store.STO_NAME_LONG = "" + resultStore.Rows[0]["STO_NAME_LONG"];
                    store.STO_NAME_SHORT = "" + resultStore.Rows[0]["STO_NAME_SHORT"];
                    store.EFF_DATE_FROM = "" + resultStore.Rows[0]["EFF_DATE_FROM"];
                    store.EFF_DATE_TO = "" + resultStore.Rows[0]["EFF_DATE_TO"];
                    store.OPEN_DATE = "" + resultStore.Rows[0]["OPEN_DATE"];
                    store.SALES_DATE = "" + resultStore.Rows[0]["SALES_DATE"];
                    store.CLOSE_DATE = "" + resultStore.Rows[0]["CLOSE_DATE"];
                    store.ZIP = "" + resultStore.Rows[0]["ZIP"];
                    store.ADDRESS = "" + resultStore.Rows[0]["ADDRESS"];
                    store.TEL_AREA = "" + resultStore.Rows[0]["TEL_AREA"];
                    store.TEL_NO = "" + resultStore.Rows[0]["TEL_NO"];
                    if (store_type == @"TRAFFIC_1")
                    {
                        store.LINE_QTY = "" + resultStore.Rows[0]["LINE_QTY"];
                        store.OPERATE_TYPE = "" + resultStore.Rows[0]["OPERATE_TYPE"];
                        store.LINE_TYPE = "" + resultStore.Rows[0]["LINE_TYPE"];
                        store.CITY_TYPE = "" + resultStore.Rows[0]["CITY_TYPE"];
                        store.LINE_NO_03 = "" + resultStore.Rows[0]["LINE_NO_03"];
                        store.LINE_NO_04 = "" + resultStore.Rows[0]["LINE_NO_04"];
                        store.TRANSFER_TYPE = "" + resultStore.Rows[0]["TRANSFER_TYPE"];
                        store.SUBSIDY_TYPE = "" + resultStore.Rows[0]["SUBSIDY_TYPE"];
                        store.LINE_SI_NO = "" + resultStore.Rows[0]["LINE_SI_NO"];
                    }
                }
                else
                { return RedirectToAction("StoreList"); }
            }
            else
            { return RedirectToAction("StoreList"); }

            store.STORE_NO = store_no;
            store.STORE_TYPE = store_type;
            SetPage_StoreCreate(group_id, merchant_no, merchant_name, @"修改", store, null);

            return View("StoreCreate", store);
        }

        [HttpPost]
        public ActionResult StoreEdit(Domain.BmStoreM store)
        {
            List<string> listError = new List<string>();
            string group_id = Request.Form["Hidden_Group_ID"];
            string merchant_no = store.MERCHANT_NO;
            string merchant_name = Request.Form["Hidden_Merchant_Name"];
            
            try
            {
                if (ModelState.IsValid)
                {
                    DataTable result = gmMerchantStoreManager.FindData(new GmMerchantStore() { MERCHANT_NO = store.MERCHANT_NO, IS_DEL = false });
                    if (result.Rows.Count == 0)
                    { listError.Add(@"此特約機構不存在"); }
                    else
                    {
                        group_id = "" + result.Rows[0]["GROUP_ID"];
                        string schema = "" + result.Rows[0]["MERCH_SCHEMAID"];
                        string store_type = "" + result.Rows[0]["STORE_TYPE"];
                        merchant_name = "" + result.Rows[0]["MERCHANT_NAME"];

                        store.STORE_NO = ("" + store.STORE_NO).Replace(" ", "");
                        store.STO_NAME_LONG = ("" + store.STO_NAME_LONG).Replace(" ", "");
                        store.STO_NAME_SHORT = ("" + store.STO_NAME_SHORT).Replace(" ", "");
                        store.ZIP = ("" + store.ZIP == "") ? store.ZIP : store.ZIP.Replace(" ", "");
                        store.ADDRESS = ("" + store.ADDRESS == "") ? store.ADDRESS : store.ADDRESS.Replace(" ", "");
                        store.TEL_AREA = ("" + store.TEL_AREA == "") ? store.TEL_AREA : store.TEL_AREA.Replace(" ", "");
                        store.TEL_NO = ("" + store.TEL_NO == "") ? store.TEL_NO : store.TEL_NO.Replace(" ", "");
                        store.STORE_TYPE = store_type;
                        switch (store_type)
                        {
                            case @"RETAIL_1":
                            case @"TRACK_1":
                                SetStoreData_Retail_1(ref store);
                                break;
                            case @"TRAFFIC_1":
                                SetStoreData_Traffic_1(ref store);
                                break;
                            default:
                                throw new Exception(@"此特約機構未提供維護門市主檔");
                                break;
                        }
                        if (this.CheckInput_Store(schema, true, store, out listError))
                        {
                            if (!gmMerchantStoreManager.UpdateStore(schema, store_type, store))
                            { throw new Exception(@"修改門市主檔發生錯誤"); }

                            return RedirectToAction("StoreList", new { group_id = group_id, merchant_no = store.MERCHANT_NO });
                        }
                    }
                }
            }
            catch (Exception ex)
            { listError.Add(ex.Message); }

            store.STORE_TYPE = Request.Form["Hidden_Store_Layout"];
            SetPage_StoreCreate(group_id, merchant_no, merchant_name, @"修改", store, listError.ToArray());

            return View("StoreCreate", store);
        }

        #endregion

        private void SetPage_StoreCreate(string group_id, string merchant_no, string merchant_name, string action_type, Domain.BmStoreM store, string[] listError)
        {
            ViewBag.hasError = (listError != null);
            ViewBag.errMsg = listError;
            ViewBag.GROUP_ID = group_id;
            ViewBag.MERCHANT_NO = merchant_no;
            ViewBag.MERCHANT_NAME = merchant_name;
            ViewBag.STORE_NO = store.STORE_NO;
            ViewBag.DDL_LINE_QTY = this.GetGmCodeMapping("BUS_STORE", "LINE_QTY", store.LINE_QTY);
            ViewBag.DDL_OPERATE_TYPE = this.GetGmCodeMapping("BUS_STORE", "OPERATE_TYPE", store.OPERATE_TYPE);
            ViewBag.DDL_LINE_TYPE = this.GetGmCodeMapping("BUS_STORE", "LINE_TYPE", store.LINE_TYPE);
            ViewBag.DDL_CITY_TYPE = this.GetGmCodeMapping("BUS_STORE", "CITY_TYPE", store.CITY_TYPE);
            ViewBag.DDL_LINE_NO_03 = this.GetGmCodeMapping("BUS_STORE", "LINE_NO_03", store.LINE_NO_03);
            ViewBag.DDL_LINE_NO_04 = this.GetGmCodeMapping("BUS_STORE", "LINE_NO_04", store.LINE_NO_04);
            ViewBag.DDL_TRANSFER_TYPE = this.GetGmCodeMapping("BUS_STORE", "TRANSFER_TYPE", store.TRANSFER_TYPE);
            ViewBag.DDL_SUBSIDY_TYPE = this.GetGmCodeMapping("BUS_STORE", "SUBSIDY_TYPE", store.SUBSIDY_TYPE);
            ViewBag.DDL_LINE_SI_NO = this.GetGmCodeMapping("BUS_STORE", "LINE_SI_NO", store.LINE_SI_NO);
            ViewBag.STORE_TYPE = store.STORE_TYPE;
            ViewBag.ACTION_TYPE = action_type;
        }

        private void SetStoreData_Retail_1(ref Domain.BmStoreM store)
        {
            store.STORE_NO = ("" + store.STORE_NO).Replace(" ", "");
            store.STO_NAME_LONG = ("" + store.STO_NAME_LONG).Replace(" ", "");
            store.STO_NAME_SHORT = ("" + store.STO_NAME_SHORT).Replace(" ", "");
            store.EFF_DATE_FROM = (store.EFF_DATE_FROM) ?? DateTime.Today.AddDays(1).ToString("yyyyMMdd");
            store.EFF_DATE_TO = (store.EFF_DATE_TO) ?? @"99991231";
            store.ZIP = ("" + store.ZIP == "") ? store.ZIP : store.ZIP.Replace(" ", "");
            store.ADDRESS = ("" + store.ADDRESS == "") ? store.ADDRESS : store.ADDRESS.Replace(" ", "");
            store.TEL_AREA = ("" + store.TEL_AREA == "") ? store.TEL_AREA : store.TEL_AREA.Replace(" ", "");
            store.TEL_NO = ("" + store.TEL_NO == "") ? store.TEL_NO : store.TEL_NO.Replace(" ", "");
        }

        private void SetStoreData_Traffic_1(ref Domain.BmStoreM store)
        {
            store.STORE_NO = ("" + store.STORE_NO).Replace(" ", "");
            store.STO_NAME_LONG = ("" + store.STO_NAME_LONG).Replace(" ", "");
            store.STO_NAME_SHORT = ("" + store.STO_NAME_SHORT).Replace(" ", "");
            store.EFF_DATE_FROM = (store.EFF_DATE_FROM) ?? DateTime.Today.AddDays(1).ToString("yyyyMMdd");
            store.EFF_DATE_TO = (store.EFF_DATE_TO) ?? @"99991231";
            //store.OPEN_DATE = (store.OPEN_DATE) ?? store.EFF_DATE_FROM;
            store.SALES_DATE = store.OPEN_DATE;
            store.CLOSE_DATE = (store.CLOSE_DATE) ?? store.EFF_DATE_TO;
            store.ZIP = string.Empty;
            store.ADDRESS = string.Empty;
            store.TEL_AREA = string.Empty;
            store.TEL_NO = string.Empty;
            store.LINE_NO_04 = ("" + store.LINE_NO_04 == "") ? "" : store.LINE_NO_04;
            store.LICENSE_NO = string.Empty;
        }

        private bool CheckInput_Store(string schema, bool isUpdate, BmStoreM value, out List<string> err_msg)
        {
            err_msg = new List<string>();
            DataTable result = gmMerchantStoreManager.FindStoreData(schema, value.MERCHANT_NO, "");
            DataTable resultIM = gmMerchantStoreManager.FindImStoreData(value.MERCHANT_NO, "");
            DateTime? chkEffDateFrom = null, chkEffDateTo = null, chkOpenDate = null, chkCloseDate = null;
            DateTime chkDate;

            if (DateTime.TryParseExact(value.EFF_DATE_FROM, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out chkDate))
            { chkEffDateFrom = chkDate; }
            if (DateTime.TryParseExact(value.EFF_DATE_TO, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out chkDate))
            { chkEffDateTo = chkDate; }
            if (DateTime.TryParseExact(value.OPEN_DATE, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out chkDate))
            { chkOpenDate = chkDate; }
            if (DateTime.TryParseExact(value.CLOSE_DATE, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out chkDate))
            { chkCloseDate = chkDate; }

            // 門市已開幕則不得異動
            if (isUpdate)
            {
                DataTable resultStore = gmMerchantStoreManager.FindStoreData(schema, value.MERCHANT_NO, value.STORE_NO);
                if (resultStore.Rows.Count > 0)
                {
                    if (DateTime.Compare(DateTime.ParseExact("" + resultStore.Rows[0]["OPEN_DATE"], "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture), DateTime.Today) <= 0)
                    { err_msg.Add(@"此門市已開幕，主檔不得再做異動"); }
                }
            }

            // 門市代號 / 路線代碼
            int textLength = 8;
            string titleName = "門市代號";
            switch (value.STORE_TYPE)
            {
                case @"RETAIL_1":
                    titleName = @"門市代號";
                    textLength = 8;
                    break;
                case @"TRACK_1":
                    titleName = @"門市代號";
                    textLength = 3;
                    break;
                case @"TRAFFIC_1":
                    titleName = @"路線代碼";
                    textLength = 6;
                    break;
            }
            System.Text.RegularExpressions.Regex rgx = new System.Text.RegularExpressions.Regex(@"^[0-9]*$");
            if (!isUpdate)
            {
                if ("" + value.STORE_NO == "")
                { err_msg.Add(string.Format(@"請輸入{0}", titleName)); }
                else if (!rgx.IsMatch(value.STORE_NO) || value.STORE_NO.Length != textLength)
                { err_msg.Add(string.Format(@"{0}只能輸入{1}碼數字", titleName, textLength)); }
                else
                {
                    DataRow data = null;
                    if (chkEffDateFrom != null && chkEffDateTo != null)
                    {
                        data = (from DataRow row in resultIM.Rows
                                where "" + row["STORE_NO"] == value.STORE_NO
                                && ((int.Parse(value.EFF_DATE_FROM) >= int.Parse("" + row["EFF_DATE_FROM"]) && int.Parse(value.EFF_DATE_FROM) <= int.Parse("" + row["EFF_DATE_TO"]))
                                 || (int.Parse(value.EFF_DATE_TO) >= int.Parse("" + row["EFF_DATE_FROM"]) && int.Parse(value.EFF_DATE_TO) <= int.Parse("" + row["EFF_DATE_TO"])))
                                select row).FirstOrDefault();
                    }
                    else
                    {
                        data = (from DataRow row in result.Rows
                                where "" + row["STORE_NO"] == value.STORE_NO
                                select row).FirstOrDefault();
                    }
                    if (data != null)
                    { err_msg.Add(string.Format(@"{0}：{1}，已存在", titleName, value.STORE_NO)); }
                }
            }

            // 門市全名 / 路線全名
            textLength = 30;
            titleName = "門市全名";
            switch (value.STORE_TYPE)
            {
                case @"RETAIL_1":
                case @"TRACK_1":
                    titleName = @"門市全名";
                    break;
                case @"TRAFFIC_1":
                    titleName = @"路線全名";
                    break;
            }
            if ("" + value.STO_NAME_LONG == "")
            { err_msg.Add(string.Format(@"請輸入{0}", titleName)); }
            else if (System.Text.Encoding.GetEncoding("big5").GetBytes(value.STO_NAME_LONG).Length > textLength)
            { err_msg.Add(string.Format(@"{0}最多只能輸入{1}個中文字", titleName, textLength / 2)); }
            //else
            //{
            //    DataRow data = (from DataRow row in result.Rows
            //                    where "" + row["STO_NAME_LONG"] == value.STO_NAME_LONG
            //                       && "" + row["STORE_NO"] != value.STORE_NO
            //                    select row).FirstOrDefault();
            //    if (data != null)
            //    { err_msg.Add(string.Format(@"{0}：{1}，已存在", titleName, value.STO_NAME_LONG)); }
            //}


            // 門市簡稱 / 路線簡稱
            textLength = 10;
            titleName = "門市簡稱";
            switch (value.STORE_TYPE)
            {
                case @"RETAIL_1":
                case @"TRACK_1":
                    titleName = @"門市簡稱";
                    break;
                case @"TRAFFIC_1":
                    titleName = @"路線簡稱";
                    break;
            }
            if ("" + value.STO_NAME_SHORT == "")
            { err_msg.Add(string.Format(@"請輸入{0}", titleName)); }
            else if (System.Text.Encoding.GetEncoding("big5").GetBytes(value.STO_NAME_SHORT).Length > textLength)
            { err_msg.Add(string.Format(@"{0}最多只能輸入{1}個中文字", titleName, textLength / 2)); }
            //else
            //{
            //    DataRow data = (from DataRow row in result.Rows
            //                    where "" + row["STO_NAME_SHORT"] == value.STO_NAME_SHORT
            //                       && "" + row["STORE_NO"] != value.STORE_NO
            //                    select row).FirstOrDefault();
            //    if (data != null)
            //    { err_msg.Add(string.Format(@"{0}：{1}，已存在", titleName, value.STO_NAME_SHORT)); }
            //}

            // 生效起日
            if ("" + value.EFF_DATE_FROM == "")
            { err_msg.Add(@"請輸入生效起日"); }
            else if (chkEffDateFrom == null)
            { err_msg.Add(@"生效起日日期格式錯誤"); }
            else if (!isUpdate && DateTime.Compare((DateTime)chkEffDateFrom, DateTime.Today) < 0)
            { err_msg.Add(@"生效起日日期最快必須是今日"); }
            
            // 生效迄日
            if ("" + value.EFF_DATE_TO == "")
            { err_msg.Add(@"請輸入生效迄日"); }
            else if (chkEffDateTo == null)
            { err_msg.Add(@"生效迄日日期格式錯誤"); }
            else if (!isUpdate && DateTime.Compare((DateTime)chkEffDateTo, DateTime.Today) < 0)
            { err_msg.Add(@"生效迄日日期最快必須是今日"); }
            else if (chkEffDateFrom != null && DateTime.Compare((DateTime)chkEffDateFrom, (DateTime)chkEffDateTo) > 0)
            { err_msg.Add(@"生效迄日日期不可在生效起日之前"); }

            // 開幕日
            if ("" + value.OPEN_DATE == "")
            { err_msg.Add(@"請輸入開幕日"); }
            else if (chkOpenDate == null)
            { err_msg.Add(@"開幕日日期格式錯誤"); }
            else if (DateTime.Compare((DateTime)chkOpenDate, DateTime.Today) <= 0)
            { err_msg.Add(@"開幕日日期最快必須是明日"); }

            // 可販售日
            if (value.STORE_TYPE == @"RETAIL_1" || value.STORE_TYPE == @"TRACK_1")
            {
                if ("" + value.SALES_DATE == "")
                { err_msg.Add(@"請輸入可販售日"); }
                else if (!DateTime.TryParseExact(value.SALES_DATE, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out chkDate))
                { err_msg.Add(@"可販售日日期格式錯誤"); }
            }

            // 關店日
            if ("" + value.CLOSE_DATE == "")
            { err_msg.Add(@"請輸入關店日"); }
            else if (chkCloseDate == null)
            { err_msg.Add(@"關店日日期格式錯誤"); }
            else if (chkOpenDate != null && DateTime.Compare((DateTime)chkOpenDate, (DateTime)chkCloseDate) > 0)
            { err_msg.Add(@"關店日日期不可在開幕日之前"); }

            if (value.STORE_TYPE == @"TRAFFIC_1")
            {
                // 路線類別
                if ("" + value.OPERATE_TYPE == "")
                { err_msg.Add(@"請選擇路線類別"); }

                // 路線總段數
                if ("" + value.LINE_QTY == "")
                { err_msg.Add(@"請選擇路線總段數"); }
                else if ("" + value.OPERATE_TYPE == "03" && "" + value.LINE_QTY != "1")
                { err_msg.Add(@"路線類別為「里程」，則路線總段數必須為「一段票」"); }

                // 主管機關
                if ("" + value.LINE_TYPE == "")
                { err_msg.Add(@"請選擇主管機關"); }

                // 市區/非市區判別
                if ("" + value.CITY_TYPE == "")
                { err_msg.Add(@"請選擇市區/非市區判別"); }

                // 捷運/台鐵轉乘判別
                if ("" + value.TRANSFER_TYPE == "")
                { err_msg.Add(@"請選擇捷運/台鐵轉乘判別"); }

                // 公車轉乘判別
                if ("" + value.LINE_NO_03 == "")
                { err_msg.Add(@"請選擇公車轉乘判別"); }

                // 路線分類
                // if ("" + value.LINE_NO_04 == "")
                // { err_msg.Add(@"請選擇路線分類"); }

                // 公路客運代碼判別
                if ("" + value.SUBSIDY_TYPE == "")
                { err_msg.Add(@"請選擇公路客運代碼判別"); }

                // 設備商代碼
                if ("" + value.LINE_SI_NO == "")
                { err_msg.Add(@"請選擇設備商代碼"); }
            }

            return (err_msg.Count == 0); 
        }

        #endregion

        #region 特約機構簡碼維護

        #region 清單

        public ActionResult MerchantTMList(string merch_tmid, string ibon_merchant_name, string errMsg)
        {
            string merchant_name = "";
            if (string.IsNullOrEmpty(errMsg))
            { ViewBag.hasError = false; }
            else
            {
                ViewBag.hasError = true;
                ViewBag.errMsg = errMsg.Split(';');
            }
            ViewBag.IBON_SHOW_TYPE = ibonShowTypeConfig;
            ViewBag.IS_ACTIVE = isActiveConfig;
            ViewBag.DDL_IS_ACTIVE = this.GetIsActive("");
            ViewBag.MERCHANT_LIST = (gmMerchTMManager.FindDataByFuzzy(new GmMerchTM() { MERCH_TMID = merch_tmid, IBON_MERCHANT_NAME = ibon_merchant_name })).AsEnumerable();
            
            return View();
        }

        [HttpPost]
        public ActionResult MerchantTMList(GmMerchTM value)
        {
            ViewBag.hasError = false;
            ViewBag.IBON_SHOW_TYPE = ibonShowTypeConfig;
            ViewBag.IS_ACTIVE = isActiveConfig;
            ViewBag.DDL_IS_ACTIVE = this.GetIsActive(value.IS_ACTIVE);
            ViewBag.MERCHANT_LIST = gmMerchTMManager.FindDataByFuzzy(value).AsEnumerable();

            return View();
        }

        public ActionResult MerchantTMDelete(string merchant_no)
        {
            List<string> listError = new List<string>();
            
            try
            {
                DataTable result = gmMerchTMManager.FindData(new GmMerchTM() { MERCHANT_NO = merchant_no });
                if (result.Rows.Count > 0)
                {
                    if ("" + result.Rows[0]["CAN_DELETE"] == "N")
                    { listError.Add(@"此特約機構已上線，不得再做異動"); }
                    else
                    {
                        gmMerchTMManager.Delete(new GmMerchTM() { MERCHANT_NO = merchant_no });

                        return RedirectToAction("MerchantTMList");
                    }
                }
            }
            catch (Exception ex)
            { listError.Add(ex.Message); }

            return RedirectToAction("MerchantTMList", new { errMsg = string.Join(";", listError.ToArray()) });
        }

        #endregion

        #region 新增

        public ActionResult MerchantTMCreate()
        {
            ViewBag.hasError = false;
            ViewBag.DDL_IBON_SHOW_TYPE = this.GetIbonShowType("");
            ViewBag.DDL_IS_ACTIVE = this.GetIsActive("");
            ViewBag.ACTION_TYPE = @"新增";

            return View();
        }

        [HttpPost]
        public ActionResult MerchantTMCreate(Domain.GmMerchTM merch, FormCollection value)
        {
            List<string> listError = new List<string>();

            try
            {
                if (ModelState.IsValid)
                {
                    merch.IBON_SHOW_TYPE = ("" + merch.IBON_SHOW_TYPE == "") ? value["DD_IBON_SHOW_TYPE"] : merch.IBON_SHOW_TYPE;
                    if (this.CheckInput_MerchTM(merch, false, out listError))
                    {
                        merch.CREATE_USER = User.Identity.Name;
                        merch.CREATE_TIME = DateTime.Now.ToString("yyyyMMddHHmmss");

                        gmMerchTMManager.Insert(merch);

                        return RedirectToAction("MerchantTMList");
                    }
                }
            }
            catch (Exception ex)
            { listError.Add(ex.Message); }

            ViewBag.hasError = true;
            ViewBag.errMsg = listError.ToArray();
            ViewBag.DDL_IBON_SHOW_TYPE = this.GetIbonShowType(merch.IBON_SHOW_TYPE);
            ViewBag.DDL_IS_ACTIVE = this.GetIsActive(merch.IS_ACTIVE);
            ViewBag.ACTION_TYPE = @"新增";

            return View(merch);
        }

        #endregion

        #region 編輯

        public ActionResult MerchantTMEdit(string merchant_no)
        {
            if ("" + merchant_no == "")
            { return RedirectToAction("MerchantTMList"); }

            GmMerchTM merch = new GmMerchTM();
            DataTable result = gmMerchTMManager.FindData(new GmMerchTM() { MERCHANT_NO = merchant_no });
            if (result.Rows.Count == 0)
            { return RedirectToAction("MerchantTMList"); }
            else
            {
                merch.MERCHANT_NO = "" + result.Rows[0]["MERCHANT_NO"];
                merch.MERCH_TMID = "" + result.Rows[0]["MERCH_TMID"];
                merch.IBON_MERCHANT_NAME = "" + result.Rows[0]["IBON_MERCHANT_NAME"];
                merch.IBON_SHOW_TYPE = "" + result.Rows[0]["IBON_SHOW_TYPE"];
                merch.IS_ACTIVE = "" + result.Rows[0]["IS_ACTIVE"];
                merch.NOTE = "" + result.Rows[0]["NOTE"];
            }

            ViewBag.hasError = false;
            ViewBag.MERCHANT_NO = merchant_no;
            ViewBag.DDL_IBON_SHOW_TYPE = this.GetIbonShowType(merch.IBON_SHOW_TYPE);
            ViewBag.DDL_IS_ACTIVE = this.GetIsActive(merch.IS_ACTIVE);
            ViewBag.ACTION_TYPE = @"修改";
            ViewBag.CAN_DELETE = "" + result.Rows[0]["CAN_DELETE"];
            ViewBag.MERCH_TMID = "" + result.Rows[0]["MERCH_TMID"];

            return View("MerchantTMCreate", merch);
        }

        [HttpPost]
        public ActionResult MerchantTMEdit(GmMerchTM merch, FormCollection value)
        {
            List<string> listError = new List<string>();

            try
            {
                if (ModelState.IsValid)
                {
                    merch.IBON_SHOW_TYPE = ("" + merch.IBON_SHOW_TYPE == "") ? value["DD_IBON_SHOW_TYPE"] : merch.IBON_SHOW_TYPE;
                    if (this.CheckInput_MerchTM(merch, true, out listError))
                    {
                        merch.UPDATE_USER = User.Identity.Name;
                        merch.UPDATE_TIME = DateTime.Now.ToString("yyyyMMddHHmmss");

                        gmMerchTMManager.Update(merch);

                        return RedirectToAction("MerchantTMList");
                    }
                }
            }
            catch (Exception ex)
            { listError.Add(ex.Message); }

            ViewBag.hasError = true;
            ViewBag.errMsg = listError.ToArray();
            ViewBag.MERCHANT_NO = merch.MERCHANT_NO;
            ViewBag.DDL_IBON_SHOW_TYPE = this.GetIbonShowType(merch.IBON_SHOW_TYPE);
            ViewBag.DDL_IS_ACTIVE = this.GetIsActive(merch.IS_ACTIVE);
            ViewBag.ACTION_TYPE = @"修改";
            ViewBag.CAN_DELETE = value["Hidden_CanDelete"];
            ViewBag.MERCH_TMID = merch.MERCH_TMID;

            return View("MerchantTMCreate", merch);
        }

        #endregion

        private bool CheckInput_MerchTM(GmMerchTM value, bool isUpdate, out List<string> err_msg)
        {
            err_msg = new List<string>();
            bool hasMaster = false;
            
            // 特約機構是否存在於主檔，存在則不得異動
            if (isUpdate)
            {
                DataTable result = gmMerchTMManager.FindData(new GmMerchTM() { MERCHANT_NO = value.MERCHANT_NO });
                if (result.Rows.Count > 0)
                {
                    if ("" + result.Rows[0]["CAN_DELETE"] == "N")
                    { 
                        err_msg.Add(@"此特約機構已上線，不得再做異動");
                        hasMaster = true;
                    }
                }
            }

            if (!hasMaster)
            {
                // 特約機構代號
                // 規則：
                // (1) 可輸入英文、數字
                // (2) 長度需要 8碼
                // (3) 不可重複
                int textLength = 8;
                System.Text.RegularExpressions.Regex rgx = new System.Text.RegularExpressions.Regex(@"^[A-Za-z0-9]+$");
                if ("" + value.MERCHANT_NO == "")
                { err_msg.Add(@"請輸入特約機構代號"); }
                else if (!rgx.IsMatch(value.MERCHANT_NO) || value.MERCHANT_NO.Length != textLength)
                { err_msg.Add(string.Format(@"特約機構代號只能輸入{0}碼英數字", textLength)); }
                else if (!isUpdate)
                {
                    DataTable result = gmMerchTMManager.FindData(new GmMerchTM() { MERCHANT_NO = value.MERCHANT_NO });
                    if (result.Rows.Count > 0)
                    { err_msg.Add(string.Format(@"特約機構代號：{0}，已存在", value.MERCHANT_NO)); }
                }

                // 簡碼
                // 規則：
                // (1) 可輸入英文、數字
                // (2) 長度只能 3碼
                // (3) 可重複，但要提醒
                textLength = 3;
                if ("" + value.MERCH_TMID == "")
                { err_msg.Add(@"請輸入簡碼"); }
                else if (!rgx.IsMatch(value.MERCH_TMID) || value.MERCH_TMID.Length != textLength)
                { err_msg.Add(string.Format(@"簡碼只能輸入{0}碼英數字", textLength)); }

                // ibon顯示名稱
                if ("" + value.IBON_MERCHANT_NAME == "")
                { err_msg.Add(@"請輸入ibon顯示名稱"); }

                // ibon顯示類型
                if ("" + value.IBON_SHOW_TYPE == "")
                { err_msg.Add(@"請選擇ibon顯示類型"); }

                // 狀態
                if ("" + value.IS_ACTIVE == "")
                { err_msg.Add(@"請選擇狀態"); }
            }

            return (err_msg.Count == 0); 
        }

        [HttpPost]
        public JsonResult CheckMerchTM(string old_merchant_no, string value)
        {
            if (value == "")
            { return this.Json(""); }

            DataTable result = gmMerchTMManager.FindData(new GmMerchTM() { MERCH_TMID = value });
            DataRow data = (from DataRow row in result.Rows
                            where "" + row["MERCHANT_NO"] != old_merchant_no
                            select row).FirstOrDefault();

            if (data != null)
            { return this.Json(new GmMerchTM() { MERCH_TMID = value, IBON_MERCHANT_NAME = "" + data["IBON_MERCHANT_NAME"], IBON_SHOW_TYPE = "" + data["IBON_SHOW_TYPE"] }); }
            else
            { return this.Json(""); }
        }

        #endregion
    }
}