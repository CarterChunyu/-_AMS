using DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business
{
    public class ICASHOPManager
    {
        private ICASHOPDAO icashopDAO { get; set; }

        public ICASHOPManager()
        {
            icashopDAO = new ICASHOPDAO();
        }

        public DataTable GetGroupNameData()
        {
            return icashopDAO.GetGroupNameData();
        }
    }
}
