using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class ACInvoiceData2021
    {
        public int STAGE { get; set; }
        public DateTime CPT_DATE { get; set; }
        public DateTime REWARD_START_DATE { get; set; }
        public DateTime REWARD_END_DATE { get; set; }
        public decimal PRE_ISSUANCE_AMT { get; set; }
        public decimal REWARD_AMT { get; set; }
        public decimal TOTAL_REWARD_AMT { get; set; }
        public decimal BALANCE { get; set; }
        public string LIMIT { get; set; }
        public string OVER_LIMIT { get; set; }
        public int BINDING_COUNT_MEMBER { get; set; }
        public int TOTAL_BINDING_COUNT { get; set; }
        public decimal TOTAL_BINDING_AMT { get; set; }
        public decimal ESTIMATE_INVOICE { get; set; }
        public string BAL_SHORT { get; set; }
        public int APPLY_STAGE { get; set; }
        public decimal NOT_ACQUIRE_AMT { get; set; }
        public decimal ACQUIRED_AMT { get; set; }
        public string IS_APPLY { get; set; }
        public string NOTE { get; set; }
    }
}
