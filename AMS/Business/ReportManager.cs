using DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business
{
    public class ReportManager
    {
        public ReportDAO ReportDAO { get; set; }

        public ReportManager()
        {
            ReportDAO = new ReportDAO();
        }

        public DataTable ReportA03(string start)
        {
            return this.ReportDAO.ReportA03(start);
        }

        public DataTable Report0111(string start)
        {
            return this.ReportDAO.Report0111(start);
        }

        public DataTable Report0111D(string start)
        {
            return this.ReportDAO.Report0111D(start);
        }

        public DataTable Report0211(string iccno, string store_no)
        {
            return this.ReportDAO.Report0211(iccno, store_no);
        }

        public DataTable Report0212T(string start)
        {
            return this.ReportDAO.Report0212T(start);
        }

        public DataTable Report0212_2T(string start)
        {
            return this.ReportDAO.Report0212_2T(start);
        }

        public DataTable Report0213T(string start, string end, string merchantNo, string SRC_FLG)
        {
            return this.ReportDAO.Report0213T(start, end, merchantNo, SRC_FLG);
        }

        public DataTable Report0214T(string start, string end, string merchantNo, string Kind)
        {
            return this.ReportDAO.Report0214T(start, end, merchantNo, Kind);
        }

        public DataTable Report0811(string start)
        {
            DataTable dt1 = this.ReportDAO.Report0811(start, "0011");
            DataTable dt2 = this.ReportDAO.Report0811(start, "0021");
            DataTable dt3 = this.ReportDAO.Report0811(start, "0022");
            DataTable dt4 = this.ReportDAO.Report0811(start, "0023");
            DataTable dt5 = this.ReportDAO.Report0811(start, "0024");

            dt1.Merge(dt2, true);
            dt1.Merge(dt3, true);
            dt1.Merge(dt4, true);
            dt1.Merge(dt5, true);

            return dt1;
        }

        public DataTable Report0112T(string start, string end, string groupId, string merchantNo)
        {
            return this.ReportDAO.Report0112T(start, end, groupId, merchantNo);
        }

        public DataTable Report0113T(string start, string end, string merchantNo)
        {
            return this.ReportDAO.Report0113T(start, end, merchantNo);
        }

        public DataTable Report0114T(string start, string end, string merchantNo)
        {
            return this.ReportDAO.Report0114T(start, end, merchantNo);
        }

        public DataTable ReportMerchant()
        {
            return this.ReportDAO.ReportMerchant();
        }

        public DataTable Report0115T(string start, string end, string merchantNo)
        {
            return this.ReportDAO.Report0115T(start, end, merchantNo);
        }

        public DataTable Report0116T(string start, string end, string merchantNo)
        {
            return this.ReportDAO.Report0116T(start, end, merchantNo);
        }

        public DataTable Report0117T(string yearMonth, string merchantNo)
        {
            return this.ReportDAO.Report0117T(yearMonth, merchantNo);
        }

        public DataTable Report0118T(string yearMonth, string merchantNo)
        {
            return this.ReportDAO.Report0118T(yearMonth, merchantNo);
        }

        public DataTable Report0119T(string start, string end, string StoreNo)
        {
            return this.ReportDAO.Report0119T(start, end, StoreNo);
        }

        public DataTable Report0120T(string start, string end)
        {
            return this.ReportDAO.Report0120T(start, end);
        }

        public DataTable Report0121T(string start)
        {
            return this.ReportDAO.Report0121T(start);
        }

        public DataTable Report0121_1T()
        {
            return this.ReportDAO.Report0121_1T();
        }

        public DataTable Report0122T(string start, string end, string merchantNo, string SRC_FLG)
        {
            return this.ReportDAO.Report0122T(start, end, merchantNo, SRC_FLG);
        }

        public DataTable Report0123T(string start, string end, string merchantNo, string Kind)
        {
            return this.ReportDAO.Report0123T(start, end, merchantNo, Kind);
        }

        public DataTable Report0124T(string startA, string endA, string startB, string endB)
        {
            return this.ReportDAO.Report0124T(startA, endA, startB, endB);
        }

        public DataTable Report0311T(string start, string end, string merchantNo)
        {
            return this.ReportDAO.Report0311T(start, end, merchantNo);
        }

        public DataTable Report0312T(string start, string end, string merchantNo, string merchantBankNo)
        {
            return this.ReportDAO.Report0312T(start, end, merchantNo, merchantBankNo);
        }

        public DataTable Report0313T(string start, string end)
        {
            return this.ReportDAO.Report0313T(start, end);
        }

        public DataTable Report0313SPT(string start, string end)
        {
            return this.ReportDAO.Report0313SPT(start, end);
        }

        public DataTable Report0314T(string start, string merchantNo, string MERC_GROUP)
        {
            return this.ReportDAO.Report0314T(start, merchantNo, MERC_GROUP);
        }

        public DataTable Report0315T(string start, string end, string merchantNo, string MERC_GROUP)
        {
            return this.ReportDAO.Report0315T(start, end, merchantNo, MERC_GROUP);
        }

        public DataTable Report0316T(string start, string merchantNo, string MERC_GROUP)
        {
            return this.ReportDAO.Report0316T(start, merchantNo, MERC_GROUP);
        }

        public DataTable Report0317T(string start, string merchantNo, string MERC_GROUP)
        {
            return this.ReportDAO.Report0317T(start, merchantNo, MERC_GROUP);
        }

        public DataTable Report0318T(string start, string end, string merchantNo, string MERC_GROUP)
        {
            return this.ReportDAO.Report0318T(start, end, merchantNo, MERC_GROUP);
        }

        public DataTable Report0319T(string start, string end, string merchantNo, string MERC_GROUP)
        {
            return this.ReportDAO.Report0319T(start, end, merchantNo, MERC_GROUP);
        }

        public DataTable Report0320T(string start, string end, string merchantNo, string MERC_GROUP)
        {
            return this.ReportDAO.Report0320T(start, end, merchantNo, MERC_GROUP);
        }

        public DataTable Report0321T(string start, string merchantNo, string MERC_GROUP)
        {
            return this.ReportDAO.Report0321T(start, merchantNo, MERC_GROUP);
        }

        public DataTable Report0322T(string start, string merchantNo, string MERC_GROUP)
        {
            return this.ReportDAO.Report0322T(start, merchantNo, MERC_GROUP);
        }

        public DataTable Report0323T(string start, string end, string merchantNo, string MERC_GROUP)
        {
            return this.ReportDAO.Report0323T(start, end, merchantNo, MERC_GROUP);
        }

        public DataTable Report0324T(string start, string end, string merchantNo, string MERC_GROUP)
        {
            return this.ReportDAO.Report0324T(start, end, merchantNo, MERC_GROUP);
        }

        public DataTable Report0325T(string start, string end,string merchantNo, string merchantBankNo, string MERC_GROUP, string PRT_TYPE)
        {
            return this.ReportDAO.Report0325T(start, end, merchantNo, merchantBankNo, MERC_GROUP, PRT_TYPE);
        }

        public DataTable Report0326T(string start, string end, string merchantBankNo, string ICC_NO, string RAMT_TYPE)
        {
            return this.ReportDAO.Report0326T(start, end, merchantBankNo, ICC_NO, RAMT_TYPE);
        }

        public DataTable Report0327T(string start)
        {
            return this.ReportDAO.Report0327T(start);
        }

        public DataTable Report0328T(string start, string end, string merchantNo, string merchantBankNo)
        {
            return this.ReportDAO.Report0328T(start, end, merchantNo, merchantBankNo);
        }

        public DataTable Report0329T(string start, string end, string merchantBankNo)
        {
            return this.ReportDAO.Report0329T(start, end, merchantBankNo);
        }


        public DataTable Report0411T(string start, string end, string merchantNo, string CARD_TYPE)
        {
            return this.ReportDAO.Report0411T(start, end, merchantNo, CARD_TYPE);
        }

        public DataTable Report0412T(string start, string end, string merchantNo)
        {
            return this.ReportDAO.Report0412T(start, end, merchantNo);
        }

        public DataTable Report151201T(string start,string end,string src)
        {
            return this.ReportDAO.Report151201T(start,end,src);
        }

        public DataTable Report151202T(string start,string end,string src)
        {
            return this.ReportDAO.Report151202T(start,end,src);
        }

        public DataTable Report151203T(string start, string end, string type, string code, string merchantNoCom)
        {
            return this.ReportDAO.Report151203T(start, end, type,code,merchantNoCom);
        }

        public DataTable Report151204T(string start, string end, string code)
        {
            return this.ReportDAO.Report151204T(start, end, code);
        }

        public DataTable Report151205T(string start, string end, string type, string code, string merchantNoCom)
        {
            return this.ReportDAO.Report151205T(start, end, type, code, merchantNoCom);
        }

        public DataTable Report151206T(string start, string end, string code)
        {
            return this.ReportDAO.Report151206T(start, end, code);
        }
        public DataTable Report170901T(string start, string end, string merchantNo)
        {
            return this.ReportDAO.Report170901(start, end, merchantNo);
        }
  

        public DataTable MerchantName(string merchantNo)
        {
            return this.ReportDAO.MerchantName(merchantNo);
        }

        public DataTable TypeName(string No, string f1, string f2, string table)
        {
            return this.ReportDAO.TypeName(No, f1, f2, table);
        }
        public DataTable Report200501T(string start, string end)
        {
            return this.ReportDAO.Report200501(start, end);
        }

        public DataTable Report200501T_2(string start, string end)
        {
            return this.ReportDAO.Report200501_2(start, end);
        }

        public DataTable Report200502T(string start)
        {
            return this.ReportDAO.Report200502(start);
        }

    }
}
