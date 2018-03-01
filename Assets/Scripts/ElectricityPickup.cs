using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricityPickup : MonoBehaviour {

	// Update is called once per frame
	void Update () {
		if (Player.playerZ > transform.position.z + 100) {
			Destroy (this.gameObject);
		}
	}

	void OnTriggerEnter(Collider other){
		if (other.gameObject.CompareTag("Player")) {
			Player.AddElectricity ();
		}
		Destroy (this.gameObject);
	}
}
