using UnityEngine;

public abstract class Peaceful : Unit
{
  #region Variables
  [Header("Benefits")]
  // Bonus points to add to the collided unit health.
  [SerializeField] private int m_BonusHealth = 0;
  // Bonus points to add to the collided unit stamina.
  [SerializeField] private int m_BonusStamina = 0;
  #endregion

  protected override void Start()
  {
    base.Start();

    MinSpeed = MaxSpeed;
    Speed = MaxSpeed;
  }

  //Logic when the peaceful unit has collided with a fighter unit.
  public void Captured(Fighter unit)
  {
    AddBonus(unit);
    Heal(unit);
    Eliminate();
  }
  // Adds the bonus to the collided fighter unit.
  private void AddBonus(Fighter unit)
  {
    unit.MaxHealth += m_BonusHealth;
    unit.MaxStamina += m_BonusStamina;
  }
  // Heals the collided fighter unit.
  private void Heal(Fighter unit)
  {
    unit.Health = unit.MaxHealth;
    unit.UpdateBar(unit.HealthBar);
  }
  // Eliminates the peaceful unit.
  private void Eliminate()
  {
    GameManager.m_Instance.RemoveUnitFromList(this);
    Destroy(gameObject);
  }
}
