using System;
using System.Collections.Generic;

namespace DaisyML
{
	public interface IClassifier
	{
		/// <summary>
		/// Train a model using training data.
		/// </summary>
		/// <param name="trainingData">
		/// A <see cref="IEnumerable<T>"/>
		/// </param>
		/// <returns>
		/// A <see cref="IModel<T>"/>
		/// </returns>
		IModel<T> Train<T>(IEnumerable<T> trainingData)
			where T : IInstance;
	}
}

