
namespace Domain.Common
{
    public class ExecInfo
    {
        /// <summary>
        /// 建構子
        /// </summary>
        public ExecInfo()
        { }
        /// <summary>
        /// 回傳執行結果，true=成功、false=失敗
        /// </summary>
        public bool RtnResult { get; set; } = false;
        /// <summary>
        /// 回傳訊息
        /// </summary>
        public string RtnMsg { get; set; } = string.Empty;
        /// <summary>
        /// 主旗標
        /// </summary>
        public string Tag_M { get; set; } = string.Empty;
        /// <summary>
        /// 明細旗標
        /// </summary>
        public string Tag_D { get; set; } = string.Empty;
        /// <summary>
        /// 組合訊息-錯誤訊息
        /// </summary>
        /// <param name="s_msg"></param>
        /// <returns></returns>
        public string fun_combine_err_msg(string s_msg)
        {
            return string.Format("執行發生錯誤，Tag_M=[{0}]，Tag_D=[{1}]，錯誤原因：{2}。",
                                 this.Tag_M,
                                 this.Tag_D,
                                 s_msg);
        }
        /// <summary>
        /// 重設
        /// </summary>
        /// <returns></returns>
        public void fun_reset()
        {
            this.RtnResult = false;
            this.RtnMsg = string.Empty;
            this.Tag_M = string.Empty;
            this.Tag_D = string.Empty;
        }
    }
}
