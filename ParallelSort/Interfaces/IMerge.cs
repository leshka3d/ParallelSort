using System;
using System.Collections.Generic;
using System.Text;

namespace ParallelSort
{
   public interface IMerger<T> where T: IComparable<T>
    {
        public void Merge(T[] Arr, int start1, int length1, int length2);
    }
}
