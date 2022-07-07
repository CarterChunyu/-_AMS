--------------------------------



select B.COMPANY_ID, B.NAME, SUM(convert(int, TRANS_AMT))
from icashsqldb.isettle2.dbo.IM_OL_TXLOG_T_ICASH3 A
inner join icashsqldb.isettle2.dbo.CC_REGISTER_ICASH3 B on A.ICC_NO = B.CARD_NO
where MERCHANT_NO = '22555003'
and TRANS_TYPE = '22' 
and FILE_TRANS_TYPE = '78' 
and ZIP_FILE_NAME >= 'ICASH3_'+@sDate+'000000'
and ZIP_FILE_NAME <= 'ICASH3_'+@eDate+'235959'
and RETURN_CODE = '00000000'
group by B.COMPANY_ID, B.NAME