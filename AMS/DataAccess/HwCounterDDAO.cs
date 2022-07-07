using Common.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class HwCounterDDAO :BasicDAL
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(HwCounterDDAO));
        public string TableName { private get; set; }

        public HwCounterDDAO()
        {
            this.TableName = "HW_COUNTER_D";
        }

        public int GetCounter(string y, string m, string ckind)
        {
            int counter = 0;
            //int itemRows = 0;
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();
                    String sqlText = @"select * from HW_COUNTER_D where CKIND=@CKIND and CYEAR=@YEAR and CMONTH=@MONTH ";

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    sqlCmd.Parameters.Add("@YEAR", SqlDbType.VarChar).Value = y;
                    sqlCmd.Parameters.Add("@MONTH", SqlDbType.VarChar).Value = m;
                    sqlCmd.Parameters.Add("@CKIND", SqlDbType.VarChar).Value = ckind;

                    using (sqlCmd)
                    {
                        SqlDataReader reader = sqlCmd.ExecuteReader();

                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                counter = Int32.Parse(reader["COUNT"].ToString());
                            }
                        }

                        reader.Close();
                    }
                }
            }
            catch (SqlException ex)
            {
                log.Debug(ex.Message);
            }
            return counter;
        }

        public void Insert(string y, string m, string ckind)
        {
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();

                    string sqlText =
                        @"INSERT INTO "
                        + this.TableName + @"
                                   (
                                    [CKIND]
                                   ,[CYEAR]
                                   ,[CMONTH]
                                   ,[COUNT]
                                    )
                            VALUES
                                   (
                                    @CKIND
                                   ,@CYEAR
                                   ,@CMONTH
                                   ,@COUNT
                                    )";

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    sqlCmd.Parameters.Add("@CKIND", SqlDbType.VarChar).Value = ckind;
                    sqlCmd.Parameters.Add("@CYEAR", SqlDbType.VarChar).Value = y;
                    sqlCmd.Parameters.Add("@CMONTH", SqlDbType.VarChar).Value = m;
                    sqlCmd.Parameters.Add("@COUNT", SqlDbType.Int).Value = 1;
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


        public void Update(string y, string m, string ckind)
        {
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();
                    string sqlText = @"
                       UPDATE "
                            + this.TableName + @" 
                        SET COUNT = COUNT + 1 where CKIND=@CKIND and CYEAR=@YEAR and CMONTH=@MONTH ";
                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    sqlCmd.Parameters.Add("@YEAR", SqlDbType.VarChar).Value = y;
                    sqlCmd.Parameters.Add("@MONTH", SqlDbType.VarChar).Value = m;
                    sqlCmd.Parameters.Add("@CKIND", SqlDbType.VarChar).Value = ckind;

                    sqlCmd.ExecuteNonQuery();
                }
            }
            catch (SqlException ex)
            {
                log.Debug(ex.Message);
                throw ex;
            }
        }
    }
}
