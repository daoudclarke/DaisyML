using System;
using System.Collections.Generic;
using System.IO;

using DaisyML.Weka;

using weka.core.converters;

namespace DaisyML.Utils
{
	public static class InstanceUtils
	{
		public static IEnumerable<IInstance> LoadInstancesFromArff(Stream stream)
		{
			var reader = new StreamReader(stream);
			var data = reader.ReadToEnd();
			var encoding = new System.Text.UTF8Encoding();
			var javaStream = new java.io.ByteArrayInputStream(
			      encoding.GetBytes(data));
			
			var arffLoader = new ArffLoader();
			arffLoader.setSource(javaStream);
			var wekaInstances = arffLoader.getDataSet();
			wekaInstances.setClassIndex(wekaInstances.numAttributes() - 1);
			return WekaInstanceUtils.ConvertFromWeka(wekaInstances);			
		}
	}
}

