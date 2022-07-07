DECLARE @tabReport TABLE (MERCHANT_NO varchar(8), TRANS_DATE varchar(8), MERCHANT_STNAME varchar(50), CARD_KIND varchar(10),
						  sumPCH_AMT bigint, sumPCHR_AMT bigint, sumPCH_CNT bigint, sumPCHR_CNT bigint)
       insert @tabReport
select	MERCHANT_NO '�S�����c' ,TRANS_DATE '�����' ,MERCHANT_STNAME  '�S�����c�W��', CARD_KIND '�d�O', 
		sum(isnull([21],0)) '�ʳf�`�B', sum(isnull([23],0)) '�h�f�`�B', 
		sum(isnull([21C],0)) '�ʳf�`��', sum(isnull([23C],0)) '�h�f�`��' 
from (
		select TRANS_DATE, G.MERCHANT_NO, GM.MERCHANT_STNAME, CARD_KIND,
			   TRANS_TYPE, TRANS_TYPE+'C' as TRANS_TYPEC,
			   sum(TRANS_AMT) SUM_AMT, sum(TRANS_CNT) SUM_CNT 
		from GM_CPT_CARD_SUM_D G
		LEFT OUTER JOIN GM_MERCHANT GM ON GM.MERCHANT_NO=G.MERCHANT_NO
		WHERE G.TRANS_DATE BETWEEN @TRANS_DATE_B AND @TRANS_DATEE_E
		  AND G.CARD_KIND in (@KIND1,@KIND2,@KIND3) 
		  AND G.MERCHANT_NO = CASE when @MERCHANT_NO = 'ALL' then G.MERCHANT_NO else @MERCHANT_NO end
		  and TRANS_TYPE in ('21','23')
		group by TRANS_DATE,G.MERCHANT_NO,GM.MERCHANT_STNAME, CARD_KIND,G.TRANS_TYPE
	) T
PIVOT(sum(SUM_AMT) FOR TRANS_TYPE IN ([21],[23])) p 
PIVOT(sum(SUM_CNT) FOR TRANS_TYPEC IN ([21C],[23C])) p1 
group by TRANS_DATE,MERCHANT_NO,MERCHANT_STNAME, CARD_KIND

----------------------------------------------------------------------------------
--	��ܳ��� [���]
----------------------------------------------------------------------------------
--����
select '1' as showRank,'1' as showID,* from @tabReport
--�p�p 
union all
select '1' as showRank,'2' as showID,MERCHANT_NO, '', '', '�p�p', SUM(sumPCH_AMT), SUM(sumPCH_CNT), SUM(sumPCHR_AMT), SUM(sumPCHR_CNT) 
from @tabReport
group by MERCHANT_NO,MERCHANT_STNAME
--�`�p
union all
select '2' as showRank,'3' as showID, '','', '', '�`�p', SUM(sumPCH_AMT), SUM(sumPCH_CNT), SUM(sumPCHR_AMT), SUM(sumPCHR_CNT) 
from @tabReport
ORDER BY showRank, MERCHANT_NO, showID, TRANS_DATE, CARD_KIND