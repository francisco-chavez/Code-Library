using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Unvi.DataStructures.Dictionaries
{
	public class AVLTreeDictionary<TKey, TValue>
		: IDictionary<TKey, TValue> where TKey : IComparable
	{
		#region Attributes
		private Node _root;
		#endregion


		#region Properties
		public int Count { get; private set; }

		public TValue this[TKey key]
		{
			get
			{
				if (!ContainsKey(key))
					throw new IndexOutOfRangeException("No entry was found under the given key.");

				var parent = GetParent(key);
				if (parent == null)
					return _root.Value;

				return key.CompareTo(parent.Key) < 0 ? parent.Left.Value : parent.Right.Value;
			}
			set
			{
				if (!ContainsKey(key))
					throw new IndexOutOfRangeException("No entry was found under the given key.");

				var parent = GetParent(key);
				if (parent == null)
					_root.Value = value;

				Node n = key.CompareTo(parent.Key) < 0 ? parent.Left : parent.Right;
				n.Value = value;
			}
		}
		#endregion


		#region Constructors
		public AVLTreeDictionary()
		{
			_root = null;
			Count = 0;
		}

		~AVLTreeDictionary()
		{
			Clear();
		}
		#endregion


		#region Public Methods
		public void Add(TKey key, TValue value)
		{
			if(key == null)
				throw new ArgumentNullException("key");
			if (ContainsKey(key))
				throw new ArgumentException(string.Format("Key \"{0}\" is already contained in {1}.", key, this.GetType()));

			// Base case, the tree is empty, so we don't have to worry
			// about keeping it balanced or searching for a location to 
			// insert the new node.
			if (Count == 0)
			{
				_root = new Node(key, value);
				Count = 1;
				return;
			}

			// Find a node to use as the parent for the new node that
			// will be added to the tree.
			var parent = GetParent(key);
			var node = new Node(key, value);

			if (key.CompareTo(parent.Key) < 0)
				parent.Left = node;
			else
				parent.Right = node;
			node.Parent = parent;

			Count++;
			RebalanceTree(node);
		}

		public void Remove(TKey key)
		{
			if(key == null)
				throw new ArgumentNullException("key");

			if (Count == 0 || !ContainsKey(key))
				throw new ArgumentOutOfRangeException("key", "There is no entry for the given key value in the dictionary.");

			// There's no need for anything complicated if we only 
			// have the one node in the tree.
			if (Count == 1)
			{
				_root = null;
				Count = 0;
				return;
			}

			Node parent;
			Node node;
			Node replacement;

			parent = GetParent(key);
			if (parent == null)
				node = _root;
			else if (key.CompareTo(parent.Key) < 0)
				node = parent.Left;
			else
				node = parent.Right;

			// Special case. The replacement node should be the rightmost
			// node of the left sub-tree of the node that will be removed
			// from the tree. If we don't have a replacement node on the
			// left substree, we will use the first node on the right 
			// subtree.
			if (node.Left == null)
			{
				replacement = node.Right;

				if (parent == null)
					_root = replacement;
				else if (key.CompareTo(parent.Key) < 0)
					parent.Left = replacement;
				else
					parent.Right = replacement;

				if (replacement != null)
					replacement.Parent = parent;

				node.Parent = null;
				node.Right = null;
				Count--;

				// This should really be replacement.Parent but the replacment could
				// be null and calling .Parent on a null node will throw an excpetion.
				// This works just fine, and we'll just end up checking one more level
				// of the tree when we rebalance the thing.
				RebalanceTree(replacement);
				return;
			}

			replacement = node.Left;
			while (replacement.Right != null)
				replacement = replacement.Right;

			Node rebalancePoint = null;

			if (replacement == node.Left)
			{
				rebalancePoint = replacement;
			}
			else
			{
				rebalancePoint = replacement.Parent;
				// The replacment was the right most child of the sub-tree,
				// so we know that it resides on the right side of its parent.
				rebalancePoint.Right = replacement.Left;
				if (rebalancePoint.Right != null)
					rebalancePoint.Right.Parent = rebalancePoint;

				replacement.Left = node.Left;
				replacement.Left.Parent = replacement;
			}

			replacement.Right = node.Right;
			if (replacement.Right != null)
				replacement.Right.Parent = replacement;

			replacement.Parent = parent;
			if (parent == null)
				_root = replacement;
			else if (key.CompareTo(parent.Key) < 0)
				parent.Left = replacement;
			else
				parent.Right = replacement;

			Count--;
			RebalanceTree(rebalancePoint);
			node.Left = null;
			node.Right = null;
			node.Parent = null;
		}

		public void Clear()
		{
			Clear(_root);
			_root = null;
			Count = 0;
		}

		public bool ContainsKey(TKey key)
		{
			if(key == null)
				return false;

			if (Count == 0)
				return false;

			var parent = GetParent(key);

			if (parent == null)
				return _root.Key.CompareTo(key) == 0;

			var node = key.CompareTo(parent.Key) < 0 ? parent.Left : parent.Right;

			return node != null;
		}


		public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
		{
			throw new NotImplementedException();
		}
		#endregion


		#region Helper Classes
		/// <summary>
		/// A recursive DFS post-order algorithm for clearing the tree.
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

		/// <summary>
		/// Returns the parent node of the node that would contain
		/// the given key. Only the root node had no parent. If the
		/// node is not in the tree, then the given parent is the
		/// location where the node should be inserted.
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		private Node GetParent(TKey key)
		{
			if (Count == 0)
				return null;
			if (key.CompareTo(_root.Key) == 0)
				return null;


			Node parent = null;
			Node current = _root;

			while (current != null)
			{
				int order = key.CompareTo(current.Key);
				if (order == 0)
					break;

				parent = current;
				current = order < 0 ? current.Left : current.Right;
			}

			return parent;
		}

		/// <summary>
		/// Using the given 'node' as a starting point, this method
		/// will move up the tree re-balancing it based on an AVL
		/// node rotation algorithm.
		/// </summary>
		private void RebalanceTree(Node node)
		{
			if (node == null)
				return;

			while (node != null)
			{
				node.UpdateHeight();
				int balance = node.Balance;

				if (balance < -1)       // if right heavy
				{
					if (node.Right.Balance > 0)
						RotateRight(node.Right);

					node = RotateLeft(node);
				}
				else if (balance > 1)   // if left heavy
				{
					if (node.Left.Balance < 0)
						RotateLeft(node.Left);

					node = RotateRight(node);
				}

				node = node.Parent;
			}
		}

		/// <summary>
		/// Rotates the given node down into its left child, while rotating
		/// the right child of the given node into its current position in
		/// the tree. This method requires that the given node contains a right
		/// child in order to function.
		/// </summary>
		/// <returns>
		/// Returns the node that has taken the tree position of the given node.
		/// </returns>
		private Node RotateLeft(Node node)
		{
			var replacement = node.Right;

			node.Right = replacement.Left;
			if (replacement.Left != null)
				replacement.Left.Parent = node;

			replacement.Parent = node.Parent;
			if (replacement.Parent == null)
				_root = replacement;
			else if (replacement.Key.CompareTo(replacement.Parent.Key) < 0)
				replacement.Parent.Left = replacement;
			else
				replacement.Parent.Right = replacement;

			node.Parent = replacement;
			replacement.Left = node;

			node.UpdateHeight();
			replacement.UpdateHeight();
			// We could update the replacmeent parent height right here, but
			// that's about to get rebalanced or have it's height updated in
			// the next step. And having it rebalanced will also result in it
			// having its height updated.

			return replacement;
		}

		/// <summary>
		/// This node will be rotated down into the position of its right child while
		/// its left child is rotated into its current position. A left child is required
		/// for the method to function.
		/// </summary>
		/// <returns>
		/// Returns the node that has taken the tree position of the given node.
		/// </returns>
		private Node RotateRight(Node node)
		{
			// We only rotate right when left heavy, so there should be
			// a node there.
			var replacement = node.Left;

			node.Left = replacement.Right;
			if (node.Left != null)
				node.Left.Parent = node;

			replacement.Parent = node.Parent;
			if (replacement.Parent == null)
				_root = replacement;
			else if (replacement.Key.CompareTo(replacement.Parent.Key) < 0)
				replacement.Parent.Left = replacement;
			else
				replacement.Parent.Right = replacement;

			replacement.Right = node;
			node.Parent = replacement;

			node.UpdateHeight();
			replacement.UpdateHeight();

			return replacement;
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			throw new NotImplementedException();
		}
		#endregion


		#region Inner Classes
		private class Node
		{
			#region Properties
			public Node		Parent	{ get; set; }
			public Node		Left	{ get; set; }
			public Node		Right	{ get; set; }

			public TKey		Key		{ get; private set; }
			public TValue	Value	{ get; set; }

			public int		Height { get; private set; }
			public int		Balance
			{
				get
				{
					int left = Left != null ? Left.Height : 0;
					int right = Right != null ? Right.Height : 0;

					return left - right;
				}
			}
			#endregion


			#region Constructors
			public Node(TKey key, TValue value)
			{
				Key		= key;
				Value	= value;
				Height	= 1;

				Parent	= null;
				Left	= null;
				Right	= null;
			}
			#endregion


			#region Methods
			public void UpdateHeight()
			{
				int left  = Left  != null ? Left.Height  : 0;
				int right = Right != null ? Right.Height : 0;

				Height = 1 + Math.Max(left, right);
			}
			#endregion
		}
		#endregion
	}
}
