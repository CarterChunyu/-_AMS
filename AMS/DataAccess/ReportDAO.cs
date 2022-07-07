using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Domain;


namespace DataAccess
{
    public class ReportDAO:BasicDAL
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ReportDAO));

        GmMerchantDAO GmDAO = null;
        public ReportDAO()
        {   
            GmDAO  = new GmMerchantDAO();
        }

        public DataTable ReportA03(string start)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();
                    string sqlText =
                            @"select CAL_DATE, BAL_LST, BAL_ACT, BAL_PP from AM_ENT_BAL_LOG_D where CAL_DATE like @CAL_DATE";
                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    sqlCmd.Parameters.AddWithValue("@CAL_DATE", String.Format("{0}%", start));

                    SqlDataAdapter adapter = new SqlDataAdapter(sqlCmd);

                    adapter.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                log.Debug(ex.Message);
            }
            return dt;
        }

        public DataTable Report0211(string iccno, string store_no)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();
                    string sqlText =
                            @"select * from IM_ISET_TXLOG_T
				WHERE (STORE_NO = @GEN_STORE_NO or @GEN_STORE_NO='')
				AND ICC_NO = @GEN_ICCNO and ICC_NO<>'' order by TXLOG_ID;";
                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    sqlCmd.Parameters.Add("@GEN_ICCNO", SqlDbType.VarChar).Value = iccno;
                    sqlCmd.Parameters.Add("@GEN_STORE_NO", SqlDbType.VarChar).Value = store_no;

                    SqlDataAdapter adapter = new SqlDataAdapter(sqlCmd);

                    adapter.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                log.Debug(ex.Message);
            }
            return dt;
        }

        public DataTable Report0212T(string start)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();

                    string sqlText = @"DECLARE @temp1 TABLE (MERCHANT_NO CHAR(8),ACT_DATE CHAR(8),STORE_NO VARCHAR(20),REG_ID VARCHAR(4),SEQ_NO VARCHAR(10), DATETIME CHAR(14),TRANS_DESC VARCHAR(20),ICC_NO CHAR(16),TRANS_AMT VARCHAR(8),TM_RCODE VARCHAR(4),帳務認列 VARCHAR(10),卡務認列 VARCHAR(10),TRANS_TYPE CHAR(2))

INSERT INTO @temp1
select MERCHANT_NO as '特約機構' , ACT_DATE as	'會計日期', STORE_NO as	'門市代號',	REG_ID as '收銀機代號',SEQ_NO as	'交易序號',DATETIME as	'交易日期時間', GM_TRANS_TYPE.TRANS_DESC as '交易代號', ICC_NO as	'卡號',cast( cast(TRANS_AMT as float) as varchar) as '交易金額',TM_RCODE,'重複認列' as '帳務認列', '重複剔退' as	'卡務認列',IM_ISET_TXLOG_T.TRANS_TYPE
 from IM_ISET_TXLOG_T ,GM_TRANS_TYPE
 where IM_ISET_TXLOG_T.TRANS_TYPE=GM_TRANS_TYPE.TRANS_TYPE 
--LEFT(convert(varchar, dateadd(d,1,cast(SETTLE_DATE as date)) ,112),8)=@Sdate or
 and ( (substring(ZIP_FILE_NAME,1,6)='TMLOG_' and substring(ZIP_FILE_NAME,7,8)=@Sdate) or (substring(ZIP_FILE_NAME,1,6)='TXLOG_' and substring(ZIP_FILE_NAME,7,8)=@Sdate) or (substring(ZIP_FILE_NAME,1,8)='ACICICCG' and substring(SETTLE_DATE,1,4)+substring(ZIP_FILE_NAME,9,4)=LEFT(convert(varchar, dateadd(d,-1,cast(@Sdate as date)) ,112),8)))
-- and ICC_NO in (select ICC_NO from IM_ISET_TXLOG_T where CM_TAG='DU' and IM_ISET_TXLOG_T.TRANS_TYPE in ('21','22','23','24','74','75','77') ) 
and CM_TAG='DU' and IM_ISET_TXLOG_T.TRANS_TYPE in ('21','22','23','24','74','75','77' )


SELECT 特約機構,會計日期,門市代號,收銀機代號,交易序號,交易日期時間,交易代號,卡號,交易金額,認列代碼,帳務認列,卡務認列 FROM
(
SELECT MERCHANT_NO AS '特約機構',ACT_DATE AS '會計日期',STORE_NO AS '門市代號',REG_ID AS '收銀機代號',SEQ_NO AS '交易序號', DATETIME AS '交易日期時間',TRANS_DESC AS '交易代號',ICC_NO AS 卡號,TRANS_AMT AS 交易金額,TM_RCODE AS 認列代碼,帳務認列,卡務認列 FROM @temp1 
UNION 
SELECT '' as '特約機構' , '' as	'會計日期', '' as	'門市代號',	'' as '收銀機代號','' as	'交易序號','' as	'交易日期時間', '' as '交易代號', case when count(*) =0 then '' else '購貨共'+cast(isnull(count(*),0) as varchar) + '筆，帳務重覆認列總金額'+ cast( isnull(sum(cast(TRANS_AMT as float)),0) as varchar)+'元' end AS 卡號, '' AS 交易金額,'' AS 認列代碼,''	 as '帳務認列', '' as	'卡務認列' FROM @temp1 WHERE TRANS_TYPE='21'
UNION
select '' as '特約機構' , '' as	'會計日期', '' as	'門市代號',	'' as '收銀機代號','' as	'交易序號','' as	'交易日期時間', '' as '交易代號', case when count(*) =0 then '' else '購貨取消共'+cast(isnull(count(*),0) as varchar) + '筆，帳務重覆認列總金額'+ cast( isnull(sum(cast(TRANS_AMT as float)),0) as varchar)+'元' end AS 卡號, '' AS 交易金額,'' AS 認列代碼,''	 as '帳務認列', '' as	'卡務認列' FROM @temp1 WHERE TRANS_TYPE='23'
UNION 
select '' as '特約機構' , '' as	'會計日期', '' as	'門市代號',	'' as '收銀機代號','' as	'交易序號','' as	'交易日期時間', '' as '交易代號', case when count(*) =0 then '' else '儲值共'+cast(isnull(count(*),0) as varchar) + '筆，帳務重覆認列總金額'+ cast( isnull(sum(cast(TRANS_AMT as float)),0) as varchar)+'元' end AS 卡號, '' AS 交易金額,'' AS 認列代碼,''	 as '帳務認列', '' as	'卡務認列' FROM @temp1 WHERE TRANS_TYPE='22'
UNION 
select '' as '特約機構' , '' as	'會計日期', '' as	'門市代號',	'' as '收銀機代號','' as	'交易序號','' as	'交易日期時間', '' as '交易代號', case when count(*) =0 then '' else '儲值取消共'+cast(isnull(count(*),0) as varchar) + '筆，帳務重覆認列總金額'+ cast( isnull(sum(cast(TRANS_AMT as float)),0) as varchar)+'元' end AS 卡號, '' AS 交易金額,'' AS 認列代碼,''	 as '帳務認列', '' as	'卡務認列' FROM @temp1 WHERE TRANS_TYPE='24'
UNION 
select '' as '特約機構' , '' as	'會計日期', '' as	'門市代號',	'' as '收銀機代號','' as	'交易序號','' as	'交易日期時間', '' as '交易代號', case when count(*) =0 then '' else '自動加值共'+cast(isnull(count(*),0) as varchar) + '筆，帳務重覆認列總金額'+ cast( isnull(sum(cast(TRANS_AMT as float)),0) as varchar)+'元' end AS 卡號, '' AS 交易金額,'' AS 認列代碼,''	 as '帳務認列', '' as	'卡務認列' FROM @temp1 WHERE TRANS_TYPE='74'
UNION
select '' as '特約機構' , '' as	'會計日期', '' as	'門市代號',	'' as '收銀機代號','' as	'交易序號','' as	'交易日期時間', '' as '交易代號', case when count(*) =0 then '' else '離線自動加值共'+cast(isnull(count(*),0) as varchar) + '筆，帳務重覆認列總金額'+ cast( isnull(sum(cast(TRANS_AMT as float)),0) as varchar)+'元' end AS 卡號, '' AS 交易金額,'' AS 認列代碼,''	 as '帳務認列', '' as	'卡務認列' FROM @temp1 WHERE TRANS_TYPE='77'
UNION
select '' as '特約機構' , '' as	'會計日期', '' as	'門市代號',	'' as '收銀機代號','' as	'交易序號','' as	'交易日期時間', '' as '交易代號', case when count(*) =0 then '' else '代行授權共'+cast(isnull(count(*),0) as varchar) + '筆，帳務重覆認列總金額'+ cast( isnull(sum(cast(TRANS_AMT as float)),0) as varchar)+'元' end AS 卡號, '' AS 交易金額,'' AS 認列代碼,''	 as '帳務認列', '' as	'卡務認列' FROM @temp1 WHERE TRANS_TYPE='75'
) x ORDER BY x.特約機構 DESC,x.交易代號,x.卡號";

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    sqlCmd.Parameters.Add("@Sdate", SqlDbType.VarChar).Value = start;

                    SqlDataAdapter adapter = new SqlDataAdapter(sqlCmd);

                    adapter.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                log.Debug(ex.Message);
            }
            return dt;
        }

        public DataTable Report0212_2T(string start)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();

                    string sqlText = @"select IM_ISET_TXLOG_T.*  from IM_ISET_TXLOG_T
where ( (substring(ZIP_FILE_NAME,1,6)='TMLOG_' and substring(ZIP_FILE_NAME,7,8)=@Sdate) or (substring(ZIP_FILE_NAME,1,6)='TXLOG_' and substring(ZIP_FILE_NAME,7,8)=@Sdate) or (substring(ZIP_FILE_NAME,1,8)='ACICICCG' and substring(SETTLE_DATE,1,4)+substring(ZIP_FILE_NAME,9,4)=LEFT(convert(varchar, dateadd(d,-1,cast(@Sdate as date)) ,112),8)))
 and ICC_NO in (select ICC_NO from IM_ISET_TXLOG_T where CM_TAG='DU' )  and CM_TAG in ('OK','DU') ";

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    sqlCmd.Parameters.Add("@Sdate", SqlDbType.VarChar).Value = start;

                    SqlDataAdapter adapter = new SqlDataAdapter(sqlCmd);

                    adapter.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                log.Debug(ex.Message);
            }
            return dt;
        }

        public DataTable Report0213T(string start, string end, string merchantNo, string SRC_FLG)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();
                    string sqlText = "";
                    sqlText = @"SELECT g.MERCHANT_STNAME AS 特約機構,CONVERT(VARCHAR, (CONVERT(datetime,CPT_DATE,112)-1),112) AS 傳輸日, 
CPT_DATE AS 清算日, 
SETTLE_DATE AS 會計日, 
PCH_CNT AS 購貨筆數, 
PCH_AMT AS 購貨金額, 
PCHR_CNT AS 購貨取消筆數, 
PCHR_AMT AS 購貨取消金額,
PCH_CNT-PCHR_CNT AS 購貨小計筆數,
PCH_AMT-PCHR_AMT AS 購貨小計金額,
LOAD_CNT AS 加值筆數, 
LOAD_AMT AS 加值金額, 
LOADR_CNT AS 加值取消筆數, 
LOADR_AMT AS 加值取消金額,
LOAD_CNT-LOADR_CNT AS 加值小計筆數,
LOAD_AMT-LOADR_AMT AS 加值小計金額
FROM AM_ISET_MERC_TRANS_LOG_D am INNER JOIN GM_MERCHANT g
ON am.MERCHANT_NO = g.MERCHANT_NO
WHERE CONVERT(VARCHAR, (CONVERT(datetime,CPT_DATE,112)-1),112) BETWEEN @start_date AND @end_date ";
                    if (SRC_FLG == "TXLOG")
                    {
                        sqlText = sqlText + @" AND SRC_FLG='TXLOG' 
";
                    }
                    else
                   {
                        sqlText = sqlText + @" AND SRC_FLG='POS' 
";
                    }
                    
                    if (merchantNo != "")
                    {
                        sqlText += "AND am.MERCHANT_NO = @mer_no ";
                    }
                    /*
                    sqlText += @"union select '合計' AS 特約機構, '' AS 傳輸日
'' AS 清算日, 
'' AS 會計日, 
isnull(sum(PCH_CNT),0) AS 購貨筆數, 
isnull(sum(PCH_AMT),0) AS 購貨金額, 
isnull(sum(PCHR_CNT),0) AS 購貨取消筆數, 
isnull(sum(PCHR_AMT),0) AS 購貨取消金額,
isnull(sum(PCH_CNT-PCHR_CNT),0) AS 購貨小計筆數,
isnull(sum(PCH_AMT-PCHR_AMT),0) AS 購貨小計金額,
isnull(sum(LOAD_CNT),0) AS 加值筆數, 
isnull(sum(LOAD_AMT),0) AS 加值金額, 
isnull(sum(LOADR_CNT),0) AS 加值取消筆數, 
isnull(sum(LOADR_AMT),0) AS 加值取消金額,
isnull(sum(LOAD_CNT-LOADR_CNT),0) AS 加值小計筆數,
isnull(sum(LOAD_AMT-LOADR_AMT),0) AS 加值小計金額
FROM AM_ISET_MERC_TRANS_LOG_D
WHERE CONVERT(VARCHAR, (CONVERT(datetime,CPT_DATE,112)-1),112) BETWEEN @start_date AND @end_date 
";

                    if (SRC_FLG == "TXLOG")
                    {
                        sqlText += @"AND SRC_FLG='TXLOG' ";
                    }
                    else
                    {
                        sqlText += @"AND SRC_FLG='POS' ";
                    }
                    if (merchantNo != "")
                    {
                        sqlText += "AND MERCHANT_NO = @mer_no ";
                    }
                    
                    sqlText += "ORDER BY 1, CPT_DATE, SETTLE_DATE";
                    */
                    sqlText += "ORDER BY 特約機構,傳輸日,清算日,會計日";

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    sqlCmd.Parameters.Add("@start_date", SqlDbType.VarChar).Value = start;
                    sqlCmd.Parameters.Add("@end_date", SqlDbType.VarChar).Value = end;
                    sqlCmd.Parameters.Add("@mer_no", SqlDbType.VarChar).Value = merchantNo;
                    sqlCmd.Parameters.Add("@SRC_FLG", SqlDbType.VarChar).Value = SRC_FLG;

                    SqlDataAdapter adapter = new SqlDataAdapter(sqlCmd);

                    adapter.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                log.Debug(ex.Message);
            }
            return dt;
        }

        public DataTable Report0214T(string start, string end, string merchantNo, string Kind)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();

//21(51) 購貨作業
//22(52) 儲值作業
//23(53) 購貨取消
//24(54) 儲值取消
                    string sqlText =
                            @"SELECT (SELECT MERCHANT_STNAME FROM GM_MERCHANT WHERE MERCHANT_NO=@p_merchant_no) AS 特約機構,SETTLE_DATE AS 會計日, 
ICC_NO AS 卡號,
(case TRANS_TYPE when '21' then '購貨' when '22' then '加值' when '23' then '購貨取消' when '24' then '加值取消' else '' end) AS 交易別,
STORE_NO AS 店號,
(SELECT TOP 1 STO_NAME_SHORT FROM IM_STOREM_D WHERE TM_ISET_CONSUMDIFF_T.STORE_NO=IM_STOREM_D.STORE_NO) AS 店名,
RESPONSE_DATETIME AS 交易時間,
REG_ID AS 機號,
SEQ_NO AS 交易序號,
TX_VALUE AS 卡機金額,
AES_VALUE AS POS金額,
--TM_RCODE AS 差異原因,
(SELECT VALUE1 FROM SM_AP_PARAM_D WHERE SYS_ID='TM' AND PARAM_SET='TM_RCODE' AND TM_ISET_CONSUMDIFF_T.TM_RCODE=PARAM_ID) AS 差異原因,
substring(CREATE_DATETIME,1,8) AS 新增日期,
substring(LSTUPD_DATETIME,1,8) AS 更新日期
FROM TM_ISET_CONSUMDIFF_T
WHERE SETTLE_DATE BETWEEN @p_start_date AND @p_end_date
AND DIFF_FLG IN (" + Kind + @") --'00'：一般，'01'：重複，'02'：跨月(超過結帳日上傳)，'03'：門市補傳
";
                    if (merchantNo != "")
                    {
                        sqlText += "AND MERCHANT_NO = @p_merchant_no ";
                    }
sqlText = sqlText + @"UNION ALL
SELECT (SELECT MERCHANT_STNAME FROM GM_MERCHANT WHERE MERCHANT_NO=@p_merchant_no) AS 特約機構,SETTLE_DATE AS 會計日, 
ICC_NO AS 卡號,
(case TRANS_TYPE when '21' then '購貨' when '22' then '加值' when '23' then '購貨取消' when '24' then '加值取消' else '' end) AS 交易別,
STORE_NO AS 店號,
(SELECT TOP 1 STO_NAME_SHORT FROM IM_STOREM_D WHERE TM_ISET_LOADDIFF_T.STORE_NO=IM_STOREM_D.STORE_NO) AS 店名,
RESPONSE_DATETIME AS 交易時間,
REG_ID AS 機號,
SEQ_NO AS 交易序號,
TX_VALUE AS 卡機金額,
AES_VALUE AS POS金額,
(SELECT VALUE1 FROM SM_AP_PARAM_D WHERE SYS_ID='TM' AND PARAM_SET='TM_RCODE' AND TM_ISET_LOADDIFF_T.TM_RCODE=PARAM_ID) AS 差異原因,
substring(CREATE_DATETIME,1,8) AS 新增日期,
substring(LSTUPD_DATETIME,1,8) AS 更新日期
FROM TM_ISET_LOADDIFF_T
WHERE SETTLE_DATE BETWEEN @p_start_date AND @p_end_date
AND DIFF_FLG IN (" + Kind + @")
";

                    if (merchantNo != "")
                    {
                        sqlText += "AND MERCHANT_NO = @p_merchant_no ";
                    }

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    sqlCmd.Parameters.Add("@p_start_date", SqlDbType.VarChar).Value = start;
                    sqlCmd.Parameters.Add("@p_end_date", SqlDbType.VarChar).Value = end;
                    sqlCmd.Parameters.Add("@p_merchant_no", SqlDbType.VarChar).Value = merchantNo;
                    sqlCmd.Parameters.Add("@p_kind", SqlDbType.VarChar).Value = Kind;

                    SqlDataAdapter adapter = new SqlDataAdapter(sqlCmd);

                    adapter.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                log.Debug(ex.Message);
            }
            return dt;
        }


        public DataTable Report0111(string start)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();
                    string sqlText =
                            @"SELECT	ROW_NUMBER() OVER(ORDER BY TOTAL_DATE)	AS '順序',
		TOTAL_DATE									AS '日期合計',
        sum(isnull([CP],0))							AS '前日餘額',
		sum(isnull([22],0)) 						AS '現金加值',			--加項   
		sum(isnull([23],0))-(select isnull(sum(isnull(UT_AMT,0)),0)+isnull(sum(isnull(UT3_AMT,0)),0) FROM AM_ISET_MERC_TRANS_UTLOG_D where TRANS_TYPE='23' and LEFT(convert(varchar, dateadd(d,-1,cast(CPT_DATE as date)) ,112),8)=TOTAL_DATE and SRC_FLG='TXLOG') 						AS '消費取消'		,	--加項
		sum(isnull([21],0))+(select isnull(sum(isnull(UT_AMT,0)),0)+isnull(sum(isnull(UT3_AMT,0)),0) FROM AM_ISET_MERC_TRANS_UTLOG_D where TRANS_TYPE='21' and LEFT(convert(varchar, dateadd(d,-1,cast(CPT_DATE as date)) ,112),8)=TOTAL_DATE and SRC_FLG='TXLOG') 						AS '消費'			,	--減項
		sum(isnull([24],0))							AS '加值取消'		,	--減項
        (select -isnull(sum(isnull(UT_AMT,0)),0)-isnull(sum(isnull(UT3_AMT,0)),0) FROM AM_ISET_MERC_TRANS_UTLOG_D where LEFT(convert(varchar, dateadd(d,-1,cast(CPT_DATE as date)) ,112),8)=TOTAL_DATE and SRC_FLG='TXLOG' and TRANS_TYPE='21') as '代收售' ,
        (select isnull(sum(isnull(UT_AMT,0)),0)+isnull(sum(isnull(UT3_AMT,0)),0) FROM AM_ISET_MERC_TRANS_UTLOG_D where LEFT(convert(varchar, dateadd(d,-1,cast(CPT_DATE as date)) ,112),8)=TOTAL_DATE and SRC_FLG='TXLOG' and TRANS_TYPE='23' ) as '代收售取消' ,
		sum(isnull([74],0)) + sum(isnull([77],0))	AS '自動加值',			--加項   bala add 20150318 (add by 20160415 rita 新增 77離線自動加值)
		sum(isnull([75],0)) 						AS '代行授權',	--加項	 bala add 20150318	 
		sum(isnull([74_OL],0)) 						AS '中心端自動加值'		,		
		-sum(isnull([74_DU],0)) - sum(isnull([75_DU],0))		AS '已下帳中心自動加值'		,--減項 add by rita 20150911
		sum(isnull([CL],0)) + sum(isnull([CLI],0))	AS '企業加值'		,	--modify by Rita 20151207 加入ibon企業加值
		--( sum(isnull([PB_BNK],0))+sum(isnull([PB_CUS],0)) )*(-1) AS '加值(回饋)', --20200518 易任 加入特店加值回饋金 --20201013取消欄位
		sum(isnull([CN],0))-sum(isnull([CI],0))		AS '客服申請加值'	,
		sum(isnull([CI],0))							AS 'IDOLLAR餘額轉置',		
		sum(isnull([CB_1],0))						AS '製卡損耗'		,	
		sum(isnull([CB_2],0))						AS '壞卡換新卡'		,			
		sum(isnull([CB_3],0))						AS '壞卡放棄'		,	
		sum(isnull([CB_4],0))						AS '退費讀卡失敗'	,
		sum(isnull([I4],0))							AS '退費'			,
		--sum(isnull([PB_CUS],0))                     AS '退費_特店回饋'  ,     --20200518 易任 加入 --20201013取消欄位
        (sum(isnull([LS_N],0))+sum(isnull([BNK_R],0)))*-1 AS '卡片停止使用',  --卡片停止使用=記名卡掛失+聯名卡餘返
		--sum(isnull([PB_BNK],0))                     AS '餘返_特店回饋'  ,     --20200518 易任 加入 --20201013取消欄位
		sum(isnull([CP],0))
		+sum(isnull([22],0))+sum(isnull([23],0))
		+sum(isnull([21],0))+sum(isnull([24],0))
		+sum(isnull([74],0))+sum(isnull([77],0))+sum(isnull([75],0))+sum(isnull([74_OL],0))		--bala add 20150318 (add by 20160415 rita 新增 77離線自動加值)
		+sum(isnull([CL],0))+sum(isnull([CLI],0))                           --modify by Rita 20151207 加入ibon企業加值
		+sum(isnull([CN],0))
		+sum(isnull([CB_1],0))+sum(isnull([CB_2],0))+sum(isnull([CB_3],0))++sum(isnull([CB_4],0))
		+sum(isnull([I4],0))
        -sum(isnull([LS_N],0))-sum(isnull([BNK_R],0))-sum(isnull([74_DU],0)) - sum(isnull([75_DU],0)) AS	'估算D日餘額', --減74_DU 75_DU
		sum(isnull([CWORK],0))						AS	'D日活卡餘額',
		sum(isnull([CLOCK],0))						AS	'D日鎖卡餘額',
		sum(isnull([CS],0))							AS	'D日卡務餘額',	 	--卡務TOTAL
		case when TOTAL_DATE <>'' then 
		(
				sum(isnull([CP],0))
				+sum(isnull([22],0))+sum(isnull([23],0))
				+sum(isnull([21],0))+sum(isnull([24],0))
				+sum(isnull([74],0))+sum(isnull([77],0))+sum(isnull([75],0))+sum(isnull([74_OL],0))  --bala add 20150318 (add by 20160415 rita 新增 77離線自動加值)
				+sum(isnull([CL],0))+sum(isnull([CLI],0))                        --modify by Rita 20151207 加入ibon企業加值
				+sum(isnull([CN],0))
				+sum(isnull([CB_1],0))+sum(isnull([CB_2],0))+sum(isnull([CB_3],0))++sum(isnull([CB_4],0))
				+sum(isnull([I4],0))
                -sum(isnull([LS_N],0))-sum(isnull([BNK_R],0))-sum(isnull([74_DU],0)) - sum(isnull([75_DU],0))
		)- sum(isnull([CS],0))
		else 0
		END												AS	'總差異',  --減74_DU 75_DU
		sum(isnull([E00621N],0))						AS  'TXLOG重複消費總額',
		sum(isnull([E00622N],0))						AS  'TXLOG重複加值總額'
FROM
(	SELECT	
			UPLOAD_DATE TOTAL_DATE ,
			'' SETTLE_DATE,
			CPT_DATE,
			'' LOG_DATE,
			SETTLE_TYPE,SUM_AMT
	FROM CM_SETTLE_SUM_D
	WHERE LEFT(UPLOAD_DATE,6)= @GEN_YM	
) a
PIVOT(sum(SUM_AMT) FOR SETTLE_TYPE 
IN ([21],[22],[23],[24],[74],[77],[75],[74_OL],[74_DU],[75_DU],[I4],
	[CL],[CLI],[CN],[CB_1],[CB_2],[CB_3],[CB_4],
	[CWORK],[CLOCK],[CS],[CP],[CI],[E006],
	[E00621N],[E00622N],[LS_N],[BNK_R],[PB_BNK],[PB_CUS])) AS P1
	--[21N],[22N],[21NR],[22NR],[21NRM],[22NRM],
	--[SUM_J],[SUM_JR])) as p1
group by TOTAL_DATE--,SETTLE_DATE,CPT_DATE,UPLOAD_DATE
ORDER BY 1";
                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    sqlCmd.Parameters.Add("@GEN_YM", SqlDbType.VarChar).Value = start;

                    SqlDataAdapter adapter = new SqlDataAdapter(sqlCmd);

                    adapter.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                log.Debug(ex.Message);
            }
            return dt;
        }

        public DataTable Report0111D(string start)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();
                    string sqlText =
                            @"SELECT	ROW_NUMBER() OVER(ORDER BY CPT_DATE,SETTLE_DATE1,TOTAL_DATE)	AS '順序',
		TOTAL_DATE									AS '日期合計',
		SETTLE_DATE1									AS '會計日',
        CPT_DATE									AS '清算日',
        UPLOAD_DATE									AS '傳輸日',
        sum(isnull([CP],0))							AS '前日餘額',
		sum(isnull([22],0)) 						AS '現金加值',			--加項
		sum(isnull([23],0))-(select isnull(sum(isnull(UT_AMT,0)),0)+isnull(sum(isnull(UT3_AMT,0)),0) FROM AM_ISET_MERC_TRANS_UTLOG_D u where u.TRANS_TYPE='23' and u.SRC_FLG='TXLOG' and (LEFT(convert(varchar, dateadd(d,-1,cast(u.CPT_DATE as date)) ,112),8)=TOTAL_DATE or (LEFT(convert(varchar, dateadd(d,-1,cast(u.CPT_DATE as date)) ,112),8)=SETTLE_DATE1 and SETTLE_DATE1=UPLOAD_DATE))) 						AS '消費取消'		,	--加項
		sum(isnull([21],0))+(select isnull(sum(isnull(UT_AMT,0)),0)+isnull(sum(isnull(UT3_AMT,0)),0) FROM AM_ISET_MERC_TRANS_UTLOG_D u where u.TRANS_TYPE='21' and u.SRC_FLG='TXLOG' and (LEFT(convert(varchar, dateadd(d,-1,cast(u.CPT_DATE as date)) ,112),8)=TOTAL_DATE or (LEFT(convert(varchar, dateadd(d,-1,cast(u.CPT_DATE as date)) ,112),8)=SETTLE_DATE1 and SETTLE_DATE1=UPLOAD_DATE))) 						AS '消費'			,	--減項
		sum(isnull([24],0))							AS '加值取消'		,	--減項
        (select -isnull(sum(isnull(UT_AMT,0)),0)-isnull(sum(isnull(UT3_AMT,0)),0) FROM AM_ISET_MERC_TRANS_UTLOG_D u where u.TRANS_TYPE='21' and u.SRC_FLG='TXLOG' and (LEFT(convert(varchar, dateadd(d,-1,cast(u.CPT_DATE as date)) ,112),8)=TOTAL_DATE or (LEFT(convert(varchar, dateadd(d,-1,cast(u.CPT_DATE as date)) ,112),8)=SETTLE_DATE1 and SETTLE_DATE1=UPLOAD_DATE))) as '代收售' ,
        (select isnull(sum(isnull(UT_AMT,0)),0)+isnull(sum(isnull(UT3_AMT,0)),0) FROM AM_ISET_MERC_TRANS_UTLOG_D u where u.TRANS_TYPE='23' and u.SRC_FLG='TXLOG' and (LEFT(convert(varchar, dateadd(d,-1,cast(u.CPT_DATE as date)) ,112),8)=TOTAL_DATE or (LEFT(convert(varchar, dateadd(d,-1,cast(u.CPT_DATE as date)) ,112),8)=SETTLE_DATE1 and SETTLE_DATE1=UPLOAD_DATE))) as '代收售取消' ,
		sum(isnull([74],0))+sum(isnull([77],0))		AS '自動加值',			--加項 --bala  add 20150318 (add by 20160415 rita 新增 77離線自動加值)
		sum(isnull([75],0)) 						AS '代行授權',	--加項 --bala  add 20150318
		sum(isnull([74_OL],0)) 						AS '中心端自動加值'		,		
		-sum(isnull([74_DU],0)) - sum(isnull([75_DU],0))		AS '已下帳中心自動加值'		,--減項 add by rita 20150911
		sum(isnull([CL],0))+sum(isnull([CLI],0)) 	AS '企業加值'		,		        --modify by Rita 20151207 加入ibon企業加值
		--( sum(isnull([PB_BNK],0))+sum(isnull([PB_CUS],0)) )*(-1) AS '加值(回饋)', --20200518 易任 加入 --20201013取消欄位
		sum(isnull([CN],0))-sum(isnull([CI],0))		AS '客服申請加值'	,
		sum(isnull([CI],0))							AS 'IDOLLAR餘額轉置',		
		sum(isnull([CB_1],0))						AS '製卡損耗'		,	
		sum(isnull([CB_2],0))						AS '壞卡換新卡'		,			
		sum(isnull([CB_3],0))						AS '壞卡放棄'		,	
		sum(isnull([CB_4],0))						AS '退費讀卡失敗'	,
		sum(isnull([I4],0))							AS '退費'			,
		--sum(isnull([PB_CUS],0))                     AS '退費_特店回饋'  ,     --20200518 易任 加入  --20201013取消欄位
        (sum(isnull([LS_N],0))+sum(isnull([BNK_R],0)))*-1 AS '卡片停止使用',  --卡片停止使用=記名卡掛失+聯名卡餘返
		--sum(isnull([PB_BNK],0))                     AS '餘返_特店回饋'  ,     --20200518 易任 加入  --20201013取消欄位
		sum(isnull([CP],0))
		+sum(isnull([22],0))+sum(isnull([23],0))
		+sum(isnull([21],0))+sum(isnull([24],0))
		+sum(isnull([74],0))+sum(isnull([77],0))+sum(isnull([75],0))+sum(isnull([74_OL],0))   --bala add 20150318 (add by 20160415 rita 新增 77離線自動加值)
		+sum(isnull([CL],0))+sum(isnull([CLI],0))                         --modify by Rita 20151207 加入ibon企業加值
		+sum(isnull([CN],0))
		+sum(isnull([CB_1],0))+sum(isnull([CB_2],0))+sum(isnull([CB_3],0))++sum(isnull([CB_4],0))
		+sum(isnull([I4],0))
        -sum(isnull([LS_N],0))-sum(isnull([BNK_R],0))-sum(isnull([74_DU],0)) - sum(isnull([75_DU],0)) AS	'估算D日餘額',  --減74_DU 75_DU
		sum(isnull([CWORK],0))						AS	'D日活卡餘額',
		sum(isnull([CLOCK],0))						AS	'D日鎖卡餘額',
		sum(isnull([CS],0))							AS	'D日卡務餘額',	 	--卡務TOTAL
		case when TOTAL_DATE <>'1' then 
		(
				sum(isnull([CP],0))
				+sum(isnull([22],0))+sum(isnull([23],0))
				+sum(isnull([21],0))+sum(isnull([24],0))
				+sum(isnull([74],0))+sum(isnull([77],0))+sum(isnull([75],0))+sum(isnull([74_OL],0))   --bala add 20150318 (add by 20160415 rita 新增 77離線自動加值)
				+sum(isnull([CL],0))+sum(isnull([CLI],0))                         --modify by Rita 20151207 加入ibon企業加值
				+sum(isnull([CN],0))
				+sum(isnull([CB_1],0))+sum(isnull([CB_2],0))+sum(isnull([CB_3],0))++sum(isnull([CB_4],0))
				+sum(isnull([I4],0))
                -sum(isnull([LS_N],0))-sum(isnull([BNK_R],0))-sum(isnull([74_DU],0)) - sum(isnull([75_DU],0))
		)- sum(isnull([CS],0))
		else 0
		END												AS	'總差異', --減74_DU 75_DU
		sum(isnull([E00621N],0))						AS  'TXLOG重複消費總額',
		sum(isnull([E00622N],0))						AS  'TXLOG重複加值總額'
		--sum(isnull([22N],0))							AS 'CM不認加值額'	,	--減項
		--sum(isnull([21N],0))							AS 'CM不認消費額'	,	--加項		
		--sum(isnull([22NR],0))+sum(isnull([22NRM],0))	AS 'CM不認加值沖銷額'	,	--加項
		--sum(isnull([21NR],0))+sum(isnull([22NRM],0))	AS 'CM不認消費沖銷額'	,	--減項
		----sum(isnull([SUM_J],0))						AS '跳號金額'	,
		----sum(isnull([SUM_JR],0))						AS '跳號已回補金額'	,
		--case when TOTAL_DATE <>'' then 
		--(
		--		sum(isnull([CP],0))
		--		+sum(isnull([22],0))
		--		+sum(isnull([23],0))
		--		+sum(isnull([21],0))
		--		+sum(isnull([24],0))
		--		+sum(isnull([CL],0))
		--		+sum(isnull([CN],0))
		--		+sum(isnull([CB_1],0))+sum(isnull([CB_2],0))+sum(isnull([CB_3],0))++sum(isnull([CB_4],0))
		--		+sum(isnull([I4],0))
		--)-sum(isnull([CS],0))-
		--(sum(isnull([22N],0))+
		--sum(isnull([21N],0))+
		--sum(isnull([22NR],0))+sum(isnull([22NRM],0))+
		--sum(isnull([21NR],0))+sum(isnull([22NRM],0)))
		----+
		----sum(isnull([SUM_J],0))+
		----sum(isnull([SUM_JR],0)))					
		--else 0
		--END											AS '帳差'
				
FROM
(
	SELECT	'' TOTAL_DATE,
			SETTLE_DATE SETTLE_DATE1,
			CPT_DATE,
			UPLOAD_DATE,
			SETTLE_TYPE,SUM_AMT
	FROM CM_SETTLE_SUM_D
	WHERE UPLOAD_DATE Like @GEN_YM
	and SETTLE_TYPE in ('21','22','23','24',
						'74','77','75','74_OL','74_DU','75_DU',				 --bala add 20150318 (add by 20160415 rita 新增 77離線自動加值)
						'I4','CL','CLI','CN',
						'CB_1','CB_2','CB_3','CB_4','CWORK','CLOCK','CS','CP','CI','E006',
						'E00621N','E00622N','LS_N','BNK_R')
						--'21N','22N','21NR','22NR','21NRM','22NRM','PB_BNK','PB_CUS')
	--union all
	--SELECT	CPT_DATE TOTAL_DATE ,
	--		'' SETTLE_DATE,
	--		CPT_DATE,
	--		'' LOG_DATE,
	--		SETTLE_TYPE,SUM(SUM_AMT)
	--FROM CM_SETTLE_SUM_D 
	--WHERE  SETTLE_TYPE in ('SUM_J','SUM_JR')
	--AND CPT_DATE=@GEN_DATE
	--GROUP BY CPT_DATE,SETTLE_TYPE
	union all	
	SELECT	
			UPLOAD_DATE TOTAL_DATE ,
			'' SETTLE_DATE1,
			CPT_DATE,
			'' LOG_DATE,
			SETTLE_TYPE,SUM_AMT
	FROM CM_SETTLE_SUM_D
	WHERE UPLOAD_DATE Like @GEN_YM	
) a
PIVOT(sum(SUM_AMT) FOR SETTLE_TYPE 
IN ([21],[22],[23],[24],
	[74],[77],[75],[74_OL],[74_DU],[75_DU],				 --bala add 20150318 (add by 20160415 rita 新增 77離線自動加值)
	[I4],
	[CL],[CLI],[CN],[CB_1],[CB_2],[CB_3],[CB_4],
	[CWORK],[CLOCK],[CS],[CP],[CI],[E006],
	[E00621N],[E00622N],[LS_N],[BNK_R])) AS P1
	--[21N],[22N],[21NR],[22NR],[21NRM],[22NRM],
	--[SUM_J],[SUM_JR]
    --[PB_BNK],[PB_CUS])) as p1
group by TOTAL_DATE,SETTLE_DATE1,CPT_DATE,UPLOAD_DATE
ORDER BY 1;";

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    //sqlCmd.Parameters.Add("@GEN_YM", SqlDbType.VarChar).Value = start;
                    sqlCmd.Parameters.AddWithValue("@GEN_YM", String.Format("{0}%", start));

                    SqlDataAdapter adapter = new SqlDataAdapter(sqlCmd);

                    adapter.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                log.Debug(ex.Message);
            }
            return dt;
        }

        public DataTable Report0811(string start, string repId)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();
                    string sqlText =
                            @"SELECT  REP_HEADER,					--傳票單別
		ACCT_DATE,
		'X'+SUBSTRING(CPT_DATE,3,6)+'001',
		REP_ID,
		REPLICATE('0',4-LEN(CAST(ROW_NUMBER() OVER (ORDER BY REP_HEADER, REP_SEQ, REP_ID, ACCT_ENTRY1)as varchar(4))))
			+ CAST(ROW_NUMBER() OVER (ORDER BY REP_SEQ, ACCT_ENTRY1)as varchar(4)) ,	--單身序號
		ACCT_NO, 						--科目代號
		'', 							--部門
		C_D,							--借貸別
		ACCT_AMT,						--原幣金額
		ACCT_ENTRY1,					--立沖帳目1
		ACCT_ENTRY2,					--立沖帳目2
		CURRENCY,						--幣別
		CUR_RATE,						--匯率
		ACCT_SET_GROUP_NO,				--關係人代號
		ACCT_REMARK						--摘要
		FROM GM_ACCT_SUM_DETAIL 
		WHERE REP_KIND='01'
		AND REP_ID=@REP_ID
		AND CPT_DATE=@cptDate
	ORDER BY REP_SEQ";
                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    sqlCmd.Parameters.Add("@cptDate", SqlDbType.VarChar).Value = start;
                    sqlCmd.Parameters.Add("@REP_ID", SqlDbType.VarChar).Value = repId;

                    SqlDataAdapter adapter = new SqlDataAdapter(sqlCmd);

                    adapter.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                log.Debug(ex.Message);
            }
            return dt;
        }

        public DataTable Report0112T(string start, string end, string groupId, string merchantNo)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();
                    string sqlText = "";

                    #region SQL設定
                    sqlText = String.Format(@"
SET NOCOUNT ON;

--DECLARE @DATE_B VARCHAR(8) = '20220221'
--DECLARE @DATE_E VARCHAR(8) = '20220221'
--DECLARE @GROUP_ID VARCHAR(20) = 'ALL'
--DECLARE @MERCHANT_NO VARCHAR(20) = 'ALL'

DECLARE @DATE_TARGET VARCHAR(8) = @DATE_B
DECLARE @T_DATE_TARGET TABLE (CPT_DATE VARCHAR(8))

WHILE (@DATE_TARGET <= @DATE_E) BEGIN
	INSERT @T_DATE_TARGET
	SELECT @DATE_TARGET

	SET @DATE_TARGET = CONVERT(VARCHAR(8),DATEADD(D,1,@DATE_TARGET),112)
END


SELECT DT.CPT_DATE [清算日]
	,GM.MERCHANT_NAME [列標籤]
	,D.MERCHANT_NO [特店代碼]
	,GA.MERCHANT_NO_ACT [門市代號]
	,NULL [:]
	,SUM(ISNULL(T.LOAD_AMT,0)) [加值]
	,SUM(ISNULL(T.PCHR_AMT,0) - ISNULL(U.UTR_AMT,0) - ISNULL(UT3R_AMT,0)) [購貨取消]
	,SUM(ISNULL(T.PCH_AMT,0) - ISNULL(U.UT_AMT,0) - ISNULL(U.UT3_AMT,0)) [購貨]
	,SUM(ISNULL(T.LOADR_AMT,0)) [加值取消]
	,SUM(ISNULL(T.LOAD_AMT,0) - ISNULL(T.LOADR_AMT,0)) [加值淨額]
	,SUM(ISNULL(UT_AMT,0) + ISNULL(UT3_AMT,0)) [代收售]
	,SUM(ISNULL(UTR_AMT,0) + ISNULL(UT3R_AMT,0)) [代收售取消]
	,SUM(ISNULL(T.PCH_AMT,0) - ISNULL(T.PCHR_AMT,0)) [購貨及代收售淨額]
	,SUM(ISNULL(T.PCH_AMT,0) - ISNULL(T.PCHR_AMT,0) - ISNULL(T.LOAD_AMT,0) + ISNULL(T.LOADR_AMT,0)) [特約機構總淨額]
	,SUM(ISNULL(B.LOAD74_AMT,0)) [自動加值]
	,SUM(ISNULL(B.LOAD75_AMT,0)) [代行授權自動加值]
	,SUM(ISNULL(A.BULOAD79_AMT,0)) [企業加值]
	,NULL [MERCHANT_TYPE]
	,NULL [MERC_GROUP]
	,NULL [MERCHANT_NO]
	,NULL [SAM_TYPE]
FROM dbo.GM_MERCHANT_TYPE_D D --依選單篩選
INNER JOIN dbo.GM_MERCHANT GM
	ON D.SHOW_FLG = 'Y'
	AND D.GROUP_ID <> 'MSTORE'
	AND D.MERCHANT_NO = GM.MERCHANT_NO
	AND D.GROUP_ID = CASE WHEN @GROUP_ID='ALL' THEN D.GROUP_ID ELSE @GROUP_ID END
	AND D.MERCHANT_NO = CASE WHEN @MERCHANT_NO='ALL' THEN D.MERCHANT_NO ELSE @MERCHANT_NO END
CROSS JOIN @T_DATE_TARGET DT --清分日期
LEFT JOIN dbo.GM_MERCHANT_ACT GA --財會工作底稿門市代號
	ON GA.MERCHANT_NO = D.MERCHANT_NO
LEFT JOIN (
	--交易-金額
	SELECT MERCHANT_NO,CPT_DATE
		,SUM(PCH_AMT) [PCH_AMT]
		,SUM(PCHR_AMT) [PCHR_AMT]
		,SUM(LOAD_AMT) [LOAD_AMT]
		,SUM(LOADR_AMT) [LOADR_AMT]
	FROM dbo.AM_ISET_MERC_TRANS_LOG_D 
	WHERE CPT_DATE BETWEEN @DATE_B AND @DATE_E
		AND SRC_FLG = 'TXLOG'
		AND MERCHANT_NO = CASE WHEN @MERCHANT_NO='ALL' THEN MERCHANT_NO ELSE @MERCHANT_NO END
	GROUP BY MERCHANT_NO,CPT_DATE
) T 
	ON T.MERCHANT_NO = D.MERCHANT_NO
	AND T.CPT_DATE = DT.CPT_DATE
LEFT JOIN (
	--交易-代收售
	SELECT MERCHANT_NO,CPT_DATE
		,SUM(CASE WHEN TRANS_TYPE='21' THEN UT_AMT ELSE 0 END) [UT_AMT]
		,SUM(CASE WHEN TRANS_TYPE='21' THEN UT3_AMT ELSE 0 END) [UT3_AMT]
		,SUM(CASE WHEN TRANS_TYPE='23' THEN UT_AMT ELSE 0 END) [UTR_AMT]
		,SUM(CASE WHEN TRANS_TYPE='23' THEN UT3_AMT ELSE 0 END) [UT3R_AMT]
	FROM dbo.AM_ISET_MERC_TRANS_UTLOG_D
	WHERE CPT_DATE BETWEEN @DATE_B AND @DATE_E
		AND SRC_FLG = 'TXLOG'
		AND MERCHANT_NO = CASE WHEN @MERCHANT_NO='ALL' THEN MERCHANT_NO ELSE @MERCHANT_NO END
	GROUP BY MERCHANT_NO,CPT_DATE
) U 
	ON U.MERCHANT_NO = D.MERCHANT_NO
	AND U.CPT_DATE = DT.CPT_DATE
LEFT JOIN (
	--交易-自動加值
	SELECT MERCHANT_NO,CPT_DATE
		,SUM(LOAD74_AMT+ISNULL(LOAD77_AMT,0)) [LOAD74_AMT]
		,SUM(LOAD75_AMT) [LOAD75_AMT]
	FROM dbo.GM_BANK_CPT_SUM
	WHERE CPT_DATE BETWEEN @DATE_B AND @DATE_E
		AND MERCHANT_NO = CASE WHEN @MERCHANT_NO='ALL' THEN MERCHANT_NO ELSE @MERCHANT_NO END
	GROUP BY MERCHANT_NO,CPT_DATE
) B 
	ON B.MERCHANT_NO = D.MERCHANT_NO
	AND B.CPT_DATE = DT.CPT_DATE
LEFT JOIN (
	--交易-企業禮金
	SELECT MERCHANT_NO,CPT_DATE
		,SUM(BULOAD79_AMT) [BULOAD79_AMT]
	FROM dbo.GM_CPT_SUM_AUTO_D
	WHERE CPT_DATE BETWEEN @DATE_B AND @DATE_E
		AND MERCHANT_NO = CASE WHEN @MERCHANT_NO='ALL' THEN MERCHANT_NO ELSE @MERCHANT_NO END
	GROUP BY MERCHANT_NO,CPT_DATE
) A 
	ON A.MERCHANT_NO = D.MERCHANT_NO
	AND A.CPT_DATE = DT.CPT_DATE
GROUP BY DT.CPT_DATE,D.MERCHANT_NO,GM.MERCHANT_NAME,GA.MERCHANT_NO_ACT
ORDER BY DT.CPT_DATE,D.MERCHANT_NO
");
                    #endregion SQL設定

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    sqlCmd.Parameters.Add("@DATE_B", SqlDbType.VarChar).Value = start;
                    sqlCmd.Parameters.Add("@DATE_E", SqlDbType.VarChar).Value = end;
                    sqlCmd.Parameters.Add("@GROUP_ID", SqlDbType.VarChar).Value = groupId;
                    sqlCmd.Parameters.Add("@MERCHANT_NO", SqlDbType.VarChar).Value = merchantNo;
                    
                    SqlDataAdapter adapter = new SqlDataAdapter(sqlCmd);

                    adapter.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                log.Debug(ex.Message);
            }
            return dt;
        }

        public DataTable Report0113T(string start, string end, string merchantNo)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();
                    string sqlText =
                            @"SELECT 
GM.MERCHANT_STNAME as 特約機構,
isnull(G.TPCHA,0)				購貨		,
isnull(G.TPCHRA,0)			購貨取消	,
isnull(G.TPCHA-G.TPCHRA,0)	購貨淨額	
FROM
GM_MERCHANT GM LEFT OUTER JOIN
(
SELECT 
T.CPT_DATE,
T.MERCHANT_NO,
ISNULL(T.PCHA	,0)		TPCHA	,
ISNULL(O.PCHA	,0)		OPCHA	,
ISNULL(T.PCHC	,0)		TPCHC	,
ISNULL(O.PCHC	,0)		OPCHC	,
ISNULL(T.PCHRA	,0)		TPCHRA	,
ISNULL(O.PCHRA	,0)		OPCHRA	,
ISNULL(T.PCHRC	,0)		TPCHRC	,
ISNULL(O.PCHRC	,0)		OPCHRC	,
ISNULL(T.LOADA	,0)		TLOADA	,
ISNULL(O.LOADA	,0)		OLOADA	,
ISNULL(T.LOADC	,0)		TLOADC	,
ISNULL(O.LOADC	,0)		OLOADC	,
ISNULL(T.LOADRA	,0)		TLOADRA	,
ISNULL(O.LOADRA	,0)		OLOADRA	,
ISNULL(T.LOADRC	,0)		TLOADRC	,
ISNULL(O.LOADRC ,0)	   	OLOADRC
FROM
(SELECT       '合計' as CPT_DATE,
              MERCHANT_NO,
              SUM(isnull(PCH_AMT  ,0))	PCHA,
              SUM(isnull(PCH_CNT  ,0))	PCHC ,
              SUM(isnull(PCHR_AMT ,0)) 	PCHRA,
              SUM(isnull(PCHR_CNT ,0))	PCHRC,
              SUM(isnull(LOAD_AMT ,0)) 	LOADA,
              SUM(isnull(LOAD_CNT ,0))	LOADC,
              SUM(isnull(LOADR_AMT,0))	LOADRA,
              SUM(isnull(LOADR_CNT,0))	LOADRC
FROM AM_ISET_MERC_TRANS_LOG_D
WHERE  SRC_FLG='TXLOG' and CPT_DATE BETWEEN @Sdate AND @Edate 
GROUP BY MERCHANT_NO) T LEFT OUTER JOIN
(SELECT       '合計' as CPT_DATE,
              MERCHANT_NO,
              SUM(isnull(PCH_AMT  ,0))	PCHA,
              SUM(isnull(PCH_CNT  ,0))	PCHC ,
              SUM(isnull(PCHR_AMT ,0)) 	PCHRA,
              SUM(isnull(PCHR_CNT ,0))	PCHRC,
              SUM(isnull(LOAD_AMT ,0)) 	LOADA,
              SUM(isnull(LOAD_CNT ,0))	LOADC,
              SUM(isnull(LOADR_AMT,0))	LOADRA,
              SUM(isnull(LOADR_CNT,0))	LOADRC
FROM AM_ISET_MERC_TRANS_LOG_D
WHERE SRC_FLG='TXLOG' and CPT_DATE BETWEEN @Sdate AND @Edate 
GROUP BY MERCHANT_NO) O
ON  T.merchant_no=O.merchant_no
AND CONVERT(VARCHAR,CONVERT(DATETIME,T.CPT_DATE)-1,112)= O.CPT_DATE) G
ON GM.MERCHANT_NO=G.MERCHANT_NO    
WHERE 1=1 ";
//and G.MERCHANT_NO in ('22555003','27359304','28070701','28459597','70790935','86379116','89589328','89627033','89798198','21242047')";
                    if (merchantNo != "")
                    {
                        sqlText += "AND GM.MERCHANT_NO in (" + merchantNo + ") ";
                    }
                    sqlText += "ORDER BY GM.MERCHANT_TYPE,GM.SAM_TYPE desc,GM.MERC_GROUP desc,GM.MERCHANT_NO,2";

                    //order by MERCHANT_TYPE,MERC_GROUP desc,MERCHANT_NO 
                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    sqlCmd.Parameters.Add("@Sdate", SqlDbType.VarChar).Value = start;
                    sqlCmd.Parameters.Add("@Edate", SqlDbType.VarChar).Value = end;
                    sqlCmd.Parameters.Add("@MERCHANT_NO", SqlDbType.VarChar).Value = merchantNo;

                    SqlDataAdapter adapter = new SqlDataAdapter(sqlCmd);

                    adapter.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                log.Debug(ex.Message);
            }
            return dt;
        }

        public DataTable Report0114T(string start, string end, string merchantNo)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();
                    string sqlText =
                            @"SELECT 
GM.MERCHANT_STNAME as 特約機構,
isnull(G.TPCHC,0)				購貨		,
isnull(G.TPCHRC,0)			購貨取消	,
isnull(G.TPCHC-G.TPCHRC,0)	購貨淨額	
FROM
GM_MERCHANT GM LEFT OUTER JOIN
(
SELECT 
T.CPT_DATE,
T.MERCHANT_NO,
ISNULL(T.PCHA	,0)		TPCHA	,
ISNULL(O.PCHA	,0)		OPCHA	,
ISNULL(T.PCHC	,0)		TPCHC	,
ISNULL(O.PCHC	,0)		OPCHC	,
ISNULL(T.PCHRA	,0)		TPCHRA	,
ISNULL(O.PCHRA	,0)		OPCHRA	,
ISNULL(T.PCHRC	,0)		TPCHRC	,
ISNULL(O.PCHRC	,0)		OPCHRC	,
ISNULL(T.LOADA	,0)		TLOADA	,
ISNULL(O.LOADA	,0)		OLOADA	,
ISNULL(T.LOADC	,0)		TLOADC	,
ISNULL(O.LOADC	,0)		OLOADC	,
ISNULL(T.LOADRA	,0)		TLOADRA	,
ISNULL(O.LOADRA	,0)		OLOADRA	,
ISNULL(T.LOADRC	,0)		TLOADRC	,
ISNULL(O.LOADRC ,0)	   	OLOADRC
FROM
(SELECT       '合計' as CPT_DATE,
              MERCHANT_NO,
              SUM(isnull(PCH_AMT  ,0))	PCHA,
              SUM(isnull(PCH_CNT  ,0))	PCHC ,
              SUM(isnull(PCHR_AMT ,0)) 	PCHRA,
              SUM(isnull(PCHR_CNT ,0))	PCHRC,
              SUM(isnull(LOAD_AMT ,0)) 	LOADA,
              SUM(isnull(LOAD_CNT ,0))	LOADC,
              SUM(isnull(LOADR_AMT,0))	LOADRA,
              SUM(isnull(LOADR_CNT,0))	LOADRC
FROM AM_ISET_MERC_TRANS_LOG_D
WHERE  SRC_FLG='TXLOG' and CPT_DATE BETWEEN @Sdate AND @Edate 
GROUP BY MERCHANT_NO) T LEFT OUTER JOIN
(SELECT       '合計' as CPT_DATE,
              MERCHANT_NO,
              SUM(isnull(PCH_AMT  ,0))	PCHA,
              SUM(isnull(PCH_CNT  ,0))	PCHC ,
              SUM(isnull(PCHR_AMT ,0)) 	PCHRA,
              SUM(isnull(PCHR_CNT ,0))	PCHRC,
              SUM(isnull(LOAD_AMT ,0)) 	LOADA,
              SUM(isnull(LOAD_CNT ,0))	LOADC,
              SUM(isnull(LOADR_AMT,0))	LOADRA,
              SUM(isnull(LOADR_CNT,0))	LOADRC
FROM AM_ISET_MERC_TRANS_LOG_D
WHERE SRC_FLG='TXLOG' and CPT_DATE BETWEEN @Sdate AND @Edate 
GROUP BY MERCHANT_NO) O
ON  T.merchant_no=O.merchant_no
AND CONVERT(VARCHAR,CONVERT(DATETIME,T.CPT_DATE)-1,112)= O.CPT_DATE) G
ON GM.MERCHANT_NO=G.MERCHANT_NO    
WHERE 1=1 ";
//and G.MERCHANT_NO in ('22555003','27359304','28070701','28459597','70790935','86379116','89589328','89627033','89798198','21242047')";
                    if (merchantNo != "")
                    {
                        sqlText += "AND GM.MERCHANT_NO in (" + merchantNo + ") ";
                    }
                    sqlText += "ORDER BY GM.MERCHANT_TYPE,GM.SAM_TYPE desc,GM.MERC_GROUP desc,GM.MERCHANT_NO,2";

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    sqlCmd.Parameters.Add("@Sdate", SqlDbType.VarChar).Value = start;
                    sqlCmd.Parameters.Add("@Edate", SqlDbType.VarChar).Value = end;
                    sqlCmd.Parameters.Add("@MERCHANT_NO", SqlDbType.VarChar).Value = merchantNo;

                    SqlDataAdapter adapter = new SqlDataAdapter(sqlCmd);

                    adapter.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                log.Debug(ex.Message);
            }
            return dt;
        }

        public DataTable Report0115T(string start, string end, string merchantNo)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();
                    string sqlText =
                            @"SELECT 
G.CPT_DATE as 清算日期,
G.TPCHA-G.TPCHRA	購貨淨額	,
G.TLOADA-G.TLOADRA	加值淨額	
FROM
(
SELECT 
T.CPT_DATE,
T.MERCHANT_NO,
ISNULL(T.PCHA	,0)		TPCHA	,
ISNULL(O.PCHA	,0)		OPCHA	,
ISNULL(T.PCHC	,0)		TPCHC	,
ISNULL(O.PCHC	,0)		OPCHC	,
ISNULL(T.PCHRA	,0)		TPCHRA	,
ISNULL(O.PCHRA	,0)		OPCHRA	,
ISNULL(T.PCHRC	,0)		TPCHRC	,
ISNULL(O.PCHRC	,0)		OPCHRC	,
ISNULL(T.LOADA	,0)		TLOADA	,
ISNULL(O.LOADA	,0)		OLOADA	,
ISNULL(T.LOADC	,0)		TLOADC	,
ISNULL(O.LOADC	,0)		OLOADC	,
ISNULL(T.LOADRA	,0)		TLOADRA	,
ISNULL(O.LOADRA	,0)		OLOADRA	,
ISNULL(T.LOADRC	,0)		TLOADRC	,
ISNULL(O.LOADRC ,0)	   	OLOADRC
FROM
(SELECT       CPT_DATE as CPT_DATE,
              '' as MERCHANT_NO,
              SUM(isnull(PCH_AMT  ,0))	PCHA,
              SUM(isnull(PCH_CNT  ,0))	PCHC ,
              SUM(isnull(PCHR_AMT ,0)) 	PCHRA,
              SUM(isnull(PCHR_CNT ,0))	PCHRC,
              SUM(isnull(LOAD_AMT ,0)) 	LOADA,
              SUM(isnull(LOAD_CNT ,0))	LOADC,
              SUM(isnull(LOADR_AMT,0))	LOADRA,
              SUM(isnull(LOADR_CNT,0))	LOADRC
FROM AM_ISET_MERC_TRANS_LOG_D
WHERE  SRC_FLG='TXLOG' and CPT_DATE BETWEEN @Sdate AND @Edate 
and 1=1 "; //MERCHANT_NO in ('22555003','27359304','28070701','28459597','70790935','86379116','89589328','89627033','89798198','21242047')";
                    if (merchantNo != "")
                    {
                        sqlText += "AND MERCHANT_NO=@MERCHANT_NO ";
                    }
sqlText += @"GROUP BY CPT_DATE) T LEFT OUTER JOIN
(SELECT       CPT_DATE as CPT_DATE,
              '' as MERCHANT_NO,
              SUM(isnull(PCH_AMT  ,0))	PCHA,
              SUM(isnull(PCH_CNT  ,0))	PCHC ,
              SUM(isnull(PCHR_AMT ,0)) 	PCHRA,
              SUM(isnull(PCHR_CNT ,0))	PCHRC,
              SUM(isnull(LOAD_AMT ,0)) 	LOADA,
              SUM(isnull(LOAD_CNT ,0))	LOADC,
              SUM(isnull(LOADR_AMT,0))	LOADRA,
              SUM(isnull(LOADR_CNT,0))	LOADRC
FROM AM_ISET_MERC_TRANS_LOG_D
WHERE SRC_FLG='TXLOG' and CPT_DATE BETWEEN @Sdate AND @Edate
and 1=1 "; //MERCHANT_NO in ('22555003','27359304','28070701','28459597','70790935','86379116','89589328','89627033','89798198','21242047')";
                    if (merchantNo != "")
                    {
                        sqlText += "AND MERCHANT_NO=@MERCHANT_NO ";
                    }
sqlText += @"GROUP BY CPT_DATE) O
ON  T.CPT_DATE=O.CPT_DATE
AND CONVERT(VARCHAR,CONVERT(DATETIME,T.CPT_DATE)-1,112)= O.CPT_DATE) G
where 1=1";

//                    sqlText += "ORDER BY 1,2";

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    sqlCmd.Parameters.Add("@Sdate", SqlDbType.VarChar).Value = start;
                    sqlCmd.Parameters.Add("@Edate", SqlDbType.VarChar).Value = end;
                    sqlCmd.Parameters.Add("@MERCHANT_NO", SqlDbType.VarChar).Value = merchantNo;

                    SqlDataAdapter adapter = new SqlDataAdapter(sqlCmd);

                    adapter.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                log.Debug(ex.Message);
            }
            return dt;
        }

        public DataTable Report0116T(string start, string end, string merchantNo)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();
                    string sqlText =
                            @"SELECT 
G.CPT_DATE as 清算日期,
G.TPCHC-G.TPCHRC	購貨數量	,
G.TLOADC-G.TLOADRC	加值數量	
FROM
(
SELECT 
T.CPT_DATE,
T.MERCHANT_NO,
ISNULL(T.PCHA	,0)		TPCHA	,
ISNULL(O.PCHA	,0)		OPCHA	,
ISNULL(T.PCHC	,0)		TPCHC	,
ISNULL(O.PCHC	,0)		OPCHC	,
ISNULL(T.PCHRA	,0)		TPCHRA	,
ISNULL(O.PCHRA	,0)		OPCHRA	,
ISNULL(T.PCHRC	,0)		TPCHRC	,
ISNULL(O.PCHRC	,0)		OPCHRC	,
ISNULL(T.LOADA	,0)		TLOADA	,
ISNULL(O.LOADA	,0)		OLOADA	,
ISNULL(T.LOADC	,0)		TLOADC	,
ISNULL(O.LOADC	,0)		OLOADC	,
ISNULL(T.LOADRA	,0)		TLOADRA	,
ISNULL(O.LOADRA	,0)		OLOADRA	,
ISNULL(T.LOADRC	,0)		TLOADRC	,
ISNULL(O.LOADRC ,0)	   	OLOADRC
FROM
(SELECT       CPT_DATE as CPT_DATE,
              '' as MERCHANT_NO,
              SUM(isnull(PCH_AMT  ,0))	PCHA,
              SUM(isnull(PCH_CNT  ,0))	PCHC ,
              SUM(isnull(PCHR_AMT ,0)) 	PCHRA,
              SUM(isnull(PCHR_CNT ,0))	PCHRC,
              SUM(isnull(LOAD_AMT ,0)) 	LOADA,
              SUM(isnull(LOAD_CNT ,0))	LOADC,
              SUM(isnull(LOADR_AMT,0))	LOADRA,
              SUM(isnull(LOADR_CNT,0))	LOADRC
FROM AM_ISET_MERC_TRANS_LOG_D
WHERE  SRC_FLG='TXLOG' and CPT_DATE BETWEEN @Sdate AND @Edate 
and 1=1 "; //MERCHANT_NO in ('22555003','27359304','28070701','28459597','70790935','86379116','89589328','89627033','89798198','21242047')";
                    if (merchantNo != "")
                    {
                        sqlText += "AND MERCHANT_NO=@MERCHANT_NO ";
                    }
                    sqlText += @"GROUP BY CPT_DATE) T LEFT OUTER JOIN
(SELECT       CPT_DATE as CPT_DATE,
              '' as MERCHANT_NO,
              SUM(isnull(PCH_AMT  ,0))	PCHA,
              SUM(isnull(PCH_CNT  ,0))	PCHC ,
              SUM(isnull(PCHR_AMT ,0)) 	PCHRA,
              SUM(isnull(PCHR_CNT ,0))	PCHRC,
              SUM(isnull(LOAD_AMT ,0)) 	LOADA,
              SUM(isnull(LOAD_CNT ,0))	LOADC,
              SUM(isnull(LOADR_AMT,0))	LOADRA,
              SUM(isnull(LOADR_CNT,0))	LOADRC
FROM AM_ISET_MERC_TRANS_LOG_D
WHERE SRC_FLG='TXLOG' and CPT_DATE BETWEEN @Sdate AND @Edate
and 1=1 "; //MERCHANT_NO in ('22555003','27359304','28070701','28459597','70790935','86379116','89589328','89627033','89798198','21242047')";
                    if (merchantNo != "")
                    {
                        sqlText += "AND MERCHANT_NO=@MERCHANT_NO ";
                    }
                    sqlText += @"GROUP BY CPT_DATE) O
ON  T.CPT_DATE=O.CPT_DATE
AND CONVERT(VARCHAR,CONVERT(DATETIME,T.CPT_DATE)-1,112)= O.CPT_DATE) G
where 1=1";

                    //                    sqlText += "ORDER BY 1,2";

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    sqlCmd.Parameters.Add("@Sdate", SqlDbType.VarChar).Value = start;
                    sqlCmd.Parameters.Add("@Edate", SqlDbType.VarChar).Value = end;
                    sqlCmd.Parameters.Add("@MERCHANT_NO", SqlDbType.VarChar).Value = merchantNo;

                    SqlDataAdapter adapter = new SqlDataAdapter(sqlCmd);

                    adapter.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                log.Debug(ex.Message);
            }
            return dt;
        }

        public DataTable Report0117T(string yearMonth, string merchantNo)
        {
            int startM = 0;
            string startD = "";
            int endM = 0;
            string endD = "";

            string start = "";
            string end = "";

            List<string[]> startEndList = new List<string[]>();

            List<string> merchantNoList = new List<string>();
                foreach (GmMerchant merch in this.GmDAO.FindAll())
                {
                    merchantNoList.Add(merch.MerchantNo);
                }
            
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {


                    sqlConn.Open();
                    if (merchantNo != "")
                    {
                        string preQuery = string.Format(@"SELECT SUM_MON_S,SUM_DAY_S,SUM_MON_E,SUM_DAY_E FROM GM_CONTRACT_M WHERE MERCHANT_NO='{0}'", merchantNo);
                        SqlCommand sqlCmd = new SqlCommand(preQuery, sqlConn);
                        SqlDataReader reader = sqlCmd.ExecuteReader();
                        while (reader.Read())
                        {
                            startM = Convert.ToInt32(reader["SUM_MON_S"]);
                            startD = reader["SUM_DAY_S"].ToString();
                            endM = Convert.ToInt32(reader["SUM_MON_E"].ToString());
                            endD = reader["SUM_DAY_E"].ToString();
                        }
                        reader.Close();
                        reader.Dispose();
                        sqlCmd.Dispose();

                        

                        int diff = 0;

                        switch (startM)
                        {
                            case -1:
                                start = DateTime.ParseExact(yearMonth + startD, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture).ToString("yyyyMMdd");
                                break;

                            case 0:
                                start = DateTime.ParseExact(yearMonth + startD, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture).AddMonths(1).ToString("yyyyMMdd");
                                break;

                            default:
                                start = "";
                                break;
                        }


                        if (endD == "99")
                        {
                            switch (endM)
                            {
                                case -1:
                                    DateTime tempN1 = DateTime.ParseExact(yearMonth + "01", "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);
                                    diff = DateTime.DaysInMonth(tempN1.Year, tempN1.Month) - 1;

                                    end = tempN1.AddDays(diff).ToString("yyyyMMdd");
                                    break;

                                case 0:
                                    DateTime temp00 = DateTime.ParseExact(yearMonth + "01", "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture).AddMonths(1);
                                    diff = DateTime.DaysInMonth(temp00.Year, temp00.Month) - 1;

                                    end = temp00.AddDays(diff).ToString("yyyyMMdd");
                                    break;

                                default:
                                    end = "";
                                    break;
                            }
                        }
                        else
                        {
                            switch (endM)
                            {
                                case -1:
                                    end = DateTime.ParseExact(yearMonth + endD, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture).ToString("yyyyMMdd");
                                    break;

                                case 0:
                                    end = DateTime.ParseExact(yearMonth + endD, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture).AddMonths(1).ToString("yyyyMMdd");
                                    break;

                                default:
                                    end = "";
                                    break;
                            }
                        }
                    }
                    else
                    {
                        

                        foreach(string mNo in merchantNoList)
                        {
                            string preQuery = string.Format(@"SELECT SUM_MON_S,SUM_DAY_S,SUM_MON_E,SUM_DAY_E FROM GM_CONTRACT_M WHERE MERCHANT_NO='{0}'", mNo);
                            SqlCommand sqlCmd = new SqlCommand(preQuery, sqlConn);
                            SqlDataReader reader = sqlCmd.ExecuteReader();
                            while (reader.Read())
                            {
                                startM = Convert.ToInt32(reader["SUM_MON_S"]);
                                startD = reader["SUM_DAY_S"].ToString();
                                endM = Convert.ToInt32(reader["SUM_MON_E"].ToString());
                                endD = reader["SUM_DAY_E"].ToString();
                            }
                            reader.Close();
                            reader.Dispose();
                            sqlCmd.Dispose();

                            int diff = 0;

                            switch (startM)
                            {
                                case -1:
                                    start = DateTime.ParseExact(yearMonth + startD, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture).ToString("yyyyMMdd");
                                    break;

                                case 0:
                                    start = DateTime.ParseExact(yearMonth + startD, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture).AddMonths(1).ToString("yyyyMMdd");
                                    break;

                                default:
                                    start = "";
                                    break;
                            }


                            if (endD == "99")
                            {
                                switch (endM)
                                {
                                    case -1:
                                        DateTime tempN1 = DateTime.ParseExact(yearMonth + "01", "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);
                                        diff = DateTime.DaysInMonth(tempN1.Year, tempN1.Month) - 1;

                                        end = tempN1.AddDays(diff).ToString("yyyyMMdd");
                                        break;

                                    case 0:
                                        DateTime temp00 = DateTime.ParseExact(yearMonth + "01", "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture).AddMonths(1);
                                        diff = DateTime.DaysInMonth(temp00.Year, temp00.Month) - 1;

                                        end = temp00.AddDays(diff).ToString("yyyyMMdd");
                                        break;

                                    default:
                                        end = "";
                                        break;
                                }
                            }
                            else
                            {
                                switch (endM)
                                {
                                    case -1:
                                        end = DateTime.ParseExact(yearMonth + endD, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture).ToString("yyyyMMdd");
                                        break;

                                    case 0:
                                        end = DateTime.ParseExact(yearMonth + endD, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture).AddMonths(1).ToString("yyyyMMdd");
                                        break;

                                    default:
                                        end = "";
                                        break;
                                }
                            }
                            startEndList.Add(new string[] {mNo,start,end});
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                log.Debug(ex.Message);
            }


            



            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();

                    if (merchantNo != "")
                    {
                        #region SQL設定
                        string sqlText = String.Format(@"
-----------------------------------------------------------------------------
--	測試用
--  declare @EXEC_MERCHANT_NO varchar(20) = 'ALL';		--活動代號:ALL、單一代號
--  declare @EXEC_CPT_DATE_B varchar(20) = '20150101';	--清分日期_開始
--  declare @EXEC_CPT_DATE_E varchar(20) = '20150128';	--清分日期_結束
--	測試用
-----------------------------------------------------------------------------

declare @s_CPT_DATE varchar(20);	--查詢日期

if(@EXEC_MERCHANT_NO = '')
begin
set @EXEC_MERCHANT_NO = 'ALL';
end

if(@EXEC_MERCHANT_NO IN (SELECT MERCHANT_NO FROM GM_MERCHANT WHERE MERCHANT_TYPE = 'MUTI_MERC' )) --判斷是否為小商家&委外
BEGIN
select	DATA.MERCHANT_NO, DATA.CPT_DATE '清算日期', 
		Case when M2.MERCHANT_SUB_STNAME is null then M1.MERCHANT_NAME else M2.MERCHANT_SUB_STNAME end '特約機構名稱',
		ISNULL(DATA.sumPCHA,0) as '購貨總額', ISNULL(DATA.sumPCHRA,0) as '退貨總額', ISNULL((DATA.sumPCHA-DATA.sumPCHRA),0) as '購貨淨額', 
		Convert(FLOAT(50),ISNULL(DATA.P_CM_AM * 100,0)) as			'購_手續費率', 
		ISNULL(cast(ROUND((DATA.sumPCHA-DATA.sumPCHRA)*DATA.P_CM_AM,0) as int),0)	as '購_手續費',
		ISNULL(DATA.sumLOADA,0) '加值額', ISNULL(DATA.sumLOADRA,0)	'加值取消額', ISNULL((DATA.sumLOADA-DATA.sumLOADRA),0) '加值淨額', 
		Convert(FLOAT(50),ISNULL(DATA.L_CM_AM * 100,0)) as			'加_手續費率', 
		ISNULL(cast(ROUND((DATA.sumLOADA-DATA.sumLOADRA)*DATA.L_CM_AM,0) as int),0) as '加_手續費',
		ISNULL(DATA.sumUTUT3_AMTA,0) '代收售總額', ISNULL(DATA.sumUTUT3_AMTRA,0) '代收售取消', ISNULL((DATA.sumUTUT3_AMTA-DATA.sumUTUT3_AMTRA),0) '代收售淨額', 
		Convert(FLOAT(50),ISNULL(DATA.P_UT_AM * 100,0)) as			'代收售_手續費率', 
		ISNULL(cast(ROUND((DATA.sumUTUT3_AMTA-DATA.sumUTUT3_AMTRA)*DATA.P_UT_AM,0) as int),0) as '代收售_手續費',
		ISNULL(DATA.sumLOAD_AMT,0)									'自動加值額', 
		Convert(FLOAT(50),ISNULL(DATA.A__AM * 100,0)) as			'自動加_手續費率', 
		ISNULL(cast(ROUND(sumLOAD_AMT*DATA.A__AM,0) as int),0) as	'自動加_手續費',
		ISNULL(DATA.sumLOAD79_AMT,0)								'企業加值額',
		Convert(FLOAT(50),ISNULL(DATA.B__AM * 100,0)) as			'企業加值_手續費率', 
		ISNULL(cast(ROUND(sumLOAD79_AMT*DATA.A__AM,0) as int),0) as '企業加值_手續費'
from (
	select	DATA1.CPT_DATE, DATA1.MERCHANT_NO, DATA1.SEQ_NO, DATA1.MERCHANT_SUB_NO,
			ISNULL(sum(DATA1.sumPCHA),0) sumPCHA,
			ISNULL(sum(DATA1.sumPCHRA),0) sumPCHRA,
			(select C.FEE_RATE from GM_CONTRACT_MUTI_MERC_D C where C.MERCHANT_NO = DATA1.MERCHANT_NO
													  and DATA1.CPT_DATE between C.EFF_DATE_FROM and C.EFF_DATE_TO
													  and C.CONTRACT_TYPE = 'P1' AND FEE_KIND = 'CM' AND FEE_CAL_FLG = 'AM'
													  and @EXEC_CPT_DATE_B between C.EFF_DATE_FROM and C.EFF_DATE_TO
													  ) P_CM_AM,
			ISNULL(sum(DATA1.sumLOADA),0) sumLOADA,
			ISNULL(sum(DATA1.sumLOADRA),0) sumLOADRA,
			(select C.FEE_RATE from GM_CONTRACT_MUTI_MERC_D C where C.MERCHANT_NO = DATA1.MERCHANT_NO
													  and DATA1.CPT_DATE between C.EFF_DATE_FROM and C.EFF_DATE_TO
													  and C.CONTRACT_TYPE = 'L1' AND FEE_CAL_FLG = 'AM'
													  and @EXEC_CPT_DATE_B between C.EFF_DATE_FROM and C.EFF_DATE_TO
													  ) L_CM_AM,
			ISNULL(sum(DATA1.sumUTUT3_AMTA),0) sumUTUT3_AMTA,
			ISNULL(sum(DATA1.sumUTUT3_AMTRA),0) sumUTUT3_AMTRA,
			(select C.FEE_RATE from GM_CONTRACT_MUTI_MERC_D C where C.MERCHANT_NO = DATA1.MERCHANT_NO
													  and DATA1.CPT_DATE between C.EFF_DATE_FROM and C.EFF_DATE_TO
													  and C.CONTRACT_TYPE = 'P1' and C.FEE_KIND = 'UT' and FEE_CAL_FLG = 'AM'
													  and @EXEC_CPT_DATE_B between C.EFF_DATE_FROM and C.EFF_DATE_TO
													  ) P_UT_AM,
			ISNULL(sum(LOAD74_AMT),0) + ISNULL(sum(LOAD75_AMT),0) + ISNULL(sum(LOAD77_AMT),0) sumLOAD_AMT,  --(add by 20160415 rita 新增 77離線自動加值)
			(select C.FEE_RATE from GM_CONTRACT_MUTI_MERC_D C where C.MERCHANT_NO = DATA1.MERCHANT_NO
													  and DATA1.CPT_DATE between C.EFF_DATE_FROM and C.EFF_DATE_TO
													  and C.CONTRACT_TYPE = 'L2' AND FEE_CAL_FLG = 'AM'
													  and @EXEC_CPT_DATE_B between C.EFF_DATE_FROM and C.EFF_DATE_TO
													  ) A__AM,
			ISNULL(sum(LOAD79_AMT),0) sumLOAD79_AMT,
			(select C.FEE_RATE from GM_CONTRACT_MUTI_MERC_D C where C.MERCHANT_NO = DATA1.MERCHANT_NO
													  and DATA1.CPT_DATE between C.EFF_DATE_FROM and C.EFF_DATE_TO
													  and C.CONTRACT_TYPE = 'B' AND FEE_CAL_FLG = 'AM'
													  and @EXEC_CPT_DATE_B between C.EFF_DATE_FROM and C.EFF_DATE_TO
													  ) B__AM
	from 
	(
		--購貨、購貨取消、加值、加值取消
		select	D1.CPT_DATE, D1.MERCHANT_NO, 
				Case when D2.MERCHANT_SUB_NO = 'POS' then 'FS1' 
					 when D2.MERCHANT_SUB_NO IS NULL then 'FS1' 
					 else '0' end SEQ_NO,
				D2.MERCHANT_SUB_NO,
				ISNULL(sum(D1.PCHA),0) - ISNULL(sum(D1.UTUT3_AMTA),0) sumPCHA, 
				ISNULL(sum(D1.PCHRA),0) - ISNULL(sum(D1.UTUT3_AMTRA),0) sumPCHRA, 
				ISNULL(sum(D1.LOADA),0) sumLOADA, 
				ISNULL(sum(D1.LOADRA),0) sumLOADRA, 
				ISNULL(sum(D1.UTUT3_AMTA),0) sumUTUT3_AMTA, 
				ISNULL(sum(D1.UTUT3_AMTRA),0) sumUTUT3_AMTRA,
				NULL LOAD74_CNT,
				NULL LOAD74_AMT,
				NULL LOAD75_CNT,
				NULL LOAD75_AMT,
				NULL LOAD77_CNT,
				NULL LOAD77_AMT,
				NULL LOAD79_CNT,
				NULL LOAD79_AMT
		from (
			SELECT	CPT_DATE,
					MERCHANT_NO,
					'POS' MERCHANT_SUB_NO,
					SUM(isnull(PCH_AMT  ,0)) PCHA,		--購貨總額
					SUM(isnull(PCHR_AMT ,0)) PCHRA,		--退貨總額
					SUM(isnull(LOAD_AMT ,0)) LOADA,		--加值額
					SUM(isnull(LOADR_AMT,0)) LOADRA,	--加值取消額
					NULL UTUT3_AMTA,					--代收售總額
					NULL UTUT3_AMTRA					--代收售取消
				FROM AM_ISET_MERC_TRANS_LOG_D 
				where CPT_DATE between @EXEC_CPT_DATE_B and @EXEC_CPT_DATE_E
				  and SRC_FLG = 'TXLOG'
				GROUP BY CPT_DATE, MERCHANT_NO
			union all 
			SELECT	CPT_DATE,
					MERCHANT_NO,
					'POS' MERCHANT_NO_SUB,
					NULL PCHA,		--購貨總額
					NULL PCHRA,		--退貨總額
					NULL LOADA,		--加值額
					NULL LOADRA,	--加值取消額
					SUM(isnull(UT_AMT ,0) + isnull(UT3_AMT ,0)) UTUT3_AMTA,	--代收售總額
					NULL UTUT3_AMTRA	--代收售取消
				FROM AM_ISET_MERC_TRANS_UTLOG_D  
				where CPT_DATE between @EXEC_CPT_DATE_B and @EXEC_CPT_DATE_E
				  and SRC_FLG = 'TXLOG'
				  and TRANS_TYPE = '21'
				GROUP BY CPT_DATE, MERCHANT_NO
			union all 
			SELECT	CPT_DATE,
					MERCHANT_NO,
					'POS' MERCHANT_NO_SUB,
					NULL PCHA,		--購貨總額
					NULL PCHRA,		--退貨總額
					NULL LOADA,		--加值額
					NULL LOADRA,	--加值取消額
					NULL UTUT3_AMTA,--代收售總額
					SUM(isnull(UT_AMT ,0) + isnull(UT3_AMT ,0)) UTUT3_AMTRA	--代收售取消
				FROM AM_ISET_MERC_TRANS_UTLOG_D  
				where CPT_DATE between @EXEC_CPT_DATE_B and @EXEC_CPT_DATE_E
				  and SRC_FLG = 'TXLOG'
				  and TRANS_TYPE = '23'
				GROUP BY CPT_DATE, MERCHANT_NO
		)D1 
		left join GM_MERCHANT_SUB D2 on D1.MERCHANT_NO = D2.MERCHANT_NO
									and D1.MERCHANT_SUB_NO = D2.MERCHANT_SUB_NO
		GROUP BY D1.CPT_DATE, D1.MERCHANT_NO, D2.MERCHANT_SUB_NO
		union all 
		--自動加值、自動加值取消、離線自動加值
		--且在GM_BANK_CPT_SUM_SUB 沒有值，代表該特約機構沒有往下一階層
		SELECT	A1.CPT_DATE,
				A1.MERCHANT_NO,
				'FS1' SEQ_NO, 
				NULL MERCHANT_SUB_NO,
				NULL sumPCHA, 
				NULL sumPCHRA, 
				NULL sumLOADA, 
				NULL sumLOADRA, 
				NULL sumUTUT3_AMTA, 
				NULL sumUTUT3_AMTRA,
				SUM(A1.LOAD74_CNT) LOAD74_CNT,
				SUM(A1.LOAD74_AMT) LOAD74_AMT,
				SUM(A1.LOAD75_CNT) LOAD75_CNT,
				SUM(A1.LOAD75_AMT) LOAD75_AMT,
				SUM(A1.LOAD77_CNT) LOAD77_CNT, -- (add by 20160415 rita 新增 77離線自動加值)
				SUM(A1.LOAD77_AMT) LOAD77_AMT, -- (add by 20160415 rita 新增 77離線自動加值)
				NULL LOAD79_CNT,
				NULL LOAD79_AMT
			FROM GM_BANK_CPT_SUM A1
			left join (
						select distinct MERCHANT_NO, CPT_DATE, SETTLE_DATE
						from GM_BANK_CPT_SUM_SUB
						where CPT_DATE between @EXEC_CPT_DATE_B and @EXEC_CPT_DATE_E	
			) A2 on A1.MERCHANT_NO = A2.MERCHANT_NO
				and A1.CPT_DATE = A2.CPT_DATE
				and A1.SETTLE_DATE = A2.SETTLE_DATE
			where A1.CPT_DATE between @EXEC_CPT_DATE_B and @EXEC_CPT_DATE_E
			  and A2.MERCHANT_NO is null
			GROUP BY A1.CPT_DATE, A1.MERCHANT_NO
		union all 
		--自動加值、自動加值取消、離線自動加值
		--且在GM_BANK_CPT_SUM_SUB 有值，代表該特約機構有往下一階層
		--則以設定哪一筆要對到購貨、購貨取消
		SELECT	A1.CPT_DATE,
				A1.MERCHANT_NO,
				Case when A2.MERCHANT_SUB_NO = 'POS' then 'FS1' else '' end SEQ_NO,
				A2.MERCHANT_SUB_NO,
				NULL sumPCHA, 
				NULL sumPCHRA, 
				NULL sumLOADA, 
				NULL sumLOADRA, 
				NULL sumUTUT3_AMTA, 
				NULL sumUTUT3_AMTRA,
				SUM(A2.LOAD74_CNT) LOAD74_CNT,
				SUM(A2.LOAD74_AMT) LOAD74_AMT,
				SUM(A2.LOAD75_CNT) LOAD75_CNT,
				SUM(A2.LOAD75_AMT) LOAD75_AMT,
				SUM(A2.LOAD77_CNT) LOAD77_CNT, -- (add by 20160415 rita 新增 77離線自動加值)
				SUM(A2.LOAD77_AMT) LOAD77_AMT, -- (add by 20160415 rita 新增 77離線自動加值)
				SUM(A2.LOAD79_CNT) LOAD79_CNT,
				SUM(A2.LOAD79_AMT) LOAD79_AMT
			FROM GM_BANK_CPT_SUM A1
			inner join GM_BANK_CPT_SUM_SUB A2 on A1.MERCHANT_NO = A2.MERCHANT_NO
											and A1.BANK_MERCHANT = A2.BANK_MERCHANT
											and A1.CPT_DATE = A2.CPT_DATE
											and A1.SETTLE_DATE = A2.SETTLE_DATE
			where A1.CPT_DATE between @EXEC_CPT_DATE_B and @EXEC_CPT_DATE_E
			GROUP BY A1.CPT_DATE, A1.MERCHANT_NO, A2.MERCHANT_SUB_NO
	)DATA1
	GROUP BY DATA1.CPT_DATE, DATA1.MERCHANT_NO, DATA1.SEQ_NO, DATA1.MERCHANT_SUB_NO
)DATA
inner join GM_MERCHANT M1 on DATA.MERCHANT_NO = M1.MERCHANT_NO
							and M1.MERCHANT_NO = case when @EXEC_MERCHANT_NO = 'ALL' then M1.MERCHANT_NO else @EXEC_MERCHANT_NO end
left join GM_MERCHANT_SUB M2 on DATA.MERCHANT_SUB_NO = M2.MERCHANT_SUB_NO
order by M1.MERCHANT_TYPE, M1.MERC_GROUP desc, M1.MERCHANT_NO, DATA.CPT_DATE
END
--非委外
ELSE
BEGIN
select	DATA.MERCHANT_NO, DATA.CPT_DATE '清算日期', 
		Case when M2.MERCHANT_SUB_STNAME is null then M1.MERCHANT_NAME else M2.MERCHANT_SUB_STNAME end '特約機構名稱',
		ISNULL(DATA.sumPCHA,0) as '購貨總額', ISNULL(DATA.sumPCHRA,0) as '退貨總額', ISNULL((DATA.sumPCHA-DATA.sumPCHRA),0) as '購貨淨額', 
		Convert(FLOAT(50),ISNULL(DATA.P_CM_AM * 100,0)) as			'購_手續費率', 
		ISNULL(cast(ROUND((DATA.sumPCHA-DATA.sumPCHRA)*DATA.P_CM_AM,0) as int),0)	as '購_手續費',
		ISNULL(DATA.sumLOADA,0) '加值額', ISNULL(DATA.sumLOADRA,0)	'加值取消額', ISNULL((DATA.sumLOADA-DATA.sumLOADRA),0) '加值淨額', 
		Convert(FLOAT(50),ISNULL(DATA.L_CM_AM * 100,0)) as			'加_手續費率', 
		ISNULL(cast(ROUND((DATA.sumLOADA-DATA.sumLOADRA)*DATA.L_CM_AM,0) as int),0) as '加_手續費',
		ISNULL(DATA.sumUTUT3_AMTA,0) '代收售總額', ISNULL(DATA.sumUTUT3_AMTRA,0) '代收售取消', ISNULL((DATA.sumUTUT3_AMTA-DATA.sumUTUT3_AMTRA),0) '代收售淨額', 
		Convert(FLOAT(50),ISNULL(DATA.P_UT_AM * 100,0)) as			'代收售_手續費率', 
		ISNULL(cast(ROUND((DATA.sumUTUT3_AMTA-DATA.sumUTUT3_AMTRA)*DATA.P_UT_AM,0) as int),0) as '代收售_手續費',
		ISNULL(DATA.sumLOAD_AMT,0)									'自動加值額', 
		Convert(FLOAT(50),ISNULL(DATA.A__AM * 100,0)) as			'自動加_手續費率', 
		ISNULL(cast(ROUND(sumLOAD_AMT*DATA.A__AM,0) as int),0) as	'自動加_手續費',
		ISNULL(DATA.sumLOAD79_AMT,0)								'企業加值額',
		Convert(FLOAT(50),ISNULL(DATA.B__AM * 100,0)) as			'企業加值_手續費率', 
		ISNULL(cast(ROUND(sumLOAD79_AMT*DATA.A__AM,0) as int),0) as '企業加值_手續費'
from (
	select	DATA1.CPT_DATE, DATA1.MERCHANT_NO, DATA1.SEQ_NO, DATA1.MERCHANT_SUB_NO,
			ISNULL(sum(DATA1.sumPCHA),0) sumPCHA,
			ISNULL(sum(DATA1.sumPCHRA),0) sumPCHRA,
			(select C.FEE_RATE from GM_CONTRACT_D C where C.MERCHANT_NO = DATA1.MERCHANT_NO
													  and DATA1.CPT_DATE between C.EFF_DATE_FROM and C.EFF_DATE_TO
													  and C.SETTLE_TYPE = 'P' and C.FEE_KIND = 'CM' and FEE_CAL_FLG = 'AM'
													  and @EXEC_CPT_DATE_B between C.EFF_DATE_FROM and C.EFF_DATE_TO) P_CM_AM,
			ISNULL(sum(DATA1.sumLOADA),0) sumLOADA,
			ISNULL(sum(DATA1.sumLOADRA),0) sumLOADRA,
			(select C.FEE_RATE from GM_CONTRACT_D C where C.MERCHANT_NO = DATA1.MERCHANT_NO
													  and DATA1.CPT_DATE between C.EFF_DATE_FROM and C.EFF_DATE_TO
													  and C.SETTLE_TYPE = 'L' and FEE_CAL_FLG = 'AM'
													  and @EXEC_CPT_DATE_B between C.EFF_DATE_FROM and C.EFF_DATE_TO) L_CM_AM,
			ISNULL(sum(DATA1.sumUTUT3_AMTA),0) sumUTUT3_AMTA,
			ISNULL(sum(DATA1.sumUTUT3_AMTRA),0) sumUTUT3_AMTRA,
			(select C.FEE_RATE from GM_CONTRACT_D C where C.MERCHANT_NO = DATA1.MERCHANT_NO
													  and DATA1.CPT_DATE between C.EFF_DATE_FROM and C.EFF_DATE_TO
													  and C.SETTLE_TYPE = 'P' and C.FEE_KIND = 'UT' and FEE_CAL_FLG = 'AM'
													  and @EXEC_CPT_DATE_B between C.EFF_DATE_FROM and C.EFF_DATE_TO) P_UT_AM,
			ISNULL(sum(LOAD74_AMT),0) + ISNULL(sum(LOAD75_AMT),0) + ISNULL(sum(LOAD77_AMT),0) sumLOAD_AMT,  --(add by 20160415 rita 新增 77離線自動加值)
			(select C.FEE_RATE from GM_CONTRACT_D C where C.MERCHANT_NO = DATA1.MERCHANT_NO
													  and DATA1.CPT_DATE between C.EFF_DATE_FROM and C.EFF_DATE_TO
													  and C.SETTLE_TYPE = 'A' and FEE_CAL_FLG = 'AM'
													  and @EXEC_CPT_DATE_B between C.EFF_DATE_FROM and C.EFF_DATE_TO) A__AM,
			ISNULL(sum(LOAD79_AMT),0) sumLOAD79_AMT,
			(select C.FEE_RATE from GM_CONTRACT_D C where C.MERCHANT_NO = DATA1.MERCHANT_NO
													  and DATA1.CPT_DATE between C.EFF_DATE_FROM and C.EFF_DATE_TO
													  and C.SETTLE_TYPE = 'B' and FEE_CAL_FLG = 'AM'
													  and @EXEC_CPT_DATE_B between C.EFF_DATE_FROM and C.EFF_DATE_TO) B__AM
	from 
	(
		--購貨、購貨取消、加值、加值取消
		select	D1.CPT_DATE, D1.MERCHANT_NO, 
				Case when D2.MERCHANT_SUB_NO = 'POS' then 'FS1' 
					 when D2.MERCHANT_SUB_NO IS NULL then 'FS1' 
					 else '0' end SEQ_NO,
				D2.MERCHANT_SUB_NO,
				ISNULL(sum(D1.PCHA),0) - ISNULL(sum(D1.UTUT3_AMTA),0) sumPCHA, 
				ISNULL(sum(D1.PCHRA),0) - ISNULL(sum(D1.UTUT3_AMTRA),0) sumPCHRA, 
				ISNULL(sum(D1.LOADA),0) sumLOADA, 
				ISNULL(sum(D1.LOADRA),0) sumLOADRA, 
				ISNULL(sum(D1.UTUT3_AMTA),0) sumUTUT3_AMTA, 
				ISNULL(sum(D1.UTUT3_AMTRA),0) sumUTUT3_AMTRA,
				NULL LOAD74_CNT,
				NULL LOAD74_AMT,
				NULL LOAD75_CNT,
				NULL LOAD75_AMT,
				NULL LOAD77_CNT,
				NULL LOAD77_AMT,
				NULL LOAD79_CNT,
				NULL LOAD79_AMT
		from (
			SELECT	CPT_DATE,
					MERCHANT_NO,
					'POS' MERCHANT_SUB_NO,
					SUM(isnull(PCH_AMT  ,0)) PCHA,		--購貨總額
					SUM(isnull(PCHR_AMT ,0)) PCHRA,		--退貨總額
					SUM(isnull(LOAD_AMT ,0)) LOADA,		--加值額
					SUM(isnull(LOADR_AMT,0)) LOADRA,	--加值取消額
					NULL UTUT3_AMTA,					--代收售總額
					NULL UTUT3_AMTRA					--代收售取消
				FROM AM_ISET_MERC_TRANS_LOG_D 
				where CPT_DATE between @EXEC_CPT_DATE_B and @EXEC_CPT_DATE_E
				  and SRC_FLG = 'TXLOG'
				GROUP BY CPT_DATE, MERCHANT_NO
			union all 
			SELECT	CPT_DATE,
					MERCHANT_NO,
					'POS' MERCHANT_NO_SUB,
					NULL PCHA,		--購貨總額
					NULL PCHRA,		--退貨總額
					NULL LOADA,		--加值額
					NULL LOADRA,	--加值取消額
					SUM(isnull(UT_AMT ,0) + isnull(UT3_AMT ,0)) UTUT3_AMTA,	--代收售總額
					NULL UTUT3_AMTRA	--代收售取消
				FROM AM_ISET_MERC_TRANS_UTLOG_D  
				where CPT_DATE between @EXEC_CPT_DATE_B and @EXEC_CPT_DATE_E
				  and SRC_FLG = 'TXLOG'
				  and TRANS_TYPE = '21'
				GROUP BY CPT_DATE, MERCHANT_NO
			union all 
			SELECT	CPT_DATE,
					MERCHANT_NO,
					'POS' MERCHANT_NO_SUB,
					NULL PCHA,		--購貨總額
					NULL PCHRA,		--退貨總額
					NULL LOADA,		--加值額
					NULL LOADRA,	--加值取消額
					NULL UTUT3_AMTA,--代收售總額
					SUM(isnull(UT_AMT ,0) + isnull(UT3_AMT ,0)) UTUT3_AMTRA	--代收售取消
				FROM AM_ISET_MERC_TRANS_UTLOG_D  
				where CPT_DATE between @EXEC_CPT_DATE_B and @EXEC_CPT_DATE_E
				  and SRC_FLG = 'TXLOG'
				  and TRANS_TYPE = '23'
				GROUP BY CPT_DATE, MERCHANT_NO
		)D1 
		left join GM_MERCHANT_SUB D2 on D1.MERCHANT_NO = D2.MERCHANT_NO
									and D1.MERCHANT_SUB_NO = D2.MERCHANT_SUB_NO
		GROUP BY D1.CPT_DATE, D1.MERCHANT_NO, D2.MERCHANT_SUB_NO
		union all 
		--自動加值、自動加值取消、離線自動加值
		--且在GM_BANK_CPT_SUM_SUB 沒有值，代表該特約機構沒有往下一階層
		SELECT	A1.CPT_DATE,
				A1.MERCHANT_NO,
				'FS1' SEQ_NO, 
				NULL MERCHANT_SUB_NO,
				NULL sumPCHA, 
				NULL sumPCHRA, 
				NULL sumLOADA, 
				NULL sumLOADRA, 
				NULL sumUTUT3_AMTA, 
				NULL sumUTUT3_AMTRA,
				SUM(A1.LOAD74_CNT) LOAD74_CNT,
				SUM(A1.LOAD74_AMT) LOAD74_AMT,
				SUM(A1.LOAD75_CNT) LOAD75_CNT,
				SUM(A1.LOAD75_AMT) LOAD75_AMT,
				SUM(A1.LOAD77_CNT) LOAD77_CNT, -- (add by 20160415 rita 新增 77離線自動加值)
				SUM(A1.LOAD77_AMT) LOAD77_AMT, -- (add by 20160415 rita 新增 77離線自動加值)
				NULL LOAD79_CNT,
				NULL LOAD79_AMT
			FROM GM_BANK_CPT_SUM A1
			left join (
						select distinct MERCHANT_NO, CPT_DATE, SETTLE_DATE
						from GM_BANK_CPT_SUM_SUB
						where CPT_DATE between @EXEC_CPT_DATE_B and @EXEC_CPT_DATE_E	
			) A2 on A1.MERCHANT_NO = A2.MERCHANT_NO
				and A1.CPT_DATE = A2.CPT_DATE
				and A1.SETTLE_DATE = A2.SETTLE_DATE
			where A1.CPT_DATE between @EXEC_CPT_DATE_B and @EXEC_CPT_DATE_E
			  and A2.MERCHANT_NO is null
			GROUP BY A1.CPT_DATE, A1.MERCHANT_NO
		union all 
		--自動加值、自動加值取消、離線自動加值
		--且在GM_BANK_CPT_SUM_SUB 有值，代表該特約機構有往下一階層
		--則以設定哪一筆要對到購貨、購貨取消
		SELECT	A1.CPT_DATE,
				A1.MERCHANT_NO,
				Case when A2.MERCHANT_SUB_NO = 'POS' then 'FS1' else '' end SEQ_NO,
				A2.MERCHANT_SUB_NO,
				NULL sumPCHA, 
				NULL sumPCHRA, 
				NULL sumLOADA, 
				NULL sumLOADRA, 
				NULL sumUTUT3_AMTA, 
				NULL sumUTUT3_AMTRA,
				SUM(A2.LOAD74_CNT) LOAD74_CNT,
				SUM(A2.LOAD74_AMT) LOAD74_AMT,
				SUM(A2.LOAD75_CNT) LOAD75_CNT,
				SUM(A2.LOAD75_AMT) LOAD75_AMT,
				SUM(A2.LOAD77_CNT) LOAD77_CNT, -- (add by 20160415 rita 新增 77離線自動加值)
				SUM(A2.LOAD77_AMT) LOAD77_AMT, -- (add by 20160415 rita 新增 77離線自動加值)
				SUM(A2.LOAD79_CNT) LOAD79_CNT,
				SUM(A2.LOAD79_AMT) LOAD79_AMT
			FROM GM_BANK_CPT_SUM A1
			inner join GM_BANK_CPT_SUM_SUB A2 on A1.MERCHANT_NO = A2.MERCHANT_NO
											and A1.BANK_MERCHANT = A2.BANK_MERCHANT
											and A1.CPT_DATE = A2.CPT_DATE
											and A1.SETTLE_DATE = A2.SETTLE_DATE
			where A1.CPT_DATE between @EXEC_CPT_DATE_B and @EXEC_CPT_DATE_E
			GROUP BY A1.CPT_DATE, A1.MERCHANT_NO, A2.MERCHANT_SUB_NO
	)DATA1
	GROUP BY DATA1.CPT_DATE, DATA1.MERCHANT_NO, DATA1.SEQ_NO, DATA1.MERCHANT_SUB_NO
)DATA
inner join GM_MERCHANT M1 on DATA.MERCHANT_NO = M1.MERCHANT_NO
							and M1.MERCHANT_NO = case when @EXEC_MERCHANT_NO = 'ALL' then M1.MERCHANT_NO else @EXEC_MERCHANT_NO end
left join GM_MERCHANT_SUB M2 on DATA.MERCHANT_SUB_NO = M2.MERCHANT_SUB_NO
order by M1.MERCHANT_TYPE, M1.MERC_GROUP desc, M1.MERCHANT_NO, DATA.CPT_DATE
END");
                        #endregion SQL設定

                        SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                        sqlCmd.Parameters.Add("@EXEC_CPT_DATE_B", SqlDbType.VarChar).Value = start;
                        sqlCmd.Parameters.Add("@EXEC_CPT_DATE_E", SqlDbType.VarChar).Value = end;
                        sqlCmd.Parameters.Add("@EXEC_MERCHANT_NO", SqlDbType.VarChar).Value = merchantNo;

                        SqlDataAdapter adapter = new SqlDataAdapter(sqlCmd);

                        adapter.Fill(dt);

                        sqlCmd.Dispose();
                        adapter.Dispose();
                    }
                    else
                    {

                        #region SQL設定
                        string sqlText = String.Format(@"select	DATA.MERCHANT_NO, DATA.CPT_DATE '清算日期', 
		Case when M2.MERCHANT_SUB_STNAME is null then M1.MERCHANT_NAME else M2.MERCHANT_SUB_STNAME end '特約機構名稱',
		ISNULL(DATA.sumPCHA,0) as '購貨總額', ISNULL(DATA.sumPCHRA,0) as '退貨總額', ISNULL((DATA.sumPCHA-DATA.sumPCHRA),0) as '購貨淨額', 
		Convert(FLOAT(50),ISNULL(DATA.P_CM_AM * 100,0)) as			'購_手續費率', 
		ISNULL(cast(ROUND((DATA.sumPCHA-DATA.sumPCHRA)*DATA.P_CM_AM,0) as int),0)	as '購_手續費',
		ISNULL(DATA.sumLOADA,0) '加值額', ISNULL(DATA.sumLOADRA,0)	'加值取消額', ISNULL((DATA.sumLOADA-DATA.sumLOADRA),0) '加值淨額', 
		Convert(FLOAT(50),ISNULL(DATA.L_CM_AM * 100,0)) as			'加_手續費率', 
		ISNULL(cast(ROUND((DATA.sumLOADA-DATA.sumLOADRA)*DATA.L_CM_AM,0) as int),0) as '加_手續費',
		ISNULL(DATA.sumUTUT3_AMTA,0) '代收售總額', ISNULL(DATA.sumUTUT3_AMTRA,0) '代收售取消', ISNULL((DATA.sumUTUT3_AMTA-DATA.sumUTUT3_AMTRA),0) '代收售淨額', 
		Convert(FLOAT(50),ISNULL(DATA.P_UT_AM * 100,0)) as			'代收售_手續費率', 
		ISNULL(cast(ROUND((DATA.sumUTUT3_AMTA-DATA.sumUTUT3_AMTRA)*DATA.P_UT_AM,0) as int),0) as '代收售_手續費',
		ISNULL(DATA.sumLOAD_AMT,0)									'自動加值額', 
		Convert(FLOAT(50),ISNULL(DATA.A__AM * 100,0)) as			'自動加_手續費率', 
		ISNULL(cast(ROUND(sumLOAD_AMT*DATA.A__AM,0) as int),0) as	'自動加_手續費',
		ISNULL(DATA.sumLOAD79_AMT,0)								'企業加值額',
		Convert(FLOAT(50),ISNULL(DATA.B__AM * 100,0)) as			'企業加值_手續費率', 
		ISNULL(cast(ROUND(sumLOAD79_AMT*DATA.A__AM,0) as int),0) as '企業加值_手續費'
from (
	select	DATA1.CPT_DATE, DATA1.MERCHANT_NO, DATA1.SEQ_NO, DATA1.MERCHANT_SUB_NO,
			ISNULL(sum(DATA1.sumPCHA),0) sumPCHA,
			ISNULL(sum(DATA1.sumPCHRA),0) sumPCHRA,
			(select C.FEE_RATE from GM_CONTRACT_D C where C.MERCHANT_NO = DATA1.MERCHANT_NO
													  and DATA1.CPT_DATE between C.EFF_DATE_FROM and C.EFF_DATE_TO
													  and C.SETTLE_TYPE = 'P' and C.FEE_KIND = 'CM' and FEE_CAL_FLG = 'AM'
													  and @EXEC_CPT_DATE_B between C.EFF_DATE_FROM and C.EFF_DATE_TO) P_CM_AM,
			ISNULL(sum(DATA1.sumLOADA),0) sumLOADA,
			ISNULL(sum(DATA1.sumLOADRA),0) sumLOADRA,
			(select C.FEE_RATE from GM_CONTRACT_D C where C.MERCHANT_NO = DATA1.MERCHANT_NO
													  and DATA1.CPT_DATE between C.EFF_DATE_FROM and C.EFF_DATE_TO
													  and C.SETTLE_TYPE = 'L' and FEE_CAL_FLG = 'AM'
													  and @EXEC_CPT_DATE_B between C.EFF_DATE_FROM and C.EFF_DATE_TO) L_CM_AM,
			ISNULL(sum(DATA1.sumUTUT3_AMTA),0) sumUTUT3_AMTA,
			ISNULL(sum(DATA1.sumUTUT3_AMTRA),0) sumUTUT3_AMTRA,
			(select C.FEE_RATE from GM_CONTRACT_D C where C.MERCHANT_NO = DATA1.MERCHANT_NO
													  and DATA1.CPT_DATE between C.EFF_DATE_FROM and C.EFF_DATE_TO
													  and C.SETTLE_TYPE = 'P' and C.FEE_KIND = 'UT' and FEE_CAL_FLG = 'AM'
													  and @EXEC_CPT_DATE_B between C.EFF_DATE_FROM and C.EFF_DATE_TO) P_UT_AM,
			ISNULL(sum(LOAD74_AMT),0) + ISNULL(sum(LOAD75_AMT),0) + ISNULL(sum(LOAD77_AMT),0) sumLOAD_AMT,  --(add by 20160415 rita 新增 77離線自動加值)
			(select C.FEE_RATE from GM_CONTRACT_D C where C.MERCHANT_NO = DATA1.MERCHANT_NO
													  and DATA1.CPT_DATE between C.EFF_DATE_FROM and C.EFF_DATE_TO
													  and C.SETTLE_TYPE = 'A' and FEE_CAL_FLG = 'AM'
													  and @EXEC_CPT_DATE_B between C.EFF_DATE_FROM and C.EFF_DATE_TO) A__AM,
			ISNULL(sum(LOAD79_AMT),0) sumLOAD79_AMT,
			(select C.FEE_RATE from GM_CONTRACT_D C where C.MERCHANT_NO = DATA1.MERCHANT_NO
													  and DATA1.CPT_DATE between C.EFF_DATE_FROM and C.EFF_DATE_TO
													  and C.SETTLE_TYPE = 'B' and FEE_CAL_FLG = 'AM'
													  and @EXEC_CPT_DATE_B between C.EFF_DATE_FROM and C.EFF_DATE_TO) B__AM
	from 
	(
		--購貨、購貨取消、加值、加值取消
		select	D1.CPT_DATE, D1.MERCHANT_NO, 
				Case when D2.MERCHANT_SUB_NO = 'POS' then 'FS1' 
					 when D2.MERCHANT_SUB_NO IS NULL then 'FS1' 
					 else '0' end SEQ_NO,
				D2.MERCHANT_SUB_NO,
				ISNULL(sum(D1.PCHA),0) - ISNULL(sum(D1.UTUT3_AMTA),0) sumPCHA, 
				ISNULL(sum(D1.PCHRA),0) - ISNULL(sum(D1.UTUT3_AMTRA),0) sumPCHRA, 
				ISNULL(sum(D1.LOADA),0) sumLOADA, 
				ISNULL(sum(D1.LOADRA),0) sumLOADRA, 
				ISNULL(sum(D1.UTUT3_AMTA),0) sumUTUT3_AMTA, 
				ISNULL(sum(D1.UTUT3_AMTRA),0) sumUTUT3_AMTRA,
				NULL LOAD74_CNT,
				NULL LOAD74_AMT,
				NULL LOAD75_CNT,
				NULL LOAD75_AMT,
				NULL LOAD77_CNT,
				NULL LOAD77_AMT,
				NULL LOAD79_CNT,
				NULL LOAD79_AMT
		from (
			SELECT	CPT_DATE,
					MERCHANT_NO,
					'POS' MERCHANT_SUB_NO,
					SUM(isnull(PCH_AMT  ,0)) PCHA,		--購貨總額
					SUM(isnull(PCHR_AMT ,0)) PCHRA,		--退貨總額
					SUM(isnull(LOAD_AMT ,0)) LOADA,		--加值額
					SUM(isnull(LOADR_AMT,0)) LOADRA,	--加值取消額
					NULL UTUT3_AMTA,					--代收售總額
					NULL UTUT3_AMTRA					--代收售取消
				FROM AM_ISET_MERC_TRANS_LOG_D 
				where CPT_DATE between @EXEC_CPT_DATE_B and @EXEC_CPT_DATE_E
				  and SRC_FLG = 'TXLOG'
				GROUP BY CPT_DATE, MERCHANT_NO
			union all 
			SELECT	CPT_DATE,
					MERCHANT_NO,
					'POS' MERCHANT_NO_SUB,
					NULL PCHA,		--購貨總額
					NULL PCHRA,		--退貨總額
					NULL LOADA,		--加值額
					NULL LOADRA,	--加值取消額
					SUM(isnull(UT_AMT ,0) + isnull(UT3_AMT ,0)) UTUT3_AMTA,	--代收售總額
					NULL UTUT3_AMTRA	--代收售取消
				FROM AM_ISET_MERC_TRANS_UTLOG_D  
				where CPT_DATE between @EXEC_CPT_DATE_B and @EXEC_CPT_DATE_E
				  and SRC_FLG = 'TXLOG'
				  and TRANS_TYPE = '21'
				GROUP BY CPT_DATE, MERCHANT_NO
			union all 
			SELECT	CPT_DATE,
					MERCHANT_NO,
					'POS' MERCHANT_NO_SUB,
					NULL PCHA,		--購貨總額
					NULL PCHRA,		--退貨總額
					NULL LOADA,		--加值額
					NULL LOADRA,	--加值取消額
					NULL UTUT3_AMTA,--代收售總額
					SUM(isnull(UT_AMT ,0) + isnull(UT3_AMT ,0)) UTUT3_AMTRA	--代收售取消
				FROM AM_ISET_MERC_TRANS_UTLOG_D  
				where CPT_DATE between @EXEC_CPT_DATE_B and @EXEC_CPT_DATE_E
				  and SRC_FLG = 'TXLOG'
				  and TRANS_TYPE = '23'
				GROUP BY CPT_DATE, MERCHANT_NO
		)D1 
		left join GM_MERCHANT_SUB D2 on D1.MERCHANT_NO = D2.MERCHANT_NO
									and D1.MERCHANT_SUB_NO = D2.MERCHANT_SUB_NO
		GROUP BY D1.CPT_DATE, D1.MERCHANT_NO, D2.MERCHANT_SUB_NO
		union all 
		--自動加值、自動加值取消、離線自動加值
		--且在GM_BANK_CPT_SUM_SUB 沒有值，代表該特約機構沒有往下一階層
		SELECT	A1.CPT_DATE,
				A1.MERCHANT_NO,
				'FS1' SEQ_NO, 
				NULL MERCHANT_SUB_NO,
				NULL sumPCHA, 
				NULL sumPCHRA, 
				NULL sumLOADA, 
				NULL sumLOADRA, 
				NULL sumUTUT3_AMTA, 
				NULL sumUTUT3_AMTRA,
				SUM(A1.LOAD74_CNT) LOAD74_CNT,
				SUM(A1.LOAD74_AMT) LOAD74_AMT,
				SUM(A1.LOAD75_CNT) LOAD75_CNT,
				SUM(A1.LOAD75_AMT) LOAD75_AMT,
				SUM(A1.LOAD77_CNT) LOAD77_CNT, -- (add by 20160415 rita 新增 77離線自動加值)
				SUM(A1.LOAD77_AMT) LOAD77_AMT, -- (add by 20160415 rita 新增 77離線自動加值)
				NULL LOAD79_CNT,
				NULL LOAD79_AMT
			FROM GM_BANK_CPT_SUM A1
			left join (
						select distinct MERCHANT_NO, CPT_DATE, SETTLE_DATE
						from GM_BANK_CPT_SUM_SUB
						where CPT_DATE between @EXEC_CPT_DATE_B and @EXEC_CPT_DATE_E	
			) A2 on A1.MERCHANT_NO = A2.MERCHANT_NO
				and A1.CPT_DATE = A2.CPT_DATE
				and A1.SETTLE_DATE = A2.SETTLE_DATE
			where A1.CPT_DATE between @EXEC_CPT_DATE_B and @EXEC_CPT_DATE_E
			  and A2.MERCHANT_NO is null
			GROUP BY A1.CPT_DATE, A1.MERCHANT_NO
		union all 
		--自動加值、自動加值取消、離線自動加值
		--且在GM_BANK_CPT_SUM_SUB 有值，代表該特約機構有往下一階層
		--則以設定哪一筆要對到購貨、購貨取消
		SELECT	A1.CPT_DATE,
				A1.MERCHANT_NO,
				Case when A2.MERCHANT_SUB_NO = 'POS' then 'FS1' else '' end SEQ_NO,
				A2.MERCHANT_SUB_NO,
				NULL sumPCHA, 
				NULL sumPCHRA, 
				NULL sumLOADA, 
				NULL sumLOADRA, 
				NULL sumUTUT3_AMTA, 
				NULL sumUTUT3_AMTRA,
				SUM(A2.LOAD74_CNT) LOAD74_CNT,
				SUM(A2.LOAD74_AMT) LOAD74_AMT,
				SUM(A2.LOAD75_CNT) LOAD75_CNT,
				SUM(A2.LOAD75_AMT) LOAD75_AMT,
				SUM(A2.LOAD77_CNT) LOAD77_CNT, -- (add by 20160415 rita 新增 77離線自動加值)
				SUM(A2.LOAD77_AMT) LOAD77_AMT, -- (add by 20160415 rita 新增 77離線自動加值)
				SUM(A2.LOAD79_CNT) LOAD79_CNT,
				SUM(A2.LOAD79_AMT) LOAD79_AMT
			FROM GM_BANK_CPT_SUM A1
			inner join GM_BANK_CPT_SUM_SUB A2 on A1.MERCHANT_NO = A2.MERCHANT_NO
											and A1.BANK_MERCHANT = A2.BANK_MERCHANT
											and A1.CPT_DATE = A2.CPT_DATE
											and A1.SETTLE_DATE = A2.SETTLE_DATE
			where A1.CPT_DATE between @EXEC_CPT_DATE_B and @EXEC_CPT_DATE_E
			GROUP BY A1.CPT_DATE, A1.MERCHANT_NO, A2.MERCHANT_SUB_NO
	)DATA1
	GROUP BY DATA1.CPT_DATE, DATA1.MERCHANT_NO, DATA1.SEQ_NO, DATA1.MERCHANT_SUB_NO
)DATA
inner join GM_MERCHANT M1 on DATA.MERCHANT_NO = M1.MERCHANT_NO
							and M1.MERCHANT_NO = case when @EXEC_MERCHANT_NO = 'ALL' then M1.MERCHANT_NO else @EXEC_MERCHANT_NO end
left join GM_MERCHANT_SUB M2 on DATA.MERCHANT_SUB_NO = M2.MERCHANT_SUB_NO
order by M1.MERCHANT_TYPE, M1.MERC_GROUP desc, M1.MERCHANT_NO, DATA.CPT_DATE");

                        #endregion SQL設定

                        foreach (string mNo in merchantNoList)
                        {
                            DataTable dt0 = new DataTable();

                            SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                            sqlCmd.Parameters.Add("@EXEC_CPT_DATE_B", SqlDbType.VarChar).Value = startEndList.Where(x => x[0] == mNo).FirstOrDefault()[1];
                            sqlCmd.Parameters.Add("@EXEC_CPT_DATE_E", SqlDbType.VarChar).Value = startEndList.Where(x => x[0] == mNo).FirstOrDefault()[2];
                            sqlCmd.Parameters.Add("@EXEC_MERCHANT_NO", SqlDbType.VarChar).Value = mNo;

                            SqlDataAdapter adapter = new SqlDataAdapter(sqlCmd);
                            adapter.Fill(dt0);

                            sqlCmd.Dispose();
                            adapter.Dispose();

                            dt.Merge(dt0);
                        }


                    }
                }
                
            }
            catch (Exception ex)
            {
                log.Debug(ex.Message);
            }
            return dt;
        }

        public DataTable Report0122T(string start, string end, string merchantNo, string SRC_FLG)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();
                    string sqlText =
                            @"SELECT 
G.MERCHANT_NO,
LEFT(convert(varchar, dateadd(d,-1,cast(G.CPT_DATE as date)) ,112),8) 傳輸日期,
G.CPT_DATE      清算日期,
G.SETTLE_DATE      會計日期,
GM.MERCHANT_STNAME    特約機構名稱,
G.TPCHA - (select isnull(sum(isnull(UT_AMT,0)),0)+isnull(sum(isnull(UT3_AMT,0)),0) FROM AM_ISET_MERC_TRANS_UTLOG_D where TRANS_TYPE='21' and SRC_FLG=@SRC_FLG and CPT_DATE =G.CPT_DATE and SETTLE_DATE=G.SETTLE_DATE)				'購貨總額'		,
G.TPCHRA - (select isnull(sum(isnull(UT_AMT,0)),0)+isnull(sum(isnull(UT3_AMT,0)),0) FROM AM_ISET_MERC_TRANS_UTLOG_D where TRANS_TYPE='23' and SRC_FLG=@SRC_FLG and CPT_DATE =G.CPT_DATE and SETTLE_DATE=G.SETTLE_DATE)		'退貨總額'	,
G.TPCHA - (select isnull(sum((case when TRANS_TYPE='21' then 1 else -1 end)*isnull(UT_AMT,0)),0)+isnull(sum((case when TRANS_TYPE='21' then 1 else -1 end)*isnull(UT3_AMT,0)),0) FROM AM_ISET_MERC_TRANS_UTLOG_D where SRC_FLG=@SRC_FLG and CPT_DATE =G.CPT_DATE and SETTLE_DATE=G.SETTLE_DATE) -G.TPCHRA	'購貨淨額'	,
case when G.MERCHANT_NO='22555003' then '0.42' else cast((SELECT substring(cast(max(FEE_RATE)*100 as varchar),1,4)
FROM  GM_CONTRACT_D
where FEE_KIND='CM' and FEE_CAL_FLG='AM' and SETTLE_TYPE='P'
and MERCHANT_NO=G.MERCHANT_NO
and G.CPT_DATE between EFF_DATE_FROM and EFF_DATE_TO
) as varchar) end 購_手續費率,
cast(round((G.TPCHA - (select isnull(sum((case when TRANS_TYPE='21' then 1 else -1 end)*isnull(UT_AMT,0)),0)+isnull(sum((case when TRANS_TYPE='21' then 1 else -1 end)*isnull(UT3_AMT,0)),0) FROM AM_ISET_MERC_TRANS_UTLOG_D where SRC_FLG=@SRC_FLG and CPT_DATE =G.CPT_DATE and SETTLE_DATE=G.SETTLE_DATE) -G.TPCHRA)*0.42/100,0) as int) 購_手續費,
G.TLOADA			加值額		,
G.TLOADRA			加值取消額	,
G.TLOADA-G.TLOADRA	加值淨額	,
case when G.MERCHANT_NO='22555003' then '0.20' else cast((SELECT substring(cast(max(FEE_RATE)*100 as varchar),1,4)
FROM  GM_CONTRACT_D
where FEE_KIND='LAM' and FEE_CAL_FLG='AM' and SETTLE_TYPE='L'
and MERCHANT_NO=G.MERCHANT_NO
and G.CPT_DATE between EFF_DATE_FROM and EFF_DATE_TO
) as varchar) end 加_手續費率,
cast(round((G.TLOADA-G.TLOADRA)*0.2/100,0) as int) 加_手續費,
(select isnull(sum((case when TRANS_TYPE='21' then 1 else -1 end)*isnull(UT_AMT,0)),0) FROM AM_ISET_MERC_TRANS_UTLOG_D where MERCHANT_NO=G.MERCHANT_NO and SRC_FLG=@SRC_FLG and CPT_DATE =G.CPT_DATE and SETTLE_DATE=G.SETTLE_DATE) 代收淨額,
case when G.MERCHANT_NO='22555003' then '0' else cast((SELECT isnull(substring(cast(max(FEE_RATE)*100 as varchar),1,4),0)
FROM  GM_CONTRACT_D
where FEE_KIND='UT' and FEE_CAL_FLG='AM' and SETTLE_TYPE='P'
and MERCHANT_NO=G.MERCHANT_NO
and G.CPT_DATE between EFF_DATE_FROM and EFF_DATE_TO
) as varchar) end 代收_手續費率,
cast(round((select isnull(sum((case when TRANS_TYPE='21' then 1 else -1 end)*isnull(UT_AMT,0)),0) FROM AM_ISET_MERC_TRANS_UTLOG_D where MERCHANT_NO=G.MERCHANT_NO and SRC_FLG=@SRC_FLG and CPT_DATE =G.CPT_DATE and SETTLE_DATE=G.SETTLE_DATE)*0/100,0) as int) 代收_手續費,
(select isnull(sum((case when TRANS_TYPE='21' then 1 else -1 end)*isnull(UT3_AMT,0)),0) FROM AM_ISET_MERC_TRANS_UTLOG_D where MERCHANT_NO=G.MERCHANT_NO and SRC_FLG=@SRC_FLG and CPT_DATE =G.CPT_DATE and SETTLE_DATE=G.SETTLE_DATE) 代售淨額,
case when G.MERCHANT_NO='22555003' then '0' else cast((SELECT isnull(substring(cast(max(FEE_RATE)*100 as varchar),1,4),0)
FROM  GM_CONTRACT_D
where FEE_KIND='UT3' and FEE_CAL_FLG='AM' and SETTLE_TYPE='P'
and MERCHANT_NO=G.MERCHANT_NO
and G.CPT_DATE between EFF_DATE_FROM and EFF_DATE_TO
) as varchar) end 代售_手續費率,
cast(round((select isnull(sum((case when TRANS_TYPE='21' then 1 else -1 end)*isnull(UT3_AMT,0)),0) FROM AM_ISET_MERC_TRANS_UTLOG_D where MERCHANT_NO=G.MERCHANT_NO and SRC_FLG=@SRC_FLG and CPT_DATE =G.CPT_DATE and SETTLE_DATE=G.SETTLE_DATE)*0/100,0) as int) 代售_手續費,
(SELECT  isnull(sum(LOAD74_AMT),0)+isnull(sum(LOAD75_AMT),0)+isnull(sum(LOAD77_AMT),0)  -- (add by 20160415 rita 新增 77離線自動加值)
FROM GM_BANK_CPT_SUM
where CPT_DATE=G.CPT_DATE and SETTLE_DATE=G.SETTLE_DATE and MERCHANT_NO=G.MERCHANT_NO) as '自動加值額',
isnull((select substring(cast(max(FEE_RATE)*100 as varchar),1,4) from GM_CONTRACT_D
where SETTLE_TYPE='A'
and MERCHANT_NO=G.MERCHANT_NO
and EFF_DATE_FROM<=G.CPT_DATE
and EFF_DATE_TO>=G.CPT_DATE),0) as '自動加_手續費率' ,
cast(round((SELECT  isnull(sum(LOAD74_AMT),0)+isnull(sum(LOAD75_AMT),0)+isnull(sum(LOAD77_AMT),0)  -- (add by 20160415 rita 新增 77離線自動加值)
FROM GM_BANK_CPT_SUM
where CPT_DATE=G.CPT_DATE and SETTLE_DATE=G.SETTLE_DATE and MERCHANT_NO=G.MERCHANT_NO)
*(isnull((select isnull(max(FEE_RATE),0)*100 from GM_CONTRACT_D
where SETTLE_TYPE='A'
and MERCHANT_NO=G.MERCHANT_NO
and EFF_DATE_FROM<=G.CPT_DATE
and EFF_DATE_TO>=G.CPT_DATE),0)/100 ),0) as int)			自動加_手續費	
FROM
GM_MERCHANT GM LEFT OUTER JOIN
(
SELECT 
T.CPT_DATE,
T.SETTLE_DATE,
T.MERCHANT_NO,
ISNULL(T.PCHA	,0)		TPCHA	,
ISNULL(O.PCHA	,0)		OPCHA	,
ISNULL(T.PCHC	,0)		TPCHC	,
ISNULL(O.PCHC	,0)		OPCHC	,
ISNULL(T.PCHRA	,0)		TPCHRA	,
ISNULL(O.PCHRA	,0)		OPCHRA	,
ISNULL(T.PCHRC	,0)		TPCHRC	,
ISNULL(O.PCHRC	,0)		OPCHRC	,
ISNULL(T.LOADA	,0)		TLOADA	,
ISNULL(O.LOADA	,0)		OLOADA	,
ISNULL(T.LOADC	,0)		TLOADC	,
ISNULL(O.LOADC	,0)		OLOADC	,
ISNULL(T.LOADRA	,0)		TLOADRA	,
ISNULL(O.LOADRA	,0)		OLOADRA	,
ISNULL(T.LOADRC	,0)		TLOADRC	,
ISNULL(O.LOADRC ,0)	   	OLOADRC
FROM
(SELECT       CPT_DATE,
              SETTLE_DATE,
              MERCHANT_NO,
              SUM(isnull(PCH_AMT  ,0))	PCHA,
              SUM(isnull(PCH_CNT  ,0))	PCHC ,
              SUM(isnull(PCHR_AMT ,0)) 	PCHRA,
              SUM(isnull(PCHR_CNT ,0))	PCHRC,
              SUM(isnull(LOAD_AMT ,0)) 	LOADA,
              SUM(isnull(LOAD_CNT ,0))	LOADC,
              SUM(isnull(LOADR_AMT,0))	LOADRA,
              SUM(isnull(LOADR_CNT,0))	LOADRC
FROM AM_ISET_MERC_TRANS_LOG_D
WHERE  SRC_FLG=@SRC_FLG
GROUP BY CPT_DATE,SETTLE_DATE,MERCHANT_NO) T LEFT OUTER JOIN
(SELECT       CPT_DATE,
              MERCHANT_NO,
              SUM(isnull(PCH_AMT  ,0))	PCHA,
              SUM(isnull(PCH_CNT  ,0))	PCHC ,
              SUM(isnull(PCHR_AMT ,0)) 	PCHRA,
              SUM(isnull(PCHR_CNT ,0))	PCHRC,
              SUM(isnull(LOAD_AMT ,0)) 	LOADA,
              SUM(isnull(LOAD_CNT ,0))	LOADC,
              SUM(isnull(LOADR_AMT,0))	LOADRA,
              SUM(isnull(LOADR_CNT,0))	LOADRC
FROM AM_ISET_MERC_TRANS_LOG_D
WHERE SRC_FLG=@SRC_FLG
GROUP BY CPT_DATE,MERCHANT_NO) O
ON  T.merchant_no=O.merchant_no
--and  CONVERT(VARCHAR,CONVERT(DATETIME,T.SETTLE_DATE)-1,112)= O.SETTLE_DATE
AND CONVERT(VARCHAR,CONVERT(DATETIME,T.CPT_DATE)-1,112)= O.CPT_DATE) G
ON GM.MERCHANT_NO=G.MERCHANT_NO
WHERE G.CPT_DATE BETWEEN @Sdate AND @Edate 
and 1=1 "; // G.MERCHANT_NO in ('22555003','27359304','28070701','28459597','70790935','86379116','89589328','89627033','89798198','21242047')";
                    if (merchantNo != "")
                    {
                        sqlText += "AND GM.MERCHANT_NO=@MERCHANT_NO ";
                    }
                    sqlText += "ORDER BY GM.MERCHANT_TYPE,GM.MERC_GROUP desc,GM.MERCHANT_NO,2";

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    sqlCmd.Parameters.Add("@Sdate", SqlDbType.VarChar).Value = start ;
                    sqlCmd.Parameters.Add("@Edate", SqlDbType.VarChar).Value = end ;
                    sqlCmd.Parameters.Add("@MERCHANT_NO", SqlDbType.VarChar).Value = merchantNo;
                    sqlCmd.Parameters.Add("@SRC_FLG", SqlDbType.VarChar).Value = SRC_FLG;

                    SqlDataAdapter adapter = new SqlDataAdapter(sqlCmd);

                    adapter.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                log.Debug(ex.Message);
            }
            return dt;
        }


        public DataTable Report0118T(string yearMonth, string merchantNo)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();
                    string sqlText =
                            @"SELECT 
G.MERCHANT_NO,
G.CPT_DATE      清算日期,
GM.MERCHANT_STNAME    特約機構名稱,
G.TPCHC				購貨總數		,
G.TPCHRC			退貨總數	,
G.TPCHC-G.TPCHRC	淨購貨數	,
G.TLOADC			加值數		,
G.TLOADRC			加值取消數	,
G.TLOADC-G.TLOADRC	淨加值數	
FROM
GM_MERCHANT GM LEFT OUTER JOIN
(
SELECT 
T.CPT_DATE,
T.MERCHANT_NO,
ISNULL(T.PCHA	,0)		TPCHA	,
ISNULL(O.PCHA	,0)		OPCHA	,
ISNULL(T.PCHC	,0)		TPCHC	,
ISNULL(O.PCHC	,0)		OPCHC	,
ISNULL(T.PCHRA	,0)		TPCHRA	,
ISNULL(O.PCHRA	,0)		OPCHRA	,
ISNULL(T.PCHRC	,0)		TPCHRC	,
ISNULL(O.PCHRC	,0)		OPCHRC	,
ISNULL(T.LOADA	,0)		TLOADA	,
ISNULL(O.LOADA	,0)		OLOADA	,
ISNULL(T.LOADC	,0)		TLOADC	,
ISNULL(O.LOADC	,0)		OLOADC	,
ISNULL(T.LOADRA	,0)		TLOADRA	,
ISNULL(O.LOADRA	,0)		OLOADRA	,
ISNULL(T.LOADRC	,0)		TLOADRC	,
ISNULL(O.LOADRC ,0)	   	OLOADRC
FROM
(SELECT       CPT_DATE,
              MERCHANT_NO,
              SUM(isnull(PCH_AMT  ,0))	PCHA,
              SUM(isnull(PCH_CNT  ,0))	PCHC ,
              SUM(isnull(PCHR_AMT ,0)) 	PCHRA,
              SUM(isnull(PCHR_CNT ,0))	PCHRC,
              SUM(isnull(LOAD_AMT ,0)) 	LOADA,
              SUM(isnull(LOAD_CNT ,0))	LOADC,
              SUM(isnull(LOADR_AMT,0))	LOADRA,
              SUM(isnull(LOADR_CNT,0))	LOADRC
FROM AM_ISET_MERC_TRANS_LOG_D
WHERE  SRC_FLG='TXLOG'
GROUP BY CPT_DATE,MERCHANT_NO) T LEFT OUTER JOIN
(SELECT       CPT_DATE,
              MERCHANT_NO,
              SUM(isnull(PCH_AMT  ,0))	PCHA,
              SUM(isnull(PCH_CNT  ,0))	PCHC ,
              SUM(isnull(PCHR_AMT ,0)) 	PCHRA,
              SUM(isnull(PCHR_CNT ,0))	PCHRC,
              SUM(isnull(LOAD_AMT ,0)) 	LOADA,
              SUM(isnull(LOAD_CNT ,0))	LOADC,
              SUM(isnull(LOADR_AMT,0))	LOADRA,
              SUM(isnull(LOADR_CNT,0))	LOADRC
FROM AM_ISET_MERC_TRANS_LOG_D
WHERE SRC_FLG='TXLOG'
GROUP BY CPT_DATE,MERCHANT_NO) O
ON  T.merchant_no=O.merchant_no
AND CONVERT(VARCHAR,CONVERT(DATETIME,T.CPT_DATE)-1,112)= O.CPT_DATE) G
ON GM.MERCHANT_NO=G.MERCHANT_NO
WHERE G.CPT_DATE BETWEEN @Sdate AND @Edate 
and 1=1 "; //G.MERCHANT_NO in ('22555003','27359304','28070701','28459597','70790935','86379116','89589328','89627033','89798198','21242047')";
                    if (merchantNo != "")
                    {
                        sqlText += "AND GM.MERCHANT_NO=@MERCHANT_NO ";
                    }
                    sqlText += "ORDER BY GM.MERCHANT_TYPE,GM.MERC_GROUP desc,GM.MERCHANT_NO,2";

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    sqlCmd.Parameters.Add("@Sdate", SqlDbType.VarChar).Value = yearMonth + "01";
                    sqlCmd.Parameters.Add("@Edate", SqlDbType.VarChar).Value = yearMonth + "31";
                    sqlCmd.Parameters.Add("@MERCHANT_NO", SqlDbType.VarChar).Value = merchantNo;

                    SqlDataAdapter adapter = new SqlDataAdapter(sqlCmd);

                    adapter.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                log.Debug(ex.Message);
            }
            return dt;
        }

        public DataTable Report0119T(string start, string end, string StoreNo)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();
                    string sqlText =
                            @" SELECT 
         CPT_DATE                                      AS '清算日',
         STORE_NO                                      AS '特店代號',
         STO_NAME_SHORT                                AS '特店名稱',
         sum(isnull([52],0))                           AS '現金加值',
--       sum(isnull([74],0))                           AS '自動加值',
--       sum(isnull([75],0))                           AS '代行授權自動加值',
         sum(isnull([54],0))                           AS '加值取消',
         sum(isnull([52],0)) -sum(isnull([54],0)) AS '加值淨額',
         sum(isnull([51],0))                           AS '消費',    
         sum(isnull([53],0))                           AS '消費取消',
         sum(isnull([51],0)) -sum(isnull([53],0)) AS '消費淨額'
FROM
(select a.CPT_DATE,a.store_no,b.STO_NAME_SHORT,a.TRANS_TYPE,sum(a.CM_VAL) SUM_AMT
from mpg.TM_SETTLE_DATA_D a,mpg.BM_STORE_M b
where a.MERCHANT_NO=b.MERCHANT_NO
and a.STORE_NO=b.STORE_NO
and a.CPT_DATE BETWEEN @Sdate AND @Edate ";
                    if (StoreNo != "")
                    {
                        sqlText += "and a.store_no=@STORE_NO ";
                    }
sqlText += @"group by a.CPT_DATE,a.store_no,b.STO_NAME_SHORT,a.TRANS_TYPE) T
PIVOT(sum(SUM_AMT) FOR TRANS_TYPE IN ([51],[52],[53],[54],[74],[75])) AS P1
GROUP BY CPT_DATE,store_no,STO_NAME_SHORT
ORDER BY CPT_DATE,STORE_NO
"; 

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    sqlCmd.Parameters.Add("@Sdate", SqlDbType.VarChar).Value = start ;
                    sqlCmd.Parameters.Add("@Edate", SqlDbType.VarChar).Value = end ;
                    sqlCmd.Parameters.Add("@STORE_NO", SqlDbType.VarChar).Value = StoreNo;

                    SqlDataAdapter adapter = new SqlDataAdapter(sqlCmd);

                    adapter.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                log.Debug(ex.Message);
            }
            return dt;
        }


        public DataTable Report0120T(string start, string end)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();
                    string sqlText =
                            @"select CPT_DATE '清分日',M.IP_REQ_USER '提案人',A.IP_KIND_NO '點數中心活動代號',D.IP_KIND_NAME '點數中心活動名稱',A.MERCHANT_NO '特約機構',A.IP_DATE_FROM '活動起日',A.IP_DATE_TO '活動迄日',sum(IP_CNT) '送出點數'
from GM_IP_DATA A,GM_IP_DETAIL D , GM_IP_MST M 
where D.IP_NO=M.IP_NO and D.IP_NO=A.IP_NO
and A.IP_KIND_NO=D.IP_KIND_NO
and A.IP_DATE_FROM=D.IP_DATE_FROM
and A.CPT_DATE BETWEEN @Sdate AND @Edate
group by CPT_DATE,M.IP_REQ_USER ,M.IP_NO,A.IP_KIND_NO,A.MERCHANT_NO,A.IP_DATE_FROM,A.IP_DATE_TO,D.IP_KIND_NAME
union 
select '' '清分日','' '提案人','總計' '點數中心活動代號','' '點數中心活動名稱','' '特約機構','' '活動起日','' '活動迄日',isnull(sum(IP_CNT),0) '送出點數'
from GM_IP_DATA A
where A.CPT_DATE BETWEEN @Sdate AND @Edate
order by A.IP_KIND_NO
";

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    sqlCmd.Parameters.Add("@Sdate", SqlDbType.VarChar).Value = start;
                    sqlCmd.Parameters.Add("@Edate", SqlDbType.VarChar).Value = end;

                    SqlDataAdapter adapter = new SqlDataAdapter(sqlCmd);

                    adapter.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                log.Debug(ex.Message);
            }
            return dt;
        }

        public DataTable Report0121T(string start)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();
                    string sqlText =
                            @"select c1 '製卡日',d1 '卡號',e1 'IDOLLAR 餘額轉置' from (
--SELECT 1 as a1,'' as b1,'愛金卡股份有限公司' as c1,'' as d1,'' as e1
--UNION
--SELECT 2 as a1,'' as b1,'IDollar餘額轉置月結明細表' as c1,'' as d1,'' as e1
--UNION
--SELECT 3 as a1,'' as b1,'' as c1 ,'' as d1,'' as e1  
--UNION
SELECT 7 as a1 ,'' as b1,Left(REG_END,8) as c1,a.CARD_ID as d1,convert(varchar,sum(convert(numeric,a.IDOLLAR))) as e1
from CM_IPOINT_D a,
CM_CARDNEED_D b,
VW_CC_CASE_M c
where 
     Left(REG_END,6)= @Sdate
     and b.SAM4_ID=c.CASE_ID
     and b.NEED_TYPE='L'
     and b.CASE_STATUS = 'Y'
     and a.CARD_ID=c.CARD_NO
     and b.need_note='壞卡換新卡'
     and a.MOD_DATE<'20140401'
GROUP BY Left(REG_END,8),a.CARD_ID
union
SELECT 7 as a1 ,'' as b1,Left(REG_END,8) as c1,a.CARD_ID as d1,convert(varchar,sum(convert(numeric,a.IDOLLAR))) as e1
from CM_IPOINT_D a,
CM_CARDNEED_D b,
CC_CASE_M c
where 
     Left(REG_END,6)= @Sdate
     and b.SAM4_ID=c.CASE_ID
     and b.NEED_TYPE='L'
     and b.CASE_STATUS = 'Y'
     and a.CARD_ID=c.CARD_NO
     and b.need_note='壞卡換新卡'
     and a.MOD_DATE<'20140401'
GROUP BY Left(REG_END,8),a.CARD_ID
union
select 9 as a1 ,'' as b1,'' as c1,'合計' as d1,convert(varchar,isnull(sum(convert(numeric,s1.IDOLLAR)),0)) as e1
from (
	select a.CARD_ID,a.IDOLLAR
	from CM_IPOINT_D a,
	CM_CARDNEED_D b,
	VW_CC_CASE_M c
	where 
		 Left(REG_END,6)= @Sdate
		 and b.SAM4_ID=c.CASE_ID
		 and b.NEED_TYPE='L'
		 and b.CASE_STATUS = 'Y'
		 and a.CARD_ID=c.CARD_NO
		 and b.need_note='壞卡換新卡'
		 and a.MOD_DATE<'20140401'
	union 
	select a.CARD_ID,a.IDOLLAR
	from CM_IPOINT_D a,
	CM_CARDNEED_D b,
	VW_CC_CASE_M c
	where 
		 Left(REG_END,6)= @Sdate
		 and b.SAM4_ID=c.CASE_ID
		 and b.NEED_TYPE='L'
		 and b.CASE_STATUS = 'Y'
		 and a.CARD_ID=c.CARD_NO
		 and b.need_note='壞卡換新卡'
		 and a.MOD_DATE<'20140401'
	) s1
) t1
order by a1
";

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    sqlCmd.Parameters.Add("@Sdate", SqlDbType.VarChar).Value = start;

                    SqlDataAdapter adapter = new SqlDataAdapter(sqlCmd);

                    adapter.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                log.Debug(ex.Message);
            }
            return dt;
        }

        public DataTable Report0121_1T()
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();
                    string sqlText =
                            @"SELECT '目前 idollar未轉置餘額' as d1,convert(varchar,sum(convert(numeric,a.IDOLLAR))-37) as e1             
from CM_IPOINT_D a
where
    a.CARD_ID not in 
    (    select c.CARD_NO
         from CM_CARDNEED_D b,VW_CC_CASE_M c
         where Left(b.REG_END,8)>='20140401'
         and b.SAM4_ID=c.CASE_ID
         and b.NEED_TYPE='L'
         and b.CASE_STATUS = 'Y'
         and b.need_note='壞卡換新卡'
            union 
         select c.CARD_NO
         from CM_CARDNEED_D b,CC_CASE_M c
         where Left(b.REG_END,8)>='20140401'
         and b.SAM4_ID=c.CASE_ID
         and b.NEED_TYPE='L'
         and b.CASE_STATUS = 'Y'
         and b.need_note='壞卡換新卡'
         )
     and a.MOD_DATE<'20140401'
     and  CSTATUS <= 2
";

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);

                    SqlDataAdapter adapter = new SqlDataAdapter(sqlCmd);

                    adapter.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                log.Debug(ex.Message);
            }
            return dt;
        }

        public DataTable Report0123T(string start, string end, string merchantNo, string Kind)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();
                    string sqlText =
                            @"select MERCHANT_NO '特約機構',TRANS_DATE '交易日' ,MERCHANT_STNAME  特約機構名稱, CARD_KIND 卡別, sum(isnull([21],0)) 購貨總額, sum(isnull([23],0)) 退貨總額, sum(isnull([21],0))-sum(isnull([23],0)) 購貨淨額, sum(isnull([21C],0)) 購貨總數, sum(isnull([23C],0)) 退貨總數 , sum(isnull([21C],0))-sum(isnull([23C],0)) 購貨淨數 
from 
(select TRANS_DATE ,G.MERCHANT_NO,GM.MERCHANT_STNAME , CARD_KIND , TRANS_TYPE,TRANS_TYPE+'C' as TRANS_TYPEC,sum(TRANS_AMT) SUM_AMT, SUM(TRANS_CNT) SUM_CNT from GM_CPT_CARD_SUM_D G
LEFT OUTER JOIN GM_MERCHANT GM
ON GM.MERCHANT_NO=G.MERCHANT_NO
WHERE G.TRANS_DATE BETWEEN @Sdate AND @Edate 
and TRANS_TYPE in ('21','23')
";
                    if (Kind != "''")
                    {
                        sqlText += "AND G.CARD_KIND in (" + @Kind  +")";
                    }
                    if (merchantNo != "")
                    {
                        sqlText += "AND GM.MERCHANT_NO=@MERCHANT_NO ";
                    }

                    sqlText += @"group by TRANS_DATE,G.MERCHANT_NO,GM.MERCHANT_STNAME, CARD_KIND,G.TRANS_TYPE
) T
PIVOT(sum(SUM_AMT) FOR TRANS_TYPE IN ([21],[23])) p 
PIVOT(sum(SUM_CNT) FOR TRANS_TYPEC IN ([21C],[23C])) p1 
";
                    sqlText += "group by TRANS_DATE,MERCHANT_NO,MERCHANT_STNAME, CARD_KIND ";
                    sqlText += "ORDER BY MERCHANT_NO,MERCHANT_STNAME,TRANS_DATE, CARD_KIND";

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    sqlCmd.Parameters.Add("@Sdate", SqlDbType.VarChar).Value = start ;
                    sqlCmd.Parameters.Add("@Edate", SqlDbType.VarChar).Value = end ;
                    sqlCmd.Parameters.Add("@MERCHANT_NO", SqlDbType.VarChar).Value = merchantNo;
                    sqlCmd.Parameters.Add("@Kind", SqlDbType.VarChar).Value = Kind;

                    SqlDataAdapter adapter = new SqlDataAdapter(sqlCmd);

                    adapter.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                log.Debug(ex.Message);
            }
            return dt;
        }

        public DataTable Report0124T(string StartA, string EndA, string StartB, string EndB)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();
                    string sqlText =
                            @"
    --主條件 交易日期區間
    --DECLARE @TRANS_DATE_B VARCHAR(8) = '20200101'
    --DECLARE @TRANS_DATE_E VARCHAR(8) = '20211231'
    --子條件 清分日期區間 (子條件可不選 不選時 前端要帶「-」)
    --DECLARE @CPT_DATE_B VARCHAR(8) = '20200101'
    --DECLARE @CPT_DATE_E VARCHAR(8) = '20211231'
    SELECT ISNULL(ICP.TradeType,'') [電支_交易類型]
	,ISNULL(CONVERT(VARCHAR(23),ICP.TradeCreateDate,121),'') [電支_訂單日期]
	,ISNULL(ICP.MerchantICPMID,'') [電支_收款方電支帳號]
	,ISNULL(ICP.ICPMID,'') [電支_付款方電支帳號]
	,ISNULL(ICP.ICC_NO,'')+ISNULL(ICP.RRN,'') [電支_特店訂單編號]
	,ISNULL(ICP.PaymentModeName,'') [電支_付款方式]
	,ISNULL(ICP.AmountSource,'') [電支_款項來源(銀行)]
	,ISNULL(CONVERT(VARCHAR(8),ICP.TrustAmount),'') [電支_信託金額]
	,ISNULL(ICP.PaymentStatusName,'') [電支_付款狀態]
	,CASE ICP.IsReversal WHEN 'Y' THEN '已沖正' ELSE '' END [電支_人工沖正狀態]
	,ISNULL(ICASH.MERCHANT_NAME,'') [電票_特約機構]
	,ISNULL(ICASH.CPT_DATE,'') [電票_清算日期]
	,ISNULL(ICASH.STO_NAME_LONG,'') [電票_門市名稱]
	,ISNULL(ICASH.TRANS_DATE_TXLOG,'') [電票_卡機交易時間]
	,ISNULL(ICASH.ICC_NO,'') [電票_卡號]
	,ISNULL(ICASH.RRN,'') [電票_RRN]
	,ISNULL(CONVERT(VARCHAR(8),ICASH.TRANS_AMT),'') [電票_交易金額]
FROM (
	SELECT CONVERT(VARCHAR(8),TradeCreateDate,112) [TRANS_DATE]
		,*
	FROM dbo.AL_ICP_TM_ICPDATA_D WITH(NOLOCK)
	WHERE TradeCreateDate BETWEEN DATEADD(D,-1,@TRANS_DATE_B)+' 23:50:00.000' AND DATEADD(D,1,@TRANS_DATE_E)+' 00:10:00.000'
) ICP
FULL OUTER JOIN (
	SELECT CONVERT(VARCHAR(8),D.TRANS_DATE_TXLOG) [TRANS_DATE]
		,M.MERCHANT_NAME
		,D.CPT_DATE
		,B.STO_NAME_LONG
		,D.TRANS_DATE_TXLOG
		,D.ICC_NO
		,D.RRN
		,D.TRANS_AMT
	FROM dbo.AL_ICP_TM_SETTLE_DATA_D D WITH(NOLOCK)
	INNER JOIN dbo.GM_MERCHANT M WITH(NOLOCK)
		ON D.TRANS_DATE_TXLOG BETWEEN CONVERT(VARCHAR(8),DATEADD(D,-1,@TRANS_DATE_B),112)+'235000' AND CONVERT(VARCHAR(8),DATEADD(D,1,@TRANS_DATE_E),112)+'001000'
		AND (
			(@CPT_DATE_B='-')
			OR (@CPT_DATE_E='-')
			OR (@CPT_DATE_B<>'-' AND @CPT_DATE_E<>'-' AND CPT_DATE BETWEEN @CPT_DATE_B AND @CPT_DATE_E)
		)
		AND D.MERCHANT_NO = M.MERCHANT_NO
	LEFT JOIN dbo.IM_STOREM_D B WITH(NOLOCK)
		ON CONVERT(VARCHAR(8),GETDATE(),112) BETWEEN B.EFF_DATE_FROM AND B.EFF_DATE_TO
		AND D.MERCHANT_NO = B.MERCHANT_NO
		AND B.STORE_NO = CASE D.MERCHANT_NO WHEN '22555003' THEN RIGHT(D.STORE_NO,6) ELSE D.STORE_NO END
) ICASH
	ON ICP.ICC_NO = ICASH.ICC_NO
	AND ICP.RRN = ICASH.RRN
	AND ICP.TrustAmount = ICASH.TRANS_AMT
WHERE (ICP.TRANS_DATE BETWEEN @TRANS_DATE_B AND @TRANS_DATE_E
		OR ICASH.TRANS_DATE BETWEEN @TRANS_DATE_B AND @TRANS_DATE_E)
ORDER BY ICP.TradeCreateDate DESC
";

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    sqlCmd.Parameters.Add("@TRANS_DATE_B", SqlDbType.VarChar).Value = StartA;
                    sqlCmd.Parameters.Add("@TRANS_DATE_E", SqlDbType.VarChar).Value = EndA;
                    sqlCmd.Parameters.Add("@CPT_DATE_B", SqlDbType.VarChar).Value = StartB;
                    sqlCmd.Parameters.Add("@CPT_DATE_E", SqlDbType.VarChar).Value = EndB;

                    SqlDataAdapter adapter = new SqlDataAdapter(sqlCmd);

                    adapter.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                log.Debug(ex.Message);
            }
            return dt;
        }

            public DataTable Report0311T(string start, string end, string merchantNo)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();
                    string sqlText =
                            @"
---- (add by 20160503 rita 新增 77離線自動加值 start)
--SELECT   MERCHANT_STNAME                     AS '銀行名稱',
--         CPT_DATE                            AS '特約銀行',
--                '' as ':',
--                sum(isnull([74AL],0)) + sum(isnull([77AL],0))                                                 AS '自動加值',
--                sum(isnull([75AL],0))                                                                         AS '代行授權自動加值',
--                sum(isnull([74AL],0)) + sum(isnull([75AL],0)) + sum(isnull([77AL],0))                         AS '加值TOTAL',
--         sum(isnull([74ICSH_RSP],0))+ sum(isnull([75ICSH_RSP],0))+ sum(isnull([77ICSH_RSP],0))                AS '再提示',
--         sum(isnull([74BANK_RSP_SKIP],0))*-1+sum(isnull([75BANK_RSP_SKIP],0))*-1+sum(isnull([77BANK_RSP_SKIP],0))*-1 +
--        SUM(ISNULL([74SKIP_REJ],0))*-1+SUM(ISNULL([75SKIP_REJ],0))*-1+SUM(ISNULL([77SKIP_REJ],0))*-1          AS '剔退(-)',
--                0 AS 'ICA不認列剔退(-)',
--                sum(isnull([ICSH_RA],0))                                                                      AS '餘返(-)',    
--                --資料已為負項，不需再乘-1
--        sum(isnull([21ICSH_RA_EX],0))+sum(isnull([24ICSH_RA_EX],0))+sum(isnull([22ICSH_RA_EX],0))+sum(isnull([23ICSH_RA_EX],0))   AS '餘返異常(+)', --2018.08.10加入22,23 秀汶
--                sum(isnull([ICSH_ADJ_ADD],0)) + sum(isnull([ICSH_ADJ_SUB],0))   AS '帳務調整(-)'     ,
--                --【ICSH_ADJ = ICSH_ADJ_ADD-ICSH_ADJ_SUB(資料已為負項)】
--                sum(isnull([ICSH_DAILY_NET],0))                                                               AS '匯款總金額',
--                [REMITTANCE_DATE]                                                                             AS '預計匯款日'
--FROM
--(
--                select CPT_DATE,b1.MERCHANT_STNAME,TRANS_TYPE + BNK_SETTLE_TYPE as SETTLE_TYPE, REMITTANCE_DATE, 
--                           sum(isnull(SETTLE_AMT,0)) as sumAmt, sum(isnull(SETTLE_CNT,0)) as sumCnt 
--                from CR_BNK_TOTAL_D a1,GM_MERCHANT b1
--                where  a1.BANK_MERCHANT=b1.MERCHANT_NO
--                --AND LEFT(CPT_DATE,6) = @Sdate   --年月 YYYYMM
--        AND CPT_DATE between @Sdate and @Edate 
--        AND (a1.BANK_MERCHANT=@p_merchant_no or @p_merchant_no='')
--                Group by CPT_DATE,b1.MERCHANT_STNAME,BNK_SETTLE_TYPE, TRANS_TYPE, REMITTANCE_DATE
--) a 
--PIVOT(sum(sumAmt) FOR SETTLE_TYPE 
--      IN ([74AL],[75AL],[77AL],
--                  [74ICSH_RSP],[75ICSH_RSP],[77ICSH_RSP],
--                  [74BANK_RSP_SKIP],[75BANK_RSP_SKIP],[77BANK_RSP_SKIP],[74SKIP_REJ],[75SKIP_REJ],[77SKIP_REJ],
--          [ICSH_RA],[21ICSH_RA_EX],[24ICSH_RA_EX],[22ICSH_RA_EX],[23ICSH_RA_EX],[ICSH_ADJ_ADD],[ICSH_ADJ_SUB],[ICSH_DAILY_NET])) AS P1 --2018.08.10加入22,23秀汶
--group by CPT_DATE, REMITTANCE_DATE,MERCHANT_STNAME
--ORDER BY 1,2;
Exec dbo.GEND_GM_BANK_CT_ADJAMT_D
DECLARE @T TABLE (
BANK_MERCHANT VARCHAR(50),		--銀行代碼
MERCHANT_STNAME VARCHAR(50),	--'銀行名稱'
CPT_DATE VARCHAR(8),			--特約銀行
FREE_SPACE VARCHAR(1),			--
AL_AMT BIGINT,					--自動加值
AL_AMT_2 BIGINT,				--離線自動加值
AL_TOTAL BIGINT,				--加值TOTAL
ICASH_RSP BIGINT,				--再提示
SKIP_REJ BIGINT,				--剔退(-)
ICA INT ,						--ICA不認列剔退(-)
ICASH_RA BIGINT,				--餘返
ICASH_RA_EX BIGINT,				--餘返異常(+)
ICASH_ADJ BIGINT,				--帳務調整(-)
ICSH_DAILY_NET BIGINT,			--匯款總金額
REMITTANCE_DATE VARCHAR(8)		--預計匯款日
)
--新增BANK_MERCHANT來對應GM_BANK_CT_ADJAMT_D
--增加北捷回饋金金額
insert into @T
SELECT  BANK_MERCHANT ,
		MERCHANT_STNAME                     AS '銀行名稱',
         CPT_DATE                            AS '特約銀行',
                '' as ':',
                sum(isnull([74AL],0)) + sum(isnull([77AL],0))                                                 AS '自動加值',
                sum(isnull([75AL],0))                                                                         AS '代行授權自動加值',
                sum(isnull([74AL],0)) + sum(isnull([75AL],0)) + sum(isnull([77AL],0))                         AS '加值TOTAL',
         sum(isnull([74ICSH_RSP],0))+ sum(isnull([75ICSH_RSP],0))+ sum(isnull([77ICSH_RSP],0))                AS '再提示',
         sum(isnull([74BANK_RSP_SKIP],0))*-1+sum(isnull([75BANK_RSP_SKIP],0))*-1+sum(isnull([77BANK_RSP_SKIP],0))*-1 +
        SUM(ISNULL([74SKIP_REJ],0))*-1+SUM(ISNULL([75SKIP_REJ],0))*-1+SUM(ISNULL([77SKIP_REJ],0))*-1          AS '剔退(-)',
                0 AS 'ICA不認列剔退(-)',
                sum(isnull([ICSH_RA],0))                                                                      AS '餘返(-)',    
                --資料已為負項，不需再乘-1
        sum(isnull([21ICSH_RA_EX],0))+sum(isnull([24ICSH_RA_EX],0))+sum(isnull([22ICSH_RA_EX],0))+sum(isnull([23ICSH_RA_EX],0))   AS '餘返異常(+)', --2018.08.10加入22,23 秀汶
                sum(isnull([ICSH_ADJ_ADD],0)) + sum(isnull([ICSH_ADJ_SUB],0))   AS '帳務調整(-)'     ,
                --【ICSH_ADJ = ICSH_ADJ_ADD-ICSH_ADJ_SUB(資料已為負項)】
                sum(isnull([ICSH_DAILY_NET],0))                                                               AS '匯款總金額',
                [REMITTANCE_DATE]                                                                             AS '預計匯款日'
FROM
(
                select CPT_DATE,b1.MERCHANT_STNAME,a1.BANK_MERCHANT,TRANS_TYPE + BNK_SETTLE_TYPE as SETTLE_TYPE, REMITTANCE_DATE, 
                           sum(isnull(SETTLE_AMT,0)) as sumAmt, sum(isnull(SETTLE_CNT,0)) as sumCnt 
                from CR_BNK_TOTAL_D a1,GM_MERCHANT b1
                where  a1.BANK_MERCHANT=b1.MERCHANT_NO
                --AND LEFT(CPT_DATE,6) = @Sdate   --年月 YYYYMM
        AND CPT_DATE between @Sdate and @Edate 
        AND (a1.BANK_MERCHANT=@p_merchant_no or @p_merchant_no='')
                Group by CPT_DATE,b1.MERCHANT_STNAME,BANK_MERCHANT,BNK_SETTLE_TYPE, TRANS_TYPE, REMITTANCE_DATE
) a 
PIVOT(sum(sumAmt) FOR SETTLE_TYPE 
      IN ([74AL],[75AL],[77AL],
                  [74ICSH_RSP],[75ICSH_RSP],[77ICSH_RSP],
                  [74BANK_RSP_SKIP],[75BANK_RSP_SKIP],[77BANK_RSP_SKIP],[74SKIP_REJ],[75SKIP_REJ],[77SKIP_REJ],
          [ICSH_RA],[21ICSH_RA_EX],[24ICSH_RA_EX],[22ICSH_RA_EX],[23ICSH_RA_EX],[ICSH_ADJ_ADD],[ICSH_ADJ_SUB],[ICSH_DAILY_NET])) AS P1 --2018.08.10加入22,23秀汶
group by CPT_DATE, REMITTANCE_DATE,MERCHANT_STNAME,BANK_MERCHANT
--ORDER BY 1,2;

UNION ALL

SELECT 
	BANK_MERCHANT,
	'愛金卡'                    AS '銀行名稱',
	CPT_DATE                    AS '特約銀行',
	'' as ':',
	sum(isnull(LOAD74_AMT,0))     AS '自動加值',
	0                             AS '代行授權自動加值',
	sum(isnull(LOAD74_AMT,0))     AS '加值TOTAL',
	0								AS '再提示',
	0								AS '剔退(-)',
	0								AS 'ICA不認列剔退(-)',
	0								AS '餘返(-)',    
	0								AS '餘返異常(+)', 
	0								AS '帳務調整(-)'     ,
	sum(isnull(LOAD_NET,0))		AS '匯款總金額',
	REMITTANCE_DATE				AS '預計匯款日'
FROM [dbo].[AL_ICP_TM_CPT_SUM_D]
WHERE CPT_DATE BETWEEN @Sdate AND @Edate
AND (BANK_MERCHANT = CASE WHEN @p_merchant_no = 'ICP' THEN BANK_MERCHANT ELSE @p_merchant_no END or @p_merchant_no='')
GROUP BY BANK_MERCHANT,CPT_DATE,REMITTANCE_DATE

select 

MERCHANT_STNAME AS '銀行名稱',	
a.CPT_DATE AS '特約銀行' ,		
FREE_SPACE ':' ,			
AL_AMT AS '自動加值' ,					
AL_AMT_2 AS '離線自動加值' ,				
AL_TOTAL AS '加值TOTAL' ,				
ICASH_RSP AS '再提示',				
SKIP_REJ AS '剔退(-)',				
ICA AS 'ICA不認列剔退(-)',						
ICASH_RA AS '餘返(-)' ,
ISNULL(TRT_ADJ,0) AS '餘返_特店回饋(-)' ,				
ICASH_RA_EX AS '餘返異常(+)' ,				
ICASH_ADJ-ISNULL(TRT_ADJ,0) AS '帳務調整(-)', --
ICSH_DAILY_NET AS '匯款總金額' ,			
a.REMITTANCE_DATE AS'預計匯款日'
from @T a
left join  
(select BANK_MERCHANT,CPT_DATE,REMITTANCE_DATE,sum(isnull(adj_amt,0) )*-1 AS TRT_ADJ from  GM_BANK_CT_ADJAMT_D where ADJCASE_NO like 'AA%'
and CPT_DATE between @Sdate and @Edate
GROUP BY BANK_MERCHANT,CPT_DATE,REMITTANCE_DATE) b
on a.BANK_MERCHANT = b.BANK_MERCHANT
and a.REMITTANCE_DATE = b.REMITTANCE_DATE
AND a.CPT_DATE = b.CPT_DATE
order by 1,2,15
";

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    sqlCmd.Parameters.Add("@Sdate", SqlDbType.VarChar).Value = start ;
                    sqlCmd.Parameters.Add("@Edate", SqlDbType.VarChar).Value = end;
                    sqlCmd.Parameters.Add("@p_merchant_no", SqlDbType.VarChar).Value = merchantNo;

                    SqlDataAdapter adapter = new SqlDataAdapter(sqlCmd);

                    adapter.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                log.Debug(ex.Message);
            }
            return dt;
        }

        public DataTable Report0312T(string start, string end, string merchantNo, string merchantBankNo)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();
                    string sqlText =
                            @"
SELECT  CPT_DATE 清算日期,A.SETTLE_DATE 特約機構清分日,A.MERCHANT_NO 統編,C.MERCHANT_STNAME 特約機構 ,B.MERCHANT_STNAME 銀行,
        ISNULL(SUM(LOAD74_CNT),0) + ISNULL(SUM(LOAD77_CNT),0) 自動加值筆數, -- (add by 20160415 rita 新增 77離線自動加值)
        ISNULL(SUM(LOAD74_AMT),0) + ISNULL(SUM(LOAD77_AMT),0) 自動加值金額, -- (add by 20160415 rita 新增 77離線自動加值)
        ISNULL(SUM(LOAD75_CNT),0) 代行加值筆數, ISNULL(SUM(LOAD75_AMT),0) 代行加值金額,
        ISNULL(SUM(LOAD74_CNT),0) + ISNULL(SUM(LOAD75_CNT),0) + ISNULL(SUM(LOAD77_CNT),0)  總筆數,  -- (add by 20160415 rita 新增 77離線自動加值)
        ISNULL(SUM(LOAD74_AMT),0) + ISNULL(SUM(LOAD75_AMT),0) + ISNULL(SUM(LOAD77_AMT),0)  總加值   -- (add by 20160415 rita 新增 77離線自動加值)
FROM  GM_BANK_CPT_SUM A,
(SELECT MERCHANT_NO ,MERCHANT_NAME ,MERCHANT_STNAME,MERC_GROUP
FROM GM_MERCHANT
WHERE MERCHANT_TYPE='BANK') B,
(SELECT MERCHANT_NO ,MERCHANT_NAME ,MERCHANT_STNAME,MERC_GROUP
FROM GM_MERCHANT
WHERE MERCHANT_TYPE<>'BANK') C
WHERE A.MERCHANT_NO=C.MERCHANT_NO
AND A.BANK_MERCHANT=B.MERCHANT_NO ";
sqlText += "and A.CPT_DATE between @Sdate and @Edate ";
                    if (merchantNo != "")
                    {
                        sqlText += "AND A.MERCHANT_NO=@MERCHANT_NO ";
                    }
                    if (merchantBankNo != "")
                    {
                        sqlText += "AND A.BANK_MERCHANT=@BANK_MERCHANT ";
                    }
                    sqlText += "GROUP BY CPT_DATE,A.SETTLE_DATE,A.MERCHANT_NO,C.MERCHANT_STNAME,BANK_MERCHANT,B.MERCHANT_STNAME ";
sqlText += "order by 1,2";

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    sqlCmd.Parameters.Add("@Sdate", SqlDbType.VarChar).Value = start;
                    sqlCmd.Parameters.Add("@Edate", SqlDbType.VarChar).Value = end;
                    sqlCmd.Parameters.Add("@MERCHANT_NO", SqlDbType.VarChar).Value = merchantNo;
                    sqlCmd.Parameters.Add("@BANK_MERCHANT", SqlDbType.VarChar).Value = merchantBankNo;

                    SqlDataAdapter adapter = new SqlDataAdapter(sqlCmd);

                    adapter.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                log.Debug(ex.Message);
            }
            return dt;
        }

        public DataTable Report0313SPT(string start, string end)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();
                    SqlCommand cmd = new SqlCommand("Insert_CM_CARDLOSS_RPT", sqlConn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@START_DATE", SqlDbType.NVarChar, 8).Value = start;
                    cmd.Parameters.Add("@END_DATE", SqlDbType.NVarChar, 8).Value = end;
                    SqlParameter retVal = cmd.Parameters.Add("@ReturnValue", SqlDbType.Int);
                    retVal.Direction = ParameterDirection.ReturnValue;
                    cmd.ExecuteNonQuery();

                    string sqlText =
                                                    @"SELECT CARD_NO as '卡號', case CARD_TYPE when '1' then '聯名卡' else (case CARD_TYPE when '2' then '記名卡' else '非記名卡' end) end as '卡別',
                        						 V1 as '掛失卡務餘額', V1-V2 as '3小時後損失' , V3 as '補發加值金額',(case when V3=0 then V4 else 0 end) as '補發餘返金額',
                                            (case when V3=0 then D4 else D3 end) as '補發日期'
                        					FROM dbo.CM_CARDLOSS_RPT
                        					WHERE SUBSTRING(MOD_DATE,1,8)>=@Sdate and SUBSTRING(MOD_DATE,1,8)<=@Edate order by 2 desc";

                    //sqlText += "and A.CPT_DATE between @Sdate and @Edate ";
                    //sqlText += "GROUP BY CPT_DATE,A.MERCHANT_NO,C.MERCHANT_STNAME,BANK_MERCHANT,B.MERCHANT_STNAME ";
                    //sqlText += "order by 1,2";

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    sqlCmd.Parameters.Add("@Sdate", SqlDbType.VarChar).Value = start;
                    sqlCmd.Parameters.Add("@Edate", SqlDbType.VarChar).Value = end;

                    SqlDataAdapter adapter = new SqlDataAdapter(sqlCmd);

                    adapter.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                log.Debug(ex.Message);
            }
            return dt;
        }

        public DataTable Report0313T(string start, string end)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();

                    string sqlText =
//                            @"SELECT CARD_NO as '卡號', case CARD_TYPE when '1' then '聯名卡' else (case CARD_TYPE when '2' then '記名卡' else '非記名卡' end) end as '卡別',
//						 V1 as '掛失卡務餘額', V1-V2 as '3小時後損失' , V3 as '補發加值金額',(case when V3=0 then V4 else 0 end) as '補發餘返金額',
//                    (case when V3=0 then D4 else D3 end) as '補發日期'
//					FROM dbo.CM_CARDLOSS_RPT
//					WHERE SUBSTRING(MOD_DATE,1,8)>=@Sdate and SUBSTRING(MOD_DATE,1,8)<=@Edate order by 2 desc";
                    @"select C.CPT_DATE 清分日期, G.MERCHANT_STNAME 發卡銀行,ICC_NO 卡號,case when CARD_GPKIND='01' then '聯名卡' else (case when CARD_GPKIND='02' then '記名卡' else '非記名卡' end)  end 卡別,C.RAMT_TYPE+':'+T.RAMT_NAME 狀態,CARD_AMT 掛失卡務餘額,LOST_AMT '3小時後損失',LOAD_AMT 補發加值金額,RETURN_AMT 餘返金額 from GM_STOP_CARD C
left outer join GM_MERCHANT G on G.MERCHANT_NO=C.BANK_MERCHANT
left outer join CR_RAMT_TYPE_MST T on T.RAMT_TYPE=C.RAMT_TYPE
where CPT_DATE between @Sdate and @Edate order by 1,2,4,5";
                    //sqlText += "and A.CPT_DATE between @Sdate and @Edate ";
                    //sqlText += "GROUP BY CPT_DATE,A.MERCHANT_NO,C.MERCHANT_STNAME,BANK_MERCHANT,B.MERCHANT_STNAME ";
                    //sqlText += "order by 1,2";

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    sqlCmd.Parameters.Add("@Sdate", SqlDbType.VarChar).Value = start;
                    sqlCmd.Parameters.Add("@Edate", SqlDbType.VarChar).Value = end;

                    SqlDataAdapter adapter = new SqlDataAdapter(sqlCmd);

                    adapter.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                log.Debug(ex.Message);
            }
            return dt;
        }

        public DataTable Report0314T(string start, string merchantNo, string MERC_GROUP)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();

                    string sqlText =
                            String.Format(@"SELECT G.MERCHANT_STNAME as '銀行',
@p_cpt_date as '請款日期',
ISNULL(SUM(A.AL_AMT),0) as '金額',			--自動加值金額
ISNULL(SUM(A.AL_CNT),0) as '筆數',			--自動加值數量
ISNULL(SUM(A.BNK_SKIP_AMT),0) as '　金額　',		--銀行剔退金額
ISNULL(SUM(A.BNK_SKIP_CNT),0) as '　筆數　',		--銀行剔退數量
ISNULL(SUM(A.ICSH_R_AMT),0) as '　金額',		--再提示金額
ISNULL(SUM(A.ICSH_R_CNT),0) as '　筆數',		--再提示數量
ISNULL(SUM(A.AL_AMT),0)+ISNULL(SUM(A.BNK_SKIP_AMT),0)+ISNULL(SUM(A.ICSH_R_AMT),0) as '發卡行應付金額'
FROM
(SELECT 
BANK_MERCHANT,
	ISNULL(SETTLE_AMT,0) AL_AMT,
	ISNULL(SETTLE_CNT,0) AL_CNT,
	0	BNK_SKIP_AMT,
	0	BNK_SKIP_CNT,
	0	ICSH_R_AMT,
	0	ICSH_R_CNT
FROM GM_BANK_SU_BNK_TOTAL_D
WHERE CPT_DATE=@p_cpt_date
AND (BANK_MERCHANT=@p_merchant_no or @p_merchant_no='') 
AND BNK_SETTLE_TYPE='AL'
UNION ALL
SELECT
BANK_MERCHANT,
	0 AL_AMT,
	0 AL_CNT,
	ISNULL(SETTLE_AMT,0) BNK_SKIP_AMT,
	ISNULL(SETTLE_CNT,0) BNK_SKIP_CNT,
	0	ICSH_R_AMT,
	0	ICSH_R_CNT
FROM GM_BANK_SU_BNK_TOTAL_D
WHERE  CPT_DATE=@p_cpt_date
AND (BANK_MERCHANT=@p_merchant_no or @p_merchant_no='') 
AND BNK_SETTLE_TYPE IN ('SKIP_REJ','BANK_RSP_SKIP')
UNION ALL
SELECT
BANK_MERCHANT,
	0	AL_AMT,
	0	AL_CNT,
	0	BNK_SKIP_AMT,
	0	BNK_SKIP_CNT,
	ISNULL(SETTLE_AMT,0) ICSH_R_AMT,
	ISNULL(SETTLE_CNT,0) ICSH_R_CNT
FROM GM_BANK_SU_BNK_TOTAL_D
WHERE  CPT_DATE=@p_cpt_date
AND (BANK_MERCHANT=@p_merchant_no or @p_merchant_no='') 
AND BNK_SETTLE_TYPE='ICSH_RSP') A, GM_MERCHANT G where A.BANK_MERCHANT=G.MERCHANT_NO group by G.MERCHANT_STNAME", MERC_GROUP);

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    sqlCmd.Parameters.Add("@p_cpt_date", SqlDbType.VarChar).Value = start;
                    sqlCmd.Parameters.Add("@p_merchant_no", SqlDbType.VarChar).Value = merchantNo;
                    sqlCmd.Parameters.Add("@p_schema", SqlDbType.VarChar).Value = MERC_GROUP;

                    SqlDataAdapter adapter = new SqlDataAdapter(sqlCmd);

                    adapter.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                log.Debug(ex.Message);
            }
            return dt;
        }

        public DataTable Report0315T(string start, string end, string merchantNo, string MERC_GROUP)
        {
            int year = DateTime.Today.Year;
            int month = DateTime.Today.Month;

            start = start + "01";
            DateTime startDate = DateTime.Parse(start.Substring(0, 4) + "-" + start.Substring(4, 2) + "-" + start.Substring(6, 2)).AddMonths(-2);
            year = startDate.Year;
            month = startDate.Month;
            startDate = new DateTime(year, month, 25);
            DateTime EndDate = DateTime.Parse(start.Substring(0, 4) + "-" + start.Substring(4, 2) + "-" + start.Substring(6, 2)).AddMonths(-1);
            year = EndDate.Year;
            month = EndDate.Month;
            EndDate = new DateTime(year, month, 24);

            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();

                    string sqlText = "SELECT MERCHANT_STNAME '銀行','" + startDate.ToString("yyyy/MM/dd") + "' + '~' + '" + EndDate.ToString("yyyy/MM/dd") + "'";
                    sqlText +=String.Format(@" as 簽帳金額計算期間,
ISNULL(SUM(ISNULL(C1,0)),0) as '卡數',
ISNULL(SUM(ISNULL(A1,0)),0) as '簽帳金額',
ISNULL(SUM(ISNULL(F1,0)),0) as '簽帳回饋金',
ISNULL(SUM(ISNULL(C2,0)),0) as '　卡數　',
ISNULL(SUM(ISNULL(A2,0)),0) as '　簽帳金額　',
ISNULL(SUM(ISNULL(F2,0)),0) as '　簽帳回饋金　',
ISNULL(SUM(ISNULL(F1,0)),0)+ISNULL(SUM(ISNULL(F2,0)),0) as '發卡行應付金額'
FROM
(
SELECT MERCHANT_STNAME,CPT_SDATE,CPT_EDATE,CPT_CNT AS C1,CPT_AMT AS A1,FEE_AMT AS F1,0 AS C2,0 AS A2,0 AS F2
FROM GM_BANK_FS_FEESUM_M
WHERE FEE_DATE between @p_start_date and @p_end_date
AND CR_FEE_KIND='BANK_REFUND'
AND CARD_PN02='0'
AND (BANK_MERCHANT=@p_merchant_no or @p_merchant_no='')
UNION ALL
SELECT MERCHANT_STNAME,CPT_SDATE,CPT_EDATE,0 AS C1,0 AS A1,0 AS F1,CPT_CNT AS C2,CPT_AMT AS A2,FEE_AMT AS F2
FROM GM_BANK_FS_FEESUM_M
WHERE FEE_DATE between @p_start_date and @p_end_date
AND CR_FEE_KIND='BANK_REFUND'
AND CARD_PN02='1'
AND (BANK_MERCHANT=@p_merchant_no or @p_merchant_no='')) A
GROUP BY MERCHANT_STNAME,CPT_SDATE,CPT_EDATE", MERC_GROUP);

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    sqlCmd.Parameters.Add("@p_start_date", SqlDbType.VarChar).Value = start ;
                    sqlCmd.Parameters.Add("@p_end_date", SqlDbType.VarChar).Value = end + "31";
                    sqlCmd.Parameters.Add("@p_merchant_no", SqlDbType.VarChar).Value = merchantNo;

                    SqlDataAdapter adapter = new SqlDataAdapter(sqlCmd);

                    adapter.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                log.Debug(ex.Message);
            }
            return dt;
        }

        public DataTable Report0316T(string start, string merchantNo, string MERC_GROUP)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();

                    string sqlText = String.Format(@"SELECT ROW_NUMBER() OVER(ORDER BY A.ADJCASE_NO) as '序號', A.MERCHANT_STNAME '銀行',A.ADJCASE_NO as '案件編號',M.ADJCASE_INFO as '調整原因內容顯示',A.ICC_NO as 'icash卡號',
CASE A.ADJ_FLG WHEN 'BK' THEN A.ADJ_AMT*-1 ELSE A.ADJ_AMT END as '該筆帳務調整金額',
M.ADJCASE_CONTEXT as '該筆帳務調整詳細說明'
FROM GM_BANK_CT_ADJAMT_D A 
left outer join dbo.CR_BANK_ADJCASE_M M
on A.ADJCASE_NO=M.ADJCASE_NO 
WHERE A.CPT_DATE=@p_cpt_date
", MERC_GROUP);
                    if (merchantNo != "")
                    {
                        sqlText += "AND A.BANK_MERCHANT=@p_merchant_no";
                    }
                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    sqlCmd.Parameters.Add("@p_cpt_date", SqlDbType.VarChar).Value = start;
                    sqlCmd.Parameters.Add("@p_merchant_no", SqlDbType.VarChar).Value = merchantNo;

                    SqlDataAdapter adapter = new SqlDataAdapter(sqlCmd);

                    adapter.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                log.Debug(ex.Message);
            }
            return dt;
        }

        public DataTable Report0317T(string start, string merchantNo, string MERC_GROUP)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();

                    string sqlText = String.Format(@"SELECT  A.MERCHANT_STNAME '銀行',A.ADJCASE_NO as '案件編號',M.ADJCASE_INFO as '調整原因內容顯示',COUNT(*) as '愛金卡應付之回饋',SUM(CASE A.ADJ_FLG WHEN 'BK' THEN ADJ_AMT*-1 ELSE ADJ_AMT END) as '愛金卡應付之回饋調整金額' 
FROM GM_BANK_CT_ADJAMT_D A,
dbo.CR_BANK_ADJCASE_M M
WHERE 
A.CPT_DATE=@p_cpt_date
AND A.ADJCASE_NO=M.ADJCASE_NO ", MERC_GROUP);
                    if (merchantNo != "")
                    {
                        sqlText += "AND A.BANK_MERCHANT=@p_merchant_no ";
                    }
                    sqlText += @"GROUP BY A.MERCHANT_STNAME,A.ADJCASE_NO,M.ADJCASE_INFO
UNION ALL
SELECT '','','總計',COUNT(*),isnull(SUM(CASE A.ADJ_FLG WHEN 'BK' THEN ADJ_AMT*-1 ELSE ADJ_AMT END),0)
FROM GM_BANK_CT_ADJAMT_D A,
dbo.CR_BANK_ADJCASE_M M
WHERE 
A.CPT_DATE=@p_cpt_date
AND A.ADJCASE_NO=M.ADJCASE_NO ";

                    if (merchantNo != "")
                    {
                        sqlText += "AND A.BANK_MERCHANT=@p_merchant_no";
                    }
                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    sqlCmd.Parameters.Add("@p_cpt_date", SqlDbType.VarChar).Value = start;
                    sqlCmd.Parameters.Add("@p_merchant_no", SqlDbType.VarChar).Value = merchantNo;

                    SqlDataAdapter adapter = new SqlDataAdapter(sqlCmd);

                    adapter.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                log.Debug(ex.Message);
            }
            return dt;
        }

        public DataTable Report0318T(string start, string end, string merchantNo, string MERC_GROUP)
        {
            int year = DateTime.Today.Year;
            int month = DateTime.Today.Month;

            DateTime startDate = DateTime.Parse(start.Substring(0, 4) + "-" + start.Substring(4, 2) + "-" + start.Substring(6, 2)).AddMonths(-2);
            year = startDate.Year;
            month = startDate.Month;
            startDate = new DateTime(year, month, 25);
            DateTime EndDate = DateTime.Parse(start.Substring(0, 4) + "-" + start.Substring(4, 2) + "-" + start.Substring(6, 2)).AddMonths(-1);
            year = EndDate.Year;
            month = EndDate.Month;
            EndDate = new DateTime(year, month, 24);

            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();

                    string sqlText = String.Format(@"--宣告清分日的參數
declare @S_DATE varchar(8),@E_DATE varchar(8)
select @E_DATE = MAX(INFO_DATE),@S_DATE = MIN(INFO_DATE) 
from [{0}].FS_FEESUM_M 
where FEE_DATE between @p_start_date and @p_end_date
AND (BANK_MERCHANT=@p_merchant_no or @p_merchant_no='')
AND CR_FEE_KIND='AUTOLOAD_FEE'
and ISNULL(INFO_DATE,'') != ''

--宣告手續費費率(固定費率)
declare @FEE_RATE float
select @FEE_RATE = FEE_RATE
from CR_CONTRACT_D 
where MERCHANT_NO=@p_merchant_no
AND CR_FEE_KIND='AUTOLOAD_FEE'
and CARD_PN02 = '0'
and EFF_DATE_FROM < @S_DATE
and EFF_DATE_TO > @E_DATE

--產生明細
select 
MERCHANT_STNAME '銀行',
d.CPT_DATE as '自動加值請款日期',
SUM(d.AUTOLOAD_FEE_AMT) as '自動加值金額',
SUM(d.TPE_BUS_AMT+d.UT3_AMT-d.UT3_REJ_AMT) as '無手續費收入之金額',
SUM(d.LA_AMT-d.LA_REJ_AMT) as '現金加值金額'
from
(
	select MERCHANT_STNAME,a.INFO_DATE CPT_DATE, a.BANK_MERCHANT, a.CARD_PN02, COALESCE(SUM(ISNULL(CONVERT(NUMERIC,a.CPT_AMT),0)),0) as AUTOLOAD_FEE_AMT 
			 ,0 as TPE_BUS_AMT 
			 ,0 as UT3_AMT
			 ,0 as UT3_REJ_AMT 
			 ,0 as LA_AMT
			 ,0 as LA_REJ_AMT 
	from [{0}].FS_FEESUM_M a 
	INNER JOIN GM_MERCHANT b ON b.MERCHANT_NO=a.BANK_MERCHANT
	WHERE (a.BANK_MERCHANT=@p_merchant_no or @p_merchant_no='')
	AND a.FEE_DATE between @p_start_date and @p_end_date
	AND a.CR_FEE_KIND='AUTOLOAD_FEE'
	group by MERCHANT_STNAME,a.INFO_DATE, a.BANK_MERCHANT, a.CARD_PN02
	union all
	select MERCHANT_STNAME,p1.CPT_DATE, p1.BANK_MERCHANT, p1.CARD_PN02, 0 as AUTOLOAD_FEE_AMT
			 , COALESCE(SUM(ISNULL(CONVERT(NUMERIC,p1.TPE_BUS),0)),0) as TPE_BUS_AMT
			 , COALESCE(SUM(ISNULL(CONVERT(NUMERIC,p1.UT3),0)),0)   as UT3_AMT 
			 , COALESCE(SUM(ISNULL(CONVERT(NUMERIC,p1.UT3_REJ),0)),0) as UT3_REJ_AMT 
			 , COALESCE(SUM(ISNULL(CONVERT(NUMERIC,p1.LA),0)),0)      as LA_AMT
			 , COALESCE(SUM(ISNULL(CONVERT(NUMERIC,p1.LA_REJ),0)),0)  as LA_REJ_AMT 
	from [{0}].AL_DISCOUNT_SUM_D b
	INNER JOIN GM_MERCHANT c ON c.MERCHANT_NO=b.BANK_MERCHANT
	pivot(sum(SUM_VALUE) for D_TYPE
	in ([TPE_BUS],[UT3],[UT3_REJ],[LA],[LA_REJ])) as p1
	where p1.CPT_DATE between @S_DATE and @E_DATE 
	group by MERCHANT_STNAME,p1.CPT_DATE, p1.BANK_MERCHANT, p1.CARD_PN02
) d
group by MERCHANT_STNAME,d.CPT_DATE

union all

select '',
'A.合計' as INFO_DATE,
SUM(d.AUTOLOAD_FEE_AMT) as CPT_AMT,
SUM(d.TPE_BUS_AMT+d.UT3_AMT-d.UT3_REJ_AMT) as DIS_AMT,
SUM(d.LA_AMT-d.LA_REJ_AMT) as LA_NET
from
(
	select MERCHANT_STNAME,a.INFO_DATE CPT_DATE, a.BANK_MERCHANT, a.CARD_PN02, COALESCE(SUM(ISNULL(CONVERT(NUMERIC,a.CPT_AMT),0)),0) as AUTOLOAD_FEE_AMT 
			 ,0 as TPE_BUS_AMT 
			 ,0 as UT3_AMT
			 ,0 as UT3_REJ_AMT 
			 ,0 as LA_AMT
			 ,0 as LA_REJ_AMT 
	from [{0}].FS_FEESUM_M a 
	INNER JOIN GM_MERCHANT b ON b.MERCHANT_NO=a.BANK_MERCHANT
	WHERE (a.BANK_MERCHANT=@p_merchant_no or @p_merchant_no='')
	AND a.FEE_DATE between @p_start_date and @p_end_date
	AND a.CR_FEE_KIND='AUTOLOAD_FEE'
	group by MERCHANT_STNAME,a.INFO_DATE, a.BANK_MERCHANT, a.CARD_PN02
	union all
	select MERCHANT_STNAME,p1.CPT_DATE, p1.BANK_MERCHANT, p1.CARD_PN02, 0 as AUTOLOAD_FEE_AMT
			 , COALESCE(SUM(ISNULL(CONVERT(NUMERIC,p1.TPE_BUS),0)),0) as TPE_BUS_AMT
			 , COALESCE(SUM(ISNULL(CONVERT(NUMERIC,p1.UT3),0)),0)   as UT3_AMT 
			 , COALESCE(SUM(ISNULL(CONVERT(NUMERIC,p1.UT3_REJ),0)),0) as UT3_REJ_AMT 
			 , COALESCE(SUM(ISNULL(CONVERT(NUMERIC,p1.LA),0)),0)      as LA_AMT
			 , COALESCE(SUM(ISNULL(CONVERT(NUMERIC,p1.LA_REJ),0)),0)  as LA_REJ_AMT 
	from [{0}].AL_DISCOUNT_SUM_D b
	INNER JOIN GM_MERCHANT c ON c.MERCHANT_NO=b.BANK_MERCHANT
	pivot(sum(SUM_VALUE) for D_TYPE
	in ([TPE_BUS],[UT3],[UT3_REJ],[LA],[LA_REJ])) as p1
	where 
	p1.CPT_DATE between @S_DATE and @E_DATE 
	group by MERCHANT_STNAME,p1.CPT_DATE, p1.BANK_MERCHANT, p1.CARD_PN02
) d

union all

SELECT '',
'B.可計算自動加值手續費之金額',
case 
	when SUM(d.TPE_BUS_AMT+d.UT3_AMT-d.UT3_REJ_AMT) > SUM(d.LA_AMT-d.LA_REJ_AMT) --無手續費金額 > 現金加值金額
		then SUM(d.AUTOLOAD_FEE_AMT) -SUM(d.TPE_BUS_AMT+d.UT3_AMT-d.UT3_REJ_AMT) + SUM(d.LA_AMT-d.LA_REJ_AMT)
	when SUM(d.TPE_BUS_AMT+d.UT3_AMT-d.UT3_REJ_AMT) <= SUM(d.LA_AMT-d.LA_REJ_AMT)  --無手續費金額 <= 現金加值金額
		then SUM(d.AUTOLOAD_FEE_AMT) 
else 0
end,
0,0
FROM (
	select MERCHANT_STNAME,a.INFO_DATE CPT_DATE, a.BANK_MERCHANT, a.CARD_PN02, COALESCE(SUM(ISNULL(CONVERT(NUMERIC,a.CPT_AMT),0)),0) as AUTOLOAD_FEE_AMT 
			 ,0 as TPE_BUS_AMT 
			 ,0 as UT3_AMT
			 ,0 as UT3_REJ_AMT 
			 ,0 as LA_AMT
			 ,0 as LA_REJ_AMT 
	from [{0}].FS_FEESUM_M a 
	INNER JOIN GM_MERCHANT b ON b.MERCHANT_NO=a.BANK_MERCHANT
	WHERE (a.BANK_MERCHANT=@p_merchant_no or @p_merchant_no='')
	AND a.FEE_DATE between @p_start_date and @p_end_date
	AND a.CR_FEE_KIND='AUTOLOAD_FEE'
	group by MERCHANT_STNAME,a.INFO_DATE, a.BANK_MERCHANT, a.CARD_PN02
	union all
	select MERCHANT_STNAME,p1.CPT_DATE, p1.BANK_MERCHANT, p1.CARD_PN02, 0 as AUTOLOAD_FEE_AMT
			 , COALESCE(SUM(ISNULL(CONVERT(NUMERIC,p1.TPE_BUS),0)),0) as TPE_BUS_AMT
			 , COALESCE(SUM(ISNULL(CONVERT(NUMERIC,p1.UT3),0)),0)   as UT3_AMT 
			 , COALESCE(SUM(ISNULL(CONVERT(NUMERIC,p1.UT3_REJ),0)),0) as UT3_REJ_AMT 
			 , COALESCE(SUM(ISNULL(CONVERT(NUMERIC,p1.LA),0)),0)      as LA_AMT
			 , COALESCE(SUM(ISNULL(CONVERT(NUMERIC,p1.LA_REJ),0)),0)  as LA_REJ_AMT 
	from [{0}].AL_DISCOUNT_SUM_D b
	INNER JOIN GM_MERCHANT c ON c.MERCHANT_NO=b.BANK_MERCHANT
	pivot(sum(SUM_VALUE) for D_TYPE
	in ([TPE_BUS],[UT3],[UT3_REJ],[LA],[LA_REJ])) as p1
	where p1.CPT_DATE between @S_DATE and @E_DATE 
	group by MERCHANT_STNAME,p1.CPT_DATE, p1.BANK_MERCHANT, p1.CARD_PN02
) d

union all

SELECT '',
'C.本月自動加值手續費之金額',
case
	when SUM(d.TPE_BUS_AMT+d.UT3_AMT-d.UT3_REJ_AMT) > SUM(d.LA_AMT-d.LA_REJ_AMT) --無手續費金額 > 現金加值金額
		then ROUND((SUM(d.AUTOLOAD_FEE_AMT) -SUM(d.TPE_BUS_AMT+d.UT3_AMT-d.UT3_REJ_AMT) + SUM(d.LA_AMT-d.LA_REJ_AMT)) * @FEE_RATE,0)
	when SUM(d.TPE_BUS_AMT+d.UT3_AMT-d.UT3_REJ_AMT) <= SUM(d.LA_AMT-d.LA_REJ_AMT)  --無手續費金額 <= 現金加值金額
		then ROUND(SUM(d.AUTOLOAD_FEE_AMT) * @FEE_RATE,0)
else 0 end,
0,0
FROM  
(
	select MERCHANT_STNAME,a.INFO_DATE CPT_DATE, a.BANK_MERCHANT, a.CARD_PN02, COALESCE(SUM(ISNULL(CONVERT(NUMERIC,a.CPT_AMT),0)),0) as AUTOLOAD_FEE_AMT 
			 ,0 as TPE_BUS_AMT 
			 ,0 as UT3_AMT
			 ,0 as UT3_REJ_AMT 
			 ,0 as LA_AMT
			 ,0 as LA_REJ_AMT 
			 --,a.FEE_RATE FEE_RATE
	from [{0}].FS_FEESUM_M a 
	INNER JOIN GM_MERCHANT b ON b.MERCHANT_NO=a.BANK_MERCHANT
	WHERE (a.BANK_MERCHANT=@p_merchant_no or @p_merchant_no='')
	AND a.FEE_DATE between @p_start_date and @p_end_date
	AND a.CR_FEE_KIND='AUTOLOAD_FEE'
	group by MERCHANT_STNAME,a.INFO_DATE, a.BANK_MERCHANT, a.CARD_PN02--,a.FEE_RATE
	union all
	select MERCHANT_STNAME,p1.CPT_DATE, p1.BANK_MERCHANT, p1.CARD_PN02, 0 as AUTOLOAD_FEE_AMT
			 , COALESCE(SUM(ISNULL(CONVERT(NUMERIC,p1.TPE_BUS),0)),0) as TPE_BUS_AMT
			 , COALESCE(SUM(ISNULL(CONVERT(NUMERIC,p1.UT3),0)),0)   as UT3_AMT 
			 , COALESCE(SUM(ISNULL(CONVERT(NUMERIC,p1.UT3_REJ),0)),0) as UT3_REJ_AMT 
			 , COALESCE(SUM(ISNULL(CONVERT(NUMERIC,p1.LA),0)),0)      as LA_AMT
			 , COALESCE(SUM(ISNULL(CONVERT(NUMERIC,p1.LA_REJ),0)),0)  as LA_REJ_AMT 
			 --,0.0 as FEE_RATE
	from [{0}].AL_DISCOUNT_SUM_D b
	INNER JOIN GM_MERCHANT c ON c.MERCHANT_NO=b.BANK_MERCHANT
	pivot(sum(SUM_VALUE) for D_TYPE
	in ([TPE_BUS],[UT3],[UT3_REJ],[LA],[LA_REJ])) as p1
	where 
	p1.CPT_DATE between @S_DATE and @E_DATE 
	group by MERCHANT_STNAME,p1.CPT_DATE, p1.BANK_MERCHANT, p1.CARD_PN02 
) d 
--group by d.FEE_RATE
order by CPT_DATE,MERCHANT_STNAME ", MERC_GROUP);

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    sqlCmd.Parameters.Add("@p_start_date", SqlDbType.VarChar).Value = start;
                    sqlCmd.Parameters.Add("@p_end_date", SqlDbType.VarChar).Value = end + "31";
                    sqlCmd.Parameters.Add("@p_merchant_no", SqlDbType.VarChar).Value = merchantNo;

                    SqlDataAdapter adapter = new SqlDataAdapter(sqlCmd);

                    adapter.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                log.Debug(ex.Message);
            }
            return dt;
        }

        public DataTable Report0319T(string start, string end, string merchantNo, string MERC_GROUP)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();

                    string sqlText = String.Format(@"SELECT ROW_NUMBER() OVER(ORDER BY A.MERCHANT_STNAME,E_TIME,ICC_NO) as '序號',A.MERCHANT_STNAME as '銀行',
ICC_NO as 'icash卡號',E_TIME as '首次自動加值時間',B.MERCHANT_STNAME as '發生之特約機構',
CASE EN_1ST
WHEN 'Y' THEN '新辦卡首次啟用'
WHEN 'C' THEN '補換續卡首次啟用' 
WHEN 'N' THEN '非首次啟用' 
WHEN '0' THEN '未啟用' 
END as '發生期間'
FROM GM_BANK_FS_CARD_INFO A,DBO.GM_MERCHANT B
WHERE CPT_DATE between @p_start_date and @p_end_date
AND SUBSTRING(A.CR_FEE_KIND,1,10) = 'CARD_FIRST'
AND A.MERCHANT_NO=B.MERCHANT_NO
AND (A.BANK_MERCHANT=@p_merchant_no or @p_merchant_no='')
order by A.MERCHANT_STNAME", MERC_GROUP);

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    sqlCmd.Parameters.Add("@p_start_date", SqlDbType.VarChar).Value = start;
                    sqlCmd.Parameters.Add("@p_end_date", SqlDbType.VarChar).Value = end + "31";
                    sqlCmd.Parameters.Add("@p_merchant_no", SqlDbType.VarChar).Value = merchantNo;

                    SqlDataAdapter adapter = new SqlDataAdapter(sqlCmd);

                    adapter.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                log.Debug(ex.Message);
            }
            return dt;
        }

        public DataTable Report0320T(string start, string end, string merchantNo, string MERC_GROUP)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();

                    string sqlText = String.Format(@"
SELECT 	MERCHANT_STNAME as '銀行',DATE1,
		ROUND(cast(ISNULL(SUM(ISNULL(ROUND(CNT1,0),0)),0) as int),0) as '卡數',
		ROUND(cast(ISNULL(SUM(ISNULL(ROUND(AMT1,0),0)),0) as int),0) as '手續費',
		ROUND(cast(ISNULL(SUM(ISNULL(ROUND(CNT2,0),0)),0) as int),0) as '　卡數　',
		ROUND(cast(ISNULL(SUM(ISNULL(ROUND(AMT2,0),0)),0) as int),0) as '　手續費　',
		ROUND(cast(ISNULL(SUM(ISNULL(ROUND(AMT1,0),0)),0)+ISNULL(SUM(ISNULL(ROUND(AMT2,0),0)),0)  as int),0)
	FROM
	(SELECT B1.MERCHANT_STNAME ,LEFT(E_TIME,8) DATE1,COUNT(*) CNT1,COUNT(*) * B1.FEE_RATE AMT1,0 CNT2,0 AMT2
		FROM GM_BANK_FS_CARD_INFO A1, 
			 (SELECT C.MERCHANT_STNAME,B.FEE_RATE, B.CR_FEE_KIND, B.CARD_PN01, B.CARD_PN02, A.MERCHANT_NO
				FROM (SELECT *, CONVERT(CHAR(6), DATEADD(MONTH, CONVERT(int, A.SUM_MON_S),@p_start_date), 112) + A.SUM_DAY_S as sumDateStart, 
							   case when A.SUM_DAY_E = '99' 
									then CONVERT(CHAR(8), DATEADD(DAY,-1, CONVERT(CHAR(6), DATEADD(MONTH, CONVERT(int, A.SUM_MON_E) + 1 ,@p_start_date), 112) + '01'),112) 
									else CONVERT(CHAR(6), DATEADD(MONTH, CONVERT(int, A.SUM_MON_E),@p_start_date), 112) + A.SUM_DAY_E end as sumDateEnd
						   from CR_CONTRACT_M A
						  WHERE @p_start_date BETWEEN A.EFF_DATE_FROM AND A.EFF_DATE_TO) A,      
					  (SELECT * FROM CR_CONTRACT_D WHERE @p_end_date BETWEEN EFF_DATE_FROM AND EFF_DATE_TO) B,     
					  (SELECT MERCHANT_NO,MERCHANT_STNAME FROM GM_MERCHANT WHERE MERCHANT_TYPE='BANK' AND TM_FLG='Y') C,     
					  (SELECT * FROM CR_FEE_KIND_MST) D     
				WHERE A.MERCHANT_NO=B.MERCHANT_NO       
				  AND A.CR_FEE_KIND=B.CR_FEE_KIND       
				  AND A.MERCHANT_NO=C.MERCHANT_NO       
				  AND A.CR_FEE_KIND=D.CR_FEE_KIND
				  AND SUBSTRING(A.CR_FEE_KIND,1,10) = 'CARD_FIRST'
				  AND (A.MERCHANT_NO=@p_merchant_no or @p_merchant_no='')
				  )B1
		WHERE A1.CPT_DATE between @p_start_date and @p_end_date
          AND (A1.BANK_MERCHANT=@p_merchant_no or @p_merchant_no='')
          AND A1.BANK_MERCHANT=B1.MERCHANT_NO
		  AND A1.CR_FEE_KIND = B1.CR_FEE_KIND
		  AND A1.CARD_PN01 = (Case when B1.CARD_PN01 = '' then A1.CARD_PN01
								   else B1.CARD_PN01 end)
		  AND A1.CARD_PN02 = (Case when B1.CARD_PN02 = '' then A1.CARD_PN02
								   else B1.CARD_PN02 end)
		  AND A1.EN_1ST = 'Y'
		GROUP BY LEFT(A1.E_TIME,8), B1.MERCHANT_STNAME,B1.FEE_RATE
	UNION ALL
		SELECT B1.MERCHANT_STNAME ,LEFT(E_TIME,8) DATE1,0 CNT1 ,0 AMT1,COUNT(*) CNT2,COUNT(*) * B1.FEE_RATE AMT2
			FROM GM_BANK_FS_CARD_INFO A1, 
			 (SELECT C.MERCHANT_STNAME,B.FEE_RATE, B.CR_FEE_KIND, B.CARD_PN01, B.CARD_PN02, A.MERCHANT_NO
				FROM (SELECT *, CONVERT(CHAR(6), DATEADD(MONTH, CONVERT(int, A.SUM_MON_S),@p_start_date), 112) + A.SUM_DAY_S as sumDateStart, 
							   case when A.SUM_DAY_E = '99' 
									then CONVERT(CHAR(8), DATEADD(DAY,-1, CONVERT(CHAR(6), DATEADD(MONTH, CONVERT(int, A.SUM_MON_E) + 1 ,@p_start_date), 112) + '01'),112) 
									else CONVERT(CHAR(6), DATEADD(MONTH, CONVERT(int, A.SUM_MON_E),@p_start_date), 112) + A.SUM_DAY_E end as sumDateEnd
						   from CR_CONTRACT_M A
						  WHERE @p_start_date BETWEEN A.EFF_DATE_FROM AND A.EFF_DATE_TO) A,      
					  (SELECT * FROM CR_CONTRACT_D WHERE @p_start_date BETWEEN EFF_DATE_FROM AND EFF_DATE_TO) B,     
					  (SELECT MERCHANT_NO,MERCHANT_STNAME FROM GM_MERCHANT WHERE MERCHANT_TYPE='BANK' AND TM_FLG='Y') C,     
					  (SELECT * FROM CR_FEE_KIND_MST) D     
				WHERE A.MERCHANT_NO=B.MERCHANT_NO       
				  AND A.CR_FEE_KIND=B.CR_FEE_KIND       
				  AND A.MERCHANT_NO=C.MERCHANT_NO       
				  AND A.CR_FEE_KIND=D.CR_FEE_KIND
				  AND SUBSTRING(A.CR_FEE_KIND,1,10) = 'CARD_FIRST'
				  AND (A.MERCHANT_NO=@p_merchant_no or @p_merchant_no='')
				  )B1
		WHERE A1.CPT_DATE between @p_start_date and @p_end_date
          AND (A1.BANK_MERCHANT=@p_merchant_no or @p_merchant_no='')
          AND A1.BANK_MERCHANT=B1.MERCHANT_NO
		  AND A1.CR_FEE_KIND = B1.CR_FEE_KIND
		  AND A1.CARD_PN01 = (Case when B1.CARD_PN01 = '' then A1.CARD_PN01
								   else B1.CARD_PN01 end)
		  AND A1.CARD_PN02 = (Case when B1.CARD_PN02 = '' then A1.CARD_PN02
								   else B1.CARD_PN02 end)
		  AND A1.EN_1ST <> 'Y'
		GROUP BY LEFT(A1.E_TIME,8), B1.MERCHANT_STNAME,B1.FEE_RATE) A
	GROUP BY MERCHANT_STNAME,DATE1
union all	
SELECT '總計','',ROUND(cast(ISNULL(SUM(ISNULL(CNT1,0)),0) as int),0),
		ROUND(cast(ISNULL(SUM(ISNULL(ROUND(AMT1,0),0)),0) as int),0),
		ROUND(cast(ISNULL(SUM(ISNULL(ROUND(CNT2,0),0)),0) as int),0),
		ROUND(cast(ISNULL(SUM(ISNULL(ROUND(AMT2,0),0)),0) as int),0),
		ROUND(cast(ISNULL(SUM(ISNULL(ROUND(AMT1,0),0)),0)+ISNULL(SUM(ISNULL(ROUND(AMT2,0),0)),0)  as int),0)
	FROM
	(SELECT LEFT(E_TIME,8) DATE1,COUNT(*) CNT1,COUNT(*) * B1.FEE_RATE AMT1,0 CNT2,0 AMT2
		FROM GM_BANK_FS_CARD_INFO A1, 
			 (SELECT B.FEE_RATE, B.CR_FEE_KIND, B.CARD_PN01, B.CARD_PN02, A.MERCHANT_NO
				FROM (SELECT *, CONVERT(CHAR(6), DATEADD(MONTH, CONVERT(int, A.SUM_MON_S),@p_start_date), 112) + A.SUM_DAY_S as sumDateStart, 
							   case when A.SUM_DAY_E = '99' 
									then CONVERT(CHAR(8), DATEADD(DAY,-1, CONVERT(CHAR(6), DATEADD(MONTH, CONVERT(int, A.SUM_MON_E) + 1 ,@p_start_date), 112) + '01'),112) 
									else CONVERT(CHAR(6), DATEADD(MONTH, CONVERT(int, A.SUM_MON_E),@p_start_date), 112) + A.SUM_DAY_E end as sumDateEnd
						   from CR_CONTRACT_M A
						  WHERE @p_start_date BETWEEN A.EFF_DATE_FROM AND A.EFF_DATE_TO) A,      
					  (SELECT * FROM CR_CONTRACT_D WHERE @p_start_date BETWEEN EFF_DATE_FROM AND EFF_DATE_TO) B,     
					  (SELECT MERCHANT_NO,MERCHANT_STNAME FROM GM_MERCHANT WHERE MERCHANT_TYPE='BANK' AND TM_FLG='Y') C,     
					  (SELECT * FROM CR_FEE_KIND_MST) D     
				WHERE A.MERCHANT_NO=B.MERCHANT_NO       
				  AND A.CR_FEE_KIND=B.CR_FEE_KIND       
				  AND A.MERCHANT_NO=C.MERCHANT_NO       
				  AND A.CR_FEE_KIND=D.CR_FEE_KIND
				  AND SUBSTRING(A.CR_FEE_KIND,1,10) = 'CARD_FIRST'
				  AND (A.MERCHANT_NO=@p_merchant_no or @p_merchant_no='') )B1
		WHERE A1.CPT_DATE between @p_start_date and @p_end_date
          AND (A1.BANK_MERCHANT=@p_merchant_no or @p_merchant_no='')
          AND A1.BANK_MERCHANT=B1.MERCHANT_NO
		  AND A1.CR_FEE_KIND = B1.CR_FEE_KIND
		  AND A1.CARD_PN01 = (Case when B1.CARD_PN01 = '' then A1.CARD_PN01
								   else B1.CARD_PN01 end)
		  AND A1.CARD_PN02 = (Case when B1.CARD_PN02 = '' then A1.CARD_PN02
								   else B1.CARD_PN02 end)
		  AND A1.EN_1ST = 'Y'
		GROUP BY LEFT(A1.E_TIME,8), B1.FEE_RATE
	UNION ALL
		SELECT LEFT(E_TIME,8) DATE1,0 CNT1 ,0 AMT1,COUNT(*) CNT2,COUNT(*) * B1.FEE_RATE AMT2
			FROM GM_BANK_FS_CARD_INFO A1, 
			 (SELECT B.FEE_RATE, B.CR_FEE_KIND, B.CARD_PN01, B.CARD_PN02, A.MERCHANT_NO
				FROM (SELECT *, CONVERT(CHAR(6), DATEADD(MONTH, CONVERT(int, A.SUM_MON_S),@p_start_date), 112) + A.SUM_DAY_S as sumDateStart, 
							   case when A.SUM_DAY_E = '99' 
									then CONVERT(CHAR(8), DATEADD(DAY,-1, CONVERT(CHAR(6), DATEADD(MONTH, CONVERT(int, A.SUM_MON_E) + 1 ,@p_start_date), 112) + '01'),112) 
									else CONVERT(CHAR(6), DATEADD(MONTH, CONVERT(int, A.SUM_MON_E),@p_start_date), 112) + A.SUM_DAY_E end as sumDateEnd
						   from CR_CONTRACT_M A
						  WHERE @p_start_date BETWEEN A.EFF_DATE_FROM AND A.EFF_DATE_TO) A,      
					  (SELECT * FROM CR_CONTRACT_D WHERE @p_start_date BETWEEN EFF_DATE_FROM AND EFF_DATE_TO) B,     
					  (SELECT MERCHANT_NO,MERCHANT_STNAME FROM GM_MERCHANT WHERE MERCHANT_TYPE='BANK' AND TM_FLG='Y') C,     
					  (SELECT * FROM CR_FEE_KIND_MST) D     
				WHERE A.MERCHANT_NO=B.MERCHANT_NO       
				  AND A.CR_FEE_KIND=B.CR_FEE_KIND       
				  AND A.MERCHANT_NO=C.MERCHANT_NO       
				  AND A.CR_FEE_KIND=D.CR_FEE_KIND
				  AND SUBSTRING(A.CR_FEE_KIND,1,10) = 'CARD_FIRST'
				  AND (A.MERCHANT_NO=@p_merchant_no or @p_merchant_no='') )B1
		WHERE A1.CPT_DATE between @p_start_date and @p_end_date
          AND (A1.BANK_MERCHANT=@p_merchant_no or @p_merchant_no='')
          AND A1.BANK_MERCHANT=B1.MERCHANT_NO
		  AND A1.CR_FEE_KIND = B1.CR_FEE_KIND
		  AND A1.CARD_PN01 = (Case when B1.CARD_PN01 = '' then A1.CARD_PN01
								   else B1.CARD_PN01 end)
		  AND A1.CARD_PN02 = (Case when B1.CARD_PN02 = '' then A1.CARD_PN02
								   else B1.CARD_PN02 end)
		  AND A1.EN_1ST <> 'Y'
		GROUP BY LEFT(A1.E_TIME,8), B1.FEE_RATE) A
	ORDER BY MERCHANT_STNAME,DATE1
", MERC_GROUP);

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    sqlCmd.Parameters.Add("@p_start_date", SqlDbType.VarChar).Value = start + "01";
                    sqlCmd.Parameters.Add("@p_end_date", SqlDbType.VarChar).Value = end + "31";
                    sqlCmd.Parameters.Add("@p_merchant_no", SqlDbType.VarChar).Value = merchantNo;

                    SqlDataAdapter adapter = new SqlDataAdapter(sqlCmd);

                    adapter.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                log.Debug(ex.Message);
            }
            return dt;
        }

        public DataTable Report0321T(string start, string merchantNo, string MERC_GROUP)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();

                    string sqlText = String.Format(@"SELECT MERCHANT_STNAME '銀行',NOTIFY_DATES as '通知日期',REAL_CPT_DATE as '處理日期',N_AMT as '金額',N_CNT as '筆數',L_AMT as '　金額　',L_CNT as '　筆數　',N_AMT+L_AMT as T_AMT  
FROM
(SELECT A.MERCHANT_STNAME,CONVERT(varchar(8),	A.NOTIFY_DATE,112) NOTIFY_DATES,A.REAL_CPT_DATE,SUM(A.RETURN_AMT) N_AMT,COUNT(*) N_CNT,0 AS L_AMT,0 AS L_CNT
FROM GM_BANK_RA_DATA_REALBACK A,
	 GM_BANK_RA_DATA_D B
WHERE (A.BANK_MERCHANT=@p_merchant_no or @p_merchant_no='')
AND A.REAL_CPT_DATE=@p_cpt_date
AND A.BANK_MERCHANT=B.BANK_MERCHANT
AND A.ICC_NO=B.ICC_NO
AND B.STATUS<>'E'
AND B.RAMT_TYPE<>'03'
GROUP BY A.MERCHANT_STNAME,CONVERT(varchar(8),A.NOTIFY_DATE,112),A.REAL_CPT_DATE
UNION 
SELECT A.MERCHANT_STNAME,CONVERT(varchar(8),A.NOTIFY_DATE,112) NOTIFY_DATES,A.REAL_CPT_DATE,0 AS N_AMT,0 AS N_CNT,SUM(A.RETURN_AMT) AS L_AMT,COUNT(*) AS L_CNT
FROM GM_BANK_RA_DATA_REALBACK A,
     GM_BANK_RA_DATA_D B
WHERE (A.BANK_MERCHANT=@p_merchant_no or @p_merchant_no='')
AND A.REAL_CPT_DATE=@p_cpt_date
AND A.BANK_MERCHANT=B.BANK_MERCHANT
AND A.ICC_NO=B.ICC_NO
AND B.STATUS<>'E'
AND B.RAMT_TYPE='03'
GROUP BY A.MERCHANT_STNAME,CONVERT(varchar(8),A.NOTIFY_DATE,112),A.REAL_CPT_DATE) T
", MERC_GROUP);

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    sqlCmd.Parameters.Add("@p_cpt_date", SqlDbType.VarChar).Value = start;
                    sqlCmd.Parameters.Add("@p_merchant_no", SqlDbType.VarChar).Value = merchantNo;

                    SqlDataAdapter adapter = new SqlDataAdapter(sqlCmd);

                    adapter.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                log.Debug(ex.Message);
            }
            return dt;
        }


        public DataTable Report0322T(string start, string merchantNo, string MERC_GROUP)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();

                    string sqlText =
                            String.Format(@"SELECT '自動加值金' as '項目',ISNULL(SUM(ISNULL(T.SETTLE_AMT,0)),0) as '總金額','加項' as '說明'
FROM (
SELECT SETTLE_AMT
FROM [{0}].SU_BNK_TOTAL_D 
WHERE BNK_SETTLE_TYPE='AL'
AND BANK_MERCHANT=@p_merchant_no 
AND CPT_DATE=@p_cpt_date 
UNION ALL
SELECT SETTLE_AMT SETTLE_AMT
FROM [{0}].SU_BNK_TOTAL_D 
WHERE BNK_SETTLE_TYPE='BANK_RSP_SKIP'
AND BANK_MERCHANT=@p_merchant_no 
AND CPT_DATE=@p_cpt_date 
UNION ALL
SELECT SETTLE_AMT SETTLE_AMT
FROM [{0}].SU_BNK_TOTAL_D 
WHERE BNK_SETTLE_TYPE='ICSH_RSP'
AND BANK_MERCHANT=@p_merchant_no 
AND CPT_DATE=@p_cpt_date) T
UNION ALL
SELECT B.SETTLE_TYPE_DESC,SUM(ISNULL(A.SETTLE_AMT,0)),CASE SETTLE_SIGN WHEN 1 THEN '加項' ELSE '減項' END
FROM
	(SELECT * FROM [{0}].BM_SETTLE_TYPE 
	WHERE BNK_SETTLE_TYPE NOT IN ('AL','BANK_RSP_SKIP','ICSH_RSP','ICSH_RSP_SKIP','ICSH_DAILY_NET')) B 
	LEFT OUTER JOIN 
	(SELECT * FROM [{0}].SU_BNK_TOTAL_D  WHERE CPT_DATE=@p_cpt_date AND BANK_MERCHANT=@p_merchant_no) A ON 
	A.BNK_SETTLE_TYPE=B.BNK_SETTLE_TYPE
GROUP BY B.SETTLE_TYPE_DESC,SETTLE_SIGN 
UNION ALL
SELECT  B.SETTLE_TYPE_DESC,sum(ISNULL(A.SETTLE_AMT,0)),'總計'
FROM
	(SELECT * FROM [{0}].BM_SETTLE_TYPE 
	WHERE BNK_SETTLE_TYPE ='ICSH_DAILY_NET') B 
	LEFT OUTER JOIN 
	(SELECT * FROM [{0}].SU_BNK_TOTAL_D  WHERE CPT_DATE=@p_cpt_date AND BANK_MERCHANT=@p_merchant_no) A ON 
	A.BNK_SETTLE_TYPE=B.BNK_SETTLE_TYPE
	WHERE CPT_DATE=@p_cpt_date GROUP BY B.SETTLE_TYPE_DESC", MERC_GROUP);

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    sqlCmd.Parameters.Add("@p_cpt_date", SqlDbType.VarChar).Value = start;
                    sqlCmd.Parameters.Add("@p_merchant_no", SqlDbType.VarChar).Value = merchantNo;
                    sqlCmd.Parameters.Add("@p_schema", SqlDbType.VarChar).Value = MERC_GROUP;

                    SqlDataAdapter adapter = new SqlDataAdapter(sqlCmd);

                    adapter.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                log.Debug(ex.Message);
            }
            return dt;
        }

        public DataTable Report0323T(string start, string end, string merchantNo, string MERC_GROUP)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();

                    string sqlText =
                            String.Format(@"--宣告清分日的參數
declare @S_DATE varchar(8),@E_DATE varchar(8)
select @E_DATE = MAX(INFO_DATE),@S_DATE = MIN(INFO_DATE) 
from {0}.FS_FEESUM_M 
where FEE_DATE between @p_start_date and @p_end_date
AND BANK_MERCHANT=@p_merchant_no 
AND CR_FEE_KIND='AUTOLOAD_FEE'
and ISNULL(INFO_DATE,'') != ''

--宣告手續費費率(固定費率)
declare @FEE_RATE float
select @FEE_RATE = FEE_RATE
from CR_CONTRACT_D 
where MERCHANT_NO=@p_merchant_no 
AND CR_FEE_KIND='AUTOLOAD_FEE'
and CARD_PN02 = '0'
and EFF_DATE_FROM < @S_DATE
and EFF_DATE_TO > @E_DATE

--不含自動加值手續費
select Data.CR_FEE_KIND_NAME as '項目', sum(Data.sumAmt) as '總金額', Data.sumAcctype  as '說明'
from (
	select	B.CR_FEE_KIND_NAME,
			CASE B.CR_ACC_TYPE WHEN 'AR' then A.FEE_AMT else A.FEE_AMT * -1 end as sumAmt,
			CASE B.CR_ACC_TYPE WHEN 'AR' then '加項' else '減項' end as sumAcctype
		from {0}.[FS_FEESUM_M] A,CR_FEE_KIND_MST B
	   WHERE FEE_DATE between @p_start_date and @p_end_date
		 AND A.CR_FEE_KIND=B.CR_FEE_KIND
		 and a.CR_FEE_KIND <> 'AUTOLOAD_FEE'
	)Data
group by Data.CR_FEE_KIND_NAME, Data.sumAcctype

union all

--自動加值手續費
SELECT 
'自動加值手續費' as CR_FEE_KIND_NAME,
case
	when SUM(d.TPE_BUS_AMT+d.UT3_AMT-d.UT3_REJ_AMT) > SUM(d.LA_AMT-d.LA_REJ_AMT) --無手續費金額 > 現金加值金額
		then FLOOR(ROUND((SUM(d.AUTOLOAD_FEE_AMT) -SUM(d.TPE_BUS_AMT+d.UT3_AMT-d.UT3_REJ_AMT) + SUM(d.LA_AMT-d.LA_REJ_AMT)) * @FEE_RATE * -1,0))
	when SUM(d.TPE_BUS_AMT+d.UT3_AMT-d.UT3_REJ_AMT) <= SUM(d.LA_AMT-d.LA_REJ_AMT)  --無手續費金額 <= 現金加值金額
		then FLOOR(ROUND(SUM(d.AUTOLOAD_FEE_AMT) * @FEE_RATE * -1,0))
		else 0 end,
'減項' as sumAcctype
FROM (
	select a.INFO_DATE CPT_DATE, a.BANK_MERCHANT, a.CARD_PN02, COALESCE(SUM(ISNULL(CONVERT(NUMERIC,a.CPT_AMT),0)),0) as AUTOLOAD_FEE_AMT 
			 ,0 as TPE_BUS_AMT 
			 ,0 as UT3_AMT
			 ,0 as UT3_REJ_AMT 
			 ,0 as LA_AMT
			 ,0 as LA_REJ_AMT 
			 --,a.FEE_RATE FEE_RATE
	from {0}.FS_FEESUM_M a 
	WHERE a.BANK_MERCHANT=@p_merchant_no  
	AND a.FEE_DATE between @p_start_date and @p_end_date
	AND a.CR_FEE_KIND='AUTOLOAD_FEE'
	group by a.INFO_DATE, a.BANK_MERCHANT, a.CARD_PN02--,a.FEE_RATE
	union all
	select p1.CPT_DATE, p1.BANK_MERCHANT, p1.CARD_PN02, 0 as AUTOLOAD_FEE_AMT
			 , COALESCE(SUM(ISNULL(CONVERT(NUMERIC,p1.TPE_BUS),0)),0) as TPE_BUS_AMT
			 , COALESCE(SUM(ISNULL(CONVERT(NUMERIC,p1.UT3),0)),0)   as UT3_AMT 
			 , COALESCE(SUM(ISNULL(CONVERT(NUMERIC,p1.UT3_REJ),0)),0) as UT3_REJ_AMT 
			 , COALESCE(SUM(ISNULL(CONVERT(NUMERIC,p1.LA),0)),0)      as LA_AMT
			 , COALESCE(SUM(ISNULL(CONVERT(NUMERIC,p1.LA_REJ),0)),0)  as LA_REJ_AMT 
			 --,0.0 as FEE_RATE
	from {0}.AL_DISCOUNT_SUM_D b
	pivot(sum(SUM_VALUE) for D_TYPE
	in ([TPE_BUS],[UT3],[UT3_REJ],[LA],[LA_REJ])) as p1
	where 
	p1.CPT_DATE between @S_DATE and @E_DATE 
	group by p1.CPT_DATE, p1.BANK_MERCHANT, p1.CARD_PN02 
) d 

union all

--應撥付款項
select '應撥付款項' as CR_FEE_KIND_NAME, sum(FeeData.sumAmt) as sumAmt, '總計' assumAcctype
from (
		select Data.CR_FEE_KIND_NAME, sum(Data.sumAmt) as sumAmt, Data.sumAcctype
		from (
			select	B.CR_FEE_KIND_NAME,
					CASE B.CR_ACC_TYPE WHEN 'AR' then A.FEE_AMT else A.FEE_AMT * -1 end as sumAmt,
					CASE B.CR_ACC_TYPE WHEN 'AR' then '加項' else '減項' end as sumAcctype
				from {0}.[FS_FEESUM_M] A,CR_FEE_KIND_MST B
			   WHERE FEE_DATE  between @p_start_date and @p_end_date
				 AND A.CR_FEE_KIND=B.CR_FEE_KIND
				 and a.CR_FEE_KIND <> 'AUTOLOAD_FEE'
			)Data
		group by Data.CR_FEE_KIND_NAME, Data.sumAcctype

		union all

		--自動加值手續費
		SELECT 
		'自動加值手續費' as CR_FEE_KIND_NAME,
		case
			when SUM(d.TPE_BUS_AMT+d.UT3_AMT-d.UT3_REJ_AMT) > SUM(d.LA_AMT-d.LA_REJ_AMT) --無手續費金額 > 現金加值金額
				then FLOOR(ROUND((SUM(d.AUTOLOAD_FEE_AMT) -SUM(d.TPE_BUS_AMT+d.UT3_AMT-d.UT3_REJ_AMT) + SUM(d.LA_AMT-d.LA_REJ_AMT)) * @FEE_RATE * -1,0))
			when SUM(d.TPE_BUS_AMT+d.UT3_AMT-d.UT3_REJ_AMT) <= SUM(d.LA_AMT-d.LA_REJ_AMT)  --無手續費金額 <= 現金加值金額
				then FLOOR(ROUND( SUM(d.AUTOLOAD_FEE_AMT)  * @FEE_RATE * -1,0))
		else 0 end,
		'減項' as sumAcctype
		FROM  (
			select a.INFO_DATE CPT_DATE, a.BANK_MERCHANT, a.CARD_PN02, COALESCE(SUM(ISNULL(CONVERT(NUMERIC,a.CPT_AMT),0)),0) as AUTOLOAD_FEE_AMT 
					 ,0 as TPE_BUS_AMT 
					 ,0 as UT3_AMT
					 ,0 as UT3_REJ_AMT 
					 ,0 as LA_AMT
					 ,0 as LA_REJ_AMT 
					 --,a.FEE_RATE FEE_RATE
			from {0}.FS_FEESUM_M a 
			WHERE a.BANK_MERCHANT=@p_merchant_no  
			AND a.FEE_DATE between @p_start_date and @p_end_date
			AND a.CR_FEE_KIND='AUTOLOAD_FEE'
			group by a.INFO_DATE, a.BANK_MERCHANT, a.CARD_PN02--,a.FEE_RATE
			union all
			select p1.CPT_DATE, p1.BANK_MERCHANT, p1.CARD_PN02, 0 as AUTOLOAD_FEE_AMT
					 , COALESCE(SUM(ISNULL(CONVERT(NUMERIC,p1.TPE_BUS),0)),0) as TPE_BUS_AMT
					 , COALESCE(SUM(ISNULL(CONVERT(NUMERIC,p1.UT3),0)),0)   as UT3_AMT 
					 , COALESCE(SUM(ISNULL(CONVERT(NUMERIC,p1.UT3_REJ),0)),0) as UT3_REJ_AMT 
					 , COALESCE(SUM(ISNULL(CONVERT(NUMERIC,p1.LA),0)),0)      as LA_AMT
					 , COALESCE(SUM(ISNULL(CONVERT(NUMERIC,p1.LA_REJ),0)),0)  as LA_REJ_AMT 
					 --,0.0 as FEE_RATE
			from {0}.AL_DISCOUNT_SUM_D b
			pivot(sum(SUM_VALUE) for D_TYPE
			in ([TPE_BUS],[UT3],[UT3_REJ],[LA],[LA_REJ])) as p1
			where 
			p1.CPT_DATE between @S_DATE and @E_DATE 
			group by p1.CPT_DATE, p1.BANK_MERCHANT, p1.CARD_PN02 
		) d 
	)FeeData", MERC_GROUP);

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    sqlCmd.Parameters.Add("@p_start_date", SqlDbType.VarChar).Value = start;
                    sqlCmd.Parameters.Add("@p_end_date", SqlDbType.VarChar).Value = end + "31";
                    sqlCmd.Parameters.Add("@p_merchant_no", SqlDbType.VarChar).Value = merchantNo;
                    sqlCmd.Parameters.Add("@p_schema", SqlDbType.VarChar).Value = MERC_GROUP;

                    SqlDataAdapter adapter = new SqlDataAdapter(sqlCmd);

                    adapter.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                log.Debug(ex.Message);
            }
            return dt;
        }


        public DataTable Report0324T(string start, string end, string merchantNo, string MERC_GROUP)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();

                    string sqlText =
                            String.Format(@"--0324

--發卡暨補換續卡統計月報表
SELECT 
MERCHANT_STNAME '銀行',
INFO_DATE '資料日期',				--清分日
SUM(CNT_1) '一般新發卡數',SUM(AMT_1) '一般新發卡金額',	--新發卡數(一般)
SUM(CNT_2) '第三方新發卡數',SUM(AMT_2) '第三方新發卡金額',	--新發卡數(三方聯名)
SUM(CNT_3) '換補卡數',SUM(AMT_3) '換補卡服務費',  --補換卡
SUM(CNT_4) '續卡數',SUM(AMT_4) '續卡服務費',  --續卡
SUM(CNT_5) 'NFC新辦卡數',SUM(AMT_5) 'NFC新辦卡數金額',  --NFC新辦卡
SUM(AMT_1)+SUM(AMT_2)'總金額',
SUM(AMT_3)+SUM(AMT_4)'總服務費',
SUM(AMT_5) 'NFC新辦卡'
FROM
(
	--新發卡 CARD_PREMIUM&一般
	SELECT MERCHANT_STNAME,INFO_DATE,SUM(CPT_CNT) CNT_1,SUM(FEE_AMT) AMT_1,0 CNT_2,0 AMT_2,0 CNT_3,0 AMT_3,0 CNT_4,0 AMT_4,0 CNT_5,0 AMT_5
	FROM {0}.FS_FEESUM_M A
	INNER JOIN GM_MERCHANT B ON B.MERCHANT_NO=A.BANK_MERCHANT
	WHERE (BANK_MERCHANT=@p_merchant_no or @p_merchant_no='')
	AND FEE_DATE between @p_start_date and @p_end_date
	AND CR_FEE_KIND='CARD_PREMIUM'
	AND CARD_PN02 in ('0','')
	GROUP BY MERCHANT_STNAME,INFO_DATE
	UNION ALL
	--新發卡 CARD_PREMIUM &第三方
	SELECT MERCHANT_STNAME,INFO_DATE,0 CNT_1,0 AMT_1,SUM(CPT_CNT) CNT_2,SUM(FEE_AMT) AMT_2,0 CNT_3,0 AMT_3,0 CNT_4,0 AMT_4,0 CNT_5,0 AMT_5
	FROM {0}.FS_FEESUM_M A
	INNER JOIN GM_MERCHANT B ON B.MERCHANT_NO=A.BANK_MERCHANT
	WHERE (BANK_MERCHANT=@p_merchant_no or @p_merchant_no='')
	AND FEE_DATE between @p_start_date and @p_end_date
	AND CR_FEE_KIND='CARD_PREMIUM'
	AND CARD_PN02='1'
	GROUP BY MERCHANT_STNAME,INFO_DATE
	UNION ALL
	--換補卡服務費 CARD_CNT_B,CARD_CNT_C
	SELECT MERCHANT_STNAME,INFO_DATE,0 CNT_1,0 AMT_1,0 CNT_2,0 AMT_2,SUM(CPT_CNT) CNT_3,SUM(FEE_AMT) AMT_3,0 CNT_4,0 AMT_4,0 CNT_5,0 AMT_5
	FROM {0}.FS_FEESUM_M A
	INNER JOIN GM_MERCHANT B ON B.MERCHANT_NO=A.BANK_MERCHANT
	WHERE (BANK_MERCHANT=@p_merchant_no or @p_merchant_no='')
	AND FEE_DATE between @p_start_date and @p_end_date
	AND CR_FEE_KIND IN ('CARD_CNT_B','CARD_CNT_C')
	GROUP BY MERCHANT_STNAME,INFO_DATE
	UNION ALL
	--續卡服務費 CARD_CNT_D
	SELECT MERCHANT_STNAME,INFO_DATE,0 CNT_1,0 AMT_1,0 CNT_2,0 AMT_2,0 CNT_3,0 AMT_3,SUM(CPT_CNT) CNT_4,SUM(FEE_AMT) AMT_4,0 CNT_5,0 AMT_5
	FROM {0}.FS_FEESUM_M A
	INNER JOIN GM_MERCHANT B ON B.MERCHANT_NO=A.BANK_MERCHANT
	WHERE (BANK_MERCHANT=@p_merchant_no or @p_merchant_no='')
	AND FEE_DATE between @p_start_date and @p_end_date
	AND CR_FEE_KIND IN ('CARD_CNT_D')
	GROUP BY MERCHANT_STNAME,INFO_DATE
	union all
	--NFC新辦卡 CARD_CNT_G		--20170523 文鴻新增
	SELECT MERCHANT_STNAME,INFO_DATE,0 CNT_1,0 AMT_1,0 CNT_2,0 AMT_2,0 CNT_3,0 AMT_3,0 CNT_4,0 AMT_4,SUM(CPT_CNT) CNT_5,SUM(FEE_AMT) AMT_5
	FROM {0}.FS_FEESUM_M A
	INNER JOIN GM_MERCHANT B ON B.MERCHANT_NO=A.BANK_MERCHANT
	WHERE (BANK_MERCHANT=@p_merchant_no or @p_merchant_no='')
	AND FEE_DATE between @p_start_date and @p_end_date
	AND CR_FEE_KIND IN ('CARD_CNT_G')
	GROUP BY MERCHANT_STNAME,INFO_DATE
) a
GROUP BY MERCHANT_STNAME,INFO_DATE
union all
SELECT 
'總計','',
SUM(CNT_1) '一般新發卡數',SUM(AMT_1) '一般新發卡金額',	--新發卡數(一般)
SUM(CNT_2) '第三方新發卡數',SUM(AMT_2) '第三方新發卡金額',	--新發卡數(三方聯名)
SUM(CNT_3) '換補卡數',SUM(AMT_3) '換補卡服務費',  --補換卡
SUM(CNT_4) '續卡數',SUM(AMT_4) '續卡服務費',  --續卡
SUM(CNT_5) 'NFC新辦卡數',SUM(AMT_5) 'NFC新辦卡數金額',  --NFC新辦卡
SUM(AMT_1)+SUM(AMT_2)'總金額',
SUM(AMT_3)+SUM(AMT_4)'總服務費',
SUM(AMT_5) 'NFC新辦卡'
FROM
(
	--新發卡 CARD_PREMIUM&一般
	SELECT SUM(CPT_CNT) CNT_1,SUM(FEE_AMT) AMT_1,0 CNT_2,0 AMT_2,0 CNT_3,0 AMT_3,0 CNT_4,0 AMT_4,0 CNT_5,0 AMT_5
	FROM {0}.FS_FEESUM_M
	WHERE (BANK_MERCHANT=@p_merchant_no or @p_merchant_no='')
	AND FEE_DATE between @p_start_date and @p_end_date
	AND CR_FEE_KIND='CARD_PREMIUM'
	AND CARD_PN02 in ('0','')
	UNION ALL
	--新發卡 CARD_PREMIUM &第三方
	SELECT 0 CNT_1,0 AMT_1,SUM(CPT_CNT) CNT_2,SUM(FEE_AMT) AMT_2,0 CNT_3,0 AMT_3,0 CNT_4,0 AMT_4,0 CNT_5,0 AMT_5
	FROM {0}.FS_FEESUM_M
	WHERE (BANK_MERCHANT=@p_merchant_no or @p_merchant_no='')
	AND FEE_DATE between @p_start_date and @p_end_date
	AND CR_FEE_KIND='CARD_PREMIUM'
	AND CARD_PN02='1'
	UNION ALL
	--換補卡服務費 CARD_CNT_B,CARD_CNT_C
	SELECT 0 CNT_1,0 AMT_1,0 CNT_2,0 AMT_2,SUM(CPT_CNT) CNT_3,SUM(FEE_AMT) AMT_3,0 CNT_4,0 AMT_4,0 CNT_5,0 AMT_5
	FROM {0}.FS_FEESUM_M
	WHERE (BANK_MERCHANT=@p_merchant_no or @p_merchant_no='')
	AND FEE_DATE between @p_start_date and @p_end_date
	AND CR_FEE_KIND IN ('CARD_CNT_B','CARD_CNT_C')
	UNION ALL
	--續卡服務費 CARD_CNT_D
	SELECT 0 CNT_1,0 AMT_1,0 CNT_2,0 AMT_2,0 CNT_3,0 AMT_3,SUM(CPT_CNT) CNT_4,SUM(FEE_AMT) AMT_4,0 CNT_5,0 AMT_5
	FROM {0}.FS_FEESUM_M
	WHERE (BANK_MERCHANT=@p_merchant_no or @p_merchant_no='')
	AND FEE_DATE between @p_start_date and @p_end_date
	AND CR_FEE_KIND IN ('CARD_CNT_D')
	union all
	--NFC新辦卡 CARD_CNT_G		--20170523 文鴻新增
	SELECT 0 CNT_1,0 AMT_1,0 CNT_2,0 AMT_2,0 CNT_3,0 AMT_3,0 CNT_4,0 AMT_4,SUM(CPT_CNT) CNT_5,SUM(FEE_AMT) AMT_5
	FROM {0}.FS_FEESUM_M
	WHERE (BANK_MERCHANT=@p_merchant_no or @p_merchant_no='')
	AND FEE_DATE between @p_start_date and @p_end_date
	AND CR_FEE_KIND IN ('CARD_CNT_G')
) a", MERC_GROUP);

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    sqlCmd.Parameters.Add("@p_start_date", SqlDbType.VarChar).Value = start + "01";
                    sqlCmd.Parameters.Add("@p_end_date", SqlDbType.VarChar).Value = end + "31";
                    sqlCmd.Parameters.Add("@p_merchant_no", SqlDbType.VarChar).Value = merchantNo;
                    sqlCmd.Parameters.Add("@p_schema", SqlDbType.VarChar).Value = MERC_GROUP;

                    SqlDataAdapter adapter = new SqlDataAdapter(sqlCmd);

                    adapter.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                log.Debug(ex.Message);
            }
            return dt;
        }

        public DataTable Report0325T(string start, string end, string merchantNo, string merchantBankNo, string MERC_GROUP, string PRT_TYPE)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();

                    string sqlText =
                            String.Format(@"select F_DATE '原清分日期',A.CPT_DATE '清分日期',C.DIFF_DESC '剔退原因',B.MERCHANT_STNAME '銀行',A.ICC_NO '卡號',G.MERCHANT_NAME '特約機構',A.STORE_NO '店號',A.REG_ID '機號',A.TRANS_TYPE '交易代號',TRANS_DATE_TXLOG '交易時間',cast(A.TRANS_AMT as int) '交易金額',BANK_FLG_DESC '銀行回覆',L.RCPT_DATE '再提示日'
from GM_BANK_RT_SKIP_A07B_RSP A
left outer join CR_DIFF_M C on A.DIFF_FLG=C.DIFF_FLG
left outer join GM_MERCHANT G on right(A.MERCHANT_NO,8)=G.MERCHANT_NO 
left outer join GM_MERCHANT B on right(A.BANK_MERCHANT,8)=B.MERCHANT_NO 
inner join GM_BANK_AL_SETTLE_D_VIEW L on A.ICC_NO = L.ICC_NO and A.TRANS_DATE_TXLOG = L.TRANS_DATE and A.TRANS_TYPE = L.TRANS_TYPE and A.TRANS_AMT = L.TRANS_AMT --建議利用AL_SETTLE_DATA_D的資訊
where A.CPT_DATE between @p_start_date and @p_end_date 
and (right(A.MERCHANT_NO,8)=@p_merchant_no or @p_merchant_no='' )
", MERC_GROUP);

                    if (merchantBankNo != "")
                    {
                        sqlText += "and (A.BANK_MERCHANT=@p_merchantBank_no) ";
                    }

                    
                    if (PRT_TYPE =="21")
                    {
                        sqlText += "and A.DIFF_FLG<>'S001' ";
                    }
                    else
                    {
                        sqlText += "and A.DIFF_FLG='S001' ";
                    }
                    if (MERC_GROUP != "")
                    {
                        //sqlText += "and A.SCHEMAID= @p_schema ";
                    }
                    sqlText += "order by A.BANK_MERCHANT ";
                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    sqlCmd.Parameters.Add("@p_start_date", SqlDbType.VarChar).Value = start ;
                    sqlCmd.Parameters.Add("@p_end_date", SqlDbType.VarChar).Value = end;
                    sqlCmd.Parameters.Add("@p_merchant_no", SqlDbType.VarChar).Value = merchantNo;
                    sqlCmd.Parameters.Add("@p_merchantBank_no", SqlDbType.VarChar).Value = merchantBankNo;
                    sqlCmd.Parameters.Add("@p_schema", SqlDbType.VarChar).Value = MERC_GROUP;

                    SqlDataAdapter adapter = new SqlDataAdapter(sqlCmd);

                    adapter.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                log.Debug(ex.Message);
            }
            return dt;
        }

        public DataTable Report0326T(string start, string end, string merchantNo, string ICC_NO, string RAMT_TYPE)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();

                    string sqlText =
                            @"SELECT CPT_DATE 清分日,B.MERCHANT_STNAME 銀行別,
A.ICC_NO 卡號,A.RAMT_TYPE+':'+C.RAMT_NAME 餘返原因,
D.REAL_CPT_DATE 實際餘返日,isnull(D.CARD_AMT,0) 卡務金額,isnull(D.FEE_AMT,0) 手續費,isnull(D.RETURN_AMT,0) 餘返金額,
A.NOTIFY_DATE 通知日,RAMT_FROM_TYPE 通知來源,RAMT_FROM_RESOURCE 通知檔
FROM CR_RA_DATA_D A LEFT OUTER JOIN CR_RA_DATA_REALBACK D ON A.ICC_NO=D.ICC_NO,
GM_MERCHANT B,CR_RAMT_TYPE_MST C
WHERE A.BANK_MERCHANT=B.MERCHANT_NO
AND A.RAMT_TYPE=C.RAMT_TYPE
and CPT_DATE between @p_start_date and @p_end_date
and (B.MERCHANT_NO=@p_merchant_no or @p_merchant_no='')
and A.ICC_NO like @p_ICC_NO+'%'
and (C.RAMT_TYPE=@p_RAMT_TYPE or @p_RAMT_TYPE='') 
ORDER BY CPT_DATE,B.MERCHANT_STNAME,A.RAMT_TYPE,D.REAL_CPT_DATE";

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    sqlCmd.Parameters.Add("@p_start_date", SqlDbType.VarChar).Value = start;
                    sqlCmd.Parameters.Add("@p_end_date", SqlDbType.VarChar).Value = end;
                    sqlCmd.Parameters.Add("@p_merchant_no", SqlDbType.VarChar).Value = merchantNo;
                    sqlCmd.Parameters.Add("@p_ICC_NO", SqlDbType.VarChar).Value = ICC_NO;
                    sqlCmd.Parameters.Add("@p_RAMT_TYPE", SqlDbType.VarChar).Value = RAMT_TYPE;

                    SqlDataAdapter adapter = new SqlDataAdapter(sqlCmd);

                    adapter.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                log.Debug(ex.Message);
            }
            return dt;
        }

        public DataTable Report0327T(string start)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();

                    string sqlText =
                            @"select E.ICC_NO 卡號,M.MERCHANT_STNAME 發卡銀行,
sum(cast((case when E.TRANS_TYPE in ('22','23','52','53') then E.TRANS_AMT else '0' end) as float)) 現金加值及退貨,
sum(cast((case when E.TRANS_TYPE in ('24','54') then E.TRANS_AMT else '0' end) as float)) 加值取消,
sum(cast((case when E.TRANS_TYPE in ('74','75','77') then E.TRANS_AMT else '0' end) as float)) 自動加值, --(add by 20160415 rita 新增 77離線自動加值)
sum(cast((case when E.TRANS_TYPE in ('21','51') then E.TRANS_AMT else '0' end) as float)) 消費 from CR_CARD_EXCEPT_D E
left outer join GM_TRANS_TYPE T on E.TRANS_TYPE=T.TRANS_TYPE
left outer join GM_MERCHANT M on E.BANK_MERCHANT=M.MERCHANT_NO
where E.CPT_DATE=@p_start_date
group by E.ICC_NO,M.MERCHANT_STNAME order by 2,1";

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    sqlCmd.Parameters.Add("@p_start_date", SqlDbType.VarChar).Value = start;

                    SqlDataAdapter adapter = new SqlDataAdapter(sqlCmd);

                    adapter.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                log.Debug(ex.Message);
            }
            return dt;
        }

        public DataTable Report0328T(string start, string end, string merchantNo, string merchantBankNo)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();
                    string sqlText =
                            @"SELECT CPT_DATE '清分日',REMITTANCE_DATE '預計匯款日',D.MERCHANT_STNAME '銀行名稱',E.MERCHANT_STNAME '特約機構',STORE_NO '門市代號',REG_ID '收銀機代號',SEQ_NO '交易序號',TRANS_DATE '交易日期',C.TRANS_DESC '交易代號',ICC_NO '卡號',TRANS_AMT '交易金額',B.DIFF_DESC '認列原因',STAN '授權STAN',RRN '授權RRN'			
FROM  GM_BANK_F103_VIEW A,CR_DIFF_M B,GM_TRANS_TYPE C,GM_MERCHANT D, GM_MERCHANT E			
WHERE A.DIFF_FLG=B.DIFF_FLG			
AND A.TRANS_TYPE=C.TRANS_TYPE			
AND A.BANK_MERCHANT=D.MERCHANT_NO			
AND A.MERCHANT_NO=E.MERCHANT_NO	
and A.CPT_DATE between @p_start_date and @p_end_date 
and (right(A.MERCHANT_NO,8)=@p_merchant_no or @p_merchant_no='' )
and (right(A.BANK_MERCHANT,8)=@p_merchantBank_no or @p_merchantBank_no='' )";
//union all 
//SELECT '總計','','','','','','','','','',isnull(sum(TRANS_AMT),0),'','',''			
//FROM  GM_BANK_F103_VIEW A
//where A.CPT_DATE between @p_start_date and @p_end_date 
//and (right(A.MERCHANT_NO,8)=@p_merchant_no or @p_merchant_no='' )
//and (right(A.BANK_MERCHANT,8)=@p_merchantBank_no or @p_merchantBank_no='' )
//";

                    if (merchantBankNo != "")
                    {
                        sqlText += "and (A.BANK_MERCHANT=@p_merchantBank_no) ";
                    }


                    sqlText += "order by A.CPT_DATE ";
                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    sqlCmd.CommandTimeout = 300;
                    sqlCmd.Parameters.Add("@p_start_date", SqlDbType.VarChar).Value = start;
                    sqlCmd.Parameters.Add("@p_end_date", SqlDbType.VarChar).Value = end;
                    sqlCmd.Parameters.Add("@p_merchant_no", SqlDbType.VarChar).Value = merchantNo;
                    sqlCmd.Parameters.Add("@p_merchantBank_no", SqlDbType.VarChar).Value = merchantBankNo;

                    SqlDataAdapter adapter = new SqlDataAdapter(sqlCmd);

                    adapter.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                log.Debug(ex.Message);
            }
            return dt;
        }


        public DataTable Report0329T(string start, string end, string merchantBankNo)
        {
            string sqlText = "";
            string sqltemp = "";
            string SCHEMAID = "";
            string BANK_CODE = "";
            DataTable dtd = ReportBankMerchant(merchantBankNo);
            if (dtd.Rows.Count > 0)
            {
                for (int j = 0; j < dtd.Rows.Count; j++)
                {
                    SCHEMAID = dtd.Rows[j][0].ToString();
                    BANK_CODE = dtd.Rows[j][0].ToString().Substring(3, 3);
                    sqltemp =
                                                @"select CPT_DATE '清分日',  CM_BANK_D.BANK_NAME '銀行簡稱',";
                    sqltemp = sqltemp + SCHEMAID + @".AL_SETTLE_DATA_D.MERCHANT_NO '特店代號', TRANS_DATE '中心端交易時間', 
TSAM_OSN,TSAM_TSN,
TRANS_DATE_TXLOG '卡機交易時間', ICC_NO '卡號', TRANS_AMT '交易金額', DIFF_FLG '處理方式', STAN , RRN,
BANK_RDATE '剔退日', BANK_FLG '剔退代碼', RCPT_DATE '再提示時間', RREMIT_DATE '再提示預計匯款日'
from " + SCHEMAID+ @".AL_SETTLE_DATA_D
left outer join CM_BANK_D on substring("+SCHEMAID+ @".AL_SETTLE_DATA_D.ICC_NO,1,2)=CM_BANK_D.CA_DPT
where (BANK_RDATE = @p_start_date or RCPT_DATE=@p_start_date )
and CM_BANK_D.BANK_CODE='" + BANK_CODE + "' and ((BANK_FLG <>'000' and BANK_FLG is not null and BANK_FLG<>'') or (RCPT_DATE is not null and RCPT_DATE<>'')) ";
                    if (merchantBankNo != "ALL")
                    {
                        sqltemp += "and (BANK_MERCHANT=@p_merchantBank_no) ";
                    }


                    if (sqlText == "")
                        sqlText = sqltemp;
                    else
                        sqlText = sqlText + " union " + sqltemp;

                    sqltemp =
                                                @"select '' '清分日',  '' '銀行簡稱',";
                    sqltemp = sqltemp + @"'' '特店代號', '' '中心端交易時間', 
'' TSAM_OSN,'' TSAM_TSN,
'' '卡機交易時間', BANK_RDATE+CM_BANK_D.BANK_NAME+'再提示總計' '卡號', sum(TRANS_AMT) '交易金額', '' '處理方式', '' STAN ,'' RRN,
'' '剔退日', '' '剔退代碼', '' '再提示時間', '' '再提示預計匯款日'
from " + SCHEMAID + @".AL_SETTLE_DATA_D
left outer join CM_BANK_D on substring(" + SCHEMAID + @".AL_SETTLE_DATA_D.ICC_NO,1,2)=CM_BANK_D.CA_DPT
where (BANK_RDATE = @p_start_date or RCPT_DATE=@p_start_date )
and CM_BANK_D.BANK_CODE='" + BANK_CODE + "' and ( (RCPT_DATE is not null and RCPT_DATE<>'')) ";
                    if (merchantBankNo != "ALL")
                    {
                        sqltemp += "and (BANK_MERCHANT=@p_merchantBank_no ) ";
                    }


                    if (sqlText == "")
                        sqlText = sqltemp + " group by BANK_RDATE,CM_BANK_D.BANK_NAME ";
                    else
                        sqlText = sqlText + " union " + sqltemp + " group by BANK_RDATE,CM_BANK_D.BANK_NAME ";

                    sqltemp =
                                                @"select '' '清分日',  '' '銀行簡稱',";
                    sqltemp = sqltemp + @"'' '特店代號', '' '中心端交易時間', 
'' TSAM_OSN,'' TSAM_TSN,
'' '卡機交易時間', BANK_RDATE+CM_BANK_D.BANK_NAME+'剔退總計' '卡號', sum(TRANS_AMT) '交易金額', '' '處理方式', '' STAN ,'' RRN,
'' '剔退日', '' '剔退代碼', '' '再提示時間', '' '再提示預計匯款日'
from " + SCHEMAID + @".AL_SETTLE_DATA_D
left outer join CM_BANK_D on substring(" + SCHEMAID + @".AL_SETTLE_DATA_D.ICC_NO,1,2)=CM_BANK_D.CA_DPT
where (BANK_RDATE = @p_start_date or RCPT_DATE=@p_start_date )
and CM_BANK_D.BANK_CODE='" + BANK_CODE + "' and ((BANK_FLG <>'000' and BANK_FLG is not null and BANK_FLG<>'') or (RCPT_DATE is not null and RCPT_DATE<>'')) ";
                    if (merchantBankNo != "ALL")
                    {
                        sqltemp += "and (BANK_MERCHANT=@p_merchantBank_no) ";
                    }


                    if (sqlText == "")
                        sqlText = sqltemp + " group by BANK_RDATE,CM_BANK_D.BANK_NAME ";
                    else
                        sqlText = sqlText + " union " + sqltemp + " group by BANK_RDATE,CM_BANK_D.BANK_NAME ";

                }
            }
            sqlText = sqlText + " order by CM_BANK_D.BANK_NAME desc, TRANS_DATE_TXLOG ";

            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();


                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    sqlCmd.Parameters.Add("@p_start_date", SqlDbType.VarChar).Value = start;
                    sqlCmd.Parameters.Add("@p_end_date", SqlDbType.VarChar).Value = end;
                    sqlCmd.Parameters.Add("@p_merchantBank_no", SqlDbType.VarChar).Value = merchantBankNo;

                    SqlDataAdapter adapter = new SqlDataAdapter(sqlCmd);

                    adapter.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                log.Debug(ex.Message);
            }
            return dt;
        }

        public DataTable Report0411T(string start, string end, string merchantNo, string CARD_TYPE)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();
//                    string sqlText =
//                            @"select g.cname as '資料名稱',sum(isnull(gm.SUM_DATA,0)) as '總額', max(unit) as '單位' 
//from (select * from GM_FSC_SUM_D where CPT_DATE between @Sdate and @Edate
//and CARD_TYPE=@CARD_TYPE
//and (MERCHANT_NO=@MERCHANT_NO or @MERCHANT_NO='')) gm 
//right outer join
//(select 1 as seq,'CREATE_CARD_CNT_TOTAL' as code,'發卡總數' as cname, '' as unit
//union
//select 2 as seq,'USING_CARD_CNT' as code,'使用之卡數' as cname, '' as unit
//union
//select 3 as seq,'CREATE_CARD_CNT' as code,'當月發卡數' as cname, '' as unit
//union
//select 4 as seq,'CHARGES_AMT' as code,'當月儲(加)值金額' as cname, '元' as unit
//union
//select 5 as seq,'CHARGES_CARD_CNT' as code,'當月儲(加)值卡數' as cname, '' as unit
//union
//select 6 as seq,'CHARGES_CNT' as code,'當月儲(加)值次數' as cname, '' as unit
//union
//select 7 as seq,'SALES_AMT' as code,'當月購貨金額' as cname, '元' as unit
//union
//select 8 as seq,'SALES_CARD_CNT' as code,'當月消費卡數' as cname, '' as unit
//union
//select 9 as seq,'SALES_CNT' as code,'當月購貨次數' as cname, '' as unit
//union
//select 10 as seq,'STOP_CARD_CNT' as code,'當月停卡數' as cname, '' as unit
//union
//select 11 as seq,'BACK_AMT' as code,'當月贖回金額' as cname, '元' as unit
//union
//select 12 as seq,'CHARGES_AMT_TOTAL' as code,'儲值總餘額' as cname, '元' as unit
//union
//select 13 as seq,'FAKE_CARD_CNT' as code,'當月偽冒停卡張數' as cname, '' as unit
//union
//select 14 as seq,'FAKE_CARD_CNT_TOTAL' as code,'當年度累計偽冒停卡張數' as cname, '' as unit
//union
//select 15 as seq,'FAKE_CARD_AMT' as code,'當月偽冒損失金額' as cname, '元' as unit
//union
//select 16 as seq,'FAKE_CARD_AMT_TOTAL' as code,'當年度累計偽冒損失金額' as cname, '元' as unit)  g
//on gm.SETTLE_TYPE=g.code
//group by g.cname,g.seq
//order by g.seq;";
                    string sqlText =
        @"select D.TYPE_DESC as '資料名稱', isnull(D.sumData,0) as '總額', T2.TYPE_UNIT as '單位' 
	from (
			select T.SETTLE_TYPE, T.TYPE_DESC, sum(isnull(S.SUM_DATA,0)) as sumData
			from GM_FSC_SUM_D S
			right join GM_FSC_TYPE_M T on S.SETTLE_TYPE = T.SETTLE_TYPE
											and S.SETTLE_DATE between @Sdate and @Edate
											and S.CARD_TYPE = @CARD_TYPE
                                            and (S.MERCHANT_NO=@MERCHANT_NO or @MERCHANT_NO='')
			where T.ISUSE = 'Y'
			  and T.ISSHOW = 'Y'
			  and T.REP_GROUP = 'Publish_REP'
			GROUP by T.TYPE_DESC, T.SETTLE_TYPE
	)D
	inner join GM_FSC_TYPE_M T2 on D.SETTLE_TYPE = T2.SETTLE_TYPE
							   and T2.ISUSE = 'Y'
							   and T2.ISSHOW = 'Y'
							   and T2.REP_GROUP = 'Publish_REP' 
order by T2.TYPE_ORDER;";

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    sqlCmd.Parameters.Add("@Sdate", SqlDbType.VarChar).Value = start + "01";
                    sqlCmd.Parameters.Add("@Edate", SqlDbType.VarChar).Value = end + "31";
                    sqlCmd.Parameters.Add("@MERCHANT_NO", SqlDbType.VarChar).Value = merchantNo;
                    sqlCmd.Parameters.Add("@CARD_TYPE", SqlDbType.VarChar).Value = CARD_TYPE;

                    SqlDataAdapter adapter = new SqlDataAdapter(sqlCmd);

                    adapter.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                log.Debug(ex.Message);
            }
            return dt;
        }

        public DataTable Report0412T(string start, string end, string merchantNo)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();
//                    string sqlText =
//                            @"select g.cname as '資料名稱',sum(isnull(gm.SUM_DATA,0)) as '總額', max(unit) as '單位' 
//from (select * from GM_FSC_SUM_D where CPT_DATE between @Sdate and @Edate
//and (MERCHANT_NO=@MERCHANT_NO or @MERCHANT_NO='')) gm 
//right outer join
//(select 1 as seq,'ST1_SMALL_AMT' as code,'小額交易' as cname, '元' as unit
//union
//select 2 as seq,'ST1_TRANS_ANT' as code,'不限金額交易' as cname, '元' as unit
//union
//select 3 as seq,'ST2_SMALL_AMT' as code,'小額交易' as cname, '元' as unit
//union
//select 4 as seq,'ST2_TRANS_ANT' as code,'不限金額交易' as cname, '元' as unit
//union
//select 5 as seq,'FAKE_CARD_AMT' as code,'偽冒交易之金額' as cname, '元' as unit
//union
//select 6 as seq,'FAKE_DISPUTE_AMT' as code,'爭議帳號扣款之金額' as cname, '元' as unit
//union
//select 7 as seq,'FAKE_READER_AMT' as code,'端末設備異常交易之金額' as cname, '元' as unit
//union
//select 8 as seq,'FAKE_AMT_TOTAL' as code,'異常交易金額合計' as cname, '元' as unit)  g
//on gm.SETTLE_TYPE=g.code
//group by g.cname,g.seq
//order by g.seq;";
                    string sqlText =
                                    @"select D.TYPE_DESC as '資料名稱', isnull(D.sumData,0) as '總額', T2.TYPE_UNIT as '單位' 
	from (
			select T.SETTLE_TYPE, T.TYPE_DESC, sum(isnull(S.SUM_DATA,0)) as sumData
			from GM_FSC_SUM_D S
			right join GM_FSC_TYPE_M T on S.SETTLE_TYPE = T.SETTLE_TYPE
											and S.SETTLE_DATE between @Sdate and @Edate
                                            and (S.MERCHANT_NO=@MERCHANT_NO or @MERCHANT_NO='')
			where T.ISUSE = 'Y'
			  and T.ISSHOW = 'Y'
			  and T.REP_GROUP = 'WorkStrength_REP'
			GROUP by T.TYPE_DESC, T.SETTLE_TYPE
	)D
	inner join GM_FSC_TYPE_M T2 on D.SETTLE_TYPE = T2.SETTLE_TYPE
							   and T2.ISUSE = 'Y'
							   and T2.ISSHOW = 'Y'
							   and T2.REP_GROUP = 'WorkStrength_REP'
order by T2.TYPE_ORDER;";

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    sqlCmd.Parameters.Add("@Sdate", SqlDbType.VarChar).Value = start + "01";
                    sqlCmd.Parameters.Add("@Edate", SqlDbType.VarChar).Value = end + "31";
                    sqlCmd.Parameters.Add("@MERCHANT_NO", SqlDbType.VarChar).Value = merchantNo;

                    SqlDataAdapter adapter = new SqlDataAdapter(sqlCmd);

                    adapter.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                log.Debug(ex.Message);
            }
            return dt;
        }

        public DataTable Report151201T(string start, string end, string src)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();
                    string sqlText = "";

                    if (src == "AMUSEMENT_PARK")
                    {
                        sqlText = @"select STO_NAME_SHORT as '門市名稱'
	 , TRANS_DATE_D as '交易日'
	 , sum(isnull([51],0)) as '購貨總金額', sum(isnull([53],0)) as '購貨取消總金額'
	 , sum(isnull([51],0))-sum(isnull([53],0)) as '購貨淨額'
	 , sum(isnull([74],0))+sum(isnull([75],0)) as '自動加值總金額'     
	 , CPT_DATE as '清分日'
from (
	select B.STO_NAME_SHORT, CPT_DATE, CONVERT(varchar(8), TRANS_DATE,112) TRANS_DATE_D, A.TRANS_TYPE,SUM(CONVERT(int, TRANS_AMT)) as VAL_SUM 
	from MPG.TM_SETTLE_DATA_D A 
	inner join MPG.BM_STORE_M B on A.STORE_NO = B.STORE_NO
	where CPT_DATE between @EXEC_DATE_B and @EXEC_DATE_E
	and A.STORE_NO <> '27309332'	--傳藝不列入計算
	group by B.STO_NAME_SHORT, CPT_DATE, CONVERT(varchar(8), TRANS_DATE,112), A.TRANS_TYPE
)D
PIVOT(sum(VAL_SUM)FOR TRANS_TYPE IN ([51],[53],[74],[75])) as p1
group by STO_NAME_SHORT,TRANS_DATE_D, CPT_DATE
ORDER BY 1;";
                    }
                    else
                    {
                        sqlText = @"select STO_NAME_SHORT as '門市名稱'
	 , TRANS_DATE_D as '交易日'
	 , sum(isnull([51],0)) as '購貨總金額', sum(isnull([53],0)) as '購貨取消總金額'
	 , sum(isnull([51],0))-sum(isnull([53],0)) as '購貨淨額'
	 , sum(isnull([74],0))+sum(isnull([75],0)) as '自動加值總金額'
	 , CPT_DATE as '清分日'
from (
	select B.STO_NAME_SHORT, CPT_DATE, CONVERT(varchar(8), TRANS_DATE,112) TRANS_DATE_D, A.TRANS_TYPE,SUM(CONVERT(int, TRANS_AMT)) as VAL_SUM 
	from PAC.TM_SETTLE_DATA_D A 
	inner join PAC.BM_STORE_M B on A.STORE_NO = B.STORE_NO
	where CPT_DATE between @EXEC_DATE_B and @EXEC_DATE_E
	group by B.STO_NAME_SHORT, CPT_DATE, CONVERT(varchar(8), TRANS_DATE,112), A.TRANS_TYPE
	--傳藝列入計算
	union all
	select B.STO_NAME_SHORT, CPT_DATE, CONVERT(varchar(8), TRANS_DATE,112) TRANS_DATE_D, A.TRANS_TYPE,SUM(CONVERT(int, TRANS_AMT)) as VAL_SUM 
	from MPG.TM_SETTLE_DATA_D A 
	inner join MPG.BM_STORE_M B on A.STORE_NO = B.STORE_NO
	where CPT_DATE between @EXEC_DATE_B and @EXEC_DATE_E
	and A.STORE_NO = '27309332'	--傳藝列入計算
	group by B.STO_NAME_SHORT, CPT_DATE, CONVERT(varchar(8), TRANS_DATE,112), A.TRANS_TYPE
)D
PIVOT(sum(VAL_SUM)FOR TRANS_TYPE IN ([51],[53],[74],[75])) as p1
group by STO_NAME_SHORT,TRANS_DATE_D, CPT_DATE
ORDER BY 1; 
";
                    }

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    sqlCmd.Parameters.Add("@EXEC_DATE_B", SqlDbType.VarChar).Value = start;
                    sqlCmd.Parameters.Add("@EXEC_DATE_E", SqlDbType.VarChar).Value = end;
                    

                    SqlDataAdapter adapter = new SqlDataAdapter(sqlCmd);

                    adapter.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                log.Debug(ex.Message);
            }
            return dt;
        }

        public DataTable Report151202T(string start, string end, string src)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();
                    string sqlText = "";

                    if (src == "STATION")
                    {
                        sqlText = @"select R.ROUTE_NAME as '線路'
	 , CONVERT(varchar(8), TRANS_DATE,112) as '交易日期'
	 , SUM( CONVERT(int, TRANS_AMT) ) as '交易總金額'
	 , CPT_DATE as '清分日期'
from KML.TM_SETTLE_DATA_D D
left join KML.BM_TR_ROUTE R on D.ROUTE_NO = R.ROUTE_NO
where  CONVERT(varchar(8), TRANS_DATE,112) between @EXEC_DATE_B and @EXEC_DATE_E
group by R.ROUTE_NAME, CONVERT(varchar(8), TRANS_DATE,112), TRANS_TYPE, CPT_DATE 
order by 2";
                    }
                    else
                    {
                        sqlText = @"select R.STO_NAME_SHORT as '線路'
	 , CONVERT(varchar(8), TRANS_DATE,112) as '交易日期'
	 , SUM( CONVERT(int, TRANS_AMT) ) as '交易總金額'
	 , CPT_DATE as '清分日期'
from KML.TM_SETTLE_STATION_DATA_D D
left join KML.BM_STORE_M R on D.STORE_NO = R.STORE_NO
where CONVERT(varchar(8), TRANS_DATE,112) between @EXEC_DATE_B and @EXEC_DATE_E
group by R.STO_NAME_SHORT, CONVERT(varchar(8), TRANS_DATE,112),TRANS_TYPE, CPT_DATE 
order by 2
";
                    }

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    sqlCmd.Parameters.Add("@EXEC_DATE_B", SqlDbType.VarChar).Value = start;
                    sqlCmd.Parameters.Add("@EXEC_DATE_E", SqlDbType.VarChar).Value = end;


                    SqlDataAdapter adapter = new SqlDataAdapter(sqlCmd);

                    adapter.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                log.Debug(ex.Message);
            }
            return dt;
        }

        public DataTable Report151203T(string start, string end, string type, string code,string merchantNoCom)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();
                    string sqlText = "";
                    if (type == "BY_DATE")
                    {
                        sqlText = @"DECLARE @report TABLE (MERCHANT_NO_COM VARCHAR(8),COMPANY_NAME VARCHAR(20),ACTIVITY_CODE VARCHAR(10),ACTIVITY_NAME VARCHAR(20),CPT_DATE VARCHAR(8),SUM_AMT BIGINT,LOADING_FLG VARCHAR(20) )

INSERT INTO @report
select D.MERCHANT_NO_COM, D.COMPANY_NAME, D.ACTIVITY_CODE, D.ACTIVITY_NAME,
		   D.CPT_DATE, SUM(D.TRANS_AMT) as sumAmt, LOADING_FLG
	from (
				select	X.MERCHANT_NO_COM,		--公司統一編號
				X.COMPANY_NAME,			--公司名稱
				X.ACTIVITY_CODE,		--活動代號
				case when X.ACTIVITY_NAME is null then '查無對應活動'	--活動名稱
					 else X.ACTIVITY_NAME	end as ACTIVITY_NAME,
				A.CPT_DATE,				--清分日期
				A.ICC_NO,				--卡號
				A.TRANS_AMT,			--加值金額
				A.TRANS_DATE_TXLOG,		--加值日期(yyyyMMdd HH:mm:ss)
				case when X.LOADING_FLG is null then '查無對應活動'		--on-line認列狀態
					 when X.LOADING_FLG = '0' then '尚未加值'
					 when X.LOADING_FLG = '1' then '已加值'
					 else '狀態錯誤'	end as LOADING_FLG
		from IBN.TM_SETTLE_DATA_D A
		LEFT OUTER JOIN (SELECT M.MERCHANT_NO_COM,		--公司統一編號
				M.COMPANY_NAME,			--公司名稱
				M.ACTIVITY_CODE, M.ACTIVITY_NAME,M.START_DATE,M.END_DATE,D.ICC_NO,D.LOADING_FLG FROM IBON_COMPANY_LOADING_D D LEFT OUTER JOIN dbo.IBON_COMPANY_LOADING_M M
											
				on M.ACTIVITY_CODE = D.ACTIVITY_CODE
				and M.MERCHANT_NO_COM = D.MERCHANT_NO_COM
			    ) X
				
				ON CONVERT(varchar(8), A.TRANS_DATE_TXLOG,112) between X.[START_DATE] and X.[END_DATE] 
				AND A.ICC_NO = X.ICC_NO
				WHERE A.TRANS_TYPE = '79' AND A.CPT_DATE between @EXEC_DATE_B and @EXEC_DATE_E 
	)D
	GROUP by D.MERCHANT_NO_COM, D.COMPANY_NAME, D.ACTIVITY_CODE, D.ACTIVITY_NAME,
			 D.CPT_DATE, D.LOADING_FLG
	order by MERCHANT_NO_COM, ACTIVITY_CODE, CPT_DATE

	SELECT * FROM (
	SELECT 1 RNK,a.* FROM @report a
	UNION ALL 
	SELECT 2 RNK,'總計', '', '', '',''
		   , ISNULL(SUM(b.SUM_AMT),0),'' FROM @report b ) c ORDER BY RNK,MERCHANT_NO_COM, ACTIVITY_CODE, CPT_DATE";
                        SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                        sqlCmd.Parameters.Add("@EXEC_DATE_B", SqlDbType.VarChar).Value = start;
                        sqlCmd.Parameters.Add("@EXEC_DATE_E", SqlDbType.VarChar).Value = end;


                        SqlDataAdapter adapter = new SqlDataAdapter(sqlCmd);
                        adapter.Fill(dt);
                        return dt;
                    }

                    else
                    {
                        sqlText = @"DECLARE @report TABLE (MERCHANT_NO_COM VARCHAR(8),COMPANY_NAME VARCHAR(20),ACTIVITY_CODE VARCHAR(10),ACTIVITY_NAME VARCHAR(20),CPT_DATE VARCHAR(8),SUM_AMT BIGINT,LOADING_FLG VARCHAR(20) )



INSERT INTO @report
select D.MERCHANT_NO_COM, D.COMPANY_NAME, D.ACTIVITY_CODE, D.ACTIVITY_NAME,
		   D.CPT_DATE, SUM(D.TRANS_AMT) as sumAmt, LOADING_FLG
	from (
		select	M.MERCHANT_NO_COM,		--公司統一編號
				M.COMPANY_NAME,			--公司名稱
				M.ACTIVITY_CODE,		--活動代號
				case when M.ACTIVITY_NAME is null then '查無對應活動'	--活動名稱
					 else M.ACTIVITY_NAME	end as ACTIVITY_NAME,
				A.CPT_DATE,				--清分日期
				A.ICC_NO,				--卡號
				A.TRANS_AMT,			--加值金額
				A.TRANS_DATE_TXLOG,		--加值日期(yyyyMMdd HH:mm:ss)
				case when D.LOADING_FLG is null then '查無對應活動'		--on-line認列狀態
					 when D.LOADING_FLG = '0' then '尚未加值'
					 when D.LOADING_FLG = '1' then '已加值'
					 else '狀態錯誤'	end as LOADING_FLG
		from IBN.TM_SETTLE_DATA_D A
		inner join IBON_COMPANY_LOADING_D D on A.ICC_NO = D.ICC_NO
											
		inner join dbo.IBON_COMPANY_LOADING_M M on M.ACTIVITY_CODE = D.ACTIVITY_CODE
											  and M.MERCHANT_NO_COM = D.MERCHANT_NO_COM
											  and CONVERT(varchar(8), A.TRANS_DATE_TXLOG,112) between M.[START_DATE] and M.[END_DATE] 
											  and M.ACTIVITY_CODE = Case when @EXEC_ACTIVITY_CODE = 'ALL' then M.ACTIVITY_CODE
																		else @EXEC_ACTIVITY_CODE end
											  and M.MERCHANT_NO_COM = Case when @EXEC_MERCHANT_NO_COM = 'ALL' then M.MERCHANT_NO_COM
																		else @EXEC_MERCHANT_NO_COM end
		where A.CPT_DATE between @EXEC_DATE_B and @EXEC_DATE_E and A.TRANS_TYPE = '79'
	)D
	GROUP by D.MERCHANT_NO_COM, D.COMPANY_NAME, D.ACTIVITY_CODE, D.ACTIVITY_NAME,
			 D.CPT_DATE, D.LOADING_FLG
	order by MERCHANT_NO_COM, ACTIVITY_CODE, CPT_DATE

	SELECT * FROM (
	SELECT 1 RNK,a.* FROM @report a
	UNION ALL 
	SELECT 2 RNK,'總計', '', '', '',''
		   , SUM(b.SUM_AMT),'' FROM @report b ) c ORDER BY RNK,MERCHANT_NO_COM, ACTIVITY_CODE, CPT_DATE";
                        SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                        sqlCmd.Parameters.Add("@EXEC_DATE_B", SqlDbType.VarChar).Value = start;
                        sqlCmd.Parameters.Add("@EXEC_DATE_E", SqlDbType.VarChar).Value = end;
                        sqlCmd.Parameters.Add("@EXEC_ACTIVITY_CODE", SqlDbType.VarChar).Value = code;
                        sqlCmd.Parameters.Add("@EXEC_MERCHANT_NO_COM", SqlDbType.VarChar).Value = merchantNoCom;

                        SqlDataAdapter adapter = new SqlDataAdapter(sqlCmd);
                        adapter.Fill(dt);
                        //return dt;
                    }
                    
                }
                
            }
            catch(Exception ex)
            {
                log.Debug(ex.Message);
            }
            return dt;
        }
        public DataTable Report151204T(string start, string end,string code)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();
                    string sqlText = "";
                    sqlText = @"select	D.MERCHANT_NO_COM,						--公司統一編號
		D.COMPANY_NAME,							--公司名稱
		D.ACTIVITY_CODE,						--活動代號
		D.ACTIVITY_NAME,						--活動名稱
		D.[START_DATE],							--加值有效起始日
		D.[END_DATE],							--加值有效終止日
	   SUM(D.sumAMOUNT) '企業加值活動總金額',	--企業加值活動總金額
	   SUM(D.sumTRANS_AMT) '已清分總額',		--已清分總額
	   SUM(D.sumAMOUNT) - SUM(D.sumTRANS_AMT) as '未使用餘額' --未使用餘額
from (
		select M.MERCHANT_NO_COM,		--公司統一編號
			   M.COMPANY_NAME,			--公司名稱
			   M.ACTIVITY_CODE,			--活動代號
			   M.ACTIVITY_NAME,			--活動名稱
			   M.[START_DATE],			--加值有效起始日
			   M.[END_DATE],			--加值有效終止日
			   SUM(ISNULL(D.AMOUNT,0))	as sumAMOUNT,	--企業加值活動總金額
			   NULL	as sumTRANS_AMT		--已清分總額
		from dbo.IBON_COMPANY_LOADING_M M
		inner join IBON_COMPANY_LOADING_D D on M.ACTIVITY_CODE = D.ACTIVITY_CODE
										   and M.MERCHANT_NO_COM = D.MERCHANT_NO_COM
										   and M.[END_DATE] >= @EXEC_DATE_B and M.[START_DATE] <= @EXEC_DATE_E
										   and M.ACTIVITY_CODE = Case when @EXEC_ACTIVITY_CODE = 'ALL' then M.ACTIVITY_CODE
																	  else @EXEC_ACTIVITY_CODE end
		GROUP BY M.MERCHANT_NO_COM, M.COMPANY_NAME, M.ACTIVITY_CODE, M.ACTIVITY_NAME, M.[START_DATE], M.[END_DATE]
		union all
		select M.MERCHANT_NO_COM,		--公司統一編號
			   M.COMPANY_NAME,			--公司名稱
			   M.ACTIVITY_CODE,			--活動代號
			   M.ACTIVITY_NAME,			--活動名稱
			   M.[START_DATE],			--加值有效起始日
			   M.[END_DATE],			--加值有效終止日
			   NULL,					--企業加值活動總金額
			   SUM(ISNULL(CONVERT(int, A.TRANS_AMT,0),0))	as sumTRANS_AMT	--已清分總額
		from dbo.IBON_COMPANY_LOADING_M M
		inner join IBON_COMPANY_LOADING_D D on M.ACTIVITY_CODE = D.ACTIVITY_CODE
										   and M.MERCHANT_NO_COM = D.MERCHANT_NO_COM
										   and M.[END_DATE] >= @EXEC_DATE_B and M.[START_DATE] <= @EXEC_DATE_E
										   and M.ACTIVITY_CODE = Case when @EXEC_ACTIVITY_CODE = 'ALL' then M.ACTIVITY_CODE
																	  else @EXEC_ACTIVITY_CODE end
		left join IBN.TM_SETTLE_DATA_D A on A.ICC_NO = D.ICC_NO
										and CONVERT(varchar(8), A.TRANS_DATE_TXLOG,112) between M.[START_DATE] and M.[END_DATE] 
										and A.TRANS_TYPE = '79'
		GROUP BY M.MERCHANT_NO_COM, M.COMPANY_NAME, M.ACTIVITY_CODE, M.ACTIVITY_NAME, M.[START_DATE], M.[END_DATE]
)D
GROUP BY D.MERCHANT_NO_COM, D.COMPANY_NAME, D.ACTIVITY_CODE, D.ACTIVITY_NAME, D.[START_DATE], D.[END_DATE]
order by MERCHANT_NO_COM, ACTIVITY_CODE";
                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    sqlCmd.Parameters.Add("@EXEC_DATE_B", SqlDbType.VarChar).Value = start;
                    sqlCmd.Parameters.Add("@EXEC_DATE_E", SqlDbType.VarChar).Value = end;
                    sqlCmd.Parameters.Add("@EXEC_ACTIVITY_CODE", SqlDbType.VarChar).Value = code;
                    SqlDataAdapter adapter = new SqlDataAdapter(sqlCmd);
                    adapter.Fill(dt);
                    //return dt;
                }
            }
            catch(Exception ex)
            {
                log.Debug(ex.Message);
            }
            return dt;
        }

        public DataTable Report151205T(string start, string end, string type, string code, string merchantNoCom)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();
                    string sqlText = "";
                    //if (type == "BY_DATE")
                    {
                        sqlText = @"DECLARE @report TABLE (MERCHANT_NO_COM VARCHAR(8),COMPANY_NAME VARCHAR(60),ACTIVITY_CODE VARCHAR(10),ACTIVITY_NAME VARCHAR(20),CPT_DATE VARCHAR(8),SUM_AMT BIGINT )

INSERT INTO @report
select D.MERCHANT_NO_COM, D.COMPANY_NAME, D.ACTIVITY_CODE, D.ACTIVITY_NAME,
		   D.CPT_DATE, SUM(D.TRANS_AMT) as sumAmt
	from (
				select	X.MERCHANT_NO_COM,		--公司統一編號
				X.COMPANY_NAME,			--公司名稱
				X.ACTIVITY_CODE,		--活動代號
				case when X.ACTIVITY_NAME is null then '查無對應活動'	--活動名稱
					 else X.ACTIVITY_NAME	end as ACTIVITY_NAME,
				A.CPT_DATE,				--清分日期
				A.ICC_NO,				--卡號
				A.TRANS_AMT,			--加值金額
				A.TRANS_DATE_TXLOG		--加值日期(yyyyMMdd HH:mm:ss)

		from IBN.TM_SETTLE_DATA_D A
		LEFT OUTER JOIN (SELECT M.MERCHANT_NO_COM,		--公司統一編號
				M.COMPANY_NAME,			--公司名稱
				M.ACTIVITY_CODE, M.ACTIVITY_NAME,M.START_DATE,M.END_DATE,D.ICC_NO,D.LOAD_DATE,D.AMOUNT,D.LOADING_FLG FROM IBON_COMPANY_LOADING_D D LEFT OUTER JOIN dbo.IBON_COMPANY_LOADING_M M
											
				on M.ACTIVITY_CODE = D.ACTIVITY_CODE
				and M.MERCHANT_NO_COM = D.MERCHANT_NO_COM 
			    ) X
				
				ON A.TRANS_DATE_TXLOG=X.LOAD_DATE 
				AND A.ICC_NO = X.ICC_NO AND A.TRANS_AMT=X.AMOUNT
				WHERE A.TRANS_TYPE = '79' AND A.CPT_DATE between @EXEC_DATE_B and @EXEC_DATE_E AND X.LOADING_FLG='1'
	)D
	GROUP by D.MERCHANT_NO_COM, D.COMPANY_NAME, D.ACTIVITY_CODE, D.ACTIVITY_NAME,
			 D.CPT_DATE
	order by MERCHANT_NO_COM, ACTIVITY_CODE, CPT_DATE

	SELECT * FROM (
	SELECT 1 RNK,a.* FROM @report a
	UNION ALL 
	SELECT 2 RNK,'總計', '', '', '',''
		   , ISNULL(SUM(b.SUM_AMT),0) FROM @report b ) c ORDER BY RNK,MERCHANT_NO_COM, ACTIVITY_CODE, CPT_DATE";
                        SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                        sqlCmd.Parameters.Add("@EXEC_DATE_B", SqlDbType.VarChar).Value = start;
                        sqlCmd.Parameters.Add("@EXEC_DATE_E", SqlDbType.VarChar).Value = end;


                        SqlDataAdapter adapter = new SqlDataAdapter(sqlCmd);
                        adapter.Fill(dt);
                        return dt;
                    }
        /*
                    else
                    {
                        sqlText = @"DECLARE @report TABLE (MERCHANT_NO_COM VARCHAR(8),COMPANY_NAME VARCHAR(20),ACTIVITY_CODE VARCHAR(10),ACTIVITY_NAME VARCHAR(20),CPT_DATE VARCHAR(8),SUM_AMT BIGINT )



INSERT INTO @report
select D.MERCHANT_NO_COM, D.COMPANY_NAME, D.ACTIVITY_CODE, D.ACTIVITY_NAME,
		   D.CPT_DATE, SUM(D.TRANS_AMT) as sumAmt
	from (
		select	M.MERCHANT_NO_COM,		--公司統一編號
				M.COMPANY_NAME,			--公司名稱
				M.ACTIVITY_CODE,		--活動代號
				case when M.ACTIVITY_NAME is null then '查無對應活動'	--活動名稱
					 else M.ACTIVITY_NAME	end as ACTIVITY_NAME,
				A.CPT_DATE,				--清分日期
				A.ICC_NO,				--卡號
				A.TRANS_AMT,			--加值金額
				A.TRANS_DATE_TXLOG		--加值日期(yyyyMMdd HH:mm:ss)

		from IBN.TM_SETTLE_DATA_D A
		inner join IBON_COMPANY_LOADING_D D on A.ICC_NO = D.ICC_NO AND A.TRANS_DATE_TXLOG=D.LOAD_DATE AND A.TRANS_AMT=D.AMOUNT
											
		inner join dbo.IBON_COMPANY_LOADING_M M on M.ACTIVITY_CODE = D.ACTIVITY_CODE
											  and M.MERCHANT_NO_COM = D.MERCHANT_NO_COM
											  --and CONVERT(varchar(8), A.TRANS_DATE_TXLOG,112) between M.[START_DATE] and M.[END_DATE] 
											  and M.ACTIVITY_CODE = Case when @EXEC_ACTIVITY_CODE = 'ALL' then M.ACTIVITY_CODE
																		else @EXEC_ACTIVITY_CODE end
											  and M.MERCHANT_NO_COM = Case when @EXEC_MERCHANT_NO_COM = 'ALL' then M.MERCHANT_NO_COM
																		else @EXEC_MERCHANT_NO_COM end
		where A.CPT_DATE between @EXEC_DATE_B and @EXEC_DATE_E and A.TRANS_TYPE = '79' AND D.LOADING_FLG = '1'
	)D
	GROUP by D.MERCHANT_NO_COM, D.COMPANY_NAME, D.ACTIVITY_CODE, D.ACTIVITY_NAME,
			 D.CPT_DATE
	order by MERCHANT_NO_COM, ACTIVITY_CODE, CPT_DATE

	SELECT * FROM (
	SELECT 1 RNK,a.* FROM @report a
	UNION ALL 
	SELECT 2 RNK,'總計', '', '', '',''
		   , SUM(b.SUM_AMT) FROM @report b ) c ORDER BY RNK,MERCHANT_NO_COM, ACTIVITY_CODE, CPT_DATE";
    
                        SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                        sqlCmd.Parameters.Add("@EXEC_DATE_B", SqlDbType.VarChar).Value = start;
                        sqlCmd.Parameters.Add("@EXEC_DATE_E", SqlDbType.VarChar).Value = end;
                        sqlCmd.Parameters.Add("@EXEC_ACTIVITY_CODE", SqlDbType.VarChar).Value = code;
                        sqlCmd.Parameters.Add("@EXEC_MERCHANT_NO_COM", SqlDbType.VarChar).Value = merchantNoCom;

                        SqlDataAdapter adapter = new SqlDataAdapter(sqlCmd);
                        adapter.Fill(dt);
                        //return dt;
                    }
            */
                }

            }
            catch (Exception ex)
            {
                log.Debug(ex.Message);
            }
            return dt;
        }

        public DataTable Report151206T(string start, string end, string code)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();
                    string sqlText = "";
                    sqlText = @"select	D.MERCHANT_NO_COM,						--公司統一編號
		D.COMPANY_NAME,							--公司名稱
		D.ACTIVITY_CODE,						--活動代號
		D.ACTIVITY_NAME,						--活動名稱
		D.[START_DATE],							--加值有效起始日
		D.[END_DATE],							--加值有效終止日
	   SUM(D.sumAMOUNT) '企業加值活動總金額',	--企業加值活動總金額
	   SUM(D.sumTRANS_AMT) '已清分總額',		--已清分總額
	   SUM(D.sumAMOUNT) - SUM(D.sumTRANS_AMT) as '未使用餘額' --未使用餘額
from (
		select M.MERCHANT_NO_COM,		--公司統一編號
			   M.COMPANY_NAME,			--公司名稱
			   M.ACTIVITY_CODE,			--活動代號
			   M.ACTIVITY_NAME,			--活動名稱
			   M.[START_DATE],			--加值有效起始日
			   M.[END_DATE],			--加值有效終止日
			   SUM(ISNULL(D.AMOUNT,0))	as sumAMOUNT,	--企業加值活動總金額
			   NULL	as sumTRANS_AMT		--已清分總額
		from dbo.IBON_COMPANY_LOADING_M M
		inner join IBON_COMPANY_LOADING_D D on M.ACTIVITY_CODE = D.ACTIVITY_CODE
										   and M.MERCHANT_NO_COM = D.MERCHANT_NO_COM
										   and M.[END_DATE] >= @EXEC_DATE_B and M.[START_DATE] <= @EXEC_DATE_E
										   and M.ACTIVITY_CODE = Case when @EXEC_ACTIVITY_CODE = 'ALL' then M.ACTIVITY_CODE
																	  else @EXEC_ACTIVITY_CODE end
		GROUP BY M.MERCHANT_NO_COM, M.COMPANY_NAME, M.ACTIVITY_CODE, M.ACTIVITY_NAME, M.[START_DATE], M.[END_DATE]
		union all
		select M.MERCHANT_NO_COM,		--公司統一編號
			   M.COMPANY_NAME,			--公司名稱
			   M.ACTIVITY_CODE,			--活動代號
			   M.ACTIVITY_NAME,			--活動名稱
			   M.[START_DATE],			--加值有效起始日
			   M.[END_DATE],			--加值有效終止日
			   NULL,					--企業加值活動總金額
			   SUM(ISNULL(CONVERT(bigint, A.TRANS_AMT,0),0))	as sumTRANS_AMT	--已清分總額
		from dbo.IBON_COMPANY_LOADING_M M
		inner join IBON_COMPANY_LOADING_D D on M.ACTIVITY_CODE = D.ACTIVITY_CODE
										   and M.MERCHANT_NO_COM = D.MERCHANT_NO_COM
										   and M.[END_DATE] >= @EXEC_DATE_B and M.[START_DATE] <= @EXEC_DATE_E
										   and M.ACTIVITY_CODE = Case when @EXEC_ACTIVITY_CODE = 'ALL' then M.ACTIVITY_CODE
																	  else @EXEC_ACTIVITY_CODE end
		left join IBN.TM_SETTLE_DATA_D A on A.ICC_NO = D.ICC_NO
										and A.TRANS_DATE_TXLOG=D.LOAD_DATE and A.TRANS_AMT=D.AMOUNT
										and A.TRANS_TYPE = '79' and D.LOADING_FLG='1'
		GROUP BY M.MERCHANT_NO_COM, M.COMPANY_NAME, M.ACTIVITY_CODE, M.ACTIVITY_NAME, M.[START_DATE], M.[END_DATE]
)D
GROUP BY D.MERCHANT_NO_COM, D.COMPANY_NAME, D.ACTIVITY_CODE, D.ACTIVITY_NAME, D.[START_DATE], D.[END_DATE]
order by MERCHANT_NO_COM, ACTIVITY_CODE";
                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    sqlCmd.Parameters.Add("@EXEC_DATE_B", SqlDbType.VarChar).Value = start;
                    sqlCmd.Parameters.Add("@EXEC_DATE_E", SqlDbType.VarChar).Value = end;
                    sqlCmd.Parameters.Add("@EXEC_ACTIVITY_CODE", SqlDbType.VarChar).Value = code;
                    SqlDataAdapter adapter = new SqlDataAdapter(sqlCmd);
                    adapter.Fill(dt);
                    //return dt;
                }
            }
            catch (Exception ex)
            {
                log.Debug(ex.Message);
            }
            return dt;
        }
        public DataTable ReportMerchant()
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();
                    string sqlText =
                            @"--select MERCHANT_NO,MERCHANT_STNAME
                                --        from GM_MERCHANT
                                --        where MERCHANT_TYPE not in ('BANK','OTHER') and MERCHANT_NO not in ('70762591','23525767','12411160','21251214') 
                                --        order by MERCHANT_TYPE,MERC_GROUP desc,MERCHANT_NO 
                                select D.MERCHANT_NO,MERCHANT_STNAME
                                from GM_MERCHANT_TYPE_D D
                                LEFT JOIN GM_MERCHANT G 
                                ON D.MERCHANT_NO = G.MERCHANT_NO
                                where D.SHOW_FLG = 'Y' 
                                order by MERCHANT_TYPE,MERC_GROUP desc,MERCHANT_NO ";

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);

                    SqlDataAdapter adapter = new SqlDataAdapter(sqlCmd);

                    adapter.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                log.Debug(ex.Message);
            }
            return dt;
        }

        public DataTable ReportBankMerchant()
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();
                    string sqlText =
                            @"SELECT distinct MERCH_SCHEMAID
	FROM GM_MERCHANT
	WHERE MERCHANT_TYPE='BANK' and MERCH_SCHEMAID<>'BNK567' ";

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);

                    SqlDataAdapter adapter = new SqlDataAdapter(sqlCmd);

                    adapter.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                log.Debug(ex.Message);
            }
            return dt;
        }
        public DataTable ReportBankMerchant(string merNo)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();
                    string sqlText;
				    if (merNo.Equals("ALL") )
				    {
                        sqlText= @"SELECT distinct MERCH_SCHEMAID
					    FROM GM_MERCHANT
					    WHERE MERCHANT_TYPE='BANK' and MERCH_SCHEMAID<>'BNK567' ";

                        SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
					    sqlCmd.Parameters.Add("@p_mer_no", SqlDbType.VarChar).Value = merNo;
					    SqlDataAdapter adapter = new SqlDataAdapter(sqlCmd);
                        adapter.Fill(dt);
                    }
				    else
				    {
					    sqlText= @"SELECT distinct MERCH_SCHEMAID
					    FROM GM_MERCHANT
					    WHERE MERCHANT_TYPE='BANK' and MERCH_SCHEMAID<>'BNK567'
					    and MERCHANT_NO = @p_mer_no ";

                        SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
					    sqlCmd.Parameters.Add("@p_mer_no", SqlDbType.VarChar).Value = merNo;
					    SqlDataAdapter adapter = new SqlDataAdapter(sqlCmd);
                        adapter.Fill(dt);
				    }
                }
            }
            catch (Exception ex)
            {
                log.Debug(ex.Message);
            }
            return dt;
        }
        

        public DataTable MerchantName(string merchantNo)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();
                    string sqlText =
                            @"select MERCHANT_NO,MERCHANT_STNAME,MERC_GROUP
                                        from GM_MERCHANT
                                        where MERCHANT_NO = @MERCHANT_NO ";

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    sqlCmd.Parameters.Add("@MERCHANT_NO", SqlDbType.VarChar).Value = merchantNo;

                    SqlDataAdapter adapter = new SqlDataAdapter(sqlCmd);

                    adapter.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                log.Debug(ex.Message);
            }
            return dt;
        }


        public DataTable TypeName(string No, string f1, string f2, string table)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();
                    string sqlText =
                            String.Format(@"select [{0}] as MERCHANT_NO, [{1}] as MERCHANT_STNAME
                                        from [{2}]
                                        where [{0}] = @NO ", f1, f2, table);

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    sqlCmd.Parameters.Add("@NO", SqlDbType.VarChar).Value = No;

                    SqlDataAdapter adapter = new SqlDataAdapter(sqlCmd);

                    adapter.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                log.Debug(ex.Message);
            }
            return dt;
        }
        public DataTable Report170901(string start, string end, string merchantNo)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();
                    string sqlText = "";
                    sqlText = @"
                                SELECT g.MERCHANT_STNAME AS 特約機構,
                                TRANS_DATE AS 交易日,
                                CASE WHEN STO_NAME_SHORT IS NULL THEN gs.STORE_NO ELSE STO_NAME_SHORT END AS 店名,
                                ISNULL(SUM(CASE TRANS_TYPE WHEN '21' THEN ISNULL(TRANS_CNT,0) END),0) AS 購貨筆數, 
                                ISNULL(SUM(CASE TRANS_TYPE WHEN '21' THEN ISNULL(TRANS_AMT,0) END),0) AS 購貨金額, 
                                ISNULL(SUM(CASE TRANS_TYPE WHEN '23' THEN ISNULL(TRANS_CNT,0) END),0) AS 購貨取消筆數, 
                                ISNULL(SUM(CASE TRANS_TYPE WHEN '23' THEN ISNULL(TRANS_AMT,0) END),0) AS 購貨取消金額,			 
                                ISNULL(SUM(CASE TRANS_TYPE WHEN '22' THEN ISNULL(TRANS_CNT,0) END),0) AS 加值筆數, 
                                ISNULL(SUM(CASE TRANS_TYPE WHEN '22' THEN ISNULL(TRANS_AMT,0) END),0) AS 加值金額, 
                                ISNULL(SUM(CASE TRANS_TYPE WHEN '24' THEN ISNULL(TRANS_CNT,0) END),0) AS 加值取消筆數, 
                                ISNULL(SUM(CASE TRANS_TYPE WHEN '24' THEN ISNULL(TRANS_AMT,0) END),0) AS 加值取消金額
                                FROM GM_CPT_STORE_SUM_D gs INNER JOIN GM_MERCHANT g
                                ON gs.MERCHANT_NO = g.MERCHANT_NO
                                LEFT OUTER JOIN IM_STOREM_ALL_VIEW v
                                on gs.MERCHANT_NO = v.MERCHANT_NO
                                and gs.STORE_NO = v.STORE_NO
                                and v.M_KIND = '21'
                                WHERE gs.TRANS_DATE BETWEEN @start_date AND @end_date 
                                 
                               ";

                    if (merchantNo != "")
                    {
                        sqlText += "AND gs.MERCHANT_NO = @mer_no ";
                    }
                    sqlText += "GROUP BY MERCHANT_STNAME,TRANS_DATE,STO_NAME_SHORT,gs.STORE_NO  ORDER BY 特約機構,交易日,店名 DESC";

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    sqlCmd.Parameters.Add("@start_date", SqlDbType.VarChar).Value = start;
                    sqlCmd.Parameters.Add("@end_date", SqlDbType.VarChar).Value = end;
                    sqlCmd.Parameters.Add("@mer_no", SqlDbType.VarChar).Value = merchantNo;


                    SqlDataAdapter adapter = new SqlDataAdapter(sqlCmd);

                    adapter.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                log.Debug(ex.Message);
            }
            return dt;
        }
        //20200501-20200502 汪翊新增
        //20210127 汪翊修改


        public DataTable Report200501(string start, string end)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();
                    string sqlText = String.Format(@" WITH d  AS (
                        SELECT '台北捷運'   AS 特約機構, 
                               a.CPT_DATE   AS 清算日期,
                               c.BANK_NAME  AS 來源,
                               a.ICC_NO     AS 卡號,
                               a.ADJ_AMT    AS 交易金額
                        FROM iSettle2.dbo.GM_BANK_CT_ADJAMT_D AS a
                        INNER JOIN iSettle2.dbo.CT_BACK_START_M AS b
                            ON a.ICC_NO = b.ICC_NO
                            AND b.REAL_BK_CPT_DATE = a.CPT_DATE
                            AND b.BK_FLG = '03' --03：返還完成
                            LEFT JOIN iSettle2.dbo.CM_BANK_D AS c
                            ON a.ICC_NO like c.CA_DPT + '%'
                        WHERE a.ADJCASE_NO LIKE 'AA%'
                           AND a.CPT_DATE BETWEEN @CPT_DATE_S AND @CPT_DATE_E
                        UNION ALL
                        SELECT '台北捷運'   , 
                               a.CPT_DATE   ,
                               (CASE WHEN a.KIND_SYSTEM IN ('CUS','BANK_SKIP') THEN '客服'
                                     WHEN a.KIND_SYSTEM IN ('TCG')             THEN '市政府' 
                                END) AS 來源,
                               a.ICC_NO     ,
                               a.REWARD_AMT
                        FROM iSettle2.dbo.GM_TRT_FREQUENT_PASSENGER AS a
                        WHERE a.KIND_SYSTEM in ('BANK_SKIP','CUS','TCG')
                           AND a.CPT_DATE BETWEEN @CPT_DATE_S AND @CPT_DATE_E)
                        SELECT * 
                        FROM   (SELECT TOP 100 PERCENT * 
                                FROM   d 
                                ORDER  BY 清算日期) P 
                        UNION ALL
                        SELECT '總計','','','',Sum(交易金額) FROM   d
					");

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    sqlCmd.Parameters.Add("@CPT_DATE_S", SqlDbType.VarChar).Value = start;
                    sqlCmd.Parameters.Add("@CPT_DATE_E", SqlDbType.VarChar).Value = end;

                    SqlDataAdapter adapter = new SqlDataAdapter(sqlCmd);

                    adapter.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                log.Debug(ex.Message);
            }
            return dt;
        }

        public DataTable Report200501_2(string start, string end)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();
                    string sqlText = String.Format(@"SELECT a.cpt_date,
                                    c.bank_name,
                                    '聯名卡' AS [TYPE],
                                    a.icc_no,
                                    a.adj_amt
                             INTO   #tempa
                             FROM   isettle2.dbo.gm_bank_ct_adjamt_d AS a
                                    INNER JOIN isettle2.dbo.ct_back_start_m AS b
                                            ON a.icc_no = b.icc_no
                                               AND b.real_bk_cpt_date = a.cpt_date
                                               AND b.bk_flg = '03' --03：返還完成
                                    LEFT JOIN isettle2.dbo.cm_bank_d AS c
                                           ON a.icc_no LIKE c.ca_dpt + '%'
                             WHERE  a.adjcase_no LIKE 'AA%'
                                    AND a.cpt_date BETWEEN @CPT_DATE_S AND @CPT_DATE_E;         
                             
                             INSERT #tempa
                             SELECT a.cpt_date,
                                    (CASE WHEN a.KIND_SYSTEM IN ('CUS','BANK_SKIP') THEN '客服'
                                     WHEN a.KIND_SYSTEM IN ('TCG')                  THEN '市政府' 
                                     END) AS [BANK_NAME],
                                    (CASE WHEN a.KIND_SYSTEM IN ('CUS','BANK_SKIP') THEN '客服'
                                     WHEN a.KIND_SYSTEM IN ('TCG')                  THEN '市政府' 
                                     END) AS [TYPE],
                                    a.icc_no,
                                    a.reward_amt
                             FROM   isettle2.dbo.gm_trt_frequent_passenger AS a
                             WHERE  a.kind_system IN ( 'BANK_SKIP', 'CUS' , 'TCG')
                                    AND a.cpt_date BETWEEN @CPT_DATE_S AND @CPT_DATE_E
                             
                             SELECT type         AS 來源,
                                    bank_name    AS 備註,
                                    Sum(adj_amt) 合計
                             INTO   #tempb
                             FROM   #tempa
                             GROUP  BY type,
                                       bank_name
                             ORDER  BY type
                             
                             SELECT *
                             FROM   #tempb
                             UNION ALL
                             SELECT '總計',
                                    '',
                                    Sum(adj_amt)
                             FROM   #tempa
                             
                             DROP TABLE #tempa, #tempb ");
                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    sqlCmd.Parameters.Add("@CPT_DATE_S", SqlDbType.VarChar).Value = start;
                    sqlCmd.Parameters.Add("@CPT_DATE_E", SqlDbType.VarChar).Value = end;

                    SqlDataAdapter adapter = new SqlDataAdapter(sqlCmd);

                    adapter.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                log.Debug(ex.Message);
            }
            return dt;
        }



        public DataTable Report200502(string start)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();
                    string sqlText = String.Format(@"DECLARE @TABLE TABLE (統編 VARCHAR(20), 特約機構 VARCHAR(50), 簡稱 VARCHAR(50), 購貨淨額 INT, 回饋率未稅 VARCHAR(4), 回饋金總額 decimal(18,0), 回饋金未稅 decimal(18,0), 回饋金稅額 decimal(18,0) )
                            INSERT @TABLE
                            SELECT 
	                             G.MERCHANT_NO 統編,
	                             G.MERCHANT_NAME 特約機構,
	                             G.MERCHANT_STNAME 簡稱,
	                             SUM(PUR_NET) 購貨淨額,
	                             '0.4%'回饋率未稅,
	                             cast( ROUND( SUM(S.PUR_NET)*0.4/100,0) +ROUND( ROUND( SUM(S.PUR_NET)*0.4/100,0)*0.05,0) as decimal(18,0)) 回饋金總額 ,
	                             cast( ROUND( SUM(S.PUR_NET)*0.4/100,0)as decimal(18,0)) 回饋金未稅,
	                             cast( ROUND( ROUND( SUM(S.PUR_NET)*0.4/100,0)*0.05,0)as decimal(18,0))回饋金稅額 
                            FROM dbo.GM_MERCHANT G
                            INNER JOIN NCCC.TM_CPT_SUM_D S
	                            ON G.MERCHANT_NO = S.MERCHANT_NO
                            WHERE SUBSTRING(CPT_DATE,0,7)=@CPT_DATE
	                            AND MERC_GROUP ='NCCC'
	                            AND MERCHANT_STNAME LIKE '%麥當勞%'
                            GROUP BY G.MERCHANT_NO,G.MERCHANT_NAME,G.MERCHANT_STNAME

                            SELECT *
                            FROM @TABLE
                            union all
                            SELECT '' ,'總計' ,'', SUM(購貨淨額),'0.4%',SUM(回饋金總額),SUM(回饋金未稅),SUM(回饋金稅額)
                            FROM @TABLE");

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    sqlCmd.Parameters.Add("@CPT_DATE", SqlDbType.VarChar).Value = start;
                    SqlDataAdapter adapter = new SqlDataAdapter(sqlCmd);

                    adapter.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                log.Debug(ex.Message);
            }
            return dt;
        }




    }
}
