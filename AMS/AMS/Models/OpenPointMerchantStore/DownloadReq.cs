using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AMS.Models.OpenPointMerchantStore
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
        public string Download_UnifiedBusinessNo { get; set; }
        /// <summary>
        /// 特店統編(查詢)
        /// </summary>
        public string Query_UnifiedBusinessNo { get; set; }
        /// <summary>
        /// 特店統編(查詢)
        /// </summary>
        public string Query_MerchantName { get; set; }
    }
}