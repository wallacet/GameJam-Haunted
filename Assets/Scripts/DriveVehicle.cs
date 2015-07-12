// Converted from UnityScript to C# at http://www.M2H.nl/files/js_to_c.php - by Mike Hergaarden
// Do test the code! You usually need to change a few small bits.

using UnityEngine;
using System.Collections;

public class DriveVehicle : MonoBehaviour {
	public WheelCollider wheelFL;
	public WheelCollider wheelFR;
	public WheelCollider wheelRL;
	public WheelCollider wheelRR;
	public float maxTorque = 50f;
	public bool inCar = false;
	public Camera carCam = null;
	public float centerOfMass = -0.9f;
	public int maxTurn = 2;
	public float turnI = 0;
	public int speedThreshA = 600;
	public int speedThreshB = 1200;
	public int speedThreshC = 2500;
	public float turnSpeedA = 2f;
	public float turnSpeedB = .6f;
	public float turnSpeedC = .1f;

	public bool pressingA = false;
	public bool pressingD = false;
	//http://www.youtube.com/watch?v=hIWBd7jpcyA


	void Start() {
		Rigidbody rb = GetComponent<Rigidbody>();
		rb.centerOfMass = new Vector3( rb.centerOfMass.x, centerOfMass, rb.centerOfMass.z );
	}


	//TODO: Add counters to smooth out torque, wheel turning via "hold and ++ until reached maximum torque/turn speed.
	//TODO: Add bonus steer to handbrake. Minus torque OR check out "Brake" factor in wheelcollider.
	void Update() {
		wheelRL.motorTorque = 0;
		wheelRR.motorTorque = 0;
		wheelFR.steerAngle = 0;
		wheelFL.steerAngle = 0;
		pressingA = false;
		pressingD = false;

		if ( Input.GetKey( KeyCode.A ) || Input.GetKey( KeyCode.LeftArrow ) ) {
			pressingA = true;
		}
		if ( Input.GetKey( KeyCode.D ) || Input.GetKey( KeyCode.RightArrow ) ) {
			pressingD = true;
		}
		if ( inCar == true ) {
			if ( !pressingA && !pressingD ) {
				if ( wheelFL.rpm <= speedThreshA ) {
					if ( turnI < (turnSpeedA + .1) && turnI > (-turnSpeedA - .1) ) {
						turnI = 0;
					}
					if ( turnI >= .01 && !pressingD ) {
						turnI -= turnSpeedA;
					}
					if ( turnI <= -.01 && !pressingA ) {
						turnI += turnSpeedA;
					}
				}
				if ( wheelFL.rpm > speedThreshA && wheelFL.rpm < speedThreshB ) {
					if ( turnI <= (turnSpeedB + .1) && turnI >= (-turnSpeedB - .1) ) {
						turnI = 0;
					}
					if ( turnI >= .01 ) {
						turnI -= turnSpeedB;
					}
					if ( turnI <= -.01 ) {
						turnI += turnSpeedB;
					}
				}
				if ( wheelFL.rpm >= speedThreshB ) {
					if ( turnI <= (turnSpeedC + .1) && turnI >= (-turnSpeedC - .1) ) {
						turnI = 0;
					}
					if ( turnI >= .01 ) {
						turnI -= turnSpeedC;
					}
					if ( turnI <= -.01 ) {
						turnI += turnSpeedC;
					}
				}
			}
			if ( pressingA || pressingD ) {
				if ( wheelFL.rpm <= speedThreshA ) {
					if ( turnI >= turnSpeedA && pressingA ) {
						turnI -= turnSpeedA;
					}
					if ( turnI <= -turnSpeedA && pressingD ) {
						turnI += turnSpeedA;
					}
					if ( pressingA ) {
						turnI -= turnSpeedA;
					}
					if ( pressingD ) {
						turnI += turnSpeedB;
					}
				}
				if ( wheelFL.rpm > speedThreshA && wheelFL.rpm < speedThreshB ) {
					if ( turnI >= turnSpeedB && pressingA ) {
						turnI -= turnSpeedB;
					}
					if ( turnI <= -turnSpeedB && pressingD ) {
						turnI += turnSpeedB;
					}
					if ( pressingA ) {
						turnI -= turnSpeedB;
					}
					if ( pressingD ) {
						turnI += turnSpeedB;
					}
				}
				if ( wheelFL.rpm >= speedThreshB ) {
					if ( turnI >= turnSpeedC && pressingA ) {
						turnI -= turnSpeedC;
					}
					if ( turnI <= -turnSpeedC && pressingD ) {
						turnI += turnSpeedC;
					}
					if ( pressingA ) {
						turnI -= turnSpeedC;
					}
					if ( pressingD ) {
						turnI += turnSpeedC;
					}
				}
			}

			//GameObject.Find( "Player" ).transform.position = this.transform.position;
			//GameObject.FindWithTag("Player").vertSpeed = 0;

			if ( Input.GetKey( KeyCode.W ) ) {
				if ( wheelFL.rpm <= 600 ) {
					wheelRL.motorTorque = maxTorque;
					wheelRR.motorTorque = maxTorque;
				}
				if ( wheelFL.rpm <= 1200 && wheelFL.rpm > 600 ) {
					wheelRL.motorTorque = maxTorque / 3.0f;
					wheelRR.motorTorque = maxTorque / 3.0f;
				}
			}
			if ( turnI >= 15 ) {
				turnI = 15;
			}
			if ( turnI <= -15 ) {
				turnI = -15;
			}
			//Turn the wheel slowly and incrementally with each update tick.
			wheelFL.steerAngle = turnI;
			wheelFR.steerAngle = turnI;

			if ( Input.GetKey( KeyCode.S ) ) {
				if ( wheelFL.rpm > -500 ) {
					if ( wheelFL.rpm >= 1500 ) {
						wheelRR.motorTorque = -maxTorque / 2.0f;
						wheelRL.motorTorque = -maxTorque / 2.0f;
					}
					if ( wheelFL.rpm <= 800 ) {
						wheelRR.motorTorque = -maxTorque / 4.0f;
						wheelRL.motorTorque = -maxTorque / 4.0f;
					}
				}
			}

		}
	}
}