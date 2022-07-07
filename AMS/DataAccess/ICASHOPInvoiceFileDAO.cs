using Common.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class ICASHOPInvoiceFileDAO : BasicOPDAL
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ICASHOPInvoiceFileDAO));
        public ICASHOPInvoiceFileDAO()
        {

        }

        public DataTable GetInvoiceData(string TARGET_MONTH, string TARGET_DateType, string Group_TaiYa, string Group_TaiSu, string Group_XiOu, string Group_FuMou, string Group_TongYiDuJiaCun)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    #region SQL
                    string sqlText = string.Concat(new object[] {
                        "DECLARE @TARGET_MONTH VARCHAR(7) = @EXEC_TARGET_MONTH",Environment.NewLine,
                        "DECLARE @Group_TaiYa          VARCHAR(1) = @EXEC_Group_TaiYa --台亞",Environment.NewLine,
                        "DECLARE @Group_TaiSu          VARCHAR(1) = @EXEC_Group_TaiSu --台塑",Environment.NewLine,
                        "DECLARE @Group_XiOu           VARCHAR(1) = @EXEC_Group_XiOu  --西歐",Environment.NewLine,
                        "DECLARE @Group_FuMou          VARCHAR(1) = @EXEC_Group_FuMou --福懋",Environment.NewLine,
                        "DECLARE @Group_TongYiDuJiaCun VARCHAR(1) = @EXEC_Group_TongYiDuJiaCun --統一渡假村",Environment.NewLine,
                        "DECLARE @TARGET_DateType VARCHAR(10) = @EXEC_TARGET_DateType",Environment.NewLine,
                        "DECLARE @GenerateDate VARCHAR(8) = CONVERT(VARCHAR(8),GETDATE(),112)",Environment.NewLine,
                        "SELECT RIGHT('000'+CONVERT(VARCHAR(4),ROW_NUMBER() OVER(ORDER BY UnifiedBusinessNo,POINT_TYPE)),4) [單據編號]",Environment.NewLine,
                        "	,@GenerateDate [日期]",Environment.NewLine,
                        "	,UnifiedBusinessNo [客戶代號]",Environment.NewLine,
                        "	,CASE",Environment.NewLine,
                        "		WHEN MerchantName LIKE '%統一渡假村%' AND POINT_TYPE='Get' THEN CONVERT(NUMERIC,ROUND(TotalGetPoint*1.2,0))",Environment.NewLine,
                        "		WHEN POINT_TYPE='Get' AND @TARGET_MONTH<'202203' THEN CONVERT(NUMERIC,ROUND(TotalGetPoint*1.02,0))",Environment.NewLine,
                        "		WHEN POINT_TYPE='Get' AND @TARGET_MONTH>='202203' THEN CONVERT(NUMERIC,ROUND(TotalGetPoint*1.05,0))",Environment.NewLine,
                        "		WHEN POINT_TYPE='Redeem' THEN CONVERT(NUMERIC,ROUND(TotalRedeemPoint*0.05,0))",Environment.NewLine,
                        "	 END [未稅]",Environment.NewLine,
                        "	,CASE",Environment.NewLine,
                        "		WHEN MerchantName LIKE '%統一渡假村%' AND POINT_TYPE='Get' THEN CONVERT(NUMERIC,ROUND(TotalGetPoint*1.2*0.05,0))",Environment.NewLine,
                        "		WHEN POINT_TYPE='Get' AND @TARGET_MONTH<'202203' THEN CONVERT(NUMERIC,ROUND(TotalGetPoint*1.02*0.05,0))",Environment.NewLine,
                        "		WHEN POINT_TYPE='Get' AND @TARGET_MONTH>='202203' THEN CONVERT(NUMERIC,ROUND(TotalGetPoint*1.05*0.05,0))",Environment.NewLine,
                        "		WHEN POINT_TYPE='Redeem' THEN CONVERT(NUMERIC,ROUND(TotalRedeemPoint*0.05*0.05,0))",Environment.NewLine,
                        "	 END [稅額]",Environment.NewLine,
                        "	,CONVERT(VARCHAR(6),DATEADD(M,2,@TARGET_MONTH+'01'),112)+'25' [預計收款日]",Environment.NewLine,
                        "	,'' [發票號碼]",Environment.NewLine,
                        "	,'0001' [列號]",Environment.NewLine,
                        "	,'9' [類別]",Environment.NewLine,
                        "	,CASE",Environment.NewLine,
                        "		WHEN MerchantName LIKE '%統一渡假村%' AND POINT_TYPE='Get' THEN CONVERT(NUMERIC,ROUND(TotalGetPoint*1.2,0))",Environment.NewLine,
                        "		WHEN POINT_TYPE='Get' AND @TARGET_MONTH<'202203' THEN CONVERT(NUMERIC,ROUND(TotalGetPoint*1.02,0))",Environment.NewLine,
                        "		WHEN POINT_TYPE='Get' AND @TARGET_MONTH>='202203' THEN CONVERT(NUMERIC,ROUND(TotalGetPoint*1.05,0))",Environment.NewLine,
                        "		WHEN POINT_TYPE='Redeem' THEN CONVERT(NUMERIC,ROUND(TotalRedeemPoint*0.05,0))",Environment.NewLine,
                        "	 END [未稅]",Environment.NewLine,
                        "	,CASE",Environment.NewLine,
                        "		WHEN MerchantName LIKE '%統一渡假村%' AND POINT_TYPE='Get' THEN CONVERT(NUMERIC,ROUND(TotalGetPoint*1.2*0.05,0))",Environment.NewLine,
                        "		WHEN POINT_TYPE='Get' AND @TARGET_MONTH<'202203' THEN CONVERT(NUMERIC,ROUND(TotalGetPoint*1.02*0.05,0))",Environment.NewLine,
                        "		WHEN POINT_TYPE='Get' AND @TARGET_MONTH>='202203' THEN CONVERT(NUMERIC,ROUND(TotalGetPoint*1.05*0.05,0))",Environment.NewLine,
                        "		WHEN POINT_TYPE='Redeem' THEN CONVERT(NUMERIC,ROUND(TotalRedeemPoint*0.05*0.05,0))",Environment.NewLine,
                        "	 END [稅額]",Environment.NewLine,
                        "	,CASE",Environment.NewLine,
                        "		WHEN POINT_TYPE='Get' THEN @TARGET_MONTH+' '+'處理費-行銷費用 '+MerchantName",Environment.NewLine,
                        "		WHEN POINT_TYPE='Redeem' THEN @TARGET_MONTH+' '+'處理費-行銷獎勵回饋 '+MerchantName",Environment.NewLine,
                        "	 END [品名]",Environment.NewLine,
                        "	,'461105'[會計科目]",Environment.NewLine,
                        "	,'2110' [部門代號]",Environment.NewLine,
                        "	,CONVERT(VARCHAR(6),DATEADD(M,2,@TARGET_MONTH+'01'),112)+'25' [預計兌現日]",Environment.NewLine,
                        "	,'I0001'[專案代號]",Environment.NewLine,
                        "	,''[備註]",Environment.NewLine,
                        "	,''[B2C統編]",Environment.NewLine,
                        "	,1[數量]",Environment.NewLine,
                        "	,'6114'[單別代號]",Environment.NewLine,
                        "FROM (",Environment.NewLine,
                        "	SELECT D.UnifiedBusinessNo",Environment.NewLine,
                        "		,D.MerchantName",Environment.NewLine,
                        "		,SUM(CASE WHEN D.PointType='累點' THEN D.Point ELSE 0 END)",Environment.NewLine,
                        "		 +SUM(CASE WHEN D.PointType='累點退貨' THEN D.Point ELSE 0 END)",Environment.NewLine,
                        "		 +SUM(CASE WHEN D.PointType='客服累點' THEN D.Point ELSE 0 END)",Environment.NewLine,
                        "		 +SUM(CASE WHEN D.PointType='客服累點退貨' THEN D.Point ELSE 0 END)",Environment.NewLine,
                        "		 +SUM(CASE WHEN D.PointType='OP代墊負點數淨額' THEN D.Point ELSE 0 END) [TotalGetPoint]",Environment.NewLine,
                        "		,(-1)*(SUM(CASE WHEN D.PointType='兌點' THEN D.Point ELSE 0 END)",Environment.NewLine,
                        "		 +SUM(CASE WHEN D.PointType='兌點取消' THEN D.Point ELSE 0 END)",Environment.NewLine,
                        "		 +SUM(CASE WHEN D.PointType='客服兌點' THEN D.Point ELSE 0 END)",Environment.NewLine,
                        "		 +SUM(CASE WHEN D.PointType='客服兌點取消' THEN D.Point ELSE 0 END)) [TotalRedeemPoint]",Environment.NewLine,
                        "	FROM (",Environment.NewLine,
                        "		SELECT",Environment.NewLine,
                        "			CASE M.UnifiedBusinessNo",Environment.NewLine,
                        "				WHEN '72769260' THEN '23115927'",Environment.NewLine,
                        "				WHEN '52795374' THEN '52538517'",Environment.NewLine,
                        "				WHEN '55780851' THEN '52538517'",Environment.NewLine,
                        "				WHEN '55793407' THEN '52538517'",Environment.NewLine,
                        "				WHEN '09765625' THEN '27359183'",Environment.NewLine,
                        "				ELSE M.UnifiedBusinessNo",Environment.NewLine,
                        "			END [UnifiedBusinessNo]",Environment.NewLine,
                        "			,CASE M.UnifiedBusinessNo",Environment.NewLine,
                        "				WHEN '72769260' THEN '台塑-世新礁溪'",Environment.NewLine,
                        "				WHEN '52795374' THEN '台塑-挑戰者'",Environment.NewLine,
                        "				WHEN '55780851' THEN '台塑-挑戰者'",Environment.NewLine,
                        "				WHEN '55793407' THEN '台塑-挑戰者'",Environment.NewLine,
                        "				WHEN '09765625' THEN '台塑-指南成泰'",Environment.NewLine,
                        "				ELSE M.MerchantName",Environment.NewLine,
                        "			 END [MerchantName]",Environment.NewLine,
                        "			,SUBSTRING(D.TransType,1,LEN(D.TransType)-3) [PointType]",Environment.NewLine,
                        "			,SUM(D.Point) [Point]",Environment.NewLine,
                        "		FROM ICASHOP.dbo.OpenPoint_TransDetail D WITH(NOLOCK)",Environment.NewLine,
                        "		INNER JOIN ICASHOP.dbo.OpenPoint_MerchantData M WITH(NOLOCK)",Environment.NewLine,
                        "			ON ((@TARGET_DateType='Merchant' AND SUBSTRING(ISNULL(D.WriteOffNo,''),7,6) = @TARGET_MONTH)",Environment.NewLine,
                        "				OR (@TARGET_DateType='OP' AND CONVERT(varchar(7),D.CptDate) = LEFT(@TARGET_MONTH,4)+'/'+RIGHT(@TARGET_MONTH,2)))",Environment.NewLine,
                        "			AND ((@Group_TaiYa='Y' AND M.MerchantName LIKE '%台亞%')",Environment.NewLine,
                        "				OR (@Group_TaiSu='Y' AND M.MerchantName LIKE '%台塑%')",Environment.NewLine,
                        "				OR (@Group_XiOu='Y' AND M.MerchantName LIKE '%西歐%')",Environment.NewLine,
                        "				OR (@Group_FuMou='Y' AND M.MerchantName LIKE '%福懋%')",Environment.NewLine,
                        "				OR (@Group_TongYiDuJiaCun='Y' AND M.MerchantName LIKE '%統一渡假村%'))",Environment.NewLine,
                        "			AND CONVERT(VARCHAR(4),D.StoreNo) = M.ICashMID",Environment.NewLine,
                        "		GROUP BY M.UnifiedBusinessNo,M.MerchantName,SUBSTRING(D.TransType,1,LEN(D.TransType)-3)",Environment.NewLine,
                        "	) D",Environment.NewLine,
                        "	GROUP BY D.UnifiedBusinessNo,D.MerchantName",Environment.NewLine,
                        ") D",Environment.NewLine,
                        "CROSS JOIN (",Environment.NewLine,
                        "	SELECT 'Get'[POINT_TYPE]",Environment.NewLine,
                        "	UNION ALL",Environment.NewLine,
                        "	SELECT 'Redeem' [POINT_TYPE]",Environment.NewLine,
                        ") T",Environment.NewLine,
                        "WHERE (POINT_TYPE='Get' AND TotalGetPoint<>0)",Environment.NewLine,
                        "	OR (POINT_TYPE='Redeem' AND TotalRedeemPoint<>0)",Environment.NewLine,
                    });
                    #endregion
                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    sqlCmd.Parameters.Add(new SqlParameter("@EXEC_TARGET_MONTH", TARGET_MONTH));
                    sqlCmd.Parameters.Add(new SqlParameter("@EXEC_TARGET_DateType", TARGET_DateType));
                    sqlCmd.Parameters.Add(new SqlParameter("@EXEC_Group_TaiYa", Group_TaiYa));
                    sqlCmd.Parameters.Add(new SqlParameter("@EXEC_Group_TaiSu", Group_TaiSu));
                    sqlCmd.Parameters.Add(new SqlParameter("@EXEC_Group_XiOu", Group_XiOu));
                    sqlCmd.Parameters.Add(new SqlParameter("@EXEC_Group_FuMou", Group_FuMou));
                    sqlCmd.Parameters.Add(new SqlParameter("@EXEC_Group_TongYiDuJiaCun", Group_TongYiDuJiaCun));
                    SqlDataAdapter adapter = new SqlDataAdapter(sqlCmd);
                    adapter.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                log.Debug(ex.StackTrace);
                log.Debug(ex.Message);
            }
            return dt;
        }
    }
}
