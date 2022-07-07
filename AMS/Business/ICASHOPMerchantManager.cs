using System;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;

using DataAccess;
using Domain.Common;
using Domain.ICASHOPMerchant;

namespace Business
{
    public class ICASHOPMerchantManager
    {
        #region 設定全域變數
        private ICASHOPMerchantDAO icashopMerchantDAO { get; set; }
        #endregion

        #region 建構子
        public ICASHOPMerchantManager()
        {
            icashopMerchantDAO = new ICASHOPMerchantDAO();
        }
        #endregion

        /// <summary>
        /// 取得自串點數特店主檔資料
        /// </summary>
        /// <param name="ei">執行訊息</param>
        /// <param name="qr">查詢條件</param>
        /// <param name="pcv">分頁資訊</param>
        /// <returns></returns>
        public DataTable get_MerchantData(ref ExecInfo ei,
                                          QueryReq qr,
                                          PageCountViewModel pcv)
        {
            return icashopMerchantDAO.get_MerchantData(ref ei,
                                                       qr,
                                                       pcv);
        }

        /// <summary>
        /// 設定分頁資訊
        /// </summary>
        /// <param name="ei">執行訊息</param>
        /// <param name="pcv">分頁物件</param>
        /// <param name="qr">查詢條件</param>
        /// <param name="dt_QueryResult">查詢結果</param>
        /// <returns></returns>
        public void set_PageCount(ref ExecInfo ei,
                                  ref PageCountViewModel pcv,
                                  QueryReq qr,
                                  DataTable dt_QueryResult)
        {
            #region 設定tag
            ei.Tag_M = "設定分頁資訊";
            #endregion

            try
            {
                ei.Tag_D = "設定當前頁碼";
                pcv.CurrentPage = qr.CurrentPage;//第幾頁

                ei.Tag_D = "設定總筆數";
                if (dt_QueryResult == null)
                {
                    pcv.TotolCount = 0;
                }
                else
                {
                    DataRow dr_first = dt_QueryResult.AsEnumerable().FirstOrDefault();

                    if (dr_first == null)
                    {
                        pcv.TotolCount = 0;
                    }
                    else
                    {
                        pcv.TotolCount = dr_first.Field<int>("TotalCount");
                    }
                }

                #region 執行成功
                ei.RtnResult = true;
                #endregion
            }
            catch (Exception ex)
            {
                ei.RtnResult = false;
                ei.RtnMsg = ei.fun_combine_err_msg(ex.Message);
            }
        }

        /// <summary>
        /// 檢核特店主檔資訊
        /// </summary>
        /// <param name="ei">執行訊息</param>
        /// <param name="d_MerchantData">特店主檔資訊</param>
        public void check_MerchantData(ref ExecInfo ei,
                                       dynamic d_OPMerchantData)
        {
            #region 設定tag
            ei.Tag_M = "檢核特店主檔資訊";
            #endregion

            #region 設定變數

            string s_break_line = "<br/>";

            string s_UnifiedBusinessNo = string.Empty;
            string s_MerchantName = string.Empty;
            string s_Trans_FTPType = string.Empty;
            string s_Trans_FtpHost = string.Empty;
            string s_Trans_FtpPort = string.Empty;
            string s_Trans_FtpUser = string.Empty;
            string s_Trans_FtpPwd = string.Empty;
            string s_Trans_PoxyIP = string.Empty;
            string s_Trans_PoxyPort = string.Empty;
            string s_Trans_PoxyUser = string.Empty;
            string s_Trans_PoxyPwd = string.Empty;
            string s_Trans_FtpDir = string.Empty;
            string s_Store_FTPType = string.Empty;
            string s_Store_FtpHost = string.Empty;
            string s_Store_FtpPort = string.Empty;
            string s_Store_FtpUser = string.Empty;
            string s_Store_FtpPwd = string.Empty;
            string s_Store_PoxyIP = string.Empty;
            string s_Store_PoxyPort = string.Empty;
            string s_Store_PoxyUser = string.Empty;
            string s_Store_PoxyPwd = string.Empty;
            string s_Store_FtpDir = string.Empty;
            string s_Store_FileType = string.Empty;

            #endregion

            try
            {
                #region 對應變數

                ei.Tag_D = "對應變數";
                s_UnifiedBusinessNo = d_OPMerchantData.UnifiedBusinessNo;
                s_MerchantName = d_OPMerchantData.MerchantName;
                s_Trans_FTPType = d_OPMerchantData.Trans_FTPType;
                s_Trans_FtpHost = d_OPMerchantData.Trans_FtpHost;
                s_Trans_FtpPort = d_OPMerchantData.Trans_FtpPort;
                s_Trans_FtpUser = d_OPMerchantData.Trans_FtpUser;
                s_Trans_FtpPwd = d_OPMerchantData.Trans_FtpPwd;
                s_Trans_PoxyIP = d_OPMerchantData.Trans_PoxyIP;
                s_Trans_PoxyPort = d_OPMerchantData.Trans_PoxyPort;
                s_Trans_PoxyUser = d_OPMerchantData.Trans_PoxyUser;
                s_Trans_PoxyPwd = d_OPMerchantData.Trans_PoxyPwd;
                s_Trans_FtpDir = d_OPMerchantData.Trans_FtpDir;
                s_Store_FTPType = d_OPMerchantData.Store_FTPType;
                s_Store_FtpHost = d_OPMerchantData.Store_FtpHost;
                s_Store_FtpPort = d_OPMerchantData.Store_FtpPort;
                s_Store_FtpUser = d_OPMerchantData.Store_FtpUser;
                s_Store_FtpPwd = d_OPMerchantData.Store_FtpPwd;
                s_Store_PoxyIP = d_OPMerchantData.Store_PoxyIP;
                s_Store_PoxyPort = d_OPMerchantData.Store_PoxyPort;
                s_Store_PoxyUser = d_OPMerchantData.Store_PoxyUser;
                s_Store_PoxyPwd = d_OPMerchantData.Store_PoxyPwd;
                s_Store_FtpDir = d_OPMerchantData.Store_FtpDir;
                s_Store_FileType = d_OPMerchantData.Store_FileType;

                #endregion

                #region 檢核必填欄位

                ei.Tag_D = "檢核必填欄位";
                ei.RtnMsg += fun_chk_must_fill(s_UnifiedBusinessNo, $"{s_break_line}欄位[特店統編]為必填欄位");
                ei.RtnMsg += fun_chk_must_fill(s_MerchantName, $"{s_break_line}欄位[特店名稱]為必填欄位");

                ei.RtnMsg += fun_chk_must_fill(s_Trans_FTPType, $"{s_break_line}每日交易檔案FTP相關設定，欄位[FTP類型]為必填欄位");

                if (s_Trans_FTPType != string.Empty &&
                    s_Trans_FTPType != "NoTrans")
                {
                    ei.RtnMsg += fun_chk_must_fill(s_Trans_FtpHost, $"{s_break_line}每日交易檔案FTP相關設定，欄位[HOST名稱]為必填欄位");
                    ei.RtnMsg += fun_chk_must_fill(s_Trans_FtpPort, $"{s_break_line}每日交易檔案FTP相關設定，欄位[PORT]為必填欄位");
                    ei.RtnMsg += fun_chk_must_fill(s_Trans_FtpUser, $"{s_break_line}每日交易檔案FTP相關設定，欄位[帳號]為必填欄位");
                    ei.RtnMsg += fun_chk_must_fill(s_Trans_FtpPwd, $"{s_break_line}每日交易檔案FTP相關設定，欄位[密碼]為必填欄位");
                    ei.RtnMsg += fun_chk_must_fill(s_Trans_FtpDir, $"{s_break_line}每日交易檔案FTP相關設定，欄位[FTP路徑]為必填欄位");
                }
                else
                {
                    ei.RtnMsg += fun_chk_must_not_fill(s_Trans_FtpHost, $"{s_break_line}每日交易檔案FTP相關設定，欄位[FTP類型]選擇[不傳送檔案]，欄位[HOST名稱]不可有值");
                    ei.RtnMsg += fun_chk_must_not_fill(s_Trans_FtpPort, $"{s_break_line}每日交易檔案FTP相關設定，欄位[FTP類型]選擇[不傳送檔案]，欄位[PORT]不可有值");
                    ei.RtnMsg += fun_chk_must_not_fill(s_Trans_FtpUser, $"{s_break_line}每日交易檔案FTP相關設定，欄位[FTP類型]選擇[不傳送檔案]，欄位[帳號]不可有值");
                    ei.RtnMsg += fun_chk_must_not_fill(s_Trans_FtpPwd, $"{s_break_line}每日交易檔案FTP相關設定，欄位[FTP類型]選擇[不傳送檔案]，欄位[密碼]不可有值");
                    ei.RtnMsg += fun_chk_must_not_fill(s_Trans_PoxyIP, $"{s_break_line}每日交易檔案FTP相關設定，欄位[FTP類型]選擇[不傳送檔案]，欄位[Proxy主機名稱]不可有值");
                    ei.RtnMsg += fun_chk_must_not_fill(s_Trans_PoxyPort, $"{s_break_line}每日交易檔案FTP相關設定，欄位[FTP類型]選擇[不傳送檔案]，欄位[ProxyPort]不可有值");
                    ei.RtnMsg += fun_chk_must_not_fill(s_Trans_PoxyUser, $"{s_break_line}每日交易檔案FTP相關設定，欄位[FTP類型]選擇[不傳送檔案]，欄位[Proxy帳號]不可有值");
                    ei.RtnMsg += fun_chk_must_not_fill(s_Trans_PoxyPwd, $"{s_break_line}每日交易檔案FTP相關設定，欄位[FTP類型]選擇[不傳送檔案]，欄位[Proxy密碼]不可有值");
                    ei.RtnMsg += fun_chk_must_not_fill(s_Trans_FtpDir, $"{s_break_line}每日交易檔案FTP相關設定，欄位[FTP類型]選擇[不傳送檔案]，欄位[FTP路徑]不可有值");
                }

                ei.RtnMsg += fun_chk_must_fill(s_Store_FileType, $"{s_break_line}門市主檔檔案FTP相關設定，欄位[門市檔案匯入類型]為必填欄位");

                if (s_Store_FileType == "FTP")
                {
                    ei.RtnMsg += fun_chk_must_fill(s_Store_FTPType, $"{s_break_line}門市主檔檔案FTP相關設定，欄位[門市檔案匯入類型]選擇[文字檔JOB匯入]，則欄位[FTP類型]為必填欄位");
                }
                else
                {
                    ei.RtnMsg += fun_chk_must_not_fill(s_Store_FTPType, $"{s_break_line}門市主檔檔案FTP相關設定，欄位[門市檔案匯入類型]不是選擇[文字檔JOB匯入]，則欄位[FTP類型]不可有值");
                }

                if (string.IsNullOrWhiteSpace(s_Store_FTPType) == false)
                {
                    ei.RtnMsg += fun_chk_must_fill(s_Store_FTPType, $"{s_break_line}門市主檔檔案FTP相關設定，欄位[FTP類型]有選擇，則欄位[FTP類型]為必填欄位");
                    ei.RtnMsg += fun_chk_must_fill(s_Store_FtpHost, $"{s_break_line}門市主檔檔案FTP相關設定，欄位[FTP類型]有選擇，則欄位[HOST名稱]為必填欄位");
                    ei.RtnMsg += fun_chk_must_fill(s_Store_FtpPort, $"{s_break_line}門市主檔檔案FTP相關設定，欄位[FTP類型]有選擇，則欄位[PORT]為必填欄位");
                    ei.RtnMsg += fun_chk_must_fill(s_Store_FtpUser, $"{s_break_line}門市主檔檔案FTP相關設定，欄位[FTP類型]有選擇，則欄位[帳號]為必填欄位");
                    ei.RtnMsg += fun_chk_must_fill(s_Store_FtpPwd, $"{s_break_line}門市主檔檔案FTP相關設定，欄位[FTP類型]有選擇，則欄位[密碼]為必填欄位");
                    ei.RtnMsg += fun_chk_must_fill(s_Store_FtpDir, $"{s_break_line}門市主檔檔案FTP相關設定，欄位[FTP類型]有選擇，則欄位[FTP路徑]為必填欄位");
                }
                else
                {
                    ei.RtnMsg += fun_chk_must_not_fill(s_Store_FTPType, $"{s_break_line}門市主檔檔案FTP相關設定，欄位[FTP類型]沒有選擇，則欄位[FTP類型]不可有值");
                    ei.RtnMsg += fun_chk_must_not_fill(s_Store_FtpHost, $"{s_break_line}門市主檔檔案FTP相關設定，欄位[FTP類型]沒有選擇，則欄位[HOST名稱]不可有值");
                    ei.RtnMsg += fun_chk_must_not_fill(s_Store_FtpPort, $"{s_break_line}門市主檔檔案FTP相關設定，欄位[FTP類型]沒有選擇，則欄位[PORT]不可有值");
                    ei.RtnMsg += fun_chk_must_not_fill(s_Store_FtpUser, $"{s_break_line}門市主檔檔案FTP相關設定，欄位[FTP類型]沒有選擇，則欄位[帳號]不可有值");
                    ei.RtnMsg += fun_chk_must_not_fill(s_Store_FtpPwd, $"{s_break_line}門市主檔檔案FTP相關設定，欄位[FTP類型]沒有選擇，則欄位[密碼]不可有值");
                    ei.RtnMsg += fun_chk_must_not_fill(s_Store_PoxyIP, $"{s_break_line}門市主檔檔案FTP相關設定，欄位[FTP類型]沒有選擇，則欄位[Proxy主機名稱]不可有值");
                    ei.RtnMsg += fun_chk_must_not_fill(s_Store_PoxyPort, $"{s_break_line}門市主檔檔案FTP相關設定，欄位[FTP類型]沒有選擇，則欄位[ProxyPort]不可有值");
                    ei.RtnMsg += fun_chk_must_not_fill(s_Store_PoxyUser, $"{s_break_line}門市主檔檔案FTP相關設定，欄位[FTP類型]沒有選擇，則欄位[Proxy帳號]不可有值");
                    ei.RtnMsg += fun_chk_must_not_fill(s_Store_PoxyPwd, $"{s_break_line}門市主檔檔案FTP相關設定，欄位[FTP類型]沒有選擇，則欄位[Proxy密碼]不可有值");
                    ei.RtnMsg += fun_chk_must_not_fill(s_Store_FtpDir, $"{s_break_line}門市主檔檔案FTP相關設定，欄位[FTP類型]沒有選擇，則欄位[FTP路徑]不可有值");
                }

                #endregion

                #region 以上檢核有誤則跳出

                if (string.IsNullOrEmpty(ei.RtnMsg) == false)
                { throw new chk_exception(); }

                #endregion

                #region 檢核欄位長度

                ei.Tag_D = "檢核欄位長度";
                ei.RtnMsg += fun_chk_str_length(s_MerchantName, 50, $"{s_break_line}欄位[特店名稱]長度為({{0}})個字，超過限制長度({{1}})個字");

                #endregion

                #region 檢核欄位Byte長度

                ei.Tag_D = "檢核欄位Byte長度";
                ei.RtnMsg += fun_chk_str_byte_length(s_Trans_FtpHost, 20, $"{s_break_line}每日交易檔案FTP相關設定，欄位[HOST名稱]長度為({{0}})個Byte，超過限制長度({{1}})個Byte");
                ei.RtnMsg += fun_chk_str_byte_length(s_Trans_FtpPort, 5, $"{s_break_line}每日交易檔案FTP相關設定，欄位[PORT]長度為({{0}})個Byte，超過限制長度({{1}})個Byte");
                ei.RtnMsg += fun_chk_str_byte_length(s_Trans_FtpUser, 50, $"{s_break_line}每日交易檔案FTP相關設定，欄位[帳號]長度為({{0}})個Byte，超過限制長度({{1}})個Byte");
                ei.RtnMsg += fun_chk_str_byte_length(s_Trans_FtpPwd, 50, $"{s_break_line}每日交易檔案FTP相關設定，欄位[密碼]長度為({{0}})個Byte，超過限制長度({{1}})個Byte");
                ei.RtnMsg += fun_chk_str_byte_length(s_Trans_PoxyIP, 20, $"{s_break_line}每日交易檔案FTP相關設定，欄位[Proxy主機名稱]長度為({{0}})個Byte，超過限制長度({{1}})個Byte");
                ei.RtnMsg += fun_chk_str_byte_length(s_Trans_PoxyPort, 5, $"{s_break_line}每日交易檔案FTP相關設定，欄位[ProxyPort]長度為({{0}})個Byte，超過限制長度({{1}})個Byte");
                ei.RtnMsg += fun_chk_str_byte_length(s_Trans_PoxyUser, 50, $"{s_break_line}每日交易檔案FTP相關設定，欄位[Proxy帳號]長度為({{0}})個Byte，超過限制長度({{1}})個Byte");
                ei.RtnMsg += fun_chk_str_byte_length(s_Trans_PoxyPwd, 50, $"{s_break_line}每日交易檔案FTP相關設定，欄位[Proxy密碼]長度為({{0}})個Byte，超過限制長度({{1}})個Byte");
                ei.RtnMsg += fun_chk_str_byte_length(s_Trans_FtpDir, 50, $"{s_break_line}每日交易檔案FTP相關設定，欄位[FTP路徑]長度為({{0}})個Byte，超過限制長度({{1}})個Byte");

                ei.RtnMsg += fun_chk_str_byte_length(s_Store_FtpHost, 20, $"{s_break_line}門市主檔檔案FTP相關設定，欄位[HOST名稱]長度為({{0}})個Byte，超過限制長度({{1}})個Byte");
                ei.RtnMsg += fun_chk_str_byte_length(s_Store_FtpPort, 5, $"{s_break_line}門市主檔檔案FTP相關設定，欄位[PORT]長度為({{0}})個Byte，超過限制長度({{1}})個Byte");
                ei.RtnMsg += fun_chk_str_byte_length(s_Store_FtpUser, 50, $"{s_break_line}門市主檔檔案FTP相關設定，欄位[帳號]長度為({{0}})個Byte，超過限制長度({{1}})個Byte");
                ei.RtnMsg += fun_chk_str_byte_length(s_Store_FtpPwd, 50, $"{s_break_line}門市主檔檔案FTP相關設定，欄位[密碼]長度為({{0}})個Byte，超過限制長度({{1}})個Byte");
                ei.RtnMsg += fun_chk_str_byte_length(s_Store_PoxyIP, 20, $"{s_break_line}門市主檔檔案FTP相關設定，欄位[Proxy主機名稱]長度為({{0}})個Byte，超過限制長度({{1}})個Byte");
                ei.RtnMsg += fun_chk_str_byte_length(s_Store_PoxyPort, 5, $"{s_break_line}門市主檔檔案FTP相關設定，欄位[ProxyPort]長度為({{0}})個Byte，超過限制長度({{1}})個Byte");
                ei.RtnMsg += fun_chk_str_byte_length(s_Store_PoxyUser, 50, $"{s_break_line}門市主檔檔案FTP相關設定，欄位[Proxy帳號]長度為({{0}})個Byte，超過限制長度({{1}})個Byte");
                ei.RtnMsg += fun_chk_str_byte_length(s_Store_PoxyPwd, 50, $"{s_break_line}門市主檔檔案FTP相關設定，欄位[Proxy密碼]長度為({{0}})個Byte，超過限制長度({{1}})個Byte");
                ei.RtnMsg += fun_chk_str_byte_length(s_Store_FtpDir, 50, $"{s_break_line}門市主檔檔案FTP相關設定，欄位[FTP路徑]長度為({{0}})個Byte，超過限制長度({{1}})個Byte");

                #endregion

                #region 以上檢核有誤則跳出

                if (string.IsNullOrEmpty(ei.RtnMsg) == false)
                { throw new chk_exception(); }

                #endregion

                #region 檢核統編是否為8碼數字

                ei.Tag_D = "檢核統編是否為8碼數字";
                Regex rg_01 = new Regex(@"^\d{8}$");
                if (rg_01.IsMatch(s_UnifiedBusinessNo) == false)
                {
                    ei.RtnMsg += string.Format("{0}欄位[特店統編]({1})需為8碼數字",
                                               s_break_line,
                                               s_UnifiedBusinessNo);
                }

                #endregion

                #region 檢核欄位FTP類型

                ei.Tag_D = "檢核欄位FTP類型";
                //未來若有增加，檢核也需增加
                if (s_Trans_FTPType != string.Empty &&
                    s_Trans_FTPType != "FTPS" &&
                    s_Trans_FTPType != "FTP" &&
                    s_Trans_FTPType != "NoTrans")
                {
                    ei.RtnMsg += string.Format("{0}每日交易檔案FTP相關設定，欄位[FTP類型]值({1})異常",
                                               s_break_line,
                                               s_Trans_FTPType);
                }

                if (s_Store_FTPType != string.Empty &&
                    s_Store_FTPType != "FTPS" &&
                    s_Store_FTPType != "FTP")
                {
                    ei.RtnMsg += string.Format("{0}門市主檔檔案FTP相關設定，欄位[FTP類型]值({1})異常",
                                               s_break_line,
                                               s_Store_FTPType);
                }

                #endregion

                #region 檢核欄位門市檔案匯入類型

                ei.Tag_D = "檢核欄位門市檔案匯入類型";
                //未來若有增加，檢核也需增加
                if (s_Store_FileType != string.Empty &&
                    s_Store_FileType != "Excel" &&
                    s_Store_FileType != "FTP")
                {
                    ei.RtnMsg += string.Format("{0}門市主檔檔案FTP相關設定，欄位[門市檔案匯入類型]值({1})異常",
                                               s_break_line,
                                               s_Store_FileType);
                }

                #endregion

                #region 以上檢核有誤則跳出

                if (string.IsNullOrEmpty(ei.RtnMsg) == false)
                { throw new chk_exception(); }

                #endregion

                #region 檢核成功

                ei.RtnResult = true;

                #endregion
            }
            catch (chk_exception)
            {
                ei.RtnResult = false;
            }
            catch (Exception ex)
            {
                ei.RtnResult = false;
                ei.RtnMsg = ei.fun_combine_err_msg(ex.Message);
            }
        }

        /// <summary>
        /// 新增自串點數特店主檔
        /// </summary>
        /// <param name="ei">執行訊息</param>
        /// <param name="esr">新增自串點數特店主檔資料</param>
        public void add_MerchantData(ref ExecInfo ei,
                                     EditSaveReq esr)
        {
            icashopMerchantDAO.add_MerchantData(ref ei,
                                                esr);
        }

        /// <summary>
        /// 更新自串點數特店主檔
        /// </summary>
        /// <param name="ei">執行訊息</param>
        /// <param name="esr">更新自串點數特店主檔資料</param>
        public void modify_MerchantData(ref ExecInfo ei,
                                        EditSaveReq esr)
        {
            icashopMerchantDAO.modify_MerchantData(ref ei,
                                                   esr);
        }

        /// <summary>
        /// 同步同一個統編的Merchant及Store資訊
        /// </summary>
        /// <param name="ei">執行訊息</param>
        /// <param name="esr">同一個統編的資訊</param>
        public void synchronize_Merchant(ref ExecInfo ei,
                                        EditSaveReq esr)
        {
            icashopMerchantDAO.synchronize_Merchant(ref ei,
                                                   esr);
        }

        #region 自訂方法

        #region 檢核相關


        private string fun_chk_must_fill(string s_str, string s_chk_str)
        {
            string s_rtn = string.Empty;

            if (string.IsNullOrWhiteSpace(s_str) == true)
            {
                s_rtn = s_chk_str;
            }

            return s_rtn;
        }

        private string fun_chk_must_not_fill(string s_str, string s_chk_str)
        {
            string s_rtn = string.Empty;

            if (string.IsNullOrWhiteSpace(s_str) == false)
            {
                s_rtn = s_chk_str;
            }

            return s_rtn;
        }

        private string fun_chk_str_length(string s_str, int i_limit, string s_chk_str)
        {
            string s_rtn = string.Empty;
            int i_length = s_str.Length;

            if (i_length > i_limit)
            {
                s_rtn = string.Format(s_chk_str,
                                      i_length.ToString(),
                                      i_limit.ToString());
            }

            return s_rtn;
        }

        private string fun_chk_str_byte_length(string s_str, int i_limit, string s_chk_str)
        {
            string s_rtn = string.Empty;
            int i_length = Encoding.GetEncoding(950).GetBytes(s_str).Length;

            if (i_length > i_limit)
            {
                s_rtn = string.Format(s_chk_str,
                                      i_length.ToString(),
                                      i_limit.ToString());
            }

            return s_rtn;
        }

        #endregion

        /// <summary>
        /// 自訂例外：廠商回覆電文格式異常
        /// </summary>
        private class chk_exception : Exception
        {
            public chk_exception() { }
            public chk_exception(string message) : base(message) { }
            public chk_exception(string message, Exception inner) : base(message, inner) { }
        }

        #endregion
    }
}
