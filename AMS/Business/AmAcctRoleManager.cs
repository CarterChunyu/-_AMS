using DataAccess;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business
{
    public class AmAcctRoleManager
    {
        public AmAccountRoleDAO AmAccountRoleDAO { get; set; }
        public AmRolesDAO AmRolesDAO { get; set; }

        public AmAcctRoleManager()
        {
            AmAccountRoleDAO = new AmAccountRoleDAO();
            AmRolesDAO = new AmRolesDAO();
        }

        public void Insert(string bankNo, string bankAcct, string roleId)
        {
            this.AmAccountRoleDAO.Insert(bankNo, bankAcct, roleId);
        }

        public void Delete(string roleId, string bankNo, string bankAcct)
        {
            this.AmAccountRoleDAO.Delete(roleId, bankNo, bankAcct);
        }

        public List<AmRoles> FindRolesByBankInfo(string bankNo, string bankAcct)
        {
            return this.AmAccountRoleDAO.FindRolesByBankInfo(bankNo, bankAcct);
        }

        public bool IsRoleBankAcct(string bankAcct, string roleId)
        {
            return this.AmAccountRoleDAO.IsRoleBankAcct(bankAcct, roleId);
        }

        public List<AmRoles> FindNoRolesByBankInfo(string bankNo, string bankAcct)
        {
            List<AmRoles> roleAll = this.AmRolesDAO.FindAll();
            List<AmRoles> nRolesList = new List<AmRoles>();
            for (int i = 0; i < roleAll.Count; i++)
            {
                AmRoles ro = roleAll[i];
                if (!this.AmAccountRoleDAO.Exist(ro.RoleId, bankNo, bankAcct))
                    nRolesList.Add(ro);
            }
            return nRolesList;
        }
    }
}
