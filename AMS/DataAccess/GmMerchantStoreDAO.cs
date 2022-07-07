using Domain;
using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Transactions;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

namespace DataAccess
{
    public class GmMerchantStoreDAO : BasicDAL
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(GmMerchantDAO));
        
        public GmMerchantStoreDAO()
        {
        }

        public void Insert(GmMerchantStore entity)
        {
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();

                    string sqlText = @"
INSERT INTO [dbo].[GM_MERCHANT_STORE]
             ([MERCHANT_NO]
             ,[CREATE_USER]
             ,[CREATE_TIME]
             ,[UPDATE_USER]
             ,[UPDATE_TIME]
             ,[IS_DEL])
       VALUES
             (@MERCHANT_NO
             ,@CREATE_USER
             ,@CREATE_TIME
             ,@UPDATE_USER
             ,@UPDATE_TIME
             ,@IS_DEL)
";

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    sqlCmd.Parameters.Add("@MERCHANT_NO", SqlDbType.VarChar).Value = entity.MERCHANT_NO;
                    sqlCmd.Parameters.Add("@CREATE_USER", SqlDbType.NVarChar).Value = entity.CREATE_USER;
                    sqlCmd.Parameters.Add("@CREATE_TIME", SqlDbType.NVarChar).Value = entity.CREATE_TIME;
                    sqlCmd.Parameters.Add("@UPDATE_USER", SqlDbType.VarChar).Value = entity.CREATE_USER;
                    sqlCmd.Parameters.Add("@UPDATE_TIME", SqlDbType.VarChar).Value = entity.CREATE_TIME;
                    sqlCmd.Parameters.Add("@IS_DEL", SqlDbType.VarChar).Value = @"N";
                    
                    sqlCmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                log.Debug(ex.StackTrace);
                log.Debug(ex.Message);
            }
        }

        public void Delete(GmMerchantStore entity)
        {
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();

                    string sqlText = @"
UPDATE [dbo].[GM_MERCHANT_STORE]
   SET [UPDATE_USER] = @UPDATE_USER
      ,[UPDATE_TIME] = @UPDATE_TIME
      ,[IS_DEL] = 'Y'
 WHERE [ID] = @ID
";

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    sqlCmd.Parameters.Add("@ID", SqlDbType.VarChar).Value = entity.ID;
                    sqlCmd.Parameters.Add("@UPDATE_USER", SqlDbType.VarChar).Value = entity.UPDATE_USER;
                    sqlCmd.Parameters.Add("@UPDATE_TIME", SqlDbType.VarChar).Value = entity.UPDATE_TIME;
                    
                    sqlCmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                log.Debug(ex.StackTrace);
                log.Debug(ex.Message);
            }
        }

        public DataTable FindData(GmMerchantStore entity)
        {
            DataTable dt = new DataTable();
            List<SqlParameter> listPara = new List<SqlParameter>();
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();

                    string sqlText = @"
SELECT S.ID, 
       TD.GROUP_ID, 
       TM.GROUP_NAME, 
       S.MERCHANT_NO, 
	   M.MERCHANT_STNAME AS MERCHANT_NAME, 
	   S.CREATE_USER, S.CREATE_TIME, S.UPDATE_USER, S.UPDATE_TIME, S.IS_DEL, 
       M.MERCH_SCHEMAID, 
       TD.STORE_TYPE
  FROM GM_MERCHANT_STORE S
  LEFT JOIN GM_MERCHANT_TYPE_D TD ON S.MERCHANT_NO = TD.MERCHANT_NO
  LEFT JOIN GM_MERCHANT_TYPE_M TM ON TD.GROUP_ID = TM.GROUP_ID
  LEFT JOIN GM_MERCHANT M ON S.MERCHANT_NO = M.MERCHANT_NO
 WHERE 1 = 1
";

                    if (entity != null)
                    {
                        if (entity.ID != null)
                        {
                            sqlText += @"   AND S.ID = @ID ";
                            listPara.Add(new SqlParameter("@ID", SqlDbType.VarChar) { Value = entity.ID });
                        }
                        if ("" + entity.GROUP_ID != "")
                        {
                            sqlText += @"   AND ISNULL(TM.GROUP_ID, M.MERCHANT_TYPE) = @GROUP_ID ";
                            listPara.Add(new SqlParameter("@GROUP_ID", SqlDbType.VarChar) { Value = entity.GROUP_ID });
                        }
                        if ("" + entity.MERCHANT_NO != "")
                        {
                            sqlText += @"   AND S.MERCHANT_NO = @MERCHANT_NO ";
                            listPara.Add(new SqlParameter("@MERCHANT_NO", SqlDbType.VarChar) { Value = entity.MERCHANT_NO });
                        }
                        if (entity.IS_DEL != null)
                        {
                            sqlText += @"   AND S.IS_DEL = @IS_DEL ";
                            listPara.Add(new SqlParameter("@IS_DEL", SqlDbType.VarChar) { Value = ((bool)entity.IS_DEL) ? "Y" : "N" });
                        }
                    }

                    sqlText += @" ORDER BY TM.SHOW_ORDER, TD.SHOW_ORDER ";

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    foreach (SqlParameter para in listPara)
                    { sqlCmd.Parameters.Add(para); }
                    SqlDataAdapter adapter = new SqlDataAdapter(sqlCmd);
                    adapter.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                log.Debug(ex.StackTrace);
                log.Debug(ex.Message);
            }

            return dt;
        }

        public bool InsertStore_Retail_1(string schema, Domain.BmStoreM entity)
        {
            try
            {
                GmMerchantDAO dao = new GmMerchantDAO();
                string serverName = dao.FindMerchantServerName(entity.MERCHANT_NO);
                schema = (serverName == "") ? schema : string.Format(@"{0}.{1}", serverName, schema);

                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();

                    string sqlText = string.Format(@"
INSERT INTO {0}.BM_STORE_M (MERCHANT_NO, STORE_NO, STO_NAME_LONG, STO_NAME_SHORT, 
                            EFF_DATE_FROM, EFF_DATE_TO, OPEN_DATE, SALES_DATE, CLOSE_DATE, 
                            ZIP, ADDRESS, TEL_AREA, TEL_NO, UPD_DATE)
VALUES
(@MERCHANT_NO, @STORE_NO, @STO_NAME_LONG, @STO_NAME_SHORT,
 @EFF_DATE_FROM, @EFF_DATE_TO, @OPEN_DATE, @SALES_DATE, @CLOSE_DATE,
 @ZIP, @ADDRESS, @TEL_AREA, @TEL_NO, @UPD_DATE);
", schema);

                    string sqlCommon = @"
INSERT INTO {0}.IM_STOREM_D
			(M_KIND, MERCHANT_NO, STORE_NO, EFF_DATE_FROM, EFF_DATE_TO, STO_NAME_LONG, STO_NAME_SHORT,
			 OLD_STORE_NO, OPEN_DATE, SALES_DATE, CLOSE_DATE, ZO_CD, DO_CD, FM_CD, COUNTY, ZIP, [ADDRESS], TEL_AREA, TEL_NO, FAX_NO,
			 AREA, STORE_TYPE, SPECIAL_FLAG, INVOICE_FLAG, JOURNAL_FLAG, OP_TIME, OP_TYPE,
			 STORE_LOCAL, STORE_WORKS, UPD_DATE, UPD_TIME, EXIST_FLAG, PFS_FLAG)
" + string.Format(@"
SELECT LEFT(b.CARD_TYPE, 2), MERCHANT_NO, STORE_NO, EFF_DATE_FROM, EFF_DATE_TO, STO_NAME_LONG, STO_NAME_SHORT,
       STORE_NO, OPEN_DATE, SALES_DATE, CLOSE_DATE, '', '', '', '', ZIP, [ADDRESS], TEL_AREA, TEL_NO, '',
       '', '', '', '', '', '', '00',
       '', 'Y', UPD_DATE, @UPD_TIME, '', ''
  FROM {0}.BM_STORE_M a,
       (SELECT CARD_TYPE FROM GM_CARDTYPE_M WHERE TYPE_STATUS = '1') b
 WHERE MERCHANT_NO = @MERCHANT_NO
   AND STORE_NO = @STORE_NO;
", schema);

                    using (TransactionScope ts = new TransactionScope())
                    {
                        DateTime dtNow = DateTime.Now;
                        SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                        sqlCmd.Parameters.Add("@MERCHANT_NO", SqlDbType.VarChar).Value = entity.MERCHANT_NO;
                        sqlCmd.Parameters.Add("@STORE_NO", SqlDbType.VarChar).Value = entity.STORE_NO;
                        sqlCmd.Parameters.Add("@STO_NAME_LONG", SqlDbType.VarChar).Value = entity.STO_NAME_LONG;
                        sqlCmd.Parameters.Add("@STO_NAME_SHORT", SqlDbType.VarChar).Value = entity.STO_NAME_SHORT;
                        sqlCmd.Parameters.Add("@EFF_DATE_FROM", SqlDbType.VarChar).Value = entity.EFF_DATE_FROM;
                        sqlCmd.Parameters.Add("@EFF_DATE_TO", SqlDbType.VarChar).Value = entity.EFF_DATE_TO;
                        sqlCmd.Parameters.Add("@OPEN_DATE", SqlDbType.VarChar).Value = entity.OPEN_DATE;
                        sqlCmd.Parameters.Add("@SALES_DATE", SqlDbType.VarChar).Value = entity.SALES_DATE;
                        sqlCmd.Parameters.Add("@CLOSE_DATE", SqlDbType.VarChar).Value = entity.CLOSE_DATE;
                        sqlCmd.Parameters.Add("@ZIP", SqlDbType.VarChar).Value = entity.ZIP ?? (object)DBNull.Value;
                        sqlCmd.Parameters.Add("@ADDRESS", SqlDbType.VarChar).Value = entity.ADDRESS ?? (object)DBNull.Value;
                        sqlCmd.Parameters.Add("@TEL_AREA", SqlDbType.VarChar).Value = entity.TEL_AREA ?? (object)DBNull.Value; ;
                        sqlCmd.Parameters.Add("@TEL_NO", SqlDbType.VarChar).Value = entity.TEL_NO ?? (object)DBNull.Value; ;
                        sqlCmd.Parameters.Add("@UPD_DATE", SqlDbType.VarChar).Value = dtNow.ToString("yyyyMMdd");
                        sqlCmd.Parameters.Add("@UPD_TIME", SqlDbType.VarChar).Value = dtNow.ToString("HHmm");

                        sqlCmd.ExecuteNonQuery();

                        sqlCmd.CommandText = string.Format(sqlCommon, "dbo");
                        sqlCmd.ExecuteNonQuery();

                        if (serverName != "")
                        {
                            sqlCmd.CommandText = string.Format(sqlCommon, serverName + ".dbo");
                            sqlCmd.ExecuteNonQuery();
                        }

                        ts.Complete();
                    }
                }
            }
            catch (Exception ex)
            {
                log.Debug(ex.StackTrace);
                log.Debug(ex.Message);

                return false;
            }

            return true;
        }

        public bool UpdateStore_Retail_1(string schema, Domain.BmStoreM entity)
        {
            try
            {
                GmMerchantDAO dao = new GmMerchantDAO();
                string serverName = dao.FindMerchantServerName(entity.MERCHANT_NO);
                schema = (serverName == "") ? schema : string.Format(@"{0}.{1}", serverName, schema);

                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();

                    string sqlText = string.Format(@"
UPDATE {0}.BM_STORE_M
   SET STORE_NO = @STORE_NO 
      ,STO_NAME_LONG = @STO_NAME_LONG
      ,STO_NAME_SHORT = @STO_NAME_SHORT
      ,OPEN_DATE = @OPEN_DATE
      ,SALES_DATE = @SALES_DATE
      ,CLOSE_DATE = @CLOSE_DATE
      ,ZIP = @ZIP
      ,ADDRESS = @ADDRESS
      ,TEL_AREA = @TEL_AREA
      ,TEL_NO = @TEL_NO
      ,UPD_DATE = @UPD_DATE
 WHERE MERCHANT_NO = @MERCHANT_NO 
   AND STORE_NO = @STORE_NO
   AND EFF_DATE_FROM = @EFF_DATE_FROM;
", schema);

                    string sqlCommon = @"
UPDATE {0}.IM_STOREM_D
   SET STO_NAME_LONG = @STO_NAME_LONG
      ,STO_NAME_SHORT = @STO_NAME_SHORT
      ,OPEN_DATE = @OPEN_DATE
      ,SALES_DATE = @SALES_DATE
      ,CLOSE_DATE = @CLOSE_DATE
      ,ZIP = @ZIP
      ,[ADDRESS] = @ADDRESS
      ,TEL_AREA = @TEL_AREA
      ,TEL_NO = @TEL_NO
      ,UPD_DATE = @UPD_DATE
      ,UPD_TIME = @UPD_TIME
 WHERE MERCHANT_NO = @MERCHANT_NO 
   AND STORE_NO = @STORE_NO
   AND EFF_DATE_FROM = @EFF_DATE_FROM;
";

                    using (TransactionScope ts = new TransactionScope())
                    {
                        DateTime dtNow = DateTime.Now;
                        SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                        sqlCmd.Parameters.Add("@MERCHANT_NO", SqlDbType.VarChar).Value = entity.MERCHANT_NO;
                        sqlCmd.Parameters.Add("@STORE_NO", SqlDbType.VarChar).Value = entity.STORE_NO;
                        sqlCmd.Parameters.Add("@STO_NAME_LONG", SqlDbType.VarChar).Value = entity.STO_NAME_LONG;
                        sqlCmd.Parameters.Add("@STO_NAME_SHORT", SqlDbType.VarChar).Value = entity.STO_NAME_SHORT;
                        sqlCmd.Parameters.Add("@EFF_DATE_FROM", SqlDbType.VarChar).Value = entity.EFF_DATE_FROM;
                        sqlCmd.Parameters.Add("@OPEN_DATE", SqlDbType.VarChar).Value = entity.OPEN_DATE;
                        sqlCmd.Parameters.Add("@SALES_DATE", SqlDbType.VarChar).Value = entity.SALES_DATE;
                        sqlCmd.Parameters.Add("@CLOSE_DATE", SqlDbType.VarChar).Value = entity.CLOSE_DATE;
                        sqlCmd.Parameters.Add("@ZIP", SqlDbType.VarChar).Value = entity.ZIP ?? (object)DBNull.Value;
                        sqlCmd.Parameters.Add("@ADDRESS", SqlDbType.VarChar).Value = entity.ADDRESS ?? (object)DBNull.Value;
                        sqlCmd.Parameters.Add("@TEL_AREA", SqlDbType.VarChar).Value = entity.TEL_AREA ?? (object)DBNull.Value; ;
                        sqlCmd.Parameters.Add("@TEL_NO", SqlDbType.VarChar).Value = entity.TEL_NO ?? (object)DBNull.Value; ;
                        sqlCmd.Parameters.Add("@UPD_DATE", SqlDbType.VarChar).Value = dtNow.ToString("yyyyMMdd");
                        sqlCmd.Parameters.Add("@UPD_TIME", SqlDbType.VarChar).Value = dtNow.ToString("HHmm");

                        sqlCmd.ExecuteNonQuery();

                        sqlCmd.CommandText = string.Format(sqlCommon, "dbo");
                        sqlCmd.ExecuteNonQuery();

                        if (serverName != "")
                        {
                            sqlCmd.CommandText = string.Format(sqlCommon, serverName + ".dbo");
                            sqlCmd.ExecuteNonQuery();
                        }

                        ts.Complete();
                    }
                }
            }
            catch (Exception ex)
            {
                log.Debug(ex.StackTrace);
                log.Debug(ex.Message);

                return false;
            }

            return true;
        }

        public bool InsertStore_Traffic_1(string schema, Domain.BmStoreM entity)
        {
            try
            {
                GmMerchantDAO dao = new GmMerchantDAO();
                string serverName = dao.FindMerchantServerName(entity.MERCHANT_NO);
                schema = (serverName == "") ? schema : string.Format(@"{0}.{1}", serverName, schema);

                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();

                    string sqlText = string.Format(@"
INSERT INTO {0}.BM_STORE_M (MERCHANT_NO, STORE_NO, STO_NAME_LONG, STO_NAME_SHORT, 
                            EFF_DATE_FROM, EFF_DATE_TO, OPEN_DATE, SALES_DATE, CLOSE_DATE, 
                            ZIP, ADDRESS, TEL_AREA, TEL_NO, 
                            OPERATE_TYPE, LINE_TYPE, CITY_TYPE, TRANSFER_TYPE, SUBSIDY_TYPE, LINE_NO_03, LINE_NO_04, LICENSE_NO, LINE_QTY, LINE_SI_NO, UPD_DATE)
VALUES
(@MERCHANT_NO, @STORE_NO, @STO_NAME_LONG, @STO_NAME_SHORT, 
 @EFF_DATE_FROM, @EFF_DATE_TO, @OPEN_DATE, @SALES_DATE, @CLOSE_DATE, 
 @ZIP, @ADDRESS, @TEL_AREA,
 @TEL_NO, @OPERATE_TYPE, @LINE_TYPE, @CITY_TYPE, @TRANSFER_TYPE, @SUBSIDY_TYPE, @LINE_NO_03, @LINE_NO_04, @LICENSE_NO, @LINE_QTY, @LINE_SI_NO, @UPD_DATE);
", schema);

                    string sqlCommon = @"
INSERT INTO {0}.IM_STOREM_D
			(M_KIND, MERCHANT_NO, STORE_NO, EFF_DATE_FROM, EFF_DATE_TO, STO_NAME_LONG, STO_NAME_SHORT,
			 OLD_STORE_NO, OPEN_DATE, SALES_DATE, CLOSE_DATE, ZO_CD, DO_CD, FM_CD, COUNTY, ZIP, [ADDRESS], TEL_AREA, TEL_NO, FAX_NO,
			 AREA, STORE_TYPE, SPECIAL_FLAG, INVOICE_FLAG, JOURNAL_FLAG, OP_TIME, OP_TYPE,
			 STORE_LOCAL, STORE_WORKS, UPD_DATE, UPD_TIME, EXIST_FLAG, PFS_FLAG)
" + string.Format(@"
SELECT LEFT(b.CARD_TYPE, 2), MERCHANT_NO, STORE_NO, EFF_DATE_FROM, EFF_DATE_TO, STO_NAME_LONG, STO_NAME_SHORT,
       STORE_NO, OPEN_DATE, SALES_DATE, CLOSE_DATE, '', '', '', '', ZIP, [ADDRESS], TEL_AREA, TEL_NO, '',
       '', '', '', '', '', '', '00',
       '', 'Y', UPD_DATE, @UPD_TIME, '', ''
  FROM {0}.BM_STORE_M a,
       (SELECT CARD_TYPE FROM GM_CARDTYPE_M WHERE TYPE_STATUS = '1') b
 WHERE MERCHANT_NO = @MERCHANT_NO
   AND STORE_NO = @STORE_NO;
", schema);

                    using (TransactionScope ts = new TransactionScope())
                    {
                        DateTime dtNow = DateTime.Now;
                        SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                        sqlCmd.Parameters.Add("@MERCHANT_NO", SqlDbType.VarChar).Value = entity.MERCHANT_NO;
                        sqlCmd.Parameters.Add("@STORE_NO", SqlDbType.VarChar).Value = entity.STORE_NO;
                        sqlCmd.Parameters.Add("@STO_NAME_LONG", SqlDbType.VarChar).Value = entity.STO_NAME_LONG;
                        sqlCmd.Parameters.Add("@STO_NAME_SHORT", SqlDbType.VarChar).Value = entity.STO_NAME_SHORT;
                        sqlCmd.Parameters.Add("@EFF_DATE_FROM", SqlDbType.VarChar).Value = entity.EFF_DATE_FROM;
                        sqlCmd.Parameters.Add("@EFF_DATE_TO", SqlDbType.VarChar).Value = entity.EFF_DATE_TO;
                        sqlCmd.Parameters.Add("@OPEN_DATE", SqlDbType.VarChar).Value = entity.OPEN_DATE;
                        sqlCmd.Parameters.Add("@SALES_DATE", SqlDbType.VarChar).Value = entity.SALES_DATE;
                        sqlCmd.Parameters.Add("@CLOSE_DATE", SqlDbType.VarChar).Value = entity.CLOSE_DATE;
                        sqlCmd.Parameters.Add("@ZIP", SqlDbType.VarChar).Value = entity.ZIP ?? (object)DBNull.Value;
                        sqlCmd.Parameters.Add("@ADDRESS", SqlDbType.VarChar).Value = entity.ADDRESS ?? (object)DBNull.Value;
                        sqlCmd.Parameters.Add("@TEL_AREA", SqlDbType.VarChar).Value = entity.TEL_AREA ?? (object)DBNull.Value;
                        sqlCmd.Parameters.Add("@TEL_NO", SqlDbType.VarChar).Value = entity.TEL_NO ?? (object)DBNull.Value;
                        sqlCmd.Parameters.Add("@OPERATE_TYPE", SqlDbType.VarChar).Value = entity.OPERATE_TYPE ?? (object)DBNull.Value;
                        sqlCmd.Parameters.Add("@LINE_TYPE", SqlDbType.VarChar).Value = entity.LINE_TYPE ?? (object)DBNull.Value;
                        sqlCmd.Parameters.Add("@CITY_TYPE", SqlDbType.VarChar).Value = entity.CITY_TYPE ?? (object)DBNull.Value;
                        sqlCmd.Parameters.Add("@TRANSFER_TYPE", SqlDbType.VarChar).Value = entity.TRANSFER_TYPE ?? (object)DBNull.Value;
                        sqlCmd.Parameters.Add("@SUBSIDY_TYPE", SqlDbType.VarChar).Value = entity.SUBSIDY_TYPE ?? (object)DBNull.Value;
                        sqlCmd.Parameters.Add("@LINE_NO_03", SqlDbType.VarChar).Value = entity.LINE_NO_03 ?? (object)DBNull.Value;
                        sqlCmd.Parameters.Add("@LINE_NO_04", SqlDbType.VarChar).Value = entity.LINE_NO_04 ?? (object)DBNull.Value;
                        sqlCmd.Parameters.Add("@LICENSE_NO", SqlDbType.VarChar).Value = entity.LICENSE_NO ?? (object)DBNull.Value;
                        sqlCmd.Parameters.Add("@LINE_QTY", SqlDbType.VarChar).Value = entity.LINE_QTY ?? (object)DBNull.Value;
                        sqlCmd.Parameters.Add("@LINE_SI_NO", SqlDbType.VarChar).Value = entity.LINE_SI_NO ?? (object)DBNull.Value;
                        sqlCmd.Parameters.Add("@UPD_DATE", SqlDbType.VarChar).Value = dtNow.ToString("yyyyMMdd");
                        sqlCmd.Parameters.Add("@UPD_TIME", SqlDbType.VarChar).Value = dtNow.ToString("HHmm");

                        sqlCmd.ExecuteNonQuery();

                        sqlCmd.CommandText = string.Format(sqlCommon, "dbo");
                        sqlCmd.ExecuteNonQuery();

                        if (serverName != "")
                        {
                            sqlCmd.CommandText = string.Format(sqlCommon, serverName + ".dbo");
                            sqlCmd.ExecuteNonQuery();
                        }

                        ts.Complete();
                    }
                }
            }
            catch (Exception ex)
            {
                log.Debug(ex.StackTrace);
                log.Debug(ex.Message);

                return false;
            }

            return true;
        }

        public bool UpdateStore_Traffic_1(string schema, Domain.BmStoreM entity)
        {
            try
            {
                GmMerchantDAO dao = new GmMerchantDAO();
                string serverName = dao.FindMerchantServerName(entity.MERCHANT_NO);
                schema = (serverName == "") ? schema : string.Format(@"{0}.{1}", serverName, schema);

                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();

                    string sqlText = string.Format(@"
UPDATE {0}.BM_STORE_M
   SET STORE_NO       = @STORE_NO 
      ,STO_NAME_LONG  = @STO_NAME_LONG
      ,STO_NAME_SHORT = @STO_NAME_SHORT
      ,OPEN_DATE      = @OPEN_DATE
      ,SALES_DATE     = @SALES_DATE
      ,CLOSE_DATE     = @CLOSE_DATE
      ,ZIP            = @ZIP
      ,ADDRESS        = @ADDRESS
      ,TEL_AREA       = @TEL_AREA
      ,TEL_NO         = @TEL_NO
      ,OPERATE_TYPE   = @OPERATE_TYPE
      ,LINE_TYPE      = @LINE_TYPE
      ,CITY_TYPE      = @CITY_TYPE
      ,TRANSFER_TYPE  = @TRANSFER_TYPE
      ,SUBSIDY_TYPE   = @SUBSIDY_TYPE
      ,LINE_NO_03     = @LINE_NO_03
      ,LINE_NO_04     = @LINE_NO_04
      ,LICENSE_NO     = @LICENSE_NO
      ,LINE_QTY       = @LINE_QTY
      ,LINE_SI_NO     = @LINE_SI_NO
      ,UPD_DATE       = @UPD_DATE
 WHERE MERCHANT_NO   = @MERCHANT_NO 
   AND STORE_NO      = @STORE_NO
   AND EFF_DATE_FROM = @EFF_DATE_FROM;
", schema);

                    string sqlCommon = @"
UPDATE {0}.IM_STOREM_D
   SET STO_NAME_LONG  = @STO_NAME_LONG
      ,STO_NAME_SHORT = @STO_NAME_SHORT
      ,OPEN_DATE      = @OPEN_DATE
      ,SALES_DATE     = @SALES_DATE
      ,CLOSE_DATE     = @CLOSE_DATE
      ,ZIP            = @ZIP
      ,[ADDRESS]      = @ADDRESS
      ,TEL_AREA       = @TEL_AREA
      ,TEL_NO         = @TEL_NO
      ,UPD_DATE       = @UPD_DATE
      ,UPD_TIME       = @UPD_TIME
 WHERE MERCHANT_NO   = @MERCHANT_NO 
   AND STORE_NO      = @STORE_NO
   AND EFF_DATE_FROM = @EFF_DATE_FROM;
";

                    using (TransactionScope ts = new TransactionScope())
                    {
                        DateTime dtNow = DateTime.Now;
                        SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                        sqlCmd.Parameters.Add("@MERCHANT_NO", SqlDbType.VarChar).Value = entity.MERCHANT_NO;
                        sqlCmd.Parameters.Add("@STORE_NO", SqlDbType.VarChar).Value = entity.STORE_NO;
                        sqlCmd.Parameters.Add("@STO_NAME_LONG", SqlDbType.VarChar).Value = entity.STO_NAME_LONG;
                        sqlCmd.Parameters.Add("@STO_NAME_SHORT", SqlDbType.VarChar).Value = entity.STO_NAME_SHORT;
                        sqlCmd.Parameters.Add("@EFF_DATE_FROM", SqlDbType.VarChar).Value = entity.EFF_DATE_FROM;
                        sqlCmd.Parameters.Add("@OPEN_DATE", SqlDbType.VarChar).Value = entity.OPEN_DATE;
                        sqlCmd.Parameters.Add("@SALES_DATE", SqlDbType.VarChar).Value = entity.SALES_DATE;
                        sqlCmd.Parameters.Add("@CLOSE_DATE", SqlDbType.VarChar).Value = entity.CLOSE_DATE;
                        sqlCmd.Parameters.Add("@ZIP", SqlDbType.VarChar).Value = entity.ZIP ?? (object)DBNull.Value;
                        sqlCmd.Parameters.Add("@ADDRESS", SqlDbType.VarChar).Value = entity.ADDRESS ?? (object)DBNull.Value;
                        sqlCmd.Parameters.Add("@TEL_AREA", SqlDbType.VarChar).Value = entity.TEL_AREA ?? (object)DBNull.Value;
                        sqlCmd.Parameters.Add("@TEL_NO", SqlDbType.VarChar).Value = entity.TEL_NO ?? (object)DBNull.Value;
                        sqlCmd.Parameters.Add("@OPERATE_TYPE", SqlDbType.VarChar).Value = entity.OPERATE_TYPE ?? (object)DBNull.Value;
                        sqlCmd.Parameters.Add("@LINE_TYPE", SqlDbType.VarChar).Value = entity.LINE_TYPE ?? (object)DBNull.Value;
                        sqlCmd.Parameters.Add("@CITY_TYPE", SqlDbType.VarChar).Value = entity.CITY_TYPE ?? (object)DBNull.Value;
                        sqlCmd.Parameters.Add("@TRANSFER_TYPE", SqlDbType.VarChar).Value = entity.TRANSFER_TYPE ?? (object)DBNull.Value;
                        sqlCmd.Parameters.Add("@SUBSIDY_TYPE", SqlDbType.VarChar).Value = entity.SUBSIDY_TYPE ?? (object)DBNull.Value;
                        sqlCmd.Parameters.Add("@LINE_NO_03", SqlDbType.VarChar).Value = entity.LINE_NO_03 ?? (object)DBNull.Value;
                        sqlCmd.Parameters.Add("@LINE_NO_04", SqlDbType.VarChar).Value = entity.LINE_NO_04 ?? (object)DBNull.Value;
                        sqlCmd.Parameters.Add("@LICENSE_NO", SqlDbType.VarChar).Value = entity.LICENSE_NO ?? (object)DBNull.Value;
                        sqlCmd.Parameters.Add("@LINE_QTY", SqlDbType.VarChar).Value = entity.LINE_QTY ?? (object)DBNull.Value;
                        sqlCmd.Parameters.Add("@LINE_SI_NO", SqlDbType.VarChar).Value = entity.LINE_SI_NO ?? (object)DBNull.Value;
                        sqlCmd.Parameters.Add("@UPD_DATE", SqlDbType.VarChar).Value = dtNow.ToString("yyyyMMdd");
                        sqlCmd.Parameters.Add("@UPD_TIME", SqlDbType.VarChar).Value = dtNow.ToString("HHmm");

                        sqlCmd.ExecuteNonQuery();

                        sqlCmd.CommandText = string.Format(sqlCommon, "dbo");
                        sqlCmd.ExecuteNonQuery();

                        if (serverName != "")
                        {
                            sqlCmd.CommandText = string.Format(sqlCommon, serverName + ".dbo");
                            sqlCmd.ExecuteNonQuery();
                        }

                        ts.Complete();
                    }
                }
            }
            catch (Exception ex)
            {
                log.Debug(ex.StackTrace);
                log.Debug(ex.Message);

                return false;
            }

            return true;
        }

        public void DeleteStore(string schema, string merchant_no, string store_no)
        {
            try
            {
                GmMerchantDAO dao = new GmMerchantDAO();
                string serverName = dao.FindMerchantServerName(merchant_no);
                schema = (serverName == "") ? schema : string.Format(@"{0}.{1}", serverName, schema);

                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();

                    string sqlText = string.Format(@"
DELETE {0}.BM_STORE_M
 WHERE MERCHANT_NO = @MERCHANT_NO
   AND STORE_NO = @STORE_NO;
", schema);

                    string sqlCommon = @"
DELETE dbo.IM_STOREM_D
 WHERE MERCHANT_NO = @MERCHANT_NO
   AND STORE_NO = @STORE_NO;
";

                    using (TransactionScope ts = new TransactionScope())
                    {
                        SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                        sqlCmd.Parameters.Add("@MERCHANT_NO", SqlDbType.VarChar).Value = merchant_no;
                        sqlCmd.Parameters.Add("@STORE_NO", SqlDbType.VarChar).Value = store_no;

                        sqlCmd.ExecuteNonQuery();

                        sqlCmd.CommandText = sqlCommon;
                        sqlCmd.ExecuteNonQuery();

                        ts.Complete();
                    }
                }
            }
            catch (Exception ex)
            {
                log.Debug(ex.StackTrace);
                log.Debug(ex.Message);
            }
        }

        public DataTable FindStoreData(string schema, string merchant_no, string store_no)
        {
            DataTable dt = new DataTable();
            List<SqlParameter> listPara = new List<SqlParameter>();
            try
            {
                GmMerchantDAO dao = new GmMerchantDAO();
                string serverName = dao.FindMerchantServerName(merchant_no);
                schema = (serverName == "") ? schema : string.Format(@"{0}.{1}", serverName, schema);

                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();

                    string sqlText = string.Format(@"
SELECT *
  FROM {0}.BM_STORE_M
 WHERE 1 = 1 
", schema);

                    if (merchant_no != "")
                    {
                        sqlText += @"   AND MERCHANT_NO = @MERCHANT_NO ";
                        listPara.Add(new SqlParameter("@MERCHANT_NO", SqlDbType.VarChar) { Value = merchant_no });
                    }
                    if (store_no != "")
                    {
                        sqlText += @"   AND STORE_NO = @STORE_NO ";
                        listPara.Add(new SqlParameter("@STORE_NO", SqlDbType.VarChar) { Value = store_no });
                    }

                    sqlText += @" ORDER BY STORE_NO";

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    foreach (SqlParameter para in listPara)
                    { sqlCmd.Parameters.Add(para); }
                    SqlDataAdapter adapter = new SqlDataAdapter(sqlCmd);
                    adapter.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                log.Debug(ex.StackTrace);
                log.Debug(ex.Message);
            }

            return dt;
        }

        public DataTable FindTrafficStoreData(string schema, string merchant_no, string store_no)
        {
            DataTable dt = new DataTable();
            List<SqlParameter> listPara = new List<SqlParameter>();
            try
            {
                GmMerchantDAO dao = new GmMerchantDAO();
                string serverName = dao.FindMerchantServerName(merchant_no);
                schema = (serverName == "") ? schema : string.Format(@"{0}.{1}", serverName, schema);

                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();

                    string sqlText = string.Format(@"
SELECT S.*, LT.OUTPUT_DESC AS LINE_TYPE_DESC, LN4.OUTPUT_DESC AS LINE_NO_04_DESC
  FROM {0}.BM_STORE_M S
  LEFT JOIN GM_CODE_MAPPING LT  ON  LT.CODE_TYPE = 'LINE_TYPE'  AND S.LINE_TYPE  =  LT.INPUT_VALUE
  LEFT JOIN GM_CODE_MAPPING LN4 ON LN4.CODE_TYPE = 'LINE_NO_04' AND S.LINE_NO_04 = LN4.INPUT_VALUE
 WHERE 1 = 1 
", schema);

                    if (merchant_no != "")
                    {
                        sqlText += @"   AND S.MERCHANT_NO = @MERCHANT_NO ";
                        listPara.Add(new SqlParameter("@MERCHANT_NO", SqlDbType.VarChar) { Value = merchant_no });
                    }
                    if (store_no != "")
                    {
                        sqlText += @"   AND S.STORE_NO = @STORE_NO ";
                        listPara.Add(new SqlParameter("@STORE_NO", SqlDbType.VarChar) { Value = store_no });
                    }

                    sqlText += @" ORDER BY S.STORE_NO";

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    foreach (SqlParameter para in listPara)
                    { sqlCmd.Parameters.Add(para); }
                    SqlDataAdapter adapter = new SqlDataAdapter(sqlCmd);
                    adapter.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                log.Debug(ex.StackTrace);
                log.Debug(ex.Message);
            }

            return dt;
        }

        public DataTable FindImStoreData(string merchant_no, string store_no)
        {
            DataTable dt = new DataTable();
            List<SqlParameter> listPara = new List<SqlParameter>();
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();

                    string sqlText = @"
SELECT *
  FROM dbo.IM_STOREM_D
 WHERE 1 = 1 
";

                    if (merchant_no != "")
                    {
                        sqlText += @"   AND MERCHANT_NO = @MERCHANT_NO ";
                        listPara.Add(new SqlParameter("@MERCHANT_NO", SqlDbType.VarChar) { Value = merchant_no });
                    }
                    if (store_no != "")
                    {
                        sqlText += @"   AND STORE_NO = @STORE_NO ";
                        listPara.Add(new SqlParameter("@STORE_NO", SqlDbType.VarChar) { Value = store_no });
                    }

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    foreach (SqlParameter para in listPara)
                    { sqlCmd.Parameters.Add(para); }
                    SqlDataAdapter adapter = new SqlDataAdapter(sqlCmd);
                    adapter.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                log.Debug(ex.StackTrace);
                log.Debug(ex.Message);
            }

            return dt;
        }
    }
}
