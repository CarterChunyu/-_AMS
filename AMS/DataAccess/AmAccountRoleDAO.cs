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
    public class AmAccountRoleDAO : BasicDAL, ICloneable
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(AmRolesDAO));
        public string TableName { private get; set; }

        public AmAccountRoleDAO()
        {
            this.TableName = "AM_ACCOUNT_ROLE";
        }

        public void Insert(string bankNo, string bankAcct, string roleId)
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
                                   [BANK_NO]
                                   ,[BANK_ACCT]
                                   ,[ROLE_ID]
                                    )
                            VALUES
                                   (
                                   @BANK_NO
                                   ,@BANK_ACCT
                                   ,@ROLE_ID
                                    )";

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    sqlCmd.Parameters.Add("@BANK_NO", SqlDbType.VarChar).Value = bankNo;
                    sqlCmd.Parameters.Add("@BANK_ACCT", SqlDbType.VarChar).Value = bankAcct;
                    sqlCmd.Parameters.Add("@ROLE_ID", SqlDbType.VarChar).Value = roleId;

                    sqlCmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                log.Debug(ex.StackTrace);
                log.Debug(ex.Message);
            }
        }

        public void Delete(string roleId, string bankNo, string bankAcct)
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
                            [BANK_NO] = @BANK_NO and [BANK_ACCT] = @BANK_ACCT and [ROLE_ID] = @ROLE_ID";

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    sqlCmd.Parameters.Add("@BANK_NO", SqlDbType.VarChar).Value = bankNo;
                    sqlCmd.Parameters.Add("@BANK_ACCT", SqlDbType.VarChar).Value = bankAcct;
                    sqlCmd.Parameters.Add("@ROLE_ID", SqlDbType.VarChar).Value = roleId;

                    sqlCmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                log.Debug(ex.StackTrace);
                log.Debug(ex.Message);
            }
        }

        public bool IsRoleBankAcct(string bankAcct, string roleId)
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
                              ROLE_ID = @ROLE_ID and BANK_ACCT = @BANK_ACCT
                         ";
                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    sqlCmd.Parameters.Add("@ROLE_ID", SqlDbType.VarChar).Value = roleId;
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

        public List<AmRoles> FindRolesByBankInfo(string bankNo, string bankAcct)
        {
            List<AmRoles> objList = new List<AmRoles>();
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();
                    string sqlText = @" select
                                A.* 
                from AM_ROLES A, AM_ACCOUNT_ROLE R 
                where
                    A.ROLE_ID = R.ROLE_ID 
                and R.BANK_NO = @BANK_NO
                and R.BANK_ACCT = @BANK_ACCT
                ";

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    sqlCmd.Parameters.Add("@BANK_NO", SqlDbType.VarChar).Value = bankNo;
                    sqlCmd.Parameters.Add("@BANK_ACCT", SqlDbType.VarChar).Value = bankAcct;
                    SqlDataReader dr = sqlCmd.ExecuteReader();
                    while (dr.Read())
                    {
                        AmRoles obj = new AmRoles();
                        obj.RoleId = dr["ROLE_ID"].ToString();
                        obj.RoleName = dr["ROLE_NAME"].ToString();
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

        public bool Exist(string roleId, string bankNo, string bankAcct)
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
                              ROLE_ID=@ROLE_ID and BANK_NO = @BANK_NO and BANK_ACCT = @BANK_ACCT
                         ";
                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    sqlCmd.Parameters.Add("@ROLE_ID", SqlDbType.VarChar).Value = roleId;
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

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
