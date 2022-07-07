using Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AMS.Models
{
    public class AmEntReceDModel
    {
        public AmEntReceD AmEntReceD { get; set; }

        public IEnumerable<AmEntReceD> AmEntReceDList { get; set; }

        [Display(Name = "繳款編號")]
        [Required]
        public string Sn
        {
            get { return this.AmEntReceD.Sn; }
            set { value = this.AmEntReceD.Sn; }
        }

        [Display(Name = "帳號類型")]
        [Required]
        public string ReceType { get; set; }

        [Display(Name = "收款帳號")]
        [Required]
        public string BankAcctTo
        {
            get { return this.AmEntReceD.BankAcctTo; }
            set { value = this.AmEntReceD.BankAcctTo; }
        }

        [Display(Name = "交易日期")]
        [Required]
        public string ActDate
        {
            get { return this.AmEntReceD.ActDate; }
            set { value = this.AmEntReceD.ActDate; }
        }

        [Display(Name = "存入金額")]
        [Required]
        public string Amt
        {
            get { return this.AmEntReceD.Amt; }
            set { value = this.AmEntReceD.Amt; }
        }

        [Display(Name = "摘要")]
        [Required]
        public string Summary
        {
            get { return this.AmEntReceD.Summary; }
            set { value = this.AmEntReceD.Summary; }
        }

        [Display(Name = "案件編號")]
        [Required]
        public string CaseNo
        {
            get { return this.AmEntReceD.CaseNo; }
            set { value = this.AmEntReceD.CaseNo; }
        }

        [Display(Name = "核對狀態")]
        [Required]
        public string CheckStatus
        {
            get { return this.AmEntReceD.CheckStatus; }
            set { value = this.AmEntReceD.CheckStatus; }
        }

        [Display(Name = "會計核對狀態")]
        [Required]
        public string AcctCheckStatus
        {
            get { return this.AmEntReceD.AcctCheckStatus; }
            set { value = this.AmEntReceD.AcctCheckStatus; }
        }

        [Display(Name = "經辦人")]
        [Required]
        public string Operator
        {
            get { return this.AmEntReceD.Operator; }
            set { value = this.AmEntReceD.Operator; }
        }

        [Display(Name = "備註")]
        public string Remark
        {
            get { return this.AmEntReceD.Remark; }
            set { value = this.AmEntReceD.Remark; }
        }

        [Display(Name = "匯款帳號")]
        [Required]
        public string BankAcctFrom
        {
            get { return this.AmEntReceD.BankAcctFrom; }
            set { value = this.AmEntReceD.BankAcctFrom; }
        }

    }
}