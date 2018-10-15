using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	GameObject player;

	// smooth follow variables
	Vector3 target;
	private Vector3 positionVelocity;
    Vector3 offset = new Vector3(0, 26, -40);
    float follow = 0.15f;

	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player");
	}

	void FixedUpdate () {
		if (!Player.dead && !Player.powerLoss) {
            
			target = player.transform.position + offset;
			transform.position = Vector3.SmoothDamp(transform.position, target, ref positionVelocity, follow);
		}
	}
}
