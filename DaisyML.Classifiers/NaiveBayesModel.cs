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
			
			string targetName;
			if (instance.NumericTargets.Count() > 0) {
				targetName = instance.NumericTargets.First().Name;
			} else if (instance.NominalTargets.Count() > 0) {
				targetName = instance.NominalTargets.First().Name;			
			} else {
				throw new InvalidOperationException(
				  "Wrong number of targets for classification. " +
				  "This classifier needs instances with exactly one target.");
			}
			
			var wekaInstances = WekaInstanceUtils.ConvertToWeka(
			  new T[] {instance});
			var wekaInstance = wekaInstances.firstInstance();
			var result = _classifier.classifyInstance(wekaInstance);
			instance.SetTarget(targetName, result);
		}
	}
}

