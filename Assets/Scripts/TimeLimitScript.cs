using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TimeLimitScript : MonoBehaviour {

	#region Public Variables
	[Tooltip( "Time in seconds until the game ends from the start." )]
	public float startingTime = 30.0f;
	#endregion

	#region Private Variables
	private float timeRemaining;
	private Text timeText;
	#endregion

	#region Unity Callbacks
	void Start() {
		this.timeRemaining = this.startingTime;
		this.timeText = this.GetComponent<Text>();
	}

	void Update() {
		this.timeRemaining -= Time.deltaTime;

		if ( this.timeRemaining <= 0 ) {
			this.timeRemaining = 0;

			// End the game here
		}
		int minutes = (int) timeRemaining / 60;
		int seconds = (int) timeRemaining % 60;
		this.timeText.text = minutes.ToString( "D2" ) + " : " + seconds.ToString( "D2" );

	}
	#endregion
}
