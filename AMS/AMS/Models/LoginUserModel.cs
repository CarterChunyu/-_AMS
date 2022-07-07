using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AMS.Models
{
    public class LoginUserModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "使用者帳號")]
        public string Username { get; set; }

        [Required]
        [Display(Name = "使用者密碼")]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}