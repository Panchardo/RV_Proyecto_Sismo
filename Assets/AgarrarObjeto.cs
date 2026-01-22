using UnityEngine;
using UnityEngine.UI;

public class AgarrarObjeto : MonoBehaviour
{
    public float distanciaAlcance = 3f; // Qué tan lejos podés alcanzar
    private GameObject objetoAgarrado;
    public float potencia = 10.0f;
    public Transform puntoSujecion; // Un objeto vacío hijo de la cámara donde irá el cubo
    private Rigidbody rbObjeto;
    public Image miraPuntero;

    void Update()
    {
        if(objetoAgarrado == null)
        {
            EscanearEntorno();
        }
        else
        {
             miraPuntero.color = Color.white; 
        }
        // Si presionás el botón de disparo (gatillo o botón principal)
        if (Input.GetButtonDown("Fire1")) 
        {
            if (objetoAgarrado == null) {
                IntentarAgarrar();
            } else {
                Soltar();
            }
        }

        // Si tenemos algo agarrado, que siga la posición del punto de sujeción
        if (objetoAgarrado != null)
        {
            objetoAgarrado.transform.position = puntoSujecion.position;
            objetoAgarrado.transform.rotation = puntoSujecion.rotation;
            
        }
    }



    void IntentarAgarrar()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, distanciaAlcance))
        {
            // 1. Buscamos el Rigidbody UNA SOLA VEZ y guardamos la referencia (el cable)
            rbObjeto = hit.collider.GetComponent<Rigidbody>();

            // Si el cable conectó con algo (existe el rigidbody)...
            if (rbObjeto != null)
            {
                objetoAgarrado = hit.collider.gameObject;
                
                // 2. Usamos la referencia. Esto afecta al objeto real inmediatamente.
                rbObjeto.isKinematic = true; 
            }
        }
    }

    void Soltar()
    {
        // Como ya tenemos el cable conectado en 'rbObjeto', lo usamos directo.
        rbObjeto.isKinematic = false; 
        rbObjeto.AddForce(transform.forward * potencia, ForceMode.Impulse);
        
        // Limpiamos todo
        objetoAgarrado = null;
        rbObjeto = null; // Cortamos el cable para no quedarnos conectados a la nada
    }
    void EscanearEntorno()
    {
        RaycastHit hitScan;

        // Lanzamos el rayo. Fondealo igual que en IntentarAgarrar
        if (Physics.Raycast(transform.position, transform.forward, out hitScan, distanciaAlcance))
        {
            // EL DESAFÍO:
            // Chequeá si hitScan.collider tiene un Rigidbody attached.
            // SI TIENE: Poné miraPuntero.color = Color.green;
            // SI NO TIENE (es una pared): Poné miraPuntero.color = Color.white;
            
            // Pista: Es casi el mismo IF que usaste en IntentarAgarrar()
             if (hitScan.collider.GetComponent<Rigidbody>() != null)
             {
                 miraPuntero.color = Color.green;
             }
             else
             {
                 miraPuntero.color = Color.white;
             }
        }
        else
        {
            // Si el rayo no toca nada (estás mirando al cielo), volvé a BLANCO
             miraPuntero.color = Color.white;
        }
    }
}
