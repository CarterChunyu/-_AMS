using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Domain.Entities
{
    public class GM_BUILD_MERCHANT_LOG
    {
        [DisplayName("機構編號")]
        public string MERCHANT_NO { get; set; }

      
        [DisplayName("狀態")]
        public string LOG_KIND { get; set; }


        [DisplayName("狀態訊息")]        
        public string LOG_MSG { get; set; }


        [DisplayName("操作人員")]        
        public string BUILD_USER { get; set; }
        
        
        [DisplayName("新增日期")]
        public string BUILD_DATETIME { get; set; }


        [DisplayName("註銷")]
        public string IS_DEL { get; set; }


        [DisplayName("註銷日期")]
        public string DEL_DATE { get; set; }
    }
}
