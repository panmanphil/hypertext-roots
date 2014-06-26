using System.Collections.Generic;

namespace HypertextRoots.HAL
{
    public interface ILinkedResourceCollection
    {
        void Add(LinkedResource resource);
    }

    public class LinkedResourceCollection<T> : LinkedResource, ILinkedResourceCollection, ICollection<T> where T : LinkedResource
    {
        List<T> innerList = new List<T>();

        public void Add(T item)
        {
            this.innerList.Add(item);
        }

        public void Clear()
        {
            this.innerList.Clear();
        }

        public bool Contains(T item)
        {
            return this.innerList.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            this.innerList.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return this.innerList.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(T item)
        {
            return this.innerList.Remove(item);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return this.innerList.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.innerList.GetEnumerator();
        }
        
        public void Add(LinkedResource resource)
        {
            this.innerList.Add((T)resource);
        }
    }
}
