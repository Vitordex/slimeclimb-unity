using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
  public Obstacle obstaclePrefab;

  public Obstacle GetObstacle()
  {
    var obstacle = Instantiate(obstaclePrefab);
    obstacle.RandomStatus();
    return obstacle;
  }
}