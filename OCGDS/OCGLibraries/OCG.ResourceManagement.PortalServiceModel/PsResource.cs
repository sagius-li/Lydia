using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OCG.ResourceManagement.PortalServiceModel
{
    public class PsResourcePost
    {
        public PsResource Resource { get; set; }
        public string Connection { get; set; }
    }

    public class PsResource
    {
        private Dictionary<string, PsAttribute> attributes = new Dictionary<string, PsAttribute>();

        public string DisplayName { get; set; }
        public string ObjectType { get; set; }
        public string ObjectID { get; set; }

        public Dictionary<string, PsAttribute> Attributes
        {
            get
            {
                return attributes;
            }
            set
            {
                attributes = value;
            }
        }

        public PsResource()
        {
            DisplayName = string.Empty;
            ObjectType = string.Empty;
            ObjectID = string.Empty;
        }

        public bool AttributeIsPresent(string attName)
        {
            if (attributes.ContainsKey(attName))
            {
                if (attributes[attName] != null && !string.IsNullOrEmpty(attributes[attName].Value))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
