using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

using Domain;
using DataAccess;

namespace Business
{
    public class GmAccountStoreManager
    {
        public GmMerchantActStoreDAO ActStoreDAO { get; set; }
        public GmAccountStoreManager()
        {
            ActStoreDAO = new GmMerchantActStoreDAO();
        }
        public List<GmMerchant> FindAllGroup_Store()
        {
            return ActStoreDAO.FindAllGroup_Store();
        }
        public List<BmStoreno> FindAllBMStore(string merchant)
        {
            return ActStoreDAO.FindAllBMStore(merchant);
        }
        public DataTable GMACT_STORE_INDEX(string merchant, string storeno)
        {
            return this.ActStoreDAO.GMACT_STORE_INDEX(merchant, storeno);
        }
        public DataTable GMACT_STORE_EDIT(string merchant, string storeno)
        {
            return this.ActStoreDAO.GMACT_STORE_EDIT(merchant, storeno);
        }
        public bool CheckMerchantActStore(string merchant, string storeno)
        {
            return ActStoreDAO.CheckMerchantActStore(merchant, storeno);
        }
        public void GMACT_STORE_INSERT(GmAccountActStore entity)
        {
            this.ActStoreDAO.GMACT_STORE_INSERT(entity);
        }
        public void GMACT_STORE_UPDATE(GmAccountActStore entity)
        {
            this.ActStoreDAO.GMACT_STORE_UPDATE(entity);
        }
    }
}
