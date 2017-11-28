using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using NATTraversal;
public class CustomNetworkManager : NATTraversal.NetworkManager {

	public List<MatchInfoSnapshot> MatchList;
	public bool FetchingMatches;

	public void Host (string name) {
		StartHostAll (name, matchSize);
	}

	public void Join () {
		matchMaker.ListMatches(0, 10, "", true, 0, 0, OnMatchList);		
	}

	public IEnumerator FetchMatches () {
		FetchingMatches = true;
		MatchList = null;
		matchMaker.ListMatches(0, 10, "", true, 0, 0, OnMatchListFetched);		

		while (FetchingMatches)
			yield return new WaitForSeconds (1);
	}

	public void OnMatchListFetched (bool success, string extendedInfo, List<MatchInfoSnapshot> matchList)
	{
		FetchingMatches = false;

		if (!success) 
			return;
		
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
		}
	}
}
