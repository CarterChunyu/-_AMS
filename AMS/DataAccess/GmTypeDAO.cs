using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using log4net;
using System.Data;
using System.Data.SqlClient;
using Domain;

namespace DataAccess
{
    public class GmTypeDAO : BasicDAL
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(GmMerchantTypeDAO));

        //特約機構群組維護_查詢後選單
        public DataTable GM_TYPE_INDEX(string group, string merchant)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();
                    string sqlText = @"
                    SELECT GM.MERCHANT_NO,GM.MERCHANT_STNAME as MERCHANT_NAME,D.GROUP_ID,M.GROUP_NAME,CAST(D.SHOW_ORDER AS INT) SHOW_ORDER                    
                    FROM GM_MERCHANT_TYPE_D D
                    INNER JOIN GM_MERCHANT GM ON D.MERCHANT_NO=GM.MERCHANT_NO 
                    INNER JOIN GM_MERCHANT_TYPE_M M ON D.GROUP_ID=M.GROUP_ID
                    WHERE D.SHOW_FLG='Y'
                    ";

                    if (group != "ALL")
                    { sqlText = String.Format(@"{0} AND D.GROUP_ID=@EXEC_GROUP_ID ", sqlText); }
                    if (merchant != "ALL" && group != "ALL")
                    { sqlText = String.Format(@"{0} AND D.MERCHANT_NO=@EXEC_MERCHANT_NO ", sqlText); }


                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    sqlCmd.Parameters.Add("@EXEC_MERCHANT_NO", SqlDbType.VarChar).Value = merchant;
                    sqlCmd.Parameters.Add("@EXEC_GROUP_ID", SqlDbType.VarChar).Value = group;

                    SqlDataAdapter adapter = new SqlDataAdapter(sqlCmd);
                    adapter.Fill(dt);

                    sqlConn.Close();

                }
            }

            catch (Exception ex)
            {
                log.Debug("Query Ex: GM_MERCHANT_TYPE_D");////
                log.Debug(ex.StackTrace);
                log.Debug(ex.Message);
            }

            return dt;
        }
        //特約機構群組維護_選擇編輯的單筆資料
        public DataTable GM_TYPE_EDIT(string group, string merchant)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();

                    string sqlText = @"
                    SELECT D.MERCHANT_NO,GM.MERCHANT_STNAME as MERCHANT_NAME,D.GROUP_ID,M.GROUP_NAME,CAST(D.SHOW_ORDER AS INT) SHOW_ORDER                    
                    FROM GM_MERCHANT_TYPE_D D
                    INNER JOIN GM_MERCHANT GM ON D.MERCHANT_NO=GM.MERCHANT_NO 
                    INNER JOIN GM_MERCHANT_TYPE_M M ON D.GROUP_ID=M.GROUP_ID
                    WHERE D.SHOW_FLG='Y' AND D.MERCHANT_NO=@EXEC_MERCHANT_NO;
                    ";

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    sqlCmd.Parameters.Add("@EXEC_MERCHANT_NO", SqlDbType.VarChar).Value = merchant;
                    sqlCmd.Parameters.Add("@EXEC_GROUP_ID", SqlDbType.VarChar).Value = group;

                    SqlDataAdapter adapter = new SqlDataAdapter(sqlCmd);
                    adapter.Fill(dt);

                    sqlConn.Close();
                }
            }
            catch (Exception ex)
            {
                log.Debug("Query Ex: GM_MERCHANT_TYPE_D");////
                log.Debug(ex.StackTrace);
                log.Debug(ex.Message);
            }
            return dt;
        }
        //特約機構群組維護_修改
        public void GM_TYPE_UPDATE(GmType entity, string username)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();
                    string sqlText = @"DECLARE @OLD_GROUP_ID VARCHAR(20)
                                       SELECT @OLD_GROUP_ID=GROUP_ID FROM GM_MERCHANT_TYPE_D
                                       WHERE MERCHANT_NO=@EXEC_MERCHANT_NO;

                                       INSERT INTO GM_MERCHANT_TYPE_LOG
                                       VALUES ('UPDATE',@EXEC_MERCHANT_NO,@OLD_GROUP_ID,@EXEC_GROUP_ID,@EXEC_USER_NAME,getDate());

                                       UPDATE GM_MERCHANT_TYPE_D
                                       SET GROUP_ID = @EXEC_GROUP_ID,SHOW_ORDER = Right('000'+@EXEC_SHOW_ORDER,3)
                                       WHERE MERCHANT_NO = @EXEC_MERCHANT_NO;";

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    sqlCmd.Parameters.Add("@EXEC_MERCHANT_NO", SqlDbType.VarChar).Value = entity.MERCHANT_NO.ToString();
                    sqlCmd.Parameters.Add("@EXEC_GROUP_ID", SqlDbType.VarChar).Value = entity.GROUP_ID.ToString();
                    sqlCmd.Parameters.Add("@EXEC_SHOW_ORDER", SqlDbType.VarChar).Value = entity.SHOW_ORDER.ToString();
                    sqlCmd.Parameters.Add("@EXEC_USER_NAME", SqlDbType.VarChar).Value = username;

                    SqlDataAdapter adapter = new SqlDataAdapter(sqlCmd);
                    adapter.Fill(dt);

                    sqlConn.Close();

                }
            }

            catch (Exception ex)
            {
                log.Debug(ex.Message);
            }
        }




    }
}
