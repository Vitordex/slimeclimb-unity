using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Quiver.Slime
{
  public class Platform : MonoBehaviour, IPoolObject<Platform>
  {
    public float size;

    [Header("Score")]
    [SerializeField] private int score;
    [SerializeField] private uint weight;
    [SerializeField] private bool playerArrived;

    [Header("Fall")]
    [SerializeField] private bool isFall;
    [SerializeField] private float delayToFall;

    private Transform cacheTransform;
    private PlatformManager manager;
    private Material material;

    public int Score => score;
    public uint Weight => weight;
    public Vector3 Position => GetTransform().localPosition;
    public PoolManager<Platform> PoolManager { get; set; }
    public Obstacle Obstacle { get; set; }

    private void Awake()
    {
      material = GetComponentInChildren<Renderer>().material;
    }

    internal void Setup(PlatformManager manager)
    {
      this.manager = manager;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
      if (!playerArrived && other.CompareTag("Player"))
      {
        playerArrived = true;
        manager.PlayerArrived(this);

        if (isFall)
          StartCoroutine(Fall(delayToFall));
      }
    }

    private IEnumerator Fall(float delay)
    {
      yield return new WaitForSeconds(delay);
      BackToPool();
    }

    public void SetDelayFall(float delay)
    {
      isFall = delay != 0;
      delayToFall = delay;
    }

    public void SetColor(Color color)
    {
      material.SetColor("_Color", color);
    }

    public void SetPosition(Vector3 value)
    {
      GetTransform().localPosition = value;
    }

    public Vector3 GetDistance()
    {
      return new Vector3(0, size, 0);
    }

    public Transform GetTransform()
    {
      if (cacheTransform == null)
        cacheTransform = transform;

      return cacheTransform;
    }

    public void BackToPool()
    {
      if (PoolManager != null)
      {
        PoolManager.Add(this);
      }
      else
      {
        OnAddPool();
      }
    }

    public void OnAddPool()
    {
      Obstacle?.BackToPool();
      Obstacle = null;
      gameObject.SetActive(false);
    }

    public void ResetObject()
    {
      playerArrived = false;
    }
  }

  [System.Serializable]
  public class PlatformEvent : UnityEvent<Platform> { }
}
