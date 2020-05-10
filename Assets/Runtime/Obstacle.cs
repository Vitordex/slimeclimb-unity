using UnityEngine;

public class Obstacle : MonoBehaviour
{
  private Transform cacheTransform;
  public Vector2 speedMultiplier;
  public Animator animator;

  public void RandomStatus()
  {
    var speed = Random.Range(speedMultiplier.x, speedMultiplier.y);
    animator.Play("Loop", 0, Random.Range(0f, 1f));
    animator.SetFloat("speedMultiplier", speed);
  }

  public void ResetStatus()
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