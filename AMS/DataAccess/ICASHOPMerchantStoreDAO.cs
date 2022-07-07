using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

using Domain.Common;
using Domain.ICASHOPMerchantStore;
using log4net;

namespace DataAccess
{
    public class ICASHOPMerchantStoreDAO : BasicOPDAL
    {
        #region 設定變數
        private static readonly ILog log = LogManager.GetLogger(typeof(ICASHOPMerchantStoreDAO));
        #endregion

        /// <summary>
        /// 建構子
        /// </summary>
        public ICASHOPMerchantStoreDAO() { }

        /// <summary>
        /// 取得特店門市明細資料
        /// </summary>
        /// <param name="ei">執行訊息</param>
        /// <param name="s_UnifiedBusinessNo">特店統編</param>
        /// <returns></returns>
        public DataTable get_MerchantStoreDetail(ref ExecInfo ei,
                                                 string s_UnifiedBusinessNo)
        {
            #region 設定變數

            string s_sql = string.Empty;
            List<SqlParameter> li_para = new List<SqlParameter>();
            CommonDAO cdao = new CommonDAO();
            DataTable dt_rtn = new DataTable();

            #endregion

            try
            {
                #region 組合SQL字串

                ei.Tag_D = "組合SQL字串";
                s_sql = @"ausp_ICashOP_OpenPoint_MerchantStoreDetail_S";

                #endregion

                #region 處理變數

                ei.Tag_D = "處理變數";
                li_para.Clear();
                li_para.Add(new SqlParameter("@UnifiedBusinessNo", cdao.Convert_Sql_Parameter(s_UnifiedBusinessNo, CommonDAO.en_convert_type.en_string, string.Empty)));

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

            return dt_rtn;
        }

        /// <summary>
        /// 作業開始
        /// </summary>
        /// <param name="ei">執行訊息</param>
        /// <param name="taskInfo">作業資訊</param>
        public void Task_Begin(ref ExecInfo ei,
                               ref TaskInfo taskInfo)
        {
            #region 設定Tag
            ei.Tag_M = "作業開始";
            #endregion

            #region 設定變數

            ei.Tag_D = "設定變數";

            string s_sql = string.Empty;
            List<SqlParameter> li_para = new List<SqlParameter>();
            CommonDAO cdao = new CommonDAO();
            DataSet ds_rtn = new DataSet();

            const string s_MODULE_NAME = "JGP_D_Import_2_ICASHOP.bat";
            const string s_JOB_NAME = "ICashOP_Import_MerchantStoreData_2_ICASHOP";
            const string s_EXEC_GROUP = "UI";
            string s_SERVER_IP = System.Net.Dns.GetHostName();

            #endregion

            try
            {
                #region 組合SQL字串

                ei.Tag_D = "組合SQL字串";
                s_sql = @"ausp_ICashOP_Job_Begin";

                #endregion

                #region 處理變數

                ei.Tag_D = "處理變數";
                li_para.Clear();
                li_para.Add(new SqlParameter("@MODULE_NAME", cdao.Convert_Sql_Parameter(s_MODULE_NAME, CommonDAO.en_convert_type.en_string, string.Empty)));
                li_para.Add(new SqlParameter("@JOB_NAME", cdao.Convert_Sql_Parameter(s_JOB_NAME, CommonDAO.en_convert_type.en_string, string.Empty)));
                li_para.Add(new SqlParameter("@EXEC_GROUP", cdao.Convert_Sql_Parameter(s_EXEC_GROUP, CommonDAO.en_convert_type.en_string, string.Empty)));
                li_para.Add(new SqlParameter("@EXEC_User", cdao.Convert_Sql_Parameter(taskInfo.User, CommonDAO.en_convert_type.en_string, string.Empty)));
                li_para.Add(new SqlParameter("@EXEC_IP", cdao.Convert_Sql_Parameter(taskInfo.IP, CommonDAO.en_convert_type.en_string, string.Empty)));
                li_para.Add(new SqlParameter("@SERVER_IP", cdao.Convert_Sql_Parameter(s_SERVER_IP, CommonDAO.en_convert_type.en_string, string.Empty)));
                li_para.Add(new SqlParameter("@EXEC_ID", cdao.Convert_Sql_Parameter(string.Empty, CommonDAO.en_convert_type.en_int, string.Empty)));
                li_para.Add(new SqlParameter("@EXEC_VERSION_ID", cdao.Convert_Sql_Parameter(string.Empty, CommonDAO.en_convert_type.en_int, string.Empty)));
                li_para.Add(new SqlParameter("@EXEC_TIME", cdao.Convert_Sql_Parameter(string.Empty, CommonDAO.en_convert_type.en_string, string.Empty)));

                #endregion

                #region 連接資料庫

                ei.Tag_D = "連接資料庫";

                ds_rtn.Clear();

                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    using (SqlCommand sqlCmd = new SqlCommand(s_sql, sqlConn))
                    {
                        sqlCmd.CommandType = CommandType.StoredProcedure;

                        ei.Tag_D = "連接資料庫，傳入參數";
                        foreach (SqlParameter para_detl in li_para)
                        { sqlCmd.Parameters.Add(para_detl); }

                        sqlCmd.Parameters["@EXEC_ID"].DbType = DbType.Int32;
                        sqlCmd.Parameters["@EXEC_ID"].Direction = ParameterDirection.Output;
                        sqlCmd.Parameters["@EXEC_VERSION_ID"].DbType = DbType.Int32;
                        sqlCmd.Parameters["@EXEC_VERSION_ID"].Direction = ParameterDirection.Output;
                        sqlCmd.Parameters["@EXEC_TIME"].Direction = ParameterDirection.Output;
                        sqlCmd.Parameters["@EXEC_TIME"].Size = 17;

                        ei.Tag_D = "連接資料庫，開啟連線";
                        sqlConn.Open();

                        ei.Tag_D = "連接資料庫，執行資料庫作業";
                        SqlDataAdapter adapter = new SqlDataAdapter(sqlCmd);
                        adapter.Fill(ds_rtn);
                    }
                }

                #endregion

                #region 判斷回傳結果

                ei.Tag_D = "判斷回傳結果";

                if (ds_rtn.Tables[0].Rows[0]["RTN_RESULT"].ToString() != "OK")
                {
                    ei.Tag_D = "判斷回傳結果，判斷回傳失敗";
                    throw new Exception(ds_rtn.Tables[0].Rows[0]["RTN_MSG"].ToString());
                }
                else
                {
                    ei.Tag_D = "判斷回傳結果，判斷回傳成功";
                    taskInfo.EXEC_ID = int.Parse(li_para[6].Value.ToString());
                    taskInfo.EXEC_VERSION_ID = int.Parse(li_para[7].Value.ToString());
                    taskInfo.EXEC_TIME = li_para[8].Value.ToString();
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
        /// 作業結束
        /// </summary>
        /// <param name="ei">執行訊息</param>
        /// <param name="taskInfo">作業資訊</param>
        public void Task_End(ref ExecInfo ei,
                             TaskInfo taskInfo)
        {
            #region 設定Tag
            ei.Tag_M = "作業結束";
            #endregion

            #region 設定變數

            string s_sql = string.Empty;
            List<SqlParameter> li_para = new List<SqlParameter>();
            CommonDAO cdao = new CommonDAO();
            DataSet ds_rtn = new DataSet();

            const string s_MODULE_NAME = "JGP_D_Import_2_ICASHOP.bat";
            const string s_JOB_NAME = "ICashOP_Import_MerchantStoreData_2_ICASHOP";
            const string s_EXEC_GROUP = "UI";
            string s_SERVER_IP = System.Net.Dns.GetHostName();

            #endregion

            try
            {
                #region 判斷是否繼續

                ei.Tag_D = "判斷是否繼續";
                if (taskInfo.EXEC_ID == 0 ||
                    taskInfo.EXEC_VERSION_ID == 0)
                {
                    /* 如果有成功[作業開始]
                     * 則欄位[EXEC_ID]、[EXEC_VERSION_ID]都會大於0
                     * 如果還都是等於0，代表[作業開始]就失敗了，就不要再呼叫[作業結束]
                     */
                    ei.RtnResult = true;
                    ei.RtnMsg = string.Empty;
                    return;
                }

                #endregion

                #region 組合SQL字串

                ei.Tag_D = "組合SQL字串";
                s_sql = @"ausp_ICashOP_Job_End";

                #endregion

                #region 處理變數

                ei.Tag_D = "處理變數";
                li_para.Clear();
                li_para.Add(new SqlParameter("@MODULE_NAME", cdao.Convert_Sql_Parameter(s_MODULE_NAME, CommonDAO.en_convert_type.en_string, string.Empty)));
                li_para.Add(new SqlParameter("@JOB_NAME", cdao.Convert_Sql_Parameter(s_JOB_NAME, CommonDAO.en_convert_type.en_string, string.Empty)));
                li_para.Add(new SqlParameter("@EXEC_GROUP", cdao.Convert_Sql_Parameter(s_EXEC_GROUP, CommonDAO.en_convert_type.en_string, string.Empty)));
                li_para.Add(new SqlParameter("@EXEC_User", cdao.Convert_Sql_Parameter(taskInfo.User, CommonDAO.en_convert_type.en_string, string.Empty)));
                li_para.Add(new SqlParameter("@EXEC_IP", cdao.Convert_Sql_Parameter(taskInfo.IP, CommonDAO.en_convert_type.en_string, string.Empty)));
                li_para.Add(new SqlParameter("@SERVER_IP", cdao.Convert_Sql_Parameter(s_SERVER_IP, CommonDAO.en_convert_type.en_string, string.Empty)));
                li_para.Add(new SqlParameter("@EXEC_ID", cdao.Convert_Sql_Parameter(taskInfo.EXEC_ID.ToString(), CommonDAO.en_convert_type.en_int, string.Empty)));
                li_para.Add(new SqlParameter("@EXEC_VERSION_ID", cdao.Convert_Sql_Parameter(taskInfo.EXEC_VERSION_ID.ToString(), CommonDAO.en_convert_type.en_int, string.Empty)));
                li_para.Add(new SqlParameter("@EXEC_TIME", cdao.Convert_Sql_Parameter(taskInfo.EXEC_TIME, CommonDAO.en_convert_type.en_string, string.Empty)));
                li_para.Add(new SqlParameter("@EXEC_STATUS", cdao.Convert_Sql_Parameter(taskInfo.EXEC_STATUS, CommonDAO.en_convert_type.en_string, string.Empty)));

                #endregion

                #region 連接資料庫

                ei.Tag_D = "連接資料庫";

                ds_rtn.Clear();
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
                        adapter.Fill(ds_rtn);
                    }
                }

                #endregion

                #region 判斷回傳結果

                ei.Tag_D = "判斷回傳結果";

                if (ds_rtn.Tables[0].Rows[0]["RTN_RESULT"].ToString() != "OK")
                {
                    ei.Tag_D = "判斷回傳結果，判斷回傳失敗";
                    throw new Exception(ds_rtn.Tables[0].Rows[0]["RTN_MSG"].ToString());
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
        /// 將匯入檔案資料寫入資料庫
        /// </summary>
        /// <param name="ei">執行訊息</param>
        /// <param name="taskInfo">作業資訊</param>
        /// <param name="li_ifi">匯入檔案資訊</param>
        /// <param name="dt_data">匯入檔案內容</param>
        public void import_MerchantStoreData(ref ExecInfo ei,
                                             TaskInfo taskInfo,
                                             ref List<ImportFileInfo> li_ifi,
                                             DataTable dt_data)
        {
            #region 設定Tag
            ei.Tag_M = "將匯入檔案資料寫入資料庫";
            #endregion

            #region 設定變數

            string s_sql = string.Empty;
            List<SqlParameter> li_para = new List<SqlParameter>();
            CommonDAO cdao = new CommonDAO();

            const string s_MODULE_NAME = "JGP_D_Import_2_ICASHOP.bat";
            const string s_JOB_NAME = "ICashOP_Import_MerchantStoreData_2_ICASHOP";
            const string s_EXEC_GROUP = "UI";
            string s_SERVER_IP = System.Net.Dns.GetHostName();
            string s_break_line = "<br/>";

            #endregion

            try
            {
                #region 清空暫存Table

                #region 組合SQL字串

                ei.Tag_D = "清空暫存Table，組合SQL字串";
                s_sql = @"
TRUNCATE TABLE dbo.OpenPoint_MerchantStoreDetailTemp;
";

                #endregion

                #region 連接資料庫

                ei.Tag_D = "清空暫存Table，連接資料庫";
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    using (SqlCommand sqlCmd = new SqlCommand(s_sql, sqlConn))
                    {
                        sqlCmd.CommandType = CommandType.Text;
                        sqlConn.Open();
                        sqlCmd.ExecuteNonQuery();
                    }
                }

                #endregion

                #endregion

                #region 使用BulkInsert寫入資料至暫存Table

                #region 連接資料庫

                ei.Tag_D = "使用BulkInsert寫入資料至暫存Table，連接資料庫";
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();

                    using (var sbc = new SqlBulkCopy(sqlConn))
                    {
                        sbc.DestinationTableName = "dbo.OpenPoint_MerchantStoreDetailTemp";
                        sbc.WriteToServer(dt_data);
                    }
                }

                #endregion

                #endregion

                #region 將暫存Table資料寫入正式主檔

                foreach (ImportFileInfo ifi in li_ifi)
                {
                    #region 組合SQL字串

                    ei.Tag_D = $"檔案：[{ifi.UploadFileName}]，將暫存Table資料寫入正式主檔，組合SQL字串";
                    s_sql = @"ausp_ICashOP_Import_StoreData_IU";

                    #endregion

                    #region 處理變數

                    ei.Tag_D = $"檔案：[{ifi.UploadFileName}]，將暫存Table資料寫入正式主檔，處理變數";
                    li_para.Clear();
                    li_para.Add(new SqlParameter("@EXEC_SYS_DATE", cdao.Convert_Sql_Parameter(string.Empty, CommonDAO.en_convert_type.en_string, string.Empty)));
                    li_para.Add(new SqlParameter("@MODULE_NAME", cdao.Convert_Sql_Parameter(s_MODULE_NAME, CommonDAO.en_convert_type.en_string, string.Empty)));
                    li_para.Add(new SqlParameter("@JOB_NAME", cdao.Convert_Sql_Parameter(s_JOB_NAME, CommonDAO.en_convert_type.en_string, string.Empty)));
                    li_para.Add(new SqlParameter("@EXEC_TIME", cdao.Convert_Sql_Parameter(taskInfo.EXEC_TIME, CommonDAO.en_convert_type.en_string, string.Empty)));
                    li_para.Add(new SqlParameter("@EXEC_GROUP", cdao.Convert_Sql_Parameter(s_EXEC_GROUP, CommonDAO.en_convert_type.en_string, string.Empty)));
                    li_para.Add(new SqlParameter("@UnifiedBusinessNo", cdao.Convert_Sql_Parameter(ifi.UnifiedBusinessNo, CommonDAO.en_convert_type.en_string, string.Empty)));
                    li_para.Add(new SqlParameter("@EXEC_User", cdao.Convert_Sql_Parameter(taskInfo.User, CommonDAO.en_convert_type.en_string, string.Empty)));
                    li_para.Add(new SqlParameter("@EXEC_IP", cdao.Convert_Sql_Parameter(taskInfo.IP, CommonDAO.en_convert_type.en_string, string.Empty)));

                    #endregion

                    #region 連接資料庫

                    ei.Tag_D = $"檔案：[{ifi.UploadFileName}]，將暫存Table資料寫入正式主檔，連接資料庫";

                    DataSet ds_rtn = new DataSet();
                    using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                    {
                        using (SqlCommand sqlCmd = new SqlCommand(s_sql, sqlConn))
                        {
                            sqlCmd.CommandType = CommandType.StoredProcedure;

                            foreach (SqlParameter para_detl in li_para)
                            { sqlCmd.Parameters.Add(para_detl); }

                            sqlConn.Open();
                            SqlDataAdapter adapter = new SqlDataAdapter(sqlCmd);
                            adapter.Fill(ds_rtn);
                        }
                    }

                    #endregion

                    #region 記錄回傳結果

                    ei.Tag_D = $"檔案：[{ifi.UploadFileName}]，將暫存Table資料寫入正式主檔，記錄回傳結果";

                    ifi.ErrMsg = string.Empty;
                    if (ds_rtn.Tables[0].Rows[0]["RTN_RESULT"].ToString() != "OK")
                    {
                        ifi.ErrMsg += $"{s_break_line}{new string('=', 50)}";
                        ifi.ErrMsg += $"{s_break_line}檔案：[{ifi.UploadFileName}]，轉入失敗";
                        ifi.ErrMsg += $"{s_break_line}{ds_rtn.Tables[0].Rows[0]["RTN_MSG"].ToString()}";

                        foreach (DataRow dr in ds_rtn.Tables[1].Rows)
                        { ifi.ErrMsg += $"{s_break_line}{dr["Msg"].ToString()}"; }
                    }

                    #endregion
                } //foreach結束

                #endregion

                #region 組合訊息

                ei.Tag_D = "組合訊息";

                //匯入成功資料
                var var_Import_OK = from x in li_ifi
                                    where string.IsNullOrEmpty(x.ErrMsg) == true
                                    orderby x.UploadFileName
                                    select x;

                //匯入失敗資料
                var var_Import_NG = from x in li_ifi
                                    where string.IsNullOrEmpty(x.ErrMsg) == false
                                    orderby x.UploadFileName
                                    select x;

                //組合總結訊息
                ei.RtnMsg += $"{s_break_line}{new string('=', 50)}";
                ei.RtnMsg += $"{s_break_line}上傳匯入檔案共({li_ifi.Count.ToString()})個檔案";
                ei.RtnMsg += $"{s_break_line}匯入失敗檔案共({var_Import_NG.Count().ToString()})個檔案";
                ei.RtnMsg += $"{s_break_line}匯入成功檔案共({var_Import_OK.Count().ToString()})個檔案";

                //組合錯誤訊息
                foreach (ImportFileInfo ifi in var_Import_NG)
                {
                    ei.RtnMsg += ifi.ErrMsg;
                }

                //組合錯誤訊息
                ei.RtnMsg += $"{s_break_line}{new string('=', 50)}";
                ei.RtnMsg += $"{s_break_line}資料匯入成功檔案明細如下：";
                foreach (ImportFileInfo ifi in var_Import_OK)
                {
                    ei.RtnMsg += $"{s_break_line}檔案：[{ifi.UploadFileName}]，資料匯入成功";
                }
                ei.RtnMsg += $"{s_break_line}{new string('=', 50)}";

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
