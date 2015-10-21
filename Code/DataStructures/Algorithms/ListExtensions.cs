using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unvi.Algorithms
{
	public static class ListExtensions
	{
		public static void SwapValues<T>(this List<T> data, int indexA, int indexB)
		{
			if (data == null)
				throw new ArgumentNullException("There is no array to work with.");

			if (indexA < 0 || data.Count <= indexA)
				throw new ArgumentOutOfRangeException("indexA");

			if (indexB < 0 || data.Count <= indexB)
				throw new ArgumentOutOfRangeException("indexB");

			if (indexA == indexB)
				return;

			T datum = data[indexA];
			data[indexA] = data[indexB];
			data[indexB] = datum;
		}

	}
}
