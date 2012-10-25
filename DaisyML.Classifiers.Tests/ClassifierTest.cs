using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;
using DaisyML;
using DaisyML.Classifiers;

namespace DaisyML.Classifiers.Tests
{
	[TestFixture]
	public class ClassifierTest
	{
		[Test]
		public void TestClassifiers() {
			var classifiers = new List<IClassifier>();

			classifiers.Add(new NaiveBayesClassifier());

			
			
//			// Get all classifier implementations
//			var ignore = new Classifiers.MyClass();
//			var assembly = Assembly.GetAssembly(ignore.GetType());
//			var types = assembly.GetExportedTypes();
//			foreach (var type in types) {
//				if (type.GetInterface("IClassifer") != null) {
//					Console.WriteLine(type.ToString());
//					var constructor = type.GetConstructor(new Type[] {}); 
//					var classifier =
//						(IClassifier)constructor.Invoke(new object[] {});
//					classifiers.Add(classifier);
//				}
//			}
//			
//			Assert.Greater(0, classifiers.Count);
		}

		private IEnumerable<Instance> GetTestData() {
			var stream = System.Reflection.Assembly.GetEntryAssembly()
				.GetManifestResourceStream(
				  "DaisyML.Classifiers.Tests.Resources.iris.arff");
			
			return null;
		}
	}
}

