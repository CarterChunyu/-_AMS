SELECT �S�����c,����N��,SUM(������B) AS SUM,'���ƻ{�C' AS '�b�Ȼ{�C', '���ƭ�h' AS '�d�Ȼ{�C'
FROM
(

select  GM_MERCHANT.MERCHANT_STNAME AS '�S�����c',GM_TRANS_TYPE.TRANS_DESC AS '����N��',CAST(TRANS_AMT AS BIGINT) AS '������B'
 from IM_ISET_TXLOG_T 
 INNER JOIN  
 GM_TRANS_TYPE 

 ON IM_ISET_TXLOG_T.TRANS_TYPE=GM_TRANS_TYPE.TRANS_TYPE
 INNER JOIN GM_MERCHANT
 ON  IM_ISET_TXLOG_T.MERCHANT_NO=GM_MERCHANT.MERCHANT_NO

 WHERE ((substring(ZIP_FILE_NAME,1,6)='TMLOG_' and substring(ZIP_FILE_NAME,7,8) BETWEEN @firstDay AND @lastDay) OR (substring(ZIP_FILE_NAME,1,6)='TXLOG_' and substring(ZIP_FILE_NAME,7,8) BETWEEN @firstDay AND @lastDay) OR (substring(ZIP_FILE_NAME,1,8)='ACICICCG' and (substring(SETTLE_DATE,1,4)+substring(ZIP_FILE_NAME,9,4) BETWEEN LEFT(convert(varchar, dateadd(d,-1,cast(@firstDay as date)) ,112),8) AND LEFT(convert(varchar, dateadd(d,-1,cast(@lastDay as date)) ,112),8))))  
and CM_TAG='DU' and IM_ISET_TXLOG_T.TRANS_TYPE in ('21','22','23','24','74','75','77' ) AND IM_ISET_TXLOG_T.MERCHANT_NO=CASE WHEN @merchant='' THEN IM_ISET_TXLOG_T.MERCHANT_NO ELSE @merchant END

) x GROUP BY x.�S�����c,x.����N��