using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour {

	#region Public Variables
	[Tooltip( "How far away can the player haunt an object?" )]
	public float maxHauntDistance = 10.0f;
	[Tooltip( "Which physics layers should be able to be haunted?" )]
	public LayerMask layersToHaunt;
	#endregion

	#region Private Variables
	private Camera cam;
	#endregion

	#region Unity Callbacks
	public void Start() {
		this.cam = this.GetComponentInChildren<Camera>();
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
	}

	public void Update() {
		if ( Input.GetButtonDown( "Haunt" ) ) {
			RaycastHit hit;
			if ( Physics.Raycast( this.cam.transform.position, this.cam.transform.forward, out hit, this.maxHauntDistance, this.layersToHaunt ) ) {
				Debug.Log( "Now haunting: " + hit.collider.gameObject.name );
			}
		}
	}
	#endregion
}
