using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Business;
using Domain.Entities;
using AMS.Models;
using DataAccess;



namespace AMS.Controllers
{
    [CustomAuthorize(AccessLevel = "System,Accounting,Sales,PM,SystemSE,SalesManager")]
    public class GmMerController : BaseController
    {
        GmMerDAO gmMerDAO = new GmMerDAO();
        // GET: /GmMer/
        public ActionResult Index(string Input)
        {
            if (Input == null)
            {
                return View();
            }
            else
            {
                var list = gmMerDAO.SelectByID(Input);
                if (list == null)
                {
                    TempData["MERCHANT_NO"] = Input;
                    return RedirectToAction("Create");
                }
                else
                {
                    var result=ParseToViewModel(list);
                    return View("List", result);
                }
            }
        }

        //
        // GET: /GmMer/Create
        public ActionResult Create()
        {
            List<Tuple<string, string>> typeList = new List<Tuple<string, string>>();
            typeList.Add(new Tuple<string, string>("日","D"));
            typeList.Add(new Tuple<string, string>("周","W"));
            typeList.Add(new Tuple<string, string>("月","M"));

            //營收款撥付週期
            var REM_TYPE = new SelectList(typeList,"Item2", "Item1");
            //手續費撥付週期
            var REM_FEE_TYPE= new SelectList(typeList,"Item2", "Item1");
        
            //清分群組
            var MercGroup = gmMerDAO.SelectMercGroup();
            SelectList MERC_GROUP = new SelectList(MercGroup, "MERC_GROUP", "GROUP_DESC");
           
            //AMS選單群組
            var GroupID = gmMerDAO.SelectGroupID();
            SelectList GROUP_ID = new SelectList(GroupID);
            
            //OL_TYPE，[1](加值)、[2](自動加值)、[3](加值&自動加值)
            var OlTYPE = new List<Tuple<string, string>>();
            OlTYPE.Add(new Tuple<string, string>("僅加值", "1"));
            OlTYPE.Add(new Tuple<string, string>("僅自動加值", "2"));
            OlTYPE.Add(new Tuple<string, string>("加值&自動加值", "3"));

            var OL_TYPE = new SelectList(OlTYPE, "Item2", "Item1");

           ViewBag.REM_TYPE = REM_TYPE;
           ViewBag.REM_FEE_TYPE =REM_FEE_TYPE;
           ViewBag.MERC_GROUP = MERC_GROUP;
           ViewBag.GROUP_ID = GROUP_ID;
           ViewBag.OL_TYPE = OL_TYPE;

            return View();
        }

        //
        // POST: /GmMer/Create
        [HttpPost]
        public ActionResult Create(GmMerModel collection)
        {
            try
            {
                TempData["wait2Check"] = collection;
                return RedirectToAction("CheckCreation");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult CheckCreation()
        {
            TempData["checked"] = TempData["wait2Check"];
            return View(TempData["wait2Check"]);
        }

     
        public ActionResult InsertToDB()
        {
            try
            {
                gmMerDAO.InsertMerchant(ParseToPOCO((GmMerModel)TempData["checked"]));
                TempData["message"] = "建檔完成";
                return RedirectToAction("Index");
            }

            catch
            {
                TempData["message"] = "建檔失敗";
                return RedirectToAction("Index");
            }
        }

        public GmMerModel ParseToViewModel(GmMerInput gmMerInput)
        {
            GmMerModel gmm = new GmMerModel();
            gmm.MERCHANT_NO = gmMerInput.MERCHANT_NO;
            gmm.MERC_GROUP = gmMerInput.MERC_GROUP;
            gmm.MERCHANT_NAME = gmMerInput.MERCHANT_NAME;
            gmm.MERCHANT_STNAME = gmMerInput.MERCHANT_STNAME;
            gmm.INVOICE_NO = gmMerInput.INVOICE_NO;
            gmm.MERCH_TMID = gmMerInput.MERCH_TMID;
            gmm.OL_TYPE = gmMerInput.OL_TYPE;
            gmm.PUR_FEE_RATE = gmMerInput.PUR_FEE_RATE;
            gmm.LOAD_FEE_RATE = gmMerInput.LOAD_FEE_RATE;
            gmm.AUTO_LOAD_FEE_RATE = gmMerInput.AUTO_LOAD_FEE_RATE;
            gmm.REM_TYPE = gmMerInput.REM_TYPE;
            gmm.DAYLY_REM_DAY = gmMerInput.DAYLY_REM_DAY;
            gmm.REM_FEE_TYPE = gmMerInput.REM_FEE_TYPE;
            gmm.DAYLY_REM_FEE_DAY = gmMerInput.DAYLY_REM_FEE_DAY;
            gmm.GROUP_ID = gmMerInput.GROUP_ID;
            gmm.BUILD_USER = gmMerInput.BUILD_USER;
            return gmm;
        }

        public GmMerInput ParseToPOCO(GmMerModel gmMerModel)
        {
            GmMerInput gi = new GmMerInput();
            gi.MERCHANT_NO = gmMerModel.MERCHANT_NO;
            gi.MERC_GROUP = gmMerModel.MERC_GROUP;
            gi.MERCHANT_NAME = gmMerModel.MERCHANT_NAME;
            gi.MERCHANT_STNAME = gmMerModel.MERCHANT_STNAME;
            gi.INVOICE_NO = gmMerModel.INVOICE_NO;
            gi.MERCH_TMID = gmMerModel.MERCH_TMID;
            gi.OL_TYPE = gmMerModel.OL_TYPE;
            gi.PUR_FEE_RATE = gmMerModel.PUR_FEE_RATE;
            gi.LOAD_FEE_RATE = gmMerModel.LOAD_FEE_RATE;
            gi.AUTO_LOAD_FEE_RATE = gmMerModel.AUTO_LOAD_FEE_RATE;
            gi.REM_TYPE = gmMerModel.REM_TYPE;
            gi.DAYLY_REM_DAY = gmMerModel.DAYLY_REM_DAY;
            gi.REM_FEE_TYPE = gmMerModel.REM_FEE_TYPE;
            gi.DAYLY_REM_FEE_DAY = gmMerModel.DAYLY_REM_FEE_DAY;
            gi.GROUP_ID = gmMerModel.GROUP_ID;
            gi.BUILD_USER = gmMerModel.BUILD_USER;
            return gi;
        }
    }
}
