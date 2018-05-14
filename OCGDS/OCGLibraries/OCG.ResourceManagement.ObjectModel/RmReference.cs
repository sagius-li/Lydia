using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OCG.ResourceManagement.ObjectModel
{
    public class RmReference : IComparable, IComparable<RmReference>
    {
        Guid guidValue;
        String stringValue;
        public RmReference()
        {
            this.Value = String.Empty;
        }
        public RmReference(String value)
        {
            this.Value = value;
        }

        static String RemoveReference(String input)
        {
            if (String.IsNullOrEmpty(input))
                return Guid.Empty.ToString();
            else
                return input.Replace(@"urn:uuid:", String.Empty);
        }

        static String AddReference(String input)
        {
            if (String.IsNullOrEmpty(input))
                return null;
            else
                //return String.Format(@"urn:uuid:{0}", this.value.ToString());
                return input;
        }

        public String Value
        {
            get
            {
                return this.stringValue;
            }
            set
            {
                try
                {
                    this.guidValue = new Guid(RmReference.RemoveReference(value));
                    this.stringValue = RmReference.AddReference(this.guidValue.ToString());
                }
                catch (FormatException)
                {
                    throw new ArgumentException("The provided value did not match the reference format of urn:uuid:{guid}");
                }
            }
        }

        public override string ToString()
        {
            return this.stringValue;
        }

        public override bool Equals(object obj)
        {
            RmReference other = obj as RmReference;
            if (other as Object == null || other.guidValue == null)
                return false;
            else
                return other.guidValue.Equals(this.guidValue);
        }
        public override int GetHashCode()
        {
            return this.guidValue.GetHashCode();
        }

        #region IComparable Members

        public int CompareTo(object obj)
        {
            if (obj as Object == null)
                throw new ArgumentNullException("obj");
            RmReference reference = obj as RmReference;
            if (reference as Object == null)
                throw new ArgumentNullException("obj");
            return this.CompareTo(reference);
        }

        public static bool operator ==(RmReference attrib1, RmReference attrib2)
        {
            if (attrib1 as Object == null && attrib2 as Object == null)
                return true;
            if (attrib1 as Object == null)
                return false;
            if (attrib2 as Object == null)
                return false;
            return attrib1.CompareTo(attrib2) == 0;
        }

        public static bool operator !=(RmReference attrib1, RmReference attrib2)
        {
            if (attrib1 as Object == null && attrib2 as Object == null)
                return false;
            if (attrib1 as Object == null)
                return true;
            if (attrib2 as Object == null)
                return true;
            return attrib1.CompareTo(attrib2) != 0;
        }

        public static bool operator <(RmReference attrib1, RmReference attrib2)
        {
            if (attrib1 as object == null)
                return false;
            return attrib1.CompareTo(attrib2) < 0;
        }

        public static bool operator >(RmReference attrib1, RmReference attrib2)
        {
            if (attrib1 as object == null)
                return false;
            return attrib1.CompareTo(attrib2) > 0;
        }

        public static bool operator <=(RmReference attrib1, RmReference attrib2)
        {
            if (attrib1 as object == null)
                return false;
            return attrib1.CompareTo(attrib2) <= 0;
        }

        public static bool operator >=(RmReference attrib1, RmReference attrib2)
        {
            if (attrib1 as object == null)
                return false;
            return attrib1.CompareTo(attrib2) >= 0;
        }

        #endregion

        #region IComparable<RmReference> Members

        public int CompareTo(RmReference other)
        {
            if (other as object == null)
                throw new ArgumentNullException("other");
            else
                return this.guidValue.CompareTo(other.guidValue);
        }

        #endregion
    }
}
