using DataAccess;
using Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business
{
    public class GmCodeMappginManager
    {
        public GmCodeMappingDAO GmCodeMappingDAO { get; set; }

        public GmCodeMappginManager()
        {
            GmCodeMappingDAO = new GmCodeMappingDAO();
        }

        public List<GmCodeMapping> FindData(string mapping_group, string code_type, string input_value)
        {
            List<GmCodeMapping> itemList = new List<GmCodeMapping>();
            DataTable result = GmCodeMappingDAO.FindData(mapping_group, code_type, input_value);
            for (int i = 0; i < result.Rows.Count; i++)
            {
                itemList.Add(new GmCodeMapping()
                    {
                        MAPPING_GROUP = "" + result.Rows[i]["MAPPING_GROUP"],
                        CODE_TYPE = "" + result.Rows[i]["CODE_TYPE"],
                        INPUT_VALUE = "" + result.Rows[i]["INPUT_VALUE"],
                        OUTPUT_VALUE = "" + result.Rows[i]["OUTPUT_VALUE"],
                        OUTPUT_DESC = string.Format(@"{0} {1}", "" + result.Rows[i]["OUTPUT_VALUE"], "" + result.Rows[i]["OUTPUT_DESC"]),
                        NOTE = "" + result.Rows[i]["NOTE"],
                    });
            }

            return itemList;
        }
    }
}
