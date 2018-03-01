using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartMenuController : MonoBehaviour {

	public Canvas helpCanvas;
	public Canvas settingsCanvas;

	// Use this for initialization
	void Start () {
		helpCanvas.gameObject.SetActive (false);
		settingsCanvas.gameObject.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Play(){
		SceneManager.LoadScene ("Game");
	}

	public void Help(){
		helpCanvas.gameObject.SetActive (true);
	}

	public void Settings(){
		settingsCanvas.gameObject.SetActive (true);
	}

	public void Quit(){
		Application.Quit ();
	}

	public void closeHelp(){
		helpCanvas.gameObject.SetActive (false);
	}

	public void closeSettings(){
		settingsCanvas.gameObject.SetActive (false);
	}
}
