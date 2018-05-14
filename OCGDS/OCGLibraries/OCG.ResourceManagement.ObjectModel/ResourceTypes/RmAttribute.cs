using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OCG.ResourceManagement.ObjectModel.ResourceTypes
{
    public class RmAttribute : RmResource
    {
        public RmAttribute()
            : base()
        {

        }

        protected const String ResourceType = @"AttributeTypeDescription";

        protected override void EnsureAllAttributesExist()
        {
            base.EnsureAllAttributesExist();

            base.EnsureAttributeExists(AttributeNames.Name);
            base.EnsureAttributeExists(AttributeNames.DataType);
            base.EnsureAttributeExists(AttributeNames.Multivalued);
        }

        public override string GetResourceType()
        {
            return RmAttribute.ResourceType;
        }

        public static string StaticResourceType()
        {
            return RmAttribute.ResourceType;
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

        public String DataType
        {
            get
            {
                return GetString(AttributeNames.DataType);
            }
            set
            {
                base[AttributeNames.DataType].Value = value;
            }
        }

        public Boolean Multivalued
        {
            get
            {
                return GetBoolean(AttributeNames.Multivalued);
            }
            set
            {
                base[AttributeNames.Multivalued].Value = value;
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

        public string RegEx;

        #endregion

        public sealed new class AttributeNames
        {
            public static RmAttributeName Name = new RmAttributeName(@"Name");
            public static RmAttributeName DataType = new RmAttributeName(@"DataType");
            public static RmAttributeName Multivalued = new RmAttributeName(@"Multivalued");
            public static RmAttributeName UsageKeyword = new RmAttributeName(@"UsageKeyword");
        }
    }
}
