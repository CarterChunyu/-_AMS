using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class GmType
    {
        public string MERCHANT_NO { get; set; }
        //特約機構編號
        public string GROUP_ID { get; set; }
        //群組代號
        public string SHOW_ORDER { get; set; }
        //顯示順序
        public string SHOW_FLG { get; set; }
        //是否顯示，Y=顯示，N=不顯示
    }
}
