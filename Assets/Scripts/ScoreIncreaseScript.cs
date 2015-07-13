using UnityEngine;
using System.Collections;

public class ScoreIncreaseScript : MonoBehaviour {

	public int scoreIncrease = 0;

	public void Update() {
		this.transform.Find( "Text" ).GetComponent<UnityEngine.UI.Text>().text = "+ " + this.scoreIncrease.ToString();
	}
}
