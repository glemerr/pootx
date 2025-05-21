using System;
using System.Collections.Generic;
using UnityEngine;

public class SombraEnemiga : MonoBehaviour
{
    // Propiedades del enemigo
    public float velocidadMovimiento = 3.0f;
    public float amplitudMovimiento = 2.0f;
    
    // Propiedades de la sombra
    public GameObject objetoSombra;
    public float anchoDeSombra = 4.0f;
    public float altoDeSombra = 3.0f;
    public float dañoPorSegundo = 10.0f;
    
    private Vector3 posicionInicial;
    private float tiempoTranscurrido;
    
    void Start()
    {
        posicionInicial = transform.position;
        
        // Crear la sombra si no existe
        if (objetoSombra == null)
        {
            objetoSombra = GameObject.CreatePrimitive(PrimitiveType.Cube);
            objetoSombra.transform.localScale = new Vector3(anchoDeSombra, 0.1f, altoDeSombra);
            
            // Configurar material de la sombra
            Renderer rendererSombra = objetoSombra.GetComponent<Renderer>();
            rendererSombra.material = new Material(Shader.Find("Transparent/Diffuse"));
            rendererSombra.material.color = new Color(0, 0, 0, 0.5f); // Negro semi-transparente
            
            // Eliminar el collider del objeto visual de la sombra
            Destroy(objetoSombra.GetComponent<BoxCollider>());
            
            // Añadir un trigger collider para detectar colisiones
            BoxCollider areaDaño = objetoSombra.AddComponent<BoxCollider>();
            areaDaño.isTrigger = true;
            areaDaño.size = new Vector3(1, 0.1f, 1);
            
            // Añadir componente para manejar el daño
            objetoSombra.AddComponent<AreaDeDaño>();
            objetoSombra.GetComponent<AreaDeDaño>().dañoPorSegundo = dañoPorSegundo;
        }
    }
    
    void Update()
    {
        // Mover el enemigo en un patrón sinusoidal
        tiempoTranscurrido += Time.deltaTime;
        float desplazamientoX = Mathf.Sin(tiempoTranscurrido * velocidadMovimiento) * amplitudMovimiento;
        float desplazamientoZ = Mathf.Cos(tiempoTranscurrido * velocidadMovimiento) * amplitudMovimiento;
        
        transform.position = posicionInicial + new Vector3(desplazamientoX, 0, desplazamientoZ);
        
        // Actualizar posición de la sombra (debajo del enemigo)
        if (objetoSombra != null)
        {
            Vector3 posicionSombra = transform.position;
            posicionSombra.y = 3.0f; // Justo por encima del suelo
            objetoSombra.transform.position = posicionSombra;
        }
    }
}

// Componente para el área de daño de la sombra
public class AreaDeDaño : MonoBehaviour
{
    public float dañoPorSegundo = 10.0f;
    private List<Player> playersEnSombra = new List<Player>();
    private Color colorNormal;
    private Color colorDaño = new Color(0.8f, 0.2f, 0.2f, 0.6f); // Rojo semi-transparente
    
    void Start()
    {
        colorNormal = GetComponent<Renderer>().material.color;
    }
    
    void Update()
    {
        // Aplicar daño a todos los players dentro de la sombra
        foreach (Player player in playersEnSombra)
        {
            if (player != null)
            {
                player.RecibirDaño(dañoPorSegundo * Time.deltaTime);
            }
        }
        
        // Cambiar color de la sombra si hay players dentro
        if (playersEnSombra.Count > 0)
        {
            GetComponent<Renderer>().material.color = colorDaño;
        }
        else
        {
            GetComponent<Renderer>().material.color = colorNormal;
        }
    }
    
    void OnTriggerEnter(Collider other)
    {
        Player player = other.GetComponent<Player>();
        if (player != null && !playersEnSombra.Contains(player))
        {
            playersEnSombra.Add(player);
            player.EnSombra = true;
        }
    }
    
    void OnTriggerExit(Collider other)
    {
        Player player = other.GetComponent<Player>();
        if (player != null)
        {
            playersEnSombra.Remove(player);
            player.EnSombra = false;
        }
    }
}

// Clase para el player
public class Player : MonoBehaviour
{
    public float velocidad = 5.0f;
    public float saludMaxima = 100.0f;
    public float saludActual;
    public bool EnSombra { get; set; }
    
    private Renderer rendererPlayer;
    private Color colorNormal;
    private Color colorDañado = Color.red;
    
    void Start()
    {
        saludActual = saludMaxima;
        rendererPlayer = GetComponent<Renderer>();
        colorNormal = rendererPlayer.material.color;
    }
    
    void Update()
    {
        // Movimiento básico del player
        float movimientoHorizontal = Input.GetAxis("Horizontal");
        float movimientoVertical = Input.GetAxis("Vertical");
        
        Vector3 movimiento = new Vector3(movimientoHorizontal, 0, movimientoVertical);
        transform.Translate(movimiento * velocidad * Time.deltaTime);
        
        // Efecto visual cuando está en la sombra
        if (EnSombra)
        {
            // Parpadeo cuando recibe daño
            float pulso = Mathf.PingPong(Time.time * 5, 1);
            rendererPlayer.material.color = Color.Lerp(colorNormal, colorDañado, pulso);
        }
        else
        {
            rendererPlayer.material.color = colorNormal;
        }
    }
    
    public void RecibirDaño(float cantidad)
    {
        saludActual -= cantidad;
        
        // Verificar si el player ha muerto
        if (saludActual <= 0)
        {
            saludActual = 0;
            Debug.Log("¡Player derrotado!");
            // Aquí podrías implementar lógica de game over
        }
    }
}

// Clase para gestionar el juego
public class GestorDeJuego : MonoBehaviour
{
    public Player player;
    public GameObject prefabEnemigo;
    public int cantidadEnemigos = 3;
    public float areaDeJuego = 10.0f;
    
    void Start()
    {
        // Crear player si no existe
        if (player == null)
        {
            GameObject objetoPlayer = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            objetoPlayer.transform.position = new Vector3(0, 0.5f, 0);
            objetoPlayer.name = "Player";
            player = objetoPlayer.AddComponent<Player>();
            
            // Configurar material del player
            objetoPlayer.GetComponent<Renderer>().material.color = Color.blue;
        }
        
        // Crear enemigos
        for (int i = 0; i < cantidadEnemigos; i++)
        {
            CrearEnemigo();
        }
    }
    
    void CrearEnemigo()
    {
        // Posición aleatoria para el enemigo
        float posX = UnityEngine.Random.Range(-areaDeJuego, areaDeJuego);
        float posZ = UnityEngine.Random.Range(-areaDeJuego, areaDeJuego);
        
        GameObject nuevoEnemigo;
        if (prefabEnemigo != null)
        {
            nuevoEnemigo = Instantiate(prefabEnemigo, new Vector3(posX, 1, posZ), Quaternion.identity);
        }
        else
        {
            nuevoEnemigo = GameObject.CreatePrimitive(PrimitiveType.Cube);
            nuevoEnemigo.transform.position = new Vector3(posX, 1, posZ);
            nuevoEnemigo.name = "Enemigo" + UnityEngine.Random.Range(1000, 9999);
            
            // Configurar material del enemigo
            nuevoEnemigo.GetComponent<Renderer>().material.color = Color.red;
        }
        
        // Añadir componente de enemigo
        nuevoEnemigo.AddComponent<SombraEnemiga>();
    }
    
    void Update()
    {
        // Mostrar salud del player en la consola o UI
        if (player != null)
        {
            Debug.Log("Salud: " + player.saludActual.ToString("F0") + "/" + player.saludMaxima);
        }
    }
}