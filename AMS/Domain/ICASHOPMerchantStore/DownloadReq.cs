
namespace Domain.ICASHOPMerchantStore
{
    public class DownloadReq
    {
        /// <summary>
        /// 下載列舉
        /// </summary>
        public enum en_DownloadType
        {
            /// <summary>
            /// 範本
            /// </summary>
            Template,
            /// <summary>
            /// 特店門市資料
            /// </summary>
            Store
        }
        /// <summary>
        /// 下載類型
        /// </summary>
        public en_DownloadType DownloadType { get; set; }
        /// <summary>
        /// 特店統編
        /// </summary>
        public string UnifiedBusinessNo { get; set; }
    }
}
