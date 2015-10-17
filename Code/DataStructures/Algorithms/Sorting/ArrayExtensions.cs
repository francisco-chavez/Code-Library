using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Unvi.DataStructures;
using Unvi.DataStructures.Heaps;


namespace Unvi.Algorithms.Sorting {
	public static class ArrayExtensions {
		#region Normal Heap Sort Code
		public static void HeapSort<T>(this T[] array)
			where T : IComparable<T> {
			var heap = new ListHeap<T>(HeapType.Min, array);
			
			int i = 0;
			while (heap.Count > 0)
				array[i++] = heap.Pop();
		}
		#endregion
	}
}
