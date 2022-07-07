using DataAccess;
using Domain.ICASHOPOverdraft;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business
{
    public class ICASHOPOverdraftManager
    {
        public ICASHOPOverdraftDAO ICASHOPOverdraftDAO { get; set; }

        public ICASHOPOverdraftManager()
        {
            ICASHOPOverdraftDAO = new ICASHOPOverdraftDAO();
        }

        public List<OverdraftData> GetData(string REPORT_YM)
        {
            DataTable dt = this.ICASHOPOverdraftDAO.GetOverdraftData(REPORT_YM);
            List<OverdraftData> list = new List<OverdraftData>();
            if (dt.Rows.Count > 0)
            {
                foreach(DataRow irow in dt.Rows)
                {
                    list.Add(new OverdraftData {
                        CptDate = irow["CptDate"].ToString(),
                        UnifiedBusinessNo = irow["UnifiedBusinessNo"].ToString(),
                        StoreNo =irow["StoreNo"].ToString(),
                        PosNo=irow["PosNo"].ToString(),
                        TransNo =irow["TransNo"].ToString(),
                        TransDate = irow["TransDate"].ToString(),
                        CardType = irow["CardType"].ToString(),
                        CardNo =irow["CardNo"].ToString(),
                        PointType =irow["PointType"].ToString(),
                        OverdraftPoint = Convert.ToDecimal(irow["OverdraftPoint"].ToString()),
                        CorrectionPoint=Convert.ToDecimal(irow["CorrectionPoint"].ToString()),
                        WriteOffNo = irow["WriteOffNo"].ToString(),
                        CreateDate =Convert.ToDateTime( irow["CreateDate"].ToString()),
                        FileName = irow["FileName"].ToString()
                    });
                }
            }
            return list;
        }
    }

    
}
