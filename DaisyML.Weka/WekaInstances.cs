using System;
using System.Collections.Generic;

using DaisyML;

namespace DaisyML.Weka
{	
	public class WekaInstances : IEnumerable<IInstance>
	{
		private weka.core.Instances _instances;
		private EnumRepository _repository = new EnumRepository();
		
		public WekaInstances (weka.core.Instances instances)
		{
			_instances = instances;
		}
	
		public string GetArff() {
			return _instances.toString();
		}

		#region IEnumerable<IInstance> implementation
		public IEnumerator<IInstance> GetEnumerator ()
		{
			for (int i=0; i<_instances.numInstances(); ++i) {
				var instance = _instances.instance(i);
				yield return new WekaInstance(instance, _repository);
			}
		}
		
		#endregion
		
		#region IEnumerable implementation
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator ()
		{
			foreach (IInstance instance in this) {
				yield return instance;
			}
		}
		
		#endregion
	}
}

