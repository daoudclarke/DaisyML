using System;
using System.Collections.Generic;
namespace DaisyML
{
	public interface IInstance
	{
		IEnumerable<IAttribute<string>> StringFeatures { get; }

		IEnumerable<IAttribute<double>> NumericFeatures { get; }

		IEnumerable<IAttribute<Enum>> NominalFeatures { get; }

		IEnumerable<IAttribute<Type>> MissingFeatures { get; }

		IEnumerable<IAttribute<double>> NumericTargets { get; }

		IEnumerable<IAttribute<Enum>> NominalTargets { get; }

		IEnumerable<IAttribute<Type>> MissingTargets { get; }

		void SetTarget (string name, double targetValue);

		void SetTarget (string name, Enum targetValue);
		
		void SetTargetMissing (string name);
		
		string TypeIdentifier { get; }
	}

	
//	/// <summary>
//	/// An instance is a piece of data used to train a classifier, or
//	/// to be classified.
//	/// </summary>
//	public interface IInstance
//	{
//		IEnumerable<KeyValuePair<string, object>> Features { get; }
//		IEnumerable<KeyValuePair<string, object>> Targets { get ; }
//		void SetTarget(string targetName, object targetValue);
//		string TypeIdentifier { get; }		
//		object GetValue(string attributeName);
//	}
//	
//	public interface IInstance5
//	{
//		IEnumerable<Attribute> Features { get; }
//		IEnumerable<Attribute> Targets { get; }
//		void SetTarget (Attribute target);
//		string TypeIdentifier { get; }		
//	}
	
//	public interface IInstance1
//	{
//		IEnumerable<FeatureValue> Features {get;}
//		FeatureValue GetValue(string attributeName);
//		void SetTarget(string targetName, object targetValue);
//
//		/// <summary>
//		/// A unique string specifying the type of the instance.
//		/// Classifiers should train and classify the same type of
//		/// instance.
//		/// </summary>
//		string TypeIdentifier { get; }
//	}
	
//	public interface IInstance2
//	{
//		IEnumerable<string> AttributeNames {get;}	
//		object GetValue(string attributeName);
//		Type GetType(string attributeName);
//		void SetTarget (string targetName, object targetValue);
//
//		bool IsTarget(string attributeName);
//	}
//	
//	public interface IInstance3
//	{
//		IEnumerable<KeyValuePair<string, object>> Attributes {get;}
//		object this[string name] {get; set;}
//		Type GetType (string attributeName);
//		bool IsTarget (string attributeName);
//
//		/// <summary>
//		/// A unique string specifying the type of the instance.
//		/// Classifiers should train and classify the same type of
//		/// instance.
//		/// </summary>
//		string TypeIdentifier { get; }
//	}
//
}