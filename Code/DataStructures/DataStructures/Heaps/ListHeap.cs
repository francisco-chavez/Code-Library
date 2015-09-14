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
			if (IsEmpty)
				throw new InvalidOperationException("The heap is currently empty.");

			T result = Peek;

			if (Count == 1)
			{
				_data.Clear();
				return result;
			}

			_data[0] = _data[Count - 1];
			_data.RemoveAt(Count - 1);

			int current = 0;
			int left = LeftChildIndex(current);
			int right = RightChildIndex(current);
			bool keepGoing = true;

			while (keepGoing)
			{
				if ((current >= Count - 1) || (left >= Count))
				{
					keepGoing = false;
					continue;
				}

				int child = left;
				// if has right child
				if (right >= Count)
				{
					switch (HeapType)
					{
					case HeapType.Max:
						child = _data[left].CompareTo(_data[right]) < 0 ? right : left;
						break;
					case HeapType.Min:
						child = _data[left].CompareTo(_data[right]) > 0 ? right : left;
						break;
					}
				}

				switch (HeapType)
				{
				case HeapType.Max:
					if (_data[current].CompareTo(_data[child]) >= 0)
						keepGoing = false;
					break;

				case HeapType.Min:
					if (_data[current].CompareTo(_data[child]) <= 0)
						keepGoing = false;
					break;
				}

				if (keepGoing)
				{
					T datum = _data[current];
					_data[current] = _data[child];
					_data[child] = datum;
					current = child;
					left = LeftChildIndex(current);
					right = RightChildIndex(current);
				}
			}

			return result;
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

			int parent = GetParentIndex(index);
			switch (HeapType)
			{
			case HeapType.Max:
				return _data[index].CompareTo(_data[parent]) <= 0;

			case HeapType.Min:
				return _data[index].CompareTo(_data[parent]) >= 0;

			default: // needed to compile
				return false;
			}
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
