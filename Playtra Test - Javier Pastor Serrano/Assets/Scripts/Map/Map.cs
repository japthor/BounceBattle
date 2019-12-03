using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class Map : MonoBehaviour
{
  #region Variables
  [Header("Collider")]
  // Reference to Gameobject which will contain the inverted collider.
  [SerializeField] private GameObject m_Collider = null;
  // Reference to the PhysicMaterial.
  [SerializeField] private PhysicMaterial m_ColliderMat = null;

  [Header("Variables")]
  // Radius offset fos spawning correctly inside the sphere.
  [SerializeField] private float m_RadiusOffset = 0;

  // Reference to the Renderer.
  private Renderer m_Renderer;
  // Radius of the sphere.
  private float m_Radius;
  // Distance from the edge to the center of the sphere.
  private Vector3 m_HalfDistanceMap;
  #endregion

  #region Setters/Getters
  public Vector3 HalfDistanceMap
  {
    get { return m_HalfDistanceMap; }
  }
  #endregion

  private void Awake()
  {
    m_Renderer = GetComponent<Renderer>();
    InvertCollider();
    CalculateRadius();
  }
  private void Start()
  {
    CalculateHalfDistance();
  }

  // Calculates the radius of the sphere.
  private void CalculateRadius()
  {
    m_Radius = m_Renderer.bounds.extents.magnitude;
  }

  // Gets a random point inside the sphere.
  public Vector3 PointArea()
  {
    return Random.insideUnitSphere * (m_Radius / m_RadiusOffset);
  }

  // Calculates the distance from the edge to the center of the sphere.
  private void CalculateHalfDistance()
  {
    Vector3 center = m_Renderer.bounds.max;
    m_HalfDistanceMap = center;
  }

  // Inverts the collider and adds the new MeshCollider & MeshRenderer.
  private void InvertCollider()
  {
    RemoveExistingColliders();
    InvertMesh();

    MeshCollider mc = m_Collider.GetComponent<MeshCollider>();

    if (mc == null)
      mc = m_Collider.gameObject.AddComponent<MeshCollider>();

    mc.material = m_ColliderMat;

    MeshRenderer mr = m_Collider.GetComponent<MeshRenderer>();

    if(mr == null)
      mr = m_Collider.gameObject.AddComponent<MeshRenderer>();

    mr.enabled = false;


  }

  // Removes the actual Collider.
  private void RemoveExistingColliders()
  {
    Collider[] colliders = m_Collider.GetComponents<Collider>();
    
    if(colliders != null)
    {
      for (int i = 0; i < colliders.Length; i++)
        DestroyImmediate(colliders[i]);
    }
  }

  // Inverts the sphere Mesh.
  private void InvertMesh()
  {
    Mesh mesh = m_Collider.GetComponent<MeshFilter>().mesh;

    if (mesh != null)
      mesh.triangles = mesh.triangles.Reverse().ToArray();
  }
}
