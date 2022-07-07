--DECLARE @TARGET_MONTH VARCHAR(6) = '202108'--@EXEC_TARGET_MONTH
--DECLARE @Group_TaiYa          VARCHAR(1) = @EXEC_Group_TaiYa--台亞
--DECLARE @Group_TaiSu          VARCHAR(1) = @EXEC_Group_TaiSu--台塑
--DECLARE @Group_XiOu           VARCHAR(1) = @EXEC_Group_XiOu--西歐
--DECLARE @Group_FuMou          VARCHAR(1) = @EXEC_Group_FuMou--福懋
--DECLARE @Group_TongYiDuJiaCun VARCHAR(1) = @EXEC_Group_TongYiDuJiaCun--統一渡假村

--●兩明細差異
SELECT *
INTO #TargetData_OpenPoint_TransDetail
FROM dbo.OpenPoint_TransDetail ot WITH(NOLOCK)
WHERE (SUBSTRING(ot.WriteOffNo,7,6) = @TARGET_MONTH
		OR CONVERT(varchar(7),ot.CptDate) = LEFT(@TARGET_MONTH,4)+'/'+RIGHT(@TARGET_MONTH,2))
	AND SUBSTRING(ISNULL(ot.WriteOffNo,''),7,6) <> CONVERT(VARCHAR(6),REPLACE(ot.CptDate,'/',''))
	AND ((@Group_TaiYa='Y' AND StoreName LIKE '%台亞%')
		OR (@Group_TaiSu='Y' AND StoreName LIKE '%台塑%')
		OR (@Group_XiOu='Y' AND StoreName LIKE '%西歐%')
		OR (@Group_FuMou='Y' AND StoreName LIKE '%福懋%')
		OR (@Group_TongYiDuJiaCun='Y' AND StoreName LIKE '%統一渡假村%'))

SELECT 
	REPLACE(ot.CptDate,'/','') [OP日期]
	,SUBSTRING(ISNULL(ot.WriteOffNo,''),7,8) [機構日期]
	,ot.CptDate
	,SUBSTRING(ISNULL(ot.WriteOffNo,''),11,2) [機構認列月份]
	,ot.UnifiedBusinessNo [UnifiedBusinessName]
	,ot.StoreNo,ot.StoreName,ot.PosNo,ot.TransNo,ot.TransDate,ot.TransType,ot.Amount,ot.CardType,ot.CardNo,ot.Point,ot.WriteOffNo,ot.CreateDate,ot.FileName
	,CASE opmv.UnifiedBusinessNo
		WHEN '72769260' THEN '23115927'
		WHEN '52795374' THEN '52538517'
		WHEN '55780851' THEN '52538517'
		WHEN '55793407' THEN '52538517'
		WHEN '09765625' THEN '27359183'
		ELSE opmv.UnifiedBusinessNo
	 END [UnifiedBusinessNo]
FROM #TargetData_OpenPoint_TransDetail ot WITH(NOLOCK)
LEFT JOIN dbo.OpenPoint_Merchant_VIEW opmv WITH(NOLOCK)
	ON ICashMID+ICashSID = ot.StoreNo
	AND substring(replace(ot.TransDate,'/',''),1,8) between EFF_DATE_FROM and EFF_DATE_TO

DROP TABLE #TargetData_OpenPoint_TransDetail