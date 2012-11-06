using System;
using System.Collections.Generic;

using DaisyML;
using DaisyML.Weka;

namespace DaisyML.Classifiers
{
	public class NaiveBayesClassifier : IClassifier
	{
		public NaiveBayesClassifier ()
		{

		}

		#region IClassifier implementation
		public IModel<T> Train<T> (IEnumerable<T> trainingData)
			where T : IInstance
		{
			var wekaInstances = WekaInstanceUtils.ConvertToWeka(trainingData);
			weka.classifiers.bayes.NaiveBayes classifier = new weka.classifiers.bayes.NaiveBayes();
			classifier.buildClassifier(wekaInstances);
			return new NaiveBayesModel<T>(classifier);
		}
		
		#endregion
	}
}

