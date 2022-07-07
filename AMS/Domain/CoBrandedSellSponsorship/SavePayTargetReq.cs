namespace Domain.CoBrandedSellSponsorship
{
    public class SavePayTargetReq
    {
        /// <summary>
        /// 存檔類型，Add=新增、Modify=修改、Delete=刪除
        /// </summary>
        public string ExecType { get; set; }
        /// <summary>
        /// 流水號
        /// </summary>
        public int? PTID { get; set; }
        /// <summary>
        /// 支付對象統編
        /// </summary>
        public string UnifiedBusinessNo { get; set; }
        /// <summary>
        /// 支付對象名稱
        /// </summary>
        public string PayTargetName { get; set; }
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
