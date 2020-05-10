using UnityEngine;
using UnityEngine.Events;

public class Platform : MonoBehaviour
{
  public float size;
  public bool playerArrived;
  public PlatformEvent onPlayerArrived;
  public PlatformEvent onResetValue;
  public Vector3 Position => GetTransform().position;
  private Transform cacheTransform;

  public Vector3 GetDistance()
  {
    return new Vector3(0, size, 0);
  }

  public void ResetValues()
  {
    playerArrived = false;
    onResetValue.Invoke(this);
    onResetValue.RemoveAllListeners();
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
      onPlayerArrived.Invoke(this);
    }
  }
}

[System.Serializable]
public class PlatformEvent : UnityEvent<Platform> { }
