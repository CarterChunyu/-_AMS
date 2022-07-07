using Domain;
using log4net;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace DataAccess
{
    public class GmMerchantActStoreDAO : BasicDAL
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(GmMerchantActStoreDAO));
        public string TableName { private get; set; }

        //特店選單
        public List<GmMerchant> FindAllGroup_Store()
        {
            List<GmMerchant> objList = new List<GmMerchant>();
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();
                    string sqlText = @"select a.MERCHANT_NO,b.MERCHANT_NAME from gm_merchant_act a
                                        inner join gm_merchant b on a.merchant_no=b.merchant_no
                                        where a.pay_type='02'
                                        order by a.MERCHANT_NO
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
        //特店_門市選單
        public List<BmStoreno> FindAllBMStore(string merchant)
        {
            List<BmStoreno> storeList = new List<BmStoreno>();
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();

                    string sqlText = @"
                    DECLARE @SCHEMAID VARCHAR(10)

                    SELECT @SCHEMAID=MERCH_SCHEMAID FROM GM_MERCHANT WHERE MERCHANT_NO=@EXEC_MERCHANT_NO;

                    EXEC('select STORE_NO,STO_NAME_LONG from '+@SCHEMAID+'.BM_STORE_M');";

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    sqlCmd.Parameters.Add("@EXEC_MERCHANT_NO", SqlDbType.VarChar).Value = merchant;
                    SqlDataReader dr = sqlCmd.ExecuteReader();
                    while (dr.Read())
                    {
                        BmStoreno store = new BmStoreno();
                        store.StoreNo = dr["STORE_NO"].ToString();
                        store.StoreName = dr["STO_NAME_LONG"].ToString();
                        storeList.Add(store);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Debug("Query Ex: " + this.TableName);////
                log.Debug(ex.StackTrace);
                log.Debug(ex.Message);
                storeList = null;
            }
            return storeList;
        }
        //查詢後選單
        public DataTable GMACT_STORE_INDEX(string merchant, string storeno)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();
                    string sqlText = @"
                    DECLARE @SCHEMAID VARCHAR(10)

                    SELECT @SCHEMAID=MERCH_SCHEMAID FROM GM_MERCHANT WHERE MERCHANT_NO=@EXEC_MERCHANT_NO;

                    EXEC('select a.MERCHANT_NO,a.STORE_NO,a.STO_NAME_LONG,b.MERCHANT_NO_ACT,b.Merchant_Note,cls.class_name,p.pay_name,m.set_group_name
                    from '+@SCHEMAID+'.BM_STORE_M a
                    left join gm_merchant_act_store b on a.MERCHANT_NO=b.MERCHANT_NO and a.STORE_NO=b.STORE_NO
                    left join gm_merchant_act act on a.MERCHANT_NO=act.MERCHANT_NO
                    left join GM_MERCHANT_ACT_MAPPING m ON act.set_group = m.SET_GROUP 
                    left join GM_MERCHANT_ACT_CLASS cls on act.class=cls.CLASS_TYPE
                    left join GM_MERCHANT_ACT_PAYTYPE p on act.pay_type=p.pay_type
                    "; 
                    
                    if (storeno != "ALL")
                    { sqlText = String.Format(@"{0} WHERE A.STORE_NO='+@EXEC_STORENO+' ", sqlText); }

                    sqlText = String.Format(@"{0} ');", sqlText);

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    sqlCmd.Parameters.Add("@EXEC_MERCHANT_NO", SqlDbType.VarChar).Value = merchant;
                    sqlCmd.Parameters.Add("@EXEC_STORENO", SqlDbType.VarChar).Value = storeno;

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
        //選擇編輯的單筆資料
        public DataTable GMACT_STORE_EDIT(string merchant, string storeno)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();
                    string sqlText = @"
                    DECLARE @SCHEMAID VARCHAR(10)

                    SELECT @SCHEMAID=MERCH_SCHEMAID FROM GM_MERCHANT WHERE MERCHANT_NO=@EXEC_MERCHANT_NO;

                    EXEC('select a.MERCHANT_NO,mt.MERCHANT_NAME,a.STORE_NO,a.STO_NAME_LONG,b.MERCHANT_NO_ACT,b.Merchant_Note,act.class,cls.CLASS_NAME,act.PAY_TYPE,p.PAY_NAME,act.SET_GROUP,m.SET_GROUP_NAME
                    from '+@SCHEMAID+'.BM_STORE_M a
                    inner join gm_merchant mt on a.merchant_no=mt.merchant_no
                    left join gm_merchant_act_store b on a.MERCHANT_NO=b.MERCHANT_NO and a.STORE_NO=b.STORE_NO
                    left join gm_merchant_act act on a.MERCHANT_NO=act.MERCHANT_NO
                    left join GM_MERCHANT_ACT_MAPPING m ON act.set_group = m.SET_GROUP 
                    left join GM_MERCHANT_ACT_CLASS cls on act.class=cls.CLASS_TYPE
                    left join GM_MERCHANT_ACT_PAYTYPE p on act.pay_type=p.pay_type
                    WHERE A.STORE_NO='+@EXEC_STORE_NO);
                    ";

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    sqlCmd.Parameters.Add("@EXEC_MERCHANT_NO", SqlDbType.VarChar).Value = merchant;
                    sqlCmd.Parameters.Add("@EXEC_STORE_NO", SqlDbType.VarChar).Value = storeno;

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
        public bool CheckMerchantActStore(string merchant, string storeno)
        {
            bool checkData = false;
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();
                    string sqlText = @"SELECT * FROM GM_MERCHANT_ACT_STORE WHERE MERCHANT_NO=@EXEC_MERCHANT_NO AND STORE_NO = @EXEC_STORE_NO ";

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    sqlCmd.Parameters.Add("@EXEC_MERCHANT_NO", SqlDbType.VarChar).Value = merchant;
                    sqlCmd.Parameters.Add("@EXEC_STORE_NO", SqlDbType.VarChar).Value = storeno;
                    var t = sqlCmd.ExecuteScalar();
                    if (t != null)
                    { checkData = true; }
                    
                    sqlConn.Close();

                }
            }

            catch (Exception ex)
            {
                log.Debug(ex.Message);
            }

            return checkData;
        }
        
        public void GMACT_STORE_INSERT(GmAccountActStore entity)
        {                 
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();
                    string sqlText = @"
                    INSERT INTO GM_MERCHANT_ACT_STORE (MERCHANT_NO,STORE_NO,MERCHANT_NO_ACT,MERCHANT_NOTE,CLASS) VALUES
                    (@EXEC_MERCHANT_NO,@EXEC_STORE_NO,@EXEC_MERCHANT_NO_ACT,@EXEC_MERCHANT_NOTE,@EXEC_CLASS);";

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    sqlCmd.Parameters.Add("@EXEC_MERCHANT_NO", SqlDbType.VarChar).Value = entity.MERCHANT_NO.ToString();
                    sqlCmd.Parameters.Add("@EXEC_STORE_NO", SqlDbType.VarChar).Value = entity.STORE_NO.ToString();
                    sqlCmd.Parameters.Add("@EXEC_MERCHANT_NO_ACT", SqlDbType.VarChar).Value = entity.MERCHANT_NO_ACT.ToString();
                    sqlCmd.Parameters.Add("@EXEC_MERCHANT_NOTE", SqlDbType.VarChar).Value = entity.MERCHANT_NOTE.ToString();
                    sqlCmd.Parameters.Add("@EXEC_CLASS", SqlDbType.VarChar).Value = entity.CLASS.ToString();
                    //sqlCmd.Parameters.Add("@EXEC_PAY_TYPE", SqlDbType.VarChar).Value = entity.PAY_TYPE.ToString();
                    //sqlCmd.Parameters.Add("@EXEC_SET_GROUP", SqlDbType.VarChar).Value = entity.SET_GRAOUP.ToString();
                    sqlCmd.ExecuteNonQuery();
                }
            }

            catch (Exception ex)
            {
                log.Debug(ex.Message);
            }
        }
        public void GMACT_STORE_UPDATE(GmAccountActStore entity)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();
                    string sqlText = @"
                                        UPDATE GM_MERCHANT_ACT_STORE
                                        SET MERCHANT_NO_ACT = @EXEC_MERCHANT_NO_ACT ,MERCHANT_NOTE = @EXEC_MERCHANT_NOTE,
                                        CLASS = @EXEC_CLASS 
                                        WHERE MERCHANT_NO = @EXEC_MERCHANT_NO AND STORE_NO = @EXEC_STORE_NO";

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    sqlCmd.Parameters.Add("@EXEC_MERCHANT_NO", SqlDbType.VarChar).Value = entity.MERCHANT_NO.ToString();
                    sqlCmd.Parameters.Add("@EXEC_STORE_NO", SqlDbType.VarChar).Value = entity.STORE_NO.ToString();
                    sqlCmd.Parameters.Add("@EXEC_MERCHANT_NO_ACT", SqlDbType.VarChar).Value = entity.MERCHANT_NO_ACT.ToString();
                    sqlCmd.Parameters.Add("@EXEC_MERCHANT_NOTE", SqlDbType.VarChar).Value = entity.MERCHANT_NOTE.ToString();
                    sqlCmd.Parameters.Add("@EXEC_CLASS", SqlDbType.VarChar).Value = entity.CLASS.ToString();
                    //sqlCmd.Parameters.Add("@EXEC_PAY_TYPE", SqlDbType.VarChar).Value = entity.PAY_TYPE.ToString();
                    //sqlCmd.Parameters.Add("@EXEC_SET_GROUP", SqlDbType.VarChar).Value = entity.SET_GRAOUP.ToString();

                    SqlDataAdapter adapter = new SqlDataAdapter(sqlCmd);
                    adapter.Fill(dt);

                    sqlConn.Close();

                }
            }

            catch (Exception ex)
            {
                log.Debug(ex.Message);
            }
        }
    }
}
