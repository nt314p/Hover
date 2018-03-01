using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	GameObject player;

	// smooth follow
	Vector3 target;
	private Vector3 positionVelocity;

	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player");
	}

	void FixedUpdate () {
		if (!Player.dead && !Player.powerLoss) {
			target = player.transform.position + new Vector3 (0, 25, -50);
			transform.position = Vector3.SmoothDamp(transform.position, target, ref positionVelocity, 0.15f);
		}
	}
}
