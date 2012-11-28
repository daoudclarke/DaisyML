using System;
using System.Collections.Generic;
using System.Linq;

namespace DaisyML
{
	public class DictionaryInstance : IInstance
	{
		public IDictionary<string, double> NumericFeatureIndex {get; private set;}
		public IDictionary<string, Enum> NominalFeatureIndex {get; private set;}
		public IDictionary<string, string> StringFeatureIndex {get; private set;}
		public IDictionary<string, Type> MissingFeatureIndex {get; private set;}

		public IDictionary<string, double> NumericTargetIndex {get; private set;}
		public IDictionary<string, Enum> NominalTargetIndex {get; private set;}
		public IDictionary<string, Type> MissingTargetIndex {get; private set;}
								
		public DictionaryInstance ()
		{
			NumericFeatureIndex = new SortedDictionary<string, double>();
			NominalFeatureIndex = new SortedDictionary<string, Enum>();
			StringFeatureIndex = new SortedDictionary<string, string>();
			MissingFeatureIndex = new SortedDictionary<string, Type>();
			
			NumericTargetIndex = new SortedDictionary<string, double>();
			NominalTargetIndex = new SortedDictionary<string, Enum>();
			MissingTargetIndex = new SortedDictionary<string, Type>();
		}

		public DictionaryInstance(IInstance instance)
		{
			NumericFeatureIndex = instance.NumericFeatures.ToDictionary(
				x => x.Name, x => x.Value);
			NominalFeatureIndex = instance.NominalFeatures.ToDictionary (
				x => x.Name, x => x.Value);
			StringFeatureIndex = instance.StringFeatures.ToDictionary (
				x => x.Name, x => x.Value);
			MissingFeatureIndex = instance.MissingFeatures.ToDictionary (
				x => x.Name, x => x.Value);
			
			NumericTargetIndex = instance.NumericTargets.ToDictionary (
				x => x.Name, x => x.Value);
			NominalTargetIndex = instance.NominalTargets.ToDictionary (
				x => x.Name, x => x.Value);
			MissingTargetIndex = instance.MissingTargets.ToDictionary (
				x => x.Name, x => x.Value);
		}

		#region IInstance implementation
		public void SetTarget (string name, double targetValue)
		{
			NumericTargetIndex[name] = targetValue;
		}
	
		public void SetTarget (string name, Enum targetValue)
		{
			NominalTargetIndex[name] = targetValue;
		}
		
		public void SetTargetMissing (string name)
		{
			if (NumericTargetIndex.ContainsKey(name)) {
				NumericTargetIndex.Remove(name);
				MissingTargetIndex[name] = typeof(double);
			} else if (NominalTargetIndex.ContainsKey(name)) {
				var type = NominalTargetIndex[name].GetType();
				NominalTargetIndex.Remove(name);
				MissingTargetIndex[name] = type;
			}
		}
	
		public IEnumerable<IAttribute<string>> StringFeatures {
			get {
				return StringFeatureIndex.Select(
					x => (IAttribute<string>)(new Attribute<string>(x.Key, x.Value)));
			}
		}
	
		public IEnumerable<IAttribute<double>> NumericFeatures {
			get {
				return NumericFeatureIndex.Select (
					x => (IAttribute<double>)(new Attribute<double> (x.Key, x.Value)));
			}
		}
	
		public IEnumerable<IAttribute<Enum>> NominalFeatures {
			get {
				return NominalFeatureIndex.Select (
					x => (IAttribute<Enum>)(new Attribute<Enum> (x.Key, x.Value)));
			}
		}
	
		public IEnumerable<IAttribute<Type>> MissingFeatures {
			get {
				return MissingFeatureIndex.Select (
					x => (IAttribute<Type>)(new Attribute<Type> (x.Key, x.Value)));
			}
		}
	
		public IEnumerable<IAttribute<double>> NumericTargets {
			get {
				return NumericTargetIndex.Select (
					x => (IAttribute<double>)(new Attribute<double> (x.Key, x.Value)));
			}
		}
	
		public IEnumerable<IAttribute<Enum>> NominalTargets {
			get {
				return NominalFeatureIndex.Select (
					x => (IAttribute<Enum>)(new Attribute<Enum> (x.Key, x.Value)));
			}
		}
	
		public IEnumerable<IAttribute<Type>> MissingTargets {
			get {
				return MissingFeatureIndex.Select (
					x => (IAttribute<Type>)(new Attribute<Type> (x.Key, x.Value)));
			}
		}
	
		public string TypeIdentifier {
			get {
				return this.GetType().FullName;
			}
		}
		#endregion
	}
}

