using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class IbonActivity : AbstractDO, ICloneable
    {
        public string Name { get; set; }

        public string Code { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
