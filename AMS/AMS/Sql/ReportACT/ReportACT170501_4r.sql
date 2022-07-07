select C.FEE_RATE from GM_CONTRACT_D C where C.MERCHANT_NO = '22555003'
													  and C.SETTLE_TYPE = 'A' and FEE_CAL_FLG = 'AM'
													  and @EXEC_CPT_DATE_B between C.EFF_DATE_FROM and C.EFF_DATE_TO