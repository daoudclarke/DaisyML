using System;
using System.Collections.Generic;
using System.Linq;

using DaisyML.Weka;

namespace DaisyML.Classifiers
{
	internal static class WekaTrain
	{
		#region IClassifier implementation
		public static IModel<TInstance> Train<TClassifier, TInstance> (
			IEnumerable<TInstance> trainingData)
			where TInstance : IInstance
			where TClassifier : weka.classifiers.Classifier, new()
		{
			var wekaInstances = WekaInstanceUtils.ConvertToWeka (trainingData);
			var classifier = new TClassifier();
			classifier.buildClassifier (wekaInstances);
			return new WekaModel<TInstance, TClassifier>(classifier);
		}
		
		#endregion
	}
}

