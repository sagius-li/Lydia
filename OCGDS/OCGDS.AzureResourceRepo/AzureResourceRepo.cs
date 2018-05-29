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
    }
}
