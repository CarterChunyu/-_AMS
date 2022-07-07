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
    public class AmRolesDAO : BasicDAL, ICloneable
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(AmRolesDAO));
        public string TableName { private get; set; }

        public AmRolesDAO()
        {
            this.TableName = "AM_ROLES";
        }

        public void Insert(AmRoles entity)
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
                                    [Role_Id]                                  
                                   ,[Role_Name]
                                    )
                            VALUES
                                   (
                                    @Role_Id
                                   ,@Role_Name
                                    )";

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    sqlCmd.Parameters.Add("@Role_Id", SqlDbType.VarChar).Value = entity.RoleId;
                    sqlCmd.Parameters.Add("@Role_Name", SqlDbType.NVarChar).Value = entity.RoleName;

                    sqlCmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                log.Debug(ex.StackTrace);
                log.Debug(ex.Message);
            }
        }

        public void Delete(string roleId)
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
                            [Role_Id] = @Role_Id";

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    sqlCmd.Parameters.Add("@Role_Id", SqlDbType.VarChar).Value = roleId;

                    sqlCmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                log.Debug(ex.StackTrace);
                log.Debug(ex.Message);
            }
        }

        public AmRoles FindByPk(string rid)
        {
            DataTable dt = new DataTable();
            AmRoles entity = new AmRoles();
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();
                    string sqlText = @"select
                                Role_Id 
                                ,Role_Name
                            from 
                              " + this.TableName + @" 
                            where
                                Role_Id = @Role_Id";

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    sqlCmd.Parameters.Add("@Role_Id", SqlDbType.VarChar).Value = rid;

                    SqlDataAdapter adapter = new SqlDataAdapter(sqlCmd);

                    adapter.Fill(dt);
                    entity.RoleId = dt.Rows[0]["Role_Id"].ToString();
                    entity.RoleName = dt.Rows[0]["Role_Name"].ToString();
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

        public List<AmRoles> FindAll()
        {
            List<AmRoles> objList = new List<AmRoles>();
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();
                    string sqlText = @"
                            select
                                Role_Id 
                                ,Role_Name
                from " + this.TableName + @" 
                ";

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);

                    SqlDataReader dr = sqlCmd.ExecuteReader();
                    while (dr.Read())
                    {
                        AmRoles obj = new AmRoles();
                        obj.RoleId = dr["Role_Id"].ToString();
                        obj.RoleName = dr["Role_Name"].ToString();
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
