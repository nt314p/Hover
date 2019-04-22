using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour {

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
	public float obsEveryDist = 200f;
	public float everyDistCounter = 0;
	float distThisFrame;

	int obsDensity = 60; // number of objects to distribute across the range
	int wrenchDensity = 2;
	int electricityDensity = 14;
	public int rowNum = 0; // 0 - 6 is 1 - 7
	int rows; // how many rows of obstacles can exist at once

	float singleObsP = 0.3f;
	float doubleObsP = 0.1f;
	// pickups only spawn if an obstacle does not
	float electricityP = 0.3f;
	float wrenchP = 0.1f;
	// should both probabilities be true, a double will spawn, probabilities will be calculated twice

	int chunks = 100;
	int chuckSize = 50;

	void Start () {

		rows = (int)(obsOffset / obsEveryDist) + 2; // setting rows

		player = GameObject.FindGameObjectWithTag ("Player");

		// sizing object pools
		obstaclePool = new GameObject[chunks, rows];
		wrenchPool = new GameObject[chunks, rows];
		electricityPool = new GameObject[chunks, rows];

		// filling object pools
		for (int j = 0; j < rows; j++) {
			for (int i = 0; i < chunks; i++) {
				obstaclePool [i, j] = Instantiate (obstacle, new Vector3 (0, 0, -obsOffset), Quaternion.identity);
				wrenchPool [i, j] = Instantiate (wrenchPickup, new Vector3 (0, 0, -obsOffset), Quaternion.identity);
				electricityPool [i, j] = Instantiate (electricityPickup, new Vector3 (0, 0, -obsOffset), Quaternion.identity);
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (!Player.IsDead() && !Player.IsPowerLoss()) {
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

	public void spawnObject(GameObject obj, float objX, bool isPickup) {;

		// calculating obstacle z value
		float objZ = player.transform.position.z + obsOffset;

		float w = 1;
		float h = 1;
		float l = isPickup ? 1 : 20;

		if (!isPickup) {
			// randomizing width and height
			w = Random.Range (10, 40);
			h = Random.Range (30, 180);
		}

		obj.transform.localScale = new Vector3 (w, h, l); // scaling obstacle

		float objY = isPickup ? 10f : (h / 2.0f - 2);

		obj.transform.position = new Vector3 (objX, objY, objZ);
		obj.transform.rotation = Quaternion.identity;
	}

	public void spawnRow(){
		//Debug.Log("row spawn begun");
		float offsetZ = player.transform.position.z + obsOffset;

		// iterating though chunks
		int chunkRange = chunks * chuckSize;
		int obsCtr = 0;
		int elecCtr = 0;
		int wrenchCtr = 0;
		for (int x = chuckSize/2; x < chunkRange; x+=chuckSize) {
			float xOffset = player.transform.position.x - chunkRange/2 + Random.Range(-10, 10);

			float randomObsRange = Random.Range(0f, 1f);
			//Debug.Log("Random value:" + randomObsRange);
			if (randomObsRange < singleObsP) { // single obstacle
				//Debug.Log("Single Obs");
				spawnObject(obstaclePool[obsCtr, rowNum], x + xOffset, false);
				obsCtr++;
			} else if (randomObsRange < singleObsP + doubleObsP) { // double obstacle
				//Debug.Log("Double Obs");
				float obs1XOffset = Random.Range(-20, 20);
				float obs2XOffset = Random.Range(-20, 20);
				spawnObject(obstaclePool[obsCtr, rowNum], x + xOffset + obs1XOffset, false);
				obsCtr++;
				spawnObject(obstaclePool[obsCtr, rowNum], x + xOffset + obs2XOffset, false);
				obsCtr++;
			} else { // no obstacles have spawned, spawn pickups
				//Debug.Log("Pickups");
				float randomPickupRange = Random.Range(0f, 1f);
				if (randomPickupRange < electricityP) { // single electricity
					spawnObject(electricityPool[elecCtr, rowNum], x + xOffset, true);
					elecCtr++;
				} else if (randomPickupRange < electricityP + wrenchP) { // single wrench
					spawnObject(wrenchPool[elecCtr, rowNum], x + xOffset, true);
					wrenchCtr++;
				}
			}
			
		}

		rowNum++;

		// resetting the row counter
		if (rowNum >= rows) {
			rowNum = 0;
		}

		/*
		// spawning obstacles
		for (int i = 0; i < obsDensity; i++) {
			// setting x of obstacle
			float obsX = Random.Range (-obsRange, obsRange) + player.transform.position.x;

			// randomizing width and height
			float w = Random.Range (10, 40);
			float h = Random.Range (30, 180);

			// spawning obstacle
			obstaclePool[i, rowNum].transform.localScale = new Vector3 (w, h, 20); // scaling obstacle
			obstaclePool[i, rowNum].transform.position = new Vector3 (obsX, (h / 2 - 2), offsetZ);
			obstaclePool[i, rowNum].transform.rotation = Quaternion.identity;
		}

		for (int i = 0; i < wrenchDensity; i++) {
			float pkupX = Random.Range (-obsRange, obsRange) + player.transform.position.x;

			// spawning wrench pickup
			wrenchPool[i, rowNum].SetActive(true); // enabling
			wrenchPool[i, rowNum].transform.position = new Vector3 (pkupX, 10f, offsetZ);
			wrenchPool[i, rowNum].transform.rotation = Quaternion.Euler(0, 0, 90);
		}

		for (int i = 0; i < electricityDensity; i++) {
			float pkupX = Random.Range (-obsRange, obsRange) + player.transform.position.x;

			// spawning electricity pickup
			electricityPool[i, rowNum].SetActive(true); // enabling
			electricityPool[i, rowNum].transform.position = new Vector3 (pkupX, 10f, offsetZ);
			electricityPool[i, rowNum].transform.rotation = Quaternion.Euler(0, 180, 0);
		}
		*/
		
	}
}
