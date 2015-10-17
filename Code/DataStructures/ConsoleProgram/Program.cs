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
			int testItemCount = 20;
			List<int> sourceData1 = new List<int>(testItemCount);
			Random r = new Random(5);
			for (int i = 0; i < testItemCount; i++)
				sourceData1.Add(r.Next(-5000, 5000));

			int testCount = 3000;

			DateTime t1Start;
			DateTime t1End;
			DateTime t2Start;
			DateTime t2End;

			var dump = sourceData1.ToArray();
			dump.HeapSort();
		}
	}
}
