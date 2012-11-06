using System;
using DaisyML;

namespace DaisyML.Classifiers.Tests
{
	public class IrisInstance : Instance
	{
		[Feature]
		public double SepalLength;

		[Feature]
		public double SepalWidth;

		[Feature]
		public double PetalLength;

		[Feature]
		public double PetalWidth;

		[Target]
		public string Class;
	}
}

