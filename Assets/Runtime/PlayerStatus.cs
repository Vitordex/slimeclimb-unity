using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PlayerStatus : MonoBehaviour
{
  public int currentScore;
  public Text scoreText;

  [Header("Event")]
  public UnityEvent onReset;
  public UnityEvent onDie;

  private bool isDie;
  private Player player;

  public bool IsDie => isDie;

  private void Awake()
  {
    player = GetComponent<Player>();
  }

  internal void ResetStatus()
  {
    isDie = false;
    SetScore(0);
    player.Rigidbody.bodyType = RigidbodyType2D.Dynamic;
    onReset.Invoke();
  }

  internal void AddScore(int plus)
  {
    SetScore(currentScore + Mathf.Abs(plus));
  }

  internal void Die()
  {
    player.Rigidbody.bodyType = RigidbodyType2D.Static;
    isDie = true;
    onDie.Invoke();
  }

  private void SetScore(int value)
  {
    currentScore = value;
    scoreText.text = currentScore.ToString();
  }
}
