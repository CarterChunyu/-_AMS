using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class AmUsers : AbstractDO, ICloneable
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public string Status { get; set; }
        public DateTime RegDate { get; set; }
        public string Opid { get; set; }
        public DateTime UpdateDate { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
