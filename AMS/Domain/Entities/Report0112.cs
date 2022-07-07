using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Report0112
    {
        public string CptDate { get; set; }
        public string MerchantNo { get; set; }
        public string MerchantName { get; set; }
        public int TLOADA { get; set; }
        public int TPCHRA { get; set; }
        public int TPCHA { get; set; }
        public int TLOADRA { get; set; }
        public int TLOADA1 { get; set; }
        public int TPCHA1 { get; set; }
        public int TPCHA2 { get; set; }
        public List<Report0112> SubReport0112List { get; set; }
    }
}
