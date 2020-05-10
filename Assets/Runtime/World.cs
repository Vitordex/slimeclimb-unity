using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{
  public int maxPlatform;
  public Obstacles Obstacles;
  public Platform platformPrefab;
  public Player player;

  [Header("Events")]
  public PlatformEvent onPlayerArrived;

  private PlayerStatus playerStatus;
  private Platform lastPlatform;
  private Queue<Platform> platforms;

  private void Start()
  {
    platforms = new Queue<Platform>();
    lastPlatform = player.currentPlatform;
    playerStatus = player.GetComponent<PlayerStatus>();

    for (int i = 0; i < maxPlatform; i++)
    {
      CreatePlatform(i != 0);
    }
  }

  private void OnPlayerArrived(Platform platform)
  {
    playerStatus.AddScore(1);
    if (playerStatus.currentScore > 4)
    {
      GenerateNextPlatform();
    }

    player.currentPlatform = platform;
    onPlayerArrived.Invoke(platform);
  }

  private void CreatePlatform(bool addObstacle)
  {
    var nextPosition = NextPlatformPosition();
    var platform = Instantiate(platformPrefab, nextPosition, Quaternion.identity);
    platform.onPlayerArrived.AddListener(OnPlayerArrived);
    if (addObstacle) AddObstacle(platform);
    AddPlatform(platform);
  }

  private void GenerateNextPlatform()
  {
    var platform = platforms.Dequeue();
    platform.ResetValues();
    platform.GetTransform().position = NextPlatformPosition(); ;
    AddObstacle(platform);
    AddPlatform(platform);
  }

  private Vector3 NextPlatformPosition()
  {
    var distance = lastPlatform.GetDistance();
    return lastPlatform.Position + distance;
  }

  private void AddPlatform(Platform platform)
  {
    platforms.Enqueue(platform);
    lastPlatform = platform;
  }

  private void AddObstacle(Platform platform)
  {
    var distance = lastPlatform.GetDistance();
    var obstaclePosition = lastPlatform.Position + (distance * .5f);
    var obstacle = Obstacles.GetRandomObstacle();
    obstacle.GetTransform().position = obstaclePosition;
    platform.onResetValue.AddListener((p) => obstacle.ResetStatus());
  }
}
