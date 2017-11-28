using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTC.Base;

public class Commands {

	public static ConsoleBehaviour Instance;

	[Command(Alias = "ping", Usage = "ping", Description = "Logs latency with server")]
	public static void Ping() {
		Instance.Ping ();
	}

	[Command(Alias = "lmatch", Usage = "matches", Description = "Lists matches available")]
	public static void ListMatches() {
		Instance.ListMatches ();
	}

	[Command(Alias = "join", Description = "Join a match by name", Usage = "join \"name\"")]
	public static void Join(string name)
	{
		Instance.JoinMatch (name);
	}

	[Command(Alias = "create", Description = "create a match", Usage = "create \"name\"")]
	public static void Create(string name)
	{
		Instance.CreateMatch (name);
	}

}
