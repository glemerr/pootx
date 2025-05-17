using UnityEngine;

public class SistemaEnergia : Estadistica
{
    public bool UseEnergy(int amount)
    {
        if (currentValue >= amount)
        {
            currentValue -= amount;
            return true;
        }
        return false;
    }

    public void RecoverEnergy(int amount)
    {
        currentValue = Mathf.Clamp(currentValue + amount, minValue, maxValue);
    }
}
