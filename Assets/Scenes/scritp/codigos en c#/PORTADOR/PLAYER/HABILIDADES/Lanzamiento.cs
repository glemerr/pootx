using System.Collections.Generic;
using UnityEngine;

public class Lanzamiento : Habilidad
{
    [SerializeField] private GameObject prefabProyectil;
    [SerializeField] private float velocidad = 10f;
    [SerializeField] private float daño = 15f;
    [SerializeField] private Transform puntoDisparo;

    public override int Use()
    {
        if (base.Use() == 1)
        {
            // Obtener punto de disparo
            Transform punto = puntoDisparo != null ? puntoDisparo : transform;

            // Instanciar proyectil
            GameObject proyectil = Instantiate(prefabProyectil, punto.position, punto.rotation);

            // Configurar proyectil
            Proyectil comp = proyectil.GetComponent<Proyectil>();
            if (comp != null)
            {
                comp.Inicializar(Mathf.RoundToInt(daño), velocidad, portador.gameObject.layer);
            }
            else
            {
                // Si no tiene componente Proyectil, al menos darle velocidad
                Rigidbody rb = proyectil.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.velocity = punto.forward * velocidad;
                }

                // Destruir después de un tiempo
                Destroy(proyectil, 5f);
            }
            return 1;
        }
        return 0;
    }
}