using System;
using System.Collections.Generic;
namespace DaisyML
{
	/// <summary>
	/// An instance is a piece of data used to train a classifier, or
	/// to be classified.
	/// </summary>
	public interface IInstance
	{
		IEnumerable<KeyValuePair<string, Object> > Features { get; }
		IEnumerable<KeyValuePair<string, Object> > Targets { get; }
		object GetValue(string attributeName);
		void SetTarget(string targetName, object targetValue);

		/// <summary>
		/// A unique string specifying the type of the instance.
		/// Classifiers should train and classify the same type of
		/// instance.
		/// </summary>
		string TypeIdentifier { get; }
	}
}

