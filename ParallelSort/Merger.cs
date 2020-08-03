using System;
using System.Buffers;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using ParallelArraySort;
using ParallelSort;

namespace ParallelArraySort

{
   public class Merger<T>:IMerger<T> where T: IComparable<T>
    {

        public Merger()
        {
            _queue = null;
        }
        public Merger(IQueue<T> queue )
        {
            _queue = queue;
        }
        IQueue<T> _queue ;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public  void Merge(T[] Arr, int start1, int length1, int length2)
        {
            
            int aPos;
            int bPos;
            int arrEnd;

            int medium = start1 + length1; ;


            IQueue<T> queue = _queue ?? new ArrayQueue<T>(length1 >> 1); ;

                    
            if (Arr[medium].CompareTo (Arr[medium - 1]) > 0)
            {
                return;
            }
                     

            aPos = start1;
            bPos = medium;
            arrEnd = medium + length2;

            var ahalf = length1 >> 2 + aPos; 

            while (aPos < medium && bPos < arrEnd)
            {
                if (queue.Length > 0)
                {
                    var savedItem = queue.Look();
                    var aPartItem = Arr[aPos];
                    var bPartItem = Arr[bPos];
                    
                    if ((savedItem.CompareTo (aPartItem) < 0) && (savedItem.CompareTo (bPartItem) < 0))
                    {
                        // use saved Item 
                        queue.Next();
                        queue.Put(aPartItem);
                        Arr[aPos] = savedItem;
                        aPos++;
                        continue;
                    }
                    
                    if ((aPartItem.CompareTo (savedItem) <=0) && (aPartItem.CompareTo (bPartItem) <=0))
                    {
                        // use item from 1st part - no chages
                        aPos++;
                        continue;
                    }

                    
                    if ((bPartItem.CompareTo (savedItem) <=0) && (bPartItem.CompareTo (aPartItem)) < 0)
                    {
                        // use Item from second part 
                        queue.Put(aPartItem);
                        Arr[aPos] = bPartItem;                                                
                        aPos++;
                        bPos++;
                        continue;
                    }
                }
                else
                {
                    // no items in queue - just compare items from meged parts 
                    var aItem = Arr[aPos];
                    var bItem = Arr[bPos];
                    if (aItem.CompareTo (bItem)<=0 ) //<=
                    {
                        aPos++;
                        continue;
                    }
                    else
                    {
                        queue.Put(aItem);
                        Arr[aPos] = bItem;
                        aPos++;
                        bPos++;
                    }
                }

            }


            // tail of 2nd part 
            int c = medium;
            while (c < arrEnd && bPos < arrEnd && queue.Length > 0)
            {
                var savedItem = queue.Look();
                //if (savedItem < Arr[bPos])
                if (savedItem.CompareTo (Arr[bPos]) < 0)
                {
                    Arr[c++] = savedItem;
                    queue.Next();
                }
                else
                {
                    Arr[c++] = Arr[bPos];
                    //	Arr[b] = -1;
                    bPos++;
                }
            }

            // just put saved Items to the end
            if (queue.Length > 0)
            {
                var cnt = queue.Length;
                var start = arrEnd - cnt;

                for (var i = start; i < arrEnd; i++)
                {
                    Arr[i] = queue.Get();
                }
            }

        }

    }
}
