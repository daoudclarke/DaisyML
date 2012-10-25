using System;
using System.Collections.Generic;
namespace DaisyML
{
	/// <summary>
	/// An instance is a piece of data used to train a classifier, or
	/// to be classified.
	/// </summary>
	public interface IInstance : IEnumerable<KeyValuePair<string, Object> >
	{
	}
}

