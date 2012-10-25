using System;
using DaisyML;

namespace DaisyML.Classifiers
{
	public class NaiveBayesModel<T> : IModel<T>
		where T : IInstance
	{
		public NaiveBayesModel()
		{
		}
		
		public T Classify(T instance) {
			return instance;
		}
	}
}

