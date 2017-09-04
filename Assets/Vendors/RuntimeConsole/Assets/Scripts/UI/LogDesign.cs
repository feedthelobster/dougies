using System;
using UnityEngine;

namespace RTC.UI
{
	[Serializable]
	public class LogTypeDesign
	{
		public LogTypeDesign()
		{
			TitleColor = Color.white;
		}

		public string LogType;
		public Color TitleColor;
		public Sprite Icon;
	}
}
