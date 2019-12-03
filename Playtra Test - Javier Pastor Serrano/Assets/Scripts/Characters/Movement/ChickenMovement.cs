using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenMovement : FighterMovement
{
  #region Variables
  // Reference to the chicken script.
  private Chicken m_Chicken;
  #endregion

  protected override void Awake()
  {
    base.Awake();
    m_Chicken = GetComponent<Chicken>();
  }
  protected override void Start()
  {
    base.Start();
    LastPosition = transform.position;
  }

  // Starts the movement.
  public void StartMovement()
  {
    if(m_Chicken != null)
    {
      ResetValues();
      IsMoving = true;
    }
  }
  // Checks if the chicken is moving.
  public void Moving()
  {
    if (IsMoving && m_Chicken != null)
      Movement();
  }
  // Cakculates the stamina depending of the Bar force.
  private void CalculateStamina()
  {
    int stamina = Mathf.RoundToInt(m_Chicken.Arrow.Force * (float)m_Chicken.Stamina);

    if (stamina > m_Chicken.MaxStaminaConsume)
      stamina = m_Chicken.MaxStaminaConsume;

    ConsumeStamina(m_Chicken, stamina);
  }
  // Movement cycle.
  private void Movement()
  {
    if (IsMoving)
    {
      if (!HasInitialized)
      {
        if (HasMinStamina(m_Chicken))
        {
          StopVelocity();
          CalculateStamina();
          GenerateDistanceToTravel(m_Chicken);
          SetVelocity(m_Chicken);
          ObjectiveDir = m_Chicken.Arrow.transform.right;
          IsUpdating = true;
          HasInitialized = true;
        }
        else
          ResetValues();
      }
      else
      {
        if (HasTravelledDistance() || Collided)
        {
          StopVelocity();
          ResetValues();
          m_Chicken.StartCoroutineStaminaBar();
        }
        else
          CalculateTravelledDistance(m_Chicken);
      }
    }
  }
  // Update for physics...
  private void LateUpdate()
  {
    if(IsUpdating && m_Chicken != null)
      MovePosition(m_Chicken, ObjectiveDir);
  }
  // Collision between the chicken and other units.
  protected override void OnCollisionEnter(Collision collision)
  {
    if (collision.collider.CompareTag("Wolf"))
    {
      Wolf unit = collision.collider.GetComponent<Wolf>();

      if(unit != null)
      {
        CollisionForce(unit, GameManager.m_Instance.PushForce);

        if(IsMoving)
          unit.TakeDamage(StaminaUsed);
      }

    }
    else if (collision.collider.CompareTag("Pig"))
    {
      Pig unit = collision.collider.GetComponent<Pig>();

      if (unit != null)
        CollisionForce(unit, GameManager.m_Instance.PushForce);
    }

    Collided = true;
  }
}
