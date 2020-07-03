using UnityEngine;

[CreateAssetMenu(fileName = "PlatformConfig")]
public class PlatformConfig : ScriptableObject
{
  public ColorPreset[] presets;

  public void FindColor(uint weight, out Color color, out float delay)
  {
    for (int i = 0; i < presets.Length - 1; i++)
    {
      var current = presets[i];
      var next = presets[i + 1];

      if (current.weight <= weight && weight < next.weight)
      {
        color = current.color;
        delay = current.delay;
        return;
      }
    }

    var last = presets[presets.Length - 1];
    color = last.color;
    delay = last.delay;
  }

  [System.Serializable]
  public struct ColorPreset
  {
    public Color color;
    public float delay;
    public uint weight;
  }
}
