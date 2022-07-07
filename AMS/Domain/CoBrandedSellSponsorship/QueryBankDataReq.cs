using System;

namespace Domain.CoBrandedSellSponsorship
{
    public class QueryBankDataReq
    {
        /// <summary>
        /// 年度區間流水號
        /// </summary>
        public int? RID { get; set; }
        /// <summary>
        /// 年度區間銀行表流水號
        /// </summary>
        public int? BID { get; set; }
        /// <summary>
        /// 年度區間開始日
        /// </summary>
        public DateTime? Date_B { get; set; }
        /// <summary>
        /// 年度區間結束日
        /// </summary>
        public DateTime? Date_E { get; set; }
        /// <summary>
        /// 排除銀行BID，Json字串
        /// </summary>
        public string ExcludeData { get; set; }
    }
}
