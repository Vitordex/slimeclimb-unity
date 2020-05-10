using UnityEngine;

public class CameraFollow : MonoBehaviour
{
  public Player player;
  private Transform cacheTransform;
  public Platform Target { get; private set; }

  private void Awake()
  {
    var world = GameObject.FindGameObjectWithTag("World").GetComponent<World>();
    world.onPlayerArrived.AddListener(SetTarget);
  }

  private void SetTarget(Platform platform)
  {
    Target = platform;
  }

  private void Update()
  {
    var transform = GetTransform();
    var playerPosition = player.GetTransform().position;
    transform.position = playerPosition;

    if (Target == null) return;
    var position = transform.position;
    var targetHeight = Target.Position.y;
    if (playerPosition.y < targetHeight)
    {
      position.y = targetHeight;
      transform.position = position;
    }
  }

  public Transform GetTransform()
  {
    if (cacheTransform == null)
      cacheTransform = transform;

    return cacheTransform;
  }
}
