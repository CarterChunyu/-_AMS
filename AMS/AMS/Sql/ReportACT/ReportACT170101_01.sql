SELECT a.CPT_DATE,ISNULL(a.SUM_AMT,0),ISNULL(b.POS_AMT,0),ISNULL(c.IBON_AMT,0),ISNULL(d.IBON_E_AMT,0),
ISNULL(SUM_COUNT,0),ISNULL(POS_COUNT,0),ISNULL(IBON_COUNT,0),ISNULL(IBON_E_COUNT,0)
FROM 
(
select 	CPT_DATE, 
	SUM(TRANS_AMT) AS SUM_AMT,COUNT(1) AS SUM_COUNT
from dbo.GM_BANK_AL_SETTLE_D_VIEW
where CPT_DATE between @sDate and @eDate
and MERCHANT_NO = '22555003' 
group by CPT_DATE
) a LEFT OUTER JOIN 
(
select 	CPT_DATE, 
	SUM(TRANS_AMT) AS POS_AMT,COUNT(1) AS POS_COUNT
from dbo.GM_BANK_AL_SETTLE_D_VIEW
where CPT_DATE between @sDate and @eDate
and MERCHANT_NO = '22555003' 
and LEFT(REG_ID, 2)='00'
group by CPT_DATE, LEFT(REG_ID, 2)
) b
ON a.CPT_DATE=b.CPT_DATE
LEFT OUTER JOIN 
(
select 	CPT_DATE, 
	SUM(TRANS_AMT) AS IBON_AMT,COUNT(1) AS IBON_COUNT
from dbo.GM_BANK_AL_SETTLE_D_VIEW
where CPT_DATE between @sDate and @eDate
and MERCHANT_NO = '22555003' 
and LEFT(REG_ID, 2)='01'
group by CPT_DATE, LEFT(REG_ID, 2)
) c
ON a.CPT_DATE=c.CPT_DATE
LEFT OUTER JOIN 
(
select  SUM(TRANS_AMT) AS IBON_E_AMT,CPT_DATE ,COUNT(1) AS IBON_E_COUNT
from IBN.TM_SETTLE_DATA_D
where CPT_DATE between @sDate and @eDate
and TRANS_TYPE in ('79')
group by CPT_DATE
) d
ON a.CPT_DATE=d.CPT_DATE
ORDER BY a.CPT_DATE