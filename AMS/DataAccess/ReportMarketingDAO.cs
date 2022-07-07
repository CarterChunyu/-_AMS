using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.IO;

using log4net;

namespace DataAccess
{
    public class ReportMarketingDAO : BasicDAL
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ReportMarketingDAO));
        public ReportMarketingDAO()
        {
        }

        public DataTable ReportMarketing160101(string start, string end, string merchantNo,string kind1,string kind2,string kind3)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();

                    string sqlText = File.ReadAllText(System.AppDomain.CurrentDomain.BaseDirectory + "\\Sql\\ReportMarketing\\ReportMarketing160101_01.sql", Encoding.GetEncoding("big5"));

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    sqlCmd.Parameters.Add("@TRANS_DATE_B", SqlDbType.VarChar).Value = start;
                    sqlCmd.Parameters.Add("@TRANS_DATEE_E", SqlDbType.VarChar).Value = end;
                    sqlCmd.Parameters.Add("@MERCHANT_NO", SqlDbType.VarChar).Value = merchantNo;
                    sqlCmd.Parameters.Add("@KIND1", SqlDbType.VarChar).Value = kind1;
                    sqlCmd.Parameters.Add("@KIND2", SqlDbType.VarChar).Value = kind2;
                    sqlCmd.Parameters.Add("@KIND3", SqlDbType.VarChar).Value = kind3;

                    SqlDataAdapter adapter = new SqlDataAdapter(sqlCmd);

                    adapter.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                log.Debug(ex.Message);
            }
            return dt;
        }

        public DataTable ReportMarketing160102(string start, string end, string merchantNo)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();

                    string sqlText = File.ReadAllText(System.AppDomain.CurrentDomain.BaseDirectory + "\\Sql\\ReportMarketing\\ReportMarketing160102.sql", Encoding.GetEncoding("big5"));

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    sqlCmd.Parameters.Add("@TRANS_DATE_B", SqlDbType.VarChar).Value = start;
                    sqlCmd.Parameters.Add("@TRANS_DATEE_E", SqlDbType.VarChar).Value = end;
                    sqlCmd.Parameters.Add("@MERCHANT_NO", SqlDbType.VarChar).Value = merchantNo;
                    SqlDataAdapter adapter = new SqlDataAdapter(sqlCmd);

                    adapter.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                log.Debug(ex.Message);
            }
            return dt;
        }
    }
}