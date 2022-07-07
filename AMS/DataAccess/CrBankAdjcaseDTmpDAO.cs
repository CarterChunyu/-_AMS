using Domain.Entities;
using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class CrBankAdjcaseDTmpDAO :BasicDAL
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(CrBankAdjcaseDTmpDAO));
        public string TableName { private get; set; }

        public CrBankAdjcaseDTmpDAO()
        {
            this.TableName = "CR_BANK_ADJCASE_D_TMP";
        }

        public void Insert(CrBankAdjcaseDTmp entity)
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
                                    [ADJCASE_NO]                                  
                                   ,[BANK_MERCHANT]
                                   ,[ICC_NO]
                                   ,[ADJ_AMT]
                                   ,[UPD_DATETIME]
                                   ,[CREATE_USER]
                                   ,[UPDATE_USER]
                                    )
                            VALUES
                                   (
                                    @ADJCASE_NO
                                   ,@BANK_MERCHANT
                                   ,@ICC_NO
                                   ,@ADJ_AMT
                                   ,@UPD_DATETIME
                                   ,@CREATE_USER
                                   ,@UPDATE_USER
                                    )";

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    sqlCmd.Parameters.Add("@ADJCASE_NO", SqlDbType.VarChar).Value = entity.AdjCaseNo;
                    sqlCmd.Parameters.Add("@BANK_MERCHANT", SqlDbType.NVarChar).Value = entity.BankMerchant;
                    sqlCmd.Parameters.Add("@ICC_NO", SqlDbType.VarChar).Value = entity.IccNo;
                    sqlCmd.Parameters.Add("@ADJ_AMT", SqlDbType.VarChar).Value = entity.AdjAmt;
                    sqlCmd.Parameters.Add("@UPD_DATETIME", SqlDbType.VarChar).Value = DateTime.Now.ToString("yyyyMMddHHmmss");
                    sqlCmd.Parameters.Add("@CREATE_USER", SqlDbType.VarChar).Value = entity.CreateUser;
                    sqlCmd.Parameters.Add("@UPDATE_USER", SqlDbType.VarChar).Value = entity.UpdateUser;

                    sqlCmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                log.Debug(ex.StackTrace);
                log.Debug(ex.Message);
            }
        }

        public CrBankAdjcaseDTmp FindByPk(string caseNo, string iccNo)
        {
            DataTable dt = new DataTable();
            CrBankAdjcaseDTmp obj = null;

            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();

                    string sqlText = @"select
                                ADJCASE_NO
                                ,BANK_MERCHANT
                                ,ICC_NO
                                ,ADJ_AMT
                                ,UPD_DATETIME
                                ,CREATE_USER
                                ,UPDATE_USER
                            from 
                              " + this.TableName + @" 
                            where
                                ADJCASE_NO = @ADJCASE_NO and ICC_NO = @ICC_NO ";

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    sqlCmd.Parameters.Add(new SqlParameter("@ADJCASE_NO", SqlDbType.VarChar)).Value = caseNo;
                    sqlCmd.Parameters.Add(new SqlParameter("@ICC_NO", SqlDbType.VarChar)).Value = iccNo;
                    SqlDataAdapter adapter = new SqlDataAdapter(sqlCmd);

                    adapter.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        obj = new CrBankAdjcaseDTmp();
                        obj.AdjCaseNo = dt.Rows[0]["ADJCASE_NO"].ToString();
                        obj.BankMerchant = dt.Rows[0]["BANK_MERCHANT"].ToString();
                        obj.IccNo = dt.Rows[0]["ICC_NO"].ToString();
                        obj.AdjAmt = dt.Rows[0]["ADJ_AMT"].ToString();
                        obj.UptDatetime = dt.Rows[0]["UPD_DATETIME"].ToString();
                        obj.CreateUser = dt.Rows[0]["CREATE_USER"].ToString();
                        obj.UpdateUser = dt.Rows[0]["UPDATE_USER"].ToString();

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

        public void Update(CrBankAdjcaseDTmp entity)
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
                           [BANK_MERCHANT] = @BANK_MERCHANT
                          ,[ADJ_AMT] = @ADJ_AMT
                          ,[UPD_DATETIME] = @UPD_DATETIME
                       WHERE
                            [ADJCASE_NO] = @ADJCASE_NO and [ICC_NO] = @ICC_NO ";

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    sqlCmd.Parameters.Add("@ADJCASE_NO", SqlDbType.VarChar).Value = entity.AdjCaseNo;
                    sqlCmd.Parameters.Add("@BANK_MERCHANT", SqlDbType.NVarChar).Value = entity.BankMerchant;
                    sqlCmd.Parameters.Add("@ICC_NO", SqlDbType.VarChar).Value = entity.IccNo;
                    sqlCmd.Parameters.Add("@ADJ_AMT", SqlDbType.VarChar).Value = entity.AdjAmt;
                    sqlCmd.Parameters.Add("@UPD_DATETIME", SqlDbType.VarChar).Value = DateTime.Now.ToString("yyyyMMddHHmmss");

                    sqlCmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                log.Debug(ex.StackTrace);
                log.Debug(ex.Message);
            }
        }

        public List<CrBankAdjcaseDTmp> FindByAdjCaseNo(string caseNo)
        {
            List<CrBankAdjcaseDTmp> objList = new List<CrBankAdjcaseDTmp>();
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();
                    string sqlText = @" select
                                ADJCASE_NO
                                ,BANK_MERCHANT
                                ,ICC_NO
                                ,ADJ_AMT
                                ,UPD_DATETIME
                                ,CREATE_USER
                                ,UPDATE_USER
                from " + this.TableName + @" 
                where
                     ADJCASE_NO = @ADJCASE_NO 
                order by ICC_NO 
                ";

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    sqlCmd.Parameters.Add("@ADJCASE_NO", SqlDbType.VarChar).Value = caseNo;
                    SqlDataReader dr = sqlCmd.ExecuteReader();
                    while (dr.Read())
                    {
                        CrBankAdjcaseDTmp obj = new CrBankAdjcaseDTmp();
                        obj.AdjCaseNo = dr["ADJCASE_NO"].ToString();
                        obj.BankMerchant = dr["BANK_MERCHANT"].ToString();
                        obj.IccNo = dr["ICC_NO"].ToString();
                        obj.AdjAmt = dr["ADJ_AMT"].ToString();
                        obj.UptDatetime = dr["UPD_DATETIME"].ToString();
                        obj.CreateUser = dr["CREATE_USER"].ToString();
                        obj.UpdateUser = dr["UPDATE_USER"].ToString();
                       
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

        public bool Exist(string caseNo, string iccNo)
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
                               [ADJCASE_NO] = @ADJCASE_NO and [ICC_NO] = @ICC_NO";

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    sqlCmd.Parameters.Add(new SqlParameter("@ADJCASE_NO", SqlDbType.VarChar)).Value = caseNo;
                    sqlCmd.Parameters.Add(new SqlParameter("@ICC_NO", SqlDbType.VarChar)).Value = iccNo;

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

        public void Delete(string caseNo, string iccNo)
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
                            [ADJCASE_NO] = @ADJCASE_NO and [ICC_NO] = @ICC_NO";

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    sqlCmd.Parameters.Add(new SqlParameter("@ADJCASE_NO", SqlDbType.VarChar)).Value = caseNo;
                    sqlCmd.Parameters.Add(new SqlParameter("@ICC_NO", SqlDbType.VarChar)).Value = iccNo;

                    sqlCmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                log.Debug(ex.StackTrace);
                log.Debug(ex.Message);
            }
        }
    }
}
