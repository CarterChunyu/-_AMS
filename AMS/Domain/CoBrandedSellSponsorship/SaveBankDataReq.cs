
namespace Domain.CoBrandedSellSponsorship
{
    public class SaveBankDataReq
    {
        /// <summary>
        /// 存檔類型，Add=新增、Modify=修改、Delete=刪除
        /// </summary>
        public string ExecType { get; set; }
        /// <summary>
        /// 流水號
        /// </summary>
        public int? BID { get; set; }
        /// <summary>
        /// 對應年度區間主表流水號
        /// </summary>
        public int? RID { get; set; }
        /// <summary>
        /// 銀行特店代號
        /// </summary>
        public string MERCHANT_NO { get; set; }
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
