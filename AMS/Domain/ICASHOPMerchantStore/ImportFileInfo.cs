using System.Web;

namespace Domain.ICASHOPMerchantStore
{
    public class ImportFileInfo
    {
        /// <summary>
        /// 上傳檔案
        /// </summary>
        public HttpPostedFileBase file { get; set; }
        /// <summary>
        /// 上傳檔案名稱(有副檔名)
        /// </summary>
        public string UploadFileName { get; set; }
        /// <summary>
        /// 上傳檔案名稱(沒有副檔名)
        /// </summary>
        public string UploadFileNameWithoutExtension { get; set; }
        /// <summary>
        /// 統編
        /// </summary>
        public string UnifiedBusinessNo { get; set; }
        /// <summary>
        /// 資料筆數(文字型態)
        /// </summary>
        public string s_DataCnt { get; set; }
        /// <summary>
        /// 資料筆數(數字型態)
        /// </summary>
        public int i_DataCnt { get; set; }
        /// <summary>
        /// 檔案於Server端的完整路徑
        /// </summary>
        public string ServerFileFullName { get; set; }
        /// <summary>
        /// 錯誤訊息
        /// </summary>
        public string ErrMsg { get; set; } = string.Empty;
    }
}
