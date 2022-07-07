using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;

using Business;
using Domain.Common;
using Domain.ICASHOPMerchant;

using log4net;
using Newtonsoft.Json;

namespace AMS.Controllers
{
    [CustomAuthorize(AccessLevel = "System,Sales,SalesManager,")]
    public class ICASHOPMerchantController : BaseController
    {
        #region 設定全域變數
        private static readonly ILog log = LogManager.GetLogger(typeof(ICASHOPMerchantController));
        private ICASHOPMerchantManager icashopMerchantManager { get; set; }
        #endregion

        #region 建構子
        public ICASHOPMerchantController()
        {
            icashopMerchantManager = new ICASHOPMerchantManager();
        }
        #endregion

        public ActionResult Index(QueryReq qr)
        {
            return View(qr);
        }

        /// <summary>
        /// 按下[查詢]鈕
        /// </summary>
        /// <param name="qr">查詢條件</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Query(QueryReq qr)
        {
            #region 設定變數

            DataTable dt_QueryResult = null;
            ExecInfo ei_all = new ExecInfo();
            ExecInfo ei_detl = new ExecInfo();
            PageCountViewModel pcv = new PageCountViewModel();

            #endregion

            #region 設定tag
            ei_all.Tag_M = "按下[查詢]鈕";
            #endregion

            try
            {
                #region 查詢自串點數特店主檔

                ei_all.Tag_D = "查詢自串點數特店主檔";
                ei_detl.fun_reset();
                dt_QueryResult = icashopMerchantManager.get_MerchantData(ref ei_detl,
                                                                         qr,
                                                                         pcv);

                if (ei_detl.RtnResult == false)
                { throw new CustomException.ExecErr(ei_detl.RtnMsg); }

                #endregion

                #region 設定分頁資訊

                ei_all.Tag_D = "設定分頁資訊";
                ei_detl.fun_reset();
                icashopMerchantManager.set_PageCount(ref ei_detl,
                                                     ref pcv,
                                                     qr,
                                                     dt_QueryResult);

                if (ei_detl.RtnResult == false)
                { throw new CustomException.ExecErr(ei_detl.RtnMsg); }

                #endregion

                #region 執行成功
                ei_all.RtnResult = true;
                #endregion
            }
            catch (CustomException.ExecErr ex_exec)
            {
                ei_all.RtnResult = false;
                ei_all.RtnMsg = ex_exec.Message;

                log.Error(ei_all.RtnMsg);
            }
            catch (Exception ex)
            {
                ei_all.RtnResult = false;
                ei_all.RtnMsg = ei_all.fun_combine_err_msg(ex.Message);

                log.Error(ei_all.RtnMsg);
            }
            finally
            {
                ViewBag.ExecInfo = ei_all;
                ViewBag.QueryResult = dt_QueryResult;
                ViewBag.PageModel = pcv;
            }

            return PartialView("_PartialQueryResult");
        }

        public ActionResult Edit(EditReq er)
        {
            return View(er);
        }

        /// <summary>
        /// 編輯頁面，按下[儲存]鈕
        /// </summary>
        /// <param name="esr"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditSave(EditSaveReq esr)
        {
            #region 設定變數

            ExecInfo ei_all = new ExecInfo();
            ExecInfo ei_detl = new ExecInfo();

            #endregion

            #region 設定tag
            ei_all.Tag_M = "編輯頁面，按下[儲存]鈕";
            #endregion

            try
            {
                #region 取得使用者、IP

                ei_all.Tag_D = "取得使用者";
                esr.User = this.User.Identity.Name;

                ei_all.Tag_D = "取得IP";
                string[] array_REMOTE_ADDR = Request.ServerVariables.GetValues("REMOTE_ADDR");

                if (array_REMOTE_ADDR != null &&
                    array_REMOTE_ADDR.Length > 0)
                {
                    esr.IP = array_REMOTE_ADDR[0];
                }
                else
                {
                    esr.IP = "WEBUI";
                }

                #endregion

                #region 檢核傳入值

                ei_all.Tag_D = "檢核傳入值，字串轉換動態物件";
                dynamic d_OPMerchantData = JsonConvert.DeserializeObject(esr.EditJsonData);

                ei_all.Tag_D = "檢核傳入值，檢核資料";
                ei_detl.fun_reset();
                this.icashopMerchantManager.check_MerchantData(ref ei_detl,
                                                               d_OPMerchantData);

                if (ei_detl.RtnResult == false)
                { throw new CustomException.ExecErr(ei_detl.RtnMsg); }

                #endregion

                #region 依據類別進行資料庫的新增或更新

                ei_all.Tag_D = "依據類別進行資料庫的新增或更新";

                ei_detl.fun_reset();
                switch (esr.EditType)
                {
                    case EditSaveReq.en_EditType.Add:
                        ei_all.Tag_D = "新增自串點數特店主檔";
                        this.icashopMerchantManager.add_MerchantData(ref ei_detl,
                                                                     esr);
                        break;
                    case EditSaveReq.en_EditType.Modify:
                        ei_all.Tag_D = "更新自串點數特店主檔";
                        this.icashopMerchantManager.modify_MerchantData(ref ei_detl,
                                                                        esr);
                        break;
                }

                if (ei_detl.RtnResult == false)
                { throw new CustomException.ExecErr(ei_detl.RtnMsg); }

                #endregion

                #region 設定成功

                ei_all.RtnResult = true;

                #endregion
            }
            catch (CustomException.ExecErr ex_exec)
            {
                ei_all.RtnResult = false;
                ei_all.RtnMsg = ex_exec.Message;

                log.Error(ei_all.RtnMsg);
            }
            catch (Exception ex)
            {
                ei_all.RtnResult = false;
                ei_all.RtnMsg = ei_all.fun_combine_err_msg(ex.Message);

                log.Error(ei_all.RtnMsg);
            }

            return Json(ei_all);
        }
    }
}