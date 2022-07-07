using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;


namespace AMS.Models
//namespace Domain.Entities
{

    public class GmMerModel
    {
        [Required]
        [StringLength(8, MinimumLength = 8, ErrorMessage = "必須為8碼")]
        [DisplayName("機構編號")]
        public string MERCHANT_NO { get; set; }

        [DisplayName("清分群組")]
        public string MERC_GROUP { get; set; }

        [DisplayName("機構名稱")]
        [Required]
        public string MERCHANT_NAME { get; set; }


        [DisplayName("機構短稱")]
        [Required]
        public string MERCHANT_STNAME { get; set; }


        [DisplayName("機構統編")]
        [StringLength(8, MinimumLength = 8, ErrorMessage = "必須為8碼")]
        [Required]
        public string INVOICE_NO { get; set; }

        [StringLength(3, MinimumLength = 3, ErrorMessage = "必須為3碼")]
        [DisplayName("機構簡碼")]
        [Required]
        public string MERCH_TMID { get; set; }


        [DisplayName("允許加值")]
        public string OL_TYPE { get; set; }


        [DisplayName("購貨手續費率")]
        [Range(0, 100)]
        [Required]
        public double PUR_FEE_RATE { get; set; }

        [DisplayName("加值手續費率")]
        [Range(0, 100)]
        [Required]
        public double LOAD_FEE_RATE { get; set; }


        [DisplayName("自動加值手續費率")]
        [Range(0, 100)]
        [Required]
        public double AUTO_LOAD_FEE_RATE { get; set; }


        [DisplayName("營收款撥付週期")]
        [Required]
        public string REM_TYPE { get; set; }

        [DisplayName("營收款撥付日")]
        [RegularExpression(@"\d{1,2}", ErrorMessage = "必須為兩位數以內數字")]
        [Required]
        public string DAYLY_REM_DAY { get; set; }

        [DisplayName("手續費撥付週期")]
        [Required]
        public string REM_FEE_TYPE { get; set; }



        [DisplayName("手續費撥付日")]
        [RegularExpression(@"\d{1,2}", ErrorMessage = "必須為兩位數以內數字")]
        [Required]
        public string DAYLY_REM_FEE_DAY { get; set; }




        [DisplayName("AMS選單群組")]
        public string GROUP_ID { get; set; }

        [Required]
        [DisplayName("新增人員姓名")]
        public string BUILD_USER { get; set; }


    }
    
}
