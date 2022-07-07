DECLARE @merchant VARCHAR(8)
DECLARE @merchantST VARCHAR(50)
DECLARE @counter DATE
DECLARE @report TABLE (merchantNo VARCHAR(8), transDate VARCHAR(8), merchantStname VARCHAR(50),cardKind VARCHAR(20),pSum DECIMAL(16,0),rSum DECIMAL(16,0),pCount BIGINT, rCount BIGINT,PurCount BIGINT,PurrCount BIGINT)

INSERT INTO @report 
SELECT * FROM 
(
select	MERCHANT_NO '�S�����c' ,TRANS_DATE '�����' ,MERCHANT_STNAME  '�S�����c�W��', CARD_KIND '�d�O', 
		sum(isnull([21],0)) '�ʳf�`�B', sum(isnull([23],0)) '�h�f�`�B', 
		sum(isnull([21C],0)) '�ʳf�`��', sum(isnull([23C],0)) '�h�f�`��' ,SUM(ISNULL([21S],0)) '�ʳf�d��',SUM(ISNULL([23S],0)) '�h�f�d��'
from (
		select TRANS_DATE, G.MERCHANT_NO, GM.MERCHANT_STNAME, CARD_KIND,
			   TRANS_TYPE, TRANS_TYPE+'C' as TRANS_TYPEC,TRANS_TYPE+'S' AS TRANS_TYPES,
			   sum(TRANS_AMT) SUM_AMT, sum(TRANS_CNT) SUM_CNT,SUM(ICC_CNT) ICC_CNT
		from [ICASHSQLDB].[iSettle2].[dbo].[GM_CPT_CARD_SUM_D] G
		LEFT OUTER JOIN [ICASHSQLDB].[iSettle2].[dbo].[GM_MERCHANT] GM ON GM.MERCHANT_NO=G.MERCHANT_NO
		WHERE G.TRANS_DATE BETWEEN @TRANS_DATE_B AND @TRANS_DATEE_E
		  AND G.CARD_KIND in (@KIND1,@KIND2,@KIND3) 
		  AND G.MERCHANT_NO = CASE when @MERCHANT_NO = 'ALL' then G.MERCHANT_NO else @MERCHANT_NO end
		  and TRANS_TYPE in ('21','23')
		group by TRANS_DATE,G.MERCHANT_NO,GM.MERCHANT_STNAME, CARD_KIND,G.TRANS_TYPE
	) T
PIVOT(sum(SUM_AMT) FOR TRANS_TYPE IN ([21],[23])) p 
PIVOT(sum(SUM_CNT) FOR TRANS_TYPEC IN ([21C],[23C])) p1 
PIVOT(SUM(ICC_CNT) FOR TRANS_TYPES IN ([21S],[23S])) p2 
group by TRANS_DATE,MERCHANT_NO,MERCHANT_STNAME, CARD_KIND

UNION ALL

select	MERCHANT_NO '�S�����c' ,TRANS_DATE '�����' ,MERCHANT_STNAME  '�S�����c�W��', CARD_KIND '�d�O', 
		sum(isnull([21],0)) '�ʳf�`�B', sum(isnull([23],0)) '�h�f�`�B', 
		sum(isnull([21C],0)) '�ʳf�`��', sum(isnull([23C],0)) '�h�f�`��' ,SUM(ISNULL([21S],0)) '�ʳf�d��',SUM(ISNULL([23S],0)) '�h�f�d��'
from (
		select TRANS_DATE, G.MERCHANT_NO, GM.MERCHANT_STNAME, CARD_KIND,
			   TRANS_TYPE, TRANS_TYPE+'C' as TRANS_TYPEC,TRANS_TYPE+'S' AS TRANS_TYPES,
			   sum(TRANS_AMT) SUM_AMT, sum(TRANS_CNT) SUM_CNT ,0 ICC_CNT
		from GM_CPT_CARD_SUM_D G
		LEFT OUTER JOIN [ICASHSQLDB].[iSettle2].[dbo].[GM_MERCHANT] GM ON GM.MERCHANT_NO=G.MERCHANT_NO
		WHERE G.TRANS_DATE BETWEEN @TRANS_DATE_B AND @TRANS_DATEE_E
		  AND G.CARD_KIND in (@KIND1,@KIND2,@KIND3) 
		  AND G.MERCHANT_NO = CASE when @MERCHANT_NO = 'ALL' then G.MERCHANT_NO else @MERCHANT_NO end
		  and TRANS_TYPE in ('21','23')
		group by TRANS_DATE,G.MERCHANT_NO,GM.MERCHANT_STNAME, CARD_KIND,G.TRANS_TYPE
	) T
PIVOT(sum(SUM_AMT) FOR TRANS_TYPE IN ([21],[23])) p 
PIVOT(sum(SUM_CNT) FOR TRANS_TYPEC IN ([21C],[23C])) p1 
PIVOT(SUM(ICC_CNT) FOR TRANS_TYPES IN ([21S],[23S])) p2 
group by TRANS_DATE,MERCHANT_NO,MERCHANT_STNAME, CARD_KIND
) x ORDER BY x.�S�����c, x.�����, x.�d�O

DECLARE merchantCur CURSOR FOR SELECT DISTINCT merchantNo,merchantStname FROM @report ORDER BY merchantNo

OPEN merchantCur

FETCH NEXT FROM merchantCur INTO @merchant,@merchantST

WHILE(@@FETCH_STATUS=0)
BEGIN

  SET @counter = CONVERT(DATE,@TRANS_DATE_B,112)


  WHILE(DATEDIFF(DAY,@counter,CONVERT(DATE,@TRANS_DATEE_E,112))>=0)
  BEGIN
	  DECLARE @rowNum1 INT
	  DECLARE @rowNum2 INT
	  DECLARE @rowNum3 INT

	  IF(@KIND1<>'')
	  BEGIN
		SET @rowNum1 = (SELECT COUNT(*) FROM @report WHERE merchantNo=@merchant AND transDate=CONVERT(VARCHAR(8),@counter,112) AND cardKind='�@�N�d' )
		IF(@rowNum1=0)
		BEGIN
			INSERT INTO @report VALUES (@merchant,CONVERT(VARCHAR(8),@counter,112),@merchantST,'�@�N�d',NULL,NULL,NULL,NULL,NULL,NULL)
		END
	  END
	  
	  IF(@KIND2<>'')
	  BEGIN
		SET @rowNum2 = (SELECT COUNT(*) FROM @report WHERE merchantNo=@merchant AND transDate=CONVERT(VARCHAR(8),@counter,112) AND cardKind='�G�N�d' )
		IF(@rowNum2=0)
		BEGIN
			INSERT INTO @report VALUES (@merchant,CONVERT(VARCHAR(8),@counter,112),@merchantST,'�G�N�d',NULL,NULL,NULL,NULL,NULL,NULL)
		END
	  END

	  IF(@KIND3<>'')
	  BEGIN
		SET @rowNum3 = (SELECT COUNT(*) FROM @report WHERE merchantNo=@merchant AND transDate=CONVERT(VARCHAR(8),@counter,112) AND cardKind='�p�W�d' )
		IF(@rowNum3=0)
		BEGIN
			INSERT INTO @report VALUES (@merchant,CONVERT(VARCHAR(8),@counter,112),@merchantST,'�p�W�d',NULL,NULL,NULL,NULL,NULL,NULL)
		END
	  END

	  SET @counter=DATEADD(DAY,1,@counter)
  END

  FETCH NEXT FROM merchantCur INTO @merchant,@merchantST
END
CLOSE merchantCur
DEALLOCATE merchantCur
 
SELECT * FROM @report ORDER BY merchantNo,transDate,cardKind