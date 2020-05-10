using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PlayerStatus : MonoBehaviour
{
  public int currentScore;
  public Text scoreText;

  [Header("Event")]
  public UnityEvent onDie;

  private bool isDie;
  private Player player;

  public bool IsDie => isDie;

  private void Awake()
  {
    player = GetComponent<Player>();
  }

  internal void AddScore(int plus)
  {
    currentScore += Mathf.Abs(plus);
    scoreText.text = currentScore.ToString();
  }

  internal void Die()
  {
    player.Rigidbody.bodyType = RigidbodyType2D.Static;
    isDie = true;
    onDie.Invoke();
  }
}
