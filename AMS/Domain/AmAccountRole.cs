using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class AmAccountRole
    {
        public string RoleId { get; set; }
        public string BankNo { get; set; }
        public string BankAcct { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
