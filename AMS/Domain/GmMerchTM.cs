using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class GmMerchTM
    {
        public string MERCHANT_NO { get; set; }
        public string MERCH_TMID { get; set; }
        public string IBON_MERCHANT_NAME { get; set; }
        public string IBON_SHOW_TYPE { get; set; }
        public string NOTE { get; set; }
        public string CREATE_USER { get; set; }
        public string CREATE_TIME { get; set; }
        public string UPDATE_USER { get; set; }
        public string UPDATE_TIME { get; set; }
        public string IS_ACTIVE { get; set; }
    }
}
