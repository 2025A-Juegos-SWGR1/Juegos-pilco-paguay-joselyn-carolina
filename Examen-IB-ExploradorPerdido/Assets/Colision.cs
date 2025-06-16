using System;
using UnityEngine;

public class Colision : MonoBehaviour 
{
    [Header("Sistema de Supervivencia")]
    [SerializeField] private float combustibleActual = 15f; // ¡CAMBIADO! Ahora empieza con poco combustible
    [SerializeField] private float consumoPorSegundo = 1f;
    [SerializeField] private float combustiblePorTanque = 30f;
    [SerializeField] private float danoPorTrampa = 20f;
    [SerializeField] private float combustiblePorVida = 40f; // ¡NUEVO! Combustible que da la vida
    
    [Header("Sistema de Victoria")]
    [SerializeField] private int tanquesNecesarios = 3;
    private int tanquesRecolectados = 0;
    
    [Header("Referencias")]
    [SerializeField] private GameObject carrito;
    
    private SpriteRenderer sprite;
    private bool juegoIniciado = false;
    private bool puedeEscapar = false;
    private bool juegoTerminado = false;
    private bool enCarrito = false;
    
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        MostrarIntroduccion();
    }
    
    void MostrarIntroduccion()
    {
        Debug.Log("🏛️ =======================================");
        Debug.Log("📖 EXPLORADOR PERDIDO EN EL LABERINTO");
        Debug.Log("🏛️ =======================================");
        Debug.Log("⚠️  Atrapado en ruinas antiguas");
        Debug.Log("⛽ Combustible actual: " + (int)combustibleActual + "% (¡MUY BAJO!)");
        Debug.Log("🎯 MISIÓN: Recolecta " + tanquesNecesarios + " tanques o encuentra la vida dorada");
        Debug.Log("💛 La vida dorada te permite escapar inmediatamente");
        Debug.Log("💀 PELIGRO: Las bombas te quitan combustible");
        Debug.Log("🏛️ =======================================");
        
        Invoke("IniciarJuego", 3f);
    }
    
    void IniciarJuego()
    {
        juegoIniciado = true;
        Debug.Log("🚀 ¡Comienza la búsqueda! ¡Muévete con WASD!");
    }
    
    void Update()
    {
        if (!juegoIniciado || juegoTerminado || enCarrito) return;
        
        if (!puedeEscapar)
        {
            combustibleActual -= consumoPorSegundo * Time.deltaTime;
            
            // Avisos de combustible bajo
            if (combustibleActual <= 15 && combustibleActual > 14)
                Debug.Log("🚨 ¡COMBUSTIBLE BAJO! " + (int)combustibleActual + "%");
            
            // Game Over
            if (combustibleActual <= 0)
            {
                GameOver();
            }
        }
    }
    
    void GameOver()
    {
        if (juegoTerminado) return;
        
        juegoTerminado = true;
        
        Debug.Log("💀 =======================================");
        Debug.Log("⚰️  SIN COMBUSTIBLE - PERDISTE");
        Debug.Log("👻 El explorador se perdió para siempre...");
        Debug.Log("💀 =======================================");
        
        GetComponent<MovimientoPersona>().enabled = false;
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (juegoTerminado || enCarrito) return;
        
        if (other.tag == "paquete") // Tanques rojos
        {
            tanquesRecolectados++;
            combustibleActual += combustiblePorTanque;
            if (combustibleActual > 100) combustibleActual = 100;
            
            Debug.Log("⛽ Tanque recolectado! (" + tanquesRecolectados + "/" + tanquesNecesarios + ") - Combustible: " + (int)combustibleActual + "%");
            
            if (tanquesRecolectados >= tanquesNecesarios)
            {
                puedeEscapar = true;
                Debug.Log("✅ ¡Suficiente combustible! ¡Ve al carrito para escapar!");
                Debug.Log("🛡️ Ya no es necesaria mas gasolina");
            }
            
            Destroy(other.gameObject);
        }
        
        // Vida Extra 
        if (other.tag == "vida")
        {
            combustibleActual += combustiblePorVida;
            if (combustibleActual > 100) combustibleActual = 100;
            
            puedeEscapar = true;
            
            Debug.Log("💛 ¡VIDA DORADA ENCONTRADA! +" + combustiblePorVida + "% combustible");
            Debug.Log("🌟 ¡Ahora puedes escapar inmediatamente!");
            Debug.Log("⛽ Combustible: " + (int)combustibleActual + "%");
            Debug.Log("🛡️ Ya no consumes combustible automáticamente");
            
            Destroy(other.gameObject);
        }
        
        if (other.tag == "cliente") // Bombas ,picos
        {
            combustibleActual -= danoPorTrampa;
            Debug.Log("💥 ¡BOMBA ACTIVADA! Perdiste " + danoPorTrampa + "% - Combustible: " + (int)combustibleActual + "%");
            
            Destroy(other.gameObject);
            
            if (combustibleActual <= 0)
            {
                GameOver();
            }
        }
        
        if (other.tag == "carrito")
        {
            if (puedeEscapar)
            {
                SubirseAlCarrito();
            }
            else
            {
                Debug.Log("🚗 'Necesito más combustible antes de intentar escapar...'");
                Debug.Log("📊 Progreso: " + tanquesRecolectados + "/" + tanquesNecesarios + " tanques");
                Debug.Log("💛 O encuentra la vida dorada para escapar inmediatamente");
            }
        }
    }
    
    void SubirseAlCarrito()
    {
        if (juegoTerminado || enCarrito) return;
        
        enCarrito = true;
        
        Debug.Log("🚗 =======================================");
        Debug.Log("🎯 ¡TE SUBISTE AL CARRITO!");
        Debug.Log("🏁 Ahora dirígete a la SALIDA ");
        Debug.Log("⛽ Combustible: " + (int)combustibleActual + "%");
        Debug.Log("🚗 =======================================");
        
        if (carrito == null)
        {
            Debug.LogError("❌ ¡La referencia 'carrito' está vacía!");
            return;
        }
        
        Debug.Log("🔍 Carrito encontrado: " + carrito.name);
        
        transform.position = carrito.transform.position;
        
        Driver driverScript = carrito.GetComponent<Driver>();
        if (driverScript != null)
        {
            Debug.Log("✅ Componente Driver encontrado, activando...");
            driverScript.ActivarCarrito();
        }
        else
        {
            Debug.LogError("❌ ¡NO se encontró el componente Driver en el carrito!");
            Debug.Log("🔍 Carrito: " + carrito.name);
            
            Component[] componentes = carrito.GetComponents<Component>();
            Debug.Log("📋 Componentes en el carrito:");
            foreach (Component comp in componentes)
            {
                Debug.Log("   - " + comp.GetType().Name);
            }
        }
        
        if (Camera.main.GetComponent<Camara>() != null)
        {
            Camera.main.GetComponent<Camara>().CambiarObjetivo(carrito);
        }
        
        GetComponent<MovimientoPersona>().enabled = false;
        gameObject.SetActive(false);
        
        Debug.Log("🎮 ¡Ahora puedes conducir! y ve a la salida!");
    }
}