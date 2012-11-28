using System;
using System.Collections.Generic;
using System.Linq;

using DaisyML.Weka;

namespace DaisyML.Classifiers
{
	internal class WekaModel<TInstance, TClassifier> : IModel<TInstance>
		where TClassifier : weka.classifiers.Classifier, new()
		where TInstance : IInstance
	{
		private readonly TClassifier _classifier;
	
		public WekaModel(TClassifier classifier)
		{
			_classifier = classifier;
		}
		
		public void Classify (ref TInstance instance)
		{
			string targetName;
			if (instance.NumericTargets.Count () > 0) {
				targetName = instance.NumericTargets.First ().Name;
			} else if (instance.NominalTargets.Count () > 0) {
				targetName = instance.NominalTargets.First ().Name;			
			} else {
				throw new InvalidOperationException (
				  "Wrong number of targets for classification. " +
				  "This classifier needs instances with exactly one target.");
			}
			
			var wekaInstances = WekaInstanceUtils.ConvertToWeka (
			  new TInstance[] {instance});
			var wekaInstance = wekaInstances.firstInstance ();
			var result = _classifier.classifyInstance (wekaInstance);
			instance.SetTarget (targetName, result);
		}
	}
}

