DECLARE @EXEC_CPT_DATE VARCHAR(8) = '{1}'
DECLARE @EXEC_MERCHANT_NO VARCHAR(8) ='{2}'

DECLARE @SDATE VARCHAR(8)
DECLARE @EDATE VARCHAR(8)
SELECT @SDATE = GET_SUM_DATE.SDATE,
        @EDATE = GET_SUM_DATE.EDATE
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
MERCHANT_NO varchar(20), 
MERCHANT_NAME varchar(20),
CNT int,
FEE_RATE varchar(4),
fee int,
fee_tax_excluded int,
fee_tax int
)

INSERT INTO @TmpSum
SELECT A.MERCHANT_NO
    ,A.MERCHANT_NAME
	,sum(cnt) as CNT
	,convert(varchar,convert(float,FEE_RATE)) as FEE_RATE
	,ISNULL(cast(ROUND(sum(cnt) * FEE_RATE,0) as bigint),0) as fee
	,ISNULL(cast(ROUND(cast(ROUND(sum(cnt) * FEE_RATE,0) as decimal(20,5))/1.05,0) as bigint),0) as fee_tax_excluded
	,ISNULL(cast(ROUND(sum(cnt) * FEE_RATE,0) as bigint)-cast(ROUND(cast(ROUND(sum(cnt) * FEE_RATE,0) as decimal(20,5))/1.05,0) as bigint),0) as fee_tax
FROM (
SELECT V.MERCHANT_NO, GM.MERCHANT_NAME,V.TRANS_TYPE, COUNT(1) as cnt
FROM icashbatchdb.itrans.{0}.TM_SETTLE_DATA_D V
INNER JOIN icashbatchdb.itrans.{0}.BM_STORE_M BM ON V.STORE_NO = BM.STORE_NO
	AND V.MERCHANT_NO = BM.MERCHANT_NO
	AND BM.LINE_NO_04 = '03'	
inner join GM_MERCHANT GM on V.MERCHANT_NO=GM.MERCHANT_NO
WHERE  V.CPT_DATE BETWEEN @SDATE AND @EDATE
  AND V.TR_METHOD = '11'
  AND V.TR_KIND = '00'
  AND V.FREE_AMT <> '0'
  GROUP BY V.MERCHANT_NO,GM.MERCHANT_NAME,V.TRANS_TYPE
UNION ALL
SELECT V.MERCHANT_NO, GM.MERCHANT_NAME,V.TRANS_TYPE, COUNT(1) as cnt
FROM icashbatchdb.itrans.{0}.TM_SETTLE_DATA_D V
INNER JOIN icashbatchdb.itrans.{0}.BM_STORE_M BM ON V.STORE_NO = BM.STORE_NO 
    AND V.MERCHANT_NO = BM.MERCHANT_NO
	AND BM.LINE_NO_04 = '03'	
inner join GM_MERCHANT GM on V.MERCHANT_NO=GM.MERCHANT_NO
WHERE V.CPT_DATE BETWEEN @SDATE AND @EDATE
  AND V.TR_METHOD = '11'
  AND V.TR_KIND = '00'
  AND V.TRANS_DISC_AMT <> '0' 
  AND V.TRANSFER_GROUP_CODE1 + V.TRANSFER_GROUP_CODE2 IN ('3233','3333','1233','0833')
GROUP BY V.MERCHANT_NO,GM.MERCHANT_NAME,V.TRANS_TYPE
	) A	
inner join icashbatchdb.itrans.{0}.BM_TRANS_TYPE T    on A.TRANS_TYPE = T.TRANS_TYPE 
inner join icashbatchdb.itrans.dbo.GM_CONTRACT_TR_D C  on T.CONTRACT_TYPE = C.CONTRACT_TYPE and A.MERCHANT_NO = C.MERCHANT_NO and C.ISZERO = 'Y'
GROUP BY A.MERCHANT_NO,A.MERCHANT_NAME,C.FEE_RATE
ORDER BY A.MERCHANT_NO,A.MERCHANT_NAME,C.FEE_RATE

select MERCHANT_NAME ,MERCHANT_NO, CNT ,FEE_RATE ,fee ,fee_tax_excluded ,fee_tax 
from @TmpSum
order by MERCHANT_NO

