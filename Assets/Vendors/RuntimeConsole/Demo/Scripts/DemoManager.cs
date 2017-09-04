using RTC.Converter;
using RTC.Core;
using RTC.Core.Extension;
using RTC.Interface;
using UnityEngine;

namespace RTC.Demo
{
	public class DemoManager : MonoBehaviour
	{
		private void Start()
		{
			AddTypeConverter(new Vector3Converter());
			AddTypeConverter(new Vector2Converter());

			// An array converter already exists for those types
#if !UNITY_WEBGL
			RTConsole.Instance.AddType(new ListConverter(new IntConverter()));
			RTConsole.Instance.AddType(new ListConverter(new FloatConverter()));
			RTConsole.Instance.AddType(new ListConverter(new DoubleConverter()));
			RTConsole.Instance.AddType(new ListConverter(new CharConverter()));
			RTConsole.Instance.AddType(new ListConverter(new StringConverter()));
#endif
			// Handles exceptions thrown by the commands
			RTConsole.Instance.HandleExceptions = true;

			// Adds commands to the console
			RTConsole.Instance.Types = new DemoCommandsProvider();

			// Executes a command
			RTConsole.Instance.Execute("RTC.Commands");
		}

		private void AddTypeConverter(ITypeConverter type)
		{
			// Adds a new type which will be recognized by the console when executing commands
			RTConsole.Instance.AddType(type);
			// Creates an array type for that type
			RTConsole.Instance.AddType(new ArrayConverter(type));

			// To avoid problems with generic types in webgl
#if !UNITY_WEBGL
			RTConsole.Instance.AddType(new ListConverter(type));
#endif
		}
	}
}
