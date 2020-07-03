using UnityEngine;
using UnityEngine.Events;

namespace Quiver.Slime
{
  [RequireComponent(typeof(InputManager))]
  public class GameMode : MonoBehaviour
  {
    public static GameMode Current;
    public Vector3 startCameraPosition;
    public int maxPlatformBefore;
    public float limiteHeight;
    public Player playerPrefab;
    public CameraFollow cameraPrefab;
    public PlatformBuilder platformBuilder;

    public UnityEvent onReset;

    private Player player;
    private CameraFollow cam;
    private InputManager inputManager;
    private int currentPlatformBefore;

    public Player Player => player;
    public InputManager InputManager => inputManager;

    private void Awake()
    {
      if (Current == null)
      {
        Current = this;
      }

      inputManager = GetComponent<InputManager>();
      player = CreatePlayer();
      cam = Instantiate(cameraPrefab, startCameraPosition, Quaternion.identity);
      cam.Player = player;
    }

    private void Start()
    {
      platformBuilder.Build();
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
      platformBuilder.ResetGame();
      var begin = platformBuilder.Peek();

      player.currentPlatform = begin;
      var startPosition = begin.GetTransform().position;
      player.GetTransform().position = startPosition;
      cam.ResetPosition(startPosition);
      player.Status.ResetStatus();
      onReset.Invoke();
    }

    private Player CreatePlayer()
    {
      return Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
    }
  }
}
