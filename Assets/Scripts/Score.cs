using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Score : MonoBehaviour {

	#region Public Variables
	public AudioClip[] scoreIncreaseSounds;
	#endregion

	#region Static Variables
	private static int score = 0;
	private static Text scoreDisplay;
	private static AudioClip[] _scoreIncreaseSounds;
	private static GameObject player;
	#endregion

	#region Unity Callbacks
	public void Start() {
		Score.scoreDisplay = this.GetComponent<Text>();
		Score._scoreIncreaseSounds = this.scoreIncreaseSounds;
		Score.player = GameObject.Find( "Player" );
	}

	public void Update() {
		Score.scoreDisplay.text = Score.score.ToString();
	}
	#endregion

	#region Static Methods
	public static int AddScore( int amountToAdd ) {
		Score.score += amountToAdd;
		AudioSource.PlayClipAtPoint(
				Score._scoreIncreaseSounds[Random.Range( 0, Score._scoreIncreaseSounds.Length )],
				Score.player.transform.position
				);
		return Score.score;
	}

	public static int SetScore( int newScore ) {
		Score.score = newScore;
		return Score.score;
	}

	public static int GetScore() {
		return Score.score;
	}
	#endregion

}
