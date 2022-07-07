using DataAccess;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business
{
    public class AmMenuManager
    {
        public AmMenuDAO menuDAO { get; set; }

        public AmMenuManager()
        {
            menuDAO = new AmMenuDAO();
        }

        public AmMenu FindByPk(int id)
        {
            if (!this.menuDAO.Exist(id))
            {
                throw new Exception("您要找的資料在資料庫中不存在唷!");
            }

            return this.menuDAO.FindByPk(id);
        }

        public IList<AmMenu> FindByRoleStatusPid(string role, string status, int pid)
        {
            List<AmMenu> result = new List<AmMenu>();
            result = this.menuDAO.FindByRoleStatusPid(role, status, pid);
            return result;
        }

        public IList<AmMenu> FindByPid(int pid)
        {
            List<AmMenu> result = new List<AmMenu>();
            result = this.menuDAO.FindByPid(pid);
            return result;
        }

        public IList<AmMenu> FindByPidm(int pid)
        {
            List<AmMenu> result = new List<AmMenu>();
            result = this.menuDAO.FindByPidm(pid);
            return result;
        }

        public IList<AmMenu> GetMenu(string role, string status, int pid)
        {
            List<AmMenu> mainList = this.menuDAO.FindByRoleStatusPid(role, status, pid);
            foreach (AmMenu item in mainList)
            {
                List<AmMenu> subList = this.menuDAO.FindByRoleStatusPid(role, status, item.Id);
                item.SubMenuList = subList;
                foreach (AmMenu itemsub in subList)
                {
                    List<AmMenu> sub2List = this.menuDAO.FindByRoleStatusPid(role, status, itemsub.Id);
                    itemsub.SubMenuList = sub2List;
                }
            }
            return mainList;
        }

        public bool Exist(int id)
        {
            return this.menuDAO.Exist(id);
        }

        public void Delete(int id)
        {
            this.menuDAO.Delete(id);
        }

        public void DeleteByPid(int pid)
        {
            this.menuDAO.DeleteByPid(pid);
        }

        public void Update(AmMenu entity)
        {
            this.menuDAO.Update(entity);
        }

        public void Insert(AmMenu entity)
        {
            this.menuDAO.Insert(entity);
        }
    }
}
