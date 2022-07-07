using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

using DataAccess;

namespace Business
{
    public class GmAccountManager
    {
        public GmAccountDAO GmAccountDAO { get; set; }
        public GmAccountManager()
        {
            GmAccountDAO = new GmAccountDAO();
        }
        public DataTable GmAccount190501(string cpt_date, string no1,string no2)
        {
            return GmAccountDAO.GmAccount190501(cpt_date, no1,no2);
        }
        public DataTable GmAccount200201(string cpt_date, string no1, string no2)
        {
            return GmAccountDAO.GmAccount200201(cpt_date, no1, no2);
        }
        public DataTable GmAccount200601_01(string cpt_date, string yearMonth, string date_range, string merchant, string groups, string invoice_rule)
        {
            return GmAccountDAO.GmAccount200601_01(cpt_date, yearMonth, date_range, merchant, groups, invoice_rule);
        }
        public DataTable GmAccount200601_05(string cpt_date, string yearMonth, string date_range, string schema, string ab_mno, string groups, string invoice_rule)
        {
            return GmAccountDAO.GmAccount200601_05(cpt_date, yearMonth, date_range, schema, ab_mno, groups, invoice_rule);
        }
        public DataTable GmAccount200601_03(string cpt_date, string yearMonth, string date_range, string merchant, string groups, string invoice_rule)
        {
            return GmAccountDAO.GmAccount200601_03(cpt_date, yearMonth, date_range, merchant, groups, invoice_rule);
        }
    }
}

