using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Unvi.Algorithms.Sorting;


namespace AlgorithmsTesting
{
	[TestClass]
	public class HeapSortUnitTests
	{
		[TestMethod]
		public void HeapSortArray01()
		{
			int[] data = new int[] { };
			data.HeapSort();
			Assert.IsTrue(IsSorted(data));
		}

		[TestMethod]
		public void HeapSortArray02()
		{
			float[] data = new float[] { 3.1f };
			data.HeapSort();
			Assert.IsTrue(IsSorted(data));
		}

		[TestMethod]
		public void HeapSortArray03()
		{
			int[] data = new int[] { 1, 2 };
			data.HeapSort();
			Assert.IsTrue(IsSorted(data));
		}

		[TestMethod]
		public void HeapSortArray04()
		{
			int[] data = new int[] { 2, 1 };
			data.HeapSort();
			Assert.IsTrue(IsSorted(data));
		}

		#region Test the Sorted Check Method
		[TestMethod]
		public void TestCheck01()
		{
			int[] data = new int[] { 1, 2, 3 };
			Assert.IsTrue(IsSorted(data));
		}

		[TestMethod]
		public void TestCheck02()
		{
			int[] data = new int[] {-3, -1, 20 };
			Assert.IsTrue(IsSorted(data));
		}

		[TestMethod]
		public void TestCheck03()
		{
			int[] data = new int[] { 0, 0, 0, 1, 3, 5, 5 };
			Assert.IsTrue(IsSorted(data));
		}

		[TestMethod]
		public void TestCheck04()
		{
			int[] data = new int[] { 2, 1 };
			Assert.IsFalse(IsSorted(data));
		}
		#endregion

		private bool IsSorted<T>(T[] array)
			where T : IComparable<T>
		{
			// These guys are always sorted.
			if (array == null || array.Length < 2)
				return true;

			T previous = array[0];
			for (int i = 1; i < array.Length; i++)
			{
				if (array[i].CompareTo(previous) < 0)
					return false;
				previous = array[i];
			}

			return true;
		}
	}
}
