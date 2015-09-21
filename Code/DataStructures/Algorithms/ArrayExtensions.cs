using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Unvi.Algorithms
{
	public static class ArrayExtensions
	{
		public static void SwapValues<T>(this T[] data, int indexA, int indexB)
		{
			if (data == null)
				throw new ArgumentNullException("There is no array to work with.");

			if (indexA < 0 || data.Length <= indexA)
				throw new ArgumentOutOfRangeException("indexA");

			if (indexB < 0 || data.Length <= indexB)
				throw new ArgumentOutOfRangeException("indexB");

			if (indexA == indexB)
				return;

			T datum = data[indexA];
			data[indexA] = data[indexB];
			data[indexB] = datum;
		}
	}
}
