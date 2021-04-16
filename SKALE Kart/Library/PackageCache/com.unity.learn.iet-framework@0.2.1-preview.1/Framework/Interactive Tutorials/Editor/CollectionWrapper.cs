using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unity.InteractiveTutorials
{
    public abstract class CollectionWrapper
    {
    }

    public abstract class CollectionWrapper<T> : CollectionWrapper, IEnumerable<T>
    {
        [SerializeField]
        List<T> m_Items = new List<T>();

        public CollectionWrapper() {}
        public CollectionWrapper(IList<T> items) { SetItems(items); }

        public T this[int i]
        {
            get { return m_Items[i]; }
            set
            {
                if (
                    (m_Items[i] == null && value != null) ||
                    (m_Items[i] != null && !m_Items[i].Equals(value))
                    )
                {
                    m_Items[i] = value;
                }
            }
        }

        public int count { get { return m_Items.Count; } }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return m_Items.GetEnumerator();
        }

        public IEnumerator<T> GetEnumerator()
        {
            return m_Items.GetEnumerator();
        }

        public void GetItems(List<T> items)
        {
            if (items.Capacity < m_Items.Count)
            {
                items.Capacity = m_Items.Count;
            }
            items.Clear();
            items.AddRange(m_Items);
        }

        public void SetItems(IEnumerable<T> items)
        {
            m_Items.Clear();
            m_Items.AddRange(items);
        }
    }
}
