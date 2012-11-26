using System;
using System.Collections.Generic;
using System.Linq;

namespace DaisyML
{
	public static class InstanceExtensions
	{
		public static bool Equals(this IInstance input, object other)
		{
			var otherInstance = other as IInstance;
			return Equals(input.NumericFeatures, otherInstance.NumericFeatures)
				&& Equals(input.NominalFeatures, otherInstance.NominalFeatures)
				&& Equals(input.StringFeatures, otherInstance.StringFeatures)
				&& Equals(input.MissingFeatures, otherInstance.MissingFeatures)
				&& Equals(input.NominalTargets, otherInstance.NominalTargets)
				&& Equals(input.NumericTargets, otherInstance.NumericTargets);				
		}
		
		private static bool Equals<T>(IEnumerable<IAttribute<T>> input,
			IEnumerable<IAttribute<T>> other)
			where T : IEquatable<T>
		{
			var matches = input.Zip<IAttribute<T>, IAttribute<T>,
				bool> (other,
				(x,y) => x.Name == y.Name
				&& EqualityComparer<T>.Default.Equals(x.Value, y.Value));
			foreach (var match in matches) {
				if (!match) {
					return false;
				}
			}
			return true;		
		}
		
	}
}

