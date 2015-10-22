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

		internal static void HeapifyMin<T>(this List<T> data, int length)
			where T : IComparable<T>
		{
			if (length < 2)
				return;

			for (int i = length / 2; i > -1; i--)
				data.FixMinHeap(i, length);
		}

		private static void FixMinHeap<T>(this List<T> data, int index, int length)
			where T : IComparable<T>
		{
			if (data.IsMinHeap(index, length))
				return;

			int left = (index * 2) + 1;
			int right = left + 1;

			int min = left;

			if (right < length && data[right].CompareTo(data[left]) < 0)
				min = right;

			data.SwapValues(index, min);
			data.FixMinHeap(min, length);
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
	}
}
