SELECT
(
select 	
	ISNULL(SUM(TRANS_AMT),0) AS SUM_AMT
from dbo.GM_BANK_AL_SETTLE_D_VIEW
where CPT_DATE between @EXEC_CPT_DATE_B and @EXEC_CPT_DATE_E
and MERCHANT_NO = '22555003'
)
+
(
select  ISNULL(SUM(TRANS_AMT),0) AS IBON_E_AMT
from IBN.TM_SETTLE_DATA_D
where CPT_DATE between @EXEC_CPT_DATE_B and @EXEC_CPT_DATE_E
and TRANS_TYPE in ('79')
)
--20180102  �s�WISNULL�P�_