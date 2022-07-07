--	測試用
--declare @EXEC_MERCHANT_NO varchar(20) = 'A0000003';		--活動代號:ALL、單一代號
--declare @SDATE varchar(20) = '20180531';	--清分日期_開始
--declare @EDATE varchar(20) = '20180625';	--清分日期_結束
--	測試用
-----------------------------------------------------------------------------

declare @s_CPT_DATE varchar(20);	--查詢日期

DECLARE @TabeRpt TABLE (CPT_DATE varchar(8),
						MERCHANT_NO varchar(10),
						MERCHANT_NAME varchar(50),
						NET_AMT NUMERIC(20,5),
						FEE_RATE NUMERIC(15,5),
						FEE_AMT NUMERIC(20,5),
						RE_AMT NUMERIC(20,5));

set @s_CPT_DATE = @SDATE;

WHILE(@s_CPT_DATE <= @EDATE)
begin
	insert @TabeRpt
	SELECT CPT_DATE,A1.MERCHANT_NO,MERCHANT_NAME,SUM(NET_AMT),
	FEE_RATE * 100 as 'FEE_RATE',SUM(NET_AMT) * FEE_RATE AS 'FEE_AMT',SUM(NET_AMT) - SUM(NET_AMT) * FEE_RATE FROM
		(
		SELECT @s_CPT_DATE AS 'CPT_DATE',MERCHANT_NO,MERCHANT_NAME,0 AS 'NET_AMT' FROM GM_MERCHANT
		WHERE MERCHANT_NO = @EXEC_MERCHANT_NO
		UNION ALL
		SELECT CPT_DATE,A.MERCHANT_NO,MERCHANT_NAME,PCH_AMT-PCHR_AMT AS 'NET_AMT' FROM AM_ISET_MERC_TRANS_LOG_D A
		INNER JOIN GM_MERCHANT B
		ON A.MERCHANT_NO = B.MERCHANT_NO
		WHERE CPT_DATE = @s_CPT_DATE AND A.MERCHANT_NO = @EXEC_MERCHANT_NO
		) A1
	INNER JOIN GM_CONTRACT_MUTI_MERC_D B1
	ON A1.MERCHANT_NO = B1.MERCHANT_NO
	WHERE B1.CONTRACT_TYPE = 'P1' 
	AND CPT_DATE BETWEEN B1.EFF_DATE_FROM AND B1.EFF_DATE_TO
	AND B1.FEE_KIND = 'CM' AND B1.FEE_CAL_FLG = 'AM' 
	GROUP BY  CPT_DATE,A1.MERCHANT_NO,MERCHANT_NAME,FEE_RATE

	select @s_CPT_DATE = convert(varchar(8), DATEADD(DAY, 1, @s_CPT_DATE),112)

end
--清分日	客戶代號(會計)	特店名稱	購貨金額	購貨手續費率	購貨手續費	匯款金額

SELECT CPT_DATE AS '清分日',
		MERCHANT_NO AS '客戶代號',
		MERCHANT_NAME AS '特店名稱',
		NULL as ' ',
		NET_AMT AS '購貨金額',
		FEE_RATE AS '購貨手續費率',
		FEE_AMT AS '購貨手續費',
		RE_AMT AS '匯款金額'
FROM @TabeRpt


