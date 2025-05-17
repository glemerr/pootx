using UnityEngine;

public class AreaDnio : Habilidad
{


    public override int Use()
    {
        // Implementación del uso de área de daño
        Debug.Log($"Usando habilidad de área de daño: {nombre}");
       return 1;
    }
}
