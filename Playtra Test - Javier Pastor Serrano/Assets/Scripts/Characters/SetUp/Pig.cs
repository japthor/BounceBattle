using UnityEngine;

[RequireComponent(typeof(AIPig))]
public class Pig : Peaceful
{
  #region Variables
  // Reference to the pig AI.
  private AIPig m_AI;
  #endregion 

  private void Awake()
  {
    m_AI = GetComponent<AIPig>();
  }
  protected override void Start()
  {
    base.Start();
  }
  private void Update()
  {
    if(m_AI != null)
      m_AI.States();
  }
}
