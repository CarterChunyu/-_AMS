using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class AmEntReceD : AbstractDO, ICloneable
    {
        public string Sn { get; set; }
        public string BankAcctTo { get; set; }
        public string ActDate { get; set; }
        public string ActTime { get; set; }
        public string ActSn { get; set; }
        public string BillNo { get; set; }
        public string Amt { get; set; }
        public string Bal { get; set; }
        public string ActStatus { get; set; }
        public string Summary { get; set; }
        public string BankAcctFrom { get; set; }
        public string CaseNo { get; set; }
        public string CheckStatus { get; set; }
        public string CheckStatusName { get; set; }
        public string AcctCheckStatus { get; set; }
        public string AcctCheckStatusName { get; set; }
        public string Operator { get; set; }
        public string Remark { get; set; }
        public string InvoiceNo { get; set; }
        public string InvoiceDate { get; set; }
        public string FileName { get; set; }
        public string CreateDateTime { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
