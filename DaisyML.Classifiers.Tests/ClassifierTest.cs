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
			var classifiers = new IClassifier[] {
				new NaiveBayesClassifier (),
				new DecisionTree()
			};

			foreach (var classifier in classifiers) {						
				// Arrange
				int numDatasetsProcessed = 0;
				var allInstances = Data.GetTestInstances();
				foreach (var instances in allInstances) {
					var data = instances.Shuffle();
					var trainingSetSize = data.Count() / 2;
					var trainingData = data.Take(trainingSetSize);
					var testData = data.Skip(trainingSetSize).ToArray();
		
					var targetName = testData.First().GetTargetNames().First();
					foreach (var instance in testData) {
						instance.SetTargetMissing(targetName);
					}
					
					// Act
					try {
						var model = classifier.Train(trainingData);
						model.Classify(testData);
					} catch (weka.core.UnsupportedAttributeTypeException) {
						// These instances are not suitable for the classifier
						continue;
					}
					
					// Assert - no instance should have a missing class value
					foreach (var instance in testData) {
						Assert.AreEqual(0, instance.MissingTargets.Count());
					}
					++numDatasetsProcessed;
				}
				Assert.Greater(numDatasetsProcessed, 0,
					"No matching datasets for classifier.");
			}
		}
	}
}

