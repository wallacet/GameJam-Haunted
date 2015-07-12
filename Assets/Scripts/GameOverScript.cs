using UnityEngine;
using System.Collections;

public class GameOverScript : MonoBehaviour {

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
	}
	#endregion
}
