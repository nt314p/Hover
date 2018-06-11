using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsCanvas : MonoBehaviour {

	public int contMethod; // 0 is any; 1 is arrow keys, 2 is touch input
	public Text ControlModeText;

	public int qualSettings; // 0 - Performance, 1 - Balanced, 2 - Quality
	public Text qualText;

	public Toggle debugToggle;

	public Text highscoreText;

	// Use this for initialization
	void Start () {

		// first run values
		if (!PlayerPrefs.HasKey ("Highscore")) {
			PlayerPrefs.SetFloat ("Highscore", 0);
		}
		if (!PlayerPrefs.HasKey ("QualityMode")) {
			PlayerPrefs.SetString ("QualityMode", "Balanced");
			// Performance, Balanced, Quality
		}
		if (!PlayerPrefs.HasKey ("ControlMode")) {
			PlayerPrefs.SetString ("ControlMode", "ArrowKeys");
			// ArrowKeys, TouchInput
		}
		if (!PlayerPrefs.HasKey ("DebugMode")) {
			PlayerPrefs.SetInt ("DebugMode", 0);
			// 0 - Off, 1 - On
		}

		// setting button text to current settings from playerprefs
		//control method
		if (PlayerPrefs.GetString ("ControlMode") == "ArrowKeys") {
			ControlModeText.text = "Arrow Keys";
			contMethod = 1;
		} else if (PlayerPrefs.GetString ("ControlMode") == "TouchInput") {
			ControlModeText.text = "Touch Input";
			contMethod = 2;
		} else if (PlayerPrefs.GetString ("ControlMode") == "Any") {
			ControlModeText.text = "Any Input";
			contMethod = 0;
		}

		//Debug
		if (PlayerPrefs.GetInt ("DebugMode") == 1) {
			debugToggle.isOn = true;
		} else if (PlayerPrefs.GetInt ("DebugMode") == 0) {
			debugToggle.isOn = false;
		}

		//Quality
		if (PlayerPrefs.GetString ("QualityMode") == "Performance") {
			qualSettings = 0;
			qualText.text = "Performance";
		} else if (PlayerPrefs.GetString ("QualityMode") == "Balanced") {
			qualSettings = 1;
			qualText.text = "Balanced";
		} else if (PlayerPrefs.GetString ("QualityMode") == "Quality") {
			qualSettings = 2;
			qualText.text = "Quality";
		}
			
		UpdateQuality ();
		UpdateHighscore ();
	}

	public void UpdateHighscore (){
		highscoreText.text = "Highscore: " + PlayerPrefs.GetFloat ("Highscore").ToString () + " m";
	}

	public void ChangeControlMethod (){
		if (contMethod == 1) {
			contMethod = 2;
			ControlModeText.text = "Touch Input";
			PlayerPrefs.SetString ("ControlMode", "TouchInput");

		} else if (contMethod == 2) {
			contMethod = 0;
			ControlModeText.text = "Any Input";
			PlayerPrefs.SetString ("ControlMode", "Any");

		} else if (contMethod == 0) {
			contMethod = 1;
			ControlModeText.text = "Arrow Keys";
			PlayerPrefs.SetString ("ControlMode", "ArrowKeys");
		}
	}

	public void ChangeQuality (){
		if (qualSettings == 0) {
			qualSettings = 1;
			PlayerPrefs.SetString ("QualityMode", "Balanced");
			qualText.text = "Balanced";

		} else if (qualSettings == 1) {
			qualSettings = 2;
			PlayerPrefs.SetString ("QualityMode", "Quality");			
			qualText.text = "Quality";

		} else if (qualSettings == 2) {
			qualSettings = 0;
			PlayerPrefs.SetString ("QualityMode", "Performance");
			qualText.text = "Performance";
		}

		UpdateQuality ();
	}


	public void UpdateQuality(){
		QualitySettings.SetQualityLevel (qualSettings);
	}

	public void resetHighscores(){
		PlayerPrefs.SetFloat ("Highscore", 0);
		highscoreText.text = "Highscore: " + PlayerPrefs.GetFloat ("Highscore") + " m";
	}

	public void DebugToggle (bool state) {
		state = debugToggle.isOn;

		if (state) {
			PlayerPrefs.SetInt ("DebugMode", 1);
		} else if (!state) {
			PlayerPrefs.SetInt ("DebugMode", 0);
		}
	}

	public void backButton (){

		PlayerPrefs.Save ();
	}
}
