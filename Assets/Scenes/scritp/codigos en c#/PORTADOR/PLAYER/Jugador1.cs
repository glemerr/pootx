using UnityEngine;

// Jugador1: Usa vida para activar habilidades
public class Jugador1 : Jugable
{
    [SerializeField] private int[] costesVida = new int[3] { 10, 20, 5 };
    
    public override int Use(int targetAbility)
    {
        if (targetAbility >= 0 && targetAbility < costesVida.Length && 
            sistemaHabilidades != null && targetAbility < sistemaHabilidades.habilidades.Count)
        {
            Habilidad hab = sistemaHabilidades.habilidades[targetAbility];
            
            if (hab.PuedeUsar() && GetVidaActual() > costesVida[targetAbility])
            {
                // Consumir vida
                RecibirDaño(costesVida[targetAbility]);
                
                // Usar habilidad
                return sistemaHabilidades.Use(targetAbility);
            }
        }
        return 0;
    }
}