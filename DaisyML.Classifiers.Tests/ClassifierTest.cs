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

			var targetName = testData.First().GetTargetNames().First();
			foreach (var instance in testData) {
				instance.SetTargetMissing(targetName);
			}
			
			// Act
			var model = classifier.Train(trainingData);
			model.Classify(testData);
			
			// Assert - no instance should have a missing class value
			foreach (var instance in testData) {
				Assert.AreEqual(0, instance.MissingTargets.Count());
			}
		}
	}
}

