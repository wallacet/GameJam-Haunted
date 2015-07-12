using UnityEngine;
using System.Collections;

public class ExplosiveBarrelScript : Hauntable {

	#region Public Variables
	public GameObject explosionPrefab;
	#endregion

	#region Implementation of Abstract Methods of Hauntable
	public override void Haunt( GameObject haunter ) {
		Destroy( this.gameObject );
		Instantiate( explosionPrefab, this.transform.position, this.transform.rotation );
	}
	#endregion

}
