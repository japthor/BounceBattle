using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FighterMovement : Movement
{
  #region Variables
  // Last position of the fighter unit.
  private Vector3 m_LastPosition;
  // The travelled distance during a movement.
  private float m_TravelledDistance;
  // The distance to travel.
  private float m_DistanceToTravel;
  // The stamina used to make the movement.
  private int m_StaminaUsed;
  // Checks if the fighter is moving because of a collision.
  private bool m_IsMovingCollision;
  // Checks if the movement has initialized before setting some variables.
  private bool m_HasInitialized;
  #endregion

  #region Setters/Getters
  public Vector3 LastPosition
  {
    get { return m_LastPosition; }
    set { m_LastPosition = value; }
  }
  public float TravelledDistance
  {
    get { return m_TravelledDistance; }
    set { m_TravelledDistance = value; }
  }
  public float DistanceToTravel
  {
    get { return m_DistanceToTravel; }
    set { m_DistanceToTravel = value; }
  }
  public int StaminaUsed
  {
    get { return m_StaminaUsed; }
    set { m_StaminaUsed = value; }
  }
  public bool IsMovingCollision
  {
    get { return m_IsMovingCollision; }
    set { m_IsMovingCollision = value; }
  }
  public bool HasInitialized
  {
    get { return m_HasInitialized; }
    set { m_HasInitialized = value; }
  }
  #endregion

  protected override void Start()
  {
    base.Start();

    m_LastPosition = Vector3.zero;
    m_TravelledDistance = 0.0f;
    m_DistanceToTravel = 0.0f;
    m_IsMovingCollision = false;
    m_LastPosition = transform.position;
    m_StaminaUsed = 0;
    HasInitialized = false;
  }
  // Sets the velocity of the fighter unit.
  protected void SetVelocity(Fighter unit)
  {
    unit.Speed = (unit.MaxSpeed / unit.MaxStamina) * m_StaminaUsed;
  }
  // Generates the distance to travel.
  protected void GenerateDistanceToTravel(Fighter unit)
  {
    m_DistanceToTravel = (GameManager.m_Instance.Map.HalfDistanceMap.x / unit.MaxStaminaConsume) * m_StaminaUsed;
  }
  // Checks if the fighter unit has the minimum stamina to make a movement.
  protected bool HasMinStamina(Fighter unit)
  {
    if (unit.Stamina >= unit.MinStaminaMove)
      return true;

    return false;
  }
  // Calculates the travelled distance.
  protected void CalculateTravelledDistance(Unit unit)
  {
    m_TravelledDistance += CheckDistance(unit, m_LastPosition);
    m_LastPosition = unit.transform.position;
  }
  // Checks if the fighter unit has travelled the required distance.
  protected bool HasTravelledDistance()
  {
    if(m_TravelledDistance >= m_DistanceToTravel)
      return true;

    return false;
  }
  // Consumes the fighter unit stamina.
  protected void ConsumeStamina(Fighter unit, int stamina)
  {
    unit.UseStamina(stamina);
    m_StaminaUsed = stamina;
  }
  //Reduces the velocity when collided.
  protected void ReduceCollisionVelocity(Fighter fighter)
  {
    if (m_IsMovingCollision)
    {
      ReduceVelocity(0.1f);

      if (IsVelocityReduced(0.1f))
      {
        StopVelocity();
        fighter.StartCoroutineStaminaBar();
        m_IsMovingCollision = false;
      }
    }
  }
  // Reset some variables.
  protected void ResetValues()
  {
    m_TravelledDistance = 0.0f;
    m_DistanceToTravel = 0.0f;
    m_LastPosition = transform.position;
    IsMoving = false;
    m_StaminaUsed = 0;
    HasInitialized = false;
  }
  // Collision logic.
  protected override void OnCollisionEnter(Collision collision)
  {
    base.OnCollisionEnter(collision);

    if (collision.collider.CompareTag("Edge"))
    {
      StopVelocity();
      m_IsMovingCollision = true;
    }
  }
}
