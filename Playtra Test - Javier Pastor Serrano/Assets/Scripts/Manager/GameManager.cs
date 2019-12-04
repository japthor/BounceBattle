using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
  #region Singleton
  // Instance of the class.
  public static GameManager m_Instance;
  private void Awake()
  {
    m_Instance = this;
    m_PigList = new List<Pig>();
    m_WolfList = new List<Wolf>();
    m_PeacefulList = new List<Peaceful>();
    m_FighterList = new List<Fighter>();
  }
  #endregion

  #region Variables
  [Header("Chicken")]
  // Reference to the chicken prefab.
  [SerializeField] private GameObject m_ChickenPrefab = null;
  // Reference to the chicken script.
  private Chicken m_Chicken = null;

  [Header("Pig")]
  // Reference to the pig prefab.
  [SerializeField] private GameObject m_PigPrefab = null;
  // Maximum number of pigs.
  [SerializeField] private int m_MaxTotalPigs = 0;
  // Minimum number of pigs.
  [SerializeField] private int m_MinTotalPigs = 0;
  // List of the number of pigs in-game.
  private List<Pig> m_PigList;

  [Header("Wolf")]
  // Reference to the wolf prefab.
  [SerializeField] private GameObject m_WolfPrefab = null;
  // Maximum number of wolfs.
  [SerializeField] private int m_MaxTotalWolfs = 0;
  // Minimum number of wolfs.
  [SerializeField] private int m_MinTotalWolfs = 0;
  // List of the number of wolfs in-game.
  private List<Wolf> m_WolfList;

  // List of the number of peaceful units in-game.
  private List<Peaceful> m_PeacefulList;
  // List of the number of fighter units in-game.
  private List<Fighter> m_FighterList;

  [Header("Map")]
  // Reference to the map prefab.
  [SerializeField] private Map m_Map = null;
  // Y Offset for spawning.
  [SerializeField] private float m_OffsetY = 0.0f;
  #endregion

  #region Setters/Getters
  public Chicken Chicken
  {
    get { return m_Chicken; }
  }
  public List<Pig> PigList
  {
    get { return m_PigList; }
  }
  public List<Wolf> WolfList
  {
    get { return m_WolfList; }
  }
  public List<Peaceful> PeacefulList
  {
    get { return m_PeacefulList; }
  }
  public List<Fighter> FighterList
  {
    get { return m_FighterList; }
  }
  public Map Map
  {
    get { return m_Map; }
  }
  #endregion

  private void Start()
  {
    SpawnChicken();
    SpawnPigs();
    SpawnWolfs();
  }

  // Spawns the chicken
  private void SpawnChicken()
  {
    GameObject go = Spawn(m_ChickenPrefab, Vector3.zero);

    Chicken chicken = go.GetComponent<Chicken>();
    m_Chicken = chicken;

    Fighter fighter = chicken as Fighter;
    FighterList.Add(fighter);
  }
  // Spawns the Pigs
  private void SpawnPigs()
  {
    int value = Random.Range(m_MinTotalPigs, m_MaxTotalPigs + 1);

    for (int i = 0; i < value; i++)
    {
      GameObject go = RandomSpawn(m_PigPrefab);

      Pig pig = go.GetComponent<Pig>();
      m_PigList.Add(pig);

      Peaceful peaceful = pig as Peaceful;
      m_PeacefulList.Add(peaceful);
    }
  }
  // Spawns the Wolfs
  private void SpawnWolfs()
  {
    int value = Random.Range(m_MinTotalWolfs, m_MaxTotalWolfs + 1);

    for (int i = 0; i < value; i++)
    {
      GameObject go = RandomSpawn(m_WolfPrefab);

      Wolf wolf = go.GetComponent<Wolf>();
      m_WolfList.Add(wolf);

      Fighter fighter = wolf as Fighter;
      m_FighterList.Add(fighter);
    }
  }
  // Returns a Gameobject that has been spawned randomly inside an area.
  private GameObject RandomSpawn(GameObject go)
  {
    Vector3 point = m_Map.PointArea();
    return Instantiate(go, new Vector3(point.x, m_OffsetY, point.z), go.transform.rotation);
  }
  // Returns a Gameobject that has been spawned inside an area.
  private GameObject Spawn(GameObject go, Vector3 point)
  {
    var gameObject = Instantiate(go, new Vector3(point.x, m_OffsetY, point.z), go.transform.rotation);
    return gameObject;
  }
  // Removes a unit from the list.
  public void RemoveUnitFromList(Unit unit)
  {
    if (unit is Peaceful)
    {
      m_PeacefulList.Remove((Peaceful)unit);

      if (unit is Pig)
        m_PigList.Remove((Pig)unit);
    }
    else if (unit is Fighter)
    {
      m_FighterList.Remove((Fighter)unit);

      if (unit is Wolf)
        m_WolfList.Remove((Wolf)unit);
    }
  }
  // Checks if the game is over.
  public bool IsGameOver()
  {
    if (m_WolfList.Count == 0 || m_Chicken.Health <= 0.0f)
      ReturnToMainMenu();

    return false;
  }
  // Returns to Main Menu.
  private void ReturnToMainMenu()
  {
    SceneManager.LoadScene(0);
  }
}
