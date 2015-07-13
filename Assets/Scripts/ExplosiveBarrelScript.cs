using UnityEngine;
using System.Collections;

public class ExplosiveBarrelScript : MonoBehaviour {

	#region Public Variables
	public GameObject explosionPrefab;
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
	#endregion

	#region Public Methods
	public void Explode() {
		this.CancelInvoke( "Explode" );
		Destroy( this.gameObject );
		GameObject explosion = (GameObject) Instantiate( this.explosionPrefab, this.transform.position, this.transform.rotation );
		explosion.GetComponent<AudioSource>().clip = this.explosionSounds[Random.Range( 0, this.explosionSounds.Length )];
		explosion.GetComponent<AudioSource>().Play();
		Score.AddScore( this.scoreValue );
	}
	#endregion

	#region Unity Callbacks
	public void Start() {
		this.rb = this.GetComponent<Rigidbody>();
	}

	public void Update() {
		// Calculating magnetude is slow; using sqrMag instead.
		if ( this.rb.velocity.sqrMagnitude >= sensitivity * sensitivity ) {
			// Since we don't do this every update, magnitude is ok.
			this.Invoke( "Explode", this.timeToExplode / this.rb.velocity.magnitude );
		}
	}
	#endregion
}
