using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenMovement : FighterMovement
{
  #region Variables
  // Reference to the chicken script.
  private Chicken m_Chicken;
  #endregion

  private void Awake()
  {
    m_Chicken = GetComponent<Chicken>();
  }
  protected override void Start()
  {
    base.Start();
    LastPosition = transform.position;
  }

  // Movement call.
  public void MovementEvent()
  {
    if(m_Chicken != null && HasMinStamina(m_Chicken))
    {
      ResetValues();
      IsMovingCollision = false;
      IsMovingNormal = true;
      ObjectiveDir = m_Chicken.Arrow.transform.right;
    }
  }
  // State
  public void MovementState()
  {
    if (IsMovingNormal && m_Chicken != null)
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
    if (IsMovingNormal)
    {
      if (!HasInitialized)
      {
        CalculateStamina();
        InitializeMovement(m_Chicken);
      }
      else
        CheckTravelledDistance(m_Chicken);
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
        if (IsMovingNormal & unit as Wolf)
        {
          Wolf wolf = unit as Wolf;
          wolf.TakeDamage(StaminaUsed);
        }

        UnitPush(unit, unit.PushForce);
        ResetValues();
        m_Chicken.StopCoroutineStaminaBar();

        IsMovingNormal = false;
        IsMovingCollision = true;
      }
    }
  }
}
