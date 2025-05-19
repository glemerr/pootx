using System.Collections.Generic;
using UnityEngine;

public class NoJugable : Portadores
{
    [SerializeField] private GameObject efectoMuerte;
    
    protected override void AlMorir()
    {
        base.AlMorir();
        
        // Efecto visual
        if (efectoMuerte != null)
        {
            Instantiate(efectoMuerte, transform.position, Quaternion.identity);
        }
        
        // Destruir objeto después de un tiempo
        Destroy(gameObject, 1f);
    } 
 }