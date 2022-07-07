using System;
using System.Data;
using System.Collections.Generic;
using System.Drawing;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Linq;

using DataAccess;
using Domain.Common;
using Domain.CoBrandedSellSponsorship;

using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace Business
{
    public class CoBrandedSellSponsorshipManager
    {
        #region 設定全域變數
        private CoBrandedSellSponsorshipDAO _coBrandedSellSponsorshipDAO { get; set; }
        #endregion

        #region 建構子
        public CoBrandedSellSponsorshipManager()
        {
            _coBrandedSellSponsorshipDAO = new CoBrandedSellSponsorshipDAO();
        }
        #endregion

        /// <summary>
        /// 取得聯名卡行銷贊助金年度資料
        /// </summary>
        /// <param name="ei">執行訊息</param>
        /// <returns></returns>
        public DataTable get_CBSS_RangeData(ref ExecInfo ei)
        {
            return _coBrandedSellSponsorshipDAO.get_CBSS_RangeData(ref ei);
        }

        /// <summary>
        /// 取得聯名卡行銷贊助金年度區間銀行清單
        /// </summary>
        /// <param name="ei">執行訊息</param>
        /// <param name="qbdrq">查詢條件</param>
        /// <returns></returns>
        public DataTable get_CBSS_BankData(ref ExecInfo ei,
                                           QueryBankDataReq qbdrq)
        {
            return _coBrandedSellSponsorshipDAO.get_CBSS_BankData(ref ei,
                                                                  qbdrq);
        }

        /// <summary>
        /// 聯名卡行銷贊助金年度區間資料存檔
        /// </summary>
        /// <param name="ei">執行訊息</param>
        /// <param name="srdrq">存檔資訊</param>
        public void save_CBSS_RangeData(ref ExecInfo ei,
                                        SaveRangeDataReq srdrq)
        {
            #region 設定tag
            ei.Tag_M = "Manager，聯名卡行銷贊助金年度區間資料存檔";
            #endregion

            #region 設定變數

            ei.Tag_D = "設定變數";
            XDocument doc_data = new XDocument();
            var xmlSerializer = new XmlSerializer(typeof(SaveRangeDataReq));

            #endregion

            try
            {
                ei.Tag_D = "將存檔資訊物件轉換為XML";
                using (var writer = doc_data.CreateWriter())
                {
                    xmlSerializer.Serialize(writer, srdrq);
                }

                ei.Tag_D = "連線DB存檔";
                _coBrandedSellSponsorshipDAO.save_CBSS_RangeData(ref ei,
                                                                 doc_data);
            }
            catch (Exception ex)
            {
                #region 執行失敗
                ei.RtnResult = false;
                ei.RtnMsg = ei.fun_combine_err_msg(ex.Message);
                #endregion
            }
        }

        /// <summary>
        /// 取得聯名卡行銷贊助金銀行特店清單
        /// </summary>
        /// <param name="ei">執行訊息</param>
        /// <returns></returns>
        public DataTable get_CBSS_BankMerchantData(ref ExecInfo ei)
        {
            return _coBrandedSellSponsorshipDAO.get_CBSS_BankMerchantData(ref ei);
        }

        /// <summary>
        /// 聯名卡行銷贊助金銀行特約機構資料存檔
        /// </summary>
        /// <param name="ei">執行訊息</param>
        /// <param name="srdrq">存檔資訊</param>
        public void save_CBSS_BankData(ref ExecInfo ei,
                                       SaveBankDataReq sbdrq)
        {
            #region 設定tag
            ei.Tag_M = "Manager，聯名卡行銷贊助金銀行特約機構資料存檔";
            #endregion

            #region 設定變數

            ei.Tag_D = "設定變數";
            XDocument doc_data = new XDocument();
            var xmlSerializer = new XmlSerializer(typeof(SaveBankDataReq));

            #endregion

            try
            {
                ei.Tag_D = "將存檔資訊物件轉換為XML";
                using (var writer = doc_data.CreateWriter())
                {
                    xmlSerializer.Serialize(writer, sbdrq);
                }

                ei.Tag_D = "連線DB存檔";
                _coBrandedSellSponsorshipDAO.save_CBSS_BankData(ref ei,
                                                                 doc_data);
            }
            catch (Exception ex)
            {
                #region 執行失敗
                ei.RtnResult = false;
                ei.RtnMsg = ei.fun_combine_err_msg(ex.Message);
                #endregion
            }
        }

        /// <summary>
        /// 取得聯名卡行銷贊助金銀行明細
        /// </summary>
        /// <param name="ei">執行訊息</param>
        /// <param name="qbdrq">查詢條件</param>
        /// <returns></returns>
        public DataSet get_CBSS_QuotaTrack(ref ExecInfo ei,
                                           QueryBankDataReq qbdrq)
        {
            return _coBrandedSellSponsorshipDAO.get_CBSS_QuotaTrack(ref ei,
                                                                    qbdrq,
                                                                    null);
        }

        /// <summary>
        /// 取得聯名卡行銷贊助金支付對象明細
        /// </summary>
        /// <param name="ei">執行訊息</param>
        /// <param name="qbdrq">查詢條件</param>
        /// <returns></returns>
        public DataTable get_CBSS_PayTarget(ref ExecInfo ei)
        {
            return _coBrandedSellSponsorshipDAO.get_CBSS_PayTarget(ref ei);
        }

        /// <summary>
        /// 聯名卡行銷贊助金支付對象存檔
        /// </summary>
        /// <param name="ei">執行訊息</param>
        /// <param name="sptrq">存檔資訊</param>
        public void save_CBSS_PayTarget(ref ExecInfo ei,
                                        SavePayTargetReq sptrq)
        {
            #region 設定tag
            ei.Tag_M = "Manager，聯名卡行銷贊助金支付對象存檔";
            #endregion

            #region 設定變數

            ei.Tag_D = "設定變數";
            XDocument doc_data = new XDocument();
            var xmlSerializer = new XmlSerializer(typeof(SavePayTargetReq));

            #endregion

            try
            {
                ei.Tag_D = "將存檔資訊物件轉換為XML";
                using (var writer = doc_data.CreateWriter())
                {
                    xmlSerializer.Serialize(writer, sptrq);
                }

                ei.Tag_D = "連線DB存檔";
                _coBrandedSellSponsorshipDAO.save_CBSS_PayTarget(ref ei,
                                                                 doc_data);
            }
            catch (Exception ex)
            {
                #region 執行失敗
                ei.RtnResult = false;
                ei.RtnMsg = ei.fun_combine_err_msg(ex.Message);
                #endregion
            }
        }

        /// <summary>
        /// 聯名卡行銷贊助金額度存檔
        /// </summary>
        /// <param name="ei">執行訊息</param>
        /// <param name="sdqreq">存檔資訊</param>
        public void save_CBSS_DetailQuota(ref ExecInfo ei,
                                          SaveDetailQuotaReq sdqreq)
        {
            #region 設定tag
            ei.Tag_M = "Manager，聯名卡行銷贊助金額度存檔";
            #endregion

            #region 設定變數

            ei.Tag_D = "設定變數";
            XDocument doc_data = new XDocument();
            var xmlSerializer = new XmlSerializer(typeof(SaveDetailQuotaReq));

            #endregion

            try
            {
                ei.Tag_D = "將存檔資訊物件轉換為XML";
                using (var writer = doc_data.CreateWriter())
                {
                    xmlSerializer.Serialize(writer, sdqreq);
                }

                ei.Tag_D = "連線DB存檔";
                _coBrandedSellSponsorshipDAO.save_CBSS_DetailQuota(ref ei,
                                                                   doc_data);
            }
            catch (Exception ex)
            {
                #region 執行失敗
                ei.RtnResult = false;
                ei.RtnMsg = ei.fun_combine_err_msg(ex.Message);
                #endregion
            }
        }

        /// <summary>
        /// 聯名卡行銷贊助金請款項目存檔
        /// </summary>
        /// <param name="ei">執行訊息</param>
        /// <param name="sdtreq">存檔資訊</param>
        public void save_CBSS_DetailTrack(ref ExecInfo ei,
                                          SaveDetailTrackReq sdtreq)
        {
            #region 設定tag
            ei.Tag_M = "Manager，聯名卡行銷贊助金請款項目存檔";
            #endregion

            #region 設定變數

            ei.Tag_D = "設定變數";
            XDocument doc_data = new XDocument();
            var xmlSerializer = new XmlSerializer(typeof(SaveDetailTrackReq));

            #endregion

            try
            {
                ei.Tag_D = "將存檔資訊物件轉換為XML";
                using (var writer = doc_data.CreateWriter())
                {
                    xmlSerializer.Serialize(writer, sdtreq);
                }

                ei.Tag_D = "連線DB存檔";
                _coBrandedSellSponsorshipDAO.save_CBSS_DetailTrack(ref ei,
                                                                   doc_data);
            }
            catch (Exception ex)
            {
                #region 執行失敗
                ei.RtnResult = false;
                ei.RtnMsg = ei.fun_combine_err_msg(ex.Message);
                #endregion
            }
        }

        /// <summary>
        /// 聯名卡行銷贊助金匯出
        /// </summary>
        /// <param name="ei"></param>
        /// <param name="qbdrq"></param>
        /// <param name="doc_data"></param>
        public void export_CBSS_Data(ref ExecInfo ei,
                                     QueryBankDataReq qbdrq,
                                     XDocument doc_data,
                                     out byte[] rtn_byte_data,
                                     out string rtn_filename)
        {
            #region 設定tag
            ei.Tag_M = "Manager，聯名卡行銷贊助金匯出";
            #endregion

            #region 設定變數

            ei.Tag_D = "設定變數";
            DataSet ds_CBSS_Data = new DataSet();
            rtn_byte_data = null;
            rtn_filename = string.Empty;

            #endregion

            try
            {
                #region 取得聯名卡行銷贊助金資料

                ei.Tag_D = "取得聯名卡行銷贊助金資料";
                ds_CBSS_Data = _coBrandedSellSponsorshipDAO.get_CBSS_QuotaTrack(ref ei,
                                                                                qbdrq,
                                                                                doc_data);

                if (ei.RtnResult == false)
                { throw new CustomException.ExecErr(ei.RtnMsg); }

                #endregion

                #region 產出Excel

                ei.Tag_D = "產出Excel";

                ExcelPackage ep = new ExcelPackage();

                DataTable dt_BankData = ds_CBSS_Data.Tables[0];
                DataTable dt_Quota = ds_CBSS_Data.Tables[1];
                DataTable dt_Track = ds_CBSS_Data.Tables[2];

                foreach (DataRow dr in dt_BankData.Rows)
                {
                    #region 設定變數

                    int i_Row_Quota_Head = 7;//額度表頭，起始行(固定)
                    int i_Row_Quota_Data = 8;//額度資料，起始行(固定)

                    int i_Row_Track_Head1 = 0;//額度表頭1，起始行(需依據額度資料往下加)
                    int i_Row_Track_Head2 = 0;//額度表頭1，起始行(需依據額度資料往下加)
                    int i_Row_Track_Data = 0;//額度資料，起始行(需依據額度資料往下加)


                    string s_Bank = string.Empty;//銀行
                    string s_Range = string.Empty;//年度區間
                    int i_AMT_Quota = 0;//可使用額度
                    int i_AMT_Track = 0;//已使用額度
                    int i_AMT_UnUse = 0;//尚未請款數

                    #endregion

                    #region 整理資料

                    s_Bank = dr["MERCHANT_STNAME"].ToString();
                    s_Range = string.Format("{0}~{1}",
                                            (qbdrq.Date_B.HasValue == false ? string.Empty : qbdrq.Date_B.Value.ToString("yyyy/MM/dd")),
                                            (qbdrq.Date_E.HasValue == false ? string.Empty : qbdrq.Date_E.Value.ToString("yyyy/MM/dd")));
                    i_AMT_Quota = int.Parse(dr["AMT_Quota"].ToString());
                    i_AMT_Track = int.Parse(dr["AMT_Track"].ToString());
                    i_AMT_UnUse = int.Parse(dr["AMT_UnUse"].ToString());

                    //額度明細資料
                    List<cls_Quota> li_Quota = (from x in dt_Quota.AsEnumerable()
                                                orderby x["CreateDate"]
                                                where x["BID"].ToString() == dr["BID"].ToString()
                                                select x)
                                               .Select((data, index) => new cls_Quota
                                               {
                                                   Id = index + 1,
                                                   ITEM = data["ITEM"].ToString(),
                                                   ITEM2 = string.Empty,
                                                   AMT = int.Parse(data["AMT"].ToString())
                                               }).ToList();

                    //請款項目資料
                    List<cls_Track> li_Track = (from x in dt_Track.AsEnumerable()
                                                orderby x["CreateDate"]
                                                where x["BID"].ToString() == dr["BID"].ToString()
                                                select x)
                                               .Select((data, index) => new cls_Track
                                               {
                                                   Id = index + 1,
                                                   ITEM = data["ITEM"].ToString(),
                                                   ITEM2 = string.Empty,
                                                   PayTarget = string.Format("{0}({1})",
                                                                             data["UnifiedBusinessNo"].ToString(),
                                                                             data["PayTargetName"].ToString()),
                                                   Range_Date = ((data["Range_Date_B"] == DBNull.Value && data["Range_Date_B"] == DBNull.Value) == true ?
                                                                 string.Empty :
                                                                 string.Format("{0}~{1}",
                                                                               (data["Range_Date_B"] == DBNull.Value ? string.Empty : DateTime.Parse(data["Range_Date_B"].ToString()).ToString("yyyy/MM/dd")),
                                                                               (data["Range_Date_E"] == DBNull.Value ? string.Empty : DateTime.Parse(data["Range_Date_E"].ToString()).ToString("yyyy/MM/dd")))),
                                                   Bus_Invoice_Date = (data["Bus_Invoice_Date"] == DBNull.Value ? string.Empty : DateTime.Parse(data["Bus_Invoice_Date"].ToString()).ToString("yyyy/MM/dd")),
                                                   Bus_Invoice_No = data["Bus_Invoice_No"].ToString(),
                                                   AMT_UnTax = int.Parse(data["AMT_UnTax"].ToString()),
                                                   AMT_TaxIncluded = int.Parse(data["AMT_TaxIncluded"].ToString()),
                                                   Comment = data["Comment"].ToString(),
                                                   SendDate = (data["SendDate"] == DBNull.Value ? string.Empty : DateTime.Parse(data["SendDate"].ToString()).ToString("yyyy/MM/dd")),
                                                   Act_Invoice_No = data["Act_Invoice_No"].ToString(),
                                                   AMT_Pay = (data["AMT_Pay"] == DBNull.Value ? 0 : int.Parse(data["AMT_Pay"].ToString()))
                                               }).ToList();

                    #endregion

                    #region 設定行數

                    i_Row_Track_Head1 = i_Row_Quota_Data + 1 + li_Quota.Count;
                    i_Row_Track_Head2 = i_Row_Track_Head1 + 1;
                    i_Row_Track_Data = i_Row_Track_Head2 + 1;

                    #endregion

                    //建立Sheet
                    string s_SheetName = string.Format("{0}({1})",
                                                       dr["MERCHANT_NO"].ToString(),
                                                       dr["MERCHANT_STNAME"].ToString());
                    ep.Workbook.Worksheets.Add(s_SheetName);

                    //讀取Sheet
                    ExcelWorksheet exec_sheet = ep.Workbook.Worksheets[s_SheetName];

                    //銀行相關資訊-設定表頭
                    exec_sheet.Cells[1, 1].Value = "銀行別";
                    exec_sheet.Cells[1, 2].Value = s_Bank;
                    exec_sheet.Cells[1, 2].Style.Font.Bold = true;
                    exec_sheet.Cells[1, 2].Style.Font.Color.SetColor(Color.Red);
                    exec_sheet.Cells[2, 1].Value = s_Range;
                    exec_sheet.Cells[2, 2].Value = "未稅";
                    exec_sheet.Cells[3, 1].Value = "可使用額度";
                    exec_sheet.Cells[3, 2].Value = i_AMT_Quota;
                    exec_sheet.Cells[4, 1].Value = "已使用額度";
                    exec_sheet.Cells[4, 2].Value = i_AMT_Track;
                    exec_sheet.Cells[5, 1].Value = "尚未請款數";
                    exec_sheet.Cells[5, 1].Style.Font.Bold = true;
                    exec_sheet.Cells[5, 2].Value = i_AMT_UnUse;
                    exec_sheet.Cells[5, 2].Style.Font.Bold = true;
                    exec_sheet.Cells[1, 1, 2, 2].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    exec_sheet.Cells[1, 1, 2, 2].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                    exec_sheet.Cells[3, 1, 4, 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    exec_sheet.Cells[3, 1, 4, 1].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                    exec_sheet.Cells[5, 1, 5, 2].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    exec_sheet.Cells[5, 1, 5, 2].Style.Fill.BackgroundColor.SetColor(Color.Pink);

                    //銀行相關資訊-凍結視窗
                    exec_sheet.View.FreezePanes(6, 1);

                    //銀行相關資訊-設定框線
                    exec_sheet.Cells[1, 1, 5, 2].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;//下框線
                    exec_sheet.Cells[1, 1, 5, 2].Style.Border.Top.Style = ExcelBorderStyle.Thin;//上框線
                    exec_sheet.Cells[1, 1, 5, 2].Style.Border.Right.Style = ExcelBorderStyle.Thin;//右框線
                    exec_sheet.Cells[1, 1, 5, 2].Style.Border.Left.Style = ExcelBorderStyle.Thin;//左框線

                    //銀行相關資訊-設定欄位格式
                    exec_sheet.Cells[3, 2, 5, 2].Style.Numberformat.Format = "$#,##0_);[Red]($#,##0)";

                    //額度明細-設定表頭
                    exec_sheet.Cells[i_Row_Quota_Head, 1].Value = "項次";
                    exec_sheet.Cells[i_Row_Quota_Head, 2, i_Row_Quota_Head, 3].Merge = true;
                    exec_sheet.Cells[i_Row_Quota_Head, 2, i_Row_Quota_Head, 3].Value = "額度項目";
                    exec_sheet.Cells[i_Row_Quota_Head, 4].Value = "金額";
                    exec_sheet.Cells[i_Row_Quota_Head, 1, i_Row_Quota_Head, 4].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    exec_sheet.Cells[i_Row_Quota_Head, 1, i_Row_Quota_Head, 4].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                    exec_sheet.Cells[i_Row_Quota_Head, 1, i_Row_Quota_Head, 4].Style.Font.Bold = true;//粗體

                    //額度明細-合併欄位[項次]
                    for (int i = 0; i < li_Quota.Count; i++)
                    {
                        int i_Quota_Merge = i_Row_Quota_Data + i;
                        exec_sheet.Cells[i_Quota_Merge, 2, i_Quota_Merge, 3].Merge = true;
                    }

                    //額度明細-寫入資料
                    exec_sheet.Cells[i_Row_Quota_Data, 1].LoadFromCollection(li_Quota, false);

                    //額度明細-設定欄位格式
                    int i_i_Row_Quota_Data_End = (i_Row_Quota_Data + li_Quota.Count - 1);//額度明細結束行數
                    if (li_Quota.Count > 0)
                    {
                        exec_sheet.Cells[i_Row_Quota_Data, 3, i_i_Row_Quota_Data_End, 4].Style.Numberformat.Format = "$#,##0_);[Red]($#,##0)";
                    }

                    //額度明細-設定框線
                    exec_sheet.Cells[i_Row_Quota_Head, 1, i_i_Row_Quota_Data_End, 4].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;//下框線
                    exec_sheet.Cells[i_Row_Quota_Head, 1, i_i_Row_Quota_Data_End, 4].Style.Border.Top.Style = ExcelBorderStyle.Thin;//上框線
                    exec_sheet.Cells[i_Row_Quota_Head, 1, i_i_Row_Quota_Data_End, 4].Style.Border.Right.Style = ExcelBorderStyle.Thin;//右框線
                    exec_sheet.Cells[i_Row_Quota_Head, 1, i_i_Row_Quota_Data_End, 4].Style.Border.Left.Style = ExcelBorderStyle.Thin;//左框線

                    //請款項目-設定表頭
                    exec_sheet.Cells[i_Row_Track_Head1, 1, i_Row_Track_Head1, 11].Merge = true;
                    exec_sheet.Cells[i_Row_Track_Head1, 1, i_Row_Track_Head1, 11].Value = "營業編輯";
                    exec_sheet.Cells[i_Row_Track_Head1, 12, i_Row_Track_Head1, 13].Merge = true;
                    exec_sheet.Cells[i_Row_Track_Head1, 12, i_Row_Track_Head1, 13].Value = "財會入帳";
                    exec_sheet.Cells[i_Row_Track_Head2, 1].Value = "項次";
                    exec_sheet.Cells[i_Row_Track_Head2, 2, i_Row_Track_Head2, 3].Merge = true;
                    exec_sheet.Cells[i_Row_Track_Head2, 2, i_Row_Track_Head2, 3].Value = "請款項目";
                    exec_sheet.Cells[i_Row_Track_Head2, 4].Value = "支付對象";
                    exec_sheet.Cells[i_Row_Track_Head2, 5].Value = "計算期間";
                    exec_sheet.Cells[i_Row_Track_Head2, 6].Value = "發票日期";
                    exec_sheet.Cells[i_Row_Track_Head2, 7].Value = "發票號碼";
                    exec_sheet.Cells[i_Row_Track_Head2, 8].Value = "金額(未稅)";
                    exec_sheet.Cells[i_Row_Track_Head2, 9].Value = "金額(含稅)";
                    exec_sheet.Cells[i_Row_Track_Head2, 10].Value = "備註";
                    exec_sheet.Cells[i_Row_Track_Head2, 11].Value = "送件日";
                    exec_sheet.Cells[i_Row_Track_Head2, 12].Value = "發票號碼";
                    exec_sheet.Cells[i_Row_Track_Head2, 13].Value = "入帳數";
                    exec_sheet.Cells[i_Row_Track_Head1, 1, i_Row_Track_Head2, 13].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    exec_sheet.Cells[i_Row_Track_Head1, 1, i_Row_Track_Head2, 13].Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                    exec_sheet.Cells[i_Row_Track_Head1, 1, i_Row_Track_Head2, 13].Style.Font.Bold = true;//粗體

                    //請款項目-合併欄位[項次]
                    for (int i = 0; i < li_Track.Count; i++)
                    {
                        int i_Track_Merge = i_Row_Track_Data + i;
                        exec_sheet.Cells[i_Track_Merge, 2, i_Track_Merge, 3].Merge = true;
                    }

                    //請款項目-寫入資料
                    exec_sheet.Cells[i_Row_Track_Data, 1].LoadFromCollection(li_Track, false);

                    //請款項目-設定欄位格式
                    int i_i_Row_Track_Data_End = (i_Row_Track_Data + li_Track.Count - 1);//請款項目結束行數
                    if (li_Track.Count > 0)
                    {
                        exec_sheet.Cells[i_Row_Track_Data, 8, i_i_Row_Track_Data_End, 9].Style.Numberformat.Format = "$#,##0_);[Red]($#,##0)";
                        exec_sheet.Cells[i_Row_Track_Data, 13, i_i_Row_Track_Data_End, 13].Style.Numberformat.Format = "$#,##0_);[Red]($#,##0)";
                    }

                    //請款項目-設定框線
                    exec_sheet.Cells[i_Row_Track_Head1, 1, i_i_Row_Track_Data_End, 13].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;//下框線
                    exec_sheet.Cells[i_Row_Track_Head1, 1, i_i_Row_Track_Data_End, 13].Style.Border.Top.Style = ExcelBorderStyle.Thin;//上框線
                    exec_sheet.Cells[i_Row_Track_Head1, 1, i_i_Row_Track_Data_End, 13].Style.Border.Right.Style = ExcelBorderStyle.Thin;//右框線
                    exec_sheet.Cells[i_Row_Track_Head1, 1, i_i_Row_Track_Data_End, 13].Style.Border.Left.Style = ExcelBorderStyle.Thin;//左框線

                    //設定字型
                    exec_sheet.Cells[1, 1, i_Row_Track_Data + li_Track.Count, 13].Style.Font.Name = "微軟正黑體";
                    exec_sheet.Cells[1, 1, i_Row_Track_Data + li_Track.Count, 13].Style.Font.Size = 12;
                    exec_sheet.Cells[1, 2].Style.Font.Size = 20;//銀行字體

                    //設定對齊
                    exec_sheet.Cells[1, 1, i_Row_Track_Data + li_Track.Count, 13].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    //自動調整欄位大小
                    exec_sheet.Cells[1, 1, i_Row_Track_Data + li_Track.Count, 13].AutoFitColumns();

                    //因應[額度項目][請款項目]欄位資料比較多，但是這兩個欄位是合併儲存格，無法自動調整欄位大小，因此固定設定寬一點
                    exec_sheet.Column(2).Width = 20;
                    exec_sheet.Column(3).Width = 80;

                }

                //設定Excel檔名
                rtn_filename = string.Format("聯名卡行銷贊助金_{0}~{1}_{2}.xlsx",
                                              (qbdrq.Date_B.HasValue == false ? string.Empty : qbdrq.Date_B.Value.ToString("yyyy-MM-dd")),
                                              (qbdrq.Date_E.HasValue == false ? string.Empty : qbdrq.Date_E.Value.ToString("yyyy-MM-dd")),
                                              DateTime.Now.ToString("yyyyMMddHHmmssfff"));

                //將Excel轉換成byte
                rtn_byte_data = ep.GetAsByteArray();

                #endregion

                #region 執行成功
                ei.RtnResult = true;
                #endregion
            }
            catch (Exception ex)
            {
                #region 執行失敗
                ei.RtnResult = false;
                ei.RtnMsg = ei.fun_combine_err_msg(ex.Message);
                #endregion
            }
        }

        #region 自訂匯出Class

        //額度
        public class cls_Quota
        {
            /// <summary>
            /// 項次
            /// </summary>
            public int Id { get; set; }
            /// <summary>
            /// 額度項目
            /// </summary>
            public string ITEM { get; set; }
            /// <summary>
            /// 額度項目(合併儲存格用)
            /// </summary>
            public string ITEM2 { get; set; }
            /// <summary>
            /// 金額
            /// </summary>
            public int AMT { get; set; }
        }

        //請款項目
        public class cls_Track
        {
            /// <summary>
            /// 項次
            /// </summary>
            public int Id { get; set; }
            /// <summary>
            /// 請款項目
            /// </summary>
            public string ITEM { get; set; }
            /// <summary>
            /// 請款項目(合併儲存格用)
            /// </summary>
            public string ITEM2 { get; set; }
            /// <summary>
            /// 支付對象
            /// </summary>
            public string PayTarget { get; set; }
            /// <summary>
            /// 計算期間
            /// </summary>
            public string Range_Date { get; set; }
            /// <summary>
            /// 發票日期(營業)
            /// </summary>
            public string Bus_Invoice_Date { get; set; }
            /// <summary>
            /// 發票號碼(營業)
            /// </summary>
            public string Bus_Invoice_No { get; set; }
            /// <summary>
            /// 金額(未稅)
            /// </summary>
            public int AMT_UnTax { get; set; }
            /// <summary>
            /// 金額(含稅)
            /// </summary>
            public int AMT_TaxIncluded { get; set; }
            /// <summary>
            /// 備註
            /// </summary>
            public string Comment { get; set; }
            /// <summary>
            /// 送件日
            /// </summary>
            public string SendDate { get; set; }
            /// <summary>
            /// 發票號碼(財會)
            /// </summary>
            public string Act_Invoice_No { get; set; }
            /// <summary>
            /// 入帳數
            /// </summary>
            public int AMT_Pay { get; set; }
        }

        #endregion





    }
}
