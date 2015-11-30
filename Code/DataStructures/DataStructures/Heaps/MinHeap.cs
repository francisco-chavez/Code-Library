using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Unvi.DataStructures.Heaps
{
	public class MinHeap<T>
		: HeapBase<T> where T: IComparable<T>
	{
		public override HeapType HeapType
		{
			get { return HeapType.Min; }
		}

		protected override bool HeapValidFor(int parent, int child)
		{
			if (child >= Count)
				return true;

			return _data[parent].CompareTo(_data[child]) <= 0;
		}

		/// <summary>
		/// We only enter this method when the value in parent is greater than the value in left
		/// and/or the value in right. This method will return the child with the lowest value
		/// because that value will do the best job at maintaining the heap when it becomes
		/// the parent value.
		/// </summary>
		/// <param name="parent">A value geater than or equal to 0 and less than left.</param>
		/// <param name="left">A value equal to parent time two plus 1 and less than Count.</param>
		/// <param name="right">A value equal to left + 1 and less than Count.</param>
		/// <returns>The index that holds the minimum value.</returns>
		protected override int GetBestParent(int parent, int left, int right)
		{
			return _data[left].CompareTo(_data[right]) <= 0 ? left : right;
		}
	}
}
