using OCGDS.DSModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCGDS.AzureResourceRepo
{
    [ExportMetadata("Name", "AzureResource")]
    [Export("ResourceManagement", typeof(IOCGDSRepository))]
    public class AzureResourceRepo : IOCGDSRepository
    {
        public new string GetType()
        {
            return "Azure Resource Repository";
        }

        public DSResource GetResourceByID(string id, string[] attributes, bool includePermission = false, ResourceOption resourceOption = null)
        {
            throw new NotImplementedException();
        }

        public DSResourceSet GetResourceByQuery(string query, string[] attributes, int pageSize = 0, int index = 0, ResourceOption resourceOption = null)
        {
            throw new NotImplementedException();
        }

        public void DeleteResource(string id, ResourceOption resourceOption = null)
        {
            throw new NotImplementedException();
        }

        public string CreateResource(DSResource resource, ResourceOption resourceOption = null)
        {
            throw new NotImplementedException();
        }

        public string UpdateResource(DSResource resource, bool isDelta = false, ResourceOption resourceOption = null)
        {
            throw new NotImplementedException();
        }
    }
}
