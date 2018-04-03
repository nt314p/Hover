using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CanvasController : MonoBehaviour {

	// status text
	Text healthText;
	Text electricityText;
	Text speedText;
	Text distanceText;
	Text deathTitleText;
	Text fpsText;
	Text newHighscoreText;
	Text endDistText;
	GameObject left;
	GameObject right;

	// audio
	AudioSource backgroundMusic;
	float backgroundMusicVol;

	//fps
	float frameCount;
	float nextUpdate;
	float fps;
	float updateRate;

	Canvas deathCanvas;


	// Use this for initialization
	void Start () {
		
		healthText = GameObject.Find ("/MainCanvas/healthText").GetComponent<Text> ();
		electricityText = GameObject.Find ("/MainCanvas/electricityText").GetComponent<Text> ();
		speedText = GameObject.Find ("/MainCanvas/speedText").GetComponent<Text> ();
		distanceText = GameObject.Find ("/MainCanvas/distanceText").GetComponent<Text> ();
		fpsText = GameObject.Find ("/MainCanvas/fpsText").GetComponent<Text> ();
		newHighscoreText = GameObject.Find ("/DeathCanvas/newHighText").GetComponent<Text> ();
		deathTitleText = GameObject.Find ("/DeathCanvas/titleText").GetComponent<Text> ();
		endDistText = GameObject.Find ("/DeathCanvas/endDistText").GetComponent<Text> ();

		deathCanvas = GameObject.Find ("/DeathCanvas").GetComponent<Canvas> ();

		left = GameObject.Find ("/MainCanvas/left");
		right = GameObject.Find ("/MainCanvas/right");

//		left.GetComponent<RectTransform> ().transform.localPosition = new Vector2(0 - Screen.width/4, 0);
//		left.GetComponent<RectTransform> ().sizeDelta = new Vector2(Screen.width/2, Screen.height);
//
//		right.GetComponent<RectTransform> ().transform.localPosition = new Vector2(0 + Screen.width/4, 0);
//		right.GetComponent<RectTransform> ().sizeDelta = new Vector2(Screen.width/2, Screen.height);

		frameCount = 0.0f;
		nextUpdate = 0.0f;
		fps = 0.0f;
		updateRate = 4.0f;

		backgroundMusic = GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<AudioSource> ();
		backgroundMusicVol = 1.0f;

		deathCanvas.gameObject.SetActive (false);

		if (PlayerPrefs.GetInt ("DebugMode") == 1) {
			fpsText.gameObject.SetActive (true);
		} else {
			fpsText.gameObject.SetActive (false);
		}

		nextUpdate = Time.time;
	}

	// Update is called once per frame
	void Update () {

		if (!Player.dead && !Player.powerLoss) {

			// displaying status
			healthText.text = "Health: " + (Mathf.Round (10 * Player.health)) / 10f;
			electricityText.text = "Electricity: " + (Mathf.Round (10 * Player.electricity)) / 10f;
			speedText.text = "Speed: " + Mathf.Round ( Player.forwardVel) + " m/s";
			distanceText.text = "Distance: " + (Mathf.Round (10 * Player.distance)) / 10 + " m";

			// fps
			if (PlayerPrefs.GetInt ("DebugMode") == 1) {

				frameCount++;
				if (Time.time > nextUpdate) {
					nextUpdate += 1.0f / updateRate;
					fps = frameCount * updateRate;
					frameCount = 0;
				}

				fpsText.text = "FPS: " + fps;
			}

		} else {

			if (backgroundMusicVol > 0.1f) {
				//slowly turning down background music volume
				backgroundMusicVol -= Time.deltaTime * 0.5f;
				backgroundMusic.volume = backgroundMusicVol;
			}

			deathCanvas.gameObject.SetActive (true);

			// setting correct death cause
			if (Player.dead) {
				deathTitleText.text = "Wrecked!";
				healthText.text = "Health: 0";

			} else if (Player.powerLoss) {
				deathTitleText.text = "Power Loss!";
				electricityText.text = "Electricity: 0";
			}

			// just aligning some stuff up
			distanceText.text = "Distance: " + (Mathf.Round (10 * Player.distance)) / 10 + " m";

			// end distance display
			endDistText.text = "Distance: " + (Mathf.Round (10 * Player.distance)) / 10 + " m";

			// setting new highscore
			if (Player.distance > PlayerPrefs.GetFloat ("Highscore")) {
				newHighscoreText.gameObject.SetActive (true);
				PlayerPrefs.SetFloat ("Highscore", Player.distance);
			} else {
				newHighscoreText.gameObject.SetActive (false);
			}
		}
	}

	public void rightPress () {
		Debug.Log ("right salad");
	}

	public void leftPress () {
		Debug.Log ("left salad");
	}

	public void Quit () {
		SceneManager.LoadScene ("Start");
	}

	public void Restart () {
		SceneManager.LoadScene ("Game");
	}
}