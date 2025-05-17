using UnityEngine;

public abstract class Jugable : MonoBehaviour
{
    
    public SistemaEnergia energia;
    public SistemaVida vida;
    public SistemaHabilidad sistemaHabilidades;

    public virtual int Use(int targetAbility)
    {
        // Verificar si tiene suficiente energía para usar la habilidad
        if (energia != null && sistemaHabilidades != null)
        {
            // Aquí se podría implementar la lógica para verificar el costo de la habilidad
            // y reducir la energía correspondiente
            return sistemaHabilidades.Use(targetAbility);
        }
        return 0;
    }

    
}
