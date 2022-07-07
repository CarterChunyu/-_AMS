using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;

using DataAccess;

namespace Business
{
    public class ReportSystemManager
    {
        public ReportSystemDAO SysDAO { get; set; }

        public ReportSystemManager()
        {
            SysDAO = new ReportSystemDAO();
        }

        public DataTable ReportSystem160501(string start, string end,string group,string schemaId,string merchantNo)
        {
            DataTable dt = SysDAO.ReportSystem160501(start, end, group,schemaId,merchantNo);

            return dt;
        }
    }
}
