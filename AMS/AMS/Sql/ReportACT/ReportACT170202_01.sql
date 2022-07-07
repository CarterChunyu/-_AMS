select B.FILE_TRANS_DESC+'-'+B.SUB_TYPE_DESC,COUNT(1),SUM(CONVERT(numeric,A.TRANS_AMT)),C.DIFF_DESC
                    from  ICASHBATCHDB.ITRANS.TRT.TM_SKIP_TMLOG_D A
                    left outer join ICASHBATCHDB.ITRANS.TRT.BM_TRANS_TYPE B
                    on A.FILE_TRANS_TYPE+A.FILE_SUB_TYPE = B.TRANS_TYPE
					left outer join ICASHBATCHDB.ITRANS.TRT.BM_DIFF_M C
					on A.DIFF_FLG = C.DIFF_FLG
                    WHERE A.DIFF_FLG IN ('6001','6006') and CPT_DATE = @settleDate
                    GROUP BY B.FILE_TRANS_DESC,B.SUB_TYPE_DESC,C.DIFF_DESC
					order by B.FILE_TRANS_DESC