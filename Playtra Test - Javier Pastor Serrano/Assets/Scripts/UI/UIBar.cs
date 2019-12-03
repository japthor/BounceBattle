using UnityEngine;

public class UIBar : MonoBehaviour
{
  #region Variables
  [Header("Bar")]
  // Reference to the movable bar.
  [SerializeField] private Transform m_Bar = null;
  #endregion

  // Sets the size of the movable bar.
  public void SetSize(float size)
  {
    if (m_Bar != null)
    {
      if (size >= 0 && size <= 1.0f)
        m_Bar.localScale = new Vector3(1.0f, 1.0f, size);
    }
  }
}
