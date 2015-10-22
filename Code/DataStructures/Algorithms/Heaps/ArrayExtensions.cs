using System;
using System.Linq;


namespace Unvi.Algorithms.Heaps
{
	public static class ArrayExtensions
	{
		#region Public Extension Methods
		/// <summary>
		/// This method will sort the elements in the array into a minimum heap, 
		/// with the smallest value at index 0.
		/// </summary>
		public static void HeapifyMin<T>(this T[] data)
			where T : IComparable<T>
		{
			if (data == null)
				throw new ArgumentNullException("There is no array to work on.");

			if (data.Any(datum => { return datum == null; }))
				throw new ArgumentException("There is one or more null entries in the array.");

			data.HeapifyMin(data.Length);
		}

		/// <summary>
		/// This method will sort the elements in the array into a maximum heap,
		/// with the largest value at index 0.
		/// </summary>
		public static void HeapifyMax<T>(this T[] data)
			where T : IComparable<T>
		{
			if (data == null)
				throw new ArgumentNullException("There is no array to work on.");

			if (data.Any(datum => { return datum == null; }))
				throw new ArgumentException("There is one or more null entries in the array.");

			data.HeapifyMax(data.Length);
		}
		#endregion


		#region Internal Methods
		internal static void HeapifyMin<T>(this T[] data, int length)
			where T : IComparable<T> 
		{
			if (length < 2)
				return;

			for (int i = length / 2; i > -1; i--)
				data.FixMinHeap(i, length);
		}

		internal static void HeapifyMax<T>(this T[] data, int length)
			where T : IComparable<T> 
		{
			if (data.Length < 2)
				return;

			for (int i = length / 2; i >= 0; i--)
				data.FixMaxHeap(i, length);
		}

		internal static T PopMin<T>(this T[] data, int length)
			where T : IComparable<T>
		{
			if ((data == null) || (length <= 0))
				throw new Exception("There's no data to work with.");

			if (data.Length < length)
				throw new Exception("Length mismatch in array.");


			T result = data[0];

			if (length == 1)
				return result;

			data[0] = data[length - 1];
			data.FixMinHeap(0, length - 1);

			return result;
		}

		internal static T PopMax<T>(this T[] data, int length)
			where T : IComparable<T>
		{
			if ((data == null) || (length <= 0))
				throw new Exception("There's no data to work with.");

			if (data.Length < length)
				throw new Exception("Length mismatch in array.");

			T result = data[0];

			if (length == 1)
				return result;

			data.SwapValues(0, length - 1);

#if CountSwaps
			Sorting.ArrayExtensions.HeapSwapCount++;
#endif

			data.FixMaxHeap(0, length - 1);

			return result;
		}
#endregion


#region Helper Methods
		private static void FixMinHeap<T>(this T[] data, int index, int length)
			where T : IComparable<T>
		{
			if (data.IsMinHeap(index, length))
				return;

			int left = (index * 2) + 1;
			int right = (index * 2) + 2;

			int min = left;

			if (right < length && data[right].CompareTo(data[left]) < 0)
				min = right;

			data.SwapValues(index, min);
			data.FixMinHeap(min, length);
		}

		private static void FixMaxHeap<T>(this T[] data, int index, int length)
			where T : IComparable<T>
		{
			if (data.IsMaxHeap(index, length))
				return;

			int left  = (index * 2) + 1;
			int right = (index * 2) + 2;

			int max = left;

#if CountSwaps
			if (right < length)
				Sorting.ArrayExtensions.HeapCompareCount++;
#endif
			if (right < length && data[right].CompareTo(data[left]) > 0)
				max = right;

			data.SwapValues(index, max);

#if CountSwaps
			Sorting.ArrayExtensions.HeapSwapCount++;
#endif
			data.FixMaxHeap(max, length);
		}

		private static bool IsMinHeap<T>(this T[] data, int index, int length)
			where T : IComparable<T>
		{
			int left = (index  * 2) + 1;
			int right = (index * 2) + 2;

			if (left >= length)
				return true;

			if (data[index].CompareTo(data[left]) > 0)
				return false;

			if (right < length)
				return data[index].CompareTo(data[right]) <= 0;
			
			return true;
		}

		private static bool IsMaxHeap<T>(this T[] data, int index, int length)
			where T : IComparable<T>
		{
			int left  = (index * 2) + 1;
			int right = (index * 2) + 2;

			if (left >= length)
				return true;

#if CountSwaps
			Sorting.ArrayExtensions.HeapCompareCount++;
#endif
			if (data[index].CompareTo(data[left]) < 0)
				return false;

#if CountSwaps
			if (right < length)
				Sorting.ArrayExtensions.HeapCompareCount++;
#endif
			if (right < length)
				return data[index].CompareTo(data[right]) >= 0;

			return true;
		}
#endregion
	}
}
