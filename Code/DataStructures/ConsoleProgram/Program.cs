
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Unvi.Algorithms.Heaps;
using Unvi.DataStructures.Dictionaries;


namespace ConsoleProgram
{
	class Program
	{
		static void Main(string[] args)
		{
			var dictionary = new AVLTreeDictionary<int, char>();

			for (int i = 0; i < 10000; i++)
				dictionary.Add(i, 'a');

			var current = 'b';
			DateTime start;
			DateTime end;

			start = DateTime.Now;
			for (int i = 0; i < 1000; i++)
				foreach (var p in dictionary)
					current = p.Value;
			end = DateTime.Now;

			var time = end - start;
		}
	}
}
