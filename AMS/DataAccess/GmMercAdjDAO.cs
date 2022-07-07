using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.SqlClient;

using log4net;
using Domain;

namespace DataAccess
{
    public class GmMercAdjDAO: BasicDAL
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(GmMercAdjDAO));

        
        public string FindTypes(string merchantNo,string DRDP)
        {
            GmSchemaObjDAO schObjDAO = new GmSchemaObjDAO();

            GmSchemaObj schObj = schObjDAO.FindSchemaObj(merchantNo);

            string schema = schObj.MerchantSchemaId;

            string sqlStatement = "";

            using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
            {
                sqlConn.Open();

                string sqlText = string.Format("SELECT 'SELECT TRANS_TYPE,'+TRANS_TYPE_NAME+' AS NAME FROM '+CASE SERVER_NAME WHEN '' THEN '{0}.BM_TRANS_TYPE ' ELSE SERVER_NAME+'.{0}.BM_TRANS_TYPE ' END+'WHERE '+ARAP_NAME+'=''{1}'''+WHERE_CONDITION AS SQL FROM GM_MERC_ADJ WHERE MERCHANT_NO ='{2}' AND GETDATE() BETWEEN EFF_DATE_FROM AND EFF_DATE_TO",schema,DRDP,merchantNo);
                SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                SqlDataReader dr = sqlCmd.ExecuteReader();
                while (dr.Read())
                {
                    sqlStatement = dr["SQL"].ToString();
                }

                sqlConn.Close();
            }

            return sqlStatement;

        }

        public string FindTypeName(string merchantNo, string transType)
        {
            GmSchemaObjDAO schObjDAO = new GmSchemaObjDAO();

            GmSchemaObj schObj = schObjDAO.FindSchemaObj(merchantNo);

            string schema = schObj.MerchantSchemaId;

            string sqlStatement = "";

            using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
            {
                sqlConn.Open();

                string sqlText = string.Format("SELECT 'SELECT '+TRANS_TYPE_NAME+' AS NAME FROM '+CASE SERVER_NAME WHEN '' THEN '{0}.BM_TRANS_TYPE ' ELSE SERVER_NAME+'.{0}.BM_TRANS_TYPE ' END+'WHERE TRANS_TYPE=''{1}'''+WHERE_CONDITION AS SQL FROM GM_MERC_ADJ WHERE MERCHANT_NO='{2}' AND GETDATE() BETWEEN EFF_DATE_FROM AND EFF_DATE_TO",schema,transType,merchantNo);
                SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                SqlDataReader dr = sqlCmd.ExecuteReader();
                while (dr.Read())
                {
                    sqlStatement = dr["SQL"].ToString();
                }

                sqlConn.Close();
            }

            return sqlStatement;

        }
    }
}
