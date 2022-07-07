using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business
{
    public class HwCounterDManager
    {
        public HwCounterDDAO HwCounterDDAO { get; set; }

        public HwCounterDManager()
        {
            HwCounterDDAO = new HwCounterDDAO();
        }

        public int GetCounter(string y, string m, string ckind)
        {
            return this.HwCounterDDAO.GetCounter(y, m, ckind);
        }

        public void Insert(string y, string m, string ckind)
        {
            this.HwCounterDDAO.Insert(y, m, ckind);
        }

        public void Update(string y, string m, string ckind)
        {
            this.HwCounterDDAO.Update(y, m, ckind);
        }
    }
}
