SELECT  A.CPT_DATE,
		                                            A.STORE_NO,		
		                                            B.STO_NAME_SHORT, 
		                                            A.REG_ID,		
				                                    A.ICC_NO,
                                                    A.TRANS_DATE_TXLOG,
				                                    C.FILE_TRANS_DESC+'-'+C.SUB_TYPE_DESC,
                                                    A.TRANS_AMT	,	
				                                    D.DIFF_DESC 
                                           FROM ICASHBATCHDB.ITRANS.TRT.TM_SKIP_TMLOG_D A LEFT OUTER JOIN 
				                                    (SELECT STORE_NO,STO_NAME_SHORT FROM ICASHBATCHDB.ITRANS.TRT.BM_STORE_M) B
				                                    ON A.STORE_NO = B.STORE_NO 
				                                    LEFT OUTER JOIN ICASHBATCHDB.ITRANS.TRT.BM_TRANS_TYPE C
				                                    ON C.FILE_TRANS_TYPE = A.FILE_TRANS_TYPE 
                                                    AND C.FILE_SUB_TYPE = A.FILE_SUB_TYPE
				                                    LEFT OUTER JOIN ICASHBATCHDB.ITRANS.TRT.BM_DIFF_M D
				                                    ON A.DIFF_FLG = D.DIFF_FLG
			                                        WHERE A.DIFF_FLG IN ('6001','6006') and CPT_DATE = @settleDate
			                              ORDER BY A.DIFF_FLG