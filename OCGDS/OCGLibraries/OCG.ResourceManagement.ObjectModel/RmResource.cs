using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OCG.ResourceManagement.ObjectModel
{
    public class RmResource : RmGeneric
    {
        public RmResource()
        {
            this.EnsureAllAttributesExist();

            this.ObjectType = this.GetResourceType();
        }

        protected virtual void EnsureAllAttributesExist()
        {
            this.EnsureAttributeExists(AttributeNames.CreatedTime);
            this.EnsureAttributeExists(AttributeNames.Creator);
            this.EnsureAttributeExists(AttributeNames.DeletedTime);
            this.EnsureAttributeExists(AttributeNames.Description);
            this.EnsureAttributeExists(AttributeNames.DetectedRulesList);
            this.EnsureAttributeExists(AttributeNames.DisplayName);
            this.EnsureAttributeExists(AttributeNames.ExpectedRulesList);
            this.EnsureAttributeExists(AttributeNames.ExpirationTime);
            this.EnsureAttributeExists(AttributeNames.Locale);
            this.EnsureAttributeExists(AttributeNames.MVObjectID);
            this.EnsureAttributeExists(AttributeNames.ObjectID);
            this.EnsureAttributeExists(AttributeNames.ObjectType);
            this.EnsureAttributeExists(AttributeNames.ResourceTime);
        }

        public virtual String GetResourceType()
        {
            return @"Resource";
        }

        #region promoted properties

        public RmReference ObjectID
        {
            get
            {
                return GetReference(AttributeNames.ObjectID);
            }
            set
            {
                this.attributes[AttributeNames.ObjectID].Value = value;
            }
        }

        public String ObjectType
        {
            get
            {
                return GetString(AttributeNames.ObjectType);
            }
            set
            {
                this[AttributeNames.ObjectType].Value = value;
            }
        }

        public String DisplayName
        {
            get
            {
                return GetString(AttributeNames.DisplayName);
            }
            set
            {
                this[AttributeNames.DisplayName].Value = value;
            }
        }

        public String Locale
        {
            get
            {
                return GetString(AttributeNames.Locale);
            }
            set
            {
                this[AttributeNames.Locale].Value = value;
            }
        }

        public String Description
        {
            get
            {
                return GetString(AttributeNames.Description);
            }
            set
            {
                this[AttributeNames.Description].Value = value;
            }
        }

        #endregion

        protected void EnsureAttributeExists(RmAttributeName attributeName)
        {
            EnsureNotDisposed();
            lock (this.attributes)
            {
                if (attributeName == null)
                {
                    throw new ArgumentNullException("attributeName");
                }
                if (this.attributes.ContainsKey(attributeName))
                {
                    return;
                }
                else
                {
                    this.attributes.Add(attributeName, new RmAttributeValue());
                }
            }
        }

        #region Object

        public override bool Equals(object obj)
        {
            RmResource other = obj as RmResource;
            if (other == null)
            {
                return false;
            }
            else
            {
                if (this.attributes.Count != other.attributes.Count)
                {
                    return false;
                }
                foreach (KeyValuePair<RmAttributeName, RmAttributeValue> item in this.attributes)
                {
                    RmAttributeValue otherValue = null;
                    other.TryGetValue(item.Key, out otherValue);
                    if (otherValue == null)
                        return false;
                    if (item.Value.Equals(otherValue) == false)
                        return false;
                }
                return true;
            }
        }

        public override int GetHashCode()
        {
            return this.ObjectID.GetHashCode();
        }

        public override string ToString()
        {
            return String.Format("{0}:{1}",
                this.ObjectType,
                this.ObjectID.ToString()
            );
        }

        #endregion

        public sealed class AttributeNames
        {
            public static RmAttributeName CreatedTime = new RmAttributeName(@"CreatedTime");
            public static RmAttributeName Creator = new RmAttributeName(@"Creator");
            public static RmAttributeName DeletedTime = new RmAttributeName(@"DeletedTime");
            public static RmAttributeName Description = new RmAttributeName(@"Description");
            public static RmAttributeName DetectedRulesList = new RmAttributeName(@"DetectedRulesList");
            public static RmAttributeName DisplayName = new RmAttributeName(@"DisplayName");
            public static RmAttributeName ExpectedRulesList = new RmAttributeName(@"ExpectedRulesList");
            public static RmAttributeName ExpirationTime = new RmAttributeName(@"ExpirationTime");
            public static RmAttributeName Locale = new RmAttributeName(@"Locale");
            public static RmAttributeName MVObjectID = new RmAttributeName(@"MVObjectID");
            public static RmAttributeName ObjectID = new RmAttributeName(@"ObjectID");
            public static RmAttributeName ObjectType = new RmAttributeName(@"ObjectType");
            public static RmAttributeName ResourceTime = new RmAttributeName(@"ResourceTime");
        }
    }
}
