using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour {

	float l;
	float h;

	// Use this for initialization
	void Start () {
		l = Random.Range (10, 30);
		h = Random.Range (30, 200);

		transform.position = new Vector3 (transform.position.x, (h - 6)/2, transform.position.z);
		transform.localScale = new Vector3 (l, h, 10);
	}
}
