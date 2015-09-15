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
			if (data == null)
				_data = new List<T>(10);
			else if (data.Any(datum => { return datum == null; }))
				throw new NullReferenceException("Given data contains null values");
			else
				_data = data != null ? new List<T>(data) : new List<T>(10);

			Heapify();
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

			FixHeap(0);

			return result;
		}

		public void Push(T value)
		{
			if(value == null)
				throw new ArgumentNullException("value");

			int child = _data.Count;
			int parent = FindParent(child);
			_data.Add(value);

			while (!HeapValidFor(parent, child))
			{
				_data[child] = _data[parent];
				_data[parent] = value;
				child = parent;
				parent = FindParent(child);
			}
		}
		#endregion


		#region Helper Methods
		private void Heapify()
		{
			// Nothing to do.
			if(Count == 0 || Count == 1)
				return;

			// Lets skip all (or most) of the leaf nodes. These guys
			// already meet the heap property by being leafs.
			for (int i = Count / 2; i >= 0; i--)
				FixHeap(i);
		}

		/// <summary>
		/// Starting from a parent, this method will move the parent value down
		/// the tree until the value falls into a node that meets the heap property
		/// with said node's children.
		/// </summary>
		/// <remarks>
		/// In order for this method to work properly, both child trees of the given
		/// parent must be proper heaps.
		/// </remarks>
		private void FixHeap(int parent)
		{
			while (!HeapValidFor(parent))
			{
				int child = FindLeft(parent);
				int right = FindRight(parent);

				// if has right child
				if (right < Count)
				{
					// One or two children are breaking the heap property. If we switch with
					// the child that breaks it the most, then the heap property will be 
					// restored at this point in the tree.
					switch (HeapType)
					{
					case HeapType.Max:
						child = _data[child].CompareTo(_data[right]) < 0 ? right : child;
						break;
					case HeapType.Min:
						child = _data[child].CompareTo(_data[right]) > 0 ? right : child;
						break;
					}
				}

				T datum = _data[parent];
				_data[parent] = _data[child];
				_data[child] = datum;
				parent = child;
			}
		}


		/// <summary>
		/// The method tells us if the heap property is valid for the given parent
		/// and both of its children.
		/// </summary>
		private bool HeapValidFor(int parent)
		{
			int left = FindLeft(parent);
			int right = FindRight(parent);

			if(!HeapValidFor(parent, left))
				return false;
			if (!HeapValidFor(parent, right))
				return false;

			return true;
		}

		/// <summary>
		/// The method tells us if the values in the given parent index and the given
		/// child index fulfill the heap property for the HeapType.
		/// </summary>
		private bool HeapValidFor(int parent, int child)
		{
			// Safety checks
			if (parent >= Count)
				return true;
			if (child >= Count)
				return true;
			if (parent < 0)
				return true;
			if (child < 0)
				return true;

			switch (this.HeapType)
			{
			case HeapType.Max:
				// Parent value is smaller than child value.
				if (_data[parent].CompareTo(_data[child]) < 0)
					return false;
				break;

			case HeapType.Min:
				// Parent value is larger than child value.
				if (_data[parent].CompareTo(_data[child]) > 0)
					return false;
				break;

			default:
				// How did you even get here?
				throw new InvalidOperationException(string.Format("HeapType set to unsuported type '{0}'.", this.HeapType));
			}

			return true;
		}


		private int FindParent(int childIndex)
		{
			return (childIndex - 1) / 2;
		}

		private int FindLeft(int parentIndex)
		{
			return parentIndex * 2 + 1;
		}

		private int FindRight(int parentIndex)
		{
			return parentIndex * 2 + 2;
		}
		#endregion
	}
}
