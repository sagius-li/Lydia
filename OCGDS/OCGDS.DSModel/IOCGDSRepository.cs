using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCGDS.DSModel
{
    public interface IOCGDSRepository
    {
        string GetType();

        DSResource GetResourceByID(
            string id, string[] attributes, bool includePermission = false, ResourceOption resourceOption = null);

        DSResourceSet GetResourceByQuery(
            string query, string[] attributes, int pageSize = 0, int index = 0, ResourceOption resourceOption = null);

        void DeleteResource(string id, ResourceOption resourceOption = null);

        string CreateResource(DSResource resource, ResourceOption resourceOption = null);

        string UpdateResource(DSResource resource, bool isDelta = false, ResourceOption resourceOption = null);
    }
}
