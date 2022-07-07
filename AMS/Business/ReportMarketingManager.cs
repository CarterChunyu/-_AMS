using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;

using DataAccess;

namespace Business
{
    public class ReportMarketingManager
    {
        public ReportMarketingDAO nowDAO { get; set; }
        public ReportMarketingHistoryDAO oldDAO { get; set; }
        public ReportMarketingManager()
        {
            nowDAO = new ReportMarketingDAO();
            oldDAO = new ReportMarketingHistoryDAO();
        }
        public DataTable ReportMarketing160101(string start, string end, string merchantNo, string kind1, string kind2, string kind3)
        {
            /*
            if(start==null) throw new Exception();

            DateTime now = DateTime.Now;
            string firstDayNowDB = new DateTime(now.Year, now.Month, 1).AddMonths(-6).ToString("yyyyMMdd");
            string lastDayInHistory = new DateTime(now.Year, now.Month, 1).AddMonths(-6).AddDays(-1).ToString("yyyyMMdd");

            if (start.CompareTo(lastDayInHistory) <= 0)
            {
                DataTable oldDT = oldDAO.ReportMarketingHistory160101(start, lastDayInHistory, firstDayNowDB, end, merchantNo, kind1, kind2, kind3);

                return oldDT;
            }
            else
            {
                DataTable nowDT = nowDAO.ReportMarketing160101(start, end, merchantNo,kind1,kind2,kind3);
                return nowDT;
            }
            */
            DataTable oldDT = oldDAO.ReportMarketingHistory160101(start, end, merchantNo, kind1, kind2, kind3);
            return oldDT;
        }

        public DataTable ReportMarketing160102(string start, string end, string merchantNo)
        {
            DataTable dt = nowDAO.ReportMarketing160102(start, end, merchantNo);
            return dt;
        }
    }
}
