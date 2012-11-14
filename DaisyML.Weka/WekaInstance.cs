using System;
using System.Collections.Generic;

using DaisyML;

namespace DaisyML.Weka
{
	internal class WekaInstance : IInstance
	{
		private weka.core.Instance _instance;
	
		private EnumRepository _repository;
							
		internal WekaInstance(weka.core.Instance instance, EnumRepository repository) {
			_instance = instance;
			_repository = repository;
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
			} else if (type.IsEnum) {
				_instance.setClassValue(targetValue.ToString());	
			} else {
				throw new InvalidOperationException(String.Format(
				  "Cannot set target of type {0}.", type));
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
			return GetInstanceValue(attribute);
		}
		
		public string TypeIdentifier { get {
				return _instance.dataset().relationName();
			}
		}
		
		#endregion
	
		private KeyValuePair<string, object> GetInstanceValue (int index)
		{
			var attribute = _instance.attribute (index);
			return GetInstanceValue (attribute);		
		}
		
		private KeyValuePair<string, object> GetInstanceValue (weka.core.Attribute attribute)
		{
			if (attribute.isNominal ()) {
				var enumValue = _repository.GetEnumValue(attribute, (int)_instance.value(attribute));
				return new KeyValuePair<string, object>(attribute.name(), enumValue);
			} else if (attribute.isString ()) {
				return new KeyValuePair<string, object> (
				  attribute.name (), _instance.stringValue (attribute.index()));				
			} else {
				return new KeyValuePair<string, object> (
				  attribute.name (), _instance.value (attribute.index()));
			}
		}		
	}
}

