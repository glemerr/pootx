using UnityEngine;

public enum TipoRegeneracion
{
    Tiempo,
    Interaccion,
    Ninguna
}

public class SistemaEnergia
{
    private Estadistica estadisticaEnergia;
    private TipoRegeneracion tipoRegeneracion;
    private float tasaRegeneracion = 5f;

    public SistemaEnergia(int energiaMaxima, TipoRegeneracion tipo)
    {
        estadisticaEnergia = new Estadistica(0, energiaMaxima);
        tipoRegeneracion = tipo;
    }

    public int GetEnergiaActual()
    {
        return estadisticaEnergia.GetCurrentValue();
    }

    public int GetEnergiaMaxima()
    {
        return estadisticaEnergia.GetMaxValue();
    }

    public void SetEnergiaActual(int value)
    {
        estadisticaEnergia.SetCurrentValue(value);
    }

    public bool ConsumirEnergia(int cantidad)
    {
        if (GetEnergiaActual() >= cantidad)
        {
            SetEnergiaActual(GetEnergiaActual() - cantidad);
            return true;
        }
        return false;
    }

    public void RegenerarEnergia(int cantidad)
    {
        SetEnergiaActual(GetEnergiaActual() + cantidad);
    }

    public void ActualizarRegeneracion(float deltaTime)
    {
        if (tipoRegeneracion == TipoRegeneracion.Tiempo)
        {
            RegenerarEnergia(Mathf.RoundToInt(tasaRegeneracion * deltaTime));
        }
    }

    public float GetPorcentajeEnergia()
    {
        return estadisticaEnergia.GetPercentage();
    }
}