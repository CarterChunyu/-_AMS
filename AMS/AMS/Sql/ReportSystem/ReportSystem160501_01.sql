SELECT 'SELECT M.CPT_DATE,M.MERCHANT_NO,M.MERCHANT_STNAME,CASE WHEN N.NUM>=1 THEN ''清分成功'' ELSE ''清分失敗'' END AS SUCCESS FROM 
(SELECT A.CPT_DATE, A.MERCHANT_NO, B.MERCHANT_STNAME
FROM '+CASE SERVER_NAME WHEN '' THEN '{0}.CO_JOB_CTRL A ' ELSE SERVER_NAME+'.{0}.CO_JOB_CTRL A ' END
+'
inner join GM_MERCHANT B on A.MERCHANT_NO = B.MERCHANT_NO
where CPT_DATE between ''{1}'' and ''{2}'' and A.MERCHANT_NO =''{3}''
group by A.MERCHANT_NO, B.MERCHANT_STNAME, A.CPT_DATE) M 
LEFT OUTER JOIN  
(SELECT A.CPT_DATE, A.MERCHANT_NO, B.MERCHANT_STNAME,count(1) AS NUM 
FROM '+CASE SERVER_NAME WHEN '' THEN '{0}.CO_JOB_CTRL A ' ELSE SERVER_NAME+'.{0}.CO_JOB_CTRL A ' END
+'
inner join GM_MERCHANT B on A.MERCHANT_NO = B.MERCHANT_NO
where CPT_DATE between ''{1}'' and ''{2}'' AND JOB_KIND IN (''DAILY_JOB_END'',''SU_DAILYJOB_END'',''DAILY_JOB_END_MUTI_MERC'') 
group by A.MERCHANT_NO, B.MERCHANT_STNAME, A.CPT_DATE) N
ON M.CPT_DATE=N.CPT_DATE AND M.MERCHANT_NO=N.MERCHANT_NO AND M.MERCHANT_STNAME=N.MERCHANT_STNAME' AS SQLS