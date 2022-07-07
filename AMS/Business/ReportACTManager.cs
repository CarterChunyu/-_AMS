using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

using DataAccess;

namespace Business
{
    public class ReportACTManager
    {
        public ReportACTDAO ReportACTDAO { get; set; }
        public ReportACTManager()
        {
            ReportACTDAO = new ReportACTDAO();
        }

        public DataTable ReportACT160901(string yearMonth, decimal newRate)
        {
            return ReportACTDAO.ReportACT160901(yearMonth, newRate);
        }

        public DataTable ReportACT161101(string firstDate, string lastDate, string merchant)
        {
            return ReportACTDAO.ReportACT161101(firstDate, lastDate, merchant);
        }

        public DataTable ReportACT161201(string settleDate, string merchant)
        {
            return ReportACTDAO.ReportACT161201(settleDate, merchant);
        }

        public DataTable ReportACT161202(string sDate, string eDate, string merchant)
        {
            return ReportACTDAO.ReportACT161202(sDate, eDate, merchant);
        }

        public DataTable ReportACT170101(string startDate, string endDate)
        {
            return ReportACTDAO.ReportACT170101(startDate, endDate);
        }

        public DataTable ReportACT170102(string startDate, string endDate)
        {
            return ReportACTDAO.ReportACT170102(startDate, endDate);
        }

        public DataTable ReportACT170103(string startDate, string endDate)
        {
            return ReportACTDAO.ReportACT170103(startDate, endDate);
        }

        public DataTable ReportACT170104(string startDate, string endDate)
        {
            return ReportACTDAO.ReportACT170104(startDate, endDate);
        }

        public DataTable ReportACT170105(string startDate, string endDate)
        {
            return ReportACTDAO.ReportACT170105(startDate, endDate);
        }
        public DataTable ReportACT170201(string startDate)
        {
            return ReportACTDAO.ReportACT170201(startDate);
        }
        public DataTable ReportACT170202(string settleDate)
        {
            return ReportACTDAO.ReportACT170202(settleDate);
        }

        public DataTable ReportACT170401(string yearMonth, string startDate, string endDate)
        {
            return ReportACTDAO.ReportACT170401(yearMonth, startDate, endDate);
        }

        public DataTable ReportACT170501(string yearMonth)
        {
            return ReportACTDAO.ReportACT170501(yearMonth);
        }
        public DataTable ReportACT170801(string settleDate, string merchant)
        {
            return ReportACTDAO.ReportACT170801(settleDate, merchant);
        }
        public DataTable ReportACT170802(string startDate, string endDate, string merchant)
        {
            return ReportACTDAO.ReportACT170802(startDate, endDate, merchant);
        }
        public DataTable ReportACT180301(string iccNo, string sDate, string merchant)
        {
            return ReportACTDAO.ReportACT180301(iccNo, sDate, merchant);
        }
        public DataTable ReportACT180401(string yearMont, string merchant)
        {
            return ReportACTDAO.ReportACT180401(yearMont, merchant);
        }
        public DataTable ReportACT180402(string yearMont, string merchant)
        {
            return ReportACTDAO.ReportACT180402(yearMont, merchant);
        }
        public DataTable ReportACT180403(string start, string end)
        {
            return ReportACTDAO.ReportACT180403(start, end);
        }
        public DataTable ReportACT180701(string startDate, string endDate, string merchant)
        {
            return ReportACTDAO.ReportACT180701(startDate, endDate, merchant);
        }
        public DataTable ReportACT180702(string startDate, string endDate)
        {
            return ReportACTDAO.ReportACT180702(startDate, endDate);
        }
        public DataTable ReportACT181101(string startDate, string endDate, string merchant,string ab_merchant)
        {
            return ReportACTDAO.ReportACT181101(startDate, endDate, merchant,ab_merchant);
        }
        public DataTable ReportACT181201(string startDate, string endDate,string ab_merchant)
        {
            return ReportACTDAO.ReportACT181201(startDate, endDate,ab_merchant);
        }
        public DataTable ReportACT181202(string firstDate, string merchant)
        {
            return ReportACTDAO.ReportACT181202(firstDate, merchant);
        }
        public DataTable ReportACT181202D(string firstDate, string merchant)
        {
            return ReportACTDAO.ReportACT181202D(firstDate, merchant);
        }
        public DataTable ReportACT190101(string firstDate, string merchant)
        {
            return ReportACTDAO.ReportACT190101(firstDate, merchant);
        }
        public DataTable ReportACT190301(string firstDate, string merchant)
        {
            return ReportACTDAO.ReportACT190301(firstDate, merchant);
        }
        public DataTable ReportACT190701(string sDate, string eDate)
        {
            return ReportACTDAO.ReportACT190701(sDate, eDate);
        }
        public DataSet ReportACT190901(string sDate, string eDate)
        {
            return ReportACTDAO.ReportACT190901(sDate, eDate);
        }
        public DataSet ReportACT191001(string ym)
        {
            return ReportACTDAO.ReportACT191001(ym);
        }
        public DataTable ReportACT190701_02(string sDate, string eDate)
        {
            return ReportACTDAO.ReportACT190701_02(sDate, eDate);
        }
        public DataTable ReportACT181101_2(string startDate, string endDate, string merchant, string ab_merchant)
        {
            return ReportACTDAO.ReportACT181101_2(startDate, endDate, merchant, ab_merchant);
        }
    }
}
