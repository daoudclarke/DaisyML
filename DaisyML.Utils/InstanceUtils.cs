using System;
using System.Collections.Generic;
using System.IO;

using weka.core.converters;

namespace DaisyML.Utils
{
	public static class InstanceUtils
	{
		public static IEnumerable<T> LoadInstancesFromArff<T>(Stream stream)
			where T : Instance
		{
			var reader = new StreamReader(stream);
			var data = reader.ReadToEnd();
			var encoding = new System.Text.UTF8Encoding();
			var javaStream = new java.io.ByteArrayInputStream(
			      encoding.GetBytes(data));
			
			var arffLoader = new ArffLoader();
			arffLoader.setSource(javaStream);
			var wekaInstances = arffLoader.getDataSet();
			return ConvertWekaInstances<T>(wekaInstances);			
		}
		
		static IEnumerable<T> ConvertWekaInstances<T>(
		  weka.core.Instances wekaInstances)
			where T : Instance
		{
			var converted = new List<T>();
			var instance = (T)Activator.CreateInstance(typeof(T));
			return converted;
		}
	}
}

