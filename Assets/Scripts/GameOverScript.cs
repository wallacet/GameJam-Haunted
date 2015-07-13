using UnityEngine;
using System.Collections;

public class GameOverScript : MonoBehaviour {

	#region Private Variables
	private AudioSource camAudio;
	#endregion

	#region Public Methods
	public void RestartGame() {
		Time.timeScale = 1.0f;
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
		Score.SetScore( 0 );
		Application.LoadLevel( 0 );
	}

	public void QuitGame() {
		Application.Quit();
	}
	#endregion

	#region Unity Callbacks
	public void Start() {
		this.GetComponent<UnityEngine.UI.Text>().text = Score.GetScore().ToString();
		this.camAudio = GameObject.Find( "Player" ).transform.Find( "Camera" ).GetComponent<AudioSource>();
		camAudio.volume = 0.4f;
	}

	public void Update() {
		if( camAudio.volume > 0.1f)
			camAudio.volume -= Time.unscaledDeltaTime * 0.333f;
		else
			camAudio.volume = 0.1f;
	}
	#endregion
}
