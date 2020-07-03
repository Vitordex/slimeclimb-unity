using Quiver.Slime;
using UnityEngine;

public class Player : MonoBehaviour
{
  [Header("Jump Config")]
  public float speed;
  public float offsetDistanceJump;
  public float maxDistanceAllowJump;
  public LayerMask mask;

  internal Platform currentPlatform;
  private PlatformBuilder platformBuilder;
  private Transform cacheTransform;

  public delegate void UpdatePosition(Vector3 before, Vector3 after);
  public UpdatePosition onUpdatePosition;
  public UpdatePosition onUpdateLocalPosition;
  private Vector3 lastPosition;

  public PlayerStatus Status { get; private set; }
  public Rigidbody2D Rigidbody { get; private set; }

  private void Awake()
  {
    Rigidbody = GetComponent<Rigidbody2D>();
    Status = GetComponent<PlayerStatus>();
    lastPosition = GetTransform().position;
  }

  private void LateUpdate()
  {
    var transform = GetTransform();
    var currentPosition = transform.position;

    if (lastPosition != currentPosition)
    {
      onUpdatePosition?.Invoke(lastPosition, currentPosition);
      lastPosition = currentPosition;
    }
  }

  public void Jump()
  {
    if (!Status.IsDie && CanJump(out var hit))
    {
      GetTransform().position = currentPlatform.GetTransform().position;
      Rigidbody.velocity = Vector2.up * speed * Rigidbody.gravityScale;
    }
  }

  internal void SetPlatformBuilder(PlatformBuilder platformBuilder)
  {
    this.platformBuilder = platformBuilder;
    currentPlatform = platformBuilder.Peek();
    platformBuilder.Manager.onPlayerArrived.AddListener(OnPlatformArrived);
  }

  private void OnPlatformArrived(Platform platform)
  {
    Status.AddScore(platform.Score);
    currentPlatform = platform;
  }

  internal void ReceiveInput(InputManager inputManager)
  {
    inputManager.onAction += Jump;
  }

  private bool CanJump(out RaycastHit2D hit)
  {
    hit = Physics2D.Raycast(GetTransform().position + new Vector3(0, offsetDistanceJump, 0), Vector2.down, maxDistanceAllowJump, mask);
    return hit.collider != null;
  }

  public void SetLocalPosition(Vector3 localPosition)
  {
    var transform = GetTransform();
    var beforeLocalPosition = transform.localPosition;
    var beforeWorldPosition = transform.position;
    transform.localPosition = localPosition;
    onUpdateLocalPosition?.Invoke(beforeLocalPosition, localPosition);
    lastPosition = transform.position;
  }

  public Transform GetTransform()
  {
    if (cacheTransform == null)
      cacheTransform = transform;
    return cacheTransform;
  }

  private void OnDrawGizmos()
  {
    var transform = GetTransform();

    var start = transform.position + new Vector3(0, offsetDistanceJump, 0);
    if (CanJump(out var hit))
    {
      Gizmos.color = Color.red;
      Gizmos.DrawLine(start, hit.point);
    }
    else
    {
      Gizmos.color = Color.green;
      Gizmos.DrawLine(start, start + Vector3.down * maxDistanceAllowJump);
    }
  }
}
