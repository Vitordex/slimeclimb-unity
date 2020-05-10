using System;
using System.Collections.Generic;
using UnityEngine;

namespace Quiver.Slime
{
  [RequireComponent(typeof(PlatformManager), typeof(Obstacles))]
  public class PlatformBuilder : MonoBehaviour
  {
    public int maxPlatform;
    public int difficulty;
    private Platform lastPlatform;
    private Queue<Platform> platforms;

    public PlatformManager Manager { get; private set; }
    public Obstacles Obstacles { get; private set; }

    private void Awake()
    {
      platforms = new Queue<Platform>(maxPlatform);
      Manager = GetComponent<PlatformManager>();
      Obstacles = GetComponent<Obstacles>();
      Manager.onPlayerArrived.AddListener((p) => AddPlatform());
    }

    public void Build()
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
      var platform = Manager.GetPlataform(difficulty);
      platform.SetPosition(nextPosition);

      if (Obstacles.GetObstacle(difficulty, out var obstacle))
      {
        obstacle.Configure(platform);
      }

      difficulty += platform.Weight;

      platform.gameObject.SetActive(true);
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

        platform.SetPosition(position);
        lastPlatform = platform;
      }
    }

    internal void Clear()
    {
      while (platforms.Count > 0)
      {
        platforms.Dequeue().BackToPool();
      }

      lastPlatform = null;
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
