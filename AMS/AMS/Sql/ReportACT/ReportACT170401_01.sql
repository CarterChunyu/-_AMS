SELECT y1.YM,y1.SUM1,y2.SUM2,y1.SUM1-y2.SUM2 AS SUM3,CAST((y1.SUM1-y2.SUM2)*(0.002) AS DECIMAL(16,2)) AS SUM4 FROM
(
SELECT @yearMonth AS YM,sum(HM)+sum(UT)+sum(UT3) AS SUM1 FROM 
(
select    
		   CASE WHEN TRANS_TYPE='21' THEN HM_AMT WHEN TRANS_TYPE='23' THEN (-1)*HM_AMT END AS HM,
		   CASE WHEN TRANS_TYPE='21' THEN UT_AMT WHEN TRANS_TYPE='23' THEN (-1)*UT_AMT END AS UT,
		   CASE WHEN TRANS_TYPE='21' THEN UT3_AMT WHEN TRANS_TYPE='23' THEN (-1)*UT3_AMT END AS UT3
	from AM_ISET_MERC_TRANS_UTLOG_D_BY_CARDTYPE
	where TRANS_TYPE in ('21','23') AND CPT_DATE BETWEEN @startDate AND @endDate
) x1
) y1
INNER JOIN
(
SELECT @yearMonth AS YM,sum(HM)+sum(UT)+sum(UT3) AS SUM2 FROM 
(
select    
		   CASE WHEN TRANS_TYPE='21' THEN HM_AMT WHEN TRANS_TYPE='23' THEN (-1)*HM_AMT END AS HM,
		   CASE WHEN TRANS_TYPE='21' THEN UT_AMT WHEN TRANS_TYPE='23' THEN (-1)*UT_AMT END AS UT,
		   CASE WHEN TRANS_TYPE='21' THEN UT3_AMT WHEN TRANS_TYPE='23' THEN (-1)*UT3_AMT END AS UT3
	from AM_ISET_MERC_TRANS_UTLOG_D_BY_CARDTYPE
	where TRANS_TYPE in ('21','23') AND CPT_DATE BETWEEN @startDate AND @endDate AND CARD_KIND='?p?W?d'
) x2
) y2
ON y1.YM=y2.YM