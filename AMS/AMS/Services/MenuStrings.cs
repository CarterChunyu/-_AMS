using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using AMS.Models;

namespace AMS.Services
{
    public static class MenuStrings
    {
        private static TRAADJEntities db = new TRAADJEntities();

        private static List<DRDPItem> majorList = new List<DRDPItem>();

        private static List<ARAPItem> minorList = new List<ARAPItem>();

        public static List<DRDPItem> DRCollection()
        {
            majorList.Clear();
            majorList =db.BM_TRANS_TYPE.Where(x => x.ARAP_ICASH=="DR").ToList().Select(y => new DRDPItem(y.FILE_TRANS_TYPE + "-" + y.FILE_SUB_TYPE, y.FILE_TRANS_DESC + "-" + y.SUB_TYPE_DESC)).ToList();
            return majorList;
        }

        public static List<DRDPItem> DPCollection()
        {
            majorList.Clear();
            majorList = db.BM_TRANS_TYPE.Where(x =>  x.ARAP_ICASH == "DP").ToList().Select(y => new DRDPItem(y.FILE_TRANS_TYPE + "-" + y.FILE_SUB_TYPE, y.FILE_TRANS_DESC + "-" + y.SUB_TYPE_DESC)).ToList();
            return majorList;
        }

        public static List<ARAPItem> ARAPCollection()
        {
            minorList.Clear();
            minorList = db.BM_TRANS_TYPE.Where(x => x.ARAP_ICASH == "AR" || x.ARAP_ICASH == "AP").ToList().Select(y => new ARAPItem(y.FILE_TRANS_TYPE + "-" + y.FILE_SUB_TYPE, y.FILE_TRANS_DESC + "-" + y.SUB_TYPE_DESC)).ToList();
            return minorList;
        }

        public static string ItemString(string transType, string fileTransType ,string fileSubType)
        {
            string resultString = db.BM_TRANS_TYPE.Where(x => x.TRANS_TYPE == transType && x.FILE_TRANS_TYPE == fileTransType && x.FILE_SUB_TYPE == fileSubType).Select(item => item.FILE_TRANS_DESC +"-"+ item.SUB_TYPE_DESC).FirstOrDefault();
            return resultString;
        }

       
        
    }
}