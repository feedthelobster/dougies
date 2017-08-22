using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;

public class DougieBehaviour : NetworkBehaviour {

	public Quaternion SpriteRot;
	public Rigidbody2D rigidbody;
	public Transform transform;
	public Collider2D feet;
	private DougieAttributes attributes;
	private DougieStates states;
	public GameObject Balloons;
	public Animator BalloonsAnimator;
	public GameObject taco;
	public Vector3 position;
	public PlayerId Id;
	private NetworkIdentity _networkIdentity;

	void Awake(){
		transform = GetComponent<Transform>();
		rigidbody = GetComponent<Rigidbody2D>();
	}

	void Start () {
		Id = GetComponent<PlayerId> ();
		states = GetComponent<DougieStates>();
		attributes= GetComponent<DougieAttributes>();
		rigidbody = GetComponent<Rigidbody2D>();
		_networkIdentity = GetComponent<NetworkIdentity> ();
	}

	void Shoot ()
	{
		if(Time.time < attributes.nextTacoShot)
			return;
		
		float horizontalOffset =  0.77f;
		float speed = attributes.horizontalSpeedTaco;

		if (states.goingLeft) {
			horizontalOffset *= -1;
			speed *= -1;
		}

		Vector3 offset = transform.position + new Vector3 (horizontalOffset, 0, 0);
		Vector2 velocity = new Vector2 (speed, 0);

		CmdShoot (offset, velocity, Id.Value);
		attributes.nextTacoShot = Time.time + attributes.tacoFireRate;
		states.shooting = false;
	}

	[Command]
	void CmdShoot(Vector3 position, Vector2 velocity, string id) {
		var gameObject = Instantiate (taco, position, Quaternion.identity);
		gameObject.GetComponent<Rigidbody2D> ().velocity = new Vector2(velocity.x, velocity.y);
		var tacoBehaviour = gameObject.GetComponent<TacoBehaviour> ();
		tacoBehaviour._ownerId = id;
		NetworkServer.SpawnWithClientAuthority(gameObject, connectionToClient);
	}

	void UpdateAnimation ()
	{
		if (BalloonsAnimator != null && BalloonsAnimator.GetInteger ("Hp") != attributes.Hp)
			BalloonsAnimator.SetInteger ("Hp", attributes.Hp);
	}		

	void FixedUpdate() {
		
		if (!isLocalPlayer)
			return;
		
		Jump();
		Move();
	}

	void Update () {

		UpdateAnimation ();

		if (!isLocalPlayer)
			return;

		Flip();

		if (states.shooting) 
			Shoot ();
		position = transform.position;
	}

	Vector2 GetUpwardForce ()
	{
		float y = rigidbody.velocity.y;

		if (y < 0)
			y = 0;
		
		float delta = attributes.verticalSpeedLimit - y;
		return new Vector2 (0, attributes.floatingBaseForce.y + (delta / 5) * 2);
	}

	void SetVerticalForce ()
	{
		
		rigidbody.AddForce (GetUpwardForce());
		if (rigidbody.velocity.y >= attributes.verticalSpeedLimit)
			rigidbody.velocity = new Vector2(rigidbody.velocity.x, attributes.verticalSpeedLimit);
	}

	void Jump(){
		if (!states.goingUp)
			return;
		
		SetVerticalForce ();		
	}

	void LimitFallingForce ()
	{
		float fallingForceLimit = attributes.verticalSpeedLimit * -2f;
		if (rigidbody.velocity.y <= fallingForceLimit)
			rigidbody.velocity = new Vector2 (rigidbody.velocity.x, fallingForceLimit);
	}

	void Stop ()
	{
		if (Mathf.Abs (rigidbody.velocity.x) > 0.25f)
			rigidbody.AddForce (new Vector2 (rigidbody.velocity.x * -1.25f, 0));
		else
			rigidbody.velocity = new Vector2 (0, rigidbody.velocity.y);
	}

	 void ChangeDirection ()
	{
		if (states.left && rigidbody.velocity.x > 3 || states.right && rigidbody.velocity.x < -3) 
			rigidbody.velocity = new Vector2 (rigidbody.velocity.x * 0.95f, rigidbody.velocity.y);
	}

	void SetHorizontalForce()
	{
		if (states.isMovingHorizontally()) {
			ChangeDirection ();

			float x = states.left ? attributes.horizontalMovingForce * -1 : attributes.horizontalMovingForce;
			rigidbody.AddForce (new Vector2(x, 0));

			if (Mathf.Abs (rigidbody.velocity.x) > attributes.horizontalSpeedLimit) {
				float speed = rigidbody.velocity.x > 0 ? attributes.horizontalSpeedLimit : attributes.horizontalSpeedLimit * -1;
				rigidbody.velocity = new Vector2 (speed, rigidbody.velocity.y);
			}
		}
		else 
			Stop (); 
		
	}

	void Move(){
		SetHorizontalForce ();
		LimitFallingForce();
	}

	void Flip(){
		if(states.goingLeft)
			transform.rotation = Quaternion.Euler(0,180, 0);
		else
			transform.rotation = Quaternion.Euler(0,0, 0);
	}

	void OnTriggerEnter2D(Collider2D collision) {
		var gameObject = collision.gameObject;

		if (collision.gameObject.tag == "Taco") {
			var id = gameObject.GetComponent<TacoBehaviour> ()._ownerId;

			if(id != _networkIdentity.netId.ToString())
				attributes.Hp -= 1;

			if (attributes.Hp < 1)
				Destroy (this.gameObject);
		}
    }

}
