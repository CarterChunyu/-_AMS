using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Helpers
{
    public class RandomPassword
    {
        private static RNGCryptoServiceProvider rngp = new RNGCryptoServiceProvider();
        private static byte[] rb = new byte[4];

        private static int Next(int max)
        {
            rngp.GetBytes(rb);
            int value = BitConverter.ToInt32(rb, 0);
            value = value % (max + 1);
            if (value < 0) value = -value;
            return value;
        }

        public static string GetRandomPassword(int length)
        {
            Regex regex = new Regex(@"^(?=.*\d)(?=.*[A-Z])(?=.*[a-z]).{8,12}$");
            int i = 1;
            string password = "";
            while (i <= 1)
            {
                password = GenRandomPassword(8);
                if (regex.IsMatch(password))
                {
                    i++;
                }
            }
            return password;
        }

        public static string GenRandomPassword(int length)
        {
            StringBuilder sb = new StringBuilder();
            char[] chars = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
            for (int i = 0; i < length; i++)
            {
                sb.Append(chars[Next(chars.Length - 1)]);
            }
            string Password = sb.ToString();
            return Password;
        }
    }
}
