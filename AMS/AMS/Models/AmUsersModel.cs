using Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AMS.Models
{
    public class AmUsersModel
    {
        public AmUsers AmUsers { get; set; }

        public IEnumerable<AmUsers> AmUsersList { get; set; }

        [Display(Name = "使用者帳號")]
        [Required]
        public string Username
        {
            get { return this.AmUsers.Username; }
            set { value = this.AmUsers.Username; }
        }

        [Display(Name = "使用者密碼")]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*\d)(?=.*[A-Z])(?=.*[a-z]).{8,12}$", ErrorMessage = "請輸入8~12位數密碼，其中至少1個大寫字母，1個小寫字母,1位數字")]
        [Required]
        public string Password
        {
            get { return this.AmUsers.Password; }
            set { value = this.AmUsers.Password; }
        }

        [Display(Name = "確認新密碼")]
        [Compare("Password")]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*\d)(?=.*[A-Z])(?=.*[a-z]).{8,12}$", ErrorMessage = "請輸入8~12位數密碼，其中至少1個大寫字母，1個小寫字母,1位數字")]
        [Required]
        public string ConfirmPassword { get; set; }

        [Display(Name = "使用者姓名")]
        [Required]
        public string Name
        {
            get { return this.AmUsers.Name; }
            set { value = this.AmUsers.Name; }
        }

        [Display(Name = "Email")]
        [Required]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "E-mail is not valid")]
        public string Email
        {
            get { return this.AmUsers.Email; }
            set { value = this.AmUsers.Email; }
        }

        [Display(Name = "部門")]
        [Required]
        public string Role
        {
            get { return this.AmUsers.Role; }
            set { value = this.AmUsers.Role; }
        }

        [Display(Name = "狀態")]
        [Required]
        public string Status
        {
            get { return this.AmUsers.Status; }
            set { value = this.AmUsers.Status; }
        }

        [Display(Name = "上一次登入時間")]
        [DataType(DataType.DateTime)]
        [Required]
        public DateTime RegDate
        {
            get { return this.AmUsers.RegDate; }
            set { value = this.AmUsers.RegDate; }
        }

        [Display(Name = "更新日期")]
        [DataType(DataType.DateTime)]
        [Required]
        public DateTime UpdateDate
        {
            get { return this.AmUsers.UpdateDate; }
            set { value = this.AmUsers.UpdateDate; }
        }
    }
}