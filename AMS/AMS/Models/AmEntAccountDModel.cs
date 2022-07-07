using Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AMS.Models
{
    public class AmEntAccountDModel
    {
        public AmEntAccountD AmEntAccountD { get; set; }

        public IEnumerable<AmEntAccountD> AmEntAccountDList { get; set; }

        [Display(Name = "戶名")]
        [Required]
        public string AcctTitle
        {
            get { return this.AmEntAccountD.AcctTitle; }
            set { value = this.AmEntAccountD.AcctTitle; }
        }

        [Display(Name = "銀行代號")]
        [Required]
        public string BankNo
        {
            get { return this.AmEntAccountD.BankNo; }
            set { value = this.AmEntAccountD.BankNo; }
        }

        [Display(Name = "銀行帳號")]
        [Required]
        public string BankAcct
        {
            get { return this.AmEntAccountD.BankAcct; }
            set { value = this.AmEntAccountD.BankAcct; }
        }

        [Display(Name = "代碼")]
        [Required]
        public string AcctCode
        {
            get { return this.AmEntAccountD.AcctCode; }
            set { value = this.AmEntAccountD.AcctCode; }
        }

        [Display(Name = "統一編號")]
        [Required]
        public string CompanyId
        {
            get { return this.AmEntAccountD.CompanyId; }
            set { value = this.AmEntAccountD.CompanyId; }
        }

        [Display(Name = "所屬部門")]
        [Required]
        public string Department
        {
            get { return this.AmEntAccountD.Department; }
            set { value = this.AmEntAccountD.Department; }
        }

        [Display(Name = "聯絡人姓名")]
        [Required]
        public string Name
        {
            get { return this.AmEntAccountD.Name; }
            set { value = this.AmEntAccountD.Name; }
        }

        [Display(Name = "經辦人")]
        [Required]
        public string Operator
        {
            get { return this.AmEntAccountD.Operator; }
            set { value = this.AmEntAccountD.Operator; }
        }

        [Display(Name = "備註")]
        public string Remark
        {
            get { return this.AmEntAccountD.Remark; }
            set { value = this.AmEntAccountD.Remark; }
        }

        [Display(Name = "狀態")]
        [Required]
        public string Status
        {
            get { return this.AmEntAccountD.Status; }
            set { value = this.AmEntAccountD.Status; }
        }

        [Display(Name = "更新時間")]
        [Required]
        public string UpdateDate
        {
            get { return this.AmEntAccountD.UpdateDate; }
            set { value = this.AmEntAccountD.UpdateDate; }
        }
    }
}