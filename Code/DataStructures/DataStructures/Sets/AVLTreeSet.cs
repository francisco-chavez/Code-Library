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
				if (Count == 0)
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
			if (value == null)
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

			if (newNode.Data.CompareTo(parent.Data) < 0)
				parent.Left = newNode;
			else
				parent.Right = newNode;

			Count++;
			RebalanceTree(newNode);
		}

		public void Remove(T value)
		{
			if (value == null)
				return;

			if (!Contains(value))
				return;

			if (Count == 1)
			{
				_root = null;
				Count = 0;
				return;
			}

			Node parent = GetParentNode(value);
			Node node = null;
			Node replacement = null;

			if (parent == null)
				node = _root;
			else
				node = value.CompareTo(parent.Data) < 0 ? parent.Left : parent.Right;

			if (node.Left == null)
			{
				replacement = node.Right;

				if (parent == null)
					_root = replacement;
				else if (value.CompareTo(parent.Data) < 0)
					parent.Left = replacement;
				else
					parent.Right = replacement;

				if (replacement != null)
					replacement.Parent = parent;

				node.Right = null;
				node.Parent = null;
				Count--;
				RebalanceTree(parent);

				return;
			}

			replacement = node.Left;
			while (replacement.Right != null)
				replacement = replacement.Right;

			Node rebalancePoint;
			if (replacement == node.Left)
			{
				rebalancePoint = replacement;
			}
			else
			{
				rebalancePoint = replacement.Parent;

				replacement.Parent.Right = replacement.Left;
				replacement.Left.Parent = replacement.Parent;

				replacement.Left = node.Left;
				if (node.Left != null)
					replacement.Left.Parent = replacement;
			}

			replacement.Right = node.Right;
			if (replacement.Right != null)
				replacement.Right.Parent = replacement;

			if (parent == null)
				_root = replacement;
			else if (replacement.Data.CompareTo(parent.Data) < 0)
				parent.Left = replacement;
			else
				parent.Right = replacement;
			replacement.Parent = parent;

			node.Left = null;
			node.Right = null;
			node.Parent = null;
			Count--;

			RebalanceTree(rebalancePoint);
		}

		public bool Contains(T value)
		{
			if (Count == 0)
				return false;

			var parent = GetParentNode(value);

			if (parent == null)
				return EqualityComparer<T>.Default.Equals(value, _root.Data);

			var node = value.CompareTo(parent.Data) < 0 ? parent.Left : parent.Right;
			if (node == null)
				return false;
			return EqualityComparer<T>.Default.Equals(value, node.Data);
		}

		public void Clear()
		{
			Clear(_root);
			_root = null;
			Count = 0;
		}
		#endregion


		#region Set Meta Data
		public bool IsSubsetOf(ISet<T> otherSet)
		{
			if (otherSet == null)
				return false;

			if (this == otherSet)
				return true;
			if (this.Count > otherSet.Count)
				return false;

			foreach (var value in this)
				if (!otherSet.Contains(value))
					return false;

			return true;
		}

		public bool IsSupersetOf(ISet<T> otherSet)
		{
			if (otherSet == null)
				return true;
			return otherSet.IsSubsetOf(this);
		}

		public bool IsProperSubsetOf(ISet<T> otherSet)
		{
			if(otherSet == null)
				return false;

			if(this.Count >= otherSet.Count)
				return false;
			
			return this.IsSubsetOf(otherSet);
		}

		public bool IsProperSupersetOf(ISet<T> otherSet)
		{
			if(otherSet == null)
				return true;
			if(this.Count <= otherSet.Count)
				return false;

			return this.IsSupersetOf(otherSet);
		}
		#endregion


		#region Set Creation
		public ISet<T> Instersection(ISet<T> otherSet)
		{
			if(otherSet == null || this.Count == 0 || otherSet.Count == 0)
				return new AVLTreeSet<T>();

			ISet<T> big = this;
			ISet<T> small = otherSet;

			if (small.Count > big.Count)
			{
				big = otherSet;
				small = this;
			}

			var result = new AVLTreeSet<T>();

			foreach (T value in small)
			{
				if(big.Contains(value))
					result.Add(value);
			}

			return result;
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
			AVLTreeSet<T> painted = new AVLTreeSet<T>();

			var current = _root;

			while (current != null)
			{
				if (current.Left != null && !painted.Contains(current.Left.Data))
				{
					current = current.Left;
				}
				else if (!painted.Contains(current.Data))
				{
					painted.Add(current.Data);
					yield return current.Data;
				}
				else if (current.Right != null && !painted.Contains(current.Right.Data))
				{
					current = current.Right;
				}
				else
				{
					current = current.Parent;
				}
			}

			painted.Clear();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
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
			if (_root == null)
				return null;

			// Root node doesn't have a parent.
			// If root node contains value return null
			if (EqualityComparer<T>.Default.Equals(value, _root.Data))
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

		private void RebalanceTree(Node startingPoint)
		{
			// Move up the tree while rotating nodes to keep things balanced.
			var current = startingPoint;
			while (current != null)
			{
				current.UpdateHeight();
				int balance = current.Balance;

				if (balance < -1)
				// Is right heavy
				{
					// Insure we have a right-right case before we rotate the
					// current node to the left.
					if (current.Right.Balance > 0)
						RotateRight(current.Right);
					current = RotateLeft(current);
				}
				else if (balance > 1)
				// Is left heavy
				{
					// Insure we have a left-left case before we rotate the 
					// current node to the right.
					if(current.Left.Balance < 0)
						RotateLeft(current.Left);
					current = RotateRight(current);
				}
				current = current.Parent;
			}
		}

		private Node RotateLeft(Node node)
		{
			var newCurrent = node.Right;

			// Unable to rotate left because there
			// is no node to take the place of our
			// current node
			if (node.Right == null)
				return node;

			node.Right = newCurrent.Left;
			if (node.Right != null)
				node.Right.Parent = node;

			newCurrent.Parent = node.Parent;
			if (newCurrent.Parent == null)
				_root = newCurrent;
			else if (newCurrent.Data.CompareTo(newCurrent.Parent.Data) < 0)
				newCurrent.Parent.Left = newCurrent;
			else
				newCurrent.Parent.Right = newCurrent;

			node.Parent = newCurrent;
			newCurrent.Left = node;

			node.UpdateHeight();
			newCurrent.UpdateHeight();

			//// We only rotate when we run up the tree. While running
			//// up the tree, the first thing we do is update the height
			//// of the nodes we visit. So the parent height doesn't need
			//// to be updated because it will be updated soon.
			//if (newCurrent != _root)
			//	newCurrent.Parent.UpdateHeight();

			return newCurrent;
		}

		private Node RotateRight(Node node)
		{
			// This is the node that will be taking the
			// place of the given node when rotation is
			// complete
			var newCurrent = node.Left;

			// Unable to rotate right because there
			// is no node to take the place of our
			// current node
			if (newCurrent == null)
				return node;

			node.Left = newCurrent.Right;
			if (node.Left != null)
				node.Left.Parent = node;

			newCurrent.Parent = node.Parent;
			if (newCurrent.Parent == null)
				_root = newCurrent;
			else if (newCurrent.Data.CompareTo(newCurrent.Parent.Data) < 0)
				newCurrent.Parent.Left = newCurrent;
			else
				newCurrent.Parent.Right = newCurrent;

			node.Parent = newCurrent;
			newCurrent.Right = node;

			// The order in which we update the heights of
			// these nodes is critical.
			node.UpdateHeight();                    // This node had a change in children
			newCurrent.UpdateHeight();              // This node had a change in children
													//if (newCurrent != _root)
													//	newCurrent.Parent.UpdateHeight();	// This node had a change in children

			// Return the node that has the tree location of
			// the node that was rotated out of the given position.
			return newCurrent;
		}

		/// <summary>
		/// Recursive method for clearning the a tree using pre-order DFS.
		/// </summary>
		private void Clear(Node n)
		{
			if (n == null)
				return;

			Clear(n.Left);
			Clear(n.Right);

			n.Left = null;
			n.Right = null;
			n.Parent = null;
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

			public int	Height	{ get; private set; }
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


			#region Public Methods
			public void UpdateHeight()
			{
				int leftHeight = Left != null ? Left.Height : 0;
				int rightHeight = Right != null ? Right.Height : 0;

				Height = 1 + Math.Max(leftHeight, rightHeight);
			}
			#endregion
		}
		#endregion
	}
}
