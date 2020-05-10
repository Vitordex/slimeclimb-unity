using UnityEngine;

public delegate void Notify();

public class InputManager : MonoBehaviour
{
  public Notify onAction;

  private void Update()
  {
    if (Input.GetKeyDown(KeyCode.Mouse0))
    {
      onAction?.Invoke();
    }
  }
}
