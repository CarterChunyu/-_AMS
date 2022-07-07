--DECLARE @TARGET_MONTH VARCHAR(6) = '202108'--@EXEC_TARGET_MONTH
--DECLARE @Group_TaiYa          VARCHAR(1) = @EXEC_Group_TaiYa--台亞
--DECLARE @Group_TaiSu          VARCHAR(1) = @EXEC_Group_TaiSu--台塑
--DECLARE @Group_XiOu           VARCHAR(1) = @EXEC_Group_XiOu--西歐
--DECLARE @Group_FuMou          VARCHAR(1) = @EXEC_Group_FuMou--福懋
--DECLARE @Group_TongYiDuJiaCun VARCHAR(1) = @EXEC_Group_TongYiDuJiaCun--統一渡假村

--●發票檔原資料
SELECT D.UnifiedBusinessNo
	,D.MerchantName
	,SUM(CASE WHEN D.PointType='累點' THEN D.Point ELSE 0 END)
		+SUM(CASE WHEN D.PointType='累點退貨' THEN D.Point ELSE 0 END)
		+SUM(CASE WHEN D.PointType='客服累點' THEN D.Point ELSE 0 END)
		+SUM(CASE WHEN D.PointType='客服累點退貨' THEN D.Point ELSE 0 END)
		+SUM(CASE WHEN D.PointType='OP代墊負點數淨額' THEN D.Point ELSE 0 END) [TotalGetPoint]
	,(-1)*(SUM(CASE WHEN D.PointType='兌點' THEN D.Point ELSE 0 END)
		+SUM(CASE WHEN D.PointType='兌點取消' THEN D.Point ELSE 0 END)
		+SUM(CASE WHEN D.PointType='客服兌點' THEN D.Point ELSE 0 END)
		+SUM(CASE WHEN D.PointType='客服兌點取消' THEN D.Point ELSE 0 END)) [TotalRedeemPoint]
INTO #TargetData_DateMerchant
FROM (
	SELECT
		CASE M.UnifiedBusinessNo
			WHEN '72769260' THEN '23115927'
			WHEN '52795374' THEN '52538517'
			WHEN '55780851' THEN '52538517'
			WHEN '55793407' THEN '52538517'
			WHEN '09765625' THEN '27359183'
			ELSE M.UnifiedBusinessNo
		 END [UnifiedBusinessNo]
		,CASE M.UnifiedBusinessNo
				WHEN '72769260' THEN '台塑-世新礁溪'
				WHEN '52795374' THEN '台塑-挑戰者'
				WHEN '55780851' THEN '台塑-挑戰者'
				WHEN '55793407' THEN '台塑-挑戰者'
				WHEN '09765625' THEN '台塑-指南成泰'
				ELSE M.MerchantName
		 END [MerchantName]
		,SUBSTRING(D.TransType,1,LEN(D.TransType)-3) [PointType]
		,SUM(D.Point) [Point]
	FROM ICASHOP.dbo.OpenPoint_TransDetail D WITH(NOLOCK)
	INNER JOIN ICASHOP.dbo.OpenPoint_MerchantData M WITH(NOLOCK)
		ON SUBSTRING(ISNULL(D.WriteOffNo,''),7,6) = @TARGET_MONTH
		AND ((@Group_TaiYa='Y' AND M.MerchantName LIKE '%台亞%')
			OR (@Group_TaiSu='Y' AND M.MerchantName LIKE '%台塑%')
			OR (@Group_XiOu='Y' AND M.MerchantName LIKE '%西歐%')
			OR (@Group_FuMou='Y' AND M.MerchantName LIKE '%福懋%')
			OR (@Group_TongYiDuJiaCun='Y' AND M.MerchantName LIKE '%統一渡假村%'))
		AND CONVERT(VARCHAR(4),D.StoreNo) = M.ICashMID
	GROUP BY M.UnifiedBusinessNo,M.MerchantName,SUBSTRING(D.TransType,1,LEN(D.TransType)-3)
) D
GROUP BY D.UnifiedBusinessNo,D.MerchantName


SELECT D.UnifiedBusinessNo
	,D.MerchantName
	,SUM(CASE WHEN D.PointType='累點' THEN D.Point ELSE 0 END)
		+SUM(CASE WHEN D.PointType='累點退貨' THEN D.Point ELSE 0 END)
		+SUM(CASE WHEN D.PointType='客服累點' THEN D.Point ELSE 0 END)
		+SUM(CASE WHEN D.PointType='客服累點退貨' THEN D.Point ELSE 0 END)
		+SUM(CASE WHEN D.PointType='OP代墊負點數淨額' THEN D.Point ELSE 0 END) [TotalGetPoint]
	,(-1)*(SUM(CASE WHEN D.PointType='兌點' THEN D.Point ELSE 0 END)
		+SUM(CASE WHEN D.PointType='兌點取消' THEN D.Point ELSE 0 END)
		+SUM(CASE WHEN D.PointType='客服兌點' THEN D.Point ELSE 0 END)
		+SUM(CASE WHEN D.PointType='客服兌點取消' THEN D.Point ELSE 0 END)) [TotalRedeemPoint]
INTO #TargetData_DateOP
FROM (
	SELECT
		CASE M.UnifiedBusinessNo
			WHEN '72769260' THEN '23115927'
			WHEN '52795374' THEN '52538517'
			WHEN '55780851' THEN '52538517'
			WHEN '55793407' THEN '52538517'
			WHEN '09765625' THEN '27359183'
			ELSE M.UnifiedBusinessNo
		 END [UnifiedBusinessNo]
		,CASE M.UnifiedBusinessNo
				WHEN '72769260' THEN '台塑-世新礁溪'
				WHEN '52795374' THEN '台塑-挑戰者'
				WHEN '55780851' THEN '台塑-挑戰者'
				WHEN '55793407' THEN '台塑-挑戰者'
				WHEN '09765625' THEN '台塑-指南成泰'
				ELSE M.MerchantName
		 END [MerchantName]
		,SUBSTRING(D.TransType,1,LEN(D.TransType)-3) [PointType]
		,SUM(D.Point) [Point]
	FROM ICASHOP.dbo.OpenPoint_TransDetail D WITH(NOLOCK)
	INNER JOIN ICASHOP.dbo.OpenPoint_MerchantData M WITH(NOLOCK)
		ON CONVERT(varchar(7),D.CptDate) = LEFT(@TARGET_MONTH,4)+'/'+RIGHT(@TARGET_MONTH,2)
		AND ((@Group_TaiYa='Y' AND M.MerchantName LIKE '%台亞%')
			OR (@Group_TaiSu='Y' AND M.MerchantName LIKE '%台塑%')
			OR (@Group_XiOu='Y' AND M.MerchantName LIKE '%西歐%')
			OR (@Group_FuMou='Y' AND M.MerchantName LIKE '%福懋%')
			OR (@Group_TongYiDuJiaCun='Y' AND M.MerchantName LIKE '%統一渡假村%'))
		AND CONVERT(VARCHAR(4),D.StoreNo) = M.ICashMID
	GROUP BY M.UnifiedBusinessNo,M.MerchantName,SUBSTRING(D.TransType,1,LEN(D.TransType)-3)
) D
GROUP BY D.UnifiedBusinessNo,D.MerchantName


SELECT CASE WHEN o.UnifiedBusinessNo IS NULL THEN M.UnifiedBusinessNo ELSE o.UnifiedBusinessNo END [UnifiedBusinessNo]
	,CASE WHEN o.MerchantName IS NULL THEN M.MerchantName ELSE o.MerchantName END [MerchantName]
	,o.TotalGetPoint [TotalGetPoint(OP帳)]
	,o.TotalRedeemPoint [TotalRedeemPoint(OP帳)]
	,m.TotalGetPoint [TotalGetPoint(機構帳)]
	,m.TotalRedeemPoint [TotalRedeemPoint(機構帳)]
FROM #TargetData_DateOP o
FULL JOIN #TargetData_DateMerchant m
	ON o.UnifiedBusinessNo = m.UnifiedBusinessNo
	AND o.MerchantName = m.MerchantName
ORDER BY [UnifiedBusinessNo]

DROP TABLE #TargetData_DateOP
DROP TABLE #TargetData_DateMerchant