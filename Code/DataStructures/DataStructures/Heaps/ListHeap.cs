using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unvi.DataStructures.Heaps
{
	public class ListHeap<T>
		: IHeap<T> where T : IComparable<T>
	{
		#region Attributes
		private List<T> _data;
		#endregion


		#region Properties
		public int Count
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		public HeapType HeapType
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		public bool IsEmpty
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		public T Peek
		{
			get
			{
				throw new NotImplementedException();
			}
		}
		#endregion


		#region Constructors
		public ListHeap(HeapType heapType)
			: this(heapType, new T[] { })
		{ }

		public ListHeap(HeapType heapType, IEnumerable<T> data)
		{
			_data = data != null ? new List<T>(data) : new List<T>(10);
		}
		#endregion


		#region Public Methods
		public T Pop()
		{
			throw new NotImplementedException();
		}

		public void Push(T value)
		{
			throw new NotImplementedException();
		}
		#endregion
	}
}
