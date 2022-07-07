using DataAccess;
using Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business
{
    public class ACInvoice2021Manager
    {
        public ACInvoice2021DAO ACInvoice2021DAO { get; set; }

        public enum PayType
        {
            T00004,   // ICASH
            E00005    // ICP
        }

        public class Response
        {
            public int RtnCode { get; set; }
            public string RtnMsg { get; set; }
        }

        public ACInvoice2021Manager()
        {
            ACInvoice2021DAO = new ACInvoice2021DAO();
        }

        public List<ACInvoiceData2021> GetACInvoiceReport(string member_id, string coupon_tp, string reward_start_date=null, string reward_end_date=null)
        {
            DataTable dt = this.ACInvoice2021DAO.GetACInvoiceReport(member_id, coupon_tp, reward_start_date, reward_end_date);
            List<ACInvoiceData2021> list = new List<ACInvoiceData2021>();
            foreach(DataRow row in dt.Rows)
            {
                list.Add(new ACInvoiceData2021()
                {
                    STAGE = int.Parse(row["STAGE"].ToString()),
                    CPT_DATE = DateTime.ParseExact(row["CPT_DATE"].ToString(), "yyyyMMdd", null, System.Globalization.DateTimeStyles.AllowWhiteSpaces),
                    REWARD_START_DATE = DateTime.ParseExact(row["REWARD_START_DATE"].ToString(), "yyyyMMdd", null, System.Globalization.DateTimeStyles.AllowWhiteSpaces),
                    REWARD_END_DATE = DateTime.ParseExact(row["REWARD_END_DATE"].ToString(), "yyyyMMdd", null, System.Globalization.DateTimeStyles.AllowWhiteSpaces),
                    PRE_ISSUANCE_AMT = decimal.Parse("" + row["PRE_ISSUANCE_AMT"]),
                    REWARD_AMT = decimal.Parse("" + row["REWARD_AMT"]),
                    TOTAL_REWARD_AMT = decimal.Parse("" + row["TOTAL_REWARD_AMT"]),
                    BALANCE = decimal.Parse("" + row["BALANCE"]),
                    LIMIT = row["LIMIT"].ToString(),
                    OVER_LIMIT = "" + row["OVER_LIMIT"],
                    BINDING_COUNT_MEMBER =Convert.ToInt32(row["BINDING_COUNT_MEMBER"]),
                    TOTAL_BINDING_COUNT = Convert.ToInt32(row["TOTAL_BINDING_COUNT"]),
                    TOTAL_BINDING_AMT = Convert.ToDecimal(row["TOTAL_BINDING_AMT"]),
                    ESTIMATE_INVOICE = decimal.Parse("" + row["ESTIMATE_INVOICE"]),
                    APPLY_STAGE = Convert.ToInt32(row["APPLY_STAGE"]),
                    NOT_ACQUIRE_AMT =Convert.ToDecimal(row["NOT_ACQUIRE_AMT"]),
                    ACQUIRED_AMT = Convert.ToDecimal(row["ACQUIRED_AMT"]),
                    IS_APPLY = "" + row["IS_APPLY"],
                    NOTE = row["NOTE"].ToString()
                });
            }
            return list;
        }

        public Response UpdateACInvoice(string member_id, string coupon_tp, string cpt_date, string note, string is_apply)
        {
            DataTable dt = this.ACInvoice2021DAO.UpdateACInvoice(member_id, coupon_tp, cpt_date, note, is_apply);
            Response response = new Response();
            if (dt.Rows.Count > 0)
            {
                response.RtnCode = Convert.ToInt32(dt.Rows[0]["RtnCode"].ToString());
                response.RtnMsg = dt.Rows[0]["RtnMsg"].ToString();
            }
            return response;
        }
    }
}
