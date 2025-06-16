using UnityEngine;

public class Driver : MonoBehaviour 
{
    [Header("Velocidades")]
    [SerializeField] private float velocidadMovimiento = 6f;
    [SerializeField] private float velocidadGiro = 85f;
    [SerializeField] private float velocidadRapida = 10f;
    [SerializeField] private float velocidadGiroRapido = 120f;
    [SerializeField] private float velocidadLenta = 3f;
    [SerializeField] private float velocidadGiroLento = 50f;
    
    [Header("Estado")]
    [SerializeField] private bool puedeMoverse = false;
    private bool juegoTerminado = false;
    
    // Variables para el movimiento realista
    private float velocidadActual;
    private float velocidadGiroActual;
    
    void Start()
    {
        velocidadActual = velocidadMovimiento;
        velocidadGiroActual = velocidadGiro;
    }
    
    void Update()
    {
        if (!puedeMoverse || juegoTerminado) return;
        
        // Obtener input
        float inputMovimiento = Input.GetAxis("Vertical");    // W/S
        float inputGiro = Input.GetAxis("Horizontal");        // A/D
        
        // MOVIMIENTO REALISTA DE CARRITO:
        // Solo gira si se está moviendo (como un carro real)
        if (Mathf.Abs(inputMovimiento) > 0.1f)
        {
            // Mover hacia adelante/atrás en la dirección que mira
            float movimiento = inputMovimiento * velocidadActual * Time.deltaTime;
            transform.Translate(Vector3.up * movimiento);
            
            // Girar solo cuando se mueve
            float giro = inputGiro * velocidadGiroActual * Time.deltaTime;
            
            // Si va hacia atrás, invertir el giro (como un carro real)
            if (inputMovimiento < 0)
                giro = -giro;
            
            transform.Rotate(0, 0, -giro);
        }
    }
    
    public void ActivarCarrito()
    {
        puedeMoverse = true;
        Debug.Log("✅ Carrito activado");
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "velocidad")
        {
            velocidadActual = velocidadRapida;
            velocidadGiroActual = velocidadGiroRapido;
            Debug.Log("⚡ ¡Carrito más rápido!");
            Destroy(other.gameObject);
        }
        else if (other.tag == "lento")
        {
            velocidadActual = velocidadLenta;
            velocidadGiroActual = velocidadGiroLento;
            Debug.Log("🐌 Carrito más lento...");
            Destroy(other.gameObject);
        }
        else if (other.tag == "salida")
        {
            Debug.Log("🎯 ¡SALIDA DETECTADA!");
            Destroy(other.gameObject);
            VictoriaFinal();
        }
    }
    
    void VictoriaFinal()
    {
        if (juegoTerminado) return;
        
        juegoTerminado = true;
        puedeMoverse = false;
        
        Debug.Log("🎉 =======================================");
        Debug.Log("🏆 ¡¡¡MISIÓN COMPLETADA!!!");
        Debug.Log("🚗 Escapaste del laberinto maldito");
        Debug.Log("⛽ Combustible restante: Suficiente para llegar a casa");
        Debug.Log("🌟 ¡Eres un explorador legendario!");
        Debug.Log("🎉 =======================================");
    }
}