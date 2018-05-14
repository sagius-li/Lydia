using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OCG.ResourceManagement.PortalServiceModel
{
    public class PsAttributeDef
    {
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public bool IsMultivalued { get; set; }
        public bool IsRequired { get; set; }

        public PsAttributeDef()
        {
            DisplayName = string.Empty;
            Description = string.Empty;
            Name = string.Empty;
            Type = string.Empty;
            IsMultivalued = false;
            IsRequired = false;
        }
    }
}
