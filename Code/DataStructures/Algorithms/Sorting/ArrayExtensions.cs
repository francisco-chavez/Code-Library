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
		public static void HeapSort<T>(this T[] array)
			where T : IComparable<T> 
		{
			array.HeapifyMax();

			int heapLength = array.Length;
			while (heapLength > 1) {
				T maxValue = array.PopMax(heapLength);
				array[--heapLength] = maxValue;
			}
		}

		public static void QuickSort<T>(this T[] array)
			where T : IComparable<T>
		{
			throw new NotImplementedException();
		}

		public static void RandomQuickStort<T>(this T[] array)
			where T : IComparable<T> 
		{
			throw new NotImplementedException();
		}
	}
}
