namespace Domain.CoBrandedSellSponsorship
{
    public class QueryYearBankRes
    {
        /// <summary>
        /// 銀行代碼
        /// </summary>
        public string MERCHANT_NO { get; set; }
        /// <summary>
        /// 銀行簡稱
        /// </summary>
        public string MERCHANT_STNAME { get; set; }
        /// <summary>
        /// 可使用額度
        /// </summary>
        public int CanUseQuota { get; set; }
        /// <summary>
        /// 已使用額度
        /// </summary>
        public int UsedQuota { get; set; }
        /// <summary>
        /// 尚未請款數
        /// </summary>
        public int UnUsedQuota { get; set; }
    }
}
