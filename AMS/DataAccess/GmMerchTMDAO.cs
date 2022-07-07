using Domain;
using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DataAccess
{
    public class GmMerchTMDAO : BasicDAL
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(GmMerchantDAO));

        public GmMerchTMDAO()
        {
        }

        public void Insert(GmMerchTM entity)
        {
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();

                    string sqlText = @"
INSERT INTO dbo.GM_MERCH_TM
            (MERCHANT_NO
            ,MERCH_TMID
            ,IBON_MERCHANT_NAME
            ,IBON_SHOW_TYPE
            ,NOTE
            ,CREATE_USER
            ,CREATE_TIME
            ,UPDATE_USER
            ,UPDATE_TIME
            ,IS_ACTIVE)
     VALUES
            (@MERCHANT_NO
            ,@MERCH_TMID
            ,@IBON_MERCHANT_NAME
            ,@IBON_SHOW_TYPE
            ,@NOTE
            ,@CREATE_USER
            ,@CREATE_TIME
            ,@UPDATE_USER
            ,@UPDATE_TIME
            ,@IS_ACTIVE)
";

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    sqlCmd.Parameters.Add("@MERCHANT_NO", SqlDbType.VarChar).Value = entity.MERCHANT_NO;
                    sqlCmd.Parameters.Add("@MERCH_TMID", SqlDbType.VarChar).Value = entity.MERCH_TMID;
                    sqlCmd.Parameters.Add("@IBON_MERCHANT_NAME", SqlDbType.VarChar).Value = entity.IBON_MERCHANT_NAME;
                    sqlCmd.Parameters.Add("@IBON_SHOW_TYPE", SqlDbType.VarChar).Value = entity.IBON_SHOW_TYPE;
                    sqlCmd.Parameters.Add("@NOTE", SqlDbType.VarChar).Value = entity.NOTE ?? (object)DBNull.Value;
                    sqlCmd.Parameters.Add("@CREATE_USER", SqlDbType.NVarChar).Value = entity.CREATE_USER;
                    sqlCmd.Parameters.Add("@CREATE_TIME", SqlDbType.NVarChar).Value = entity.CREATE_TIME;
                    sqlCmd.Parameters.Add("@UPDATE_USER", SqlDbType.VarChar).Value = entity.CREATE_USER;
                    sqlCmd.Parameters.Add("@UPDATE_TIME", SqlDbType.VarChar).Value = entity.CREATE_TIME;
                    sqlCmd.Parameters.Add("@IS_ACTIVE", SqlDbType.VarChar).Value = entity.IS_ACTIVE;
                    
                    sqlCmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                log.Debug(ex.StackTrace);
                log.Debug(ex.Message);

                throw new Exception(@"新增發生錯誤");
            }
        }

        public void Update(GmMerchTM entity)
        {
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();

                    string sqlText = @"
UPDATE dbo.GM_MERCH_TM
   SET MERCH_TMID = @MERCH_TMID
      ,IBON_MERCHANT_NAME = @IBON_MERCHANT_NAME
      ,IBON_SHOW_TYPE = @IBON_SHOW_TYPE
      ,NOTE = @NOTE
      ,UPDATE_USER = @UPDATE_USER
      ,UPDATE_TIME = @UPDATE_TIME
      ,IS_ACTIVE = @IS_ACTIVE
 WHERE MERCHANT_NO = @MERCHANT_NO
";

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    sqlCmd.Parameters.Add("@MERCHANT_NO", SqlDbType.VarChar).Value = entity.MERCHANT_NO;
                    sqlCmd.Parameters.Add("@MERCH_TMID", SqlDbType.VarChar).Value = entity.MERCH_TMID;
                    sqlCmd.Parameters.Add("@IBON_MERCHANT_NAME", SqlDbType.VarChar).Value = entity.IBON_MERCHANT_NAME;
                    sqlCmd.Parameters.Add("@IBON_SHOW_TYPE", SqlDbType.VarChar).Value = entity.IBON_SHOW_TYPE;
                    sqlCmd.Parameters.Add("@NOTE", SqlDbType.VarChar).Value = entity.NOTE ?? (object)DBNull.Value;
                    sqlCmd.Parameters.Add("@UPDATE_USER", SqlDbType.VarChar).Value = entity.UPDATE_USER;
                    sqlCmd.Parameters.Add("@UPDATE_TIME", SqlDbType.VarChar).Value = entity.UPDATE_TIME;
                    sqlCmd.Parameters.Add("@IS_ACTIVE", SqlDbType.VarChar).Value = entity.IS_ACTIVE;

                    sqlCmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                log.Debug(ex.StackTrace);
                log.Debug(ex.Message);

                throw new Exception(@"修改發生錯誤");
            }
        }

        public void Delete(GmMerchTM entity)
        {
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();

                    string sqlText = @"
DELETE dbo.GM_MERCH_TM
 WHERE MERCHANT_NO = @MERCHANT_NO
";

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    sqlCmd.Parameters.Add("@MERCHANT_NO", SqlDbType.VarChar).Value = entity.MERCHANT_NO;
                    
                    sqlCmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                log.Debug(ex.StackTrace);
                log.Debug(ex.Message);

                throw new Exception(@"刪除發生錯誤");
            }
        }

        public DataTable FindData(GmMerchTM entity, bool isFuzzy)
        {
            DataTable dt = new DataTable();
            List<SqlParameter> listPara = new List<SqlParameter>();
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();

                    string sqlText = @"
SELECT M.*, 
       CASE WHEN ISNULL(D.MERCHANT_NO, 'Y') = 'Y' THEN 'Y' ELSE 'N' END AS CAN_DELETE
  FROM dbo.GM_MERCH_TM M
  LEFT JOIN dbo.GM_MERCHANT D ON M.MERCHANT_NO = D.MERCHANT_NO
 WHERE 1 = 1
";

                    if (entity != null)
                    {
                        if ("" + entity.MERCHANT_NO != "")
                        {
                            if (isFuzzy)
                            { sqlText += @"   AND M.MERCHANT_NO like '%' + @MERCHANT_NO + '%' "; }
                            else
                            { sqlText += @"   AND M.MERCHANT_NO = @MERCHANT_NO "; }
                            listPara.Add(new SqlParameter("@MERCHANT_NO", SqlDbType.VarChar) { Value = entity.MERCHANT_NO });
                        }
                        if (entity.MERCH_TMID != null)
                        {
                            if (isFuzzy)
                            { sqlText += @"   AND M.MERCH_TMID like '%' + @MERCH_TMID + '%' "; }
                            else
                            { sqlText += @"   AND M.MERCH_TMID = @MERCH_TMID "; }
                            listPara.Add(new SqlParameter("@MERCH_TMID", SqlDbType.VarChar) { Value = entity.MERCH_TMID });
                        }
                        if (entity.IBON_MERCHANT_NAME != null)
                        {
                            if (isFuzzy)
                            { sqlText += @"   AND M.IBON_MERCHANT_NAME like '%' + @IBON_MERCHANT_NAME + '%' "; }
                            else
                            { sqlText += @"   AND M.IBON_MERCHANT_NAME = @IBON_MERCHANT_NAME "; }
                            listPara.Add(new SqlParameter("@IBON_MERCHANT_NAME", SqlDbType.VarChar) { Value = entity.IBON_MERCHANT_NAME });
                        }
                        if (entity.IS_ACTIVE != null)
                        {
                            sqlText += @"   AND M.IS_ACTIVE = @IS_ACTIVE ";
                            listPara.Add(new SqlParameter("@IS_ACTIVE", SqlDbType.VarChar) { Value = entity.IS_ACTIVE });
                        }
                    }

                    sqlText += @" ORDER BY M.MERCHANT_NO ";

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    foreach (SqlParameter para in listPara)
                    { sqlCmd.Parameters.Add(para); }
                    SqlDataAdapter adapter = new SqlDataAdapter(sqlCmd);
                    adapter.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                log.Debug(ex.StackTrace);
                log.Debug(ex.Message);
            }

            return dt;
        }
    }
}
