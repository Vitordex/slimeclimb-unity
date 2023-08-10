using UnityEngine;
using UnityEngine.EventSystems;

public delegate void Notify();

public class InputManager : MonoBehaviour
{
  public Notify onAction;
  public Notify onToggleCheat;

  private void Update()
  {
    if (Input.touchCount >= 3 || Input.GetKeyDown(KeyCode.Mouse1))
      onToggleCheat.Invoke();

    var isOver = EventSystem.current.IsPointerOverGameObject();
    if (isOver) return;

    if (Input.GetKeyDown(KeyCode.Mouse0))
    {
      onAction?.Invoke();
    }
  }
}
