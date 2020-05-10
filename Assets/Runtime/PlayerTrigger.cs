using UnityEngine;

namespace Quiver.Slime
{
  public class PlayerTrigger : MonoBehaviour
  {
    [SerializeField] private string DamageTag;
    private Player player;

    public PlayerTrigger() : this("Damage")
    {
    }

    public PlayerTrigger(string damageTag)
    {
      DamageTag = damageTag;
    }

    private void Awake()
    {
      player = GetComponent<Player>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
      var status = player.Status;
      if (!status.IsDie && other.CompareTag(DamageTag))
      {
        status.Die();
      }
    }
  }
}
