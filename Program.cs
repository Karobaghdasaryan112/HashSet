using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace HashSet
{
    internal class Program
    {
        static void Main(string[] args)
        {

            List<int> list = new List<int>
             {
                 1,
                 2,
                 3,
                 4,
             };
            MyHashSet<int> ints = new MyHashSet<int>(list);
            foreach (var item in ints)
            {
                Console.WriteLine(item);
            }
            MyHashSet<int> ints1 = new MyHashSet<int>(16);
            ints1.Add(1);
            ints1.Add(2);
            ints1.Add(3);
            ints1.Add(4);
            foreach (var item in ints1)
            {
                Console.WriteLine(item);
            }
            Console.ReadLine();
        }
    }
    public class MyEqualityComparer<T> : IEqualityComparer<T>
    {
        public bool Equals(T x, T y)
        {
            return x.Equals(y);
        }
        public int GetHashCode(T value)
        {
            return value.GetHashCode();
        }
    }
    public class MyHashSet<T> : MyEqualityComparer<T>, IEnumerable<T>
    {
        private int BacketSize = 16;
        private readonly List<T>[] Backet;
        private readonly MyEqualityComparer<T> Comparer = new MyEqualityComparer<T>();
        private readonly ICollection<T> Items;
        private int ItemIndex = 0;
        private int BacketIndex = 0;
        public MyHashSet(int BacketSize)
        {
            this.BacketSize = BacketSize;
            this.Backet = new List<T>[BacketSize];
            for (int i = 0; i < BacketSize; i++)
            {
                Backet[i] = new List<T>();
            }
        }

        public MyHashSet(ICollection<T> Collection) : this((int)Math.Pow(2, Math.Log(Collection.Count, 2) + 1))
        {

            foreach (var item in Collection)
            {
                int index = GetBacketIndex(item);
                Backet[index].Add(item);
            }
        }
        public int GetBacketIndex(T item)
        {
            return item.GetHashCode() % BacketSize;
        }
        public void Add(T item)
        {
            if (item != null)
            {
                int BacketIndex = GetBacketIndex(item);
                Backet[BacketIndex] = new List<T>();
                foreach (var item1 in Backet[BacketIndex])
                {
                    if (Comparer.Equals(item1, item))
                    {
                        return;
                    }
                }
                Backet[BacketIndex].Add(item);
            }
            throw new ArgumentException();
        }
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        public IEnumerator<T> GetEnumerator()
        {
            return new MyEnumerator<T>(Backet);
        }
    }
    public class MyEnumerator<T> : IEnumerator<T>
    {
        private int BacketIndex = 0;
        private int ItemIndex = 0;
        private List<T>[] _backet;
        private T current;
        public MyEnumerator(List<T>[] values)
        {
            _backet = values;
        }
        public void Dispose() { }
        public void Reset()
        {
            BacketIndex = 0;
            ItemIndex = 0;
            current = default(T);
        }
        public bool MoveNext()
        {
            while (BacketIndex < _backet.Length)
            {
                if (ItemIndex < _backet[BacketIndex].Count)
                {
                    current = _backet[BacketIndex][ItemIndex];
                    ItemIndex++;
                    return true;
                }
                BacketIndex++;
                ItemIndex = 0;
            }
            return false;
        }
        public T Current => current;
        object IEnumerator.Current => Current;
    }
}
