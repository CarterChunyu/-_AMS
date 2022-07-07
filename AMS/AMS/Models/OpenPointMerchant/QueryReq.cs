using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AMS.Models.OpenPointMerchant
{
    public class QueryReq
    {
        /// <summary>
        /// 統一編號
        /// </summary>
        public string UnifiedBusinessNo { get; set; }
        /// <summary>
        /// 特店名稱
        /// </summary>
        public string MerchantName { get; set; }
    }
}