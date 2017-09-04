using RTC.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using RTC.Core;
using RTC.Core.Extension;

namespace RTC
{
	public class CommandFilter
	{
		#region Variables
		private IEnumerable<CommandInfo> cachedCommands;
		private string lastPrefix = string.Empty;
		#endregion

		#region Methods
		public CommandFilter()
		{
			cachedCommands = new CommandInfo[0];
		}

		public IEnumerable<CommandInfo> FilterByName(string prefix)
		{
			if(prefix.IsNullOrWhiteSpace())
			{
				return new CommandInfo[0];
			}

			// If we delete a character
			if(lastPrefix.Length > prefix.Length)
			{
				lastPrefix = prefix;
				cachedCommands = RTConsole.Instance.Commands;
				return cachedCommands.Filter((CommandInfo info) => 
				{
					return StartsWithIgnoreCase(info.Alias, prefix);
				});
			}
			lastPrefix = prefix;
			if (cachedCommands.Count() > 0)
			{
				cachedCommands = cachedCommands.Filter((CommandInfo info) => 
				{
					return StartsWithIgnoreCase(info.Alias, prefix);
				});
			}
			else
			{
				var newCommands = RTConsole.Instance.Commands;
				cachedCommands = newCommands.Filter((CommandInfo info) => 
				{
					return StartsWithIgnoreCase(info.Alias, prefix);
				});
			}
			return cachedCommands;
		}

		private bool StartsWithIgnoreCase(string str, string prefix)
		{
			return str.StartsWith(prefix, StringComparison.OrdinalIgnoreCase);
		}
		#endregion
	}
}
