using UnityEngine;
using UnityEngine.Events;

namespace Quiver.Slime
{
  public class Platform : MonoBehaviour
  {
    public float size;
    [SerializeField] private int score;
    [SerializeField] private int weight;
    [SerializeField] private bool playerArrived;
    public UnityEvent onBackToPool;
    private Transform cacheTransform;
    private PlatformManager manager;

    public Vector3 Position => GetTransform().localPosition;

    public int Score => score;
    public int Weight => weight;

    public Vector3 GetDistance()
    {
      return new Vector3(0, size, 0);
    }

    internal void Setup(PlatformManager manager)
    {
      this.manager = manager;
      playerArrived = false;
    }

    public void SetPosition(Vector3 value)
    {
      GetTransform().localPosition = value;
    }

    public void BackToPool()
    {
      gameObject.SetActive(false);
      onBackToPool.Invoke();
      onBackToPool.RemoveAllListeners();
    }

    public Transform GetTransform()
    {
      if (cacheTransform == null)
        cacheTransform = transform;

      return cacheTransform;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
      if (!playerArrived && other.CompareTag("Player"))
      {
        playerArrived = true;
        manager.PlayerArrived(this);
      }
    }
  }

  [System.Serializable]
  public class PlatformEvent : UnityEvent<Platform> { }
}
