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
    }
}
