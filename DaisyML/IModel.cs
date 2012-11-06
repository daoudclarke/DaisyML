using System;
using System.Collections.Generic;

namespace DaisyML
{
	/// <summary>
	/// Models allow classification of instances, and are build by classifiers.
	/// </summary>
	public interface IModel<T>
		where T : IInstance
	{
		/// <summary>
		/// Classify an instance, setting targets within the instance to
		/// the value estimated by the classifier.
		/// </summary>
		/// <param name="instance">
		/// A <see cref="T"/>
		/// </param>
		void Classify(ref T instance);
	}
	
	public static class ModelUtils {
		public static void Classify<T>(this IModel<T> model,
		                            IEnumerable<T> instances)
			where T : IInstance
		{
			foreach (var instance in instances) {
				var copy = instance;
				model.Classify(ref copy);
			}
		}
	}


}

