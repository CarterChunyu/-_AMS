DECLARE @MERC_TYPE VARCHAR(10);
SELECT @MERC_TYPE = MERCHANT_TYPE
FROM GM_MERCHANT
WHERE MERCHANT_NO = '{1}'

IF @MERC_TYPE = 'MUTI_MERC'
BEGIN
SELECT 'SELECT  ''{2}'','+SELECTED_COLUMNS+',COUNT(*) AS COUNT,SUM(TRANS_AMT) AS SUM,b.TRANS_TYPE AS TRANS_TYPE FROM '
+CASE SERVER_NAME WHEN '' THEN '{0}.TM_SETTLE_DATA_D a ' ELSE SERVER_NAME+'.{0}.TM_SETTLE_DATA_D a ' END
+' INNER JOIN '+CASE SERVER_NAME WHEN '' THEN '{0}.BM_TRANS_TYPE b ' ELSE SERVER_NAME+'.{0}.BM_TRANS_TYPE b ' END
+' ON a.TRANS_TYPE=b.TRANS_TYPE AND a.MERCHANT_NO = B.MERCHANT_NO WHERE CPT_DATE=@settleDate and A.MERCHANT_NO = ''{1}'' GROUP BY b.TRANS_TYPE,'+
SELECTED_COLUMNS AS SQL FROM GM_COMMON_M 
WHERE MERCHANT_NO='{1}' AND TYPE_GROUP='TRANS_DESC1'
END 
ELSE 
BEGIN 
SELECT 'SELECT  ''{2}'','+SELECTED_COLUMNS+',COUNT(*) AS COUNT,SUM(TRANS_AMT) AS SUM,b.TRANS_TYPE AS TRANS_TYPE FROM '
+CASE SERVER_NAME WHEN '' THEN '{0}.TM_SETTLE_DATA_D a ' ELSE SERVER_NAME+'.{0}.TM_SETTLE_DATA_D a ' END
+' INNER JOIN '+CASE SERVER_NAME WHEN '' THEN '{0}.BM_TRANS_TYPE b ' ELSE SERVER_NAME+'.{0}.BM_TRANS_TYPE b ' END
+' ON a.TRANS_TYPE=b.TRANS_TYPE WHERE CPT_DATE=@settleDate GROUP BY b.TRANS_TYPE,'+
SELECTED_COLUMNS AS SQL FROM GM_COMMON_M 
WHERE MERCHANT_NO='{1}' AND TYPE_GROUP='TRANS_DESC1'
END 