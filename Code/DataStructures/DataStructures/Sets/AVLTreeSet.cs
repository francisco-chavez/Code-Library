using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Unvi.DataStructures.Sets
{
	public class AVLTreeSet<T>
		: ISet<T>
	{
		#region Attributes
		private int _count;
		#endregion


		#region Properties
		public int Count
		{
			get { return _count; }
			private set { _count = value; }
		}
		#endregion


		#region Constructors
		public AVLTreeSet()
		{
			Count = 0;
		}
		#endregion


		#region Public Methods

		#region Set Manipulation
		public void Add(T value)
		{
			throw new NotImplementedException();
		}

		public void Remove(T value)
		{
			throw new NotImplementedException();
		}

		public bool Contains(T value)
		{
			throw new NotImplementedException();
		}

		public void Clear()
		{
			throw new NotImplementedException();
		}
		#endregion


		#region Set Meta Data
		public bool IsSubsetOf(ISet<T> otherSet)
		{
			throw new NotImplementedException();
		}

		public bool IsSupersetOf(ISet<T> otherSet)
		{
			throw new NotImplementedException();
		}

		public bool IsProperSubsetOf(ISet<T> otherSet)
		{
			throw new NotImplementedException();
		}

		public bool IsProperSupersetOf(ISet<T> otherSet)
		{
			throw new NotImplementedException();
		}
		#endregion


		#region Set Creation
		public ISet<T> Instersection(ISet<T> otherSet)
		{
			throw new NotImplementedException();
		}

		public ISet<T> Union(ISet<T> otherSet)
		{
			throw new NotImplementedException();
		}

		public ISet<T> Complement(ISet<T> otherSet)
		{
			throw new NotImplementedException();
		}

		public ISet<T> SymmetricDifference(ISet<T> otherSet)
		{
			throw new NotImplementedException();
		}
		#endregion


		public IEnumerator<T> GetEnumerator()
		{
			throw new NotImplementedException();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			throw new NotImplementedException();
		}
		#endregion


		#region Helper Classes
		private class Node
		{
			public Node Parent	{ get; set; }
			public Node Left	{ get; set; }
			public Node Right	{ get; set; }
			public int	Height	{ get; set; }

			public T	Data	{ get; set; }
		}
		#endregion
	}
}
