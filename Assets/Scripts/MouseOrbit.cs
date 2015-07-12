using UnityEngine;
using System.Collections;

public class MouseOrbit : MonoBehaviour {
	public Transform target;
	public float distance = 47.0f;
	public float maxdist = 47;
	public float maxshootdist = 20;
	public float mindist = 10;
	public float xSpeed = 250.0f;
	public float ySpeed = 120.0f;
	public float yMinLimit = -20;
	public float yMaxLimit = 80;

	public float x = 0.0f;
	public float y = 0.0f;
	public LayerMask shoothitmask;
	private GameObject player;

	[AddComponentMenu( "Camera-Control/Mouse Orbit" )]

	void Start() {
		player = this.transform.parent.gameObject;
		Vector3 angles = target.transform.eulerAngles;
		x = angles.y;
		y = angles.x;
		// Make the rigid body not change rotation
		if ( GetComponent<Rigidbody>() )
			GetComponent<Rigidbody>().freezeRotation = true;
	}


	void LateUpdate() {
		if ( target ) {
			// Calculate desired rotation of our parent
			x += Input.GetAxis( "Mouse X" ) * xSpeed * 0.02f; // 0.02 = time per physics frame.
			y -= Input.GetAxis( "Mouse Y" ) * ySpeed * 0.02f;

			y = Mathf.Clamp( (float) y, (float) yMinLimit, (float) yMaxLimit );

			Quaternion rotation = Quaternion.Euler( y, x, 0 );

			player.transform.rotation = rotation;

			// See if there is a wall or something between camera and target
			Vector3 v3 = target.transform.position;
			RaycastHit hit;
			if ( Physics.Raycast( v3, -target.transform.forward, out hit, maxdist, shoothitmask ) ) {
				distance = hit.distance - 1.0f;
			} else {
				// There was no obstruction
				distance = maxdist;
			}

			if ( distance <= mindist ) {
				distance = mindist;
			}

			// Set the position of the camera, taking obstructions into account
			transform.position = target.transform.position - target.transform.forward * distance;

		}
	}


	static float ClampAngle( double angle, double min, int max ) {
		if ( angle < -360 )
			angle += 360;
		if ( angle > 360 )
			angle -= 360;
		return Mathf.Clamp( (float) angle, (float) min, (float) max );
	}
}
