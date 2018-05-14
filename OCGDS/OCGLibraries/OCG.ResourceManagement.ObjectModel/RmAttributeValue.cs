using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OCG.ResourceManagement.ObjectModel
{
    public class RmAttributeValue : ICloneable
    {
        List<IComparable> values;
        bool isMultiValue;

        public RmAttributeValue()
        {
            this.values = new List<IComparable>();
        }

        public RmAttributeValue(IComparable value)
        {
            this.values = new List<IComparable>();
            if (value != null)
                this.values.Add(value);
        }

        public RmAttributeValue(IEnumerable<IComparable> values)
        {
            this.values = new List<IComparable>();
            this.values.AddRange(values);
            this.isMultiValue = true;
        }

        public List<IComparable> Values
        {
            get
            {
                return this.values;
            }
            set
            {
                lock (this.values)
                {
                    this.values = value;
                }
            }
        }
        public IComparable Value
        {
            get
            {
                if (this.values.Count > 0)
                {
                    return this.values[this.values.Count - 1];
                }
                else
                {
                    return null;
                }
            }
            set
            {
                lock (this.values)
                {
                    if (this.values.Count > 0)
                    {
                        if (value == null)
                        {
                            this.values.Clear();
                        }
                        else
                        {
                            this.values.Add(value);
                        }
                    }
                    else
                    {
                        this.values.Add(value);
                    }
                }
            }
        }

        public bool IsMultiValue
        {
            get
            {
                return this.isMultiValue || this.values.Count > 1;
            }
            set
            {
                this.isMultiValue = value;
            }
        }

        public bool IsReadOnly { get; set; }

        public bool IsRequired { get; set; }

        #region ICloneable Members

        public object Clone()
        {
            RmAttributeValue newValue = new RmAttributeValue();
            newValue.values = new List<IComparable>();
            foreach (IComparable value in this.values)
            {
                ICloneable cloneValue = value as ICloneable;
                if (cloneValue == null)
                {
                    newValue.values.Add(value);
                }
                else
                {
                    IComparable cloneInsert = cloneValue.Clone() as IComparable;
                    if (cloneInsert == null)
                        throw new InvalidOperationException("A comparable, when cloned, returned a non-comparable: " + cloneValue.ToString());
                    newValue.values.Add(cloneInsert);
                }
            }
            return newValue;
        }

        #endregion

        public override bool Equals(object obj)
        {
            RmAttributeValue other = obj as RmAttributeValue;
            if (other == null)
                return false;
            lock (this.values)
            {
                lock (other.values)
                {
                    if (this.values.Count != other.values.Count)
                        return false;
                    this.values.Sort();
                    other.values.Sort();
                    for (int i = 0; i < this.values.Count; i++)
                    {
                        if (this.values[i].Equals(other.values[i]) == false)
                            return false;
                    }
                }
            }
            return true;
        }

        public override int GetHashCode()
        {
            return this.ToString().GetHashCode();
        }

        public override string ToString()
        {
            lock (this.values)
            {
                if (this.values.Count == 0)
                {
                    return @"RmAttributeValue.Null";
                }
                else if (this.values.Count == 1)
                {
                    return this.values[0].ToString();
                }
                else
                {
                    StringBuilder sb = new StringBuilder(this.values.Count);
                    foreach (Object v in this.values)
                    {
                        sb.Append("{");
                        sb.Append(v);
                        sb.Append("}");
                        sb.Append(@" ");
                    }
                    // take off the last space
                    if (sb.Length > 0)
                    {
                        sb.Remove(sb.Length - 1, 1);
                    }
                    return sb.ToString();
                }
            }
        }
    }
}
