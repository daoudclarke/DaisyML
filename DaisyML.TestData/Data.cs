using System;
using System.Collections.Generic;

using DaisyML;
using DaisyML.Utils;

namespace DaisyML.TestData
{
	public static class Data
	{
		public static IEnumerable<IEnumerable<IInstance>> GetTestInstances() {
			var assembly = System.Reflection.Assembly.GetAssembly(typeof(Data));
			var names = assembly.GetManifestResourceNames();
			foreach (var name in names) {
				var stream = assembly.GetManifestResourceStream(
				  name);
			
				yield return InstanceUtils.LoadInstancesFromArff(stream);
			}
		}
	}
}

