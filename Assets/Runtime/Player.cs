using System;
using Quiver.Slime;
using UnityEngine;

public class Player : MonoBehaviour
{
  [Header("Dependencies")]
  [SerializeField] private CameraFollow cameraPrefab;

  [Header("Jump Config")]
  public float speed;
  public float maxDistanceAllowJump;
  public LayerMask mask;

  internal Platform currentPlatform;
  internal CameraFollow currentCamera;
  private PlatformBuilder platformBuilder;
  private Transform cacheTransform;

  public PlayerStatus Status { get; private set; }
  public Rigidbody2D Rigidbody { get; private set; }

  private void Awake()
  {
    Rigidbody = GetComponent<Rigidbody2D>();
    Status = GetComponent<PlayerStatus>();
    currentCamera = Instantiate(cameraPrefab);
    currentCamera.Player = this;
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
    hit = Physics2D.Raycast(GetTransform().position, Vector2.down, maxDistanceAllowJump, mask);
    return hit.collider != null;
  }

  public void SetLocalPosition(Vector3 localPosition)
  {
    var transform = GetTransform();
    var beforeWorldPosition = transform.position;
    transform.localPosition = localPosition;
    currentCamera.SetPlayerPosition(beforeWorldPosition, transform.position);
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

    if (CanJump(out var hit))
    {
      Gizmos.color = Color.red;
      Gizmos.DrawLine(transform.position, hit.point);
    }
    else
    {
      Gizmos.color = Color.green;
      Gizmos.DrawLine(transform.position, transform.position + Vector3.down * maxDistanceAllowJump);
    }
  }
}
