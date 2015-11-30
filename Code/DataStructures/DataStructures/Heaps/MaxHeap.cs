using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Unvi.DataStructures.Heaps
{
	public class MaxHeap<T>
		: HeapBase<T> where T : IComparable<T>
	{
		public override HeapType HeapType
		{
			get { return HeapType.Max; }
		}

		public MaxHeap() : base() { }
		public MaxHeap(IEnumerable<T> data) : base(data) { }

		protected override bool HeapValidFor(int parent, int child)
		{
			if (child >= Count)
				return true;

			return _data[parent].CompareTo(_data[child]) >= 0;
		}

		protected override int GetBestParent(int parent, int left, int right)
		{
			return _data[left].CompareTo(_data[right]) >= 0 ? left : right;
		}
	}
}
