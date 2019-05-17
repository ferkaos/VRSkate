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
    private Vector3 torqueForce;

    public HeadPositionSensor headPositionSensor;
    private float AxisX;
    private float AxisZ;

    Rigidbody m_body;
    float m_deadZone = 0.1f;

    [Header("Hover")]
    public float m_hoverForce = 9.0f;
    public float m_hoverHeight = 2.0f;
    public GameObject[] m_hoverPoints;
    public float hoverPointsDistance = 0.5f;

    public float m_forwardAcl = 100.0f;
    public float m_backwardAcl = 25.0f;
    float m_currThrust = 0.0f;

    public float m_turnStrength = 10f;
    float m_currTurn = 0.0f;
    public bool withDirectionVelocityAssistance;
    [Range(0, 1)] public float directionVelocityAssistancePercent;
    private float staticDirectionVelocityAssistancePercent;
    public float forwardSpeedToDirectionVelocityAssistance;

    public GameObject m_leftAirBrake;
    public GameObject m_rightAirBrake;

    int m_layerMask;

    [Header("Shift")]
    public bool withShifting;
    public bool withInclination = true;
    [Range(-50, 50)] public int inclination;
    public float inclinationForce = 2;

    void Start () {
        rigidbody = GetComponent<Rigidbody>();
        m_body = GetComponent<Rigidbody>();

        m_layerMask = 1 << LayerMask.NameToLayer("Characters");
        m_layerMask = ~m_layerMask;

        staticDirectionVelocityAssistancePercent = directionVelocityAssistancePercent;
    }

    void FixedUpdate() {

        //  Hover Force
        RaycastHit hit;
        bool anyHoverPointHit = false;
        for (int i = 0; i < m_hoverPoints.Length; i++) {
            var hoverPoint = m_hoverPoints[i];
            if (Physics.Raycast(hoverPoint.transform.position,
                                -hoverPoint.transform.up, out hit,
                                m_hoverHeight,
                                m_layerMask)) {
                anyHoverPointHit = true;
                m_body.AddForceAtPosition(hoverPoint.transform.up * m_hoverForce * (1.0f - (hit.distance / m_hoverHeight)),
                                          hoverPoint.transform.position);
            } else {
                Vector3 inclinationForceDirection = Vector3.ProjectOnPlane(transform.up, Vector3.up);
                m_body.AddForceAtPosition(inclinationForceDirection * m_hoverForce,
                                      transform.position);
                //if (transform.position.y > hoverPoint.transform.position.y)
                //    m_body.AddForceAtPosition(
                //      hoverPoint.transform.up * m_hoverForce,
                //      hoverPoint.transform.position);
                //else
                //    m_body.AddForceAtPosition(
                //      hoverPoint.transform.up * -m_hoverForce,
                //      hoverPoint.transform.position);
            }
        }
        if (withDirectionVelocityAssistance && anyHoverPointHit) {
            Vector3 onNormal = Vector3.Lerp(m_body.velocity, transform.forward, directionVelocityAssistancePercent);
            Vector3 velocityProjected = Vector3.Project(m_body.velocity, onNormal);
            if (velocityProjected.sqrMagnitude > forwardSpeedToDirectionVelocityAssistance) {
                m_body.velocity = velocityProjected;
            }
        }

        // Forward
        if (Mathf.Abs(m_currThrust) > 0)
            m_body.AddForce(transform.forward * m_currThrust);

        // Turn
        if (m_currTurn > 0) {
            m_body.AddRelativeTorque(Vector3.up * m_currTurn * m_turnStrength);
        } else if (m_currTurn < 0) {
            m_body.AddRelativeTorque(Vector3.up * m_currTurn * m_turnStrength);
        }

        //Shift Movement Controller(Velocity direction assistance conflicts)
        if (withDirectionVelocityAssistance && withShifting) {
            Vector3 forwardVelocity = Vector3.Project(m_body.velocity, transform.forward);
            directionVelocityAssistancePercent = Mathf.Lerp(0, staticDirectionVelocityAssistancePercent, Mathf.Clamp(forwardVelocity.sqrMagnitude / forwardSpeedToDirectionVelocityAssistance, 0, 1));
        }
        //Inclination Movement Controller)
        if (withInclination) {
            m_body.AddRelativeTorque(m_body.transform.forward * inclination * inclinationForce);
        }

    }

    void Update () {
        //SetDirection();
        //if (setDirection) {
        //    SetVelocityDirection();
        //}
        KeyBoardControl();
        //SituateHoverpoints();
    }

    private void SituateHoverpoints() {
        foreach (GameObject hoverpoint in m_hoverPoints) {
            Vector3 tablePosition = new Vector3(transform.position.x, hoverpoint.transform.position.y, transform.position.z);
            Vector3 direction = hoverpoint.transform.position - tablePosition;
            hoverpoint.transform.position = tablePosition + direction * hoverPointsDistance;
        }
    }

    private void KeyBoardControl() {
        // Main Thrust
        m_currThrust = 0.0f;
        float aclAxis = Input.GetAxis("Vertical");
        if (aclAxis > m_deadZone)
            m_currThrust = aclAxis * m_forwardAcl;
        else if (aclAxis < -m_deadZone)
            m_currThrust = aclAxis * m_backwardAcl;

        // Turning
        m_currTurn = 0.0f;
        float turnAxis = Input.GetAxis("Horizontal");
        if (Mathf.Abs(turnAxis) > m_deadZone)
            m_currTurn = turnAxis;
    }

    void OnDrawGizmos() {

        //  Hover Force
        RaycastHit hit;
        for (int i = 0; i < m_hoverPoints.Length; i++) {
            var hoverPoint = m_hoverPoints[i];
            if (Physics.Raycast(hoverPoint.transform.position,
                                -hoverPoint.transform.up, out hit,
                                m_hoverHeight,
                                m_layerMask)) {
                Gizmos.color = Color.blue;
                Gizmos.DrawLine(hoverPoint.transform.position, hit.point);
                Gizmos.DrawSphere(hit.point, 0.05f);
            } else {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(hoverPoint.transform.position,
                               hoverPoint.transform.position - hoverPoint.transform.up * m_hoverHeight);
            }
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
