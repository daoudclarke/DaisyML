using System;
using System.Collections.Generic;
using System.Linq;

namespace DaisyML
{
	public class InstanceComparer : IEqualityComparer<IInstance>
	{
		#region IEqualityComparer[DaisyML.IInstance] implementation
		public bool Equals(IInstance input, IInstance other)
		{
			return input.Equals(other);
		}
	
		public int GetHashCode (IInstance input)
		{
			throw new NotImplementedException();
		}
		#endregion	
	}
}

