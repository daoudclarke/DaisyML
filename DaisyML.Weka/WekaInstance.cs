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
			var type = targetValue.GetType();
			if (type == typeof(string)) {
				_instance.setClassValue((string)targetValue);
			} else if (type == typeof(int)) {
				_instance.setClassValue((int)targetValue);				
			} else if (type == typeof(double)) {
				_instance.setClassValue((double)targetValue);				
			} else {
				throw new InvalidOperationException(String.Format(
				  "Cannot set target of type {0}.", type));
			}
		}
		
		private KeyValuePair<string, object> GetInstanceValue(int index)
		{
			var attribute = _instance.attribute(index);
			if (attribute.isNominal() || attribute.isString()) {
				return new KeyValuePair<string, object>(
				  attribute.name(), _instance.stringValue(index));				
			} else {
				return new KeyValuePair<string, object>(
				  attribute.name(), _instance.value(index));
			}
		}

		
		public IEnumerable<KeyValuePair<string, object>> Features {
			get {
				for (int i=0; i<_instance.numAttributes(); ++i) {
					if (i == _instance.classIndex()) {
						continue;
					}				
					yield return GetInstanceValue(i);
				}
			}
		}
		
		
		public IEnumerable<KeyValuePair<string, object>> Targets {
			get {
				int classIndex = _instance.classIndex();
				yield return GetInstanceValue(classIndex);
			}
		}

		public object GetValue(string attributeName) {
			var attribute = _instance.dataset().attribute(attributeName);
			if (attribute.isString() || attribute.isNominal()) {
				return _instance.stringValue(attribute);
			}
			return _instance.value(attribute);
		}
		
		public string TypeIdentifier { get {
				return _instance.dataset().relationName();
			}
		}
		
		#endregion
	}
}

