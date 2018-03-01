using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class CanvasController : MonoBehaviour {

	// status text
	public Text healthText;
	public Text electricityText;
	public Text speedText;
	public Text distanceText;
	public Text deathTitleText;
	public GameObject fpsText;
	public Text fpsTextObj;
	public GameObject newHighscoreText;

	// audio
	AudioSource backgroundMusic;
	float backgroundMusicVol;

	//fps
	float frameCount;
	float nextUpdate;
	float fps;
	float updateRate;


	public Text endDistText;
	public Canvas deathCanvas;


	// Use this for initialization
	void Start () {

		frameCount = 0.0f;
		nextUpdate = 0.0f;
		fps = 0.0f;
		updateRate = 4.0f;

		backgroundMusic = GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<AudioSource> ();
		backgroundMusicVol = 1.0f;


		deathCanvas.gameObject.SetActive (false);

		if (PlayerPrefs.GetInt ("DebugMode") == 1) {
			fpsText.SetActive (true);
			fpsTextObj = fpsText.GetComponent<Text> ();

		} else {
			fpsText.SetActive (false);
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

				fpsTextObj.text = "FPS: " + fps;
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
				newHighscoreText.SetActive (true);
				PlayerPrefs.SetFloat ("Highscore", Player.distance);
			}
		}
	}

	public void Quit () {
		SceneManager.LoadScene ("Start");
	}

	public void Restart () {

		SceneManager.LoadScene ("Game");
	}
}
