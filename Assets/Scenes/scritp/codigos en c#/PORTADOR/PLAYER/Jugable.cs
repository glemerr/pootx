using UnityEngine;


public class Jugable : Portadores
{
    [SerializeField] protected int energiaMaxima = 100;
    [SerializeField] protected TipoRegeneracion tipoRegeneracion = TipoRegeneracion.Tiempo;
    
    protected SistemaEnergia sistemaEnergia;
    protected SistemaHabilidad sistemaHabilidades;
    
    [SerializeField] protected UIManager uiManager;
    
    protected override void Awake()
    {
        base.Awake();
        
        sistemaEnergia = new SistemaEnergia(energiaMaxima, tipoRegeneracion);
        
        sistemaHabilidades = GetComponent<SistemaHabilidad>();
        if (sistemaHabilidades != null)
        {
            sistemaHabilidades.Inicializar(this);
        }
    }
    
    protected virtual void Update()
    {
        // Actualizar cooldowns de habilidades
        if (sistemaHabilidades != null)
        {
            sistemaHabilidades.ActualizarCooldowns(Time.deltaTime);
        }
        
        // Regenerar energía si es por tiempo
        sistemaEnergia.ActualizarRegeneracion(Time.deltaTime);
        
        // Actualizar UI
        if (uiManager != null)
        {
            uiManager.ActualizarBarraVida(GetVidaActual(), GetVidaMaxima());
            uiManager.ActualizarBarraEnergia(GetEnergiaActual(), GetEnergiaMaxima());
            
            if (sistemaHabilidades != null)
            {
                uiManager.ActualizarIconosHabilidades(sistemaHabilidades.habilidades);
            }
        }
    }
    
    public virtual int Use(int targetAbility)
    {
        if (sistemaHabilidades != null)
        {
            return sistemaHabilidades.Use(targetAbility);
        }
        return 0;
    }
    
    public int GetEnergiaActual()
    {
        return sistemaEnergia.GetEnergiaActual();
    }
    
    public int GetEnergiaMaxima()
    {
        return sistemaEnergia.GetEnergiaMaxima();
    }
    
    public float GetPorcentajeEnergia()
    {
        return sistemaEnergia.GetPorcentajeEnergia();
    }
    
    public bool ConsumirEnergia(int cantidad)
    {
        return sistemaEnergia.ConsumirEnergia(cantidad);
    }
    
    public void RegenerarEnergia(int cantidad)
    {
        sistemaEnergia.RegenerarEnergia(cantidad);
    }
}
      
