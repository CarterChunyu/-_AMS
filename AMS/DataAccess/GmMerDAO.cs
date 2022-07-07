using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.IO;
using System.Data;
using Domain.Entities;
using Dapper;
using log4net;

namespace DataAccess
{
    public class GmMerDAO : BasicDAL
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(GmMerchantTypeDAO));

        public IEnumerable<GM_MERC_GROUP_SETTING_M> SelectMercGroup()
        {
            IEnumerable<GM_MERC_GROUP_SETTING_M> results;
            using (SqlConnection conn = new SqlConnection(this._connectionString))
            {
                string sql = "select * from GM_MERC_GROUP_SETTING_M";
                results = conn.Query<GM_MERC_GROUP_SETTING_M>(sql);
            }
            return results;
        }

        public List<string> SelectGroupID()
        {
            List<string> result = new List<string>();
            string sql = "select distinct GROUP_ID from dbo.GM_MERCHANT_TYPE_D";
            using (SqlConnection conn = new SqlConnection(this._connectionString))
            {
                foreach (var item in conn.Query(sql))
                {
                    result.Add(item.GROUP_ID);
                }
            }
            return result;
        }

        public GmMerInput SelectByID(string input)
        {
            GmMerInput results;

            #region SQLCmd
            string sqlCmd = @"SELECT MERCHANT_NO
	                    ,MERC_GROUP
	                    ,MERCHANT_NAME
	                    ,MERCHANT_STNAME
	                    ,INVOICE_NO
	                    ,MERCH_TMID
	                    ,OL_TYPE
	                    ,MAX(PUR_FEE_RATE) AS PUR_FEE_RATE
	                    ,MAX(LOAD_FEE_RATE) AS LOAD_FEE_RATE
	                    ,MAX(AUTO_LOAD_FEE_RATE) AS AUTO_LOAD_FEE_RATE
	                    ,REM_TYPE
	                    ,DAYLY_REM_DAY
	                    ,REM_FEE_TYPE
	                    ,DAYLY_REM_FEE_DAY
	                    ,GROUP_ID
                    FROM (
	                    SELECT M.MERCHANT_NO
		                    ,M.MERC_GROUP
		                    ,M.MERCHANT_NAME
		                    ,M.MERCHANT_STNAME
		                    ,M.INVOICE_NO
		                    ,M.MERCH_TMID
		                    ,M.OL_TYPE
		                    ,CASE C.CONTRACT_TYPE WHEN 'P1' THEN C.FEE_RATE*100 ELSE 0 END AS PUR_FEE_RATE
		                    ,CASE C.CONTRACT_TYPE WHEN 'L1' THEN C.FEE_RATE*100 ELSE 0 END AS LOAD_FEE_RATE
		                    ,CASE C.CONTRACT_TYPE WHEN 'L2' THEN C.FEE_RATE*100 ELSE 0 END AS AUTO_LOAD_FEE_RATE
		                    ,S.REM_TYPE
		                    ,CASE S.REM_TYPE WHEN 'D' THEN CONVERT(VARCHAR(2),S.DAYLY_REM_DAY) ELSE '' END AS DAYLY_REM_DAY
		                    ,F.REM_TYPE AS REM_FEE_TYPE
		                    ,CASE F.REM_TYPE WHEN 'D' THEN CONVERT(VARCHAR(2),F.DAYLY_REM_DAY) ELSE '' END AS DAYLY_REM_FEE_DAY
		                    ,T.GROUP_ID
	                    FROM dbo.GM_MERCHANT M
	                    INNER JOIN dbo.GM_CONTRACT_MUTI_MERC_D C
		                    ON M.MERCHANT_NO = C.CPT_MERCHANT_NO
		                    AND C.MERCHANT_NO = C.CPT_MERCHANT_NO
	                    INNER JOIN dbo.GM_REM_M S
		                    ON M.MERCHANT_NO = S.MERCHANT_NO
	                    INNER JOIN dbo.GM_REM_FEE_M F
		                    ON M.MERCHANT_NO = F.MERCHANT_NO
		                    AND C.CONTRACT_TYPE = F.CONTRACT_TYPE
	                    INNER JOIN dbo.GM_MERCHANT_TYPE_D T
		                    ON M.MERCHANT_NO = T.MERCHANT_NO
	                    WHERE M.MERCHANT_NO = @MERCHANT_NO
                    ) A
                    GROUP BY MERCHANT_NO,MERC_GROUP,MERCHANT_NAME,MERCHANT_STNAME,INVOICE_NO,MERCH_TMID,OL_TYPE,REM_TYPE,DAYLY_REM_DAY,REM_FEE_TYPE,DAYLY_REM_FEE_DAY,GROUP_ID";
            #endregion
            
            using (SqlConnection conn = new SqlConnection(this._connectionString))
            {
                results = conn.QueryFirstOrDefault<GmMerInput>(sqlCmd, new { MERCHANT_NO = input });
            }

            return results;
        }

        public int InsertMerchant(GmMerInput data)
        {
            #region SQLCmd
            string sqlCmd = @"DECLARE @UPD_DATE VARCHAR(8) = CONVERT(VARCHAR(8),GETDATE(),112)
                DECLARE @BUILD_DATETIME VARCHAR(14) = @UPD_DATE+REPLACE(CONVERT(VARCHAR(8),GETDATE(),108),':','')
                DECLARE @EFF_DATE_FROM VARCHAR(8) = CONVERT(VARCHAR(4),@UPD_DATE)+'0101'
                DECLARE @STEP VARCHAR(100)

                BEGIN TRY

                SET @STEP = '01-GM_MERCHANT'
                ------------------------
                INSERT dbo.GM_MERCHANT
                (MERC_GROUP,MERCHANT_NO,MERCHANT_NAME,MERCHANT_TYPE,READER_TYPE,SAM_TYPE,OWNER,CONTACT,INVOICE_NO,ADDRESS,TEL_NO,FAX_NO,MERCH_TMID,MERCH_SCHEMAID,ACCT_SET_GROUP,ACCT_SET_GROUP_NO,ACCT_DEPT_NO,STORE_LEN,OL_TYPE,BL_FLG,TM_FLG,MERC_STATUS,MERCHANT_STNAME,MODIFIABLE)
                VALUES (@MERC_GROUP,@MERCHANT_NO,@MERCHANT_NAME,'MUTI_MERC','01','C0','','',@INVOICE_NO,'','','',@MERCH_TMID,@MERC_GROUP,'N','','2100','8',@OL_TYPE,'Y','Y','2',@MERCHANT_STNAME,'N')

                SET @STEP = '02-SM_MERCHANT_D'
                ------------------------
                INSERT dbo.SM_MERCHANT_D
                (M_KIND,MERCHANT_NO,MERCHANT_NAME,OWNER,CONTACT,INVOICE_NO,ADDRESS,TEL_NO,FAX_NO,EMAIL,SEND_TIME,STATUS,BANK_NAME,ACCOUNT,PUBLIC_KEY,PRIVATE_KEY,AUTH_KEY,IP,MERCH_TMID,MERCH_SCHEMAID)
                VALUES 
	                 ('11',@MERCHANT_NO
	                ,CONVERT(VARCHAR(20),@MERCHANT_NAME)+@MERCHANT_NO+'11'
	                ,'','',@INVOICE_NO,'','','','','','Y',NULL,NULL,NULL,NULL,NULL,NULL,@MERCH_TMID,@MERC_GROUP)
	                ,('21',@MERCHANT_NO
	                ,CONVERT(VARCHAR(20),@MERCHANT_NAME)+@MERCHANT_NO+'21'
	                ,'','',@INVOICE_NO,'','','','','','Y',NULL,NULL,NULL,NULL,NULL,NULL,@MERCH_TMID,@MERC_GROUP)

                SET @STEP = '03-GM_MERC_GROUP_D'
                ------------------------
                INSERT dbo.GM_MERC_GROUP_D
                (MERC_GROUP,MERCHANT_NO,MERCHANT_SUB_NO,ORDER_WEIGHT,IS_DEL,IS_YEAR_ADJ,UPD_DATE)
                SELECT MERC_GROUP
	                ,MERCHANT_NO
	                ,@MERCHANT_NO
	                ,''
	                ,'Y'
	                ,'N'
	                ,@UPD_DATE
                FROM dbo.GM_MERC_GROUP_D
                WHERE MERCHANT_NO = MERCHANT_SUB_NO
	                AND MERC_GROUP = @MERC_GROUP

                SET @STEP = '04-GM_MERCHANT_SUMSET_MST'
                ------------------------
                INSERT dbo.GM_MERCHANT_SUMSET_MST
                (MERCHANT_NO,SUMSET_TYPE,TABLE_NAME)
                VALUES
	                 (@MERCHANT_NO,'CR','TM_SETTLE_DATA_D')
	                ,(@MERCHANT_NO,'FSC','TM_SETTLE_DATA_D')

                SET @STEP = '05-GM_REG_MST'
                ------------------------
                INSERT dbo.GM_REG_MST
                (MERCHANT_NO,REG_ID_LEN)
                VALUES (@MERCHANT_NO,3)

                SET @STEP = '06-GM_CONTRACT_M'
                ------------------------
                INSERT dbo.GM_CONTRACT_M
                (MERCHANT_NO,EFF_DATE_FROM,EFF_DATE_TO,SUM_MON_S,SUM_DAY_S,SUM_MON_E,SUM_DAY_E,UPD_DATE)
                VALUES (@MERCHANT_NO,@EFF_DATE_FROM,'99991231','-1','01','-1','99',@UPD_DATE)

                SET @STEP = '07-GM_CONTRACT_MUTI_MERC_D'
                ------------------------
                INSERT dbo.GM_CONTRACT_MUTI_MERC_D
                (MERCHANT_NO,CPT_MERCHANT_NO,EFF_DATE_FROM,EFF_DATE_TO,CONTRACT_TYPE,DEVICE_ID,FEE_KIND,FEE_CAL_FLG,ISZERO,RANK_S,RANK_E,RANK_KIND,FEE_RATE,MERC_GROUP,TAX_NO,OPERATION_ORDER_BY_CPTDATE,OPERATION_ORDER_BY_STORE,NUM_PREC,TYPE_PREC,UPD_DATE)
                VALUES
	                 (@MERCHANT_NO,@MERCHANT_NO,@EFF_DATE_FROM,'99991231','P1','A','CM','AM','A','-9999999','9999999','RANK',@PUR_FEE_RATE/100,@MERC_GROUP,'Y','A','A','0','R',@UPD_DATE)
	                ,(@MERCHANT_NO,@MERCHANT_NO,@EFF_DATE_FROM,'99991231','L1','A','CM','AM','A','-9999999','9999999','RANK',@LOAD_FEE_RATE/100,@MERC_GROUP,'Y','A','A','0','R',@UPD_DATE)
	                ,(@MERCHANT_NO,@MERCHANT_NO,@EFF_DATE_FROM,'99991231','L2','A','CM','AM','A','-9999999','9999999','RANK',@AUTO_LOAD_FEE_RATE/100,@MERC_GROUP,'Y','A','A','0','R',@UPD_DATE)

                SET @STEP = '08-GM_REM_M'
                ------------------------
                INSERT dbo.GM_REM_M
                (MERCHANT_NO,REM_TYPE,EFF_DATE_FROM,EFF_DATE_TO,LONG_HOLIDAY_REM_DAY,DAYLY_REM_DAY,WEEKLY_REM_DAY,MONTHLY_BEGINMON,MONTHLY_INTERVAL,MONTHLY_REM_MON,MONTHLY_REM_DAY,UPD_DATE,SETTLE_TYPE,IS_NET)
                SELECT @MERCHANT_NO
	                ,@REM_TYPE
	                ,@EFF_DATE_FROM
	                ,'99991231'
	                ,'3'
	                ,CASE WHEN @REM_TYPE='D' THEN @DAYLY_REM_DAY ELSE NULL END
	                ,CASE WHEN @REM_TYPE='W' THEN 2 ELSE NULL END
	                ,CASE WHEN @REM_TYPE='M' THEN 1 ELSE NULL END
	                ,CASE WHEN @REM_TYPE='M' THEN 1 ELSE NULL END
	                ,CASE WHEN @REM_TYPE='M' THEN 1 ELSE NULL END
	                ,CASE WHEN @REM_TYPE='M' THEN '05' ELSE NULL END
	                ,@UPD_DATE
	                ,'A'
	                ,'Y'

                SET @STEP = '09-GM_REM_FEE_M'
                ------------------------
                INSERT dbo.GM_REM_FEE_M
                (MERCHANT_NO,CONTRACT_TYPE,REM_TYPE,EFF_DATE_FROM,EFF_DATE_TO,LONG_HOLIDAY_REM_DAY,DAYLY_REM_DAY,WEEKLY_ACC_DAY,WEEKLY_SUM_WEEK_S,WEEKLY_REM_DAY,MONTHLY_ACC_DAY,MONTHLY_SUM_MON_S,MONTHLY_SUM_DAY_S,MONTHLY_SUM_MON_E,MONTHLY_SUM_DAY_E,MONTHLY_REM_MON,MONTHLY_REM_DAY,IS_NET,UPD_DATE)
                SELECT MERCHANT_NO,CONTRACT_TYPE,REM_TYPE,EFF_DATE_FROM,EFF_DATE_TO,LONG_HOLIDAY_REM_DAY,DAYLY_REM_DAY,WEEKLY_ACC_DAY,WEEKLY_SUM_WEEK_S,WEEKLY_REM_DAY,MONTHLY_ACC_DAY,MONTHLY_SUM_MON_S,MONTHLY_SUM_DAY_S,MONTHLY_SUM_MON_E,MONTHLY_SUM_DAY_E,MONTHLY_REM_MON,MONTHLY_REM_DAY,IS_NET,UPD_DATE
                FROM (
                SELECT @MERCHANT_NO AS MERCHANT_NO
	                ,@REM_FEE_TYPE AS REM_TYPE
	                ,@EFF_DATE_FROM AS EFF_DATE_FROM
	                ,'99991231' AS EFF_DATE_TO
	                ,'3' AS LONG_HOLIDAY_REM_DAY
	                ,CASE WHEN @REM_FEE_TYPE='D' THEN @DAYLY_REM_FEE_DAY ELSE NULL END AS DAYLY_REM_DAY
	                ,CASE WHEN @REM_FEE_TYPE='W' THEN 1 ELSE NULL END AS WEEKLY_ACC_DAY
	                ,CASE WHEN @REM_FEE_TYPE='W' THEN 0 ELSE NULL END AS WEEKLY_SUM_WEEK_S
	                ,CASE WHEN @REM_FEE_TYPE='W' THEN 2 ELSE NULL END AS WEEKLY_REM_DAY
	                ,NULL AS MONTHLY_ACC_DAY
	                ,CASE WHEN @REM_FEE_TYPE='M' THEN -1 ELSE NULL END AS MONTHLY_SUM_MON_S
	                ,CASE WHEN @REM_FEE_TYPE='M' THEN '01' ELSE NULL END AS MONTHLY_SUM_DAY_S
	                ,CASE WHEN @REM_FEE_TYPE='M' THEN -1 ELSE NULL END AS MONTHLY_SUM_MON_E
	                ,CASE WHEN @REM_FEE_TYPE='M' THEN '99' ELSE NULL END AS MONTHLY_SUM_DAY_E
	                ,CASE WHEN @REM_FEE_TYPE='M' THEN 0 ELSE NULL END AS MONTHLY_REM_MON
	                ,CASE WHEN @REM_FEE_TYPE='M' THEN '05' ELSE NULL END AS MONTHLY_REM_DAY
	                ,'Y' AS IS_NET
	                ,@UPD_DATE AS UPD_DATE
                ) A
                CROSS JOIN (
	                SELECT 'P1' AS CONTRACT_TYPE
	                UNION ALL SELECT 'L1' AS CONTRACT_TYPE
	                UNION ALL SELECT 'L2' AS CONTRACT_TYPE
                ) Z

                SET @STEP = '10-GM_COMMON_M'
                ------------------------
                INSERT dbo.GM_COMMON_M
                (MERCHANT_NO,SERVER_NAME,TYPE_GROUP,SELECTED_COLUMNS,EFF_DATE_FROM,EFF_DATE_TO,UPD_DATE)
                VALUES
	                 (@MERCHANT_NO,'','BATCH_TABLE','',@EFF_DATE_FROM,'99991231',@UPD_DATE)
	                ,(@MERCHANT_NO,'','TRANS_DESC1','b.TRANS_DESC',@EFF_DATE_FROM,'99991231',@UPD_DATE)

                SET @STEP = '11-GM_MERCHANT_TYPE_D'
                ------------------------
                INSERT dbo.GM_MERCHANT_TYPE_D
                (MERCHANT_NO,GROUP_ID,SHOW_ORDER,SHOW_FLG,STORE_TYPE)
                SELECT @MERCHANT_NO,M.GROUP_ID
	                ,CASE WHEN D.SHOW_ORDER IS NULL
		                THEN '001'
		                ELSE REPLICATE('0',3-LEN(D.SHOW_ORDER))+CONVERT(VARCHAR(3),D.SHOW_ORDER) 
	                 END
	                ,'Y',NULL
                FROM dbo.GM_MERCHANT_TYPE_M M
                LEFT JOIN (
	                SELECT GROUP_ID,CONVERT(NUMERIC,MAX(SHOW_ORDER))+1 AS SHOW_ORDER
	                FROM dbo.GM_MERCHANT_TYPE_D
	                WHERE SHOW_ORDER <> '999'
	                GROUP BY GROUP_ID
                ) D
	                ON M.GROUP_ID = D.GROUP_ID
                WHERE M.GROUP_ID = @GROUP_ID

                SET @STEP = '---GM_BUILD_MERCHANT_LOG'
                ------------------------
                INSERT dbo.GM_BUILD_MERCHANT_LOG
                (MERCHANT_NO,LOG_KIND,LOG_MSG,BUILD_USER,BUILD_DATETIME,IS_DEL,DEL_DATE)
                VALUES (@MERCHANT_NO,'Ok','',@BUILD_USER,@BUILD_DATETIME,'','')

                END TRY
                BEGIN CATCH

                DECLARE @ErrorSeverity NUMERIC = ERROR_SEVERITY()
                DECLARE @ErrorState NUMERIC = ERROR_STATE()
                DECLARE @ErrorMessage VARCHAR(MAX) = '中斷步驟:'+@STEP+'。 錯誤行號:'+CONVERT(VARCHAR,ERROR_LINE())+'。 錯誤訊息:'+ERROR_MESSAGE()

                INSERT dbo.GM_BUILD_MERCHANT_LOG
                (MERCHANT_NO,LOG_KIND,LOG_MSG,BUILD_USER,BUILD_DATETIME,IS_DEL,DEL_DATE)
                VALUES (@MERCHANT_NO,'Err',@ErrorMessage,@BUILD_USER,@BUILD_DATETIME,'N','')

                RAISERROR( @ErrorMessage, @ErrorSeverity, @ErrorState)

                END CATCH";
            #endregion
            int executeResult = 0;
            try
            {
                using (SqlConnection conn = new SqlConnection(this._connectionString))
                {
                    executeResult = conn.Execute(sqlCmd, data);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return executeResult;
        }
    }
}
