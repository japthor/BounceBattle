using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
  #region Singleton
  // Instance of the class.
  public static InputManager m_Instance;
  private void Awake()
  {
    m_Instance = this;
  }
  #endregion

  #region Variables
  // Position of the mouse/touch
  private Vector3 m_InputPosition;
  // Variable to check if the mobile/mouse has been touched/clicked.
  private bool m_InputTouched;
  #endregion

  #region Setters/Getters
  public Vector3 InputPosition
  {
    get { return m_InputPosition; }
  }
  public bool InputTouched
  {
    get { return m_InputTouched; }
  }
  #endregion

  private void Start()
  {
    m_InputPosition = Vector3.zero;
    m_InputTouched = false;
  }
  private void Update()
  {
    #if UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX
    CheckMouseClick();
    SetMousePosition();
    #elif UNITY_ANDROID
    CheckTouch();
    #endif
  }

  // If it is Win/Mac...
  #if UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX
  // Checks if the mouse has been clicked.
  private void CheckMouseClick()
  {
    if (Input.GetMouseButtonDown(0))
      m_InputTouched = true;
    else if (Input.GetMouseButtonUp(0))
      m_InputTouched = false;
  }
  // Gets the mouse position.
  private void SetMousePosition()
  {
    m_InputPosition = Input.mousePosition;
  }

  // If it is Android...
  #elif UNITY_ANDROID
  // Checks if the mobile has been touched
  private void CheckTouch()
  {
    if (Input.touchCount > 0)
    {
      Touch touch = Input.GetTouch(0);
      if (touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Moved)
      {
        m_InputTouched = true;
        SetTouchPosition(touch);
      }
      else if(touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
        m_InputTouched = false;
    }
  }
  // Gets the touched position.
  private void SetTouchPosition(Touch touch)
  {
    if (m_InputTouched)
      m_InputPosition = touch.position;
  }
  #endif
}
