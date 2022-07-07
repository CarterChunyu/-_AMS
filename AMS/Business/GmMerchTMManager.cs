using DataAccess;
using Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business
{
    public class GmMerchTMManager
    {
        public GmMerchTMDAO GmMerchTMDAO { get; set; }

        public GmMerchTMManager()
        {
            GmMerchTMDAO = new GmMerchTMDAO();
        }

        public void Insert(GmMerchTM merchant)
        {
            try
            { this.GmMerchTMDAO.Insert(merchant); }
            catch (Exception ex)
            { throw ex; }
        }

        public void Update(GmMerchTM merchant)
        {
            try
            { this.GmMerchTMDAO.Update(merchant); }
            catch (Exception ex)
            { throw ex; }
        }

        public void Delete(GmMerchTM merchant)
        {
            try
            { this.GmMerchTMDAO.Delete(merchant); }
            catch (Exception ex)
            { throw ex; }
        }

        public DataTable FindData(GmMerchTM merchant)
        {
            return this.GmMerchTMDAO.FindData(merchant, false);
        }

        public DataTable FindDataByFuzzy(GmMerchTM merchant)
        {
            return this.GmMerchTMDAO.FindData(merchant, true);
        }
    }
}
