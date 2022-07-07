DECLARE @MERC_TYPE VARCHAR(10);
DECLARE @SERVER VARCHAR(30);
DECLARE @GROUP_ID VARCHAR(20);
SELECT @MERC_TYPE = MERCHANT_TYPE
FROM GM_MERCHANT
WHERE MERCHANT_NO = '{1}'

SELECT @SERVER = SERVER_NAME
FROM GM_COMMON_M
WHERE MERCHANT_NO = '{1}' AND TYPE_GROUP='TRANS_DESC1'
--��O�_���i�X�����A��
SELECT @GROUP_ID = GROUP_ID FROM GM_MERCHANT_TYPE_D
WHERE MERCHANT_NO = '{1}'

IF @MERC_TYPE = 'MUTI_MERC'
BEGIN
SELECT 'SELECT	''{2}'' AS MERCHANT_NAME,
		a.CPT_DATE,
		a.STORE_NO,		
		s.STO_NAME_SHORT,	
		a.ICC_NO, 
		a.REG_ID,		
        a.trans_date_txlog,	'+SELECTED_COLUMNS+
        ' AS TRANS_DESC,a.TRANS_AMT,
		a.DIFF_FLG,		
        d.DIFF_DESC,
		''SKIP_TM'' AS TABLE_NAME		
FROM '+ 
CASE SERVER_NAME WHEN '' THEN '{0}.TM_SKIP_TMLOG_D a ' ELSE SERVER_NAME+'.{0}.TM_SKIP_TMLOG_D a ' END 
+'left join '+CASE SERVER_NAME WHEN '' THEN '{0}.BM_DIFF_M d ' ELSE SERVER_NAME+'.{0}.BM_DIFF_M d ' END
+'on a.DIFF_FLG = d.DIFF_FLG '
+'left join '+CASE SERVER_NAME WHEN '' THEN '{0}.BM_STORE_M s ' ELSE SERVER_NAME+'.{0}.BM_STORE_M s ' END
+'on a.STORE_NO=s.STORE_NO AND a.MERCHANT_NO = s.MERCHANT_NO, '
+CASE SERVER_NAME WHEN '' THEN '{0}.BM_TRANS_TYPE b ' ELSE SERVER_NAME+'.{0}.BM_TRANS_TYPE b ' END
+'WHERE 
a.trans_type=b.trans_type
and b.MERCHANT_NO = ''{1}'' and a.MERCHANT_NO = ''{1}'' and s.MERCHANT_NO =  ''{1}''
and b.SETTLE_FLG=''1'' AND a.TRANS_DATE_TXLOG like ''{4}%''
and a.ICC_NO = ''{3}''
ORDER BY 1,2,3,4,5'
        AS SQL FROM GM_COMMON_M WHERE MERCHANT_NO='{1}' AND TYPE_GROUP='TRANS_DESC1'
END
ELSE IF '{0}' IN ('UBS','T02')
BEGIN
SELECT 'SELECT	''{2}'' AS MERCHANT_NAME
		,a.CPT_DATE,
		a.ROUTE_NO AS STORE_NO,		
		s.STO_NAME_SHORT,
		a.ICC_NO,  
		'''' REG_ID,		
        a.trans_date AS TRANS_DATE_TXLOG,	'+SELECTED_COLUMNS+
        ' AS TRANS_DESC ,a.TRANS_AMT,		
        a.DIFF_FLG,		
        d.DIFF_DESC,
		''SKIP_TM'' AS TABLE_NAME		
FROM '+ 
CASE SERVER_NAME WHEN '' THEN '{0}.TM_SKIP_TR_TMLOG_D a ' ELSE SERVER_NAME+'.{0}.TM_SKIP_TR_TMLOG_D a ' END 
+'left join '+CASE SERVER_NAME WHEN '' THEN '{0}.BM_DIFF_M d ' ELSE SERVER_NAME+'.{0}.BM_DIFF_M d ' END
+'on a.DIFF_FLG = d.DIFF_FLG '
+'left join '+CASE SERVER_NAME WHEN '' THEN '{0}.BM_STORE_M s ' ELSE SERVER_NAME+'.{0}.BM_STORE_M s ' END
+'on a.ROUTE_NO=s.STORE_NO, '
+CASE SERVER_NAME WHEN '' THEN '{0}.BM_TRANS_TYPE b ' ELSE SERVER_NAME+'.{0}.BM_TRANS_TYPE b ' END
+'WHERE 
a.trans_type=b.trans_type
and b.SETTLE_FLG=''1'' AND a.TRANS_DATE_TXLOG like ''{4}%''
and a.ICC_NO = ''{3}''
ORDER BY 1,2,3,4,5'
        AS SQL FROM GM_COMMON_M WHERE MERCHANT_NO='{1}' AND TYPE_GROUP='TRANS_DESC1'
END
ELSE IF '{0}' = 'KML'
BEGIN
SELECT 'SELECT	''{2}'' AS MERCHANT_NAME,
		a.CPT_DATE,
		a.ROUTE_NO AS STORE_NO,		
		s.STO_NAME_SHORT,	 
		a.ICC_NO, 
		'''' REG_ID,		
        a.trans_date AS TRANS_DATE_TXLOG,	'+SELECTED_COLUMNS+
        ' AS TRANS_DESC,a.TRANS_AMT,		
        a.DIFF_FLG,		
        d.DIFF_DESC,
		''SKIP_TM'' AS TABLE_NAME		
FROM '+ 
CASE SERVER_NAME WHEN '' THEN '{0}.TM_SKIP_TR_TMLOG_D a ' ELSE SERVER_NAME+'.{0}.TM_SKIP_TR_TMLOG_D a ' END 
+'left join '+CASE SERVER_NAME WHEN '' THEN '{0}.BM_DIFF_M d ' ELSE SERVER_NAME+'.{0}.BM_DIFF_M d ' END
+'on a.DIFF_FLG = d.DIFF_FLG '
+'left join '+CASE SERVER_NAME WHEN '' THEN '{0}.BM_STORE_M s ' ELSE SERVER_NAME+'.{0}.BM_STORE_M s ' END
+'on a.ROUTE_NO=s.STORE_NO, '
+CASE SERVER_NAME WHEN '' THEN '{0}.BM_TRANS_TYPE b ' ELSE SERVER_NAME+'.{0}.BM_TRANS_TYPE b ' END
+'WHERE 
a.trans_type=b.trans_type
and b.SETTLE_FLG=''1'' AND a.TRANS_DATE_TXLOG like ''{4}%''
and a.ICC_NO = ''{3}''
UNION ALL ' +
'
SELECT	''{2}'' AS MERCHANT_NAME,
		a.CPT_DATE,
		a.STORE_NO,		
		s.STO_NAME_SHORT,	
		a.ICC_NO,  
		a.REG_ID,		
        a.trans_date_txlog,	'+SELECTED_COLUMNS+
        ' AS TRANS_DESC,a.TRANS_AMT,		
        a.DIFF_FLG,		
        d.DIFF_DESC,
		''SKIP_TM'' AS TABLE_NAME		
FROM '+ 
CASE SERVER_NAME WHEN '' THEN '{0}.TM_SKIP_TMLOG_D a ' ELSE SERVER_NAME+'.{0}.TM_SKIP_TMLOG_D a ' END 
+'left join '+CASE SERVER_NAME WHEN '' THEN '{0}.BM_DIFF_M d ' ELSE SERVER_NAME+'.{0}.BM_DIFF_M d ' END
+'on a.DIFF_FLG = d.DIFF_FLG '
+'left join '+CASE SERVER_NAME WHEN '' THEN '{0}.BM_STORE_M s ' ELSE SERVER_NAME+'.{0}.BM_STORE_M s ' END
+'on a.STORE_NO=s.STORE_NO, '
+CASE SERVER_NAME WHEN '' THEN '{0}.BM_TRANS_TYPE b ' ELSE SERVER_NAME+'.{0}.BM_TRANS_TYPE b ' END
+'WHERE 
a.trans_type=b.trans_type
and b.SETTLE_FLG=''1'' AND a.TRANS_DATE_TXLOG like ''{4}%''
and a.ICC_NO = ''{3}''
ORDER BY 1,2,3,4,5'
        AS SQL FROM GM_COMMON_M WHERE MERCHANT_NO='{1}' AND TYPE_GROUP='TRANS_DESC1'

END
--RCP�]�O�i�X���Ҧ�
ELSE IF @GROUP_ID = 'TRACK' OR '{0}' = 'RCP'
BEGIN
SELECT 'SELECT	''{2}'' AS MERCHANT_NAME,
		a.CPT_DATE,
		a.STORE_NO,		
		s.STO_NAME_SHORT,
		a.ICC_NO,	 
		a.REG_ID,		
        a.trans_date_txlog,	'+SELECTED_COLUMNS+
        ' AS TRANS_DESC,a.TRANS_AMT,		
         a.DIFF_FLG,		
        d.DIFF_DESC,
		''SKIP_TM'' AS TABLE_NAME	
FROM '+ 
CASE SERVER_NAME WHEN '' THEN '{0}.TM_SKIP_TMLOG_D a ' ELSE SERVER_NAME+'.{0}.TM_SKIP_TMLOG_D a ' END 
+'left join '+CASE SERVER_NAME WHEN '' THEN '{0}.BM_DIFF_M d ' ELSE SERVER_NAME+'.{0}.BM_DIFF_M d ' END
+'on a.DIFF_FLG = d.DIFF_FLG '
+'left join '+CASE SERVER_NAME WHEN '' THEN '{0}.BM_STORE_M s ' ELSE SERVER_NAME+'.{0}.BM_STORE_M s ' END
+'on a.STORE_NO=s.STORE_NO, '
+CASE SERVER_NAME WHEN '' THEN '{0}.BM_TRANS_TYPE b ' ELSE SERVER_NAME+'.{0}.BM_TRANS_TYPE b ' END
+'WHERE
a.FILE_TRANS_TYPE+a.FILE_SUB_TYPE =b.trans_type
and b.SETTLE_FLG=''1'' AND a.TRANS_DATE_TXLOG like ''{4}%''
and a.ICC_NO = ''{3}''
ORDER BY 1,2,3,4,5'
        AS SQL FROM GM_COMMON_M WHERE MERCHANT_NO='{1}' AND TYPE_GROUP='TRANS_DESC1'
END
ELSE
BEGIN
SELECT 'SELECT	''{2}'' AS MERCHANT_NAME,
		a.CPT_DATE,
		a.STORE_NO,		
		s.STO_NAME_SHORT,
		a.ICC_NO,	 
		a.REG_ID,		
        a.trans_date_txlog,	'+SELECTED_COLUMNS+
        ' AS TRANS_DESC,a.TRANS_AMT,		
        a.DIFF_FLG,		
        d.DIFF_DESC,
		''SKIP_TM'' AS TABLE_NAME		
FROM '+ 
CASE SERVER_NAME WHEN '' THEN '{0}.TM_SKIP_TMLOG_D a ' ELSE SERVER_NAME+'.{0}.TM_SKIP_TMLOG_D a ' END 
+'left join '+CASE SERVER_NAME WHEN '' THEN '{0}.BM_DIFF_M d ' ELSE SERVER_NAME+'.{0}.BM_DIFF_M d ' END
+'on a.DIFF_FLG = d.DIFF_FLG '
+'left join '+CASE SERVER_NAME WHEN '' THEN '{0}.BM_STORE_M s ' ELSE SERVER_NAME+'.{0}.BM_STORE_M s ' END
+'on a.STORE_NO=s.STORE_NO, '
+CASE SERVER_NAME WHEN '' THEN '{0}.BM_TRANS_TYPE b ' ELSE SERVER_NAME+'.{0}.BM_TRANS_TYPE b ' END
+'WHERE 
a.trans_type=b.trans_type
and b.SETTLE_FLG=''1'' AND a.TRANS_DATE_TXLOG like ''{4}%''
and a.ICC_NO = ''{3}''
ORDER BY 1,2,3,4,5'
        AS SQL FROM GM_COMMON_M WHERE MERCHANT_NO='{1}' AND TYPE_GROUP='TRANS_DESC1'
END