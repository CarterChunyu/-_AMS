using DataAccess;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business
{
    public class AmEntReceDManager
    {
        public AmEntReceDDAO AmEntReceDDAO { get; set; }

        public AmEntReceDManager()
        {
            AmEntReceDDAO = new AmEntReceDDAO();
        }

        public AmEntReceD FindByPk(string sn, string bankAcctTo)
        {
            return this.AmEntReceDDAO.FindByPk(sn, bankAcctTo);
        }

        public List<AmEntReceD> FindBySearch(string receType, string acctDateS, string acctDateE, string caseNo, string checkStatus, string acctCheckStatus)
        {
            List<AmEntReceD> receListNew = new List<AmEntReceD>();
            List<AmEntReceD> receList = this.AmEntReceDDAO.FindBySearch(receType, acctDateS, acctDateE, caseNo, checkStatus, acctCheckStatus);
            AmChoiceManager acm = new AmChoiceManager();
            for (int i = 0; i < receList.Count; i++)
            {
                AmEntReceD rece = receList[i];
                rece.CheckStatusName = (acm.FindByCodeValue("Check_Status", rece.CheckStatus)).Name;
                rece.AcctCheckStatusName = (acm.FindByCodeValue("Check_Status", rece.AcctCheckStatus)).Name;
                receListNew.Add(rece);
            }
            return receListNew;
        }

        public void Update(AmEntReceD entity)
        {
            this.AmEntReceDDAO.Update(entity);
        }
    }
}
