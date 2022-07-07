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
    
    public class GmAccountController : BaseController
    {
        public GmAccountManager GmAccountManager { get; set; }
        public GmMerchantManager GMManager { get; set; }
        //
        public GmAccountController()
        {
            GmAccountManager = new GmAccountManager();
            GMManager = new GmMerchantManager();
        }
        // GET: /GmAccount/
        /// <summary>
        /// 找群組內所有的特約機構
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        public List<SelectListItem> SetAllbyGroup(string group)
        {
            List<SelectListItem> parkings = new List<SelectListItem>();
            parkings.AddRange(new SelectList(GMManager.FindAllbyGroup(group), "MerchantNo", "MerchantName", group));
            parkings.Sort((x, y) => { return x.Text.CompareTo(y.Text); }); //選單排序20190812,工作底稿only,其他報表選單不變
            parkings.Add(new SelectListItem() { Text = "ALL", Value = "ALL", Selected = false });
            return parkings;

        }
        public List<SelectListItem> SetGroup(string group)
        {
            List<SelectListItem> groups = new List<SelectListItem>();
            groups.AddRange(new SelectList(GMManager.FindAllGroup(), "MerchantNo", "MerchantName", group));
            groups.Add(new SelectListItem() { Text = "ALL", Value = "ALL", Selected = false });
            return groups;

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
        public List<SelectListItem> SetINVOICE_RULE()
        {
            List<SelectListItem> groups = new List<SelectListItem>();
            GmAccountDAO GAC = new GmAccountDAO();
            groups.AddRange(new SelectList(GAC.FindINVOICE_RULE(), "MerchantNo", "MerchantName", "SETGROUP"));

            return groups;

        }
        public List<SelectListItem> SetSET_GROUP_M()
        {
            List<SelectListItem> groups = new List<SelectListItem>();
            GmAccountDAO GAC = new GmAccountDAO();
            groups.AddRange(new SelectList(GAC.FindSET_GROUP_M(), "MerchantNo", "MerchantName", "SETGROUP"));

            return groups;

        }
        public List<SelectListItem> SetSETTLE_RULE()
        {
            List<SelectListItem> groups = new List<SelectListItem>();
            GmAccountDAO GAC = new GmAccountDAO();
            groups.AddRange(new SelectList(GAC.FindSETTLE_RULE(), "MerchantNo", "MerchantName", "SETGROUP"));

            return groups;

        }
        public List<SelectListItem> SetSOURCE_RULE()
        {
            List<SelectListItem> groups = new List<SelectListItem>();
            GmAccountDAO GAC = new GmAccountDAO();
            groups.AddRange(new SelectList(GAC.FindSOURCE_RULE(), "MerchantNo", "MerchantName", "SETGROUP"));

            return groups;

        }


        public List<SelectListItem> SetSHOW_FLG()
        {
            List<SelectListItem> groups = new List<SelectListItem>();
            GmAccountDAO GAC = new GmAccountDAO();
            groups.AddRange(new SelectList(GAC.FindSHOW_FLG(), "MerchantNo", "MerchantName", "SETGROUP"));

            return groups;

        }
        public List<SelectListItem> SetREM_TYPE()
        {
            List<SelectListItem> groups = new List<SelectListItem>();
            GmAccountDAO GAC = new GmAccountDAO();
            groups.AddRange(new SelectList(GAC.FindREM_TYPE(), "MerchantNo", "MerchantName", "SETGROUP"));

            return groups;

        }
        public List<SelectListItem> SetORDER_NO()
        {
            List<SelectListItem> groups = new List<SelectListItem>();
            GmAccountDAO GAC = new GmAccountDAO();
            groups.AddRange(new SelectList(GAC.FindORDER_NO(), "MerchantNo", "MerchantName", "SETGROUP"));

            return groups;

        }

        public List<SelectListItem> SetDATE_RANGE()
        {
            List<SelectListItem> groups = new List<SelectListItem>();
            GmAccountDAO GAC = new GmAccountDAO();
            groups.AddRange(new SelectList(GAC.FindDATE_RANGE(), "MerchantNo", "MerchantName", "SETGROUP"));
            return groups;

        }
        public List<SelectListItem> SetINVOICE_RULE_RANK()
        {
            List<SelectListItem> groups = new List<SelectListItem>();
            GmAccountDAO GAC = new GmAccountDAO();
            groups.AddRange(new SelectList(GAC.FindINVOICE_RULE_RANK(), "MerchantNo", "MerchantName", "SETGROUP"));
            return groups;

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
        [CustomAuthorize(AccessLevel = "System,Accounting")]
        public ActionResult GMACT_190501()
        {
            string cpt_date = DateTime.Now.ToString("yyyyMMdd");
            string no1 = "001"; //預設值
            string no2 = "0010"; //預設值

            DataTable dt = null;



            if (Request.Form["searchConfirm"] != null || Request.Form["ExportExcel"] != null)
            {
                cpt_date = Request.Form["settleDate"];
                no1 = Request.Form["Number1"];
                no2 = Request.Form["Number2"];
            }
            ViewBag.SettleDate = cpt_date;
            ViewBag.Number1 = no1;
            ViewBag.Number2 = no2;

            ViewBag.RepName = "每日特約機構交易表(工作底稿)";
            if (Request.Form["searchConfirm"] != null || Request.Form["ExportExcel"] != null)
            {
                dt = GmAccountManager.GmAccount190501(cpt_date, no1, no2);
            }
            if (Request.Form["searchConfirm"] != null)
            {
                
                return View(dt);
            }
            else if (Request.Form["ExportExcel"] != null)
            {
                MemoryStream ms = new MemoryStream();

                StreamWriter outStream = new StreamWriter(ms, System.Text.Encoding.GetEncoding(950));
                //寫資料至記憶體
                string temp = "";
                for (int int_Index = 0; int_Index < dt.Rows.Count; int_Index++)
                {
                    temp = String.Join("\",\"", dt.Rows[int_Index].ItemArray);
                    temp = "\"" + temp + "\"";
                    outStream.WriteLine(temp);

                }
                outStream.Flush();
                outStream.Close();
                outStream.Dispose();
                ms.Close();
                ms.Dispose();


                return File(ms.ToArray(), "application/txt", ViewBag.RepName + DateTime.Now.ToString("yyyyMMddhhmmss") + @".dat");
            }
            else return View(new DataTable());
        }
        [CustomAuthorize(AccessLevel = "System,Accounting")]
        public ActionResult Index()
        {
            string merchant = "";
            string group = "";
            string item = "";
            ViewBag.RepName = "工作底稿主檔維護";
            DataTable dt = new DataTable();
            GmAccountDAO ga = new GmAccountDAO();

            if (Request.Form["searchConfirm"] != null || Request.Form["ExportExcel"] != null || Request.Form["ExportAll"] != null)
            {


                group = Request.Form["group"];
                item = Request.Form["items"];
                merchant = Request.Form["items"];
            }
            //ViewBag.Merchant = SetMerchantDropDown(merchantNo);
            ViewBag.RepName = "工作底稿主檔維護";

            ViewBag.MERCHANT = SetGroup(group);
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

            if (Request.Form["searchConfirm"] != null || Request.Form["ExportExcel"] != null)
            {
                List<string> mNoList = new List<string>();
                if (group == "ALL")
                {
                    mNoList = new GmMerchantTypeDAO().FindAll();

                    foreach (string mNo in mNoList)
                    { dt.Merge(ga.GMACT_INDEX(mNo)); }
                }
                //else if (retail == "ALL" || bus == "ALL" || bike == "ALL" || track == "ALL" || parking == "ALL" || outsourcing == "ALL")
                else if (item == "ALL")
                {
                    mNoList = new GmMerchantTypeDAO().FindGroupNotMstore(group);
                    foreach (string mNo in mNoList)
                    { dt.Merge(ga.GMACT_INDEX(mNo)); }
                }
                else if (merchant != "")
                {
                    dt = ga.GMACT_INDEX(merchant);
                }
                else { dt = new DataTable(); }

                //排序
                if (dt.Rows.Count > 0)
                {
                    dt.DefaultView.Sort = "MERCHANT_NO_ACT ";
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


                    ExcelRange r = ws.Cells["A1:U1"];
                    r.Merge = true;
                    r.Value = ViewBag.RepName.ToString();


                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    r.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                    //r.Style.Font.Bold = true;
                    r.Style.Font.Size = 14;

                    int startRowNumber = 2;
                    var columns = new[] { "特約機構代碼", "AMS名稱", "門市代號對應值", "摘要欄位", "類別","撥款單位", "關係人",
                        "門市代號對應值(手續費)","開立方式(手續費)","關係人(手續費)","特店性質","特店簡稱","結算週期","委外-周期代碼","結算方式",
                        "手續費收款日","來源規則","預設開立(顯示/不顯示)","部門","專案代號","會計科目","單別代號"};

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
        public ActionResult Edit(string merchant)
        {
            ViewBag.RepName = "工作底稿主檔維護-編輯";
            DataTable dt = new DataTable();
            GmAccountDAO ga = new GmAccountDAO();
            dt = ga.GMACT_EDIT(merchant);
            ViewBag.MERCHANT_NO = dt.Rows[0]["MERCHANT_NO"].ToString();
            ViewBag.AMS_NAME = dt.Rows[0]["MERCHANT_NAME"].ToString();
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
            if (dt.Rows[0]["PAY_TYPE"].ToString().Equals("") || dt.Rows[0]["PAY_TYPE"].ToString() == null)
            {
                ViewBag.PAY_VALUES = "01";
            }
            else
            {
                ViewBag.PAY_VALUES = dt.Rows[0]["PAY_TYPE"].ToString();
            }
            ViewBag.GROUP = SetGroup3("");
            ViewBag.CLASS = SetCLASS("");
            ViewBag.PAY = SetPAYTYPE("");
            if (dt.Rows[0]["PAY_TYPE"].ToString().Equals("") || dt.Rows[0]["PAY_TYPE"].ToString() == null)
            {
                ViewBag.GROUP_VALUES = "N001";
            }
            else
            {
                ViewBag.GROUP_VALUES = dt.Rows[0]["SET_GROUP"].ToString();
            }
            //ViewBag.LOAD_DEBIT

            //手續費欄位
            ViewBag.MERCHANT_NO_ACT_M = dt.Rows[0]["MERCHANT_NO_ACT_M"].ToString();
            ViewBag.INVOICE_RULE_VALUE = dt.Rows[0]["INVOICE_RULE"].ToString();
            ViewBag.SET_GROUP_M_VALUE = dt.Rows[0]["SET_GROUP_M"].ToString();
            ViewBag.MERC_GROUP = dt.Rows[0]["MERC_GROUP"].ToString();
            ViewBag.MERCHANT_STNAME = dt.Rows[0]["MERCHANT_STNAME"].ToString();
            ViewBag.CONTRACT_PREIOD = dt.Rows[0]["CONTRACT_PREIOD"].ToString();
            ViewBag.SETTLE_RULE_VALUE = dt.Rows[0]["SETTLE_RULE"].ToString();
            ViewBag.FEE_DAY = dt.Rows[0]["FEE_DAY"].ToString();
            ViewBag.SOURCE_RULE_VALUE = dt.Rows[0]["SOURCE_RULE"].ToString();
            ViewBag.SHOW_FLG_VALUE = dt.Rows[0]["SHOW_FLG"].ToString();
            ViewBag.DEPARTMENT = dt.Rows[0]["DEPARTMENT"].ToString();
            ViewBag.PROJECT_NO = dt.Rows[0]["PROJECT_NO"].ToString();
            ViewBag.ACCOUNTING = dt.Rows[0]["ACCOUNTING"].ToString();
            ViewBag.ORDER_NO_VALUE = dt.Rows[0]["ORDER_NO"].ToString();
            ViewBag.REM_TYPE_VALUE = dt.Rows[0]["REM_TYPE"].ToString();

            ViewBag.INVOICE_RULE = SetINVOICE_RULE();
            ViewBag.SET_GROUP_M = SetSET_GROUP_M();
            ViewBag.SETTLE_RULE = SetSETTLE_RULE();
            ViewBag.SOURCE_RULE = SetSOURCE_RULE();
            ViewBag.SHOW_FLG = SetSHOW_FLG();
            ViewBag.REM_TYPE = SetREM_TYPE();
            ViewBag.ORDER_NO = SetORDER_NO();


            return View(dt);

        }
        [CustomAuthorize(AccessLevel = "System,Accounting")]
        public ActionResult Update()
        {
            string merchant = Request.Form["MERCHANT_NO"];
            string ams_name = Request.Form["AMS_NAME"];
            string merchant_no_act = Request.Form["MERCHANT_NO_ACT"];
            string merchant_note = Request.Form["MERCHANT_NOTE"];
            string class_type = Request.Form["classtype"];
            string pay_type = Request.Form["pay"];
            string set_group = Request.Form["GROUP"];
            //---------
            string merchant_no_act_m = Request.Form["MERCHANT_NO_ACT_M"];
            string invoice_rule = Request.Form["invoiceRule"];
            string set_group_m = Request.Form["setGroupM"];
            //string merc_group = Request.Form["MERC_GROUP"];
            string merchant_stname = Request.Form["MERCHANT_STNAME"];
            string settle_rule = Request.Form["settleRule"];
            string fee_day = Request.Form["FEE_DAY"];
            string source_rule = Request.Form["sourceRule"];
            string show_flg = Request.Form["showFlg"];
            string department = Request.Form["DEPARTMENT"];
            string project_no = Request.Form["PROJECT_NO"];
            string accounting = Request.Form["ACCOUNTING"];
            string order_no = Request.Form["orderNo"];
            string rem_type = Request.Form["remType"];






            if (Request.Form["searchConfirm"] != null)
            {
                GmAccountDAO g = new GmAccountDAO();
                if (g.CheckMerchant(merchant) == null)
                {
                    if (settle_rule =="D")
                    {
                        source_rule = "5";
                    }
                    g.GMACT_INSERT(merchant, merchant_no_act, merchant_note, class_type, pay_type, set_group,
                        merchant_no_act_m, invoice_rule, set_group_m, merchant_stname, settle_rule, fee_day,
                        source_rule, show_flg, department, project_no, accounting, order_no, rem_type,User.Identity.Name.ToString());
                }
                else
                {
                    if (settle_rule == "D")
                    {
                        source_rule = "5";
                    }
                    g.GMACT_UPDATE(merchant, merchant_no_act, merchant_note, class_type, pay_type, set_group,
                         merchant_no_act_m, invoice_rule, set_group_m, merchant_stname, settle_rule, fee_day,
                        source_rule, show_flg, department, project_no, accounting, order_no, rem_type,User.Identity.Name.ToString());
                }
                DataTable dt = g.GMACT_INDEX(merchant);
                return View(Index_1(merchant));
            }
            else { return View(); }

        }
        [CustomAuthorize(AccessLevel = "System,Accounting")]
        public DataTable Index_1(string merchant)
        {

            string group = "";
            string item = "";
            GmAccountDAO g = new GmAccountDAO();
            DataTable list = g.GMACT_ReIndex(merchant);

            ViewBag.RepName = "工作底稿主檔維護";
            ViewBag.MERCHANT = SetGroup(group);
            ViewBag.MERCHANT_NAME = list.Rows[0]["GROUP_ID"].ToString();
            ViewBag.ITEM = merchant;

            DataTable dt = g.GMACT_INDEX(merchant);
            return dt;

        }

        //日扣撥付產dat
        [CustomAuthorize(AccessLevel = "System,Accounting")]
        public ActionResult GMACT_200201()
        {
            string cpt_date = DateTime.Now.ToString("yyyyMMdd");
            string no1 = "002"; //預設值
            string no2 = "0010"; //預設值

            DataTable dt = null;



            if (Request.Form["searchConfirm"] != null || Request.Form["ExportExcel"] != null)
            {
                cpt_date = Request.Form["settleDate"];
                no1 = Request.Form["Number1"];
                no2 = Request.Form["Number2"];
            }
            ViewBag.SettleDate = cpt_date;
            ViewBag.Number1 = no1;
            ViewBag.Number2 = no2;

            ViewBag.RepName = "日扣手續費(工作底稿)";
            if (Request.Form["searchConfirm"] != null || Request.Form["ExportExcel"] != null)
            {
                dt = GmAccountManager.GmAccount200201(cpt_date, no1, no2);
            }
            if (Request.Form["searchConfirm"] != null)
            {

                return View(dt);
            }
            else if (Request.Form["ExportExcel"] != null)
            {
                MemoryStream ms = new MemoryStream();

                StreamWriter outStream = new StreamWriter(ms, System.Text.Encoding.GetEncoding(950));
                //寫資料至記憶體
                string temp = "";
                for (int int_Index = 0; int_Index < dt.Rows.Count; int_Index++)
                {
                    temp = String.Join("\",\"", dt.Rows[int_Index].ItemArray);
                    temp = "\"" + temp + "\"";
                    outStream.WriteLine(temp);

                }
                outStream.Flush();
                outStream.Close();
                outStream.Dispose();
                ms.Close();
                ms.Dispose();


                return File(ms.ToArray(), "application/txt", ViewBag.RepName + DateTime.Now.ToString("yyyyMMddhhmmss") + @".dat");
            }
            else return View(new DataTable());
        }
        //應收手續費
        [CustomAuthorize(AccessLevel = "System,Accounting")]
        public ActionResult GMACT_200601()
        {
            string cpt_date = DateTime.Now.ToString("yyyyMMdd");
            string yearMonth = DateTime.Now.AddMonths(-1).ToString("yyyyMM");
            string group = "";
            string date_range = "";
            string invoice_rule = "";
            DataTable dt = new DataTable();



            if (Request.Form["searchConfirm"] != null || Request.Form["ExportExcel"] != null || Request.Form["ExportDat"] != null)
            {
                cpt_date = Request.Form["settleDate"];
                yearMonth = Request.Form["yearMonth"];
                group = Request.Form["group"];
                date_range = Request.Form["dateRange"];
                invoice_rule = Request.Form["invoiceRule"];
                //no1 = Request.Form["Number1"];
                //no2 = Request.Form["Number2"];
            }


            string[] gArray = group.Split(',');
            ViewBag.GArray = gArray;
            ViewBag.SettleDate = cpt_date;
            ViewBag.YearMonth = yearMonth;
            ViewBag.GROUP = SetGroup("");
            ViewBag.GROUP_VALUE = group;
            ViewBag.DateRange = SetDATE_RANGE();
            ViewBag.DateRange_Value = date_range;
            ViewBag.InvoiceRule = SetINVOICE_RULE_RANK();
            ViewBag.InvoiceRule_Value = invoice_rule;



            //ViewBag.Number1 = no1;
            //ViewBag.Number2 = no2;

            ViewBag.RepName = "應收手續費";
            if (Request.Form["searchConfirm"] != null || Request.Form["ExportExcel"] != null || Request.Form["ExportDat"] != null)
            {
                GmAccountDAO gad = new GmAccountDAO();
                yearMonth = yearMonth + "01";
                //群組整合
                if (group == null)
                {
                    group = "";
                }
                if (!group.Equals("") && !group.Equals("ALL"))
                {
                    group = group.Replace(",", "','");
                    group = "'" + group + "'";
                }
                else if (group.Equals("ALL"))
                {
                    group = "GROUP_ID";
                }
                #region 清空暫存TABLE
                gad.GmAccount200601_Clear();
                #endregion

                #region 有值清分營收款
                List<string> source_1 = new GmAccountDAO().FindSOURCE_1(date_range, invoice_rule, group);
                foreach (string mNo in source_1)
                {
                    GmAccountManager.GmAccount200601_01(cpt_date, yearMonth, date_range, mNo, group, invoice_rule);
                }

                #endregion

                #region 依門市
                #endregion

                #region 高雄零值
                List<string> source_3 = new GmAccountDAO().FindSOURCE_3(date_range, invoice_rule, group);
                foreach (string mNo in source_3)
                {
                    GmAccountManager.GmAccount200601_03(cpt_date, yearMonth, date_range, mNo, group, invoice_rule);
                }



                #endregion

                #region 段次與里程
                #endregion





                #region 日扣
                //
                DataTable tmp_list = new DataTable();
                tmp_list = gad.FindOutSouringGroup();
                //tmp_list = GmAccountManager.GmAccount200601_05(cpt_date,yearMonth,date_range,)
                foreach (DataRow dr in tmp_list.Rows)
                {
                    string schema = dr["GROUP_ID"].ToString();
                    string ab_mno = dr["MERCHANT_NO"].ToString();
                    GmAccountManager.GmAccount200601_05(cpt_date, yearMonth, date_range, schema, ab_mno, group, invoice_rule);

                }
                #endregion

                #region 依筆數(總筆數)
                #endregion

                #region 依筆數(下車數)
                #endregion


                #region 撈資料
                if (invoice_rule.Equals("Y") || invoice_rule.Equals(""))
                {
                    dt = gad.GmAccount200601_Y(date_range);
                }
                else
                {
                    dt = gad.GmAccount200601_N(date_range);
                }




                #endregion



                //dt = GmAccountManager.GmAccount200201(cpt_date, no1, no2);
            }
            if (Request.Form["searchConfirm"] != null)
            {

                return View(dt);
            }
            //產出DAT
            else if (Request.Form["ExportDat"] != null)
            {
                MemoryStream ms = new MemoryStream();

                StreamWriter outStream = new StreamWriter(ms, System.Text.Encoding.GetEncoding(950));
                //寫資料至記憶體
                string temp = "";
                for (int int_Index = 0; int_Index < dt.Rows.Count; int_Index++)
                {
                    temp = String.Join("\",\"", dt.Rows[int_Index].ItemArray);
                    temp = "\"" + temp + "\"";
                    outStream.WriteLine(temp);

                }
                outStream.Flush();
                outStream.Close();
                outStream.Dispose();
                ms.Close();
                ms.Dispose();


                return File(ms.ToArray(), "application/txt", ViewBag.RepName + DateTime.Now.ToString("yyyyMMddhhmmss") + @".dat");
            }
            else if (Request.Form["ExportExcel"] != null)
            {
                using (ExcelPackage p = new ExcelPackage())
                {
                    ExcelWorksheet ws = p.Workbook.Worksheets.Add(ViewBag.RepName.ToString());

                    int startRow = 1;
                    //統編	門市代號	門市名稱	購貨金額	購貨取消金額	購貨淨額
                    var columns = new[] { "單據編號","日期","客戶代號","未稅","稅額","預計收款日",
                    "發票號碼","列號","類別","未稅","稅額","品名","會計科目","部門代號",
                    "預計兌現日","專案代號","備註","B2C統編","數量", "單別代號" };

                    for (int j = 1; j <= columns.Length; j++)
                    {
                        //設值為欄位名稱
                        ws.Cells[startRow, j].Value = columns[j - 1];
                        //設定樣式
                        ws.Cells[startRow, j].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center; //欄位置中
                        ws.Cells[startRow, j].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        ws.Cells[startRow, j].Style.Font.Bold = true;
                        ws.Cells[startRow, j].Style.Font.Size = 12;
                    }

                    for (int i = 1; i <= dt.Rows.Count; i++)
                    {

                        for (int j = 1; j <= dt.Columns.Count; j++)
                        {
                            ws.Cells[i + startRow, j].Value = dt.Rows[i - 1][j - 1].ToString();
                            ws.Cells[i + startRow, j].Style.Font.Size = 12;
                            ws.Cells[i + startRow, j].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right; //欄位靠右
                            ws.Cells[i + startRow, j].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                            if (j==12) ws.Cells[i + startRow, j].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left; //欄位靠左
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