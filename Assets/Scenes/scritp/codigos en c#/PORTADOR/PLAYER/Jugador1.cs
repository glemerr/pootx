using UnityEngine;

public class Jugador1 : Jugable
{
    private void Awake()
    {
        // Inicializar componentes si no existen
        if (energia == null)
        {
            energia = gameObject.AddComponent<SistemaEnergia>();
            energia.Initialize(0, 100, 100);
        }

        if (vida == null)
        {
            vida = gameObject.AddComponent<SistemaVida>();
            vida.Initialize(0, 100, 100);
        }

        if (sistemaHabilidades == null)
        {
            sistemaHabilidades = gameObject.AddComponent<SistemaHabilidad>();
        }
    }

    public override int Use(int targetAbility)
    {
        Debug.Log($"Jugador1 usa habilidad {targetAbility}");
        return base.Use(targetAbility);
    }
}