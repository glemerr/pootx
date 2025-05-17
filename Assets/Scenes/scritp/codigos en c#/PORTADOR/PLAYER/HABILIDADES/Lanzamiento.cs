using UnityEngine;
using System.Collections.Generic;
using System.Linq;
public class Lanzamiento : Habilidad
{
    public override int Use()
    {
        // Implementación del uso de lanzamiento
        Debug.Log($"Usando habilidad de lanzamiento: {nombre}");
        return 1;
    }
}