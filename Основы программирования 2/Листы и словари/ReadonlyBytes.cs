using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;

namespace hashes
{
    public class ReadonlyBytes : IEnumerable
    {
        readonly List<byte> list;
        private int length;
        readonly int hash = unchecked((int)2166136261);

        public ReadonlyBytes(params byte[] data)
        {
            if (data == null) throw new ArgumentNullException();
            list = new List<byte>(data);
            length = list.Count;

            int prime = 16777619;
            unchecked
            {
                foreach (byte b in list)
                    hash = (hash ^ b.GetHashCode()) * prime;
            }
        }

        public byte this[int index]
        {
            get 
            { 
                if(index < 0 || index >= list.Count) throw new IndexOutOfRangeException();
                return list[index]; 
            }
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (obj.GetType() != GetType()) return false;
            if (!(obj is ReadonlyBytes)) return false;
            var array = obj as ReadonlyBytes;
            if (array.Length != this.Length) return false;
            for (int i = 0; i < array.Length; i++)
                if (array[i] != list[i]) return false;
            return true;
        }

        public override string ToString()
        {
            if(Length == 0) return "[]";
            String[] array = new String[this.Length];
            for(int i = 0; i < array.Length; i++)
                array[i] = list[i].ToString();
            return "[" + string.Join(", ", array) + "]";
        }

        public override int GetHashCode()
        {
            return hash;
        }

        public IEnumerator<byte> GetEnumerator()
        {
            return list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int Length { get { return length; } }
    }
}