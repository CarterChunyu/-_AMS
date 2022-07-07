using DataAccess;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;


namespace Business
{
    public class GmTypeManager
    {
        public GmTypeDAO GmTypeDAO { get; set; }

        public GmTypeManager()
        {
            GmTypeDAO = new GmTypeDAO();
        }

        public void GM_TYPE_INDEX(string group, string merchant)
        {
            this.GmTypeDAO.GM_TYPE_INDEX(group, merchant);
        }

        public void GM_TYPE_EDIT(string group, string merchant)
        {
            this.GmTypeDAO.GM_TYPE_EDIT(group, merchant);
        }

        public void GM_TYPE_UPDATE(GmType entity, string username)
        {
            this.GmTypeDAO.GM_TYPE_UPDATE(entity, username);
        }

    }
}


