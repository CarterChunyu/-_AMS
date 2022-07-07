//------------------------------------------------------------------------------
// <auto-generated>
//     這個程式碼是由範本產生。
//
//     對這個檔案進行手動變更可能導致您的應用程式產生未預期的行為。
//     如果重新產生程式碼，將會覆寫對這個檔案的手動變更。
// </auto-generated>
//------------------------------------------------------------------------------

namespace AMS.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class GM_ADJ_APPLICATION_ITEM
    {
        public int APP_ITEM_ID { get; set; }
        public string ARAP { get; set; }
        public string TRANS_TYPE { get; set; }
        public int FORM_ID { get; set; }

        [Display(Name = "備註")]
        public string REMARK { get; set; }

        [Display(Name = "筆數")]
        [Range(Int32.MinValue, Int32.MaxValue, ErrorMessage = "Invalid Number")]
        public Nullable<int> TRANS_CNT { get; set; }

        [Display(Name = "金額")]
        [Range((double)Decimal.MinValue, (double)Decimal.MaxValue, ErrorMessage = "Invalid Number")]
        public Nullable<decimal> TRANS_AMT { get; set; }
        public string CREATE_DATETIME { get; set; }
        public string UPDATE_DATETIME { get; set; }
    }
}
