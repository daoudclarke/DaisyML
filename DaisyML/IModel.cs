using System;
namespace DaisyML
{
	/// <summary>
	/// Models allow classification of instances, and are build by classifiers.
	/// </summary>
	public interface IModel<T>
		where T : IInstance
	{
		/// <summary>
		/// Classify an instance, returning a copy with
		/// the class value specified.
		/// </summary>
		/// <param name="instance">
		/// A <see cref="T"/>
		/// </param>
		/// <returns>
		/// A <see cref="T"/>
		/// </returns>
		T Classify(T instance);
	}
}

