using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace DataAccess
{
    public class ICASHOPReportDAO : BasicDAL
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ReportACTDAO));

        public DataTable Report_01(string yearMonth, string Group_TaiYa, string Group_TaiSu, string Group_XiOu, string Group_FuMou, string Group_TongYiDuJiaCun)
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["ICASHOPConnection"].ToString()))
                {
                    sqlConn.Open();
                    string sqlText = File.ReadAllText(System.AppDomain.CurrentDomain.BaseDirectory + "\\Sql\\ReportICASHOP\\ReportICASHOP_01.sql", Encoding.GetEncoding("big5"));

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    sqlCmd.Parameters.Add("@TARGET_MONTH", SqlDbType.VarChar).Value = yearMonth;
                    sqlCmd.Parameters.Add("@Group_TaiYa", SqlDbType.VarChar).Value = Group_TaiYa;
                    sqlCmd.Parameters.Add("@Group_TaiSu", SqlDbType.VarChar).Value = Group_TaiSu;
                    sqlCmd.Parameters.Add("@Group_XiOu", SqlDbType.VarChar).Value = Group_XiOu;
                    sqlCmd.Parameters.Add("@Group_FuMou", SqlDbType.VarChar).Value = Group_FuMou;
                    sqlCmd.Parameters.Add("@Group_TongYiDuJiaCun", SqlDbType.VarChar).Value = Group_TongYiDuJiaCun;

                    SqlDataAdapter adapter = new SqlDataAdapter(sqlCmd);
                    adapter.Fill(dt);

                    sqlConn.Close();

                }
            }

            catch (Exception ex)
            {
                log.Debug(ex.Message);
            }
            return dt;
        }

        public DataTable Report_02(string yearMonth, string Group_TaiYa, string Group_TaiSu, string Group_XiOu, string Group_FuMou, string Group_TongYiDuJiaCun)
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["ICASHOPConnection"].ToString()))
                {
                    sqlConn.Open();
                    string sqlText = File.ReadAllText(System.AppDomain.CurrentDomain.BaseDirectory + "\\Sql\\ReportICASHOP\\ReportICASHOP_02.sql", Encoding.GetEncoding("big5"));

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    sqlCmd.Parameters.Add("@TARGET_MONTH", SqlDbType.VarChar).Value = yearMonth;
                    sqlCmd.Parameters.Add("@Group_TaiYa", SqlDbType.VarChar).Value = Group_TaiYa;
                    sqlCmd.Parameters.Add("@Group_TaiSu", SqlDbType.VarChar).Value = Group_TaiSu;
                    sqlCmd.Parameters.Add("@Group_XiOu", SqlDbType.VarChar).Value = Group_XiOu;
                    sqlCmd.Parameters.Add("@Group_FuMou", SqlDbType.VarChar).Value = Group_FuMou;
                    sqlCmd.Parameters.Add("@Group_TongYiDuJiaCun", SqlDbType.VarChar).Value = Group_TongYiDuJiaCun;

                    SqlDataAdapter adapter = new SqlDataAdapter(sqlCmd);
                    adapter.Fill(dt);

                    sqlConn.Close();

                }
            }

            catch (Exception ex)
            {
                log.Debug(ex.Message);
            }
            return dt;
        }

        public DataTable Report_03(string yearMonth, string Group_TaiYa, string Group_TaiSu, string Group_XiOu, string Group_FuMou, string Group_TongYiDuJiaCun)
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["ICASHOPConnection"].ToString()))
                {
                    sqlConn.Open();
                    string sqlText = File.ReadAllText(System.AppDomain.CurrentDomain.BaseDirectory + "\\Sql\\ReportICASHOP\\ReportICASHOP_03.sql", Encoding.GetEncoding("big5"));

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    sqlCmd.Parameters.Add("@TARGET_MONTH", SqlDbType.VarChar).Value = yearMonth;
                    sqlCmd.Parameters.Add("@Group_TaiYa", SqlDbType.VarChar).Value = Group_TaiYa;
                    sqlCmd.Parameters.Add("@Group_TaiSu", SqlDbType.VarChar).Value = Group_TaiSu;
                    sqlCmd.Parameters.Add("@Group_XiOu", SqlDbType.VarChar).Value = Group_XiOu;
                    sqlCmd.Parameters.Add("@Group_FuMou", SqlDbType.VarChar).Value = Group_FuMou;
                    sqlCmd.Parameters.Add("@Group_TongYiDuJiaCun", SqlDbType.VarChar).Value = Group_TongYiDuJiaCun;

                    SqlDataAdapter adapter = new SqlDataAdapter(sqlCmd);
                    adapter.Fill(dt);

                    sqlConn.Close();

                }
            }

            catch (Exception ex)
            {
                log.Debug(ex.Message);
            }
            return dt;
        }

        public DataTable Report_04(string yearMonth, string Group_TaiYa, string Group_TaiSu, string Group_XiOu, string Group_FuMou, string Group_TongYiDuJiaCun)
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["ICASHOPConnection"].ToString()))
                {
                    sqlConn.Open();
                    string sqlText = File.ReadAllText(System.AppDomain.CurrentDomain.BaseDirectory + "\\Sql\\ReportICASHOP\\ReportICASHOP_04.sql", Encoding.GetEncoding("big5"));

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    sqlCmd.Parameters.Add("@TARGET_MONTH", SqlDbType.VarChar).Value = yearMonth;
                    sqlCmd.Parameters.Add("@Group_TaiYa", SqlDbType.VarChar).Value = Group_TaiYa;
                    sqlCmd.Parameters.Add("@Group_TaiSu", SqlDbType.VarChar).Value = Group_TaiSu;
                    sqlCmd.Parameters.Add("@Group_XiOu", SqlDbType.VarChar).Value = Group_XiOu;
                    sqlCmd.Parameters.Add("@Group_FuMou", SqlDbType.VarChar).Value = Group_FuMou;
                    sqlCmd.Parameters.Add("@Group_TongYiDuJiaCun", SqlDbType.VarChar).Value = Group_TongYiDuJiaCun;

                    SqlDataAdapter adapter = new SqlDataAdapter(sqlCmd);
                    adapter.Fill(dt);

                    sqlConn.Close();

                }
            }

            catch (Exception ex)
            {
                log.Debug(ex.Message);
            }
            return dt;
        }
    }
}
