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
    public class CrBankAdjcaseMDAO :BasicDAL
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(CrBankAdjcaseMDAO));
        public string TableName { private get; set; }

        public CrBankAdjcaseMDAO()
        {
            this.TableName = "CR_BANK_ADJCASE_M";
        }

        public void Insert(CrBankAdjcaseM entity)
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
                                   ,[ADJCASE_INFO]
                                   ,[ADJCASE_CONTEXT]
                                   ,[ADJ_DATE]
                                   ,[CPT_DATE]
                                   ,[REMITTANCE_DATE]
                                   ,[ADJ_FLG]
                                   ,[UPD_DATETIME]
                                    )
                            VALUES
                                   (
                                    @ADJCASE_NO
                                   ,@ADJCASE_INFO
                                   ,@ADJCASE_CONTEXT
                                   ,@ADJ_DATE
                                   ,@CPT_DATE
                                   ,@REMITTANCE_DATE
                                   ,@ADJ_FLG
                                   ,@UPD_DATETIME
                                    )";

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    sqlCmd.Parameters.Add("@ADJCASE_NO", SqlDbType.VarChar).Value = entity.AdjCaseNo;
                    sqlCmd.Parameters.Add("@ADJCASE_INFO", SqlDbType.NVarChar).Value = entity.AdjCaseInfo;
                    sqlCmd.Parameters.Add("@ADJCASE_CONTEXT", SqlDbType.VarChar).Value = entity.AdjCaseContext;
                    sqlCmd.Parameters.Add("@ADJ_DATE", SqlDbType.VarChar).Value = entity.AdjDate;
                    sqlCmd.Parameters.Add("@CPT_DATE", SqlDbType.VarChar).Value = entity.CptDate;
                    sqlCmd.Parameters.Add("@REMITTANCE_DATE", SqlDbType.VarChar).Value = entity.RemittanceDate;
                    sqlCmd.Parameters.Add("@ADJ_FLG", SqlDbType.VarChar).Value = entity.AdjFlag;
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

        public CrBankAdjcaseM FindByPk(string caseNo)
        {
            DataTable dt = new DataTable();
            CrBankAdjcaseM obj = null;

            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();

                    string sqlText = @"select
                                ADJCASE_NO
                                ,ADJCASE_INFO
                                ,ADJCASE_CONTEXT
                                ,ADJ_DATE
                                ,CPT_DATE
                                ,REMITTANCE_DATE
                                ,ADJ_FLG
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
                        obj = new CrBankAdjcaseM();
                        obj.AdjCaseNo = dt.Rows[0]["ADJCASE_NO"].ToString();
                        obj.AdjCaseInfo = dt.Rows[0]["ADJCASE_INFO"].ToString();
                        obj.AdjCaseContext = dt.Rows[0]["ADJCASE_CONTEXT"].ToString();
                        obj.AdjDate = dt.Rows[0]["ADJ_DATE"].ToString();
                        obj.CptDate = dt.Rows[0]["CPT_DATE"].ToString();

                        obj.RemittanceDate = dt.Rows[0]["REMITTANCE_DATE"].ToString();
                        obj.AdjFlag = dt.Rows[0]["ADJ_FLG"].ToString();
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

        public List<CrBankAdjcaseM> FindByAdjDate(string adjDate)
        {
            List<CrBankAdjcaseM> objList = new List<CrBankAdjcaseM>();
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();
                    string sqlText = @" select
                                ADJCASE_NO
                                ,ADJCASE_INFO
                                ,ADJCASE_CONTEXT
                                ,ADJ_DATE
                                ,CPT_DATE
                                ,REMITTANCE_DATE
                                ,ADJ_FLG
                                ,UPD_DATETIME
                from " + this.TableName + @" 
                where
                     ADJ_DATE = @ADJ_DATE 
                order by ADJCASE_NO 
                ";

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    sqlCmd.Parameters.Add("@ADJ_DATE", SqlDbType.VarChar).Value = adjDate;
                    SqlDataReader dr = sqlCmd.ExecuteReader();
                    while (dr.Read())
                    {
                        CrBankAdjcaseM obj = new CrBankAdjcaseM();
                        obj.AdjCaseNo = dr["ADJCASE_NO"].ToString();
                        obj.AdjCaseInfo = dr["ADJCASE_INFO"].ToString();
                        obj.AdjCaseContext = dr["ADJCASE_CONTEXT"].ToString();
                        obj.AdjDate = dr["ADJ_DATE"].ToString();
                        obj.CptDate = dr["CPT_DATE"].ToString();
                        obj.RemittanceDate = dr["REMITTANCE_DATE"].ToString();
                        obj.AdjFlag = dr["ADJ_FLG"].ToString();
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

        public bool Exist(string caseNo)
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
                              ADJCASE_NO = @ADJCASE_NO
                         ";
                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    sqlCmd.Parameters.Add("@ADJCASE_NO", SqlDbType.VarChar).Value = caseNo;

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
