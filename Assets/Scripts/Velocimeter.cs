using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRInteraction;

public class Velocimeter : MonoBehaviour {
    [SerializeField] private VRInput hand;
    public Rigidbody thisRigidbody;
    public float velocity;
    // Use this for initialization
    void Start () {
        hand = GetComponent<VRInput>();
        thisRigidbody = GetComponent<Rigidbody>();
    }
	
	// Update is called once per frame
	void FixedUpdate() {
        if(hand == null) {
            hand = FindObjectOfType<RemoteAcelerator>().GetComponent<VRInput>();
        }
        thisRigidbody.MovePosition(hand.transform.position);
        velocity = thisRigidbody.velocity.magnitude;
	}
}
