using UnityEngine;

[RequireComponent(typeof(ChickenMovement))]
public class Chicken : Fighter
{
  #region Variables
  [Header("Arrow")]
  // Reference to the arrow script.
  [SerializeField] private Arrow m_Arrow = null;
  // Reference to the chicken movement script.
  private ChickenMovement m_Movement;
  // Checks if it has been clicked/touched.
  private bool m_Clicked;
  #endregion

  #region Setters/Getters
  public Arrow Arrow
  {
    get { return m_Arrow; }
  }
  #endregion

  protected override void Start()
  {
    base.Start();

    m_Clicked = false;

    UpdateBar(StaminaBar);
    UpdateBar(HealthBar);

  }
  private void Awake()
  {
    m_Movement = GetComponent<ChickenMovement>();
  }
  private void Update()
  {
    if (m_Movement != null && m_Arrow != null)
    {
      Touch();
      m_Movement.Moving();
    }
  }

  // Makes the arrow appear/disappear & initiates the movement.
  private void Touch()
  {

    if (InputManager.m_Instance.InputTouched && !m_Clicked)
    {
      if (Stamina > MinStaminaMove && RaycastSucceeded())
      {
        m_Arrow.gameObject.SetActive(true);
        m_Clicked = true;
      }
    }
    else if (!InputManager.m_Instance.InputTouched && m_Clicked)
    {
      m_Arrow.gameObject.SetActive(false);
      m_Movement.StartMovement();
      m_Clicked = false;
    }
  }
  // Checks if the raycast has collided with the chicken.
  private bool RaycastSucceeded()
  {
    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    RaycastHit hit;

    if (Physics.Raycast(ray, out hit, 100.0f))
    {
      if (hit.collider.tag == "Chicken")
        return true;
    }
    return false;
  }
}
