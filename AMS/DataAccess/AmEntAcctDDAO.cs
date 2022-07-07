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
    public class AmEntAcctDDAO :BasicDAL
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(AmEntAcctDDAO));
        public string TableName { private get; set; }
        public string MappingTableName { private get; set; }

        public AmEntAcctDDAO()
        {
            this.TableName = "SM_ENT_ACCOUNT_D";
            this.MappingTableName = "AM_ACCOUNT_ROLE";
        }

        public void Insert(AmEntAcctD entity)
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
                                   ,[NAME]
                                   ,[REG_ID]
                                   ,[TEL]
                                   ,[FAX]
                                   ,[EMAIL]
                                   ,[INFORM_FLG]
                                   ,[MERCHANT_NO]
                                    )
                            VALUES
                                   (
                                    @ACCT_TITLE
                                   ,@BANK_NO
                                   ,@BANK_ACCT
                                   ,@NAME
                                   ,@REG_ID
                                   ,@TEL
                                   ,@FAX
                                   ,@EMAIL
                                   ,@INFORM_FLG
                                   ,@MERCHANT_NO
                                    )";

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    sqlCmd.Parameters.Add("@ACCT_TITLE", SqlDbType.VarChar).Value = entity.AcctTitle;
                    sqlCmd.Parameters.Add("@BANK_NO", SqlDbType.VarChar).Value = entity.BankNo;
                    sqlCmd.Parameters.Add("@BANK_ACCT", SqlDbType.VarChar).Value = entity.BankAcct;
                    sqlCmd.Parameters.Add("@NAME", SqlDbType.VarChar).Value = entity.Name;
                    sqlCmd.Parameters.Add("@REG_ID", SqlDbType.VarChar).Value = entity.Reg_Id;
                    sqlCmd.Parameters.Add("@TEL", SqlDbType.VarChar).Value = entity.Tel;
                    sqlCmd.Parameters.Add("@FAX", SqlDbType.VarChar).Value = entity.Fax;
                    sqlCmd.Parameters.Add("@EMAIL", SqlDbType.VarChar).Value = entity.Email;
                    sqlCmd.Parameters.Add("@INFORM_FLG", SqlDbType.VarChar).Value = entity.Inform_Flg;
                    sqlCmd.Parameters.Add("@MERCHANT_NO", SqlDbType.VarChar).Value = entity.Merchant_No;

                    sqlCmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                log.Debug(ex.StackTrace);
                log.Debug(ex.Message);
            }
        }

        public AmEntAcctD FindByPk(string bankNo, string bankAcct)
        {
            DataTable dt = new DataTable();
            AmEntAcctD obj = null;

            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();

                    string sqlText = @"select
                                ACCT_TITLE
                                ,BANK_NO
                                ,BANK_ACCT
                                ,NAME
                                ,REG_ID
                                ,TEL
                                ,FAX
                                ,EMAIL
                                ,INFORM_FLG
                                ,MERCHANT_NO
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
                        obj = new AmEntAcctD();
                        obj.AcctTitle = dt.Rows[0]["ACCT_TITLE"].ToString();
                        obj.BankNo = dt.Rows[0]["BANK_NO"].ToString();
                        obj.BankAcct = dt.Rows[0]["BANK_ACCT"].ToString();
                        obj.Name = dt.Rows[0]["NAME"].ToString();
                        obj.Reg_Id = dt.Rows[0]["REG_ID"].ToString();
                        obj.Tel = dt.Rows[0]["TEL"].ToString();
                        obj.Fax = dt.Rows[0]["FAX"].ToString();
                        obj.Email = dt.Rows[0]["EMAIL"].ToString();
                        obj.Inform_Flg = dt.Rows[0]["INFORM_FLG"].ToString();
                        obj.Merchant_No = dt.Rows[0]["MERCHANT_NO"].ToString();

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

        public List<AmEntAcctD> FindByStatus(string status)
        {
            List<AmEntAcctD> objList = new List<AmEntAcctD>();
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();
                    string sqlText = @" select
                                ACCT_TITLE
                                ,BANK_NO
                                ,BANK_ACCT
                                ,NAME
                                ,REG_ID
                                ,TEL
                                ,FAX
                                ,EMAIL
                                ,INFORM_FLG
                                ,MERCHANT_NO
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
                        AmEntAcctD obj = new AmEntAcctD();
                        obj.AcctTitle = dr["ACCT_TITLE"].ToString();
                        obj.BankNo = dr["BANK_NO"].ToString();
                        obj.BankAcct = dr["BANK_ACCT"].ToString();
                        obj.Name = dr["NAME"].ToString();
                        obj.Reg_Id = dr["REG_ID"].ToString();
                        obj.Tel = dr["TEL"].ToString();
                        obj.Fax = dr["FAX"].ToString();
                        obj.Email = dr["EMAIL"].ToString();
                        obj.Inform_Flg = dr["INFORM_FLG"].ToString();
                        obj.Merchant_No = dr["MERCHANT_NO"].ToString();
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

        public List<AmEntAcctD> FindByRoleStatus(string roleId, string status)
        {
            List<AmEntAcctD> objList = new List<AmEntAcctD>();
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();
                    string sqlText = @" select
                                A.* 
                from SM_ENT_ACCOUNT_D A, AM_ACCOUNT_ROLE R 
                where
                    A.BANK_NO = R.BANK_NO
                and A.BANK_ACCT = R.BANK_ACCT 
                and R.ROLE_ID = @ROLE_ID
                order by A.BANK_NO 
                ";

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    sqlCmd.Parameters.Add("@ROLE_ID", SqlDbType.VarChar).Value = roleId;
                    sqlCmd.Parameters.Add("@STATUS", SqlDbType.VarChar).Value = status;
                    SqlDataReader dr = sqlCmd.ExecuteReader();
                    while (dr.Read())
                    {
                        AmEntAcctD obj = new AmEntAcctD();
                        obj.AcctTitle = dr["ACCT_TITLE"].ToString();
                        obj.BankNo = dr["BANK_NO"].ToString();
                        obj.BankAcct = dr["BANK_ACCT"].ToString();
                        obj.Name = dr["NAME"].ToString();
                        obj.Reg_Id = dr["REG_ID"].ToString();
                        obj.Tel = dr["TEL"].ToString();
                        obj.Fax = dr["FAX"].ToString();
                        obj.Email = dr["EMAIL"].ToString();
                        obj.Inform_Flg = dr["INFORM_FLG"].ToString();
                        obj.Merchant_No = dr["MERCHANT_NO"].ToString();
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

        public void Update(AmEntAcctD entity)
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
                          ,[NAME] = @NAME
                          ,[REG_ID] = @REG_ID
                          ,[TEL] = @TEL
                          ,[FAX] = @FAX
                          ,[EMAIL] = @EMAIL
                          ,[INFORM_FLG] = @INFORM_FLG
                          ,[MERCHANT_NO] = @MERCHANT_NO
                       WHERE
                            [BANK_NO] = @BANK_NO and [BANK_ACCT] = @BANK_ACCT";

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    sqlCmd.Parameters.Add("@ACCT_TITLE", SqlDbType.VarChar).Value = entity.AcctTitle;
                    sqlCmd.Parameters.Add("@BANK_NO", SqlDbType.VarChar).Value = entity.BankNo;
                    sqlCmd.Parameters.Add("@BANK_ACCT", SqlDbType.VarChar).Value = entity.BankAcct;
                    sqlCmd.Parameters.Add("@NAME", SqlDbType.VarChar).Value = entity.Name;
                    sqlCmd.Parameters.Add("@REG_ID", SqlDbType.VarChar).Value = entity.Reg_Id;
                    sqlCmd.Parameters.Add("@TEL", SqlDbType.VarChar).Value = entity.Tel;
                    sqlCmd.Parameters.Add("@FAX", SqlDbType.VarChar).Value = entity.Fax;
                    sqlCmd.Parameters.Add("@EMAIL", SqlDbType.VarChar).Value = entity.Email;
                    sqlCmd.Parameters.Add("@INFORM_FLG", SqlDbType.VarChar).Value = entity.Inform_Flg;
                    sqlCmd.Parameters.Add("@MERCHANT_NO", SqlDbType.VarChar).Value = entity.Merchant_No;

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

        public List<AmEntAcctD> FindBySearch(string AcctTitle, string Status)
        {
            List<AmEntAcctD> objList = new List<AmEntAcctD>();
            char[] charsToTrim = { '-' };
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();
                    string sqlText = @" select
                                ACCT_TITLE
                                ,BANK_NO
                                ,BANK_ACCT
                                ,NAME
                                ,REG_ID
                                ,TEL
                                ,FAX
                                ,EMAIL
                                ,case INFORM_FLG  when 'EI' then 'email入帳通知' when 'AJ' then '傳真通知' when 'AF' then '不發送入帳通知' when 'AL' then '同時發送email及傳真通知' else '' end as INFORM_FLG
                                ,(select max(MERCHANT_STNAME) from GM_MERCHANT G where G.MERCHANT_NO=SM_ENT_ACCOUNT_D.MERCHANT_NO) as MERCHANT_NO
                    from " + this.TableName + @" 
                    where
                        1=1 ";


                    if (AcctTitle != "")
                        sqlText += "and Acct_Title like @AcctTitle ";

                    //if (Status != "")
                    //    sqlText += "and STATUS = @STATUS ";

                    sqlText += "order by BANK_NO ";
                    //log.Debug("Query End: " + sqlText);

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    sqlCmd.Parameters.Add("@AcctTitle", SqlDbType.VarChar).Value = "%"+AcctTitle+"%";
                    sqlCmd.Parameters.Add("@STATUS", SqlDbType.VarChar).Value = Status;

                    SqlDataReader dr = sqlCmd.ExecuteReader();
                    while (dr.Read())
                    {
                        AmEntAcctD obj = new AmEntAcctD();
                        
                        obj.AcctTitle = dr["ACCT_TITLE"].ToString();
                        obj.BankNo = dr["BANK_NO"].ToString();
                        obj.BankAcct = dr["BANK_ACCT"].ToString();
                        obj.Name = dr["NAME"].ToString();
                        obj.Reg_Id = dr["REG_ID"].ToString();
                        obj.Tel = dr["TEL"].ToString();
                        obj.Fax = dr["FAX"].ToString();
                        obj.Email = dr["EMAIL"].ToString();
                        obj.Inform_Flg = dr["INFORM_FLG"].ToString();
                        obj.Merchant_No = dr["MERCHANT_NO"].ToString();

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
