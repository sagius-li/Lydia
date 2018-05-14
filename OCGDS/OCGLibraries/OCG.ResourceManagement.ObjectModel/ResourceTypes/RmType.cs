using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OCG.ResourceManagement.ObjectModel.ResourceTypes
{
    public class RmType : RmResource
    {
        public RmType()
            : base()
        {

        }

        protected const String ResourceType = @"ObjectTypeDescription";

        protected override void EnsureAllAttributesExist()
        {
            base.EnsureAllAttributesExist();

            base.EnsureAttributeExists(AttributeNames.Name);
            base.EnsureAttributeExists(AttributeNames.UsageKeyword);
        }

        public override string GetResourceType()
        {
            return RmType.ResourceType;
        }

        public static string StaticResourceType()
        {
            return RmType.ResourceType;
        }

        #region promoted properties

        public String Name
        {
            get
            {
                return GetString(AttributeNames.Name);
            }
            set
            {
                base[AttributeNames.Name].Value = value;
            }
        }

        public List<String> UsageKeyword
        {
            get
            {
                return GetMultiValuedString(AttributeNames.UsageKeyword);
            }
            set
            {
                foreach (string item in value)
                {
                    base[AttributeNames.UsageKeyword].Values.Add(item);
                }
            }
        }

        #endregion

        public sealed new class AttributeNames
        {
            public static RmAttributeName Name = new RmAttributeName(@"Name");
            public static RmAttributeName UsageKeyword = new RmAttributeName(@"UsageKeyword");
        }
    }
}
