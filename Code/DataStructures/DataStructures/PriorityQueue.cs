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
		public T Pop { get { throw new NotImplementedException(); } }

		public T Peek { get { throw new NotImplementedException(); } }

		public int Count { get; private set; }


		public void Enqueue(T value, int priority = int.MinValue) { throw new NotImplementedException(); }
		public void Push(T value, int priority = int.MinValue) { throw new NotImplementedException(); }
	}
}
