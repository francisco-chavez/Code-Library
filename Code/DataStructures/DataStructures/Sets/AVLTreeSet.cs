using System;
using System.Collections;
using System.Collections.Generic;


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

		/// <summary>
		/// Gets the number of elements contained in the AVLTreeSet&lt;<typeparamref name="T"/>&gt;
		/// </summary>
		/// <typeparam name="T">
		/// The element type of the Set.
		/// </typeparam>
		public int Count
		{
			get { return _count; }
			private set { _count = value; }
		}

		public bool IsReadOnly
		{
			get { return false; }
		}

		/// <summary>
		/// Returns the height of the tree.
		/// </summary>
		public int Height
		{
			get
			{
				if (Count == 0)
					return 0;
				
				return _root.Height;
			}
		}

		/// <summary>
		/// Tells us if this set contains the specicified item.
		/// </summary>
		public bool this[T item]
		{
			get { return this.Contains(item); }
		}
		#endregion


		#region Constructors
		public AVLTreeSet() : this(null) { }

		public AVLTreeSet(IEnumerable<T> collection)
		{
			_count	= 0;
			_root	= null;

			if (collection == null)
				return;

			foreach (T item in collection)
				this.Add(item);
		}

		~AVLTreeSet()
		{
			Clear();
		}
		#endregion


		#region Public Methods
		/// <summary>
		/// Adds an element to the current set and returns a value to indicate if the
		/// element was successfully added.
		/// </summary>
		/// <param name="item">
		/// The element to add to the set.
		/// </param>
		/// <returns>
		/// True if the element is added to the set; False if the element is already
		/// int the set.
		/// </returns>
		public bool Add(T item)
		{
			if (IsReadOnly)
				throw new NotSupportedException("Adding items to a read-only Set is not supported");

			if (item == null)
				return false;

			if (Contains(item))
				return false;

			// Base Case
			if (_root == null)
			{
				_root = new Node(item);
				Count++;
				return true;
			}

			var parent  = GetParentNode(item);
			var newNode = new Node(item) { Parent = parent };

			if (newNode.Data.CompareTo(parent.Data) < 0)
				parent.Left = newNode;
			else
				parent.Right = newNode;

			Count++;
			RebalanceTree(newNode);
			return true;
		}	// End +Add(item: T): bool

		/// <summary>
		/// Removes an element from the current set and returns a value to indicate if the
		/// element was successfully added.
		/// </summary>
		/// <param name="item">
		/// The element to remove from the set.
		/// </param>
		/// <returns>
		/// True if the element is removed from the set; False if the element wasn't in the
		/// set.
		/// </returns>
		public bool Remove(T item)
		{
			if (IsReadOnly)
				throw new NotSupportedException("Removing items from a read-only Set is not supported.");

			// We don't store null values.
			if (item == null)
				return false;

			// There was no item to remove.
			if (!Contains(item))
				return false;

			// Base case.
			// The value removed was the only value in the tree.
			if (Count == 1)
			{
				_root = null;
				Count = 0;
				return true;
			}

			Node parent = GetParentNode(item);
			Node node = null;
			Node replacement = null;

			///
			/// Find node to remove
			///
			if (parent == null)
				node = _root;
			else
				node = item.CompareTo(parent.Data) < 0 ? parent.Left : parent.Right;

			///
			/// Find a node in the tree that can take up the position of the
			/// node that is being removed.
			/// 

			// Edge Case: The node being removed has no left children
			if (node.Left == null)
			{
				replacement = node.Right;

				if (parent == null)
					_root = replacement;
				else if (item.CompareTo(parent.Data) < 0)
					parent.Left = replacement;
				else
					parent.Right = replacement;

				if (replacement != null)
					replacement.Parent = parent;

				node.Right = null;
				node.Parent = null;
				Count--;
				RebalanceTree(parent);

				return true;
			}

			// Find the right-most (largest valued) child in the left subtree
			// of the node that will be removed.
			replacement = node.Left;
			while (replacement.Right != null)
				replacement = replacement.Right;

			///
			/// More the replacement node into the tree location of the node
			/// that is being replaced.
			///
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

			///
			/// Clear out any refereances the that the removed node has to
			/// the tree. We don't really need to this, but it will be helpfull
			/// if we decide implement a Node Pool to pull new Nodes from.
			///
			node.Left = null;
			node.Right = null;
			node.Parent = null;
			Count--;

			///
			/// Rebalance the tree. This will also update the height of the replacement node
			/// because it will be in the rebalance path.
			/// 
			RebalanceTree(rebalancePoint);
			return true;
		}	// End +Remove(item: T): bool

		/// <summary>
		/// Removes all items from the AVLTreeSet&lt;<paramref name="T"/>&gt;.
		/// </summary>
		public void Clear()
		{
			Clear(_root);
			_root = null;
			Count = 0;
		}	// End +Clear()


		/// <summary>
		/// Determines whether the AVLTreeSet&lt;<typeparamref name="T"/>&gt; contains a specific value.
		/// </summary>
		/// <param name="item">
		/// The object to locate in the AVLTreeSet&lt;<typeparamref name="T"/>&gt;.
		/// </param>
		/// <returns>
		/// True if item is found in the AVLTreeSet&lt;<typeparamref name="T"/>&gt;; otherwise, False.
		/// </returns>
		public bool Contains(T item)
		{
			if (Count == 0)
				return false;

			var parent = GetParentNode(item);

			if (parent == null)
				return EqualityComparer<T>.Default.Equals(item, _root.Data);

			var node = item.CompareTo(parent.Data) < 0 ? parent.Left : parent.Right;
			if (node == null)
				return false;
			
			return EqualityComparer<T>.Default.Equals(item, node.Data);
		}	// +Contains(item: T): bool

		/// <summary>
		/// Determines whether the current set overlaps with the specified collection.
		/// </summary>
		/// <param name="other">The collection t compare to the current set.</param>
		/// <returns>
		/// True if the current set and other share at least one common element; otherwise,
		/// False.
		/// </returns>
		public bool Overlaps(IEnumerable<T> other)
		{
			if (other == null)
				throw new ArgumentNullException();

			if (Object.ReferenceEquals(this, other))
				return true;

			foreach (var item in other)
				if (this.Contains(item))
					return true;

			return false;
		}	// End +Overlaps(other: IEnumerable<T>): bool

		/// <summary>
		/// Determines whether the current set and the specified collection contain the
		/// same elements.
		/// </summary>
		/// <param name="other">The collection to compare to the current set.</param>
		/// <returns>True if the current set is equal to other; otherwise, False.</returns>
		public bool SetEquals(IEnumerable<T> other)
		{
			if (other == null)
				throw new ArgumentNullException();

			//
			// I could do "return this.IsSuperset(other) && this.IsSubset(other)" but this
			// is a bit faster codewise.
			// -FCT
			//

			int count = 0;
			foreach (T item in other)
			{
				if (!this.Contains(item))
					return false;

				count++;
			}

			return count == Count;
		}	// End +SetEquals(otheer: IEnumerable<T>): bool

		/// <summary>
		/// Returns an enumerator that iterates through the collection.
		/// </summary>
		/// <returns>
		/// Returns an IEnumerator&lt;<paramref name="T"/>&gt; that can be used to iterate through
		/// the collection.
		/// </returns>
		public IEnumerator<T> GetEnumerator()
		{
			if (_root == null)
				yield break;

			var stack = new Node[Height];
			int loc = 0;
			var current = _root;

			FillStack(stack, ref loc, current);
			while (loc > 0)
			{
				current = stack[--loc];
				yield return current.Data;

				current = current.Right;
				FillStack(stack, ref loc, current);
			}
		}	// +End GetEnumerator(): IEnumerator<T>

		/// <summary>
		/// Copies the elements of the AVLTreeSet&lt;<typeparamref name="T"/>&gt; to an Array,
		/// starting at a particular Array index.
		/// </summary>
		/// <param name="array">
		/// The one-dimensional Array that is the destination of the elements
		/// copied from the AVLTreeSet&lt;<typeparamref name="T"/>&gt;. The Array must
		/// have zero-based indexing.
		/// </param>
		/// <param name="arrayIndex">
		/// The zero-based index in array at which copying begins.
		/// </param>
		public void CopyTo(T[] array, int arrayIndex)
		{
			// We could get away with only putting in what fits, but the interface we're using states
			// that we should throw an exception if there isn't enough space in the array.
			if (array.Length - arrayIndex <= this.Count)
				throw new ArgumentException("The number of items in the AVLTreeSet<T> is greater than the available space in the array.");

			using (var enumerator = this.GetEnumerator())
			{
				for (int i = arrayIndex; i < array.Length && enumerator.MoveNext(); i++)
					array[i] = enumerator.Current;
			}
		}	// +CopyTo(array: T[], arrayIndex: int)


		/// <summary>
		/// Determines whether the current set is a proper (strict) subset of a specified
		/// collection.
		/// </summary>
		/// <param name="other">The collection to compare the current set.</param>
		/// <returns>True if the current set is a proper subset of other; otherwise, false.</returns>
		public bool IsProperSubsetOf(IEnumerable<T> other)
		{
			if (other == null)
				throw new ArgumentNullException();

			if (Object.ReferenceEquals(this, other))
				return false;

			var otherSet = other as ISet<T> ?? new AVLTreeSet<T>(other);

			if (this.Count >= otherSet.Count)
				return false;
			return IsSubsetOf(otherSet);
		}	// End +IsProperSubsetOf(other: IEnumerable<T>): bool

		/// <summary>
		/// Determines whether a set is a subset of a specified collection.
		/// </summary>
		/// <param name="other">The collection to compare to the current set.</param>
		/// <returns>True if the current set is a subset of other; otherwise, False.</returns>
		public bool IsSubsetOf(IEnumerable<T> other)
		{
			if (other == null)
				throw new ArgumentNullException();

			if (Object.ReferenceEquals(this, other))
				return true;

			var otherSet = other as ISet<T> ?? new AVLTreeSet<T>(other);
			
			return IsSubsetOf(otherSet);
		}	// End +IsSubsetOf(other: IEnumerable<T>): bool

		/// <summary>
		/// Determines whether the currsent set is a proper (strict) superset of a specified
		/// collection.
		/// </summary>
		/// <param name="other">The collection to compare to the current set.</param>
		/// <returns>True if the current set is a proper superset of other; otherwise, False.</returns>
		public bool IsProperSupersetOf(IEnumerable<T> other)
		{
			if (other == null)
				throw new ArgumentNullException();

			if (Object.ReferenceEquals(this, other))
				return false;

			int count = 0;
			foreach (var item in other)
			{
				if (!this.Contains(item))
					return false;

				count++;
			}

			return (Count < this.Count);
		}	// End +IsProperSupersetOf(other: IEnumerable<T>): bool

		/// <summary>
		/// Determines whether the current set is a superset of a specified collection.
		/// </summary>
		/// <param name="other">The collection to compare to the current set.</param>
		/// <returns>True if the current set is a superset of other; otherwise, false.</returns>
		public bool IsSupersetOf(IEnumerable<T> other)
		{
			if (other == null)
				throw new ArgumentNullException();

			if (Object.ReferenceEquals(this, other))
				return true;

			foreach (var item in other)
				if (!this.Contains(item))
					return false;

			return true;
		}	// End +IsSupersetOf(other: IEnumerable<T>): other


		/// <summary>
		/// Removes all elements in the specified collection from the current set.
		/// </summary>
		/// <param name="other">The collection of items to remove from the set.</param>
		public void ExceptWith(IEnumerable<T> other)
		{
			if (other == null)
				throw new ArgumentNullException();

			if (Object.ReferenceEquals(this, other))
			{
				this.Clear();
				return;
			}

			foreach (var item in other)
				this.Remove(item);
		}	// End +ExceptWith(other: IEnumerable<T>)

		/// <summary>
		/// Modifies the current set so that it contains only elements that are also
		/// in a specified collection.
		/// </summary>
		/// <param name="other">The collection to compare to the current set.</param>
		public void IntersectWith(IEnumerable<T> other)
		{
			if (other == null)
				throw new ArgumentNullException();

			if (Object.ReferenceEquals(this, other))
				return;

			ISet<T> otherSet = other as ISet<T> ?? new AVLTreeSet<T>(other);
			T[] array = new T[this.Count];
			this.CopyTo(array, 0);
			foreach (var item in array)
				if (!otherSet.Contains(item))
					this.Remove(item);
		}	// End +IntersectWith(other: IEnumerable<T>)

		/// <summary>
		/// Modifies the current set so that it contains all elements that are present
		/// in either the current set or the specified collection.
		/// </summary>
		/// <param name="other">The collection to compare to the current set.</param>
		public void UnionWith(IEnumerable<T> other)
		{
			if (other == null)
				throw new ArgumentNullException();

			if (Object.ReferenceEquals(this, other))
				return;

			foreach (var item in other)
				this.Add(item);
		}	// End +UnionWith(other: IEnumerable<T>);

		/// <summary>
		/// Removes all elements in the specified collection from the current set.
		/// </summary>
		/// <param name="other">The collection of items to remove from the set.</param>
		public void SymmetricExceptWith(IEnumerable<T> other)
		{
			if (other == null)
				throw new ArgumentNullException();

			if (Object.ReferenceEquals(this, other))
			{
				this.Clear();
				return;
			}

			foreach (var item in other)
				this.Remove(item);
		}	// End +SymmetricExceptWith(other: IEnumerable<T>)
		#endregion


		#region ???
		/// <summary>
		/// Returns an enumerator that iterates through the collection.
		/// </summary>
		/// <returns>
		/// Returns an IEnumerator that can be used to iterate through
		/// the collection.
		/// </returns>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}	// End IEnumerable.GetEnumerator(): IEnumerator

		/// <summary>
		/// Adds an element to the current set and returns a value to indicate if the
		/// element was successfully added.
		/// </summary>
		/// <param name="item">
		/// The element to add to the set.
		/// </param>
		void ICollection<T>.Add(T item)
		{
			this.Add(item);
		}	// End ICollection<T>.Add(item: T)
		#endregion


		#region Helper Methods
		/// <summary>
		/// Tells us if this object is a subset of other.
		/// </summary>
		/// <param name="other">Is an object that implements ISet&lt<typeparamref name="T"/>&gt;.</param>
		private bool IsSubsetOf(ISet<T> other)
		{
			if (this.Count > other.Count)
				return false;

			foreach (var item in this)
				if (!other.Contains(item))
					return false;

			return true;
		}	// End -IsSubsetOf(other: ISet<T>): bool

		/// <summary>
		/// This method is used to fill in the stack that is being used it enumerate 
		/// over the collection.
		/// </summary>
		/// <param name="stack">
		/// This is the stack on which we place items.
		/// </param>
		/// <param name="index">
		/// The location on which the next item in the stack will be placed.
		/// </param>
		/// <param name="current">
		/// The current item to be placed on the stack. It's also the starting point 
		/// on which to place the next items.
		/// </param>
		private void FillStack(Node[] stack, ref int index, Node current)
		{
			while (current != null)
			{
				stack[index++] = current;
				current = current.Left;
			}
		}	// End -FillStack(stack: Node[], index: in-out int, current: Node)

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
		}	// End -GetParentNode(value: T) : Node

		/// <summary>
		/// Starting from the given Node, this method will move up the tree. As it moves
		/// up the tree, it will rotate tree nodes to make sure the tree is balanced within
		/// the tolerance allowed in an AVL Tree.
		/// </summary>
		/// <param name="startingPoint">
		/// The node were we start at when we re-balance the tree. This is usually locationed
		/// at the location where the tree became unbalanced.
		/// </param>
		private void RebalanceTree(Node startingPoint)
		{
			if (startingPoint == null)
				return;

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
					if (current.Left.Balance < 0)
						RotateLeft(current.Left);
					current = RotateRight(current);
				}
				current = current.Parent;
			}

			// TODO: There's an edge case I need to look into before I know that this won't
			//		 be needed.
			_root.UpdateHeight();
		}	// End -RebalanceTree(startingPoint: Node)

		/// <summary>
		/// This method will take the given node and rotate it (counter-clockwise)
		/// to the position of its left child. Since this is a rotation, the node's
		/// right child will be moved to take the position that the given node held.
		/// </summary>
		/// <param name="node">
		/// The node to be rotated out of its current parent position in the tree.
		/// </param>
		/// <returns>
		/// The node that replaced the given node's tree position. Its right child.
		/// </returns>
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
		}	// End -RotateLeft(node: Node): Node

		/// <summary>
		/// This method will take the given node and rotate it (clockwise) to the 
		/// position of its left child. Since this is a rotation, the node's right 
		/// child will be moved to take the position that the given node held.
		/// </summary>
		/// <param name="node">
		/// The node to be rotated out of its current parent position in the tree.
		/// </param>
		/// <returns>
		/// The node that replaced the given node's tree position. Its left child.
		/// </returns>
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
		}	// End -RotateLeft(node: Node): Node

		/// <summary>
		/// Recursive method for clearning the tree using a pre-ordered DFS.
		/// </summary>
		private void Clear(Node n)
		{
			if (n == null)
				return;

			Clear(n.Left);
			Clear(n.Right);

			n.Left   = null;
			n.Right  = null;
			n.Parent = null;
		}	// End -Clear(n: Node)
		#endregion


		#region Helper Classes
		private class Node
		{
			#region Properties
			public Node Parent	{ get; set; }
			public Node Left	{ get; set; }
			public Node Right	{ get; set; }

			public T Data		{ get; set; }

			public int Height	{ get; private set; }
			public int Balance
			{
				get
				{
					int leftHeight  = Left  == null ? 0 : Left.Height;
					int rightHeight = Right == null ? 0 : Right.Height;

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
				int leftHeight  = Left  != null ? Left.Height  : 0;
				int rightHeight = Right != null ? Right.Height : 0;

				Height = 1 + Math.Max(leftHeight, rightHeight);
			}
			#endregion
		}
		#endregion
	}
}
