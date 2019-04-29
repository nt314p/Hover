using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thruster : MonoBehaviour {
    // Start is called before the first frame update
    private GameObject player;
    private Rigidbody playerRb;
    private readonly float maxThrust = 100;
    private PIDController pidCtrl;

    void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        playerRb = player.GetComponent<Rigidbody>();

        pidCtrl = new PIDController(70f, 70f, 140f, 0f, maxThrust);
    }

    public void setSetpoint(float setpoint) {
        pidCtrl.setSetpoint(setpoint);
    }

    // Update is called once per frame
    void FixedUpdate () {
        //Debug.Log(pidCtrl);
        //pidCtrl.setSetpoint(Player.GetHoverHeight());
        float val = pidCtrl.update(gameObject.transform.position.y, Time.fixedDeltaTime);
        Vector3 frc = new Vector3(Mathf.Sin(gameObject.transform.rotation.eulerAngles.x),
        Mathf.Sin(gameObject.transform.rotation.eulerAngles.y),
        Mathf.Sin(gameObject.transform.rotation.eulerAngles.z));
        Debug.Log(frc);
       //Debug.Log(gameObject.transform.rotation.eulerAngles);
        playerRb.AddForceAtPosition(val * frc, gameObject.transform.position);
        //AddForceAtPosition(Mathf.Max(0,  Player.hoverHeight - player.transform.position.y)/Player.hoverHeight * thrust * Vector3.up, this.transform.position, ForceMode.Force);
    }
}