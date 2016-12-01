using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RS2Sharp
{
	public class Random
	{
		private static System.Random rnd = new System.Random();
		public static int Next()
		{
			return rnd.Next();
		}

		public static int Next(int max)
		{
			return rnd.Next(max);
		}

		public static double NextDouble()
		{
			return rnd.NextDouble();
		}
	}
}
