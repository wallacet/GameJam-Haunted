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
				// Needs LOTS of work
				moveDir += Vector3.ProjectOnPlane(
									CurrentHaunted.transform.forward * Input.GetAxis( "Vertical" ),
									Vector3.up );

				// How are your math skillz, chumps?
				moveDir *= Mathf.Clamp( Mathf.Cos( CurrentHaunted.transform.rotation.eulerAngles.z * Mathf.Deg2Rad ), 0, 1 );
				moveDir *= Mathf.Clamp( Mathf.Cos( CurrentHaunted.transform.rotation.eulerAngles.x * Mathf.Deg2Rad ), 0, 1 );

				// Happens every frame
				RaycastHit hit;
				if ( Physics.Raycast( CurrentHaunted.transform.position, Vector3.down, out hit, 4.0f ) ) {
					CurrentHaunted.transform.position += moveDir * Time.deltaTime * moveSpeed;
				} else {
					Debug.Log( "Not touching ground." );
					Debug.DrawRay( CurrentHaunted.transform.position, Vector3.down * 4.0f, Color.red );
				}

				float curRot = CurrentHaunted.transform.rotation.eulerAngles.y;
				float targetRot = cam.transform.rotation.eulerAngles.y;

				Quaternion rot = Quaternion.Euler( CurrentHaunted.transform.rotation.eulerAngles.x,
														Mathf.LerpAngle( curRot, targetRot, Time.deltaTime ),
														CurrentHaunted.transform.rotation.eulerAngles.z
														);

				CurrentHaunted.transform.rotation = rot;
				break;
		}
	}
	#endregion
}
