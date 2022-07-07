using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DataAccess;
using Domain.Entities;

namespace Business
{
    public class IbonActivityManager
    {
        public IbonActivityDAO InstDAO { get; set; }

        public IbonActivityManager()
        {
            InstDAO = new IbonActivityDAO();
        }

        public List<IbonActivity> FindAll()
        {
            return this.InstDAO.FindAll();
        }
    }
}
