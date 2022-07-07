using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class AmEntAccountD : AbstractDO, ICloneable
    {
        public string AcctTitle { get; set; }
        public string BankNo { get; set; }
        public string BankAcct { get; set; }
        public string AcctCode { get; set; }
        public string CompanyId { get; set; }
        public string Department { get; set; }
        public string Name { get; set; }
        public string Remark { get; set; }
        public string Status { get; set; }
        public string Operator { get; set; }
        public string UpdateDate { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

    }
}
