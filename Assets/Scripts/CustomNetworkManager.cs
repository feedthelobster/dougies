using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using NATTraversal;
public class CustomNetworkManager : NATTraversal.NetworkManager {

	[SerializeField]
	GameObject _canvas;

	private void HideCanvas() {
		_canvas.SetActive (false);
	}

	public void Host () {
		StartHostAll ("default", matchSize);
		HideCanvas ();
	}

	public void Join () {
		matchMaker.ListMatches(0, 10, "", true, 0, 0, OnMatchList);		
	}

	public override void OnMatchList (bool success, string extendedInfo, List<MatchInfoSnapshot> matchList)
	{
		if (success && matchList.Count > 0) {
			var match = matchList [0];
			StartClientAll (match);	
			HideCanvas ();
		}
	}
}
