using DataAccess;
using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business
{
    public class GmMerchantManager
    {
        public GmMerchantDAO GmMerchantDAO { get; set; }

        public GmMerchantManager()
        {
            GmMerchantDAO = new GmMerchantDAO();
        }

        public List<GmMerchant> FindAll()
        {
            return this.GmMerchantDAO.FindAll();
        }

        public List<GmMerchant> FindAllGroup()
        {
            return this.GmMerchantDAO.FindAllGroup();
        }

        public List<GmMerchant> FindAllRetail()
        {
            return this.GmMerchantDAO.FindAllRetail();
        }

        public List<GmMerchant> FindAllBus()
        {
            return this.GmMerchantDAO.FindAllBus();
        }

        public List<GmMerchant> FindAllBike()
        {
            return this.GmMerchantDAO.FindAllBike();
        }

        public List<GmMerchant> FindAllTrack()
        {
            return this.GmMerchantDAO.FindAllTrack();
        }

        public List<GmMerchant> FindAllParking()
        {
            return this.GmMerchantDAO.FindAllParking();
        }

        public List<GmMerchant> FindAllOutsourcing()
        {
            return this.GmMerchantDAO.FindAllOutsourcing();
        }
        public List<GmMerchant> FindMerchant(string group_id)
        {
            return this.GmMerchantDAO.FindMerchant(group_id);
        }

        public System.Data.DataRow FindMerchantData(string merchant_no)
        {
            return this.GmMerchantDAO.FindMerchantData(merchant_no);
        }





        public List<GmMerchant> FindAllBnak()
        {
            return this.GmMerchantDAO.FindAllBank();
        }

        public List<GmMerchant> FindAllRAMT()
        {
            return this.GmMerchantDAO.FindAllRAMT();
        }

        public List<GmMerchant> FindAllStore()
        {
            return this.GmMerchantDAO.FindAllStore();
        }

        public List<GmMerchant> FindAllMerchantNoCom()
        {
            return this.GmMerchantDAO.FindAllMerchantNoCom();
        }
        public List<GmMerchant> FindRetailNotXDD()
        {
            return this.GmMerchantDAO.FindRetailNotXDD();
        }
        public List<GmMerchant> FindOnlyXDD()
        {
            return this.GmMerchantDAO.FindOnlyXDD();
        }
        public List<GmMerchant> FindGroupNoMstore()
        {
            return this.GmMerchantDAO.FindGroupNoMstore();
        }
        public List<GmMerchant> FindAllbyGroup(string group)
        {
            return this.GmMerchantDAO.FindAllbyGroup(group);
        }
        public List<GmMerchant> FindNCCC()
        {
            return this.GmMerchantDAO.FindNCCC();
        }
        public List<GmMerchant> FindKSH()
        {
            return this.GmMerchantDAO.FindKSH();
        }
        public List<GmMerchant> FindParking()
        {
            return this.GmMerchantDAO.FindParking();
        }
        public List<GmMerchant> FindGroupRetailParking()
        {
            return this.GmMerchantDAO.FindGroupRetailParking();
        }
        public List<GmMerchant> FindMutiGroup()
        {
            return this.GmMerchantDAO.FindMutiGroup();
        }
        public List<GmMerchant> FindMutiMerchant(string group)
        {
            return this.GmMerchantDAO.FindMutiMerchant(group);
        }
    }
}
