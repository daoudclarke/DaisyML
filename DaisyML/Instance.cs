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
		public IEnumerable<KeyValuePair<string, object>> Features {
			get {
				return GetValues(typeof(Feature));
			}
		}
		
		
		public IEnumerable<KeyValuePair<string, object>> Targets {
			get {
				return GetValues(typeof(Target));
			}
		}

		public void SetTarget(string targetName, object targetValue) {
			var type = this.GetType();
			var fields = type.GetFields();
			foreach (var field in fields) {
				if (field.Name == targetName) {
					var attributes = field.GetCustomAttributes(true);
					if (attributes.Select(x => x.GetType() == typeof(Target)).Count() > 0) {
						if (targetValue.GetType() != field.FieldType) {
							throw new InvalidOperationException(
							  "Attempt to set target with the wrong type.");
						}
						field.SetValue(this, targetValue);
					}
				}
			}			
		}
		
		public string TypeIdentifier { get {
				return this.GetType().Name;
			}
		}
		
		#endregion
		private IEnumerable<KeyValuePair<string, object>> GetValues
			(Type attribute)
		{
			var values = new SortedDictionary<string, object>();
			var type = this.GetType();
			var fields = type.GetFields();
			foreach (var field in fields) {
				var attributes = field.GetCustomAttributes(true);
				if (attributes.Where(x => x.GetType() == attribute).Count() > 0) {
					values[field.Name] = field.GetValue(this);
				}
			}
			return values;
		}
	}
}

