SELECT '�Τ@�W��' AS MERCHANT_STNAME,CONVERT(BIGINT,SUM(ISNULL([21],0)-ISNULL([23],0))) AS NET,0.0042 AS FEE_RATE,@newRate AS NEW_RATE,CONVERT(BIGINT,ROUND(SUM(ISNULL([21],0)-ISNULL([23],0))*0.0042,0)) AS CHARGE,CONVERT(BIGINT,ROUND(SUM(ISNULL([21],0)-ISNULL([23],0))*@newRATE,0)) AS NEW_CHARGE,CONVERT(BIGINT,ROUND(SUM(ISNULL([21],0)-ISNULL([23],0))*0.0042,0))-CONVERT(BIGINT,ROUND(SUM(ISNULL([21],0)-ISNULL([23],0))*@newRATE,0)) AS DIFF,CONVERT(BIGINT,ROUND((CONVERT(BIGINT,ROUND(SUM(ISNULL([21],0)-ISNULL([23],0))*0.0042,0))-CONVERT(BIGINT,ROUND(SUM(ISNULL([21],0)-ISNULL([23],0))*@newRATE,0)))/1.05,0)) AS UNTAXED,CONVERT(BIGINT,ROUND(SUM(ISNULL([21],0)-ISNULL([23],0))*0.0042,0))-CONVERT(BIGINT,ROUND(SUM(ISNULL([21],0)-ISNULL([23],0))*@newRATE,0))-CONVERT(BIGINT,ROUND((CONVERT(BIGINT,ROUND(SUM(ISNULL([21],0)-ISNULL([23],0))*0.0042,0))-CONVERT(BIGINT,ROUND(SUM(ISNULL([21],0)-ISNULL([23],0))*@newRATE,0)))/1.05,0)) AS TAX
FROM
(
SELECT *
from AM_ISET_MERC_TRANS_UTLOG_D WHERE SRC_FLG='POS' AND MERCHANT_NO='22555003' AND CPT_DATE BETWEEN @yearMonth+'02' AND REPLACE(DATEADD(mm,1,CAST(@yearMonth+'01' AS DATE)),'-','') 
) p 
PIVOT 
(SUM(CM_AMT) FOR TRANS_TYPE IN ([21],[23])
) pt
GROUP BY MERCHANT_NO UNION ALL 