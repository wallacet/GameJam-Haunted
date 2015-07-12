using UnityEngine;
using System.Collections;

public class ExplosiveBarrelScript : Hauntable {

	#region Implementation of Abstract Methods of Hauntable
	public override void Haunt( GameObject haunter ) {
		Debug.Log( "Ka-boom!" );
	}
	#endregion

}
