using Business;
using Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace AMS.Controllers
{
    [CustomAuthorize(AccessLevel = "System,Accounting,Sales,PM,SystemSE,SalesManager")]
    public partial class ACInvoiceController:BaseController
    {
        #region 振興券專案請款維護-2021
        public ActionResult ACInvoiceReport_2021(string errMsg)
        {
            string COUPON_TP = "1";
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
            ViewBag.INVOICE_LIST = acInvoice2021Manager.GetACInvoiceReport(item[0].Value, COUPON_TP);
            ViewBag.COLUMN_WEEK = (item[0].Value == "T00004" ? "ICA" : "ICP");
            ViewBag.COUPON_TP = COUPON_TP;
            if (COUPON_TP == "1")
            {
                ViewBag.ch1 = "checked";
                ViewBag.ch2 = "";
            }
            else
            {
                ViewBag.ch1 = "";
                ViewBag.ch2 = "checked";
            }

            return View();
        }

        [HttpPost]
        public ActionResult ACInvoiceReport_2021(FormCollection value)
        {
            ViewBag.hasError = false;

            List<SelectListItem> item = this.GetMemberList();
            ViewBag.PAY_TYPE = value["PAY_TYPE"];
            ViewBag.PAY_TYPE_DDL = item;
            ViewBag.INVOICE_LIST = this.acInvoice2021Manager.GetACInvoiceReport(value["PAY_TYPE"], value["COUPON_TP"]);
            ViewBag.COLUMN_WEEK = (value["PAY_TYPE"] == "T00004" ? "ICA" : "ICP");
            ViewBag.COUPON_TP = value["COUPON_TP"];
            if (value["COUPON_TP"] == "1")
            {
                ViewBag.ch1 = "checked";
                ViewBag.ch2 = "";
            }
            else
            {
                ViewBag.ch1 = "";
                ViewBag.ch2 = "checked";
            }
            return View();
        }

        public ActionResult ACInvoiceEdit_2021(string member_id,string coupon_tp, string reward_start_date, string reward_end_date,string rowdata)
        {
            if (!string.IsNullOrWhiteSpace(rowdata))
            {
                ACInvoiceData2021 aCInvoiceData = JsonConvert.DeserializeObject<ACInvoiceData2021>(rowdata);

                if (string.IsNullOrWhiteSpace(aCInvoiceData.REWARD_START_DATE.ToString()) || string.IsNullOrWhiteSpace(aCInvoiceData.REWARD_END_DATE.ToString()))
                {
                    return RedirectToAction("ACInvoiceReport_2021");
                }

                List<SelectListItem> items = new List<SelectListItem>();
                items.Add(new SelectListItem { Text = "是", Value = "Y" });
                items.Add(new SelectListItem { Text = "否", Value = "N" });

                ViewBag.hasError = false;
                ViewBag.PAY_TYPE = (member_id == ACInvoiceManager.PayType.E00005.ToString()) ? "ICP" : "ICASH";
                ViewBag.MEMBER_ID = member_id;
                ViewBag.IS_APPLY_DDL = items;
                ViewBag.INVOICE_LIST = aCInvoiceData;
                ViewBag.REWARD_START_DATE = aCInvoiceData.REWARD_START_DATE;
                ViewBag.REWARD_END_DATE = aCInvoiceData.REWARD_END_DATE;
                ViewBag.IS_APPLY = aCInvoiceData.IS_APPLY;
                ViewBag.NOTE = aCInvoiceData.NOTE;
                ViewBag.COUPON_TP = coupon_tp;
            }
            else
            {
                return RedirectToAction("ACInvoiceReport_2021");
            }

            return View();
        }

        [HttpPost]
        public ActionResult ACInvoiceEdit_2021()
        {
            ACInvoice2021Manager.Response response = new ACInvoice2021Manager.Response();
            List<string> listError = new List<string>();
            string member_id = Request.Form["MEMBER_ID"];
            string coupon_tp = Request.Form["COUPON_TP"];
            string reward_start_date = Request.Form["REWARD_START_DATE"];
            string reward_end_date = Request.Form["REWARD_END_DATE"];
            string is_apply = Request.Form["is_apply_show"];
            string can_edit = Request.Form["CAN_EDIT"];
            string note = Request.Form["NOTE"];
            string rowdata = Request.Form["ROWDATA"];

            if (can_edit == "N")
            {
                return RedirectToAction("ACInvoiceReport_2021", "ACInvoice");
            }

            if (!string.IsNullOrWhiteSpace(rowdata))
            {
                ACInvoiceData2021 aCInvoiceData = JsonConvert.DeserializeObject<ACInvoiceData2021>(rowdata);

                if(is_apply == "N" && note.Trim() == "")
                {
                    listError.Add(@"若不請款，請輸入原因");
                }
                else
                {
                    response = this.acInvoice2021Manager.UpdateACInvoice(member_id, coupon_tp, aCInvoiceData.CPT_DATE.ToString("yyyyMMdd"), note, is_apply);

                    if (response.RtnCode == 1)
                    {
                        return RedirectToAction("ACInvoiceReport_2021", "ACInvoice");
                    }
                    else
                    {
                        listError.Add(response.RtnMsg);
                    }
                }

                ViewBag.hasError = (listError.Count > 0);
                ViewBag.errMsg = listError.ToArray();

                List<SelectListItem> items = new List<SelectListItem>();
                items.Add(new SelectListItem { Text = "是", Value = "Y" });
                items.Add(new SelectListItem { Text = "否", Value = "N" });

                ViewBag.PAY_TYPE = (member_id == ACInvoiceManager.PayType.E00005.ToString()) ? "ICP" : "ICASH";
                ViewBag.MEMBER_ID = member_id;
                ViewBag.IS_APPLY_DDL = items;
                ViewBag.INVOICE_LIST = aCInvoiceData;
                ViewBag.REWARD_START_DATE = reward_start_date;
                ViewBag.REWARD_END_DATE = reward_end_date;
                ViewBag.IS_APPLY = is_apply;
                ViewBag.NOTE = note;
            }
            else
            {
                return RedirectToAction("ACInvoiceReport_2021", "ACInvoice");
            }

            return View();
        }
        #endregion
    }
}