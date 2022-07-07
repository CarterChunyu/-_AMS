--if (@EXEC_MERCHANT_NO in (SELECT MERCHANT_NO FROM GM_MERCHANT WHERE MERCHANT_TYPE = 'MUTI_MERC'))
--BEGIN
--select  M1.INVOICE_NO '�νs',
--		Case when M2.MERCHANT_SUB_STNAME is null then M1.MERCHANT_NAME else M2.MERCHANT_SUB_STNAME end '�S�����c�W��',
--		M1.MERCHANT_STNAME '²��',
--		ISNULL((DATA.sumPCHA-DATA.sumPCHRA),0) as '�ʳf�b�B', 
--		'' AS "�ʳf����",
--		'' AS "�ʳf�ձb",
--		Convert(FLOAT(50),ISNULL(DATA.P_CM_AM * 100,0)) as			'�ʳf����O�v', 
--		ISNULL(cast(ROUND((DATA.sumPCHA-DATA.sumPCHRA)*DATA.P_CM_AM,0) as bigint),0)	as '���O����O',
--		ISNULL(cast(ROUND(cast(ROUND((DATA.sumPCHA-DATA.sumPCHRA)*DATA.P_CM_AM,0) as decimal(20,5))/1.05,0) as bigint),0)   as '���O����O���|',
--		ISNULL(cast(ROUND((DATA.sumPCHA-DATA.sumPCHRA)*DATA.P_CM_AM,0) as bigint)-cast(ROUND(cast(ROUND((DATA.sumPCHA-DATA.sumPCHRA)*DATA.P_CM_AM,0) as decimal(15,5))/1.05,0) as bigint),0) as '���O����O�|��',
--		ISNULL((DATA.sumLOADA-DATA.sumLOADRA),0) '�[�Ȳb�B', 
--		'' AS "�[�ȭ���",
--		'' AS "�[�Ƚձb",
--		Convert(FLOAT(50),ISNULL(DATA.L_CM_AM * 100,0)) as			'�[�Ȥ���O�v', 
--		ISNULL(cast(ROUND((DATA.sumLOADA-DATA.sumLOADRA)*DATA.L_CM_AM,0) as bigint),0) as '�[�Ȥ���O',
--		ISNULL(cast(ROUND(cast(ROUND((DATA.sumLOADA-DATA.sumLOADRA)*DATA.L_CM_AM,0) as decimal(20,5))/1.05,0) as bigint),0) as '�[�Ȥ���O���|',
--		ISNULL(cast(ROUND((DATA.sumLOADA-DATA.sumLOADRA)*DATA.L_CM_AM,0) as bigint)-cast(ROUND(cast(ROUND((DATA.sumLOADA-DATA.sumLOADRA)*DATA.L_CM_AM,0) as decimal(20,5))/1.05,0) as bigint),0) as '�[�Ȥ���O�|��',
--		ISNULL(DATA.sumLOAD_AMT,0)									'�۰ʥ[���B',
--		'' AS "�۰ʥ[�ȭ���",
--		'' AS "�۰ʥ[�Ƚձb",
--		Convert(FLOAT(50),ISNULL(DATA.A__AM * 100,0)) as			'�۰ʥ[�Ȥ���O�v', 
--		ISNULL(cast(ROUND(sumLOAD_AMT*DATA.A__AM,0) as bigint),0) as	'�۰ʥ[�Ȥ���O',
--		ISNULL(cast(ROUND(cast(ROUND(sumLOAD_AMT*DATA.A__AM,0) as decimal(20,5))/1.05,0) as bigint),0) as '�۰ʥ[�Ȥ���O���|',
--		ISNULL(cast(ROUND(sumLOAD_AMT*DATA.A__AM,0) as bigint)-cast(ROUND(cast(ROUND(sumLOAD_AMT*DATA.A__AM,0) as decimal(20,5))/1.05,0) as bigint),0) as '�۰ʥ[�Ȥ���O�|��'
		
--from (
--	SELECT MERCHANT_NO,MERCHANT_SUB_NO,SUM(sumPCHA) AS sumPCHA,SUM(sumPCHRA) AS sumPCHRA,P_CM_AM,SUM(sumLOADA) AS sumLOADA,SUM(sumLOADRA) AS sumLOADRA,L_CM_AM,SUM(sumLOAD_AMT) AS sumLOAD_AMT,A__AM FROM 
--	(
--	select	DATA1.CPT_DATE, DATA1.MERCHANT_NO, DATA1.SEQ_NO, DATA1.MERCHANT_SUB_NO,
--			ISNULL(sum(DATA1.sumPCHA),0) sumPCHA,
--			ISNULL(sum(DATA1.sumPCHRA),0) sumPCHRA,
--						(select C.FEE_RATE from GM_CONTRACT_MUTI_MERC_D C where C.MERCHANT_NO = DATA1.MERCHANT_NO
--													  and DATA1.CPT_DATE between C.EFF_DATE_FROM and C.EFF_DATE_TO
--													  and C.CONTRACT_TYPE = 'P1' AND FEE_KIND = 'CM' AND FEE_CAL_FLG = 'AM'
--													  and @EXEC_CPT_DATE_B between C.EFF_DATE_FROM and C.EFF_DATE_TO
--													  ) P_CM_AM,
--			ISNULL(sum(DATA1.sumLOADA),0) sumLOADA,
--			ISNULL(sum(DATA1.sumLOADRA),0) sumLOADRA,
--			(select C.FEE_RATE from GM_CONTRACT_MUTI_MERC_D C where C.MERCHANT_NO = DATA1.MERCHANT_NO
--													  and DATA1.CPT_DATE between C.EFF_DATE_FROM and C.EFF_DATE_TO
--													  and C.CONTRACT_TYPE = 'L1' AND FEE_CAL_FLG = 'AM'
--													  and @EXEC_CPT_DATE_B between C.EFF_DATE_FROM and C.EFF_DATE_TO) L_CM_AM,
--			--ISNULL(sum(DATA1.sumUTUT3_AMTA),0) sumUTUT3_AMTA,
--			--ISNULL(sum(DATA1.sumUTUT3_AMTRA),0) sumUTUT3_AMTRA,
--			--(select C.FEE_RATE from GM_CONTRACT_D C where C.MERCHANT_NO = DATA1.MERCHANT_NO
--			--										  and DATA1.CPT_DATE between C.EFF_DATE_FROM and C.EFF_DATE_TO
--			--										  and C.SETTLE_TYPE = 'P' and C.FEE_KIND = 'UT' and FEE_CAL_FLG = 'AM'
--			--										  and @EXEC_CPT_DATE_B between C.EFF_DATE_FROM and C.EFF_DATE_TO) P_UT_AM,
--			ISNULL(sum(LOAD74_AMT),0) + ISNULL(sum(LOAD75_AMT),0) + ISNULL(sum(LOAD77_AMT),0) sumLOAD_AMT,  --(add by 20160415 rita �s�W 77���u�۰ʥ[��)
--			(select C.FEE_RATE from GM_CONTRACT_MUTI_MERC_D C where C.MERCHANT_NO = DATA1.MERCHANT_NO
--													  and DATA1.CPT_DATE between C.EFF_DATE_FROM and C.EFF_DATE_TO
--													  and C.CONTRACT_TYPE = 'L2' AND FEE_CAL_FLG = 'AM'
--													  and @EXEC_CPT_DATE_B between C.EFF_DATE_FROM and C.EFF_DATE_TO) A__AM
--			--ISNULL(sum(LOAD79_AMT),0) sumLOAD79_AMT,
--			--(select C.FEE_RATE from GM_CONTRACT_D C where C.MERCHANT_NO = DATA1.MERCHANT_NO
--			--										  and DATA1.CPT_DATE between C.EFF_DATE_FROM and C.EFF_DATE_TO
--			--										  and C.SETTLE_TYPE = 'B' and FEE_CAL_FLG = 'AM'
--			--										  and @EXEC_CPT_DATE_B between C.EFF_DATE_FROM and C.EFF_DATE_TO) B__AM
--	from 
--	(
--		--�ʳf�B�ʳf�����B�[�ȡB�[�Ȩ���
--		select	D1.CPT_DATE, D1.MERCHANT_NO, 
--				Case when D2.MERCHANT_SUB_NO = 'POS' then 'FS1' 
--					 when D2.MERCHANT_SUB_NO IS NULL then 'FS1' 
--					 else '0' end SEQ_NO,
--				D2.MERCHANT_SUB_NO,
--				ISNULL(sum(D1.PCHA),0) - ISNULL(sum(D1.UTUT3_AMTA),0) sumPCHA, 
--				ISNULL(sum(D1.PCHRA),0) - ISNULL(sum(D1.UTUT3_AMTRA),0) sumPCHRA, 
--				ISNULL(sum(D1.LOADA),0) sumLOADA, 
--				ISNULL(sum(D1.LOADRA),0) sumLOADRA, 
--				ISNULL(sum(D1.UTUT3_AMTA),0) sumUTUT3_AMTA, 
--				ISNULL(sum(D1.UTUT3_AMTRA),0) sumUTUT3_AMTRA,
--				NULL LOAD74_CNT,
--				NULL LOAD74_AMT,
--				NULL LOAD75_CNT,
--				NULL LOAD75_AMT,
--				NULL LOAD77_CNT,
--				NULL LOAD77_AMT,
--				NULL LOAD79_CNT,
--				NULL LOAD79_AMT
--		from (
--			SELECT	CPT_DATE,
--					MERCHANT_NO,
--					'POS' MERCHANT_SUB_NO,
--					SUM(isnull(PCH_AMT  ,0)) PCHA,		--�ʳf�`�B
--					SUM(isnull(PCHR_AMT ,0)) PCHRA,		--�h�f�`�B
--					SUM(isnull(LOAD_AMT ,0)) LOADA,		--�[���B
--					SUM(isnull(LOADR_AMT,0)) LOADRA,	--�[�Ȩ����B
--					NULL UTUT3_AMTA,					--�N�����`�B
--					NULL UTUT3_AMTRA					--�N�������
--				FROM AM_ISET_MERC_TRANS_LOG_D 
--				where CPT_DATE between @EXEC_CPT_DATE_B and @EXEC_CPT_DATE_E
--				  and SRC_FLG = 'TXLOG'
--				GROUP BY CPT_DATE, MERCHANT_NO
--			union all 
--			SELECT	CPT_DATE,
--					MERCHANT_NO,
--					'POS' MERCHANT_NO_SUB,
--					NULL PCHA,		--�ʳf�`�B
--					NULL PCHRA,		--�h�f�`�B
--					NULL LOADA,		--�[���B
--					NULL LOADRA,	--�[�Ȩ����B
--					SUM(isnull(UT_AMT ,0) + isnull(UT3_AMT ,0)) UTUT3_AMTA,	--�N�����`�B
--					NULL UTUT3_AMTRA	--�N�������
--				FROM AM_ISET_MERC_TRANS_UTLOG_D  
--				where CPT_DATE between @EXEC_CPT_DATE_B and @EXEC_CPT_DATE_E
--				  and SRC_FLG = 'TXLOG'
--				  and TRANS_TYPE = '21'
--				GROUP BY CPT_DATE, MERCHANT_NO
--			union all 
--			SELECT	CPT_DATE,
--					MERCHANT_NO,
--					'POS' MERCHANT_NO_SUB,
--					NULL PCHA,		--�ʳf�`�B
--					NULL PCHRA,		--�h�f�`�B
--					NULL LOADA,		--�[���B
--					NULL LOADRA,	--�[�Ȩ����B
--					NULL UTUT3_AMTA,--�N�����`�B
--					SUM(isnull(UT_AMT ,0) + isnull(UT3_AMT ,0)) UTUT3_AMTRA	--�N�������
--				FROM AM_ISET_MERC_TRANS_UTLOG_D  
--				where CPT_DATE between @EXEC_CPT_DATE_B and @EXEC_CPT_DATE_E
--				  and SRC_FLG = 'TXLOG'
--				  and TRANS_TYPE = '23'
--				GROUP BY CPT_DATE, MERCHANT_NO
--		)D1 
--		left join GM_MERCHANT_SUB D2 on D1.MERCHANT_NO = D2.MERCHANT_NO
--									and D1.MERCHANT_SUB_NO = D2.MERCHANT_SUB_NO
--		GROUP BY D1.CPT_DATE, D1.MERCHANT_NO, D2.MERCHANT_SUB_NO
--		union all 
--		--�۰ʥ[�ȡB�۰ʥ[�Ȩ����B���u�۰ʥ[��
--		--�B�bGM_BANK_CPT_SUM_SUB �S���ȡA�N��ӯS�����c�S�����U�@���h
--		SELECT	A1.CPT_DATE,
--				A1.MERCHANT_NO,
--				'FS1' SEQ_NO, 
--				NULL MERCHANT_SUB_NO,
--				NULL sumPCHA, 
--				NULL sumPCHRA, 
--				NULL sumLOADA, 
--				NULL sumLOADRA, 
--				NULL sumUTUT3_AMTA, 
--				NULL sumUTUT3_AMTRA,
--				SUM(A1.LOAD74_CNT) LOAD74_CNT,
--				SUM(A1.LOAD74_AMT) LOAD74_AMT,
--				SUM(A1.LOAD75_CNT) LOAD75_CNT,
--				SUM(A1.LOAD75_AMT) LOAD75_AMT,
--				SUM(A1.LOAD77_CNT) LOAD77_CNT, -- (add by 20160415 rita �s�W 77���u�۰ʥ[��)
--				SUM(A1.LOAD77_AMT) LOAD77_AMT, -- (add by 20160415 rita �s�W 77���u�۰ʥ[��)
--				NULL LOAD79_CNT,
--				NULL LOAD79_AMT
--			FROM GM_BANK_CPT_SUM A1
--			left join (
--						select distinct MERCHANT_NO, CPT_DATE, SETTLE_DATE
--						from GM_BANK_CPT_SUM_SUB
--						where CPT_DATE between @EXEC_CPT_DATE_B and @EXEC_CPT_DATE_E	
--			) A2 on A1.MERCHANT_NO = A2.MERCHANT_NO
--				and A1.CPT_DATE = A2.CPT_DATE
--				and A1.SETTLE_DATE = A2.SETTLE_DATE
--			where A1.CPT_DATE between @EXEC_CPT_DATE_B and @EXEC_CPT_DATE_E
--			  and A2.MERCHANT_NO is null
--			GROUP BY A1.CPT_DATE, A1.MERCHANT_NO
--		union all 
--		--�۰ʥ[�ȡB�۰ʥ[�Ȩ����B���u�۰ʥ[��
--		--�B�bGM_BANK_CPT_SUM_SUB ���ȡA�N��ӯS�����c�����U�@���h
--		--�h�H�]�w���@���n����ʳf�B�ʳf����
--		SELECT	A1.CPT_DATE,
--				A1.MERCHANT_NO,
--				Case when A2.MERCHANT_SUB_NO = 'POS' then 'FS1' else '' end SEQ_NO,
--				A2.MERCHANT_SUB_NO,
--				NULL sumPCHA, 
--				NULL sumPCHRA, 
--				NULL sumLOADA, 
--				NULL sumLOADRA, 
--				NULL sumUTUT3_AMTA, 
--				NULL sumUTUT3_AMTRA,
--				SUM(A2.LOAD74_CNT) LOAD74_CNT,
--				SUM(A2.LOAD74_AMT) LOAD74_AMT,
--				SUM(A2.LOAD75_CNT) LOAD75_CNT,
--				SUM(A2.LOAD75_AMT) LOAD75_AMT,
--				SUM(A2.LOAD77_CNT) LOAD77_CNT, -- (add by 20160415 rita �s�W 77���u�۰ʥ[��)
--				SUM(A2.LOAD77_AMT) LOAD77_AMT, -- (add by 20160415 rita �s�W 77���u�۰ʥ[��)
--				SUM(A2.LOAD79_CNT) LOAD79_CNT,
--				SUM(A2.LOAD79_AMT) LOAD79_AMT
--			FROM GM_BANK_CPT_SUM A1
--			inner join GM_BANK_CPT_SUM_SUB A2 on A1.MERCHANT_NO = A2.MERCHANT_NO
--											and A1.BANK_MERCHANT = A2.BANK_MERCHANT
--											and A1.CPT_DATE = A2.CPT_DATE
--											and A1.SETTLE_DATE = A2.SETTLE_DATE
--			where A1.CPT_DATE between @EXEC_CPT_DATE_B and @EXEC_CPT_DATE_E
--			GROUP BY A1.CPT_DATE, A1.MERCHANT_NO, A2.MERCHANT_SUB_NO
--	)DATA1
--	GROUP BY DATA1.CPT_DATE, DATA1.MERCHANT_NO, DATA1.SEQ_NO, DATA1.MERCHANT_SUB_NO
--	) DATA0 
--	GROUP BY DATA0.MERCHANT_NO, DATA0.MERCHANT_SUB_NO,P_CM_AM,L_CM_AM,A__AM
--)DATA
--inner join GM_MERCHANT M1 on DATA.MERCHANT_NO = M1.MERCHANT_NO
--							and M1.MERCHANT_NO = case when @EXEC_MERCHANT_NO = 'ALL' then M1.MERCHANT_NO else @EXEC_MERCHANT_NO end 
--left join GM_MERCHANT_SUB M2 on DATA.MERCHANT_SUB_NO = M2.MERCHANT_SUB_NO
----WHERE DATA.MERCHANT_SUB_NO IS NULL OR (DATA.MERCHANT_SUB_NO<>'IBN' AND DATA.MERCHANT_SUB_NO<>'POS') 
--order by M1.MERCHANT_NO--, DATA.CPT_DATE
--END
----�D�e�~
--ELSE
--BEGIN
--select  M1.INVOICE_NO '�νs',
--		Case when M2.MERCHANT_SUB_STNAME is null then M1.MERCHANT_NAME else M2.MERCHANT_SUB_STNAME end '�S�����c�W��',
--		M1.MERCHANT_STNAME '²��',
		
--		ISNULL((DATA.sumPCHA-DATA.sumPCHRA),0) as '�ʳf�b�B', 
--		'' AS "�ʳf����",
--		'' AS "�ʳf�ձb",
--		Convert(FLOAT(50),ISNULL(DATA.P_CM_AM * 100,0)) as			'�ʳf����O�v', 
--		ISNULL(cast(ROUND((DATA.sumPCHA-DATA.sumPCHRA)*DATA.P_CM_AM,0) as bigint),0)	as '���O����O',
--		ISNULL(cast(ROUND(cast(ROUND((DATA.sumPCHA-DATA.sumPCHRA)*DATA.P_CM_AM,0) as decimal(20,5))/1.05,0) as bigint),0)   as '���O����O���|',
--		ISNULL(cast(ROUND((DATA.sumPCHA-DATA.sumPCHRA)*DATA.P_CM_AM,0) as bigint)-cast(ROUND(cast(ROUND((DATA.sumPCHA-DATA.sumPCHRA)*DATA.P_CM_AM,0) as decimal(15,5))/1.05,0) as bigint),0) as '���O����O�|��',
--		ISNULL((DATA.sumLOADA-DATA.sumLOADRA),0) '�[�Ȳb�B', 
--		'' AS "�[�ȭ���",
--		'' AS "�[�Ƚձb",
--		Convert(FLOAT(50),ISNULL(DATA.L_CM_AM * 100,0)) as			'�[�Ȥ���O�v', 
--		ISNULL(cast(ROUND((DATA.sumLOADA-DATA.sumLOADRA)*DATA.L_CM_AM,0) as bigint),0) as '�[�Ȥ���O',
--		ISNULL(cast(ROUND(cast(ROUND((DATA.sumLOADA-DATA.sumLOADRA)*DATA.L_CM_AM,0) as decimal(20,5))/1.05,0) as bigint),0) as '�[�Ȥ���O���|',
--		ISNULL(cast(ROUND((DATA.sumLOADA-DATA.sumLOADRA)*DATA.L_CM_AM,0) as bigint)-cast(ROUND(cast(ROUND((DATA.sumLOADA-DATA.sumLOADRA)*DATA.L_CM_AM,0) as decimal(20,5))/1.05,0) as bigint),0) as '�[�Ȥ���O�|��',
--		ISNULL(DATA.sumLOAD_AMT,0)									'�۰ʥ[���B',
--		'' AS "�۰ʥ[�ȭ���",
--		'' AS "�۰ʥ[�Ƚձb",
--		Convert(FLOAT(50),ISNULL(DATA.A__AM * 100,0)) as			'�۰ʥ[�Ȥ���O�v', 
--		ISNULL(cast(ROUND(sumLOAD_AMT*DATA.A__AM,0) as bigint),0) as	'�۰ʥ[�Ȥ���O',
--		ISNULL(cast(ROUND(cast(ROUND(sumLOAD_AMT*DATA.A__AM,0) as decimal(20,5))/1.05,0) as bigint),0) as '�۰ʥ[�Ȥ���O���|',
--		ISNULL(cast(ROUND(sumLOAD_AMT*DATA.A__AM,0) as bigint)-cast(ROUND(cast(ROUND(sumLOAD_AMT*DATA.A__AM,0) as decimal(20,5))/1.05,0) as bigint),0) as '�۰ʥ[�Ȥ���O�|��'
		
--from (
--	SELECT MERCHANT_NO,MERCHANT_SUB_NO,SUM(sumPCHA) AS sumPCHA,SUM(sumPCHRA) AS sumPCHRA,P_CM_AM,SUM(sumLOADA) AS sumLOADA,SUM(sumLOADRA) AS sumLOADRA,L_CM_AM,SUM(sumLOAD_AMT) AS sumLOAD_AMT,A__AM FROM 
--	(
--	select	DATA1.CPT_DATE, DATA1.MERCHANT_NO, DATA1.SEQ_NO, DATA1.MERCHANT_SUB_NO,
--			ISNULL(sum(DATA1.sumPCHA),0) sumPCHA,
--			ISNULL(sum(DATA1.sumPCHRA),0) sumPCHRA,
--			(select C.FEE_RATE from GM_CONTRACT_D C where C.MERCHANT_NO = DATA1.MERCHANT_NO
--													  and DATA1.CPT_DATE between C.EFF_DATE_FROM and C.EFF_DATE_TO
--													  and C.SETTLE_TYPE = 'P' and C.FEE_KIND = 'CM' and FEE_CAL_FLG = 'AM'
--													  and @EXEC_CPT_DATE_B between C.EFF_DATE_FROM and C.EFF_DATE_TO) P_CM_AM,
--			ISNULL(sum(DATA1.sumLOADA),0) sumLOADA,
--			ISNULL(sum(DATA1.sumLOADRA),0) sumLOADRA,
--			(select C.FEE_RATE from GM_CONTRACT_D C where C.MERCHANT_NO = DATA1.MERCHANT_NO
--													  and DATA1.CPT_DATE between C.EFF_DATE_FROM and C.EFF_DATE_TO
--													  and C.SETTLE_TYPE = 'L' and FEE_CAL_FLG = 'AM'
--													  and @EXEC_CPT_DATE_B between C.EFF_DATE_FROM and C.EFF_DATE_TO) L_CM_AM,
--			--ISNULL(sum(DATA1.sumUTUT3_AMTA),0) sumUTUT3_AMTA,
--			--ISNULL(sum(DATA1.sumUTUT3_AMTRA),0) sumUTUT3_AMTRA,
--			--(select C.FEE_RATE from GM_CONTRACT_D C where C.MERCHANT_NO = DATA1.MERCHANT_NO
--			--										  and DATA1.CPT_DATE between C.EFF_DATE_FROM and C.EFF_DATE_TO
--			--										  and C.SETTLE_TYPE = 'P' and C.FEE_KIND = 'UT' and FEE_CAL_FLG = 'AM'
--			--										  and @EXEC_CPT_DATE_B between C.EFF_DATE_FROM and C.EFF_DATE_TO) P_UT_AM,
--			ISNULL(sum(LOAD74_AMT),0) + ISNULL(sum(LOAD75_AMT),0) + ISNULL(sum(LOAD77_AMT),0) sumLOAD_AMT,  --(add by 20160415 rita �s�W 77���u�۰ʥ[��)
--			(select C.FEE_RATE from GM_CONTRACT_D C where C.MERCHANT_NO = DATA1.MERCHANT_NO
--													  and DATA1.CPT_DATE between C.EFF_DATE_FROM and C.EFF_DATE_TO
--													  and C.SETTLE_TYPE = 'A' and FEE_CAL_FLG = 'AM'
--													  and @EXEC_CPT_DATE_B between C.EFF_DATE_FROM and C.EFF_DATE_TO) A__AM--,
--			--ISNULL(sum(LOAD79_AMT),0) sumLOAD79_AMT,
--			--(select C.FEE_RATE from GM_CONTRACT_D C where C.MERCHANT_NO = DATA1.MERCHANT_NO
--			--										  and DATA1.CPT_DATE between C.EFF_DATE_FROM and C.EFF_DATE_TO
--			--										  and C.SETTLE_TYPE = 'B' and FEE_CAL_FLG = 'AM'
--			--										  and @EXEC_CPT_DATE_B between C.EFF_DATE_FROM and C.EFF_DATE_TO) B__AM
--	from 
--	(
--		--�ʳf�B�ʳf�����B�[�ȡB�[�Ȩ���
--		select	D1.CPT_DATE, D1.MERCHANT_NO, 
--				Case when D2.MERCHANT_SUB_NO = 'POS' then 'FS1' 
--					 when D2.MERCHANT_SUB_NO IS NULL then 'FS1' 
--					 else '0' end SEQ_NO,
--				D2.MERCHANT_SUB_NO,
--				ISNULL(sum(D1.PCHA),0) - ISNULL(sum(D1.UTUT3_AMTA),0) sumPCHA, 
--				ISNULL(sum(D1.PCHRA),0) - ISNULL(sum(D1.UTUT3_AMTRA),0) sumPCHRA, 
--				ISNULL(sum(D1.LOADA),0) sumLOADA, 
--				ISNULL(sum(D1.LOADRA),0) sumLOADRA, 
--				ISNULL(sum(D1.UTUT3_AMTA),0) sumUTUT3_AMTA, 
--				ISNULL(sum(D1.UTUT3_AMTRA),0) sumUTUT3_AMTRA,
--				NULL LOAD74_CNT,
--				NULL LOAD74_AMT,
--				NULL LOAD75_CNT,
--				NULL LOAD75_AMT,
--				NULL LOAD77_CNT,
--				NULL LOAD77_AMT,
--				NULL LOAD79_CNT,
--				NULL LOAD79_AMT
--		from (
--			SELECT	CPT_DATE,
--					MERCHANT_NO,
--					'POS' MERCHANT_SUB_NO,
--					SUM(isnull(PCH_AMT  ,0)) PCHA,		--�ʳf�`�B
--					SUM(isnull(PCHR_AMT ,0)) PCHRA,		--�h�f�`�B
--					SUM(isnull(LOAD_AMT ,0)) LOADA,		--�[���B
--					SUM(isnull(LOADR_AMT,0)) LOADRA,	--�[�Ȩ����B
--					NULL UTUT3_AMTA,					--�N�����`�B
--					NULL UTUT3_AMTRA					--�N�������
--				FROM AM_ISET_MERC_TRANS_LOG_D 
--				where CPT_DATE between @EXEC_CPT_DATE_B and @EXEC_CPT_DATE_E
--				  and SRC_FLG = 'TXLOG'
--				GROUP BY CPT_DATE, MERCHANT_NO
--			union all 
--			SELECT	CPT_DATE,
--					MERCHANT_NO,
--					'POS' MERCHANT_NO_SUB,
--					NULL PCHA,		--�ʳf�`�B
--					NULL PCHRA,		--�h�f�`�B
--					NULL LOADA,		--�[���B
--					NULL LOADRA,	--�[�Ȩ����B
--					SUM(isnull(UT_AMT ,0) + isnull(UT3_AMT ,0)) UTUT3_AMTA,	--�N�����`�B
--					NULL UTUT3_AMTRA	--�N�������
--				FROM AM_ISET_MERC_TRANS_UTLOG_D  
--				where CPT_DATE between @EXEC_CPT_DATE_B and @EXEC_CPT_DATE_E
--				  and SRC_FLG = 'TXLOG'
--				  and TRANS_TYPE = '21'
--				GROUP BY CPT_DATE, MERCHANT_NO
--			union all 
--			SELECT	CPT_DATE,
--					MERCHANT_NO,
--					'POS' MERCHANT_NO_SUB,
--					NULL PCHA,		--�ʳf�`�B
--					NULL PCHRA,		--�h�f�`�B
--					NULL LOADA,		--�[���B
--					NULL LOADRA,	--�[�Ȩ����B
--					NULL UTUT3_AMTA,--�N�����`�B
--					SUM(isnull(UT_AMT ,0) + isnull(UT3_AMT ,0)) UTUT3_AMTRA	--�N�������
--				FROM AM_ISET_MERC_TRANS_UTLOG_D  
--				where CPT_DATE between @EXEC_CPT_DATE_B and @EXEC_CPT_DATE_E
--				  and SRC_FLG = 'TXLOG'
--				  and TRANS_TYPE = '23'
--				GROUP BY CPT_DATE, MERCHANT_NO
--		)D1 
--		left join GM_MERCHANT_SUB D2 on D1.MERCHANT_NO = D2.MERCHANT_NO
--									and D1.MERCHANT_SUB_NO = D2.MERCHANT_SUB_NO
--		GROUP BY D1.CPT_DATE, D1.MERCHANT_NO, D2.MERCHANT_SUB_NO
--		union all 
--		--�۰ʥ[�ȡB�۰ʥ[�Ȩ����B���u�۰ʥ[��
--		--�B�bGM_BANK_CPT_SUM_SUB �S���ȡA�N��ӯS�����c�S�����U�@���h
--		SELECT	A1.CPT_DATE,
--				A1.MERCHANT_NO,
--				'FS1' SEQ_NO, 
--				NULL MERCHANT_SUB_NO,
--				NULL sumPCHA, 
--				NULL sumPCHRA, 
--				NULL sumLOADA, 
--				NULL sumLOADRA, 
--				NULL sumUTUT3_AMTA, 
--				NULL sumUTUT3_AMTRA,
--				SUM(A1.LOAD74_CNT) LOAD74_CNT,
--				SUM(A1.LOAD74_AMT) LOAD74_AMT,
--				SUM(A1.LOAD75_CNT) LOAD75_CNT,
--				SUM(A1.LOAD75_AMT) LOAD75_AMT,
--				SUM(A1.LOAD77_CNT) LOAD77_CNT, -- (add by 20160415 rita �s�W 77���u�۰ʥ[��)
--				SUM(A1.LOAD77_AMT) LOAD77_AMT, -- (add by 20160415 rita �s�W 77���u�۰ʥ[��)
--				NULL LOAD79_CNT,
--				NULL LOAD79_AMT
--			FROM GM_BANK_CPT_SUM A1
--			left join (
--						select distinct MERCHANT_NO, CPT_DATE, SETTLE_DATE
--						from GM_BANK_CPT_SUM_SUB
--						where CPT_DATE between @EXEC_CPT_DATE_B and @EXEC_CPT_DATE_E	
--			) A2 on A1.MERCHANT_NO = A2.MERCHANT_NO
--				and A1.CPT_DATE = A2.CPT_DATE
--				and A1.SETTLE_DATE = A2.SETTLE_DATE
--			where A1.CPT_DATE between @EXEC_CPT_DATE_B and @EXEC_CPT_DATE_E
--			  and A2.MERCHANT_NO is null
--			GROUP BY A1.CPT_DATE, A1.MERCHANT_NO
--		union all 
--		--�۰ʥ[�ȡB�۰ʥ[�Ȩ����B���u�۰ʥ[��
--		--�B�bGM_BANK_CPT_SUM_SUB ���ȡA�N��ӯS�����c�����U�@���h
--		--�h�H�]�w���@���n����ʳf�B�ʳf����
--		SELECT	A1.CPT_DATE,
--				A1.MERCHANT_NO,
--				Case when A2.MERCHANT_SUB_NO = 'POS' then 'FS1' else '' end SEQ_NO,
--				A2.MERCHANT_SUB_NO,
--				NULL sumPCHA, 
--				NULL sumPCHRA, 
--				NULL sumLOADA, 
--				NULL sumLOADRA, 
--				NULL sumUTUT3_AMTA, 
--				NULL sumUTUT3_AMTRA,
--				SUM(A2.LOAD74_CNT) LOAD74_CNT,
--				SUM(A2.LOAD74_AMT) LOAD74_AMT,
--				SUM(A2.LOAD75_CNT) LOAD75_CNT,
--				SUM(A2.LOAD75_AMT) LOAD75_AMT,
--				SUM(A2.LOAD77_CNT) LOAD77_CNT, -- (add by 20160415 rita �s�W 77���u�۰ʥ[��)
--				SUM(A2.LOAD77_AMT) LOAD77_AMT, -- (add by 20160415 rita �s�W 77���u�۰ʥ[��)
--				SUM(A2.LOAD79_CNT) LOAD79_CNT,
--				SUM(A2.LOAD79_AMT) LOAD79_AMT
--			FROM GM_BANK_CPT_SUM A1
--			inner join GM_BANK_CPT_SUM_SUB A2 on A1.MERCHANT_NO = A2.MERCHANT_NO
--											and A1.BANK_MERCHANT = A2.BANK_MERCHANT
--											and A1.CPT_DATE = A2.CPT_DATE
--											and A1.SETTLE_DATE = A2.SETTLE_DATE
--			where A1.CPT_DATE between @EXEC_CPT_DATE_B and @EXEC_CPT_DATE_E
--			GROUP BY A1.CPT_DATE, A1.MERCHANT_NO, A2.MERCHANT_SUB_NO
--	)DATA1
--	GROUP BY DATA1.CPT_DATE, DATA1.MERCHANT_NO, DATA1.SEQ_NO, DATA1.MERCHANT_SUB_NO
--	) DATA0 
--	GROUP BY DATA0.MERCHANT_NO, DATA0.MERCHANT_SUB_NO,P_CM_AM,L_CM_AM,A__AM
--)DATA
--inner join GM_MERCHANT M1 on DATA.MERCHANT_NO = M1.MERCHANT_NO
--							and M1.MERCHANT_NO = case when @EXEC_MERCHANT_NO = 'ALL' then M1.MERCHANT_NO else @EXEC_MERCHANT_NO end 
--left join GM_MERCHANT_SUB M2 on DATA.MERCHANT_SUB_NO = M2.MERCHANT_SUB_NO
----WHERE DATA.MERCHANT_SUB_NO IS NULL OR (DATA.MERCHANT_SUB_NO<>'IBN' AND DATA.MERCHANT_SUB_NO<>'POS') 
--order by M1.MERCHANT_NO--, DATA.CPT_DATE
--END
--============================================================================
--20190416
DECLARE @TAX_NO VARCHAR(8) --TAX_NO �ΨӧP�_�t�|�M���|���̾ڡAY �t�|�A�P��ӭp���k�AN���|�A���|�P���O����O�A�|��������O��0.05(5%)



if (@EXEC_MERCHANT_NO in (SELECT MERCHANT_NO FROM GM_MERCHANT WHERE MERCHANT_TYPE = 'MUTI_MERC'))
BEGIN
SELECT @TAX_NO = TAX_NO FROM GM_CONTRACT_MUTI_MERC_D WHERE MERCHANT_NO = @EXEC_MERCHANT_NO AND CONTRACT_TYPE = 'P1' AND FEE_KIND = 'CM' AND FEE_CAL_FLG = 'AM'
select  M1.INVOICE_NO '�νs',

		DATA.MERCHANT_NO '�S���N�X',
		ISNULL(ga.MERCHANT_NO_ACT_M,' ') '�����N��������(����O)',
		ISNULL(gtm.GROUP_NAME,' ') '�S���ʽ�',
		case when ga.REM_TYPE in ('A004','C001') AND ga.SETTLE_RULE = 'D' THEN '6111'
		WHEN ga.ORDER_NO = 'Y' AND ga.SET_GROUP_M = 'Y' THEN '6116' 
		WHEN ga.ORDER_NO = 'Y' AND ga.SET_GROUP_M = 'N' THEN '6112'
		WHEN ga.ORDER_NO = 'N' AND ga.SET_GROUP_M = 'Y' THEN '6106'
		WHEN ga.ORDER_NO = 'N' AND ga.SET_GROUP_M = 'N' THEN '6102' 
		ELSE '' END '��O�N��',

		Case when M2.MERCHANT_SUB_STNAME is null then M1.MERCHANT_NAME else M2.MERCHANT_SUB_STNAME end '�S�����c�W��',
		M1.MERCHANT_STNAME '²��',
		ISNULL((DATA.sumPCHA-DATA.sumPCHRA),0) as '�ʳf�b�B', 
		'' AS "�ʳf����",
		'' AS "�ʳf�ձb",
		Convert(FLOAT(50),ISNULL(DATA.P_CM_AM * 100,0)) as			'�ʳf����O�v', 
		CASE WHEN @TAX_NO = 'N' THEN ISNULL(cast(ROUND(cast(ROUND((DATA.sumPCHA-DATA.sumPCHRA)*DATA.P_CM_AM,0) as decimal(20,5))* 0.05,0) as bigint),0) + ISNULL(cast(ROUND((DATA.sumPCHA-DATA.sumPCHRA)*DATA.P_CM_AM,0) as bigint),0) ELSE ISNULL(cast(ROUND((DATA.sumPCHA-DATA.sumPCHRA)*DATA.P_CM_AM,0) as bigint),0) END	as '���O����O', --���|+�W�|�� 2019.8.7
		CASE WHEN @TAX_NO = 'N' THEN ISNULL(cast(ROUND((DATA.sumPCHA-DATA.sumPCHRA)*DATA.P_CM_AM,0) as bigint),0) ELSE 	ISNULL(cast(ROUND(cast(ROUND((DATA.sumPCHA-DATA.sumPCHRA)*DATA.P_CM_AM,0) as decimal(20,5))/1.05,0) as bigint),0) END   as '���O����O���|',
		CASE WHEN @TAX_NO = 'N' THEN ISNULL(cast(ROUND(cast(ROUND((DATA.sumPCHA-DATA.sumPCHRA)*DATA.P_CM_AM,0) as decimal(20,5))* 0.05,0) as bigint),0) ELSE ISNULL(cast(ROUND((DATA.sumPCHA-DATA.sumPCHRA)*DATA.P_CM_AM,0) as bigint)-cast(ROUND(cast(ROUND((DATA.sumPCHA-DATA.sumPCHRA)*DATA.P_CM_AM,0) as decimal(15,5))/1.05,0) as bigint),0) END as '���O����O�|��',
		ISNULL((DATA.sumLOADA-DATA.sumLOADRA),0) '�[�Ȳb�B', 
		'' AS "�[�ȭ���",
		'' AS "�[�Ƚձb",
		Convert(FLOAT(50),ISNULL(DATA.L_CM_AM * 100,0)) as			'�[�Ȥ���O�v', 
		ISNULL(cast(ROUND((DATA.sumLOADA-DATA.sumLOADRA)*DATA.L_CM_AM,0) as bigint),0) as '�[�Ȥ���O',
		ISNULL(cast(ROUND(cast(ROUND((DATA.sumLOADA-DATA.sumLOADRA)*DATA.L_CM_AM,0) as decimal(20,5))/1.05,0) as bigint),0) as '�[�Ȥ���O���|',
		ISNULL(cast(ROUND((DATA.sumLOADA-DATA.sumLOADRA)*DATA.L_CM_AM,0) as bigint)-cast(ROUND(cast(ROUND((DATA.sumLOADA-DATA.sumLOADRA)*DATA.L_CM_AM,0) as decimal(20,5))/1.05,0) as bigint),0) as '�[�Ȥ���O�|��',
		ISNULL(DATA.sumLOAD_AMT,0)									'�۰ʥ[���B',
		'' AS "�۰ʥ[�ȭ���",
		'' AS "�۰ʥ[�Ƚձb",
		Convert(FLOAT(50),ISNULL(DATA.A__AM * 100,0)) as			'�۰ʥ[�Ȥ���O�v', 
		ISNULL(cast(ROUND(sumLOAD_AMT*DATA.A__AM,0) as bigint),0) as	'�۰ʥ[�Ȥ���O',
		ISNULL(cast(ROUND(cast(ROUND(sumLOAD_AMT*DATA.A__AM,0) as decimal(20,5))/1.05,0) as bigint),0) as '�۰ʥ[�Ȥ���O���|',
		ISNULL(cast(ROUND(sumLOAD_AMT*DATA.A__AM,0) as bigint)-cast(ROUND(cast(ROUND(sumLOAD_AMT*DATA.A__AM,0) as decimal(20,5))/1.05,0) as bigint),0) as '�۰ʥ[�Ȥ���O�|��'
		
from (
	SELECT MERCHANT_NO,MERCHANT_SUB_NO,SUM(sumPCHA) AS sumPCHA,SUM(sumPCHRA) AS sumPCHRA,P_CM_AM,SUM(sumLOADA) AS sumLOADA,SUM(sumLOADRA) AS sumLOADRA,L_CM_AM,SUM(sumLOAD_AMT) AS sumLOAD_AMT,A__AM FROM 
	(
	select	DATA1.CPT_DATE, DATA1.MERCHANT_NO, DATA1.SEQ_NO, DATA1.MERCHANT_SUB_NO,
			ISNULL(sum(DATA1.sumPCHA),0) sumPCHA,
			ISNULL(sum(DATA1.sumPCHRA),0) sumPCHRA,
						(select C.FEE_RATE from GM_CONTRACT_MUTI_MERC_D C where C.MERCHANT_NO = DATA1.MERCHANT_NO
													  and DATA1.CPT_DATE between C.EFF_DATE_FROM and C.EFF_DATE_TO
													  and C.CONTRACT_TYPE = 'P1' AND FEE_KIND = 'CM' AND FEE_CAL_FLG = 'AM'
													  and @EXEC_CPT_DATE_B between C.EFF_DATE_FROM and C.EFF_DATE_TO
													  ) P_CM_AM,
			ISNULL(sum(DATA1.sumLOADA),0) sumLOADA,
			ISNULL(sum(DATA1.sumLOADRA),0) sumLOADRA,
			(select C.FEE_RATE from GM_CONTRACT_MUTI_MERC_D C where C.MERCHANT_NO = DATA1.MERCHANT_NO
													  and DATA1.CPT_DATE between C.EFF_DATE_FROM and C.EFF_DATE_TO
													  and C.CONTRACT_TYPE = 'L1' AND FEE_CAL_FLG = 'AM'
													  and @EXEC_CPT_DATE_B between C.EFF_DATE_FROM and C.EFF_DATE_TO) L_CM_AM,
			--ISNULL(sum(DATA1.sumUTUT3_AMTA),0) sumUTUT3_AMTA,
			--ISNULL(sum(DATA1.sumUTUT3_AMTRA),0) sumUTUT3_AMTRA,
			--(select C.FEE_RATE from GM_CONTRACT_D C where C.MERCHANT_NO = DATA1.MERCHANT_NO
			--										  and DATA1.CPT_DATE between C.EFF_DATE_FROM and C.EFF_DATE_TO
			--										  and C.SETTLE_TYPE = 'P' and C.FEE_KIND = 'UT' and FEE_CAL_FLG = 'AM'
			--										  and @EXEC_CPT_DATE_B between C.EFF_DATE_FROM and C.EFF_DATE_TO) P_UT_AM,
			ISNULL(sum(LOAD74_AMT),0) + ISNULL(sum(LOAD75_AMT),0) + ISNULL(sum(LOAD77_AMT),0) sumLOAD_AMT,  --(add by 20160415 rita �s�W 77���u�۰ʥ[��)
			(select C.FEE_RATE from GM_CONTRACT_MUTI_MERC_D C where C.MERCHANT_NO = DATA1.MERCHANT_NO
													  and DATA1.CPT_DATE between C.EFF_DATE_FROM and C.EFF_DATE_TO
													  and C.CONTRACT_TYPE = 'L2' AND FEE_CAL_FLG = 'AM'
													  and @EXEC_CPT_DATE_B between C.EFF_DATE_FROM and C.EFF_DATE_TO) A__AM
			--ISNULL(sum(LOAD79_AMT),0) sumLOAD79_AMT,
			--(select C.FEE_RATE from GM_CONTRACT_D C where C.MERCHANT_NO = DATA1.MERCHANT_NO
			--										  and DATA1.CPT_DATE between C.EFF_DATE_FROM and C.EFF_DATE_TO
			--										  and C.SETTLE_TYPE = 'B' and FEE_CAL_FLG = 'AM'
			--										  and @EXEC_CPT_DATE_B between C.EFF_DATE_FROM and C.EFF_DATE_TO) B__AM
	from 
	(
		--�ʳf�B�ʳf�����B�[�ȡB�[�Ȩ���
		select	D1.CPT_DATE, D1.MERCHANT_NO, 
				Case when D2.MERCHANT_SUB_NO = 'POS' then 'FS1' 
					 when D2.MERCHANT_SUB_NO IS NULL then 'FS1' 
					 else '0' end SEQ_NO,
				D2.MERCHANT_SUB_NO,
				ISNULL(sum(D1.PCHA),0) - ISNULL(sum(D1.UTUT3_AMTA),0) sumPCHA, 
				ISNULL(sum(D1.PCHRA),0) - ISNULL(sum(D1.UTUT3_AMTRA),0) sumPCHRA, 
				ISNULL(sum(D1.LOADA),0) sumLOADA, 
				ISNULL(sum(D1.LOADRA),0) sumLOADRA, 
				ISNULL(sum(D1.UTUT3_AMTA),0) sumUTUT3_AMTA, 
				ISNULL(sum(D1.UTUT3_AMTRA),0) sumUTUT3_AMTRA,
				NULL LOAD74_CNT,
				NULL LOAD74_AMT,
				NULL LOAD75_CNT,
				NULL LOAD75_AMT,
				NULL LOAD77_CNT,
				NULL LOAD77_AMT,
				NULL LOAD79_CNT,
				NULL LOAD79_AMT
		from (
			SELECT	CPT_DATE,
					MERCHANT_NO,
					'POS' MERCHANT_SUB_NO,
					SUM(isnull(PCH_AMT  ,0)) PCHA,		--�ʳf�`�B
					SUM(isnull(PCHR_AMT ,0)) PCHRA,		--�h�f�`�B
					SUM(isnull(LOAD_AMT ,0)) LOADA,		--�[���B
					SUM(isnull(LOADR_AMT,0)) LOADRA,	--�[�Ȩ����B
					NULL UTUT3_AMTA,					--�N�����`�B
					NULL UTUT3_AMTRA					--�N�������
				FROM AM_ISET_MERC_TRANS_LOG_D 
				where CPT_DATE between @EXEC_CPT_DATE_B and @EXEC_CPT_DATE_E
				  and SRC_FLG = 'TXLOG'
				GROUP BY CPT_DATE, MERCHANT_NO
			union all 
			SELECT	CPT_DATE,
					MERCHANT_NO,
					'POS' MERCHANT_NO_SUB,
					NULL PCHA,		--�ʳf�`�B
					NULL PCHRA,		--�h�f�`�B
					NULL LOADA,		--�[���B
					NULL LOADRA,	--�[�Ȩ����B
					SUM(isnull(UT_AMT ,0) + isnull(UT3_AMT ,0)) UTUT3_AMTA,	--�N�����`�B
					NULL UTUT3_AMTRA	--�N�������
				FROM AM_ISET_MERC_TRANS_UTLOG_D  
				where CPT_DATE between @EXEC_CPT_DATE_B and @EXEC_CPT_DATE_E
				  and SRC_FLG = 'TXLOG'
				  and TRANS_TYPE = '21'
				GROUP BY CPT_DATE, MERCHANT_NO
			union all 
			SELECT	CPT_DATE,
					MERCHANT_NO,
					'POS' MERCHANT_NO_SUB,
					NULL PCHA,		--�ʳf�`�B
					NULL PCHRA,		--�h�f�`�B
					NULL LOADA,		--�[���B
					NULL LOADRA,	--�[�Ȩ����B
					NULL UTUT3_AMTA,--�N�����`�B
					SUM(isnull(UT_AMT ,0) + isnull(UT3_AMT ,0)) UTUT3_AMTRA	--�N�������
				FROM AM_ISET_MERC_TRANS_UTLOG_D  
				where CPT_DATE between @EXEC_CPT_DATE_B and @EXEC_CPT_DATE_E
				  and SRC_FLG = 'TXLOG'
				  and TRANS_TYPE = '23'
				GROUP BY CPT_DATE, MERCHANT_NO
		)D1 
		left join GM_MERCHANT_SUB D2 on D1.MERCHANT_NO = D2.MERCHANT_NO
									and D1.MERCHANT_SUB_NO = D2.MERCHANT_SUB_NO
		GROUP BY D1.CPT_DATE, D1.MERCHANT_NO, D2.MERCHANT_SUB_NO
		union all 
		--�۰ʥ[�ȡB�۰ʥ[�Ȩ����B���u�۰ʥ[��
		--�B�bGM_BANK_CPT_SUM_SUB �S���ȡA�N��ӯS�����c�S�����U�@���h
		SELECT	A1.CPT_DATE,
				A1.MERCHANT_NO,
				'FS1' SEQ_NO, 
				NULL MERCHANT_SUB_NO,
				NULL sumPCHA, 
				NULL sumPCHRA, 
				NULL sumLOADA, 
				NULL sumLOADRA, 
				NULL sumUTUT3_AMTA, 
				NULL sumUTUT3_AMTRA,
				SUM(A1.LOAD74_CNT) LOAD74_CNT,
				SUM(A1.LOAD74_AMT) LOAD74_AMT,
				SUM(A1.LOAD75_CNT) LOAD75_CNT,
				SUM(A1.LOAD75_AMT) LOAD75_AMT,
				SUM(A1.LOAD77_CNT) LOAD77_CNT, -- (add by 20160415 rita �s�W 77���u�۰ʥ[��)
				SUM(A1.LOAD77_AMT) LOAD77_AMT, -- (add by 20160415 rita �s�W 77���u�۰ʥ[��)
				NULL LOAD79_CNT,
				NULL LOAD79_AMT
			FROM GM_BANK_CPT_SUM A1
			left join (
						select distinct MERCHANT_NO, CPT_DATE, SETTLE_DATE
						from GM_BANK_CPT_SUM_SUB
						where CPT_DATE between @EXEC_CPT_DATE_B and @EXEC_CPT_DATE_E	
			) A2 on A1.MERCHANT_NO = A2.MERCHANT_NO
				and A1.CPT_DATE = A2.CPT_DATE
				and A1.SETTLE_DATE = A2.SETTLE_DATE
			where A1.CPT_DATE between @EXEC_CPT_DATE_B and @EXEC_CPT_DATE_E
			  and A2.MERCHANT_NO is null
			GROUP BY A1.CPT_DATE, A1.MERCHANT_NO
		union all 
		--�۰ʥ[�ȡB�۰ʥ[�Ȩ����B���u�۰ʥ[��
		--�B�bGM_BANK_CPT_SUM_SUB ���ȡA�N��ӯS�����c�����U�@���h
		--�h�H�]�w���@���n����ʳf�B�ʳf����
		SELECT	A1.CPT_DATE,
				A1.MERCHANT_NO,
				Case when A2.MERCHANT_SUB_NO = 'POS' then 'FS1' else '' end SEQ_NO,
				A2.MERCHANT_SUB_NO,
				NULL sumPCHA, 
				NULL sumPCHRA, 
				NULL sumLOADA, 
				NULL sumLOADRA, 
				NULL sumUTUT3_AMTA, 
				NULL sumUTUT3_AMTRA,
				SUM(A2.LOAD74_CNT) LOAD74_CNT,
				SUM(A2.LOAD74_AMT) LOAD74_AMT,
				SUM(A2.LOAD75_CNT) LOAD75_CNT,
				SUM(A2.LOAD75_AMT) LOAD75_AMT,
				SUM(A2.LOAD77_CNT) LOAD77_CNT, -- (add by 20160415 rita �s�W 77���u�۰ʥ[��)
				SUM(A2.LOAD77_AMT) LOAD77_AMT, -- (add by 20160415 rita �s�W 77���u�۰ʥ[��)
				SUM(A2.LOAD79_CNT) LOAD79_CNT,
				SUM(A2.LOAD79_AMT) LOAD79_AMT
			FROM GM_BANK_CPT_SUM A1
			inner join GM_BANK_CPT_SUM_SUB A2 on A1.MERCHANT_NO = A2.MERCHANT_NO
											and A1.BANK_MERCHANT = A2.BANK_MERCHANT
											and A1.CPT_DATE = A2.CPT_DATE
											and A1.SETTLE_DATE = A2.SETTLE_DATE
			where A1.CPT_DATE between @EXEC_CPT_DATE_B and @EXEC_CPT_DATE_E
			GROUP BY A1.CPT_DATE, A1.MERCHANT_NO, A2.MERCHANT_SUB_NO
	)DATA1
	GROUP BY DATA1.CPT_DATE, DATA1.MERCHANT_NO, DATA1.SEQ_NO, DATA1.MERCHANT_SUB_NO
	) DATA0 
	GROUP BY DATA0.MERCHANT_NO, DATA0.MERCHANT_SUB_NO,P_CM_AM,L_CM_AM,A__AM
)DATA
inner join GM_MERCHANT M1 on DATA.MERCHANT_NO = M1.MERCHANT_NO
							and M1.MERCHANT_NO = case when @EXEC_MERCHANT_NO = 'ALL' then M1.MERCHANT_NO else @EXEC_MERCHANT_NO end 
left join GM_MERCHANT_SUB M2 on DATA.MERCHANT_SUB_NO = M2.MERCHANT_SUB_NO
left join GM_MERCHANT_ACT ga on DATA.MERCHANT_NO = ga.MERCHANT_NO
INNER JOIN GM_MERCHANT_TYPE_D gtd on M1.MERCHANT_NO = gtd.MERCHANT_NO
inner join GM_MERCHANT_TYPE_M gtm on gtd.GROUP_ID = gtm.GROUP_ID
--WHERE DATA.MERCHANT_SUB_NO IS NULL OR (DATA.MERCHANT_SUB_NO<>'IBN' AND DATA.MERCHANT_SUB_NO<>'POS') 
order by M1.MERCHANT_NO--, DATA.CPT_DATE
END
--�D�e�~
ELSE
BEGIN
SELECT @TAX_NO = TAX_NO FROM GM_CONTRACT_D WHERE MERCHANT_NO = @EXEC_MERCHANT_NO AND SETTLE_TYPE = 'P' and FEE_KIND = 'CM' and FEE_CAL_FLG = 'AM'
select  M1.INVOICE_NO '�νs',
		
		DATA.MERCHANT_NO '�S���N�X',
		ISNULL(ga.MERCHANT_NO_ACT_M,' ') '�����N��������(����O)',
		ISNULL(gtm.GROUP_NAME,' ') '�S���ʽ�',
		case when ga.REM_TYPE in ('A004','C001') AND ga.SETTLE_RULE = 'D' THEN '6111'
		WHEN ga.ORDER_NO = 'Y' AND ga.SET_GROUP_M = 'Y' THEN '6116' 
		WHEN ga.ORDER_NO = 'Y' AND ga.SET_GROUP_M = 'N' THEN '6112'
		WHEN ga.ORDER_NO = 'N' AND ga.SET_GROUP_M = 'Y' THEN '6106'
		WHEN ga.ORDER_NO = 'N' AND ga.SET_GROUP_M = 'N' THEN '6102' 
		ELSE '' END '��O�N��',

		Case when M2.MERCHANT_SUB_STNAME is null then M1.MERCHANT_NAME else M2.MERCHANT_SUB_STNAME end '�S�����c�W��',
		M1.MERCHANT_STNAME '²��',
		
		ISNULL((DATA.sumPCHA-DATA.sumPCHRA),0) as '�ʳf�b�B', 
		'' AS "�ʳf����",
		'' AS "�ʳf�ձb",
		Convert(FLOAT(50),ISNULL(DATA.P_CM_AM * 100,0)) as			'�ʳf����O�v', 
		CASE WHEN @TAX_NO = 'N' THEN ISNULL(cast(ROUND(cast(ROUND((DATA.sumPCHA-DATA.sumPCHRA)*DATA.P_CM_AM,0) as decimal(20,5))* 0.05,0) as bigint),0) + ISNULL(cast(ROUND((DATA.sumPCHA-DATA.sumPCHRA)*DATA.P_CM_AM,0) as bigint),0) ELSE ISNULL(cast(ROUND((DATA.sumPCHA-DATA.sumPCHRA)*DATA.P_CM_AM,0) as bigint),0) END	as '���O����O',--2019.8.7 �W�[
		CASE WHEN @TAX_NO = 'N' THEN ISNULL(cast(ROUND((DATA.sumPCHA-DATA.sumPCHRA)*DATA.P_CM_AM,0) as bigint),0) ELSE 
		ISNULL(cast(ROUND(cast(ROUND((DATA.sumPCHA-DATA.sumPCHRA)*DATA.P_CM_AM,0) as decimal(20,5))/1.05,0) as bigint),0) END   as '���O����O���|',
		CASE WHEN @TAX_NO = 'N' THEN ISNULL(cast(ROUND(cast(ROUND((DATA.sumPCHA-DATA.sumPCHRA)*DATA.P_CM_AM,0) as decimal(20,5))* 0.05,0) as bigint),0) ELSE
		ISNULL(cast(ROUND((DATA.sumPCHA-DATA.sumPCHRA)*DATA.P_CM_AM,0) as bigint)-cast(ROUND(cast(ROUND((DATA.sumPCHA-DATA.sumPCHRA)*DATA.P_CM_AM,0) as decimal(15,5))/1.05,0) as bigint),0) END as '���O����O�|��',
		ISNULL((DATA.sumLOADA-DATA.sumLOADRA),0) '�[�Ȳb�B', 
		'' AS "�[�ȭ���",
		'' AS "�[�Ƚձb",
		Convert(FLOAT(50),ISNULL(DATA.L_CM_AM * 100,0)) as			'�[�Ȥ���O�v', 
		ISNULL(cast(ROUND((DATA.sumLOADA-DATA.sumLOADRA)*DATA.L_CM_AM,0) as bigint),0) as '�[�Ȥ���O',
		ISNULL(cast(ROUND(cast(ROUND((DATA.sumLOADA-DATA.sumLOADRA)*DATA.L_CM_AM,0) as decimal(20,5))/1.05,0) as bigint),0) as '�[�Ȥ���O���|',
		ISNULL(cast(ROUND((DATA.sumLOADA-DATA.sumLOADRA)*DATA.L_CM_AM,0) as bigint)-cast(ROUND(cast(ROUND((DATA.sumLOADA-DATA.sumLOADRA)*DATA.L_CM_AM,0) as decimal(20,5))/1.05,0) as bigint),0) as '�[�Ȥ���O�|��',
		ISNULL(DATA.sumLOAD_AMT,0)									'�۰ʥ[���B',
		'' AS "�۰ʥ[�ȭ���",
		'' AS "�۰ʥ[�Ƚձb",
		Convert(FLOAT(50),ISNULL(DATA.A__AM * 100,0)) as			'�۰ʥ[�Ȥ���O�v', 
		ISNULL(cast(ROUND(sumLOAD_AMT*DATA.A__AM,0) as bigint),0) as	'�۰ʥ[�Ȥ���O',
		ISNULL(cast(ROUND(cast(ROUND(sumLOAD_AMT*DATA.A__AM,0) as decimal(20,5))/1.05,0) as bigint),0) as '�۰ʥ[�Ȥ���O���|',
		ISNULL(cast(ROUND(sumLOAD_AMT*DATA.A__AM,0) as bigint)-cast(ROUND(cast(ROUND(sumLOAD_AMT*DATA.A__AM,0) as decimal(20,5))/1.05,0) as bigint),0) as '�۰ʥ[�Ȥ���O�|��'
		
from (
	SELECT MERCHANT_NO,MERCHANT_SUB_NO,SUM(sumPCHA) AS sumPCHA,SUM(sumPCHRA) AS sumPCHRA,P_CM_AM,SUM(sumLOADA) AS sumLOADA,SUM(sumLOADRA) AS sumLOADRA,L_CM_AM,SUM(sumLOAD_AMT) AS sumLOAD_AMT,A__AM FROM 
	(
	select	DATA1.CPT_DATE, DATA1.MERCHANT_NO, DATA1.SEQ_NO, DATA1.MERCHANT_SUB_NO,
			ISNULL(sum(DATA1.sumPCHA),0) sumPCHA,
			ISNULL(sum(DATA1.sumPCHRA),0) sumPCHRA,
			(select C.FEE_RATE from GM_CONTRACT_D C where C.MERCHANT_NO = DATA1.MERCHANT_NO
													  and DATA1.CPT_DATE between C.EFF_DATE_FROM and C.EFF_DATE_TO
													  and C.SETTLE_TYPE = 'P' and C.FEE_KIND = 'CM' and FEE_CAL_FLG = 'AM'
													  and @EXEC_CPT_DATE_B between C.EFF_DATE_FROM and C.EFF_DATE_TO) P_CM_AM,
			ISNULL(sum(DATA1.sumLOADA),0) sumLOADA,
			ISNULL(sum(DATA1.sumLOADRA),0) sumLOADRA,
			(select C.FEE_RATE from GM_CONTRACT_D C where C.MERCHANT_NO = DATA1.MERCHANT_NO
													  and DATA1.CPT_DATE between C.EFF_DATE_FROM and C.EFF_DATE_TO
													  and C.SETTLE_TYPE = 'L' and FEE_CAL_FLG = 'AM'
													  and @EXEC_CPT_DATE_B between C.EFF_DATE_FROM and C.EFF_DATE_TO) L_CM_AM,
			--ISNULL(sum(DATA1.sumUTUT3_AMTA),0) sumUTUT3_AMTA,
			--ISNULL(sum(DATA1.sumUTUT3_AMTRA),0) sumUTUT3_AMTRA,
			--(select C.FEE_RATE from GM_CONTRACT_D C where C.MERCHANT_NO = DATA1.MERCHANT_NO
			--										  and DATA1.CPT_DATE between C.EFF_DATE_FROM and C.EFF_DATE_TO
			--										  and C.SETTLE_TYPE = 'P' and C.FEE_KIND = 'UT' and FEE_CAL_FLG = 'AM'
			--										  and @EXEC_CPT_DATE_B between C.EFF_DATE_FROM and C.EFF_DATE_TO) P_UT_AM,
			ISNULL(sum(LOAD74_AMT),0) + ISNULL(sum(LOAD75_AMT),0) + ISNULL(sum(LOAD77_AMT),0) sumLOAD_AMT,  --(add by 20160415 rita �s�W 77���u�۰ʥ[��)
			(select C.FEE_RATE from GM_CONTRACT_D C where C.MERCHANT_NO = DATA1.MERCHANT_NO
													  and DATA1.CPT_DATE between C.EFF_DATE_FROM and C.EFF_DATE_TO
													  and C.SETTLE_TYPE = 'A' and FEE_CAL_FLG = 'AM'
													  and @EXEC_CPT_DATE_B between C.EFF_DATE_FROM and C.EFF_DATE_TO) A__AM--,
			--ISNULL(sum(LOAD79_AMT),0) sumLOAD79_AMT,
			--(select C.FEE_RATE from GM_CONTRACT_D C where C.MERCHANT_NO = DATA1.MERCHANT_NO
			--										  and DATA1.CPT_DATE between C.EFF_DATE_FROM and C.EFF_DATE_TO
			--										  and C.SETTLE_TYPE = 'B' and FEE_CAL_FLG = 'AM'
			--										  and @EXEC_CPT_DATE_B between C.EFF_DATE_FROM and C.EFF_DATE_TO) B__AM
	from 
	(
		--�ʳf�B�ʳf�����B�[�ȡB�[�Ȩ���
		select	D1.CPT_DATE, D1.MERCHANT_NO, 
				Case when D2.MERCHANT_SUB_NO = 'POS' then 'FS1' 
					 when D2.MERCHANT_SUB_NO IS NULL then 'FS1' 
					 else '0' end SEQ_NO,
				D2.MERCHANT_SUB_NO,
				ISNULL(sum(D1.PCHA),0) - ISNULL(sum(D1.UTUT3_AMTA),0) sumPCHA, 
				ISNULL(sum(D1.PCHRA),0) - ISNULL(sum(D1.UTUT3_AMTRA),0) sumPCHRA, 
				ISNULL(sum(D1.LOADA),0) sumLOADA, 
				ISNULL(sum(D1.LOADRA),0) sumLOADRA, 
				ISNULL(sum(D1.UTUT3_AMTA),0) sumUTUT3_AMTA, 
				ISNULL(sum(D1.UTUT3_AMTRA),0) sumUTUT3_AMTRA,
				NULL LOAD74_CNT,
				NULL LOAD74_AMT,
				NULL LOAD75_CNT,
				NULL LOAD75_AMT,
				NULL LOAD77_CNT,
				NULL LOAD77_AMT,
				NULL LOAD79_CNT,
				NULL LOAD79_AMT
		from (
			SELECT	CPT_DATE,
					MERCHANT_NO,
					'POS' MERCHANT_SUB_NO,
					SUM(isnull(PCH_AMT  ,0)) PCHA,		--�ʳf�`�B
					SUM(isnull(PCHR_AMT ,0)) PCHRA,		--�h�f�`�B
					SUM(isnull(LOAD_AMT ,0)) LOADA,		--�[���B
					SUM(isnull(LOADR_AMT,0)) LOADRA,	--�[�Ȩ����B
					NULL UTUT3_AMTA,					--�N�����`�B
					NULL UTUT3_AMTRA					--�N�������
				FROM AM_ISET_MERC_TRANS_LOG_D 
				where CPT_DATE between @EXEC_CPT_DATE_B and @EXEC_CPT_DATE_E
				  and SRC_FLG = 'TXLOG'
				GROUP BY CPT_DATE, MERCHANT_NO
			union all 
			SELECT	CPT_DATE,
					MERCHANT_NO,
					'POS' MERCHANT_NO_SUB,
					NULL PCHA,		--�ʳf�`�B
					NULL PCHRA,		--�h�f�`�B
					NULL LOADA,		--�[���B
					NULL LOADRA,	--�[�Ȩ����B
					SUM(isnull(UT_AMT ,0) + isnull(UT3_AMT ,0)) UTUT3_AMTA,	--�N�����`�B
					NULL UTUT3_AMTRA	--�N�������
				FROM AM_ISET_MERC_TRANS_UTLOG_D  
				where CPT_DATE between @EXEC_CPT_DATE_B and @EXEC_CPT_DATE_E
				  and SRC_FLG = 'TXLOG'
				  and TRANS_TYPE = '21'
				GROUP BY CPT_DATE, MERCHANT_NO
			union all 
			SELECT	CPT_DATE,
					MERCHANT_NO,
					'POS' MERCHANT_NO_SUB,
					NULL PCHA,		--�ʳf�`�B
					NULL PCHRA,		--�h�f�`�B
					NULL LOADA,		--�[���B
					NULL LOADRA,	--�[�Ȩ����B
					NULL UTUT3_AMTA,--�N�����`�B
					SUM(isnull(UT_AMT ,0) + isnull(UT3_AMT ,0)) UTUT3_AMTRA	--�N�������
				FROM AM_ISET_MERC_TRANS_UTLOG_D  
				where CPT_DATE between @EXEC_CPT_DATE_B and @EXEC_CPT_DATE_E
				  and SRC_FLG = 'TXLOG'
				  and TRANS_TYPE = '23'
				GROUP BY CPT_DATE, MERCHANT_NO
		)D1 
		left join GM_MERCHANT_SUB D2 on D1.MERCHANT_NO = D2.MERCHANT_NO
									and D1.MERCHANT_SUB_NO = D2.MERCHANT_SUB_NO
		GROUP BY D1.CPT_DATE, D1.MERCHANT_NO, D2.MERCHANT_SUB_NO
		union all 
		--�۰ʥ[�ȡB�۰ʥ[�Ȩ����B���u�۰ʥ[��
		--�B�bGM_BANK_CPT_SUM_SUB �S���ȡA�N��ӯS�����c�S�����U�@���h
		SELECT	A1.CPT_DATE,
				A1.MERCHANT_NO,
				'FS1' SEQ_NO, 
				NULL MERCHANT_SUB_NO,
				NULL sumPCHA, 
				NULL sumPCHRA, 
				NULL sumLOADA, 
				NULL sumLOADRA, 
				NULL sumUTUT3_AMTA, 
				NULL sumUTUT3_AMTRA,
				SUM(A1.LOAD74_CNT) LOAD74_CNT,
				SUM(A1.LOAD74_AMT) LOAD74_AMT,
				SUM(A1.LOAD75_CNT) LOAD75_CNT,
				SUM(A1.LOAD75_AMT) LOAD75_AMT,
				SUM(A1.LOAD77_CNT) LOAD77_CNT, -- (add by 20160415 rita �s�W 77���u�۰ʥ[��)
				SUM(A1.LOAD77_AMT) LOAD77_AMT, -- (add by 20160415 rita �s�W 77���u�۰ʥ[��)
				NULL LOAD79_CNT,
				NULL LOAD79_AMT
			FROM GM_BANK_CPT_SUM A1
			left join (
						select distinct MERCHANT_NO, CPT_DATE, SETTLE_DATE
						from GM_BANK_CPT_SUM_SUB
						where CPT_DATE between @EXEC_CPT_DATE_B and @EXEC_CPT_DATE_E	
			) A2 on A1.MERCHANT_NO = A2.MERCHANT_NO
				and A1.CPT_DATE = A2.CPT_DATE
				and A1.SETTLE_DATE = A2.SETTLE_DATE
			where A1.CPT_DATE between @EXEC_CPT_DATE_B and @EXEC_CPT_DATE_E
			  and A2.MERCHANT_NO is null
			GROUP BY A1.CPT_DATE, A1.MERCHANT_NO
		union all 
		--�۰ʥ[�ȡB�۰ʥ[�Ȩ����B���u�۰ʥ[��
		--�B�bGM_BANK_CPT_SUM_SUB ���ȡA�N��ӯS�����c�����U�@���h
		--�h�H�]�w���@���n����ʳf�B�ʳf����
		SELECT	A1.CPT_DATE,
				A1.MERCHANT_NO,
				Case when A2.MERCHANT_SUB_NO = 'POS' then 'FS1' else '' end SEQ_NO,
				A2.MERCHANT_SUB_NO,
				NULL sumPCHA, 
				NULL sumPCHRA, 
				NULL sumLOADA, 
				NULL sumLOADRA, 
				NULL sumUTUT3_AMTA, 
				NULL sumUTUT3_AMTRA,
				SUM(A2.LOAD74_CNT) LOAD74_CNT,
				SUM(A2.LOAD74_AMT) LOAD74_AMT,
				SUM(A2.LOAD75_CNT) LOAD75_CNT,
				SUM(A2.LOAD75_AMT) LOAD75_AMT,
				SUM(A2.LOAD77_CNT) LOAD77_CNT, -- (add by 20160415 rita �s�W 77���u�۰ʥ[��)
				SUM(A2.LOAD77_AMT) LOAD77_AMT, -- (add by 20160415 rita �s�W 77���u�۰ʥ[��)
				SUM(A2.LOAD79_CNT) LOAD79_CNT,
				SUM(A2.LOAD79_AMT) LOAD79_AMT
			FROM GM_BANK_CPT_SUM A1
			inner join GM_BANK_CPT_SUM_SUB A2 on A1.MERCHANT_NO = A2.MERCHANT_NO
											and A1.BANK_MERCHANT = A2.BANK_MERCHANT
											and A1.CPT_DATE = A2.CPT_DATE
											and A1.SETTLE_DATE = A2.SETTLE_DATE
			where A1.CPT_DATE between @EXEC_CPT_DATE_B and @EXEC_CPT_DATE_E
			GROUP BY A1.CPT_DATE, A1.MERCHANT_NO, A2.MERCHANT_SUB_NO
	)DATA1
	GROUP BY DATA1.CPT_DATE, DATA1.MERCHANT_NO, DATA1.SEQ_NO, DATA1.MERCHANT_SUB_NO
	) DATA0 
	GROUP BY DATA0.MERCHANT_NO, DATA0.MERCHANT_SUB_NO,P_CM_AM,L_CM_AM,A__AM
)DATA
inner join GM_MERCHANT M1 on DATA.MERCHANT_NO = M1.MERCHANT_NO
							and M1.MERCHANT_NO = case when @EXEC_MERCHANT_NO = 'ALL' then M1.MERCHANT_NO else @EXEC_MERCHANT_NO end 
left join GM_MERCHANT_SUB M2 on DATA.MERCHANT_SUB_NO = M2.MERCHANT_SUB_NO
left join GM_MERCHANT_ACT ga on DATA.MERCHANT_NO = ga.MERCHANT_NO
INNER JOIN GM_MERCHANT_TYPE_D gtd on M1.MERCHANT_NO = gtd.MERCHANT_NO
inner join GM_MERCHANT_TYPE_M gtm on gtd.GROUP_ID = gtm.GROUP_ID
--WHERE DATA.MERCHANT_SUB_NO IS NULL OR (DATA.MERCHANT_SUB_NO<>'IBN' AND DATA.MERCHANT_SUB_NO<>'POS') 
order by M1.MERCHANT_NO--, DATA.CPT_DATE
END


