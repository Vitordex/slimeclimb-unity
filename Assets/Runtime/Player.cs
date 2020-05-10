using Quiver.Slime;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
  [Header("Dependencies")]
  public CameraFollow cameraPrefab;
  public ParticleSystem die_PS;

  [Header("Jump Config")]
  public float speed;
  public float maxDistanceAllowJump;
  public LayerMask mask;


  [Header("Event")]
  public UnityEvent onDie;

  private bool isDie;
  internal Platform currentPlatform;
  private CameraFollow currentCamera;
  private Rigidbody2D rb2D;
  private PlayerStatus status;
  private Transform cacheTransform;
  private PlatformBuilder platformBuilder;

  public bool IsDie => isDie;

  private void Awake()
  {
    rb2D = GetComponent<Rigidbody2D>();
    status = GetComponent<PlayerStatus>();
    currentCamera = Instantiate(cameraPrefab);
    currentCamera.Player = this;
  }

  public void Jump()
  {
    if (!isDie && CanJump(out var hit))
    {
      GetTransform().position = currentPlatform.GetTransform().position;
      rb2D.velocity = Vector2.up * speed * rb2D.gravityScale;
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
    status.AddScore(platform.Score);
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

  private void OnTriggerEnter2D(Collider2D other)
  {
    if (isDie) return;
    if (other.CompareTag("Damage"))
    {
      Die();
    }
  }

  private void Die()
  {
    rb2D.bodyType = RigidbodyType2D.Static;
    enabled = false;
    isDie = true;
    die_PS.Play();
    onDie.Invoke();
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
