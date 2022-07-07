using DataAccess;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business
{
    public class AmEntAccountDManager
    {
        public AmEntAccountDDAO AmEntAccountDDAO { get; set; }
        public AmRolesDAO AmRolesDAO { get; set; }

        public AmEntAccountDManager()
        {
            AmEntAccountDDAO = new AmEntAccountDDAO();
            AmRolesDAO = new AmRolesDAO();
        }


        public AmEntAccountD FindByPk(string bankNo, string bankAcct)
        {
            return this.AmEntAccountDDAO.FindByPk(bankNo, bankAcct);
        }

        public List<AmEntAccountD> FindByStatus(string status)
        {
            return this.AmEntAccountDDAO.FindByStatus(status);
        }


        public bool Exist(string bankNo, string bankAcct)
        {
            return this.Exist(bankNo, bankAcct);
        }

        public void Delete(string bankNo, string bankAcct)
        {
            this.AmEntAccountDDAO.Delete(bankNo, bankAcct);
        }

        public void Insert(AmEntAccountD entity)
        {
            this.AmEntAccountDDAO.Insert(entity);
        }

        public void Update(AmEntAccountD entity)
        {
            this.AmEntAccountDDAO.Update(entity);
        }

        public List<AmEntAccountD> FindByRoleStatus(string roleId, string status)
        {
            return this.AmEntAccountDDAO.FindByRoleStatus(roleId, status);
        }

    }
}
