using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class AmEntAcctD : AbstractDO, ICloneable
    {
        public string AcctTitle { get; set; }
        public string BankNo { get; set; }
        public string BankAcct { get; set; }
        public string Name { get; set; }
        public string Reg_Id { get; set; }
        public string Tel { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public string Inform_Flg { get; set; }
        public string Merchant_No { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

    }
}
