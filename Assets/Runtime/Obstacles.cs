using UnityEngine;

public class Obstacles : MonoBehaviour
{
  public ObstacleManager[] obstaclesTypes;

  public bool GetObstacle(int weight, out Obstacle obstacle)
  {
    obstacle = default;
    if (weight == 0) return false;

    var manager = GetRandom();
    obstacle = manager.GetObstacle();
    return true;
  }

  private ObstacleManager GetRandom()
  {
    var size = obstaclesTypes.Length;
    var randomIndex = Random.Range(0, size);
    return obstaclesTypes[randomIndex];
  }
}
