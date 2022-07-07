using Common.Logging;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class ACInvoiceRewardReport2021DAO : BasicDAL
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ACInvoiceRewardReport2021DAO));

        private string ConnectionStr { get; set; }

        public ACInvoiceRewardReport2021DAO()
        {
            if (ConfigurationManager.ConnectionStrings["105iSettle2Connection"] == null)
            {
                ConnectionStr = this._connectionString;
            }
            else
            {
                ConnectionStr = ConfigurationManager.ConnectionStrings["105iSettle2Connection"].ToString();
            }
        }

        //ICASH
        public DataSet GetICASHReport(string date,string coupon_tp)
        {
            DataSet ds = new DataSet();
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(ConnectionStr))
                {
                    string sqlText = string.Concat(new object[] {
                        "EXEC [dbo].[AC_ICASH_REWARD_STATISTICS_2021]",Environment.NewLine,
                        "   @Today = '",date,"'",Environment.NewLine,
                        "   , @MEMBER_ID = 'T00004'",Environment.NewLine,
                        "   , @COUPON_TP = '",coupon_tp,"'",Environment.NewLine
                    });

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    SqlDataAdapter adapter = new SqlDataAdapter(sqlCmd);
                    adapter.Fill(ds);
                }
            }
            catch (Exception ex)
            {
                log.Debug(ex.StackTrace);
                log.Debug(ex.Message);
            }
            return ds;
        }

        //ICP
        public DataSet GetICPReport(string date, string coupon_tp)
        {
            DataSet ds = new DataSet();
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(ConnectionStr))
                {
                    string sqlText = string.Concat(new object[] {
                        "EXEC [dbo].[AC_ICP_REWARD_STATISTICS_2021]",Environment.NewLine,
                        "   @Today = '",date,"'",Environment.NewLine,
                        "   , @MEMBER_ID = 'E00005'",Environment.NewLine,
                        "   , @COUPON_TP = '",coupon_tp,"'",Environment.NewLine
                    });

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    SqlDataAdapter adapter = new SqlDataAdapter(sqlCmd);
                    adapter.Fill(ds);
                }
            }
            catch (Exception ex)
            {
                log.Debug(ex.StackTrace);
                log.Debug(ex.Message);
            }
            return ds;
        }

    }
}
