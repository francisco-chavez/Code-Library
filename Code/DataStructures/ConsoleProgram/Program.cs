
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Unvi.DataStructures.Sets;


namespace ConsoleProgram
{
	class Program
	{
		static void Main(string[] args)
		{
			var dictionary = new Dictionary<int, int>();

			for (int i = 0; i < 5; i++)
				dictionary.Add(i, i);

			// Needed to see what would happen if I tried to make a change
			// to the key collection from the collection itself.
			ICollection<int> keys = dictionary.Keys;
			keys.Add(8);

			//var keys = dictionary.Keys;

			//dictionary.Add(10, 10);
		}
	}
}
