
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VRInteraction;

public class RemoteAcelerator : MonoBehaviour {

    public float aceleration;
    public HeadPositionSensor headPositionSensor;
    public bool modeAcelerator;

    private VRInput hand;
    private Velocimeter velocimeter;
    private SkateController skateController;
    private Rigidbody rigidbodyHand;

    private float maxImpulse;

    [SerializeField] Text speedText;
    [SerializeField] Text impulseText;
    [SerializeField] GameObject UICenter;

    // Use this for initialization
    void Start() {
        hand = GetComponent<VRInput>();
        rigidbodyHand = GetComponent<Rigidbody>();
        skateController = FindObjectOfType<SkateController>();
        velocimeter = FindObjectOfType<Velocimeter>();
    }

    // Update is called once per frame
    void Update() {
        Debug.Log(skateController.m_body.velocity);
        if (hand.TriggerPressed) {
            if (skateController.m_body.velocity.magnitude < skateController.maxSpeed) {
                Acelerate();
            } else {
                //skateController.m_body.velocity = Vector3.forward * skateController.maxSpeed;
            }
        } else {
            maxImpulse = 0;
            if (skateController.m_body.velocity.magnitude < skateController.minSpeed) {
                skateController.m_body.velocity = Vector3.zero;
            } else {
                //Decelerate();
            }
        }
        if (hand.PadPressed) {
            headPositionSensor.RecenterHead();
        }
        speedText.text = "Speed: " + skateController.m_body.velocity.magnitude.ToString();
    }

    private void Acelerate() {
        if (modeAcelerator) {
            skateController.m_body.AddForce(skateController.transform.forward * aceleration);
            return;
        }
        Vector3 impulseforce = Vector3.Project(velocimeter.thisRigidbody.velocity, skateController.transform.forward + skateController.m_body.velocity);
        float force = impulseforce.magnitude - skateController.m_body.velocity.magnitude;
        if(impulseforce.z > 0) {
            force *= -1;
        }
        if (Mathf.Abs(force) >= Mathf.Abs(maxImpulse)) {
            maxImpulse = force;
            skateController.m_body.AddForce(skateController.transform.forward * force * aceleration, ForceMode.Acceleration);
            impulseText.text = "Impulse: " + force.ToString();
            DrawImpulse(impulseforce);
        }
    }

    private void DrawImpulse(Vector3 impulse) {
        Vector3 impulseEnd = new Vector3(impulse.x, impulse.z, impulse.y);
        Debug.DrawLine(UICenter.transform.position, impulseEnd, Color.cyan);
    }

    private void Decelerate() {
        skateController.m_body.AddForce(-skateController.transform.forward, ForceMode.Acceleration);
    }
}
