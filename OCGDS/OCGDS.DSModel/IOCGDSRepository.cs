﻿using System;
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
    }
}
