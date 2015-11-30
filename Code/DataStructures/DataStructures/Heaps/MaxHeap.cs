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
			get { throw new NotImplementedException(); }
		}

		protected override bool HeapValidFor(int parent, int child)
		{
			throw new NotImplementedException();
		}

		protected override int GetBestParent(int parent, int left, int right)
		{
			throw new NotImplementedException();
		}
	}
}
