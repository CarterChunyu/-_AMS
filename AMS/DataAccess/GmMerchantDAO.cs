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
    public class GmMerchantDAO : BasicDAL
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(GmMerchantDAO));
        public string TableName { private get; set; }

        public GmMerchantDAO()
        {
            this.TableName = "AM_MERCHANT";
        }

        public List<GmMerchant> FindAll()
        {
            List<GmMerchant> objList = new List<GmMerchant>();
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();
                    string sqlText = @"select MERCHANT_NO,MERCHANT_STNAME as MERCHANT_NAME
                                        from GM_MERCHANT
                                        where MERCHANT_TYPE<>'BANK'  
                                        order by MERCHANT_TYPE,MERC_GROUP desc,MERCHANT_NO";

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    SqlDataReader dr = sqlCmd.ExecuteReader();
                    while (dr.Read())
                    {
                        GmMerchant obj = new GmMerchant();
                        obj.MerchantNo = dr["MERCHANT_NO"].ToString();
                        obj.MerchantName = dr["MERCHANT_NAME"].ToString();
                        objList.Add(obj);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Debug("Query Ex: " + this.TableName);////
                log.Debug(ex.StackTrace);
                log.Debug(ex.Message);
                objList = null;
            }
            return objList;
        }

        public List<GmMerchant> FindAllGroup()
        {
            List<GmMerchant> objList = new List<GmMerchant>();
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();
                    string sqlText = @"select GROUP_ID AS MERCHANT_NO, GROUP_NAME as MERCHANT_NAME
                                        from GM_MERCHANT_TYPE_M ORDER BY SHOW_ORDER
                                        ";

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    SqlDataReader dr = sqlCmd.ExecuteReader();
                    while (dr.Read())
                    {
                        GmMerchant obj = new GmMerchant();
                        obj.MerchantNo = dr["MERCHANT_NO"].ToString();
                        obj.MerchantName = dr["MERCHANT_NAME"].ToString();
                        objList.Add(obj);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Debug("Query Ex: " + this.TableName);////
                log.Debug(ex.StackTrace);
                log.Debug(ex.Message);
                objList = null;
            }
            return objList;
        }

        public List<GmMerchant> FindAllRetail()
        {
            List<GmMerchant> objList = new List<GmMerchant>();
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();
                    string sqlText = @"select a.MERCHANT_NO, b.MERCHANT_STNAME as MERCHANT_NAME
                                        from GM_MERCHANT_TYPE_D a INNER JOIN  GM_MERCHANT b ON a.MERCHANT_NO=b.MERCHANT_NO WHERE a.GROUP_ID='RETAIL' AND a.SHOW_FLG='Y' ORDER BY a.SHOW_ORDER";

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    SqlDataReader dr = sqlCmd.ExecuteReader();
                    while (dr.Read())
                    {
                        GmMerchant obj = new GmMerchant();
                        obj.MerchantNo = dr["MERCHANT_NO"].ToString();
                        obj.MerchantName = dr["MERCHANT_NAME"].ToString();
                        objList.Add(obj);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Debug("Query Ex: " + this.TableName);////
                log.Debug(ex.StackTrace);
                log.Debug(ex.Message);
                objList = null;
            }
            return objList;
        }

        public List<GmMerchant> FindAllBus()
        {
            List<GmMerchant> objList = new List<GmMerchant>();
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();
                    string sqlText = @"select a.MERCHANT_NO, b.MERCHANT_STNAME as MERCHANT_NAME
                                        from GM_MERCHANT_TYPE_D a INNER JOIN  GM_MERCHANT b ON a.MERCHANT_NO=b.MERCHANT_NO WHERE a.GROUP_ID='BUS' AND a.SHOW_FLG='Y' ORDER BY a.SHOW_ORDER";

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    SqlDataReader dr = sqlCmd.ExecuteReader();
                    while (dr.Read())
                    {
                        GmMerchant obj = new GmMerchant();
                        obj.MerchantNo = dr["MERCHANT_NO"].ToString();
                        obj.MerchantName = dr["MERCHANT_NAME"].ToString();
                        objList.Add(obj);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Debug("Query Ex: " + this.TableName);////
                log.Debug(ex.StackTrace);
                log.Debug(ex.Message);
                objList = null;
            }
            return objList;
        }

        public List<GmMerchant> FindAllBike()
        {
            List<GmMerchant> objList = new List<GmMerchant>();
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();
                    string sqlText = @"select a.MERCHANT_NO, b.MERCHANT_STNAME as MERCHANT_NAME
                                        from GM_MERCHANT_TYPE_D a INNER JOIN  GM_MERCHANT b ON a.MERCHANT_NO=b.MERCHANT_NO WHERE a.GROUP_ID='BIKE' AND a.SHOW_FLG='Y' ORDER BY a.SHOW_ORDER";

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    SqlDataReader dr = sqlCmd.ExecuteReader();
                    while (dr.Read())
                    {
                        GmMerchant obj = new GmMerchant();
                        obj.MerchantNo = dr["MERCHANT_NO"].ToString();
                        obj.MerchantName = dr["MERCHANT_NAME"].ToString();
                        objList.Add(obj);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Debug("Query Ex: " + this.TableName);////
                log.Debug(ex.StackTrace);
                log.Debug(ex.Message);
                objList = null;
            }
            return objList;
        }

        public List<GmMerchant> FindAllTrack()
        {
            List<GmMerchant> objList = new List<GmMerchant>();
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();
                    string sqlText = @"select a.MERCHANT_NO, b.MERCHANT_STNAME as MERCHANT_NAME
                                        from GM_MERCHANT_TYPE_D a INNER JOIN  GM_MERCHANT b ON a.MERCHANT_NO=b.MERCHANT_NO WHERE a.GROUP_ID='TRACK' AND a.SHOW_FLG='Y' ORDER BY a.SHOW_ORDER";

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    SqlDataReader dr = sqlCmd.ExecuteReader();
                    while (dr.Read())
                    {
                        GmMerchant obj = new GmMerchant();
                        obj.MerchantNo = dr["MERCHANT_NO"].ToString();
                        obj.MerchantName = dr["MERCHANT_NAME"].ToString();
                        objList.Add(obj);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Debug("Query Ex: " + this.TableName);////
                log.Debug(ex.StackTrace);
                log.Debug(ex.Message);
                objList = null;
            }
            return objList;
        }

        public List<GmMerchant> FindAllParking()
        {
            List<GmMerchant> objList = new List<GmMerchant>();
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();
                    string sqlText = @"select a.MERCHANT_NO, b.MERCHANT_STNAME as MERCHANT_NAME
                                        from GM_MERCHANT_TYPE_D a INNER JOIN  GM_MERCHANT b ON a.MERCHANT_NO=b.MERCHANT_NO WHERE a.GROUP_ID='PARKING_LOT' AND a.SHOW_FLG='Y' ORDER BY a.SHOW_ORDER";

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    SqlDataReader dr = sqlCmd.ExecuteReader();
                    while (dr.Read())
                    {
                        GmMerchant obj = new GmMerchant();
                        obj.MerchantNo = dr["MERCHANT_NO"].ToString();
                        obj.MerchantName = dr["MERCHANT_NAME"].ToString();
                        objList.Add(obj);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Debug("Query Ex: " + this.TableName);////
                log.Debug(ex.StackTrace);
                log.Debug(ex.Message);
                objList = null;
            }
            return objList;
        }

        public List<GmMerchant> FindAllOutsourcing()
        {
            List<GmMerchant> objList = new List<GmMerchant>();
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();
                    string sqlText = @"select a.MERCHANT_NO, b.MERCHANT_STNAME as MERCHANT_NAME
                                        from GM_MERCHANT_TYPE_D a INNER JOIN  GM_MERCHANT b ON a.MERCHANT_NO=b.MERCHANT_NO WHERE a.GROUP_ID='BANK_OUTSOURCING' AND a.SHOW_FLG='Y' ORDER BY a.SHOW_ORDER";

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    SqlDataReader dr = sqlCmd.ExecuteReader();
                    while (dr.Read())
                    {
                        GmMerchant obj = new GmMerchant();
                        obj.MerchantNo = dr["MERCHANT_NO"].ToString();
                        obj.MerchantName = dr["MERCHANT_NAME"].ToString();
                        objList.Add(obj);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Debug("Query Ex: " + this.TableName);////
                log.Debug(ex.StackTrace);
                log.Debug(ex.Message);
                objList = null;
            }
            return objList;
        }

        public List<GmMerchant> FindMerchant(string group_id)
        {
            List<GmMerchant> objList = new List<GmMerchant>();
            List<SqlParameter> listPara = new List<SqlParameter>();
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();
                    string sqlText = @"
SELECT M.MERCHANT_NO, M.MERCHANT_STNAME
  FROM GM_MERCHANT_TYPE_D TD
 INNER JOIN GM_MERCHANT_TYPE_M TM ON TD.GROUP_ID = TM.GROUP_ID
 INNER JOIN GM_MERCHANT M ON TD.MERCHANT_NO = M.MERCHANT_NO
 WHERE 1 = 1
";

                    if (group_id != "")
                    {
                        sqlText += @"   AND TD.GROUP_ID = @GROUP_ID ";
                        listPara.Add(new SqlParameter("@GROUP_ID", System.Data.SqlDbType.VarChar) { Value = group_id });
                    }

                    sqlText += @" order by M.MERCHANT_STNAME";

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    foreach (SqlParameter para in listPara)
                    { sqlCmd.Parameters.Add(para); }

                    SqlDataReader dr = sqlCmd.ExecuteReader();
                    while (dr.Read())
                    {
                        GmMerchant obj = new GmMerchant();
                        obj.MerchantNo = dr["MERCHANT_NO"].ToString();
                        obj.MerchantName = dr["MERCHANT_STNAME"].ToString();
                        objList.Add(obj);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Debug("Query Ex: " + this.TableName);////
                log.Debug(ex.StackTrace);
                log.Debug(ex.Message);
                objList = null;
            }
            return objList;
        }

        public System.Data.DataRow FindMerchantData(string merchant_no)
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();

                    string sqlText = @"
select *
  from GM_MERCHANT
 where MERCHANT_NO = @MERCHANT_NO
";

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    sqlCmd.Parameters.Add(new SqlParameter("@MERCHANT_NO", System.Data.SqlDbType.VarChar) { Value = merchant_no });
                    SqlDataAdapter adapter = new SqlDataAdapter(sqlCmd);
                    adapter.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                log.Debug(ex.StackTrace);
                log.Debug(ex.Message);
            }

            if (dt.Rows.Count > 0)
            { return dt.Rows[0]; }
            else
            { return null; }
        }


        public List<GmMerchant> FindAllBank()
        {
            List<GmMerchant> objList = new List<GmMerchant>();
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();
                    string sqlText = @"select MERCHANT_NO,MERCHANT_STNAME as MERCHANT_NAME
                                        from GM_MERCHANT
                                        where MERCHANT_TYPE='BANK'  
                                        order by MERCHANT_TYPE,MERC_GROUP desc,MERCHANT_NO";

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    SqlDataReader dr = sqlCmd.ExecuteReader();
                    while (dr.Read())
                    {
                        GmMerchant obj = new GmMerchant();
                        obj.MerchantNo = dr["MERCHANT_NO"].ToString();
                        obj.MerchantName = dr["MERCHANT_NAME"].ToString();
                        objList.Add(obj);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Debug("Query Ex: " + this.TableName);////
                log.Debug(ex.StackTrace);
                log.Debug(ex.Message);
                objList = null;
            }
            return objList;
        }

        public List<GmMerchant> FindAllRAMT()
        {
            List<GmMerchant> objList = new List<GmMerchant>();
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();
                    string sqlText = @"select RAMT_TYPE as MERCHANT_NO ,RAMT_TYPE+':'+RAMT_NAME as MERCHANT_NAME
                                        from CR_RAMT_TYPE_MST
                                        order by RAMT_TYPE";

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    SqlDataReader dr = sqlCmd.ExecuteReader();
                    while (dr.Read())
                    {
                        GmMerchant obj = new GmMerchant();
                        obj.MerchantNo = dr["MERCHANT_NO"].ToString();
                        obj.MerchantName = dr["MERCHANT_NAME"].ToString();
                        objList.Add(obj);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Debug("Query Ex: " + this.TableName);////
                log.Debug(ex.StackTrace);
                log.Debug(ex.Message);
                objList = null;
            }
            return objList;
        }

        public List<GmMerchant> FindAllStore()
        {
            List<GmMerchant> objList = new List<GmMerchant>();
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();
                    string sqlText = @"select STORE_NO as MERCHANT_NO ,STO_NAME_SHORT as MERCHANT_NAME
                                        from mpg.BM_STORE_M
                                        order by STORE_NO";

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    SqlDataReader dr = sqlCmd.ExecuteReader();
                    while (dr.Read())
                    {
                        GmMerchant obj = new GmMerchant();
                        obj.MerchantNo = dr["MERCHANT_NO"].ToString();
                        obj.MerchantName = dr["MERCHANT_NAME"].ToString();
                        objList.Add(obj);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Debug("Query Ex: " + this.TableName);////
                log.Debug(ex.StackTrace);
                log.Debug(ex.Message);
                objList = null;
            }
            return objList;
        }

        public List<GmMerchant> FindAllMerchantNoCom()
        {
            List<GmMerchant> objList = new List<GmMerchant>();
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();
                    string sqlText = @"SELECT MERCHANT_NO_COM AS MERCHANT_NO,COMPANY_NAME AS MERCHANT_NAME FROM IBON_COMPANY_LOADING_M ORDER BY MERCHANT_NO";

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    SqlDataReader dr = sqlCmd.ExecuteReader();
                    while (dr.Read())
                    {
                        GmMerchant obj = new GmMerchant();
                        obj.MerchantNo = dr["MERCHANT_NO"].ToString();
                        obj.MerchantName = dr["MERCHANT_NAME"].ToString();
                        objList.Add(obj);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Debug("Query Ex: " + this.TableName);////
                log.Debug(ex.StackTrace);
                log.Debug(ex.Message);
                objList = null;
            }
            return objList;
        }

        public List<GmMerchant> FindAllMerchantADJ()
        {
            List<GmMerchant> objList = new List<GmMerchant>();
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();
                    string sqlText = @"SELECT MERCHANT_NO,MERCHANT_STNAME AS MERCHANT_NAME FROM GM_MERCHANT WHERE SAM_TYPE IN ('TR','BS') ORDER BY MERCHANT_NO";

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    SqlDataReader dr = sqlCmd.ExecuteReader();
                    while (dr.Read())
                    {
                        GmMerchant obj = new GmMerchant();
                        obj.MerchantNo = dr["MERCHANT_NO"].ToString();
                        obj.MerchantName = dr["MERCHANT_NAME"].ToString();
                        objList.Add(obj);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Debug("Query Ex: " + this.TableName);////
                log.Debug(ex.StackTrace);
                log.Debug(ex.Message);
                objList = null;
            }
            return objList;
        }



        public List<GmMerchant> FindAllSET()
        {
            List<GmMerchant> objList = new List<GmMerchant>();
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();
                    string sqlText = @"SELECT MERCHANT_NO,MERCHANT_STNAME AS MERCHANT_NAME FROM GM_MERCHANT WHERE MERC_GROUP='SET' ORDER BY MERCHANT_NO";

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    SqlDataReader dr = sqlCmd.ExecuteReader();
                    while (dr.Read())
                    {
                        GmMerchant obj = new GmMerchant();
                        obj.MerchantNo = dr["MERCHANT_NO"].ToString();
                        obj.MerchantName = dr["MERCHANT_NAME"].ToString();
                        objList.Add(obj);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Debug("Query Ex: " + this.TableName);////
                log.Debug(ex.StackTrace);
                log.Debug(ex.Message);
                objList = null;
            }
            return objList;
        }

        public List<GmMerchant> FindAllNonSET()
        {
            List<GmMerchant> objList = new List<GmMerchant>();
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();
                    string sqlText = @"SELECT a.MERCHANT_NO, b.MERCHANT_STNAME AS MERCHANT_NAME
                                        FROM GM_MERCHANT_TYPE_D a INNER JOIN  GM_MERCHANT b 
                                        ON a.MERCHANT_NO=b.MERCHANT_NO 
                                        WHERE a.SHOW_FLG='Y' and MERC_GROUP <> 'SET'
                                        AND a.GROUP_ID <> 'MSTORE'
                                        ORDER BY a.GROUP_ID DESC,a.SHOW_ORDER ";

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    SqlDataReader dr = sqlCmd.ExecuteReader();
                    while (dr.Read())
                    {
                        GmMerchant obj = new GmMerchant();
                        obj.MerchantNo = dr["MERCHANT_NO"].ToString();
                        obj.MerchantName = dr["MERCHANT_NAME"].ToString();
                        objList.Add(obj);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Debug("Query Ex: " + this.TableName);////
                log.Debug(ex.StackTrace);
                log.Debug(ex.Message);
                objList = null;
            }
            return objList;
        }
        /// <summary>
        /// 零售不包含小商家
        /// </summary>
        /// <returns></returns>
        public List<GmMerchant> FindRetailNotXDD()
        {
            List<GmMerchant> objList = new List<GmMerchant>();
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();
                    string sqlText = @"select a.MERCHANT_NO, b.MERCHANT_STNAME as MERCHANT_NAME
                                        from GM_MERCHANT_TYPE_D a INNER JOIN  GM_MERCHANT b ON a.MERCHANT_NO=b.MERCHANT_NO 
                                        WHERE a.GROUP_ID <> 'MSTORE'  a.SHOW_FLG='Y'  ORDER BY a.SHOW_ORDER";

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    SqlDataReader dr = sqlCmd.ExecuteReader();
                    while (dr.Read())
                    {
                        GmMerchant obj = new GmMerchant();
                        obj.MerchantNo = dr["MERCHANT_NO"].ToString();
                        obj.MerchantName = dr["MERCHANT_NAME"].ToString();
                        objList.Add(obj);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Debug("Query Ex: " + this.TableName);////
                log.Debug(ex.StackTrace);
                log.Debug(ex.Message);
                objList = null;
            }
            return objList;
        }
        /// <summary>
        /// 只找小商家
        /// </summary>
        /// <returns></returns>
        public List<GmMerchant> FindOnlyXDD()
        {
            List<GmMerchant> objList = new List<GmMerchant>();
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();
                    string sqlText = @"select a.MERCHANT_NO, b.MERCHANT_STNAME as MERCHANT_NAME
                                        from GM_MERCHANT_TYPE_D a INNER JOIN  GM_MERCHANT b ON a.MERCHANT_NO=b.MERCHANT_NO 
                                        WHERE a.GROUP_ID='MSTORE' AND a.SHOW_FLG='Y'  ORDER BY a.SHOW_ORDER";

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    SqlDataReader dr = sqlCmd.ExecuteReader();
                    while (dr.Read())
                    {
                        GmMerchant obj = new GmMerchant();
                        obj.MerchantNo = dr["MERCHANT_NO"].ToString();
                        obj.MerchantName = dr["MERCHANT_NAME"].ToString();
                        objList.Add(obj);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Debug("Query Ex: " + this.TableName);////
                log.Debug(ex.StackTrace);
                log.Debug(ex.Message);
                objList = null;
            }
            return objList;
        }
        /// <summary>
        /// 找不含小商家的所有群組
        /// </summary>
        /// <returns></returns>
        public List<GmMerchant> FindGroupNoMstore()
        {
            List<GmMerchant> objList = new List<GmMerchant>();
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();
                    string sqlText = @"select GROUP_ID AS MERCHANT_NO, GROUP_NAME as MERCHANT_NAME
                                        from GM_MERCHANT_TYPE_M 
										where GROUP_ID <> 'MSTORE' ORDER BY SHOW_ORDER
                                        ";

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    SqlDataReader dr = sqlCmd.ExecuteReader();
                    while (dr.Read())
                    {
                        GmMerchant obj = new GmMerchant();
                        obj.MerchantNo = dr["MERCHANT_NO"].ToString();
                        obj.MerchantName = dr["MERCHANT_NAME"].ToString();
                        objList.Add(obj);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Debug("Query Ex: " + this.TableName);////
                log.Debug(ex.StackTrace);
                log.Debug(ex.Message);
                objList = null;
            }
            return objList;
        }
        //找群組所有特約機構by選單
        public List<GmMerchant> FindAllbyGroup(string group)
        {
            List<GmMerchant> objList = new List<GmMerchant>();
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();
                    string sqlText = String.Format(@"select a.MERCHANT_NO, b.MERCHANT_STNAME as MERCHANT_NAME
                                        from GM_MERCHANT_TYPE_D a INNER JOIN  GM_MERCHANT b 
					                    ON a.MERCHANT_NO=b.MERCHANT_NO 
					                    WHERE a.GROUP_ID='{0}' AND a.SHOW_FLG='Y' 
					                    ORDER BY a.SHOW_ORDER", group);

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    SqlDataReader dr = sqlCmd.ExecuteReader();
                    while (dr.Read())
                    {
                        GmMerchant obj = new GmMerchant();
                        obj.MerchantNo = dr["MERCHANT_NO"].ToString();
                        obj.MerchantName = dr["MERCHANT_NAME"].ToString();
                        objList.Add(obj);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Debug("Query Ex: " + this.TableName);////
                log.Debug(ex.StackTrace);
                log.Debug(ex.Message);
                objList = null;
            }
            return objList;
        }
        //找所有NCCC的特店
        public List<GmMerchant> FindNCCC()
        {
            List<GmMerchant> objList = new List<GmMerchant>();
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();
                    string sqlText = @"SELECT a.MERCHANT_NO, b.MERCHANT_STNAME as MERCHANT_NAME
									FROM GM_MERCHANT_TYPE_D a INNER JOIN  GM_MERCHANT b ON a.MERCHANT_NO=b.MERCHANT_NO 
									WHERE a.GROUP_ID='BANK_OUTSOURCING' AND a.SHOW_FLG='Y' AND b.MERC_GROUP = 'NCCC' 
                                    AND a.MERCHANT_NO IN 
                                    (SELECT MERCHANT_NO FROM GM_REM_FEE_M WHERE CONTRACT_TYPE = 'P1' AND REM_TYPE = 'D')
									ORDER BY a.SHOW_ORDER";

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    SqlDataReader dr = sqlCmd.ExecuteReader();
                    while (dr.Read())
                    {
                        GmMerchant obj = new GmMerchant();
                        obj.MerchantNo = dr["MERCHANT_NO"].ToString();
                        obj.MerchantName = dr["MERCHANT_NAME"].ToString();
                        objList.Add(obj);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Debug("Query Ex: " + this.TableName);////
                log.Debug(ex.StackTrace);
                log.Debug(ex.Message);
                objList = null;
            }
            return objList;
        }
        //找高雄地區客運
        public List<GmMerchant> FindKSH()
        {
            List<GmMerchant> objList = new List<GmMerchant>();
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();
                    string sqlText = @"SELECT a.MERCHANT_NO, b.MERCHANT_STNAME as MERCHANT_NAME
									FROM GM_MERCHANT_TYPE_D a INNER JOIN  GM_MERCHANT b ON a.MERCHANT_NO=b.MERCHANT_NO 
									WHERE a.SHOW_FLG='Y' AND b.MERC_GROUP = 'KSH' 
									ORDER BY a.SHOW_ORDER";

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    SqlDataReader dr = sqlCmd.ExecuteReader();
                    while (dr.Read())
                    {
                        GmMerchant obj = new GmMerchant();
                        obj.MerchantNo = dr["MERCHANT_NO"].ToString();
                        obj.MerchantName = dr["MERCHANT_NAME"].ToString();
                        objList.Add(obj);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Debug("Query Ex: " + this.TableName);////
                log.Debug(ex.StackTrace);
                log.Debug(ex.Message);
                objList = null;
            }
            return objList;
        }
        /// <summary>
        /// 只找停車場
        /// </summary>
        /// <returns></returns>
        public List<GmMerchant> FindParking()
        {
            List<GmMerchant> objList = new List<GmMerchant>();
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();
                    string sqlText = @"SELECT a.MERCHANT_NO, b.MERCHANT_STNAME as MERCHANT_NAME
									FROM GM_MERCHANT_TYPE_D a INNER JOIN  GM_MERCHANT b ON a.MERCHANT_NO=b.MERCHANT_NO 
									WHERE a.SHOW_FLG='Y' AND a.GROUP_ID = 'PARKING_LOT' 
									ORDER BY a.SHOW_ORDER";

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    SqlDataReader dr = sqlCmd.ExecuteReader();
                    while (dr.Read())
                    {
                        GmMerchant obj = new GmMerchant();
                        obj.MerchantNo = dr["MERCHANT_NO"].ToString();
                        obj.MerchantName = dr["MERCHANT_NAME"].ToString();
                        objList.Add(obj);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Debug("Query Ex: " + this.TableName);////
                log.Debug(ex.StackTrace);
                log.Debug(ex.Message);
                objList = null;
            }
            return objList;
        }
        /// <summary>
        /// 只找停車場和零售
        /// </summary>
        /// <returns></returns>
        public List<GmMerchant> FindGroupRetailParking()
        {
            List<GmMerchant> objList = new List<GmMerchant>();
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();
                    string sqlText = @"SELECT GROUP_ID AS MERCHANT_NO, GROUP_NAME as MERCHANT_NAME
                                       FROM GM_MERCHANT_TYPE_M 
									   WHERE GROUP_ID in ('PARKING_LOT','RETAIL') ORDER BY SHOW_ORDER
                                        ";

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    SqlDataReader dr = sqlCmd.ExecuteReader();
                    while (dr.Read())
                    {
                        GmMerchant obj = new GmMerchant();
                        obj.MerchantNo = dr["MERCHANT_NO"].ToString();
                        obj.MerchantName = dr["MERCHANT_NAME"].ToString();
                        objList.Add(obj);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Debug("Query Ex: " + this.TableName);////
                log.Debug(ex.StackTrace);
                log.Debug(ex.Message);
                objList = null;
            }
            return objList;
        }

        /// <summary>
        /// 取得特約機構DB Server名稱
        /// </summary>
        /// <returns></returns>
        public string FindMerchantServerName(string merchant_no)
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();

                    string sqlText = @"
select SERVER_NAME
  from dbo.GM_COMMON_M
 where MERCHANT_NO = @MERCHANT_NO
   and TYPE_GROUP = 'BATCH_TABLE'
   and @SYSDATE BETWEEN EFF_DATE_FROM and EFF_DATE_TO
";

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    sqlCmd.Parameters.Add(new SqlParameter("@MERCHANT_NO", System.Data.SqlDbType.VarChar) { Value = merchant_no });
                    sqlCmd.Parameters.Add(new SqlParameter("@SYSDATE", System.Data.SqlDbType.VarChar) { Value = DateTime.Today.ToString("yyyyMMdd") });
                    SqlDataAdapter adapter = new SqlDataAdapter(sqlCmd);
                    adapter.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                log.Debug(ex.StackTrace);
                log.Debug(ex.Message);
            }

            if (dt.Rows.Count > 0)
            { return "" + dt.Rows[0]["SERVER_NAME"]; }
            else
            { return null; }
        }

        /// <summary>
        /// 委外收單類型群組，GM_OUTSOURING_GROUP手動INSERT
        /// </summary>
        /// <returns></returns>
        public List<GmMerchant> FindMutiGroup()
        {
            List<GmMerchant> objList = new List<GmMerchant>();
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();
                    string sqlText = @"
					SELECT MERCHANT_NO ,GROUP_NAME FROM GM_OUTSOURING_GROUP  
					";

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    SqlDataReader dr = sqlCmd.ExecuteReader();
                    while (dr.Read())
                    {
                        GmMerchant obj = new GmMerchant();
                        obj.MerchantNo = dr["MERCHANT_NO"].ToString();
                        obj.MerchantName = dr["GROUP_NAME"].ToString();
                        objList.Add(obj);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Debug("Query Ex: " + this.TableName);////
                log.Debug(ex.StackTrace);
                log.Debug(ex.Message);
                objList = null;
            }
            return objList;
        }

        /// <summary>
        /// 找委外收單類型的該群組所有特約機構
        /// </summary>
        /// <param name="group_id">群組ID</param>
        /// <returns></returns>
        public List<GmMerchant> FindMutiMerchant(string group)
        {
            List<GmMerchant> objList = new List<GmMerchant>();
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();
                    string sqlText = String.Format(@"
						SELECT M.MERCHANT_NO, M.MERCHANT_STNAME AS MERCHANT_NAME
						FROM GM_MERCHANT_TYPE_D TD
						JOIN GM_MERCHANT M 
						ON TD.MERCHANT_NO = M.MERCHANT_NO
						JOIN GM_OUTSOURING_GROUP OG
						ON M.MERC_GROUP = OG.GROUP_ID
						WHERE OG.MERCHANT_NO = '{0}'
						AND SHOW_FLG = 'Y'
                        AND TD.MERCHANT_NO IN (SELECT MERCHANT_NO FROM GM_MERCHANT_ACT WHERE REM_TYPE IN ('A004','C001') AND SETTLE_RULE = 'D')
						ORDER BY M.MERCHANT_STNAME
					", group);

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    SqlDataReader dr = sqlCmd.ExecuteReader();
                    while (dr.Read())
                    {
                        GmMerchant obj = new GmMerchant();
                        obj.MerchantNo = dr["MERCHANT_NO"].ToString();
                        obj.MerchantName = dr["MERCHANT_NAME"].ToString();
                        objList.Add(obj);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Debug("Query Ex: " + this.TableName);////
                log.Debug(ex.StackTrace); 
                log.Debug(ex.Message);
                objList = null;
            }
            return objList;
        }
    }
}
