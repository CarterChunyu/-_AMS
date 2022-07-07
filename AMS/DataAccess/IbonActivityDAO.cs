using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Common.Logging;
using Domain.Entities;
using System.Data.SqlClient;

namespace DataAccess
{
    public class IbonActivityDAO : BasicDAL
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(IbonActivityDAO));

        public string TABLE_NAME { private get; set; } 
        
        public IbonActivityDAO()
        {
             this.TABLE_NAME = "IBON_COMPANY_LOADING_M";
        }

        public List<IbonActivity> FindAll()
        {
            List<IbonActivity> objList = new List<IbonActivity>();
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(this._connectionString))
                {
                    sqlConn.Open();
                    string sqlText = @"SELECT DISTINCT ACTIVITY_NAME+' ('+ACTIVITY_CODE+')' AS ACTIVITY_NAME ,ACTIVITY_CODE FROM IBON_COMPANY_LOADING_M ORDER BY ACTIVITY_CODE";

                    SqlCommand sqlCmd = new SqlCommand(sqlText, sqlConn);
                    SqlDataReader dr = sqlCmd.ExecuteReader();
                    while (dr.Read())
                    {
                        IbonActivity obj = new IbonActivity();
                        obj.Name = dr["ACTIVITY_NAME"].ToString();
                        obj.Code = dr["ACTIVITY_CODE"].ToString();
                        objList.Add(obj);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Debug("Query Ex: " + this.TABLE_NAME);////
                log.Debug(ex.StackTrace);
                log.Debug(ex.Message);
                objList = null;
            }
            return objList;
        }

    }
}
