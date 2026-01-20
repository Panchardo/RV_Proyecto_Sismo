using UnityEngine;

public class AgarrarObjeto : MonoBehaviour
{
    public float distanciaAlcance = 3f; // Qué tan lejos podés alcanzar
    private GameObject objetoAgarrado;
    public Transform puntoSujecion; // Un objeto vacío hijo de la cámara donde irá el cubo

    void Update()
    {
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
        // Lanzamos un rayo invisible desde la cámara hacia adelante
        if (Physics.Raycast(transform.position, transform.forward, out hit, distanciaAlcance))
        {
            // Verificamos si el objeto tiene un Rigidbody (es físico)
            if (hit.collider.GetComponent<Rigidbody>() != null)
            {
                objetoAgarrado = hit.collider.gameObject;
                objetoAgarrado.GetComponent<Rigidbody>().isKinematic = true; // Desactivamos su gravedad propia para que no pese
            }
        }
    }

    void Soltar()
    {
        objetoAgarrado.GetComponent<Rigidbody>().isKinematic = false; // Recupera la física
        objetoAgarrado = null;
    }
}