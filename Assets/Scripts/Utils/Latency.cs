using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Latency : MonoBehaviour {

	private NetworkManager _nm;

	void Start () {
		_nm = GameObject.FindObjectOfType<NetworkManager> ();
	}
	
	public int GetRTT() {
		if (_nm == null || _nm.client == null)
			return -1;
		return _nm.client.GetRTT ();
	}
}
