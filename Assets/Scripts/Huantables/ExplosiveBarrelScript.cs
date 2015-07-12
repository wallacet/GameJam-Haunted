using UnityEngine;
using System.Collections;

public class ExplosiveBarrelScript : Hauntable {

	#region Public Variables
	public GameObject explosionPrefab;
	public float sensitivity = 1.0f;
	public float timeToExplode = 5.0f;
	#endregion

	#region Private Variables
	private Rigidbody rb;
	private bool exploding = false;
	#endregion

	#region Implementation of Abstract Methods of Hauntable
	public override void Haunt( GameObject haunter = null ) {
		Destroy( this.gameObject );
		Instantiate( explosionPrefab, this.transform.position, this.transform.rotation );
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
			this.Invoke( "HauntWithThis", this.timeToExplode / this.rb.velocity.magnitude );
		}
	}
	#endregion

	#region Private Helper Methods
	private void HauntWithThis() {
		this.Haunt( this.gameObject );
	}
	#endregion

}
