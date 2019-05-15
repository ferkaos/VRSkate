using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkateController : MonoBehaviour {
    public bool setDirection;
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
    }
	
	void Update () {
        SetDirection();
        if (setDirection) {
            SetVelocityDirection();
        }
    }

    private void SetVelocityDirection() {
        transform.rotation = Quaternion.LookRotation(rigidbody.velocity, Vector3.up);
    }

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
        if (rigidbody.angularVelocity.sqrMagnitude < maxSpeed && rigidbody.velocity.sqrMagnitude > 0.5f) {
            CalculateAxisX();
            torqueForce = new Vector3(0, AxisX * torqueAcelerationMultiplier /** rigidbody.velocity.sqrMagnitude*/, 0);
            float forceSmoother = Mathf.Clamp(1 - rigidbody.angularVelocity.sqrMagnitude / maxSpeed, 0.25f, 1);
            torqueForce *= forceSmoother;
            rigidbody.AddTorque(torqueForce, ForceMode.Acceleration);
        }
    }


}
