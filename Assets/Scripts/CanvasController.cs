﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CanvasController : MonoBehaviour {

	bool paused = false;
	bool debug = false;

	// status text
	Text healthText;
	Text electricityText;
	Text speedText;
	Text distanceText;
	Text deathTitleText;
	Text fpsText;
	Text newHighscoreText;
	Text endDistText;

	// audio
	AudioSource backgroundMusic;
	float backgroundMusicVol;

	//fps
	float frameCount;
	float nextUpdate;
	float fps;
	float updateRate;

	Canvas deathCanvas;
	Canvas pauseCanvas;


	// Use this for initialization
	void Start () {

		// initializing variables
		healthText = GameObject.Find ("/MainCanvas/healthText").GetComponent<Text> ();
		electricityText = GameObject.Find ("/MainCanvas/electricityText").GetComponent<Text> ();
		speedText = GameObject.Find ("/MainCanvas/speedText").GetComponent<Text> ();
		distanceText = GameObject.Find ("/MainCanvas/distanceText").GetComponent<Text> ();
		fpsText = GameObject.Find ("/MainCanvas/fpsText").GetComponent<Text> ();
		newHighscoreText = GameObject.Find ("/DeathCanvas/newHighText").GetComponent<Text> ();
		deathTitleText = GameObject.Find ("/DeathCanvas/titleText").GetComponent<Text> ();
		endDistText = GameObject.Find ("/DeathCanvas/endDistText").GetComponent<Text> ();

		deathCanvas = GameObject.Find ("/DeathCanvas").GetComponent<Canvas> ();
		pauseCanvas = GameObject.Find ("/PauseCanvas").GetComponent<Canvas> ();

		frameCount = 0.0f;
		nextUpdate = 0.0f;
		fps = 0.0f;
		updateRate = 4.0f;

		// music settings
		backgroundMusic = GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<AudioSource> ();
		backgroundMusicVol = 1.0f;

		// hiding both canvases
		deathCanvas.gameObject.SetActive (false);
		pauseCanvas.gameObject.SetActive (false);

		newHighscoreText.gameObject.SetActive (false); // hiding the new highscore

		if (PlayerPrefs.GetInt ("DebugMode") == 1) {
			debug = true;
			fpsText.gameObject.SetActive (true);
		} else {
			debug = false;
			fpsText.gameObject.SetActive (false);
		}

		nextUpdate = Time.time;
	}

	// Update is called once per frame
	void Update () {

		if (!Player.dead && !Player.powerLoss) {
			if (Input.GetKeyUp (KeyCode.Escape) && !paused) {
				Pause ();
			} else if (Input.GetKeyUp (KeyCode.Escape) && paused) {
				Resume ();
			}

			// displaying status
			healthText.text = "Health: " + System.Math.Round (Player.health, 1);
			electricityText.text = "Electricity: " + System.Math.Round (Player.electricity, 1);
			speedText.text = "Speed: " + System.Math.Round (Player.forwardVel, 1) + " m/s";
			distanceText.text = "Distance: " + System.Math.Round (Player.distance, 1) + " m";

			// fps
			if (debug) {
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
			} else if (Player.powerLoss) {
				deathTitleText.text = "Power Loss!";
			}

			// just aligning some stuff up
			distanceText.text = "Distance: " + System.Math.Round (Player.distance, 1) + " m";

			// end distance display
			endDistText.text = distanceText.text;

			// setting new highscore
			if (Player.distance > PlayerPrefs.GetFloat ("Highscore")) {
				newHighscoreText.gameObject.SetActive (true);
				PlayerPrefs.SetFloat ("Highscore", Player.distance);
			}
		}
	}

	public void Quit () {
		SceneManager.LoadScene ("Start");
	}

	public void Restart () {
		Time.timeScale = 1;
		SceneManager.LoadScene ("Game");
	}

	public void Pause () {
		paused = true;
		Time.timeScale = 0;
		pauseCanvas.gameObject.SetActive (true);
	}

	public void Resume () {
		paused = false;
		Time.timeScale = 1;
		pauseCanvas.gameObject.SetActive (false);
	}
}