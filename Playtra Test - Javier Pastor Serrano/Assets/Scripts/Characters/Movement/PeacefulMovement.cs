using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PeacefulMovement : Movement
{
  #region Variables
  [Header("Variables")]
  // Minimum distance to make the peaceful unit run away.
  [SerializeField] private float m_Distance;
  // List of all the fighters near the peaceful unit.
  private List<Fighter> m_DetectedEnemies;
  #endregion

  #region Setters/Getters
  public float Distance
  {
    get { return m_Distance; }
    set { m_Distance = value; }
  }
  public List<Fighter> DetectedEnemies
  {
    get { return m_DetectedEnemies; }
  }
  #endregion

  protected override void Awake()
  {
    base.Awake();
    m_DetectedEnemies = new List<Fighter>();
  }
  // Adss a fighter unit to the list.
  protected void AddFighterToList(Fighter fighter)
  {
    if (!m_DetectedEnemies.Contains(fighter))
      m_DetectedEnemies.Add(fighter);
  }
  // Removes a fighter unit from the list.
  protected void RemoveFighterFromList(Fighter fighter)
  {
    if (m_DetectedEnemies.Contains(fighter))
      m_DetectedEnemies.Remove(fighter);
  }
  // Checks if a fighter unit is inside the area.
  protected bool InsideArea(Unit unit)
  {
    float distance = CheckDistance(unit);

    if (distance <= m_Distance)
      return true;

    return false;
  }
}
