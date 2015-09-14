using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Unvi.DataStructures
{
	public interface IHeap<T>
		where T : IComparable<T>
	{
		HeapType	HeapType	{ get; }
		T			Peek		{ get; }
		int			Count		{ get; }
		bool		IsEmpty		{ get; }

		void Push(T value);
		T Pop();
	}
}
