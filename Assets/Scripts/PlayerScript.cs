using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour {

	#region Public Variables
	[Tooltip( "How far away can the player haunt an object?" )]
	public float maxHauntDistance = 10.0f;
	[Tooltip( "Which physics layers should be able to be haunted?" )]
	public LayerMask layersToHaunt;
	public MoveType movementType = MoveType.GHOST;
	public float moveSpeed = 2;
	public float jumpStrength = 20.0f;
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
				CurrentHaunted = null;
				movementType = MoveType.GHOST;

				// Stop following.
				transform.parent = null;

				//eject from current object and stop execution.
				return;
			}
			RaycastHit hit;
			if ( Physics.Raycast( this.cam.transform.position, this.cam.transform.forward, out hit, this.maxHauntDistance, this.layersToHaunt ) ) {
				Hauntable h = hit.collider.GetComponent<Hauntable>();
				if ( h != null ) {
					CurrentHaunted = h.Haunt();
					movementType = CurrentHaunted.GetComponent<Hauntable>().moveType;

					// Jump to haunted object.
					transform.position = h.transform.position;

					// Make haunted object our parent, so we follow it around.
					transform.parent = h.transform;

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
				this.transform.position += moveDir.normalized * (moveSpeed * Time.deltaTime);
				break;
			case MoveType.CUBE:
				// Only move while jumping. Needs work.
				if ( Input.GetButtonDown( "Jump" ) && CurrentHaunted.GetComponent<Hauntable>().canJump) {
					
					CurrentHaunted.GetComponent<Hauntable>().canJump = false;
					moveDir += transform.forward * Input.GetAxis( "Vertical" );
					moveDir += transform.right * Input.GetAxis( "Horizontal" );

					moveDir += Vector3.up;

					CurrentHaunted.GetComponent<Rigidbody>().AddForce( moveDir.normalized * this.jumpStrength );
				}
				break;
		}
	}
	#endregion
}
