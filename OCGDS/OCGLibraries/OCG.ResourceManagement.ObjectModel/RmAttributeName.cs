using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using System.Text;

namespace OCG.ResourceManagement.ObjectModel
{
    public class RmAttributeName : IComparable, ICloneable
    {
        string name;
        CultureInfo culture;
        string key;
        public RmAttributeName(String name)
            : this(name, null)
        {
        }

        public RmAttributeName(String name, CultureInfo culture)
        {
            if (String.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("name");
            }
            this.name = name;
            this.culture = culture;
            ComputeKey();
        }
        public string Name
        {
            get
            {
                return this.name;
            }
            set
            {
                if (String.IsNullOrEmpty(value))
                {
                    throw new ArgumentNullException("value");
                }
                this.name = value;
                ComputeKey();
            }
        }
        public CultureInfo Culture
        {
            get
            {
                return this.culture;
            }
            set
            {
                this.culture = value;
                ComputeKey();
            }
        }

        void ComputeKey()
        {
            if (this.culture == null)
            {
                this.key = this.name;
            }
            else
            {
                this.key = String.Format(System.Globalization.CultureInfo.InvariantCulture, "{0};{1}", this.name, this.culture.ToString());
            }
        }

        #region Object
        public override bool Equals(object obj)
        {
            RmAttributeName other = obj as RmAttributeName;
            if (other as Object == null)
            {
                return false;
            }
            else
            {
                return this.key.Equals(other.key);
            }
        }

        public override int GetHashCode()
        {
            return this.key.GetHashCode();
        }

        public override string ToString()
        {
            return this.key;
        }
        #endregion

        #region IComparable Members

        public int CompareTo(object obj)
        {
            return String.Compare(key, obj as String, StringComparison.InvariantCulture);
        }

        public static bool operator ==(RmAttributeName attrib1, RmAttributeName attrib2)
        {
            if (attrib1 as Object == null)
                return false;
            return attrib1.CompareTo(attrib2) == 0;
        }

        public static bool operator !=(RmAttributeName attrib1, RmAttributeName attrib2)
        {
            if (attrib1 == null)
                return false;
            return attrib1.CompareTo(attrib2) != 0;
        }

        public static bool operator <(RmAttributeName attrib1, RmAttributeName attrib2)
        {
            if (attrib1 == null)
                return false;
            return attrib1.CompareTo(attrib2) < 0;
        }

        public static bool operator >(RmAttributeName attrib1, RmAttributeName attrib2)
        {
            if (attrib1 == null)
                return false;
            return attrib1.CompareTo(attrib2) > 0;
        }

        public static bool operator <=(RmAttributeName attrib1, RmAttributeName attrib2)
        {
            if (attrib1 == null)
                return false;
            return attrib1.CompareTo(attrib2) <= 0;
        }

        public static bool operator >=(RmAttributeName attrib1, RmAttributeName attrib2)
        {
            if (attrib1 == null)
                return false;
            return attrib1.CompareTo(attrib2) >= 0;
        }

        #endregion

        #region ICloneable Members

        public object Clone()
        {
            RmAttributeName newObject = new RmAttributeName(this.Name, this.Culture);
            return newObject;
        }

        #endregion
    }
}
