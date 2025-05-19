using System.Collections.Generic;
using UnityEngine;

public class AreaDaño : Habilidad
{
    [SerializeField] private float radio = 5f;
    [SerializeField] private float daño = 20f;
    [SerializeField] private GameObject efectoVisual;

    public override int Use()
    {
        if (base.Use() == 1)
        {
            // Crear efecto visual
            if (efectoVisual != null)
            {
                Instantiate(efectoVisual, transform.position, Quaternion.identity);
            }

            // Buscar portadores en el área
            Collider[] colliders = Physics.OverlapSphere(transform.position, radio);
            foreach (Collider col in colliders)
            {
                Portadores p = col.GetComponent<Portadores>();
                if (p != null && p != portador)
                {
                    p.RecibirDaño(Mathf.RoundToInt(daño));
                }
            }
            return 1;
        }
        return 0;
    }
}