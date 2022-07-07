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
    public class AmEntAccountDDAO :BasicDAL
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(AmUsersDAO));
        public string TableName { private get; set; }
        public string MappingTableName { private get; set; }

        public AmEntAccountDDAO()
        {
            this.TableName = "AM_ENT_ACCOUNT_D";
            this.MappingTableName = "AM_ACCOUNT_ROLE";
        }

        public void Insert(AmEntAccountD entity)
        {
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();

                    string sqlText =
                        @"INSERT INTO "
                        + this.TableName + @"
                                   (
                                    [ACCT_TITLE]                                  
                                   ,[BANK_NO]
                                   ,[BANK_ACCT]
                                   ,[ACCT_CODE]
                                   ,[COMPANY_ID]
                                   ,[DEPARTMENT]
                                   ,[NAME]
                                   ,[REMARK]
                                   ,[STATUS]
                                   ,[OPERATOR]
                                   ,[UPDATE_DATE]
                                    )
                            VALUES
                                   (
                                    @ACCT_TITLE
                                   ,@BANK_NO
                                   ,@BANK_ACCT
                                   ,@ACCT_CODE
                                   ,@COMPANY_ID
                                   ,@DEPARTMENT
                                   ,@NAME
                                   ,@REMARK
                                   ,@STATUS
                                   ,@OPERATOR
                                   ,@UPDATE_DATE
                                    )";

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    sqlCmd.Parameters.Add("@ACCT_TITLE", SqlDbType.VarChar).Value = entity.AcctTitle;
                    sqlCmd.Parameters.Add("@BANK_NO", SqlDbType.VarChar).Value = entity.BankNo;
                    sqlCmd.Parameters.Add("@BANK_ACCT", SqlDbType.VarChar).Value = entity.BankAcct;
                    sqlCmd.Parameters.Add("@ACCT_CODE", SqlDbType.VarChar).Value = entity.AcctCode;
                    sqlCmd.Parameters.Add("@COMPANY_ID", SqlDbType.VarChar).Value = entity.CompanyId;
                    sqlCmd.Parameters.Add("@DEPARTMENT", SqlDbType.VarChar).Value = entity.Department;
                    sqlCmd.Parameters.Add("@NAME", SqlDbType.VarChar).Value = entity.Name;
                    sqlCmd.Parameters.Add("@REMARK", SqlDbType.VarChar).Value = entity.Remark;
                    sqlCmd.Parameters.Add("@STATUS", SqlDbType.VarChar).Value = entity.Status;
                    sqlCmd.Parameters.Add("@OPERATOR", SqlDbType.VarChar).Value = entity.Operator;
                    sqlCmd.Parameters.Add("@UPDATE_DATE", SqlDbType.VarChar).Value = DateTime.Now.ToString("yyyyMMddHHmmss");

                    sqlCmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                log.Debug(ex.StackTrace);
                log.Debug(ex.Message);
            }
        }

        public AmEntAccountD FindByPk(string bankNo, string bankAcct)
        {
            DataTable dt = new DataTable();
            AmEntAccountD obj = null;

            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();

                    string sqlText = @"select
                                ACCT_TITLE
                                ,BANK_NO
                                ,BANK_ACCT
                                ,ACCT_CODE
                                ,COMPANY_ID
                                ,DEPARTMENT
                                ,NAME
                                ,REMARK
                                ,STATUS
                                ,OPERATOR
                                ,UPDATE_DATE
                            from 
                              " + this.TableName + @" 
                            where
                                BANK_NO = @BANK_NO and BANK_ACCT = @BANK_ACCT ";

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    sqlCmd.Parameters.Add(new SqlParameter("@BANK_NO", SqlDbType.VarChar)).Value = bankNo;
                    sqlCmd.Parameters.Add(new SqlParameter("@BANK_ACCT", SqlDbType.VarChar)).Value = bankAcct;

                    SqlDataAdapter adapter = new SqlDataAdapter(sqlCmd);

                    adapter.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        obj = new AmEntAccountD();
                        obj.AcctTitle = dt.Rows[0]["ACCT_TITLE"].ToString();
                        obj.BankNo = dt.Rows[0]["BANK_NO"].ToString();
                        obj.BankAcct = dt.Rows[0]["BANK_ACCT"].ToString();
                        obj.AcctCode = dt.Rows[0]["ACCT_CODE"].ToString();
                        obj.CompanyId = dt.Rows[0]["COMPANY_ID"].ToString();
                        obj.Department = dt.Rows[0]["DEPARTMENT"].ToString();
                        obj.Name = dt.Rows[0]["NAME"].ToString();
                        obj.Remark = dt.Rows[0]["REMARK"].ToString();
                        obj.Status = dt.Rows[0]["STATUS"].ToString();
                        obj.Operator = dt.Rows[0]["OPERATOR"].ToString();
                        obj.UpdateDate = dt.Rows[0]["UPDATE_DATE"].ToString();
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

        public List<AmEntAccountD> FindByStatus(string status)
        {
            List<AmEntAccountD> objList = new List<AmEntAccountD>();
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();
                    string sqlText = @" select
                                ACCT_TITLE
                                ,BANK_NO
                                ,BANK_ACCT
                                ,ACCT_CODE
                                ,COMPANY_ID
                                ,DEPARTMENT
                                ,NAME
                                ,REMARK
                                ,STATUS
                                ,OPERATOR
                                ,UPDATE_DATE
                from " + this.TableName + @" 
                where
                     STATUS = @STATUS 
                order by BANK_NO 
                ";

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    sqlCmd.Parameters.Add("@STATUS", SqlDbType.VarChar).Value = status;
                    SqlDataReader dr = sqlCmd.ExecuteReader();
                    while (dr.Read())
                    {
                        AmEntAccountD obj = new AmEntAccountD();
                        obj.AcctTitle = dr["ACCT_TITLE"].ToString();
                        obj.BankNo = dr["BANK_NO"].ToString();
                        obj.BankAcct = dr["BANK_ACCT"].ToString();
                        obj.AcctCode = dr["ACCT_CODE"].ToString();
                        obj.CompanyId = dr["COMPANY_ID"].ToString();
                        obj.Department = dr["DEPARTMENT"].ToString();
                        obj.Name = dr["NAME"].ToString();
                        obj.Remark = dr["REMARK"].ToString();
                        obj.Status = dr["STATUS"].ToString();
                        obj.Operator = dr["OPERATOR"].ToString();
                        obj.UpdateDate = dr["UPDATE_DATE"].ToString();
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

        public List<AmEntAccountD> FindByRoleStatus(string roleId, string status)
        {
            List<AmEntAccountD> objList = new List<AmEntAccountD>();
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();
                    string sqlText = @" select
                                A.* 
                from AM_ENT_ACCOUNT_D A, AM_ACCOUNT_ROLE R 
                where
                    A.BANK_NO = R.BANK_NO
                and A.BANK_ACCT = R.BANK_ACCT 
                and A.STATUS = @STATUS 
                and R.ROLE_ID = @ROLE_ID
                order by A.BANK_NO 
                ";

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    sqlCmd.Parameters.Add("@ROLE_ID", SqlDbType.VarChar).Value = roleId;
                    sqlCmd.Parameters.Add("@STATUS", SqlDbType.VarChar).Value = status;
                    SqlDataReader dr = sqlCmd.ExecuteReader();
                    while (dr.Read())
                    {
                        AmEntAccountD obj = new AmEntAccountD();
                        obj.AcctTitle = dr["ACCT_TITLE"].ToString();
                        obj.BankNo = dr["BANK_NO"].ToString();
                        obj.BankAcct = dr["BANK_ACCT"].ToString();
                        obj.AcctCode = dr["ACCT_CODE"].ToString();
                        obj.CompanyId = dr["COMPANY_ID"].ToString();
                        obj.Department = dr["DEPARTMENT"].ToString();
                        obj.Name = dr["NAME"].ToString();
                        obj.Remark = dr["REMARK"].ToString();
                        obj.Status = dr["STATUS"].ToString();
                        obj.Operator = dr["OPERATOR"].ToString();
                        obj.UpdateDate = dr["UPDATE_DATE"].ToString();
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

        public void Update(AmEntAccountD entity)
        {
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();

                    string sqlText = @"
                       UPDATE "
                            + this.TableName + @"
                       SET 
                           [ACCT_TITLE] = @ACCT_TITLE
                          ,[BANK_NO] = @BANK_NO
                          ,[BANK_ACCT] = @BANK_ACCT
                          ,[ACCT_CODE] = @ACCT_CODE
                          ,[COMPANY_ID] = @COMPANY_ID
                          ,[DEPARTMENT] = @DEPARTMENT
                          ,[NAME] = @NAME
                          ,[REMARK] = @REMARK
                          ,[STATUS] = @STATUS
                          ,[OPERATOR] = @OPERATOR
                          ,[Update_Date] = @Update_Date
                       WHERE
                            [BANK_NO] = @BANK_NO and [BANK_ACCT] = @BANK_ACCT";

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    sqlCmd.Parameters.Add("@ACCT_TITLE", SqlDbType.VarChar).Value = entity.AcctTitle;
                    sqlCmd.Parameters.Add("@BANK_NO", SqlDbType.VarChar).Value = entity.BankNo;
                    sqlCmd.Parameters.Add("@BANK_ACCT", SqlDbType.VarChar).Value = entity.BankAcct;
                    sqlCmd.Parameters.Add("@ACCT_CODE", SqlDbType.VarChar).Value = entity.AcctCode;
                    sqlCmd.Parameters.Add("@COMPANY_ID", SqlDbType.VarChar).Value = entity.CompanyId;
                    sqlCmd.Parameters.Add("@DEPARTMENT", SqlDbType.VarChar).Value = entity.Department;
                    sqlCmd.Parameters.Add("@NAME", SqlDbType.VarChar).Value = entity.Name;
                    sqlCmd.Parameters.Add("@REMARK", SqlDbType.VarChar).Value = entity.Remark;
                    sqlCmd.Parameters.Add("@STATUS", SqlDbType.VarChar).Value = entity.Status;
                    sqlCmd.Parameters.Add("@OPERATOR", SqlDbType.VarChar).Value = entity.Operator;
                    sqlCmd.Parameters.Add("@Update_Date", SqlDbType.VarChar).Value = DateTime.Now.ToString("yyyyMMddHHmmss");

                    sqlCmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                log.Debug(ex.StackTrace);
                log.Debug(ex.Message);
            }
        }

        public void Delete(string bankNo, string bankAcct)
        {
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();

                    string sqlText = @"
                       DELETE from 
                        " + this.TableName + @"
                       WHERE
                            [BANK_NO] = @BANK_NO and [BANK_ACCT] = @BANK_ACCT";

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    sqlCmd.Parameters.Add("@BANK_NO", SqlDbType.VarChar).Value = bankNo;
                    sqlCmd.Parameters.Add("@BANK_ACCT", SqlDbType.VarChar).Value = bankAcct;

                    sqlCmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                log.Debug(ex.StackTrace);
                log.Debug(ex.Message);
            }
        }

        public bool Exist(string bankNo, string bankAcct)
        {
            Int32 count = 0;
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();
                    string sqlText =
                            @"select 
                              count(*)
                          from 
                              " + this.TableName + @" 
                          where
                              BANK_NO = @BANK_NO and BANK_ACCT = @BANK_ACCT
                         ";
                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    sqlCmd.Parameters.Add("@BANK_NO", SqlDbType.VarChar).Value = bankNo;
                    sqlCmd.Parameters.Add("@BANK_ACCT", SqlDbType.VarChar).Value = bankAcct;

                    count = (Int32)sqlCmd.ExecuteScalar();
                }
            }
            catch (Exception ex)
            {
                log.Debug("Query Ex: " + this.TableName);////
                log.Debug(ex.StackTrace);
                log.Debug(ex.Message);
            }
            return (count > 0);
        }
   
    }
}
