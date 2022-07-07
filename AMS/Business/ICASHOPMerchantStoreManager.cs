using System;
using System.IO;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;
using System.Web;
using System.Collections.Generic;

using DataAccess;
using Domain.Common;
using Domain.ICASHOPMerchantStore;

using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace Business
{
    public class ICASHOPMerchantStoreManager
    {
        #region 設定全域變數
        private ICASHOPMerchantStoreDAO icashopMerchantStoreDAO { get; set; }
        #endregion

        #region 建構子
        public ICASHOPMerchantStoreManager()
        {
            icashopMerchantStoreDAO = new ICASHOPMerchantStoreDAO();
        }
        #endregion

        /// <summary>
        /// 下載檔案-範本
        /// </summary>
        /// <param name="ei">執行訊息</param>
        /// <param name="fi_Template">範本檔案資訊</param>
        /// <param name="rtn_byte_data">回傳檔案Byte</param>
        /// <param name="rtn_filename">回傳檔名</param>
        public void file_Download_Template(ref ExecInfo ei,
                                           FileInfo fi_Template,
                                           out byte[] rtn_byte_data,
                                           out string rtn_filename)
        {
            #region 設定tag
            ei.Tag_M = "下載檔案-範本";
            #endregion

            #region 設定變數
            rtn_byte_data = null;
            rtn_filename = string.Empty;
            #endregion

            try
            {
                #region 範本檔轉Byte

                ei.Tag_D = "將範本檔轉Byte";
                rtn_byte_data = System.IO.File.ReadAllBytes(fi_Template.FullName);

                #endregion

                #region 設定下載檔名

                ei.Tag_D = "設定下載檔名";
                rtn_filename = "愛金卡點數平台_特店門市轉入範本.xlsx";

                #endregion

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
        /// 下載檔案-特店門市資料
        /// </summary>
        /// <param name="ei">執行訊息</param>
        /// <param name="fi_Template">範本檔案資訊</param>
        /// <param name="s_UnifiedBusinessNo">特店統編</param>
        /// <param name="rtn_byte_data">回傳檔案Byte</param>
        /// <param name="rtn_filename">回傳檔名</param>
        public void file_Download_MerchantStoreDetail(ref ExecInfo ei,
                                                      FileInfo fi_Template,
                                                      string s_UnifiedBusinessNo,
                                                      out byte[] rtn_byte_data,
                                                      out string rtn_filename)
        {
            #region 設定tag
            ei.Tag_M = "下載檔案-特店門市資料";
            #endregion

            #region 設定變數

            ei.Tag_D = "設定變數";

            rtn_byte_data = null;
            rtn_filename = string.Empty;
            DataTable dt_data = null;
            FileStream stream_template = null;
            int i_data_cnt = 0;

            #endregion

            try
            {
                #region 至資料庫取得該特店門市資料

                ei.Tag_D = "至資料庫取得該特店門市資料";
                dt_data = icashopMerchantStoreDAO.get_MerchantStoreDetail(ref ei,
                                                                          s_UnifiedBusinessNo);

                if (ei.RtnResult == false)
                { throw new Exception(ei.RtnMsg); }

                #endregion

                #region 檢核特店門市筆數

                ei.Tag_D = "檢核特店門市筆數";

                i_data_cnt = dt_data.Rows.Count;

                if (i_data_cnt == 0)
                {
                    throw new Exception(string.Format("查無特店({0})門市資料",
                                                      s_UnifiedBusinessNo));
                }

                #endregion

                #region 設定下載檔名

                ei.Tag_D = "設定下載檔名";

                rtn_filename = string.Format("愛金卡點數平台_{0}_{1}_{2}.xlsx",
                                             s_UnifiedBusinessNo,
                                             i_data_cnt.ToString().PadLeft(8, '0'),
                                             DateTime.Now.ToString("yyyyMMdd"));

                #endregion

                #region 處理Excel檔

                ei.Tag_D = "處理Excel檔，開啟範本檔";
                using (stream_template = new FileStream(fi_Template.FullName,
                                                        FileMode.Open,
                                                        FileAccess.ReadWrite,
                                                        FileShare.ReadWrite))
                {
                    ei.Tag_D = "處理Excel檔，建立Excel";
                    using (ExcelPackage p = new ExcelPackage(stream_template))
                    {

                        ei.Tag_D = "處理Excel檔，讀取Sheet";
                        ExcelWorksheet ws_store = p.Workbook.Worksheets["STORE"];

                        ei.Tag_D = "處理Excel檔，清空第一行";
                        ws_store.Cells[2, 1, 2, 9].Clear();

                        ei.Tag_D = "處理Excel檔，載入資料";
                        ws_store.Cells[2, 1].LoadFromDataTable(dt_data, false);

                        ei.Tag_D = "處理Excel檔，設定格線";
                        ws_store.Cells[2, 1, i_data_cnt + 1, 9].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        ws_store.Cells[2, 1, i_data_cnt + 1, 9].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        ws_store.Cells[2, 1, i_data_cnt + 1, 9].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        ws_store.Cells[2, 1, i_data_cnt + 1, 9].Style.Border.Left.Style = ExcelBorderStyle.Thin;

                        ei.Tag_D = "處理Excel檔，自動調整欄位大小";
                        ws_store.Cells[1, 1, i_data_cnt + 1, 9].AutoFitColumns();

                        ei.Tag_D = "處理Excel檔，凍結視窗";
                        ws_store.View.FreezePanes(2, 1);

                        ei.Tag_D = "處理Excel檔，取得Byte";
                        rtn_byte_data = p.GetAsByteArray();
                    }
                }

                #endregion

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
        /// 作業開始
        /// </summary>
        /// <param name="ei">執行訊息</param>
        /// <param name="taskInfo">作業資訊</param>
        public void Task_Begin(ref ExecInfo ei,
                               ref TaskInfo taskInfo)
        {
            icashopMerchantStoreDAO.Task_Begin(ref ei,
                                               ref taskInfo);
        }

        /// <summary>
        /// 作業結束
        /// </summary>
        /// <param name="ei">執行訊息</param>
        /// <param name="taskInfo">作業資訊</param>
        public void Task_End(ref ExecInfo ei,
                             TaskInfo taskInfo)
        {
            icashopMerchantStoreDAO.Task_End(ref ei,
                                             taskInfo);
        }

        /// <summary>
        /// 將上傳匯入檔案存入AP主機
        /// </summary>
        /// <param name="ei">執行訊息</param>
        /// <param name="taskInfo">作業資訊</param>
        /// <param name="hfcb">上傳檔案</param>
        /// <param name="s_BasePath">存檔Base目錄</param>
        /// <param name="li_ifi">匯入檔案資訊</param>
        public void UploadImportFile(ref ExecInfo ei,
                                     TaskInfo taskInfo,
                                     HttpFileCollectionBase hfcb,
                                     string s_BasePath,
                                     ref List<ImportFileInfo> li_ifi)
        {
            #region 設定tag
            ei.Tag_M = "將上傳匯入檔案存入AP主機";
            #endregion

            #region 設定變數

            string s_break_line = "<br/>";//訊息斷行符號

            #endregion

            try
            {
                #region 判斷上傳檔案數量

                ei.Tag_D = "判斷上傳檔案數量";
                if (hfcb.Count == 0)
                {
                    throw new Exception("無上傳檔案！");
                }

                #endregion

                #region 驗證上傳檔案檔名

                ei.Tag_D = "驗證上傳檔案檔名";

                for (int i = 0; i < hfcb.Count; i++)
                {
                    ImportFileInfo ifi = new ImportFileInfo();
                    li_ifi.Add(ifi);

                    ei.Tag_D = "處理檔案";
                    ifi.file = hfcb[i];

                    ei.Tag_D = "處理檔案檔名資訊";
                    ifi.UploadFileName = Path.GetFileName(ifi.file.FileName);//上傳檔案檔名
                    ifi.UploadFileNameWithoutExtension = Path.GetFileNameWithoutExtension(ifi.file.FileName);//上傳檔案檔名(沒有副檔名)

                    //依照分隔符號分解檔名
                    string[] array_UploadFileName = ifi.UploadFileNameWithoutExtension.Split('_');

                    ei.Tag_D = "驗證上傳檔案檔名格式";
                    if (array_UploadFileName.Length != 4)
                    {
                        ifi.ErrMsg += string.Format("上傳檔案檔名格式錯誤，格式為[愛金卡點數平台_統編_資料筆數_產檔系統日.xlsx]。",
                                                    ifi.UploadFileName);
                        //如果此驗證錯誤，後面不再繼續，避免產生意外錯誤
                        continue;
                    }
                    else
                    {
                        ifi.UnifiedBusinessNo = array_UploadFileName[1];//特店統編
                        ifi.s_DataCnt = array_UploadFileName[2];//檔案內容筆數
                    }

                    ei.Tag_D = "驗證上傳檔案檔名，驗證統編";
                    Regex rg = new Regex(@"^\d{8}$");
                    if (rg.IsMatch(ifi.UnifiedBusinessNo) == false)
                    {
                        ifi.ErrMsg += string.Format("特店統編({0})需為8碼數字。",
                                                    ifi.UnifiedBusinessNo);
                    }

                    ei.Tag_D = "驗證上傳檔案檔名，驗證資料筆數";
                    if (rg.IsMatch(ifi.s_DataCnt) == false)
                    {
                        ifi.ErrMsg += string.Format("資料筆數({0})需為8碼數字。",
                                                    ifi.s_DataCnt);
                    }
                    else
                    {
                        ifi.i_DataCnt = int.Parse(ifi.s_DataCnt);
                    }
                }

                #endregion

                #region 將檔案存入AP主機

                ei.Tag_D = "將檔案存入AP主機，設定路徑資料夾資訊";
                DirectoryInfo dif_UploadData = new DirectoryInfo(string.Format(@"{0}\{1}\{2}",
                                                                               s_BasePath,
                                                                               DateTime.ParseExact(taskInfo.EXEC_TIME, "yyyyMMddHHmmssfff", null).ToString("yyyyMMdd"),
                                                                               taskInfo.EXEC_TIME));

                ei.Tag_D = "將檔案存入AP主機，建立路徑資料夾";
                if (dif_UploadData.Exists == false)
                {
                    dif_UploadData.Create();
                }

                ei.Tag_D = "將檔案存入AP主機，存檔";
                foreach (ImportFileInfo li_detl in li_ifi)
                {
                    li_detl.ServerFileFullName = string.Format(@"{0}\{1}",
                                                               dif_UploadData.FullName,
                                                               li_detl.UploadFileName);
                    li_detl.file.SaveAs(li_detl.ServerFileFullName);
                }

                #endregion

                #region 驗證

                string s_chk_str = string.Empty;

                #region 驗證-上傳檔案是否存在相同統編

                ei.Tag_D = "驗證-上傳檔案是否存在相同統編";

                var var_chk_01 = from x in li_ifi
                                 group x by x.UnifiedBusinessNo into gb
                                 where gb.Count() > 1
                                 select new
                                 {
                                     UnifiedBusinessNo = gb.Key,
                                     Count = gb.Count()
                                 };

                if (var_chk_01.Count() > 0)
                {
                    foreach (var chk_detl in var_chk_01)
                    {
                        s_chk_str += string.Format("{0}統編：[{1}]，對應到多個({2})檔案",
                                                   s_break_line,
                                                   chk_detl.UnifiedBusinessNo,
                                                   chk_detl.Count.ToString());
                    }
                }

                #endregion

                #region 驗證-作業是否異常

                ei.Tag_D = "驗證-作業是否異常";

                var var_chk_02 = from x in li_ifi
                                 where string.IsNullOrEmpty(x.ErrMsg) == false
                                 select x;

                if (var_chk_02.Count() > 0)
                {
                    foreach (var chk_detl in var_chk_02)
                    {
                        s_chk_str += string.Format("{0}檔名[{1}]驗證失敗，失敗原因：[{2}]",
                                                   s_break_line,
                                                   chk_detl.UploadFileName,
                                                   chk_detl.ErrMsg);
                    }
                }

                #endregion

                #region 如果有錯誤，則跳出

                ei.Tag_D = "驗證-判斷是否異常";
                if (string.IsNullOrWhiteSpace(s_chk_str) == false)
                {
                    throw new Exception(s_chk_str);
                }

                #endregion

                #endregion

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
        /// 取得匯入檔案資料
        /// </summary>
        /// <param name="ei">執行訊息</param>
        /// <param name="taskInfo">作業資訊</param>
        /// <param name="li_ifi">匯入檔案資訊</param>
        /// <param name="dt_data">匯入檔案內容</param>
        public void GetImportData(ref ExecInfo ei,
                                  TaskInfo taskInfo,
                                  List<ImportFileInfo> li_ifi,
                                  out DataTable dt_data)
        {
            #region 設定tag
            ei.Tag_M = "取得匯入檔案資料";
            #endregion

            #region 設定變數

            string s_chk_str = string.Empty;
            string s_break_line = "<br/>";
            dt_data = null;

            #endregion

            try
            {
                #region 設定DataTable的Schema

                ei.Tag_D = "設定DataTable的Schema";
                dt_data = new DataTable();
                dt_data.Columns.Add("UnifiedBusinessNo", typeof(string));//特店統編
                dt_data.Columns.Add("StoreNo", typeof(string));//門市代號
                dt_data.Columns.Add("StoreName", typeof(string));//門市代號
                dt_data.Columns.Add("StorePhone", typeof(string));//門市電話
                dt_data.Columns.Add("StoreFax", typeof(string));//傳真號碼
                dt_data.Columns.Add("ZipCode", typeof(string));//郵遞區號
                dt_data.Columns.Add("StoreAddress", typeof(string));//門市地址
                dt_data.Columns.Add("EFF_DATE_FROM", typeof(string));//門市啟用日
                dt_data.Columns.Add("EFF_DATE_TO", typeof(string));//門市停用日
                dt_data.Columns.Add("IsTest", typeof(string));//測試門市

                #endregion

                #region 讀取上傳檔案並將資料寫入DataTable

                ei.Tag_D = "讀取上傳檔案並將資料寫入DataTable";
                foreach (ImportFileInfo ufi in li_ifi)
                {
                    ei.Tag_D = $"讀取上傳檔案並將資料寫入DataTable，檔案：[{ufi.UploadFileName}]";
                    using (FileStream fs = new FileStream(ufi.ServerFileFullName,
                                                          FileMode.Open,
                                                          FileAccess.Read,
                                                          FileShare.ReadWrite))
                    {
                        //載入Excel檔案
                        using (ExcelPackage ep = new ExcelPackage(fs))
                        {
                            ExcelWorksheet sheet = ep.Workbook.Worksheets[1]; //取得Sheet1

                            bool isLastRow = false;
                            int RowId = 2; //因為有標題列，所以從第2列開始讀起

                            do //讀取資料，直到讀到空白列為止
                            {
                                string cellValue = sheet.Cells[RowId, 1].Text;
                                if (string.IsNullOrEmpty(cellValue))
                                {
                                    isLastRow = true;
                                }
                                else
                                {
                                    // 將資料寫入DataTable
                                    DataRow dr_new = dt_data.NewRow();

                                    dr_new["UnifiedBusinessNo"] = ufi.UnifiedBusinessNo;
                                    dr_new["StoreNo"] = sheet.Cells[RowId, 1].Text.Trim();
                                    dr_new["StoreName"] = sheet.Cells[RowId, 2].Text.Trim();
                                    dr_new["StorePhone"] = sheet.Cells[RowId, 3].Text.Trim();
                                    dr_new["StoreFax"] = sheet.Cells[RowId, 4].Text.Trim();
                                    dr_new["ZipCode"] = sheet.Cells[RowId, 5].Text.Trim();
                                    dr_new["StoreAddress"] = sheet.Cells[RowId, 6].Text.Trim();
                                    dr_new["EFF_DATE_FROM"] = sheet.Cells[RowId, 7].Text.Trim();
                                    dr_new["EFF_DATE_TO"] = sheet.Cells[RowId, 8].Text.Trim();
                                    dr_new["IsTest"] = sheet.Cells[RowId, 9].Text.Trim();

                                    dt_data.Rows.Add(dr_new);

                                    RowId += 1;
                                }
                            }
                            while (isLastRow == false);
                        }
                    }
                }

                #endregion

                #region 驗證檔案資料

                ei.Tag_D = "驗證檔案資料";
                foreach (ImportFileInfo ufi in li_ifi)
                {
                    #region 設定變數
                    string s_chk_str_detl = string.Empty;
                    #endregion

                    #region 驗證檔案資料，取得驗證資料

                    ei.Tag_D = $"檔案：[{ufi.UploadFileName}]，驗證檔案資料，取得驗證資料";
                    var var_chk_data = from x in dt_data.AsEnumerable()
                                       where x["UnifiedBusinessNo"].ToString() == ufi.UnifiedBusinessNo
                                       select x;

                    #endregion

                    #region 驗證資料筆數

                    ei.Tag_D = $"檔案：[{ufi.UploadFileName}]，驗證檔案資料，驗證資料筆數";

                    if (ufi.i_DataCnt != var_chk_data.Count())
                    {
                        s_chk_str_detl += string.Format("{0}檔案：[{1}]，檔名註明的資料筆數({2})與檔案內容的資料筆數({3})不符。",
                                                        s_break_line,
                                                        ufi.UploadFileName,
                                                        ufi.i_DataCnt.ToString(),
                                                        var_chk_data.Count().ToString());
                    }

                    #endregion

                    #region 以上驗證(1)若有異常，則不繼續

                    ei.Tag_D = $"檔案：[{ufi.UploadFileName}]，以上驗證(1)若有異常，則不繼續";
                    if (string.IsNullOrEmpty(s_chk_str_detl) == false)
                    {
                        s_chk_str += $"{s_break_line}{new string('=', 50)}{s_chk_str_detl}";
                        continue;
                    }

                    #endregion

                    #region 驗證必填欄位

                    ei.Tag_D = $"檔案：[{ufi.UploadFileName}]，驗證檔案資料，驗證必填欄位";
                    var chk_data_01 = from x in var_chk_data
                                      where (string.IsNullOrWhiteSpace(x["UnifiedBusinessNo"].ToString()) == true ||
                                             string.IsNullOrWhiteSpace(x["StoreNo"].ToString()) == true ||
                                             string.IsNullOrWhiteSpace(x["StoreName"].ToString()) == true ||
                                             string.IsNullOrWhiteSpace(x["EFF_DATE_FROM"].ToString()) == true ||
                                             string.IsNullOrWhiteSpace(x["EFF_DATE_TO"].ToString()) == true ||
                                             string.IsNullOrWhiteSpace(x["IsTest"].ToString()) == true)
                                      select x;

                    foreach (var dr_detl in chk_data_01)
                    {
                        s_chk_str_detl += string.Format("{0}檔案：[{1}]，統編：{2}、門市代號：{3}、門市名稱：{4}、門市啟用日：{5}、門市停用日：{6}、測試門市：{7}，資料異常，欄位皆為必填欄位",
                                                        s_break_line,
                                                        ufi.UploadFileName,
                                                        dr_detl["UnifiedBusinessNo"].ToString(),
                                                        dr_detl["StoreNo"].ToString(),
                                                        dr_detl["StoreName"].ToString(),
                                                        dr_detl["EFF_DATE_FROM"].ToString(),
                                                        dr_detl["EFF_DATE_TO"].ToString(),
                                                        dr_detl["IsTest"].ToString());
                    }

                    #endregion

                    #region 驗證日期格式

                    ei.Tag_D = $"檔案：[{ufi.UploadFileName}]，驗證檔案資料，驗證日期格式";
                    DateTime d_out;

                    var chk_data_02 = from x in var_chk_data
                                      where (DateTime.TryParseExact(x["EFF_DATE_FROM"].ToString(), "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out d_out) == false ||
                                             DateTime.TryParseExact(x["EFF_DATE_TO"].ToString(), "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out d_out) == false)
                                      select x;

                    foreach (var dr_detl in chk_data_02)
                    {
                        s_chk_str_detl += string.Format("{0}檔案：[{1}]，統編：{2}、門市代號：{3}、門市名稱：{4}、門市啟用日：{5}、門市停用日：{6}、測試門市：{7}，資料異常，[門市啟用日]、[門市停用日]需為合法日期格式",
                                                        s_break_line,
                                                        ufi.UploadFileName,
                                                        dr_detl["UnifiedBusinessNo"].ToString(),
                                                        dr_detl["StoreNo"].ToString(),
                                                        dr_detl["StoreName"].ToString(),
                                                        dr_detl["EFF_DATE_FROM"].ToString(),
                                                        dr_detl["EFF_DATE_TO"].ToString(),
                                                        dr_detl["IsTest"].ToString());
                    }

                    #endregion

                    #region 驗證[門市啟用日]需小於等於[門市停用日]

                    ei.Tag_D = $"檔案：[{ufi.UploadFileName}]，驗證檔案資料，驗證[門市啟用日]需小於等於[門市停用日]";

                    var chk_data_03 = from x in var_chk_data
                                      where string.Compare(x["EFF_DATE_FROM"].ToString(), x["EFF_DATE_TO"].ToString()) == 1
                                      select x;

                    foreach (var dr_detl in chk_data_03)
                    {
                        s_chk_str_detl += string.Format("{0}檔案：[{1}]，統編：{2}、門市代號：{3}、門市名稱：{4}、門市啟用日：{5}、門市停用日：{6}、測試門市：{7}，資料異常，[門市啟用日]需小於等於[門市停用日]",
                                                        s_break_line,
                                                        ufi.UploadFileName,
                                                        dr_detl["UnifiedBusinessNo"].ToString(),
                                                        dr_detl["StoreNo"].ToString(),
                                                        dr_detl["StoreName"].ToString(),
                                                        dr_detl["EFF_DATE_FROM"].ToString(),
                                                        dr_detl["EFF_DATE_TO"].ToString(),
                                                        dr_detl["IsTest"].ToString());
                    }

                    #endregion

                    #region 驗證同一個[統編]+[門市代號]，[門市啟用日]不可重複

                    ei.Tag_D = $"檔案：[{ufi.UploadFileName}]，驗證檔案資料，驗證同一個[統編]+[門市代號]，[門市啟用日]不可重複";

                    var chk_data_04 = from x in var_chk_data
                                      group x by new
                                      {
                                          UnifiedBusinessNo = x["UnifiedBusinessNo"],
                                          StoreNo = x["StoreNo"],
                                          EFF_DATE_FROM = x["EFF_DATE_FROM"]
                                      } into xgb
                                      where xgb.Count() > 1
                                      select new
                                      {
                                          xgb.Key.UnifiedBusinessNo,
                                          xgb.Key.StoreNo,
                                          xgb.Key.EFF_DATE_FROM,
                                          Count = xgb.Count()
                                      };

                    foreach (var dr_detl in chk_data_04)
                    {
                        s_chk_str_detl += string.Format("{0}檔案：[{1}]，統編：{2}、門市代號：{3}、門市啟用日：{4}，有({5})筆資料，資料異常，同一個[統編]+[門市代號]，[門市啟用日]不可重複",
                                                        s_break_line,
                                                        ufi.UploadFileName,
                                                        dr_detl.UnifiedBusinessNo.ToString(),
                                                        dr_detl.StoreNo.ToString(),
                                                        dr_detl.EFF_DATE_FROM.ToString(),
                                                        dr_detl.Count.ToString());
                    }

                    #endregion

                    #region 驗證同一個[統編]+[門市代號]，[門市啟用日][門市停用日]區間不可重疊

                    ei.Tag_D = $"檔案：[{ufi.UploadFileName}]，驗證檔案資料，驗證同一個[統編]+[門市代號]，[門市啟用日][門市停用日]區間不可重疊";

                    var chk_data_05 = from x in var_chk_data
                                      join y in var_chk_data
                                      on new
                                      {
                                          UnifiedBusinessNo = x["UnifiedBusinessNo"].ToString(),
                                          StoreNo = x["StoreNo"].ToString()
                                      }
                                      equals new
                                      {
                                          UnifiedBusinessNo = y["UnifiedBusinessNo"].ToString(),
                                          StoreNo = y["StoreNo"].ToString()
                                      }
                                      where ((x["EFF_DATE_FROM"].ToString() + x["EFF_DATE_TO"].ToString() != y["EFF_DATE_FROM"].ToString() + y["EFF_DATE_TO"].ToString()) &&
                                             ((string.Compare(y["EFF_DATE_FROM"].ToString(), x["EFF_DATE_FROM"].ToString()) >= 0 && string.Compare(y["EFF_DATE_FROM"].ToString(), x["EFF_DATE_TO"].ToString()) <= 0) ||
                                              (string.Compare(y["EFF_DATE_TO"].ToString(), x["EFF_DATE_FROM"].ToString()) >= 0 && string.Compare(y["EFF_DATE_TO"].ToString(), x["EFF_DATE_TO"].ToString()) <= 0) ||
                                              (string.Compare(y["EFF_DATE_FROM"].ToString(), x["EFF_DATE_FROM"].ToString()) < 0 && string.Compare(y["EFF_DATE_TO"].ToString(), x["EFF_DATE_TO"].ToString()) > 0)))
                                      select new { x, y };

                    foreach (var dr_detl in chk_data_05)
                    {
                        s_chk_str_detl += string.Format("{0}檔案：[{1}]，統編：{2}、門市代號：{3}、門市啟用日：{4}、門市停用日：{5}，資料異常，與其他資料日期區間重疊，重疊區間(門市啟用日：{6}、門市停用日：{7})",
                                                        s_break_line,
                                                        ufi.UploadFileName,
                                                        dr_detl.x["UnifiedBusinessNo"].ToString(),
                                                        dr_detl.x["StoreNo"].ToString(),
                                                        dr_detl.x["EFF_DATE_FROM"].ToString(),
                                                        dr_detl.x["EFF_DATE_TO"].ToString(),
                                                        dr_detl.y["EFF_DATE_FROM"].ToString(),
                                                        dr_detl.y["EFF_DATE_TO"].ToString());
                    }

                    #endregion

                    #region 驗證欄位[測試門市]，只有兩種值(0=非測試店、1=測試店)

                    ei.Tag_D = $"檔案：[{ufi.UploadFileName}]，驗證檔案資料，驗證欄位[測試門市]，只有兩種值(0=非測試店、1=測試店)";
                    var chk_data_06 = from x in var_chk_data
                                      where (x["IsTest"].ToString() != "0" &&
                                             x["IsTest"].ToString() != "1")
                                      select x;

                    foreach (var dr_detl in chk_data_06)
                    {
                        s_chk_str_detl += string.Format("{0}檔案：[{1}]，統編：{2}、門市代號：{3}、門市名稱：{4}、門市啟用日：{5}、門市停用日：{6}、測試門市：{7}，資料異常，欄位[測試門市]，只會有兩種值(0=非測試店、1=測試店)",
                                                        s_break_line,
                                                        ufi.UploadFileName,
                                                        dr_detl["UnifiedBusinessNo"].ToString(),
                                                        dr_detl["StoreNo"].ToString(),
                                                        dr_detl["StoreName"].ToString(),
                                                        dr_detl["EFF_DATE_FROM"].ToString(),
                                                        dr_detl["EFF_DATE_TO"].ToString(),
                                                        dr_detl["IsTest"].ToString());
                    }

                    #endregion

                    #region 驗證欄位[門市電話]、[傳真號碼]

                    ei.Tag_D = $"檔案：[{ufi.UploadFileName}]，驗證檔案資料，驗證欄位[門市電話]、[傳真號碼]";

                    Regex rg = new Regex(@"^\d{0,20}$");
                    var chk_data_07 = from x in var_chk_data
                                      where (rg.IsMatch(x["StorePhone"].ToString()) == false ||
                                             rg.IsMatch(x["StoreFax"].ToString()) == false)
                                      select x;

                    foreach (var dr_detl in chk_data_07)
                    {
                        s_chk_str_detl += string.Format("{0}檔案：[{1}]，統編：{2}、門市代號：{3}、門市名稱：{4}、門市啟用日：{5}、門市停用日：{6}、門市電話：{7}、傳真號碼：{8}，資料異常，欄位[門市電話]、[傳真號碼]需為數字，最多20個字，不需提供()-#等",
                                                        s_break_line,
                                                        ufi.UploadFileName,
                                                        dr_detl["UnifiedBusinessNo"].ToString(),
                                                        dr_detl["StoreNo"].ToString(),
                                                        dr_detl["StoreName"].ToString(),
                                                        dr_detl["EFF_DATE_FROM"].ToString(),
                                                        dr_detl["EFF_DATE_TO"].ToString(),
                                                        dr_detl["StorePhone"].ToString(),
                                                        dr_detl["StoreFax"].ToString());
                    }

                    #endregion

                    #region 驗證欄位[門市代號]長度

                    ei.Tag_D = "驗證檔案資料，驗證欄位[門市代號]長度";

                    var chk_data_08 = from x in var_chk_data
                                      where (Encoding.GetEncoding(950).GetByteCount(x["StoreNo"].ToString()) > 10)
                                      select new
                                      {
                                          x,
                                          Data_Length = Encoding.GetEncoding(950).GetByteCount(x["StoreNo"].ToString())
                                      };

                    foreach (var dr_detl in chk_data_08)
                    {
                        s_chk_str_detl += string.Format("{0}檔案：[{1}]，統編：{2}、門市代號：{3}、門市名稱：{4}、門市啟用日：{5}、門市停用日：{6}，資料異常，欄位[門市代號]長度({7})，最多10個Byte(中文2個Byte、英數1個Byte)",
                                                        s_break_line,
                                                        ufi.UploadFileName,
                                                        dr_detl.x["UnifiedBusinessNo"].ToString(),
                                                        dr_detl.x["StoreNo"].ToString(),
                                                        dr_detl.x["StoreName"].ToString(),
                                                        dr_detl.x["EFF_DATE_FROM"].ToString(),
                                                        dr_detl.x["EFF_DATE_TO"].ToString(),
                                                        dr_detl.Data_Length.ToString());
                    }

                    #endregion

                    #region 驗證欄位[門市名稱]長度

                    ei.Tag_D = $"檔案：[{ufi.UploadFileName}]，驗證檔案資料，驗證欄位[門市名稱]長度";

                    /*
                     * 此欄位，DB是使用NVARCHAR，可以輸入難字
                     * 不管中文、英文、數字，都是算一個字
                     */

                    var chk_data_09 = from x in var_chk_data
                                      where (x["StoreName"].ToString().Length > 20)
                                      select new
                                      {
                                          x,
                                          Data_Length = x["StoreName"].ToString().Length
                                      };

                    foreach (var dr_detl in chk_data_09)
                    {
                        s_chk_str_detl += string.Format("{0}檔案：[{1}]，統編：{2}、門市代號：{3}、門市名稱：{4}、門市啟用日：{5}、門市停用日：{6}，資料異常，欄位[門市名稱]長度({7})，最多20個字(中文、英文、數字都算一個字)",
                                                   s_break_line,
                                                   ufi.UploadFileName,
                                                   dr_detl.x["UnifiedBusinessNo"].ToString(),
                                                   dr_detl.x["StoreNo"].ToString(),
                                                   dr_detl.x["StoreName"].ToString(),
                                                   dr_detl.x["EFF_DATE_FROM"].ToString(),
                                                   dr_detl.x["EFF_DATE_TO"].ToString(),
                                                   dr_detl.Data_Length.ToString());
                    }

                    #endregion

                    #region 驗證欄位[郵遞區號]長度

                    ei.Tag_D = $"檔案：[{ufi.UploadFileName}]，驗證檔案資料，驗證欄位[郵遞區號]長度";

                    var chk_data_10 = from x in var_chk_data
                                      where (Encoding.GetEncoding(950).GetByteCount(x["ZipCode"].ToString()) > 20)
                                      select new
                                      {
                                          x,
                                          Data_Length = Encoding.GetEncoding(950).GetByteCount(x["ZipCode"].ToString())
                                      };

                    foreach (var dr_detl in chk_data_10)
                    {
                        s_chk_str_detl += string.Format("{0}檔案：[{1}]，統編：{2}、門市代號：{3}、門市名稱：{4}、門市啟用日：{5}、門市停用日：{6}、郵遞區號：{7}，資料異常，欄位[郵遞區號]長度({8})，最多20個Byte(中文2個Byte、英數1個Byte)",
                                                        s_break_line,
                                                        ufi.UploadFileName,
                                                        dr_detl.x["UnifiedBusinessNo"].ToString(),
                                                        dr_detl.x["StoreNo"].ToString(),
                                                        dr_detl.x["StoreName"].ToString(),
                                                        dr_detl.x["EFF_DATE_FROM"].ToString(),
                                                        dr_detl.x["EFF_DATE_TO"].ToString(),
                                                        dr_detl.x["ZipCode"].ToString(),
                                                        dr_detl.Data_Length.ToString());
                    }

                    #endregion

                    #region 驗證欄位[門市地址]長度

                    ei.Tag_D = $"檔案：[{ufi.UploadFileName}]，驗證檔案資料，驗證欄位[門市地址]長度";

                    /*
                     * 此欄位，DB是使用NVARCHAR，可以輸入難字
                     * 不管中文、英文、數字，都是算一個字
                     */

                    var chk_data_11 = from x in var_chk_data
                                      where (x["StoreAddress"].ToString().Length > 200)
                                      select new
                                      {
                                          x,
                                          Data_Length = x["StoreAddress"].ToString().Length
                                      };

                    foreach (var dr_detl in chk_data_11)
                    {
                        s_chk_str_detl += string.Format("{0}檔案：[{1}]，統編：{2}、門市代號：{3}、門市名稱：{4}、門市啟用日：{5}、門市停用日：{6}、門市地址：{7}，資料異常，欄位[門市地址]長度({8})，最多200個字(中文、英文、數字都算一個字)",
                                                        s_break_line,
                                                        ufi.UploadFileName,
                                                        dr_detl.x["UnifiedBusinessNo"].ToString(),
                                                        dr_detl.x["StoreNo"].ToString(),
                                                        dr_detl.x["StoreName"].ToString(),
                                                        dr_detl.x["EFF_DATE_FROM"].ToString(),
                                                        dr_detl.x["EFF_DATE_TO"].ToString(),
                                                        dr_detl.x["StoreAddress"].ToString(),
                                                        dr_detl.Data_Length.ToString());
                    }

                    #endregion

                    #region 以上驗證(2)若有異常，則不繼續

                    ei.Tag_D = $"檔案：[{ufi.UploadFileName}]，以上驗證(2)若有異常，則不繼續";
                    if (string.IsNullOrEmpty(s_chk_str_detl) == false)
                    {
                        s_chk_str += $"{s_break_line}{new string('=', 50)}{s_chk_str_detl}";
                        continue;
                    }

                    #endregion

                }//foreach結束

                #endregion

                #region 驗證檔案資料有異常

                ei.Tag_D = "驗證檔案資料有異常";
                if (string.IsNullOrEmpty(s_chk_str) == false)
                {
                    throw new Exception(s_chk_str);
                }

                #endregion

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
        /// 將匯入檔案資料寫入資料庫
        /// </summary>
        /// <param name="ei">執行訊息</param>
        /// <param name="taskInfo">作業資訊</param>
        /// <param name="li_ifi">匯入檔案資訊</param>
        /// <param name="dt_data">匯入檔案內容</param>
        public void import_MerchantStoreData(ref ExecInfo ei,
                                             TaskInfo taskInfo,
                                             ref List<ImportFileInfo> li_ifi,
                                             DataTable dt_data)
        {
            icashopMerchantStoreDAO.import_MerchantStoreData(ref ei,
                                                             taskInfo,
                                                             ref li_ifi,
                                                             dt_data);
        }
    }
}
