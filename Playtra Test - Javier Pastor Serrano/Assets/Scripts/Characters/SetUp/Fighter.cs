using System.Collections;
using UnityEngine;

public abstract class Fighter : Unit
{
  #region Variables
  [Header("Health")]
  // Maximum health.
  [SerializeField] private int m_MaxHealth = 0;
  // Minimum health.
  [SerializeField] private int m_MinHealth = 0;
  // Health Per Second.
  [SerializeField] private float m_HealthPS = 0.0f;
  // Actual health.
  private float m_Health;

  [Header("Stamina")]
  // Maximum stamina.
  [SerializeField] private int m_MaxStamina = 0;
  // Minimum stamina
  [SerializeField] private int m_MinStamina = 0;
  // Stamina Per Second.
  [SerializeField] private int m_StaminaPS = 0;
  // Minimum quantity of stamina to move again. 
  [SerializeField] private int m_MinStaminaMove = 0;
  // Maximum use of stamina per movement.
  [SerializeField] private int m_MaxStaminaConsume = 0;
  private int m_Stamina;

  [Header("Bars")]
  // Reference to the health bar.
  [SerializeField] private UIBar m_HealthBar = null;
  // Reference to the stamina bar.
  [SerializeField] private UIBar m_StaminaBar = null;

  // Reference to the health regen. coroutine.
  private IEnumerator m_HealthCoroutine;
  // Reference to the stamina regen. coroutine.
  private IEnumerator m_StaminaCoroutine;
  #endregion

  #region Setters/Getters
  public int MaxHealth
  {
    get { return m_MaxHealth; }
    set { m_MaxHealth = value; }
  }
  public int MinHealth
  {
    get { return m_MinHealth; }
    set { m_MinHealth = value; }
  }
  public float HealthPS
  {
    get { return m_HealthPS; }
    set { m_HealthPS = value; }
  }
  public float Health
  {
    get { return m_Health; }
    set { m_Health = value; }
  }

  public int MaxStamina
  {
    get { return m_MaxStamina; }
    set { m_MaxStamina = value; }
  }
  public int MinStamina
  {
    get { return m_MinStamina; }
    set { m_MinStamina = value; }
  }
  public int StaminahPS
  {
    get { return m_StaminaPS; }
    set { m_StaminaPS = value; }
  }
  public int Stamina
  {
    get { return m_Stamina; }
    protected set { m_Stamina = value; }
  }
  public int MinStaminaMove
  {
    get { return m_MinStaminaMove; }
    protected set { m_MinStaminaMove = value; }
  }
  public int MaxStaminaConsume
  {
    get { return m_MaxStaminaConsume; }
    protected set { m_MaxStaminaConsume = value; }
  }

  public UIBar HealthBar
  {
    get { return m_HealthBar; }
  }
  public UIBar StaminaBar
  {
    get { return m_StaminaBar; }
  }

  public IEnumerator HealthCoroutine
  {
    get { return m_HealthCoroutine; }
  }
  public IEnumerator StaminaCoroutine
  {
    get { return m_StaminaCoroutine; }
  }

  #endregion

  protected override void Start()
  {
    base.Start();

    m_Health = m_MaxHealth;
    m_Stamina = m_MaxStamina;
  }

  // Health regeneration coroutine.
  private IEnumerator RegenerateHealth()
  {
    while (m_Health + m_HealthPS < m_MaxHealth)
    {
      yield return new WaitForSeconds(1.0f);
      m_Health += m_HealthPS;
      UpdateBar(m_HealthBar);
    }

    m_Health = m_MaxHealth;
    UpdateBar(m_HealthBar);
    m_HealthCoroutine = null;

  }
  // Stamina regeneration coroutine.
  private IEnumerator RegenerateStamina()
  {
    while (m_Stamina + m_StaminaPS < m_MaxStamina)
    {
      yield return new WaitForSeconds(1.0f);
      m_Stamina += m_StaminaPS;
      UpdateBar(m_StaminaBar);
    }

    m_Stamina = m_MaxStamina;
    UpdateBar(m_StaminaBar);
    m_StaminaCoroutine = null;
  }
  // Takes Damage
  public void TakeDamage(float damage)
  {
    if (m_Health - damage <= 0.0f)
      m_Health = 0.0f;
    else
      m_Health -= damage;

    IsDead();

    UpdateBar(m_HealthBar);
    StartCoroutineHealthBar();
  }
  // Consumes stamina.
  public void UseStamina(int stamina)
  {
    if (m_Stamina - stamina <= 0)
      m_Stamina = 0;
    else
      m_Stamina -= stamina;

    UpdateBar(StaminaBar);
    StopCoroutineStaminaBar();
  }
  // Starts health regeneration coroutine.
  public void StartCoroutineHealthBar()
  {
    if (m_HealthCoroutine == null)
    {
      m_HealthCoroutine = RegenerateHealth();
      StartCoroutine(m_HealthCoroutine);
    }
  }
  // Starts stamina regeneration coroutine.
  public void StartCoroutineStaminaBar()
  {
    if (m_StaminaCoroutine == null)
    {
      m_StaminaCoroutine = RegenerateStamina();
      StartCoroutine(m_StaminaCoroutine);
    }
  }
  // Stops health regeneration coroutine.
  public void StopCoroutineHealthBar()
  {
    if (m_HealthCoroutine != null)
    {
      StopCoroutine(m_HealthCoroutine);
      m_HealthCoroutine = null;
    }
  }
  // Stops stamina regeneration coroutine.
  public void StopCoroutineStaminaBar()
  {
    if (m_StaminaCoroutine != null)
    {
      StopCoroutine(m_StaminaCoroutine);
      m_StaminaCoroutine = null;
    }
  }
  // Updates stamina/health bar.
  public void UpdateBar(UIBar bar)
  {
    if (bar == m_HealthBar)
      bar.SetSize(m_Health / m_MaxHealth);
    else if (bar == m_StaminaBar)
      bar.SetSize((float)m_Stamina / (float)m_MaxStamina);

  }
  // Checks if the unit is dead.
  public void IsDead()
  {
    if (m_Health <= 0.0f)
    {
      GameManager.m_Instance.RemoveUnitFromList(this);
      Destroy(this.gameObject);
    }
  }
}
