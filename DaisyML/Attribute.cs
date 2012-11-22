using System;
using System.Collections.Generic;

namespace DaisyML
{
	public struct Attribute<T> : IAttribute<T>
	{
		public string Name {get; private set;}
		public T Value {get; private set;}	
	
		public Attribute(string name, T attributeValue) : this()
		{
			Name = name;
			Value = attributeValue;
		}
	}

//	public struct Attribute<T>
//	{
//		public string Name { get; private set; }
//		public T Value { get; private set; }
//
//		public Attribute(string name, T attributeValue)
//		{
//					
//			Name = name;
//			Value = attributeValue;
//		}
//	}

//	public interface IAttribute<T> : IEnumerable<KeyValuePair<string, T>> {}
}

