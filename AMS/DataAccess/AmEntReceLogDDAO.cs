using Common.Logging;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace DataAccess
{
    public class AmEntReceLogDDAO :BasicDAL
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(AmUsersDAO));
        public string TableName { private get; set; }

        public AmEntReceLogDDAO()
        {
            this.TableName = "AM_ENT_RECE_LOG_D";
        }

        public AmEntReceLogD FindByPk(string sn, string bankAcctTo)
        {
            DataTable dt = new DataTable();
            AmEntReceLogD obj = null;

            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();

                    string sqlText = @"select
                                LOG_DATE
                                ,LOG_TIME
                                ,SN
                                ,BANK_ACCT_TO
                                ,ACT_DATE
                                ,BILL_NO
                                ,AMT
                                ,BAL
                                ,ACT_STATUS
                                ,SUMMARY
                                ,BANK_ACCT_FROM
                                ,CASE_NO
                                ,CHECK_STATUS
                                ,ACCT_CHECK_STATUS
                                ,OPERATOR 
                                ,REMARK
                                ,INVOICE_NO
                                ,INVOICE_DATE
                            from 
                              " + this.TableName + @" 
                            where
                                SN = @SN 
                            and BANK_ACCT_TO = @BANK_ACCT_TO";

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    sqlCmd.Parameters.Add(new SqlParameter("@SN", SqlDbType.VarChar)).Value = sn;
                    sqlCmd.Parameters.Add(new SqlParameter("@BANK_ACCT_TO", SqlDbType.VarChar)).Value = bankAcctTo;

                    SqlDataAdapter adapter = new SqlDataAdapter(sqlCmd);

                    adapter.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        obj = new AmEntReceLogD();
                        obj.LogDate = dt.Rows[0]["LOG_DATE"].ToString();
                        obj.LogTime = dt.Rows[0]["LOG_TIME"].ToString();
                        obj.Sn = dt.Rows[0]["SN"].ToString();
                        obj.BankAcctTo = dt.Rows[0]["BANK_ACCT_TO"].ToString();
                        obj.ActDate = dt.Rows[0]["ACT_DATE"].ToString();
                        obj.BillNo = dt.Rows[0]["BILL_NO"].ToString();
                        obj.Amt = dt.Rows[0]["AMT"].ToString();
                        obj.Bal = dt.Rows[0]["BAL"].ToString();
                        obj.ActStatus = dt.Rows[0]["ACT_STATUS"].ToString();
                        obj.Summary = dt.Rows[0]["SUMMARY"].ToString();
                        obj.BankAcctFrom = dt.Rows[0]["BANK_ACCT_FROM"].ToString();
                        obj.CaseNo = dt.Rows[0]["CASE_NO"].ToString();
                        obj.CheckStatus = dt.Rows[0]["CHECK_STATUS"].ToString();
                        obj.AcctCheckStatus = dt.Rows[0]["ACCT_CHECK_STATUS"].ToString();
                        obj.Operator = dt.Rows[0]["OPERATOR"].ToString();
                        obj.Remark = dt.Rows[0]["REMARK"].ToString();
                        obj.InvoiceNo = dt.Rows[0]["INVOICE_NO"].ToString();
                        obj.InvoiceDate = dt.Rows[0]["INVOICE_DATE"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                log.Debug(ex.StackTrace);
                log.Debug(ex.Message);
                return null;
            }

            return obj;
        }

        public void Insert(AmEntReceLogD entity)
        {
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();
                    string sqlText = @"INSERT INTO "
                                    + this.TableName + @"
                                   (
                                    [LOG_DATE]
                                   ,[LOG_TIME]
                                   ,[SN]
                                   ,[BANK_ACCT_TO]
                                   ,[ACT_DATE]
                                   ,[BILL_NO]
                                   ,[AMT]
                                   ,[BAL]
                                   ,[ACT_STATUS]
                                   ,[SUMMARY]
                                   ,[BANK_ACCT_FROM]
                                   ,[CASE_NO]
                                   ,[CHECK_STATUS]
                                   ,[ACCT_CHECK_STATUS]
                                   ,[OPERATOR]
                                   ,[REMARK]
                                   ,[INVOICE_NO]
                                   ,[INVOICE_DATE]
                                    )
                            VALUES
                                   (
                                    @LOG_DATE
                                   ,@LOG_TIME
                                   ,@SN
                                   ,@BANK_ACCT_TO
                                   ,@ACT_DATE
                                   ,@BILL_NO
                                   ,@AMT
                                   ,@BAL
                                   ,@ACT_STATUS
                                   ,@SUMMARY
                                   ,@BANK_ACCT_FROM
                                   ,@CASE_NO
                                   ,@CHECK_STATUS
                                   ,@ACCT_CHECK_STATUS
                                   ,@OPERATOR
                                   ,@REMARK
                                   ,@INVOICE_NO
                                   ,@INVOICE_DATE
                                    )";

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    sqlCmd.Parameters.Add("@LOG_DATE", SqlDbType.VarChar).Value = entity.LogDate;
                    sqlCmd.Parameters.Add("@LOG_TIME", SqlDbType.VarChar).Value = entity.LogTime;
                    sqlCmd.Parameters.Add("@SN", SqlDbType.VarChar).Value = entity.Sn;
                    sqlCmd.Parameters.Add("@BANK_ACCT_TO", SqlDbType.VarChar).Value = entity.BankAcctTo;
                    sqlCmd.Parameters.Add("@ACT_DATE", SqlDbType.VarChar).Value = entity.ActDate;
                    sqlCmd.Parameters.Add("@BILL_NO", SqlDbType.VarChar).Value = entity.BillNo;
                    sqlCmd.Parameters.Add("@AMT", SqlDbType.VarChar).Value = entity.Amt;
                    sqlCmd.Parameters.Add("@BAL", SqlDbType.VarChar).Value = entity.Bal;
                    sqlCmd.Parameters.Add("@ACT_STATUS", SqlDbType.VarChar).Value = entity.ActStatus;
                    sqlCmd.Parameters.Add("@SUMMARY", SqlDbType.VarChar).Value = entity.Summary;
                    sqlCmd.Parameters.Add("@BANK_ACCT_FROM", SqlDbType.VarChar).Value = entity.BankAcctFrom;
                    sqlCmd.Parameters.Add("@CASE_NO", SqlDbType.VarChar).Value = entity.CaseNo;
                    sqlCmd.Parameters.Add("@CHECK_STATUS", SqlDbType.VarChar).Value = entity.CheckStatus;
                    sqlCmd.Parameters.Add("@ACCT_CHECK_STATUS", SqlDbType.VarChar).Value = entity.AcctCheckStatus;
                    sqlCmd.Parameters.Add("@OPERATOR", SqlDbType.VarChar).Value = entity.Operator;
                    sqlCmd.Parameters.Add("@REMARK", SqlDbType.VarChar).Value = entity.Remark;
                    sqlCmd.Parameters.Add("@INVOICE_NO", SqlDbType.VarChar).Value = entity.InvoiceNo;
                    sqlCmd.Parameters.Add("@INVOICE_DATE", SqlDbType.VarChar).Value = entity.InvoiceDate;

                    int effect = sqlCmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                log.Debug(ex.StackTrace);
                log.Debug(ex.Message);
            }
        }

        public List<AmEntReceLogD> FindBySearch(string receType, string acctDateS, string acctDateE, string caseNo, string checkStatus, string acctCheckStatus)
        {
            List<AmEntReceLogD> objList = new List<AmEntReceLogD>();
            char[] charsToTrim = { '-' };
            acctDateS = acctDateS.Replace("-","");
            acctDateE = acctDateE.Replace("-", "");
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();
                    string sqlText = @" select
                                LOG_DATE
                                ,LOG_TIME
                                ,SN
                                ,BANK_ACCT_TO
                                ,ACT_DATE
                                ,BILL_NO
                                ,AMT
                                ,BAL
                                ,ACT_STATUS
                                ,SUMMARY
                                ,BANK_ACCT_FROM
                                ,CASE_NO
                                ,CHECK_STATUS
                                ,ACCT_CHECK_STATUS
                                ,OPERATOR 
                                ,REMARK
                                ,INVOICE_NO
                                ,INVOICE_DATE
                    from " + this.TableName + @" 
                    where
                        SUBSTRING(SN, 7, 1) = @RECE_TYPE ";
                    
                    if (acctDateS!="")
                        sqlText += "and ACT_DATE >= @ACT_DATE_S ";

                    if (acctDateE != "")
                        sqlText += "and ACT_DATE <= @ACT_DATE_E ";

                    if (caseNo!="")
                        sqlText += "and CASE_NO = @CASE_NO ";

                    if (checkStatus != "")
                        sqlText += "and CHECK_STATUS = @CHECK_STATUS ";

                    if (acctCheckStatus != "")
                        sqlText += "and ACCT_CHECK_STATUS = @ACCT_CHECK_STATUS ";

                    sqlText += "order by ACT_DATE ";

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    sqlCmd.Parameters.Add("@RECE_TYPE", SqlDbType.VarChar).Value = receType;
                    sqlCmd.Parameters.Add("@ACT_DATE_S", SqlDbType.VarChar).Value = acctDateS;
                    sqlCmd.Parameters.Add("@ACT_DATE_E", SqlDbType.VarChar).Value = acctDateE;
                    sqlCmd.Parameters.Add("@CASE_NO", SqlDbType.VarChar).Value = caseNo;
                    sqlCmd.Parameters.Add("@CHECK_STATUS", SqlDbType.VarChar).Value = checkStatus;
                    sqlCmd.Parameters.Add("@ACCT_CHECK_STATUS", SqlDbType.VarChar).Value = acctCheckStatus;

                    SqlDataReader dr = sqlCmd.ExecuteReader();
                    while (dr.Read())
                    {
                        AmEntReceLogD obj = new AmEntReceLogD();

                        obj.Sn = dr["SN"].ToString();
                        obj.LogDate = dr["LOG_DATE"].ToString();
                        obj.LogTime = dr["LOG_TIME"].ToString();
                        obj.BankAcctTo = dr["BANK_ACCT_TO"].ToString();
                        obj.ActDate = dr["ACT_DATE"].ToString();
                        obj.BillNo = dr["BILL_NO"].ToString();
                        obj.Amt = dr["AMT"].ToString();
                        obj.Bal = dr["BAL"].ToString();
                        obj.ActStatus = dr["ACT_STATUS"].ToString();
                        obj.Summary = dr["SUMMARY"].ToString();
                        obj.BankAcctFrom = dr["BANK_ACCT_FROM"].ToString();
                        obj.CaseNo = dr["CASE_NO"].ToString();
                        obj.CheckStatus = dr["CHECK_STATUS"].ToString();
                        obj.AcctCheckStatus = dr["ACCT_CHECK_STATUS"].ToString();
                        obj.Operator = dr["OPERATOR"].ToString();
                        obj.Remark = dr["REMARK"].ToString();
                        obj.InvoiceNo = dr["INVOICE_NO"].ToString();
                        obj.InvoiceDate = dr["INVOICE_DATE"].ToString();

                        objList.Add(obj);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Debug("Query Ex: " + this.TableName);////
                log.Debug(ex.StackTrace);
                log.Debug(ex.Message);
                objList = null;
            }
            //log.Debug("Query End: " + this.TableName);////
            return objList;
        }

    
    }
}
