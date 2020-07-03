using System.Collections.Generic;
using Quiver.Slime;
using UnityEngine;

public class BackgroundScroll : MonoBehaviour
{
  public GameObject[] prefabs;
  public float weight;
  public float size = 13.5f;

  private List<Transform> disable;
  private LinkedList<Transform> actives;
  private Transform _transform;
  private Transform playerTransform;
  private Vector3 lastPlayerPosition;

  private void Awake()
  {
    if (actives == null)
      actives = new LinkedList<Transform>();

    if (disable == null)
      disable = new List<Transform>();
  }

  private void Start()
  {
    _transform = transform;
    var gameMode = GameMode.Current;
    var player = gameMode.Player;

    playerTransform = player.GetTransform();
    lastPlayerPosition = playerTransform.position;

    player.onUpdatePosition += OnPlayerPositionUpdate;
    player.onUpdateLocalPosition += OnPlayerLocalPositionUpdate;

    // gameMode.onReset.AddListener(ResetPosition);
    Init();
  }

  private void OnPlayerLocalPositionUpdate(Vector3 before, Vector3 after)
  {
    lastPlayerPosition = after;
  }

  private void Init()
  {
    var start = -13.5f;

    for (int i = 0; i < 5; i++)
    {
      AddActive(start);
      start += size;
    }
  }

  private void AddActive(float startY)
  {
    var item = GetGameObject();
    item.gameObject.SetActive(true);
    item.localPosition = new Vector3(0, startY, -10);
    actives.AddFirst(item);
  }

  private void OnPlayerPositionUpdate(Vector3 before, Vector3 after)
  {
    UpdatePosition(after, weight);
  }

  private void UpdatePosition(Vector3 currentPosition, float weight)
  {
    var delta = currentPosition - lastPlayerPosition;

    foreach (var active in actives)
    {
      active.localPosition -= delta - (delta * weight);
    }

    lastPlayerPosition = currentPosition;

    var last = actives.Last.Value;
    if (last.localPosition.y < -20)
    {
      RemoveLastActive();
      var first = actives.First.Value;
      AddActive(first.localPosition.y + size);
    }
  }

  private void ResetPosition()
  {
    var index = 0;

    foreach (var active in actives)
    {
      active.localPosition = new Vector3(0, size * index, -10f);
      index++;
    }

    lastPlayerPosition = playerTransform.position;
  }

  private void RemoveLastActive()
  {
    var last = actives.Last.Value;
    actives.RemoveLast();
    last.gameObject.SetActive(false);
    disable.Add(last);
  }

  private Transform GetGameObject()
  {
    if (disable.Count > 1)
    {
      var randomIndex = UnityEngine.Random.Range(0, disable.Count);
      var item = disable[randomIndex];
      disable.RemoveAt(randomIndex);
      return item;
    }

    var randomPrefabIndex = UnityEngine.Random.Range(0, prefabs.Length);
    return Instantiate(prefabs[randomPrefabIndex], transform).transform;
  }
}
