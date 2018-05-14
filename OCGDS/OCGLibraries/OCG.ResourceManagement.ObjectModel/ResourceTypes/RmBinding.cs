using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OCG.ResourceManagement.ObjectModel.ResourceTypes
{
    public class RmBinding : RmResource
    {
        public RmBinding()
            : base()
        {

        }

        protected const String ResourceType = @"BindingDescription";

        protected override void EnsureAllAttributesExist()
        {
            base.EnsureAllAttributesExist();

            base.EnsureAttributeExists(AttributeNames.BoundObjectType);
            base.EnsureAttributeExists(AttributeNames.BoundAttributeType);
            base.EnsureAttributeExists(AttributeNames.Required);
            base.EnsureAttributeExists(AttributeNames.UsageKeyword);
        }

        public override string GetResourceType()
        {
            return RmBinding.ResourceType;
        }

        public static string StaticResourceType()
        {
            return RmBinding.ResourceType;
        }

        #region promoted properties

        public RmReference BoundObjectType
        {
            get
            {
                return GetReference(AttributeNames.BoundObjectType);
            }
            set
            {
                base[AttributeNames.BoundObjectType].Value = value;
            }
        }

        public RmReference BoundAttributeType
        {
            get
            {
                return GetReference(AttributeNames.BoundAttributeType);
            }
            set
            {
                base[AttributeNames.BoundAttributeType].Value = value;
            }
        }

        public Boolean Required
        {
            get
            {
                return GetBoolean(AttributeNames.Required);
            }
            set
            {
                base[AttributeNames.Required].Value = value;
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
            public static RmAttributeName BoundObjectType = new RmAttributeName(@"BoundObjectType");
            public static RmAttributeName BoundAttributeType = new RmAttributeName(@"BoundAttributeType");
            public static RmAttributeName Required = new RmAttributeName(@"Required");
            public static RmAttributeName UsageKeyword = new RmAttributeName(@"UsageKeyword");
        }
    }
}
