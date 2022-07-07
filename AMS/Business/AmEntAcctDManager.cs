using DataAccess;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business
{
    public class AmEntAcctDManager
    {
        public AmEntAcctDDAO AmEntAcctDDAO { get; set; }
        public AmRolesDAO AmRolesDAO { get; set; }

        public AmEntAcctDManager()
        {
            AmEntAcctDDAO = new AmEntAcctDDAO();
            AmRolesDAO = new AmRolesDAO();
        }


        public AmEntAcctD FindByPk(string bankNo, string bankAcct)
        {
            return this.AmEntAcctDDAO.FindByPk(bankNo, bankAcct);
        }

        public List<AmEntAcctD> FindBySearch(string AcctTitle, string Status)
        {
            List<AmEntAcctD> receList = this.AmEntAcctDDAO.FindBySearch(AcctTitle, Status);
            return receList;
        }

        public List<AmEntAcctD> FindByStatus(string status)
        {
            return this.AmEntAcctDDAO.FindByStatus(status);
        }


        public bool Exist(string bankNo, string bankAcct)
        {
            return this.Exist(bankNo, bankAcct);
        }

        public void Delete(string bankNo, string bankAcct)
        {
            this.AmEntAcctDDAO.Delete(bankNo, bankAcct);
        }

        public void Insert(AmEntAcctD entity)
        {
            this.AmEntAcctDDAO.Insert(entity);
        }

        public void Update(AmEntAcctD entity)
        {
            this.AmEntAcctDDAO.Update(entity);
        }

        public List<AmEntAcctD> FindByRoleStatus(string roleId, string status)
        {
            return this.AmEntAcctDDAO.FindByRoleStatus(roleId, status);
        }

    }
}
