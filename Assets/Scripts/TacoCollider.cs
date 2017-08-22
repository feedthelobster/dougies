using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TacoCollider : NetworkBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

		void OnCollisionEnter2D(Collision2D collision ) {
		//Tacos shouldnt explode on contact with horizontal screen limits.
		
			if(collision.gameObject.tag == "Player"||collision.gameObject.tag == "Platform")
			{
				Destroy(gameObject);
			}
	}
}
