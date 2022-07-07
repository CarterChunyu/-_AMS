using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public abstract class BasicDAL : MarshalByRefObject
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

            if (ConfigurationManager.ConnectionStrings["iSettle2Connection"] == null ||
                ConfigurationManager.ConnectionStrings["iSettle2Connection"].ConnectionString.Trim() == "")
            {
                throw new Exception("A connection string named 'iSettle2Connection' with a valid connection string " +
                                    "must exist in the <connectionStrings> configuration section for the application.");
            }

            _connectionString =
              ConfigurationManager.ConnectionStrings["iSettle2Connection"].ConnectionString;
        }

        /// <summary>
        /// 
        /// </summary>
        public BasicDAL()
        {
            Initialize();
        }
        
        protected object ConvertValue(object value)
        {
            if (value == null) { return DBNull.Value; }
            return value;
        }
    }
}
