using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Image barraVida;
    [SerializeField] private Image barraEnergia;
    [SerializeField] private Transform contenedorIconos;
    [SerializeField] private GameObject prefabIconoHabilidad;
    
    private List<IconoHabilidad> iconosHabilidades = new List<IconoHabilidad>();
    
    public void ActualizarBarraVida(int actual, int max)
    {
        if (barraVida != null)
        {
            barraVida.fillAmount = (float)actual / max;
        }
    }
    
    public void ActualizarBarraEnergia(int actual, int max)
    {
        if (barraEnergia != null)
        {
            barraEnergia.fillAmount = (float)actual / max;
        }
    }
    
    public void ActualizarIconosHabilidades(List<Habilidad> habilidades)
    {
        // Crear iconos si no existen
        if (iconosHabilidades.Count == 0 && contenedorIconos != null && prefabIconoHabilidad != null)
        {
            foreach (Habilidad hab in habilidades)
            {
                GameObject nuevoIcono = Instantiate(prefabIconoHabilidad, contenedorIconos);
                IconoHabilidad comp = nuevoIcono.GetComponent<IconoHabilidad>();
                if (comp != null)
                {
                    comp.Inicializar(hab);
                    iconosHabilidades.Add(comp);
                }
            }
        }
        
        // Actualizar iconos existentes
        for (int i = 0; i < iconosHabilidades.Count; i++)
        {
            if (i < habilidades.Count)
            {
                iconosHabilidades[i].Actualizar();
            }
        }
    }
}

public class IconoHabilidad : MonoBehaviour
{
    [SerializeField] private Image iconoImagen;
    [SerializeField] private Image cooldownOverlay;
    [SerializeField] private Text nombreText;
    
    private Habilidad habilidadAsociada;
    
    public void Inicializar(Habilidad habilidad)
    {
        habilidadAsociada = habilidad;
        
        if (iconoImagen != null)
        {
            iconoImagen.sprite = habilidad.GetIcono();
        }
        
        if (nombreText != null)
        {
            nombreText.text = habilidad.GetNombre();
        }
        
        Actualizar();
    }
    
    public void Actualizar()
    {
        if (habilidadAsociada != null && cooldownOverlay != null)
        {
            cooldownOverlay.fillAmount = 1 - habilidadAsociada.GetPorcentajeCooldown();
        }
    }
}