using DataAccess;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business
{
    public class AmRolesManager
    {
        public AmRolesDAO AmRolesDAO { get; set; }

        public AmRolesManager()
        {
            AmRolesDAO = new AmRolesDAO();
        }

        public void Insert(AmRoles entity)
        {
            this.AmRolesDAO.Insert(entity);
        }

        public void Delete(string roleIs)
        {
            this.AmRolesDAO.Delete(roleIs);
        }

        public List<AmRoles> FindAll()
        {
            List<AmRoles> result = new List<AmRoles>();
            result = this.AmRolesDAO.FindAll();
            return result;
        }

        public AmRoles FindByPk(string roleId)
        {
            return this.AmRolesDAO.FindByPk(roleId);
        }
    }
}
