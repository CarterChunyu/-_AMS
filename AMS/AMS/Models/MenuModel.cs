using Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AMS.Models
{
    public class MenuModel
    {
        public AmMenu AmMenu { get; set; }

        public IEnumerable<AmMenu> MenuList { get; set; }

        [Display(Name = "選單編號")]
        [Required]
        public int Id
        {
            get { return this.AmMenu.Id; }
            set { value = this.AmMenu.Id; }
        }

        [Display(Name = "選單名稱")]
        [Required]
        public string Name
        {
            get { return this.AmMenu.Name; }
            set { value = this.AmMenu.Name; }
        }

        [Display(Name = "路徑")]
        [Required]
        public string Path
        {
            get { return this.AmMenu.Path; }
            set { value = this.AmMenu.Path; }
        }

        [Display(Name = "父選單編號")]
        [Required]
        public int ParentId
        {
            get { return this.AmMenu.ParentId; }
            set { value = this.AmMenu.ParentId; }
        }

        [Display(Name = "權限")]
        [Required]
        public string Roles
        {
            get { return this.AmMenu.Roles; }
            set { value = this.AmMenu.Roles; }
        }

        [Display(Name = "狀態")]
        [Required]
        public string Status
        {
            get { return this.AmMenu.Status; }
            set { value = this.AmMenu.Status; }
        }

        [Display(Name = "排序")]
        [Required]
        public int Rank
        {
            get { return this.AmMenu.Rank; }
            set { value = this.AmMenu.Rank; }
        }
    }
}