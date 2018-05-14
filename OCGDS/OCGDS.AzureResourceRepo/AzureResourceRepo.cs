using OCGDS.DSModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCGDS.AzureResourceRepo
{
    [Export("AzureResource", typeof(IOCGDSRepository))]
    public class AzureResourceRepo : IOCGDSRepository
    {
        public new string GetType()
        {
            return "Azure Resource Repository";
        }

        public DSResource GetResourceByID(ConnectionInfo info, string id, string[] attributes, bool getPermission, bool getResolved)
        {
            throw new NotImplementedException();
        }
    }
}
