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
    public class ICASHOPDAO : BasicOPDAL
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ICASHOPDAO));
        public ICASHOPDAO()
        {

        }

        public DataTable GetGroupNameData()
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    string sqlText = string.Concat(new object[] {
                        "DECLARE @TargetPattern VARCHAR(5) = '%[-]%'",Environment.NewLine,
                        "SELECT GroupName",Environment.NewLine,
                        "FROM (",Environment.NewLine,
                        "    SELECT SUBSTRING(StoreName,1,CASE WHEN PATINDEX(@TargetPattern,StoreName)>0 THEN PATINDEX(@TargetPattern,StoreName)-1 ELSE 0 END) [GroupName]",Environment.NewLine,
                        "    FROM dbo.OpenPoint_Merchant_VIEW WITH(NOLOCK)",Environment.NewLine,
                        ") D",Environment.NewLine,
                        "WHERE GroupName<>''",Environment.NewLine,
                        "GROUP BY GroupName",Environment.NewLine,
                    });
                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
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
