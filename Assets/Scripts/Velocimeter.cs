using NewtonVR;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Velocimeter : MonoBehaviour {
    [SerializeField] private NVRHand nVRHand;
    public Rigidbody thisRigidbody;
    public float velocity;
    // Use this for initialization
    void Start () {
        nVRHand = GetComponent<NVRHand>();
        thisRigidbody = GetComponent<Rigidbody>();
    }
	
	// Update is called once per frame
	void FixedUpdate() {
        if(nVRHand == null) {
            nVRHand = FindObjectOfType<RemoteAcelerator>().GetComponent<NVRHand>();
        }
        thisRigidbody.MovePosition(nVRHand.transform.position);
        velocity = thisRigidbody.velocity.magnitude;
	}
}
