using UnityEngine;

public class Estadistica : MonoBehaviour
{
    public int currentValue;
    public int maxValue;
    public int minValue;

    public void Initialize(int min, int max, int current)
    {
        minValue = min;
        maxValue = max;
        currentValue = Mathf.Clamp(current, minValue, maxValue);
    }

    public void ModifyValue(int amount)
    {
        currentValue = Mathf.Clamp(currentValue + amount, minValue, maxValue);
    }
}
