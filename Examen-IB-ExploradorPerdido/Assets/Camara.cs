using UnityEngine;

public class Camara : MonoBehaviour 
{
    [SerializeField] GameObject cosaALaQueSeguir;
    
    void LateUpdate()
    {
        if (cosaALaQueSeguir != null)
        {
            transform.position = cosaALaQueSeguir.transform.position + new Vector3(0, 0, -10);
        }
    }
    
    // ¡NUEVO! Método para cambiar el objetivo de la cámara
    public void CambiarObjetivo(GameObject nuevoObjetivo)
    {
        cosaALaQueSeguir = nuevoObjetivo;
        Debug.Log("📷 Cámara ahora sigue a: " + nuevoObjetivo.name);
    }
}