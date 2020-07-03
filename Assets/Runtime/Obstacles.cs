using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Obstacles : MonoBehaviour
{
  public Config[] configs;

  public bool GetObstacle(uint weight, out Obstacle obstacle)
  {
    var config = GetConfig(weight);
    obstacle = config.GetRandomManager().GetObstacle(weight);
    return true;
  }

  private Config GetConfig(uint weight)
  {
    for (int i = 0; i < configs.Length - 1; i++)
    {
      var config = configs[i];
      var nextConfig = configs[i + 1];

      if (config.weight <= weight && weight < nextConfig.weight)
        return config;
    }

    return configs[configs.Length - 1];
  }

  [Serializable]
  public struct Config
  {
    public uint weight;
    public Probability[] probabilities;

    public float GetTotal()
    {
      var total = .0f;
      foreach (var pro in probabilities)
        total += pro.probability;

      return total;
    }

    public ObstacleManager GetRandomManager()
    {
      var total = GetTotal();
      var number = Random.Range(0f, total);
      return FindManager(number);
    }

    private ObstacleManager FindManager(float probability)
    {
      var length = probabilities.Length;
      if (length == 0) return probabilities[0].obstacle;

      var current = 0f;
      for (int i = 0; i < length; i++)
      {
        var currentItem = probabilities[i];

        if (current <= probability && currentItem.probability + current > probability)
        {
          return currentItem.obstacle;
        }

        current += currentItem.probability;
      }

      Debug.LogWarning("Not find objstacle");
      return probabilities[length - 1].obstacle;
    }
  }

  [Serializable]
  public struct Probability
  {
    public ObstacleManager obstacle;
    public float probability;
  }
}
