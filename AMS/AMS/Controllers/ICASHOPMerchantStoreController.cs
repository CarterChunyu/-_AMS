using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;

using Business;
using Domain.Common;
using Domain.ICASHOPMerchant;
using Domain.ICASHOPMerchantStore;

using log4net;

namespace AMS.Controllers
{
    [CustomAuthorize(AccessLevel = "System,Sales,SalesManager,")]
    public class ICASHOPMerchantStoreController : BaseController
    {
        #region 設定全域變數
        private static readonly ILog log = LogManager.GetLogger(typeof(ICASHOPMerchantStoreController));
        private ICASHOPMerchantManager icashopMerchantManager { get; set; }
        private ICASHOPMerchantStoreManager icashopMerchantStoreManager { get; set; }
        #endregion

        #region 建構子
        public ICASHOPMerchantStoreController()
        {
            icashopMerchantManager = new ICASHOPMerchantManager();
            icashopMerchantStoreManager = new ICASHOPMerchantStoreManager();
        }
        #endregion

        public ActionResult Index()
        {
            return View();
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

            ExecInfo ei_all = new ExecInfo();
            ExecInfo ei_detl = new ExecInfo();
            DataTable dt_QueryResult = null;
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

        /// <summary>
        /// 下載
        /// </summary>
        /// <param name="dlr"></param>
        /// <returns></returns>
        public ActionResult FileDownload(DownloadReq dlr)
        {
            #region 設定變數

            ExecInfo ei_all = new ExecInfo();
            ExecInfo ei_detl = new ExecInfo();
            byte[] rtn_byte_data = null;
            string rtn_filename = null;
            FileInfo fi_Template = null;

            #endregion

            #region 設定tag
            ei_all.Tag_M = "下載";
            #endregion

            #region 設定範本資訊

            ei_all.Tag_D = "設定範本資訊";
            fi_Template = new FileInfo(Server.MapPath(@"~/App_Data/ICASHOPMerchantStore/FileTemplate/Template.xlsx"));

            #endregion

            try
            {
                ei_detl.fun_reset();

                switch (dlr.DownloadType)
                {
                    #region 下載範例檔

                    case DownloadReq.en_DownloadType.Template:
                        ei_all.Tag_D = "下載範例檔";
                        icashopMerchantStoreManager.file_Download_Template(ref ei_detl,
                                                                           fi_Template,
                                                                           out rtn_byte_data,
                                                                           out rtn_filename);
                        break;

                    #endregion

                    #region 下載門市明細資料

                    case DownloadReq.en_DownloadType.Store:
                        ei_all.Tag_D = "下載門市明細資料";
                        icashopMerchantStoreManager.file_Download_MerchantStoreDetail(ref ei_detl,
                                                                                      fi_Template,
                                                                                      dlr.UnifiedBusinessNo,
                                                                                      out rtn_byte_data,
                                                                                      out rtn_filename);
                        break;

                        #endregion
                }

                if (ei_detl.RtnResult == false)
                { throw new CustomException.ExecErr(ei_detl.RtnMsg); }

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

        /// <summary>
        /// 按下[匯入]鈕
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Import()
        {
            #region 設定變數

            ExecInfo ei_all = new ExecInfo();
            ExecInfo ei_detl = new ExecInfo();
            TaskInfo taskInfo = new TaskInfo();

            string s_UploadFileName = string.Empty;
            DataTable dt_data;
            string s_RtnMsg = string.Empty;

            List<ImportFileInfo> li_ifi = new List<ImportFileInfo>();

            #endregion

            #region 設定tag
            ei_all.Tag_M = "按下[匯入]鈕";
            #endregion

            try
            {
                #region 取得使用者、IP

                ei_all.Tag_D = "取得使用者、IP";
                ei_detl.fun_reset();

                try
                {
                    #region 設定Tag
                    ei_detl.Tag_M = "取得使用者、IP";
                    #endregion

                    #region 取得使用者

                    ei_detl.Tag_D = "取得使用者";
                    taskInfo.User = this.User.Identity.Name;

                    #endregion

                    #region 取得IP

                    ei_detl.Tag_D = "取得IP";
                    string[] array_REMOTE_ADDR = Request.ServerVariables.GetValues("REMOTE_ADDR");

                    if (array_REMOTE_ADDR != null && array_REMOTE_ADDR.Length > 0)
                    {
                        taskInfo.IP = array_REMOTE_ADDR[0];
                    }
                    else
                    {
                        taskInfo.IP = "WEBUI";
                    }

                    #endregion

                    #region 執行成功
                    ei_detl.RtnResult = true;
                    #endregion
                }
                catch (Exception ex)
                {
                    ei_detl.RtnResult = false;
                    ei_detl.RtnMsg = ei_detl.fun_combine_err_msg(ex.Message);
                }

                if (ei_detl.RtnResult == false)
                { throw new CustomException.ExecErr(ei_detl.RtnMsg); }

                #endregion

                #region 作業開始

                ei_all.Tag_D = "作業開始";
                ei_detl.fun_reset();

                icashopMerchantStoreManager.Task_Begin(ref ei_detl,
                                                       ref taskInfo);

                if (ei_detl.RtnResult == false)
                { throw new CustomException.ExecErr(ei_detl.RtnMsg); }

                #endregion

                #region 將上傳匯入檔案存入AP主機

                ei_all.Tag_D = "將上傳匯入檔案存入AP主機";
                ei_detl.fun_reset();

                icashopMerchantStoreManager.UploadImportFile(ref ei_detl,
                                                             taskInfo,
                                                             Request.Files,
                                                             Server.MapPath("~/App_Data/ICASHOPMerchantStore/FileUpload"),
                                                             ref li_ifi);

                if (ei_detl.RtnResult == false)
                { throw new CustomException.ExecErr(ei_detl.RtnMsg); }

                #endregion

                #region 取得匯入檔案資料

                ei_all.Tag_D = "取得匯入檔案資料";
                ei_detl.fun_reset();

                icashopMerchantStoreManager.GetImportData(ref ei_detl,
                                                          taskInfo,
                                                          li_ifi,
                                                          out dt_data);

                if (ei_detl.RtnResult == false)
                { throw new CustomException.ExecErr(ei_detl.RtnMsg); }

                #endregion

                #region 將匯入檔案資料寫入資料庫

                ei_all.Tag_D = "將匯入檔案資料寫入資料庫";
                ei_detl.fun_reset();

                icashopMerchantStoreManager.import_MerchantStoreData(ref ei_detl,
                                                                     taskInfo,
                                                                     ref li_ifi,
                                                                     dt_data);

                if (ei_detl.RtnResult == false)
                { throw new CustomException.ExecErr(ei_detl.RtnMsg); }
                else
                {
                    ei_all.RtnMsg = ei_detl.RtnMsg;
                }

                #endregion

                #region 執行成功
                taskInfo.EXEC_STATUS = "OK";
                ei_all.RtnResult = true;
                #endregion
            }
            catch (CustomException.ExecErr ex_exec)
            {
                ei_all.RtnResult = false;
                ei_all.RtnMsg = ei_all.fun_combine_err_msg(ex_exec.Message);
            }
            catch (Exception ex)
            {
                ei_all.RtnResult = false;
                ei_all.RtnMsg = ei_all.fun_combine_err_msg(ex.Message);
            }
            finally
            {
                #region 作業結束

                ei_all.Tag_D = "作業結束";
                ei_detl.fun_reset();

                icashopMerchantStoreManager.Task_End(ref ei_detl,
                                                     taskInfo);

                if (ei_detl.RtnResult == false)
                {
                    ei_all.RtnResult = false;
                    ei_all.RtnMsg += ei_all.fun_combine_err_msg(ei_detl.RtnMsg);
                }

                #endregion

                #region 記錄Log

                log.Info(string.Format("{0}{1}{0}排程執行EXEC_TIME=[{2}]{3}",
                                       Environment.NewLine,
                                       new string('=', 50),
                                       taskInfo.EXEC_TIME,
                                       ei_all.RtnMsg.Replace("<br/>", Environment.NewLine)));

                #endregion
            }

            return Json(ei_all);
        }
    }
}