using UnityEngine;

[RequireComponent(typeof(AIWolf))]
public class Wolf : Fighter
{
  #region Variables
  // Reference to the wolf AI.
  private AIWolf m_AI;
  #endregion

  protected override void Start()
  {
    base.Start();

    // We add +1 because the maximum value from Random.Range is exclusive.
    MaxHealth = Random.Range(MinHealth, MaxHealth + 1);
    Health = MaxHealth;

    MaxStamina = Random.Range(MinStamina, MaxStamina + 1);
    Stamina = MaxStamina;

    UpdateBar(StaminaBar);
    UpdateBar(HealthBar);

    m_AI = GetComponent<AIWolf>();
  }
  private void Update()
  {
    m_AI.States();
  }
}
