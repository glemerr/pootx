using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class Habilidad : MonoBehaviour
{
    [SerializeField] protected Sprite icono;
    [SerializeField] protected string nombre;
    [SerializeField] protected float coolDown = 3f;
    [SerializeField] protected float costo = 10f;
    
    protected float tiempoRestante = 0f;
    protected Portadores portador;

    public void Inicializar(Portadores portador)
    {
        this.portador = portador;
    }

    public Sprite GetIcono()
    {
        return icono;
    }

    public string GetNombre()
    {
        return nombre;
    }

    public float GetCooldown()
    {
        return coolDown;
    }

    public float GetCosto()
    {
        return costo;
    }

    public bool PuedeUsar()
    {
        return tiempoRestante <= 0;
    }

    public float GetPorcentajeCooldown()
    {
        if (coolDown <= 0) return 1;
        return 1 - (tiempoRestante / coolDown);
    }

    public void ActualizarCooldown(float deltaTime)
    {
        if (tiempoRestante > 0)
        {
            tiempoRestante -= deltaTime;
        }
    }

    public virtual int Use()
    {
        if (PuedeUsar())
        {
            tiempoRestante = coolDown;
            return 1; // Éxito
        }
        return 0; // Fallo
    }
}