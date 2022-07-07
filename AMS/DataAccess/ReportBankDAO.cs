using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.IO;

using log4net;
using Domain;

namespace DataAccess
{
    public class ReportBankDAO:BasicDAL
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ReportBankDAO));

        //private GmSchemaObjDAO gmSchema = new GmSchemaObjDAO();

        public DataTable ReportBank161001(string sDate, string eDate, string bank, string cardId)
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();
                    string sqlText = File.ReadAllText(System.AppDomain.CurrentDomain.BaseDirectory + "\\Sql\\ReportBank\\ReportBank161001_01.sql", Encoding.GetEncoding("big5"));

                    SqlCommand comm = new SqlCommand(sqlText, sqlConn);
                    comm.Parameters.Add("@bank", SqlDbType.VarChar).Value = bank;
                    comm.Parameters.Add("@cardId", SqlDbType.VarChar).Value = cardId;
                    comm.Parameters.Add("@sDate", SqlDbType.VarChar).Value = sDate;
                    comm.Parameters.Add("@eDate", SqlDbType.VarChar).Value = eDate;

                    SqlDataAdapter adapter = new SqlDataAdapter(comm);
                    adapter.Fill(dt);

                }

            }
            catch (Exception ex)
            {
                log.Debug(ex.Message);
            }

            return dt;
        }

        public DataTable ReportBank161002(string sDate, string eDate,string bankMerchant)
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();
                    string sqlText = File.ReadAllText(System.AppDomain.CurrentDomain.BaseDirectory + "\\Sql\\ReportBank\\ReportBank161002_01.sql", Encoding.GetEncoding("big5"));

                    SqlCommand comm = new SqlCommand(sqlText, sqlConn);
                    comm.Parameters.Add("@bankMerchant", SqlDbType.VarChar).Value = bankMerchant;                    
                    comm.Parameters.Add("@sDate", SqlDbType.VarChar).Value = sDate;
                    comm.Parameters.Add("@eDate", SqlDbType.VarChar).Value = eDate;

                    SqlDataAdapter adapter = new SqlDataAdapter(comm);
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
