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
			var attributes = GetAllAttributes(instances);
			var wekaAttributes = GetWekaAttributes(attributes);
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
					Console.WriteLine("Type: " + instanceValue.Value.GetType().ToString());
					if (instanceValue.GetType() == typeof(double)) {
						values[i] = (double)instanceValue.Value;
					} else if (instanceValue.GetType() == typeof(int)) {
						values[i] = (int)instanceValue.Value;						
					}
					++i;
				}
				
				if (instance.Targets.Count() > 0) {
					var target = instance.Targets.First();
					Console.WriteLine("Target type: " + target.Value.GetType().ToString());
					if (target.GetType() == typeof(double)) {
						values[i] = (double)target.Value;
					} else if (target.GetType() == typeof(int)) {
						values[i] = (int)target.Value;						
					}
					wekaInstances.setClassIndex(i);
				}

				var wekaInstance = new weka.core.Instance(1.0, values);
				wekaInstances.add(wekaInstance);
			}
			
			return wekaInstances;
		}
		
		/// <summary>
		/// Get the fields to construct Weka instances.
		/// </summary>
		/// <param name="instances">
		/// A <see cref="IEnumerable<T>"/>
		/// </param>
		/// <returns>
		/// A <see cref="IEnumerable<KeyValuePair<System.String, System.Object>>"/>
		/// </returns>
		private static IEnumerable<KeyValuePair<string, Type> > GetAllAttributes<T>(
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
			
			return attributes;
		}
	
		private static weka.core.FastVector GetWekaAttributes(
		  IEnumerable<KeyValuePair<string, Type> > attributes)
		{
		    var wekaAttributes = new weka.core.FastVector();
			foreach (var attribute in attributes) {
				weka.core.Attribute wekaAttribute;
				if (attribute.Value == typeof(int) ||
				    attribute.Value == typeof(double)) {
					wekaAttribute = new weka.core.Attribute(attribute.Key);
				} else {
					throw new NotSupportedException(
			          "This type of feature is not supported for this operation.");
				}
				wekaAttributes.addElement(wekaAttribute);
				
//				else if (attribute.Value.IsEnum) {
//					Enum type = attribute.Value as System.Enum;
//					type.
//				}
			}
			return wekaAttributes;
		}
	}
}

