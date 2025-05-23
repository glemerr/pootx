using UnityEngine;

public class Proyectil : MonoBehaviour
{
    [Header("Configuración del Proyectil")]
    private int daño;
    private float velocidad;
    private int capaOrigen;
    private bool impactado = false;

    [Header("Efectos de Impacto")]
    [SerializeField] private GameObject efectoImpacto; // Prefab del efecto de partículas
    [SerializeField] private GameObject efectoImpactoEnemigo; // Efecto específico para enemigos
    [SerializeField] private GameObject efectoImpactoSuperficie; // Efecto para superficies normales
    [SerializeField] private AudioClip sonidoImpacto; // Sonido de impacto
    [SerializeField] private AudioClip sonidoImpactoEnemigo; // Sonido específico para enemigos
    [SerializeField] private float volumenSonido = 0.5f;
    [SerializeField] private bool usarCameraShake = true;
    [SerializeField] private float fuerzaCameraShake = 0.1f;
    [SerializeField] private float duracionCameraShake = 0.2f;

    private AudioSource audioSource;

    void Awake()
    {
        // Crear AudioSource si no existe
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        
        audioSource.playOnAwake = false;
        audioSource.volume = volumenSonido;
    }

    public void Inicializar(int daño, float velocidad, int capaOrigen)
    {
        this.daño = daño;
        this.velocidad = velocidad;
        this.capaOrigen = capaOrigen;
        
        // Aplicar velocidad
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = transform.forward * velocidad;
        }
        
        // Destruir después de un tiempo
        Destroy(gameObject, 5f);
    }

    // Sobrecarga para incluir efectos personalizados
    public void Inicializar(int daño, float velocidad, int capaOrigen, 
                           GameObject efectoPersonalizado = null, 
                           AudioClip sonidoPersonalizado = null)
    {
        Inicializar(daño, velocidad, capaOrigen);
        
        if (efectoPersonalizado != null)
            efectoImpacto = efectoPersonalizado;
            
        if (sonidoPersonalizado != null)
            sonidoImpacto = sonidoPersonalizado;
    }

    private void OnTriggerEnter(Collider other)
    {
        Portadores portador = other.GetComponent<Portadores>();

        if (impactado || other.gameObject.layer == capaOrigen)
            return;

        impactado = true;

        // Determinar tipo de impacto y aplicar efectos correspondientes
        bool esEnemigo = other.CompareTag("Enemy");
        bool esPortador = portador != null;
       

        // Destruir proyectil después de un pequeño delay para que se reproduzcan los efectos
        Destroy(gameObject, 0.1f);
    }

    private void CrearEfectoImpacto(Vector3 posicionImpacto, Transform objetoImpactado, bool esEnemigo)
    {
        GameObject efectoAUsar = null;

        // Seleccionar el efecto apropiado
        if (esEnemigo && efectoImpactoEnemigo != null)
        {
            efectoAUsar = efectoImpactoEnemigo;
        }
        else if (!esEnemigo && efectoImpactoSuperficie != null)
        {
            efectoAUsar = efectoImpactoSuperficie;
        }
        else if (efectoImpacto != null)
        {
            efectoAUsar = efectoImpacto;
        }

        if (efectoAUsar != null)
        {
            // Calcular rotación del efecto basada en la normal de la superficie
            Vector3 direccionImpacto = (posicionImpacto - transform.position).normalized;
            Quaternion rotacionEfecto = Quaternion.LookRotation(-direccionImpacto);

            // Instanciar efecto
            GameObject efecto = Instantiate(efectoAUsar, posicionImpacto, rotacionEfecto);
            
            // Configurar el efecto
            ConfigurarEfectoImpacto(efecto, esEnemigo);
            
            // Destruir efecto después de un tiempo
            Destroy(efecto, 3f);
        }
        else
        {
            // Crear efecto básico si no hay prefab asignado
            CrearEfectoBasico(posicionImpacto, esEnemigo);
        }
    }

    private void ConfigurarEfectoImpacto(GameObject efecto, bool esEnemigo)
    {
        // Configurar sistema de partículas si existe
        ParticleSystem particulas = efecto.GetComponent<ParticleSystem>();
        if (particulas != null)
        {
            var main = particulas.main;
            
            if (esEnemigo)
            {
                // Efecto más dramático para enemigos (rojo/naranja)
                main.startColor = new Color(1f, 0.3f, 0f, 1f); // Naranja rojizo
                main.startSize = 0.3f;
                main.startLifetime = 1.5f;
            }
            else
            {
                // Efecto normal para superficies (gris/blanco)
                main.startColor = new Color(0.8f, 0.8f, 0.8f, 1f); // Gris claro
                main.startSize = 0.2f;
                main.startLifetime = 1f;
            }
        }

        // Configurar luz si existe
        Light luz = efecto.GetComponent<Light>();
        if (luz != null)
        {
            luz.color = esEnemigo ? Color.red : Color.white;
            luz.intensity = esEnemigo ? 2f : 1f;
            luz.range = esEnemigo ? 3f : 2f;
            
            // Hacer que la luz se desvanezca
            StartCoroutine(DesvanecerrLuz(luz, 0.5f));
        }
    }

    private System.Collections.IEnumerator DesvanecerrLuz(Light luz, float duracion)
    {
        float intensidadInicial = luz.intensity;
        float tiempo = 0f;

        while (tiempo < duracion && luz != null)
        {
            tiempo += Time.deltaTime;
            luz.intensity = Mathf.Lerp(intensidadInicial, 0f, tiempo / duracion);
            yield return null;
        }

        if (luz != null)
            luz.intensity = 0f;
    }

    private void CrearEfectoBasico(Vector3 posicion, bool esEnemigo)
    {
        // Crear un efecto básico usando primitivas de Unity
        GameObject efectoBasico = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        efectoBasico.transform.position = posicion;
        efectoBasico.transform.localScale = Vector3.one * 0.2f;
        
        // Remover collider
        Destroy(efectoBasico.GetComponent<Collider>());
        
        // Configurar material
        Renderer renderer = efectoBasico.GetComponent<Renderer>();
        if (renderer != null)
        {
            Material material = new Material(Shader.Find("Standard"));
            material.color = esEnemigo ? Color.red : Color.white;
            material.SetFloat("_Mode", 3); // Transparent mode
            material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            material.SetInt("_ZWrite", 0);
            material.DisableKeyword("_ALPHATEST_ON");
            material.EnableKeyword("_ALPHABLEND_ON");
            material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            material.renderQueue = 3000;
            
            renderer.material = material;
        }
        
        // Animar el efecto básico
        StartCoroutine(AnimarEfectoBasico(efectoBasico));
    }

    private System.Collections.IEnumerator AnimarEfectoBasico(GameObject efecto)
    {
        float duracion = 0.5f;
        float tiempo = 0f;
        Vector3 escalaInicial = efecto.transform.localScale;
        Renderer renderer = efecto.GetComponent<Renderer>();
        Color colorInicial = renderer.material.color;

        while (tiempo < duracion && efecto != null)
        {
            tiempo += Time.deltaTime;
            float progreso = tiempo / duracion;
            
            // Escalar hacia arriba y luego hacia abajo
            float escala = Mathf.Sin(progreso * Mathf.PI) * 2f;
            efecto.transform.localScale = escalaInicial * escala;
            
            // Desvanecer
            Color color = colorInicial;
            color.a = 1f - progreso;
            renderer.material.color = color;
            
            yield return null;
        }

        if (efecto != null)
            Destroy(efecto);
    }

    private void ReproducirSonidoImpacto(bool esEnemigo)
    {
        AudioClip sonidoAReproducir = null;

        // Seleccionar sonido apropiado
        if (esEnemigo && sonidoImpactoEnemigo != null)
        {
            sonidoAReproducir = sonidoImpactoEnemigo;
        }
        else if (sonidoImpacto != null)
        {
            sonidoAReproducir = sonidoImpacto;
        }

        if (sonidoAReproducir != null && audioSource != null)
        {
            // Ajustar volumen según el tipo de impacto
            audioSource.volume = esEnemigo ? volumenSonido * 1.2f : volumenSonido;
            
            // Variar ligeramente el pitch para más variedad
            audioSource.pitch = Random.Range(0.9f, 1.1f);
            
            audioSource.PlayOneShot(sonidoAReproducir);
        }
    }
}
