using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Unit))]
public abstract class Movement : MonoBehaviour
{
  #region Variables
  // Direction to move.
  private Vector3 m_ObjectiveDir;
  // Checks if the unit is moving.
  private bool m_IsMovingNormal;
  #endregion

  #region Setters/Getters
  public bool IsMovingNormal
  {
    get { return m_IsMovingNormal; }
    set { m_IsMovingNormal = value; }
  }
  public Vector3 ObjectiveDir
  {
    get { return m_ObjectiveDir; }
    set { m_ObjectiveDir = value; }
  }
  #endregion

  protected virtual void Start()
  {
    m_ObjectiveDir = Vector3.zero;
    IsMovingNormal = false;
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
    if (IsMovingNormal && unit != null)
      unit.RigidBody.MovePosition(unit.RigidBody.position + direction * Time.fixedDeltaTime * unit.Speed);
  }
  // Reduces the velocity
  protected void ReduceVelocity(Unit unit, float minimum)
  {
    if (unit.RigidBody.velocity.magnitude > minimum)
      unit.RigidBody.velocity -= unit.RigidBody.velocity * Time.fixedDeltaTime;
  }
  // Checks if the velocity has been reduced.
  protected bool IsVelocityReduced(Unit unit, float minimum)
  {
    if (unit.RigidBody.velocity.magnitude > minimum)
      return false;

    return true;
  }
  // Stops the actual velocity of the unit.
  protected void StopVelocity(Unit unit)
  {
    unit.RigidBody.velocity = Vector3.zero;
  }
  // Collision with another unit.
  protected void UnitPush(Unit unit, float force)
  {
    Vector3 dir = GenerateDirection(unit);
    unit.RigidBody.velocity = dir * force;
  }
  // Collision with Edge.
  protected void EdgePush(Unit unit, float force)
  {
    Vector3 dir = (GameManager.m_Instance.Map.transform.position - transform.position).normalized;
    unit.RigidBody.velocity = -dir * force;
  }

  // Collision logic.
  protected virtual void OnCollisionEnter(Collision collision){}
}
