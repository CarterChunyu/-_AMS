using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Domain;
using System.IO;

namespace DataAccess
{
    public class GmAccountDAO : BasicDAL
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(GmAccountDAO));

        GmMerchantDAO GmDAO = null;
        public GmAccountDAO()
        {
            GmDAO = new GmMerchantDAO();
        }
        public DataTable GmAccount190501(string cpt_date, string no1, string no2)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();
                    string sqlText = string.Format(File.ReadAllText(System.AppDomain.CurrentDomain.BaseDirectory + "\\Sql\\GmAccount\\GmAccount190501.sql", Encoding.GetEncoding("big5")));

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    sqlCmd.CommandTimeout = 300;
                    sqlCmd.Parameters.Add("@CPT_DATE", SqlDbType.VarChar).Value = cpt_date;
                    sqlCmd.Parameters.Add("@NO1", SqlDbType.VarChar).Value = no1;
                    sqlCmd.Parameters.Add("@NO2", SqlDbType.VarChar).Value = no2;
                    //sqlCmd.Parameters.Add("@merchant", SqlDbType.VarChar).Value = merchant;

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

        public DataTable GMACT_INDEX(string merchant)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    //"特約機構代碼", "AMS名稱", "門市代號對應值", "摘要欄位", "類別","撥款單位", "關係人",
                    //    "門市代號對應值(手續費)","開立方式(手續費)","關係人(手續費)","特店簡稱","結算週期","委外-周期代碼","結算方式",
                    //    "手續費收款日","來源規則","預設開立(顯示/不顯示)","部門","專案代號","會計科目","單別代號"
                    sqlConn.Open();
                    string sqlText = @"
                                        
                                        SELECT C.MERCHANT_NO, --特約機構代碼
                                        C.MERCHANT_NAME, --AMS名稱
                                        A.MERCHANT_NO_ACT, --門市代號對應值
                                        A.MERCHANT_NOTE, --摘要欄位
                                        D.CLASS_NAME, --類別
                                        E.PAY_NAME, --撥款單位
                                        B.SET_GROUP_NAME, --關係人
                                        ---------------手續費
                                        MERCHANT_NO_ACT_M,  --門市代號對應值(手續費)
                                        I.OPTION_NAME INVOICE_RULE, --開立方式(手續費)
                                        J.OPTION_NAME SET_GROUP_M, --關係人(手續費)
                                        H.GROUP_NAME MERC_GROUP,--特店性質 
                                        CASE WHEN A.MERCHANT_STNAME IS NULL OR A.MERCHANT_STNAME = '' THEN C.MERCHANT_STNAME ELSE A.MERCHANT_STNAME END MERCHANT_STNAME, --特店簡稱
                                        
                                        
                                        CASE WHEN F.SUM_MON_S = '-1'AND F.SUM_DAY_S ='01' THEN '上月1日-上月月底'
                                        WHEN F.SUM_MON_S = '-1' AND F.SUM_DAY_S = '02' THEN '上月2日-次月1日' END AS CONTRACT_PREIOD, --結算週期
                                        M.OPTION_NAME REM_TYPE, --委外-周期代碼
                                        K.OPTION_NAME SETTLE_RULE,
                                        FEE_DAY,
                                        L.OPTION_NAME SOURCE_RULE,
                                        A.SHOW_FLG,
                                        DEPARTMENT,
                                        PROJECT_NO,
                                        CASE WHEN ACCOUNTING IS NULL OR ACCOUNTING = '' THEN '461102' ELSE ACCOUNTING END ACCOUNTING,
                                        N.OPTION_NAME ORDER_NO

                                        FROM GM_MERCHANT_ACT A
                                        LEFT JOIN GM_MERCHANT_ACT_MAPPING B
                                        ON A.SET_GROUP = B.SET_GROUP 
                                        RIGHT JOIN GM_MERCHANT C
                                        ON C.MERCHANT_NO = A.MERCHANT_NO
                                        LEFT JOIN GM_MERCHANT_ACT_CLASS D
                                        ON A.CLASS = D.CLASS_TYPE
                                        LEFT JOIN GM_MERCHANT_ACT_PAYTYPE E
                                        ON A.PAY_TYPE = E.PAY_TYPE
                                        LEFT JOIN GM_CONTRACT_M F
                                        ON C.MERCHANT_NO = F.MERCHANT_NO
                                        LEFT JOIN GM_MERCHANT_TYPE_D G
                                        ON C.MERCHANT_NO = G.MERCHANT_NO
                                        JOIN GM_MERCHANT_TYPE_M H
                                        ON G.GROUP_ID = H.GROUP_ID
                                        LEFT JOIN GM_MERCHANT_ACT_OPTION I
                                        ON A.INVOICE_RULE = I.OPTION_NO AND I.GROUP_ID = 'INVOICE_RULE'
                                        LEFT JOIN GM_MERCHANT_ACT_OPTION J
                                        ON A.SET_GROUP = J.OPTION_NO AND J.GROUP_ID = 'SET_GROUP_M'
                                        LEFT JOIN GM_MERCHANT_ACT_OPTION K
                                        ON A.SETTLE_RULE = K.OPTION_NO AND K.GROUP_ID = 'SETTLE_RULE'
                                        LEFT JOIN GM_MERCHANT_ACT_OPTION L
                                        ON A.SOURCE_RULE = L.OPTION_NO AND L.GROUP_ID = 'SOURCE_RULE'
                                        LEFT JOIN GM_MERCHANT_ACT_OPTION M
                                        ON A.REM_TYPE = M.OPTION_NO AND M.GROUP_ID = 'REM_TYPE'
                                        LEFT JOIN GM_MERCHANT_ACT_OPTION N
                                        ON A.REM_TYPE = N.OPTION_NO AND N.GROUP_ID = 'ORDER_NO'

                                        WHERE C.MERCHANT_TYPE <> 'BANK' 
                                        AND C.MERCHANT_NO = @EXEC_MERCHANT_NO
                                        ORDER BY 3
                                        ";

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    sqlCmd.Parameters.Add("@EXEC_MERCHANT_NO", SqlDbType.VarChar).Value = merchant;
                    //sqlCmd.Parameters.Add("@merchant", SqlDbType.VarChar).Value = merchant;
                    sqlCmd.CommandTimeout = 300;
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
        public DataTable GMACT_EDIT(string merchant)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();
                    string sqlText = @"
                                        
                                        SELECT C.MERCHANT_NO,
		                                C.MERCHANT_NAME,
		                                A.MERCHANT_NO_ACT,
		                                ISNULL(A.MERCHANT_NOTE,'*'+C.MERCHANT_NAME) AS MERCHANT_NOTE,
		                                A.CLASS,
		                                A.PAY_TYPE,
		                                A.SET_GROUP,
		                                -------
		                                MERCHANT_NO_ACT_M,
		                                INVOICE_RULE,
		                                CASE WHEN A.SET_GROUP = 'N001' THEN 'N' WHEN A.SET_GROUP = 'Y001' THEN 'Y' ELSE A.SET_GROUP_M END AS SET_GROUP_M,
		                                H.GROUP_NAME MERC_GROUP,
		                                CASE WHEN A.MERCHANT_STNAME IS NULL OR A.MERCHANT_STNAME = '' THEN C.MERCHANT_STNAME ELSE A.MERCHANT_STNAME END MERCHANT_STNAME,
		                                CASE WHEN F.SUM_MON_S = '-1'AND F.SUM_DAY_S ='01' THEN '清分日 上月1日-上月月底'
		                                WHEN F.SUM_MON_S = '-1' AND F.SUM_DAY_S = '02' THEN '交易日 上月2日-次月1日' END AS CONTRACT_PREIOD,

		                                SETTLE_RULE,
		                                FEE_DAY,
		                                SOURCE_RULE,
		                                A.SHOW_FLG,
		                                CASE WHEN DEPARTMENT IS NULL OR DEPARTMENT = '' THEN '3100' ELSE DEPARTMENT END AS DEPARTMENT, --預設3100  20220106改為預設3100
		                                CASE WHEN PROJECT_NO IS NULL OR PROJECT_NO = '' THEN 'I0001' ELSE PROJECT_NO END AS PROJECT_NO, --預設I0001
		                                CASE WHEN ACCOUNTING IS NULL OR ACCOUNTING = '' THEN '461102' ELSE ACCOUNTING END ACCOUNTING, --預設461102
		                                ORDER_NO,
                                        A.REM_TYPE

		                                FROM GM_MERCHANT_ACT A
		                                LEFT JOIN GM_MERCHANT_ACT_MAPPING B
		                                ON A.SET_GROUP = B.SET_GROUP 
		                                RIGHT JOIN GM_MERCHANT C
		                                ON C.MERCHANT_NO = A.MERCHANT_NO
		                                LEFT JOIN GM_MERCHANT_ACT_CLASS D
		                                ON A.CLASS = D.CLASS_TYPE
		                                LEFT JOIN GM_MERCHANT_ACT_PAYTYPE E
		                                ON A.PAY_TYPE = E.PAY_TYPE
		                                LEFT JOIN GM_CONTRACT_M F
		                                ON C.MERCHANT_NO = F.MERCHANT_NO
		                                LEFT JOIN GM_MERCHANT_TYPE_D G
		                                ON C.MERCHANT_NO = G.MERCHANT_NO
		                                JOIN GM_MERCHANT_TYPE_M H
		                                ON G.GROUP_ID = H.GROUP_ID

		                                WHERE C.MERCHANT_TYPE <> 'BANK' 
		                                AND C.MERCHANT_NO = @EXEC_MERCHANT_NO
                                        
                                        ";

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    sqlCmd.Parameters.Add("@EXEC_MERCHANT_NO", SqlDbType.VarChar).Value = merchant;
                    //sqlCmd.Parameters.Add("@merchant", SqlDbType.VarChar).Value = merchant;

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
        public DataTable GMACT_UPDATE(string merchant, string merchant_act, string merchant_note, string class_type, string pay_type, string set_group,
            string merchant_no_act_m, string invoice_rule, string set_group_m, string merchant_stname, string settle_rule, string fee_day,
            string source_rule, string show_flg, string department, string project_no, string accounting, string order_no, string rem_type,string username)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();
                    string sqlText = @" 
                                        --LOG更新                                       
                                        INSERT INTO GM_MERCHANT_ACT_LOG VALUES
                                        ('UPDATE',@USERNAME,@EXEC_MERCHANT_NOTE,@EXEC_MERCHANT_NO,NULL,GETDATE())
                                        --底稿更新
                                        UPDATE GM_MERCHANT_ACT
                                        SET MERCHANT_NO_ACT = @EXEC_MERCHANT_ACT ,MERCHANT_NOTE = @EXEC_MERCHANT_NOTE,
                                        CLASS = @EXEC_CLASS ,SET_GROUP = @EXEC_SET_GROUP,PAY_TYPE = @EXEC_PAY_TYPE,
                                        MERCHANT_NO_ACT_M = @EXEC_MERCHANT_NO_ACT_M,INVOICE_RULE = @EXEC_INVOICE_RULE,
                                        SET_GROUP_M = @EXEC_SET_GROUP_M,MERCHANT_STNAME = @EXEC_MERCHANT_STNAME,
                                        SETTLE_RULE = @EXEC_SETTLE_RULE ,FEE_DAY = @EXEC_FEE_DAY,SOURCE_RULE = @EXEC_SOURCE_RULE,
                                        SHOW_FLG = @EXEC_SHOW_FLG,DEPARTMENT = @EXEC_DEPARTMENT ,PROJECT_NO = @EXEC_PROJECT_NO,
                                        ACCOUNTING = @EXEC_ACCOUNTING ,ORDER_NO = @EXEC_ORDER_NO,REM_TYPE = @EXEC_REM_TYPE
                                        WHERE MERCHANT_NO = @EXEC_MERCHANT_NO
                                        --更新STORE
                                        --DECLARE @I INT
                                        --SELECT @I = COUNT(1) FROM GM_MERCHANT_ACT_STORE WHERE MERCHANT_NO = @EXEC_MERCHANT_NO
                                        --IF @I > 0 
                                        --BEGIN 
                                        --    UPDATE GM_MERCHANT_ACT_STORE
                                        --    SET  CLASS = @EXEC_CLASS ,SET_GROUP = @EXEC_SET_GROUP,PAY_TYPE = @EXEC_PAY_TYPE
                                        --    WHERE MERCHANT_NO = @EXEC_MERCHANT_NO          
                                        --END
                                        ";

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    sqlCmd.Parameters.Add("@EXEC_MERCHANT_ACT", SqlDbType.VarChar).Value = merchant_act;
                    sqlCmd.Parameters.Add("@EXEC_MERCHANT_NOTE", SqlDbType.VarChar).Value = merchant_note;
                    sqlCmd.Parameters.Add("@EXEC_CLASS", SqlDbType.VarChar).Value = class_type;
                    sqlCmd.Parameters.Add("@EXEC_PAY_TYPE", SqlDbType.VarChar).Value = pay_type;
                    sqlCmd.Parameters.Add("@EXEC_SET_GROUP", SqlDbType.VarChar).Value = set_group;
                    sqlCmd.Parameters.Add("@EXEC_MERCHANT_NO", SqlDbType.VarChar).Value = merchant;
                    //--------------------------------------------------------------------------
                    sqlCmd.Parameters.Add("@EXEC_MERCHANT_NO_ACT_M", SqlDbType.VarChar).Value = merchant_no_act_m;
                    sqlCmd.Parameters.Add("@EXEC_INVOICE_RULE", SqlDbType.VarChar).Value = invoice_rule;
                    sqlCmd.Parameters.Add("@EXEC_SET_GROUP_M", SqlDbType.VarChar).Value = set_group_m;
                    sqlCmd.Parameters.Add("@EXEC_MERCHANT_STNAME", SqlDbType.VarChar).Value = merchant_stname;
                    sqlCmd.Parameters.Add("@EXEC_SETTLE_RULE", SqlDbType.VarChar).Value = settle_rule;
                    sqlCmd.Parameters.Add("@EXEC_FEE_DAY", SqlDbType.VarChar).Value = fee_day;
                    sqlCmd.Parameters.Add("@EXEC_SOURCE_RULE", SqlDbType.VarChar).Value = source_rule;
                    sqlCmd.Parameters.Add("@EXEC_SHOW_FLG", SqlDbType.VarChar).Value = show_flg;
                    sqlCmd.Parameters.Add("@EXEC_DEPARTMENT", SqlDbType.VarChar).Value = department;
                    sqlCmd.Parameters.Add("@EXEC_PROJECT_NO", SqlDbType.VarChar).Value = project_no;
                    sqlCmd.Parameters.Add("@EXEC_ACCOUNTING", SqlDbType.VarChar).Value = accounting;
                    sqlCmd.Parameters.Add("@EXEC_ORDER_NO", SqlDbType.VarChar).Value = order_no;
                    sqlCmd.Parameters.Add("@EXEC_REM_TYPE", SqlDbType.VarChar).Value = rem_type;
                    sqlCmd.Parameters.Add("@USERNAME", SqlDbType.VarChar).Value = username;

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
        public string CheckMerchant(string merchant)
        {
            string i = null;
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();
                    string sqlText = @"SELECT * FROM GM_MERCHANT_ACT WHERE MERCHANT_NO = @EXEC_MERCHANT_NO ";

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);

                    sqlCmd.Parameters.Add("@EXEC_MERCHANT_NO", SqlDbType.VarChar).Value = merchant;
                    //sqlCmd.Parameters.Add("@merchant", SqlDbType.VarChar).Value = merchant;
                    var t = sqlCmd.ExecuteScalar();
                    if (t == null)
                    {
                        i = null;
                    }
                    else i = t.ToString();

                    sqlConn.Close();

                }
            }

            catch (Exception ex)
            {
                log.Debug(ex.Message);
            }

            return i;
        }
        /// <summary>
        /// 關係人List
        /// </summary>
        /// <returns></returns>
        public List<GmMerchant> FindSetSET_GROUP()
        {
            List<string> mNoList = new List<string>();
            List<GmMerchant> objList = new List<GmMerchant>();
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();
                    string sqlText = @"SELECT SET_GROUP G1,SET_GROUP_NAME G2 FROM GM_MERCHANT_ACT_MAPPING";

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    SqlDataReader dr = sqlCmd.ExecuteReader();
                    while (dr.Read())
                    {
                        GmMerchant obj = new GmMerchant();
                        obj.MerchantNo = dr["G1"].ToString();
                        obj.MerchantName = dr["G2"].ToString();
                        objList.Add(obj);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Debug("Query Ex: GM_MERCHANT_ACT_MAPPING");////
                log.Debug(ex.StackTrace);
                log.Debug(ex.Message);
                mNoList = null;
            }
            return objList;
        }
        //找不到對應merchant_no 新增GM_MERCHANT_ACT資料
        public void GMACT_INSERT(string merchant, string merchant_act, string merchant_note, string class_type, string pay_type, string set_group,
            string merchant_no_act_m, string invoice_rule, string set_group_m, string merchant_stname, string settle_rule, string fee_day,
            string source_rule, string show_flg, string department, string project_no, string accounting, string order_no, string rem_type,string username)
        {

            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();
                    string sqlText = @"
                    --LOG更新
                    INSERT INTO GM_MERCHANT_ACT_LOG VALUES
                    ('INSERT',@USERNAME,@EXEC_MERCHANT_NOTE,@EXEC_MERCHANT_NO,NULL,GETDATE())
                    --底稿更新                      
                    INSERT INTO GM_MERCHANT_ACT (MERCHANT_NO,MERCHANT_NO_ACT,MERCHANT_NOTE,CLASS,SET_GROUP,PAY_TYPE,
                    MERCHANT_NO_ACT_M,INVOICE_RULE,SET_GROUP_M,MERCHANT_STNAME,SETTLE_RULE,FEE_DAY,SOURCE_RULE,SHOW_FLG,DEPARTMENT,
                    PROJECT_NO,ACCOUNTING,ORDER_NO,REM_TYPE) VALUES
                    (@EXEC_MERCHANT_NO,@EXEC_MERCHANT_NO_ACT,@EXEC_MERCHANT_NOTE,@EXEC_CLASS,@EXEC_SET_GROUP,@EXEC_PAY_TYPE,
                    @EXEC_MERCHANT_NO_ACT_M,@EXEC_INVOICE_RULE,@EXEC_SET_GROUP_M,@EXEC_MERCHANT_STNAME, @EXEC_SETTLE_RULE,
                    @EXEC_FEE_DAY, @EXEC_SOURCE_RULE, @EXEC_SHOW_FLG, @EXEC_DEPARTMENT, @EXEC_PROJECT_NO, @EXEC_ACCOUNTING,
                    @EXEC_ORDER_NO,@EXEC_REM_TYPE); ";

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    sqlCmd.Parameters.Add("@EXEC_MERCHANT_NO_ACT", SqlDbType.VarChar).Value = merchant_act;
                    sqlCmd.Parameters.Add("@EXEC_MERCHANT_NOTE", SqlDbType.VarChar).Value = merchant_note;
                    sqlCmd.Parameters.Add("@EXEC_CLASS", SqlDbType.VarChar).Value = class_type;
                    sqlCmd.Parameters.Add("@EXEC_PAY_TYPE", SqlDbType.VarChar).Value = pay_type;
                    sqlCmd.Parameters.Add("@EXEC_SET_GROUP", SqlDbType.VarChar).Value = set_group;
                    sqlCmd.Parameters.Add("@EXEC_MERCHANT_NO", SqlDbType.VarChar).Value = merchant;
                    //--------------------------------------------------------------------------
                    sqlCmd.Parameters.Add("@EXEC_MERCHANT_NO_ACT_M", SqlDbType.VarChar).Value = merchant_no_act_m;
                    sqlCmd.Parameters.Add("@EXEC_INVOICE_RULE", SqlDbType.VarChar).Value = invoice_rule;
                    sqlCmd.Parameters.Add("@EXEC_SET_GROUP_M", SqlDbType.VarChar).Value = set_group_m;
                    sqlCmd.Parameters.Add("@EXEC_MERCHANT_STNAME", SqlDbType.VarChar).Value = merchant_stname;
                    sqlCmd.Parameters.Add("@EXEC_SETTLE_RULE", SqlDbType.VarChar).Value = settle_rule;
                    sqlCmd.Parameters.Add("@EXEC_FEE_DAY", SqlDbType.VarChar).Value = fee_day;
                    sqlCmd.Parameters.Add("@EXEC_SOURCE_RULE", SqlDbType.VarChar).Value = source_rule;
                    sqlCmd.Parameters.Add("@EXEC_SHOW_FLG", SqlDbType.VarChar).Value = show_flg;
                    sqlCmd.Parameters.Add("@EXEC_DEPARTMENT", SqlDbType.VarChar).Value = department;
                    sqlCmd.Parameters.Add("@EXEC_PROJECT_NO", SqlDbType.VarChar).Value = project_no;
                    sqlCmd.Parameters.Add("@EXEC_ACCOUNTING", SqlDbType.VarChar).Value = accounting;
                    sqlCmd.Parameters.Add("@EXEC_ORDER_NO", SqlDbType.VarChar).Value = order_no;
                    sqlCmd.Parameters.Add("@EXEC_REM_TYPE", SqlDbType.VarChar).Value = rem_type;
                    sqlCmd.Parameters.Add("@USERNAME", SqlDbType.VarChar).Value = username;
                    //sqlCmd.Parameters.Add("@merchant", SqlDbType.VarChar).Value = merchant;
                    sqlCmd.ExecuteNonQuery();
                }
            }

            catch (Exception ex)
            {
                log.Debug(ex.Message);
            }


        }

        /// <summary>
        /// 類別List
        /// </summary>
        /// <returns></returns>
        public List<GmMerchant> FindCLASS()
        {
            List<string> mNoList = new List<string>();
            List<GmMerchant> objList = new List<GmMerchant>();
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();
                    string sqlText = @"SELECT CLASS_TYPE G1,CLASS_NAME G2 FROM GM_MERCHANT_ACT_CLASS";

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    SqlDataReader dr = sqlCmd.ExecuteReader();
                    while (dr.Read())
                    {
                        GmMerchant obj = new GmMerchant();
                        obj.MerchantNo = dr["G1"].ToString();
                        obj.MerchantName = dr["G2"].ToString();
                        objList.Add(obj);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Debug("Query Ex: GM_MERCHANT_ACT_CLASS");////
                log.Debug(ex.StackTrace);
                log.Debug(ex.Message);
                mNoList = null;
            }
            return objList;
        }
        /// <summary>
        /// 類別List
        /// </summary>
        /// <returns></returns>
        public List<GmMerchant> FindPAYTYPE()
        {
            List<string> mNoList = new List<string>();
            List<GmMerchant> objList = new List<GmMerchant>();
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();
                    string sqlText = @"SELECT PAY_TYPE G1,PAY_NAME G2 FROM GM_MERCHANT_ACT_PAYTYPE";

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    SqlDataReader dr = sqlCmd.ExecuteReader();
                    while (dr.Read())
                    {
                        GmMerchant obj = new GmMerchant();
                        obj.MerchantNo = dr["G1"].ToString();
                        obj.MerchantName = dr["G2"].ToString();
                        objList.Add(obj);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Debug("Query Ex: GM_MERCHANT_ACT_CLASS");////
                log.Debug(ex.StackTrace);
                log.Debug(ex.Message);
                mNoList = null;
            }
            return objList;
        }
        public DataTable GMACT_ReIndex(string merchant)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();
                    string sqlText = @"
                                        SELECT * FROM GM_MERCHANT_TYPE_D WHERE MERCHANT_NO = @EXEC_MERCHANT_NO";

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);

                    sqlCmd.Parameters.Add("@EXEC_MERCHANT_NO", SqlDbType.VarChar).Value = merchant;
                    //sqlCmd.Parameters.Add("@merchant", SqlDbType.VarChar).Value = merchant;

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



        //開立方式		
        public List<GmMerchant> FindINVOICE_RULE()
        {
            List<string> mNoList = new List<string>();
            List<GmMerchant> objList = new List<GmMerchant>();
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();
                    string sqlText = @"SELECT OPTION_NO,OPTION_NAME FROM GM_MERCHANT_ACT_OPTION
										WHERE GROUP_ID = 'INVOICE_RULE'
										ORDER BY RANKING";

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    SqlDataReader dr = sqlCmd.ExecuteReader();
                    while (dr.Read())
                    {
                        GmMerchant obj = new GmMerchant();
                        obj.MerchantNo = dr["OPTION_NO"].ToString();
                        obj.MerchantName = dr["OPTION_NAME"].ToString();
                        objList.Add(obj);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Debug("Query Ex: FindINVOICE_RULE");////
                log.Debug(ex.StackTrace);
                log.Debug(ex.Message);
                mNoList = null;
            }
            return objList;
        }

        //關係人(手續費)
        public List<GmMerchant> FindSET_GROUP_M()
        {
            List<string> mNoList = new List<string>();
            List<GmMerchant> objList = new List<GmMerchant>();
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();
                    string sqlText = @"SELECT OPTION_NO,OPTION_NAME FROM GM_MERCHANT_ACT_OPTION
										WHERE GROUP_ID = 'SET_GROUP_M'
										ORDER BY RANKING";

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    SqlDataReader dr = sqlCmd.ExecuteReader();
                    while (dr.Read())
                    {
                        GmMerchant obj = new GmMerchant();
                        obj.MerchantNo = dr["OPTION_NO"].ToString();
                        obj.MerchantName = dr["OPTION_NAME"].ToString();
                        objList.Add(obj);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Debug("Query Ex: FindSET_GROUP_M");////
                log.Debug(ex.StackTrace);
                log.Debug(ex.Message);
                mNoList = null;
            }
            return objList;
        }

        //結算方式
        public List<GmMerchant> FindSETTLE_RULE()
        {
            List<string> mNoList = new List<string>();
            List<GmMerchant> objList = new List<GmMerchant>();
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();
                    string sqlText = @"SELECT OPTION_NO,OPTION_NAME FROM GM_MERCHANT_ACT_OPTION
										WHERE GROUP_ID = 'SETTLE_RULE'
										ORDER BY RANKING";

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    SqlDataReader dr = sqlCmd.ExecuteReader();
                    while (dr.Read())
                    {
                        GmMerchant obj = new GmMerchant();
                        obj.MerchantNo = dr["OPTION_NO"].ToString();
                        obj.MerchantName = dr["OPTION_NAME"].ToString();
                        objList.Add(obj);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Debug("Query Ex: FindSETTLE_RULE");////
                log.Debug(ex.StackTrace);
                log.Debug(ex.Message);
                mNoList = null;
            }
            return objList;
        }

        //來源表格&規則
        public List<GmMerchant> FindSOURCE_RULE()
        {
            List<string> mNoList = new List<string>();
            List<GmMerchant> objList = new List<GmMerchant>();
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();
                    string sqlText = @"SELECT OPTION_NO,OPTION_NAME FROM GM_MERCHANT_ACT_OPTION
										WHERE GROUP_ID = 'SOURCE_RULE'
										ORDER BY RANKING";

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    SqlDataReader dr = sqlCmd.ExecuteReader();
                    while (dr.Read())
                    {
                        GmMerchant obj = new GmMerchant();
                        obj.MerchantNo = dr["OPTION_NO"].ToString();
                        obj.MerchantName = dr["OPTION_NAME"].ToString();
                        objList.Add(obj);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Debug("Query Ex: FindSETTLE_RULE");////
                log.Debug(ex.StackTrace);
                log.Debug(ex.Message);
                mNoList = null;
            }
            return objList;
        }

        //預設開立
        public List<GmMerchant> FindSHOW_FLG()
        {
            List<string> mNoList = new List<string>();
            List<GmMerchant> objList = new List<GmMerchant>();
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();
                    string sqlText = @"SELECT OPTION_NO,OPTION_NAME FROM GM_MERCHANT_ACT_OPTION
										WHERE GROUP_ID = 'SHOW_FLG'
										ORDER BY RANKING";

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    SqlDataReader dr = sqlCmd.ExecuteReader();
                    while (dr.Read())
                    {
                        GmMerchant obj = new GmMerchant();
                        obj.MerchantNo = dr["OPTION_NO"].ToString();
                        obj.MerchantName = dr["OPTION_NAME"].ToString();
                        objList.Add(obj);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Debug("Query Ex: FindSHOW_FLG");////
                log.Debug(ex.StackTrace);
                log.Debug(ex.Message);
                mNoList = null;
            }
            return objList;
        }
        public List<GmMerchant> FindREM_TYPE()
        {
            List<string> mNoList = new List<string>();
            List<GmMerchant> objList = new List<GmMerchant>();
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();
                    string sqlText = @"SELECT OPTION_NO,OPTION_NAME FROM GM_MERCHANT_ACT_OPTION
										WHERE GROUP_ID = 'REM_TYPE'
										ORDER BY RANKING";

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    SqlDataReader dr = sqlCmd.ExecuteReader();
                    while (dr.Read())
                    {
                        GmMerchant obj = new GmMerchant();
                        obj.MerchantNo = dr["OPTION_NO"].ToString();
                        obj.MerchantName = dr["OPTION_NAME"].ToString();
                        objList.Add(obj);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Debug("Query Ex: FindREM_TYPE");////
                log.Debug(ex.StackTrace);
                log.Debug(ex.Message);
                mNoList = null;
            }
            return objList;
        }

        public DataTable GmAccount200201(string cpt_date, string no1, string no2)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();
                    string sqlText = string.Format(File.ReadAllText(System.AppDomain.CurrentDomain.BaseDirectory + "\\Sql\\GmAccount\\GmAccount200201.sql", Encoding.GetEncoding("big5")));

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    sqlCmd.CommandTimeout = 300;
                    sqlCmd.Parameters.Add("@CPT_DATE", SqlDbType.VarChar).Value = cpt_date;
                    sqlCmd.Parameters.Add("@NO1", SqlDbType.VarChar).Value = no1;
                    sqlCmd.Parameters.Add("@NO2", SqlDbType.VarChar).Value = no2;
                    //sqlCmd.Parameters.Add("@merchant", SqlDbType.VarChar).Value = merchant;

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

        public List<GmMerchant> FindORDER_NO()
        {
            List<string> mNoList = new List<string>();
            List<GmMerchant> objList = new List<GmMerchant>();
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();
                    string sqlText = @"SELECT OPTION_NO,OPTION_NAME FROM GM_MERCHANT_ACT_OPTION
										WHERE GROUP_ID = 'ORDER_NO'
										ORDER BY RANKING";

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    SqlDataReader dr = sqlCmd.ExecuteReader();
                    while (dr.Read())
                    {
                        GmMerchant obj = new GmMerchant();
                        obj.MerchantNo = dr["OPTION_NO"].ToString();
                        obj.MerchantName = dr["OPTION_NAME"].ToString();
                        objList.Add(obj);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Debug("Query Ex: FindREM_TYPE");////
                log.Debug(ex.StackTrace);
                log.Debug(ex.Message);
                mNoList = null;
            }
            return objList;
        }
        public List<GmMerchant> FindDATE_RANGE()
        {
            List<string> mNoList = new List<string>();
            List<GmMerchant> objList = new List<GmMerchant>();
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();
                    string sqlText = @"SELECT OPTION_NO,OPTION_NAME FROM GM_MERCHANT_ACT_OPTION
										WHERE GROUP_ID = 'DATE_RANGE'
										ORDER BY RANKING";

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    SqlDataReader dr = sqlCmd.ExecuteReader();
                    while (dr.Read())
                    {
                        GmMerchant obj = new GmMerchant();
                        obj.MerchantNo = dr["OPTION_NO"].ToString();
                        obj.MerchantName = dr["OPTION_NAME"].ToString();
                        objList.Add(obj);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Debug("Query Ex: FindDATE_RANGE");////
                log.Debug(ex.StackTrace);
                log.Debug(ex.Message);
                mNoList = null;
            }
            return objList;
        }


        /// <summary>
        /// 找委外有設定分潤日扣的群組
        /// </summary>
        /// <returns></returns>
        public DataTable FindOutSouringGroup()
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();
                    string sqlText = string.Format(@"select GROUP_ID,MERCHANT_NO from GM_OUTSOURING_GROUP");

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    SqlDataAdapter adapter = new SqlDataAdapter(sqlCmd);
                    adapter.Fill(dt);

                    sqlConn.Close();
                }
            }
            catch (Exception ex)
            {
                log.Debug("Query Ex: GM_OUTSOURING_GROUP");////
                log.Debug(ex.StackTrace);
                log.Debug(ex.Message);
                dt = null;
            }
            return dt;
        }


        public List<GmMerchant> FindINVOICE_RULE_RANK()
        {
            List<string> mNoList = new List<string>();
            List<GmMerchant> objList = new List<GmMerchant>();
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();
                    string sqlText = @"SELECT OPTION_NO,OPTION_NAME FROM GM_MERCHANT_ACT_OPTION
										WHERE GROUP_ID = 'INVOICE_RULE_RANK'
										ORDER BY RANKING";

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    SqlDataReader dr = sqlCmd.ExecuteReader();
                    while (dr.Read())
                    {
                        GmMerchant obj = new GmMerchant();
                        obj.MerchantNo = dr["OPTION_NO"].ToString();
                        obj.MerchantName = dr["OPTION_NAME"].ToString();
                        objList.Add(obj);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Debug("Query Ex: FindREM_TYPE");////
                log.Debug(ex.StackTrace);
                log.Debug(ex.Message);
                mNoList = null;
            }
            return objList;
        }
        //找手續費匯總表來源的所有特店
        /// <summary>
        /// 有值清分
        /// </summary>
        /// <param name="date_range">交易日，清分日</param>
        /// <param name="invoice_rule">排序方式 同門市代號彙總/分別</param>
        /// <param name="group">群組代碼</param>
        /// <returns></returns>
        public List<string> FindSOURCE_1(string date_range, string invoice_rule, string group)
        {
            List<string> mNoList = new List<string>();
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();
                    string sqlText = string.Format(
                                @"SELECT MERCHANT_NO FROM GM_MERCHANT_ACT 
                                where SOURCE_RULE = '1' AND SHOW_FLG = 'Y' and INVOICE_RULE = @INVOICE_RULE
                                AND MERCHANT_NO IN (SELECT MERCHANT_NO FROM GM_CONTRACT_M 
                                                    WHERE SUM_DAY_S = CASE WHEN @DATE_RANGE = 'ALL' THEN SUM_DAY_S ELSE @DATE_RANGE END)
                                AND MERCHANT_NO IN (SELECT MERCHANT_NO FROM GM_MERCHANT_TYPE_D WHERE GROUP_ID IN ({0}) )
                                ", group);

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    sqlCmd.Parameters.Add("@DATE_RANGE", SqlDbType.VarChar).Value = date_range;
                    sqlCmd.Parameters.Add("@INVOICE_RULE", SqlDbType.VarChar).Value = invoice_rule;

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
        /// 暫存TABLE清空
        /// </summary>
        /// <returns></returns>
        public DataTable GmAccount200601_Clear()
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();
                    string sqlText = @"TRUNCATE TABLE GM_MERCHANT_ACT_DATA_TEMP";

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    sqlCmd.CommandTimeout = 300;
                    //sqlCmd.Parameters.Add("@merchant", SqlDbType.VarChar).Value = merchant;

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

        /// <summary>
        /// 有值清分營收款
        /// </summary>
        /// <param name="cpt_date"></param>
        /// <param name="yearMonth"></param>
        /// <param name="date_range"></param>
        /// <param name="merchant"></param>
        /// <param name="groups"></param>
        /// <param name="invoice_rule"></param>
        /// <returns></returns>
        public DataTable GmAccount200601_01(string cpt_date, string yearMonth, string date_range, string merchant, string groups, string invoice_rule)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();
                    string sqlText = string.Format(File.ReadAllText(System.AppDomain.CurrentDomain.BaseDirectory + "\\Sql\\GmAccount\\GmAccount200601_01.sql", Encoding.GetEncoding("big5")), groups);

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    sqlCmd.CommandTimeout = 300;
                    sqlCmd.Parameters.Add("@EXEC_CPT_DATE", SqlDbType.VarChar).Value = cpt_date;
                    sqlCmd.Parameters.Add("@YEARMONTH", SqlDbType.VarChar).Value = yearMonth;
                    sqlCmd.Parameters.Add("@DATE_RANGE", SqlDbType.VarChar).Value = date_range;
                    sqlCmd.Parameters.Add("@EXEC_MERCHANT_NO", SqlDbType.VarChar).Value = merchant;
                    sqlCmd.Parameters.Add("@GROUP", SqlDbType.VarChar).Value = groups;
                    sqlCmd.Parameters.Add("@INVOICE_RULE", SqlDbType.VarChar).Value = invoice_rule;
                    //sqlCmd.Parameters.Add("@merchant", SqlDbType.VarChar).Value = merchant;

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









        /// <summary>
        /// 日扣
        /// </summary>
        /// <param name="cpt_date"></param>
        /// <param name="yearMonth"></param>
        /// <param name="date_range"></param>
        /// <param name="schema"></param>
        /// <param name="ab_mno">收單行代號</param>
        /// <param name="groups"></param>
        /// <param name="invoice_rule"></param>
        /// <returns></returns>
        public DataTable GmAccount200601_05(string cpt_date, string yearMonth, string date_range, string schema, string ab_mno, string groups, string invoice_rule)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();
                    string sqlText = string.Format(File.ReadAllText(System.AppDomain.CurrentDomain.BaseDirectory + "\\Sql\\GmAccount\\GmAccount200601_05.sql", Encoding.GetEncoding("big5")), schema, groups);

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    sqlCmd.CommandTimeout = 300;
                    sqlCmd.Parameters.Add("@EXEC_CPT_DATE", SqlDbType.VarChar).Value = cpt_date;
                    sqlCmd.Parameters.Add("@YEARMONTH", SqlDbType.VarChar).Value = yearMonth;
                    sqlCmd.Parameters.Add("@DATE_RANGE", SqlDbType.VarChar).Value = date_range;
                    sqlCmd.Parameters.Add("@AB_MERCHANT_NO", SqlDbType.VarChar).Value = ab_mno;
                    sqlCmd.Parameters.Add("@GROUP", SqlDbType.VarChar).Value = groups;
                    sqlCmd.Parameters.Add("@INVOICE_RULE", SqlDbType.VarChar).Value = invoice_rule;
                    //sqlCmd.Parameters.Add("@merchant", SqlDbType.VarChar).Value = merchant;

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
        public DataTable GmAccount200601_Y(string date_range)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();
                    string sqlText = string.Format(File.ReadAllText(System.AppDomain.CurrentDomain.BaseDirectory + "\\Sql\\GmAccount\\GmAccount200601_Y.sql", Encoding.GetEncoding("big5")));

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    sqlCmd.CommandTimeout = 300;
                    sqlCmd.Parameters.Add("@DATE_RANGE", SqlDbType.VarChar).Value = date_range;
                    //sqlCmd.Parameters.Add("@merchant", SqlDbType.VarChar).Value = merchant;

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
        public DataTable GmAccount200601_N(string date_range)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();
                    string sqlText = string.Format(File.ReadAllText(System.AppDomain.CurrentDomain.BaseDirectory + "\\Sql\\GmAccount\\GmAccount200601_N.sql", Encoding.GetEncoding("big5")));

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    sqlCmd.CommandTimeout = 300;
                    sqlCmd.Parameters.Add("@DATE_RANGE", SqlDbType.VarChar).Value = date_range;
                    //sqlCmd.Parameters.Add("@merchant", SqlDbType.VarChar).Value = merchant;

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

        /// <summary>
        /// 找高雄零值群組
        /// </summary>
        /// <param name="date_range"></param>
        /// <param name="invoice_rule"></param>
        /// <param name="group"></param>
        /// <returns></returns>
        public List<string> FindSOURCE_3(string date_range, string invoice_rule, string group)
        {
            List<string> mNoList = new List<string>();
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();
                    string sqlText = string.Format(
                                @"SELECT MERCHANT_NO FROM GM_MERCHANT_ACT 
                                where SOURCE_RULE = '3' AND SHOW_FLG = 'Y' and INVOICE_RULE = @INVOICE_RULE
                                AND MERCHANT_NO IN (SELECT MERCHANT_NO FROM GM_CONTRACT_M 
                                                    WHERE SUM_DAY_S = CASE WHEN @DATE_RANGE = 'ALL' THEN SUM_DAY_S ELSE @DATE_RANGE END)
                                AND MERCHANT_NO IN (SELECT MERCHANT_NO FROM GM_MERCHANT_TYPE_D WHERE GROUP_ID IN ({0}) )
                                ", group);

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    sqlCmd.Parameters.Add("@DATE_RANGE", SqlDbType.VarChar).Value = date_range;
                    sqlCmd.Parameters.Add("@INVOICE_RULE", SqlDbType.VarChar).Value = invoice_rule;

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
                log.Debug("Query Ex: FindSource_3");////
                log.Debug(ex.StackTrace);
                log.Debug(ex.Message);
                mNoList = null;
            }
            return mNoList;
        }

        /// <summary>
        /// 高雄零值
        /// </summary>
        /// <param name="cpt_date"></param>
        /// <param name="yearMonth"></param>
        /// <param name="date_range"></param>
        /// <param name="merchant"></param>
        /// <param name="groups"></param>
        /// <param name="invoice_rule"></param>
        /// <returns></returns>
        public DataTable GmAccount200601_03(string cpt_date, string yearMonth, string date_range, string merchant, string groups, string invoice_rule)
        {
            DataTable dt = new DataTable();
            GmSchemaObjDAO schemaDAO = new GmSchemaObjDAO();
            string schema = schemaDAO.FindSchemaObj(merchant).MerchantSchemaId;

            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();
                    string sqlText = string.Format(File.ReadAllText(System.AppDomain.CurrentDomain.BaseDirectory + "\\Sql\\GmAccount\\GmAccount200601_03.sql", Encoding.GetEncoding("big5")),schema);
                    sqlText = string.Format(sqlText, schema);

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    sqlCmd.CommandTimeout = 300;
                    sqlCmd.Parameters.Add("@EXEC_CPT_DATE", SqlDbType.VarChar).Value = cpt_date;
                    sqlCmd.Parameters.Add("@YEARMONTH", SqlDbType.VarChar).Value = yearMonth;
                    sqlCmd.Parameters.Add("@DATE_RANGE", SqlDbType.VarChar).Value = date_range;
                    sqlCmd.Parameters.Add("@EXEC_MERCHANT_NO", SqlDbType.VarChar).Value = merchant;
                    sqlCmd.Parameters.Add("@GROUP", SqlDbType.VarChar).Value = groups;
                    sqlCmd.Parameters.Add("@INVOICE_RULE", SqlDbType.VarChar).Value = invoice_rule;
                    //sqlCmd.Parameters.Add("@merchant", SqlDbType.VarChar).Value = merchant;

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
















        }
    }
