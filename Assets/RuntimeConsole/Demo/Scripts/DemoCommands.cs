using System;
using RTC.Base;
using RTC.Core;
using RTC.Core.Extension;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;

namespace RTC.Demo
{
	public static class DemoCommands
	{
		[Command(Alias = "add", Description = "Adds an int to a float.", Usage = "add int float")]
		public static void Add(int x, float y)
		{
			RTConsole.Instance.LogType(DemoLogTypes.None, (x + y).ToString());
		}

		[Command(Alias = "default", Description = "Adds an int to a float.", Usage = "add int float")]
		public static void Default(bool b = false)
		{
			RTConsole.Instance.LogType(DemoLogTypes.None, b.ToString());
		}

		[Command(Alias = "log", Description = "Logs a string to the console.", Usage = "log \"string\"")]
		public static void Log(string item)
		{
			RTConsole.Instance.LogType(DemoLogTypes.None, item);
		}

		[Command(Alias = "intlist", Description = "Prints a list of integers.", Usage = "print [int int ..]")]
		public static void IntList(List<int> ints)
		{
			RTConsole.Instance.LogType(DemoLogTypes.None, string.Join(" ", ints.Select(i => i.ToString()).ToArray()));
		}

		[Command(Alias = "chararray", Description = "Prints an array of characters", Usage = "chararray ['char' 'char' ..]")]
		public static void CharArray(char[] arr)
		{
			RTConsole.Instance.LogType(DemoLogTypes.None, string.Join(" ", arr.Select(c => c.ToString()).ToArray()));
		}

		[Command(Alias = "vector", Description = "Prints a vector", Usage = "vector {x y z}")]
		public static void PrintVector(Vector3 vector)
		{
			RTConsole.Instance.LogType(DemoLogTypes.None, vector.ToString());
		}

		[Command(Alias = "vector2list", Description = "Prints a list of vectors.", Usage = "vector2list [{x y} {x y} ..]")]
		public static void VectorList(List<Vector2> list)
		{
			RTConsole.Instance.LogType(DemoLogTypes.None, string.Join(" ", list.Select(v => v.ToString()).ToArray()));
		}

		[Command(Alias = "exception", Description = "Throws a DivisionByZeroException", Usage = "exception")]
		public static void Exception()
		{
			throw new DivideByZeroException();
		}

		[Command(Alias = "warning", Description = "Prints a warning", Usage = "warning \"string\"")]
		public static void Warning(string warning)
		{
			Debug.LogWarning(warning);
		}

		[Command(Alias = "quit", Description = "Quits the application.", Usage = "quit")]
		public static void Quit()
		{
			Application.Quit();
		}

		[Command(Description = "A command without alias.", Usage = "TestCommands.Test")]
		public static void Test()
		{
			RTConsole.Instance.LogException("Test exception.");
		}

		[Command]
		public static void Empty()
		{
			RTConsole.Instance.LogWarning("No alias, no description, no usage !");
		}
	}
}