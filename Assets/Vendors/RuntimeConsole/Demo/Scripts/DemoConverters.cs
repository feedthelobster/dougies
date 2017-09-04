using RTC.Interface;
using System;
using System.Linq;
using UnityEngine;

namespace RTC.Demo
{
	public class Vector3Converter : ITypeConverter
	{
		public string Regex
		{
			get
			{
				return @"{(\s*-?[0-9]+(\.[0-9]+)?\s*){3}}";
			}
		}

		public Type Type
		{
			get
			{
				return typeof(Vector3);
			}
		}

		public object ConvertFrom(string match)
		{
			var m = System.Text.RegularExpressions.Regex.Match(match, @"(\s*-?[0-9]+(\.[0-9]+)?\s*){3}");
			var values = System.Text.RegularExpressions.Regex.Split(m.Value, @"\s+")
				.Where(s => !string.IsNullOrEmpty(s))
				.Select(s => Convert.ToSingle(s))
				.ToArray();
			return new Vector3(values[0], values[1], values[2]);
		}
	}

	public class Vector2Converter : ITypeConverter
	{
		public string Regex
		{
			get
			{
				return @"{(\s*-?[0-9]+(\.[0-9]+)?\s*){2}}";
			}
		}

		public Type Type
		{
			get
			{
				return typeof(Vector2);
			}
		}

		public object ConvertFrom(string regexMatch)
		{
			var m = System.Text.RegularExpressions.Regex.Match(regexMatch, @"(\s*-?[0-9]+(\.[0-9]+)?\s*){2}");
			var values = System.Text.RegularExpressions.Regex.Split(m.Value, @"\s+")
				.Where(s => !string.IsNullOrEmpty(s))
				.Select(s => Convert.ToSingle(s))
				.ToArray();
			return new Vector2(values[0], values[1]);
		}
	}
}
