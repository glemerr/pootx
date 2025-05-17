using UnityEngine;

public class SistemaVida : Estadistica

{ 

    public bool TakeDamage(int amount)
    {
        currentValue = Mathf.Clamp(currentValue - amount, minValue, maxValue);
        return currentValue <= minValue;
    }

    public void Heal(int amount)
    {
        currentValue = Mathf.Clamp(currentValue + amount, minValue, maxValue);
    }
}