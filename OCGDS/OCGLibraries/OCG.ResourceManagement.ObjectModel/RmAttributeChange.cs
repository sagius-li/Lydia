using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OCG.ResourceManagement.ObjectModel
{
    public class RmAttributeChange
    {
        private RmAttributeName name;
        private IComparable attributeValue;
        private RmAttributeChangeOperation operation;

        internal RmAttributeChange(RmAttributeName name, IComparable atomicValue, RmAttributeChangeOperation operation)
        {
            this.name = name;
            this.attributeValue = atomicValue;
            this.operation = operation;
        }

        public RmAttributeName Name
        {
            get
            {
                return this.name;
            }
            set
            {
                this.name = value;
            }
        }

        public IComparable Value
        {
            get
            {
                return this.attributeValue;
            }
            set
            {
                this.attributeValue = value;
            }
        }

        public RmAttributeChangeOperation Operation
        {
            get
            {
                return this.operation;
            }
        }

        public override bool Equals(object obj)
        {
            RmAttributeChange other = obj as RmAttributeChange;
            if (other == null)
            {
                return false;
            }
            if (this.Name == null)
            {
                return false;
            }
            if (this.Name.Equals(other.Name) == false)
            {
                return false;
            }
            if (this.Value == null)
            {
                return other.Value == null;
            }
            else
            {
                return this.Value.Equals(other.Value);
            }
        }

        public override int GetHashCode()
        {
            if (this.attributeValue != null)
            {
                return this.attributeValue.GetHashCode();
            }
            else
            {
                return base.GetHashCode();
            }
        }

        public override string ToString()
        {
            return String.Format(System.Globalization.CultureInfo.InvariantCulture, "{0}:{1}", this.Name.ToString(), this.Value.ToString());
        }
    }

    public enum RmAttributeChangeOperation
    {
        None = 0,
        Add = 1,
        Delete = 2,
        Replace = 3
    }
}
