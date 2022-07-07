using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AMS.Models
{
    public class PasswordModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Opid { get; set; }

        [Display(Name = "原密碼")]
        [DataType(DataType.Password)]
        [Required]
        public string OldPassword { get; set; }

        [Display(Name = "新密碼")]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*\d)(?=.*[A-Z])(?=.*[a-z]).{8,12}$", ErrorMessage = "請輸入8~12位數密碼，其中至少1個大寫字母，1個小寫字母,1位數字")]
        [Required]
        public string Password { get; set; }

        [Display(Name = "確認新密碼")]
        [Compare("Password")]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*\d)(?=.*[A-Z])(?=.*[a-z]).{8,12}$", ErrorMessage = "請輸入8~12位數密碼，其中至少1個大寫字母，1個小寫字母,1位數字")]
        [Required]
        public string ConfirmPassword { get; set; }
    }
}