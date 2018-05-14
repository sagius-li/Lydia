using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OCG.ResourceManagement.PortalServiceModel
{
    public class PsAttribute
    {
        private string v = null;
        private List<string> values = new List<string>();

        private string resolvedValue = null;
        private List<string> resolvedValues = new List<string>();

        private List<string> resolvedTypes = new List<string>();

        public string DisplayName { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public bool IsMultivalued { get; set; }
        public bool IsRequired { get; set; }
        public string ResolvedType { get; set; }

        public string Value
        {
            get
            {
                if (string.IsNullOrEmpty(v))
                {
                    if (values.Count > 0)
                    {
                        return values.FirstOrDefault();
                    }
                }

                return v;
            }
            set
            {
                v = value;
            }
        }

        public List<string> Values
        {
            get
            {
                return values;
            }
            set
            {
                values = value;
                if (values != null)
                {
                    Value = values.FirstOrDefault();
                }
            }
        }

        public string ResolvedValue
        {
            get
            {
                if (string.IsNullOrEmpty(v))
                {
                    if (resolvedValues.Count > 0)
                    {
                        return resolvedValues.FirstOrDefault();
                    }
                }

                return resolvedValue;
            }
            set
            {
                resolvedValue = value;
            }
        }

        public List<string> ResolvedValues
        {
            get
            {
                return resolvedValues;
            }
            set
            {
                resolvedValues = value;
                if (resolvedValues != null)
                {
                    resolvedValue = resolvedValues.FirstOrDefault();
                }
            }
        }

        public List<string> ResolvedTypes
        {
            get
            {
                return resolvedTypes;
            }
            set
            {
                resolvedTypes = value;
            }
        }

        public PsAttribute()
        {
            DisplayName = string.Empty;
            Description = string.Empty;
            Name = string.Empty;
            Type = string.Empty;
            IsMultivalued = false;
            IsRequired = false;
            Value = string.Empty;
            ResolvedValue = string.Empty;
        }
    }
}
