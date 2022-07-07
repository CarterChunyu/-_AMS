using System;

namespace Domain.CoBrandedSellSponsorship
{
    public class SaveDetailTrackReq
    {
        /// <summary>
        /// 存檔類型，Add=新增、Modify=修改、Delete=刪除
        /// </summary>
        public string ExecType { get; set; }
        /// <summary>
        /// 流水號
        /// </summary>
        public int? DTID { get; set; }
        /// <summary>
        /// 對應年度區間銀行表流水號
        /// </summary>
        public int? BID { get; set; }
        /// <summary>
        /// 請款項目
        /// </summary>
        public string ITEM { get; set; }
        /// <summary>
        /// 支付對象流水號
        /// </summary>
        public int? PTID { get; set; }
        /// <summary>
        /// 期間開始日
        /// </summary>
        public DateTime? Range_Date_B { get; set; }
        /// <summary>
        /// 期間結束日
        /// </summary>
        public DateTime? Range_Date_E { get; set; }
        /// <summary>
        /// 營業請款發票日期
        /// </summary>
        public DateTime? Bus_Invoice_Date { get; set; }
        /// <summary>
        /// 營業請款發票號碼
        /// </summary>
        public string Bus_Invoice_No { get; set; }
        /// <summary>
        /// 金額(未稅)
        /// </summary>
        public int? AMT_UnTax { get; set; }
        /// <summary>
        /// 金額(含稅)
        /// </summary>
        public int? AMT_TaxIncluded { get; set; }
        /// <summary>
        /// 備註
        /// </summary>
        public string Comment { get; set; }
        /// <summary>
        /// 送件時間
        /// </summary>
        public DateTime? SendDate { get; set; }
        /// <summary>
        /// 財會確認發票號碼
        /// </summary>
        public string Act_Invoice_No { get; set; }
        /// <summary>
        /// 發票入帳數
        /// </summary>
        public int? AMT_Pay { get; set; }
        /// <summary>
        /// 存檔人員
        /// </summary>
        public string SaveUser { get; set; }
        /// <summary>
        /// 存檔人員IP
        /// </summary>
        public string SaveIP { get; set; }
    }
}
