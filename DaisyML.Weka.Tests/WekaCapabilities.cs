using System;

using System.Linq;
using System.Collections.Generic;

using NUnit.Framework;

using DaisyML;
using DaisyML.Weka;

namespace DaisyML.Weka.Tests
{
	[TestFixture]
	public class WekaCapabilities
	{
		[Test]
		public void EnumerateCapabilities()
		{
			var wekaAssembly = 
				System.Reflection.Assembly.GetAssembly(typeof(weka.classifiers.Classifier));
			var classifierTypes = wekaAssembly.GetTypes()
				.Where(x => x.IsSubclassOf(typeof(weka.classifiers.Classifier))
					   && !x.IsAbstract && x.GetConstructor(Type.EmptyTypes) != null);
			foreach (var classifier in classifierTypes) {
				Console.WriteLine();
				Console.WriteLine(classifier.Name);
				var instance = (weka.classifiers.Classifier)Activator.CreateInstance(classifier);
				
				var capabilities = weka.core.Capabilities.Capability.values();
				var classifierCapabilities = instance.getCapabilities();
				foreach (var capability in capabilities) {
					if (classifierCapabilities.handles(capability)) {
						Console.WriteLine(capability.ToString());
					}
				}
			}
		}
	}
}

