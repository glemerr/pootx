using UnityEngine;

public class Habilidad : MonoBehaviour
{
    public Sprite icono;
    public string nombre;
    public float coolDown;
    public float coss;

    public virtual int Use()
    {
        // Implementación del uso de habilidad genérica
        Debug.Log($"Usando habilidad: {nombre}");
        return 1;
    }
}
