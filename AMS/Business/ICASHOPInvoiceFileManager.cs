using DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business
{
    public class ICASHOPInvoiceFileManager
    {
        private ICASHOPInvoiceFileDAO icashopInvoiceFileDAO { get; set; }
        public ICASHOPInvoiceFileManager()
        {
            icashopInvoiceFileDAO = new ICASHOPInvoiceFileDAO();
        }

        public DataTable GetInvoiceData(string TARGET_MONTH, string TARGET_DateType, string Group_TaiYa, string Group_TaiSu, string Group_XiOu, string Group_FuMou, string Group_TongYiDuJiaCun)
        {
            return icashopInvoiceFileDAO.GetInvoiceData(TARGET_MONTH, TARGET_DateType, Group_TaiYa, Group_TaiSu, Group_XiOu, Group_FuMou, Group_TongYiDuJiaCun);
        }
    }
}
