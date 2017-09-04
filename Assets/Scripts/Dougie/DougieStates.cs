using UnityEngine.Networking;
using UnityEngine;

public class DougieStates : NetworkBehaviour {
	
	public bool isMovingHorizontally ()
	{
		return left || right;
	}

	public  bool right = false,
				 left = false,
				 goingUp = false,
				 onair = false,
				 shooting = false,
				 goingLeft = false;
}
