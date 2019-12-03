using UnityEngine;

public abstract class Unit : MonoBehaviour
{
  #region Variables
  [Header("Movement")]
  // Maximum Speed.
  [SerializeField] private float m_MaxSpeed = 0.0f;
  // Minimum Speed.
  [SerializeField] private float m_MinSpeed = 0.0f;
  // Actual Speed.
  private float m_Speed;
  #endregion

  #region Setters/Getters
  public float MaxSpeed
  {
    get { return m_MaxSpeed; }
    set { m_MaxSpeed = value; }
  }
  public float MinSpeed
  {
    get { return m_MinSpeed; }
    set { m_MinSpeed = value; }
  }
  public float Speed
  {
    get { return m_Speed; }
    set { m_Speed = value; }
  }
  #endregion

  protected virtual void Start()
  {
    m_Speed = 0.0f;
  }
}
