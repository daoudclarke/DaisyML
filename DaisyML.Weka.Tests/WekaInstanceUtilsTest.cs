using System;
using System.Linq;
using System.Collections.Generic;

using NUnit.Framework;

using DaisyML;
using DaisyML.Weka;

namespace DaisyML.Weka.Tests
{
	enum TestEnum {
		apple, orange, banana
	}
	
	class TestInstance : Instance {
		[Feature]
		public int feature;

		[Feature]
		public TestEnum fruit;
		
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
				fruit = TestEnum.orange,
				target = 2.0 };
			Assert.AreEqual(1, instance.Targets.Count());
			Assert.AreEqual(2, instance.Features.Count());
			
			
			var converted = WekaInstanceUtils.ConvertToWeka(new [] {instance});
			
			
			var expected = @"@relation DaisyML.Weka.Tests.TestInstance

@attribute feature numeric
@attribute fruit {value__,apple,orange,banana}
@attribute target numeric

@data
1,orange,2";
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
				var featureValues = new HashSet<double>();
				foreach (var instance in wekaInstances) {
					featureValues.UnionWith(instance.Features.Select(x=>(double)x.Value));
				}
				Assert.Greater(featureValues.Count, 1, 
				   "Should be more than one feature value");
				var originalArff = wekaInstances.GetArff();
				var converted = WekaInstanceUtils.ConvertToWeka(wekaInstances);
				
				var convertedArff = converted.toString();
				
				Assert.AreEqual(originalArff, convertedArff,
				                "Conversion does not give the same ARFF.");
			}
		}
	}
}

