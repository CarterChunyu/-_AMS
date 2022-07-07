using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Data;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

using Business;
using Domain.Common;
using Domain.CoBrandedSellSponsorship;

using log4net;

/// <summary>
/// 聯名卡行銷贊助金
/// </summary>
namespace AMS.Controllers
{
    [CustomAuthorize(AccessLevel = "System,Sales,Accounting")]
    public class CoBrandedSellSponsorshipController : BaseController
    {
        #region 設定全域變數
        private static readonly ILog log = LogManager.GetLogger(typeof(ICASHOPMerchantController));
        private CoBrandedSellSponsorshipManager _coBrandedSellSponsorshipManager { get; set; }
        #endregion

        #region 建構子
        public CoBrandedSellSponsorshipController()
        {
            _coBrandedSellSponsorshipManager = new CoBrandedSellSponsorshipManager();
        }
        #endregion

        public ActionResult Index()
        {
            ViewBag.UserData = ((FormsIdentity)User.Identity).Ticket.UserData;

            return View();
        }

        /// <summary>
        /// 查詢聯名卡行銷贊助金年度清單
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult QueryRange()
        {
            #region 設定變數

            ExecInfo ei_all = new ExecInfo();
            ExecInfo ei_detl = new ExecInfo();
            DataTable dt_QueryResult = null;

            #endregion

            #region 設定tag
            ei_all.Tag_M = "查詢[聯名卡行銷贊助金]的年度資料";
            #endregion

            try
            {
                #region 查詢自串點數特店主檔

                ei_all.Tag_D = "查詢自串點數特店主檔";
                ei_detl.fun_reset();
                dt_QueryResult = _coBrandedSellSponsorshipManager.get_CBSS_RangeData(ref ei_detl);

                if (ei_detl.RtnResult == false)
                { throw new CustomException.ExecErr(ei_detl.RtnMsg); }

                #endregion

                #region 執行成功
                ei_all.RtnResult = true;
                #endregion
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
            }

            return PartialView("_PartialQueryRange");
        }

        /// <summary>
        /// 聯名卡行銷贊助金年度區間資料存檔
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SaveRangeData(SaveRangeDataReq srdrq)
        {
            #region 設定變數

            ExecInfo ei_all = new ExecInfo();
            ExecInfo ei_detl = new ExecInfo();
            DataTable dt_QueryResult = null;

            #endregion

            #region 設定tag
            ei_all.Tag_M = "聯名卡行銷贊助金年度區間資料存檔";
            #endregion

            try
            {
                #region 取得使用者、IP

                ei_all.Tag_D = "取得使用者";
                srdrq.SaveUser = this.User.Identity.Name;

                ei_all.Tag_D = "取得IP";
                string[] array_REMOTE_ADDR = Request.ServerVariables.GetValues("REMOTE_ADDR");

                if (array_REMOTE_ADDR != null &&
                    array_REMOTE_ADDR.Length > 0)
                {
                    srdrq.SaveIP = array_REMOTE_ADDR[0];
                }
                else
                {
                    srdrq.SaveIP = "WEBUI";
                }

                #endregion

                #region 資料存檔

                ei_all.Tag_D = "資料存檔";
                ei_detl.fun_reset();

                _coBrandedSellSponsorshipManager.save_CBSS_RangeData(ref ei_detl,
                                                                     srdrq);

                if (ei_detl.RtnResult == false)
                { throw new CustomException.ExecErr(ei_detl.RtnMsg); }

                #endregion

                #region 執行成功
                ei_all.RtnResult = true;
                #endregion
            }
            catch (Exception ex)
            {
                ei_all.RtnResult = false;
                ei_all.RtnMsg = ei_all.fun_combine_err_msg(ex.Message);

                log.Error(ei_all.RtnMsg);
            }

            return Json(new { ExecInfo = ei_all });
        }

        public ActionResult IndexBankData(QueryBankDataReq qbdrq)
        {
            #region 設定變數

            ExecInfo ei_all = new ExecInfo();
            ExecInfo ei_detl = new ExecInfo();
            DataTable dt_QueryResult = null;

            #endregion

            #region 設定tag
            ei_all.Tag_M = "IndexBankData";
            #endregion

            try
            {
                #region 查詢自串點數特店主檔

                ei_all.Tag_D = "查詢銀行特店清單";
                ei_detl.fun_reset();
                dt_QueryResult = _coBrandedSellSponsorshipManager.get_CBSS_BankMerchantData(ref ei_detl);

                if (ei_detl.RtnResult == false)
                { throw new CustomException.ExecErr(ei_detl.RtnMsg); }

                #endregion

                #region 執行成功
                ei_all.RtnResult = true;
                #endregion
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
                ViewBag.UserData = ((FormsIdentity)User.Identity).Ticket.UserData;
            }

            return View(qbdrq);
        }

        /// <summary>
        /// 查詢聯名卡行銷贊助金年度區間銀行清單
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult QueryBankData(QueryBankDataReq qbdrq)
        {
            #region 設定變數

            ExecInfo ei_all = new ExecInfo();
            ExecInfo ei_detl = new ExecInfo();
            DataTable dt_QueryResult = null;

            #endregion

            #region 設定tag
            ei_all.Tag_M = "查詢聯名卡行銷贊助金年度區間銀行清單";
            #endregion

            try
            {
                #region 查詢聯名卡行銷贊助金年度區間銀行清單

                ei_all.Tag_D = "自DB查詢聯名卡行銷贊助金年度區間銀行清單";
                ei_detl.fun_reset();
                dt_QueryResult = _coBrandedSellSponsorshipManager.get_CBSS_BankData(ref ei_detl,
                                                                                    qbdrq);

                if (ei_detl.RtnResult == false)
                { throw new CustomException.ExecErr(ei_detl.RtnMsg); }

                #endregion

                #region 執行成功
                ei_all.RtnResult = true;
                #endregion
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
                ViewBag.QueryRequest = qbdrq;
                ViewBag.QueryResult = dt_QueryResult;
            }

            return PartialView("_PartialQueryBankData");
        }

        /// <summary>
        /// 聯名卡行銷贊助金銀行特約機構資料存檔
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SaveBankData(SaveBankDataReq sbdrq)
        {
            #region 設定變數

            ExecInfo ei_all = new ExecInfo();
            ExecInfo ei_detl = new ExecInfo();
            DataTable dt_QueryResult = null;

            #endregion

            #region 設定tag
            ei_all.Tag_M = "聯名卡行銷贊助金銀行特約機構資料存檔";
            #endregion

            try
            {
                #region 取得使用者、IP

                ei_all.Tag_D = "取得使用者";
                sbdrq.SaveUser = this.User.Identity.Name;

                ei_all.Tag_D = "取得IP";
                string[] array_REMOTE_ADDR = Request.ServerVariables.GetValues("REMOTE_ADDR");

                if (array_REMOTE_ADDR != null &&
                    array_REMOTE_ADDR.Length > 0)
                {
                    sbdrq.SaveIP = array_REMOTE_ADDR[0];
                }
                else
                {
                    sbdrq.SaveIP = "WEBUI";
                }

                #endregion

                #region 資料存檔

                ei_all.Tag_D = "資料存檔";
                ei_detl.fun_reset();

                _coBrandedSellSponsorshipManager.save_CBSS_BankData(ref ei_detl,
                                                                    sbdrq);

                if (ei_detl.RtnResult == false)
                { throw new CustomException.ExecErr(ei_detl.RtnMsg); }

                #endregion

                #region 執行成功
                ei_all.RtnResult = true;
                #endregion
            }
            catch (Exception ex)
            {
                ei_all.RtnResult = false;
                ei_all.RtnMsg = ei_all.fun_combine_err_msg(ex.Message);

                log.Error(ei_all.RtnMsg);
            }

            return Json(new { ExecInfo = ei_all });
        }

        public ActionResult IndexQuotaTrack(QueryBankDataReq qbdrq)
        {
            #region 設定變數

            ExecInfo ei_all = new ExecInfo();
            ExecInfo ei_detl = new ExecInfo();
            DataTable dt_QueryResult = null;

            #endregion

            #region 設定tag
            ei_all.Tag_M = "進入額度、請款明細頁面，查詢支付對象明細";
            #endregion

            try
            {
                #region 查詢聯名卡行銷贊助金支付對象明細

                ei_all.Tag_D = "自DB查詢聯名卡行銷贊助金支付對象明細";
                ei_detl.fun_reset();
                dt_QueryResult = _coBrandedSellSponsorshipManager.get_CBSS_PayTarget(ref ei_detl);

                if (ei_detl.RtnResult == false)
                { throw new CustomException.ExecErr(ei_detl.RtnMsg); }

                #endregion

                #region 執行成功
                ei_all.RtnResult = true;
                #endregion
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
                ViewBag.UserData = ((FormsIdentity)User.Identity).Ticket.UserData;
            }

            return View(qbdrq);
        }

        /// <summary>
        /// 查詢聯名卡行銷贊助金銀行明細
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult QueryQuotaTrack(QueryBankDataReq qbdrq)
        {
            #region 設定變數

            ExecInfo ei_all = new ExecInfo();
            ExecInfo ei_detl = new ExecInfo();
            DataSet ds_QueryResult = null;

            #endregion

            #region 設定tag
            ei_all.Tag_M = "查詢聯名卡行銷贊助金銀行明細";
            #endregion

            try
            {
                #region 查詢聯名卡行銷贊助金銀行明細

                ei_all.Tag_D = "自DB查詢聯名卡行銷贊助金銀行明細";
                ei_detl.fun_reset();
                ds_QueryResult = _coBrandedSellSponsorshipManager.get_CBSS_QuotaTrack(ref ei_detl,
                                                                                      qbdrq);

                if (ei_detl.RtnResult == false)
                { throw new CustomException.ExecErr(ei_detl.RtnMsg); }

                #endregion

                #region 執行成功
                ei_all.RtnResult = true;
                #endregion
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
                ViewBag.QueryRequest = qbdrq;
                ViewBag.QueryResult = ds_QueryResult;
            }

            return PartialView("_PartialQueryQuotaTrack");
        }

        public ActionResult IndexMaintainPayTarget(QueryBankDataReq qbdrq)
        {
            return View(qbdrq);
        }

        /// <summary>
        /// 查詢聯名卡行銷贊助金支付對象明細
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult QueryPayTarget()
        {
            #region 設定變數

            ExecInfo ei_all = new ExecInfo();
            ExecInfo ei_detl = new ExecInfo();
            DataTable dt_QueryResult = null;

            #endregion

            #region 設定tag
            ei_all.Tag_M = "查詢聯名卡行銷贊助金支付對象明細";
            #endregion

            try
            {
                #region 查詢聯名卡行銷贊助金支付對象明細

                ei_all.Tag_D = "自DB查詢聯名卡行銷贊助金支付對象明細";
                ei_detl.fun_reset();
                dt_QueryResult = _coBrandedSellSponsorshipManager.get_CBSS_PayTarget(ref ei_detl);

                if (ei_detl.RtnResult == false)
                { throw new CustomException.ExecErr(ei_detl.RtnMsg); }

                #endregion

                #region 執行成功
                ei_all.RtnResult = true;
                #endregion
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
            }

            return PartialView("_PartialQueryPayTarget");
        }

        /// <summary>
        /// 聯名卡行銷贊助金支付對象存檔
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SavePayTarget(SavePayTargetReq sptrq)
        {
            #region 設定變數

            ExecInfo ei_all = new ExecInfo();
            ExecInfo ei_detl = new ExecInfo();
            DataTable dt_QueryResult = null;

            #endregion

            #region 設定tag
            ei_all.Tag_M = "聯名卡行銷贊助金支付對象存檔";
            #endregion

            try
            {
                #region 取得使用者、IP

                ei_all.Tag_D = "取得使用者";
                sptrq.SaveUser = this.User.Identity.Name;

                ei_all.Tag_D = "取得IP";
                string[] array_REMOTE_ADDR = Request.ServerVariables.GetValues("REMOTE_ADDR");

                if (array_REMOTE_ADDR != null &&
                    array_REMOTE_ADDR.Length > 0)
                {
                    sptrq.SaveIP = array_REMOTE_ADDR[0];
                }
                else
                {
                    sptrq.SaveIP = "WEBUI";
                }

                #endregion

                #region 資料存檔

                ei_all.Tag_D = "資料存檔";
                ei_detl.fun_reset();

                _coBrandedSellSponsorshipManager.save_CBSS_PayTarget(ref ei_detl,
                                                                     sptrq);

                if (ei_detl.RtnResult == false)
                { throw new CustomException.ExecErr(ei_detl.RtnMsg); }

                #endregion

                #region 執行成功
                ei_all.RtnResult = true;
                #endregion
            }
            catch (Exception ex)
            {
                ei_all.RtnResult = false;
                ei_all.RtnMsg = ei_all.fun_combine_err_msg(ex.Message);

                log.Error(ei_all.RtnMsg);
            }

            return Json(new { ExecInfo = ei_all });
        }

        /// <summary>
        /// 聯名卡行銷贊助金額度存檔
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SaveDetailQuota(SaveDetailQuotaReq sdqreq)
        {
            #region 設定變數

            ExecInfo ei_all = new ExecInfo();
            ExecInfo ei_detl = new ExecInfo();
            DataTable dt_QueryResult = null;

            #endregion

            #region 設定tag
            ei_all.Tag_M = "聯名卡行銷贊助金額度存檔";
            #endregion

            try
            {
                #region 取得使用者、IP

                ei_all.Tag_D = "取得使用者";
                sdqreq.SaveUser = this.User.Identity.Name;

                ei_all.Tag_D = "取得IP";
                string[] array_REMOTE_ADDR = Request.ServerVariables.GetValues("REMOTE_ADDR");

                if (array_REMOTE_ADDR != null &&
                    array_REMOTE_ADDR.Length > 0)
                {
                    sdqreq.SaveIP = array_REMOTE_ADDR[0];
                }
                else
                {
                    sdqreq.SaveIP = "WEBUI";
                }

                #endregion

                #region 資料存檔

                ei_all.Tag_D = "資料存檔";
                ei_detl.fun_reset();

                _coBrandedSellSponsorshipManager.save_CBSS_DetailQuota(ref ei_detl,
                                                                       sdqreq);

                if (ei_detl.RtnResult == false)
                { throw new CustomException.ExecErr(ei_detl.RtnMsg); }

                #endregion

                #region 執行成功
                ei_all.RtnResult = true;
                #endregion
            }
            catch (Exception ex)
            {
                ei_all.RtnResult = false;
                ei_all.RtnMsg = ei_all.fun_combine_err_msg(ex.Message);

                log.Error(ei_all.RtnMsg);
            }

            return Json(new { ExecInfo = ei_all });
        }

        /// <summary>
        /// 聯名卡行銷贊助金請款項目存檔
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SaveDetailTrack(SaveDetailTrackReq sdtreq)
        {
            #region 設定變數

            ExecInfo ei_all = new ExecInfo();
            ExecInfo ei_detl = new ExecInfo();
            DataTable dt_QueryResult = null;

            #endregion

            #region 設定tag
            ei_all.Tag_M = "聯名卡行銷贊助金請款項目存檔";
            #endregion

            try
            {
                #region 取得使用者、IP

                ei_all.Tag_D = "取得使用者";
                sdtreq.SaveUser = this.User.Identity.Name;

                ei_all.Tag_D = "取得IP";
                string[] array_REMOTE_ADDR = Request.ServerVariables.GetValues("REMOTE_ADDR");

                if (array_REMOTE_ADDR != null &&
                    array_REMOTE_ADDR.Length > 0)
                {
                    sdtreq.SaveIP = array_REMOTE_ADDR[0];
                }
                else
                {
                    sdtreq.SaveIP = "WEBUI";
                }

                #endregion

                #region 資料存檔

                ei_all.Tag_D = "資料存檔";
                ei_detl.fun_reset();

                _coBrandedSellSponsorshipManager.save_CBSS_DetailTrack(ref ei_detl,
                                                                       sdtreq);

                if (ei_detl.RtnResult == false)
                { throw new CustomException.ExecErr(ei_detl.RtnMsg); }

                #endregion

                #region 執行成功
                ei_all.RtnResult = true;
                #endregion
            }
            catch (Exception ex)
            {
                ei_all.RtnResult = false;
                ei_all.RtnMsg = ei_all.fun_combine_err_msg(ex.Message);

                log.Error(ei_all.RtnMsg);
            }

            return Json(new { ExecInfo = ei_all });
        }

        /// <summary>
        /// 聯名卡行銷贊助金匯出
        /// </summary>
        /// <returns></returns>
        public ActionResult Export(QueryBankDataReq qbdrq)
        {
            #region 設定變數

            ExecInfo ei_all = new ExecInfo();
            ExecInfo ei_detl = new ExecInfo();
            XmlDocument doc_XmlDocument = null;
            XDocument doc_XDocument = null;
            byte[] rtn_byte_data = null;
            string rtn_filename = null;

            #endregion

            #region 設定tag
            ei_all.Tag_M = "聯名卡行銷贊助金匯出";
            #endregion

            try
            {
                #region 轉換資料

                ei_all.Tag_D = "轉換資料";
                //前端傳過來的是Json字串，將之轉換為XML物件
                doc_XmlDocument = Newtonsoft.Json.JsonConvert.DeserializeXmlNode(qbdrq.ExcludeData);
                doc_XDocument = XDocument.Parse(doc_XmlDocument.OuterXml);

                #endregion

                #region 匯出Excel檔

                ei_all.Tag_D = "匯出Excel檔";
                ei_detl.fun_reset();

                _coBrandedSellSponsorshipManager.export_CBSS_Data(ref ei_detl,
                                                                  qbdrq,
                                                                  doc_XDocument,
                                                                  out rtn_byte_data,
                                                                  out rtn_filename);

                if (ei_detl.RtnResult == false)
                { throw new CustomException.ExecErr(ei_detl.RtnMsg); }

                #endregion

                #region 執行成功
                ei_all.RtnResult = true;
                #endregion
            }
            catch (Exception ex)
            {
                ei_all.RtnResult = false;
                ei_all.RtnMsg = ei_all.fun_combine_err_msg(ex.Message);

                log.Error(ei_all.RtnMsg);
            }

            #region 判斷執行結果進行回傳

            if (ei_all.RtnResult == true)
            {
                //成功，回傳檔案
                return File(rtn_byte_data, System.Net.Mime.MediaTypeNames.Application.Octet, rtn_filename);
            }
            else
            {
                //失敗，回傳錯誤訊息
                return Content(ei_all.RtnMsg);
            }

            #endregion
        }


    }
}