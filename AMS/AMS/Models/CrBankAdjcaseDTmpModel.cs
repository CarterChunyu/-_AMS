using Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AMS.Models
{
    public class CrBankAdjcaseDTmpModel
    {
        public CrBankAdjcaseDTmp CrBankAdjcaseDTmp { get; set; }

        public IEnumerable<CrBankAdjcaseDTmp> CrBankAdjcaseDTmpList { get; set; }

        [Display(Name = "案件編號")]
        [Required]
        public string AdjCaseNo
        {
            get { return this.CrBankAdjcaseDTmp.AdjCaseNo; }
            set { value = this.CrBankAdjcaseDTmp.AdjCaseNo; }
        }

        [Display(Name = "銀行統編")]
        [Required]
        public string BankMerchant
        {
            get { return this.CrBankAdjcaseDTmp.BankMerchant; }
            set { value = this.CrBankAdjcaseDTmp.BankMerchant; }
        }

        [Display(Name = "卡號")]
        [Required]
        public string IccNo
        {
            get { return this.CrBankAdjcaseDTmp.IccNo; }
            set { value = this.CrBankAdjcaseDTmp.IccNo; }
        }

        [Display(Name = "調帳金額")]
        [Required]
        public string AdjAmt
        {
            get { return this.CrBankAdjcaseDTmp.AdjAmt; }
            set { value = this.CrBankAdjcaseDTmp.AdjAmt; }
        }

        [Display(Name = "更新日期")]
        [Required]
        public string UptDatetime
        {
            get { return this.CrBankAdjcaseDTmp.UptDatetime; }
            set { value = this.CrBankAdjcaseDTmp.UptDatetime; }
        }

        [Display(Name = "建立者")]
        [Required]
        public string CreateUser
        {
            get { return this.CrBankAdjcaseDTmp.CreateUser; }
            set { value = this.CrBankAdjcaseDTmp.CreateUser; }
        }

        [Display(Name = "更新者")]
        [Required]
        public string UpdateUser
        {
            get { return this.CrBankAdjcaseDTmp.UpdateUser; }
            set { value = this.CrBankAdjcaseDTmp.UpdateUser; }
        }
    }
}