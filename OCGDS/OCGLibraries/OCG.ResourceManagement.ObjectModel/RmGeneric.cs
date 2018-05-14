using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OCG.ResourceManagement.ObjectModel
{
    public class RmGeneric : IDictionary<RmAttributeName, RmAttributeValue>, IDisposable
    {
        internal Dictionary<RmAttributeName, RmAttributeValue> attributes;

        public RmGeneric()
        {
            this.attributes = new Dictionary<RmAttributeName, RmAttributeValue>();
        }

        #region Get-Value Methods

        public Dictionary<RmAttributeName, RmAttributeValue> Attributes
        {
            get
            {
                return this.attributes;
            }
        }

        protected String GetString(RmAttributeName attributeName)
        {
            Object o = null;
            RmAttributeValue rma = null;
            this.TryGetValue(attributeName, out rma);
            if (rma != null)
                o = rma.Value;
            if (o == null)
            {
                return String.Empty;
            }
            else
            {
                return (String)o;
            }
        }

        protected RmReference GetReference(RmAttributeName attributeName)
        {
            IComparable o = null;
            RmAttributeValue rma = null;
            this.attributes.TryGetValue(attributeName, out rma);
            if (rma != null && rma.Value != null)
                o = rma.Value;
            return o as RmReference;
        }

        protected DateTime GetDateTime(RmAttributeName attributeName)
        {
            IComparable o = null;
            RmAttributeValue rma = null;
            this.attributes.TryGetValue(attributeName, out rma);
            if (rma != null && rma.Value != null)
            {
                o = rma.Value;
            }
            return Convert.ToDateTime(o);
        }

        protected bool GetBoolean(RmAttributeName attributeName)
        {
            Object o = null;
            RmAttributeValue rma = null;
            this.attributes.TryGetValue(attributeName, out rma);
            if (rma != null)
                o = rma.Value;
            if (o == null)
            {
                return false;
            }
            else
            {
                return (bool)o;
            }
        }

        protected int GetInteger(RmAttributeName attributeName)
        {
            Object o = null;
            RmAttributeValue rma = null;
            this.attributes.TryGetValue(attributeName, out rma);
            if (rma != null)
                o = rma.Value;
            if (o == null)
            {
                return 0;
            }
            else
            {
                return (Int32)o;
            }

        }

        protected List<String> GetMultiValuedString(RmAttributeName attributeName)
        {
            List<IComparable> o = null;
            RmAttributeValue rma = null;
            this.attributes.TryGetValue(attributeName, out rma);
            if (rma == null)
            {
                rma = new RmAttributeValue();
                this.attributes[attributeName] = rma;
            }
            o = rma.Values;
            if (o == null)
            {
                return null;
            }
            else
            {
                var retVal = new List<String>();
                foreach (Object item in o)
                {
                    retVal.Add((String)item);
                }
                return retVal;
            }
        }

        protected RmList<RmReference> GetMultiValuedReference(RmAttributeName attributeName)
        {
            IList<IComparable> o = null;
            RmAttributeValue rma = null;
            this.attributes.TryGetValue(attributeName, out rma);
            if (rma == null)
            {
                rma = new RmAttributeValue();
                this.attributes[attributeName] = rma;
            }
            o = rma.Values;
            if (o == null)
            {
                return null;
            }
            else
            {
                return new RmList<RmReference>(o);
            }
        }

        #endregion

        #region IDictionary<RmAttributeName,RmAttributeValue> Members

        public void Add(RmAttributeName key, RmAttributeValue value)
        {
            EnsureNotDisposed();
            this.attributes.Add(key, value);
        }

        public bool ContainsKey(RmAttributeName key)
        {
            EnsureNotDisposed();
            return this.attributes.ContainsKey(key);
        }

        public ICollection<RmAttributeName> Keys
        {
            get
            {
                EnsureNotDisposed();
                return this.attributes.Keys;
            }
        }

        public bool Remove(RmAttributeName key)
        {
            EnsureNotDisposed();
            return this.attributes.Remove(key);
        }

        public bool TryGetValue(RmAttributeName key, out RmAttributeValue value)
        {
            EnsureNotDisposed();
            return this.attributes.TryGetValue(key, out value);
        }

        public ICollection<RmAttributeValue> Values
        {
            get
            {
                EnsureNotDisposed();
                return this.attributes.Values;
            }
        }

        public RmAttributeValue this[String key]
        {
            get
            {
                EnsureNotDisposed();
                if (this.attributes.ContainsKey(new RmAttributeName(key)))
                {
                    return this.attributes[new RmAttributeName(key)];
                }
                else
                {
                    return new RmAttributeValue();
                }
            }
            set
            {
                EnsureNotDisposed();
                RmAttributeName myKey = new RmAttributeName(key);
                this.attributes[myKey] = value;
            }
        }

        public RmAttributeValue this[RmAttributeName key]
        {
            get
            {
                EnsureNotDisposed();
                if (this.attributes.ContainsKey(key))
                {
                    return this.attributes[key];
                }
                else
                {
                    return new RmAttributeValue();
                }
            }
            set
            {
                EnsureNotDisposed();
                this.attributes[key] = value;
            }
        }

        #endregion

        #region ICollection<KeyValuePair<RmAttributeName,RmAttributeValue>> Members

        public void Add(KeyValuePair<RmAttributeName, RmAttributeValue> item)
        {
            EnsureNotDisposed();
            this.Add(item.Key, item.Value);
        }

        public void Clear()
        {
            EnsureNotDisposed();
            this.attributes.Clear();
        }

        public bool Contains(KeyValuePair<RmAttributeName, RmAttributeValue> item)
        {
            EnsureNotDisposed();
            return this.attributes.ContainsKey(item.Key) && this.attributes[item.Key].Value.Equals(item.Value);
        }

        public void CopyTo(KeyValuePair<RmAttributeName, RmAttributeValue>[] array, int arrayIndex)
        {
            EnsureNotDisposed();
            throw new NotImplementedException();
        }

        public int Count
        {
            get
            {
                EnsureNotDisposed();
                return this.attributes.Count;
            }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(KeyValuePair<RmAttributeName, RmAttributeValue> item)
        {
            EnsureNotDisposed();
            return this.attributes.Remove(item.Key);
        }

        #endregion

        #region IEnumerable<KeyValuePair<RmAttributeName,RmAttributeValue>> Members

        public IEnumerator<KeyValuePair<RmAttributeName, RmAttributeValue>> GetEnumerator()
        {
            EnsureNotDisposed();
            return this.attributes.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            EnsureNotDisposed();
            return this.attributes.GetEnumerator();
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            EnsureNotDisposed();
            lock (this.attributes)
            {
                this.attributes.Clear();
                this.attributes = null;
                GC.SuppressFinalize(this);
            }
        }

        protected void EnsureNotDisposed()
        {
            if (this.attributes == null)
            {
                throw new ObjectDisposedException("RmObject", "The RmObject object has already been disposed");
            }
        }

        #endregion
    }
}
