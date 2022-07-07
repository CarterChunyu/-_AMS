using DataAccess;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business
{
    public class AmEntReceLogDManager
    {
        public AmEntReceLogDDAO AmEntReceLogDDAO { get; set; }

        public AmEntReceLogDManager()
        {
            AmEntReceLogDDAO = new AmEntReceLogDDAO();
        }

        public AmEntReceLogD FindByPk(string sn, string bankAcctTo)
        {
            return this.AmEntReceLogDDAO.FindByPk(sn, bankAcctTo);
        }

        public List<AmEntReceLogD> FindBySearch(string receType, string acctDateS, string acctDateE, string caseNo, string checkStatus, string acctCheckStatus)
        {
            return this.AmEntReceLogDDAO.FindBySearch(receType, acctDateS, acctDateE, caseNo, checkStatus, acctCheckStatus);
        }

        public void Insert(AmEntReceLogD entity)
        {
            this.AmEntReceLogDDAO.Insert(entity);
        }
    }
}
