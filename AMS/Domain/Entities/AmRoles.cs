using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class AmRoles
    {
        public string RoleId { get; set; }
        public string RoleName { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
