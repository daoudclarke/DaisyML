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
			var values = instance.OrderBy(x => x.Key).ToArray();

			Assert.AreEqual(3, values.Length);
			Assert.AreEqual("Length", values[0].Key);
			Assert.AreEqual("Name", values[1].Key);
			Assert.AreEqual("Size", values[2].Key);

			Assert.AreEqual(2.3, values[0].Value);
			Assert.AreEqual("test", values[1].Value);
			Assert.AreEqual(1, values[2].Value);
		}
	}
}

