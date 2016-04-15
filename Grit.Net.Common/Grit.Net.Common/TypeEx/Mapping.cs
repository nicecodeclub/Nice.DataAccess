using System;
using System.Collections;
using System.Collections.Generic;

namespace Grit.Net.Common.TypeEx
{
    /// <summary>
    /// 自定义轻量级Mapping
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public class Mapping<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>>
    {
        private int length;
        private int currentIndex;
        private const int Incremental = 64;

        public int Length
        {
            get
            {
                return length;
            }
        }

        private KeyValuePair<TKey, TValue>[] items;
        public KeyValuePair<TKey, TValue>[] Items
        {
            get
            {
                return items;
            }

            set
            {
                items = value;
            }
        }
        public Mapping()
        {
            items = new KeyValuePair<TKey, TValue>[Incremental];
            this.length = Incremental;
        }
        public Mapping(int length)
        {
            items = new KeyValuePair<TKey, TValue>[length];
            this.length = length;
        }

        public TValue this[TKey key]
        {
            get
            {
                foreach (var item in items)
                {
                    if (item.Key == null) break;
                    if (item.Key.Equals(key))
                    {
                        return item.Value;
                    }
                }
                return default(TValue);
            }
        }
        public void TrimEmpty()
        {
            int numnull = 0;
            for (int i = items.Length - 1; i >= 0; i--)
            {
                if (items[i].Key == null)
                    numnull++;
                else break;
            }
            if (numnull > 0)
            {
                this.length -= numnull;
                Array.Resize(ref items, this.length);
            }
        }


        public void Add(TKey key, TValue value)
        {
            if (currentIndex < length)
            {
                items[currentIndex] = new KeyValuePair<TKey, TValue>(key, value);
                currentIndex++;
            }
            else
            {
                this.length += Incremental;
                KeyValuePair<TKey, TValue>[] newitems = new KeyValuePair<TKey, TValue>[this.length];
                Array.Copy(items, newitems, items.Length);
                items = null;
                items = newitems;
                items[currentIndex] = new KeyValuePair<TKey, TValue>(key, value);
                currentIndex++;
            }

        }


        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            for (int i = 0; i < items.Length; i++)
            {
                yield return items[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
