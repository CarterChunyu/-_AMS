using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AMS.Models.OpenPointMerchant
{
    public class EditReq
    {
        /// <summary>
        /// 編輯列舉
        /// </summary>
        public enum en_EditType
        {
            /// <summary>
            /// 新增
            /// </summary>
            Add,
            /// <summary>
            /// 修改
            /// </summary>
            Modify
        }
        /// <summary>
        /// 編輯類型
        /// </summary>
        public en_EditType EditType { get; set; }
        /// <summary>
        /// 前頁-統一編號
        /// </summary>
        public string PreUnifiedBusinessNo { get; set; }
        /// <summary>
        /// 前頁-特店名稱
        /// </summary>
        public string PreMerchantName { get; set; }
        /// <summary>
        /// 編輯-統一編號
        /// </summary>
        public string UnifiedBusinessNo { get; set; }
        /// <summary>
        /// 編輯-特店名稱
        /// </summary>
        public string MerchantName { get; set; }
        public string Trans_FTPType { get; set; }
        public string Trans_FtpHost { get; set; }
        public string Trans_FtpPort { get; set; }
        public string Trans_FtpUser { get; set; }
        public string Trans_FtpPwd { get; set; }
        public string Trans_PoxyIP { get; set; }
        public string Trans_PoxyPort { get; set; }
        public string Trans_PoxyUser { get; set; }
        public string Trans_PoxyPwd { get; set; }
        public string Trans_FtpDir { get; set; }
        public string Store_FTPType { get; set; }
        public string Store_FtpHost { get; set; }
        public string Store_FtpPort { get; set; }
        public string Store_FtpUser { get; set; }
        public string Store_FtpPwd { get; set; }
        public string Store_PoxyIP { get; set; }
        public string Store_PoxyPort { get; set; }
        public string Store_PoxyUser { get; set; }
        public string Store_PoxyPwd { get; set; }
        public string Store_FtpDir { get; set; }
        public string Store_FileType { get; set; }
    }
}