using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTC.Core;
using UnityEngine.Networking.Match;

public class ConsoleBehaviour : MonoBehaviour {

	[SerializeField]
	CustomNetworkManager _cm;
	Latency _latency;

	void Start () {
		RTConsole.Instance.Types = new CommandProvider ();		 
		Commands.Instance = this;

		_latency = GameObject.Find ("Latency").GetComponent<Latency> ();
		_cm = GameObject.Find ("Network Manager").GetComponent<CustomNetworkManager> ();
	}

	public void Ping() {
		for (int i = 0; i < 5; i++) 
			Invoke ("LogRTT", i);
	}

	public void ListMatches() {
		_cm.FetchMatches ();
		Invoke ("ListMatchesRoutine", 1);
	}

	private string GetMatchName (MatchInfoSnapshot match)
	{
		return match.name.Split ('|') [0];
	}

	public void ListMatchesRoutine() {
		Debug.Log ("Fetching Matches...");

		if (_cm.FetchingMatches)
			Invoke ("ListMatchesRoutine", 1);
		else {
			if (_cm.MatchList.Count < 1) {
				Debug.Log ("No matches found");
				return;
			}

			Delimiter ();
			Debug.Log ("Matches found:");
			foreach (MatchInfoSnapshot match in _cm.MatchList) 
				Debug.Log (GetMatchName(match));
			Delimiter ();
		}
	}

	void Delimiter() {
		Debug.Log ("----------");
	}

	void LogRTT() {
		Debug.Log (_latency.GetRTT() + "ms");
	}
}
