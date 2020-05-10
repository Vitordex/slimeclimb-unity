using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(InputManager))]
public class GameManager : MonoBehaviour
{
  private Player player;
  public bool isShowStatus;
  public GameObject status;

  private void Awake()
  {
    var inputManager = GetComponent<InputManager>();
    inputManager.onAction += Action;

    player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
  }

  private void Action()
  {
    if (player.IsDie)
    {
      if (!isShowStatus)
        ShowStatus();
      else
        SceneManager.LoadScene(0);
    }
    else
    {
      player.Jump();
    }
  }

  private void ShowStatus()
  {
    status.SetActive(true);
    isShowStatus = true;
  }
}
