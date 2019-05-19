using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRInteraction;

public class Velocimeter : MonoBehaviour {
    [SerializeField] private VRInput hand;
    public Rigidbody thisRigidbody;
    public float smoothValue = 5f;
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
        Vector3 direction = (hand.transform.position - transform.position).normalized;
        float distance = Vector3.Distance(hand.transform.position, transform.position);
        thisRigidbody.velocity = (transform.position + direction * distance/smoothValue * Time.deltaTime);
        velocity = thisRigidbody.velocity.magnitude;
	}
}
