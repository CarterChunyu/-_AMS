using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Web;

using log4net;
using Domain;

namespace DataAccess
{
    public class ReportSystemDAO : BasicDAL
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ReportSystemDAO));

        private GmSchemaObjDAO schemaDAO = new GmSchemaObjDAO();


        public DataTable ReportSystem160501(string start, string end, string group, string schemaId, string merchantNo)
        {
            DataTable dt = new DataTable();
            
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();

                    string sqlStatement = "";
                    
                    if (schemaId == "ALL")
                    {
                        List<KeyValuePair<GmSchemaObj, string>> schemaList = new List<KeyValuePair<GmSchemaObj, string>>();

                        if (group == "ALL")
                        {
                            
                            foreach (KeyValuePair<GmSchemaObj, string> schemaObj in schemaDAO.FindAllSchemaNumber())
                            {
                                string schema = schemaObj.Key.MerchantSchemaId;
                                if (schema != "ZOO" && schema != "MCD" && schema != "UPN" && schema != "PZH" && schema != "ICA" && schema != "UBK" && schema != "PRTC" && schema != "KIN" && schema != "UTI")
                                    schemaList.Add(schemaObj);
                            }
                        }
                        else
                        {
                            
                            foreach (KeyValuePair<GmSchemaObj, string> schemaObj in schemaDAO.FindSchemaNumberByGroup(group))
                            {
                                string schema = schemaObj.Key.MerchantSchemaId;
                                if (schema != "ZOO" && schema != "MCD" && schema != "UPN" && schema != "PZH" && schema != "ICA" && schema != "UBK" && schema != "PRTC" && schema != "KIN" && schema != "UTI")
                                    schemaList.Add(schemaObj);
                            }
                        }

                        sqlStatement += "SELECT * FROM (";
                        //sqlText = "SELECT * FROM' ('+";

                        foreach (KeyValuePair<GmSchemaObj, string> schemaObj in schemaList)
                        {
                            if (schemaObj.Key.MerchantSchemaId != schemaList.Last().Key.MerchantSchemaId || schemaObj.Value != schemaList.Last().Value)
                            {
                                string sqlText = "";

                                sqlText += File.ReadAllText(System.AppDomain.CurrentDomain.BaseDirectory + "\\Sql\\ReportSystem\\ReportSystem160501_01.sql", Encoding.GetEncoding("big5"));
                                //log.Debug(sqlText);
                                sqlText += " FROM GM_COMMON_M WHERE MERCHANT_NO='{3}' AND TYPE_GROUP='BATCH_TABLE'";
                                sqlText= string.Format(sqlText,schemaObj.Key.MerchantSchemaId, start, end,schemaObj.Value);
                                SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                                SqlDataReader dr = sqlCmd.ExecuteReader();

                                bool solidRecord = false;
                                while (dr.Read())
                                {
                                    if (!DBNull.Value.Equals(dr["SQLS"]))
                                    {
                                        solidRecord = true;
                                        sqlStatement += dr["SQLS"].ToString();                                       
                                    }
                                }

                                if (solidRecord == true)
                                {
                                    sqlStatement += " UNION ALL ";
                                }
                                dr.Close();
                                
                            }
                            else
                            {
                                string sqlText = "";
                                sqlText += File.ReadAllText(System.AppDomain.CurrentDomain.BaseDirectory + "\\Sql\\ReportSystem\\ReportSystem160501_01.sql", Encoding.GetEncoding("big5"));
                                sqlText += " FROM GM_COMMON_M WHERE MERCHANT_NO='{3}' AND TYPE_GROUP='BATCH_TABLE'";
                                sqlText= string.Format(sqlText,schemaObj.Key.MerchantSchemaId, start, end,schemaObj.Value);
                                SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                                SqlDataReader dr = sqlCmd.ExecuteReader();
                                while (dr.Read())
                                {
                                    sqlStatement += dr["SQLS"].ToString();
                                }
                                dr.Close();
                                sqlStatement+=" ) X ORDER BY X.CPT_DATE,X.MERCHANT_NO ";
                                
                            }
                        }
                        //log.Debug(sqlStatement);

                    }
                    else
                    {
                        string sqlText = "";
                        
                        if (schemaId == null) sqlText = "";
                        else
                        {
                            sqlText = File.ReadAllText(System.AppDomain.CurrentDomain.BaseDirectory + "\\Sql\\ReportSystem\\ReportSystem160501_01.sql", Encoding.GetEncoding("big5"));
                            
                        }
                        sqlText += " FROM GM_COMMON_M WHERE MERCHANT_NO='{3}' AND TYPE_GROUP='BATCH_TABLE'";
                        sqlText= string.Format(sqlText,schemaId, start, end,merchantNo);
                        SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                        SqlDataReader dr = sqlCmd.ExecuteReader();
                        while (dr.Read())
                        {
                            sqlStatement += dr["SQLS"].ToString();
                        }
                        dr.Close();
                        //log.Debug(sqlText);
                    }

                    
                    /*
                    if (schemaId != null)
                    {
                        SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                        SqlDataReader dr = sqlCmd.ExecuteReader();
                        while (dr.Read())
                        {
                            sqlStatement = dr["SQLS"].ToString();
                        }

                    }
                    */

                    if (schemaId != null)
                    {
                        SqlCommand sqlCmd = new SqlCommand(sqlStatement, sqlConn);
                        SqlDataAdapter da = new SqlDataAdapter(sqlCmd);
                        da.Fill(dt);
                    }

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
