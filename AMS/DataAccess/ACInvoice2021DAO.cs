using Common.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class ACInvoice2021DAO : BasicDAL
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ACInvoice2021DAO));

        public ACInvoice2021DAO()
        {

        }

        public DataTable GetACInvoiceReport(string member_id,string coupon_tp,string reward_start_date,string reward_end_date)
        {
            DataTable dt = new DataTable();
            try
            {
                using(SqlConnection sqlConn =new SqlConnection(this._connectionString))
                {

                    string sqlText = string.Concat(new object[] {
                        "EXEC [dbo].[AC_GET_INVOICE_DATA_2021]",Environment.NewLine,
                        "   @MEMBER_ID = '",string.IsNullOrEmpty(member_id) ? "" : member_id,"',",Environment.NewLine,
                        "   @COUPON_TP = '",string.IsNullOrEmpty(coupon_tp) ? "" : coupon_tp,"'",Environment.NewLine
                    });

                    //List<SqlParameter> listPara = new List<SqlParameter>();
                    //listPara.Add(new SqlParameter("@REWARD_START_DATE", string.IsNullOrEmpty(reward_start_date) ? "" : reward_start_date));
                    //listPara.Add(new SqlParameter("@REWARD_END_DATE", string.IsNullOrEmpty(reward_end_date) ? "" : reward_end_date));
                    //listPara.Add(new SqlParameter("@MEMBER_ID", string.IsNullOrEmpty(member_id) ? "" : member_id));
                    //listPara.Add(new SqlParameter("@COUPON_TP", string.IsNullOrEmpty(coupon_tp) ? "" : coupon_tp));

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    //foreach (SqlParameter para in listPara)
                    //{
                    //    sqlCmd.Parameters.Add(para);
                    //}
                    SqlDataAdapter adapter = new SqlDataAdapter(sqlCmd);
                    adapter.Fill(dt);
                }
            }catch(Exception ex)
            {
                log.Debug(ex.StackTrace);
                log.Debug(ex.Message);
            }
            return dt;
        }

        public DataTable UpdateACInvoice(string member_id,string coupon_tp,string cpt_date,string note,string is_apply)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {

                    string sqlText = string.Concat(new object[] {
                        "EXEC [dbo].[AC_UPDATE_INVOICE_DATA_2021]",Environment.NewLine,
                        "   @MEMBER_ID = '",string.IsNullOrEmpty(member_id) ? "" : member_id,"',",Environment.NewLine,
                        "   @COUPON_TP = '",string.IsNullOrEmpty(coupon_tp) ? "" : coupon_tp,"',",Environment.NewLine,
                        "   @CPT_DATE = '",string.IsNullOrEmpty(cpt_date)?"":cpt_date,"',",Environment.NewLine,
                        "   @NOTE = '",string.IsNullOrEmpty(note)?"":note,"',",Environment.NewLine,
                        "   @IS_APPLY = '",string.IsNullOrEmpty(is_apply)?"N":is_apply,"'",Environment.NewLine,
                    });

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    SqlDataAdapter adapter = new SqlDataAdapter(sqlCmd);
                    adapter.Fill(dt);
                }
            }
            catch(Exception ex)
            {
                log.Debug(ex.StackTrace);
                log.Debug(ex.Message);
            }
            return dt;
        }  
    }
}
