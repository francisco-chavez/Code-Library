﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Unvi.DataStructures.Sets;


namespace Unvi.DataStructures.Dictionaries
{
	public class AVLTreeDictionary<TKey, TValue>
		: IDictionary<TKey, TValue> where TKey : IComparable<TKey>
	{
		#region Attributes
		private Node	_root;
		#endregion


		#region Properties

		public bool IsReadOnly { get { return false; } }

		public int Count { get; private set; }

		/// <summary>
		/// Gets or sets the element with the specified key.
		/// </summary>
		/// <param name="key">The key of the element to get or set.</param>
		/// <returns>The element with the specified key.</returns>
		public TValue this[TKey key]
		{
			get
			{
				if (key == null)
					throw new ArgumentNullException();

				if (!ContainsKey(key))
					throw new KeyNotFoundException();

				var parent = GetParent(key);
				if (parent == null)
					return _root.Value;

				return key.CompareTo(parent.Key) < 0 ? parent.Left.Value : parent.Right.Value;
			}	// End get
			set
			{
				if (IsReadOnly)
					throw new NotSupportedException("Dictionary is read-only.");

				if (key == null)
					throw new ArgumentNullException("key");

				if (!ContainsKey(key))
					throw new KeyNotFoundException();

				var parent = GetParent(key);
				Node n = _root;

				if (parent != null)
					n = key.CompareTo(parent.Key) < 0 ? parent.Left : parent.Right;
				n.Value = value;
			}	// End set
		}	// End [] property.

		/// <summary>
		/// Gets an ICollection&lt;TKey&gt; containing the keys of the
		/// AVLTreeDictionary&lt;TKey, TValue&gt;.
		/// </summary>
		public KeyCollection Keys { get; private set; }

		ICollection<TKey> IDictionary<TKey, TValue>.Keys { get { return this.Keys; } }

		/// <summary>
		/// Gets a ValueCollection containing the values in the dictionary.
		/// </summary>
		public ValueCollection Values { get; private set; }
		ICollection<TValue> IDictionary<TKey, TValue>.Values { get { return this.Values; } }

		#endregion


		#region Constructors
		public AVLTreeDictionary()
		{
			_root	= null;
			Count	= 0;

			Keys	= new KeyCollection(this);
			Values	= new ValueCollection(this);
		}

		~AVLTreeDictionary()
		{
			Clear();

			Keys.ClearOut();
			Values.ClearOut();
			
			Keys	= null;
			Values	= null;
		}
		#endregion


		#region Public Methods

		/// <summary>
		/// Adds an element with the provided key and value to the AVLTreeDictionary&lt;Tkey, TValue&gt;.
		/// </summary>
		/// <param name="key">The object to use as the key of the element to add.</param>
		/// <param name="value">The object to use as the value of the element to add.</param>
		public void Add(TKey key, TValue value)
		{
			if (IsReadOnly)
				throw new NotSupportedException("IDictionary is in read-only mode.");

			if (key == null)
				throw new ArgumentNullException("Key is null.");

			if (ContainsKey(key))
				throw new ArgumentException("An element with the same key already exists in the IDictionary.");

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
		}	// End +Add(key: TKey, value: TValue)

		/// <summary>
		/// Adds an element with the provided key and value to the AVLTreeDictionary&lt;Tkey, TValue&gt;.
		/// </summary>
		public void Add(KeyValuePair<TKey, TValue> item)
		{
			Add(item.Key, item.Value);
		}

		/// <summary>
		/// Removes the element with the spedified key from the AVLTreeDictionary&lt;TKey, TValue&gt;.
		/// </summary>
		/// <param name="key">The key of the element to remove.</param>
		/// <returns>
		/// True if the element is successfully removed; otherwise, false. This method also returns false
		/// if the key was not found.
		/// </returns>
		public bool Remove(TKey key)
		{
			if (key == null)
				throw new ArgumentNullException();

			if (IsReadOnly)
				throw new NotSupportedException("The IDictionary<TKey> is read-only.");

			// Key not found.
			if (!ContainsKey(key))
				return false;

			// Edge Case: It's the only item.
			if (Count == 1)
			{
				_root = null;
				Count = 0;
				return true;
			}

			Node rebalancePoint = RemoveNode(key);
			RebalanceTree(rebalancePoint);
			return true;
		}	// End +Remove(key: TKey): bool

		/// <summary>
		/// Removes the first (and only) occurrence of the item. It a different value
		/// is held under this key, nothing will be removed.
		/// </summary>
		public bool Remove(KeyValuePair<TKey, TValue> item)
		{
			if (!this.ContainsKey(item.Key))
				return false;

			var value = this[item.Key];
			if (!item.Value.Equals(value))
				return false;

			return this.Remove(item.Key);
		}	// End +Remove(item: KeyValuePair<TKey, TValue>): bool

		/// <summary>
		/// Clears out all entries in the AVLTreeDictionary&lt;Tkey, TValue&gt;.
		/// </summary>
		public void Clear()
		{
			Clear(_root);
			_root = null;
			Count = 0;
		}	// End +Clear()


		/// <summary>
		/// Determines whether the AVLTreeDictionary&lt;TKey, TValue&gt;
		/// contains an element with the specified key.
		/// </summary>
		/// <param name="key">The key to locate in the AVLTreeDictionary&lt;TKey, TValue&gt;.</param>
		/// <returns>
		/// True if the AVLTreeDictionary&lt;TKey, TValue&gt; contains an element with the key;
		/// otherwise, False.
		/// </returns>
		public bool ContainsKey(TKey key)
		{
			if (key == null)
				throw new ArgumentNullException("key", "key is null.");

			if (Count == 0)
				return false;

			var parent = GetParent(key);

			if (parent == null)
				return key.CompareTo(_root.Key) == 0;

			var node = key.CompareTo(parent.Key) < 0 ? parent.Left : parent.Right;

			return node != null;
		}	// End +ContainsKey(key: TKey): bool
	
		/// <summary>
		/// This method will tell us if there is an entry with this key-value pairing in
		/// the dictionary.
		/// </summary>
		public bool Contains(KeyValuePair<TKey, TValue> item)
		{
			if (!ContainsKey(item.Key))
				return false;

			return EqualityComparer<TValue>.Default.Equals(item.Value, this[item.Key]);
		}	// End +Contains(item, KeyValuePair<TKey, TValue>): bool


		/// <summary>
		/// Gets the value associated with the specified key.
		/// </summary>
		/// <param name="key">The key whose value to get.</param>
		/// <param name="value">
		/// When this method returns, the value associated with the specified key, if
		/// the key is found; otherwise, the default value for the type of the value 
		/// parameter. This parameter is passed uninitiaized.
		/// </param>
		/// <returns>
		/// True if the object that implements IDictionary&lt;TKey, TValue&gt;
		/// contsin an element with the specified key; otherwise, false.
		/// </returns>
		public bool TryGetValue(TKey key, out TValue value)
		{
			if (key == null)
				throw new ArgumentNullException("key");

			if (!ContainsKey(key))
			{
				value = default(TValue);
				return false;
			}

			value = this[key];
			return true;
		}	// End +TryGetValue(key: TKey, out value: TValue): bool

		public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
		{
			if (_root == null)
				yield break;

			var stack = new Node[_root.Height];
			int loc = 0;
			var current = _root;

			FillStack(stack, ref loc, current);

			while (loc > 0)
			{
				current = stack[--loc];
				yield return new KeyValuePair<TKey, TValue>(current.Key, current.Value);

				current = current.Right;
				FillStack(stack, ref loc, current);
			}
		}	// End +GetEnumerator(): IEnumerator<KeyValuePair<TKey, TValue>>

		/// <summary>
		/// Copies the elements of the IDictionary to an Array, starting at a particular index.
		/// </summary>
		/// <param name="array">
		/// The one-dimensional Array that is the destination of the elements
		/// copied from the IDictionary. The Array must have zero-based indexing.
		/// </param>
		/// <param name="arrayIndex">
		/// The zero-based index in array at which copying begins.
		/// </param>
		public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
		{
			if (array == null)
				throw new ArgumentNullException("array");

			if (arrayIndex < 0)
				throw new ArgumentOutOfRangeException("arrayIndex");

			if (array.Length - arrayIndex < this.Count)
				throw new ArgumentException("The number of elements in the Dictionary is greater than the available space in the array.");

			using (var ittor = this.GetEnumerator())
			{
				for (int i = arrayIndex; ittor.MoveNext(); i++)
					array[i] = ittor.Current;
			}
		}	// End +CopyTo(array: KeyValuePair<Tkey, TValue>[], arrayIndex: int)

		#endregion


		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}


		#region Helper Methods

		private void FillStack(Node[] stack, ref int index, Node current)
		{
			while (current != null)
			{
				stack[index++] = current;
				current = current.Left;
			}
		}

		/// <summary>
		/// Returns the parent node of the node that would contain the given 
		/// key. Only the root node had no parent. If the node is not in the 
		/// tree, then the given parent is the location where the node should 
		/// be inserted.
		/// </summary>
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
		}	// End -GetParent(key: TKey): Node
		
		/// <summary>
		/// Using the given 'node' as a starting point, this method will move 
		/// up the tree re-balancing it based on an AVL node rotation algorithm.
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
		}	// End -RebalanceTree(node: Node)

		/// <summary>
		/// Rotates the given node down into its left child, while rotating the 
		/// right child of the given node into its current position in the tree. 
		/// This method requires that the given node contains a right child in 
		/// order to function.
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
		}	// End -RotateLeft(node: Node)

		/// <summary>
		/// This node will be rotated down into the position of its right child 
		/// while its left child is rotated into its current position. A left 
		/// child is required for the method to function.
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
		}	// End -RotateRight(node: Node)

		/// <summary>
		/// This method will remove the node with the given key from the tree. It will then
		/// return the node that is located at the spot of the tree where the tree needs to
		/// be re-balanced. It will also keep the tree's node count up to date.
		/// </summary>
		private Node RemoveNode(TKey key)
		{
			// ToDo: Extract finding the replacement node into its own method.
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

			// Special case. The replacement node should be the rightmost node
			// of the left sub-tree of the node that will be removed from the
			// tree. If we don't have a replacement node on the left subtree,
			// we will use the first node on the right subtree.
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

				return replacement;
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

				// The replacement was the right most child of the sub-tree, so we
				// know that it resides on the right side of its parent.
				replacement.Right = replacement.Left;
				if (replacement.Right != null)
					rebalancePoint.Right.Parent = replacement;

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

			node.Left = null;
			node.Right = null;
			node.Parent = null;

			Count--;
			return rebalancePoint;
		}	// End -RemoveNode(key: TKey): Node

		/// <summary>
		/// A recursive DFS post-order algorithm for clearing the tree.
		/// </summary>
		private void Clear(Node node)
		{
			if (node == null)
				return;

			Clear(node.Left);
			Clear(node.Right);

			node.Left	= null;
			node.Right	= null;
			node.Parent = null;
		}	// End -Clear(node: Node)
		
		#endregion


		#region Helper Classes
		public sealed class KeyCollection
			: ICollection<TKey>
		{
			#region Attributes
			private AVLTreeDictionary<TKey, TValue> _dictionary;
			#endregion


			#region Properties
			public int Count
			{
				get { return _dictionary.Count; }
			}

			public bool IsReadOnly
			{
				get { return true; }
			}
			#endregion


			#region Constructors
			internal KeyCollection(AVLTreeDictionary<TKey, TValue> dictionary)
			{
				_dictionary = dictionary;
			}

			~KeyCollection()
			{
				ClearOut();
			}
			#endregion


			#region Public Methods

			/// <summary>
			/// Copies the keys contained in the collection into the given array.
			/// </summary>
			/// <param name="array">The array to copy the keys into.</param>
			/// <param name="arrayIndex">
			/// The index (of the given array) to start placing the keys into. This have a
			/// default value of 0.
			/// </param>
			public void CopyTo(TKey[] array, int arrayIndex = 0)
			{
				if (array == null)
					throw new ArgumentNullException("array");
				if (arrayIndex < 0)
					throw new IndexOutOfRangeException("arrayIndex is out of bounds.");
				if (array.Length - arrayIndex < Count)
					throw new ArgumentException("Not enough space in given array.");

				using (var ittor = _dictionary.GetEnumerator())
				{
					for (int i = arrayIndex; ittor.MoveNext(); i++)
						array[i] = ittor.Current.Key;
				}
			}	// End +CopyTo(array: TKey[], arrayIndex: int)

			public IEnumerator<TKey> GetEnumerator()
			{
				foreach (var entry in _dictionary)
					yield return entry.Key;
			}	// End +GetEnumerator(): IEnuerator<TKey>

			#endregion


			#region Explicite Method Implementations
			void ICollection<TKey>.Add(TKey item)
			{
				throw new NotSupportedException();
			}	// End Add()

			bool ICollection<TKey>.Remove(TKey item)
			{
				throw new NotSupportedException();
			}	// End Remove()

			void ICollection<TKey>.Clear()
			{
				throw new NotSupportedException();
			}	// End Clear()


			bool ICollection<TKey>.Contains(TKey item)
			{
				return _dictionary.ContainsKey(item);
			}	// End Contains()

			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.GetEnumerator();
			}	// End GetEnumerator()
			#endregion

			#region Internal Methods
			/// <summary>
			/// If I could, I would turn this into a  friend method for the AVLTreeDictionary,
			/// but C# doesn't do friends. Internal is the best I can do; this way, anyone
			/// using the library won't be able to see this method.
			/// </summary>
			internal void ClearOut()
			{
				_dictionary = null;
			}
			#endregion
		}

		public class ValueCollection
			: ICollection<TValue>
		{
			#region Attributes
			private AVLTreeDictionary<TKey, TValue> _dictionary;
			#endregion


			#region Properties
			public int Count
			{
				get { return _dictionary.Count; }
			}

			public bool IsReadOnly
			{
				get { return true; }
			}
			#endregion


			#region Constructors
			internal ValueCollection(AVLTreeDictionary<TKey, TValue> dictionary)
			{
				_dictionary = dictionary;
			}

			~ValueCollection()
			{
				ClearOut();
			}
			#endregion


			#region Public Methods

			/// <summary>
			/// Copies the values contained in the collection into the given array.
			/// </summary>
			/// <param name="array">The array to copy the keys into.</param>
			/// <param name="arrayIndex">
			/// The index (of the given array) to start placing the keys into. This have a
			/// default value of 0.
			/// </param>
			public void CopyTo(TValue[] array, int arrayIndex = 0)
			{
				if (array == null)
					throw new ArgumentNullException("array");
				if (arrayIndex < 0)
					throw new IndexOutOfRangeException("arrayIndex is out of bounds.");
				if (array.Length - arrayIndex < Count)
					throw new ArgumentException("Not enough space in given array.");

				using (var ittor = _dictionary.GetEnumerator())
				{
					for (int i = arrayIndex; ittor.MoveNext(); i++)
						array[i] = ittor.Current.Value;
				}
			}	// End +CopyTo(array: TKey[], arrayIndex: int)

			public IEnumerator<TValue> GetEnumerator()
			{
				foreach (var entry in _dictionary)
					yield return entry.Value;
			}	// End +GetEnumerator(): IEnuerator<TKey>

			#endregion


			#region Explicite Method Implementations

			void ICollection<TValue>.Add(TValue item)
			{
				throw new NotSupportedException();
			}	// End Add()

			bool ICollection<TValue>.Remove(TValue item)
			{
				throw new NotSupportedException();
			}	// End Remove()

			void ICollection<TValue>.Clear()
			{
				throw new NotSupportedException();
			}	// End Clear()

			/// <summary>
			/// This is a O(n) time operation where n == this.Count. If any of the values in the
			/// dictionary are equal to the given item, this will return true.
			/// </summary>
			bool ICollection<TValue>.Contains(TValue item)
			{
				// This looks a bit long, but it's actually quite short. Normally, we could do
				// an == or .Equals(), but that's not really an option in this case. We can't do
				// an == because we're using generics. And, we can't wall .Equals() because a
				// value could be null. We could do our own null value checks, but all those if
				// statments would take up additional space. The EqualityComaprer does this for
				// us while running the .Equals() (or something similar) when there's a value
				// for both paramaters. It takes care of all those edge cases with a single 
				// method call. After that we just use Linq to run this on every value. We could
				// do this in our own loop, but Linq works just as well.
				// -FCT
				return this.Any(val => { return EqualityComparer<TValue>.Default.Equals(val, item); });
			}	// End Contains()

			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.GetEnumerator();
			}	// End GetEnumerator()
			#endregion

			#region Internal Methods
			/// <summary>
			/// If I could, I would turn this into a  friend method for the AVLTreeDictionary,
			/// but C# doesn't do friends. Internal is the best I can do; this way, anyone
			/// using the library won't be able to see this method.
			/// </summary>
			internal void ClearOut()
			{
				_dictionary = null;
			}
			#endregion
		}

		private class Node
		{
			#region Properties
			public Node		Parent	{ get; set; }
			public Node		Left	{ get; set; }
			public Node		Right	{ get; set; }

			public TKey		Key		{ get; private set; }
			public TValue	Value	{ get; set; }

			public int Height { get; private set; }
			public int Balance
			{
				get
				{
					int left  = Left  != null ? Left.Height  : 0;
					int right = Right != null ? Right.Height : 0;

					return left - right;
				}
			}
			#endregion


			#region Constructors
			public Node(TKey key, TValue value)
			{
				Key = key;
				Value = value;
				Height = 1;

				Parent = null;
				Left = null;
				Right = null;
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
