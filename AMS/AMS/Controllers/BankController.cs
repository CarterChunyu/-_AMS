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
    public class BankController : BaseController
    {
        public AmEntAccountDManager amEntAccountDManager { get; set; }
        public AmAccountRoleManager amAccountRoleManager { get; set; }
        public AmChoiceManager choiceManager { get; set; }
        public AmRolesManager rolesManager { get; set; }

        public BankController()
        {
            amEntAccountDManager = new AmEntAccountDManager();
            amAccountRoleManager = new AmAccountRoleManager();
            choiceManager = new AmChoiceManager();
            rolesManager = new AmRolesManager();
        }
        public ActionResult Index(string status)
        {
            ViewBag.Status = new SelectList(this.choiceManager.FindByCode("Status"), "Value", "Name", status);
            List<AmEntAccountD> list = this.amEntAccountDManager.FindByStatus(status);
            AmEntAccountDModel result = new AmEntAccountDModel();
            result.AmEntAccountDList = list;
            return View(result);
        }

        public ActionResult Create()
        {
            ViewBag.Status = new SelectList(this.choiceManager.FindByCode("Status"), "Value", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                AmEntAccountD obj = new AmEntAccountD();
                obj.AcctTitle = Request.Form["AcctTitle"];
                obj.BankNo = Request.Form["BankNo"];
                obj.BankAcct = Request.Form["BankAcct"];
                obj.AcctCode = Request.Form["AcctCode"];
                obj.CompanyId = Request.Form["CompanyId"];
                obj.Department = Request.Form["Department"];
                obj.Name = Request.Form["Name"];
                obj.Remark = Request.Form["Remark"];
                obj.Status = Request.Form["Status"];
                obj.Operator = User.Identity.Name.ToString();

                this.amEntAccountDManager.Insert(obj);
                return RedirectToAction("Index", new { status = obj.Status });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View();
            }
        }

        public ActionResult Edit(string bankNo, string bankAcct)
        {
            AmEntAccountD obj = this.amEntAccountDManager.FindByPk(bankNo, bankAcct);
            if (obj == null)
            {
                return HttpNotFound();
            }
            ViewBag.Status = new SelectList(this.choiceManager.FindByCode("Status"), "Value", "Name", obj.Status);
            AmEntAccountDModel result = new AmEntAccountDModel();
            result.AmEntAccountD = obj;
            return View(result);
        }

        public ActionResult GetRoleListing(string bankNo, string bankAcct)
        {
            ViewBag.BankNo = bankNo;
            ViewBag.BankAcct = bankAcct;
            List<AmRoles> noRoleList = this.amAccountRoleManager.FindNoRolesByBankInfo(bankNo, bankAcct);
            ViewBag.NoRoleCount = noRoleList.Count;
            ViewBag.RoleIdN = new SelectList(noRoleList, "RoleId", "RoleName");
            List<AmRoles> selRoleList = this.amAccountRoleManager.FindRolesByBankInfo(bankNo, bankAcct);
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
                this.amAccountRoleManager.Insert(bankNo, bankAcct, Request.Form["RoleIdN"]);
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
        public ActionResult DeleteAccountRole(FormCollection collection)
        {
            string roleId = Request.Form["roleId"];
            string bankNo = Request.Form["BankNo"];
            string bankAcct = Request.Form["BankAcct"];
            try
            {
                this.amAccountRoleManager.Delete(roleId, bankNo, bankAcct);
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
        public ActionResult Edit(string bankNo, string bankAcct, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here
                AmEntAccountD obj = this.amEntAccountDManager.FindByPk(bankNo, bankAcct);
                obj.Department = Request.Form["Name"];
                obj.CompanyId = Request.Form["CompanyId"];
                obj.Department = Request.Form["Department"];
                obj.Name = Request.Form["Name"];
                obj.Remark = Request.Form["Remark"];
                obj.Status = Request.Form["Status"];
                obj.Operator = User.Identity.Name.ToString();

                this.amEntAccountDManager.Update(obj);
                return RedirectToAction("Index", new { status = obj.Status });
            }
            catch
            {
                return View();
            }
        }
    }
}