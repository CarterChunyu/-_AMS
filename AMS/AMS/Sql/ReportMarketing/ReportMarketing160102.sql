SELECT TRANS_DATE, B.BANK_NAME, G.MERCHANT_STNAME STNAME, SUM(TRANS_AMT) sumTRANS_AMT, SUM(TRANS_CNT) sumTRANS_CNT
FROM GM_CPT_CARD_SUM_D A 
LEFT OUTER JOIN CM_BANK_D B ON A.CA_DPT = B.CA_DPT
LEFT OUTER JOIN GM_MERCHANT G ON A.MERCHANT_NO = G.MERCHANT_NO 							   				
WHERE CARD_KIND = '�p�W�d'
AND TRANS_TYPE = '21'
AND TRANS_DATE BETWEEN @TRANS_DATE_B AND @TRANS_DATEE_E
AND G.MERCHANT_NO = CASE WHEN @MERCHANT_NO = 'ALL' THEN G.MERCHANT_NO ELSE @MERCHANT_NO END
GROUP BY B.BANK_NAME, TRANS_DATE, G.MERCHANT_STNAME
ORDER BY TRANS_DATE