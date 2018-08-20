using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartMenuController : MonoBehaviour {

	// canvases
	Canvas helpCanvas;
	Canvas settingsCanvas;

	// Use this for initialization
	void Start () {
		helpCanvas = GameObject.Find("/HelpCanvas").GetComponent<Canvas>();
		settingsCanvas = GameObject.Find("/SettingsCanvas").GetComponent<Canvas>();
		toggleCanvas (helpCanvas, false);
		toggleCanvas (settingsCanvas, false);
		settingsCanvas.gameObject.GetComponent<SettingsCanvas> ().UpdateHighscore (); // updating highscore
	}

	public void Play(){
		SceneManager.LoadScene ("Game");
	}

	public void Help(){
		toggleCanvas (helpCanvas, false);
	}

	public void Settings(){
		toggleCanvas (settingsCanvas, true);
	}

	public void Quit(){
		Application.Quit ();
	}

	public void closeHelp(){
		toggleCanvas (helpCanvas, false);
	}

	public void closeSettings(){
		toggleCanvas (settingsCanvas, false);
	}

	public void toggleCanvas (Canvas c, bool enable) { // enables or disables canvas
		c.enabled = enable;
		c.gameObject.SetActive (enable);
	}
}
