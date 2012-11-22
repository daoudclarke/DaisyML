using System;
using System.Collections.Generic;
using System.Linq;


namespace DaisyML
{
	/// <summary>
	/// This class allows the easy creation of a piece of data to train a
	/// classifier, or to be classified by a model.
	/// 
	/// To use this class, create a derived class and give some fields the
	/// custom attribute [Feature]. These fields will then be used to train
	/// the model.
	/// </summary>
	public class Instance : IInstance
	{
		#region IInstance implementation
		public IEnumerable<IAttribute<string>> StringFeatures {
			get {
				return GetValues<string>(typeof(Feature),
					x => x == typeof(string),
					x => (string)x);
			}
		}
	
		public IEnumerable<IAttribute<double>> NumericFeatures {
			get {
				return GetValues<double> (typeof(Feature),
					x => x == typeof(double) ||
						 x == typeof(int),
					x => Convert.ToDouble(x));
			}
		}
	
		public IEnumerable<IAttribute<Enum>> NominalFeatures {
			get {
				return GetValues<Enum> (typeof(Feature),
					x => x.IsEnum,
					x => (Enum)x);
			}
		}
	
		public IEnumerable<IAttribute<System.Type>> MissingFeatures {
			get {
				return GetMissingFeatures(typeof(Feature));
			}
		}
	
		public IEnumerable<IAttribute<double>> NumericTargets {
			get {
				return GetValues<double> (typeof(Target),
					x => x == typeof(double) ||
						 x == typeof(int),
					x => Convert.ToDouble(x));
			}
		}
	
		public IEnumerable<IAttribute<Enum>> NominalTargets {
			get {
				return GetValues<Enum> (typeof(Target),
					x => x.IsEnum,
					x => (Enum)x);
			}
		}
	
		public IEnumerable<IAttribute<System.Type>> MissingTargets {
			get {
				return GetMissingFeatures (typeof(Target));
			}
		}

		public void SetTarget (string name, double targetValue)
		{
			SetTargetInternal(name, targetValue);
		}
	
		public void SetTarget (string name, Enum targetValue)
		{
			SetTargetInternal (name, targetValue);
		}
	
		public string TypeIdentifier {
			get {
				return this.GetType().FullName;
			}
		}	
	
		#endregion

		
		
//		public IEnumerable<KeyValuePair<string, object>> Features {
//			get {
//				return GetValues(typeof(Feature));
//			}
//		}
//		
//		
//		public IEnumerable<KeyValuePair<string, object>> Targets {
//			get {
//				return GetValues(typeof(Target));
//			}
//		}
//
//		public void SetTarget(string targetName, object targetValue) {
//			var type = this.GetType();
//			var fields = type.GetFields();
//			foreach (var field in fields) {
//				if (field.Name == targetName) {
//					var attributes = field.GetCustomAttributes(true);
//					if (attributes.Select(x => x.GetType() == typeof(Target)).Count() > 0) {
//						if (targetValue.GetType() != field.FieldType) {
//							throw new InvalidOperationException(
//							  "Attempt to set target with the wrong type.");
//						}
//						field.SetValue(this, targetValue);
//					}
//				}
//			}			
//		}
//		
//		public string TypeIdentifier { get {
//				return this.GetType().FullName;
//			}
//		}
//		
//		public object GetValue(string attributeName) {
//			var type = this.GetType();
//			return type.GetField(attributeName).GetValue(this);
//		}
//
		private IEnumerable<IAttribute<T>> GetValues<T>
			(Type featureOrTarget, Func<Type, bool> selector,
			Func<object, T> converter)
		{
			var type = this.GetType();
			var fields = type.GetFields()
				.Where(x => selector(x.FieldType))
				.OrderBy(x => x.Name);
			foreach (var field in fields) {
				var attributes = field.GetCustomAttributes(true);
				if (attributes.Where(x => x.GetType() == featureOrTarget).Count() > 0) {
					yield return new Attribute<T>(field.Name, converter(field.GetValue(this)));
				}
			}
		}
		
		private IEnumerable<IAttribute<Type>> GetMissingFeatures(
			Type featureOrTarget)
		{
			var type = this.GetType ();
			var fields = type.GetFields().OrderBy(x => x.Name);
			foreach (var field in fields) {
				var attributes = field.GetCustomAttributes (true);
				if (attributes.Where (x => x.GetType () == featureOrTarget).Count () > 0) {
					if (field.GetValue(this) == null) {
						yield return new Attribute<Type>(field.Name, field.FieldType);
					}
				}
			}			
		}
		
        public void SetTargetInternal(string targetName, object targetValue) {
	        var type = this.GetType();
			var field = type.GetField(targetName);
            if (field == null ||
            	field.GetCustomAttributes(true)
            		 .Select(x => x.GetType() == typeof(Target)).Count() == 0) {
                throw new InvalidOperationException(
                    "Could not find a target of the specified name.");
            }
			
			if (targetValue.GetType() != field.FieldType) {
                throw new InvalidOperationException(
                  "Attempt to set target with the wrong type.");
            }
            
            field.SetValue(this, targetValue);
        }                       
	}
}

