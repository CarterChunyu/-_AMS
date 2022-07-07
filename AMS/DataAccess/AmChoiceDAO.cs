using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using System.Data;
using System.Data.SqlClient;
using Common.Logging;

namespace DataAccess
{
    public class AmChoiceDAO :BasicDAL, ICloneable
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(AmChoiceDAO));
        public string TableName { private get; set; }

        public AmChoiceDAO()
        {
            this.TableName = "AM_CHOICE";
        }

        public List<AmChoice> FindByCode(string code)
        {
            List<AmChoice> objList = new List<AmChoice>();
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();
                    string sqlText = @"
                select
                        Code
                        ,Value
                        ,Name
                        ,Remark
                        ,Status
                        ,Rank
                from " + this.TableName + @" 
                where
                     Code = @Code 
                 and Status = @Status 
                order by Rank 
                ";


                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    sqlCmd.Parameters.Add("@Code", SqlDbType.VarChar).Value = code;
                    sqlCmd.Parameters.Add("@Status", SqlDbType.VarChar).Value = "ONLINE";

                    SqlDataReader dr = sqlCmd.ExecuteReader();
                    while (dr.Read())
                    {
                        AmChoice obj = new AmChoice();
                        obj.Code = dr["Code"].ToString();
                        obj.Value = dr["Value"].ToString();
                        obj.Name = dr["Name"].ToString();
                        obj.Remark = dr["Remark"].ToString();
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

        public AmChoice FindByCodeValue(string code, string value)
        {
            DataTable dt = new DataTable();
            AmChoice obj = new AmChoice();

            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();

                    string sqlText = @"select
                        Code
                        ,Value
                        ,Name
                        ,Remark
                        ,Status
                        ,Rank
                            from 
                              " + this.TableName + @" 
                            where
                                Code = @Code and
                                Value = @Value";

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    sqlCmd.Parameters.Add("@Code", SqlDbType.VarChar).Value = code;
                    sqlCmd.Parameters.Add("@Value", SqlDbType.VarChar).Value = value;

                    SqlDataAdapter adapter = new SqlDataAdapter(sqlCmd);

                    adapter.Fill(dt);

                    obj.Code = dt.Rows[0]["Code"].ToString();
                    obj.Value = dt.Rows[0]["Value"].ToString();
                    obj.Name = dt.Rows[0]["Name"].ToString();
                    obj.Remark = dt.Rows[0]["Remark"].ToString();
                }
            }
            catch (Exception ex)
            {
                log.Debug("Query Ex: " + this.TableName);////
                log.Debug(ex.StackTrace);
                log.Debug(ex.Message);
                return null;
            }

            return obj;
        }

        public void Delete(string choiceCode, string choiceValue)
        {
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();

                    string sqlText = @"
                       DELETE from 
                        " + this.TableName + @"
                       WHERE
                            [Code] = @Code and [Value] = @Value";

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    sqlCmd.Parameters.Add("@Code", SqlDbType.VarChar).Value = choiceCode;
                    sqlCmd.Parameters.Add("@Value", SqlDbType.VarChar).Value = choiceValue;

                    sqlCmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                log.Debug(ex.StackTrace);
                log.Debug(ex.Message);
            }
        }

        public List<AmChoice> FindByCodeAll(string code)
        {
            List<AmChoice> objList = new List<AmChoice>();
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();
                    string sqlText = @"
                select
                        Code
                        ,Value
                        ,Name
                        ,Remark
                        ,Status
                        ,Rank
                from " + this.TableName + @" 
                where
                     Code = @Code 
                order by Value  
                ";


                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    sqlCmd.Parameters.Add("@Code", SqlDbType.VarChar).Value = code;

                    SqlDataReader dr = sqlCmd.ExecuteReader();
                    while (dr.Read())
                    {
                        AmChoice obj = new AmChoice();
                        obj.Code = dr["Code"].ToString();
                        obj.Value = dr["Value"].ToString();
                        obj.Name = dr["Name"].ToString();
                        obj.Remark = dr["Remark"].ToString();
                        obj.Status = dr["Status"].ToString();
                        obj.Rank = Int32.Parse(dr["Rank"].ToString());
                        objList.Add(obj);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Debug(ex.Message);
                objList = null;
            }
            return objList;
        }

        public List<AmChoice> FindByCodeSpecial(string code, List<string> selections)
        {
            List<AmChoice> objList = new List<AmChoice>();
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();
                    string sqlText = @"
                select
                        Code
                        ,Value
                        ,Name
                        ,Remark
                        ,Status
                        ,Rank
                from " + this.TableName + @" 
                where
                     Code = @Code and Status = @Status and Value in ({0}) 
                order by Rank 
                ";


                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    sqlCmd.Parameters.Add("@Code", SqlDbType.VarChar).Value = code;
                    sqlCmd.Parameters.Add("@Status", SqlDbType.VarChar).Value = "ONLINE";

                    //var nameList = new List<String> { "0", "1" };
                    var nameParameter = new List<string>();
                    var index = 0;
                    foreach (var name in selections)
                    {
                        var paramName = "@nameParam" + index;
                        sqlCmd.Parameters.AddWithValue(paramName, name);
                        nameParameter.Add(paramName);
                        index++;
                    }

                    sqlCmd.CommandText = String.Format(sqlText, string.Join(",", nameParameter));

                    SqlDataReader dr = sqlCmd.ExecuteReader();
                    while (dr.Read())
                    {
                        AmChoice obj = new AmChoice();
                        obj.Code = dr["Code"].ToString();
                        obj.Value = dr["Value"].ToString();
                        obj.Name = dr["Name"].ToString();
                        obj.Remark = dr["Remark"].ToString();
                        objList.Add(obj);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Debug(ex.Message);
                objList = null;
            }
            return objList;
        }

        public AmChoice FindByCodeName(string code, string name)
        {
            DataTable dt = new DataTable();
            AmChoice obj = new AmChoice();

            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();

                    string sqlText = @"select
                        Code
                        ,Value
                        ,Name
                        ,Remark
                        ,Status
                        ,Rank
                            from 
                              " + this.TableName + @" 
                            where
                                Code = @Code and
                                Name = @Name";

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    sqlCmd.Parameters.Add("@Code", SqlDbType.VarChar).Value = code;
                    sqlCmd.Parameters.Add("@Name", SqlDbType.VarChar).Value = name;

                    SqlDataAdapter adapter = new SqlDataAdapter(sqlCmd);

                    adapter.Fill(dt);

                    obj.Code = dt.Rows[0]["Code"].ToString();
                    obj.Value = dt.Rows[0]["Value"].ToString();
                    obj.Name = dt.Rows[0]["Name"].ToString();
                    obj.Remark = dt.Rows[0]["Remark"].ToString();
                }
            }
            catch (Exception ex)
            {
                log.Debug(ex.Message);
                return null;
            }

            return obj;
        }

        public List<AmChoice> FindCodeList()
        {
            List<AmChoice> objList = new List<AmChoice>();
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();
                    string sqlText = @"
                select distinct Code 
                from " + this.TableName + @" 
                order by Code 
                ";

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);

                    SqlDataReader dr = sqlCmd.ExecuteReader();
                    while (dr.Read())
                    {
                        AmChoice obj = new AmChoice();
                        obj.Code = dr["Code"].ToString();
                        objList.Add(obj);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Debug(ex.Message);
                objList = null;
            }
            return objList;
        }

        public void Insert(AmChoice entity)
        {
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();

                    string sqlText =
                        @"INSERT INTO "
                        + this.TableName + @"
                                   (
                                    [Code]
                                   ,[Value]
                                   ,[Name]
                                   ,[Remark]
                                   ,[Status]
                                   ,[Rank]
                                    )
                            VALUES
                                   (
                                    @Code
                                   ,@Value
                                   ,@Name
                                   ,@Remark
                                   ,@Status
                                   ,@Rank
                                    )";

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    sqlCmd.Parameters.Add("@Code", SqlDbType.VarChar).Value = entity.Code;
                    sqlCmd.Parameters.Add("@Value", SqlDbType.VarChar).Value = entity.Value;
                    sqlCmd.Parameters.Add("@Name", SqlDbType.VarChar).Value = entity.Name;
                    sqlCmd.Parameters.Add("@Remark", SqlDbType.VarChar).Value = entity.Remark;
                    sqlCmd.Parameters.Add("@Status", SqlDbType.VarChar).Value = entity.Status;
                    sqlCmd.Parameters.Add("@Rank", SqlDbType.Int).Value = entity.Rank;
                    sqlCmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                log.Debug(ex.StackTrace);
                log.Debug(ex.Message);
            }
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
