﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TacoCollider : MonoBehaviour {
		void OnCollisionEnter2D(Collision2D collision ) {		
			if(collision.gameObject.tag == "Player" || collision.gameObject.tag == "Platform")
			{
				Destroy(gameObject);
			}
	}
}
