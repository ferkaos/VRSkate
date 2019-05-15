using NewtonVR;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Velocimeter : MonoBehaviour {
    private NVRHand nVRHand;
    public Rigidbody thisRigidbody;
    // Use this for initialization
    void Start () {
        nVRHand = GetComponent<NVRHand>();
        thisRigidbody = GetComponent<Rigidbody>();
    }
	
	// Update is called once per frame
	void Update () {
        thisRigidbody.MovePosition(nVRHand.transform.position);
	}
}
