--DECLARE @EXEC_MERCHANT_NO VARCHAR(8) = '85248708'
--DECLARE @EXEC_CPT_DATE VARCHAR(8) = '20190201'
--DECLARE @EXEC_CPT_DATE_B VARCHAR(8) = '20181201'
--DECLARE @EXEC_CPT_DATE_E VARCHAR(8) = '20181231' 
DECLARE @EXEC_MERCHANT_NO VARCHAR(8) = '{2}'
DECLARE @EXEC_CPT_DATE VARCHAR(8) = '{1}'
DECLARE @EXEC_CPT_DATE_B VARCHAR(8) = ''
DECLARE @EXEC_CPT_DATE_E VARCHAR(8) = '' 

SELECT @EXEC_CPT_DATE_B = GET_SUM_DATE.SDATE,
        @EXEC_CPT_DATE_E = GET_SUM_DATE.EDATE
FROM
(
	SELECT
		CASE WHEN SUM_DAY_S = '99' THEN CONVERT(VARCHAR(8), DATEADD(DAY, -1				, DATEADD(MONTH, DATEDIFF(MONTH, '', @EXEC_CPT_DATE) + (SUM_MON_S + 1), '')), 112)
								    ELSE CONVERT(VARCHAR(8), DATEADD(DAY, -1 + SUM_DAY_S	, DATEADD(MONTH, DATEDIFF(MONTH, '', @EXEC_CPT_DATE) + (SUM_MON_S + 0), '')), 112) END AS 'SDATE',
		CASE WHEN SUM_DAY_E = '99' THEN CONVERT(VARCHAR(8), DATEADD(DAY, -1				, DATEADD(MONTH, DATEDIFF(MONTH, '', @EXEC_CPT_DATE) + (SUM_MON_E + 1), '')), 112)
								    ELSE CONVERT(VARCHAR(8), DATEADD(DAY, -1 + SUM_DAY_E	, DATEADD(MONTH, DATEDIFF(MONTH, '', @EXEC_CPT_DATE) + (SUM_MON_E + 0), '')), 112) END AS 'EDATE'
		,SUM_DAY_S,SUM_DAY_E
	FROM	GM_CONTRACT_M
	WHERE	MERCHANT_NO = @EXEC_MERCHANT_NO
		AND	@EXEC_CPT_DATE BETWEEN EFF_DATE_FROM AND EFF_DATE_TO
)GET_SUM_DATE

DECLARE @TmpSum TABLE (
ID TINYINT,
MERCHANT_NO varchar(20), 
MERCHANT_NAME varchar(100),
INVOICE_NO VARCHAR(20),
PUR_NET DEC,
SPACE_AERA CHAR(1),
CNT dec,
FEE_RATE float(10),
fee bigint,
fee_tax_excluded bigint,
fee_tax bigint
)

INSERT INTO @TmpSum
select  1,
		M1.MERCHANT_NO,
		M1.MERCHANT_NAME+'-有值' AS '特約機構名稱',
		M1.INVOICE_NO,
		ISNULL((DATA.sumPCHA-DATA.sumPCHRA),0) as '購貨淨額', 
		'' AS '重複',
		0 AS '0值交易筆數',
		Convert(FLOAT(50),ISNULL((SELECT FEE_RATE FROM GM_CONTRACT_D WHERE MERCHANT_NO = @EXEC_MERCHANT_NO AND SETTLE_TYPE = 'P' AND FEE_KIND = 'CM' AND FEE_CAL_FLG = 'AM'),0)) as			'購貨手續費率', 
		ISNULL(cast(ROUND((DATA.sumPCHA-DATA.sumPCHRA)*DATA.P_CM_AM,0) as bigint),0)	as '消費手續費',
		ISNULL(cast(ROUND(cast(ROUND((DATA.sumPCHA-DATA.sumPCHRA)*DATA.P_CM_AM,0) as decimal(20,5))/1.05,0) as bigint),0)   as '消費手續費未稅',
		ISNULL(cast(ROUND((DATA.sumPCHA-DATA.sumPCHRA)*DATA.P_CM_AM,0) as bigint)-cast(ROUND(cast(ROUND((DATA.sumPCHA-DATA.sumPCHRA)*DATA.P_CM_AM,0) as decimal(15,5))/1.05,0) as bigint),0) as '消費手續費稅金'--,
		--ISNULL((DATA.sumLOADA-DATA.sumLOADRA),0) '加值淨額', 
		--Convert(FLOAT(50),ISNULL(DATA.L_CM_AM * 100,0)) as			'加值手續費率', 
		--ISNULL(cast(ROUND((DATA.sumLOADA-DATA.sumLOADRA)*DATA.L_CM_AM,0) as bigint),0) as '加值手續費',
		--ISNULL(cast(ROUND(cast(ROUND((DATA.sumLOADA-DATA.sumLOADRA)*DATA.L_CM_AM,0) as decimal(20,5))/1.05,0) as bigint),0) as '未稅',
		--ISNULL(cast(ROUND((DATA.sumLOADA-DATA.sumLOADRA)*DATA.L_CM_AM,0) as bigint)-cast(ROUND(cast(ROUND((DATA.sumLOADA-DATA.sumLOADRA)*DATA.L_CM_AM,0) as decimal(20,5))/1.05,0) as bigint),0) as '稅金',
		--ISNULL(DATA.sumLOAD_AMT,0)									'自動加值額',
		--Convert(FLOAT(50),ISNULL(DATA.A__AM * 100,0)) as			'自動加值手續費率', 
		--ISNULL(cast(ROUND(sumLOAD_AMT*DATA.A__AM,0) as bigint),0) as	'自動加值手續費',
		--ISNULL(cast(ROUND(cast(ROUND(sumLOAD_AMT*DATA.A__AM,0) as decimal(20,5))/1.05,0) as bigint),0) as '未稅',
		--ISNULL(cast(ROUND(sumLOAD_AMT*DATA.A__AM,0) as bigint)-cast(ROUND(cast(ROUND(sumLOAD_AMT*DATA.A__AM,0) as decimal(20,5))/1.05,0) as bigint),0) as '稅金'
		
from (
	SELECT MERCHANT_NO,MERCHANT_SUB_NO,SUM(sumPCHA) AS sumPCHA,SUM(sumPCHRA) AS sumPCHRA,P_CM_AM,SUM(sumLOADA) AS sumLOADA,SUM(sumLOADRA) AS sumLOADRA,L_CM_AM,SUM(sumLOAD_AMT) AS sumLOAD_AMT,A__AM FROM 
	(
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
			--ISNULL(sum(DATA1.sumUTUT3_AMTA),0) sumUTUT3_AMTA,
			--ISNULL(sum(DATA1.sumUTUT3_AMTRA),0) sumUTUT3_AMTRA,
			--(select C.FEE_RATE from GM_CONTRACT_D C where C.MERCHANT_NO = DATA1.MERCHANT_NO
			--										  and DATA1.CPT_DATE between C.EFF_DATE_FROM and C.EFF_DATE_TO
			--										  and C.SETTLE_TYPE = 'P' and C.FEE_KIND = 'UT' and FEE_CAL_FLG = 'AM'
			--										  and @EXEC_CPT_DATE_B between C.EFF_DATE_FROM and C.EFF_DATE_TO) P_UT_AM,
			ISNULL(sum(LOAD74_AMT),0) + ISNULL(sum(LOAD75_AMT),0) + ISNULL(sum(LOAD77_AMT),0) sumLOAD_AMT,  --(add by 20160415 rita 新增 77離線自動加值)
			(select C.FEE_RATE from GM_CONTRACT_D C where C.MERCHANT_NO = DATA1.MERCHANT_NO
													  and DATA1.CPT_DATE between C.EFF_DATE_FROM and C.EFF_DATE_TO
													  and C.SETTLE_TYPE = 'A' and FEE_CAL_FLG = 'AM'
													  and @EXEC_CPT_DATE_B between C.EFF_DATE_FROM and C.EFF_DATE_TO) A__AM--,
			--ISNULL(sum(LOAD79_AMT),0) sumLOAD79_AMT,
			--(select C.FEE_RATE from GM_CONTRACT_D C where C.MERCHANT_NO = DATA1.MERCHANT_NO
			--										  and DATA1.CPT_DATE between C.EFF_DATE_FROM and C.EFF_DATE_TO
			--										  and C.SETTLE_TYPE = 'B' and FEE_CAL_FLG = 'AM'
			--										  and @EXEC_CPT_DATE_B between C.EFF_DATE_FROM and C.EFF_DATE_TO) B__AM
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
	) DATA0 
	GROUP BY DATA0.MERCHANT_NO, DATA0.MERCHANT_SUB_NO,P_CM_AM,L_CM_AM,A__AM
)DATA
right join GM_MERCHANT M1 on DATA.MERCHANT_NO = M1.MERCHANT_NO
							and M1.MERCHANT_NO = case when @EXEC_MERCHANT_NO = 'ALL' then M1.MERCHANT_NO else @EXEC_MERCHANT_NO end 
left join GM_MERCHANT_SUB M2 on DATA.MERCHANT_SUB_NO = M2.MERCHANT_SUB_NO
WHERE M1.MERCHANT_NO = @EXEC_MERCHANT_NO
--WHERE DATA.MERCHANT_SUB_NO IS NULL OR (DATA.MERCHANT_SUB_NO<>'IBN' AND DATA.MERCHANT_SUB_NO<>'POS') 
order by M1.MERCHANT_NO--, DATA.CPT_DATE





INSERT INTO @TmpSum
SELECT 2,
	G.MERCHANT_NO
    ,G.MERCHANT_NAME+'-零值'
	,G.INVOICE_NO
	,0
	,''
	,ISNULL(SUM(cnt),0) as CNT
	,ISNULL((SELECT FEE_RATE FROM GM_CONTRACT_TR_D WHERE MERCHANT_NO = @EXEC_MERCHANT_NO AND CONTRACT_TYPE = 'P1' AND FEE_CAL_FLG = 'CT'),0) as FEE_RATE
	,ISNULL(cast(ROUND(sum(cnt) * FEE_RATE,0) as bigint),0) as fee
	,ISNULL(cast(ROUND(cast(ROUND(sum(cnt) * FEE_RATE,0) as decimal(20,5))/1.05,0) as bigint),0) as fee_tax_excluded
	,ISNULL(cast(ROUND(sum(cnt) * FEE_RATE,0) as bigint)-cast(ROUND(cast(ROUND(sum(cnt) * FEE_RATE,0) as decimal(20,5))/1.05,0) as bigint),0) as fee_tax
FROM (
SELECT V.MERCHANT_NO, GM.MERCHANT_NAME,GM.INVOICE_NO,V.TRANS_TYPE, ISNULL(COUNT(1),0) as cnt
FROM icashbatchdb.itrans.{0}.TM_SETTLE_DATA_D V
INNER JOIN icashbatchdb.itrans.{0}.BM_STORE_M BM ON V.STORE_NO = BM.STORE_NO
	AND V.MERCHANT_NO = BM.MERCHANT_NO
	AND BM.LINE_NO_04 = '03'	
inner join GM_MERCHANT GM on V.MERCHANT_NO=GM.MERCHANT_NO
WHERE   V.CPT_DATE BETWEEN @EXEC_CPT_DATE_B AND @EXEC_CPT_DATE_E
  AND V.TR_METHOD = '11'
  AND V.TR_KIND = '00'
  AND V.FREE_AMT <> '0'
  AND V.MERCHANT_NO = @EXEC_MERCHANT_NO
  GROUP BY V.MERCHANT_NO,GM.MERCHANT_NAME,V.TRANS_TYPE,INVOICE_NO
UNION ALL
SELECT V.MERCHANT_NO, GM.MERCHANT_NAME,GM.INVOICE_NO,V.TRANS_TYPE,ISNULL(COUNT(1),0) as cnt
FROM icashbatchdb.itrans.{0}.TM_SETTLE_DATA_D V
INNER JOIN icashbatchdb.itrans.{0}.BM_STORE_M BM ON V.STORE_NO = BM.STORE_NO 
    AND V.MERCHANT_NO = BM.MERCHANT_NO
	AND BM.LINE_NO_04 = '03'	
inner join GM_MERCHANT GM on V.MERCHANT_NO=GM.MERCHANT_NO
WHERE V.CPT_DATE BETWEEN @EXEC_CPT_DATE_B AND @EXEC_CPT_DATE_E
  AND V.TR_METHOD = '11'
  AND V.TR_KIND = '00'
  AND V.TRANS_DISC_AMT <> '0' 
  AND V.TRANSFER_GROUP_CODE1 + V.TRANSFER_GROUP_CODE2 IN ('3233','3333','1233','0833')
  AND V.MERCHANT_NO = @EXEC_MERCHANT_NO
GROUP BY V.MERCHANT_NO,GM.MERCHANT_NAME,V.TRANS_TYPE,INVOICE_NO
	) A	
RIGHT JOIN GM_MERCHANT G ON G.MERCHANT_NO = A.MERCHANT_NO 
LEFT join icashbatchdb.itrans.{0}.BM_TRANS_TYPE T    on A.TRANS_TYPE = T.TRANS_TYPE 
LEFT join icashbatchdb.itrans.dbo.GM_CONTRACT_TR_D C  on T.CONTRACT_TYPE = C.CONTRACT_TYPE and A.MERCHANT_NO = C.MERCHANT_NO and C.ISZERO = 'Y'
WHERE G.MERCHANT_NO = @EXEC_MERCHANT_NO
GROUP BY G.MERCHANT_NO,G.MERCHANT_NAME,G.INVOICE_NO,C.FEE_RATE


--SELECT * ,CONVERT(bigint,%%physloc%%) FROM @TmpSum 
--insert into @TmpSum 
--SELECT 3, @EXEC_MERCHANT_NO,'小計','',ISNULL(SUM(PUR_NET),0),'',ISNULL(SUM(CNT),0),null,ISNULL(SUM(FEE),0),ISNULL(SUM(FEE_TAX_EXCLUDED),0),ISNULL(SUM(FEE_TAX),0) FROM @TmpSum
--20210421 手續費改為未稅加總再乘0.05+上未稅
insert into @TmpSum 
SELECT 3, @EXEC_MERCHANT_NO,'小計','',ISNULL(SUM(PUR_NET),0),'',ISNULL(SUM(CNT),0),null,
ISNULL(SUM(FEE_TAX_EXCLUDED) + CONVERT(BIGINT,ROUND(ISNULL(SUM(FEE_TAX_EXCLUDED),0)*0.05,0)) ,0),
ISNULL(SUM(FEE_TAX_EXCLUDED),0),
ISNULL(CONVERT(BIGINT,ROUND(ISNULL(SUM(FEE_TAX_EXCLUDED),0) * 0.05,0) ),0)
FROM @TmpSum


SELECT * ,'','','','','','','','','',''  FROM @TmpSum ORDER BY ID
--order by CONVERT(bigint,%%physloc%%) 
--union ALL


--select MERCHANT_NAME+'零值' , INVOICE_NO,'' AS '購貨淨額','' AS '重複' ,CNT ,FEE_RATE ,fee ,fee_tax_excluded ,fee_tax 
--from @TmpSum
--order by MERCHANT_NO
