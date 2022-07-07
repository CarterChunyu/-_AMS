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
    public class MenuController : BaseController
    {
        public AmMenuManager menuManager { get; set; }
        public AmChoiceManager choiceManager { get; set; }

        public MenuController()
        {
            menuManager = new AmMenuManager();
            choiceManager = new AmChoiceManager();
        }
        public ActionResult Index(int pid)
        {
            IList<AmMenu> mainList = this.menuManager.FindByPidm(0);
            IList<AmMenu> menuList = this.menuManager.FindByPid(pid);
            AmMenu mMenuSel = new AmMenu();
            mMenuSel.Id = 0;
            mMenuSel.Name = "--主選單--";
            mainList.Add(mMenuSel);
            ViewBag.MainMenu = new SelectList(mainList, "Id", "Name", pid);
            MenuModel result = new MenuModel();
            result.MenuList = menuList;
            return View(result);
        }

        public ActionResult Create()
        {
            IList<AmMenu> mainList = this.menuManager.FindByPidm(0);
            AmMenu mMenuSel = new AmMenu();
            mMenuSel.Id = 0;
            mMenuSel.Name = "--主選單--";
            mainList.Add(mMenuSel);
            ViewBag.MainMenu = new SelectList(mainList, "Id", "Name");
            ViewBag.Status = new SelectList(this.choiceManager.FindByCode("Status"), "Value", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                AmMenu obj = new AmMenu();
                obj.Name = Request.Form["Name"];
                obj.Path = Request.Form["Path"];
                obj.Roles = Request.Form["Roles"];
                obj.Status = Request.Form["Status"];
                obj.ParentId = Convert.ToInt32(Request.Form["MainMenu"]);
                obj.Rank = Convert.ToInt32(Request.Form["Rank"]);

                this.menuManager.Insert(obj);
                return RedirectToAction("Index", new { pid = obj.ParentId });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View();
            }
        }

        public ActionResult Edit(int id)
        {
            AmMenu obj = this.menuManager.FindByPk(id);
            if (obj == null)
            {
                return HttpNotFound();
            }
            IList<AmMenu> mainList = this.menuManager.FindByPidm(0);
            AmMenu mMenuSel = new AmMenu();
            mMenuSel.Id = 0;
            mMenuSel.Name = "--主選單--";
            mainList.Add(mMenuSel);
            ViewBag.MainMenu = new SelectList(mainList, "Id", "Name", obj.ParentId);
            ViewBag.Status = new SelectList(this.choiceManager.FindByCode("Status"), "Value", "Name", obj.Status);
            MenuModel result = new MenuModel();
            result.AmMenu = obj;
            return View(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here
                AmMenu obj = this.menuManager.FindByPk(id);
                obj.Name = Request.Form["Name"];
                obj.Path = Request.Form["Path"];
                obj.Roles = Request.Form["Roles"];
                obj.Status = Request.Form["Status"];
                obj.ParentId = Convert.ToInt32(Request.Form["MainMenu"]);
                obj.Rank = Convert.ToInt32(Request.Form["Rank"]);

                this.menuManager.Update(obj);
                return RedirectToAction("Index", new { pid = obj.ParentId });
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Delete(int id)
        {
            try
            {
                // TODO: Add delete logic here
                AmMenu obj = this.menuManager.FindByPk(id);
                if (obj == null)
                {
                    return HttpNotFound();
                }

                this.menuManager.Delete(id);
                this.menuManager.DeleteByPid(id);
                return RedirectToAction("Index", new { pid = obj .ParentId});
            }
            catch
            {
                return View();
            }
        }
    }
}