using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

using DataAccess;



namespace Business
{
    public class ReportBankManager
    {
        public ReportBankDAO RBDAO { get; set; }
        public ReportBankManager()
        {
            RBDAO = new ReportBankDAO();
        }

        public DataTable ReportBank161001(string sDate, string eDate, string bank, string cardId)
        {
            return RBDAO.ReportBank161001(sDate, eDate, bank, cardId);
        }

        public DataTable ReportBank161002(string sDate, string eDate, string bankMerchant)
        {
            return RBDAO.ReportBank161002(sDate, eDate, bankMerchant);
        }
    }
}
