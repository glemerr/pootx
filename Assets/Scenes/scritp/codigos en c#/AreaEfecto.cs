using UnityEngine;

public abstract class AreaEfecto : MonoBehaviour
{
    [SerializeField] protected float valorEfecto = 10f;
    [SerializeField] protected bool activo = true;
    [SerializeField] protected float tiempoEntreTriggers = 1f;
    [SerializeField] protected GameObject efectoVisual;
    
    protected float tiempoUltimoTrigger;
    
    protected virtual void Start()
    {
        tiempoUltimoTrigger = -tiempoEntreTriggers;
    }
    
    protected virtual void OnTriggerEnter(Collider other)
    {
        if (activo && Time.time >= tiempoUltimoTrigger + tiempoEntreTriggers)
        {
            Portadores portador = other.GetComponent<Portadores>();
            if (portador != null)
            {
                AplicarEfecto(portador);
                tiempoUltimoTrigger = Time.time;
                
                // Efecto visual
                if (efectoVisual != null)
                {
                    Instantiate(efectoVisual, other.transform.position, Quaternion.identity);
                }
            }
        }
    }
    
    public abstract void AplicarEfecto(Portadores portador);
}

public class AreaDañoEntorno : AreaEfecto
{
    public override void AplicarEfecto(Portadores portador)
    {
        portador.RecibirDaño(Mathf.RoundToInt(valorEfecto));
    }
}

public class AreaCarga : AreaEfecto
{
    public override void AplicarEfecto(Portadores portador)
    {
        Jugable jugable = portador as Jugable;
        if (jugable != null)
        {
            jugable.RegenerarEnergia(Mathf.RoundToInt(valorEfecto));
        }
    }
}