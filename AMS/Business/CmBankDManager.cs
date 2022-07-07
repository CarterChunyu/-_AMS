using DataAccess;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business
{
    public class CmBankDManager
    {
        public CmBankDDAO CmBankDDAO { get; set; }

        public CmBankDManager()
        {
            CmBankDDAO = new CmBankDDAO();
        }

        public CmBankD FindByPk(string pk)
        {
            return this.CmBankDDAO.FindByPk(pk);
        }

        public CmBankD FindByMerchantNo(string merchantNo)
        {
            return this.CmBankDDAO.FindByMerchantNo(merchantNo);
        }
    }
}
