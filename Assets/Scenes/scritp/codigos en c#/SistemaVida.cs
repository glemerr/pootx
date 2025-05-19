using UnityEngine;
using System;

public class SistemaVida
{
    private Estadistica estadisticaVida;
    public event Action OnMuerte;

    public SistemaVida(int vidaMaxima)
    {
        estadisticaVida = new Estadistica(0, vidaMaxima);
    }

    public int GetVidaActual()
    {
        return estadisticaVida.GetCurrentValue();
    }

    public int GetVidaMaxima()
    {
        return estadisticaVida.GetMaxValue();
    }

    public void SetVidaActual(int value)
    {
        int vidaAnterior = estadisticaVida.GetCurrentValue();
        estadisticaVida.SetCurrentValue(value);
        
        if (vidaAnterior > 0 && estadisticaVida.GetCurrentValue() <= 0)
        {
            OnMuerte?.Invoke();
        }
    }

    public void RecibirDaño(int cantidad)
    {
        SetVidaActual(GetVidaActual() - cantidad);
    }

    public void RecibirCuracion(int cantidad)
    {
        SetVidaActual(GetVidaActual() + cantidad);
    }

    public bool EstaVivo()
    {
        return GetVidaActual() > 0;
    }

    public float GetPorcentajeVida()
    {
        return estadisticaVida.GetPercentage();
    }
}