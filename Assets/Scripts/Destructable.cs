using UnityEngine;
using System.Collections;

public class Destructable : MonoBehaviour {
	//The same as a barrel, without an explosion.
	#region Public Variables
	public GameObject gibs;
	[Tooltip( "How much random force should be given to a gib on destroy?" )]
	public int gibForce = 50;
	public int numberGibs = 1;
	public AudioClip[] explosionSounds;
	[Tooltip( "How much does it take for this barrel to explode?" )]
	public float sensitivity = 1.0f;
	[Tooltip( "How long does it take to explode after being nudged barely over our sensitivity? When hit harder, the time before exploding will be shorter." )]
	public float timeToExplode = 5.0f;
	[Tooltip( "How much score is this barrel worth when it explodes?" )]
	public int scoreValue = 100;
	#endregion

	#region Private Variables
	private Rigidbody rb;
	private bool exploding = false;
	#endregion

	#region Public Methods
	public void Explode() {
		Destroy( this.gameObject );
		for(int i = 0; i <= numberGibs; i++)
		{
			Vector3 startposition = this.transform.position;
			if(numberGibs > 1)
			{
				//if there's more, start them separate.
				startposition.x += Random.Range (-gibForce,gibForce);
				startposition.y += Random.Range (-gibForce,gibForce);
				startposition.z += Random.Range (-gibForce,gibForce);
			}
			GameObject gib = (GameObject) Instantiate( this.gibs, startposition, this.transform.rotation );
			Vector3 torque = this.rb.velocity;
			torque.x += Random.Range (-gibForce,gibForce);
			torque.y += Random.Range (-gibForce,gibForce);
			torque.z += Random.Range (-gibForce,gibForce);
			gib.GetComponent<Rigidbody>().AddTorque(torque);
			if(this.explosionSounds.Length > 0)
			{
				gib.GetComponent<AudioSource>().clip = this.explosionSounds[Random.Range( 0, this.explosionSounds.Length )];
				gib.GetComponent<AudioSource>().Play();
			}
		}

		
		Score.AddScore( this.scoreValue );
	}
	#endregion

	#region Unity Callbacks
	public void Start() {
		this.rb = this.GetComponent<Rigidbody>();
	}

	public void Update() {
		// Calculating magnetude is slow; using sqrMag instead.
		if ( !exploding && this.rb.velocity.sqrMagnitude >= sensitivity * sensitivity ) {
			this.exploding = true;
			// Since we don't do this every update, magnitude is ok.
			this.Invoke( "Explode", this.timeToExplode / this.rb.velocity.magnitude );
		}
	}
	#endregion
}
