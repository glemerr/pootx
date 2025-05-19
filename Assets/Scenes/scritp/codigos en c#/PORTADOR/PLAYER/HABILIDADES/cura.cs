using System.Collections.Generic;
using UnityEngine;

public class Cura : Habilidad
{
    [SerializeField] private float cantidadCuracion = 25f;
    [SerializeField] private GameObject efectoCuracion;

    public override int Use()
    {
        if (base.Use() == 1)
        {
            // Curar al portador
            portador.RecibirCuracion(Mathf.RoundToInt(cantidadCuracion));

            // Efecto visual
            if (efectoCuracion != null)
            {
                Instantiate(efectoCuracion, portador.transform.position, Quaternion.identity);
            }
            return 1;
        }
        return 0;
    }
}
