using UnityEngine;

public class portadores : MonoBehaviour
{
    public SistemaVida vida;

    public virtual void Initialize()
    {
        if (vida == null)
        {
            vida = gameObject.AddComponent<SistemaVida>();
            vida.Initialize(0, 100, 100);
        }
    }
}
