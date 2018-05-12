using OCGDS.DSModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCGDS.MIMResourceRepo
{
    [Export("MIMResource", typeof(IOCGDSRepository))]
    public class MIMResourceRepo : IOCGDSRepository
    {
        public new string GetType()
        {
            return "MIM Resource Repository";
        }
    }
}
