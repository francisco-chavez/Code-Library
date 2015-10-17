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
		#region Normal Heap Sort Code
		public static void HeapSort<T>(this T[] array)
			where T : IComparable<T> 
		{
			array.HeapifyMax();

			for (int heapEnd = array.Length; heapEnd > 0; heapEnd--) 
			{
				T maxVal = array.PopMax(heapEnd);
				array[heapEnd - 1] = maxVal;
			}
		}
		#endregion
	}
}
