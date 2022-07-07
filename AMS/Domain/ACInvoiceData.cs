using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class ACInvoiceData
    {
        public int WEEK { get; set; }
        public DateTime CPT_DATE { get; set; }
        public DateTime REWARD_START_DATE{ get; set; }
        public DateTime REWARD_END_DATE { get; set; }
        public decimal PRE_ISSUANCE_AMT { get; set; }
        public decimal PRE_BALANCE { get; set; }
        public decimal REWARD_AMT { get; set; }
        public decimal TOTAL_REWARD_AMT { get; set; }
        public decimal BALANCE { get; set; }
        public string OVER_LIMIT{ get; set; }
        public int BINDING_COUNT { get; set; }
        public decimal ESTIMATE_INVOICE { get; set; }
        public decimal INVOICE_AMT { get; set; }
        public string CAN_APPLY { get; set; }
        public int NEXT_STAGE { get; set; }
        public string IS_APPLY { get; set; }
        public int STAGE_ALREADY_REWARD { get; set; }
    }
}
