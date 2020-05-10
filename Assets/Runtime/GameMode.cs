using System;
using UnityEngine;

namespace Quiver.Slime
{
  [RequireComponent(typeof(InputManager))]
  public class GameMode : MonoBehaviour
  {
    public int maxPlatformBefore;
    public Player playerPrefab;
    public PlatformBuilder platformBuilder;
    private Player player;
    private InputManager inputManager;
    public int currentPlatformBefore;

    public Player Player => player;

    private void Awake()
    {
      inputManager = GetComponent<InputManager>();
    }

    private void Start()
    {
      platformBuilder.InitOrReset();
      player = CreatePlayer();
      player.ReceiveInput(inputManager);
      player.SetPlatformBuilder(platformBuilder);

      platformBuilder.Manager.onPlayerArrived.AddListener(OnPlayerArrived);
    }

    private void OnPlayerArrived(Platform platform)
    {
      if (currentPlatformBefore >= maxPlatformBefore)
      {
        var firstPlatform = platformBuilder.GetPlatform();
        Destroy(firstPlatform.gameObject);
      }
      else
      {
        currentPlatformBefore++;
      }

      var height = platform.Position.y;
      if (height < 24) return;

      var playerTransform = player.GetTransform();
      var playerOffset = playerTransform.localPosition - platform.Position;
      platformBuilder.ResetHeight();
      player.SetLocalPosition(playerOffset + platform.Position);
    }

    private Player CreatePlayer()
    {
      return Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
    }
  }
}
