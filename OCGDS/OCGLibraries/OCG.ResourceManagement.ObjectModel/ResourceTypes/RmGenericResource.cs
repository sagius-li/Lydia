using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OCG.ResourceManagement.ObjectModel.ResourceTypes
{
    public class RmGenericResource : RmResource
    {
        public RmGenericResource(RmResource resource)
            : base()
        {
            foreach (RmAttributeName key in resource.Attributes.Keys)
            {
                EnsureAttributeExists(key);
                if (this[key].IsMultiValue)
                {
                    this[key].Values = resource[key].Values;
                }
                else
                {
                    this[key].Value = resource[key].Value;
                }
            }
        }

        public string GetString(string name)
        {
            return base.GetString(new RmAttributeName(name));
        }

        public RmReference GetReference(string name)
        {
            return base.GetReference(new RmAttributeName(name));
        }

        public DateTime GetDateTime(string name)
        {
            return base.GetDateTime(new RmAttributeName(name));
        }

        public bool GetBoolean(string name)
        {
            return base.GetBoolean(new RmAttributeName(name));
        }

        public int GetInteger(string name)
        {
            return base.GetInteger(new RmAttributeName(name));
        }

        public List<string> GetMultiValuedString(string name)
        {
            return base.GetMultiValuedString(new RmAttributeName(name));
        }

        public RmList<RmReference> GetMultiValuedReference(string name)
        {
            return base.GetMultiValuedReference(new RmAttributeName(name));
        }
    }
}
