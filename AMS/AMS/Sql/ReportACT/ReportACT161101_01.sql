SELECT 特約機構,交易代號,SUM(交易金額) AS SUM,'重複認列' AS '帳務認列', '重複剔退' AS '卡務認列'
FROM
(

select  GM_MERCHANT.MERCHANT_STNAME AS '特約機構',GM_TRANS_TYPE.TRANS_DESC AS '交易代號',CAST(TRANS_AMT AS BIGINT) AS '交易金額'
 from IM_ISET_TXLOG_T 
 INNER JOIN  
 GM_TRANS_TYPE 

 ON IM_ISET_TXLOG_T.TRANS_TYPE=GM_TRANS_TYPE.TRANS_TYPE
 INNER JOIN GM_MERCHANT
 ON  IM_ISET_TXLOG_T.MERCHANT_NO=GM_MERCHANT.MERCHANT_NO

 WHERE ((substring(ZIP_FILE_NAME,1,6)='TMLOG_' and substring(ZIP_FILE_NAME,7,8) BETWEEN @firstDay AND @lastDay) OR (substring(ZIP_FILE_NAME,1,6)='TXLOG_' and substring(ZIP_FILE_NAME,7,8) BETWEEN @firstDay AND @lastDay) OR (substring(ZIP_FILE_NAME,1,8)='ACICICCG' and (substring(SETTLE_DATE,1,4)+substring(ZIP_FILE_NAME,9,4) BETWEEN LEFT(convert(varchar, dateadd(d,-1,cast(@firstDay as date)) ,112),8) AND LEFT(convert(varchar, dateadd(d,-1,cast(@lastDay as date)) ,112),8))))  
and CM_TAG='DU' and IM_ISET_TXLOG_T.TRANS_TYPE in ('21','22','23','24','74','75','77' ) AND IM_ISET_TXLOG_T.MERCHANT_NO=CASE WHEN @merchant='' THEN IM_ISET_TXLOG_T.MERCHANT_NO ELSE @merchant END

) x GROUP BY x.特約機構,x.交易代號