using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OCG.ResourceManagement.PortalServiceModel
{
    public class PsRight
    {
        public string ActionName { get; set; }

        public string AttributeName { get; set; }

        public bool IsMultivalue { get; set; }

        public int AttributeID { get; set; }

        public PsRight()
        {
            ActionName = string.Empty;
            AttributeName = string.Empty;
            IsMultivalue = false;
            AttributeID = 0;
        }
    }
}
