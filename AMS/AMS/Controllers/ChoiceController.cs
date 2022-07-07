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
    public class ChoiceController : BaseController
    {
        public AmChoiceManager choiceManager { get; set; }

        public ChoiceController()
        {
            choiceManager = new AmChoiceManager();
        }

        // GET: SystemUsers
        public ActionResult Index(string code)
        {
            ViewBag.Code = new SelectList(this.choiceManager.FindCodeList(), "Code", "Code", code);
            IList<AmChoice> list = this.choiceManager.FindByCodeAll(code);
            ChoiceModel result = new ChoiceModel();
            result.CcSysChoiceList = list;
            return View(result);
        }

        public ActionResult IndexCode(string code)
        {
            IList<AmChoice> list = this.choiceManager.FindByCodeAll(code);
            ChoiceModel result = new ChoiceModel();
            result.CcSysChoiceList = list;
            return View(result);
        }

        public ActionResult Delete(string choiceCode, string choiceValue)
        {
            try
            {
                this.choiceManager.Delete(choiceCode, choiceValue);
                return RedirectToAction("Index", new { code = "CARD_STATUS" });
            }
            catch
            {
                return View();
            }
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
                AmChoice obj = new AmChoice();
                obj.Code = Request.Form["Code"];
                obj.Value = Request.Form["Value"];
                obj.Name = Request.Form["Name"];
                obj.Remark = Request.Form["Remark"];
                obj.Status = Request.Form["Status"];
                obj.Rank = Int32.Parse(Request.Form["Rank"]);

                this.choiceManager.Insert(obj);
                return RedirectToAction("Index", new { code = obj.Code });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View();
            }
        }

        public ActionResult CreateCode()
        {
            ViewBag.Status = new SelectList(this.choiceManager.FindByCode("Status"), "Value", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateCode(FormCollection collection)
        {
            try
            {
                AmChoice obj = new AmChoice();
                obj.Code = Request.Form["Code"];
                obj.Value = Request.Form["Value"];
                obj.Name = Request.Form["Name"];
                obj.Remark = Request.Form["Remark"];
                obj.Status = Request.Form["Status"];
                obj.Rank = Int32.Parse(Request.Form["Rank"]);

                this.choiceManager.Insert(obj);
                return RedirectToAction("Index", new { code = obj.Code });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View();
            }
        }
    }
}