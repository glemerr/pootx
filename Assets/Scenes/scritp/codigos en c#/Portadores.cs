using UnityEngine;

public class Portadores : MonoBehaviour
{
    [SerializeField] protected int vidaMaxima = 100;
    protected SistemaVida sistemaVida;
    
    protected virtual void Awake()
    {
        sistemaVida = new SistemaVida(vidaMaxima);
        sistemaVida.OnMuerte += AlMorir;
    }
    
    public virtual void RecibirDaño(int cantidad)
    {
        sistemaVida.RecibirDaño(cantidad);
        Debug.Log(gameObject.name + " recibió " + cantidad + " de daño. Vida actual: " + sistemaVida.GetVidaActual());
    }
    
    public virtual void RecibirCuracion(int cantidad)
    {
        sistemaVida.RecibirCuracion(cantidad);
        Debug.Log(gameObject.name + " recibió " + cantidad + " de curación. Vida actual: " + sistemaVida.GetVidaActual());
    }
    
    public bool EstaVivo()
    {
        return sistemaVida.EstaVivo();
    }
    
    public int GetVidaActual()
    {
        return sistemaVida.GetVidaActual();
    }
    
    public int GetVidaMaxima()
    {
        return sistemaVida.GetVidaMaxima();
    }
    
    public float GetPorcentajeVida()
    {
        return sistemaVida.GetPorcentajeVida();
    }
    
    protected virtual void AlMorir()
    {
        Debug.Log(gameObject.name + " ha muerto");
    }
}