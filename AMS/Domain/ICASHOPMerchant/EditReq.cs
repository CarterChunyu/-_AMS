
namespace Domain.ICASHOPMerchant
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
        /// 查詢頁，是否有觸發查詢。true=有、false=沒有
        /// </summary>
        public bool Qry_IsQuery { get; set; }
        /// <summary>
        /// 查詢頁，頁碼
        /// </summary>
        public int Qry_CurrentPage { get; set; }
        /// <summary>
        /// 查詢頁，統一編號輸入值
        /// </summary>
        public string Qry_UnifiedBusinessNo { get; set; }
        /// <summary>
        /// 查詢頁，特店名稱輸入值
        /// </summary>
        public string Qry_MerchantName { get; set; }
        /// <summary>
        /// 編輯類型
        /// </summary>
        public en_EditType EditType { get; set; }
        /// <summary>
        /// 統一編號
        /// </summary>
        public string UnifiedBusinessNo { get; set; }
        /// <summary>
        /// 特店名稱
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
