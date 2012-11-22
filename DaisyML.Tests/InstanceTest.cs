using System;
using System.Linq;

using NUnit.Framework;
using DaisyML;

namespace DaisyML.Tests
{
	class TestInstance : Instance
	{
		[Feature]
		public int Size;
		
		[Feature]
		public string Name;

		[Feature]
		public double Length;
	}
	
	[TestFixture]
	public class InstanceTest
	{
		[Test]
		public void TestGetEnumerator() 
		{
			var instance = new TestInstance() {
				Size = 1,
				Name = "test",
				Length = 2.3
			};
			var stringValues = instance.StringFeatures.OrderBy(x => x.Name).ToArray();
			var numericValues = instance.NumericFeatures.OrderBy(x => x.Name).ToArray();

			Assert.AreEqual(1, stringValues.Length);
			Assert.AreEqual("Length", numericValues[0].Name);
			Assert.AreEqual("Name", stringValues[0].Name);
			Assert.AreEqual("Size", numericValues[1].Name);

			Assert.AreEqual(2.3, numericValues[0].Value);
			Assert.AreEqual("test", stringValues[0].Value);
			Assert.AreEqual(1, numericValues[1].Value);
		}
	}
}

