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
		
		[Feature]
		public double? nullable;
		
		[Feature]
		public double? notNull;
						
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
				nullable = null,
				notNull = 3.0,
				target = 2.0 };
			Assert.AreEqual(1, instance.NumericTargets.Count());
			Assert.AreEqual(0, instance.NominalTargets.Count());
			Assert.AreEqual(2, instance.NumericFeatures.Count());
			Assert.AreEqual (1, instance.MissingFeatures.Count ());
			Assert.AreEqual(1, instance.NominalFeatures.Count ());
			
			
			var converted = WekaInstanceUtils.ConvertToWeka(new [] {instance});
			
			
			var expected = @"@relation DaisyML.Weka.Tests.TestInstance

@attribute feature numeric
@attribute fruit {value__,apple,orange,banana}
@attribute notNull numeric
@attribute nullable numeric
@attribute target numeric

@data
1,orange,3,?,2";
			var data = converted.ToString();
			Console.WriteLine(data);
			Assert.AreEqual(expected, data, "Conversion to Weka format incorrect."); 
		}
		
//		public void CastTest() {
//			IEnumerable<int?> ints = new int?[] {1,2,3};
//			IEnumerable<object> objects = ints;
//		}
		
		[Test]
		public void TestConvertFromAndToWeka()
		{
			// Arrange
			var data = TestData.Data.GetTestInstances();
			foreach (var instances in data) {
				Assert.Less(instances.First().NumericTargets.Count()
					+ instances.First().NominalTargets.Count(), 2);
				var wekaInstances = (WekaInstances)instances;
				var originalArff = wekaInstances.GetArff();
				var converted = WekaInstanceUtils.ConvertToWeka(wekaInstances);
				
				// Act
				var convertedArff = converted.toString();
				
				// Assert
				Assert.AreEqual(originalArff, convertedArff,
				                "Conversion does not give the same ARFF.");
			}
		}
	}
}

