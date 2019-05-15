using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadPositionSensor : MonoBehaviour {

    private Vector3 center;
    private Vector3 headPosition;
    public GameObject head;
	// Use this for initialization
	void Start () {
        RecenterHead();
	}
	
	// Update is called once per frame
	void Update () {
        headPosition = head.transform.localPosition;
	}

    public float GetAxisX() {
        return headPosition.x - center.x;
    }

    public float GetAxisZ() {
        return headPosition.z - center.z;
    }

    public void RecenterHead() {
        center = headPosition;
    }
}
