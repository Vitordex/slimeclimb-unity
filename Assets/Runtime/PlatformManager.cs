using UnityEngine;

namespace Quiver.Slime
{
  public class PlatformManager : PoolManager<Platform>
  {
    [SerializeField] private Platform platformBegin;
    [SerializeField] private Platform platformPrefab;
    public PlatformEvent onPlayerArrived;

    public PlatformManager()
    {
    }

    public PlatformManager(Platform platformPrefab) : this()
    {
      this.platformPrefab = platformPrefab;
    }

    public Platform GetPlataform(int weight)
    {
      var platform = (weight == 0) ? platformBegin : GetOrCreate();
      platform.Setup(this);
      return platform;
    }

    internal void PlayerArrived(Platform platform)
    {
      onPlayerArrived.Invoke(platform);
    }
  }
}
