using UnityEngine;
using UnityEngine.UI;

public class PlayerStatus : MonoBehaviour
{
  public int currentScore;
  public Text scoreText;

  public void AddScore(int plus)
  {
    currentScore += Mathf.Abs(plus);

    scoreText.text = currentScore.ToString();
  }
}