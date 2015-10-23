using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Unvi.Algorithms.Heaps
{
	public static class ListExtensions
	{
		public static void HeapifyMin<T>(this List<T> data)
			where T : IComparable<T>
		{
			if (data == null)
				throw new ArgumentNullException("There is no list to work on.");
			if (data.Any(datum => { return datum == null; }))
				throw new ArgumentException("There are one or more null entries in the list.");

			data.HeapifyMin(data.Count);
		}

		public static void HeapifyMax<T>(this List<T> data)
			where T : IComparable<T>
		{
			if (data == null)
				throw new ArgumentNullException("data");

			if (data.Any(datum => {
				return datum == null;
			}))
				throw new ArgumentNullException("There are one or more null entries in the list.");

			data.HeapifyMax(data.Count);
		}

		internal static void HeapifyMin<T>(this List<T> data, int length)
			where T : IComparable<T>
		{
			if (length < 2)
				return;

			for (int i = length / 2; i > -1; i--)
				data.FixMinHeap(i, length);
		}

		internal static void HeapifyMax<T>(this List<T> data, int length)
			where T : IComparable<T>
		{
			if (length < 2)
				return;

			for (int i = length / 2; i >= 0; i--)
				data.FixMaxHeap(i, length);
		}

		private static void FixMinHeap<T>(this List<T> data, int index, int length)
			where T : IComparable<T>
		{
			if (data.IsMinHeap(index, length))
				return;

			int left = (index * 2) + 1;
			int right = left + 1;       // Previous code: int right = (index * 2) + 2;
										//		
										// Odd, this cuts down the amount of work needed. Yet,
										// when you throw pipelines into the picture, this may
										// end up taking longer because we now need to wait
										// for the CPU to finish calculating the value of 'left' 
										// before it can start running the instructions for 
										// calculating 'right'
										// -FCT

			int min = left;

			if (right < length && data[right].CompareTo(data[left]) < 0)
				min = right;

			data.SwapValues(index, min);
			data.FixMinHeap(min, length);
		}

		private static void FixMaxHeap<T>(this List<T> data, int index, int length)
			where T : IComparable<T>
		{
			if (data.IsMaxHeap(index, length))
				return;

			int left = index * 2 + 1;
			int right = left + 1;

			int max = left;

			if (right < length && data[right].CompareTo(data[left]) > 0)
				max = right;

			data.SwapValues(index, max);
			data.FixMaxHeap(max, length);
		}

		private static bool IsMinHeap<T>(this List<T> data, int index, int length)
			where T : IComparable<T>
		{
			int left = (index * 2) + 1;
			int right = left + 1;

			if (left >= length)
				return true;

			if (data[index].CompareTo(data[left]) > 0)
				return false;

			if (right < length)
				return data[index].CompareTo(data[right]) <= 0;

			return true;
		}

		private static bool IsMaxHeap<T>(this List<T> data, int index, int length)
			where T : IComparable<T>
		{
			int left = index * 2 + 1;
			int right = left + 1;

			if (left >= length)
				return true;

			if (data[index].CompareTo(data[left]) < 0)
				return false;

			if (right < length)
				return data[index].CompareTo(data[right]) >= 0;

			return true;
		}
	}
}
