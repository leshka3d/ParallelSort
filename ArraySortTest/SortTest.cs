using NUnit.Framework;
using ParallelArraySort;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ArraySortTest
{
    public class SortTest
    {
        [OneTimeSetUp]
        public void Setup()
        {

            var size =  (int.MaxValue >> 2); // + (int.MaxValue >> 4) ;// = 4 * 1000000;
                size = 100027;  //
                              // size = 27;
            
            Arr = new int[size];
            var rnd = new Random(1);
            for (var a = 0; a < Arr.Length; a++)
            {
                Arr[a] = rnd.Next(int.MaxValue);
                //Arr[a] = rnd.Next(85) + 10;
            }
          
        }

        class cmp : IComparer<int>
        {
            public int Compare(int a, int b)
            {
                return a - b;
            }
        }

        static int[] Arr;

        [Test]
        public void Standart()
        {
            Array.Sort(Arr);
        }
        [Test]
        public void FastParallelT()
        {
            //ParallelSort<int>.Sort(Arr);
        }
        [Test]
        public void FastParallel()
        {
            ParallelArraySort.ParallelSort<int>.Sort4(Arr);
        }
        [Test]
        public async Task _FastParallelValidateAsync()
        {
            for (var x = 0; x <= 100; x++)
            {
                TestContext.Progress.WriteLine(x);

                await ParallelArraySort.ParallelSort<int>.Sort8Async(Arr);

                var l = new List<int>();
                var a = 0;
                var b = 0;
                for (var i = 0; i < Arr.Length - 1; i++)
                {
                    if (Arr[i] > Arr[i + 1])
                    {
                        a = Arr[i];
                        b = Arr[i + 1];
                        throw new InvalidOperationException();
                    }

                    if ((Arr[i] == a) || (Arr[i] == b))
                    {
                        l.Add(Arr[i]);
                    }
                }

                var rnd = new Random(x);

                for (var aa = 0; aa < Arr.Length; aa++)
                {
                    Arr[aa] = rnd.Next(int.MaxValue);
                }

                GC.Collect(2, GCCollectionMode.Forced);
            }

        }
        [Test]
        public void _FastParallelValidateTmpl()
        {
            for (var x = 0; x <= 100; x++)
            {
                TestContext.Progress.WriteLine(x);

                ParallelArraySort.ParallelSort<int>.Sort4(Arr);

                var l = new List<int>();
                var a = 0;
                var b = 0;
                for (var i = 0; i < Arr.Length - 1; i++)
                {
                    if (Arr[i] > Arr[i + 1])
                    {
                        a = Arr[i];
                        b = Arr[i + 1];
                        throw new InvalidOperationException();
                    }

                    if ((Arr[i] == a) || (Arr[i] == b))
                    {
                        l.Add(Arr[i]);
                    }
                }

                var rnd = new Random(x);

                for (var aa = 0; aa < Arr.Length; aa++)
                {
                    Arr[aa] = rnd.Next(int.MaxValue);
                }

                GC.Collect(2, GCCollectionMode.Forced);
            }

        }

       

        [Test]
        public void Standart_Reverse()
        {
            Array.Sort<int>(Arr);
            Array.Reverse<int>(Arr);

            //Array.Sort<int>(Arr,new cmp());
        }

     



        [Test]
		public void Test2()
		{
			var backgroundTasks = new[]
			{
				Task.Run(() =>{ Assert.IsTrue(true); }),
				Task.Run(() =>{ Assert.IsTrue(true); }),
				Task.Run(() =>{ Assert.IsTrue(true); }),
				Task.Run(() =>{ Assert.IsTrue(true); }),

			};
		}
	}
}