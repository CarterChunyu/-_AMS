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
using Common.Logging;

namespace AMS.Controllers
{

    public class GmAccountStoreController : BaseController
    {
        public GmAccountStoreManager GmA_StoreManager { get; set; }
        private static readonly ILog log = LogManager.GetLogger(typeof(GmAccountStoreController));
        //
        public GmAccountStoreController()
        {
            GmA_StoreManager = new GmAccountStoreManager();
        }
        // GET: /GmAccount_Store/
        /// <summary>
        /// 找群組內所有的門市
        /// 
        public List<SelectListItem> SetAllbyGroup(string group)
        {
            List<SelectListItem> actstore = new List<SelectListItem>();
            actstore.AddRange(new SelectList(GmA_StoreManager.FindAllBMStore(group), "StoreNo", "StoreName", group));
            actstore.Add(new SelectListItem() { Text = "ALL", Value = "ALL", Selected = false });
            return actstore;
        }
        public List<SelectListItem> SetGroup(string group)
        {
            List<SelectListItem> merchantstore = new List<SelectListItem>();
            merchantstore.AddRange(new SelectList(GmA_StoreManager.FindAllGroup_Store(), "MerchantNo", "MerchantName", group));
            return merchantstore;
        }
        public List<SelectListItem> SetGroup3(string group)
        {
            List<SelectListItem> groups = new List<SelectListItem>();
            GmAccountDAO GAC = new GmAccountDAO();
            groups.AddRange(new SelectList(GAC.FindSetSET_GROUP(), "MerchantNo", "MerchantName", "SETGROUP"));
            //groups.Add(new SelectListItem() { Text = "ALL", Value = "ALL", Selected = false });
            return groups;
        }
        public List<SelectListItem> SetCLASS(string group)
        {
            List<SelectListItem> groups = new List<SelectListItem>();
            GmAccountDAO GAC = new GmAccountDAO();
            groups.AddRange(new SelectList(GAC.FindCLASS(), "MerchantNo", "MerchantName", "SETGROUP"));
            //groups.Add(new SelectListItem() { Text = "ALL", Value = "ALL", Selected = false });
            return groups;
        }
        public List<SelectListItem> SetPAYTYPE(string group)
        {
            List<SelectListItem> groups = new List<SelectListItem>();
            GmAccountDAO GAC = new GmAccountDAO();
            groups.AddRange(new SelectList(GAC.FindPAYTYPE(), "MerchantNo", "MerchantName", "SETGROUP"));
            //groups.Add(new SelectListItem() { Text = "ALL", Value = "ALL", Selected = false });
            return groups;
        }
        [HttpPost]
        public JsonResult GetMerchantActStore(string groupName)
        {
            List<KeyValuePair<string, string>> items = new List<KeyValuePair<string, string>>();

            if (!string.IsNullOrWhiteSpace(groupName))
            {
                var actstores = this.SetAllbyGroup(groupName);
                if (actstores != null && actstores.Count() > 0)
                {
                    foreach (var actstore in actstores)
                    {
                        items.Add(new KeyValuePair<string, string>(
                            actstore.Value.ToString(),
                            actstore.Text));
                    }
                }
            }

            return this.Json(items);
        }

        [CustomAuthorize(AccessLevel = "System,Accounting")]
        public ActionResult Index_Store()
        {
            string merchant = "";
            string storeno = "";
            string item = "";
            ViewBag.RepName = "工作底稿主檔維護";
            DataTable dt = new DataTable();
            GmMerchantActStoreDAO ga = new GmMerchantActStoreDAO();

            if (Request.Form["searchConfirm"] != null || Request.Form["ExportExcel"] != null || Request.Form["ExportAll"] != null)
            {
                merchant = Request.Form["group"];
                item = Request.Form["items"];
                storeno = Request.Form["items"];
            }
            ViewBag.RepName = "門市主檔維護";

            ViewBag.MERCHANT = SetGroup(merchant);
            ViewBag.MERCHANT_NAME = merchant;
            ViewBag.ITEM = item;

            if (Request.Form["searchConfirm"] != null || Request.Form["ExportExcel"] != null)
            {
                List<BmStoreno> mNoList = new List<BmStoreno>();
                if (item == "ALL")
                {
                    mNoList = ga.FindAllBMStore(merchant);
                    foreach (var mNo in mNoList)
                    { dt.Merge(ga.GMACT_STORE_INDEX(merchant, mNo.StoreNo)); }
                }
                else if (storeno != "")
                {
                    dt = ga.GMACT_STORE_INDEX(merchant, storeno);
                }
                else { dt = new DataTable(); }

                //排序
                if (dt.Rows.Count > 0)
                {
                    dt.DefaultView.Sort = "STORE_NO";
                    dt = dt.DefaultView.ToTable();
                }
            }
            if (Request.Form["searchConfirm"] != null)
            {
                return View(dt);
            }
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

                    int startRowNumber = 2;
                    var columns = new[] { "特約機構代碼", "門市代碼", "門市名稱", "門市代號對應值", "摘要欄位", "類別", "撥款單位", "關係人" };

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
        [CustomAuthorize(AccessLevel = "System,Accounting")]
        public ActionResult Edit_Store(string merchant, string storeno)
        {
            ViewBag.RepName = "門市主檔維護-編輯";
            DataTable dt = new DataTable();
            GmMerchantActStoreDAO ga = new GmMerchantActStoreDAO();
            dt = ga.GMACT_STORE_EDIT(merchant, storeno);
            ViewBag.MERCHANT_NO = dt.Rows[0]["MERCHANT_NO"].ToString();
            ViewBag.MERCHANT_NAME = dt.Rows[0]["MERCHANT_NAME"].ToString();
            ViewBag.STORE_NO = dt.Rows[0]["STORE_NO"].ToString();
            ViewBag.MERCHANT_NAME = dt.Rows[0]["STO_NAME_LONG"].ToString();
            ViewBag.MERCHANT_NO_ACT = dt.Rows[0]["MERCHANT_NO_ACT"].ToString();
            ViewBag.MERCHANT_NOTE = dt.Rows[0]["MERCHANT_NOTE"].ToString();
            if (dt.Rows[0]["CLASS"].ToString().Equals("") || dt.Rows[0]["CLASS"].ToString() == null)
            {
                ViewBag.CLASS_VALUES = "01";
            }
            else
            {
                ViewBag.CLASS_VALUES = dt.Rows[0]["CLASS"].ToString();
            }
            //if (dt.Rows[0]["PAY_TYPE"].ToString().Equals("") || dt.Rows[0]["PAY_TYPE"].ToString() == null)
            //{
            //    ViewBag.PAY_VALUES = "01";
            //}
            //else
            //{
            //    ViewBag.PAY_VALUES = dt.Rows[0]["PAY_TYPE"].ToString();
            //}
            //if (dt.Rows[0]["PAY_TYPE"].ToString().Equals("") || dt.Rows[0]["PAY_TYPE"].ToString() == null)
            //{
            //    ViewBag.GROUP_VALUES = "N001";
            //}
            //else
            //{
            //    ViewBag.GROUP_VALUES = dt.Rows[0]["SET_GROUP"].ToString();
            //}
            //ViewBag.GROUP = SetGroup3("");
            ViewBag.CLASS = SetCLASS("");
            //ViewBag.PAY = SetPAYTYPE("");
            return View(dt);
        }
        [CustomAuthorize(AccessLevel = "System,Accounting")]
        public ActionResult Update_Store()
        {
            GmAccountActStore entity = new GmAccountActStore();
            entity.MERCHANT_NO = Request.Form["MERCHANT_NO"];
            entity.STORE_NO = Request.Form["STORE_NO"];
            try
            {
                entity.MERCHANT_NO_ACT = Request.Form["MERCHANT_NO_ACT"].Trim();
                entity.MERCHANT_NOTE = Request.Form["MERCHANT_NOTE"].Trim();
            }
            catch (Exception ex)
            {
                log.Debug(ex.Message);
            }
            entity.CLASS = Request.Form["classtype"];
            //entity.PAY_TYPE = Request.Form["pay"];
            //entity.SET_GRAOUP = Request.Form["GROUP"];

            if (Request.Form["searchConfirm"] != null)
            {
                GmMerchantActStoreDAO g = new GmMerchantActStoreDAO();

                if (g.CheckMerchantActStore(entity.MERCHANT_NO, entity.STORE_NO))
                {
                    g.GMACT_STORE_UPDATE(entity);
                }
                else
                {
                    g.GMACT_STORE_INSERT(entity);
                }
                DataTable dt = g.GMACT_STORE_INDEX(entity.MERCHANT_NO, entity.STORE_NO);
                return View(ReIndex(entity.MERCHANT_NO, entity.STORE_NO));
            }
            else { return View(); }
        }

        [CustomAuthorize(AccessLevel = "System,Accounting")]
        public DataTable ReIndex(string merchant, string store_no)
        {

            string group = "";
            string item = "";
            GmMerchantActStoreDAO g = new GmMerchantActStoreDAO();
            //DataTable list = g.GMACT_ReIndex(merchant);

            ViewBag.RepName = "門市主檔維護";
            ViewBag.MERCHANT = SetGroup(merchant);
            ViewBag.MERCHANT_NAME = merchant;
            ViewBag.ITEM = store_no;

            DataTable dt = g.GMACT_STORE_INDEX(merchant, store_no);
            return dt;

        }
    }
}