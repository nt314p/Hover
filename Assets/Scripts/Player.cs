using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour {

	// transform and rotation
	readonly float acceleration = 15000f;
	readonly float turnSpeed = 20000f;
	private static float turnAngle = 0f;
	private float turnStatus = 0; // -1 left, 0 nono, 1 right
	private float velocity;

	private static float health = 100f;
	private static float electricity = 1000f;
	private static float forwardVel;
	private static float distance = 0f;
	private static bool dead = false;
	private static bool powerLoss = false;
	private static float playerZ;
	private readonly static float hoverHeight = 5f;

	private string contMode;

	// audio
	private AudioSource powerDown;
	private bool powerDownPlay = false;
	private AudioSource crash;
	private bool crashPlay = false;

	private float distThisFrame;

	private Rigidbody rb;

	// Use this for initialization
	void Start () {

		powerDown = GameObject.Find ("/Main Camera/powerDown").GetComponent<AudioSource> ();
		crash = GameObject.Find ("/Main Camera/crash").GetComponent<AudioSource> ();

		contMode = PlayerPrefs.GetString ("ControlMode");

		rb = GetComponent<Rigidbody> ();
		rb.angularVelocity = Vector3.zero;
		//rb.constraints = RigidbodyConstraints.FreezePositionY;
	}
	
	// Update is called once per frame
	void Update () {
		
		if (!dead && !powerLoss) {
			forwardVel = rb.velocity.z;

			turnStatus = 0;

			// TOUCH INPUT
			if (contMode == "TouchInput" || contMode == "Any") {
				if (Input.touchCount > 0) {
					Touch t = Input.GetTouch (Input.touchCount-1); // get latest touch
					if (t.position.x < Screen.width / 2) {
						turnStatus = -1;
					} else if (t.position.x > Screen.width / 2) {
						turnStatus = 1;
					}
				}
			}

			// ARROW KEY INPUT
			if (contMode == "ArrowKeys" || contMode == "Any") {
				float i = Input.GetAxis ("Horizontal");
				if (!(i == 0)) {
					turnStatus = i;
				}
			}

			// turn status 0: no turn; 1: right; -1: left;

			// calculating the turn angle
			if (!(Mathf.Abs (turnAngle - 45 * turnStatus * Time.deltaTime) > 15)) {
				
				turnAngle -= 45 * turnStatus * Time.deltaTime;
			}

			// returning to middle
			if (turnStatus == 0) { 
				if (turnAngle > 0) {
					turnAngle = Mathf.Max (0, turnAngle - 45 * Time.deltaTime);
				} else if (turnAngle < 0) {
					turnAngle = Mathf.Min (0, turnAngle + 45 * Time.deltaTime);
				}
			}

			// turning the hovercraft
			transform.eulerAngles = new Vector3 (0, 0, turnAngle);


			// moving the hovercraft left and right
			rb.AddForce (Vector3.right * turnAngle / -15 * turnSpeed * Time.deltaTime);

			// forward movement
			if (forwardVel < 250) {
				//rb.AddRelativeForce (Vector3.forward * acceleration * Time.deltaTime);
			}

			distThisFrame = Mathf.Round (100 * forwardVel * Time.deltaTime) / 100f;

			// adding distance
			distance += distThisFrame;

			// taking away electricity
			electricity = Mathf.Max(electricity- 18f * Time.deltaTime, 0);

			// checking for low health and electricity
			if (health <= 0) {
				dead = true;
				Death ();
			}

			if (electricity <= 0) {
				powerLoss = true;
				PowerLoss ();
			}

			playerZ = this.transform.position.z;
		}
	}

	void FixedUpdate () {
		// setting velocity for collision
		velocity = rb.velocity.magnitude;			
		//transform.position = new Vector3 (transform.position.x, hoverHeight, transform.position.z);
	}

	void OnCollisionEnter (Collision other) {

		// detecting collisions and deducting health
		if (other.gameObject.CompareTag ("obstacle")) {
			health = Mathf.Max(health - Mathf.Abs (velocity * 0.02f), 0);
		}
	}

	// checking for pickups
	void OnTriggerEnter (Collider other) {
		if (other.gameObject.CompareTag("wrenchPickup")) {
			AddHealth ();
			other.gameObject.SetActive (false);
		}
		if (other.gameObject.CompareTag("electricityPickup")) {
			AddElectricity ();
			other.gameObject.SetActive (false);
		}
	}

	public static void AddElectricity () {
		electricity = Mathf.Min(1000, electricity + 50);
	}

	public static void AddHealth () {
		health = Mathf.Min(100, health + 5);
	}

	public void Death () {
		if (!crashPlay) {
			crash.Play ();
			crashPlay = true;
		}
		rb.constraints = RigidbodyConstraints.None;
	}

	public void PowerLoss () {
		if (!powerDownPlay) {
			powerDown.Play ();
			powerDownPlay = true;
		}
		rb.constraints = RigidbodyConstraints.None;
	}

	void OnEnable () {
		// Tell our 'OnLevelFinishedLoading' function to start listening for a scene change as soon as this script is enabled.
		SceneManager.sceneLoaded += OnLevelFinishedLoading;
	}

	void OnDisable () {
		// Tell our 'OnLevelFinishedLoading' function to stop listening for a scene change as soon as this script is disabled. 
		// Remember to always have an unsubscription for every delegate you subscribe to!
		SceneManager.sceneLoaded -= OnLevelFinishedLoading;
	}

	void OnLevelFinishedLoading (Scene scene, LoadSceneMode mode) {
		// reseting values
		health = 100;
		electricity = 1000;
		distance = 0;
		dead = false;
		powerLoss = false;

		// resetting rigidbody values
		rb = GetComponent<Rigidbody> ();
		rb.angularVelocity = Vector3.zero;
		rb.velocity = Vector3.zero;
		//rb.constraints = RigidbodyConstraints.FreezePositionY;

		Debug.Log ("Level Loaded");
		Debug.Log (scene.name);
		Debug.Log (mode);
	}

	public static float GetHealth() {
		return health;
	}

	public static float GetElectricity() {
		return electricity;
	}

	public static float GetDistance() {
		return distance;
	}

	public static float GetForwardVelocity() {
		return forwardVel;
	}

	public static bool IsDead() {
		return dead;
	}

	public static bool IsPowerLoss() {
		return powerLoss;
	}

	public static float GetPlayerZ() {
		return playerZ;
	}

	public static float GetHoverHeight() {
		return hoverHeight;
	}


}