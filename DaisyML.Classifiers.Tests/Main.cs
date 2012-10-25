using System;
using System.Reflection;

using NUnit.ConsoleRunner;

namespace DaisyML.Classifiers.Tests
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			NUnit.ConsoleRunner.Runner.Main (new string[] {
				Assembly.GetExecutingAssembly().Location });
		}
	}
}


