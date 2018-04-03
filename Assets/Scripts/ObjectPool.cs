using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour {

	// the object pools
	GameObject[,] obstaclePool;
	GameObject[,] wrenchPool;
	GameObject[,] electricityPool;

	public GameObject obstacle;
	public GameObject electricityPickup;
	public GameObject wrenchPickup;
	GameObject player;

	float forwardVel; // the velocity of the player

	// obstacle generation
	float obsOffset = 1600; // the original offset (new obstacles will spawn [x] m in the distance)
	float obsRange = 4000; // the left or right maximum spawn 
	public float obsEveryDist = 200f;
	public float everyDistCounter = 0;
	float distThisFrame;

	int obsDensity = 80; // number of objects to distribute across the range
	int wrenchDensity = 3;
	int electricityDensity = 16;
	public int rowNum = 0; // 0 - 6 is 1 - 7
	int rows; // how many rows of obstacles can exist at once

	void Start () {

		rows = (int)(obsOffset / obsEveryDist) + 2; // setting rows

		player = GameObject.FindGameObjectWithTag ("Player");

		// sizing object pools
		obstaclePool = new GameObject[obsDensity, rows];
		wrenchPool = new GameObject[wrenchDensity, rows];
		electricityPool = new GameObject[electricityDensity, rows];

		// filling object pools
		for (int j = 0; j < rows; j++) {
			for (int i = 0; i < obsDensity; i++) {
				obstaclePool [i, j] = Instantiate (obstacle, new Vector3 (0, 0, -obsOffset), Quaternion.identity);
			}
			for (int i = 0; i < wrenchDensity; i++) {
				wrenchPool [i, j] = Instantiate (wrenchPickup, new Vector3 (0, 0, -obsOffset), Quaternion.identity);
			}
			for (int i = 0; i < electricityDensity; i++) {
				electricityPool [i, j] = Instantiate (electricityPickup, new Vector3 (0, 0, -obsOffset), Quaternion.identity);
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (!Player.dead && !Player.powerLoss) {
			forwardVel = player.GetComponent<Rigidbody> ().velocity.z;

			// spawning objects
			if (everyDistCounter >= obsEveryDist) {
				everyDistCounter = 0;
				spawnRow ();
			}

			distThisFrame = Mathf.Round (100 * forwardVel * Time.deltaTime) / 100f;
			everyDistCounter += distThisFrame;
		}
	}

	public void spawnRow(){
		float offsetZ = player.transform.position.z + obsOffset;

		// spawning obstacles
		for (int i = 0; i < obsDensity; i++) {
			// setting x of obstacle
			float obsX = Random.Range (-obsRange, obsRange) + player.transform.position.x;

			// randomizing width and height
			float w = Random.Range (10, 40);
			float h = Random.Range (30, 180);

			// spawning obstacle
			obstaclePool[i, rowNum].transform.localScale = new Vector3 (w, h, 20); // scaling obstacle
			obstaclePool[i, rowNum].transform.position = new Vector3 (obsX, (h/2 - 6), offsetZ);
			obstaclePool[i, rowNum].transform.rotation = Quaternion.identity;
		}

		for (int i = 0; i < wrenchDensity; i++) {
			float pkupX = Random.Range (-obsRange, obsRange) + player.transform.position.x;

			// spawning wrench pickup
			wrenchPool[i, rowNum].SetActive(true); // enabling
			wrenchPool[i, rowNum].transform.position = new Vector3 (pkupX, 10f, offsetZ);
			wrenchPool[i, rowNum].transform.rotation = Quaternion.identity;
		}

		for (int i = 0; i < electricityDensity; i++) {
			float pkupX = Random.Range (-obsRange, obsRange) + player.transform.position.x;

			// spawning electricity pickup
			electricityPool[i, rowNum].SetActive(true); // enabling
			electricityPool[i, rowNum].transform.position = new Vector3 (pkupX, 10f, offsetZ);
			electricityPool[i, rowNum].transform.rotation = Quaternion.identity;
		}

		rowNum++;

		// resetting the row counter
		if (rowNum >= rows) {
			rowNum = 0;
		}
	}
}
