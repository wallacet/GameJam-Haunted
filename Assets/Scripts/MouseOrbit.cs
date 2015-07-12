using UnityEngine;
using System.Collections;

public class MouseOrbit : MonoBehaviour {
	public Transform target;
	public float distance= 47.0f;
	public float maxdist= 47;
	public float maxshootdist= 20;
	public float mindist= 10;
	public float xSpeed= 250.0f;
	public float ySpeed= 120.0f;
	public float yMinLimit= -20;
	public float yMaxLimit= 80;

	public float x= 0.0f;
	public float y= 0.0f;
	public LayerMask shoothitmask;
	private GameObject player;

	[AddComponentMenu("Camera-Control/Mouse Orbit")]

	void  Start (){
		player = this.transform.parent.gameObject;
		Vector3 angles = transform.eulerAngles;
		x = angles.y;
		y = angles.x;
		// Make the rigid body not change rotation
		if (GetComponent<Rigidbody>())
			GetComponent<Rigidbody>().freezeRotation = true;
	}

	
	void  LateUpdate (){
		Vector3 v3 = target.transform.position;
		RaycastHit hit;
		if (Physics.Raycast(v3, -target.transform.forward, out hit, 300, shoothitmask)) {
			if(hit.distance <= distance)
			{
				distance = hit.distance - 1;
			}
		}
		if(distance <= mindist)
		{
			distance = mindist;
		}
		
		if (target) {
			
			x += Input.GetAxis("Mouse X") * xSpeed * 0.02f;
			y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;

			y = Mathf.Clamp((float)y, (float)yMinLimit, (float)yMaxLimit);
			if(y >= yMinLimit)
			{
			}
			Quaternion rotation= Quaternion.Euler(y, x, 0);
			Vector3 position= rotation * new Vector3(0.0f, 0.0f, -distance) + target.position;
			
			transform.rotation = rotation;
			transform.position = position;

			player.transform.rotation = this.transform.rotation;
		}
	}


	static float  ClampAngle (double angle, double min, int max){
		if (angle < -360)
			angle += 360;
		if (angle > 360)
			angle -= 360;
		return Mathf.Clamp((float)angle, (float)min, (float)max);
	}
}
