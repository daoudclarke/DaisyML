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
		public IEnumerable<IAttribute<string>> StringFeatures {
			get {
				for (int i=0; i<_instance.numAttributes(); ++i) {
					var attribute = _instance.attribute(i);
					if (attribute.isString()
						 && !_instance.isMissing(attribute)
						 && i != _instance.classIndex()) {
						yield return new Attribute<string>(
							attribute.name(), _instance.stringValue(i));
					}
				}
			}
		}
	
		public IEnumerable<IAttribute<double>> NumericFeatures {
			get {
				for (int i=0; i<_instance.numAttributes(); ++i) {
					var attribute = _instance.attribute (i);
					if (attribute.isNumeric()
						 && !_instance.isMissing (attribute)
						 && i != _instance.classIndex ()) {
						yield return new Attribute<double>(
							attribute.name(), _instance.value(i));
					}
				}
			}
		}
	
		public IEnumerable<IAttribute<Enum>> NominalFeatures {
			get {
				for (int i=0; i<_instance.numAttributes(); ++i) {
					var attribute = _instance.attribute (i);
					if (attribute.isNominal()
						&& !_instance.isMissing (attribute)
						&& i != _instance.classIndex ()) {
						yield return new Attribute<Enum>(
							attribute.name(),
							_repository.GetEnumValue(attribute,
								(int)_instance.value(attribute)));
					}
				}
			}
		}
	
		public IEnumerable<IAttribute<Type>> MissingFeatures {
			get {
				for (int i=0; i<_instance.numAttributes(); ++i) {
					var attribute = _instance.attribute (i);
					if (_instance.isMissing(attribute)
						&& i != _instance.classIndex ()) {
						yield return GetAttributeType(attribute);
					}
				}
			}
		}
	
		public IEnumerable<IAttribute<double>> NumericTargets {
			get {
				var index = _instance.classIndex();
				if (index >= 0) {
					var attribute = _instance.attribute(index);
					if (attribute.isNumeric()) {
						yield return
						  new Attribute<double>(attribute.name(), _instance.value(attribute));
					}
				}
			}
		}
	
		public IEnumerable<IAttribute<Enum>> NominalTargets {
			get {
				var index = _instance.classIndex ();
				if (index >= 0) {
					var attribute = _instance.attribute (index);
					if (attribute.isNominal()) {
						var nominal = _repository.GetEnumValue(
							attribute, (int)_instance.value(attribute));
						yield return
						  new Attribute<Enum>(attribute.name(), nominal);
					}
				}
			}
		}
	
		public IEnumerable<IAttribute<System.Type>> MissingTargets {
			get {
				if (_instance.classIsMissing()) {
					yield return GetAttributeType(_instance.classAttribute());
				}
			}
		}
	
		public string TypeIdentifier {
			get {
				return _instance.dataset().relationName();			
			}
		}
		
		public void SetTarget (string name, double targetValue)
		{
			var index = _instance.classIndex();
			var attributeName = _instance.attribute(index).name();
			if (index < 0 || attributeName != name) {
				throw new InvalidOperationException(
					"No target with specified name exists.");
			}
			
			_instance.setClassValue(targetValue);
		}
	
		public void SetTarget (string name, Enum targetValue)
		{
			var index = _instance.classIndex ();
			if (index < 0 || _instance.attribute(index).name () == name) {
				throw new InvalidOperationException (
					"No class with specified name exists.");
			}

			_instance.setClassValue(targetValue.ToString());
		}
		#endregion		
		
//		#region IInstance implementation
//		public void SetTarget (string targetName, object targetValue)
//		{
//			var type = targetValue.GetType();
//			if (type == typeof(string)) {
//				_instance.setClassValue((string)targetValue);
//			} else if (type == typeof(int)) {
//				_instance.setClassValue((int)targetValue);				
//			} else if (type == typeof(double)) {
//				_instance.setClassValue((double)targetValue);
//			} else if (type.IsEnum) {
//				_instance.setClassValue(targetValue.ToString());	
//			} else {
//				throw new InvalidOperationException(String.Format(
//				  "Cannot set target of type {0}.", type));
//			}
//		}
//		
//		public IEnumerable<KeyValuePair<string, object>> Features {
//			get {
//				for (int i=0; i<_instance.numAttributes(); ++i) {
//					if (i == _instance.classIndex()) {
//						continue;
//					}				
//					yield return GetInstanceValue(i);
//				}
//			}
//		}		
//		
//		public IEnumerable<KeyValuePair<string, object>> Targets {
//			get {
//				int classIndex = _instance.classIndex();
//				yield return GetInstanceValue(classIndex);
//			}
//		}
//
//		public object GetValue(string attributeName) {
//			var attribute = _instance.dataset().attribute(attributeName);
//			return GetInstanceValue(attribute);
//		}
//		
//		public string TypeIdentifier { get {
//				return _instance.dataset().relationName();
//			}
//		}
//		
//		#endregion

	
		private Attribute<Type> GetAttributeType(weka.core.Attribute attribute)
		{
			if (attribute.isString ()) {
				return new Attribute<Type>(
								attribute.name(), typeof(string));						
			} else if (attribute.isNumeric ()) {
				return new Attribute<Type>(
								attribute.name(), typeof(double));												
			} else if (attribute.isNominal ()) {
				return new Attribute<Type>(
								attribute.name(),
								_repository.GetEnumValue(attribute,
									(int)_instance.value(attribute)).GetType());	
			} else {
				throw new InvalidOperationException (
								"Unexpected Weka attribute type.");
			}		
		}
			
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

