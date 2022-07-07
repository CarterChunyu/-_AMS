using AMS.Models;
using Business;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AMS.Controllers
{
    [CustomAuthorize(AccessLevel = "System,Accounting")]
    public class EntAcctController : BaseController
    {
        public AmEntAcctDManager amEntAcctDManager { get; set; }
        public AmAcctRoleManager amAcctRoleManager { get; set; }
        public AmChoiceManager choiceManager { get; set; }
        public AmRolesManager rolesManager { get; set; }
        public GmMerchantManager gmMerchantManager { get; set; }

        public EntAcctController()
        {
            amEntAcctDManager = new AmEntAcctDManager();
            amAcctRoleManager = new AmAcctRoleManager();
            choiceManager = new AmChoiceManager();
            rolesManager = new AmRolesManager();
            gmMerchantManager = new GmMerchantManager();
        }
        public ActionResult Index(string status)
        {
            ViewBag.Status = new SelectList(this.choiceManager.FindByCode("Status"), "Value", "Name", status);
            List<AmEntAcctD> list = this.amEntAcctDManager.FindByStatus(status);
            AmEntAcctDModel result = new AmEntAcctDModel();
            result.AmEntAccountDList = list;
            return View(result);
        }

        // 初始化DropDownList      
        List<SelectListItem> GetSelectItemF(bool dvalue = true)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            if (dvalue) { items.Insert(0, new SelectListItem { Text = "同時發送email及傳真通知", Value = "AL" }); }
            if (dvalue) { items.Insert(0, new SelectListItem { Text = "不發送入帳通知", Value = "AF" }); }
            if (dvalue) { items.Insert(0, new SelectListItem { Text = "傳真通知", Value = "AJ" }); }
            if (dvalue) { items.Insert(0, new SelectListItem { Text = "email入帳通知", Value = "EI" }); }
            if (dvalue) { items.Insert(0, new SelectListItem { Text = "--請選擇--", Value = "" }); }
            return items;
        }

        // 入帳通知下拉選項       
        private List<SelectListItem> SetFlgDropDown(string no = "")
        {
            List<SelectListItem> items = new List<SelectListItem>();
            items.AddRange(new SelectList(GetSelectItemF(), "Value", "Text", no));
            return items;
        }


        List<SelectListItem> GetSelectItem(bool dvalue = true)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            //if (dvalue) { items.Insert(0, new SelectListItem { Text = "--請選擇--", Value = "" }); }
            return items;
        }

        // Status下拉選項       
        private List<SelectListItem> SetStatusDropDown(string no = "")
        {
            List<SelectListItem> items = GetSelectItem();
            items.AddRange(new SelectList(this.choiceManager.FindByCode("Status"), "Value", "Name"));
            return items;
        }

        // 初始化DropDownList      
        List<SelectListItem> GetSelectItems(bool dvalue = true)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            if (dvalue) { items.Insert(0, new SelectListItem { Text = "--請選擇--", Value = "" }); }
            return items;
        }

        // 特約機構下拉選項       
        private List<SelectListItem> SetMerchantDropDown(string no = "")
        {
            List<SelectListItem> items = GetSelectItems();
            items.AddRange(new SelectList(this.gmMerchantManager.FindAll(), "MerchantNo", "MerchantName", no));
            return items;
        }

        
        public ActionResult Result()
        {
            
            string Status = "";
            string caseNo = "";
            if (Request.Form["searchConfirm"] != null)
            {
                //Status = Request.Form["Status"];
                caseNo = Request.Form["AcctTitleSearch"];
            }
            //ViewBag.Status = new SelectList(this.choiceManager.FindByCode("Status"), "Value", "Name");
            //ViewBag.Status = SetStatusDropDown(Status);
            
            ViewBag.AcctTitleSearch = caseNo;

            List<AmEntAcctD> amEntAcctList = amEntAcctDManager.FindBySearch(caseNo, Status);

                AmEntAcctDModel result = new AmEntAcctDModel();
                result.AmEntAccountDList = amEntAcctList;

                ViewBag.Inform_Name = "test";
                //return HttpNotFound();
                return View(result);

        }


        public ActionResult Create()
        {
            //ViewBag.Status = new SelectList(this.choiceManager.FindByCode("Status"), "Value", "Name");

            List<SelectListItem> items = GetSelectItems();
            items.AddRange(new SelectList(this.gmMerchantManager.FindAll(), "MerchantNo", "MerchantName"));

            ViewBag.Merchant_No = items;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                AmEntAcctD obj = new AmEntAcctD();
                obj.AcctTitle = Request.Form["AcctTitle"];
                obj.BankNo = Request.Form["BankNo"];
                obj.BankAcct = Request.Form["BankAcct"];
                obj.Name = Request.Form["Name"];//User.Identity.Name.ToString();
                obj.Reg_Id = Request.Form["Reg_Id"];
                obj.Tel = Request.Form["Tel"];
                obj.Fax = Request.Form["Fax"];
                obj.Email = Request.Form["Email"];
                obj.Inform_Flg = Request.Form["Inform_Flg"];
                obj.Merchant_No = Request.Form["Merchant_No"];

                this.amEntAcctDManager.Insert(obj);
                //return RedirectToAction("Index", new { status = obj.Status });



                return RedirectToAction("Result");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View();
            }
        }

        public ActionResult Edit(string bankNo, string bankAcct)
        {
            AmEntAcctD obj = this.amEntAcctDManager.FindByPk(bankNo, bankAcct);
            if (obj == null)
            {
                return HttpNotFound();
            }
            //ViewBag.Status = new SelectList(this.choiceManager.FindByCode("Status"), "Value", "Name", obj.Status);
            ViewBag.Merchant_No = SetMerchantDropDown(obj.Merchant_No);

            ViewBag.Inform_Flg = SetFlgDropDown(obj.Inform_Flg);

            AmEntAcctDModel result = new AmEntAcctDModel();
            result.AmEntAccountD = obj;
            return View(result);
        }

        public ActionResult GetRoleListing(string bankNo, string bankAcct)
        {
            ViewBag.BankNo = bankNo;
            ViewBag.BankAcct = bankAcct;
            List<AmRoles> noRoleList = this.amAcctRoleManager.FindNoRolesByBankInfo(bankNo, bankAcct);
            ViewBag.NoRoleCount = noRoleList.Count;
            ViewBag.RoleIdN = new SelectList(noRoleList, "RoleId", "RoleName");
            List<AmRoles> selRoleList = this.amAcctRoleManager.FindRolesByBankInfo(bankNo, bankAcct);
            AmRolesModel result = new AmRolesModel();
            result.AmRolesList = selRoleList;
            return PartialView(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateDept(FormCollection collection)
        {
            string bankNo = Request.Form["BankNo"];
            string bankAcct = Request.Form["BankAcct"];
            try
            {
                this.amAcctRoleManager.Insert(bankNo, bankAcct, Request.Form["RoleIdN"]);
                return RedirectToAction("GetRoleListing", new { bankNo = bankNo, bankAcct = bankAcct });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteAcctRole(FormCollection collection)
        {
            string bankNo = Request.Form["BankNo"];
            string bankAcct = Request.Form["BankAcct"];
            try
            {
                this.amEntAcctDManager.Delete(bankNo, bankAcct);
                return RedirectToAction("GetRoleListing", new { bankNo = bankNo, bankAcct = bankAcct });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View();
            }
        }

        public ActionResult Delete(string bankNo, string bankAcct)
        {
            try
            {
                this.amEntAcctDManager.Delete(bankNo, bankAcct);
                return RedirectToAction("Result");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View();
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(string bankNo, string bankAcct,FormCollection collection)
        {
            try
            {
                bankNo = Request.Form["BankNo"];
                bankAcct = Request.Form["BankAcct"];
                this.amEntAcctDManager.Delete(bankNo, bankAcct);
                return RedirectToAction("Result");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View();
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(string bankNo, string bankAcct, FormCollection collection)
        {
            try
            {
                bankNo = Request.Form["BankNo"];
                bankAcct = Request.Form["BankAcct"];

                // TODO: Add update logic here
                AmEntAcctD obj = this.amEntAcctDManager.FindByPk(bankNo, bankAcct);
                obj.Name = Request.Form["Name"];//User.Identity.Name.ToString();
                obj.Reg_Id = Request.Form["Reg_Id"];
                obj.Tel = Request.Form["Tel"];
                obj.Fax = Request.Form["Fax"];
                obj.Email = Request.Form["Email"];
                obj.Inform_Flg = Request.Form["Inform_Flg"];
                obj.Merchant_No = Request.Form["Merchant_No"];

                this.amEntAcctDManager.Update(obj);
                //return RedirectToAction("Index", new { status = obj.Status });
                return RedirectToAction("Result");
            }
            catch
            {
                return View();
            }
        }
    }
}