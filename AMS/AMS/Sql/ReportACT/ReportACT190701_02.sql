
--DECLARE @SDATE VARCHAR(8) = '20180401'
--DECLARE @EDATE VARCHAR(8) = '20180430'


SELECT CPT_DATE,S.MERCHANT_NO_ACT, T.STORE_NO,STO_NAME_LONG,SUM(PCH_AMT) AS PCH_AMT ,SUM(PCHR_AMT)*-1 AS PCHR_AMT,SUM(PCH_AMT)-SUM(PCHR_AMT) NET_AMT FROM 
(
SELECT CPT_DATE, a.MERCHANT_NO, a.STORE_NO,b.STO_NAME_LONG,ISNULL(CASE WHEN TR_TRANS_TYPE = '21' THEN SUM(TRANS_AMT) END,0) AS PCH_AMT,
ISNULL(CASE WHEN TR_TRANS_TYPE = '23' THEN SUM(TRANS_AMT) END,0) AS PCHR_AMT
from BRN.TM_SETTLE_DATA_D a 
left join BRN.BM_STORE_M b
on a.STORE_NO = b.STORE_NO
join BRN.BM_TRANS_TYPE c
on a.TRANS_TYPE = c.TRANS_TYPE
where c.TR_TRANS_TYPE in ('21','23')
and CPT_DATE between @SDATE and @EDATE
group by CPT_DATE, a.MERCHANT_NO,a.STORE_NO,b.STO_NAME_LONG,TR_TRANS_TYPE
) T
LEFT JOIN BRN.BM_STORE_MERCHANT S
ON T.STORE_NO = S.STORE_NO
and t.MERCHANT_NO = s.MERCHANT_NO
GROUP BY CPT_DATE, S.MERCHANT_NO_ACT,T.STORE_NO,STO_NAME_LONG
ORDER BY 3,1
