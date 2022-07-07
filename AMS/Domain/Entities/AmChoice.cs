using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class AmChoice : AbstractDO, ICloneable
    {
        public string Code { get; set; }
        public string Value { get; set; }
        public string Name { get; set; }
        public string Remark { get; set; }
        public string Status { get; set; }
        public int Rank { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
