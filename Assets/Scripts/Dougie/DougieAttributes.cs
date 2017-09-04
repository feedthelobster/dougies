using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class DougieAttributes : NetworkBehaviour {

	[SyncVar]
	private int _syncHp;

	public int Hp = 3;
	public Vector2 floatingBaseForce;
  	public float verticalSpeedLimit = 5f,
				 horizontalSpeedLimit = 6f,
                 horizontalSpeed = 4f,
                 horizontalSpeedTaco = 9f,
                 tacoFireRate = 0.8f,
				 horizontalStoppingForce = 0.5f,
				 horizontalMovingForce = 5f,
  	             nextTacoShot = 0;

	void Start () {
		floatingBaseForce = new Vector2(0, 20f);
	}

	void Update() {
		if (isLocalPlayer)
			CmdSyncHp (Hp);
		else
			SyncHp();
	}

	[Command]
	public void CmdSyncHp(int hp){
		_syncHp = hp;
	} 

	public void SyncHp() {
		if (isLocalPlayer)
			CmdSyncHp (Hp);
		else
			Hp = _syncHp;
	}

}
