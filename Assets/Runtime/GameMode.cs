using UnityEngine;

namespace Quiver.Slime
{
  [RequireComponent(typeof(InputManager))]
  public class GameMode : MonoBehaviour
  {
    public int maxPlatformBefore;
    public float limiteHeight;
    public Player playerPrefab;
    public PlatformBuilder platformBuilder;
    private Player player;
    private InputManager inputManager;
    private int currentPlatformBefore;

    public Player Player => player;

    private void Awake()
    {
      inputManager = GetComponent<InputManager>();
    }

    private void Start()
    {
      platformBuilder.Build();
      player = CreatePlayer();
      player.ReceiveInput(inputManager);
      player.SetPlatformBuilder(platformBuilder);
      platformBuilder.Manager.onPlayerArrived.AddListener(OnPlayerArrived);

      inputManager.onAction += ResetGame;
    }

    private void OnPlayerArrived(Platform platform)
    {
      if (currentPlatformBefore >= maxPlatformBefore)
        platformBuilder.GetPlatform().BackToPool();
      else
        currentPlatformBefore++;

      var height = platform.Position.y;
      if (height < limiteHeight) return;

      var playerTransform = player.GetTransform();
      var playerOffset = playerTransform.localPosition - platform.Position;
      platformBuilder.ResetHeight();
      player.SetLocalPosition(playerOffset + platform.Position);
    }

    private void ResetGame()
    {
      if (!player.Status.IsDie) return;

      currentPlatformBefore = 0;
      player.Status.ResetStatus();
      player.GetTransform().position = Vector3.zero;

      platformBuilder.difficulty = 0;
      platformBuilder.Clear();
      platformBuilder.Build();
    }

    private Player CreatePlayer()
    {
      return Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
    }
  }
}
