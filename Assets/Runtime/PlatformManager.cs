using UnityEngine;

namespace Quiver.Slime
{
  public class PlatformManager : PoolManager<Platform>
  {
    [SerializeField] private PlatformConfig platformConfig;
    [SerializeField] private Platform platformBegin;
    public PlatformEvent onPlayerArrived;

    protected override void Awake()
    {
      base.Awake();
      platformBegin.Setup(this);
    }

    public Platform GetPlataform(uint weight)
    {
      if (weight == 0)
      {
        return platformBegin;
      }

      var platform = GetOrCreate();
      platformConfig.FindColor(weight, out var color, out var delay);
      platform.Setup(this);
      platform.SetColor(color);
      platform.SetDelayFall(delay);
      return platform;
    }

    internal void PlayerArrived(Platform platform)
    {
      onPlayerArrived.Invoke(platform);
    }
  }
}
