using Common.Logging;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class AmUsersDAO : BasicDAL
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(AmUsersDAO));
        public string TableName { private get; set; }

        public AmUsersDAO()
        {
            this.TableName = "AM_USERS";
        }

        public void Insert(AmUsers entity)
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
                                    [Username]                                  
                                   ,[Password]
                                   ,[Name]
                                   ,[Email]
                                   ,[Role]
                                   ,[Status]
                                   ,[Opid]
                                   ,[Update_Date]
                                    )
                            VALUES
                                   (
                                    @Username
                                   ,@Password
                                   ,@Name
                                   ,@Email
                                   ,@Role
                                   ,@Status
                                   ,@Opid
                                   ,@Update_Date
                                    )

                            INSERT INTO AM_USERS_PASSWORD_LOG
                            SELECT @Username,@Password,@Update_Date  
                            INSERT INTO AM_USERS_PASSWORD_LOG
                            SELECT @Username,'','2000-01-01 00:00:00.000'  
                            INSERT INTO AM_USERS_PASSWORD_LOG
                            SELECT @Username,'','2001-04-01 00:00:00.000'  
                            INSERT INTO AM_USERS_PASSWORD_LOG
                            SELECT @Username,'','2002-07-01 00:00:00.000'  ";

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    sqlCmd.Parameters.Add("@Username", SqlDbType.VarChar).Value = entity.Username;
                    sqlCmd.Parameters.Add("@Password", SqlDbType.NVarChar).Value = Helpers.SHA1.Encode(entity.Password);
                    sqlCmd.Parameters.Add("@Name", SqlDbType.NVarChar).Value = entity.Name;
                    sqlCmd.Parameters.Add("@Email", SqlDbType.VarChar).Value = entity.Email;
                    sqlCmd.Parameters.Add("@Role", SqlDbType.VarChar).Value = entity.Role;
                    sqlCmd.Parameters.Add("@Status", SqlDbType.VarChar).Value = entity.Status;
                    sqlCmd.Parameters.Add("@Opid", SqlDbType.VarChar).Value = entity.Opid;
                    sqlCmd.Parameters.Add("@Update_Date", SqlDbType.DateTime).Value = DateTime.Now;

                    sqlCmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                log.Debug(ex.StackTrace);
                log.Debug(ex.Message);
            }
        }

        public AmUsers FindByPk(string username)
        {
            DataTable dt = new DataTable();
            AmUsers obj = null;

            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();

                    string sqlText = @"select
                                Name
                                ,Username
                                ,Password
                                ,Email
                                ,Reg_Date
                                ,Role
                                ,Status
                                ,Update_Date
                            from 
                              " + this.TableName + @" 
                            where
                                Username = @Username ";

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    sqlCmd.Parameters.Add(new SqlParameter("@Username", SqlDbType.NVarChar)).Value = username;

                    SqlDataAdapter adapter = new SqlDataAdapter(sqlCmd);

                    adapter.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        obj = new AmUsers();
                        obj.Name = dt.Rows[0]["Name"].ToString();
                        obj.Username = dt.Rows[0]["Username"].ToString();
                        obj.Email = dt.Rows[0]["Email"].ToString();
                        obj.Role = dt.Rows[0]["Role"].ToString();
                        obj.Status = dt.Rows[0]["Status"].ToString();
                        if (!DBNull.Value.Equals(dt.Rows[0]["Reg_Date"]))
                        {
                            obj.RegDate = Convert.ToDateTime(dt.Rows[0]["Reg_Date"]);
                        }
                        if (!DBNull.Value.Equals(dt.Rows[0]["Update_Date"]))
                        {
                            obj.UpdateDate = Convert.ToDateTime(dt.Rows[0]["Update_Date"]);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Debug(ex.StackTrace);
                log.Debug(ex.Message);
                return null;
            }

            return obj;
        }

        public AmUsers FindByAuth(string username)
        {
            AmUsers obj = null;

            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();

                    string sqlText = @"select
                                Name
                                ,Username
                                ,Password
                                ,Email
                                ,Reg_Date
                                ,Role
                                ,Status
                                ,Update_Date
                            from 
                              " + this.TableName + @" 
                            where
                                Username = @Username  
                                and  Status = @Status";

                    using(SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn))
                    {
                        sqlCmd.Parameters.Add(new SqlParameter("@Username", SqlDbType.NVarChar)).Value = username;
                        sqlCmd.Parameters.Add("@Status", SqlDbType.VarChar).Value = "ONLINE";

                        using(SqlDataReader dr = sqlCmd.ExecuteReader())
                        {
                            while(dr.Read())
                            {
                                obj          = new AmUsers();
                                obj.Name     = dr["Name"] +"";
                                obj.Username = dr["Username"] + "";
                                obj.Email    = dr["Email"] + "";
                                obj.Role     = dr["Role"] + "";
                                obj.Status   = dr["Status"] + "";
                                if (!DBNull.Value.Equals(dr["Reg_Date"]))
                                {
                                    obj.RegDate = Convert.ToDateTime(dr["Reg_Date"]);
                                }
                                if (!DBNull.Value.Equals(dr["Update_Date"]))
                                {
                                    obj.UpdateDate = Convert.ToDateTime(dr["Update_Date"]);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Debug(ex.StackTrace);
                log.Debug(ex.Message);
                return null;
            }
            return obj;
        }

        public AmUsers FindByAuth(string username, string password)
        {
            DataTable dt = new DataTable();
            AmUsers obj = null;

            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();

                    string sqlText = @"select
                                Name
                                ,Username
                                ,Password
                                ,Email
                                ,Reg_Date
                                ,Role
                                ,Status
                                ,Update_Date
                            from 
                              " + this.TableName + @" 
                            where
                                Username = @Username  
                                and  Password = @Password 
                                and  Status = @Status";

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    sqlCmd.Parameters
                    .Add(new SqlParameter("@Username", SqlDbType.NVarChar))
                    .Value = username;
                    string encode_str = Helpers.SHA1.Encode(password);
                    sqlCmd.Parameters.Add(new SqlParameter("@Password", SqlDbType.NVarChar)).Value = encode_str;
                    sqlCmd.Parameters.Add("@Status", SqlDbType.VarChar).Value = "ONLINE";

                    SqlDataAdapter adapter = new SqlDataAdapter(sqlCmd);

                    adapter.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        obj = new AmUsers();
                        obj.Name = dt.Rows[0]["Name"].ToString();
                        obj.Username = dt.Rows[0]["Username"].ToString();
                        obj.Email = dt.Rows[0]["Email"].ToString();
                        obj.Role = dt.Rows[0]["Role"].ToString();
                        obj.Status = dt.Rows[0]["Status"].ToString();
                        if (!DBNull.Value.Equals(dt.Rows[0]["Reg_Date"]))
                        {
                            obj.RegDate = Convert.ToDateTime(dt.Rows[0]["Reg_Date"]);
                        }
                        if (!DBNull.Value.Equals(dt.Rows[0]["Update_Date"]))
                        {
                            obj.UpdateDate = Convert.ToDateTime(dt.Rows[0]["Update_Date"]);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Debug(ex.StackTrace);
                log.Debug(ex.Message);
                return null;
            }

            return obj;
        }

        public List<AmUsers> FindByStatus(string status)
        {
            List<AmUsers> objList = new List<AmUsers>();
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();
                    string sqlText = @" select
                                Name
                                ,Username
                                ,Password
                                ,Email
                                ,Reg_Date
                                ,Role
                                ,Status
                                ,Update_Date
                from " + this.TableName + @" 
                where
                     Status = @Status 
                order by Username 
                ";

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    sqlCmd.Parameters.Add("@Status", SqlDbType.VarChar).Value = status;
                    SqlDataReader dr = sqlCmd.ExecuteReader();
                    while (dr.Read())
                    {
                        AmUsers obj = new AmUsers();
                        obj.Username = dr["Username"].ToString();
                        obj.Name = dr["Name"].ToString();
                        obj.Email = dr["Email"].ToString();
                        obj.Role = dr["Role"].ToString();
                        obj.Status = dr["Status"].ToString();
                        if (!DBNull.Value.Equals(dr["Reg_Date"]))
                        {
                            obj.RegDate = Convert.ToDateTime(dr["Reg_Date"]);
                        }
                        if (!DBNull.Value.Equals(dr["Update_Date"]))
                        {
                            obj.UpdateDate = Convert.ToDateTime(dr["Update_Date"]);
                        }
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
            //log.Debug("Query End: " + this.TableName);////
            return objList;
        }

        public List<String> GetUserRights(string username)
        {
            string userRoleTable = "User_Role";
            List<string> roleCollection = new List<string>();
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();
                    string sqlText = @" select
                                Role_Id
                from " + userRoleTable + @" 
                where
                     Username = @Username 
                ";

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    sqlCmd.Parameters.Add("@Username", SqlDbType.VarChar).Value = username;
                    SqlDataReader dr = sqlCmd.ExecuteReader();
                    while (dr.Read())
                    {
                        roleCollection.Add(dr["Role_Id"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                log.Debug("Query Ex: " + this.TableName);////
                log.Debug(ex.StackTrace);
                log.Debug(ex.Message);
                roleCollection = null;
            }
            //log.Debug("Query End: " + this.TableName);////
            return roleCollection;
        }

        public void Update(AmUsers entity)
        {
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();

                    string sqlText = @"
                       UPDATE "
                            + this.TableName + @"
                       SET 
                           [Name] = @Name
                          ,[Email] = @Email
                          ,[Role] = @Role
                          ,[Status] = @Status
                          ,[Opid] = @Opid
                          ,[Update_Date] = @Update_Date
                       WHERE
                            [Username] = @Username";

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    sqlCmd.Parameters.Add("@Username", SqlDbType.VarChar).Value = entity.Username;
                    sqlCmd.Parameters.Add("@Name", SqlDbType.NVarChar).Value = entity.Name;
                    sqlCmd.Parameters.Add("@Email", SqlDbType.NVarChar).Value = entity.Email;
                    sqlCmd.Parameters.Add("@Role", SqlDbType.VarChar).Value = entity.Role;
                    sqlCmd.Parameters.Add("@Status", SqlDbType.VarChar).Value = entity.Status;
                    sqlCmd.Parameters.Add("@Opid", SqlDbType.VarChar).Value = entity.Opid;
                    sqlCmd.Parameters.Add("@Update_Date", SqlDbType.DateTime).Value = DateTime.Now;

                    sqlCmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                log.Debug(ex.StackTrace);
                log.Debug(ex.Message);
            }
        }

        public int UpdatePassword(string username, string password, string opid)
        {
            int affRows = 0;
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();

                    string sqlText = @"
                       UPDATE "
                            + this.TableName + @"
                       SET 
                           [Password] = @Password
                          ,[Opid] = @Opid
                          ,[Update_Date] = @Update_Date
                       WHERE
                            [Username] = @Username";

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    sqlCmd.Parameters.Add("@Username", SqlDbType.VarChar).Value = username;
                    sqlCmd.Parameters.Add("@Password", SqlDbType.NVarChar).Value = Helpers.SHA1.Encode(password);
                    sqlCmd.Parameters.Add("@Opid", SqlDbType.VarChar).Value = opid;
                    sqlCmd.Parameters.Add("@Update_Date", SqlDbType.DateTime).Value = DateTime.Now;

                    affRows = sqlCmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                log.Debug(ex.StackTrace);
                log.Debug(ex.Message);
            }
            return affRows;
        }

        public void UpdateRegDate(string username,DateTime? datetime)
        {
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();

                    string sqlText = @"
                       UPDATE "
                            + this.TableName + @"
                       SET 
                          [Reg_Date] = @Reg_Date
                       WHERE
                            [Username] = @Username";

                    using (SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn))
                    {
                        sqlCmd.Parameters.Add("@Username", SqlDbType.NVarChar).Value = username;
                        sqlCmd.Parameters.Add("@Reg_Date", SqlDbType.DateTime).Value = ConvertValue(datetime);
                        sqlCmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Query Ex:" + this.TableName);
                log.Error(ex.StackTrace);
                log.Error(ex.Message);
            }
        }

        public void Delete(string username)
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
                            [Username] = @Username";

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    sqlCmd.Parameters.Add("@Username", SqlDbType.VarChar).Value = username;

                    sqlCmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                log.Debug(ex.StackTrace);
                log.Debug(ex.Message);
            }
        }

        public bool Exist(string username)
        {
            Int32 count = 0;
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();
                    string sqlText =
                            @"select 
                              count(*)
                          from 
                              " + this.TableName + @" 
                          where
                              Username = @Username
                         ";
                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    sqlCmd.Parameters.Add("@Username", SqlDbType.VarChar).Value = username;

                    count = (Int32)sqlCmd.ExecuteScalar();
                }
            }
            catch (Exception ex)
            {
                log.Debug("Query Ex: " + this.TableName);////
                log.Debug(ex.StackTrace);
                log.Debug(ex.Message);
            }
            return (count > 0);
        }
        public AmUsers FindHistory(string username, string password)
        {
            DataTable dt = new DataTable();
            AmUsers obj = null;

            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();

                    string sqlText = @"
								 SELECT A.Name,
										A.Username,
										B.Password,
										A.Email,
										A.Reg_Date,
										A.Role,
										A.Status,
										B.Update_Date
								FROM AM_USERS A
								JOIN AM_USERS_PASSWORD_LOG B
								ON A.Username = B.Username
								WHERE A.Username = @Username   
								AND B.Password = @Password ";

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    sqlCmd.Parameters
                    .Add(new SqlParameter("@Username", SqlDbType.NVarChar))
                    .Value = username;
                    string encode_str = Helpers.SHA1.Encode(password);
                    sqlCmd.Parameters.Add(new SqlParameter("@Password", SqlDbType.NVarChar)).Value = encode_str;
                    SqlDataAdapter adapter = new SqlDataAdapter(sqlCmd);

                    adapter.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        obj = new AmUsers();
                        obj.Name = dt.Rows[0]["Name"].ToString();
                        obj.Username = dt.Rows[0]["Username"].ToString();
                        obj.Email = dt.Rows[0]["Email"].ToString();
                        obj.Role = dt.Rows[0]["Role"].ToString();
                        obj.Status = dt.Rows[0]["Status"].ToString();
                        obj.Password = dt.Rows[0]["Password"].ToString();
                        if (!DBNull.Value.Equals(dt.Rows[0]["Reg_Date"]))
                        {
                            obj.RegDate = Convert.ToDateTime(dt.Rows[0]["Reg_Date"]);
                        }
                        if (!DBNull.Value.Equals(dt.Rows[0]["Update_Date"]))
                        {
                            obj.UpdateDate = Convert.ToDateTime(dt.Rows[0]["Update_Date"]);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Debug(ex.StackTrace);
                log.Debug(ex.Message);
                return null;
            }

            return obj;
        }
        public int UpdatePasswordLog(string username, string password, string opid)
        {
            int affRows = 0;
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();

                    string sqlText = @"
                        UPDATE AM_USERS_PASSWORD_LOG
                        SET Update_Date = @Update_Date ,Password = @Password
                        WHERE Update_Date = (
                        SELECT MIN(Update_Date) FROM AM_USERS_PASSWORD_LOG
                        where Username =  @Username
                        ) AND Username =  @Username
			            ";

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    sqlCmd.Parameters.Add("@Username", SqlDbType.VarChar).Value = username;
                    sqlCmd.Parameters.Add("@Password", SqlDbType.NVarChar).Value = Helpers.SHA1.Encode(password);
                    sqlCmd.Parameters.Add("@Opid", SqlDbType.VarChar).Value = opid;
                    sqlCmd.Parameters.Add("@Update_Date", SqlDbType.DateTime).Value = DateTime.Now;

                    affRows = sqlCmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                log.Debug(ex.StackTrace);
                log.Debug(ex.Message);
            }
            return affRows;
        }

    }
}
