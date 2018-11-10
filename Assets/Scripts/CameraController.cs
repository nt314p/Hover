using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	GameObject player;
	GameObject ground;
	// smooth follow variables
	Vector3 target;
	private Vector3 positionVelocity;
    Vector3 offset = new Vector3(0, 26, -20);
    float follow = 0.15f;
	Rigidbody playerR;
	Rigidbody rb;
	float scale = 0.05f;
	Vector3 acceleration;
	Vector3 lastVelocity;
	

	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player");
		ground = GameObject.Find("Ground");
		playerR = player.GetComponent<Rigidbody>();
		rb = GetComponent<Rigidbody>();
	}

	void FixedUpdate () {
		ground.transform.position = new Vector3(ground.transform.position.x, ground.transform.position.y, Player.playerZ+571.8f);
    	// acceleration = (r.velocity - lastVelocity) / Time.fixedDeltaTime;
    	// lastVelocity = r.velocity;

		if (!Player.dead && !Player.powerLoss) {
			float dist = Vector3.Distance(this.transform.position, player.transform.position);
			rb.AddForce((offset+player.transform.position-this.transform.position)*dist*0.1f);
			// target = player.transform.position + acceleration*scale + offset;
			// transform.position = Vector3.SmoothDamp(transform.position, target, ref positionVelocity, follow);
		}
	}
}
