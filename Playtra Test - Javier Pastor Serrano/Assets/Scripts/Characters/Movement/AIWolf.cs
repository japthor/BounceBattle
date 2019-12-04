using System.Collections.Generic;
using UnityEngine;

public class AIWolf : FighterMovement
{
  #region Variables
  [Header("Variables")]
  // Waiting Time
  [SerializeField] private float m_Time = 0.0f;
  // Minimum health to make the wolf run away.
  [SerializeField] private float m_MinHealthToRun = 0.0f;
  // Probability (%) to make the wolf wait.
  [Range(0.0f, 100.0f)] [SerializeField] private float m_PercentageToWait = 0.0f;
  // Probability (%) to make the wolf run away.
  [Range(0.0f, 100.0f)] [SerializeField] private float m_PercentageToRunAway = 0.0f;
  // Reference to the Wolf script.
  private Wolf m_Wolf;
  // Time left from the Waiting time.
  private float m_TimeLeft;
  // States
  private enum StatesWolf { Idle, Wait, AttackChicken, RunAway, AttackPig };
  private StatesWolf m_State;
  #endregion

  private void Awake()
  {
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
  // States Logic (Update).
  public void States()
  {
    if (GameManager.m_Instance.Chicken != null && m_Wolf != null)
    {
      switch (m_State)
      {
        case StatesWolf.Idle:
          Idle();
          break;

        case StatesWolf.Wait:
          Wait();
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
    if (HasMinStamina(m_Wolf))
    {
      IsMovingCollision = false;
      DecideMovement();
    }
  }
  // Wait State
  private void Wait()
  {
    if (TimeLeft())
      m_State = StatesWolf.Idle;
  }
  // Attack chicken state.
  private void AttackChicken()
  {
    if (!IsMovingNormal)
      m_State = StatesWolf.Idle;
    else
      Movement();
  }
  // Run away state.
  private void RunAway()
  {
    if (!IsMovingNormal)
      m_State = StatesWolf.Idle;
    else
      Movement();
  }
  // Attack pig state.
  private void AttackPig()
  {
    if (!IsMovingNormal)
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
  // Consumes a random quantity of stamina.
  private void ConsumeRandomStamina()
  {
    int stamina = Random.Range(m_Wolf.MinStaminaMove, m_Wolf.Stamina + 1);
    ConsumeStamina(m_Wolf, stamina);
  }
  // Decides the next state.
  private void DecideMovement()
  {
    float percentageToWait = Random.Range(0.0f, 100.0f);

    if (percentageToWait < m_PercentageToWait)
      m_State = StatesWolf.Wait;
    else
    {
      if (m_Wolf.Health < m_MinHealthToRun)
      {
        float percentageToRun = Random.Range(0.0f, 100.0f);

        if (percentageToRun < m_PercentageToRunAway && GameManager.m_Instance.PigList.Count > 0)
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

      IsMovingNormal = true;
    }
  }
  // Movement cycle.
  private void Movement()
  {
    if (!HasInitialized)
    {
      ConsumeRandomStamina();
      InitializeMovement(m_Wolf);
    }
    else
      CheckTravelledDistance(m_Wolf);
  }
  // Update for physics.
  private void LateUpdate()
  {
    MovePosition(m_Wolf, ObjectiveDir);
    ReduceCollisionVelocity(m_Wolf);
  }

  // Collision logic between the wolf and other units.
  protected override void OnCollisionEnter(Collision collision)
  {
    base.OnCollisionEnter(collision);

    if (collision.collider.CompareTag("Chicken") || collision.collider.CompareTag("Wolf") ||
        collision.collider.CompareTag("Pig"))
    {
      Unit unit = collision.collider.GetComponent<Unit>();

      if (unit != null)
      {
        if (IsMovingNormal && unit is Chicken)
        {
          Chicken chicken = unit as Chicken;
          chicken.TakeDamage(StaminaUsed);
        }

        UnitPush(unit, unit.PushForce);
        ResetValues();
        m_Wolf.StopCoroutineStaminaBar();

        IsMovingNormal = false;
        IsMovingCollision = true;
      }
    }
  }

  #endregion
}
