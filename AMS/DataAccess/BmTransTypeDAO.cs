using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;
using System.Data.SqlClient;
using log4net;

using Domain;
using Domain.Entities;

namespace DataAccess
{
    public class BmTransTypeDAO : BasicDAL
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(BmTransTypeDAO));

      
        public List<BmTransType> DRCollection(string merchantNo)
        {
            List<BmTransType> bmList = new List<BmTransType>();

            GmMercAdjDAO mercAdjDAO = new GmMercAdjDAO();

            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();
                    string sqlText = mercAdjDAO.FindTypes(merchantNo,"DR");

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    DataTable dt = new DataTable();
                    SqlDataAdapter adapter = new SqlDataAdapter(sqlCmd);
                    adapter.Fill(dt);
                    DataTableReader dr = dt.CreateDataReader();
                    while (dr.Read())
                    {
                        BmTransType bm = new BmTransType();
                        
                        bm.TransType = dr["TRANS_TYPE"].ToString();
                        

                        bm.ACTNameMerchant = dr["NAME"].ToString();

                        bmList.Add(bm);
                    }
                    dr.Close();
                    dt.Dispose();
                    adapter.Dispose();
                    sqlCmd.Dispose();
                    sqlConn.Close();
                }

            }
            catch (Exception ex)
            {
                log.Debug(ex.Message);
            }
            return bmList;
        }

        public List<BmTransType> DPCollection(string merchantNo)
        {
            List<BmTransType> bmList = new List<BmTransType>();

            GmMercAdjDAO mercAdjDAO = new GmMercAdjDAO();

            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();
                    string sqlText = mercAdjDAO.FindTypes(merchantNo, "DP");

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);

                    DataTable dt = new DataTable();
                    SqlDataAdapter adapter = new SqlDataAdapter(sqlCmd);
                    adapter.Fill(dt);
                    DataTableReader dr = dt.CreateDataReader();
                    while (dr.Read())
                    {
                        BmTransType bm = new BmTransType();
                        
                        bm.TransType = dr["TRANS_TYPE"].ToString();
                       
                        bm.ACTNameMerchant = dr["NAME"].ToString();

                        bmList.Add(bm);
                    }
                    dr.Close();
                    dt.Dispose();
                    adapter.Dispose();
                    sqlCmd.Dispose();
                    sqlConn.Close();
                }

            }
            catch (Exception ex)
            {
                log.Debug(ex.Message);
            }
            return bmList;
        }

        public string ItemString(string merchantNo, string transType)
        {
            BmTransType bm = new BmTransType();

            GmMercAdjDAO mercAdjDAO = new GmMercAdjDAO();

            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();
                    string sqlText = mercAdjDAO.FindTypeName(merchantNo,transType);

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);

                    DataTable dt = new DataTable();
                    SqlDataAdapter adapter = new SqlDataAdapter(sqlCmd);
                    adapter.Fill(dt);
                    DataTableReader dr = dt.CreateDataReader();

                    while (dr.Read())
                    {
                        bm.ACTNameMerchant = dr["NAME"].ToString();
                    }

                    dr.Close();
                    dt.Dispose();
                    adapter.Dispose();
                    sqlCmd.Dispose();
                    sqlConn.Close();
                }

            }
            catch (Exception ex)
            {
                log.Debug(ex.Message);
            }
            return bm.ACTNameMerchant;
        }
    }
}
