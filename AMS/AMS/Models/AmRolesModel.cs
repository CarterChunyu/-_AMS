using Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AMS.Models
{
    public class AmRolesModel
    {
        public AmRoles AmRoles { get; set; }

        public IEnumerable<AmRoles> AmRolesList { get; set; }

        [Display(Name = "部門代號")]
        [Required]
        public string RoleId
        {
            get { return this.AmRoles.RoleId; }
            set { value = this.AmRoles.RoleId; }
        }

        [Display(Name = "部門名稱")]
        [Required]
        public string RoleName
        {
            get { return this.AmRoles.RoleName; }
            set { value = this.AmRoles.RoleName; }
        }

    }
}