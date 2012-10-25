using System;
using System.Collections.Generic;

using DaisyML;

namespace DaisyML.Classifiers
{
	public class NaiveBayesClassifier : IClassifier
	{
		public NaiveBayesClassifier ()
		{

		}
		
		#region IClassifier implementation
		IModel<T> IClassifier.Train<T> (IEnumerable<T> trainingData)
		
		{
			return new NaiveBayesModel<T>();	
		}
		
		#endregion
	}
}

