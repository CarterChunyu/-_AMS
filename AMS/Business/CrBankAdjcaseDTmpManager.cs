using DataAccess;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business
{
    public class CrBankAdjcaseDTmpManager
    {
        public CrBankAdjcaseDTmpDAO CrBankAdjcaseDTmpDAO { get; set; }
        public AmUsersManager amUsersManager;
        public CmBankDManager cmBankDManager;

        public CrBankAdjcaseDTmpManager()
        {
            CrBankAdjcaseDTmpDAO = new CrBankAdjcaseDTmpDAO();
            amUsersManager = new AmUsersManager();
            cmBankDManager = new CmBankDManager();
        }

        public CrBankAdjcaseDTmp FindByPk(string caseNo, string iccNo)
        {
            return this.CrBankAdjcaseDTmpDAO.FindByPk(caseNo, iccNo);
        }

        public List<CrBankAdjcaseDTmp> FindByAdjCaseNo(string caseNo)
        {
            return this.CrBankAdjcaseDTmpDAO.FindByAdjCaseNo(caseNo);
        }

        public List<CrBankAdjcaseDTmp> FindByAdjCaseNoDisplay(string caseNo)
        {
            List<CrBankAdjcaseDTmp> result = new List<CrBankAdjcaseDTmp>();
            result = this.CrBankAdjcaseDTmpDAO.FindByAdjCaseNo(caseNo);
            for (int i = 0; i < result.Count; i++)
            {
                AmUsers cUser = amUsersManager.FindByPk(result[i].CreateUser);
                if (cUser != null)
                    result[i].CreateUser = cUser.Name;

                AmUsers uUser = amUsersManager.FindByPk(result[i].UpdateUser);
                if (uUser != null)
                    result[i].UpdateUser = uUser.Name;

                CmBankD bd = cmBankDManager.FindByMerchantNo(result[i].BankMerchant);
                if (bd != null)
                    result[i].BankMerchant = bd.BankName;

                result[i].UptDatetime = Helpers.Utility.SetTimeFormat(result[i].UptDatetime);
            }
            return result;
        }

        public bool Exist(string caseNo, string iccNo)
        {
            return this.CrBankAdjcaseDTmpDAO.Exist(caseNo, iccNo);
        }

        public void Insert(CrBankAdjcaseDTmp entity)
        {
            this.CrBankAdjcaseDTmpDAO.Insert(entity);
        }

        public void Update(CrBankAdjcaseDTmp entity)
        {
            this.CrBankAdjcaseDTmpDAO.Update(entity);
        }

        public void Delete(string caseNo, string iccNo)
        {
            this.CrBankAdjcaseDTmpDAO.Delete(caseNo, iccNo);
        }
    }
}
