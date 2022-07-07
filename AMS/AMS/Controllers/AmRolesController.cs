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
    [CustomAuthorize(AccessLevel = "System")]
    public class AmRolesController : BaseController
    {
        public AmRolesManager amRolesManager { get; set; }

        public AmRolesController()
        {
            amRolesManager = new AmRolesManager();
        }
        // GET: AmRoles
        public ActionResult Index()
        {
            List<AmRoles> list = this.amRolesManager.FindAll();
            AmRolesModel result = new AmRolesModel();
            result.AmRolesList = list;
            return View(result);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                AmRoles obj = new AmRoles();
                obj.RoleId = Request.Form["RoleId"];
                obj.RoleName = Request.Form["RoleName"];

                this.amRolesManager.Insert(obj);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View();
            }
        }

        public ActionResult Delete(string roleId)
        {
            try
            {
                // TODO: Add delete logic here
                AmRoles obj = this.amRolesManager.FindByPk(roleId);
                if (obj == null)
                {
                    return HttpNotFound();
                }

                this.amRolesManager.Delete(roleId);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}