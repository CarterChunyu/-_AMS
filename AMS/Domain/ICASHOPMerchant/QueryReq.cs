
namespace Domain.ICASHOPMerchant
{
    public class QueryReq
    {
        /// <summary>
        /// 是否要觸發查詢，true=要、false=不要
        /// </summary>
        public bool IsQuery { get; set; } = false;
        /// <summary>
        /// 當前頁碼
        /// </summary>
        public int CurrentPage { get; set; } = 1;
        /// <summary>
        /// 統一編號
        /// </summary>
        public string UnifiedBusinessNo { get; set; } = string.Empty;
        /// <summary>
        /// 特店名稱
        /// </summary>
        public string MerchantName { get; set; } = string.Empty;
    }
}
