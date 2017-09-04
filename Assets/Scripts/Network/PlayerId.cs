using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerId : NetworkBehaviour {

	[SyncVar] 
	private string _syncValue;
	public string Value = "";
	private NetworkInstanceId _id;

	void Update () {

		if (Value != null && Value.Length > 0)
			return;

		if (isLocalPlayer) {
			Value = _id.ToString ();
			CmdTellId (Value);
		} else 
			Value = _syncValue;		
	}

	[Command]
	void CmdTellId (string id)
	{
		_syncValue = id;
	}

	[Client]
	void SetNetworkId ()
	{
		_id = GetComponent < NetworkIdentity> ().netId;
		CmdTellId(_id.ToString()); 
	}

	public override void OnStartLocalPlayer() {
		SetNetworkId ();	 
	}
}

