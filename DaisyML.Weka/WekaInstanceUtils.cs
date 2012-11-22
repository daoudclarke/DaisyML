using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

using DaisyML;

namespace DaisyML.Weka
{
	public static class WekaInstanceUtils
	{
		private static double GetWekaValue(weka.core.Attribute attribute, string val)
		{
			return attribute.indexOfValue((string)val);
		}
		
		private static double GetWekaValue(weka.core.Attribute attribute, Enum val)
		{
			return attribute.indexOfValue(val.ToString());
		}

		/// <summary>
		/// Convert DaisyML instances to Weka ones.
		/// </summary>
		/// <param name="instances">
		/// A <see cref="IEnumerable<T>"/>
		/// </param>
		/// <returns>
		/// A <see cref="weka.core.Instances"/>
		/// </returns>
		public static weka.core.Instances ConvertToWeka(
		  IEnumerable<IInstance> instances)
		{
			
			string name = instances.First().TypeIdentifier;
			var wekaAttributes = GetWekaAttributes(instances);
			int size = instances.Count();

			var wekaInstances = new weka.core.Instances(
			  name, wekaAttributes.Values.ToFastVector(), size);

			foreach (var instance in instances) {
				var numTargets = instance.NominalTargets.Count() +
					instance.NumericTargets.Count();
				if (numTargets > 1) {
					throw new InvalidOperationException(
					  "This operation only supports instances with at most one target, " +
					  String.Format("this instance has {0} targets.", numTargets));
				}
				
				var values = new SortedDictionary<string, double>();
				foreach (var feature in instance.NumericFeatures
							.Concat(instance.NumericTargets)) {
					values[feature.Name] = feature.Value;
				}
				
				foreach(var feature in instance.NominalFeatures
							.Concat(instance.NominalTargets)) {
					values[feature.Name] = GetWekaValue(
						wekaAttributes[feature.Name],
						feature.Value);
				}

				foreach(var feature in instance.StringFeatures) {
					values[feature.Name] = GetWekaValue (
						wekaAttributes [feature.Name],
						feature.Value);
				}
				
				foreach(var feature in instance.MissingFeatures
					.Concat(instance.MissingTargets)) {
					values[feature.Name] = 0.0;
				}
				
				Debug.Fail("This is a test.");
				Debug.Assert(values.Keys.SequenceEqual(wekaAttributes.Keys));
				
				var attributeNames = wekaAttributes.Keys.ToList();
				var wekaInstance = new weka.core.Instance(1.0, values.Values.ToArray());
								
				if (instance.NumericTargets.Count() != 0) {
					var classIndex = attributeNames.IndexOf(instance.NumericTargets.First().Name);
					wekaInstances.setClassIndex(classIndex);
				}
				
				if (instance.NominalTargets.Count() != 0) {
					var classIndex = attributeNames.IndexOf(instance.NominalTargets.First().Name);
					wekaInstances.setClassIndex(classIndex);
				}
				
				foreach (var feature in instance.MissingFeatures
					.Concat(instance.MissingTargets)) {
					var index = attributeNames.IndexOf(feature.Name);
					wekaInstance.setMissing(index);
				}
				
				wekaInstances.add(wekaInstance);
			}
			
			return wekaInstances;
		}
			
		private static SortedDictionary<string, weka.core.Attribute> GetWekaAttributes(
		  IEnumerable<IInstance> instances)
		{
			var attributes = new HashSet<Attribute<Type>>();

			var wekaAttributes = new SortedDictionary<string, weka.core.Attribute> ();
			var stringValues = new Dictionary<string, HashSet<string>>();
			foreach (var instance in instances) {
				attributes.UnionWith(
					instance.NominalFeatures.Concat(instance.NominalTargets).Select(
						x => new Attribute<Type>(x.Name, x.Value.GetType())));
				
				attributes.UnionWith (
					instance.NumericFeatures.Concat(instance.NumericTargets).Select (
						x => new Attribute<Type> (x.Name, typeof(double))));

				attributes.UnionWith (
					instance.StringFeatures.Select (
						x => new Attribute<Type> (x.Name, typeof(string))));
				
				attributes.UnionWith(
					instance.MissingFeatures.Concat(instance.MissingTargets).Select (
						x => new Attribute<Type>(x.Name, x.Value)));

				foreach (var feature in instance.StringFeatures) {
					if (!stringValues.ContainsKey (feature.Name)) {
						stringValues [feature.Name] = new HashSet<string> ();
					}
					stringValues[feature.Name].Add(feature.Value);
				}
			}
			
			foreach (var attribute in attributes) {
				weka.core.Attribute wekaAttribute;
				if (attribute.Value == typeof(int) ||
				    attribute.Value == typeof(double)) {
					wekaAttribute = new weka.core.Attribute(attribute.Name);
				} else if (attribute.Value == typeof(string)) {
					var orderedValues = stringValues[attribute.Name]
						.OrderBy(y => y).ToArray();
					var vector = new weka.core.FastVector(orderedValues.Length);
					foreach (var v in orderedValues) {
						vector.addElement(v);
					}
					wekaAttribute = new weka.core.Attribute(attribute.Name, vector);
				} else if (attribute.Value.IsEnum) {
					var values = attribute.Value.GetFields()
									.Select(x => x.Name).ToArray();
					var vector = new weka.core.FastVector (values.Length);
					foreach (var v in values) {
						vector.addElement (v);
					}
					wekaAttribute = new weka.core.Attribute (attribute.Name, vector);
				} else {
					throw new NotSupportedException(
			          "This type of feature is not supported for this operation.");
				}
				wekaAttributes[attribute.Name] = wekaAttribute;
			}
			return wekaAttributes;
		}
		
		public static weka.core.FastVector ToFastVector<T>(this IEnumerable<T> values)
		{
			var result = new weka.core.FastVector();
			foreach (var v in values) {
				result.addElement(v);
			}
			return result;
		}
	}
}

