using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Xml.Linq;

using Domain.Common;
using Domain.CoBrandedSellSponsorship;

using log4net;

namespace DataAccess
{
    public class CoBrandedSellSponsorshipDAO : BasicDAL
    {
        #region 設定變數
        private static readonly ILog log = LogManager.GetLogger(typeof(ICASHOPMerchantDAO));
        #endregion

        /// <summary>
        /// 建構子
        /// </summary>
        public CoBrandedSellSponsorshipDAO() { }

        /// <summary>
        /// 取得聯名卡行銷贊助金年度資料
        /// </summary>
        /// <param name="ei"></param>
        /// <returns></returns>
        public DataTable get_CBSS_RangeData(ref ExecInfo ei)
        {
            #region 設定tag
            ei.Tag_M = "連線資料庫，取得聯名卡行銷贊助金年度資料";
            #endregion

            #region 設定變數

            ei.Tag_D = "設定變數";
            string s_sql = string.Empty;
            DataTable dt_rtn = new DataTable();
            CommonDAO cdao = new CommonDAO();

            #endregion

            try
            {
                #region 組合SQL字串

                ei.Tag_D = "組合SQL字串";
                s_sql = @"CBSS_GetRangeData";

                #endregion

                #region 連接資料庫

                ei.Tag_D = "連接資料庫";
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    using (SqlCommand sqlCmd = new SqlCommand(s_sql, sqlConn))
                    {
                        sqlCmd.CommandType = CommandType.StoredProcedure;

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
        /// 聯名卡行銷贊助金年度區間資料存檔
        /// </summary>
        /// <param name="ei">執行訊息</param>
        /// <param name="doc_data">存檔資訊</param>
        public void save_CBSS_RangeData(ref ExecInfo ei,
                                        XDocument doc_data)
        {
            #region 設定tag
            ei.Tag_M = "連線資料庫，聯名卡行銷贊助金年度區間資料存檔";
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
                s_sql = @"CBSS_SaveRangeData";

                #endregion

                #region 處理變數

                ei.Tag_D = "處理變數";
                li_para.Add(new SqlParameter("@DATA_XML", cdao.Convert_Sql_Parameter(doc_data.ToString(), CommonDAO.en_convert_type.en_string, string.Empty)));

                #endregion

                #region 連接資料庫

                ei.Tag_D = "連接資料庫";
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    using (SqlCommand sqlCmd = new SqlCommand(s_sql, sqlConn))
                    {
                        ei.Tag_D = "連接資料庫，設定Command類型";
                        sqlCmd.CommandType = CommandType.StoredProcedure;

                        ei.Tag_D = "連接資料庫，傳入參數";
                        foreach (SqlParameter para_detl in li_para)
                        { sqlCmd.Parameters.Add(para_detl); }

                        sqlCmd.Parameters[0].DbType = DbType.Xml;

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
        /// 取得聯名卡行銷贊助金銀行特店清單
        /// </summary>
        /// <param name="ei">執行訊息</param>
        /// <returns></returns>
        public DataTable get_CBSS_BankMerchantData(ref ExecInfo ei)
        {
            #region 設定tag
            ei.Tag_M = "連線資料庫，取得聯名卡行銷贊助金銀行特店清單";
            #endregion

            #region 設定變數

            ei.Tag_D = "設定變數";
            DataTable dt_rtn = new DataTable();
            string s_sql = string.Empty;
            CommonDAO cdao = new CommonDAO();

            #endregion

            try
            {
                #region 組合SQL字串

                ei.Tag_D = "組合SQL字串";
                s_sql = @"CBSS_GetBankMerchantData";

                #endregion

                #region 連接資料庫

                ei.Tag_D = "連接資料庫";
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    using (SqlCommand sqlCmd = new SqlCommand(s_sql, sqlConn))
                    {
                        sqlCmd.CommandType = CommandType.StoredProcedure;

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
        /// 取得聯名卡行銷贊助金年度區間銀行清單
        /// </summary>
        /// <param name="ei">執行訊息</param>
        /// <param name="qbdrq">查詢條件</param>
        /// <returns></returns>
        public DataTable get_CBSS_BankData(ref ExecInfo ei,
                                           QueryBankDataReq qbdrq)
        {
            #region 設定tag
            ei.Tag_M = "連線資料庫，聯名卡行銷贊助金年度區間銀行清單";
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
                s_sql = @"CBSS_GetBankData";

                #endregion

                #region 處理變數

                ei.Tag_D = "處理變數";
                li_para.Add(new SqlParameter("@RID", cdao.Convert_Sql_Parameter(qbdrq.RID.ToString(), CommonDAO.en_convert_type.en_int, string.Empty)));
                li_para.Add(new SqlParameter("@BID", cdao.Convert_Sql_Parameter((qbdrq.BID.HasValue == true ? qbdrq.BID.ToString() : string.Empty), CommonDAO.en_convert_type.en_int, string.Empty)));
                li_para.Add(new SqlParameter("@DATA_XML", DBNull.Value));

                #endregion

                #region 連接資料庫

                ei.Tag_D = "連接資料庫";
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    using (SqlCommand sqlCmd = new SqlCommand(s_sql, sqlConn))
                    {
                        sqlCmd.CommandType = CommandType.StoredProcedure;

                        ei.Tag_D = "連接資料庫，開啟連線";
                        sqlConn.Open();

                        ei.Tag_D = "連接資料庫，傳入參數";
                        foreach (SqlParameter para_detl in li_para)
                        { sqlCmd.Parameters.Add(para_detl); }

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
        /// 聯名卡行銷贊助金銀行特約機構資料存檔
        /// </summary>
        /// <param name="ei">執行訊息</param>
        /// <param name="doc_data">存檔資訊</param>
        public void save_CBSS_BankData(ref ExecInfo ei,
                                       XDocument doc_data)
        {
            #region 設定tag
            ei.Tag_M = "連線資料庫，聯名卡行銷贊助金銀行特約機構資料存檔";
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
                s_sql = @"CBSS_SaveBankData";

                #endregion

                #region 處理變數

                ei.Tag_D = "處理變數";
                li_para.Add(new SqlParameter("@DATA_XML", cdao.Convert_Sql_Parameter(doc_data.ToString(), CommonDAO.en_convert_type.en_string, string.Empty)));

                #endregion

                #region 連接資料庫

                ei.Tag_D = "連接資料庫";
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    using (SqlCommand sqlCmd = new SqlCommand(s_sql, sqlConn))
                    {
                        ei.Tag_D = "連接資料庫，設定Command類型";
                        sqlCmd.CommandType = CommandType.StoredProcedure;

                        ei.Tag_D = "連接資料庫，傳入參數";
                        foreach (SqlParameter para_detl in li_para)
                        { sqlCmd.Parameters.Add(para_detl); }

                        sqlCmd.Parameters[0].DbType = DbType.Xml;

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
        /// 取得聯名卡行銷贊助金銀行明細
        /// </summary>
        /// <param name="ei">執行訊息</param>
        /// <param name="qbdrq">查詢條件</param>
        /// <returns></returns>
        public DataSet get_CBSS_QuotaTrack(ref ExecInfo ei,
                                           QueryBankDataReq qbdrq,
                                           XDocument doc_data)
        {
            #region 設定tag
            ei.Tag_M = "連線資料庫，聯名卡行銷贊助金銀行明細";
            #endregion

            #region 設定變數

            ei.Tag_D = "設定變數";
            DataSet ds_rtn = new DataSet();
            string s_sql = string.Empty;
            List<SqlParameter> li_para = new List<SqlParameter>();
            CommonDAO cdao = new CommonDAO();

            #endregion

            try
            {
                #region 組合SQL字串

                ei.Tag_D = "組合SQL字串";
                s_sql = @"CBSS_GetQuotaTrack";

                #endregion

                #region 處理變數

                ei.Tag_D = "處理變數";
                li_para.Add(new SqlParameter("@RID", cdao.Convert_Sql_Parameter(qbdrq.RID.ToString(), CommonDAO.en_convert_type.en_int, string.Empty)));
                li_para.Add(new SqlParameter("@BID", cdao.Convert_Sql_Parameter((qbdrq.BID.HasValue == true ? qbdrq.BID.ToString() : string.Empty), CommonDAO.en_convert_type.en_int, string.Empty)));

                if (string.IsNullOrEmpty(qbdrq.ExcludeData) == true)
                {
                    li_para.Add(new SqlParameter("@DATA_XML", DBNull.Value));
                }
                else
                {
                    li_para.Add(new SqlParameter("@DATA_XML", cdao.Convert_Sql_Parameter(doc_data.ToString(), CommonDAO.en_convert_type.en_string, string.Empty)));
                }

                #endregion

                #region 連接資料庫

                ei.Tag_D = "連接資料庫";
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    using (SqlCommand sqlCmd = new SqlCommand(s_sql, sqlConn))
                    {
                        sqlCmd.CommandType = CommandType.StoredProcedure;

                        ei.Tag_D = "連接資料庫，開啟連線";
                        sqlConn.Open();

                        ei.Tag_D = "連接資料庫，傳入參數";
                        foreach (SqlParameter para_detl in li_para)
                        { sqlCmd.Parameters.Add(para_detl); }
                        sqlCmd.Parameters[2].DbType = DbType.Xml;

                        ei.Tag_D = "連接資料庫，執行資料庫作業";
                        SqlDataAdapter adapter = new SqlDataAdapter(sqlCmd);
                        adapter.Fill(ds_rtn);
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

            return ds_rtn;

            #endregion
        }

        /// <summary>
        /// 取得聯名卡行銷贊助金支付對象明細
        /// </summary>
        /// <param name="ei">執行訊息</param>
        /// <returns></returns>
        public DataTable get_CBSS_PayTarget(ref ExecInfo ei)
        {
            #region 設定tag
            ei.Tag_M = "連線資料庫，取得聯名卡行銷贊助金支付對象明細";
            #endregion

            #region 設定變數

            ei.Tag_D = "設定變數";
            DataTable dt_rtn = new DataTable();
            string s_sql = string.Empty;
            CommonDAO cdao = new CommonDAO();

            #endregion

            try
            {
                #region 組合SQL字串

                ei.Tag_D = "組合SQL字串";
                s_sql = @"CBSS_GetPayTarget";

                #endregion

                #region 連接資料庫

                ei.Tag_D = "連接資料庫";
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    using (SqlCommand sqlCmd = new SqlCommand(s_sql, sqlConn))
                    {
                        sqlCmd.CommandType = CommandType.StoredProcedure;

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
        /// 聯名卡行銷贊助金支付對象存檔
        /// </summary>
        /// <param name="ei">執行訊息</param>
        /// <param name="doc_data">存檔資訊</param>
        public void save_CBSS_PayTarget(ref ExecInfo ei,
                                        XDocument doc_data)
        {
            #region 設定tag
            ei.Tag_M = "連線資料庫，聯名卡行銷贊助金支付對象存檔";
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
                s_sql = @"CBSS_SavePayTarget";

                #endregion

                #region 處理變數

                ei.Tag_D = "處理變數";
                li_para.Add(new SqlParameter("@DATA_XML", cdao.Convert_Sql_Parameter(doc_data.ToString(), CommonDAO.en_convert_type.en_string, string.Empty)));

                #endregion

                #region 連接資料庫

                ei.Tag_D = "連接資料庫";
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    using (SqlCommand sqlCmd = new SqlCommand(s_sql, sqlConn))
                    {
                        ei.Tag_D = "連接資料庫，設定Command類型";
                        sqlCmd.CommandType = CommandType.StoredProcedure;

                        ei.Tag_D = "連接資料庫，傳入參數";
                        foreach (SqlParameter para_detl in li_para)
                        { sqlCmd.Parameters.Add(para_detl); }

                        sqlCmd.Parameters[0].DbType = DbType.Xml;

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
        /// 聯名卡行銷贊助金額度存檔
        /// </summary>
        /// <param name="ei">執行訊息</param>
        /// <param name="doc_data">存檔資訊</param>
        public void save_CBSS_DetailQuota(ref ExecInfo ei,
                                          XDocument doc_data)
        {
            #region 設定tag
            ei.Tag_M = "連線資料庫，聯名卡行銷贊助金額度存檔";
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
                s_sql = @"CBSS_SaveDetailQuota";

                #endregion

                #region 處理變數

                ei.Tag_D = "處理變數";
                li_para.Add(new SqlParameter("@DATA_XML", cdao.Convert_Sql_Parameter(doc_data.ToString(), CommonDAO.en_convert_type.en_string, string.Empty)));

                #endregion

                #region 連接資料庫

                ei.Tag_D = "連接資料庫";
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    using (SqlCommand sqlCmd = new SqlCommand(s_sql, sqlConn))
                    {
                        ei.Tag_D = "連接資料庫，設定Command類型";
                        sqlCmd.CommandType = CommandType.StoredProcedure;

                        ei.Tag_D = "連接資料庫，傳入參數";
                        foreach (SqlParameter para_detl in li_para)
                        { sqlCmd.Parameters.Add(para_detl); }

                        sqlCmd.Parameters[0].DbType = DbType.Xml;

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
        /// 聯名卡行銷贊助金請款項目存檔
        /// </summary>
        /// <param name="ei">執行訊息</param>
        /// <param name="doc_data">存檔資訊</param>
        public void save_CBSS_DetailTrack(ref ExecInfo ei,
                                          XDocument doc_data)
        {
            #region 設定tag
            ei.Tag_M = "連線資料庫，聯名卡行銷贊助金請款項目存檔";
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
                s_sql = @"CBSS_SaveDetailTrack";

                #endregion

                #region 處理變數

                ei.Tag_D = "處理變數";
                li_para.Add(new SqlParameter("@DATA_XML", cdao.Convert_Sql_Parameter(doc_data.ToString(), CommonDAO.en_convert_type.en_string, string.Empty)));

                #endregion

                #region 連接資料庫

                ei.Tag_D = "連接資料庫";
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    using (SqlCommand sqlCmd = new SqlCommand(s_sql, sqlConn))
                    {
                        ei.Tag_D = "連接資料庫，設定Command類型";
                        sqlCmd.CommandType = CommandType.StoredProcedure;

                        ei.Tag_D = "連接資料庫，傳入參數";
                        foreach (SqlParameter para_detl in li_para)
                        { sqlCmd.Parameters.Add(para_detl); }

                        sqlCmd.Parameters[0].DbType = DbType.Xml;

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
