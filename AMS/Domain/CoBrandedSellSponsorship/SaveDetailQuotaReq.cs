
namespace Domain.CoBrandedSellSponsorship
{
    public class SaveDetailQuotaReq
    {
        /// <summary>
        /// 存檔類型，Add=新增、Modify=修改、Delete=刪除
        /// </summary>
        public string ExecType { get; set; }
        /// <summary>
        /// 流水號
        /// </summary>
        public int? DQID { get; set; }
        /// <summary>
        /// 對應年度區間銀行表流水號
        /// </summary>
        public int? BID { get; set; }
        /// <summary>
        /// 額度項目
        /// </summary>
        public string ITEM { get; set; }
        /// <summary>
        /// 金額
        /// </summary>
        public int? AMT { get; set; }
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
