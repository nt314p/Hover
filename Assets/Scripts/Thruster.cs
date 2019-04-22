using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thruster : MonoBehaviour {
    // Start is called before the first frame update
    GameObject player;
    Rigidbody playerRb;
    float thrust = 70;
    PIDController pidCtrl;

    void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        playerRb = player.GetComponent<Rigidbody>();

        pidCtrl = new PIDController(1f, 1f, 2f, 0f, 100f);
    }

    void setThrust (float thrust) {
        this.thrust = thrust;
    }

    // Update is called once per frame
    void FixedUpdate () {
        //Debug.Log(pidCtrl);
        pidCtrl.setSetpoint(Player.GetHoverHeight());
        float val = pidCtrl.update(player.transform.position.y, Time.fixedDeltaTime);
        playerRb.AddRelativeForce(0f, Mathf.Max(0, val) * thrust, 0f);
        //AddForceAtPosition(Mathf.Max(0,  Player.hoverHeight - player.transform.position.y)/Player.hoverHeight * thrust * Vector3.up, this.transform.position, ForceMode.Force);
    }
}