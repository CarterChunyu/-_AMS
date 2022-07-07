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
    public class AmMenuDAO : BasicDAL, ICloneable
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(AmMenuDAO));
        public string TableName { private get; set; }

        public AmMenuDAO()
        {
            this.TableName = "AM_MENU";
        }

        public AmMenu FindByPk(int id)
        {
            DataTable dt = new DataTable();
            AmMenu entity = new AmMenu();
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();
                    string sqlText = @"select
                                Id 
                                ,Name
                                ,Path
                                ,Parent_Id
                                ,Status
                                ,Rank
                                ,Roles
                            from 
                              " + this.TableName + @" 
                            where
                                Id = @Id";

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    sqlCmd.Parameters.Add("@Id", SqlDbType.Int).Value = id;

                    SqlDataAdapter adapter = new SqlDataAdapter(sqlCmd);

                    adapter.Fill(dt);
                    entity.Id = Int32.Parse(dt.Rows[0]["Id"].ToString());
                    entity.Name = dt.Rows[0]["Name"].ToString();
                    entity.Path = dt.Rows[0]["Path"].ToString();
                    entity.ParentId = Int32.Parse(dt.Rows[0]["Parent_Id"].ToString());
                    entity.Status = dt.Rows[0]["Status"].ToString();
                    entity.Roles = dt.Rows[0]["Roles"].ToString();
                    entity.Rank = Int32.Parse(dt.Rows[0]["Rank"].ToString());
                }
            }
            catch (Exception ex)
            {
                log.Debug(ex.StackTrace);
                log.Debug(ex.Message);

                return null;
            }
            return entity;
        }

        public void Insert(AmMenu entity)
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
                                    [Name]
                                    ,[Path]
                                    ,[Parent_Id]
                                    ,[Status]
                                    ,[Rank]
                                    ,[Roles]
                                    )
                            VALUES
                                   (
                                    @Name
                                    ,@Path
                                    ,@Parent_Id
                                    ,@Status
                                    ,@Rank
                                    ,@Roles
                                   )";

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    sqlCmd.Parameters.Add("@Name", SqlDbType.NVarChar).Value = entity.Name;
                    sqlCmd.Parameters.Add("@Path", SqlDbType.NVarChar).Value = entity.Path;
                    sqlCmd.Parameters.Add("@Parent_Id", SqlDbType.Int).Value = entity.ParentId;
                    sqlCmd.Parameters.Add("@Roles", SqlDbType.VarChar).Value = entity.Roles;
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

        public void Update(AmMenu entity)
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
                          ,[Path] = @Path
                          ,[Parent_Id] = @Parent_Id
                          ,[Status] = @Status
                          ,[Rank] = @Rank
                          ,[Roles] = @Roles
                       WHERE
                            [Id] = @Id";

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    sqlCmd.Parameters.Add("@Id", SqlDbType.Int).Value = entity.Id;
                    sqlCmd.Parameters.Add("@Name", SqlDbType.NVarChar).Value = entity.Name;
                    sqlCmd.Parameters.Add("@Path", SqlDbType.NVarChar).Value = entity.Path;
                    sqlCmd.Parameters.Add("@Parent_Id", SqlDbType.Int).Value = entity.ParentId;
                    sqlCmd.Parameters.Add("@Roles", SqlDbType.VarChar).Value = entity.Roles;
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

        public void Delete(int id)
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
                            [Id] = @Id";

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    sqlCmd.Parameters.Add("@Id", SqlDbType.Int).Value = id;

                    sqlCmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                log.Debug(ex.StackTrace);
                log.Debug(ex.Message);
            }
        }

        public void DeleteByPid(int pid)
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
                            [Parent_Id] = @Parent_Id";

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    sqlCmd.Parameters.Add("@Parent_Id", SqlDbType.Int).Value = pid;

                    sqlCmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                log.Debug(ex.StackTrace);
                log.Debug(ex.Message);
            }
        }

        public bool Exist(int id)
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
                              Id = @Id
                         ";
                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    sqlCmd.Parameters.Add("@Id", SqlDbType.Int).Value = id;

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

        public List<AmMenu> FindByRoleStatusPid(string role, string status, int pid)
        {
            List<AmMenu> objList = new List<AmMenu>();
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();
                    string sqlText = @"
                            select
                                Id 
                                ,Name
                                ,Path
                                ,Parent_Id
                                ,Status
                                ,Rank
                                ,Roles
                from " + this.TableName + @" 
                where
                     Roles Like @Role 
                     and Status = @Status 
                     and Parent_Id = @Parent_Id 
                order by Rank
                ";

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    sqlCmd.Parameters.AddWithValue("@Role", String.Format("%{0}%", "," + role + ","));
                    sqlCmd.Parameters.Add("@Status", SqlDbType.VarChar).Value = status;
                    sqlCmd.Parameters.Add("@Parent_Id", SqlDbType.Int).Value = pid;

                    SqlDataReader dr = sqlCmd.ExecuteReader();
                    while (dr.Read())
                    {
                        AmMenu obj = new AmMenu();
                        obj.Id = Int32.Parse(dr["Id"].ToString());
                        obj.Name = dr["Name"].ToString();
                        obj.Path = dr["Path"].ToString();
                        obj.ParentId = Int32.Parse(dr["Parent_Id"].ToString());
                        obj.Roles = dr["Roles"].ToString();
                        obj.Status = dr["Status"].ToString();
                        obj.Rank = Int32.Parse(dr["Rank"].ToString());
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

        public List<AmMenu> FindByPid(int pid)
        {
            List<AmMenu> objList = new List<AmMenu>();
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();
                    string sqlText = @"
                            select
                                Id 
                                ,Name
                                ,Path
                                ,Parent_Id
                                ,Status
                                ,Rank
                                ,Roles
                from " + this.TableName + @" 
                where
                     Parent_Id = @Parent_Id  
                order by Rank
                ";

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    sqlCmd.Parameters.Add("@Parent_Id", SqlDbType.Int).Value = pid;

                    SqlDataReader dr = sqlCmd.ExecuteReader();
                    while (dr.Read())
                    {
                        AmMenu obj = new AmMenu();
                        obj.Id = Int32.Parse(dr["Id"].ToString());
                        obj.Name = dr["Name"].ToString();
                        obj.Path = dr["Path"].ToString();
                        obj.ParentId = Int32.Parse(dr["Parent_Id"].ToString());
                        obj.Roles = dr["Roles"].ToString();
                        obj.Status = dr["Status"].ToString();
                        obj.Rank = Int32.Parse(dr["Rank"].ToString());
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

        public List<AmMenu> FindByPidm(int pid)
        {
            List<AmMenu> objList = new List<AmMenu>();
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();
                    string sqlText = @"
                            select
                                Id 
                                ,Name
                                ,Path
                                ,Parent_Id
                                ,Status
                                ,Rank
                                ,Roles
                from " + this.TableName + @" 
                where
                     Parent_Id = @Parent_Id or Path='##' 
                order by Rank
                ";

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    sqlCmd.Parameters.Add("@Parent_Id", SqlDbType.Int).Value = pid;

                    SqlDataReader dr = sqlCmd.ExecuteReader();
                    while (dr.Read())
                    {
                        AmMenu obj = new AmMenu();
                        obj.Id = Int32.Parse(dr["Id"].ToString());
                        obj.Name = dr["Name"].ToString();
                        obj.Path = dr["Path"].ToString();
                        obj.ParentId = Int32.Parse(dr["Parent_Id"].ToString());
                        obj.Roles = dr["Roles"].ToString();
                        obj.Status = dr["Status"].ToString();
                        obj.Rank = Int32.Parse(dr["Rank"].ToString());
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

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
