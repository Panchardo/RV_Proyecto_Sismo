using UnityEngine;
using System.Collections.Generic;

public class DetectorObstaculos : MonoBehaviour
{
    private List<GameObject> cajasEnZona = new List<GameObject>();
    private int capaObstaculo;

    [Header("Efectos Visuales de la Zona")]
    // Arrastrá acá el MeshRenderer del cubo que marca la zona
    public MeshRenderer renderizadorZona; 
    
    // Usamos colores semitransparentes (el último valor '0.3f' es la opacidad o canal Alpha)
    public Color colorOcupado = new Color(1f, 0f, 0f, 0.3f); // Rojo
    public Color colorLibre = new Color(0f, 1f, 0f, 0.3f);   // Verde

    void Start()
    {
        capaObstaculo = LayerMask.NameToLayer("Obstaculo");
        ActualizarColorZona(); // Seteamos el color apenas arranca el nivel
    }

    public bool EstaDespejado() => cajasEnZona.Count == 0;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == capaObstaculo)
        {
            GameObject objetoRaiz = other.attachedRigidbody != null ? other.attachedRigidbody.gameObject : other.gameObject;

            if (!cajasEnZona.Contains(objetoRaiz))
            {
                cajasEnZona.Add(objetoRaiz);
                ActualizarColorZona(); // Cambia a rojo si entró una caja
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == capaObstaculo)
        {
            GameObject objetoRaiz = other.attachedRigidbody != null ? other.attachedRigidbody.gameObject : other.gameObject;

            if (cajasEnZona.Contains(objetoRaiz))
            {
                cajasEnZona.Remove(objetoRaiz);
                ActualizarColorZona(); // Evalúa si quedó vacío para pasar a verde
            }
        }
    }

    private void ActualizarColorZona()
    {
        if (renderizadorZona != null)
        {
            // Cambiamos el color del material dinámicamente
            renderizadorZona.material.color = EstaDespejado() ? colorLibre : colorOcupado;
        }
    }

    // Esta función la va a llamar la puerta cuando se abra
    public void ApagarEfectoVisual()
    {
        if (renderizadorZona != null)
        {
            renderizadorZona.enabled = false; // Oculta el cubo por completo
        }
    }
}