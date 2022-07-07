using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Domain;
using DataAccess;

namespace Business
{
        public class GmSchemaObjManager
        {
            private GmSchemaObjDAO schemaDAO = new GmSchemaObjDAO();
 
            public GmSchemaObj FindSchemaObj(string merchantNo)
            {
                return schemaDAO.FindSchemaObj(merchantNo);
            }

            public List<GmSchemaObj> FindAllSchema()
            {
                return schemaDAO.FindAllSchema();
            }


    }
}
