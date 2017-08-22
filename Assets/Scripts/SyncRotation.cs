using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SyncRotation : NetworkBehaviour {

	[SyncVar]
	private float rotation;

	void Start () {
		
	}

	void Update () {
		if (isLocalPlayer)
			CmdSync (transform.rotation.y);
		else
			Sync ();
	}

	[Command]
	void CmdSync (float y)
	{
		rotation = y;
	}

	void Sync ()
	{
		transform.rotation = Quaternion.Euler(transform.rotation.x, rotation == 1 ? 180 : 0, 0);
	}
}
