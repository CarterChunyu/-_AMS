using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class AmMenu : AbstractDO, ICloneable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public int ParentId { get; set; }
        public string Roles { get; set; }
        public string Status { get; set; }
        public int Rank { get; set; }
        public List<AmMenu> SubMenuList { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
