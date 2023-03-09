using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Incapsulation.Weights
{
    public class Indexer
    {
        private readonly double[] array;

        public Indexer(double[] array, int start, int length)
        {
            this.array = array;
            if (start < 0 || length < 0 || start > array.Length
                || start + length > array.Length)
                throw new ArgumentException();
            Start = start;
            Length = length;
        }

        public double this[int index]
        {
            get
            {
                if (index + Start < 0 || index + Start >= array.Length
                    || index >= Length || index + Start < Start)
                    throw new IndexOutOfRangeException();
                return array[index + Start];
            }
            set
            {
                if (index + Start < 0 || index + Start >= array.Length
                    || index >= Length || index + Start < Start)
                    throw new IndexOutOfRangeException();
                array[index + Start] = value;
            }
        }

        public int Length { get; private set; }

        public int Start { get; private set; }
    }
}