using System.Collections.Generic;
using UnityEngine;

public class SistemaHabilidad : MonoBehaviour
{
    public List<Habilidad> habilidades = new List<Habilidad>();
    public int numHabilidades = 0;

    public int Use (int targetAbility)
    {
        if (targetAbility >= 0 && targetAbility < habilidades.Count)
        {
            return habilidades[targetAbility].Use();
        }
        
        Debug.LogWarning("Índice de habilidad fuera de rango");
        return 1;
    }
}
