using Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AMS.Models
{
    public class ChoiceModel
    {
        public AmChoice AmChoice { get; set; }

        public IEnumerable<AmChoice> CcSysChoiceList { get; set; }

        [Display(Name = "類別")]
        [Required]
        public string Code
        {
            get { return this.AmChoice.Code; }
            set { value = this.AmChoice.Code; }
        }

        [Display(Name = "值")]
        [Required]
        public string Value
        {
            get { return this.AmChoice.Value; }
            set { value = this.AmChoice.Value; }
        }

        [Display(Name = "名稱")]
        [Required]
        public string Name
        {
            get { return this.AmChoice.Name; }
            set { value = this.AmChoice.Name; }
        }

        [Display(Name = "備註")]
        [Required]
        public string Remark
        {
            get { return this.AmChoice.Remark; }
            set { value = this.AmChoice.Remark; }
        }

        [Display(Name = "狀態")]
        [Required]
        public string Status
        {
            get { return this.AmChoice.Status; }
            set { value = this.AmChoice.Status; }
        }

        [Display(Name = "排序")]
        [Required]
        public int Rank
        {
            get { return this.AmChoice.Rank; }
            set { value = this.AmChoice.Rank; }
        }
    }
}