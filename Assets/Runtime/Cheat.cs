using Quiver.Slime;
using UnityEngine;

public class Cheat : MonoBehaviour
{
  private bool isShow;
  private GameObject canvas;

  private void Awake()
  {
    canvas = GetComponentInChildren<Canvas>(true).gameObject;
  }

  private void Start()
  {
    var inputManageeer = GameMode.Current.InputManager;
    inputManageeer.onToggleCheat += ToggleShow;
  }

  public void InvulnerableToggle()
  {
    var playerTrigger = GameMode.Current
    .Player.GetComponent<PlayerTrigger>();
    playerTrigger.isInvulnerable = !playerTrigger.isInvulnerable;
  }

  public void ToggleShow()
  {
    isShow = !isShow;
    canvas.SetActive(isShow);
  }
}
