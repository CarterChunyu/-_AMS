using DataAccess;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business
{
    public class AmUsersManager
    {
        public AmUsersDAO AmUsersDAO { get; set; }

        public AmUsersManager()
        {
            AmUsersDAO = new AmUsersDAO();
        }

        public AmUsers FindByAuth(string _username)
        {
            return this.AmUsersDAO.FindByAuth(_username);
        }

        public AmUsers FindByAuth(string _username, string _password)
        {
            return this.AmUsersDAO.FindByAuth(_username, _password);
        }

        public AmUsers FindByPk(string _username)
        {
            return this.AmUsersDAO.FindByPk(_username);
        }

        public List<AmUsers> FindByStatus(string status)
        {
            return this.AmUsersDAO.FindByStatus(status);
        }

        public List<String> GetUserRights(string _username)
        {
            return this.AmUsersDAO.GetUserRights(_username);
        }

        public bool Exist(string _username)
        {
            return this.Exist(_username);
        }

        public void Delete(string _username)
        {
            this.AmUsersDAO.Delete(_username);
        }

        public void Insert(AmUsers entity)
        {
            this.AmUsersDAO.Insert(entity);
        }

        public void Update(AmUsers entity)
        {
            this.AmUsersDAO.Update(entity);
        }

        public int UpdatePassword(string username, string password, string opid)
        {
            return this.AmUsersDAO.UpdatePassword(username, password, opid);
        }

        public void UpdateRegDate(string _username,DateTime? datetime)
        {
            this.AmUsersDAO.UpdateRegDate(_username, datetime);
        }
        public int UpdatePasswordLog(string username, string password, string opid)
        {
            return this.AmUsersDAO.UpdatePasswordLog(username, password, opid);
        }
        public AmUsers FindHistory(string _username, string _password)
        {
            return this.AmUsersDAO.FindHistory(_username, _password);
        }
    }
}
