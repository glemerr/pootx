using UnityEngine;

public class NoJugable : portadores
{
    public override void Initialize()
    {
        base.Initialize();
        // Inicializa el sistema de vida del enemigo
        vida.Initialize(0, 100, 100);
    }
}
