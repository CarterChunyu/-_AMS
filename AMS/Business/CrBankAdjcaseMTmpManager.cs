using DataAccess;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business
{
    public class CrBankAdjcaseMTmpManager
    {
        public CrBankAdjcaseMTmpDAO CrBankAdjcaseMTmpDAO { get; set; }
        public AmUsersManager amUsersManager;
        public AmChoiceManager amChoiceManager;

        public CrBankAdjcaseMTmpManager()
        {
            CrBankAdjcaseMTmpDAO = new CrBankAdjcaseMTmpDAO();
            amUsersManager = new AmUsersManager();
            amChoiceManager = new AmChoiceManager();
        }

        public CrBankAdjcaseMTmp FindByPk(string _username)
        {
            return this.CrBankAdjcaseMTmpDAO.FindByPk(_username);
        }

        public List<CrBankAdjcaseMTmp> FindByAdjDate(string sdate, string edate, string status)
        {
            return this.CrBankAdjcaseMTmpDAO.FindByAdjDate(sdate, edate, status);
        }

        public List<CrBankAdjcaseMTmp> FindByAdjDateDisplay(string sdate, string edate, string status)
        {
            List<CrBankAdjcaseMTmp> result = new List<CrBankAdjcaseMTmp>();
            result = this.CrBankAdjcaseMTmpDAO.FindByAdjDate(sdate, edate, status);
            for (int i = 0; i < result.Count; i++)
            {
                AmUsers cUser = amUsersManager.FindByPk(result[i].CreateUser);
                if (cUser != null)
                    result[i].CreateUser = cUser.Name;

                AmUsers uUser = amUsersManager.FindByPk(result[i].UpdateUser);
                if (uUser != null)
                    result[i].UpdateUser = uUser.Name;

                AmChoice choice1 = amChoiceManager.FindByCodeValue("ADJ_FLAG", result[i].AdjFlag);
                result[i].AdjFlag = choice1.Name;

                AmChoice choice2 = amChoiceManager.FindByCodeValue("Check_Status", result[i].Status);
                result[i].Status = choice2.Name;
                if (result[i].AdjDate != "")
                {
                    result[i].AdjDate = Helpers.Utility.SetDateFormat(result[i].AdjDate);
                    result[i].RemittanceDate = Helpers.Utility.SetDateFormat(result[i].RemittanceDate);
                    result[i].CptDate = Helpers.Utility.SetDateFormat(result[i].CptDate);
                }
                result[i].UptDatetime = Helpers.Utility.SetTimeFormat(result[i].UptDatetime);
            }
            return result;
        }

        public bool Exist(string caseNo)
        {
            return this.Exist(caseNo);
        }

        public void Insert(CrBankAdjcaseMTmp entity)
        {
            this.CrBankAdjcaseMTmpDAO.Insert(entity);
        }

        public void Update(CrBankAdjcaseMTmp entity)
        {
            this.CrBankAdjcaseMTmpDAO.Update(entity);
        }
    }
}
