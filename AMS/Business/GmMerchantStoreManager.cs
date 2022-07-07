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
    public class GmMerchantStoreManager
    {
        public GmMerchantStoreDAO GmMerchantStoreDAO { get; set; }

        public GmMerchantStoreManager()
        {
            GmMerchantStoreDAO = new GmMerchantStoreDAO();
        }

        public void Insert(GmMerchantStore merchant)
        {
            this.GmMerchantStoreDAO.Insert(merchant);
        }

        public void Delete(GmMerchantStore merchant)
        {
            this.GmMerchantStoreDAO.Delete(merchant);
        }

        public DataTable FindAll()
        {
            return this.GmMerchantStoreDAO.FindData(null);
        }

        public DataTable FindData(GmMerchantStore merchant)
        {
            return this.GmMerchantStoreDAO.FindData(merchant);
        }

        public List<GmMerchantStore> FindList(GmMerchantStore merchant)
        {
            List<GmMerchantStore> list = new List<GmMerchantStore>();
            DataTable result = this.FindData(merchant);
            foreach (DataRow row in result.Rows)
            { 
                list.Add(new GmMerchantStore() 
                {
                    ID = int.Parse("" + row["ID"]), 
                    GROUP_NAME = "" + row["GROUP_NAME"],
                    MERCHANT_NO = "" + row["MERCHANT_NO"],
                    MERCHANT_NAME = "" + row["MERCHANT_NAME"],
                    UPDATE_USER = "" + row["UPDATE_USER"],
                    UPDATE_TIME = "" + row["UPDATE_TIME"],
                    IS_DEL = ("" + row["IS_DEL"] == "Y") ? true : false
                }); 
            }

            return list;
        }

        public DataTable FindStoreData(string schema, string merchant_no, string store_no)
        {
            return this.FindStoreData(schema, merchant_no, store_no, "");
        }

        public DataTable FindStoreData(string schema, string merchant_no, string store_no, string store_type)
        {
            switch (store_type)
            {
                case @"TRAFFIC_1":
                    return this.GmMerchantStoreDAO.FindTrafficStoreData(schema, merchant_no, store_no);
                default:
                    return this.GmMerchantStoreDAO.FindStoreData(schema, merchant_no, store_no);
            }
        }

        public DataTable FindImStoreData(string merchant_no, string store_no)
        { return this.GmMerchantStoreDAO.FindImStoreData(merchant_no, store_no); }

        public bool InsertStore(string schema, string store_type, object store)
        {
            bool result = false;
            switch (store_type)
            {
                case @"RETAIL_1":
                case @"TRACK_1":
                    result = this.GmMerchantStoreDAO.InsertStore_Retail_1(schema, (Domain.BmStoreM)store);
                    break;
                case @"TRAFFIC_1":
                    result = this.GmMerchantStoreDAO.InsertStore_Traffic_1(schema, (Domain.BmStoreM)store);
                    break;
            }

            return result;
        }

        public bool UpdateStore(string schema, string store_type, object store)
        {
            bool result = false;
            switch (store_type)
            {
                case @"RETAIL_1":
                case @"TRACK_1":
                    result = this.GmMerchantStoreDAO.UpdateStore_Retail_1(schema, (Domain.BmStoreM)store);
                    break;
                case @"TRAFFIC_1":
                    result = this.GmMerchantStoreDAO.UpdateStore_Traffic_1(schema, (Domain.BmStoreM)store);
                    break;
            }

            return result;
        }

        public void DeleteStore(string schema, string merchant_no, string store_no)
        { this.GmMerchantStoreDAO.DeleteStore(schema, merchant_no, store_no); }
    }
}
