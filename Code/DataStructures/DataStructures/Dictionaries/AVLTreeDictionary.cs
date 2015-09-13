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
				throw new NotImplementedException();
			}

			set
			{
				throw new NotImplementedException();
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
			throw new NotImplementedException();
		}

		public void Remove(TKey key)
		{
			throw new NotImplementedException();
		}

		public void Clear()
		{
			throw new NotImplementedException();
		}

		public bool ContainsKey(TKey key)
		{
			throw new NotImplementedException();
		}


		public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
		{
			throw new NotImplementedException();
		}
		#endregion


		#region Helper Classes
		private void Clear(Node n)
		{
			throw new NotImplementedException();
		}

		private Node GetParent(TKey key)
		{
			throw new NotImplementedException();
		}

		private void ReblanceTree(Node node)
		{
			throw new NotImplementedException();
		}

		private Node RotateLeft(Node node)
		{
			throw new NotImplementedException();
		}

		private Node RotateRight(Node node)
		{
			throw new NotImplementedException();
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
