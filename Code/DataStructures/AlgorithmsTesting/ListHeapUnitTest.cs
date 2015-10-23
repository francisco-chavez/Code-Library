using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Unvi.Algorithms.Heaps;


namespace AlgorithmsTesting
{
	[TestClass]
	public class ListHeapUnitTest
	{
		#region Min Heap Tests
		[TestMethod]
		public void DoesMinHeapifyTest0()
		{
			List<int> sourceData = new List<int>();
			sourceData.HeapifyMin();
			var isHeap = IsMinHeap(sourceData);
			Assert.IsTrue(isHeap);
		}

		[TestMethod]
		public void DoesMinHeapifyTest1()
		{
			List<int> sourceData = new List<int>(new int[] { 0 });
			sourceData.HeapifyMin();
			var isHeap = IsMinHeap(sourceData);
			Assert.IsTrue(isHeap);
		}

		[TestMethod]
		public void DoesMinHeapifyTest2()
		{
			List<int> sourceData = new List<int>(new int[] { 0, 1, 2 });
			sourceData.HeapifyMin();
			var isHeap = IsMinHeap(sourceData);
			Assert.IsTrue(isHeap);
		}

		[TestMethod]
		public void DoesMinHeapifyTest3()
		{
			List<int> sourceData = new List<int>(new int[] { 2, 1, 0 });
			sourceData.HeapifyMin();
			var isHeap = IsMinHeap(sourceData);
			Assert.IsTrue(isHeap);
		}

		[TestMethod]
		public void DoesMinHeapifyTest4()
		{
			List<int> sourceData = new List<int>(new int[] { -5, -4, -3, -2, -1, 0, 1, 2, 3, 4, 5 });
			sourceData.HeapifyMin();
			var isHeap = IsMinHeap(sourceData);
			Assert.IsTrue(isHeap);
		}

		[TestMethod]
		public void DoesMinHeapifyTest5()
		{
			List<int> sourceData = new List<int>(new int[] { -5, -4, -3, -2, -1, 0, 1, 2, 3, 4, 5 });
			sourceData.Reverse();

			sourceData.HeapifyMin();
			var isHeap = IsMinHeap(sourceData);
			Assert.IsTrue(isHeap);
		}

		[TestMethod]
		public void DoesMinHeapifyRandomTest()
		{
			Random r = new Random(0);
			int testCount = 200;

			for (int i = 6; i < testCount + 6; i++)
			{
				var sourceData = BuildRandomList(i * -125, i * 125, i * 25, r);
				sourceData.HeapifyMin();
				bool isMinHeap = IsMinHeap(sourceData);
				Assert.IsTrue(isMinHeap);
			}
		}
		#endregion


		#region Max Heap Tests
		[TestMethod]
		public void DoesMaxHeapifyTest0()
		{
			List<int> sourceData = new List<int>();
			sourceData.HeapifyMax();
			var isHeap = IsMaxHeap(sourceData);
			Assert.IsTrue(isHeap);
		}

		[TestMethod]
		public void DoesMaxHeapifyTest1()
		{
			List<int> sourceData = new List<int>(new int[] { 0 });
			sourceData.HeapifyMax();
			var isHeap = IsMaxHeap(sourceData);
			Assert.IsTrue(isHeap);
		}

		[TestMethod]
		public void DoesMaxHeapifyTest2()
		{
			List<int> sourceData = new List<int>(new int[] { 0, 1, 2 });
			sourceData.HeapifyMax();
			var isHeap = IsMaxHeap(sourceData);
			Assert.IsTrue(isHeap);
		}

		[TestMethod]
		public void DoesMaxHeapifyTest3()
		{
			List<int> sourceData = new List<int>(new int[] { 2, 1, 0 });
			sourceData.HeapifyMax();
			var isHeap = IsMaxHeap(sourceData);
			Assert.IsTrue(isHeap);
		}

		[TestMethod]
		public void DoesMaxHeapifyTest4()
		{
			List<int> sourceData = new List<int>(new int[] { -5, -4, -3, -2, -1, 0, 1, 2, 3, 4, 5 });
			sourceData.HeapifyMax();
			var isHeap = IsMaxHeap(sourceData);
			Assert.IsTrue(isHeap);
		}

		[TestMethod]
		public void DoesMaxHeapifyTest5()
		{
			List<int> sourceData = new List<int>(new int[] { -5, -4, -3, -2, -1, 0, 1, 2, 3, 4, 5 });
			sourceData.Reverse();

			sourceData.HeapifyMax();
			var isHeap = IsMaxHeap(sourceData);
			Assert.IsTrue(isHeap);
		}

		[TestMethod]
		public void DoesMaxHeapifyRandomTest()
		{
			Random r = new Random(0);
			int testCount = 200;

			for (int i = 6; i < testCount + 6; i++)
			{
				var sourceData = BuildRandomList(i * -125, i * 125, i * 25, r);
				sourceData.HeapifyMax();
				bool isMaxHeap = IsMaxHeap(sourceData);
				Assert.IsTrue(isMaxHeap);
			}
		}
		#endregion


		private List<int> BuildRandomList(int minValue, int maxValue, int itemCount, Random random)
		{
			List<int> results = new List<int>(itemCount);
			for (int i = 0; i < itemCount; i++)
				results.Add(random.Next(minValue, maxValue));
			return results;
		}

		private bool IsMinHeap(List<int> heap)
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

		private bool IsMaxHeap(List<int> heap)
		{
			for (int i = heap.Count / 2; i >= 0; i--)
			{
				int left = i * 2 + 1;
				int right = left + 1;

				if (left >= heap.Count)
					continue;

				if (heap[i] < heap[left])
					return false;

				if (right >= heap.Count)
					continue;

				if (heap[i] < heap[right])
					return false;
			}

			return true;
		}
	}
}
