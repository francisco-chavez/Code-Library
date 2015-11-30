using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Unvi.DataStructures.Heaps
{
	public class MinHeap<T>
		: IHeap<T> where T: IComparable<T>
	{
		#region Attributes
		#endregion


		#region Properties

		public HeapType HeapType
		{
			get { throw new NotImplementedException(); }
		}

		public T Peek
		{
			get { throw new NotImplementedException(); }
		}

		public int Count
		{
			get { throw new NotImplementedException(); }
		}

		public bool IsEmpty
		{
			get { throw new NotImplementedException(); }
		}

		#endregion


		#region Constructors
		#endregion


		#region Public Methods

		public void Push(T value)
		{
			throw new NotImplementedException();
		}

		public void Clear()
		{
			throw new NotImplementedException();
		}

		public T Pop()
		{
			throw new NotImplementedException();
		}

		#endregion

	}
}
