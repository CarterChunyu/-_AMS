using Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AMS.Models
{
    public class CrBankAdjcaseMTmpModel
    {
        public CrBankAdjcaseMTmp CrBankAdjcaseMTmp { get; set; }

        public IEnumerable<CrBankAdjcaseMTmp> CrBankAdjcaseMTmpList { get; set; }

        [Display(Name = "案件編號")]
        [Required]
        public string AdjCaseNo
        {
            get { return this.CrBankAdjcaseMTmp.AdjCaseNo; }
            set { value = this.CrBankAdjcaseMTmp.AdjCaseNo; }
        }

        [Display(Name = "調帳原因")]
        [Required]
        public string AdjCaseInfo
        {
            get { return this.CrBankAdjcaseMTmp.AdjCaseInfo; }
            set { value = this.CrBankAdjcaseMTmp.AdjCaseInfo; }
        }

        [Display(Name = "調帳說明")]
        [Required]
        public string AdjCaseContext
        {
            get { return this.CrBankAdjcaseMTmp.AdjCaseContext; }
            set { value = this.CrBankAdjcaseMTmp.AdjCaseContext; }
        }

        [Display(Name = "調帳日期")]
        [Required]
        public string AdjDate
        {
            get { return this.CrBankAdjcaseMTmp.AdjDate; }
            set { value = this.CrBankAdjcaseMTmp.AdjDate; }
        }

        [Display(Name = "清分日")]
        [Required]
        public string CptDate
        {
            get { return this.CrBankAdjcaseMTmp.CptDate; }
            set { value = this.CrBankAdjcaseMTmp.CptDate; }
        }

        [Display(Name = "預計匯款日")]
        [Required]
        public string RemittanceDate
        {
            get { return this.CrBankAdjcaseMTmp.RemittanceDate; }
            set { value = this.CrBankAdjcaseMTmp.RemittanceDate; }
        }

        [Display(Name = "調帳方向")]
        [Required]
        public string AdjFlag
        {
            get { return this.CrBankAdjcaseMTmp.AdjFlag; }
            set { value = this.CrBankAdjcaseMTmp.AdjFlag; }
        }

        [Display(Name = "狀態")]
        [Required]
        public string Status
        {
            get { return this.CrBankAdjcaseMTmp.Status; }
            set { value = this.CrBankAdjcaseMTmp.Status; }
        }

        [Display(Name = "更新日期")]
        [Required]
        public string UptDatetime
        {
            get { return this.CrBankAdjcaseMTmp.UptDatetime; }
            set { value = this.CrBankAdjcaseMTmp.UptDatetime; }
        }

        [Display(Name = "建立者")]
        [Required]
        public string CreateUser
        {
            get { return this.CrBankAdjcaseMTmp.CreateUser; }
            set { value = this.CrBankAdjcaseMTmp.CreateUser; }
        }

        [Display(Name = "更新者")]
        [Required]
        public string UpdateUser
        {
            get { return this.CrBankAdjcaseMTmp.UpdateUser; }
            set { value = this.CrBankAdjcaseMTmp.UpdateUser; }
        }
    }
}