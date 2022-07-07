using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AMS.Models;
using AMS.ViewModels;
using AMS.Services;

//using System.Data.Entity.Infrastructure;
//using System.Data.Entity.Validation;

namespace AMS.Controllers
{
    public class TRAADJController : Controller
    {
        private TRAADJEntities db = new TRAADJEntities();


        /* GET: /TRAADJ/
        public ActionResult Index()
        {
            return View();
        }
        */
        
        public ActionResult Index(string begin, string end)
        {
            if(begin !=null)
            Session["beginDate"] = begin;
            if(end != null)
            Session["endDate"] = end;

            return View(db.TM_ADJ_APPLICATION.Where(x => string.Compare(x.APP_DATE,begin)>=0 && string.Compare(x.APP_DATE,end)<=0).OrderBy(x=>x.APP_DATE).ToList());
        }

        // GET: /TRAADJ/Details/5
        /*public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ADJ_APPLICATION adj_application = db.ADJ_APPLICATION.Find(id);
            if (adj_application == null)
            {
                return HttpNotFound();
            }
            return View(adj_application);
        }
        */

        // GET: /TRAADJ/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /TRAADJ/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        
        public ActionResult Create(string APP_DATE,string OPP_DATE_START,string OPP_DATE_END,string FORM_SERIAL,string ADJ_REMARK,string REPLY_REMARK)
        {
            TM_ADJ_APPLICATION adj_application = new TM_ADJ_APPLICATION();

            if (ModelState.IsValid)
            {    
                adj_application.APP_DATE = APP_DATE;
                adj_application.OPP_DATE_START = OPP_DATE_START;
                adj_application.OPP_DATE_END = OPP_DATE_END;
                adj_application.FORM_SERIAL = FORM_SERIAL;
                adj_application.MODIFIABLE = "Y";
                adj_application.ADJ_REMARK = ADJ_REMARK;
                adj_application.REPLY_REMARK = REPLY_REMARK;
                adj_application.CREATE_DATETIME = DateTime.Now.ToString("yyyyMMddHHmmss");
                db.TM_ADJ_APPLICATION.Add(adj_application);
                
                db.SaveChanges();

                return RedirectToAction("Index", new { begin = Session["beginDate"], end = Session["endDate"] });
            }
            //else
            //{ return View();}

            return View(adj_application);
        }

        // GET: /TRAADJ/Edit/5
        public ActionResult Edit(int? id)
        {
            TM_ADJ_APPLICATION adj_application = null;
            
            if (id != null)
            {
                adj_application = db.TM_ADJ_APPLICATION.Find(id);
                Session["formId"] = id;
            }
            ApplicationItemSums itemSums = new ApplicationItemSums();
            List<TM_ADJ_APPLICATION_ITEM> arItemsList = new List<TM_ADJ_APPLICATION_ITEM>();
            List<TM_ADJ_APPLICATION_ITEM> apItemsList = new List<TM_ADJ_APPLICATION_ITEM>();
            if (id != null)
            {
                arItemsList = db.TM_ADJ_APPLICATION_ITEM.Where(x => x.FORM_ID == id && x.ARAP == "AR").OrderBy(x => x.FILE_TRANS_TYPE).ToList();
                apItemsList = db.TM_ADJ_APPLICATION_ITEM.Where(x => x.FORM_ID == id && x.ARAP == "AP").OrderBy(x => x.FILE_TRANS_TYPE).ToList();
            }

            List<Tuple<int, string, int, long, string,int>> arTuples = new List<Tuple<int, string, int, long, string,int>>();
            for (int i = 0; i < arItemsList.Count(); i++)
            {
                TM_ADJ_APPLICATION_ITEM currentItem = arItemsList[i];
                arTuples.Add(new Tuple<int, string, int, long, string,int>(i + 1, MenuStrings.ItemString(currentItem.TRANS_TYPE, currentItem.FILE_TRANS_TYPE, currentItem.FILE_SUB_TYPE), (int)currentItem.TRANS_CNT, (long)currentItem.TRANS_AMT, currentItem.REMARK,currentItem.APP_ITEM_ID));
            }


            List<Tuple<int, string, int, long, string,int>> apTuples = new List<Tuple<int, string, int, long, string,int>>();
            for (int i = 0; i < apItemsList.Count(); i++)
            {
                TM_ADJ_APPLICATION_ITEM currentItem = apItemsList[i];
                
                apTuples.Add(new Tuple<int, string, int, long, string,int>(i + 1, MenuStrings.ItemString(currentItem.TRANS_TYPE, currentItem.FILE_TRANS_TYPE, currentItem.FILE_SUB_TYPE), (int)currentItem.TRANS_CNT, (long)currentItem.TRANS_AMT, currentItem.REMARK,currentItem.APP_ITEM_ID));
            }
            itemSums.ADJApplication = adj_application;
            itemSums.ARCountAmount = arTuples;
            itemSums.APCountAmount = apTuples;
            return View(itemSums);
        }

        // POST: /TRAADJ/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int FORM_ID,string APP_DATE, string OPP_DATE_START, string OPP_DATE_END, string FORM_SERIAL, string ADJ_REMARK, string REPLY_REMARK)
        {

            TM_ADJ_APPLICATION adj_application = db.TM_ADJ_APPLICATION.Where(x => x.FORM_ID == FORM_ID).FirstOrDefault();

                adj_application.APP_DATE = APP_DATE;
                adj_application.OPP_DATE_START = OPP_DATE_START;
                adj_application.OPP_DATE_END = OPP_DATE_END;
                adj_application.FORM_SERIAL = FORM_SERIAL;
                
                adj_application.ADJ_REMARK = ADJ_REMARK;
                adj_application.REPLY_REMARK = REPLY_REMARK;
                adj_application.UPDATE_DATETIME = DateTime.Now.ToString("yyyyMMddHHmmss");
                
                db.SaveChanges();
            
            return RedirectToAction("Index", new { begin = Session["beginDate"], end = Session["endDate"] });
            
        }

        // GET: /TRAADJ/Delete/5
        /*public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ADJ_APPLICATION adj_application = db.ADJ_APPLICATION.Find(id);
            if (adj_application == null)
            {
                return HttpNotFound();
            }
            return View(adj_application);
        }

        // POST: /TRAADJ/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ADJ_APPLICATION adj_application = db.ADJ_APPLICATION.Find(id);
            db.ADJ_APPLICATION.Remove(adj_application);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        */
        public ActionResult Delete(int? id)
        {
            if (id != null)
            {
                TM_ADJ_APPLICATION adj_application = db.TM_ADJ_APPLICATION.Find(id);
                db.TM_ADJ_APPLICATION.Remove(adj_application);
                db.SaveChanges();
            }
            return RedirectToAction("Index", new { begin = Session["beginDate"], end = Session["endDate"] });
        }
        public ActionResult Freeze(int id)
        {
            TM_ADJ_APPLICATION adj_application = db.TM_ADJ_APPLICATION.Find(id);
                return View(adj_application);
        }

        [HttpPost]
        public ActionResult Freeze(int id, string rcptDate)
        {
            TM_ADJ_APPLICATION adj_application = db.TM_ADJ_APPLICATION.Find(id);
            adj_application.RCPT_DATE = rcptDate;
            adj_application.MODIFIABLE = "N";
            db.SaveChanges();
            
            return RedirectToAction("Index", new { begin = Session["beginDate"], end = Session["endDate"] });
        }

        public ActionResult Viewing(int? id)
        {
            TM_ADJ_APPLICATION adj_application = null;
            if (id != null)
            {
                adj_application = db.TM_ADJ_APPLICATION.Find(id);
            }

            ApplicationItemSums itemSums = new ApplicationItemSums();
            List<TM_ADJ_APPLICATION_ITEM> arItemsList = new List<TM_ADJ_APPLICATION_ITEM>();
            List<TM_ADJ_APPLICATION_ITEM> apItemsList = new List<TM_ADJ_APPLICATION_ITEM>();
            if (id != null)
            {
                arItemsList = db.TM_ADJ_APPLICATION_ITEM.Where(x => x.FORM_ID == id && x.ARAP == "AR").OrderBy(x => x.FILE_TRANS_TYPE).ToList();
                apItemsList = db.TM_ADJ_APPLICATION_ITEM.Where(x => x.FORM_ID == id && x.ARAP == "AP").OrderBy(x => x.FILE_TRANS_TYPE).ToList();
            }

            List<Tuple<int, string, int, long, string, int>> arTuples = new List<Tuple<int, string, int, long, string, int>>();
            for (int i = 0; i < arItemsList.Count(); i++)
            {
                TM_ADJ_APPLICATION_ITEM currentItem = arItemsList[i];
                arTuples.Add(new Tuple<int, string, int, long, string, int>(i + 1, MenuStrings.ItemString(currentItem.TRANS_TYPE, currentItem.FILE_TRANS_TYPE, currentItem.FILE_SUB_TYPE), (int)currentItem.TRANS_CNT, (long)currentItem.TRANS_AMT, currentItem.REMARK, currentItem.APP_ITEM_ID));
            }


            List<Tuple<int, string, int, long, string, int>> apTuples = new List<Tuple<int, string, int, long, string, int>>();
            for (int i = 0; i < apItemsList.Count(); i++)
            {
                TM_ADJ_APPLICATION_ITEM currentItem = apItemsList[i];

                apTuples.Add(new Tuple<int, string, int, long, string, int>(i + 1, MenuStrings.ItemString(currentItem.TRANS_TYPE, currentItem.FILE_TRANS_TYPE, currentItem.FILE_SUB_TYPE), (int)currentItem.TRANS_CNT, (long)currentItem.TRANS_AMT, currentItem.REMARK, currentItem.APP_ITEM_ID));
            }
            itemSums.ADJApplication = adj_application;
            itemSums.ARCountAmount = arTuples;
            itemSums.APCountAmount = apTuples;
            return View(itemSums);
        }

        /*
        public ActionResult ContentListing(int? appItemId)
        {
            List<TM_SETTLE_ADJ_DATA_D> listOfData = new List<TM_SETTLE_ADJ_DATA_D>();

            if (appItemId != null)
            {
                listOfData = db.TM_SETTLE_ADJ_DATA_D.Where(x => x.APP_ITEM_ID == appItemId).OrderBy(y => y.CPT_DATE).ToList();
            }
            if (appItemId == null) throw new Exception();
            return View(listOfData);
        }
        */
      

        

        public ActionResult ItemEditing(int appItemId)
        {
            TM_ADJ_APPLICATION_ITEM adjApplicationItem = db.TM_ADJ_APPLICATION_ITEM.Find(appItemId);

            List<SelectListItem> codesList = new List<SelectListItem>();
            if (adjApplicationItem.ARAP == "AR")
            {
                ViewBag.ARAP="應收";
                SelectList list = new SelectList(MenuStrings.DRCollection(), "TransTypes", "TypeDescs");

                SelectListItem item = list.Where(x => x.Value == (adjApplicationItem.FILE_TRANS_TYPE + "-"+ adjApplicationItem.FILE_SUB_TYPE)).FirstOrDefault();
                item.Selected = true;

                codesList.AddRange(list);
            }
            else if (adjApplicationItem.ARAP == "AP")
            {
                ViewBag.ARAP = "應付";
                SelectList list = new SelectList(MenuStrings.DPCollection(), "TransTypes", "TypeDescs");

                SelectListItem item = list.Where(x => x.Value == (adjApplicationItem.FILE_TRANS_TYPE + "-" +adjApplicationItem.FILE_SUB_TYPE)).FirstOrDefault();
                item.Selected = true;

                codesList.AddRange(list);
            }

                ViewBag.CodesList = codesList;
            
            return View(adjApplicationItem);
        }


        [HttpPost]
        public ActionResult ItemEditing(int APP_ITEM_ID, string SELECTITEM , int TRANS_CNT, decimal TRANS_AMT, string REMARK)
        {
            TM_ADJ_APPLICATION_ITEM adjApplicationItem = db.TM_ADJ_APPLICATION_ITEM.Find(APP_ITEM_ID);


            string[] arr = SELECTITEM.Split('-');
            adjApplicationItem.FILE_TRANS_TYPE = arr[0];
            adjApplicationItem.FILE_SUB_TYPE = arr[1];
            adjApplicationItem.TRANS_TYPE = arr[0] + arr[1];
            adjApplicationItem.TRANS_CNT = TRANS_CNT;
            adjApplicationItem.TRANS_AMT = TRANS_AMT;
            adjApplicationItem.REMARK = REMARK;
            adjApplicationItem.UPDATE_DATETIME = DateTime.Now.ToString("yyyyMMddHHmmss");
            
            db.SaveChanges();

            return RedirectToAction("Edit", new { id = adjApplicationItem.FORM_ID });
        }

        public ActionResult ItemDeleting(int appItemId)
        {
            TM_ADJ_APPLICATION_ITEM adjApplicationItem = db.TM_ADJ_APPLICATION_ITEM.Find(appItemId);
            db.TM_ADJ_APPLICATION_ITEM.Remove(adjApplicationItem);
            db.SaveChanges();

            return RedirectToAction("Edit", new { id= Session["formId"]});
        }
        
        public ActionResult NewARItem()
        {
            List<SelectListItem> codesList = new List<SelectListItem>();
            
            codesList.AddRange(new SelectList(MenuStrings.DRCollection(), "TransTypes", "TypeDescs"));
            
            ViewBag.CodesList = codesList;

            return View();
        }

        [HttpPost]
        public ActionResult NewARItem(string SELECTITEM, int TRANS_CNT, decimal TRANS_AMT, string REMARK)
        {
            TM_ADJ_APPLICATION_ITEM adjApplicationItem = new TM_ADJ_APPLICATION_ITEM();
            string[] arr = SELECTITEM.Split('-');
            adjApplicationItem.FILE_TRANS_TYPE = arr[0];
            adjApplicationItem.FILE_SUB_TYPE = arr[1];
            adjApplicationItem.TRANS_TYPE = arr[0] + arr[1];
            adjApplicationItem.ARAP = "AR";
            adjApplicationItem.FORM_ID = (int)Session["formId"];
            adjApplicationItem.TRANS_CNT = TRANS_CNT;
            adjApplicationItem.TRANS_AMT = TRANS_AMT;
            adjApplicationItem.REMARK = REMARK;
            adjApplicationItem.CREATE_DATETIME = DateTime.Now.ToString("yyyyMMddHHmmss");
            db.TM_ADJ_APPLICATION_ITEM.Add(adjApplicationItem);
            db.SaveChanges();
            return RedirectToAction("Edit", new { id = adjApplicationItem.FORM_ID });;
        }

        public ActionResult NewAPItem()
        {
            List<SelectListItem> codesList = new List<SelectListItem>();

            codesList.AddRange(new SelectList(MenuStrings.DPCollection(), "TransTypes", "TypeDescs"));

            ViewBag.CodesList = codesList;

            return View();
        }

        [HttpPost]
        public ActionResult NewAPItem(string SELECTITEM, int TRANS_CNT, decimal TRANS_AMT, string REMARK)
        {
            TM_ADJ_APPLICATION_ITEM adjApplicationItem = new TM_ADJ_APPLICATION_ITEM();
            string[] arr = SELECTITEM.Split('-');
            adjApplicationItem.FILE_TRANS_TYPE = arr[0];
            adjApplicationItem.FILE_SUB_TYPE = arr[1];
            adjApplicationItem.TRANS_TYPE = arr[0] + arr[1];
            adjApplicationItem.ARAP = "AP";
            adjApplicationItem.FORM_ID = (int)Session["formId"];
            adjApplicationItem.TRANS_CNT = TRANS_CNT;
            adjApplicationItem.TRANS_AMT = TRANS_AMT;
            adjApplicationItem.REMARK = REMARK;
            adjApplicationItem.CREATE_DATETIME = DateTime.Now.ToString("yyyyMMddHHmmss");
            db.TM_ADJ_APPLICATION_ITEM.Add(adjApplicationItem);
            db.SaveChanges();
            return RedirectToAction("Edit", new { id = adjApplicationItem.FORM_ID }); ;
        }


   

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
