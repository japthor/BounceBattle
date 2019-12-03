using System.Collections.Generic;
using UnityEngine;

public class AIPig : PeacefulMovement
{
  #region Variables
  // Reference to the pig script.
  private Pig m_Pig;
  // Pig States
  private enum StatesPig { Idle, Run };
  private StatesPig m_State;
  #endregion

  protected override void Awake()
  {
    base.Awake();
    m_Pig = GetComponent<Pig>();
  }
  protected override void Start()
  {
    base.Start();
    m_State = StatesPig.Idle;
  }

  #region Mind
  // States Logic.
  public void States()
  {
    if (GameManager.m_Instance.Chicken != null && m_Pig != null)
    {
      switch (m_State)
      {
        case StatesPig.Idle:
          Idle();
          break;

        case StatesPig.Run:
          Run();
          break;

        default:
          break;
      }
    }

  }
  // Idle state.
  private void Idle()
  {
    if (IsEnemyDetected())
      m_State = StatesPig.Run;
  }
  // Run state.
  private void Run()
  {
    if (!IsMoving)
      m_State = StatesPig.Idle;
    else
      Movement();
  }
  #endregion

  #region Body
  // Starts the movement when a fighter is detected.
  private bool IsEnemyDetected()
  {
    CheckAllEnemiesDistances();

    if (DetectedEnemies.Count > 0)
    {
      IsMoving = true;
      IsUpdating = true;
      return true;
    }

    return false;
  }
  // Adds or removes the fighters from the list if it is near the pig.
  private void CheckAllEnemiesDistances()
  {
    if (GameManager.m_Instance.FighterList.Count > 0)
    {
      for (int i = 0; i < GameManager.m_Instance.FighterList.Count; i++)
      {
        Fighter fighter = GameManager.m_Instance.FighterList[i];

        if (InsideArea(fighter))
          AddFighterToList(fighter);
        else
          RemoveFighterFromList(fighter);
      }
    }
  }
  // Movement cycle.
  private void Movement()
  {
    if (IsMoving)
    {
      CheckAllEnemiesDistances();

      if (DetectedEnemies.Count > 0)
      {
        for (int i = 0; i < DetectedEnemies.Count; i++)
        {
          ObjectiveDir = GenerateDirection(DetectedEnemies[i]) * -1.0f;
        }
      }
      else
      {
        IsUpdating = false;
        IsMoving = false;
      }
    }
  }
  // Update for physics...
  private void LateUpdate()
  {
    if (IsUpdating && m_Pig != null)
      MovePosition(m_Pig, ObjectiveDir);
  }
  // Collision between the pig and other units.
  protected override void OnCollisionEnter(Collision collision)
  {
    if (collision.collider.CompareTag("Wolf") || collision.collider.CompareTag("Chicken"))
    {
      Fighter fighter = collision.gameObject.GetComponent<Fighter>();

      if(fighter != null)
        m_Pig.Captured(fighter);
    }
  }
  #endregion
}
