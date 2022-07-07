using System;

namespace Domain.CoBrandedSellSponsorship
{
    public class SaveRangeDataReq
    {
        /// <summary>
        /// 存檔類型，Add=新增、Modify=修改、Delete=刪除
        /// </summary>
        public string ExecType { get; set; }
        /// <summary>
        /// 流水號
        /// </summary>
        public int? RID { get; set; }
        /// <summary>
        /// 年度起日
        /// </summary>
        public DateTime? Date_B { get; set; }
        /// <summary>
        /// 年度迄日
        /// </summary>
        public DateTime? Date_E { get; set; }
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
