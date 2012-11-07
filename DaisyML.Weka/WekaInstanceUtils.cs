using System;
using System.Collections.Generic;
using System.Linq;

using DaisyML;

namespace DaisyML.Weka
{
	public static class WekaInstanceUtils
	{
		public static IEnumerable<IInstance> ConvertFromWeka(
		  weka.core.Instances wekaInstances)
		{
			return new WekaInstances(wekaInstances);
		}
		
		private static double GetWekaValue(weka.core.Attribute attribute, object val)
		{
			if (val.GetType() == typeof(double)) {
				return (double)val;
			} else if (val.GetType() == typeof(int)) {
				return (int)val;						
			} else if (val.GetType() == typeof(string)) {
				return attribute.indexOfValue((string)val);
			} else {
				throw new InvalidOperationException(String.Format(
				  "This operation does not support the type {0}",
				  val.GetType()));
			}
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
		public static weka.core.Instances ConvertToWeka<T>(
		  IEnumerable<T> instances)
			where T : IInstance {
			
			string name = instances.First().TypeIdentifier;
			var wekaAttributes = GetWekaAttributes(instances);
			int size = instances.Count();

			var wekaInstances = new weka.core.Instances(
			  name, wekaAttributes, size);

			foreach (var instance in instances) {
				var numTargets = instance.Targets.Count();
				if (numTargets > 1) {
					throw new InvalidOperationException(
					  "This operation only supports instances with at most one target, " +
					  String.Format("this instance has {0} targets.", numTargets));
				}
				
				var values = new double[wekaAttributes.size()];
				int i=0;
				foreach (var instanceValue in instance.Features) {
					values[i] = GetWekaValue((weka.core.Attribute)wekaAttributes.elementAt(i), instanceValue.Value);
					++i;
				}
				
				if (instance.Targets.Count() > 0) {
					var target = instance.Targets.First();
					values[i] = GetWekaValue((weka.core.Attribute)wekaAttributes.elementAt(i), target.Value);
					wekaInstances.setClassIndex(i);
				}

				var wekaInstance = new weka.core.Instance(1.0, values);
				wekaInstances.add(wekaInstance);
			}
			
			return wekaInstances;
		}
			
		private static weka.core.FastVector GetWekaAttributes<T>(
		  IEnumerable<T> instances)
			where T : IInstance
		{
			var attributes = new HashSet<KeyValuePair<string, Type>>();

			foreach (var instance in instances) {
				foreach (var attribute in instance.Features.Concat(instance.Targets)) {
					attributes.Add(new KeyValuePair<string, Type> (
					  attribute.Key, attribute.Value.GetType()));
				}
			}
			
			var wekaAttributes = new weka.core.FastVector();
			foreach (var attribute in attributes) {
				weka.core.Attribute wekaAttribute;
				if (attribute.Value == typeof(int) ||
				    attribute.Value == typeof(double)) {
					wekaAttribute = new weka.core.Attribute(attribute.Key);
				} else if (attribute.Value == typeof(string)) {
					var values = new HashSet<string>(instances.Select(
					  x => (string)x.GetValue(attribute.Key)));
					var orderedValues = values.OrderBy(y => y).ToArray();
					var vector = new weka.core.FastVector(orderedValues.Length);
					foreach (var v in values) {
						vector.addElement(v);
					}
					wekaAttribute = new weka.core.Attribute(attribute.Key, vector);
				} else {
					throw new NotSupportedException(
			          "This type of feature is not supported for this operation.");
				}
				wekaAttributes.addElement(wekaAttribute);
			}
			return wekaAttributes;
		}
	}
}

