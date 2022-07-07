using System;
using System.Configuration;

namespace DataAccess
{
    public class BasicOPDAL : MarshalByRefObject
    {
        #region ==== 預設物件成員 ====
        /// <summary>
        /// 連線字串
        /// </summary>
        protected string _connectionString;
        #endregion ==== 預設物件成員 ====

        /// <summary>
        /// 
        /// </summary>
        public BasicOPDAL()
        {
            Initialize();
        }

        /// <summary>
        /// Database connection Initialize.
        /// </summary>
        public void Initialize()
        {

            if (ConfigurationManager.ConnectionStrings["ICASHOPConnection"] == null ||
                ConfigurationManager.ConnectionStrings["ICASHOPConnection"].ConnectionString.Trim() == "")
            {
                throw new Exception("A connection string named 'OpenPointConnection' with a valid connection string " +
                                    "must exist in the <connectionStrings> configuration section for the application.");
            }

            _connectionString = ConfigurationManager.ConnectionStrings["ICASHOPConnection"].ConnectionString;
        }
    }
}
