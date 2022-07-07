using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class ACInvoiceRewardReport2021
    {
        public class ReportData
        {
            public date Date { get; set; }
            public total Total { get; set; }
            public List<Report> Reports { get; set; }
            public List<Detail> Details { get; set; }
        }
        public class date
        {
            /// <summary>
            /// 資料表分類
            /// </summary>
            public string DBNO { get; set; }
            /// <summary>
            /// 提報區間
            /// </summary>
            public string SEDATE { get; set; }
        }

        public class total
        {
            /// <summary>
            /// 資料表分類
            /// </summary>
            public string DBNO { get; set; }
            /// <summary>
            /// 總計
            /// </summary>
            public string TOTAL { get; set; }
        }

        ///<summary>總表</summary>
        public class Report
        {
            public string DBNO { get; set; }
            /// <summary>
            /// 序號
            /// </summary>
            public int TW_ID { get; set; }
            /// <summary>
            /// 消費者 UUID
            /// </summary>
            public string CUS_UUID { get; set; } 
            /// <summary>
            /// 綁定日
            /// </summary>
            public string AUTH_TIME { get; set; }
            /// <summary>
            /// 綁定序號
            /// </summary>
            public string TXN_NO { get; set; }
            /// <summary>
            /// 交易筆數
            /// </summary>
            public int TRANS_CNT { get; set; }
            /// <summary>
            /// 交易總額
            /// </summary>
            public string TRANS_AMT { get; set; }
            /// <summary>
            /// 五倍券實際匯入回饋總額
            /// </summary>
            public string REWARD_AMT { get; set; }
            /// <summary>
            /// 註記
            /// </summary>
            public string REMARK { get; set; }
        }

        ///<summary>明細</summary>
        public class Detail
        {
            public string DBNO { get; set; }
            /// <summary>
            /// 序號
            /// </summary>
            public int TW_ID { get; set; }
            /// <summary>
            /// UUID
            /// </summary>
            public string CUS_UUID { get; set; }
            /// <summary>
            /// 交易店鋪名稱
            /// </summary>
            public string STORE_NAME { get; set; }
            /// <summary>
            /// 交易日期
            /// </summary>
            public string TRANS_DATE { get; set; }
            /// <summary>
            /// 交易序號或流水編號
            /// </summary>
            public string TXLOG_ID { get; set; }
            /// <summary>
            /// 交易金額
            /// </summary>
            public string TRANS_AMT { get; set; }
            /// <summary>
            /// 統編或特店編號
            /// </summary>
            public string TAX_ID { get; set; }
            /// <summary>
            /// 交易店家行業類別
            /// </summary>
            public string TRANS_STORE_TYPE { get; set; }
            /// <summary>
            /// 五倍券實際匯入回饋總額
            /// </summary>
            public string REWARD_AMT { get; set; }
            /// <summary>
            /// 提報檔名
            /// </summary>
            public string FILE_NAME { get; set; }
        }
    }
}
