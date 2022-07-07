using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

using Domain.Common;
using Domain.ICASHOPMerchant;

using log4net;

namespace DataAccess
{
    public class ICASHOPMerchantDAO : BasicOPDAL
    {
        #region 設定變數
        private static readonly ILog log = LogManager.GetLogger(typeof(ICASHOPMerchantDAO));
        #endregion

        /// <summary>
        /// 建構子
        /// </summary>
        public ICASHOPMerchantDAO() { }

        /// <summary>
        /// 取得自串點數特店主檔資料
        /// </summary>
        /// <param name="ei">執行訊息</param>
        /// <param name="qr">查詢條件</param>
        /// <param name="pcv">分頁資訊</param>
        /// <returns></returns>
        public DataTable get_MerchantData(ref ExecInfo ei,
                                          QueryReq qr,
                                          PageCountViewModel pcv)
        {
            #region 設定tag
            ei.Tag_M = "連線資料庫，取得自串點數特店主檔資料";
            #endregion

            #region 設定變數

            ei.Tag_D = "設定變數";
            DataTable dt_rtn = new DataTable();
            string s_sql = string.Empty;
            List<SqlParameter> li_para = new List<SqlParameter>();
            CommonDAO cdao = new CommonDAO();

            #endregion

            try
            {
                #region 組合SQL字串

                ei.Tag_D = "組合SQL字串";
                s_sql = @"ausp_ICashOP_OpenPoint_MerchantData_S";

                #endregion

                #region 處理變數

                ei.Tag_D = "處理變數";
                li_para.Add(new SqlParameter("@PageSize", cdao.Convert_Sql_Parameter(pcv.PageSize.ToString(), CommonDAO.en_convert_type.en_int, string.Empty)));
                li_para.Add(new SqlParameter("@CurrentPage", cdao.Convert_Sql_Parameter(qr.CurrentPage.ToString(), CommonDAO.en_convert_type.en_int, string.Empty)));
                li_para.Add(new SqlParameter("@UnifiedBusinessNo", cdao.Convert_Sql_Parameter(qr.UnifiedBusinessNo, CommonDAO.en_convert_type.en_string, string.Empty)));
                li_para.Add(new SqlParameter("@MerchantName", cdao.Convert_Sql_Parameter(qr.MerchantName, CommonDAO.en_convert_type.en_string, string.Empty)));

                #endregion

                #region 連接資料庫

                ei.Tag_D = "連接資料庫";
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    using (SqlCommand sqlCmd = new SqlCommand(s_sql, sqlConn))
                    {
                        sqlCmd.CommandType = CommandType.StoredProcedure;

                        ei.Tag_D = "連接資料庫，傳入參數";
                        foreach (SqlParameter para_detl in li_para)
                        { sqlCmd.Parameters.Add(para_detl); }

                        ei.Tag_D = "連接資料庫，開啟連線";
                        sqlConn.Open();

                        ei.Tag_D = "連接資料庫，執行資料庫作業";
                        SqlDataAdapter adapter = new SqlDataAdapter(sqlCmd);
                        adapter.Fill(dt_rtn);
                    }
                }

                #endregion

                #region 執行成功
                ei.RtnResult = true;
                #endregion
            }
            catch (Exception ex)
            {
                #region 執行失敗
                ei.RtnResult = false;
                ei.RtnMsg = ei.fun_combine_err_msg(ex.Message);
                #endregion

                log.Debug(ex.StackTrace);
                log.Debug(ex.ToString());
            }

            #region 回傳結果

            return dt_rtn;

            #endregion
        }

        /// <summary>
        /// 新增自串點數特店主檔
        /// </summary>
        /// <param name="ei">執行訊息</param>
        /// <param name="esr">新增自串點數特店主檔資料</param>
        public void add_MerchantData(ref ExecInfo ei,
                                     EditSaveReq esr)
        {
            #region 設定tag
            ei.Tag_M = "連線資料庫，新增自串點數特店主檔";
            #endregion

            #region 設定變數

            ei.Tag_D = "設定變數";
            DataTable dt_rtn = new DataTable();
            string s_sql = string.Empty;
            List<SqlParameter> li_para = new List<SqlParameter>();
            CommonDAO cdao = new CommonDAO();

            #endregion

            try
            {
                #region 組合SQL字串

                ei.Tag_D = "組合SQL字串";
                s_sql = @"ausp_ICashOP_OpenPoint_MerchantData_I";

                #endregion

                #region 處理變數

                ei.Tag_D = "處理變數";
                li_para.Clear();
                li_para.Add(new SqlParameter("@EditJsonData", cdao.Convert_Sql_Parameter(esr.EditJsonData, CommonDAO.en_convert_type.en_string, string.Empty)));
                li_para.Add(new SqlParameter("@User", cdao.Convert_Sql_Parameter(esr.User, CommonDAO.en_convert_type.en_string, string.Empty)));
                li_para.Add(new SqlParameter("@IP", cdao.Convert_Sql_Parameter(esr.IP, CommonDAO.en_convert_type.en_string, string.Empty)));

                #endregion

                #region 連接資料庫

                ei.Tag_D = "連接資料庫";
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    using (SqlCommand sqlCmd = new SqlCommand(s_sql, sqlConn))
                    {
                        sqlCmd.CommandType = CommandType.StoredProcedure;

                        ei.Tag_D = "連接資料庫，傳入參數";
                        foreach (SqlParameter para_detl in li_para)
                        { sqlCmd.Parameters.Add(para_detl); }

                        ei.Tag_D = "連接資料庫，開啟連線";
                        sqlConn.Open();

                        ei.Tag_D = "連接資料庫，執行資料庫作業";
                        SqlDataAdapter adapter = new SqlDataAdapter(sqlCmd);
                        adapter.Fill(dt_rtn);
                    }
                }

                #endregion

                #region 檢核回傳資訊

                ei.Tag_D = "檢核回傳資訊";
                if (dt_rtn.Rows[0]["RTN_RESULT"].ToString() != "OK")
                {
                    throw new Exception(dt_rtn.Rows[0]["RTN_MSG"].ToString());
                }

                #endregion

                #region 執行成功
                ei.RtnResult = true;
                #endregion
            }
            catch (Exception ex)
            {
                #region 執行失敗
                ei.RtnResult = false;
                ei.RtnMsg = ei.fun_combine_err_msg(ex.Message);
                #endregion

                log.Debug(ex.StackTrace);
                log.Debug(ex.ToString());
            }
        }

        /// <summary>
        /// 更新自串點數特店主檔
        /// </summary>
        /// <param name="ei">執行訊息</param>
        /// <param name="esr">更新自串點數特店主檔資料</param>
        public void modify_MerchantData(ref ExecInfo ei,
                                        EditSaveReq esr)
        {
            #region 設定tag
            ei.Tag_M = "連線資料庫，新增自串點數特店主檔";
            #endregion

            #region 設定變數

            ei.Tag_D = "設定變數";
            DataTable dt_rtn = new DataTable();
            string s_sql = string.Empty;
            List<SqlParameter> li_para = new List<SqlParameter>();
            CommonDAO cdao = new CommonDAO();

            #endregion

            try
            {
                #region 組合SQL字串

                ei.Tag_D = "組合SQL字串";
                s_sql = @"ausp_ICashOP_OpenPoint_MerchantData_U";

                #endregion

                #region 處理變數

                ei.Tag_D = "處理變數";
                li_para.Add(new SqlParameter("@EditJsonData", cdao.Convert_Sql_Parameter(esr.EditJsonData, CommonDAO.en_convert_type.en_string, string.Empty)));
                li_para.Add(new SqlParameter("@User", cdao.Convert_Sql_Parameter(esr.User, CommonDAO.en_convert_type.en_string, string.Empty)));
                li_para.Add(new SqlParameter("@IP", cdao.Convert_Sql_Parameter(esr.IP, CommonDAO.en_convert_type.en_string, string.Empty)));

                #endregion

                #region 連接資料庫

                ei.Tag_D = "連接資料庫";
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    using (SqlCommand sqlCmd = new SqlCommand(s_sql, sqlConn))
                    {
                        sqlCmd.CommandType = CommandType.StoredProcedure;

                        ei.Tag_D = "連接資料庫，傳入參數";
                        foreach (SqlParameter para_detl in li_para)
                        { sqlCmd.Parameters.Add(para_detl); }

                        ei.Tag_D = "連接資料庫，開啟連線";
                        sqlConn.Open();

                        ei.Tag_D = "連接資料庫，執行資料庫作業";
                        SqlDataAdapter adapter = new SqlDataAdapter(sqlCmd);
                        adapter.Fill(dt_rtn);
                    }
                }

                #endregion

                #region 檢核回傳資訊

                ei.Tag_D = "檢核回傳資訊";
                if (dt_rtn.Rows[0]["RTN_RESULT"].ToString() != "OK")
                {
                    throw new Exception(dt_rtn.Rows[0]["RTN_MSG"].ToString());
                }

                #endregion

                #region 執行成功
                ei.RtnResult = true;
                #endregion
            }
            catch (Exception ex)
            {
                #region 執行失敗
                ei.RtnResult = false;
                ei.RtnMsg = ei.fun_combine_err_msg(ex.Message);
                #endregion

                log.Debug(ex.StackTrace);
                log.Debug(ex.ToString());
            }
        }



        /// <summary>
        /// 同步同一個統編的Merchant及Store資訊
        /// </summary>
        /// <param name="ei">執行訊息</param>
        /// <param name="esr">同一個統編的資訊</param>
        public void synchronize_Merchant(ref ExecInfo ei,
                                        EditSaveReq esr)
        {
            #region 設定tag
            ei.Tag_M = "連線資料庫，同步同一個統編的Merchant及Store資訊";
            #endregion

            #region 設定變數

            ei.Tag_D = "設定變數";
            DataTable dt_rtn = new DataTable();
            string s_sql = string.Empty;
            List<SqlParameter> li_para = new List<SqlParameter>();
            CommonDAO cdao = new CommonDAO();

            #endregion

            try
            {
                #region 組合SQL字串

                ei.Tag_D = "組合SQL字串";
                s_sql = @"ausp_ICashOP_OpenPoint_MerchantSynchronize_IU";

                #endregion

                #region 處理變數

                ei.Tag_D = "處理變數";
                li_para.Add(new SqlParameter("@EditJsonData", cdao.Convert_Sql_Parameter(esr.EditJsonData, CommonDAO.en_convert_type.en_string, string.Empty)));

                #endregion

                #region 連接資料庫

                ei.Tag_D = "連接資料庫";
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    using (SqlCommand sqlCmd = new SqlCommand(s_sql, sqlConn))
                    {
                        sqlCmd.CommandType = CommandType.StoredProcedure;

                        ei.Tag_D = "連接資料庫，傳入參數";
                        foreach (SqlParameter para_detl in li_para)
                        { sqlCmd.Parameters.Add(para_detl); }

                        ei.Tag_D = "連接資料庫，開啟連線";
                        sqlConn.Open();

                        ei.Tag_D = "連接資料庫，執行資料庫作業";
                        SqlDataAdapter adapter = new SqlDataAdapter(sqlCmd);
                        adapter.Fill(dt_rtn);
                    }
                }

                #endregion

                #region 檢核回傳資訊

                ei.Tag_D = "檢核回傳資訊";
                if (dt_rtn.Rows[0]["RTN_RESULT"].ToString() != "OK")
                {
                    throw new Exception(dt_rtn.Rows[0]["RTN_MSG"].ToString());
                }

                #endregion

                #region 執行成功
                ei.RtnResult = true;
                #endregion
            }
            catch (Exception ex)
            {
                #region 執行失敗
                ei.RtnResult = false;
                ei.RtnMsg = ei.fun_combine_err_msg(ex.Message);
                #endregion

                log.Debug(ex.StackTrace);
                log.Debug(ex.ToString());
            }
        }
    }
}
