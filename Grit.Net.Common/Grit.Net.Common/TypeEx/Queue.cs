using System;
namespace Grit.Net.Common.TypeEx
{
    /// <summary>
    /// 自定义轻量级队列类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Queue<T>
    {
        private int index;
        private int length = 4;

        T[] array = null;
        public Queue()
        {
            array = new T[length];
        }

        public int Count()
        {
            return index;
        }

        public void Add(T element)
        {
            if (index < length)
            {
                array[index] = element;
                index++;
            }
            else
            {
                length *= 2;
                T[] _array = new T[length];
                for (int i = 0; i < index; i++)
                    _array[i] = array[i];
                array = new T[length];
                array = _array;
                index++;
                array[index] = element;
                _array = null;
            }
        }

        public T Dequeue()
        {
            T[] _array = new T[index];
            index--;
            T element = array[index];
            for (int i = 0; i < index; i++)
                _array[i] = array[i];
            array = _array;
            _array = null;
            return element;
        }
    }
}
