using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.CoBrandedSellSponsorship
{
    public class QueryYearBankReq
    {
        /// <summary>
        /// 年度起始日
        /// </summary>
        public string Date_B { get; set; }
        /// <summary>
        /// 年度結束日
        /// </summary>
        public string Date_E { get; set; }
    }
}
