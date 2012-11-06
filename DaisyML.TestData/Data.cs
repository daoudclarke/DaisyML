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
			var stream = assembly.GetManifestResourceStream(
				  "DaisyML.TestData.Resources.iris.arff");
			
			yield return InstanceUtils.LoadInstancesFromArff(stream);
		}
	}
}

