using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Unvi.DataStructures.Sets
{
	public class AVLTreeSet<T>
		: ISet<T> where T : IComparable<T>
	{
		#region Attributes
		private int		_count;
		private Node	_root;
		#endregion


		#region Properties
		public int Count
		{
			get { return _count; }
			private set { _count = value; }
		}

		public int Height
		{
			get
			{
				if(Count == 0)
					return 0;
				else
					return _root.Height;
			}
		}
		#endregion


		#region Constructors
		public AVLTreeSet()
		{
			_root = null;
			Count = 0;
		}

		/// <summary>
		/// Destructor for AVLTreeSet. This isn't really needed, but it should break
		/// the tree down a bit faster making it easier on the GC.
		/// </summary>
		~AVLTreeSet()
		{
			Clear();
		}
		#endregion


		#region Public Methods

		#region Set Manipulation
		public void Add(T value)
		{
			// Null is not a valid value
			if(value == null)
				return;

			// The value is already present
			if (Contains(value))
				return;

			if (_root == null)
			{
				_root = new Node(value);
				Count++;
				return;
			}

			var parent = GetParentNode(value);
			var newNode = new Node(value) { Parent = parent };

			if(newNode.Data.CompareTo(parent.Data) < 0)
				parent.Left = newNode;
			else
				parent.Right = newNode;

			Count++;
			RebalanceTree(newNode);
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


		#region Helper Methods
		/// <summary>
		/// Find a reference to the node that should be the parent to the node containing
		/// the given value. If there is no node containing the current value, return the
		/// the node that would be the parent if we were to insert a node containing the
		/// current value. Keep in mind, that the root node has no parent, so if root 
		/// contains the value or the value should be inserted at root, then this will 
		/// return null.
		/// </summary>
		private Node GetParentNode(T value)
		{
			if(_root == null)
				return null;

			// Root node doesn't have a parent.
			// If root node contains value return null
			if(EqualityComparer<T>.Default.Equals(value, _root.Data))
				return null;

			Node current = _root;
			Node parent = null;

			// while (current != null) and (value != current.data)
			while (current != null && !EqualityComparer<T>.Default.Equals(value, current.Data))
			{
				parent = current;
				current = (value.CompareTo(parent.Data) < 0) ? parent.Left : parent.Right;
			}

			return parent;
		}

		private void RebalanceTree(Node newNode)
		{
			throw new NotImplementedException();
		}
		#endregion


		#region Helper Classes
		private class Node
		{
			#region Properties
			public Node Parent	{ get; set; }
			public Node Left	{ get; set; }
			public Node Right	{ get; set; }

			public T	Data	{ get; set; }

			public int	Height	{ get; set; }
			public int	Balance
			{
				get
				{
					int leftHeight  = Left  == null ? 0 : Left.Height;
					int rightHeight = Right == null ? 0 : Left.Height;

					return leftHeight - rightHeight;
				}
			}
			#endregion


			#region Constructors
			public Node()
				: this(Activator.CreateInstance<T>())
			{
			}

			public Node(T value)
			{
				this.Parent = null;
				this.Left	= null;
				this.Right	= null;
				this.Height = 1;

				this.Data	= value;
			}
			#endregion
		}
		#endregion
	}
}
