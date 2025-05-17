using UnityEngine;

public class Cura : Habilidad
{

    public override int Use()
    {
        // Implementación del uso de curación
        Debug.Log($"Usando habilidad de curación: {nombre}");
        return 1;
    }
}
