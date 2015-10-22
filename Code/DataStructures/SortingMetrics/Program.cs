using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Unvi.Algorithms;
using Unvi.Algorithms.Sorting;


namespace SortingMetrics
{
	class Program
	{
		static void Main(string[] args)
		{
#if CountSwaps
			Random r = new Random(0);
			for (int i = 64; i < 700; i *= 2)
			{
				for (int j = 0; j < i / 8; j++)
				{
					var sourceData = new List<int>(i);
					for (int k = 0; k < i; k++)
						sourceData.Add(r.Next(-1000, 1000));

					RunTest(sourceData);

					System.Threading.Thread.Sleep(1500);
				}
			}
#endif
		}

#if CountSwaps
		static void RunTest(List<int> sourceData)
		{
			var quick = sourceData.ToArray();
			var heap = sourceData.ToArray();

			quick.QuickSort();
			heap.HeapSort();

			int compCountQ		= Unvi.Algorithms.Sorting.ArrayExtensions.QuickCompareCount;
			int compCountH		= Unvi.Algorithms.Sorting.ArrayExtensions.HeapCompareCount;
			int switchCountQ	= Unvi.Algorithms.Sorting.ArrayExtensions.QuickSwapCount;
			int switchCountH	= Unvi.Algorithms.Sorting.ArrayExtensions.HeapSwapCount;

			Console.WriteLine("Test Results");
			Console.WriteLine("Item Count: {0}", sourceData.Count);
			Console.WriteLine("Quick Sort:");
			Console.WriteLine("    Comparison Count: {0}", compCountQ);
			Console.WriteLine("    Switch Count:     {0}", switchCountQ);
			Console.WriteLine("Heap Sort:");
			Console.WriteLine("    Comparison Count: {0}", compCountH);
			Console.WriteLine("    Switch Count:     {0}", switchCountH);
			Console.WriteLine();
		}
#endif
	}
}
