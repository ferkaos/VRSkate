using NewtonVR;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RemoteAcelerator : MonoBehaviour {

    public float aceleration;
    public HeadPositionSensor headPositionSensor;

    private NVRHand nVRHand;
    private Velocimeter velocimeter;
    private SkateController skateController;
    private Rigidbody rigidbodyHand;

    private float maxImpulse;

    [SerializeField] Text speedText;
    [SerializeField] Text impulseText;
    [SerializeField] GameObject UICenter;

    // Use this for initialization
    void Start() {
        nVRHand = GetComponent<NVRHand>();
        rigidbodyHand = GetComponent<Rigidbody>();
        skateController = FindObjectOfType<SkateController>();
        velocimeter = FindObjectOfType<Velocimeter>();
    }

    // Update is called once per frame
    void Update() {
        Debug.Log(skateController.rigidbody.velocity);
        if (nVRHand.UseButtonPressed) {
            if (skateController.rigidbody.velocity.magnitude < skateController.maxSpeed) {
                Acelerate();
            } else {
                skateController.rigidbody.velocity = Vector3.forward * skateController.maxSpeed;
            }
        } else {
            maxImpulse = 0;
            if (skateController.rigidbody.velocity.magnitude < skateController.minSpeed) {
                skateController.rigidbody.velocity = Vector3.zero;
            } else {
                //Decelerate();
            }
        }
        if (nVRHand.HoldButtonDown) {
            headPositionSensor.RecenterHead();
        }
        speedText.text = "Speed: " + skateController.rigidbody.velocity.magnitude.ToString();
    }

    private void Acelerate() {
        Vector3 impulseforce = Vector3.Project(velocimeter.thisRigidbody.velocity, skateController.transform.forward + skateController.rigidbody.velocity);
        float force = impulseforce.magnitude - skateController.rigidbody.velocity.magnitude;
        if (force >= maxImpulse) {
            maxImpulse = force;
            skateController.rigidbody.AddForce(skateController.transform.forward * force * aceleration, ForceMode.Acceleration);
            impulseText.text = "Impulse: " + force.ToString();
            DrawImpulse(impulseforce);
        }
    }

    private void DrawImpulse(Vector3 impulse) {
        Vector3 impulseEnd = new Vector3(impulse.x, impulse.z, impulse.y);
        Debug.DrawLine(UICenter.transform.position, impulseEnd, Color.cyan);
    }

    private void Decelerate() {
        skateController.rigidbody.AddForce(-skateController.transform.forward, ForceMode.Acceleration);
    }
}
