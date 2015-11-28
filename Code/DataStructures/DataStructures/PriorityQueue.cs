using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Unvi.DataStructures
{
	public class PriorityQueue<T>
	{
		public T Dequeue { get { throw new NotImplementedException(); } }
		public T Pop { get { return Dequeue; } }

		public T Peek { get { throw new NotImplementedException(); } }

		public int Count { get; private set; }


		#region Constructors
		public PriorityQueue()
		{
			throw new NotImplementedException();
		}

		public PriorityQueue(IEnumerable<T> values, int priority = int.MinValue)
		{
			throw new NotImplementedException();
		}

		public PriorityQueue(IEnumerable<IEnumerable<T>> values, int startingPriority = int.MinValue)
		{
			throw new NotImplementedException();
		}

		~PriorityQueue()
		{
		}
		#endregion


		public void Enqueue(T value, int priority = int.MinValue) { throw new NotImplementedException(); }
		public void Push(T value, int priority = int.MinValue) { throw new NotImplementedException(); }
	}
}
