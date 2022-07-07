using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AMS.Services
{
    public class DRDPItem
    {
        public DRDPItem(string transTypes,string typeDescs)
        {
            this.TransTypes = transTypes;
            this.TypeDescs = typeDescs;
        }
        public string TransTypes {get;set;}

        public string TypeDescs {get; set;}
    }

    public class ARAPItem
    {
        public ARAPItem(string transTypes,string typeDescs)
        {
            this.TransTypes = transTypes;
            this.TypeDescs = typeDescs;
        }
        public string TransTypes { get; set; }

        public string TypeDescs { get; set; }
    }
}