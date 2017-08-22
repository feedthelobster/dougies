using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;

public class Explosion : NetworkBehaviour {

	public Animator animator;

	void Start () {
		animator = GetComponent<Animator> ();
	}
	
	void Update () {
		
		if (this.animator.GetCurrentAnimatorStateInfo (0).IsName ("Ended")) 
			Destroy (gameObject);
	}
}
