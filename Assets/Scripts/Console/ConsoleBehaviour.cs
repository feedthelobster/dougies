using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTC.Core;

public class ConsoleBehaviour : MonoBehaviour {

	Latency _latency;

	void Start () {
		RTConsole.Instance.Types = new CommandProvider ();		 
		Commands.Instance = this;

		_latency = GameObject.Find ("Latency").GetComponent<Latency> ();
	}

	public void Ping() {
		for (int i = 0; i < 5; i++) 
			Invoke ("LogRTT", i);
	}

	void LogRTT() {
		Debug.Log (_latency.GetRTT() + "ms");
	}
}
