using UnityEngine;
using System.Collections;

public class TimedDestoryScript : MonoBehaviour {

	#region Public Variables
	[Tooltip( "How long until this object dies." )]
	public float timeToLive = 1.0f;
	#endregion

	#region Unity Callbacks
	public void Update() {
		this.timeToLive -= Time.deltaTime;

		if ( this.timeToLive <= 0 )
			Destroy( this.gameObject );
	}
	#endregion
}
