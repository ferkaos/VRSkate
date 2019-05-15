using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkateController : MonoBehaviour {

    public Rigidbody rigidbody;
    public GameObject head;
    public GameObject tableModel;
    public float tiltMultiplier = 1;
    public float torqueAcelerationMultiplier = 5;

    public float maxSpeed = 5;
    public float minSpeed = 0;

    //public Vector3 relativePositionToSkate;
    private Vector3 torqueForce;

    //public GameObject testPrefab;
    //private GameObject test;

    public HeadPositionSensor headPositionSensor;
    private float AxisX;
    private float AxisZ;
	
	void Start () {
        rigidbody = GetComponent<Rigidbody>();
        //test = Instantiate(testPrefab);
        //test.transform.parent = this.transform;
    }
	
	void Update () {
        //CalculateDistanceHeadSkate();
        //SetTableTilt();
        SetDirection();
    }

    //private void CalculateDistanceHeadSkate() {
    //    //relativePositionToSkate = head.transform.InverseTransformPoint(transform.position);
    //    //test.transform.position = new Vector3(head.transform.position.x, this.transform.position.y, head.transform.position.z);
    //}

    private void CalculateAxisX() {
        AxisX = headPositionSensor.GetAxisX();
    }
    private void CalculateAxisY() {
        AxisZ = headPositionSensor.GetAxisZ();
    }

    private void SetTableTilt() {
        Vector3 rotation = tableModel.transform.eulerAngles;
        tableModel.transform.eulerAngles = new Vector3(90 + (AxisX * tiltMultiplier), -90, -90);
    }

    private void SetDirection() {
        //if (rigidbody.velocity.magnitude > minSpeed) {
            if (rigidbody.angularVelocity.sqrMagnitude < 5) {
                CalculateAxisX();
                torqueForce = new Vector3(0, AxisX * torqueAcelerationMultiplier /** rigidbody.velocity.sqrMagnitude*/, 0);
                rigidbody.AddTorque(torqueForce, ForceMode.Acceleration);
            }
        //}
    }


}
