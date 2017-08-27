using System.Collections;
using System.Collections.Generic;
using RTC.Core.Extension;
using RTC.Interface;

public class CommandProvider : ITypesProvider {
	#region ITypesProvider implementation
	public System.Type[] GetTypes ()
	{
		return typeof(Commands).ToOneItemArray ();
	}
	#endregion
	

}
