using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helpers
{
    public class Utility
    {
        public static string SetTimeFormat(string date)
        {
            string newdate;
            if (date != null && date.Length >= 14)
            {
                newdate = date.Substring(0, 4) + "-" + date.Substring(4, 2) + "-" + date.Substring(6, 2) + " " + date.Substring(8, 2) + ":" + date.Substring(10, 2) + ":" + date.Substring(12);
            }
            else
            {
                newdate = date;
            }
            return newdate;
        }

        public static string SetDateFormat(string date)
        {
            string newdate;
            if (date != null && date.Length >= 8)
            {
                newdate = date.Substring(0, 4) + "-" + date.Substring(4, 2) + "-" + date.Substring(6, 2);
            }
            else
            {
                newdate = date;
            }
            return newdate;
        }

        public static bool CheckStrLength(string str, int maxLen)
        {
            bool isMatchLen = true;
            Encoding big5 = Encoding.GetEncoding("big5");
            byte[] b = big5.GetBytes(str);
            if (b.Length > maxLen) //超長
                isMatchLen = false;
            return isMatchLen;
        }

        public static string TrimForBig5(string str, int len, bool ellipsis)
        {
            Encoding big5 = Encoding.GetEncoding("big5");
            byte[] b = big5.GetBytes(str);
            if (b.Length <= len) //未超長，直接傳回
                return str;
            else
            {
                //如果要加刪節號再扣三個字元
                if (ellipsis) len -= 3;

                string res = big5.GetString(b, 0, len);
                //由於可能最後一個字元可能切到中文字的前一碼形成亂碼
                //透過截斷的亂碼與完整轉換結果會有出入的原理來偵測
                if (!big5.GetString(b).StartsWith(res))
                    res = big5.GetString(b, 0, len - 1);
                return res + (ellipsis ? "..." : "");
            }
        }

        ///字串轉全形
        ///</summary>
        ///<param name="input">任一字元串</param>
        ///<returns>全形字元串</returns>
        public static string ToWide(string input)
        {
            //移除空白字元
            //input = input.Replace(" ","");
            //半形轉全形：
            char[] c = input.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                //全形空格為12288，半形空格為32
                if (c[i] == 32)
                {
                    c[i] = (char)12288;
                    continue;
                }
                //其他字元半形(33-126)與全形(65281-65374)的對應關係是：均相差65248
                if (c[i] < 127)
                    c[i] = (char)(c[i] + 65248);
            }
            return new string(c);
        }

        ///<summary>
        ///字串轉半形
        ///</summary>
        ///<paramname="input">任一字元串</param>
        ///<returns>半形字元串</returns>
        public static string ToNarrow(string input)
        {
            char[] c = input.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                if (c[i] == 12288)
                {
                    c[i] = (char)32;
                    continue;
                }
                if (c[i] > 65280 && c[i] < 65375)
                    c[i] = (char)(c[i] - 65248);
            }
            return new string(c);
        }
    }
}
