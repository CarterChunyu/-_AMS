using Common.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class ICASHOPOverdraftDAO:BasicOPDAL
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ICASHOPOverdraftDAO));
        public ICASHOPOverdraftDAO()
        {

        }

        public DataTable GetOverdraftData(string REPORT_YM)
        {
            DataTable dt = new DataTable();
            DateTime dateTime = DateTime.ParseExact(REPORT_YM, "yyyyMM", CultureInfo.InvariantCulture, DateTimeStyles.AllowWhiteSpaces);
            string EDate = dateTime.AddMonths(1).ToString("yyyyMM");
            try
            {
                using(SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    string sqlText = string.Concat(new object[] {
                        "SELECT",Environment.NewLine,
                        "   CptDate",Environment.NewLine,
                        "   , UnifiedBusinessNo",Environment.NewLine,
                        "   , StoreNo",Environment.NewLine,
                        "   , PosNo",Environment.NewLine,
                        "   , TransNo",Environment.NewLine,
                        "   , TransDate",Environment.NewLine,
                        "   , CardType",Environment.NewLine,
                        "   , CardNo",Environment.NewLine,
                        "   , PointType",Environment.NewLine,
                        "   , OverdraftPoint",Environment.NewLine,
                        "   , CorrectionPoint",Environment.NewLine,
                        "   , WriteOffNo",Environment.NewLine,
                        "   , CreateDate",Environment.NewLine,
                        "   , FileName",Environment.NewLine,
                        "FROM OpenPoint_Overdraft",Environment.NewLine,
                        "WHERE SUBSTRING(FileName,21,8) BETWEEN @DATEStart AND @DATEEnd",Environment.NewLine
                    });

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    sqlCmd.Parameters.Add(new SqlParameter("@DATEStart", string.Format("{0}02", REPORT_YM)));
                    sqlCmd.Parameters.Add(new SqlParameter("@DATEEnd", string.Format("{0}01", EDate)));
                    SqlDataAdapter adapter = new SqlDataAdapter(sqlCmd);
                    adapter.Fill(dt);
                }
            }catch(Exception ex)
            {
                log.Debug(ex.StackTrace);
                log.Debug(ex.Message);
            }
            return dt;
        }
    }
}
