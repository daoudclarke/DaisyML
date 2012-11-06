using System;
using System.Linq;
using System.Collections.Generic;

using NUnit.Framework;

using DaisyML;
using DaisyML.Weka;

namespace DaisyML.Weka.Tests
{
	class TestInstance : Instance {
		[Feature]
		public int feature;
		
		[Target]
		public double target;
	}
	
	[TestFixture]
	public class WekaInstanceUtilsTest
	{
		[Test]
		public void TestConvertToWeka()
		{
			var instance = new TestInstance() {
				feature = 1,
				target = 2.0 };
			Assert.AreEqual(1, instance.Targets.Count());
			Assert.AreEqual(1, instance.Features.Count());
			
			
			var converted = WekaInstanceUtils.ConvertToWeka(new [] {instance});
			
			
			var expected = @"@relation DaisyML.Weka.Tests.TestInstance

@attribute feature numeric
@attribute target numeric

@data
0,0";
			var data = converted.ToString();
			Console.WriteLine(data);
			Assert.AreEqual(expected, data, "Conversion to Weka format incorrect."); 
		}
		
		[Test]
		public void TestConvertFromAndToWeka()
		{
			var data = TestData.Data.GetTestInstances();
			foreach (var instances in data) {
				Assert.Less(instances.First().Targets.Count(), 2);
				var wekaInstances = (WekaInstances)instances;
				var originalArff = wekaInstances.GetArff();
				var converted = WekaInstanceUtils.ConvertToWeka(wekaInstances);

				var convertedArff = converted.toString();
				
				Assert.AreEqual(originalArff, convertedArff,
				                "Conversion does not give the same ARFF.");
			}
		}
	}
}

