using Quiver.Slime;
using UnityEngine;

public class Obstacle : MonoBehaviour, IPoolObject<Obstacle>
{
  public Animator animator;
  private Transform cacheTransform;

  public PoolManager<Obstacle> PoolManager { get; set; }

  internal void Configure(Platform platform)
  {
    platform.Obstacle = this;
    var transform = GetTransform();
    transform.parent = platform.GetTransform();
    transform.localPosition = platform.GetDistance() / 2f;
  }

  public void StartAnimation(uint weight)
  {
    animator.Play("Loop");
  }

  public Transform GetTransform()
  {
    if (cacheTransform == null)
      cacheTransform = transform;

    return cacheTransform;
  }

  public void BackToPool()
  {
    PoolManager.Add(this);
    var transform = GetTransform();
    transform.parent = PoolManager.GetTransform();
  }

  public void ResetObject()
  {
    gameObject.SetActive(true);
  }

  public void OnAddPool()
  {
    gameObject.SetActive(false);
  }
}
