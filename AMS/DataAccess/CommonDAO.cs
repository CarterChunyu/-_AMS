using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class CommonDAO
    {
        /// <summary>
        /// 列舉SQL參數轉型型態
        /// </summary>
        public enum en_convert_type
        {
            /// <summary>
            /// string
            /// </summary>
            en_string,
            /// <summary>
            /// Int32，範圍[+-2147483648，10位]
            /// </summary>
            en_int,
            /// <summary>
            /// Int64，範圍[+-9223372036854775808之間，19位]
            /// </summary>
            en_int64,
            /// <summary>
            /// 浮點數
            /// </summary>
            en_float,
            /// <summary>
            /// date，無時分秒
            /// </summary>
            en_date,
            /// <summary>
            /// date，有時分秒
            /// </summary>
            en_datetime,
            /// <summary>
            /// date，傳入yyyymmdd
            /// </summary>
            en_datenoslash,
            /// <summary>
            /// date，自訂格式
            /// </summary>
            en_datecustom
        }

        /// <summary>
        /// 轉型SQL參數值
        /// </summary>
        /// <param name="s_Value">傳入的值</param>
        /// <param name="en_Type">輸出型態</param>
        /// <param name="s_Format">自訂格式化</param>
        /// <returns>回傳結果</returns>
        public object Convert_Sql_Parameter(string s_Value, en_convert_type en_Type, string s_Format)
        {
            #region 設定變數

            object o_Return_Object = new object();
            string s_TrimValue = string.Empty;

            #endregion

            #region 處理傳入值

            s_Value = string.IsNullOrWhiteSpace(s_Value) == true ? string.Empty : s_Value;
            s_TrimValue = s_Value.Trim();

            #endregion

            #region 依照輸出型態，轉型傳入的值

            if (s_TrimValue == string.Empty)
            { o_Return_Object = System.DBNull.Value; }
            else
            {
                switch (en_Type)
                {
                    case en_convert_type.en_string:
                        o_Return_Object = s_TrimValue;
                        break;
                    case en_convert_type.en_int:
                        o_Return_Object = int.Parse(s_TrimValue);
                        break;
                    case en_convert_type.en_int64:
                        o_Return_Object = Int64.Parse(s_TrimValue);
                        break;
                    case en_convert_type.en_float:
                        o_Return_Object = float.Parse(s_TrimValue);
                        break;
                    case en_convert_type.en_date:
                        o_Return_Object = DateTime.ParseExact(DateTime.Parse(s_TrimValue).ToString("yyyy/MM/dd"), "yyyy/MM/dd", null);
                        break;
                    case en_convert_type.en_datetime:
                        o_Return_Object = DateTime.Parse(s_TrimValue);
                        break;
                    case en_convert_type.en_datenoslash:
                        o_Return_Object = DateTime.ParseExact(s_TrimValue, "yyyyMMdd", null);
                        break;
                    case en_convert_type.en_datecustom:
                        o_Return_Object = DateTime.ParseExact(s_TrimValue, s_Format, null);
                        break;
                }
            }

            #endregion

            return o_Return_Object;
        }
    }
}
