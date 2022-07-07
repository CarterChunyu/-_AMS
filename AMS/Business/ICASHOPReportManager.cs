using DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business
{
    public class ICASHOPReportManager
    {
        public ICASHOPReportDAO ReportDAO { get; set; }
        public ICASHOPReportManager()
        {
            ReportDAO = new ICASHOPReportDAO();
        }
        public DataTable Report_01(string yearMonth, string Group_TaiYa, string Group_TaiSu, string Group_XiOu, string Group_FuMou, string Group_TongYiDuJiaCun)
        {
            return ReportDAO.Report_01(yearMonth, Group_TaiYa, Group_TaiSu, Group_XiOu, Group_FuMou, Group_TongYiDuJiaCun);
        }

        public DataTable Report_02(string yearMonth,  string Group_TaiYa, string Group_TaiSu, string Group_XiOu, string Group_FuMou, string Group_TongYiDuJiaCun)
        {
            return ReportDAO.Report_02(yearMonth, Group_TaiYa, Group_TaiSu, Group_XiOu, Group_FuMou, Group_TongYiDuJiaCun);
        }

        public DataTable Report_03(string yearMonth, string Group_TaiYa, string Group_TaiSu, string Group_XiOu, string Group_FuMou, string Group_TongYiDuJiaCun)
        {
            return ReportDAO.Report_03(yearMonth, Group_TaiYa, Group_TaiSu, Group_XiOu, Group_FuMou, Group_TongYiDuJiaCun);
        }

        public DataTable Report_04(string yearMonth, string Group_TaiYa, string Group_TaiSu, string Group_XiOu, string Group_FuMou, string Group_TongYiDuJiaCun)
        {
            return ReportDAO.Report_04(yearMonth, Group_TaiYa, Group_TaiSu, Group_XiOu, Group_FuMou, Group_TongYiDuJiaCun);
        }
    }
}
