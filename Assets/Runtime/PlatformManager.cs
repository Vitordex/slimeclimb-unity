using System;
using UnityEngine;

namespace Quiver.Slime
{
  public class PlatformManager : MonoBehaviour
  {
    [SerializeField] private Platform platformPrefab;
    public PlatformEvent onPlayerArrived;
    private Transform cacheTransform;

    public PlatformManager()
    {
    }

    public PlatformManager(Platform platformPrefab) : this()
    {
      this.platformPrefab = platformPrefab;
    }

    public Platform GetPlataform()
    {
      var platform = Instantiate(platformPrefab, GetTransform());
      platform.Setup(this);
      return platform;
    }

    internal void PlayerArrived(Platform platform)
    {
      onPlayerArrived.Invoke(platform);
    }

    public Transform GetTransform()
    {
      if (cacheTransform == null)
        cacheTransform = transform;
      return cacheTransform;
    }
  }
}
