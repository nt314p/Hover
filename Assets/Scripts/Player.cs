using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour {

	// transform and rotation
	public float acceleration = 150f;
	float turnSpeed = 20000f;
	public float turnAngle = 0f;
	public float velocity;

	public static float health = 100f;
	public static float electricity = 1000f;
	public static float forwardVel;
	public static float distance = 0f;
	public static bool dead = false;
	public static bool powerLoss = false;

	// audio
	public AudioSource powerDown;
	bool powerDownPlay = false;

	float distThisFrame;

	public static float playerZ;

	// pickups
	public GameObject electricityPickup;
	public GameObject wrenchPickup;

	Rigidbody rb;


	// Use this for initialization
	void Start () {
		powerDown = GameObject.Find ("/Main Camera/powerDown").GetComponent<AudioSource> ();
		rb = GetComponent<Rigidbody> ();
		rb.angularVelocity = Vector3.zero;
		rb.constraints = RigidbodyConstraints.FreezePositionY;
	}
	
	// Update is called once per frame
	void Update () {
		
		if (!dead && !powerLoss) {
			forwardVel = rb.velocity.z;

			// turning the hovercraft
			transform.eulerAngles = new Vector3 (0, 0, Input.GetAxis ("Horizontal") * -15);

			if (Mathf.Abs (rb.velocity.x) > 300) {
				Debug.Log ("Speed over 300!");
			}

			// moving the hovercraft left and right
			rb.AddForce (Vector3.right * Input.GetAxis ("Horizontal") * turnSpeed * Time.deltaTime);
			// Debug.Log(Input.GetAxis ("Horizontal"));

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

			// setting player's y to 5
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

	public static void AddElectricity () {
		electricity += 50;

		// capping electricity
		if (electricity > 1000) {
			electricity = 1000;
		}
	}

	public static void AddHealth () {
		health += 5;

		// capping health
		if (health > 100) {
			health = 100;
		}
	}

	void Death () {
		rb.constraints = RigidbodyConstraints.None;
		rb.AddTorque (Vector3.up * 100000f);
		rb.AddTorque (Vector3.right * 100000f);
		rb.AddTorque (Vector3.forward * 100000f);
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
		rb = GetComponent<Rigidbody> ();
		rb.angularVelocity = Vector3.zero;
		rb.velocity = Vector3.zero;
		rb.constraints = RigidbodyConstraints.FreezePositionY;
		Debug.Log ("Level Loaded");
		Debug.Log (scene.name);
		Debug.Log (mode);
	}
}
