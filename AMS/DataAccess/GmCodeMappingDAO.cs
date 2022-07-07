using Domain;
using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DataAccess
{
    public class GmCodeMappingDAO : BasicDAL
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(GmMerchantDAO));

        public GmCodeMappingDAO()
        {
        }

        public DataTable FindData(string mapping_group, string code_type, string input_value)
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
  FROM dbo.GM_CODE_MAPPING M
 WHERE 1 = 1
";

                    if (mapping_group != "")
                    {
                        sqlText += @"   AND MAPPING_GROUP = @MAPPING_GROUP ";
                        listPara.Add(new SqlParameter("MAPPING_GROUP", SqlDbType.VarChar) { Value = mapping_group });
                    }
                    if (code_type != "")
                    {
                        sqlText += @"   AND CODE_TYPE = @CODE_TYPE ";
                        listPara.Add(new SqlParameter("CODE_TYPE", SqlDbType.VarChar) { Value = code_type });
                    }
                    if (input_value != "")
                    {
                        sqlText += @"   AND INPUT_VALUE = @INPUT_VALUE ";
                        listPara.Add(new SqlParameter("INPUT_VALUE", SqlDbType.VarChar) { Value = input_value });
                    }

                    sqlText += @" ORDER BY MAPPING_GROUP, CODE_TYPE, INPUT_VALUE ";

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
