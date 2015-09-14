
using System.Collections.Generic;


namespace Unvi.DataStructures
{
	public interface IDictionary<TKey, TValue>
		: IEnumerable<KeyValuePair<TKey, TValue>>
	{
		/// <summary>
		/// Gets or sets the entry under the given key.
		/// </summary>
		TValue this[TKey key] { get; set; }
		/// <summary>
		/// Gets the current number of entries.
		/// </summary>
		int Count { get; }

		/// <summary>
		/// Adds given value as a new entry under the given key.
		/// </summary>
		void Add(TKey key, TValue value);
		/// <summary>
		/// Removes the entry under the given key.
		/// </summary>
		void Remove(TKey key);
		/// <summary>
		/// Checks to see if there's an entry under the given key.
		/// </summary>
		bool ContainsKey(TKey key);
		/// <summary>
		/// Removes all entries.
		/// </summary>
		void Clear();
	}
}
