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
			where T : IComparable, IComparable<T>
		{
			if (data == null)
				throw new ArgumentNullException("There is no array to work on.");

			if (data.Length < 2)
				return;

			if (data.Any(datum => { return datum == null; }))
				throw new ArgumentException("There is one or more null entries in the array.");

			for (int i = data.Length / 2; i > -1; i--)
				data.FixMinHeap(i);
		}

		/// <summary>
		/// This method will sort the elements in the array into a maximum heap,
		/// with the largest value at index 0.
		/// </summary>
		public static void HeapifyMax<T>(this T[] data)
			where T : IComparable, IComparable<T>
		{
			if (data == null)
				throw new ArgumentNullException("There is no array to work on.");

			if (data.Length < 2)
				return;

			if (data.Any(datum => { return datum == null; }))
				throw new ArgumentException("There is one or more null entries in the array.");

			for (int i = data.Length / 2; i > -1; i--)
				data.FixMaxHeap(i);
		}
		#endregion


		#region Helper Methods
		private static void FixMinHeap<T>(this T[] data, int index)
			where T : IComparable, IComparable<T>
		{
			if (data.IsMinHeap(index))
				return;

			int left = (index * 2) + 1;
			int right = (index * 2) + 2;

			int min = left;

			if (right < data.Length && data[right].CompareTo(data[left]) < 0)
				min = right;

			data.SwapValues(index, min);
		}

		private static void FixMaxHeap<T>(this T[] data, int index)
			where T : IComparable, IComparable<T>
		{
			if (data.IsMaxHeap(index))
				return;

			int left = (index * 2) + 1;
			int right = (index * 2) + 2;

			int max = left;

			if (right < data.Length && data[right].CompareTo(data[left]) > 0)
				max = right;

			data.SwapValues(index, max);
		}

		private static bool IsMinHeap<T>(this T[] data, int index)
			where T : IComparable, IComparable<T>
		{
			int left = (index  * 2) + 1;
			int right = (index * 2) + 2;

			if (left >= data.Length)
				return true;

			if (data[index].CompareTo(data[left]) > 0)
				return false;

			if (right < data.Length)
				return data[index].CompareTo(data[right]) <= 0;
			
			return true;
		}

		private static bool IsMaxHeap<T>(this T[] data, int index)
			where T : IComparable, IComparable<T>
		{
			int left = (index * 2) + 1;
			int right = (index * 2) + 2;

			if (left >= data.Length)
				return true;

			if (data[index].CompareTo(data[left]) < 0)
				return false;

			if (right < data.Length)
				return data[index].CompareTo(data[right]) >= 0;

			return true;
		}
		#endregion
	}
}
