using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AMS.Models;

namespace AMS.ViewModels
{
    public class ApplicationItemSums
    {
        public GM_ADJ_APPLICATION ADJApplication { get; set; }
        public IEnumerable<Tuple<int,string,int,decimal,string,int>> ARCountAmount { get; set; }  //項次、調整項目、筆數、金額、備註

        public IEnumerable<Tuple<int,string,int,decimal,string,int>> APCountAmount { get; set; }
    }
}