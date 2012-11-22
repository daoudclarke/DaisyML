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
//			var initialTarget = testData.First().Targets.First();
//			string targetName = initialTarget.Key;
//			object initialTargetValue = initialTarget.Value;
//			foreach (var instance in testData) {
//				instance.SetTarget(targetName, initialTargetValue);
//			}

//			Assert.AreEqual(1, testData.Select(x => x.Targets.First().Value)
//			              .Distinct().Count(),
//			              "Should only be one classification value.");
			
			// Act
			var model = classifier.Train(trainingData);
			model.Classify(testData);
			
//			var allowedClasses = new HashSet<string>(
//			   trainingData.Select(x => x.Targets.First().Value.ToString()));
			
			// Assert
//			Assert.IsTrue(allowedClasses.Count > 0,
//			              "No class values found");

//			var newTargetValues = testData.Select(x => x.Targets.First().Value).Distinct();
//			Assert.IsTrue(newTargetValues.Count() > 1 ||
//						  !newTargetValues.Contains(initialTargetValue.ToString()),
//			              "Classification value should change.");
//			for (int i=0; i<testData.Length; ++i) {
//				Assert.AreNotEqual(null, testData[i].Targets.First().Value,
//				              "Result class should not be null.");
//				Assert.IsTrue(allowedClasses.Contains(
//					testData[i].Targets.First().Value.ToString()),
//				              "Result should be in allowed class.");
//			}
		}
	}
}

