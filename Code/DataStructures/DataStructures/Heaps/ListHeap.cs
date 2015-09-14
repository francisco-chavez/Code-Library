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
		public int		Count		{ get { return _data.Count; } }
		public HeapType HeapType	{ get; private set; }
		public bool		IsEmpty		{ get { return Count == 0; } }

		public T		Peek
		{
			get
			{
				if (IsEmpty)
					throw new InvalidOperationException("The heap is currently empty.");

				return _data[0];
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
			throw new NotImplementedException();
		}
		#endregion


		#region Public Methods
		public T Pop()
		{
			throw new NotImplementedException();
		}

		public void Push(T value)
		{
			if(value == null)
				throw new ArgumentNullException("value");

			int index = _data.Count;
			_data.Add(value);

			while (!IsValidChild(index) && index > 0)
			{
				int parentIndex = GetParentIndex(index);
				_data[index] = _data[parentIndex];
				_data[parentIndex] = value;
				index = parentIndex;
			}
		}
		#endregion


		#region Helper Methods
		private bool IsValidChild(int index)
		{
			if (index == 0)
				return true;

			int parentIndex = GetParentIndex(index);
			bool result = false;
			switch (HeapType)
			{
			case HeapType.Max:
				result = _data[index].CompareTo(_data[parentIndex]) <= 0;
				break;
			case HeapType.Min:
				result = _data[index].CompareTo(_data[parentIndex]) >= 0;
				break;
			}
			return result;
		}

		private int GetParentIndex(int childIndex)
		{
			return (childIndex - 1) / 2;
		}

		private int LeftChildIndex(int parentIndex)
		{
			return parentIndex * 2 + 1;
		}

		private int RightChildIndex(int parentIndex)
		{
			return parentIndex * 2 + 2;
		}
		#endregion
	}
}
