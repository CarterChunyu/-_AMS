using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.CoBrandedSellSponsorship
{
    public class QueryYearBankDetailRes
    {
        public string Item { get; set; }
        public string PayTarget_ID { get; set; }
        public string PayTarget_Name { get; set; }
        public string Range_Date_B { get; set; }
        public string Range_Date_E { get; set; }
        public string Business_Invoice_Date { get; set; }
        public string Business_Invoice_No { get; set; }
        public int AMT_UnTax { get; set; }
        public int AMT_TaxIncluded { get; set; }
        public string Comment { get; set; }
        public string SendDate { get; set; }
        public string Account_Invoice_No { get; set; }
        public int AMT_Pay { get; set; }
    }
}
