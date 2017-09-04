using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using NATTraversal;
public class CustomNetworkManager : NATTraversal.NetworkManager {

	[SerializeField]
	GameObject _connectionPanel;
	public List<MatchInfoSnapshot> MatchList;
	public bool FetchingMatches;

	private void HideConnectionPanel() {
		_connectionPanel.SetActive (false);
	}

	public void Host () {
		StartHostAll ("default", matchSize);
		HideConnectionPanel ();
	}

	public void Join () {
		matchMaker.ListMatches(0, 10, "", true, 0, 0, OnMatchList);		
	}

	public void FetchMatches () {
		FetchingMatches = true;
		matchMaker.ListMatches(0, 10, "", true, 0, 0, OnMatchListFetched);		
	}

	public void OnMatchListFetched (bool success, string extendedInfo, List<MatchInfoSnapshot> matchList)
	{
		if (!success)
			return;

		FetchingMatches = false;
		MatchList = matchList;
	}

	public override void OnMatchList (bool success, string extendedInfo, List<MatchInfoSnapshot> matchList)
	{
		if (!success)
			return;

		OnMatchListFetched (success, extendedInfo, matchList);

		if (MatchList.Count > 0) {
			var match = MatchList [0];
			StartClientAll (match);	
			HideConnectionPanel ();
		}
	}
}
