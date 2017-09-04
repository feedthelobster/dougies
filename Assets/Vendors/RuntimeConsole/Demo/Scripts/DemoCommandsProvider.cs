using RTC.Core.Extension;
using RTC.Interface;
using System;

namespace RTC.Demo
{
	public class DemoCommandsProvider : ITypesProvider
	{
		public Type[] GetTypes()
		{
			return typeof(DemoCommands).ToOneItemArray();
		}
	}
}
