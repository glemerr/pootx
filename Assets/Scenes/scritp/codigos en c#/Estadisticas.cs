using UnityEngine;

public class Estadistica
{
    [SerializeField] private int minValue;
    [SerializeField] private int maxValue;
    private int currentValue;

    public Estadistica(int minValue, int maxValue)
    {
        this.minValue = minValue;
        this.maxValue = maxValue;
        this.currentValue = maxValue;
    }

    public int GetCurrentValue()
    {
        return currentValue;
    }

    public void SetCurrentValue(int value)
    {
        currentValue = Mathf.Clamp(value, minValue, maxValue);
    }

    public int GetMaxValue()
    {
        return maxValue;
    }

    public int GetMinValue()
    {
        return minValue;
    }

    public float GetPercentage()
    {
        return (float)currentValue / maxValue;
    }
}