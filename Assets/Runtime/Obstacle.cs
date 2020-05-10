using System;
using Quiver.Slime;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
  private Transform cacheTransform;
  public Vector2 speedMultiplier;
  public Animator animator;

  public void RandomStatus()
  {
    var speed = UnityEngine.Random.Range(speedMultiplier.x, speedMultiplier.y);
    animator.Play("Loop", 0, UnityEngine.Random.Range(0f, 1f));
    animator.SetFloat("speedMultiplier", speed);
  }

  internal void Configure(Platform platform)
  {
    var transform = GetTransform();
    transform.parent = platform.GetTransform();
    transform.localPosition = platform.GetDistance() / 2f;
    platform.onBackToPool.AddListener(BackToPool);
  }

  internal void BackToPool()
  {
    Destroy(gameObject);
  }

  public Transform GetTransform()
  {
    if (cacheTransform == null)
      cacheTransform = transform;

    return cacheTransform;
  }
}
