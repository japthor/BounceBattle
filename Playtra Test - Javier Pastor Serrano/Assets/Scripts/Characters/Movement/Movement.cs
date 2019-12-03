using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public abstract class Movement : MonoBehaviour
{
  #region Variables
  // Reference to the rigidbody.
  private Rigidbody m_RigidBody;
  // Direction to move.
  private Vector3 m_ObjectiveDir;
  // Checks if the unit is moving.
  private bool m_IsMoving;
  /* Checks if the unit is moving, so it can
   * make the movement inside LateUpdate.
   * */
  private bool m_IsUpdating;
  #endregion

  #region Setters/Getters
  public Rigidbody RigidBody
  {
    get { return m_RigidBody; }
  }
  public bool IsMoving
  {
    get { return m_IsMoving; }
    set { m_IsMoving = value; }
  }
  public bool IsUpdating
  {
    get { return m_IsUpdating; }
    set { m_IsUpdating = value; }
  }
  public Vector3 ObjectiveDir
  {
    get { return m_ObjectiveDir; }
    set { m_ObjectiveDir = value; }
  }
  #endregion

  protected virtual void Awake()
  {
    m_RigidBody = GetComponent<Rigidbody>();
  }
  protected virtual void Start()
  {
    m_ObjectiveDir = Vector3.zero;
    m_IsUpdating = false;
    m_IsMoving = false;
  }

  // Checks the distance between an unit and itself.
  protected float CheckDistance(Unit unit)
  {
    Vector3 enemyPos = unit.transform.position;
    Vector3 thisPos = transform.position;

    float distance = Vector3.Distance(enemyPos, thisPos);

    return distance;
  }
  // Checks the distance between a position and itself.
  protected float CheckDistance(Unit unit, Vector3 position)
  {
    Vector3 unitPos = unit.transform.position;

    float distance = Vector3.Distance(unitPos, position);

    return distance;
  }
  // Generates a direction between an unit and itself.
  protected Vector3 GenerateDirection(Unit unit)
  {
    Vector3 enemyPos = unit.transform.position;
    Vector3 thisPos = transform.position;

    Vector3 direction = (enemyPos - thisPos).normalized;

    return direction;
  }
  // Moves the unit.
  protected void MovePosition(Unit unit, Vector3 direction)
  {
    m_RigidBody.MovePosition(m_RigidBody.position + direction * Time.deltaTime * unit.Speed);
  }
  // Stops the actual velocity of the unit.
  protected void StopVelocity()
  {
    m_RigidBody.velocity = Vector3.zero;
  }
  // Collision.
  protected virtual void OnCollisionEnter(Collision collision) { }
  // Pushes the unit.
  protected void CollisionForce(Unit unit, float force)
  {
    Vector3 dir = (unit.transform.position - transform.position).normalized;
    RigidBody.AddForce(-dir * force);
  }
}
