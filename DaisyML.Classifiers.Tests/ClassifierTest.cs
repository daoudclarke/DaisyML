using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;
using DaisyML;
using DaisyML.Classifiers;
using DaisyML.Utils;
using DaisyML.TestData;

namespace DaisyML.Classifiers.Tests
{
	[TestFixture]
	public class ClassifierTest
	{
		[Test]
		public void TestClassifiers() {
			// Arrange
			var classifier = new NaiveBayesClassifier();
			var data = Data.GetTestInstances().First().Shuffle();
			var trainingSetSize = data.Count() / 2;
			var trainingData = data.Take(trainingSetSize);
			var testData = data.Skip(trainingSetSize).ToArray();
			foreach (var instance in testData) {
				instance.SetTarget("class", "Iris-setosa");
			}

			Assert.AreEqual(1, testData.Select(x => x.Targets.First().Value)
			              .Distinct().Count(),
			              "Should only be one classification value.");
			
			// Act
			var model = classifier.Train(trainingData);
			model.Classify(testData);
			
			var allowedClasses = new HashSet<string>(
			   trainingData.Select(x => (string)x.Targets.First().Value));
			
			// Assert
			Assert.IsTrue(allowedClasses.Count > 0,
			              "No class values found");
			
			Assert.IsTrue(testData.Select(x => x.Targets.First().Value).Distinct().Count() > 1,
			              "Should be more than one classification value.");
			for (int i=0; i<testData.Length; ++i) {
				Assert.AreNotEqual(null, testData[i].Targets.First().Value,
				              "Result class should not be null.");
				Assert.IsTrue(allowedClasses.Contains((string)testData[i].Targets.First().Value),
				              "Result should be in allowed class.");
			}
		}
	}
}

