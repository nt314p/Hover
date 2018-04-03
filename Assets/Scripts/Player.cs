using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour {

	// transform and rotation
	public float acceleration = 150f;
	float turnSpeed = 20000f;
	public float turnAngle = 0f;
	public float turnStatus = 0; // -1 left, 0 nono, 1 right
	public float velocity;

	public static float health = 100f;
	public static float electricity = 1000f;
	public static float forwardVel;
	public static float distance = 0f;
	public static bool dead = false;
	public static bool powerLoss = false;
	public static float playerZ;

	// audio
	AudioSource powerDown;
	bool powerDownPlay = false;
	AudioSource crash;
	bool crashPlay = false;

	float distThisFrame;

	Rigidbody rb;

	// Use this for initialization
	void Start () {

		powerDown = GameObject.Find ("/Main Camera/powerDown").GetComponent<AudioSource> ();
		crash = GameObject.Find ("/Main Camera/crash").GetComponent<AudioSource> ();

		rb = GetComponent<Rigidbody> ();
		rb.angularVelocity = Vector3.zero;
		rb.constraints = RigidbodyConstraints.FreezePositionY;
	}
	
	// Update is called once per frame
	void Update () {
		
		if (!dead && !powerLoss) {
			forwardVel = rb.velocity.z;

			// turning the hovercraft
			if (Input.touchCount > 0) {
				Touch t = Input.GetTouch (0);
				//Debug.Log (t.position);
				if (t.position.x < Screen.width / 2) {
					turnStatus = -1;
				} else if (t.position.x > Screen.width / 2) {
					turnStatus = 1;
				}
			} else {
				turnStatus = 0;
			}

			turnStatus = Input.GetAxis ("Horizontal");

			if (!(Mathf.Abs (turnAngle + 45 * turnStatus * Time.deltaTime) > 15)) {
				turnAngle -= 45 * turnStatus * Time.deltaTime;
			}

			if (turnStatus == 0) { // returning to middle
				if (turnAngle > 0) {
					turnAngle = Mathf.Max(0, turnAngle - 45 * Time.deltaTime);
				} else if (turnAngle < 0) {
					turnAngle = Mathf.Min(0, turnAngle + 45 * Time.deltaTime);
				}
			}
				
			transform.eulerAngles = new Vector3 (0, 0, turnAngle);

			// moving the hovercraft left and right
			//rb.AddForce (Vector3.right * Input.GetAxis ("Horizontal") * turnSpeed * Time.deltaTime);
			rb.AddForce (Vector3.right * turnAngle / -15 * turnSpeed * Time.deltaTime);

			//Debug.Log(Input.GetAxis ("Horizontal") + ", " + Time.time * 1000);

			// forward movement
			if (forwardVel < 250) {
				rb.AddRelativeForce (Vector3.forward * acceleration * 100 * Time.deltaTime);
			}

			distThisFrame = Mathf.Round (100 * forwardVel * Time.deltaTime) / 100f;

			// adding distance
			distance += distThisFrame;

			// taking away electricity
			electricity -= 18f * Time.deltaTime;

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
		transform.position = new Vector3 (transform.position.x, 5f, transform.position.z);
	}

	void OnCollisionEnter (Collision other) {

		// detecting collisions and deducting health
		if (other.gameObject.CompareTag ("obstacle")) {
			health -= Mathf.Abs (velocity * 0.02f);
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

	void Death () {
		if (!crashPlay) {
			crash.Play ();
			crashPlay = true;
		}
		rb.constraints = RigidbodyConstraints.None;
	}

	void PowerLoss () {
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
		rb.constraints = RigidbodyConstraints.FreezePositionY;

		Debug.Log ("Level Loaded");
		Debug.Log (scene.name);
		Debug.Log (mode);
	}
}