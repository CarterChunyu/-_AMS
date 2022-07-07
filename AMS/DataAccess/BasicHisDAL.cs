using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class BasicHisDAL : MarshalByRefObject
    {
        #region ==== 預設物件成員 ====
        /// <summary>
        /// 連線字串
        /// </summary>
        protected string _connectionString;
        #endregion ==== 預設物件成員 ====

        /// <summary>
        /// Database connection Initialize.
        /// </summary>
        public void Initialize()
        {

            if (ConfigurationManager.ConnectionStrings["hisConnection"] == null ||
                ConfigurationManager.ConnectionStrings["hisConnection"].ConnectionString.Trim() == "")
            {
                throw new Exception("A connection string named 'hisConnection' with a valid connection string " +
                                    "must exist in the <connectionStrings> configuration section for the application.");
            }

            _connectionString =
              ConfigurationManager.ConnectionStrings["hisConnection"].ConnectionString;
        }

        /// <summary>
        /// 
        /// </summary>
        public BasicHisDAL()
        {
            Initialize();
        }
    }
}
