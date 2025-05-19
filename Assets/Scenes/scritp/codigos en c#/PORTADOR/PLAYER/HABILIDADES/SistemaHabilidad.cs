using System.Collections.Generic;
using UnityEngine;

public class SistemaHabilidad : MonoBehaviour
{
    public List<Habilidad> habilidades = new List<Habilidad>();
    private Portadores portador;

    public void Inicializar(Portadores portador)
    {
        this.portador = portador;
        
        // Inicializar habilidades existentes
        foreach (Habilidad hab in habilidades)
        {
            hab.Inicializar(portador);
        }
    }

    public void AgregarHabilidad(Habilidad habilidad)
    {
        if (habilidades.Count < 3 && !habilidades.Contains(habilidad))
        {
            habilidad.Inicializar(portador);
            habilidades.Add(habilidad);
        }
    }

    public int Use(int targetAbility)
    {
        if (targetAbility >= 0 && targetAbility < habilidades.Count)
        {
            return habilidades[targetAbility].Use();
        }
        return 0;
    }

    public void ActualizarCooldowns(float deltaTime)
    {
        foreach (Habilidad hab in habilidades)
        {
            hab.ActualizarCooldown(deltaTime);
        }
    }
}
