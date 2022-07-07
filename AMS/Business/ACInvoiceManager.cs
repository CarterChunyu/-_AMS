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
    public class ACInvoiceManager
    {
        public ACInvoiceDAO ACInvoiceDAO { get; set; }

        public enum PayType
        {
            T00004,   // ICASH
            E00005    // ICP
        }

        public ACInvoiceManager()
        {
            ACInvoiceDAO = new ACInvoiceDAO();
        }

        public List<ACInvoiceData> GetACInvoiceReport(string member_id)
        {
            return this.GetACInvoiceReport(member_id, "", "");
        }

        public List<ACInvoiceData> GetACInvoiceReport(string member_id, string reward_start_date, string reward_end_date)
        {
            DataTable result = new DataTable();
            if (member_id == PayType.E00005.ToString())
            { result = this.ACInvoiceDAO.GetACICPInvoiceReport(reward_start_date, reward_end_date); }
            else if (member_id == PayType.T00004.ToString())
            { result = this.ACInvoiceDAO.GetACICASHInvoiceReport(reward_start_date, reward_end_date); }

            List<ACInvoiceData> list = new List<ACInvoiceData>();
            foreach (DataRow row in result.Rows)
            {
                list.Add(new ACInvoiceData()
                {
                    WEEK = int.Parse("" + row["WEEK"]),  
                    CPT_DATE = DateTime.Parse("" + row["CPT_DATE"]),
                    REWARD_START_DATE = DateTime.Parse("" + row["REWARD_START_DATE"]),
                    REWARD_END_DATE = DateTime.Parse("" + row["REWARD_END_DATE"]),
                    PRE_ISSUANCE_AMT = decimal.Parse("" + row["PRE_ISSUANCE_AMT"]),
                    PRE_BALANCE = decimal.Parse("" + row["PRE_BALANCE"]),
                    REWARD_AMT = decimal.Parse("" + row["REWARD_AMT"]),
                    TOTAL_REWARD_AMT = decimal.Parse("" + row["TOTAL_REWARD_AMT"]),
                    BALANCE = decimal.Parse("" + row["BALANCE"]), 
                    OVER_LIMIT = "" + row["OVER_LIMIT"],
                    BINDING_COUNT = int.Parse("" + row["BINDING_COUNT"]),
                    ESTIMATE_INVOICE = decimal.Parse("" + row["ESTIMATE_INVOICE"]),
                    INVOICE_AMT = decimal.Parse("" + row["INVOICE_AMT"]),
                    CAN_APPLY = "" + row["CAN_APPLY"], 
                    NEXT_STAGE = int.Parse("" + row["NEXT_STAGE"]), 
                    IS_APPLY = "" + row["IS_APPLY"],
                    STAGE_ALREADY_REWARD = int.Parse("" + row["STAGE_ALREADY_REWARD"])
                }); 
            }

            return list;
        }

        public DataRow GetACInvoiceData(string member_id, string reward_start_date, string reward_end_date)
        {
            if (member_id == PayType.E00005.ToString())
            { return this.ACInvoiceDAO.GetACICPInvoiceData(reward_start_date, reward_end_date); }
            else if (member_id == PayType.T00004.ToString())
            { return this.ACInvoiceDAO.GetACICASHInvoiceData(reward_start_date, reward_end_date); }
            else
            { return null; }
        }

        public void UpdateACInvoiceData(string member_id, string cpt_date, string reward_start_date, string reward_end_date, string is_apply, string note, string user_id, string user_ip, decimal invoice_amt)
        {
            try
            {
                if (member_id == PayType.E00005.ToString())
                { this.ACInvoiceDAO.UpdateACICPInvoiceData(cpt_date, reward_start_date, reward_end_date, is_apply, note, user_id, user_ip, invoice_amt); }
                else if (member_id == PayType.T00004.ToString())
                { this.ACInvoiceDAO.UpdateACICASHInvoiceData(cpt_date, reward_start_date, reward_end_date, is_apply, note, user_id, user_ip, invoice_amt); }
                else
                { throw new Exception("找不到支付方式"); }
                
            }
            catch (Exception ex)
            { throw ex; }
        }
    }
}
