using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Domain.Entities
{   
   
    public partial class GmMerInput
    {

        public string MERCHANT_NO { get; set; }
        public string MERC_GROUP { get; set; }
        public string MERCHANT_NAME { get; set; }
        public string MERCHANT_STNAME { get; set; }
        public string INVOICE_NO { get; set; }
        public string MERCH_TMID { get; set; }
        public string OL_TYPE { get; set; }
        public double PUR_FEE_RATE { get; set; }
        public double LOAD_FEE_RATE { get; set; }
        public double AUTO_LOAD_FEE_RATE { get; set; }
        public string REM_TYPE { get; set; }
        public string DAYLY_REM_DAY { get; set; }
        public string REM_FEE_TYPE { get; set; }
        public string DAYLY_REM_FEE_DAY { get; set; }
        public string GROUP_ID { get; set; }
        public string BUILD_USER { get; set; }
    }
}
