using System;
using System.Collections.Generic;

using DaisyML;
using DaisyML.Weka;

namespace DaisyML.Classifiers
{
	public class NaiveBayesClassifier : IClassifier
	{
		#region IClassifier implementation
		public DaisyML.IModel<T> Train<T> (System.Collections.Generic.IEnumerable<T> trainingData) where T : DaisyML.IInstance
		{
			return WekaTrain.Train<weka.classifiers.bayes.NaiveBayes, T>(trainingData);
		}
		#endregion
	}
}

