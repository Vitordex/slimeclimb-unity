using UnityEngine;
using UnityEngine.Events;

namespace Quiver.Slime
{
  public class Platform : MonoBehaviour
  {
    public float size;
    [SerializeField] private bool playerArrived;
    private Transform cacheTransform;
    private PlatformManager manager;

    public Vector3 Position { get => GetTransform().localPosition; set => GetTransform().localPosition = value; }

    public Vector3 GetDistance()
    {
      return new Vector3(0, size, 0);
    }

    internal void Setup(PlatformManager manager)
    {
      this.manager = manager;
      playerArrived = false;
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
