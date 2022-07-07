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
    public class CrBankAdjcaseDDAO :BasicDAL
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(CrBankAdjcaseDDAO));
        public string TableName { private get; set; }

        public CrBankAdjcaseDDAO()
        {
            this.TableName = "CR_BANK_ADJCASE_D";
        }

        public void Insert(CrBankAdjcaseD entity)
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
                                    )
                            VALUES
                                   (
                                    @ADJCASE_NO
                                   ,@BANK_MERCHANT
                                   ,@ICC_NO
                                   ,@ADJ_AMT
                                   ,@UPD_DATETIME
                                    )";

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

        public CrBankAdjcaseD FindByPk(string caseNo)
        {
            DataTable dt = new DataTable();
            CrBankAdjcaseD obj = null;

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
                            from 
                              " + this.TableName + @" 
                            where
                                ADJCASE_NO = @ADJCASE_NO ";

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    sqlCmd.Parameters.Add(new SqlParameter("@ADJCASE_NO", SqlDbType.VarChar)).Value = caseNo;

                    SqlDataAdapter adapter = new SqlDataAdapter(sqlCmd);

                    adapter.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        obj = new CrBankAdjcaseD();
                        obj.AdjCaseNo = dt.Rows[0]["ADJCASE_NO"].ToString();
                        obj.BankMerchant = dt.Rows[0]["BANK_MERCHANT"].ToString();
                        obj.IccNo = dt.Rows[0]["ICC_NO"].ToString();
                        obj.AdjAmt = dt.Rows[0]["ADJ_AMT"].ToString();
                        obj.UptDatetime = dt.Rows[0]["UPD_DATETIME"].ToString();

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

        public List<CrBankAdjcaseD> FindByAdjCaseNo(string caseNo)
        {
            List<CrBankAdjcaseD> objList = new List<CrBankAdjcaseD>();
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
                        CrBankAdjcaseD obj = new CrBankAdjcaseD();
                        obj.AdjCaseNo = dr["ADJCASE_NO"].ToString();
                        obj.BankMerchant = dr["BANK_MERCHANT"].ToString();
                        obj.IccNo = dr["ICC_NO"].ToString();
                        obj.AdjAmt = dr["ADJ_AMT"].ToString();
                        obj.UptDatetime = dr["UPD_DATETIME"].ToString();
                       
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
