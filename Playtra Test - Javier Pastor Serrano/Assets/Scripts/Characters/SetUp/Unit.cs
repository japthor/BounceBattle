using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public abstract class Unit : MonoBehaviour
{
  #region Variables
  [Header("Movement")]
  // Maximum Speed.
  [SerializeField] private float m_MaxSpeed = 0.0f;
  // Minimum Speed.
  [SerializeField] private float m_MinSpeed = 0.0f;
  // Push force when a unit collides with another.
  [SerializeField] private float m_PushForce = 0.0f;
  // Actual Speed.
  private float m_Speed;
  // Reference to the rigidbody.
  private Rigidbody m_RigidBody;
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
  public float PushForce
  {
    get { return m_PushForce; }
    set { m_PushForce = value; }
  }
  public Rigidbody RigidBody
  {
    get { return m_RigidBody; }
    set { m_RigidBody = value; }
  }
  #endregion

  protected virtual void Awake()
  {
    m_RigidBody = GetComponent<Rigidbody>();
  }
  protected virtual void Start()
  {
    m_Speed = 0.0f;
  }
}
