using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TacoBehaviour : MonoBehaviour {
	public	float start = 0f;
	public float lifespan = 0.25f;
	public GameObject explosion;
	public float despawnTime;
	public string OwnerId;

	void Start () {
		start = Time.time;
		Invoke ("Despawn", despawnTime);
	}

	void Despawn() {
		var position = transform.position;
		var explosionInstance = Instantiate (explosion, position, Quaternion.identity);
		Destroy (gameObject);
	}

	bool IsOwner (GameObject gameObject)
	{
		if (gameObject.tag == "Player") {
			var id = gameObject.GetComponent<PlayerId> ().Value;

			if (id == OwnerId)
				return true;
		}
		return false;
	}

	void OnTriggerEnter2D(Collider2D collision) {

		var gameObject = collision.gameObject;
		if(!IsOwner (gameObject))
			Despawn ();
	}
}
