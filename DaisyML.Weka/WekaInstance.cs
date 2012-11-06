using System;
using System.Collections.Generic;

namespace DaisyML.Weka
{
	internal class WekaInstance : IInstance
	{
		private weka.core.Instance _instance;
			
		internal WekaInstance(weka.core.Instance instance) {
			_instance = instance;
		}
		
		#region IInstance implementation
		public void SetTarget (string targetName, object targetValue)
		{
			throw new System.NotImplementedException();
		}
		
		
		public IEnumerable<KeyValuePair<string, object>> Features {
			get {
				for (int i=0; i<_instance.numAttributes(); ++i) {
					if (i == _instance.classIndex()) {
						continue;
					}
					yield return new KeyValuePair<string, object>(
					  _instance.attribute(i).name(), _instance.value(i));
				}
			}
		}
		
		
		public IEnumerable<KeyValuePair<string, object>> Targets {
			get {
				int classIndex = _instance.classIndex();
				yield return new KeyValuePair<string, object>(
				  _instance.attribute(classIndex).name(), _instance.value(classIndex));
			}
		}

		public string TypeIdentifier { get {
				return _instance.dataset().relationName();
			}
		}
		
		#endregion
	}
}

