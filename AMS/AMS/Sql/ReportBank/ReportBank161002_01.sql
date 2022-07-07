select 
LEFT(CREATED_TIME,8) as '�R���d�{�C��',
CARD_NO as '�d��',
GET_CARD_REASON as '�ӿ��]',
case
when c.CARD_PN02 = '1' then '�O'
else '�_'
end as '�ĤT���p�W',
c.CARD_NAME as '�d�ڦW��'
from CC_REGISTER a
left join CM_BANK_D b on a.BANK_CODE = b.BANK_CODE and LEFT(a.CARD_NO,2) = b.CA_DPT
inner join CM_CARDTYPE_BANK_D c on LEFT(a.CARD_NO,2) = c.CA_DPT and SUBSTRING(a.CARD_NO,5,2) = c.CARD_YEAR and SUBSTRING(a.CARD_NO,7,3) = c.BATCH_NO
inner join CR_PERSO_D p on a.CARD_NO = p.CARD_ID
where 
b.MERCHANT_NO = @bankMerchant
and LEFT(CREATED_TIME,8) between @sDATE and @eDATE
order by LEFT(CREATED_TIME,8),CARD_NO