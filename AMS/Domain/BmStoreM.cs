using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class BmStoreM
    {
        public string STORE_TYPE { get; set; }
        public string MERCHANT_NO { get; set; }
        //[Display(Name = @"門市代號")]
        //[RegularExpression(@"^[A-Za-z0-9]+$", ErrorMessage = "門市代號只能輸入英文或數字")]
        //[Required]
        public string STORE_NO { get; set; }
        //[Display(Name = @"門市全名")]
        //[Required]
        public string STO_NAME_LONG { get; set; }
        //[Display(Name = @"門市簡稱")]
        //[Required]
        public string STO_NAME_SHORT { get; set; }
        //[Display(Name = @"生效起日")]
        //[RegularExpression(@"(^0000|0001|9999|[0-9]{4})+(0[1-9]|1[0-2])+(0[1-9]|[12][0-9]|3[01])$", ErrorMessage = "生效起日日期格式錯誤(YYYYMMDD)")]
        //[Required]
        public string EFF_DATE_FROM { get; set; }
        //[Display(Name = @"生效迄日")]
        //[RegularExpression(@"(^0000|0001|9999|[0-9]{4})+(0[1-9]|1[0-2])+(0[1-9]|[12][0-9]|3[01])$", ErrorMessage = "生效迄日日期格式錯誤(YYYYMMDD)")]
        //[Required]
        public string EFF_DATE_TO { get; set; }
        //[Display(Name = @"開幕日")]
        //[RegularExpression(@"(^0000|0001|9999|[0-9]{4})+(0[1-9]|1[0-2])+(0[1-9]|[12][0-9]|3[01])$", ErrorMessage = "開幕日日期格式錯誤(YYYYMMDD)")]
        //[Required]
        public string OPEN_DATE { get; set; }
        //[Display(Name = @"可販售日")]
        //[RegularExpression(@"(^0000|0001|9999|[0-9]{4})+(0[1-9]|1[0-2])+(0[1-9]|[12][0-9]|3[01])$", ErrorMessage = "可販售日日期格式錯誤(YYYYMMDD)")]
        //[Required]
        public string SALES_DATE { get; set; }
        //[Display(Name = @"關店日")]
        //[RegularExpression(@"(^0000|0001|9999|[0-9]{4})+(0[1-9]|1[0-2])+(0[1-9]|[12][0-9]|3[01])$", ErrorMessage = "關店日日期格式錯誤(YYYYMMDD)")]
        //[Required]
        public string CLOSE_DATE { get; set; }
        //[Display(Name = @"郵遞區號")]
        public string ZIP { get; set; }
        //[Display(Name = @"地址")]
        public string ADDRESS { get; set; }
        //[Display(Name = @"電話區域碼")]
        public string TEL_AREA { get; set; }
        //[Display(Name = @"電話")]
        public string TEL_NO { get; set; }
        public string OPERATE_TYPE { get; set; }
        public string LINE_TYPE { get; set; }
        public string CITY_TYPE { get; set; }
        public string TRANSFER_TYPE { get; set; }
        public string SUBSIDY_TYPE { get; set; }
        public string LINE_NO_03 { get; set; }
        public string LINE_NO_04 { get; set; }
        public string LICENSE_NO { get; set; }
        public string LINE_QTY { get; set; }
        public string LINE_SI_NO { get; set; }
        public string UPD_DATE { get; set; }
    }
}
