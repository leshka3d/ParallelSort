using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Threading;
using ParallelSort;
using Microsoft.VisualBasic;

namespace ParallelArraySort
{
	
	public class ParallelSort<T> where T: IComparable<T>
	{
		ParallelSort ()
        {

        }
		static public async Task Sort8Async(T[] Arr, IMerger<T> merger = null)
		{

			if (merger == null)
			{
				merger = new Merger<T>();
			}

			var half = Arr.Length >> 1;

			var tail = Arr.Length - half;

			var o1 = new ParallelSort<T>();
			var o2 = new ParallelSort<T>();
			await o1._Sort4Async(Arr,0, half, merger);
			await o2._Sort4Async(Arr, half, tail, merger);
						
			await Task.Run(() => { merger.Merge(Arr, 0, half, tail); }); 			

		}
		static public  void Sort8(T[] Arr, IMerger<T> merger = null)
		{

			if (merger == null)
			{
				merger = new Merger<T>();
			}

			var half = Arr.Length >> 1;
		
			var tail = Arr.Length -  half  ; 
		
		 	var t1 = Task.Run (()=> { var o = new ParallelSort<T>();  o._Sort4(Arr,0, half, merger); });
			var t2 = Task.Run(() => { var o = new ParallelSort<T>();  o._Sort4(Arr, half, tail, merger); });
			Task.WaitAll(t1, t2	);
			
			merger.Merge(Arr, 0, half, tail);

		}

		 int _quarta;
		 int _medium;
		 int _quarta4;
		 int _tail4;
		 int _tail2;
		 int _length;
		 int _startIndex; 
		 IMerger<T> _merger;
		static public async Task Sort4Async(T[] Arr, IMerger<T> merger = null)
		{
			var o = new ParallelSort<T>();
			await o._Sort4Async(Arr,0, Arr.Length, merger);
		}
		private async Task _Sort4Async(T[] Arr, int startIndex, int length, IMerger<T> merger = null)
		{
			var backgroundTasks = Sort4Sort(Arr, startIndex, length, merger);
			await Sort4AsyncMerge(backgroundTasks, Arr);
			return;
		}
		static public void Sort4(T[] Arr, IMerger<T> merger = null)
		{
			var o = new ParallelSort<T>();
			o._Sort4(Arr,0, Arr.Length,  merger);			
        }
		private void _Sort4(T[] Arr, int startIndex, int length, IMerger<T> merger = null)
        {
			var backgroundTasks = Sort4Sort(Arr, startIndex,length, merger);
			Sort4SyncMerge(backgroundTasks, Arr);
			return;
		}
		 public async Task Sort4Asyc(T[] Arr, IMerger<T> merger = null)
		{
			var backgroundTasks = Sort4Sort(Arr, 0, Arr.Length, merger);
			await Sort4AsyncMerge(backgroundTasks, Arr);
			return;
		}
		 private Task[] Sort4Sort (T[] Arr, int startIndex = 0 , int length = 0 , IMerger<T> merger = null)
        {
			_startIndex = startIndex;
			_length = length == 0?Arr.Length:length; 

			_merger = merger ?? new Merger<T>();

			_quarta = _length >> 2;
			_medium = _quarta << 1;
			_quarta4 = (_quarta << 2);
			_tail4 = _length - _quarta4 + _quarta;
			_tail2 = _length - (_medium << 1) + _medium;


			var backgroundTasks = new[]
			{

			Task.Run(() => { Array.Sort<T>(Arr,_startIndex +  0, _quarta); }),

			Task.Run(() => { Array.Sort<T>(Arr,_startIndex +  _quarta, _quarta); }),

			Task.Run(() => { Array.Sort<T>(Arr,_startIndex +  _medium, _quarta); }),

			Task.Run(() => { Array.Sort<T>(Arr,_startIndex +  _medium + _quarta,  _tail4); })
			};
			return backgroundTasks;
		}
		
		private void Sort4SyncMerge(Task[] SortTasks, T[] Arr)
		{

			Task.WaitAll(SortTasks);

			var backgroundTasks2 = new[]
			{
				Task.Run(() => { _merger.Merge(Arr,_startIndex + 0, _quarta, _quarta); }),

				Task.Run(() => { _merger.Merge(Arr, _startIndex + _medium, _quarta, _tail4); })
			};

			Task.WaitAll(backgroundTasks2);
			
			_merger.Merge(Arr, _startIndex +  0, _medium, _tail2);
		}
		 private async Task Sort4AsyncMerge (Task[] SortTasks, T[] Arr)
        {
			await Task.WhenAll(SortTasks);
			

			var backgroundTasks2 = new[]
			{
				Task.Run(() => { _merger.Merge(Arr,_startIndex + 0, _quarta, _quarta); }),

				Task.Run(() => { _merger.Merge(Arr,_startIndex +  _medium, _quarta, _tail4); })
			};

			await Task.WhenAll(backgroundTasks2);
			

			await Task.Run(() => { _merger.Merge(Arr, _startIndex + 0, _medium, _tail2); });
			
		}

	}
}

