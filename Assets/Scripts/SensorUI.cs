using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensorUI : MonoBehaviour {

    public HeadPositionSensor headPositionSensor;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.localPosition = new Vector3(headPositionSensor.GetAxisX(), headPositionSensor.GetAxisZ(), 0);
	}
}
