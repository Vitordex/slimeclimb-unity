using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
  public float speed;
  public float maxDistanceAllowJump;
  public LayerMask mask;

  public ParticleSystem die_PS;
  public Platform currentPlatform;

  private bool isDie;
  private Rigidbody2D rb2D;
  private Transform cacheTransform;

  [Header("Event")]
  public UnityEvent onDie;

  public bool IsDie => isDie;

  private void Awake()
  {
    rb2D = GetComponent<Rigidbody2D>();
  }

  public void Jump()
  {
    if (!isDie && CanJump(out var hit))
    {
      GetTransform().position = currentPlatform.GetTransform().position;
      rb2D.velocity = Vector2.up * speed * rb2D.gravityScale;
    }
  }

  private bool CanJump(out RaycastHit2D hit)
  {
    hit = Physics2D.Raycast(GetTransform().position, Vector2.down, maxDistanceAllowJump, mask);
    return hit.collider != null;
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
