using NewtonVR;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoteAcelerator : MonoBehaviour {

    public float aceleration;
    public HeadPositionSensor headPositionSensor;

    private NVRHand nVRHand;
    private SkateController skateController;

	// Use this for initialization
	void Start () {
        nVRHand = GetComponent<NVRHand>();
        skateController = FindObjectOfType<SkateController>();
	}
	
	// Update is called once per frame
	void Update () {
        Debug.Log(skateController.rigidbody.velocity);
        if (nVRHand.UseButtonPressed) {
            if (skateController.rigidbody.velocity.magnitude < skateController.maxSpeed) {
                Acelerate();
            } else {
                skateController.rigidbody.velocity = Vector3.forward * skateController.maxSpeed;
            }
        } else {
            if(skateController.rigidbody.velocity.magnitude < skateController.minSpeed) {
                skateController.rigidbody.velocity = Vector3.zero;
            } else {
                //Decelerate();
            }            
        }
        if (nVRHand.HoldButtonDown) {
            headPositionSensor.RecenterHead();
        }
	}

    private void Acelerate() {
        skateController.rigidbody.AddForce(skateController.transform.forward,ForceMode.Acceleration);
    }

    private void Decelerate() {
        skateController.rigidbody.AddForce(-skateController.transform.forward, ForceMode.Acceleration);
    }
}
