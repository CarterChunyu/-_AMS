using Common.Logging;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Domain;

namespace DataAccess
{
    public class WebPasswordLogDao : BasicDAL
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(WebPasswordLog));
        public string TableName { private get; set; }

        public WebPasswordLogDao()
        {
            this.TableName = "Web_Password_Log";
        }

        public WebPasswordLog FindByPk(string systemId, string userName)
        {
            WebPasswordLog obj = new WebPasswordLog();
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();
                    string sqlText = @"  SELECT ID, 
                                                SYSTEM_ID SYSTEMID, 
                                                USERNAME, 
                                                STATUS, 
                                                ERROR_COUNT ERRORCOUNT, 
                                                UPDATE_TIME UPDATETIME
                                           FROM " + this.TableName + @" 
                                          WHERE 
                                                SYSTEM_ID = @SYSTEMID
                                           AND  USERNAME  = @USERNAME ";

                    obj = sqlConn.Query<WebPasswordLog>(sqlText, new WebPasswordLog
                    {
                        SystemId = systemId,
                        UserName = userName
                    }).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.StackTrace);
                log.Error(ex.Message);
                return null;
            }
            return obj;
        }


        public void ClearErrorCount(string systemId, string userName)
        {
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();
                    string sqlText = @"  UPDATE " + this.TableName + @" 
                                            SET ERROR_COUNT='0'
                                          WHERE 
                                                SYSTEM_ID = @SYSTEMID
                                           AND  USERNAME  = @USERNAME ";

                    sqlConn.Execute(sqlText, new WebPasswordLog
                    {
                        SystemId = systemId,
                        UserName = userName
                    });
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.StackTrace);
                log.Error(ex.Message);
            }
        }

        public void AddErrorCount(string systemId, string userName)
        {
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();
                    string sqlText = @"  UPDATE " + this.TableName + @" 
                                            SET ERROR_COUNT= ERROR_COUNT+1
                                          WHERE 
                                                SYSTEM_ID = @SYSTEMID
                                           AND  USERNAME  = @USERNAME ";

                    sqlConn.Execute(sqlText, new WebPasswordLog
                    {
                        SystemId = systemId,
                        UserName = userName
                    });
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.StackTrace);
                log.Error(ex.Message);
            }
        }

        public void UpdateStatus(string systemId, string userName, WebPasswordLog.PWStatusType status)
        {
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();
                    string sqlText = @"  UPDATE " + this.TableName + @" 
                                            SET STATUS      = @STATUS
                                          WHERE 
                                                SYSTEM_ID   = @SYSTEMID
                                           AND  USERNAME    = @USERNAME ";

                    sqlConn.Execute(sqlText, new WebPasswordLog
                    {
                        SystemId = systemId,
                        UserName = userName,
                        Status   = status
                    });
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.StackTrace);
                log.Error(ex.Message);
            }
        }

        public void UpdateDate(string systemId, string userName, DateTime datetime)
        {
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();
                    string sqlText = @"  UPDATE " + this.TableName + @" 
                                            SET UPDATE_TIME = @UPDATETIME
                                          WHERE 
                                                SYSTEM_ID   = @SYSTEMID
                                           AND  USERNAME    = @USERNAME ";

                    sqlConn.Execute(sqlText, new WebPasswordLog
                    {
                        SystemId = systemId,
                        UserName = userName,
                        UpdateTime = datetime
                    });
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.StackTrace);
                log.Error(ex.Message);
            }
        }

        public void Insert(string systemId, string userName)
        {
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();
                    string sqlText = @"  INSERT INTO  " + this.TableName + @" 
                                                           ([SYSTEM_ID]
                                                           ,[USERNAME]
                                                           ,[STATUS]
                                                           ,[UPDATE_TIME]
		                                                   )
                                                     VALUES
                                                           (@SYSTEMID
                                                           ,@USERNAME
                                                           ,@STATUS
                                                           ,@UPDATETIME)";

                    sqlConn.Execute(sqlText, new WebPasswordLog
                    {
                        SystemId = systemId,
                        UserName = userName,
                        Status = WebPasswordLog.PWStatusType.First,
                        UpdateTime = DateTime.Now
                    });
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.StackTrace);
                log.Error(ex.Message);
            }
        }

//        public void UpdateCheck(int id, string IsChecked, string updateUser)
//        {
//            try
//            {
//                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
//                {
//                    sqlConn.Open();

//                    string sqlText = @"
//                       UPDATE "
//                            + this.TableName + @"
//                       SET 
//                          [Is_Checked] = @Is_Checked
//                          ,[Updated_User] = @Updated_User
//                          ,[Updated_Date] = @Updated_Date
//                       WHERE
//                            [Id] = @Id";

//                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
//                    sqlCmd.Parameters.Add("@Id", SqlDbType.Int).Value = id;
//                    sqlCmd.Parameters.Add("@Updated_Date", SqlDbType.DateTime).Value = DateTime.Now;
//                    sqlCmd.Parameters.Add("@Updated_User", SqlDbType.VarChar).Value = updateUser;
//                    sqlCmd.Parameters.Add("@Is_Checked", SqlDbType.VarChar).Value = IsChecked;
//                    sqlCmd.ExecuteNonQuery();
//                }
//            }
//            catch (Exception ex)
//            {
//                log.Debug(ex.StackTrace);
//                log.Debug(ex.Message);
//                throw ex;
//            }
//        }

//        public void UpdateCheckByTypeId(string typeId, string IsChecked, string updateUser)
//        {
//            try
//            {
//                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
//                {
//                    sqlConn.Open();

//                    string sqlText = @"
//                       UPDATE "
//                            + this.TableName + @"
//                       SET 
//                          [Is_Checked] = @Is_Checked
//                          ,[Updated_User] = @Updated_User
//                          ,[Updated_Date] = @Updated_Date
//                       WHERE
//                            [Type_Id] = @Type_Id";

//                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
//                    sqlCmd.Parameters.Add("@Type_Id", SqlDbType.VarChar).Value = typeId;
//                    sqlCmd.Parameters.Add("@Updated_Date", SqlDbType.DateTime).Value = DateTime.Now;
//                    sqlCmd.Parameters.Add("@Updated_User", SqlDbType.VarChar).Value = updateUser;
//                    sqlCmd.Parameters.Add("@Is_Checked", SqlDbType.VarChar).Value = IsChecked;
//                    sqlCmd.ExecuteNonQuery();
//                }
//            }
//            catch (Exception ex)
//            {
//                log.Debug(ex.StackTrace);
//                log.Debug(ex.Message);
//                throw ex;
//            }
//        }

//        public void Insert(CcRemark obj)
//        {
//            try
//            {
//                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
//                {
//                    sqlConn.Open();
//                    string sqlText = @"INSERT INTO "
//                                    + this.TableName + @"
//                                   (
//                                    [Type]
//                                   ,[Type_Id]
//                                   ,[Description]
//                                   ,[Opid]
//                                   ,[Create_Date]
//                                   ,[Card_Status]
//                                   ,[Is_Checked]
//                                   ,[Created_User]
//                                   ,[Updated_Role]
//                                   ,[Case_Id]
//                                    )
//                            VALUES
//                                   (
//                                    @Type
//                                   ,@Type_Id
//                                   ,@Description
//                                   ,@Opid                                
//                                   ,@Create_Date
//                                   ,@Card_Status
//                                   ,@Is_Checked
//                                   ,@Created_User
//                                   ,@Updated_Role
//                                   ,@Case_Id
//                                    )";

//                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
//                    sqlCmd.Parameters.Add("@Type", SqlDbType.VarChar).Value         = ConvertValue(obj.Type);
//                    sqlCmd.Parameters.Add("@Type_Id", SqlDbType.VarChar).Value      = ConvertValue(obj.TypeId);
//                    sqlCmd.Parameters.Add("@Description", SqlDbType.NVarChar).Value = ConvertValue(obj.Description);
//                    sqlCmd.Parameters.Add("@Opid", SqlDbType.VarChar).Value         = ConvertValue(obj.Opid);
//                    sqlCmd.Parameters.Add("@Create_Date", SqlDbType.DateTime).Value = ConvertValue(DateTime.Now);
//                    sqlCmd.Parameters.Add("@Card_Status", SqlDbType.VarChar).Value  = ConvertValue(obj.CardStatus);
//                    sqlCmd.Parameters.Add("@Is_Checked", SqlDbType.VarChar).Value   = ConvertValue(obj.IsChecked);
//                    sqlCmd.Parameters.Add("@Created_User", SqlDbType.VarChar).Value = ConvertValue(obj.CreatedUser);
//                    sqlCmd.Parameters.Add("@Updated_Role", SqlDbType.VarChar).Value = ConvertValue(obj.UpdatedRole);
//                    sqlCmd.Parameters.Add("@Case_Id", SqlDbType.VarChar).Value      = ConvertValue(obj.CaseId);
//                    sqlCmd.ExecuteNonQuery();
//                }
//            }
//            catch (Exception ex)
//            {
//                log.Debug(ex.StackTrace);
//                log.Debug(ex.Message);
//                throw ex;
//            }
//        }

//        public void Delete(int id)
//        {
//            try
//            {
//                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
//                {
//                    sqlConn.Open();

//                    string sqlText = @"
//                       DELETE from 
//                        " + this.TableName + @"
//                       WHERE
//                            [Id] = @Id";

//                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
//                    sqlCmd.Parameters.Add("@Id", SqlDbType.Int).Value = id;

//                    sqlCmd.ExecuteNonQuery();
//                }
//            }
//            catch (Exception ex)
//            {
//                log.Debug(ex.StackTrace);
//                log.Debug(ex.Message);
//            }
//        }
//        public bool Exist(int id)
//        {
//            Int32 count = 0;
//            try
//            {
//                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
//                {
//                    sqlConn.Open();
//                    string sqlText =
//                            @"select 
//                              count(*)
//                          from 
//                              " + this.TableName + @" 
//                          where
//                              Id = @Id
//                         ";
//                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
//                    sqlCmd.Parameters.Add("@Id", SqlDbType.Int).Value = id;

//                    count = (Int32)sqlCmd.ExecuteScalar();
//                }
//            }
//            catch (Exception ex)
//            {
//                log.Debug("Query Ex: " + this.TableName);////
//                log.Debug(ex.StackTrace);
//                log.Debug(ex.Message);
//            }
//            return (count > 0);
//        }

//        public List<CcRemark> FindByTypeId(string type, string typeId)
//        {
//            List<CcRemark> objList = new List<CcRemark>();
//            try
//            {
//                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
//                {
//                    sqlConn.Open();
//                    string sqlText = @"
//                            select
//                                Id 
//                                ,Type
//                                ,Type_Id
//                                ,Description
//                                ,Opid
//                                ,Create_Date
//                                ,Card_Status
//                                ,Is_Checked
//                                ,Created_User
//                                ,Updated_Role
//                                ,Case_Id
//                from " + this.TableName + @" 
//                where Type = @Type and Type_Id = @Type_Id
//                order by Id 
//                ";

//                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
//                    sqlCmd.Parameters.Add("@Type", SqlDbType.VarChar).Value = type;
//                    sqlCmd.Parameters.Add("@Type_Id", SqlDbType.VarChar).Value = typeId;
//                    SqlDataReader dr = sqlCmd.ExecuteReader();
//                    while (dr.Read())
//                    {
//                        CcRemark obj = new CcRemark();
//                        obj.Id = Int32.Parse(dr["Id"].ToString());
//                        obj.Type = dr["Type"].ToString();
//                        obj.TypeId = dr["Type_Id"].ToString();
//                        obj.Description = dr["Description"].ToString();
//                        obj.Opid = dr["Opid"].ToString();
//                        obj.CreateDate = Convert.ToDateTime(dr["Create_Date"]);
//                        obj.CardStatus = dr["Card_Status"].ToString();
//                        obj.IsChecked = dr["Is_Checked"].ToString();
//                        obj.CreatedUser = dr["Created_User"].ToString();
//                        obj.UpdatedRole = dr["Updated_Role"].ToString();
//                        obj.CaseId = dr["Case_Id"].ToString();
//                        objList.Add(obj);
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                log.Debug("Query Ex: " + this.TableName);////
//                log.Debug(ex.StackTrace);
//                log.Debug(ex.Message);
//                objList = null;
//            }
//            //log.Debug("Query End: " + this.TableName);////
//            return objList;
//        }

//        public List<CcRemark> FindByCaseId(string type, string caseId)
//        {
//            List<CcRemark> objList = new List<CcRemark>();
//            try
//            {
//                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
//                {
//                    sqlConn.Open();
//                    string sqlText = @"
//                            select
//                                Id 
//                                ,Type
//                                ,Type_Id
//                                ,Description
//                                ,Opid
//                                ,Create_Date
//                                ,Card_Status
//                                ,Is_Checked
//                                ,Created_User
//                                ,Updated_Role
//                                ,Case_Id
//                            from " + this.TableName + @" 
//                            where Type = @Type and Case_Id = @Case_Id
//                            order by Id 
//                            ";

//                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
//                    sqlCmd.Parameters.Add("@Type", SqlDbType.VarChar).Value = type;
//                    sqlCmd.Parameters.Add("@Case_Id", SqlDbType.VarChar).Value = caseId;
//                    SqlDataReader dr = sqlCmd.ExecuteReader();
//                    while (dr.Read())
//                    {
//                        CcRemark obj = new CcRemark();
//                        obj.Id = Int32.Parse(dr["Id"].ToString());
//                        obj.Type = dr["Type"].ToString();
//                        obj.TypeId = dr["Type_Id"].ToString();
//                        obj.Description = dr["Description"].ToString();
//                        obj.Opid = dr["Opid"].ToString();
//                        obj.CreateDate = Convert.ToDateTime(dr["Create_Date"]);
//                        obj.CardStatus = dr["Card_Status"].ToString();
//                        obj.IsChecked = dr["Is_Checked"].ToString();
//                        obj.CreatedUser = dr["Created_User"].ToString();
//                        obj.UpdatedRole = dr["Updated_Role"].ToString();
//                        obj.CaseId = dr["Case_Id"].ToString();
//                        objList.Add(obj);
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                log.Debug("Query Ex: " + this.TableName);////
//                log.Debug(ex.StackTrace);
//                log.Debug(ex.Message);
//                objList = null;
//            }
//            //log.Debug("Query End: " + this.TableName);////
//            return objList;
//        }

//        public int GetCount(string type, string cardStatus, string createdUser)
//        {
//            Int32 count = 0;
//            try
//            {
//                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
//                {
//                    sqlConn.Open();
//                    string sqlText =
//                            @"select 
//                              count(*)
//                          from 
//                              " + this.TableName + @" 
//                          where Type = @Type ";

//                    if (createdUser != "")
//                        sqlText += "and Created_User = @Created_User ";

//                    sqlText += "and Card_Status = @Card_Status and Is_Checked=@Is_Checked ";

//                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
//                    sqlCmd.Parameters.Add("@Type", SqlDbType.VarChar).Value = type;
//                    sqlCmd.Parameters.Add("@Card_Status", SqlDbType.VarChar).Value = cardStatus;
//                    sqlCmd.Parameters.Add("@Created_User", SqlDbType.VarChar).Value = createdUser;
//                    sqlCmd.Parameters.Add("@Is_Checked", SqlDbType.VarChar).Value = "N";

//                    count = (Int32)sqlCmd.ExecuteScalar();
//                }
//            }
//            catch (Exception ex)
//            {
//                log.Debug("Query Ex: " + this.TableName);////
//                log.Debug(ex.StackTrace);
//                log.Debug(ex.Message);
//            }
//            return count;
//        }

//        public List<CcRemark> FindByUnChecked(string type, string cardStatus, string createdUser)
//        {
//            List<CcRemark> objList = new List<CcRemark>();
//            try
//            {
//                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
//                {
//                    sqlConn.Open();
//                    string sqlText = @"
//                            select
//                                Id 
//                                ,Type
//                                ,Type_Id
//                                ,Description
//                                ,Opid
//                                ,Create_Date
//                                ,Card_Status
//                                ,Is_Checked
//                                ,Created_User
//                                ,Updated_Role
//                                ,Case_Id
//                from " + this.TableName + @" 
//                where Type = @Type ";

//                if (createdUser != "")
//                    sqlText += "and Created_User = @Created_User ";

//                sqlText += "and Card_Status = @Card_Status and Is_Checked=@Is_Checked order by Case_Id  ";

//                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
//                    sqlCmd.Parameters.Add("@Type", SqlDbType.VarChar).Value = type;
//                    sqlCmd.Parameters.Add("@Card_Status", SqlDbType.VarChar).Value = cardStatus;
//                    sqlCmd.Parameters.Add("@Created_User", SqlDbType.VarChar).Value = createdUser;
//                    sqlCmd.Parameters.Add("@Is_Checked", SqlDbType.VarChar).Value = "N";
//                    SqlDataReader dr = sqlCmd.ExecuteReader();
//                    while (dr.Read())
//                    {
//                        CcRemark obj = new CcRemark();
//                        obj.Id = Int32.Parse(dr["Id"].ToString());
//                        obj.Type = dr["Type"].ToString();
//                        obj.TypeId = dr["Type_Id"].ToString();
//                        obj.Description = dr["Description"].ToString();
//                        obj.Opid = dr["Opid"].ToString();
//                        obj.CreateDate = Convert.ToDateTime(dr["Create_Date"]);
//                        obj.CardStatus = dr["Card_Status"].ToString();
//                        obj.IsChecked = dr["Is_Checked"].ToString();
//                        obj.CreatedUser = dr["Created_User"].ToString();
//                        obj.UpdatedRole = dr["Updated_Role"].ToString();
//                        obj.CaseId = dr["Case_Id"].ToString();
//                        objList.Add(obj);
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                log.Debug("Query Ex: " + this.TableName);////
//                log.Debug(ex.StackTrace);
//                log.Debug(ex.Message);
//                objList = null;
//            }
//            return objList;
//        }

//        public List<CcRemark> FindByITTS(string type, string sdate, string edate, string isChecked)
//        {
//            List<CcRemark> objList = new List<CcRemark>();
//            try
//            {
//                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
//                {
//                    sqlConn.Open();
//                    string sqlText = @"
//                            select
//                                Id 
//                                ,Type
//                                ,Type_Id
//                                ,Description
//                                ,Opid
//                                ,Create_Date
//                                ,Card_Status
//                                ,Is_Checked
//                                ,Created_User
//                                ,Updated_Role
//                                ,Case_Id
//                    from " + this.TableName + @" 
//                    where Type = @Type and Updated_Role='ITTSCustomer' ";
//                    if(sdate!=""){
//                        sqlText += "and CONVERT(varchar(100), Create_Date, 23) >= @SDate and CONVERT(varchar(100), Create_Date, 23) <= @EDate ";
//                    }
//                    if (isChecked != "")
//                    {
//                        sqlText += "and Is_Checked=@Is_Checked ";
//                    }
//                    sqlText += "order by Case_Id ";

//                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
//                    sqlCmd.Parameters.Add("@Type", SqlDbType.VarChar).Value = type;
//                    sqlCmd.Parameters.Add("@Is_Checked", SqlDbType.VarChar).Value = isChecked;
//                    if (sdate != "")
//                    {
//                        sqlCmd.Parameters.Add("@SDate", SqlDbType.Date).Value = sdate;
//                        sqlCmd.Parameters.Add("@EDate", SqlDbType.Date).Value = edate;
//                    }
//                    SqlDataReader dr = sqlCmd.ExecuteReader();
//                    while (dr.Read())
//                    {
//                        CcRemark obj = new CcRemark();
//                        obj.Id = Int32.Parse(dr["Id"].ToString());
//                        obj.Type = dr["Type"].ToString();
//                        obj.TypeId = dr["Type_Id"].ToString();
//                        obj.Description = dr["Description"].ToString();
//                        obj.Opid = dr["Opid"].ToString();
//                        obj.CreateDate = Convert.ToDateTime(dr["Create_Date"]);
//                        obj.CardStatus = dr["Card_Status"].ToString();
//                        obj.IsChecked = dr["Is_Checked"].ToString();
//                        obj.CreatedUser = dr["Created_User"].ToString();
//                        obj.UpdatedRole = dr["Updated_Role"].ToString();
//                        obj.CaseId = dr["Case_Id"].ToString();
//                        objList.Add(obj);
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                log.Debug("Query Ex: " + this.TableName);////
//                log.Debug(ex.StackTrace);
//                log.Debug(ex.Message);
//                objList = null;
//            }
//            return objList;
//        }

//        public CcRemark FindByPWChangeer(string type, string typeId, string cardStatus)
//        {
//            CcRemark objList = new CcRemark();
//            try
//            {
//                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
//                {
//                    sqlConn.Open();
//                    string sqlText = @"  SELECT TOP 1 Create_Date CreateDate
//                                           FROM CC_REMARK
//                                          WHERE [TYPE]=@type
//	                                        AND Card_Status=@cardStatus
//	                                        AND Type_Id=@typeId
//	                                   ORDER BY Create_Date DESC ";

//                    objList = sqlConn.Query<CcRemark>(sqlText, new CcRemark
//                    {
//                        Type=type,
//                        TypeId = typeId,
//                        CardStatus=cardStatus
//                    }).FirstOrDefault();
//                }
//            }
//            catch (Exception ex)
//            {
//                log.Error("Query ex:" + this.TableName);
//                log.Error(ex.Message);
//                log.Error(ex.StackTrace);
//                objList = null;
//            }
//            return objList;
//        } 
        
//        public object Clone()
//        {
//            return this.MemberwiseClone();
//        }
    }
}
