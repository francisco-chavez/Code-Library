using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Unvi.Algorithms.Heaps;


namespace Unvi.Algorithms.Sorting 
{
	public static class ArrayExtensions 
	{
#if CountSwaps
		public static int QuickSwapCount;
		public static int HeapSwapCount;

		public static int QuickCompareCount;
		public static int HeapCompareCount;
#endif

		public static void HeapSort<T>(this T[] array)
			where T : IComparable<T> 
		{
#if CountSwaps
			HeapSwapCount = 0;
			HeapCompareCount = 0;
#endif
			array.HeapifyMax();

			//// This would be the proper way to do this, but since I'm the one
			//// that wrote the pop method, I happen to know that poping will
			//// place the pop value in the location where the last value used
			//// to be. So, we can just call the pop method until the heap length
			//// reaches 1.
			//// -FCT
			//int heapLength = array.Length;
			//while (heapLength > 1) {
			//	T maxValue = array.PopMax(heapLength);
			//	array[--heapLength] = maxValue;
			//}
			for (int heapLength = array.Length; heapLength > 1;)
				array.PopMax(heapLength--);
		}

		public static void QuickSort<T>(this T[] array)
			where T : IComparable<T>
		{
			if (array == null)
				throw new ArgumentNullException("There is no array to sort.");

			if (array.Any(d => { return d == null; }))
				throw new ArgumentNullException("There is null data in the array.");

			if (array.Length < 2)
				return;

#if CountSwaps
			QuickSwapCount = 0;
			QuickCompareCount = 0;
#endif
			array.QuickSort(0, array.Length);
		}

		public static void RandomQuickStort<T>(this T[] array)
			where T : IComparable<T> 
		{
			if (array == null)
				throw new ArgumentNullException("There is no array to sort.");

			if (array.Any(d => { return d == null; }))
				throw new ArgumentNullException("There is null data in the array.");

			if (array.Length < 2)
				return;

			Random randomGen = new Random((int) DateTime.Now.Ticks);

			array.RandomQuickSort(0, array.Length, randomGen);
		}

		private static void QuickSort<T>(this T[] array, int start, int end)
			where T : IComparable<T>
		{
			// There are less than 2 items, therefore, it's already sorted.
			if (end - start < 2)
				return;

			int pivotPoint = end - 1;
			T pivotValue = array[pivotPoint];

			for (int i = 0; i < pivotPoint; i++)
			{
#if CountSwaps
				QuickCompareCount++;
#endif
				if (pivotValue.CompareTo(array[i]) < 0)
				{
					pivotPoint--;
					array.SwapValues(i, pivotPoint);
#if CountSwaps
					QuickSwapCount++;
#endif
					i--;
				}
			}

			if (pivotPoint != end - 1)
			{
				array.SwapValues(pivotPoint, end - 1);
#if CountSwaps
				QuickSwapCount++;
#endif
			}

			if (end - start < 3)
				return;

			array.QuickSort(start, pivotPoint);
			array.QuickSort(pivotPoint + 1, end);
		}

		private static void RandomQuickSort<T>(this T[] array, int start, int end, Random randomGen)
			where T : IComparable<T>
		{
			// There are less than 2 items, therefore, it's already sorted.
			if (end - start < 2)
				return;

			int pivotPoint = randomGen.Next(start, end);
			array.SwapValues(pivotPoint, end - 1);
			pivotPoint = end - 1;
			T pivotValue = array[pivotPoint];

			for (int i = 0; i < pivotPoint; i++)
				if (pivotValue.CompareTo(array[i]) < 0)
				{
					pivotPoint--;
					array.SwapValues(i, pivotPoint);
					i--;
				}

			if (pivotPoint != end - 1)
				array.SwapValues(pivotPoint, end - 1);

			if (end - start < 3)
				return;

			array.QuickSort(start, pivotPoint);
			array.QuickSort(pivotPoint + 1, end);
		}
	}
}
