using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ICASHOPOverdraft
{
    public class OverdraftData
    {
        /// <summary>
        /// 點數日結日期
        /// </summary>
        public string CptDate { get; set; }
        /// <summary>
        /// 通路代號
        /// </summary>
        public string UnifiedBusinessNo { get; set; }
        /// <summary>
        /// 店號
        /// </summary>
        public string StoreNo { get; set; }
        /// <summary>
        /// 機號
        /// </summary>
        public string PosNo { get; set; }
        /// <summary>
        /// 交易序號
        /// </summary>
        public string TransNo { get; set; }
        /// <summary>
        /// 交易時間
        /// </summary>
        public string TransDate { get; set; }
        /// <summary>
        /// 卡別
        /// </summary>
        public string CardType { get; set; }
        /// <summary>
        /// 卡號
        /// </summary>
        public string CardNo { get; set; }
        /// <summary>
        /// 點別
        /// </summary>
        public string PointType { get; set; }
        /// <summary>
        /// OP代墊負點數
        /// </summary>
        public decimal OverdraftPoint { get; set; }
        /// <summary>
        /// 用戶補正負點數
        /// </summary>
        public decimal CorrectionPoint { get; set; }
        /// <summary>
        /// 銷帳編號
        /// </summary>
        public string WriteOffNo { get; set; }
        /// <summary>
        /// 建立日期
        /// </summary>
        public DateTime CreateDate { get; set; }
        /// <summary>
        /// 檔案名稱
        /// </summary>
        public string FileName { get; set; }
    }
}
