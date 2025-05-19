using UnityEngine;

public class Proyectil : MonoBehaviour
{
    private int daño;
    private float velocidad;
    private int capaOrigen;
    private bool impactado = false;

    public void Inicializar(int daño, float velocidad, int capaOrigen)
    {
        this.daño = daño;
        this.velocidad = velocidad;
        this.capaOrigen = capaOrigen;
        
        // Aplicar velocidad
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = transform.forward * velocidad;
        }
        
        // Destruir después de un tiempo
        Destroy(gameObject, 5f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (impactado || other.gameObject.layer == capaOrigen)
            return;

        impactado = true;
        
        // Aplicar daño si es un portador
        Portadores portador = other.GetComponent<Portadores>();
        if (portador != null)
        {
            portador.RecibirDaño(daño);
        }
        
        // Efecto de impacto (opcional)
        // Aquí podrías instanciar un efecto visual
        
        // Destruir proyectil
        Destroy(gameObject);
    }
}