using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OCG.ResourceManagement.ObjectModel
{
    public class RmBinary : IComparable, IComparable<RmBinary>
    {
        byte[] value;
        public RmBinary()
        {
            this.value = new byte[0];
        }
        public RmBinary(String value)
        {
            this.Value = value;
        }
        /// <summary>
        /// The string must be supported by the DateTime class
        /// </summary>
        public String Value
        {
            get
            {
                return this.ToString();
            }
            set
            {
                if (value != null)
                {
                    this.value = UnicodeEncoding.UTF8.GetBytes(value);
                }
                else
                {
                    this.value = new byte[0];
                }
            }
        }

        public byte[] ValueAsBinary
        {
            get
            {
                return this.value;
            }
        }

        public override bool Equals(object obj)
        {
            RmBinary other = obj as RmBinary;
            if (other as Object == null)
                return false;
            else
                return this.ToString().Equals(other.ToString());
        }

        public override int GetHashCode()
        {
            return this.ToString().GetHashCode();
        }

        public override string ToString()
        {
            return UnicodeEncoding.UTF8.GetString(this.value);
        }



        #region IComparable Members

        public int CompareTo(object obj)
        {
            if (obj as Object == null)
                throw new ArgumentNullException("obj");
            return this.CompareTo(obj as RmBinary);
        }

        public static bool operator ==(RmBinary attrib1, RmBinary attrib2)
        {
            if (attrib1 as Object == null)
                return false;
            if (attrib2 as Object == null)
                return false;
            return attrib1.CompareTo(attrib2) == 0;
        }

        public static bool operator !=(RmBinary attrib1, RmBinary attrib2)
        {
            if (attrib1 == null)
                return false;
            return attrib1.CompareTo(attrib2) != 0;
        }

        public static bool operator <(RmBinary attrib1, RmBinary attrib2)
        {
            if (attrib1 == null)
                return false;
            return attrib1.CompareTo(attrib2) < 0;
        }

        public static bool operator >(RmBinary attrib1, RmBinary attrib2)
        {
            if (attrib1 == null)
                return false;
            return attrib1.CompareTo(attrib2) > 0;
        }

        public static bool operator <=(RmBinary attrib1, RmBinary attrib2)
        {
            if (attrib1 == null)
                return false;
            return attrib1.CompareTo(attrib2) <= 0;
        }

        public static bool operator >=(RmBinary attrib1, RmBinary attrib2)
        {
            if (attrib1 == null)
                return false;
            return attrib1.CompareTo(attrib2) >= 0;
        }

        #endregion

        #region IComparable<RmDateTime> Members

        public int CompareTo(RmBinary other)
        {
            if (other == null)
                throw new ArgumentNullException("other");
            else
                return this.ToString().CompareTo(other.ToString());

        }

        #endregion
    }
}
