--DECLARE @EXEC_MERCHANT_NO VARCHAR(8) = '80738683'
--DECLARE @EXEC_CPT_DATE VARCHAR(8) = '20180401'
DECLARE @EXEC_CPT_DATE_B VARCHAR(8)
DECLARE @EXEC_CPT_DATE_E VARCHAR(8)

SELECT @EXEC_CPT_DATE_B = GET_SUM_DATE.SDATE,
        @EXEC_CPT_DATE_E = GET_SUM_DATE.EDATE
FROM
(
	SELECT
		CASE WHEN SUM_DAY_S = '99' THEN CONVERT(VARCHAR(8), DATEADD(DAY, -1				, DATEADD(MONTH, DATEDIFF(MONTH, '', @EXEC_CPT_DATE) + (SUM_MON_S + 1), '')), 112)
								    ELSE CONVERT(VARCHAR(8), DATEADD(DAY, -1 + SUM_DAY_S	, DATEADD(MONTH, DATEDIFF(MONTH, '', @EXEC_CPT_DATE) + (SUM_MON_S + 0), '')), 112) END AS 'SDATE',
		CASE WHEN SUM_DAY_E = '99' THEN CONVERT(VARCHAR(8), DATEADD(DAY, -1				, DATEADD(MONTH, DATEDIFF(MONTH, '', @EXEC_CPT_DATE) + (SUM_MON_E + 1), '')), 112)
								    ELSE CONVERT(VARCHAR(8), DATEADD(DAY, -1 + SUM_DAY_E	, DATEADD(MONTH, DATEDIFF(MONTH, '', @EXEC_CPT_DATE) + (SUM_MON_E + 0), '')), 112) END AS 'EDATE'
		,SUM_DAY_S,SUM_DAY_E
	FROM	GM_CONTRACT_M
	WHERE	MERCHANT_NO = @EXEC_MERCHANT_NO
		AND	@EXEC_CPT_DATE BETWEEN EFF_DATE_FROM AND EFF_DATE_TO
)GET_SUM_DATE
--"�S�����c", "����", "�ʳf�b�B", "����","�ձb","�ʳf����O�v", "���O����O", "���|","�|��"
	--		,"�[�Ȳb�B" ,"����","�ձb","�[�Ȥ���O�v","�[�Ȥ���O","���|","�|��",
		--	"�۰ʥ[���B","�۰ʥ[�Ȥ���O�v","�۰ʥ[�Ȥ���O","���|","�|��"

SELECT 
		MERCHANT_NAME ,STO_NAME_LONG,
		ISNULL(PCHA,0) as '�ʳf�b�B', 
		'' AS "�ʳf����",
		'' AS "�ʳf�ձb",
		Convert(FLOAT(50),ISNULL(P_CM_AM * 100,0)) as			'�ʳf����O�v', 
		ISNULL(cast(ROUND(PCHA * P_CM_AM,5) as  decimal(15,0)),0)	as '���O����O',
		ISNULL(cast(ROUND(ISNULL(cast(ROUND(PCHA * P_CM_AM,5) as  decimal(15,0)),0)/1.05,0) as  decimal(15,0)),0)   as '���O����O���|',
		ISNULL(cast(ROUND(PCHA * P_CM_AM,5) as  decimal(15,0)),0) - ISNULL(cast(ROUND(ISNULL(cast(ROUND(PCHA * P_CM_AM,5) as  decimal(15,0)),0)/1.05,0) as  decimal(15,0)),0) as '���O����O�|��',
		ISNULL(LOADA,0) '�[�Ȳb�B', 
		'' AS "�[�ȭ���",
		'' AS "�[�Ƚձb",
		Convert(FLOAT(50),ISNULL(L_CM_AM * 100,0)) as			'�[�Ȥ���O�v', 
		ISNULL(cast(ROUND(LOADA * L_CM_AM,5) as  decimal(15,0)),0) as '�[�Ȥ���O',
		ISNULL(cast(ROUND(cast(ROUND(LOADA * L_CM_AM,5) as decimal(20,5))/1.05,0) as  decimal(15,0)),0) as '�[�Ȥ���O���|',
		ISNULL(cast(ROUND(LOADA * L_CM_AM,5) as  decimal(15,0))-cast(ROUND(cast(ROUND(LOADA * L_CM_AM,5) as decimal(20,5))/1.05,0) as  decimal(15,0)),0) as '�[�Ȥ���O�|��',
		ISNULL(ALOAD,0)									'�۰ʥ[���B',
		'' AS "�۰ʥ[�ȭ���",
		'' AS "�۰ʥ[�Ƚձb",
		Convert(FLOAT(50),ISNULL(A__AM * 100,0)) as			'�۰ʥ[�Ȥ���O�v', 
		ISNULL(cast(ROUND(ALOAD * A__AM,5) as  decimal(15,0)),0) as	'�۰ʥ[�Ȥ���O',
		ISNULL(cast(ROUND(cast(ROUND(ALOAD * A__AM,5) as decimal(20,5))/1.05,0) as  decimal(15,0)),0) as '�۰ʥ[�Ȥ���O���|',
		ISNULL(cast(ROUND(ALOAD * A__AM,5) as  decimal(15,0))-cast(ROUND(cast(ROUND(ALOAD * A__AM,5) as decimal(20,5))/1.05,0) as  decimal(15,0)),0) as '�۰ʥ[�Ȥ���O�|��'
	
FROM 
(
	SELECT MERCHANT_NO, STORE_NO, SUM(PCHA) AS PCHA ,SUM(LOADA) AS LOADA,SUM(ALOAD) AS ALOAD,P_CM_AM,L_CM_AM,A__AM
	FROM 
	(
		SELECT  
		A.CPT_DATE,
		A.MERCHANT_NO ,
		A.STORE_NO,
		ISNULL(SUM(CASE WHEN TRANS_TYPE = '51' THEN 1 WHEN TRANS_TYPE = '53' THEN -1 END * TRANS_AMT),0) AS 'PCHA',
		ISNULL(SUM(CASE WHEN TRANS_TYPE = '52' THEN 1 WHEN TRANS_TYPE = '54' THEN -1 END * TRANS_AMT),0) AS 'LOADA',
		ISNULL(SUM(CASE WHEN TRANS_TYPE in ('74','75','77') THEN 1 END * TRANS_AMT),0) AS 'ALOAD',
		(select C.FEE_RATE from GM_CONTRACT_D C where C.MERCHANT_NO = A.MERCHANT_NO
		and A.CPT_DATE between C.EFF_DATE_FROM and C.EFF_DATE_TO
		and C.SETTLE_TYPE = 'P' and C.FEE_KIND = 'CM' and FEE_CAL_FLG = 'AM'
		and @EXEC_CPT_DATE_B between C.EFF_DATE_FROM and C.EFF_DATE_TO) P_CM_AM,
		(select C.FEE_RATE from GM_CONTRACT_D C where C.MERCHANT_NO = A.MERCHANT_NO
		and A.CPT_DATE between C.EFF_DATE_FROM and C.EFF_DATE_TO
		and C.SETTLE_TYPE = 'L' and FEE_CAL_FLG = 'AM'
		and @EXEC_CPT_DATE_B between C.EFF_DATE_FROM and C.EFF_DATE_TO) L_CM_AM,
		(select C.FEE_RATE from GM_CONTRACT_D C where C.MERCHANT_NO = A.MERCHANT_NO
		and A.CPT_DATE between C.EFF_DATE_FROM and C.EFF_DATE_TO
		and C.SETTLE_TYPE = 'A' and FEE_CAL_FLG = 'AM'
		and @EXEC_CPT_DATE_B between C.EFF_DATE_FROM and C.EFF_DATE_TO) A__AM
		from {0}.TM_SETTLE_DATA_D A 
		WHERE CPT_DATE BETWEEN @EXEC_CPT_DATE_B AND @EXEC_CPT_DATE_E
		GROUP BY A.CPT_DATE, A.MERCHANT_NO,A.STORE_NO
	) TE
	GROUP BY MERCHANT_NO,STORE_NO,P_CM_AM,L_CM_AM,A__AM
)TE2
LEFT JOIN {0}.BM_STORE_M B
ON TE2.MERCHANT_NO = B.MERCHANT_NO AND TE2.STORE_NO= B.STORE_NO
LEFT JOIN GM_MERCHANT C
ON TE2.MERCHANT_NO = C.MERCHANT_NO