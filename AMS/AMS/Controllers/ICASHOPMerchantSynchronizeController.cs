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
using Domain.ICASHOPMerchantSynchronize;
using System.Data.SqlClient;
using DataAccess;

namespace AMS.Controllers
{
    
    public class ICASHOPMerchantSynchronizeController : Controller
    {
        #region 設定全域變數
        private static readonly ILog log = LogManager.GetLogger(typeof(ICASHOPMerchantSynchronizeController));
        private ICASHOPMerchantManager icashopMerchantManager { get; set; }
        #endregion

        #region 建構子
        public ICASHOPMerchantSynchronizeController()
        {
            icashopMerchantManager = new ICASHOPMerchantManager();
        }
        #endregion

        [HttpPost]
        public ActionResult Synchronize(SynchronizeRequest request)
        {
            #region 設定變數

            ExecInfo ei_all = new ExecInfo();
            ExecInfo ei_detl = new ExecInfo();

            #endregion

            #region 設定tag
            ei_all.Tag_M = "同步Merchant & Store";
            #endregion

            try
            {

                ei_all.Tag_D = "同步";
                var esr = new EditSaveReq()
                {
                    EditJsonData = request.JsonData
                };
                this.icashopMerchantManager.synchronize_Merchant(ref ei_detl,
                                                                esr);
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

            return Json(ei_all);
        }
    }
}