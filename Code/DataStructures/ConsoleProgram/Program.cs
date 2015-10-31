
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

			dictionary.Add(1, 'a');
			dictionary.Add(2, 'b');
			dictionary.Add(3, 'c');
			dictionary.Add(4, 'd');
			dictionary.Add(5, 'e');
			dictionary.Add(6, 'f');
			dictionary.Add(7, 'g');
			dictionary.Add(8, 'h');

			foreach (var p in dictionary)
				Console.WriteLine(p.Value);
		}
	}
}
