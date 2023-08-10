using Quiver.Slime;

public class ObstacleManager : PoolManager<Obstacle>
{
  public Obstacle GetObstacle(uint weight)
  {
    var obstacle = GetOrCreate();
    obstacle.StartAnimation(weight);
    return obstacle;
  }
}
