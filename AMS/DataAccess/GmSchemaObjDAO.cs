using Domain;
using log4net;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class GmSchemaObjDAO:BasicDAL
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(GmSchemaObjDAO));

        public List<GmSchemaObj> FindAllSchema()
        {
            List<GmSchemaObj> schemaList = new List<GmSchemaObj>();

            using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
            {
                sqlConn.Open();
   
                string sqlText = @"SELECT DISTINCT MERCH_SCHEMAID,MERCHANT_STNAME as MERCHANT_NAME FROM GM_MERCHANT";
                SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                SqlDataReader dr = sqlCmd.ExecuteReader();
                while (dr.Read())
                {
                    GmSchemaObj schemaObj = new GmSchemaObj();
                    schemaObj.MerchantSchemaId = dr["MERCH_SCHEMAID"].ToString();
                    schemaObj.MerchantName = dr["MERCHANT_NAME"].ToString();
                    schemaList.Add(schemaObj);
                }
                
                sqlConn.Close();
            }

            return schemaList;

        }

        public Dictionary<GmSchemaObj,string> FindAllSchemaNumber()
        {
            Dictionary<GmSchemaObj,string> schemaList = new Dictionary<GmSchemaObj,string>();

            using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
            {
                sqlConn.Open();

                string sqlText = @"SELECT DISTINCT b.MERCH_SCHEMAID,b.MERCHANT_STNAME as MERCHANT_NAME,b.MERCHANT_NO  
                                FROM GM_MERCHANT_TYPE_D a 
                                INNER JOIN GM_MERCHANT b 
                                ON a.MERCHANT_NO=b.MERCHANT_NO 
                                WHERE SHOW_FLG = 'Y'";
                SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                SqlDataReader dr = sqlCmd.ExecuteReader();
                while (dr.Read())
                {
                    GmSchemaObj schemaObj = new GmSchemaObj();
                    schemaObj.MerchantSchemaId = dr["MERCH_SCHEMAID"].ToString();
                    schemaObj.MerchantName = dr["MERCHANT_NAME"].ToString();
                    string merchantNo = dr["MERCHANT_NO"].ToString();
                    schemaList.Add(schemaObj,merchantNo);
                }

                sqlConn.Close();
            }

            return schemaList;

        }

        public Dictionary<GmSchemaObj, string> FindSchemaNumberByGroup(string group)
        {
            Dictionary<GmSchemaObj, string> schemaList = new Dictionary<GmSchemaObj, string>();

            using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
            {
                sqlConn.Open();

                string sqlText = string.Format(@"SELECT DISTINCT b.MERCH_SCHEMAID,b.MERCHANT_STNAME as MERCHANT_NAME,b.MERCHANT_NO  FROM GM_MERCHANT_TYPE_D a INNER JOIN GM_MERCHANT b ON a.MERCHANT_NO=b.MERCHANT_NO WHERE a.GROUP_ID='{0}'", group);
                SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                SqlDataReader dr = sqlCmd.ExecuteReader();
                while (dr.Read())
                {
                    GmSchemaObj schemaObj = new GmSchemaObj();
                    schemaObj.MerchantSchemaId = dr["MERCH_SCHEMAID"].ToString();
                    schemaObj.MerchantName = dr["MERCHANT_NAME"].ToString();
                    string merchantNo = dr["MERCHANT_NO"].ToString();
                    schemaList.Add(schemaObj,merchantNo);
                }

                sqlConn.Close();
            }

            return schemaList;

        }

        public List<GmSchemaObj> FindSchemaByGroup(string group)
        {
            List<GmSchemaObj> schemaList = new List<GmSchemaObj>();

            using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
            {
                sqlConn.Open();

                string sqlText = string.Format(@"SELECT DISTINCT b.MERCH_SCHEMAID,b.MERCHANT_STNAME as MERCHANT_NAME FROM GM_MERCHANT_TYPE_D a INNER JOIN GM_MERCHANT b ON a.MERCHANT_NO=b.MERCHANT_NO WHERE a.GROUP_ID='{0}'",group);
                SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                SqlDataReader dr = sqlCmd.ExecuteReader();
                while (dr.Read())
                {
                    GmSchemaObj schemaObj = new GmSchemaObj();
                    schemaObj.MerchantSchemaId = dr["MERCH_SCHEMAID"].ToString();
                    schemaObj.MerchantName = dr["MERCHANT_NAME"].ToString();
                    schemaList.Add(schemaObj);
                }

                sqlConn.Close();
            }

            return schemaList;

        }

        public List<GmSchemaObj> FindAllSchemaSET()
        {
            List<GmSchemaObj> schemaList = new List<GmSchemaObj>();

            using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
            {
                sqlConn.Open();

                string sqlText = @"SELECT MERCH_SCHEMAID,MERCHANT_STNAME as MERCHANT_NAME FROM GM_MERCHANT WHERE MERC_GROUP='SET'";
                SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                SqlDataReader dr = sqlCmd.ExecuteReader();
                while (dr.Read())
                {
                    GmSchemaObj schemaObj = new GmSchemaObj();
                    schemaObj.MerchantSchemaId = dr["MERCH_SCHEMAID"].ToString();
                    schemaObj.MerchantName = dr["MERCHANT_NAME"].ToString();
                    schemaList.Add(schemaObj);
                }

                sqlConn.Close();
            }

            return schemaList;

        }
        public  GmSchemaObj FindSchemaObj(string merchantNo)
        {
            GmSchemaObj schema = new GmSchemaObj();

            using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
            {
                sqlConn.Open();
                if (merchantNo == "ALL")
                {
                    schema.MerchantSchemaId = "ALL";
                    schema.MerchantName = "ALL";
                }
                else
                {
                    string sqlText = string.Format("SELECT MERCH_SCHEMAID,MERCHANT_STNAME as MERCHANT_NAME FROM GM_MERCHANT WHERE MERCHANT_NO='{0}'", merchantNo);
                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    SqlDataReader dr = sqlCmd.ExecuteReader();
                    while (dr.Read())
                    {
                        schema.MerchantSchemaId = dr["MERCH_SCHEMAID"].ToString();
                        schema.MerchantName = dr["MERCHANT_NAME"].ToString();
                    }
                }

                sqlConn.Close();
            }

            return schema;
        }

    }
}
