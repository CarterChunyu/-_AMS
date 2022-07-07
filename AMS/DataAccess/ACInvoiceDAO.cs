using Domain;
using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class ACInvoiceDAO : BasicDAL
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ACInvoiceDAO));
        public string TableName { private get; set; }

        public ACInvoiceDAO()
        {
        }

        public DataTable GetACICPInvoiceReport(string reward_start_date, string reward_end_date)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();

                    string sqlText = @"
select ROW_NUMBER() over(order by CPT_DATE) as WEEK,
       dbo.TO_DATE(CPT_DATE, 'yyyyMMdd') as CPT_DATE, 
       dbo.TO_DATE(REWARD_START_DATE, 'yyyyMMdd') as REWARD_START_DATE,
	   dbo.TO_DATE(REWARD_END_DATE, 'yyyyMMdd') as REWARD_END_DATE, 
	   STAGE,
       PRE_ISSUANCE_AMT, REWARD_AMT, TOTAL_REWARD_AMT, BALANCE, 
	   OVER_LIMIT, 
	   BINDING_COUNT_ICP as BINDING_COUNT, ESTIMATE_INVOICE, INVOICE_AMT, 
	   case when OVER_LIMIT = 'Y' and BAL_SHORT = 'Y' then 'Y' else 'N' end as CAN_APPLY, 
	   IS_APPLY,
	   case when IS_APPLY = 'Y' then INVOICE_AMT else 0 end as REAL_INVOICE_AMT,
       CAST((((BINDING_COUNT_ICP * 1000) - ESTIMATE_INVOICE) / 1000) AS DECIMAL(10,0)) AS STAGE_ALREADY_REWARD
  into #DATA
  from AC_ICP_INVOICE_DATA	
 order by CPT_DATE;

select a.WEEK, a.CPT_DATE, a.REWARD_START_DATE, a.REWARD_END_DATE, a.PRE_ISSUANCE_AMT, 
       isnull(b.BALANCE + b.REAL_INVOICE_AMT, a.PRE_ISSUANCE_AMT) as PRE_BALANCE, 
	   a.REWARD_AMT, a.TOTAL_REWARD_AMT, a.BALANCE, 
	   a.OVER_LIMIT, a.BINDING_COUNT, a.ESTIMATE_INVOICE, a.INVOICE_AMT, a.CAN_APPLY, 
	   a.STAGE as NEXT_STAGE, 
	   a.IS_APPLY,
       a.STAGE_ALREADY_REWARD
  from #DATA a
  left join #DATA b on b.WEEK = a.WEEK - 1
 where 1 = 1
";
                    if (reward_start_date != "" && reward_end_date != "")
                    {
                        sqlText += " and a.REWARD_START_DATE = @REWARD_START_DATE and a.REWARD_END_DATE = @REWARD_END_DATE";
                    }

                    List<SqlParameter> listPara = new List<SqlParameter>();
                    listPara.Add(new SqlParameter("@REWARD_START_DATE", reward_start_date));
                    listPara.Add(new SqlParameter("@REWARD_END_DATE", reward_end_date));

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

        public DataTable GetACICASHInvoiceReport(string reward_start_date, string reward_end_date)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();

                    string sqlText = @"
select ROW_NUMBER() over(order by CPT_DATE) as WEEK,
       dbo.TO_DATE(CPT_DATE, 'yyyyMMdd') as CPT_DATE, 
       dbo.TO_DATE(REWARD_START_DATE, 'yyyyMMdd') as REWARD_START_DATE,
	   dbo.TO_DATE(REWARD_END_DATE, 'yyyyMMdd') as REWARD_END_DATE, 
	   STAGE,
       PRE_ISSUANCE_AMT, REWARD_AMT, TOTAL_REWARD_AMT, BALANCE, 
	   OVER_LIMIT, 
	   BINDING_COUNT_ICASH as BINDING_COUNT, ESTIMATE_INVOICE, INVOICE_AMT, 
	   case when OVER_LIMIT = 'Y' and BAL_SHORT = 'Y' then 'Y' else 'N' end as CAN_APPLY, 
	   IS_APPLY,
	   case when IS_APPLY = 'Y' then INVOICE_AMT else 0 end as REAL_INVOICE_AMT,
        CAST((((BINDING_COUNT_ICASH * 1000) - ESTIMATE_INVOICE) / 1000) AS DECIMAL(10,0)) AS STAGE_ALREADY_REWARD
  into #DATA
  from AC_ICASH_INVOICE_DATA	
 order by CPT_DATE;

select a.WEEK, a.CPT_DATE, a.REWARD_START_DATE, a.REWARD_END_DATE, a.PRE_ISSUANCE_AMT, 
       isnull(b.BALANCE + b.REAL_INVOICE_AMT, a.PRE_ISSUANCE_AMT) as PRE_BALANCE, 
	   a.REWARD_AMT, a.TOTAL_REWARD_AMT, a.BALANCE, 
	   a.OVER_LIMIT, a.BINDING_COUNT, a.ESTIMATE_INVOICE, a.INVOICE_AMT, a.CAN_APPLY, 
	   a.STAGE as NEXT_STAGE, 
	   a.IS_APPLY,
       a.STAGE_ALREADY_REWARD
  from #DATA a
  left join #DATA b on b.WEEK = a.WEEK - 1
 where 1 = 1
";
                    if (reward_start_date != "" && reward_end_date != "")
                    {
                        sqlText += " and a.REWARD_START_DATE = @REWARD_START_DATE and a.REWARD_END_DATE = @REWARD_END_DATE";
                    }

                    List<SqlParameter> listPara = new List<SqlParameter>();
                    listPara.Add(new SqlParameter("@REWARD_START_DATE", reward_start_date));
                    listPara.Add(new SqlParameter("@REWARD_END_DATE", reward_end_date));

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

        public DataRow GetACICPInvoiceData(string reward_start_date, string reward_end_date)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();

                    string sqlText = @"
select CPT_DATE, REWARD_START_DATE, REWARD_END_DATE, STAGE, PRE_ISSUANCE_AMT, REWARD_AMT, TOTAL_REWARD_AMT, LIMIT, OVER_LIMIT, TOTAL_BINDING_COUNT, BINDING_COUNT_DONATE, BINDING_COUNT_ATM, BINDING_COUNT_ICP, ESTIMATE_INVOICE, BALANCE, INVOICE_AMT, BAL_SHORT, IS_APPLY, NOTE
  from AC_ICP_INVOICE_DATA
 where REWARD_START_DATE = @REWARD_START_DATE
   and REWARD_END_DATE = @REWARD_END_DATE;
";

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    List<SqlParameter> listPara = new List<SqlParameter>();
                    listPara.Add(new SqlParameter("@REWARD_START_DATE", reward_start_date));
                    listPara.Add(new SqlParameter("@REWARD_END_DATE", reward_end_date));

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

            if (dt.Rows.Count == 0)
            { return null; }
            else
            { return dt.Rows[0]; }
        }

        public DataRow GetACICASHInvoiceData(string reward_start_date, string reward_end_date)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();

                    string sqlText = @"
select CPT_DATE, REWARD_START_DATE, REWARD_END_DATE, STAGE, PRE_ISSUANCE_AMT, REWARD_AMT, TOTAL_REWARD_AMT, LIMIT, OVER_LIMIT, TOTAL_BINDING_COUNT, BINDING_COUNT_DONATE, BINDING_COUNT_ATM, BINDING_COUNT_ICASH, ESTIMATE_INVOICE, BALANCE, INVOICE_AMT, BAL_SHORT, IS_APPLY, NOTE
  from AC_ICASH_INVOICE_DATA
 where REWARD_START_DATE = @REWARD_START_DATE
   and REWARD_END_DATE = @REWARD_END_DATE;
";

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    List<SqlParameter> listPara = new List<SqlParameter>();
                    listPara.Add(new SqlParameter("@REWARD_START_DATE", reward_start_date));
                    listPara.Add(new SqlParameter("@REWARD_END_DATE", reward_end_date));

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

            if (dt.Rows.Count == 0)
            { return null; }
            else
            { return dt.Rows[0]; }
        }

        public void UpdateACICPInvoiceData(string cpt_date, string reward_start_date, string reward_end_date, string is_apply, string note, string user_id, string user_ip, decimal invoice_amt)
        {
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();

                    string sqlText = @"
begin try
    begin tran

        update AC_ICP_INVOICE_DATA
           set IS_APPLY = @IS_APPLY, 
               NOTE = @NOTE
        output inserted.CPT_DATE
              ,inserted.REWARD_START_DATE
              ,inserted.REWARD_END_DATE
              ,inserted.STAGE
              ,inserted.PRE_ISSUANCE_AMT
              ,inserted.REWARD_AMT
              ,inserted.TOTAL_REWARD_AMT
              ,inserted.LIMIT
              ,inserted.OVER_LIMIT
              ,inserted.TOTAL_BINDING_COUNT
              ,inserted.BINDING_COUNT_DONATE
              ,inserted.BINDING_COUNT_ATM
              ,inserted.BINDING_COUNT_ICP
              ,inserted.ESTIMATE_INVOICE
              ,inserted.BALANCE
              ,inserted.INVOICE_AMT
              ,inserted.BAL_SHORT
              ,inserted.IS_APPLY
              ,inserted.NOTE
              ,@UPDATE_USER
              ,@UPDATE_DATE
              ,'Update'
          into AC_ICP_INVOICE_DATA_LOG (CPT_DATE, REWARD_START_DATE, REWARD_END_DATE, STAGE, PRE_ISSUANCE_AMT, REWARD_AMT, TOTAL_REWARD_AMT, LIMIT, OVER_LIMIT, TOTAL_BINDING_COUNT, BINDING_COUNT_DONATE, BINDING_COUNT_ATM, BINDING_COUNT_ICP, ESTIMATE_INVOICE, BALANCE, INVOICE_AMT, BAL_SHORT, IS_APPLY, NOTE, ACTION_USER, ACTION_DATE, ACTION_TYPE)
         where REWARD_START_DATE = @REWARD_START_DATE
           and REWARD_END_DATE = @REWARD_END_DATE;

        update AC_ICP_INVOICE_RECEIVE
           set UPDATE_USER = @UPDATE_USER, 
               UPDATE_IP   = @UPDATE_IP, 
               UPDATE_DATE = @UPDATE_DATE,
               RECEIVE_AMT = case when @IS_APPLY = 'Y' then @INVOICE_AMT else 0 end
        output inserted.CREATE_USER
              ,inserted.CREATE_IP
              ,inserted.CREATE_DATE
              ,inserted.UPDATE_USER
              ,inserted.UPDATE_IP
              ,inserted.UPDATE_DATE
              ,inserted.CPT_DATE
              ,inserted.RECEIVE_DATE
              ,inserted.RECEIVE_AMT
              ,@UPDATE_USER
              ,@UPDATE_DATE
              ,'Update'
          into AC_ICP_INVOICE_RECEIVE_LOG (CREATE_USER, CREATE_IP, CREATE_DATE, UPDATE_USER, UPDATE_IP, UPDATE_DATE, CPT_DATE, RECEIVE_DATE, RECEIVE_AMT, ACTION_USER, ACTION_DATE, ACTION_TYPE)
         where CPT_DATE = @CPT_DATE;

        update AC_ICP_INVOICE_MASTER
           set UPDATE_USER = @UPDATE_USER, 
               UPDATE_IP   = @UPDATE_IP, 
               UPDATE_DATE = @UPDATE_DATE,
               IS_ACTIVE   = @IS_APPLY
        output inserted.CREATE_USER
              ,inserted.CREATE_IP
              ,inserted.CREATE_DATE
              ,inserted.UPDATE_USER
              ,inserted.UPDATE_IP
              ,inserted.UPDATE_DATE
              ,inserted.REWARD_START_DATE
              ,inserted.STAGE
              ,inserted.ISSUANCE_AMT
              ,inserted.IS_ACTIVE
              ,@UPDATE_USER
              ,@UPDATE_DATE
              ,'Update'
          into AC_ICP_INVOICE_MASTER_LOG (CREATE_USER, CREATE_IP, CREATE_DATE, UPDATE_USER, UPDATE_IP, UPDATE_DATE, REWARD_START_DATE, STAGE, ISSUANCE_AMT, IS_ACTIVE, ACTION_USER, ACTION_DATE, ACTION_TYPE)
         where REWARD_START_DATE = @CPT_DATE;

    commit tran
end try
begin catch

    rollback tran;

    DECLARE @ErrorSeverity NUMERIC = ERROR_SEVERITY();
    DECLARE @ErrorState NUMERIC = ERROR_STATE();
    DECLARE @ErrorMessage VARCHAR(MAX) = '錯誤行號:'+CONVERT(VARCHAR,ERROR_LINE())+'。 錯誤訊息:'+ERROR_MESSAGE();

    RAISERROR( @ErrorMessage, @ErrorSeverity, @ErrorState);

end catch
";

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    List<SqlParameter> listPara = new List<SqlParameter>();
                    sqlCmd.Parameters.Add(new SqlParameter("@IS_APPLY", is_apply));
                    sqlCmd.Parameters.Add(new SqlParameter("@NOTE", note));
                    sqlCmd.Parameters.Add(new SqlParameter("@INVOICE_AMT", invoice_amt));
                    sqlCmd.Parameters.Add(new SqlParameter("@CPT_DATE", cpt_date));
                    sqlCmd.Parameters.Add(new SqlParameter("@REWARD_START_DATE", reward_start_date));
                    sqlCmd.Parameters.Add(new SqlParameter("@REWARD_END_DATE", reward_end_date));
                    sqlCmd.Parameters.Add(new SqlParameter("@UPDATE_USER", user_id));
                    sqlCmd.Parameters.Add(new SqlParameter("@UPDATE_IP", user_ip));
                    sqlCmd.Parameters.Add(new SqlParameter("@UPDATE_DATE", DateTime.Now));

                    sqlCmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                log.Debug(ex.StackTrace);
                log.Debug(ex.Message);

                throw ex;
            }
        }

        public void UpdateACICASHInvoiceData(string cpt_date, string reward_start_date, string reward_end_date, string is_apply, string note, string user_id, string user_ip, decimal invoice_amt)
        {
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();

                    string sqlText = @"
begin try
    begin tran

        update AC_ICASH_INVOICE_DATA
           set IS_APPLY = @IS_APPLY, 
               NOTE = @NOTE
        output inserted.CPT_DATE
              ,inserted.REWARD_START_DATE
              ,inserted.REWARD_END_DATE
              ,inserted.STAGE
              ,inserted.PRE_ISSUANCE_AMT
              ,inserted.REWARD_AMT
              ,inserted.TOTAL_REWARD_AMT
              ,inserted.LIMIT
              ,inserted.OVER_LIMIT
              ,inserted.TOTAL_BINDING_COUNT
              ,inserted.BINDING_COUNT_DONATE
              ,inserted.BINDING_COUNT_ATM
              ,inserted.BINDING_COUNT_ICASH
              ,inserted.ESTIMATE_INVOICE
              ,inserted.BALANCE
              ,inserted.INVOICE_AMT
              ,inserted.BAL_SHORT
              ,inserted.IS_APPLY
              ,inserted.NOTE
              ,@UPDATE_USER
              ,@UPDATE_DATE
              ,'Update'
          into AC_ICASH_INVOICE_DATA_LOG (CPT_DATE, REWARD_START_DATE, REWARD_END_DATE, STAGE, PRE_ISSUANCE_AMT, REWARD_AMT, TOTAL_REWARD_AMT, LIMIT, OVER_LIMIT, TOTAL_BINDING_COUNT, BINDING_COUNT_DONATE, BINDING_COUNT_ATM, BINDING_COUNT_ICASH, ESTIMATE_INVOICE, BALANCE, INVOICE_AMT, BAL_SHORT, IS_APPLY, NOTE, ACTION_USER, ACTION_DATE, ACTION_TYPE)
         where REWARD_START_DATE = @REWARD_START_DATE
           and REWARD_END_DATE = @REWARD_END_DATE;

        update AC_ICASH_INVOICE_RECEIVE
           set UPDATE_USER = @UPDATE_USER, 
               UPDATE_IP   = @UPDATE_IP, 
               UPDATE_DATE = @UPDATE_DATE,
               RECEIVE_AMT = case when @IS_APPLY = 'Y' then @INVOICE_AMT else 0 end
        output inserted.CREATE_USER
              ,inserted.CREATE_IP
              ,inserted.CREATE_DATE
              ,inserted.UPDATE_USER
              ,inserted.UPDATE_IP
              ,inserted.UPDATE_DATE
              ,inserted.CPT_DATE
              ,inserted.RECEIVE_DATE
              ,inserted.RECEIVE_AMT
              ,@UPDATE_USER
              ,@UPDATE_DATE
              ,'Update'
          into AC_ICASH_INVOICE_RECEIVE_LOG (CREATE_USER, CREATE_IP, CREATE_DATE, UPDATE_USER, UPDATE_IP, UPDATE_DATE, CPT_DATE, RECEIVE_DATE, RECEIVE_AMT, ACTION_USER, ACTION_DATE, ACTION_TYPE)
         where CPT_DATE = @CPT_DATE;

        update AC_ICASH_INVOICE_MASTER
           set UPDATE_USER = @UPDATE_USER, 
               UPDATE_IP   = @UPDATE_IP, 
               UPDATE_DATE = @UPDATE_DATE,
               IS_ACTIVE   = @IS_APPLY
        output inserted.CREATE_USER
              ,inserted.CREATE_IP
              ,inserted.CREATE_DATE
              ,inserted.UPDATE_USER
              ,inserted.UPDATE_IP
              ,inserted.UPDATE_DATE
              ,inserted.REWARD_START_DATE
              ,inserted.STAGE
              ,inserted.ISSUANCE_AMT
              ,inserted.IS_ACTIVE
              ,@UPDATE_USER
              ,@UPDATE_DATE
              ,'Update'
          into AC_ICASH_INVOICE_MASTER_LOG (CREATE_USER, CREATE_IP, CREATE_DATE, UPDATE_USER, UPDATE_IP, UPDATE_DATE, REWARD_START_DATE, STAGE, ISSUANCE_AMT, IS_ACTIVE, ACTION_USER, ACTION_DATE, ACTION_TYPE)
         where REWARD_START_DATE = @CPT_DATE;

    commit tran
end try
begin catch

    rollback tran;

    DECLARE @ErrorSeverity NUMERIC = ERROR_SEVERITY();
    DECLARE @ErrorState NUMERIC = ERROR_STATE();
    DECLARE @ErrorMessage VARCHAR(MAX) = '錯誤行號:'+CONVERT(VARCHAR,ERROR_LINE())+'。 錯誤訊息:'+ERROR_MESSAGE();

    RAISERROR( @ErrorMessage, @ErrorSeverity, @ErrorState);

end catch
";

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    List<SqlParameter> listPara = new List<SqlParameter>();
                    sqlCmd.Parameters.Add(new SqlParameter("@IS_APPLY", is_apply));
                    sqlCmd.Parameters.Add(new SqlParameter("@NOTE", note));
                    sqlCmd.Parameters.Add(new SqlParameter("@INVOICE_AMT", invoice_amt));
                    sqlCmd.Parameters.Add(new SqlParameter("@CPT_DATE", cpt_date));
                    sqlCmd.Parameters.Add(new SqlParameter("@REWARD_START_DATE", reward_start_date));
                    sqlCmd.Parameters.Add(new SqlParameter("@REWARD_END_DATE", reward_end_date));
                    sqlCmd.Parameters.Add(new SqlParameter("@UPDATE_USER", user_id));
                    sqlCmd.Parameters.Add(new SqlParameter("@UPDATE_IP", user_ip));
                    sqlCmd.Parameters.Add(new SqlParameter("@UPDATE_DATE", DateTime.Now));

                    sqlCmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                log.Debug(ex.StackTrace);
                log.Debug(ex.Message);

                throw ex;
            }
        }
    }
}
