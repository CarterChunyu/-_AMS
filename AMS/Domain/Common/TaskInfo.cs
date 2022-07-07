
namespace Domain.Common
{
    public class TaskInfo
    {
        /// <summary>
        /// 使用者
        /// </summary>
        public string User { get; set; }
        /// <summary>
        /// ClientIP
        /// </summary>
        public string IP { get; set; }
        /// <summary>
        /// 排程ID
        /// </summary>
        public int EXEC_ID { get; set; } = 0;
        /// <summary>
        /// 排程版號
        /// </summary>
        public int EXEC_VERSION_ID { get; set; } = 0;
        /// <summary>
        /// 排程執行時間
        /// </summary>
        public string EXEC_TIME { get; set; } = string.Empty;
        /// <summary>
        /// 排程執行狀態(OK=成功、NG=失敗)
        /// </summary>
        public string EXEC_STATUS { get; set; } = "NG";
        /// <summary>
        /// 設定排程執行結果
        /// </summary>
        /// <param name="b_IsSuccess">true=成功、false=失敗</param>
        public void fn_SetEXEC_STATUS(bool b_IsSuccess)
        {
            this.EXEC_STATUS = (b_IsSuccess == true ? "OK" : "NG");
        }
    }
}
