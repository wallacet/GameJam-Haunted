using UnityEngine;
using System.Collections;

public class lowercenter : MonoBehaviour {
	public float centerOfMass = -0.9f;

	void Start() {
		Rigidbody rb = GetComponent<Rigidbody>();

		rb.centerOfMass = new Vector3( rb.centerOfMass.x, centerOfMass, rb.centerOfMass.z );
	}
}
