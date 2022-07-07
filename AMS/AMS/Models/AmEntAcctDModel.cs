using Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AMS.Models
{
    public class AmEntAcctDModel
    {
        public AmEntAcctD AmEntAccountD { get; set; }

        public IEnumerable<AmEntAcctD> AmEntAccountDList { get; set; }

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

        [Display(Name = "聯絡人姓名")]
        [Required]
        public string Name
        {
            get { return this.AmEntAccountD.Name; }
            set { value = this.AmEntAccountD.Name; }
        }

        [Display(Name = "統一編號")]
        [Required]
        public string Reg_Id
        {
            get { return this.AmEntAccountD.Reg_Id; }
            set { value = this.AmEntAccountD.Reg_Id; }
        }

        [Display(Name = "電話號碼")]
        [Required]
        public string Tel
        {
            get { return this.AmEntAccountD.Tel; }
            set { value = this.AmEntAccountD.Tel; }
        }

        [Display(Name = "傳真號碼")]
        [Required]
        public string Fax
        {
            get { return this.AmEntAccountD.Fax; }
            set { value = this.AmEntAccountD.Fax; }
        }

        [Display(Name = "Email")]
        [Required]
        public string Email
        {
            get { return this.AmEntAccountD.Email; }
            set { value = this.AmEntAccountD.Email; }
        }

        [Display(Name = "入帳通知處理方式")]
        public string Inform_Flg
        {
            get { return this.AmEntAccountD.Inform_Flg; }
            set { value = this.AmEntAccountD.Inform_Flg; }
        }

        [Display(Name = "對應特約機構")]
        [Required]
        public string Merchant_No
        {
            get { return this.AmEntAccountD.Merchant_No; }
            set { value = this.AmEntAccountD.Merchant_No; }
        }

    }
}