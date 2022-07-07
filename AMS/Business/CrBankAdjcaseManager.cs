using DataAccess;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business
{
    public class CrBankAdjcaseManager
    {
        public CrBankAdjcaseMDAO CrBankAdjcaseMDAO { get; set; }
        public CrBankAdjcaseDDAO CrBankAdjcaseDDAO { get; set; }

        public CrBankAdjcaseManager()
        {
            CrBankAdjcaseMDAO = new CrBankAdjcaseMDAO();
            CrBankAdjcaseDDAO = new CrBankAdjcaseDDAO();
        }

        public CrBankAdjcaseM FindByPkM(string caseNo)
        {
            return this.CrBankAdjcaseMDAO.FindByPk(caseNo);
        }

        public CrBankAdjcaseD FindByPkD(string caseNo)
        {
            return this.CrBankAdjcaseDDAO.FindByPk(caseNo);
        }

        public List<CrBankAdjcaseD> FindDByAdjCaseNo(string caseNo)
        {
            return this.CrBankAdjcaseDDAO.FindByAdjCaseNo(caseNo);
        }

        public bool Exist(string caseNo)
        {
            return this.Exist(caseNo);
        }

        public void InsertM(CrBankAdjcaseM entity)
        {
            this.CrBankAdjcaseMDAO.Insert(entity);
        }

        public void InsertD(CrBankAdjcaseD entity)
        {
            this.CrBankAdjcaseDDAO.Insert(entity);
        }
    }
}
