using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unvi.DataStructures
{
	public interface ISet<T>
		: IEnumerable<T>
	{
		/// <summary>
		/// Returns the number of items in the set (a.k.a. the set's cardinality).
		/// </summary>
		int Count { get; }

		/// <summary>
		/// Adds the given value to the set if it isn't already present.
		/// </summary>
		void Add(T value);
		/// <summary>
		/// Removes the given value from the set if it is present.
		/// </summary>
		void Remove(T value);
		/// <summary>
		/// Tells us if the given value is present in the set.
		/// </summary>
		bool Contains(T value);
		/// <summary>
		/// Clears out all values from the set.
		/// </summary>
		void Clear();

		/// <summary>
		/// Tells us if the current set is a subset of the other set.
		/// </summary>
		bool IsSubsetOf(ISet<T> otherSet);
		/// <summary>
		/// Tells us if the current set is a superset of the other set.
		/// </summary>
		bool IsSupersetOf(ISet<T> otherSet);

		/// <summary>
		/// Tells us if the current set is a proper subset of the other set.
		/// </summary>
		/// <remarks>
		/// A proper subset is a subset that is not equal to the other set.
		/// </remarks>
		bool IsProperSubsetOf(ISet<T> otherSet);
		/// <summary>
		/// Tells us if the current set is a proper superset of the other set.
		/// </summary>
		/// <remarks>
		/// A proper super set is a superset that is not equal to the other set.
		/// </remarks>
		bool IsProperSupersetOf(ISet<T> otherSet);

		/// <summary>
		/// Returns a new set containing the union of this set and the other set.
		/// </summary>
		ISet<T> Union(ISet<T> otherSet);
		/// <summary>
		/// Returns a new set containing the intersection of this set and the other set.
		/// </summary>
		ISet<T> Instersection(ISet<T> otherSet);
		/// <summary>
		/// Returns a new set containing the complement of the other set in this set. In other words,
		/// it returns this set minus the other set.
		/// </summary>
		ISet<T> Complement(ISet<T> otherSet);

		/// <summary>
		/// Returns a new set containing the symmetric difference of this set and the other
		/// set. This is the complement of the intersection of both sets in the union of 
		/// both sets.  (this set &222a; other set) - (this set &2229; other set)
		/// </summary>
		ISet<T> SymmetricDifference(ISet<T> otherSet);
	}
}
