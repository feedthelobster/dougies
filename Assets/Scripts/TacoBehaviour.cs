using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TacoBehaviour : NetworkBehaviour {
	public	float start = 0f;
	public float lifespan = 0.25f;
	public GameObject explosion;
	public float despawnTime;

	[SyncVar]
	public string _ownerId;

	void Start () {
		start = Time.time;
		Invoke ("Despawn", despawnTime);
	}

	void Despawn() {
		NetworkServer.Destroy (gameObject);
		var position = this.transform.position;
		var explosionInstance = Instantiate (explosion, position, Quaternion.identity);
	}

	bool IsOwner (GameObject gameObject)
	{
		if (gameObject.tag == "Player") {
			var id = gameObject.GetComponent<PlayerId> ().Value;

			if (id == _ownerId)
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
