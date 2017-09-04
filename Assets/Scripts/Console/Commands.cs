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

}
