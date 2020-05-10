using UnityEngine;

public class Obstacles : MonoBehaviour
{
  public ObstacleManager[] obstaclesTypes;

  public Obstacle GetRandomObstacle()
  {
    var manager = GetRandom();
    return manager.GetObstacle();
  }

  private ObstacleManager GetRandom()
  {
    var size = obstaclesTypes.Length;
    var randomIndex = Random.Range(0, size);
    return obstaclesTypes[randomIndex];
  }
}
