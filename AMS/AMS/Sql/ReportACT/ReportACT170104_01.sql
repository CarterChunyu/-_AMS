select MERCHANT_NO,CPT_DATE,TRANS_SUM_DATE,CM_AMT,CM_CNT,CMR_AMT,CMR_CNT,NULL AS P_RATE,LOAD_AMT,LOAD_CNT,LOADR_AMT,LOADR_CNT,NULL AS L_RATE from KRT.TM_TRANS_SUM_D
where CPT_DATE between @sDate and @eDate
order by CPT_DATE, TRANS_SUM_DATE