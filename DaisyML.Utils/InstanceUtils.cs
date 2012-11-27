using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

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
			return new WekaInstances(wekaInstances);
		}
		
		/// <summary>
		/// Shuffle a sequence. Ordering is consistent between multiple calls.
		/// </summary>
		/// <param name="sequence">
		/// A <see cref="IEnumerable<T>"/>
		/// </param>
		/// <returns>
		/// A <see cref="IEnumerable<T>"/>
		/// </returns>
		public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> sequence) {
			return Shuffle(sequence, new Random(1));
		}

		/// <summary>
		/// Shuffle a sequence using the given random number generator.
		/// </summary>
		/// <param name="sequence">
		/// A <see cref="IEnumerable<T>"/>
		/// </param>
		/// <param name="random">
		/// A <see cref="Random"/>
		/// </param>
		/// <returns>
		/// A <see cref="IEnumerable<T>"/>
		/// </returns>
		public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> sequence,
		                                        Random random) {
			var result = sequence.ToArray();
			for (int i=0; i<result.Length; ++i) {
				int index = random.Next(i, result.Length);
				var swap = result[i];
				result[i] = result[index];
				result[index] = swap;
			}
			return result;
		}
		
		public static string ToJson(IEnumerable<IInstance> instances) {
			var dictionaryInstances = instances.Select(
				x => new DictionaryInstance(x)).ToArray();
			return JsonConvert.SerializeObject(dictionaryInstances);
		}
	}
}

