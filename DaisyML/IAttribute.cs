using System;

namespace DaisyML
{
	public interface IAttribute<out T>
	{
		string Name {get;}
		T Value {get;}
	}
}

