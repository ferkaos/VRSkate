﻿using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class HoverCarControl : MonoBehaviour
{
  Rigidbody m_body;
  float m_deadZone = 0.1f;

  public float m_hoverForce = 9.0f;
  public float m_hoverHeight = 2.0f;
  public GameObject[] m_hoverPoints;

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

    [Header("Debug")]
    public bool withShifting;
    public bool withInclination = true;
    [Range(-50, 50)] public int inclination;
    public float inclinationForce = 2;

  void Start()
  {
    m_body = GetComponent<Rigidbody>();

    m_layerMask = 1 << LayerMask.NameToLayer("Characters");
    m_layerMask = ~m_layerMask;

    staticDirectionVelocityAssistancePercent = directionVelocityAssistancePercent;
  }

  void OnDrawGizmos()
  {

    //  Hover Force
    RaycastHit hit;
    for (int i = 0; i < m_hoverPoints.Length; i++)
    {
      var hoverPoint = m_hoverPoints [i];
      if (Physics.Raycast(hoverPoint.transform.position, 
                          -hoverPoint.transform.up, out hit,
                          m_hoverHeight, 
                          m_layerMask))
      {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(hoverPoint.transform.position, hit.point);
        Gizmos.DrawSphere(hit.point, 0.5f);
      } else
      {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(hoverPoint.transform.position, 
                       hoverPoint.transform.position - hoverPoint.transform.up * m_hoverHeight);
      }
    }
  }
	
  void Update() {
        KeyBoardControl();
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

  void FixedUpdate()
  {

    //  Hover Force
    RaycastHit hit;
    for (int i = 0; i < m_hoverPoints.Length; i++)
    {
      var hoverPoint = m_hoverPoints [i];
      if (Physics.Raycast(hoverPoint.transform.position, 
                          -hoverPoint.transform.up, out hit,
                          m_hoverHeight,
                          m_layerMask))
        m_body.AddForceAtPosition(hoverPoint.transform.up * m_hoverForce * (1.0f - (hit.distance / m_hoverHeight)), 
                                  hoverPoint.transform.position);
      else
      {
        if (transform.position.y > hoverPoint.transform.position.y)
          m_body.AddForceAtPosition(
            hoverPoint.transform.up * m_hoverForce,
            hoverPoint.transform.position);
        else
          m_body.AddForceAtPosition(
            hoverPoint.transform.up * -m_hoverForce,
            hoverPoint.transform.position);
      }
    }
        if (withDirectionVelocityAssistance) {
            Vector3 onNormal = Vector3.Lerp(m_body.velocity, transform.forward, directionVelocityAssistancePercent);
            m_body.velocity = Vector3.Project(m_body.velocity, onNormal);
        }

    // Forward
    if (Mathf.Abs(m_currThrust) > 0)
      m_body.AddForce(transform.forward * m_currThrust);

    // Turn
    if (m_currTurn > 0)
    {
      m_body.AddRelativeTorque(Vector3.up * m_currTurn * m_turnStrength);
    } else if (m_currTurn < 0)
    {
      m_body.AddRelativeTorque(Vector3.up * m_currTurn * m_turnStrength);
    }

        //Shift Movement Controller(Velocity direction assistance conflicts)
        if (withDirectionVelocityAssistance && withShifting)  {
            Vector3 forwardVelocity = Vector3.Project(m_body.velocity, transform.forward);
            directionVelocityAssistancePercent = Mathf.Lerp(0, staticDirectionVelocityAssistancePercent, Mathf.Clamp(forwardVelocity.sqrMagnitude / forwardSpeedToDirectionVelocityAssistance, 0, 1));
        }
        //Inclination Movement Controller)
        if (withInclination) {
            m_body.AddRelativeTorque(m_body.transform.forward * inclination * inclinationForce);
        }
        
    }
}
