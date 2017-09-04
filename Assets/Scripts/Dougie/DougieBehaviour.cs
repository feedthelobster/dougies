using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;

public class DougieBehaviour : NetworkBehaviour {

	public Quaternion SpriteRot;
	public Rigidbody2D Rigidbody;
	public Transform Transform;
	public GameObject Balloons;
	public Animator BalloonsAnimator;
	public GameObject taco;
	public Vector3 position;
	public PlayerId Id;
	private DougieAttributes attributes;
	private DougieStates states;
	private NetworkIdentity _networkIdentity;

	void Awake(){
		Transform = GetComponent<Transform>();
		Rigidbody = GetComponent<Rigidbody2D>();
	}

	void Start () {
		Id = GetComponent<PlayerId> ();
		states = GetComponent<DougieStates>();
		attributes= GetComponent<DougieAttributes>();
		Rigidbody = GetComponent<Rigidbody2D>();
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

		Vector3 offset = Transform.position + new Vector3 (horizontalOffset, 0, 0);
		Vector2 velocity = new Vector2 (speed, 0);

		CmdShoot (offset, velocity, Id.Value);
		attributes.nextTacoShot = Time.time + attributes.tacoFireRate;
		states.shooting = false;

		InstantiateProjectile (offset, velocity, Id.Value);
	}

	[Command]
	void CmdShoot(Vector3 position, Vector2 velocity, string id) {
		RpcShoot (position, velocity, id);
	}

	void InstantiateProjectile(Vector3 position, Vector2 velocity, string id) {
		var gameObject = Instantiate (taco, position, Quaternion.identity);
		gameObject.GetComponent<Rigidbody2D> ().velocity = new Vector2(velocity.x, velocity.y);
		gameObject.transform.rotation = transform.rotation;
		var tacoBehaviour = gameObject.GetComponent<TacoBehaviour> ();
		tacoBehaviour.OwnerId = id;
	}

	[ClientRpc]
	void RpcShoot(Vector3 position, Vector2 velocity, string id) {
		
		if (isLocalPlayer && id == Id.Value) 
			return;
		InstantiateProjectile (position, velocity, id);
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

		if (states.shooting) 
			Shoot ();
	}

	void Update () {

		UpdateAnimation ();

		if (!isLocalPlayer)
			return;

		Flip();
		position = Transform.position;
	}

	Vector2 GetUpwardForce ()
	{
		float y = Rigidbody.velocity.y;

		if (y < 0)
			y = 0;
		
		float delta = attributes.verticalSpeedLimit - y;
		return new Vector2 (0, attributes.floatingBaseForce.y + (delta / 5) * 2);
	}

	void SetVerticalForce ()
	{
		Rigidbody.AddForce (GetUpwardForce());
		if (Rigidbody.velocity.y >= attributes.verticalSpeedLimit)
			Rigidbody.velocity = new Vector2(Rigidbody.velocity.x, attributes.verticalSpeedLimit);
	}

	void Jump(){
		if (!states.goingUp)
			return;
		
		SetVerticalForce ();		
	}

	void LimitFallingForce ()
	{
		float fallingForceLimit = attributes.verticalSpeedLimit * -2f;
		if (Rigidbody.velocity.y <= fallingForceLimit)
			Rigidbody.velocity = new Vector2 (Rigidbody.velocity.x, fallingForceLimit);
	}

	void Stop ()
	{
		if (Mathf.Abs (Rigidbody.velocity.x) > 0.25f)
			Rigidbody.AddForce (new Vector2 (Rigidbody.velocity.x * -1.25f, 0));
		else
			Rigidbody.velocity = new Vector2 (0, Rigidbody.velocity.y);
	}

	 void ChangeDirection ()
	{
		if (states.left && Rigidbody.velocity.x > 3 || states.right && Rigidbody.velocity.x < -3) 
			Rigidbody.velocity = new Vector2 (Rigidbody.velocity.x * 0.95f, Rigidbody.velocity.y);
	}

	void SetHorizontalForce()
	{
		if (states.isMovingHorizontally()) {
			ChangeDirection ();

			float x = states.left ? attributes.horizontalMovingForce * -1 : attributes.horizontalMovingForce;
			Rigidbody.AddForce (new Vector2(x, 0));

			if (Mathf.Abs (Rigidbody.velocity.x) > attributes.horizontalSpeedLimit) {
				float speed = Rigidbody.velocity.x > 0 ? attributes.horizontalSpeedLimit : attributes.horizontalSpeedLimit * -1;
				Rigidbody.velocity = new Vector2 (speed, Rigidbody.velocity.y);
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
			Transform.rotation = Quaternion.Euler(0,180, 0);
		else
			Transform.rotation = Quaternion.Euler(0,0, 0);
	}

	void OnTriggerEnter2D(Collider2D collision) {
		var gameObject = collision.gameObject;

		if (collision.gameObject.tag == "Taco") {

			if (!isServer)
				return;
			
			var id = gameObject.GetComponent<TacoBehaviour> ().OwnerId;

			if (id != _networkIdentity.netId.ToString ())
				RpcTakeDamage (_networkIdentity.netId.ToString ());
		}
    }

	[ClientRpc]
	void RpcTakeDamage(string id)
	{
		if (id != _networkIdentity.netId.ToString ())
			return;
		
		attributes.Hp -= 1;
		if (attributes.Hp < 1)
			Destroy (this.gameObject);
	}
}
