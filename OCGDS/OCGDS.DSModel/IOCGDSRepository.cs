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

        DSResource GetResourceByID(ConnectionInfo info, 
            string id, string[] attributes, bool getPermission, bool getResolved);
    }
}
