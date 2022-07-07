
using System;
namespace Domain.Entities
{
    public class WebPasswordLog
    {
        public string Id { get; set; }
        public string SystemId { get; set; }
        public string UserName { get; set; }
        public PWStatusType Status { get; set; }
        public int ErrorCount { get; set; }
        public DateTime UpdateTime { get; set; }

        public enum PWStatusType
        {
            /// <summary>首次
            /// </summary>
            First  = 0,
            
            /// <summary>正常
            /// </summary>
            Normal = 1,
            
            /// <summary>帳號鎖定
            /// </summary>
            Lock   = 2,
            
            /// <summary>重送密碼
            /// </summary>
            ReSend = 3,

            /// <summary>密碼到期
            /// </summary>
            Expired = 4
        }
    }
}
