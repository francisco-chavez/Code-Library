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
		private IHeap<int>				_heap;
		private Dictionary<int, Queue> _priorityMap;
		#endregion


		#region Properties
		public T Dequeue { get { throw new NotImplementedException(); } }
		public T Pop { get { return Dequeue; } }

		public T Peek { get { throw new NotImplementedException(); } }

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
			_heap			= new ListHeap<int>(HeapType.Min);
			_priorityMap	= new Dictionary<int, Queue>();
			Count			= 0;


			throw new NotImplementedException();
		}

		~PriorityQueue()
		{
		}
		#endregion


		#region Methods
		public void Enqueue(T value, int priority = int.MinValue) { throw new NotImplementedException(); }
		public void Push(T value, int priority = int.MinValue)
		{
			Enqueue(value, priority);
		}
		#endregion
	}
}
