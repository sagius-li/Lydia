using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OCG.ResourceManagement.ObjectModel
{
    public class RmList<T> : IList<T>
    {
        IList<IComparable> values;
        public RmList()
        {
            this.values = new List<IComparable>();
        }
        public RmList(IList<IComparable> values)
        {
            this.values = values;
        }

        IComparable ConvertTo(T item)
        {
            IComparable converted = item as IComparable;
            if (converted == null)
                throw new InvalidCastException("Attempting to convert an item in a list that does not implement IComparable.");
            return converted;
        }

        T ConvertFrom(IComparable item)
        {
            T converted = (T)item;
            if (converted == null)
                throw new InvalidCastException("Attempting to convert an item in a list that does not implement the generic contract.");
            return converted;
        }

        #region IList<T> Members

        public int IndexOf(T item)
        {
            return this.values.IndexOf(this.ConvertTo(item));
        }

        public void Insert(int index, T item)
        {
            this.values.Insert(index, this.ConvertTo(item));
        }

        public void RemoveAt(int index)
        {
            this.values.RemoveAt(index);
        }

        public T this[int index]
        {
            get
            {
                return this.ConvertFrom(this.values[index]);
            }
            set
            {
                this.values[index] = this.ConvertTo(value);
            }
        }

        #endregion

        #region ICollection<T> Members

        public void Add(T item)
        {
            this.values.Add(this.ConvertTo(item));
        }

        public void Clear()
        {
            this.values.Clear();
        }

        public bool Contains(T item)
        {
            return this.values.Contains(this.ConvertTo(item));
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            int j = arrayIndex;
            for (int i = 0; i < array.Length; i++)
            {
                this.values[j] = this.ConvertTo(array[i]);
                j++;
            }
        }

        public int Count
        {
            get { return this.values.Count; }
        }

        public bool IsReadOnly
        {
            get { return this.values.IsReadOnly; }
        }

        public bool Remove(T item)
        {
            return this.values.Remove(this.ConvertTo(item));
        }

        #endregion

        #region IEnumerable<T> Members

        public IEnumerator<T> GetEnumerator()
        {
            return new RmListEnumerator<T>(this, this.values.GetEnumerator());
        }

        class RmListEnumerator<K> : IEnumerator<K>
        {
            IEnumerator<IComparable> enumerator;
            RmList<K> list;
            public RmListEnumerator(RmList<K> list, IEnumerator<IComparable> enumerator)
            {
                this.list = list;
                this.enumerator = enumerator;
            }
            #region IEnumerator<K> Members

            public K Current
            {
                get { return this.list.ConvertFrom(this.enumerator.Current); }
            }

            #endregion

            #region IDisposable Members

            public void Dispose()
            {
                this.enumerator.Dispose();
            }

            #endregion

            #region IEnumerator Members

            object System.Collections.IEnumerator.Current
            {
                get { return this.enumerator.Current; }
            }

            public bool MoveNext()
            {
                return this.enumerator.MoveNext();
            }

            public void Reset()
            {
                this.enumerator.Reset();
            }

            #endregion
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion
    }
}
