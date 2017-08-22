using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalloonSetter : MonoBehaviour {

	static int index = 0;
	public GameObject Blue;
	public GameObject Red;
	public GameObject Green;
	public GameObject Yellow;
	public Dictionary<int, GameObject> Balloons;
	public DougieBehaviour Dougie;
	public PlayerId Id;

	void PopulateDictionary ()
	{
		Balloons = new Dictionary<int, GameObject> ();
		Balloons.Add (0, Blue);
		Balloons.Add (1, Red);
		Balloons.Add (2, Green);
		Balloons.Add (3, Yellow);
	}

	void Start () {
		PopulateDictionary ();
	}

	void Update() {

		if (Dougie.Id == null || Id.Value == null || Dougie.Balloons != null)
			return;

		Vector3 position = new Vector3 (Dougie.transform.position.x - 0.045f, Dougie.transform.position.y + 0.45f, 0);
		GameObject instance = Instantiate (GetBalloonsPrefab(), position, Quaternion.identity);
		instance.gameObject.transform.parent = Dougie.transform;
		Dougie.Balloons = instance;
		Dougie.BalloonsAnimator = instance.GetComponent<Animator> ();
	}

	public GameObject GetBalloonsPrefab() {
		return Balloons[index++ % 4];
	}
}
