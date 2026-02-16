using UnityEngine;

public class DetectorZonaInsegura : MonoBehaviour
{
    public bool NOestasA_Salvo = false; // Mirá esta casilla en el Inspector al probar

    void OnTriggerEnter(Collider other)
    {
        // Si lo que tocamos tiene la etiqueta correcta...
        if (other.CompareTag("ZonaInsegura"))
        {
            NOestasA_Salvo = true;
            Debug.Log("ZONA PELIGROSA, SALIR");
        }
    }

    void OnTriggerExit(Collider other)
    {
        // Si salimos de la etiqueta correcta...
        if (other.CompareTag("ZonaInsegura"))
        {
            NOestasA_Salvo = false;
            Debug.Log("SALISTA DE LA ZONA PELIGROSA");
        }
    }
}