using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour {

	#region Public Variables
	public AudioClip[] possessionSounds;
	[Tooltip( "How far away can the player haunt an object?" )]
	public float maxHauntDistance = 10.0f;
	[Tooltip( "Which physics layers should be able to be haunted?" )]
	public LayerMask layersToHaunt;
	public MoveType movementType = MoveType.GHOST;
	public float moveSpeed = 2;
	public float jumpStrength = 20.0f;
	public float steeringSpeed = 10.0f;
	#endregion


	#region Private Variables
	private Camera cam;
	private GameObject CurrentHaunted = null;
	#endregion

	#region Unity Callbacks
	public void Start() {
		this.cam = this.GetComponentInChildren<Camera>();
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;

		//todo: Define custom controls here.
	}

	public void Update() {
		UpdateMovement();

		if ( Input.GetButtonDown( "Haunt" ) ) {
			if ( CurrentHaunted != null ) {
				if ( movementType == MoveType.VEHICLE )
					CurrentHaunted.GetComponent<DriveVehicle>().inCar = false;
				CurrentHaunted = null;
				movementType = MoveType.GHOST;

				// Stop following.
				transform.parent = null;

				//eject from current object and stop execution.
				return;
			}
			RaycastHit hit;
			if ( Physics.Raycast( this.cam.transform.position, this.cam.transform.forward, out hit, this.maxHauntDistance, this.layersToHaunt ) ) {
				GameObject hitRoot = hit.collider.gameObject;
				while ( hitRoot.transform.parent != null )
					hitRoot = hitRoot.transform.parent.gameObject;

				Hauntable h = hitRoot.GetComponent<Hauntable>();
				if ( h != null ) {
					CurrentHaunted = h.Haunt();
					movementType = CurrentHaunted.GetComponent<Hauntable>().moveType;

					// Jump to haunted object.
					transform.position = h.transform.position;

					// Play possession sound.
					this.GetComponent<AudioSource>().clip = this.possessionSounds[Random.Range( 0, possessionSounds.Length )];
					this.GetComponent<AudioSource>().Play();

					// Make haunted object our parent, so we follow it around.
					transform.parent = h.transform;

					if ( movementType == MoveType.VEHICLE )
						CurrentHaunted.GetComponent<DriveVehicle>().inCar = true;

				}
			}
		}
	}
	#endregion

	#region Private Helper Methods
	private void UpdateMovement() {
		Vector3 moveDir = Vector3.zero;

		switch ( movementType ) {
			case MoveType.GHOST:
				moveDir += transform.forward * Input.GetAxis( "Vertical" );
				moveDir += transform.right * Input.GetAxis( "Horizontal" );

				//Happens every frame
				this.transform.position += moveDir.normalized * moveSpeed * Time.deltaTime;
				break;
			case MoveType.CUBE:
				// Only move while jumping
				if ( Input.GetButtonDown( "Jump" ) && CurrentHaunted.GetComponent<Hauntable>().canJump ) {

					CurrentHaunted.GetComponent<Hauntable>().canJump = false;
					moveDir += transform.forward * Input.GetAxis( "Vertical" );
					moveDir += transform.right * Input.GetAxis( "Horizontal" );

					moveDir += Vector3.up;

					// Happens for one frame
					CurrentHaunted.GetComponent<Rigidbody>().AddForce( moveDir.normalized * this.jumpStrength );
				}
				break;
			case MoveType.VEHICLE:
				// Handled by drive script
				break;
			case MoveType.CRANE:
				if ( Input.GetButton( "Jump" ) ) {
					foreach ( Transform t in CurrentHaunted.transform.Find( "Beam" ) ) {
						t.parent = null;
						t.gameObject.GetComponent<Rigidbody>().isKinematic = false;
						if ( t.gameObject.GetComponent<BoxCollider>() != null ) {
							t.gameObject.GetComponent<BoxCollider>().enabled = true;
						}
						if ( t.gameObject.GetComponent<CapsuleCollider>() != null ) {
							t.gameObject.GetComponent<CapsuleCollider>().enabled = true;
						}
					}
				}
				break;
		}
	}
	#endregion
}
