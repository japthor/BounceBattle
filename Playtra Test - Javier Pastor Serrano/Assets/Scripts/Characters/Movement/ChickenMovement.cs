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
    if(m_Chicken != null && HasMinStamina(m_Chicken))
    {
      ResetValues();
      IsMovingCollision = false;
      IsMoving = true;
    }
  }
  // Checks if the chicken is moving (Update).
  public void Moving()
  {
    if (IsMoving && m_Chicken != null)
      Movement();
  }
  // Calculates the stamina that the chicken is going to use.
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
        StopVelocity();
        CalculateStamina();
        GenerateDistanceToTravel(m_Chicken);
        SetVelocity(m_Chicken);
        ObjectiveDir = m_Chicken.Arrow.transform.right;
        m_Chicken.StopCoroutineStaminaBar();
        HasInitialized = true;
      }
      else
      {
        if (HasTravelledDistance())
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
  // Update for physics.
  private void LateUpdate()
  {
    MovePosition(m_Chicken, ObjectiveDir);
    ReduceCollisionVelocity(m_Chicken);
  }
  // Collision logic between the chicken and other units.
  protected override void OnCollisionEnter(Collision collision)
  {
    base.OnCollisionEnter(collision);

    if (collision.collider.CompareTag("Wolf") || collision.collider.CompareTag("Pig"))
    {
      Unit unit = collision.collider.GetComponent<Unit>();

      if (unit != null)
      {
        UnitCollisionForce(unit, GameManager.m_Instance.PushForce + StaminaUsed);

        if (IsMoving && unit is Wolf)
        {
          Wolf wolf = collision.collider.GetComponent<Wolf>();

          if(wolf != null)
           wolf.TakeDamage(StaminaUsed);
        }

        ResetValues();
        m_Chicken.StopCoroutineStaminaBar();
        IsMovingCollision = true;
      }
    }
  }
}
