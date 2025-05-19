using UnityEngine;

// Jugador2: Usa energía para activar habilidades
public class Jugador2 : Jugable
{
    [SerializeField] private int[] costesEnergia = new int[3] { 15, 30, 25 };
    
    public override int Use(int targetAbility)
    {
        if (targetAbility >= 0 && targetAbility < costesEnergia.Length && 
            sistemaHabilidades != null && targetAbility < sistemaHabilidades.habilidades.Count)
        {
            Habilidad hab = sistemaHabilidades.habilidades[targetAbility];
            
            if (hab.PuedeUsar() && ConsumirEnergia(costesEnergia[targetAbility]))
            {
                // Usar habilidad (la energía ya se consumió)
                return sistemaHabilidades.Use(targetAbility);
            }
        }
        return 0;
    }
}
