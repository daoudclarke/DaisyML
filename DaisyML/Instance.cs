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
		#region IEnumerable implementation
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator ()
		{
			throw new NotImplementedException ();
		}
		#endregion

		#region IEnumerable[System.Collections.Generic.KeyValuePair[System.String,System.Object]] implementation
		public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
		{
			IDictionary<string, object> values =
				new SortedDictionary<string, object>();
			var type = this.GetType();
			var fields = type.GetFields();
			foreach (var field in fields) {
				var attributes = field.GetCustomAttributes(true);
				if (attributes.Select(x => x.GetType() == typeof(Feature)).Count() > 0) {
					values[field.Name] = field.GetValue(this);
				}
			}
			return values.GetEnumerator();
		}
		#endregion
	}
}

