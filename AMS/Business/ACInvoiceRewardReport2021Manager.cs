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
    public class ACInvoiceRewardReport2021Manager
    {
        public ACInvoiceRewardReport2021DAO ACInvoiceRewardReportDAO { get; set; }

        public enum PayType
        {
            T00004,   // ICASH
            E00005    // ICP
        }

        public ACInvoiceRewardReport2021Manager()
        {
            ACInvoiceRewardReportDAO = new ACInvoiceRewardReport2021DAO();
        }

        public ACInvoiceRewardReport2021.ReportData GetReportData(string member_id, string date,string coupon_tp)
        {
            DataSet ds = new DataSet();
            ACInvoiceRewardReport2021.ReportData reportData = new ACInvoiceRewardReport2021.ReportData();
            if (!string.IsNullOrWhiteSpace(member_id) && member_id == PayType.T00004.ToString())
            {
                ds = this.ACInvoiceRewardReportDAO.GetICASHReport(date, coupon_tp);
            }
            else
            {
                ds = this.ACInvoiceRewardReportDAO.GetICPReport(date, coupon_tp);
            }

            if (ds.Tables[0] != null)
            {
                reportData.Date = new ACInvoiceRewardReport2021.date();
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    reportData.Date.DBNO = row[0].ToString();
                    reportData.Date.SEDATE = row[1].ToString();
                }
            }

            if (ds.Tables[1] != null)
            {
                reportData.Total = new ACInvoiceRewardReport2021.total();
                foreach (DataRow row in ds.Tables[1].Rows)
                {
                    reportData.Total.DBNO = row[0].ToString();
                    reportData.Total.TOTAL = row[1].ToString();
                }
            }

            if (ds.Tables[2] != null)
            {
                reportData.Reports = new List<ACInvoiceRewardReport2021.Report>();
                foreach (DataRow row in ds.Tables[2].Rows)
                {
                    reportData.Reports.Add(new ACInvoiceRewardReport2021.Report
                    {
                        DBNO = row[0].ToString(),
                        TW_ID = int.Parse(row[1].ToString()),
                        CUS_UUID = row[2].ToString(),
                        AUTH_TIME = DateTime.ParseExact(row[3].ToString(), "yyyyMMddHHmmss", null).ToString("yyyy-MM-dd"),
                        TXN_NO = row[4].ToString(),
                        TRANS_CNT = int.Parse(row[5].ToString()),
                        TRANS_AMT = decimal.Parse(row[6].ToString()).ToString("#,0"),
                        REWARD_AMT = decimal.Parse(row[7].ToString()).ToString("#,0"),
                        REMARK = row[8].ToString()
                    });
                }
            }

            if (ds.Tables[3] != null)
            {
                reportData.Details = new List<ACInvoiceRewardReport2021.Detail>();
                foreach (DataRow row in ds.Tables[3].Rows)
                {
                    reportData.Details.Add(new ACInvoiceRewardReport2021.Detail
                    {
                        DBNO = row[0].ToString(),
                        TW_ID = int.Parse(row[1].ToString()),
                        CUS_UUID = row[2].ToString(),
                        STORE_NAME = row[3].ToString(),
                        TRANS_DATE = DateTime.ParseExact(row[4].ToString(), "yyyyMMdd", null).ToString("yyyy-MM-dd"),
                        TXLOG_ID = row[5].ToString(),
                        TRANS_AMT =decimal.Parse(row[6].ToString()).ToString("#,0"),
                        TAX_ID = row[7].ToString(),
                        TRANS_STORE_TYPE = row[8].ToString(),
                        REWARD_AMT = decimal.Parse(row[9].ToString()).ToString("#,0"),
                        FILE_NAME = row[10].ToString()
                    });
                }
            }

            return reportData;

        }
    }
}
