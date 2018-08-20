using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpin : MonoBehaviour {

	float dir = -0.8f;
	bool justFlipped = false;
	GameObject highscoreCanvas;
	int maxY = 12;
	int minY = 10;

	void Start () {
		highscoreCanvas = GameObject.Find ("/HighscoreCanvas");
	}

	// Update is called once per frame
	void Update () {
		transform.Rotate(Vector3.up, 10 * Time.deltaTime);

		// moving canvas up and down
		highscoreCanvas.transform.position += Vector3.up*dir*Time.deltaTime;

		// moving the hovercraft up and down
		transform.position += Vector3.up*dir*Time.deltaTime;

		// changing direction
		if (transform.position.y > maxY || transform.position.y < minY) {
			if (!justFlipped) {
				dir *= -1;
				justFlipped = true;
			}
		} else {
			justFlipped = false;
		}
	}
}
