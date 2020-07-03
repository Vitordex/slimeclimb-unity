using System;
using UnityEngine;

namespace Quiver.Slime
{
  public class CameraFollow : MonoBehaviour
  {
    public float speed;
    public Vector3 offset;
    private Transform camTransform;
    private Transform cacheTransform;

    public Player Player { get; internal set; }

    private void Awake()
    {
      camTransform = Camera.main.transform;
    }

    private void Update()
    {
      var transform = GetTransform();
      var playerPosition = Player.GetTransform().position;
      transform.position = playerPosition;

      var target = Player.currentPlatform;
      var position = transform.position;
      var targetHeight = target.Position.y;

      if (playerPosition.y < targetHeight)
      {
        position.y = targetHeight;
        transform.position = position;
      }

      var camPosition = camTransform.position;
      var targetPosition = transform.position + offset;
      camTransform.position = Vector3.Lerp(camPosition, targetPosition, Time.deltaTime * speed);
    }

    public Transform GetTransform()
    {
      if (cacheTransform == null)
        cacheTransform = transform;

      return cacheTransform;
    }

    internal void SetPlayerPosition(Vector3 before, Vector3 after)
    {
      var virtualOffset = camTransform.position - before;
      var offset = transform.position - before;
    }

    public void SetPosition(Vector3 position)
    {
      transform.position = position;
      camTransform.position = position;
    }

    public void ResetPosition(Vector3 position)
    {
      transform.position = position + offset;
      camTransform.position = position + offset;
    }
  }
}
