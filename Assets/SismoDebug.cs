using UnityEngine;
using System.Collections;

public class TestSismoAutomatico : MonoBehaviour
{
    [Header("Arrastrá tu SimuladorSismo acá")]
    public SimuladorSismo scriptSismo;

    void Start()
    {
        // Apenas arranca la escena, disparamos el cronómetro
        StartCoroutine(CuentaRegresiva());
    }

    IEnumerator CuentaRegresiva()
    {
        Debug.Log("⏳ MODO DEBUG: Sismo de prueba en 5 segundos...");
        
        yield return new WaitForSeconds(5.0f); // Esperamos 5 segundos
        
        if (scriptSismo != null)
        {
            // Forzamos la bandera del sismo para engañar al sistema
            scriptSismo.enZonaTerremoto = true;
            Debug.Log("💥 ¡SISMO FORZADO ACTIVADO!");
            
            // Nota: Si en tu SimuladorSismo tenías un método específico 
            // como IniciarSismo(), podés llamarlo acá en vez de la variable.
        }
        else
        {
            Debug.LogError("Te olvidaste de asignar el SimuladorSismo al script de testeo.");
        }
    }
}