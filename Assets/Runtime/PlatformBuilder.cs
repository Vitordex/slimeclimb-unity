using System.Collections.Generic;
using UnityEngine;

namespace Quiver.Slime
{
  [RequireComponent(typeof(PlatformManager))]
  public class PlatformBuilder : MonoBehaviour
  {
    public int maxPlatform;
    private Platform lastPlatform;
    private Queue<Platform> platforms;

    public PlatformManager Manager { get; private set; }

    private void Awake()
    {
      platforms = new Queue<Platform>(maxPlatform);
      Manager = GetComponent<PlatformManager>();
      Manager.onPlayerArrived.AddListener((p) => AddPlatform());
    }

    public void InitOrReset()
    {
      for (int i = 0; i < maxPlatform; i++)
      {
        AddPlatform();
      }
    }

    public Platform GetPlatform()
    {
      if (platforms.Count < maxPlatform)
        AddPlatform();
      return platforms.Dequeue();
    }

    public Platform Peek()
    {
      return platforms.Peek();
    }

    private void AddPlatform()
    {
      var nextPosition = NextPosition();
      var platform = Manager.GetPlataform();
      platform.Position = nextPosition;
      lastPlatform = platform;
      platforms.Enqueue(platform);
    }

    internal void ResetHeight()
    {
      Platform lastPlatform = default;
      foreach (var platform in platforms)
      {
        var position = Vector3.zero;

        if (lastPlatform != null)
          position = lastPlatform.Position + lastPlatform.GetDistance();

        platform.Position = position;
        lastPlatform = platform;
      }
    }

    private Vector3 NextPosition()
    {
      var distance = Vector3.zero;

      if (lastPlatform != null)
      {
        distance = lastPlatform.Position + lastPlatform.GetDistance();
      }

      return distance;
    }
  }
}
