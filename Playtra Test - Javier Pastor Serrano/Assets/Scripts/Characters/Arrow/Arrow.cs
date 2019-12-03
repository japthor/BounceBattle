using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
  #region Variables
  [Header("Bar")]
  // Reference to the force bar.
  [SerializeField] private UIBar m_ForceBar = null;

  [Header("Variables")]
  // Maximum distance the touch/mouse can reach from the Initial Input Position.
  [SerializeField] private float m_MaximumDistance = 0.0f;

  // The force to apply to the movement.
  private float m_Force;
  // Reference to the camera.
  private Camera m_Camera;
  // Initial position of the mouse/finger.
  private Vector3 m_InitialInputPosition;
  #endregion

  #region Setters/Getters
  public float Force
  {
    get { return m_Force; }
  }
  #endregion

  private void Awake()
  {
    m_Camera = Camera.main;
    gameObject.SetActive(false);
  }
  private void OnEnable()
  {
    m_Force = 0.0f;
    m_InitialInputPosition = Vector3.zero;
    m_InitialInputPosition = InputManager.m_Instance.InputPosition;
  }
  private void Update()
  {
    CalculateRotation();
    CalculateForce();
  }

  /* 
   * Calculates the rotation depending of the direction between
     the position of the click/touch and the actual position of object.
  */
  private void CalculateRotation()
  {
    Vector3 pos = m_Camera.WorldToScreenPoint(transform.position);
    Vector3 inputPos = InputManager.m_Instance.InputPosition;

    Vector3 dir = (inputPos - pos).normalized;
    float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
    transform.rotation = Quaternion.Euler(-90.0f, 0.0f, -angle);
  }
  /* 
   * Calculates the force depending ofthe distance between the
     position of the first click/touch and the actual position 
     of the mouse/finger.
  */
  private void CalculateForce()
  {
    float distance = Distance(InputManager.m_Instance.InputPosition, m_InitialInputPosition);

    if (distance >= 0.0f && distance <= m_MaximumDistance)
    {
      float div = distance / m_MaximumDistance;
      m_Force = Mathf.Round(div * 10.0f) * 0.1f;
    }

    UpdateBar(distance);
  }
  // Updates the force bar.
  private void UpdateBar(float distance)
  {
    if (distance <= 0.0f)
      m_ForceBar.SetSize(0.0f);
    else if (distance >= m_MaximumDistance)
      m_ForceBar.SetSize(1.0f);
    else
      m_ForceBar.SetSize(distance / m_MaximumDistance);
  }
  // Returns the distance between two points.
  private float Distance(Vector3 pos1, Vector3 pos2)
  {
    return Vector3.Distance(pos1, pos2);
  }
}

