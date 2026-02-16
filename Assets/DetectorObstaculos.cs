using UnityEngine;
using System.Collections.Generic;

public class DetectorObstaculos : MonoBehaviour
{
    // Ahora guardamos GameObjects (la caja entera) para que sea más claro
    private List<GameObject> cajasEnZona = new List<GameObject>();
    private int capaObstaculo;

    void Start()
    {
        capaObstaculo = LayerMask.NameToLayer("Obstaculo");
    }

    public bool EstaDespejado() => cajasEnZona.Count == 0;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == capaObstaculo)
        {
            // Buscamos el Rigidbody principal de la caja (el padre de todos los colliders)
            GameObject objetoRaiz = other.attachedRigidbody != null ? other.attachedRigidbody.gameObject : other.gameObject;

            if (!cajasEnZona.Contains(objetoRaiz))
            {
                cajasEnZona.Add(objetoRaiz);
                Debug.Log("Caja detectada: " + objetoRaiz.name + " | Total real: " + cajasEnZona.Count);
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
                Debug.Log("Caja removida: " + objetoRaiz.name + " | Quedan: " + cajasEnZona.Count);
                
                if (EstaDespejado()) Debug.Log("¡Pasillo 100% libre!");
            }
        }
    }
}