using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Unvi.Algorithms.Sorting;

namespace ConsoleProgram
{
	class Program
	{
		static void Main(string[] args)
		{
			List<int> sourceData1 = new List<int>(new int[] { 30, 100, 437, 3, 0, -2947593, 2984, 3, 2, 1 });
			Random r = new Random(5);
			for(int i = 0; i < 500; i++)
				sourceData1.Add(r.Next(-2000, 2000));

			int testCount = 3000;

			DateTime t1Start;
			DateTime t1End;
			DateTime t2Start;
			DateTime t2End;

			var dump = sourceData1.ToArray();
		}
	}
}
