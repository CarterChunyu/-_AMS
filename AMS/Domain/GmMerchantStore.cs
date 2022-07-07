using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class GmMerchantStore
    {
        public int? ID { get; set; }
        public string GROUP_ID { get; set; }
        public string GROUP_NAME { get; set; }
        public string MERCHANT_NO { get; set; }
        public string MERCHANT_NAME { get; set; }
        public string CREATE_USER { get; set; }
        public string CREATE_TIME { get; set; }
        public string UPDATE_USER { get; set; }
        public string UPDATE_TIME { get; set; }
        public bool? IS_DEL { get; set; }
    }
}
