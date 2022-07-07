using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AMS.Models.OpenPointMerchant
{
    public class EditSaveReq
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
        /// 編輯Json資料
        /// </summary>
        public string EditJsonData { get; set; }
        public string User { get; set; }
        public string IP { get; set; }
    }
}