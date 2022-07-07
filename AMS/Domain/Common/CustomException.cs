using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Common
{
    public class CustomException
    {
        /// <summary>
        /// 自訂例外：頁面檢核錯誤
        /// </summary>
        public class ExecErr : Exception
        {
            public ExecErr() { }
            public ExecErr(string message) : base(message) { }
            public ExecErr(string message, Exception inner) : base(message, inner) { }
        }
    }
}
