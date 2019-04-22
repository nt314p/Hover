using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	GameObject player;
	GameObject ground;
	// smooth follow variables
	Vector3 target;
	private Vector3 positionVelocity;
    Vector3 offset = new Vector3(0, 26, -30); // 0, 26, -20
	float minHeight = 1f;
    float follow = 0.2f;
	Rigidbody playerR;
	Rigidbody rb;
	float maxDistX = 70f;
	readonly float groundOffset = 571.8f;
	

	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player");
		ground = GameObject.Find("Ground");
		playerR = player.GetComponent<Rigidbody>();
		rb = GetComponent<Rigidbody>();
	}

	void LateUpdate () {
		ground.transform.position = new Vector3(player.transform.position.x, ground.transform.position.y, Player.GetPlayerZ()+groundOffset);
    	// acceleration = (r.velocity - lastVelocity) / Time.fixedDeltaTime;
    	// lastVelocity = r.velocity;

		if (!Player.IsDead() && !Player.IsPowerLoss()) {
			// Vector3 newPosition = player.transform.position + offset;
			// newPosition.y = Mathf.Max (newPosition.y + offset.y, minHeight);
			// transform.position = Vector3.SmoothDamp(transform.position, newPosition, ref positionVelocity, follow);

			//float dist = Vector3.Distance(this.transform.position, player.transform.position);
			//rb.AddForce((offset+player.transform.position-this.transform.position)*dist*0.1f);

			target = player.transform.position + offset;
			Vector3 newPosition = Vector3.SmoothDamp(transform.position, target, ref positionVelocity, follow);
			float rawDistX = newPosition.x - player.transform.position.x;
			if (Mathf.Abs(rawDistX) > maxDistX) {
				newPosition.x = player.transform.position.x + Mathf.Sign(rawDistX) * maxDistX;
			}
			transform.position = newPosition;
		}
	}
}
