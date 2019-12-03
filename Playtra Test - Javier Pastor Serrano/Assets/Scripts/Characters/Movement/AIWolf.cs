using System.Collections.Generic;
using UnityEngine;

public class AIWolf : FighterMovement
{
  #region Variables
  [Header("Variables")]
  // Time for deciding the next state.
  [SerializeField] private float m_Time = 0.0f;
  // Minimum health to make the wolf run away.
  [SerializeField] private float m_MinHealthToRun = 0.0f;
  // Probability percentage to make the wolf run away.
  [Range(0.0f, 100.0f)] [SerializeField] private float m_PercentageToRunAway = 0.0f;
  // Reference to the Wolf script.
  private Wolf m_Wolf;
  // Time left.
  private float m_TimeLeft;
  // States
  private enum StatesWolf { Idle, AttackChicken, RunAway, AttackPig };
  private StatesWolf m_State;
  #endregion

  protected override void Awake()
  {
    base.Awake();
    m_Wolf = GetComponent<Wolf>();
  }
  protected override void Start()
  {
    base.Start();
    LastPosition = transform.position;
    m_State = StatesWolf.Idle;
    m_TimeLeft = m_Time;
  }

  #region Mind
  // States Logic.
  public void States()
  {
    if (GameManager.m_Instance.Chicken != null && m_Wolf != null)
    {
      switch (m_State)
      {
        case StatesWolf.Idle:
          Idle();
          break;

        case StatesWolf.AttackChicken:
          AttackChicken();
          break;

        case StatesWolf.AttackPig:
          AttackPig();
          break;

        case StatesWolf.RunAway:
          RunAway();
          break;

        default:
          break;
      }
    }
  }
  // Idle state.
  private void Idle()
  {
    if (TimeLeft() & HasMinStamina(m_Wolf))
      DecideMovement();
  }
  // Attack chicken state.
  private void AttackChicken()
  {
    if (!IsMoving)
      m_State = StatesWolf.Idle;
    else
      Movement();
  }
  // Run away state.
  private void RunAway()
  {
    if (!IsMoving)
      m_State = StatesWolf.Idle;
    else
      Movement();
  }
  // Attack pig state.
  private void AttackPig()
  {
    if (!IsMoving)
      m_State = StatesWolf.Idle;
    else
      Movement();
  }
  #endregion

  #region Body
  // Checks if a number of seconds have passed.
  private bool TimeLeft()
  {
    m_TimeLeft -= Time.deltaTime;

    if (m_TimeLeft < 0.0f)
    {
      m_TimeLeft = m_Time;
      return true;
    }

    return false;
  }
  // Consumes a random number of stamina.
  private void ConsumeRandomStamina()
  {
    int stamina = Random.Range(1, m_Wolf.Stamina + 1);
    ConsumeStamina(m_Wolf, stamina);
  }
  // Decides the next state.
  private void DecideMovement()
  {
    if (m_Wolf.Health < m_MinHealthToRun)
    {
      float percentage = Random.Range(0.0f, 100.0f);

      if (percentage > m_PercentageToRunAway && GameManager.m_Instance.PigList.Count > 0)
      {
        m_State = StatesWolf.AttackPig;
        ObjectiveDir = GenerateDirection(GameManager.m_Instance.PigList[Random.Range(0, GameManager.m_Instance.PigList.Count)]);
      }
      else
      {
        m_State = StatesWolf.RunAway;
        ObjectiveDir = GenerateDirection(GameManager.m_Instance.Chicken) * -1.0f;
      }

    }
    else
    {
      m_State = StatesWolf.AttackChicken;
      ObjectiveDir = GenerateDirection(GameManager.m_Instance.Chicken);
    }

    IsMoving = true;
  }
  // Movement cycle.
  private void Movement()
  {
    if (!HasInitialized)
    {
      StopVelocity();
      ConsumeRandomStamina();
      GenerateDistanceToTravel(m_Wolf);
      SetVelocity(m_Wolf);
      IsUpdating = true;
      HasInitialized = true;
    }
    else
    {
      if (HasTravelledDistance() || Collided)
      {
        ResetValues();
        StopVelocity();
        m_Wolf.StartCoroutineStaminaBar();
      }
      else
        CalculateTravelledDistance(m_Wolf);
    }
  }
  // Update for physics...
  private void LateUpdate()
  {
    if (IsUpdating && m_Wolf != null)
      MovePosition(m_Wolf, ObjectiveDir);
  }
  // Collision between the wolf and other units.
  protected override void OnCollisionEnter(Collision collision)
  {
    base.OnCollisionEnter(collision);

    if (collision.collider.CompareTag("Chicken") || collision.collider.CompareTag("Wolf") ||
        collision.collider.CompareTag("Pig"))
    {
      Unit unit = collision.collider.GetComponent<Unit>();

      if(unit != null)
      {
        CollisionForce(unit, GameManager.m_Instance.PushForce);

        if (IsMoving && unit is Chicken)
        {
          Chicken chicken = GameManager.m_Instance.Chicken;

          if (chicken != null)
            chicken.TakeDamage(StaminaUsed);
        }

        if (unit is Pig || unit is Chicken)
          Collided = true;
      }
    }
  }
  
  #endregion
}