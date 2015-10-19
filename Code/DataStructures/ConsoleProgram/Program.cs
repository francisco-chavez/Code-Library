
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Unvi.Algorithms.Sorting;


namespace ConsoleProgram
{
	class Program
	{
		static void Main(string[] args)
		{
			int testItemCount = 200;
			List<int> sourceData1 = new List<int>(testItemCount);
			Random r = new Random(5);
			for (int i = 0; i < testItemCount; i++)
				sourceData1.Add(r.Next(-5000, 5000));

			List<int> sourceData2 = new List<int>(new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 });
			List<int> sourceData3 = new List<int>(new int[] { 10, 9, 8, 7, 6, 5, 4, 3, 2, 1, 0 });


			var dump = sourceData3.ToArray();
			dump.QuickSort();

			//bool isSorted = true;
			//for (int i = 0; i < dump.Length - 1; i++)
			//	if (dump[i] > dump[i + 1])
			//	{
			//		isSorted = false;
			//		break;
			//	}

#if CountSwaps
			int swapsQuick = ArrayExtensions.QuickSwapCount;
			int swapsHeap = ArrayExtensions.HeapSwapCount;
			Console.WriteLine("Array Length: {0}", dump.Length);
			Console.WriteLine("Quick Swaps: {0}", swapsQuick);
			Console.WriteLine("Heap Swaps: {0}", swapsHeap);
#endif
		}
	}
}
