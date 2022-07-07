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
    public partial class ACInvoiceController : BaseController
    {
        public ACInvoiceManager acInvoiceManager { get; set; }
        public ACInvoice2021Manager acInvoice2021Manager { get; set; }
        
        public ACInvoiceController()
        {
            acInvoiceManager = new ACInvoiceManager();
            acInvoice2021Manager = new ACInvoice2021Manager();
        }

        #region UI元件

        /// <summary>
        /// 取得支付方式清單
        /// </summary>
        /// <param name="member_id">支付方式</param>
        /// <returns></returns>
        public List<SelectListItem> GetMemberList()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = "Icash 2.0", Value = "T00004" });
            items.Add(new SelectListItem { Text = "ICP", Value = "E00005" });

            return items;
        }

        #endregion

        #region 振興券專案請款維護

        #region 清單

        public ActionResult ACInvoiceReport(string errMsg)
        {
            if (string.IsNullOrEmpty(errMsg))
            { ViewBag.hasError = false; }
            else
            {
                ViewBag.hasError = true;
                ViewBag.errMsg = errMsg.Split(';');
            }
            List<SelectListItem> item = this.GetMemberList();
            ViewBag.PAY_TYPE = item[0].Value;
            ViewBag.PAY_TYPE_DDL = item;
            ViewBag.INVOICE_LIST = this.acInvoiceManager.GetACInvoiceReport(item[0].Value);
            ViewBag.COLUMN_WEEK = (item[0].Value == "T00004" ? "ICA" : "ICP");

            return View();
        }

        [HttpPost]
        public ActionResult ACInvoiceReport(FormCollection value)
        {
            ViewBag.hasError = false;

            List<SelectListItem> item = this.GetMemberList();
            ViewBag.PAY_TYPE = value["PAY_TYPE"];
            ViewBag.PAY_TYPE_DDL = item;
            ViewBag.INVOICE_LIST = this.acInvoiceManager.GetACInvoiceReport(value["PAY_TYPE"]);
            ViewBag.COLUMN_WEEK = (value["PAY_TYPE"] == "T00004" ? "ICA" : "ICP");
            return View();
        }

        #endregion

        #region 編輯

        public ActionResult ACInvoiceEdit(string member_id, string reward_start_date, string reward_end_date)
        {
            if ("" + reward_start_date == "" && "" + reward_end_date == "")
            { return RedirectToAction("ACInvoiceReport"); }

            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = "是", Value = "Y" });
            items.Add(new SelectListItem { Text = "否", Value = "N" });

            List<ACInvoiceData> report = this.acInvoiceManager.GetACInvoiceReport(member_id, reward_start_date, reward_end_date);
            DataRow data = this.acInvoiceManager.GetACInvoiceData(member_id, reward_start_date, reward_end_date);
            if (data == null)
            { return RedirectToAction("ACInvoiceReport"); }

            ViewBag.hasError = false;
            ViewBag.PAY_TYPE = (member_id == ACInvoiceManager.PayType.E00005.ToString()) ? "ICP" : "ICASH";
            ViewBag.MEMBER_ID = member_id;
            ViewBag.IS_APPLY_DDL = items;
            ViewBag.INVOICE_LIST = report;
            ViewBag.REWARD_START_DATE = reward_start_date;
            ViewBag.REWARD_END_DATE = reward_end_date;
            ViewBag.IS_APPLY = "" + data["IS_APPLY"];
            ViewBag.NOTE = "" + data["NOTE"];
            ViewBag.CAN_EDIT = (report[0].CAN_APPLY == "Y" && DateTime.Compare(DateTime.Today, report[0].REWARD_END_DATE) > 0 && (int)DateTime.Today.DayOfWeek <= 3) ? "Y" : "N";
            ViewBag.COLUMN_WEEK = (member_id == ACInvoiceManager.PayType.T00004.ToString() ? "ICA" : "ICP");

            return View();
        }

        [HttpPost]
        public ActionResult ACInvoiceEdit()
        {
            List<string> listError = new List<string>();
            string member_id = Request.Form["MEMBER_ID"];
            string reward_start_date = Request.Form["REWARD_START_DATE"];
            string reward_end_date = Request.Form["REWARD_END_DATE"];
            string is_apply = Request.Form["is_apply_show"];
            string can_edit = Request.Form["CAN_EDIT"];
            string note = Request.Form["NOTE"];

            if (can_edit == "N")
            { return RedirectToAction("ACInvoiceReport", "ACInvoice"); }
            DataRow data = this.acInvoiceManager.GetACInvoiceData(member_id, reward_start_date, reward_end_date);

            try
            {
                if (data == null)
                { throw new Exception(@"找不到請款資料"); }
                else if (is_apply == "N" && note.Trim() == "")
                { throw new Exception(@"若不請款，請輸入原因"); }
                else
                {
                    string cpt_date = "" + data["CPT_DATE"];
                    decimal invoice_amt = (decimal)data["INVOICE_AMT"];
                    this.acInvoiceManager.UpdateACInvoiceData(member_id, cpt_date, reward_start_date, reward_end_date, is_apply, note, User.Identity.Name, Request.UserHostAddress, invoice_amt);

                    return RedirectToAction("ACInvoiceReport", "ACInvoice");
                }
            }
            catch (Exception ex)
            { listError.Add(ex.Message); }

            ViewBag.hasError = (listError.Count > 0);
            ViewBag.errMsg = listError.ToArray();

            List<ACInvoiceData> report = this.acInvoiceManager.GetACInvoiceReport(member_id, reward_start_date, reward_end_date);
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = "是", Value = "Y" });
            items.Add(new SelectListItem { Text = "否", Value = "N" });

            ViewBag.PAY_TYPE = (member_id == ACInvoiceManager.PayType.E00005.ToString()) ? "ICP" : "ICASH";
            ViewBag.MEMBER_ID = member_id;
            ViewBag.IS_APPLY_DDL = items;
            ViewBag.INVOICE_LIST = report;
            ViewBag.REWARD_START_DATE = reward_start_date;
            ViewBag.REWARD_END_DATE = reward_end_date;
            ViewBag.IS_APPLY = is_apply;
            ViewBag.NOTE = note;
            ViewBag.CAN_EDIT = (report[0].CAN_APPLY == "Y" && DateTime.Compare(DateTime.Today, report[0].REWARD_START_DATE) > 0 && (int)DateTime.Today.DayOfWeek <= 3) ? "Y" : "N";
            ViewBag.COLUMN_WEEK = (member_id == ACInvoiceManager.PayType.T00004.ToString() ? "ICA" : "ICP");

            return View();
        }

        #endregion

        #endregion
  
    }
}