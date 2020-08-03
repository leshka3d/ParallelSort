using System;
using System.Diagnostics;
using System.Drawing;
using ParallelArraySort;
namespace Run
{
	class Program
	{
		static int[] lastArr;
		static int[] lastArr8;
		static int[] MakeArray (int size)
        {
			var Arr = new int[size];
			lastArr = new int[size];
			lastArr8 = new int[size];
			var rnd = new Random(DateTime.Now.Millisecond);
			for (var a = 0; a < Arr.Length; a++)
			{
				Arr[a] = rnd.Next(int.MaxValue);
			}
			
			Arr.CopyTo(lastArr,0);
			Arr.CopyTo(lastArr8, 0);
			return Arr;
		}
		static void Main(string[] args)
		{
			var sizes = new[] { 30000, 30027, 100000, 100027, 1000000, 1000027, 10000000, 10000027, 100000000 };

			//var size = 100000; 
			
			foreach (var size in sizes)
			{
				Console.WriteLine("Size : " + size);
				var turns = 10;

				var totallStd = 0L;
				var totalPar = 0L;
				var totalPar8 = 0L;

				for (var i = 0; i < turns; i++)
				{
					Stopwatch wMain = new Stopwatch();
					var Arr = MakeArray(size);
					wMain.Start();
					ParallelArraySort.ParallelSort<int>.Sort4(Arr);					
					wMain.Stop();

					Stopwatch wMain8 = new Stopwatch();					
					wMain8.Start();
					ParallelArraySort.ParallelSort<int>.Sort8(lastArr8);
					wMain8.Stop();

					Stopwatch wStd = new Stopwatch();
					wStd.Start();
					Array.Sort(lastArr);
					wStd.Stop();
					Console.Write("[" + wMain.ElapsedMilliseconds + " / " + wMain8.ElapsedMilliseconds + " / " + wStd.ElapsedMilliseconds + "] ");
					Arr = null;
					lastArr = null;

					if (i > 1)
					{
						totalPar += wMain.ElapsedMilliseconds;
						totalPar8 += wMain8.ElapsedMilliseconds;
						totallStd += wStd.ElapsedMilliseconds;

					}
					GC.Collect(2, GCCollectionMode.Forced);
				}

				Console.WriteLine("\r\nParallel[4] / Parallel[8] / Standart = " + totalPar / (turns - 2) + "ms / " + totalPar8 / (turns - 2) + "ms / " + totallStd / (turns - 2) + "ms\r\n");
			}

		}
	}
}
