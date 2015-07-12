using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TimeLimitScript : MonoBehaviour {

	#region Public Variables
	[Tooltip( "Time in seconds until the game ends from the start." )]
	public float startingTime = 30.0f;
	public GameObject gameOverPrefab;
	#endregion

	#region Private Variables
	private float timeRemaining;
	private Text timeText;
	private GameObject player;
	private bool gameOver = false;
	#endregion

	#region Unity Callbacks
	void Start() {
		this.timeRemaining = this.startingTime;
		this.timeText = this.GetComponent<Text>();
		this.player = GameObject.Find( "Player" );
	}

	void Update() {
		this.timeRemaining -= Time.deltaTime;

		if ( this.timeRemaining <= 0 && !gameOver ) {
			this.timeRemaining = 0;
			this.gameOver = true;

			// End the game here
			GameObject go = Instantiate( gameOverPrefab, Vector3.zero, Quaternion.identity ) as GameObject;
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;

			Time.timeScale = 0;
			player.GetComponent<PlayerScript>().enabled = false;
			player.GetComponentInChildren<MouseOrbit>().enabled = false;
		}
		int minutes = (int) timeRemaining / 60;
		int seconds = (int) timeRemaining % 60;
		this.timeText.text = minutes.ToString( "D2" ) + " : " + seconds.ToString( "D2" );

	}
	#endregion
}
