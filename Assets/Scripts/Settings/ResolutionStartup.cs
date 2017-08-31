using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResolutionStartup : MonoBehaviour {

	// Use this for initialization
	void Start () {
		 Screen.SetResolution(640, 480, false);
		Resolution max=Screen.currentResolution;
		 Screen.SetResolution(max.width, max.height, true);
	}
	
}
