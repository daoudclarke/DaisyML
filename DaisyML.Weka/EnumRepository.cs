using System;
using System.Reflection;
using System.Reflection.Emit;

namespace DaisyML.Weka
{
	public class EnumRepository
	{
		private ModuleBuilder _builder;
		
		public EnumRepository ()
		{
			// Get the current application domain for the current thread
			AppDomain currentDomain = AppDomain.CurrentDomain;
			
			// Create a dynamic assembly in the current application domain,
			// and allow it to be executed and saved to disk.
			AssemblyName name = new AssemblyName("MyEnums");
			AssemblyBuilder assemblyBuilder = currentDomain.DefineDynamicAssembly(name,
			                                      AssemblyBuilderAccess.RunAndSave);
			
			// Define a dynamic module in "MyEnums" assembly.
			// For a single-module assembly, the module has the same name as the assembly.
			_builder = assemblyBuilder.DefineDynamicModule(name.Name,
			                                  name.Name + ".dll");
			
		}
		
		public object GetEnumValue(weka.core.Attribute attribute,
			                       int enumValue)
		{
			var enumType = GetEnumType(attribute);
			return Enum.ToObject(enumType, enumValue);
		}
		
		private Type GetEnumType(weka.core.Attribute attribute)
		{
			var attributeName = attribute.name();
			var enumType =_builder.Assembly.GetType(attributeName);
			if (enumType != null) {
				return enumType;
			}

			// Define a public enumeration with the name "MyEnum" and an underlying type of Integer.
			EnumBuilder myEnum = _builder.DefineEnum (attributeName,
			                         TypeAttributes.Public, typeof(int));
						
			for (int i=0; i<attribute.numValues(); ++i) {
				var name = attribute.value(i);
				myEnum.DefineLiteral (name, i);
			}
			
			// Create the enum
			return myEnum.CreateType ();
		}
	}
}

