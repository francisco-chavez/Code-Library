
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Unvi.Algorithms.Heaps;


namespace ConsoleProgram
{
	class Program
	{
		static void Main(string[] args)
		{
			List<int> sourceData = new List<int>(new int[] { -5, -4, -3, -2, -1, 0, 1, 2, 3, 4, 5 });
			sourceData.Reverse();

			sourceData.HeapifyMin();
			var isHeap = IsMinHeap(sourceData);
		}

		private static bool IsMinHeap(List<int> heap)
		{
			for (int i = heap.Count / 2; i >= 0; i--)
			{
				int left = i * 2 + 1;
				int right = left + 1;

				if (left >= heap.Count)
					continue;

				if (heap[i] > heap[left])
					return false;

				if (right >= heap.Count)
					continue;

				if (heap[i] > heap[right])
					return false;
			}

			return true;
		}
	}
}
