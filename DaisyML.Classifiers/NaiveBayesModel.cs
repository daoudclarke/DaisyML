using System;
using System.Linq;

using DaisyML;
using DaisyML.Weka;

namespace DaisyML.Classifiers
{
	public class NaiveBayesModel<T> : IModel<T>
		where T : IInstance
	{
		private weka.classifiers.bayes.NaiveBayes _classifier;
		
		internal NaiveBayesModel(weka.classifiers.bayes.NaiveBayes classifier)
		{
			_classifier = classifier;
		}
		
		public void Classify(ref T instance) {
			if (instance.Targets.Count() != 1) {
				throw new InvalidOperationException(
				  "Wrong number of targets for classification. " +
				  "This classifier needs instances with exactly one target.");
			}
			string targetName = instance.Targets.First().Key;
			
			var wekaInstances = WekaInstanceUtils.ConvertToWeka(
			  new T[] {instance});
			var wekaInstance = wekaInstances.firstInstance();
			var result = _classifier.classifyInstance(wekaInstance);
			instance.SetTarget(targetName, result);
		}
	}
}

