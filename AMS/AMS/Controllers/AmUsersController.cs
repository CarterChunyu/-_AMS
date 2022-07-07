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
    public class AmUsersController : BaseController
    {
        public AmUsersManager amUsersManager { get; set; }
        public AmChoiceManager choiceManager { get; set; }
        public AmRolesManager amRolesManager { get; set; }

        public AmUsersController()
        {
            amUsersManager = new AmUsersManager();
            choiceManager = new AmChoiceManager();
            amRolesManager = new AmRolesManager();
        }

        [CustomAuthorize(AccessLevel = "System,SystemSE")]
        public ActionResult Index(string status)
        {
            ViewBag.Status = new SelectList(this.choiceManager.FindByCode("Status"), "Value", "Name", status);
            List<AmUsers> userList = this.amUsersManager.FindByStatus(status);
            AmUsersModel result = new AmUsersModel();
            result.AmUsersList = userList;
            return View(result);
        }

        [CustomAuthorize(AccessLevel = "System,SystemSE")]
        public ActionResult Create()
        {

            ViewBag.Role = new SelectList(this.amRolesManager.FindAll(), "RoleId", "RoleName");
            ViewBag.Status = new SelectList(this.choiceManager.FindByCode("Status"), "Value", "Name");
            return View();
        }

        [CustomAuthorize(AccessLevel = "System,SystemSE")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                AmUsers obj = new AmUsers();
                obj.Username = Request.Form["Username"];
                obj.Password = Request.Form["Password"];
                obj.Name = Request.Form["Name"];
                obj.Email = Request.Form["Email"];
                obj.Role = Request.Form["Role"];
                obj.Status = Request.Form["Status"];
                obj.Opid = User.Identity.Name.ToString();

                this.amUsersManager.Insert(obj);
                return RedirectToAction("Index", new { status = obj.Status });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View();
            }
        }

        [CustomAuthorize(AccessLevel = "System,SystemSE")]
        public ActionResult Edit(string username)
        {
            AmUsers obj = this.amUsersManager.FindByPk(username);
            if (obj == null)
            {
                return HttpNotFound();
            }
            ViewBag.Role = new SelectList(this.amRolesManager.FindAll(), "RoleId", "RoleName", obj.Role);
            ViewBag.Status = new SelectList(this.choiceManager.FindByCode("Status"), "Value", "Name", obj.Status);
            AmUsersModel result = new AmUsersModel();
            result.AmUsers = obj;
            return View(result);
        }

        [CustomAuthorize(AccessLevel = "System,SystemSE")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(string username, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here
                AmUsers obj = this.amUsersManager.FindByPk(username);
                obj.Name = Request.Form["Name"];
                obj.Email = Request.Form["Email"];
                obj.Role = Request.Form["Role"];
                obj.Status = Request.Form["Status"];
                obj.Opid = User.Identity.Name.ToString();

                this.amUsersManager.Update(obj);
                return RedirectToAction("Index", new { status = obj.Status });
            }
            catch
            {
                return View();
            }
        }

        //[CustomAuthorize(AccessLevel = "System,Accounting,Sales,Customer,SystemSE,Marketing,PM,SalesManager")]
        //public ActionResult ChangePasswordUser()
        //{
        //    AmUsers obj = this.amUsersManager.FindByPk(User.Identity.Name.ToString());
        //    if (obj == null)
        //    {
        //        return HttpNotFound();
        //    }

        //    PasswordModel result = new PasswordModel();
        //    result.Username = obj.Username;
        //    return View("ChangePassword", result);
        //}

        [CustomAuthorize(AccessLevel = "System,SystemSE")]
        public ActionResult ChangePassword(string username)
        {
            AmUsers obj = this.amUsersManager.FindByPk(username);
            if (obj == null)
            {
                return HttpNotFound();
            }

            return View();
        }

        [CustomAuthorize(AccessLevel = "System,Accounting,Sales,Customer,SystemSE,Marketing")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(PasswordModel pm)
        {
            try
            {
                if (this.amUsersManager.UpdatePassword(pm.Username, pm.Password, User.Identity.Name.ToString()) > 0)
                {
                    if (pm.Username == User.Identity.Name.ToString())
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        this.webPasswordLogManager.UpdateStatus(systemId, pm.Username, WebPasswordLog.PWStatusType.Normal);
                        this.webPasswordLogManager.ClearErrorCount(systemId, pm.Username);
                        this.webPasswordLogManager.UpdateDate(systemId, pm.Username, DateTime.Now); 
                        return RedirectToAction("Index", new { status = "ONLINE" });
                    }
                }
                else {
                    ModelState.AddModelError("", "密碼變更失敗!!");
                    return View();
                }
            }
            catch
            {
                return View();
            }
        }

        [CustomAuthorize(AccessLevel = "System,SystemSE")]
        public ActionResult Delete(string username)
        {
            try
            {
                // TODO: Add delete logic here
                AmUsers obj = this.amUsersManager.FindByPk(username);
                if (obj == null)
                {
                    return HttpNotFound();
                }

                this.amUsersManager.Delete(username);
                return RedirectToAction("Index", new { status = "ONLINE" });
            }
            catch
            {
                return View();
            }
        }
    }
}