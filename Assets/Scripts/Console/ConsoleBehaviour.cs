using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTC.Core;
using UnityEngine.Networking.Match;
using System.Linq;

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
		
	public void JoinMatch(string name) {
		StartCoroutine (JoinMatchRoutine(name));
	}

	public IEnumerator JoinMatchRoutine(string name) {

		yield return StartCoroutine (_cm.FetchMatches ());

		if (_cm.MatchList == null) {
			Debug.Log ("Error Joining Match");
			yield break;
		}

		var match = _cm.MatchList.FirstOrDefault (x => GetMatchName (x) == name);

		if(match == null) {
			Debug.Log ("Match not found");
			yield break;
		}

		_cm.Join (match);
	}

	public void CreateMatch(string name) {
		StartCoroutine (CreateMatchRoutine(name));	
	}

	public IEnumerator CreateMatchRoutine(string name) {
		yield return StartCoroutine(_cm.FetchMatches ());

		var filtered = _cm.MatchList
			.Where (x => GetMatchName (x) == name).ToList();

		if(filtered.Count > 0)
			name += "(" + filtered.Count + ")";

		_cm.Host (name);
	}

	private string GetMatchName (MatchInfoSnapshot match)
	{
		return match.name.Split ('|') [0];
	}

	public void ListMatches() {
		StartCoroutine (ListMatchesRoutine());
	}

	public IEnumerator ListMatchesRoutine() {
		Debug.Log ("Fetching Matches...");

		yield return StartCoroutine (_cm.FetchMatches ());

		if (_cm.MatchList == null) {
			Debug.Log ("Error Fetching Matches");
			yield break;
		}

		if (_cm.MatchList.Count < 1) {
			Debug.Log ("No matches found");
			yield break;
		}

		Delimiter ();
		foreach (MatchInfoSnapshot match in _cm.MatchList) 
			Debug.Log (GetMatchName(match));
		Delimiter ();
	}

	void Delimiter() {
		Debug.Log ("----------");
	}

	void LogRTT() {
		Debug.Log (_latency.GetRTT() + "ms");
	}
}
