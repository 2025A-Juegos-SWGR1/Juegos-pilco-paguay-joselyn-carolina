using UnityEngine;

public class MovimientoPersona : MonoBehaviour 
{
    [Header("Movimiento de Persona")]
    [SerializeField] private float velocidadMovimiento = 4f;
    [SerializeField] private float velocidadRapida = 6f;
    
    private float velocidadOriginal;
    private Rigidbody2D rb;
    private Vector3 escalaOriginal; 
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        velocidadOriginal = velocidadMovimiento;
        escalaOriginal = transform.localScale;
    }
    
    void Update()
    {

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        
        Vector2 movimiento = new Vector2(horizontal, vertical) * velocidadMovimiento;
        
        rb.linearVelocity = movimiento;
        
        if (horizontal > 0)
            transform.localScale = new Vector3(Mathf.Abs(escalaOriginal.x), escalaOriginal.y, escalaOriginal.z); // Mirando derecha
        else if (horizontal < 0)
            transform.localScale = new Vector3(-Mathf.Abs(escalaOriginal.x), escalaOriginal.y, escalaOriginal.z); // Mirando izquierda
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "velocidad")
        {
            velocidadMovimiento = velocidadRapida;
            Destroy(other.gameObject);
            
         
            Invoke("RestaurarVelocidad", 5f);
        }
    }
    
    void RestaurarVelocidad()
    {
        velocidadMovimiento = velocidadOriginal;
    }
}