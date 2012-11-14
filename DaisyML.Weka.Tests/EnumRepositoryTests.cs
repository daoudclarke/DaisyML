using System;
using System.Linq;
using System.Collections.Generic;

using NUnit.Framework;

using DaisyML;
using DaisyML.Weka;

namespace DaisyML.Weka.Tests
{
	[TestFixture]
	public class EnumRepositoryTests
	{
		[Test]
		public void TestCreationOfEnums()
		{
			// Arrange
			var values = new weka.core.FastVector();
			values.addElement("beech");
			values.addElement("oak");
			values.addElement("ash");
			
			var attribute = new weka.core.Attribute("trees", values);
			var repo = new EnumRepository();
			
			// Act
			var enumValue = repo.GetEnumValue(attribute, 0);
			
			// Assert
			var fields = enumValue.GetType().GetFields();
			Assert.AreEqual(3, fields.Length);
			Assert.AreEqual("beech", fields[0]);
		}
	}
}

