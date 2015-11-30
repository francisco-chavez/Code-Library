using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Unvi.DataStructures.Heaps;


namespace Unvi.DataStructures
{
	public class PriorityQueue<T>
	{
		#region Attributes
		private IHeap<int>					_heap;
		private Dictionary<int, Queue<T>>	_priorityMap;
		#endregion


		#region Properties
		public T Dequeue
		{
			get
			{
				if (Count == 0)
					throw new InvalidOperationException("Items cannot be removed from an empty queue.");

				int priority = _heap.Peek;
				T value = _priorityMap[priority].Dequeue();
				Count--;

				if (_priorityMap[priority].Count == 0)
				{
					_priorityMap.Remove(priority);
					_heap.Pop();
				}

				return value;
			}
		}

		public T Pop { get { return Dequeue; } }


		public T Peek
		{
			get
			{
				if (Count == 0)
					throw new InvalidOperationException("The queue contains no items to peek at.");

				int priority = _heap.Peek;
				return _priorityMap[priority].Peek();
			}
		}


		public int Count { get; private set; }
		#endregion


		#region Constructors
		public PriorityQueue()
			: this(new T[][] { })
		{
		}

		public PriorityQueue(IEnumerable<T> values, int priority = int.MinValue)
			: this(new IEnumerable<T>[] { values })
		{
		}

		public PriorityQueue(IEnumerable<IEnumerable<T>> values, int startingPriority = int.MaxValue)
		{
			_heap			= new ListHeap<int>(HeapType.Max);
			_priorityMap	= new Dictionary<int, Queue<T>>();
			Count			= 0;

			throw new NotImplementedException();
		}

		~PriorityQueue()
		{
		}
		#endregion


		#region Methods
		public void Enqueue(T value, int priority = int.MinValue)
		{
			if (!_priorityMap.ContainsKey(priority))
			{
				_priorityMap.Add(priority, new Queue<T>());
				_heap.Push(priority);
			}

			_priorityMap[priority].Enqueue(value);
			Count++;
		}

		public void Push(T value, int priority = int.MinValue)
		{
			Enqueue(value, priority);
		}
		#endregion
	}
}
