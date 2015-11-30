﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unvi.DataStructures.Heaps
{
	public abstract class HeapBase<T>
		: IHeap<T> where T : IComparable<T>
	{
		#region Attributes
		protected List<T> _data;
		#endregion

		#region Properties
		public abstract HeapType HeapType { get; }

		public T Peek
		{
			get
			{
				if (IsEmpty)
					throw new InvalidOperationException("The heap is currently empty.");

				return _data[0];
			}
		}

		public int Count
		{
			get { return _data.Count; }
		}

		public bool IsEmpty
		{
			get { return _data.Count == 0; }
		}
		#endregion


		#region Constructors

		public HeapBase()
		{
			_data = new List<T>(10);
		}

		~HeapBase()
		{
			_data.Clear();
			_data = null;
		}

		#endregion


		#region Public Methods

		public void Push(T value)
		{
			if (value == null)
				throw new ArgumentNullException("value");

			int child = _data.Count;
			int parent = (child - 1) / 2;
			_data.Add(value);

			while (!HeapValidFor(parent, child))
			{
				_data[child] = _data[parent];
				_data[parent] = value;
				child = parent;
				parent = (child - 1) / 2;
			}
		}

		public void Clear()
		{
			_data.Clear();
		}

		public T Pop()
		{
			if (IsEmpty)
				throw new InvalidOperationException("The heap is currently empty..");

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

		#endregion


		#region Helper Methods
		protected bool HeapValidFor(int parent, out int left, out int right)
		{
			left  = parent * 2 + 1;
			right = parent * 2 + 2;

			if (!HeapValidFor(parent, left))
				return false;
			if (!HeapValidFor(parent, right))
				return false;

			return true;
		}

		protected abstract bool HeapValidFor(int parent, int child);

		protected void FixHeap(int parent)
		{
			int child;
			int right;

			// Can I replace the while loop for an if statment?
			// Do I need to call FixHeap again when I'm in a loop?
			// -FCT
			while (!HeapValidFor(parent, out child, out right))
			{
				if (right < Count)
					child = GetBestParent(parent, child, right);

				T datum = _data[parent];
				_data[parent] = _data[child];
				parent = child;
				FixHeap(child);
			}
		}

		protected abstract int GetBestParent(int parent, int left, int right);
		#endregion
	}
}
