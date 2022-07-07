using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

using log4net;
using Domain;
using System.IO;

namespace DataAccess
{
    public class ReportACTDAO:BasicDAL
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ReportACTDAO));

        private GmSchemaObjDAO gmSchema = new GmSchemaObjDAO();

        GmMerchantDAO GmDAO = null;
        public ReportACTDAO()
        {   
            GmDAO  = new GmMerchantDAO();
        }
        public DataTable ReportACT160901(string yearMonth, decimal newRate)
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();
                    string sqlText = "SELECT * FROM (";

                    List<string> schemaList = new List<string>();

                    GmSchemaObjDAO schemaDAO = new GmSchemaObjDAO();

                    foreach (GmSchemaObj schemaObj in schemaDAO.FindAllSchemaSET())
                    {
                        string schema = schemaObj.MerchantSchemaId;
                        if (schema != "ZOO" && schema != "MCD" && schema != "UPN" && schema != "PZH" && schema != "P01" && schema != "ICA" && schema != "UBK" && schema != "PRTC" && schema != "KIN" && schema != "UTI")
                            schemaList.Add(schema);
                    }

                    sqlText += File.ReadAllText(System.AppDomain.CurrentDomain.BaseDirectory + "\\Sql\\ReportACT\\ReportACT160901_01.sql", Encoding.GetEncoding("big5"));

                    foreach(string schema in schemaList)
                    {

                        sqlText += string.Format(File.ReadAllText(System.AppDomain.CurrentDomain.BaseDirectory + "\\Sql\\ReportACT\\ReportACT160901_02.sql", Encoding.GetEncoding("big5")), schema);
                        
                        if(schema != schemaList.Last())
                        {
                            sqlText+=" UNION ALL ";
                        
                        }
                        else
                        { sqlText+=") X ORDER BY X.MERCHANT_STNAME";}
                    }

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    sqlCmd.Parameters.Add("@yearMonth", SqlDbType.VarChar).Value = yearMonth;
                    sqlCmd.Parameters.Add("@newRate", SqlDbType.Decimal).Value = newRate;

                    SqlDataAdapter adapter = new SqlDataAdapter(sqlCmd);
                    adapter.Fill(dt);

                    sqlConn.Close();
                    
                }
            }
            catch(Exception ex)
            {
                log.Debug(ex.Message);
            }
            return dt;
        }

        public DataTable ReportACT161101(string firstDate, string lastDate, string merchant)
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();
                    string sqlText = File.ReadAllText(System.AppDomain.CurrentDomain.BaseDirectory + "\\Sql\\ReportACT\\ReportACT161101_01.sql", Encoding.GetEncoding("big5"));

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    sqlCmd.Parameters.Add("@firstDay", SqlDbType.VarChar).Value = firstDate;
                    sqlCmd.Parameters.Add("@lastDay", SqlDbType.VarChar).Value = lastDate;
                    sqlCmd.Parameters.Add("@merchant", SqlDbType.VarChar).Value = merchant;

                    SqlDataAdapter adapter = new SqlDataAdapter(sqlCmd);
                    adapter.Fill(dt);

                    sqlConn.Close();

                }
            }
            catch(Exception ex)
            {
                log.Debug(ex.Message);
            }
            return dt;
        }

        public DataTable ReportACT161201(string settleDate, string merchant)
        {
            DataTable dt = new DataTable();

            GmSchemaObjDAO schemaDAO = new GmSchemaObjDAO();

            string schema = schemaDAO.FindSchemaObj(merchant).MerchantSchemaId;

            string mName = schemaDAO.FindSchemaObj(merchant).MerchantName;

            string sqlStatement = "";

            using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
            {
                sqlConn.Open();

                string sqlText = string.Format(File.ReadAllText(System.AppDomain.CurrentDomain.BaseDirectory + "\\Sql\\ReportACT\\ReportACT161201_01.sql", Encoding.GetEncoding("big5")), schema, merchant, mName);
                SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                SqlDataReader dr = sqlCmd.ExecuteReader();
                while (dr.Read())
                {
                    sqlStatement = dr["SQL"].ToString();
                }
                
                sqlConn.Close();
            }      


            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();

                   
                    //string sqlText = string.Format("SELECT b.TRANS_DESC,COUNT(*) AS COUNT,SUM(TRANS_AMT) AS SUM FROM {0}.TM_SETTLE_DATA_D a INNER JOIN {0}.BM_TRANS_TYPE b ON a.TRANS_TYPE=b.TRANS_TYPE WHERE CPT_DATE=@settleDate GROUP BY b.TRANS_TYPE,b.TRANS_DESC", schema);

                    SqlCommand sqlCmd = new SqlCommand(sqlStatement, sqlConn);
                    sqlCmd.Parameters.Add("@settleDate", SqlDbType.VarChar).Value = settleDate;
                    //sqlCmd.Parameters.Add("@merchant", SqlDbType.Decimal).Value = merchant;
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

        public DataTable ReportACT161202(string sDate,string eDate, string merchant)
        {
            DataTable dt = new DataTable();

            GmSchemaObjDAO schemaDAO = new GmSchemaObjDAO();

            string schema = schemaDAO.FindSchemaObj(merchant).MerchantSchemaId;

            string mName = schemaDAO.FindSchemaObj(merchant).MerchantName;

            string sqlStatement = "";

            using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
            {
                sqlConn.Open();

                string sqlText = string.Format(File.ReadAllText(System.AppDomain.CurrentDomain.BaseDirectory + "\\Sql\\ReportACT\\ReportACT161202_01.sql", Encoding.GetEncoding("big5")), schema, merchant, mName);
                SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                SqlDataReader dr = sqlCmd.ExecuteReader();
                while (dr.Read())
                {
                    sqlStatement = dr["SQL"].ToString();
                }

                sqlConn.Close();
            }      

            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();

                    SqlCommand sqlCmd = new SqlCommand(sqlStatement, sqlConn);
                    sqlCmd.Parameters.Add("@sDate", SqlDbType.VarChar).Value = sDate;
                    sqlCmd.Parameters.Add("@eDate", SqlDbType.VarChar).Value = eDate;
                    //sqlCmd.Parameters.Add("@merchant", SqlDbType.Decimal).Value = merchant;
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

        public DataTable ReportACT170101(string startDate, string endDate)
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();
                    string sqlText = File.ReadAllText(System.AppDomain.CurrentDomain.BaseDirectory + "\\Sql\\ReportACT\\ReportACT170101_01.sql", Encoding.GetEncoding("big5"));

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    sqlCmd.Parameters.Add("@sDate", SqlDbType.VarChar).Value = startDate;
                    sqlCmd.Parameters.Add("@eDate", SqlDbType.VarChar).Value = endDate;
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

        public DataTable ReportACT170102(string startDate, string endDate)
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();
                    string sqlText = File.ReadAllText(System.AppDomain.CurrentDomain.BaseDirectory + "\\Sql\\ReportACT\\ReportACT170102_01.sql", Encoding.GetEncoding("big5"));

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    sqlCmd.Parameters.Add("@sDate", SqlDbType.VarChar).Value = startDate;
                    sqlCmd.Parameters.Add("@eDate", SqlDbType.VarChar).Value = endDate;
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

        public DataTable ReportACT170103(string startDate, string endDate)
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();
                    string sqlText = File.ReadAllText(System.AppDomain.CurrentDomain.BaseDirectory + "\\Sql\\ReportACT\\ReportACT170103_01.sql", Encoding.GetEncoding("big5"));

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    sqlCmd.Parameters.Add("@sDate", SqlDbType.VarChar).Value = startDate;
                    sqlCmd.Parameters.Add("@eDate", SqlDbType.VarChar).Value = endDate;
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

        public DataTable ReportACT170104(string startDate, string endDate)
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();
                    string sqlText = File.ReadAllText(System.AppDomain.CurrentDomain.BaseDirectory + "\\Sql\\ReportACT\\ReportACT170104_01.sql", Encoding.GetEncoding("big5"));

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    sqlCmd.Parameters.Add("@sDate", SqlDbType.VarChar).Value = startDate;
                    sqlCmd.Parameters.Add("@eDate", SqlDbType.VarChar).Value = endDate;
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

        public DataTable ReportACT170105(string startDate, string endDate)
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();
                    string sqlText = File.ReadAllText(System.AppDomain.CurrentDomain.BaseDirectory + "\\Sql\\ReportACT\\ReportACT170105_01.sql", Encoding.GetEncoding("big5"));

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    sqlCmd.Parameters.Add("@sDate", SqlDbType.VarChar).Value = startDate;
                    sqlCmd.Parameters.Add("@eDate", SqlDbType.VarChar).Value = endDate;
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

        
        public DataTable ReportACT170201(string settleDate)
        {
            DataTable dt = new DataTable();

            GmSchemaObjDAO schemaDAO = new GmSchemaObjDAO();

            //string schema = schemaDAO.FindSchemaObj(merchant).MerchantSchemaId;

            //string sqlStatement = "";


            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();


                    string sqlText = string.Format(File.ReadAllText(System.AppDomain.CurrentDomain.BaseDirectory + "\\Sql\\ReportACT\\ReportACT170201_01.sql", Encoding.GetEncoding("big5")));

                    //sqlCmd.Parameters.Add("@merchant", SqlDbType.Decimal).Value = merchant;
                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    sqlCmd.Parameters.Add("@settleDate", SqlDbType.VarChar).Value = settleDate;
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
        public DataTable ReportACT170202(string settleDate)
        {
            DataTable dt = new DataTable();






            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();


                    string sqlText = string.Format(File.ReadAllText(System.AppDomain.CurrentDomain.BaseDirectory + "\\Sql\\ReportACT\\ReportACT170202_01.sql", Encoding.GetEncoding("big5")));
                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    sqlCmd.Parameters.Add("@settleDate", SqlDbType.VarChar).Value = settleDate;
                    //sqlCmd.Parameters.Add("@merchant", SqlDbType.Decimal).Value = merchant;

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

        public DataTable ReportACT170401(string yearMonth,string startDate, string endDate)
        {

            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();


                    string sqlText = File.ReadAllText(System.AppDomain.CurrentDomain.BaseDirectory + "\\Sql\\ReportACT\\ReportACT170401_01.sql", Encoding.GetEncoding("big5"));
                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);

                    sqlCmd.Parameters.Add("@yearMonth", SqlDbType.VarChar).Value = yearMonth;
                    sqlCmd.Parameters.Add("@startDate", SqlDbType.VarChar).Value = startDate;
                    sqlCmd.Parameters.Add("@endDate", SqlDbType.VarChar).Value = endDate;
                    //sqlCmd.Parameters.Add("@settleDate", SqlDbType.VarChar).Value = settleDate;
                    //sqlCmd.Parameters.Add("@merchant", SqlDbType.Decimal).Value = merchant;

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

        public DataTable ReportACT170501(string yearMonth)
        {
            int startM = 0;
            string startD = "";
            int endM = 0;
            string endD = "";

            string start = "";
            string end = "";

            List<string[]> setStartEndList = new List<string[]>();
            List<string[]> nonSetStartEndList = new List<string[]>();

            List<string> merchantSetList = new List<string>();
            List<string> merchantNonSetList = new List<string>();

            foreach (GmMerchant merch in this.GmDAO.FindAllSET())
            {              
                merchantSetList.Add(merch.MerchantNo);               
            }

            foreach (GmMerchant merch in this.GmDAO.FindAllNonSET())
            {
                
                merchantNonSetList.Add(merch.MerchantNo);
            }

             merchantSetList.OrderBy(x => x);
            merchantNonSetList.OrderBy(x => x);


            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {


                    sqlConn.Open();
                    {


                        foreach (string mNo in merchantSetList)
                        {
                            string preQuery = string.Format(@"SELECT SUM_MON_S,SUM_DAY_S,SUM_MON_E,SUM_DAY_E FROM GM_CONTRACT_M WHERE MERCHANT_NO='{0}'", mNo);
                            SqlCommand sqlCmd = new SqlCommand(preQuery, sqlConn);
                            SqlDataReader reader = sqlCmd.ExecuteReader();
                            int counter = 0;
                            while (reader.Read())
                            {
                                startM = Convert.ToInt32(reader["SUM_MON_S"]);
                                startD = reader["SUM_DAY_S"].ToString();
                                endM = Convert.ToInt32(reader["SUM_MON_E"].ToString());
                                endD = reader["SUM_DAY_E"].ToString();

                                counter++;
                            }
                            reader.Close();
                            reader.Dispose();
                            sqlCmd.Dispose();

                            if (counter > 0)
                            {

                                int diff = 0;

                                switch (startM)
                                {
                                    case -1:
                                        start = DateTime.ParseExact(yearMonth + startD, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture).ToString("yyyyMMdd");
                                        break;

                                    case 0:
                                        start = DateTime.ParseExact(yearMonth + startD, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture).AddMonths(1).ToString("yyyyMMdd");
                                        break;

                                    default:
                                        start = "";
                                        break;
                                }


                                if (endD == "99")
                                {
                                    switch (endM)
                                    {
                                        case -1:
                                            DateTime tempN1 = DateTime.ParseExact(yearMonth + "01", "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);
                                            diff = DateTime.DaysInMonth(tempN1.Year, tempN1.Month) - 1;

                                            end = tempN1.AddDays(diff).ToString("yyyyMMdd");
                                            break;

                                        case 0:
                                            DateTime temp00 = DateTime.ParseExact(yearMonth + "01", "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture).AddMonths(1);
                                            diff = DateTime.DaysInMonth(temp00.Year, temp00.Month) - 1;

                                            end = temp00.AddDays(diff).ToString("yyyyMMdd");
                                            break;

                                        default:
                                            end = "";
                                            break;
                                    }
                                }
                                else
                                {
                                    switch (endM)
                                    {
                                        case -1:
                                            end = DateTime.ParseExact(yearMonth + endD, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture).ToString("yyyyMMdd");
                                            break;

                                        case 0:
                                            end = DateTime.ParseExact(yearMonth + endD, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture).AddMonths(1).ToString("yyyyMMdd");
                                            break;

                                        default:
                                            end = "";
                                            break;
                                    }
                                }
                                setStartEndList.Add(new string[] { mNo, start, end });
                            }
                            
                            //log.Debug(mNo + " " + start + " " + end);
                        }

                        foreach (string mNo in merchantNonSetList)
                        {
                            string preQuery = string.Format(@"SELECT SUM_MON_S,SUM_DAY_S,SUM_MON_E,SUM_DAY_E FROM GM_CONTRACT_M WHERE MERCHANT_NO='{0}'", mNo);
                            SqlCommand sqlCmd = new SqlCommand(preQuery, sqlConn);
                            SqlDataReader reader = sqlCmd.ExecuteReader();
                            int counter = 0;
                            while (reader.Read())
                            {
                                startM = Convert.ToInt32(reader["SUM_MON_S"]);
                                startD = reader["SUM_DAY_S"].ToString();
                                endM = Convert.ToInt32(reader["SUM_MON_E"].ToString());
                                endD = reader["SUM_DAY_E"].ToString();

                                counter++;
                            }
                            reader.Close();
                            reader.Dispose();
                            sqlCmd.Dispose();

                            if (counter > 0)
                            {

                                int diff = 0;

                                switch (startM)
                                {
                                    case -1:
                                        start = DateTime.ParseExact(yearMonth + startD, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture).ToString("yyyyMMdd");
                                        break;

                                    case 0:
                                        start = DateTime.ParseExact(yearMonth + startD, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture).AddMonths(1).ToString("yyyyMMdd");
                                        break;

                                    default:
                                        start = "";
                                        break;
                                }


                                if (endD == "99")
                                {
                                    switch (endM)
                                    {
                                        case -1:
                                            DateTime tempN1 = DateTime.ParseExact(yearMonth + "01", "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);
                                            diff = DateTime.DaysInMonth(tempN1.Year, tempN1.Month) - 1;

                                            end = tempN1.AddDays(diff).ToString("yyyyMMdd");
                                            break;

                                        case 0:
                                            DateTime temp00 = DateTime.ParseExact(yearMonth + "01", "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture).AddMonths(1);
                                            diff = DateTime.DaysInMonth(temp00.Year, temp00.Month) - 1;

                                            end = temp00.AddDays(diff).ToString("yyyyMMdd");
                                            break;

                                        default:
                                            end = "";
                                            break;
                                    }
                                }
                                else
                                {
                                    switch (endM)
                                    {
                                        case -1:
                                            end = DateTime.ParseExact(yearMonth + endD, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture).ToString("yyyyMMdd");
                                            break;

                                        case 0:
                                            end = DateTime.ParseExact(yearMonth + endD, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture).AddMonths(1).ToString("yyyyMMdd");
                                            break;

                                        default:
                                            end = "";
                                            break;
                                    }
                                }
                                nonSetStartEndList.Add(new string[] { mNo, start, end });
                            }

                            //log.Debug(mNo + " " + start + " " + end);
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                log.Debug(ex.Message);
            }

            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();

                    string sqlText = File.ReadAllText(System.AppDomain.CurrentDomain.BaseDirectory + "\\Sql\\ReportACT\\ReportACT170501_01.sql", Encoding.GetEncoding("big5"));

                    foreach (string[] startEnd in setStartEndList)
                    {
                        if (startEnd[0] != "22555003")
                        {
                            DataTable dt0 = new DataTable();

                            SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                            sqlCmd.CommandTimeout = 300;
                            //log.Debug(mNo + " " + startEndList.Where(x => x[0] == mNo).FirstOrDefault()[1]);
                            //log.Debug(mNo + " " + startEndList.Where(x => x[0] == mNo).FirstOrDefault()[2]);
                            sqlCmd.Parameters.Add("@EXEC_CPT_DATE_B", SqlDbType.VarChar).Value = startEnd[1];
                            sqlCmd.Parameters.Add("@EXEC_CPT_DATE_E", SqlDbType.VarChar).Value = startEnd[2];
                            sqlCmd.Parameters.Add("@EXEC_MERCHANT_NO", SqlDbType.VarChar).Value = startEnd[0];
                            sqlCmd.CommandTimeout = 300;
                            SqlDataAdapter adapter = new SqlDataAdapter(sqlCmd);
                            adapter.Fill(dt0);

                            sqlCmd.Dispose();
                            adapter.Dispose();

                            dt.Merge(dt0);
                        }
                        else
                        {
                            string firstDate = yearMonth + "02";
                            string lastDate = DateTime.ParseExact(yearMonth + "02", "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture).AddMonths(1).AddDays(-1).ToString("yyyyMMdd");

                            string sqlText1 = File.ReadAllText(System.AppDomain.CurrentDomain.BaseDirectory + "\\Sql\\ReportACT\\ReportACT170501_02.sql", Encoding.GetEncoding("big5"));
                            SqlCommand sqlCmd1 = new SqlCommand(sqlText1, sqlConn);
                            sqlCmd1.CommandTimeout = 300;
                            sqlCmd1.Parameters.Add("@EXEC_CPT_DATE_B", SqlDbType.VarChar).Value = firstDate;
                            sqlCmd1.Parameters.Add("@EXEC_CPT_DATE_E", SqlDbType.VarChar).Value = lastDate;

                            decimal cm=Convert.ToDecimal(sqlCmd1.ExecuteScalar());

                            string sqlText1r = File.ReadAllText(System.AppDomain.CurrentDomain.BaseDirectory + "\\Sql\\ReportACT\\ReportACT170501_2r.sql", Encoding.GetEncoding("big5"));
                            SqlCommand sqlCmd1r = new SqlCommand(sqlText1r, sqlConn);
                            sqlCmd1r.Parameters.Add("@EXEC_CPT_DATE_B", SqlDbType.VarChar).Value = firstDate;
                            decimal cmRate = Convert.ToDecimal(sqlCmd1r.ExecuteScalar());


                            string sqlText2 = File.ReadAllText(System.AppDomain.CurrentDomain.BaseDirectory + "\\Sql\\ReportACT\\ReportACT170501_03.sql", Encoding.GetEncoding("big5"));

                            SqlCommand sqlCmd2 = new SqlCommand(sqlText2, sqlConn);

                            sqlCmd2.Parameters.Add("@EXEC_CPT_DATE_B", SqlDbType.VarChar).Value = firstDate;
                            sqlCmd2.Parameters.Add("@EXEC_CPT_DATE_E", SqlDbType.VarChar).Value = lastDate;

                            decimal load = Convert.ToDecimal(sqlCmd2.ExecuteScalar());

                            string sqlText2r = File.ReadAllText(System.AppDomain.CurrentDomain.BaseDirectory + "\\Sql\\ReportACT\\ReportACT170501_3r.sql", Encoding.GetEncoding("big5"));

                            SqlCommand sqlCmd2r = new SqlCommand(sqlText2r, sqlConn);
                            sqlCmd2r.Parameters.Add("@EXEC_CPT_DATE_B", SqlDbType.VarChar).Value = firstDate;
                            decimal loadRate= Convert.ToDecimal(sqlCmd2r.ExecuteScalar());


                            string sqlText3 = File.ReadAllText(System.AppDomain.CurrentDomain.BaseDirectory + "\\Sql\\ReportACT\\ReportACT170501_04.sql", Encoding.GetEncoding("big5"));

                            SqlCommand sqlCmd3 = new SqlCommand(sqlText3, sqlConn);
                            sqlCmd3.CommandTimeout = 300;
                            sqlCmd3.Parameters.Add("@EXEC_CPT_DATE_B", SqlDbType.VarChar).Value = yearMonth + "01";
                            sqlCmd3.Parameters.Add("@EXEC_CPT_DATE_E", SqlDbType.VarChar).Value = DateTime.ParseExact(yearMonth + "01", "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture).AddMonths(1).AddDays(-1).ToString("yyyyMMdd");

                            decimal autoLoad = Convert.ToDecimal(sqlCmd3.ExecuteScalar());

                            string sqlText3r= File.ReadAllText(System.AppDomain.CurrentDomain.BaseDirectory + "\\Sql\\ReportACT\\ReportACT170501_4r.sql", Encoding.GetEncoding("big5"));
                            SqlCommand sqlCmd3r = new SqlCommand(sqlText3r, sqlConn);
                            sqlCmd3r.Parameters.Add("@EXEC_CPT_DATE_B", SqlDbType.VarChar).Value = yearMonth + "01";
                            decimal autoLoadRate= Convert.ToDecimal(sqlCmd3r.ExecuteScalar());


                            DataRow pcsc = dt.NewRow();
                            pcsc["特約機構名稱"] = "統一超商(POS)";
                            pcsc["統編"] = "22555003";
                            pcsc["特店代碼"] = "22555003";
                            pcsc["特店性質"] = "零售";
                            pcsc["簡稱"] = "統一超商"; //20190131加入統編和簡稱
                            pcsc["購貨淨額"] = Convert.ToInt64(cm);
                            pcsc["購貨重複"] = "";
                            pcsc["購貨調帳"] = "";
                            pcsc["購貨手續費率"] = cmRate * 100;
                            long cmFee = Convert.ToInt64(Math.Round(cm*cmRate,MidpointRounding.AwayFromZero));
                            pcsc["消費手續費"] = cmFee;
                            pcsc["消費手續費未稅"] = Convert.ToInt64(Math.Round(cmFee / 1.05, MidpointRounding.AwayFromZero));
                            pcsc["消費手續費稅金"] = cmFee - Convert.ToInt64(Math.Round(cmFee / 1.05, MidpointRounding.AwayFromZero));

                            pcsc["加值淨額"] = Convert.ToInt64(load);
                            pcsc["加值重複"] = "";
                            pcsc["加值調帳"] = "";
                            pcsc["加值手續費率"] = loadRate * 100;
                            long loadFee = Convert.ToInt64(Math.Round(load*loadRate,MidpointRounding.AwayFromZero));
                            pcsc["加值手續費"] = loadFee;
                            pcsc["加值手續費未稅"] = Convert.ToInt64(Math.Round(loadFee / 1.05, MidpointRounding.AwayFromZero));
                            pcsc["加值手續費稅金"] = loadFee - Convert.ToInt64(Math.Round(loadFee / 1.05, MidpointRounding.AwayFromZero));

                            pcsc["自動加值額"] = Convert.ToInt64(autoLoad);
                            pcsc["自動加值重複"] = "";
                            pcsc["自動加值調帳"] = "";
                            pcsc["自動加值手續費率"] = autoLoadRate * 100;
                            long autoLoadFee = Convert.ToInt64(Math.Round(autoLoad * autoLoadRate, MidpointRounding.AwayFromZero));
                            pcsc["自動加值手續費"] = autoLoadFee;
                            pcsc["自動加值手續費未稅"] = Convert.ToInt64(Math.Round(autoLoadFee / 1.05, MidpointRounding.AwayFromZero));
                            pcsc["自動加值手續費稅金"] = autoLoadFee - Convert.ToInt64(Math.Round(autoLoadFee / 1.05, MidpointRounding.AwayFromZero));

                            dt.Rows.Add(pcsc);

                        }

                    }

                    foreach (string[] startEnd in nonSetStartEndList)
                    {
                        DataTable dt0 = new DataTable();

                        SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                        //log.Debug(mNo + " " + startEndList.Where(x => x[0] == mNo).FirstOrDefault()[1]);
                        //log.Debug(mNo + " " + startEndList.Where(x => x[0] == mNo).FirstOrDefault()[2]);
                        sqlCmd.Parameters.Add("@EXEC_CPT_DATE_B", SqlDbType.VarChar).Value = startEnd[1];
                        sqlCmd.Parameters.Add("@EXEC_CPT_DATE_E", SqlDbType.VarChar).Value = startEnd[2];
                        sqlCmd.Parameters.Add("@EXEC_MERCHANT_NO", SqlDbType.VarChar).Value = startEnd[0];

                        SqlDataAdapter adapter = new SqlDataAdapter(sqlCmd);
                        adapter.Fill(dt0);

                        sqlCmd.Dispose();
                        adapter.Dispose();

                        //if (startEnd[0] == "22662550")
                        //{
                        //    foreach(DataRow theRow in dt0.Rows)
                        //    {
                        //        long crfFee = Convert.ToInt64(theRow["消費手續費"]);
                        //        theRow["消費手續費未稅"] = crfFee;
                        //        theRow["消費手續費稅金"] = Convert.ToInt64(Math.Round(crfFee * 0.05, MidpointRounding.AwayFromZero));
                        //    }
                        //}

                        dt.Merge(dt0);

                        
                    }

                    DataColumn col = new DataColumn("Numbering",System.Type.GetType("System.Int32"));
                    dt.Columns.Add(col);
                    col.SetOrdinal(0);

                    int index = 0; 
                    foreach (DataRow row in dt.Rows)
                    {
                        row.SetField("Numbering", ++index);
                    }
                    
                }
            }
            catch (Exception ex)
            {
                log.Debug(ex.Message);
            }
            return dt;
        }
        public DataTable ReportACT170801(string settleDate, string merchant)
        {
            DataTable dt = new DataTable();

            GmSchemaObjDAO schemaDAO = new GmSchemaObjDAO();

            string schema = schemaDAO.FindSchemaObj(merchant).MerchantSchemaId;

            string mName = schemaDAO.FindSchemaObj(merchant).MerchantName;

            string sqlStatement = "";

            using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
            {
                sqlConn.Open();

                string sqlText = string.Format(File.ReadAllText(System.AppDomain.CurrentDomain.BaseDirectory + "\\Sql\\ReportACT\\ReportACT170801_01.sql", Encoding.GetEncoding("big5")), schema, merchant, mName);
                SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                SqlDataReader dr = sqlCmd.ExecuteReader();
                while (dr.Read())
                {
                    sqlStatement = dr["SQL"].ToString();
                }

                sqlConn.Close();
            }      

            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();

                    SqlCommand sqlCmd = new SqlCommand(sqlStatement, sqlConn);
                    sqlCmd.CommandTimeout = 180;
                    sqlCmd.Parameters.Add("@settleDate", SqlDbType.VarChar).Value = settleDate;
                    //sqlCmd.Parameters.Add("@merchant", SqlDbType.Decimal).Value = merchant;

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

        public DataTable ReportACT170802(string startDate,string endDate, string merchant)
        {
            DataTable dt = new DataTable();

            GmSchemaObjDAO schemaDAO = new GmSchemaObjDAO();

            string schema = schemaDAO.FindSchemaObj(merchant).MerchantSchemaId;

            string mName = schemaDAO.FindSchemaObj(merchant).MerchantName;

            string sqlStatement = "";

            using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
            {
                sqlConn.Open();

                string sqlText = string.Format(File.ReadAllText(System.AppDomain.CurrentDomain.BaseDirectory + "\\Sql\\ReportACT\\ReportACT170802_01.sql", Encoding.GetEncoding("big5")), schema, merchant, startDate,endDate);
                SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                SqlDataReader dr = sqlCmd.ExecuteReader();
                while (dr.Read())
                {
                    sqlStatement = dr["SQL"].ToString();
                }

                sqlConn.Close();
            }

            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();
                    //string sqlText = string.Format(File.ReadAllText(System.AppDomain.CurrentDomain.BaseDirectory + "\\Sql\\ReportACT\\ReportACT170802_01.sql", Encoding.GetEncoding("big5")), schema, merchant, mName);
                    SqlCommand sqlCmd = new SqlCommand(sqlStatement, sqlConn);
                    sqlCmd.Parameters.Add("@startDate", SqlDbType.VarChar).Value = startDate;
                    sqlCmd.Parameters.Add("@endDate", SqlDbType.VarChar).Value = endDate;
                    sqlCmd.Parameters.Add("@merchant", SqlDbType.VarChar).Value = merchant;

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
        public DataTable ReportACT180301(string iccNo, string sDate, string merchant)
        {
            DataTable dt1 = new DataTable();
            DataTable dt2 = new DataTable();

            GmSchemaObjDAO schemaDAO = new GmSchemaObjDAO();

            string schema = schemaDAO.FindSchemaObj(merchant).MerchantSchemaId;

            string mName = schemaDAO.FindSchemaObj(merchant).MerchantName;

            string sqlStatement = "";
            string s1 = string.Empty;
            string s2 = string.Empty;
            string s3 = string.Empty;
            string s4 = string.Empty;

            using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
            {
                sqlConn.Open();

                string sqlText1 = string.Format(File.ReadAllText(System.AppDomain.CurrentDomain.BaseDirectory + "\\Sql\\ReportACT\\ReportACT180301_01.sql", Encoding.GetEncoding("big5")), schema, merchant, mName,iccNo,sDate);
                string sqlText2 = string.Format(File.ReadAllText(System.AppDomain.CurrentDomain.BaseDirectory + "\\Sql\\ReportACT\\ReportACT180301_02.sql", Encoding.GetEncoding("big5")), schema, merchant, mName,iccNo,sDate);
                string sqlText3 = string.Format(File.ReadAllText(System.AppDomain.CurrentDomain.BaseDirectory + "\\Sql\\ReportACT\\ReportACT180301_03.sql", Encoding.GetEncoding("big5")), schema, merchant, mName,iccNo,sDate);
                string sqlText4 = string.Format(File.ReadAllText(System.AppDomain.CurrentDomain.BaseDirectory + "\\Sql\\ReportACT\\ReportACT180301_04.sql", Encoding.GetEncoding("big5")), schema, merchant, mName,iccNo,sDate);

                //TMLOG
                SqlCommand sqlCmd = new SqlCommand(sqlText1, sqlConn);
                SqlDataReader dr1 = sqlCmd.ExecuteReader();
                while (dr1.Read())
                {
                    s1 = dr1["SQL"].ToString();
                }
                sqlConn.Close();
                //TXLOG
                sqlConn.Open();
                sqlCmd.CommandText = sqlText2;
                SqlDataReader dr2 = sqlCmd.ExecuteReader();
                while (dr2.Read())
                {
                    s2 = dr2["SQL"].ToString();
                }
                sqlConn.Close();
                //SKIP_TMLOG
                sqlConn.Open();
                sqlCmd.CommandText = sqlText3;
                SqlDataReader dr3 = sqlCmd.ExecuteReader();
                while (dr3.Read())
                {
                    s3 = dr3["SQL"].ToString();
                }
                sqlConn.Close();
                //SKIP_TXLOG
                sqlConn.Open();
                sqlCmd.CommandText = sqlText4;
                SqlDataReader dr4 = sqlCmd.ExecuteReader();
                while (dr4.Read())
                {
                    s4 = dr4["SQL"].ToString();
                }
               
                



                sqlConn.Close();
            }

            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();

                    //TMLOG
                    SqlCommand sqlCmd1 = new SqlCommand(s1, sqlConn);
                    SqlDataAdapter adapter = new SqlDataAdapter(sqlCmd1);
                    adapter.Fill(dt1);
                    sqlConn.Close();

                    //TXLOG
                    sqlConn.Open();
                    SqlCommand sqlCmd2 = new SqlCommand(s2, sqlConn);
                    adapter = new SqlDataAdapter(sqlCmd2);
                    adapter.Fill(dt1);
                    sqlConn.Close();

                    //SKIP_TMLOG
                    sqlConn.Open();
                    SqlCommand sqlCmd3 = new SqlCommand(s3, sqlConn);
                    adapter = new SqlDataAdapter(sqlCmd3);
                    adapter.Fill(dt1);
                    sqlConn.Close();

                    //SKIP_TXLOG
                    sqlConn.Open();
                    SqlCommand sqlCmd4 = new SqlCommand(s4, sqlConn);
                    adapter = new SqlDataAdapter(sqlCmd4);
                    adapter.Fill(dt1);

                    /*sqlCmd1.Parameters.Add("@sDate", SqlDbType.VarChar).Value = sDate;
                    sqlCmd1.Parameters.Add("@eDate", SqlDbType.VarChar).Value = sDate;
                    sqlCmd2.Parameters.Add("@sDate", SqlDbType.VarChar).Value = sDate;
                    sqlCmd2.Parameters.Add("@eDate", SqlDbType.VarChar).Value = sDate;
                    sqlCmd3.Parameters.Add("@sDate", SqlDbType.VarChar).Value = sDate;
                    sqlCmd3.Parameters.Add("@eDate", SqlDbType.VarChar).Value = sDate;
                    sqlCmd4.Parameters.Add("@sDate", SqlDbType.VarChar).Value = sDate;
                    sqlCmd4.Parameters.Add("@eDate", SqlDbType.VarChar).Value = sDate;
                    */

                    /*SqlDataAdapter adapter = new SqlDataAdapter(sqlCmd1);
                    adapter.Fill(dt1);
                    adapter = new SqlDataAdapter(sqlCmd2);
                    adapter.Fill(dt1);
                    adapter = new SqlDataAdapter(sqlCmd3);
                    adapter.Fill(dt1);
                    adapter = new SqlDataAdapter(sqlCmd4);
                    adapter.Fill(dt1);
                    */
                    sqlConn.Close();

                }
            }
            catch (Exception ex)
            {
                log.Debug(ex.Message);
            }
            return dt1;
        }
        public DataTable ReportACT180401(string firstDate, string merchant)
        {
            DataTable dt = new DataTable();
            GmSchemaObjDAO schemaDAO = new GmSchemaObjDAO();
            string schema = schemaDAO.FindSchemaObj(merchant).MerchantSchemaId;
            firstDate = firstDate + "02";
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();
                    string sqlText = string.Format(File.ReadAllText(System.AppDomain.CurrentDomain.BaseDirectory + "\\Sql\\ReportACT\\ReportACT180401_01.sql", Encoding.GetEncoding("big5")), schema);

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    sqlCmd.CommandTimeout = 180;
                    sqlCmd.Parameters.Add("@firstDate", SqlDbType.VarChar).Value = firstDate;
                    sqlCmd.Parameters.Add("@EXEC_MERCHANT_NO", SqlDbType.VarChar).Value = merchant;

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
        public DataTable ReportACT180402(string firstDate, string merchant)
        {
            DataTable dt = new DataTable();
            GmSchemaObjDAO schemaDAO = new GmSchemaObjDAO();
            string schema = schemaDAO.FindSchemaObj(merchant).MerchantSchemaId;
            firstDate = firstDate + "02";
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();
                    string sqlText = string.Format(File.ReadAllText(System.AppDomain.CurrentDomain.BaseDirectory + "\\Sql\\ReportACT\\ReportACT180402_01.sql", Encoding.GetEncoding("big5")), schema);

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    sqlCmd.CommandTimeout = 180;
                    sqlCmd.Parameters.Add("@firstDate", SqlDbType.VarChar).Value = firstDate;
                    sqlCmd.Parameters.Add("@EXEC_MERCHANT_NO", SqlDbType.VarChar).Value = merchant;

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
        public DataTable ReportACT180403(string sDate, string eDate)
        {
            DataTable dt = new DataTable();
            GmSchemaObjDAO schemaDAO = new GmSchemaObjDAO();
            

            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();
                    string sqlText = string.Format(File.ReadAllText(System.AppDomain.CurrentDomain.BaseDirectory + "\\Sql\\ReportACT\\ReportACT180403_01.sql", Encoding.GetEncoding("big5")));

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    sqlCmd.Parameters.Add("@sDate", SqlDbType.VarChar).Value = sDate;
                    sqlCmd.Parameters.Add("@eDate", SqlDbType.VarChar).Value = eDate;
                    

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
        public DataTable ReportACT180701(string sDate, string eDate, string merchant)
        {
            DataTable dt = new DataTable();
            GmSchemaObjDAO schemaDAO = new GmSchemaObjDAO();


            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();
                    string sqlText = string.Format(File.ReadAllText(System.AppDomain.CurrentDomain.BaseDirectory + "\\Sql\\ReportACT\\ReportACT180701_01.sql", Encoding.GetEncoding("big5")));

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    sqlCmd.Parameters.Add("@sDate", SqlDbType.VarChar).Value = sDate;
                    sqlCmd.Parameters.Add("@eDate", SqlDbType.VarChar).Value = eDate;
                    sqlCmd.Parameters.Add("@EXEC_MERCHANT_NO", SqlDbType.VarChar).Value = merchant;

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
        public DataTable ReportACT180702(string sDate, string eDate)
        {
            DataTable dt = new DataTable();
            GmSchemaObjDAO schemaDAO = new GmSchemaObjDAO();


            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();
                    string sqlText = string.Format(File.ReadAllText(System.AppDomain.CurrentDomain.BaseDirectory + "\\Sql\\ReportACT\\ReportACT180702_01.sql", Encoding.GetEncoding("big5")));

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    sqlCmd.Parameters.Add("@SDATE", SqlDbType.VarChar).Value = sDate;
                    sqlCmd.Parameters.Add("@EDATE", SqlDbType.VarChar).Value = eDate;
                    //sqlCmd.Parameters.Add("@EXEC_MERCHANT_NO", SqlDbType.VarChar).Value = merchant;

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
        public DataTable ReportACT181101(string sDate,string eDate, string merchant, string ab_merchant)
        {
            DataTable dt = new DataTable();

            GmSchemaObjDAO schemaDAO = new GmSchemaObjDAO();

            string schema = schemaDAO.FindSchemaObj(ab_merchant).MerchantSchemaId;

            string mName = schemaDAO.FindSchemaObj(ab_merchant).MerchantName;

                       
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();
                    string sqlText = string.Format(File.ReadAllText(System.AppDomain.CurrentDomain.BaseDirectory + "\\Sql\\ReportACT\\ReportACT181101_01.sql", Encoding.GetEncoding("big5")), schema, merchant, mName);
                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    sqlCmd.CommandTimeout = 180;
                    sqlCmd.Parameters.Add("@SDATE", SqlDbType.VarChar).Value = sDate;
                    sqlCmd.Parameters.Add("@EDATE", SqlDbType.VarChar).Value = eDate;
                    //sqlCmd.Parameters.Add("@EXEC_MERCHANT_NO", SqlDbType.VarChar).Value = merchant;
                    sqlCmd.Parameters.Add("@AB_MERCHANT_NO", SqlDbType.VarChar).Value = ab_merchant;
                    sqlCmd.Parameters.Add("@SCHEMA", SqlDbType.VarChar).Value = schema;

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

        public DataTable ReportACT181201(string sDate, string eDate, string ab_merchant)
        {
            GmSchemaObjDAO schemaDAO = new GmSchemaObjDAO();
            string schema = schemaDAO.FindSchemaObj(ab_merchant).MerchantSchemaId;
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();
                    string sqlText = string.Format(File.ReadAllText(System.AppDomain.CurrentDomain.BaseDirectory + "\\Sql\\ReportACT\\ReportACT181201_01.sql", Encoding.GetEncoding("big5")),schema,sDate,eDate);
                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    sqlCmd.CommandTimeout = 180;
                    sqlCmd.Parameters.Add("@SDATE", SqlDbType.VarChar).Value = sDate;
                    sqlCmd.Parameters.Add("@EDATE", SqlDbType.VarChar).Value = eDate;
                    sqlCmd.Parameters.Add("@AB_MERCHANT_NO", SqlDbType.VarChar).Value = ab_merchant;
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
        public DataTable ReportACT181202(string firstDate, string merchant)
        {
            DataTable dt = new DataTable();
            GmSchemaObjDAO schemaDAO = new GmSchemaObjDAO();
            string schema = schemaDAO.FindSchemaObj(merchant).MerchantSchemaId;

            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();
                    string sqlText = string.Format(File.ReadAllText(System.AppDomain.CurrentDomain.BaseDirectory + "\\Sql\\ReportACT\\ReportACT181202_01.sql", Encoding.GetEncoding("big5")), schema, firstDate, merchant);
                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    sqlCmd.CommandTimeout = 180;
                    //sqlCmd.Parameters.Add("@EXEC_CPT_DATE", SqlDbType.VarChar).Value = firstDate;
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
        public DataTable ReportACT181202D(string firstDate, string merchant)
        {
            DataTable dt = new DataTable();
            GmSchemaObjDAO schemaDAO = new GmSchemaObjDAO();
            string schema = schemaDAO.FindSchemaObj(merchant).MerchantSchemaId;

            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();
                    string sqlText = string.Format(File.ReadAllText(System.AppDomain.CurrentDomain.BaseDirectory + "\\Sql\\ReportACT\\ReportACT181202_02.sql", Encoding.GetEncoding("big5")), schema, firstDate, merchant);
                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    sqlCmd.CommandTimeout = 180;
                    //sqlCmd.Parameters.Add("@EXEC_CPT_DATE", SqlDbType.VarChar).Value = firstDate;
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
        public DataTable ReportACT190101(string firstDate, string merchant)
        {
            DataTable dt = new DataTable();
            GmSchemaObjDAO schemaDAO = new GmSchemaObjDAO();
            string schema = schemaDAO.FindSchemaObj(merchant).MerchantSchemaId;

            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();
                    string sqlText = string.Format(File.ReadAllText(System.AppDomain.CurrentDomain.BaseDirectory + "\\Sql\\ReportACT\\ReportACT190101_01.sql", Encoding.GetEncoding("big5")), schema, firstDate, merchant);
                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    //sqlCmd.Parameters.Add("@EXEC_CPT_DATE", SqlDbType.VarChar).Value = firstDate;
                    sqlCmd.CommandTimeout = 180;
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
        public DataTable ReportACT190301(string firstDate, string merchant)
        {
            DataTable dt = new DataTable();
            GmSchemaObjDAO schemaDAO = new GmSchemaObjDAO();
            string schema = schemaDAO.FindSchemaObj(merchant).MerchantSchemaId;

            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();
                    string sqlText = string.Format(File.ReadAllText(System.AppDomain.CurrentDomain.BaseDirectory + "\\Sql\\ReportACT\\ReportACT190301_01.sql", Encoding.GetEncoding("big5")), schema, firstDate, merchant);
                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    sqlCmd.CommandTimeout = 180;
                    sqlCmd.Parameters.Add("@EXEC_CPT_DATE", SqlDbType.VarChar).Value = firstDate;
                    sqlCmd.Parameters.Add("@EXEC_MERCHANT_NO", SqlDbType.VarChar).Value = merchant;
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
        public DataTable ReportACT190701(string sDate, string eDate)
        {
            DataTable dt = new DataTable();
            //GmSchemaObjDAO schemaDAO = new GmSchemaObjDAO();
            //string schema = schemaDAO.FindSchemaObj(merchant).MerchantSchemaId;

            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();
                    string sqlText = string.Format(File.ReadAllText(System.AppDomain.CurrentDomain.BaseDirectory + "\\Sql\\ReportACT\\ReportACT190701_01.sql", Encoding.GetEncoding("big5")),sDate,eDate);
                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    sqlCmd.CommandTimeout = 180;
                    sqlCmd.Parameters.Add("@SDATE", SqlDbType.VarChar).Value = sDate;
                    sqlCmd.Parameters.Add("@EDATE", SqlDbType.VarChar).Value = eDate;
                    //sqlCmd.Parameters.Add("@EXEC_MERCHANT_NO", SqlDbType.VarChar).Value = merchant;
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
        public DataSet ReportACT190901(string sDate, string eDate)
        {
            DataSet ds = new DataSet();
            //GmSchemaObjDAO schemaDAO = new GmSchemaObjDAO();
            //string schema = schemaDAO.FindSchemaObj(merchant).MerchantSchemaId;

            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();
                    string sqlText = string.Format(File.ReadAllText(System.AppDomain.CurrentDomain.BaseDirectory + "\\Sql\\ReportACT\\ReportACT190901_01.sql", Encoding.GetEncoding("big5")), sDate, eDate);
                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    sqlCmd.CommandTimeout = 180;
                    sqlCmd.Parameters.Add("@SDATE", SqlDbType.VarChar).Value = sDate;
                    sqlCmd.Parameters.Add("@EDATE", SqlDbType.VarChar).Value = eDate;
                    //sqlCmd.Parameters.Add("@EXEC_MERCHANT_NO", SqlDbType.VarChar).Value = merchant;
                    SqlDataAdapter adapter = new SqlDataAdapter(sqlCmd);
                    adapter.Fill(ds);

                    sqlConn.Close();

                }
            }
            catch (Exception ex)
            {
                log.Debug(ex.Message);
            }
            return ds;
        }

        public DataSet ReportACT191001(string yearmonth)
        {
            DataSet ds = new DataSet();
            //GmSchemaObjDAO schemaDAO = new GmSchemaObjDAO();
            //string schema = schemaDAO.FindSchemaObj(merchant).MerchantSchemaId;

            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();
                    string sqlText = string.Format(File.ReadAllText(System.AppDomain.CurrentDomain.BaseDirectory + "\\Sql\\ReportACT\\ReportACT191001_01.sql", Encoding.GetEncoding("big5")), yearmonth);
                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    sqlCmd.CommandTimeout = 180;
                    sqlCmd.Parameters.Add("@EXEC_CPT_DATE", SqlDbType.VarChar).Value = yearmonth;
                    SqlDataAdapter adapter = new SqlDataAdapter(sqlCmd);
                    adapter.Fill(ds);

                    sqlConn.Close();

                }
            }
            catch (Exception ex)
            {
                log.Debug(ex.Message);
            }
            return ds;
        }

        //布萊恩增加依清分日
        public DataTable ReportACT190701_02(string sDate, string eDate)
        {
            DataTable dt = new DataTable();
            //GmSchemaObjDAO schemaDAO = new GmSchemaObjDAO();
            //string schema = schemaDAO.FindSchemaObj(merchant).MerchantSchemaId;

            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();
                    string sqlText = string.Format(File.ReadAllText(System.AppDomain.CurrentDomain.BaseDirectory + "\\Sql\\ReportACT\\ReportACT190701_02.sql", Encoding.GetEncoding("big5")), sDate, eDate);
                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    sqlCmd.CommandTimeout = 180;
                    sqlCmd.Parameters.Add("@SDATE", SqlDbType.VarChar).Value = sDate;
                    sqlCmd.Parameters.Add("@EDATE", SqlDbType.VarChar).Value = eDate;
                    //sqlCmd.Parameters.Add("@EXEC_MERCHANT_NO", SqlDbType.VarChar).Value = merchant;
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
        //02
        public DataTable ReportACT181101_2(string sDate, string eDate, string merchant, string ab_merchant)
        {
            DataTable dt = new DataTable();

            GmSchemaObjDAO schemaDAO = new GmSchemaObjDAO();

            string schema = schemaDAO.FindSchemaObj(ab_merchant).MerchantSchemaId;

            string mName = schemaDAO.FindSchemaObj(ab_merchant).MerchantName;


            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();
                    string sqlText = string.Format(File.ReadAllText(System.AppDomain.CurrentDomain.BaseDirectory + "\\Sql\\ReportACT\\ReportACT181101_02.sql", Encoding.GetEncoding("big5")), schema, merchant, mName);
                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    sqlCmd.CommandTimeout = 180;
                    sqlCmd.Parameters.Add("@SDATE", SqlDbType.VarChar).Value = sDate;
                    sqlCmd.Parameters.Add("@EDATE", SqlDbType.VarChar).Value = eDate;
                    //sqlCmd.Parameters.Add("@EXEC_MERCHANT_NO", SqlDbType.VarChar).Value = merchant;
                    sqlCmd.Parameters.Add("@AB_MERCHANT_NO", SqlDbType.VarChar).Value = ab_merchant;
                    sqlCmd.Parameters.Add("@SCHEMA", SqlDbType.VarChar).Value = schema;

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
