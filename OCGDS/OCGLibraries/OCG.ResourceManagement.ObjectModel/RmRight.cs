using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OCG.ResourceManagement.ObjectModel
{
    public class RmRight
    {
        public string ActionName;

        public string AttributeName;

        public bool IsMultivalue;

        public int AttributeID;

        public RmRight()
        {
            ActionName = string.Empty;
            AttributeName = string.Empty;
            IsMultivalue = false;
        }
    }
}
