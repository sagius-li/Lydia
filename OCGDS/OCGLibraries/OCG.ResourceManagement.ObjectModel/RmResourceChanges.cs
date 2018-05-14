using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OCG.ResourceManagement.ObjectModel
{
    public class RmResourceChanges : IDisposable
    {
        private RmResource rmObject;
        private Dictionary<RmAttributeName, RmAttributeValue> originalAttributes;

        public RmResource RmObject
        {
            get
            {
                return this.rmObject;
            }
        }

        /// <summary>
        /// Begins a transaction on the provided RmResource object.
        /// </summary>
        /// <param name="rmObject">The object to watch during the transaction.</param>
        public RmResourceChanges(RmResource rmObject)
        {
            if (rmObject == null)
            {
                throw new ArgumentNullException("rmObject");
            }
            this.rmObject = rmObject;
        }

        /// <summary>
        /// Returns a list of changes made to this object since the transaction began or the last call to AcceptChanges.
        /// </summary>
        /// <returns></returns>
        public IList<RmAttributeChange> GetChanges()
        {
            EnsureNotDisposed();
            lock (rmObject.Attributes)
            {
                // there was no original, so we just return an empty list
                if (originalAttributes == null)
                {
                    return new List<RmAttributeChange>();
                }
                else
                {
                    return RmResourceChanges.GetDifference(rmObject.Attributes, this.originalAttributes);
                }
            }
        }

        public void BeginChanges()
        {
            EnsureNotDisposed();
            lock (rmObject.attributes)
            {
                this.originalAttributes = new Dictionary<RmAttributeName, RmAttributeValue>();
                foreach (RmAttributeName key in rmObject.attributes.Keys)
                {
                    this.originalAttributes[key] = new RmAttributeValue();
                    RmAttributeValue value = rmObject.attributes[key];
                    this.originalAttributes[key] = value.Clone() as RmAttributeValue;
                }
            }
        }

        /// <summary>
        /// Commits all of the changes made the RmResource object.
        /// 
        /// GetChanges() will return an empty list immediately after this call.
        /// </summary>
        public void AcceptChanges()
        {
            EnsureNotDisposed();
            lock (rmObject.attributes)
            {
                this.originalAttributes = null;
            }
        }

        /// <summary>
        /// Discards all of the changes made ot the RmResource object since the transaction began or a call to AcceptChanges()
        /// </summary>
        public void DiscardChanges()
        {
            EnsureNotDisposed();
            lock (rmObject.attributes)
            {
                rmObject.attributes = this.originalAttributes;
            }
        }

        #region IDisposable Members

        public void Dispose()
        {
            EnsureNotDisposed();
            lock (rmObject.attributes)
            {
                this.originalAttributes = null;
                GC.SuppressFinalize(this);
                this.rmObject = null;
            }
        }

        private void EnsureNotDisposed()
        {
            if (this.rmObject == null)
            {
                throw new ObjectDisposedException("RmObjectTransaction", "The RmObjectTransaction object has already been disposed");
            }
        }

        #endregion

        #region difference calculation
        /// <summary>
        /// Returns a list of changes to make to obj1 in order for it to look like obj2.
        /// </summary>
        /// <param name="obj1">The starting object state.</param>
        /// <param name="obj2">The ending object state.</param>
        /// <returns>A list of RmAttributeChanges to apply to obj1 for it to look like obj2</returns>
        public static IList<RmAttributeChange> GetDifference(RmResource obj1, RmResource obj2)
        {
            if (obj1 == null)
            {
                throw new ArgumentNullException("obj1");
            }
            if (obj2 == null)
            {
                throw new ArgumentNullException("obj2");
            }
            return GetDifference(obj1.attributes, obj2.attributes);
        }

        private static IList<RmAttributeChange> GetDifference(Dictionary<RmAttributeName, RmAttributeValue> attributes1, Dictionary<RmAttributeName, RmAttributeValue> attributes2)
        {
            IList<RmAttributeChange> changedAttributes = new List<RmAttributeChange>();
            foreach (KeyValuePair<RmAttributeName, RmAttributeValue> item1 in attributes1)
            {
                if (attributes2.ContainsKey(item1.Key) == false)
                {
                    foreach (IComparable value1 in item1.Value.Values)
                    {
                        changedAttributes.Add(new RmAttributeChange(item1.Key, value1, RmAttributeChangeOperation.Add));
                    }
                }
                else
                {
                    RmAttributeValue rmValue2 = attributes2[item1.Key];
                    foreach (IComparable value1 in item1.Value.Values)
                    {
                        if (rmValue2.Values.Contains(value1) == false)
                        {
                            changedAttributes.Add(new RmAttributeChange(item1.Key, value1, RmAttributeChangeOperation.Add));
                        }
                    }
                    foreach (IComparable value2 in rmValue2.Values)
                    {
                        if (item1.Value.Values.Contains(value2) == false)
                        {
                            changedAttributes.Add(new RmAttributeChange(item1.Key, value2, RmAttributeChangeOperation.Delete));
                        }
                    }
                }
            }
            foreach (KeyValuePair<RmAttributeName, RmAttributeValue> item2 in attributes2)
            {
                if (attributes1.ContainsKey(item2.Key) == false)
                {
                    foreach (IComparable value2 in item2.Value.Values)
                    {
                        changedAttributes.Add(new RmAttributeChange(item2.Key, value2, RmAttributeChangeOperation.Delete));
                    }
                }
            }
            return changedAttributes;
        }
        #endregion
    }
}
