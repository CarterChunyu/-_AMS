using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using log4net;
using System.Data;
using System.Data.SqlClient;

namespace DataAccess
{
    public class GmMerchantTypeDAO: BasicDAL
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(GmMerchantTypeDAO));

        public List<string> FindAll()
        {
            List<string> mNoList = new List<string>();
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();
                    string sqlText = @"select DISTINCT MERCHANT_NO FROM GM_MERCHANT_TYPE_D WHERE SHOW_FLG = 'Y' ORDER BY MERCHANT_NO";

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    SqlDataReader dr = sqlCmd.ExecuteReader();
                    while (dr.Read())
                    {
                        string mNo = dr["MERCHANT_NO"].ToString();

                        mNoList.Add(mNo);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Debug("Query Ex: GM_MERCHANT_TYPE_D");////
                log.Debug(ex.StackTrace);
                log.Debug(ex.Message);
                mNoList = null;
            }
            return mNoList;
        }

        public List<string> FindGroupAll(string group)
        {
            List<string> mNoList = new List<string>();
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();
                    string sqlText = string.Format(@"select DISTINCT MERCHANT_NO FROM GM_MERCHANT_TYPE_D WHERE GROUP_ID='{0}' AND SHOW_FLG = 'Y' ORDER BY MERCHANT_NO", group);

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    SqlDataReader dr = sqlCmd.ExecuteReader();
                    while (dr.Read())
                    {
                        string mNo = dr["MERCHANT_NO"].ToString();

                        mNoList.Add(mNo);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Debug("Query Ex: GM_MERCHANT_TYPE_D");////
                log.Debug(ex.StackTrace);
                log.Debug(ex.Message);
                mNoList = null;
            }
            return mNoList;
        }
        /// <summary>
        /// 找所有的特約機構，不含小商家
        /// </summary>
        /// <returns></returns>
        public List<string> FindAllNotMstore()
        {
            List<string> mNoList = new List<string>();
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();
                    string sqlText = string.Format(@"SELECT A.MERCHANT_NO FROM GM_MERCHANT_TYPE_D A
									INNER JOIN GM_MERCHANT B
									ON A.MERCHANT_NO = B.MERCHANT_NO
									WHERE SHOW_FLG = 'Y' 
									AND GROUP_ID <> 'MSTORE' 
									ORDER BY MERCHANT_NO ");

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    SqlDataReader dr = sqlCmd.ExecuteReader();
                    while (dr.Read())
                    {
                        string mNo = dr["MERCHANT_NO"].ToString();

                        mNoList.Add(mNo);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Debug("Query Ex: GM_MERCHANT_TYPE_D");////
                log.Debug(ex.StackTrace);
                log.Debug(ex.Message);
                mNoList = null;
            }
            return mNoList;
        }
        /// <summary>
        /// 找不含小商家的群組特約機構
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        public List<string> FindGroupNotMstore(string group)
        {
            List<string> mNoList = new List<string>();
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();
                    string sqlText = string.Format(@"select a.MERCHANT_NO from GM_MERCHANT_TYPE_D a 
									INNER JOIN  GM_MERCHANT b ON a.MERCHANT_NO=b.MERCHANT_NO 
									WHERE a.GROUP_ID ='{0}' AND a.SHOW_FLG='Y'
									AND a.GROUP_ID <> 'MSTORE'
									ORDER BY MERCHANT_NO", group);

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    SqlDataReader dr = sqlCmd.ExecuteReader();
                    while (dr.Read())
                    {
                        string mNo = dr["MERCHANT_NO"].ToString();

                        mNoList.Add(mNo);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Debug("Query Ex: GM_MERCHANT_TYPE_D");////
                log.Debug(ex.StackTrace);
                log.Debug(ex.Message);
                mNoList = null;
            }
            return mNoList;
        }
        /// <summary>
        /// 只找小商家
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        public List<string> FindMstore(string group)
        {
            List<string> mNoList = new List<string>();
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();
                    string sqlText = string.Format(@"select a.MERCHANT_NO from GM_MERCHANT_TYPE_D a 
									INNER JOIN  GM_MERCHANT b ON a.MERCHANT_NO=b.MERCHANT_NO 
									WHERE a.GROUP_ID ='MSTORE' AND a.SHOW_FLG='Y'
									ORDER BY MERCHANT_NO", group);

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    SqlDataReader dr = sqlCmd.ExecuteReader();
                    while (dr.Read())
                    {
                        string mNo = dr["MERCHANT_NO"].ToString();

                        mNoList.Add(mNo);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Debug("Query Ex: GM_MERCHANT_TYPE_D");////
                log.Debug(ex.StackTrace);
                log.Debug(ex.Message);
                mNoList = null;
            }
            return mNoList;
        }
        /// <summary>
        /// 只找NCCC
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        public List<string> FindNCCC(string group)
        {
            List<string> mNoList = new List<string>();
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();
                    string sqlText = string.Format(@"SELECT a.MERCHANT_NO FROM GM_MERCHANT_TYPE_D a 
									INNER JOIN  GM_MERCHANT b ON a.MERCHANT_NO=b.MERCHANT_NO 
									WHERE a.GROUP_ID='BANK_OUTSOURCING' AND a.SHOW_FLG='Y' AND b.MERC_GROUP = 'NCCC' 
                                    AND a.MERCHANT_NO IN 
                                    (SELECT MERCHANT_NO FROM GM_REM_FEE_M WHERE CONTRACT_TYPE = 'P1' AND REM_TYPE = 'D')
									ORDER BY a.MERCHANT_NO", group);

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    SqlDataReader dr = sqlCmd.ExecuteReader();
                    while (dr.Read())
                    {
                        string mNo = dr["MERCHANT_NO"].ToString();

                        mNoList.Add(mNo);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Debug("Query Ex: GM_MERCHANT_TYPE_D");////
                log.Debug(ex.StackTrace);
                log.Debug(ex.Message);
                mNoList = null;
            }
            return mNoList;
        }
        /// <summary>
        /// 只找KSH高雄市區公車
        /// </summary>
        /// <returns></returns>
        public List<string> FindKSH()
        {
            List<string> mNoList = new List<string>();
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();
                    string sqlText = string.Format(@"SELECT a.MERCHANT_NO FROM GM_MERCHANT_TYPE_D a 
									INNER JOIN  GM_MERCHANT b ON a.MERCHANT_NO=b.MERCHANT_NO 
									WHERE a.SHOW_FLG='Y' AND b.MERC_GROUP = 'KSH' 
									ORDER BY a.SHOW_ORDER");

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    SqlDataReader dr = sqlCmd.ExecuteReader();
                    while (dr.Read())
                    {
                        string mNo = dr["MERCHANT_NO"].ToString();

                        mNoList.Add(mNo);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Debug("Query Ex: GM_MERCHANT_TYPE_D");////
                log.Debug(ex.StackTrace);
                log.Debug(ex.Message);
                mNoList = null;
            }
            return mNoList;
        }
        /// <summary>
        /// 只找停車場
        /// </summary>
        /// <returns></returns>
        public List<string> FindParking()
        {
            List<string> mNoList = new List<string>();
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();
                    string sqlText = string.Format(@"SELECT a.MERCHANT_NO FROM GM_MERCHANT_TYPE_D a 
									INNER JOIN  GM_MERCHANT b ON a.MERCHANT_NO=b.MERCHANT_NO 
									WHERE a.SHOW_FLG='Y' AND b.MERC_GROUP = 'PARKING_LOT' 
									ORDER BY a.SHOW_ORDER");

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    SqlDataReader dr = sqlCmd.ExecuteReader();
                    while (dr.Read())
                    {
                        string mNo = dr["MERCHANT_NO"].ToString();

                        mNoList.Add(mNo);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Debug("Query Ex: GM_MERCHANT_TYPE_D");////
                log.Debug(ex.StackTrace);
                log.Debug(ex.Message);
                mNoList = null;
            }
            return mNoList;
        }
        /// <summary>
        /// 只找零售停車場
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        public List<string> FindRetailParking()
        {
            List<string> mNoList = new List<string>();
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();
                    string sqlText = string.Format(@"Select a.MERCHANT_NO from GM_MERCHANT_TYPE_D a 
									INNER JOIN  GM_MERCHANT b ON a.MERCHANT_NO=b.MERCHANT_NO 
									WHERE a.GROUP_ID in('RETAIL','PARKING_LOT') AND a.SHOW_FLG='Y'
									ORDER BY MERCHANT_NO");

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    SqlDataReader dr = sqlCmd.ExecuteReader();
                    while (dr.Read())
                    {
                        string mNo = dr["MERCHANT_NO"].ToString();

                        mNoList.Add(mNo);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Debug("Query Ex: GM_MERCHANT_TYPE_D");////
                log.Debug(ex.StackTrace);
                log.Debug(ex.Message);
                mNoList = null;
            }
            return mNoList;
        }

        /// <summary>
        /// 找委外類型群組所有特約機構
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        public List<string> FindMutiMercMerchant(string group)
        {
            List<string> mNoList = new List<string>();
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();
                    string sqlText = string.Format(@"SELECT M.MERCHANT_NO
						FROM GM_MERCHANT_TYPE_D TD
						JOIN GM_MERCHANT M 
						ON TD.MERCHANT_NO = M.MERCHANT_NO
						JOIN GM_OUTSOURING_GROUP OG
						ON M.MERC_GROUP = OG.GROUP_ID
						WHERE OG.MERCHANT_NO = '{0}'
						AND SHOW_FLG = 'Y'
                        AND TD.MERCHANT_NO IN (SELECT MERCHANT_NO FROM GM_MERCHANT_ACT WHERE REM_TYPE IN ('A004','C001')  AND SETTLE_RULE = 'D')
						ORDER BY M.MERCHANT_STNAME", group);

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    SqlDataReader dr = sqlCmd.ExecuteReader();
                    while (dr.Read())
                    {
                        string mNo = dr["MERCHANT_NO"].ToString();

                        mNoList.Add(mNo);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Debug("Query Ex: GM_MERCHANT_TYPE_D");////
                log.Debug(ex.StackTrace);
                log.Debug(ex.Message);
                mNoList = null;
            }
            return mNoList;
        }


    }

}
