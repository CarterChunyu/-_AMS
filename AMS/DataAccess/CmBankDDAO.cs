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
    public class CmBankDDAO:BasicDAL
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(CmBankDDAO));
        public string TableName { private get; set; }

        public CmBankDDAO()
        {
            this.TableName = "CM_BANK_D";
        }

        public CmBankD FindByPk(string pk)
        {
            DataTable dt = new DataTable();
            CmBankD obj = null;

            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();

                    string sqlText = @"select
                                BANK_CODE
                                ,CA_DPT
                                ,MOD_DATE
                                ,BANK_NAME
                                ,ADD_USER
                                ,MERCHANT_NO
                            from 
                              " + this.TableName + @" 
                            where
                                CA_DPT = @CA_DPT ";

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    sqlCmd.Parameters.Add(new SqlParameter("@CA_DPT", SqlDbType.VarChar)).Value = pk;

                    SqlDataAdapter adapter = new SqlDataAdapter(sqlCmd);

                    adapter.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        obj = new CmBankD();
                        obj.BankCode = dt.Rows[0]["BANK_CODE"].ToString();
                        obj.CaDpt = dt.Rows[0]["CA_DPT"].ToString();
                        obj.BankName = dt.Rows[0]["BANK_NAME"].ToString();
                        obj.MerchantNo = dt.Rows[0]["MERCHANT_NO"].ToString();

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

        public CmBankD FindByMerchantNo(string merchantNo)
        {
            DataTable dt = new DataTable();
            CmBankD obj = null;

            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();

                    string sqlText = @"select
                                BANK_CODE
                                ,CA_DPT
                                ,MOD_DATE
                                ,BANK_NAME
                                ,ADD_USER
                                ,MERCHANT_NO
                            from 
                              " + this.TableName + @" 
                            where
                                MERCHANT_NO = @MERCHANT_NO ";

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    sqlCmd.Parameters.Add(new SqlParameter("@MERCHANT_NO", SqlDbType.VarChar)).Value = merchantNo;

                    SqlDataAdapter adapter = new SqlDataAdapter(sqlCmd);

                    adapter.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        obj = new CmBankD();
                        obj.BankCode = dt.Rows[0]["BANK_CODE"].ToString();
                        obj.CaDpt = dt.Rows[0]["CA_DPT"].ToString();
                        obj.BankName = dt.Rows[0]["BANK_NAME"].ToString();
                        obj.MerchantNo = dt.Rows[0]["MERCHANT_NO"].ToString();

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
    }
}
