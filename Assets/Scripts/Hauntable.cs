using UnityEngine;
using System.Collections;

[RequireComponent( typeof( Rigidbody ) )]
public class Hauntable : MonoBehaviour {

	#region Public Variables
	public MoveType moveType = MoveType.CUBE;

	// Need to discuss what to do with these.
	public int health = 100;
	public bool canJump = true;
	#endregion

	#region Public Methods
	public GameObject Haunt() {
		return this.gameObject;
	}
	#endregion

	public void OnCollisionEnter( Collision collision ) {
		if ( collision.gameObject.layer == 0 ) //default, the world
		{
			if ( Physics.Raycast( transform.position, Vector3.down * 5, 5 ) ) {
				//as long as there's generally something close by to the ground, go to jump again.
				canJump = true;
			}
		}
	}
}

// Enum's are less error prone than passing strings and have much faster
// comparisions (since they are just fancy ints).
public enum MoveType { NONE, CUBE, VEHICLE, CRANE, GHOST }
/*
 * NONE = Cannot move at all in this object. Added for sake of convention.
 * CUBE = Can not move when on the ground. Can hop. Has great air control.
 * VEHICLE = Can drive. No air control.
 * CRANE = Action button to break beam.
 * GHOST = Player defualt free move.
*/
